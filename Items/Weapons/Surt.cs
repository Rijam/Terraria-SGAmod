using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Projectiles;
using Idglibrary;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class Surt : ModItem
	{
		bool altfired = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Surt");
			Tooltip.SetDefault("A molten blade of a cursed demon lord\nHold left click to lift the sword by the blade end and release to plunge it into the ground, unleashing a molten eruption!\nHold for longer for a stronger effect, you will be set on fire\nHold Right click to rapidly swing, throwing out waves of immense heat. You swing faster while on fire.\nTargets hit by the blade are set on fire.");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 150;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 40;
			Item.useAnimation = 15;
			Item.useStyle = 1;
			Item.knockBack = 5;
			Item.sellPrice(0, 50, 0, 0);
			Item.rare = 8;
			Item.UseSound = SoundID.Item1;
			Item.shoot = Mod.Find<ModProjectile>("SurtCharging").Type;
			Item.shootSpeed = 30f;
			Item.useTurn = false;
			Item.autoReuse = false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("ClayMore"), 1).AddIngredient(ItemID.BrokenHeroSword, 1).AddIngredient(mod.ItemType("PrimordialSkull"), 1).AddIngredient(ItemID.HellstoneBar, 15).AddIngredient(mod.ItemType("FieryShard"), 8).AddTile(TileID.MythrilAnvil).Register();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 60 * (GetType() == typeof(BrimflameHarbinger) ? 8 : 5));
		}

		public override bool CanUseItem(Player player)
		{

			altfired = player.altFunctionUse == 2 ? true : false;

			if (!altfired)
			{
				Item.autoReuse = true;
				Item.channel = true;
				Item.useStyle = 5;
				Item.noMelee = true;
				Item.noUseGraphic = true;
				if (!Main.dedServ)
				{
					Item.GetGlobalItem<ItemUseGlow>().glowTexture = null;
				}
			}
			else
			{
				Item.autoReuse = false;
				Item.channel = false;
				Item.noMelee = false;
				Item.useStyle = 1;
				Item.noUseGraphic = false;
				if (!Main.dedServ)
				{
					Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/Surt_Glow").Value;
				}
			}

			return true;
		}

		public override float UseTimeMultiplier(Player player)
		{
			if (player.HasBuff(BuffID.OnFire))
				return 2f;
			return 1f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			if (altfired)
			{
				float speed = 2.0f;
				float numberProjectiles = 1;
				float rotation = MathHelper.ToRadians(8);
				Vector2 speedz = new Vector2(speedX, speedY);
				speedz.Normalize(); speedz *= 30f; speedX = speedz.X; speedY = speedz.Y;

				position += Vector2.Normalize(speedz) * 45f;
				SoundEngine.PlaySound(SoundID.Item, player.Center, 45);
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = (speedz * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
					int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("HeatWave").Type, (int)((float)damage * 0.20f), knockBack / 3f, player.whoAmI);
					Main.projectile[proj].DamageType = DamageClass.Melee;
					// Main.projectile[proj].magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
					Main.projectile[proj].netUpdate = true;
					IdgProjectile.Sync(proj);

					//NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
				}

			}
			return !altfired;
		}


		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			//if (player.ownedProjectileCounts[mod.ProjectileType("TrueMoonlightCharging")]>0)
			//return;

			for (int num475 = 0; num475 < 5; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, Mod.Find<ModDust>("HotDust").Type);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + ((player.itemRotation.ToRotationVector2() * 2f).RotatedBy(MathHelper.ToRadians(-90)));
				Main.dust[dust].noGravity = true;
			}

			Lighting.AddLight(player.position, 0.9f, 0.1f, 0.1f);
		}
	}
	public class SurtWave : ModProjectile
	{

		int leveleffect = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SurtWave");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/CrateBossWeaponThrown"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 6;
			height = 2;
			fallThrough = false;
			return true;
		}

		public override void SetDefaults()
		{
			Projectile.timeLeft = 200;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 30;
		}

		public override void AI()
		{
			//int rayloc = Idglib.RaycastDown((int)(projectile.Center.X + 4) / 16, (int)(projectile.Center.Y - 36f) / 16) * 16;
			//if ((rayloc - 16) - (projectile.position.Y) > 70 || (rayloc - 16) - (projectile.position.Y) < -30)
			//projectile.Kill();
			//projectile.position.X += projectile.velocity.X*1;
			//projectile.position.Y += -16;

			Projectile.ai[0] += 1;

			if (Projectile.ai[0] % 24 == 0)
			{
				int thisoned = Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.Center.Y - 128), new Vector2(0, 1), ModContent.ProjectileType<SurtWave2>(), Projectile.damage, Projectile.knockBack * 1f, Main.player[Projectile.owner].whoAmI, ai1: GetType() == typeof(SurtWaveBrimFlame) ? 1 : 0);
				//Main.projectile[thisoned].timeLeft = 25;
				//Main.projectile[thisoned].melee = true;
			}
		}

	}

	public class SurtWave2 : ModProjectile
	{

		int leveleffect = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Surt Wave 2");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/CrateBossWeaponThrown"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 6;
			height = 2;
			fallThrough = false;
			return true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.timeLeft = 1200;
			Projectile.tileCollide = true;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 600;
			Projectile.velocity = new Vector2(0, 1);
		}

		public override void Kill(int timeLeft)
		{
			if (timeLeft > 600 && timeLeft < 1190)
			{
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 74, 0.5f, 0);
				int thisoned = Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, -6), ModContent.ProjectileType<SurtExplosion>(), (int)((float)Projectile.damage * 0.15f), Projectile.knockBack * 0.5f, Main.player[Projectile.owner].whoAmI,ai1: Projectile.ai[1]);
			}
		}

	}


	public class SurtRocks : LavaRocks
	{
		public float trans2 = -0.2f;
		public override bool hitwhilefalling => true;
		public override float trans => Math.Max(0, trans2);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Rocks");
		}

		public override void AI()
		{
			trans2 = Math.Min(1f, trans2 + 0.05f);
			base.AI();
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			// projectile.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.DamageType = DamageClass.Melee;
		}

	}

	public class SurtExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Boom");
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 basepoint = (new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(Projectile.localAI[0]) }.ToVector2()) + new Vector2(0, 8);

			Texture2D tex = SGAmod.ExtraTextures[97];

			if (Projectile.ai[1] > 0)
				spriteBatch.Draw(tex, basepoint - Main.screenPosition, null, (Color.Red) * ((float)Projectile.timeLeft / 25f)*0.25f, Projectile.rotation, new Vector2(tex.Width / 2f, tex.Height), new Vector2((Projectile.scale * 0.5f) % 1f, (Projectile.scale * 0.5f) % 1f) * new Vector2(12f, 6f), SpriteEffects.None, 0f);


			spriteBatch.Draw(tex, basepoint - Main.screenPosition, null, (Projectile.ai[1]>0 ? Color.Yellow : Color.White) * ((float)Projectile.timeLeft / 25f), Projectile.rotation, new Vector2(tex.Width / 2f, tex.Height), new Vector2(Projectile.scale, Projectile.scale), SpriteEffects.None, 0f);
			return false;
		}

		public override void SetDefaults()
		{
			Projectile.width = 96;
			Projectile.height = 96;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 25;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.localNPCHitCooldown = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.netImportant = true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Surt"); }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//target.immune[projectile.owner] = 2;
		}

		public override void AI()
		{
			if (Projectile.localAI[1] == 0)
			{
				HalfVector2 half = new HalfVector2(Projectile.Center.X, Projectile.Center.Y);
				Projectile.localAI[0] = ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
				Projectile.localAI[1] = 1;
				SGAmod.AddScreenShake(24f, 320, Projectile.Center);
			}

			if (Projectile.ai[0] % 1 == 0 && Projectile.ai[1]>0 && Projectile.ai[0]<12)//GetType() == typeof(SurtWaveBrimFlame))
			{
				Vector2 where = new Vector2(Main.rand.Next(-64, 64), Main.rand.Next(-64, 64));
				int proj2 = Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.Center.Y - Projectile.ai[0]*15) + where, new Vector2(0, -8), Mod.Find<ModProjectile>("BoulderBlast").Type, (int)((float)Projectile.damage * 0.75f), Projectile.knockBack / 3f, Projectile.owner);
				Main.projectile[proj2].DamageType = DamageClass.Melee;
				// Main.projectile[proj2].magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
				Main.projectile[proj2].timeLeft = 3;
				Main.projectile[proj2].usesLocalNPCImmunity = true;
				Main.projectile[proj2].localNPCHitCooldown = -1;
				Main.projectile[proj2].netUpdate = true;
				IdgProjectile.Sync(proj2);
			}

			Projectile.scale += 0.2f;

			Projectile.ai[0] += 1;

			Vector2 basepoint = (new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(Projectile.localAI[0]) }.ToVector2()) + new Vector2(0, -8);


			Lighting.AddLight(basepoint, 2f * (Color.Yellow.ToVector3() * ((float)Projectile.timeLeft / 24f)));
			Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3());

			if (Projectile.ai[0] < 4)
			{

				int thisoned = Projectile.NewProjectile(new Vector2(Projectile.Center.X, basepoint.Y), new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-8f, -3f) - (Projectile.ai[0] / 0.75f)), ModContent.ProjectileType<SurtRocks>(), Projectile.damage * 6, Projectile.knockBack * 2f, Main.player[Projectile.owner].whoAmI);
				IdgProjectile.Sync(thisoned);

			}

		}

	}
	public class SurtCharging : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Surt"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>(this.Texture);
			spriteBatch.Draw(tex, (Projectile.Center + new Vector2(Projectile.direction * 10, -24)) - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(tex.Width / 2f, tex.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);
			tex = ModContent.Request<Texture2D>("SGAmod/Items/GlowMasks/Surt_Glow");
			if (GetType() == typeof(BrimflameCharging))
				tex = ModContent.Request<Texture2D>("SGAmod/Items/GlowMasks/BrimflameHarbinger_Glow");

			spriteBatch.Draw(tex, (Projectile.Center + new Vector2(Projectile.direction * 10, -24)) - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(tex.Width / 2f, tex.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);

			return false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 90;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			AIType = 0;
		}

		public override void AI()
		{
			Vector2 mousePos = Main.MouseWorld;
			Player player = Main.player[Projectile.owner];

			if (Projectile.ai[0] > 1000f || player.dead)
			{
				Projectile.Kill();
			}
			Projectile.localAI[1] += 1f;
			if ((!player.channel || Projectile.ai[0] > 0))
			{


				Projectile.ai[0] += 1;
				Projectile.netUpdate = true;
				Projectile.velocity /= 2f;
				if (Projectile.ai[0] < 2)
				{
					Projectile.ai[1] = Projectile.timeLeft;
					if (Projectile.ai[1] < 130)
					{
						player.itemAnimation = 5;
						player.itemTime = 5;
						Projectile.Kill();
						return;
					}

					Projectile.timeLeft /= 4;
					Projectile.timeLeft += 90;
				}
				else
				{

					if (Projectile.ai[0] > 20f && Projectile.ai[0] < 1000f)
					{
						Projectile.ai[0] = 1000f;

						if (!Collision.CanHitLine(player.Center, 4, 4, player.Center + new Vector2(0,32), 4, 4))
						{

							//int rayloc = Idglib.RaycastDown((int)(player.Center.X) / 16, (int)(player.Center.Y - 8f) / 16) * 16;
							//if (!((rayloc - 16) - (projectile.position.Y) > 30 || (rayloc - 16) - (projectile.position.Y) < -30))
							//{

								//player.Hurt(PlayerDeathReason.ByCustomReason("Testing"), 5, projectile.direction, true, false, false, -1);
								Vector2 pos = player.Center;//new Vector2((int)(player.Center.X/16), (int)(player.Center.Y/16)) * 16;

								float damagemul = 1f + (Projectile.ai[1] - 130) / 250f;

								int thisoned = Projectile.NewProjectile(pos, new Vector2(Projectile.direction * 4, 0), GetType() == typeof(BrimflameCharging) ? ModContent.ProjectileType<SurtWaveBrimFlame>() : ModContent.ProjectileType<SurtWave>(), (int)(Projectile.damage * damagemul) * 3, Projectile.knockBack * 2f, Main.player[Projectile.owner].whoAmI);
								Main.projectile[thisoned].timeLeft = (int)Projectile.ai[1];
								SoundEngine.PlaySound(SoundID.Item, player.Center, 74);

						}

					}

				}
			}
			else
			{
				if (Projectile.timeLeft < 300)
				{
					Projectile.timeLeft += (GetType() == typeof(BrimflameCharging) ? 3 : 2);
				}
				if (Projectile.localAI[1] % 10 == 0 && Projectile.timeLeft > 129 && Projectile.timeLeft < 300)
					SoundEngine.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 102, 0.25f, -0.5f + (float)Projectile.timeLeft / 250f);

			}
			Projectile.rotation = MathHelper.ToRadians(135);
			// Multiplayer support here, only run this code if the client running it is the owner of the projectile
			if (Projectile.owner == Main.myPlayer)
			{
				Vector2 diff = mousePos - player.Center;
				diff.Normalize();

				if (Projectile.ai[0] < 1f)
					Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
				Projectile.netUpdate = true;
				Projectile.Center = mousePos;
			}
			if (Projectile.ai[0] < 1f)
				Projectile.velocity = new Vector2(Projectile.direction * 2, -((float)Projectile.timeLeft - 60f)) * 0.15f;

			Projectile.Center = player.Center;
			player.heldProj = Projectile.whoAmI;
			player.itemAnimation = 60;
			player.itemTime = 60;

			player.AddBuff(BuffID.OnFire, (int)MathHelper.Clamp((int)(((float)Projectile.timeLeft - 260f) * 5f), 1, 450));

			int dir = Projectile.direction;
			player.itemRotation = (MathHelper.ToRadians(90) + MathHelper.ToRadians(Projectile.velocity.Y * 5f)) * dir;
			player.ChangeDir(dir);


		}

	}



	public class BrimflameHarbinger : Surt
	{
		bool altfired = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brimflame Harbinger");
			Tooltip.SetDefault("'It's Brimflame, not Brimestone Flames!, and its one of the few times Hellstone bars are OK to farm!'" +
				"\nHold left click to lift the sword by the blade end and release to plunge it into the ground, unleashing an EXTREME molten eruption!\nHold for longer for a stronger effect, you will be set on fire\nHold Right click to rapidly swing, throwing out waves of Brimflame blasts and immense heat. You swing faster while on fire.\nTargets hit by the blade are set on fire for longer.");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 160;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.crit = 15;
			Item.height = 54;
			Item.useTime = 25;
			Item.useAnimation = 15;
			Item.useStyle = 1;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 25, 0, 0);
			Item.rare = 10;
			Item.UseSound = SoundID.Item1;
			Item.shoot = Mod.Find<ModProjectile>("BrimflameCharging").Type;
			Item.shootSpeed = 30f;
			Item.useTurn = false;
			Item.autoReuse = false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("Surt"), 1).AddIngredient(mod.ItemType("FieryShard"), 5).AddIngredient(mod.ItemType("CalamityRune"), 2).AddIngredient(mod.ItemType("Entrophite"), 50).AddIngredient(mod.ItemType("StygianCore"), 2).AddIngredient(ItemID.LunarBar, 10).AddIngredient(mod.ItemType("LunarRoyalGel"), 15).AddTile(TileID.LunarCraftingStation).Register();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{

			altfired = player.altFunctionUse == 2 ? true : false;

			if (!altfired)
			{
				Item.autoReuse = true;
				Item.channel = true;
				Item.useStyle = 5;
				Item.noMelee = true;
				Item.noUseGraphic = true;
				if (!Main.dedServ)
				{
					Item.GetGlobalItem<ItemUseGlow>().glowTexture = null;
				}
			}
			else
			{
				Item.autoReuse = false;
				Item.channel = false;
				Item.noMelee = false;
				Item.useStyle = 1;
				Item.noUseGraphic = false;
				if (!Main.dedServ)
				{
					Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/BrimflameHarbinger_Glow").Value;
				}
			}

			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			if (altfired)
			{
				float speed = 0.5f;
				float numberProjectiles = 3;
				float rotation = MathHelper.ToRadians(20);
				Vector2 speedz = new Vector2(speedX, speedY);
				speedz.Normalize(); speedz *= 30f; speedX = speedz.X; speedY = speedz.Y;

				position += Vector2.Normalize(speedz) * 45f;
				SoundEngine.PlaySound(SoundID.Item, player.Center, 45);
				for (int i = 0; i < numberProjectiles; i++)
				{
					if (i != 1)
					{
						Vector2 perturbedSpeed = (speedz * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))).RotatedBy(MathHelper.Lerp(-rotation / 4f, rotation / 4f, (float)Main.rand.Next(0, 100) / 100f)); // Watch out for dividing by 0 if there is only 1 projectile.
						int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("HeatWave").Type, (int)((float)damage * 0.5f), knockBack / 3f, player.whoAmI);
						Main.projectile[proj].DamageType = DamageClass.Melee;
						// Main.projectile[proj].magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
						Main.projectile[proj].netUpdate = true;
						IdgProjectile.Sync(proj);
					}

					Vector2 perturbedSpeed2 = (speedz * speed).RotatedBy(MathHelper.Lerp(-rotation / 2, rotation / 2f, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
					int proj2 = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed2.X * 2f, perturbedSpeed2.Y * 2f, Mod.Find<ModProjectile>("BoulderBlast").Type, (int)((float)damage * 1f), knockBack / 3f, player.whoAmI);
					Main.projectile[proj2].DamageType = DamageClass.Melee;
					// Main.projectile[proj2].magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
					Main.projectile[proj2].timeLeft = 15;
					Main.projectile[proj2].usesLocalNPCImmunity = true;
					Main.projectile[proj2].localNPCHitCooldown = -1;
					Main.projectile[proj2].netUpdate = true;
					IdgProjectile.Sync(proj2);

					//NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
				}

			}
			return !altfired;
		}


		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			//if (player.ownedProjectileCounts[mod.ProjectileType("TrueMoonlightCharging")]>0)
			//return;

			for (int num475 = 0; num475 < 5; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 235);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + ((player.itemRotation.ToRotationVector2() * 2f).RotatedBy(MathHelper.ToRadians(-90)));
				Main.dust[dust].noGravity = true;
			}

			Lighting.AddLight(player.position, 0.9f, 0.1f, 0.1f);
		}


	}

	public class BrimflameCharging : SurtCharging
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/BrimflameHarbinger"); }
		}


	}

	public class SurtWaveBrimFlame : SurtWave
	{

		int leveleffect = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SurtWave Brimflame");
		}
	}

}