using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using SGAmod.Projectiles;
using Idglibrary;
using System.Linq;

using Terraria.Utilities;
using Terraria.Audio;

namespace SGAmod.Items.Weapons.Caliburn
{


	public class TrueCaliburn : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Caliburn");
			Tooltip.SetDefault("Summons True Spectral Blades to home in on your mouse cursor for a few seconds or until it hits 4 times, then it returns to you.\nTrue Spectral Blades summon Spectral Swords on hit, and do not hit while returning");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.crit = 10;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 23;
			Item.useAnimation = 16;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value=Item.sellPrice(1, 0, 0, 0);
			Item.rare = 7;
			Item.UseSound = SoundID.Item1;
			Item.shoot = Mod.Find<ModProjectile>("CaliburnHomingSwordTrue").Type;
			Item.shootSpeed = 20f;
			Item.useTurn = false;
			Item.autoReuse = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			damage = (int)(damage * 1.50);
			SoundEngine.PlaySound(SoundID.Item101, player.Center);
			return (player.ownedProjectileCounts[Mod.Find<ModProjectile>("CaliburnHomingSword").Type] < 100);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("CaliburnTypeA"), 1).AddIngredient(mod.ItemType("CaliburnTypeB"), 1).AddIngredient(mod.ItemType("CaliburnTypeC"), 1).AddIngredient(mod.ItemType("CaliburnCompess"), 1).AddIngredient(mod.ItemType("PrismalBar"), 10).AddIngredient(ItemID.BrokenHeroSword, 1).AddTile(TileID.MythrilAnvil).Register();
		}


	}


	public class CaliburnTypeA : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn");
			Tooltip.SetDefault("One of the lost swords\nSummons Spectral copies of the sword to strike nearby enemies on swing\nThese do not cause immunity frames, but hit only once");
			Item.staff[Item.type] = true;
		}

        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {

			if (pre == -1)
				Item.prefix = (byte)TrapPrefix.GetBustedPrefix;

			return true;
        }

        public override void SetDefaults()
		{
			Item.damage = 20;
			Item.crit = 0;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 16;
			Item.useAnimation = 22;
			Item.reuseDelay = 30;
			Item.useStyle = 1;
			Item.knockBack = 5;
			Item.value=Item.buyPrice(0, 5, 0, 0);
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.shoot = Mod.Find<ModProjectile>("CaliburnSpectralBlade").Type;
			Item.shootSpeed = 20f;
			Item.useTurn = false;
			Item.autoReuse = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/CaliburnBlade1").Value;
				Item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Color.White * 0.25f;
				};
			}
		}

		public static void SpectralSummon(Player player, int type, int damage,Vector2 there)
		{

			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(16);

			List<NPC> guys = new List<NPC>();

			for (int a = 0; a < Main.maxNPCs; a++)
			{
				NPC npchim = Main.npc[a];
				Projectile proj = new Projectile();
				proj.SetDefaults(ProjectileID.ChlorophyteBullet);

				if (npchim.active && !npchim.friendly && npchim.CanBeChasedBy() && (npchim.Center - there).Length() < 400)
				{
					guys.Add(npchim);
				}

			}
			if (guys.Count > 0)
			{
				for (int i = 0; i < numberProjectiles; i++)
				{
					NPC theone = guys[Main.rand.Next(0, guys.Count)];

					Vector2 perturbedSpeed = new Vector2(Main.rand.NextFloat(2f, 7f) * (Main.rand.NextBool() ? 1f : -1f), Main.rand.NextFloat(1f, 4f)); // Watch out for dividing by 0 if there is only 1 projectile.
					perturbedSpeed *= Main.rand.NextFloat(0.7f, 2.2f);
					int proj = Projectile.NewProjectile((theone.Center.X - perturbedSpeed.X * ((float)(8f))) + ((perturbedSpeed.X > 0 ? -0.5f : 0.5f) * theone.width), (theone.Center.Y - 64) - perturbedSpeed.Y * 12f, perturbedSpeed.X, perturbedSpeed.Y, type, (int)((float)damage * 1f), 0, player.whoAmI);
					Main.projectile[proj].DamageType = DamageClass.Melee;
					// Main.projectile[proj].magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
					Main.projectile[proj].hostile = false;
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].netUpdate = true;
					SoundEngine.PlaySound(SoundID.Item18, Main.projectile[proj].Center);
					IdgProjectile.Sync(proj);
				}
			}


		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			float rotation = MathHelper.ToRadians(16);

			CaliburnTypeA.SpectralSummon(player,type,damage,player.Center);

			/*List<NPC> guys = new List<NPC>();

			for (int a = 0; a < Main.maxNPCs; a++)
			{
				NPC npchim = Main.npc[a];
				Projectile proj = new Projectile();
				proj.SetDefaults(ProjectileID.ChlorophyteBullet);

				if (npchim.active && !npchim.friendly && !npchim.dontTakeDamage && npchim.CanBeChasedBy(proj) && (npchim.Center-player.Center).Length()<400)
				{
					guys.Add(npchim);
				}

			}
			if (guys.Count > 0)
			{
				for (int i = 0; i < numberProjectiles; i++)
				{
					NPC theone = guys[Main.rand.Next(0, guys.Count)];

					Vector2 perturbedSpeed = new Vector2(Main.rand.NextFloat(2f, 7f) * (Main.rand.NextBool() ? 1f : -1f), Main.rand.NextFloat(1f, 4f)); // Watch out for dividing by 0 if there is only 1 projectile.
					perturbedSpeed *= Main.rand.NextFloat(0.7f, 2.2f);
					int proj = Projectile.NewProjectile((theone.Center.X- perturbedSpeed.X*((float)(8f)))+ ((perturbedSpeed.X > 0 ? -0.5f : 0.5f) *theone.width), (theone.Center.Y-64)- perturbedSpeed.Y*12f, perturbedSpeed.X, perturbedSpeed.Y, type, (int)((float)damage * 1f), 0, player.whoAmI);
					Main.projectile[proj].melee = true;
					Main.projectile[proj].magic = false;
					Main.projectile[proj].hostile = false;
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].netUpdate = true;
					Main.PlaySound(SoundID.Item18, Main.projectile[proj].Center);
					IdgProjectile.Sync(proj);
				}
			}*/

			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "Moredamnage", "Damage improves by 25% per spirit defeated"));
			tooltips.Add(new TooltipLine(Mod, "Moredamnage", "Damage increase: "+ (SGAWorld.downedCaliburnGuardians-1)*25+"%"));
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			if (SGAWorld.downedCaliburnGuardians > 1)
			add += (float)((SGAWorld.downedCaliburnGuardians - 1) * 0.25f);
		}

	}

	public class CaliburnTypeB : CaliburnTypeA
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn");
			Tooltip.SetDefault("One of the lost swords\nSummons a spectral blade to home in on your mouse cursor for a few seconds or until it hits 3 times, then it returns to you.\nYou can only summon 1 Sword at a time and it does not hit while returning");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.crit = 0;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 40;
			Item.useAnimation = 30;
			Item.reuseDelay = 30;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.shoot = Mod.Find<ModProjectile>("CaliburnHomingSword").Type;
			Item.shootSpeed = 20f;
			Item.useTurn = false;
			Item.autoReuse = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/CaliburnBlade2").Value;
				Item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Color.White * 0.25f;
				};
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			damage *= 2;
			return (player.ownedProjectileCounts[Mod.Find<ModProjectile>("CaliburnHomingSword").Type] < 1);
		}


	}

	public class CaliburnHomingSwordTrue : CaliburnHomingSword
	{
		public override float maxspeed => 8f;
		public override float minspeed => 3f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn");
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			AIType = ProjectileID.Boulder;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.light = 1f;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 2;
			Projectile.penetrate = 1005;
			Projectile.timeLeft = 400;
			Projectile.tileCollide = false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			CaliburnTypeA.SpectralSummon(Main.player[Projectile.owner], Mod.Find<ModProjectile>("CaliburnSpectralBlade").Type, damage, Projectile.Center);
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Caliburn/CaliburnTypeB"; }
		}
	}


		public class CaliburnHomingSword : ModProjectile
	{
		private float[] oldRot = new float[12];
		private Vector2[] oldPos = new Vector2[12];
		private int subdamage;
		public float appear = 1f;
		public float gtime = 0;
		public virtual float maxspeed => 6f;
		public virtual float minspeed => 3f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn");
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}


		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			AIType = ProjectileID.Boulder;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.light = 1f;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 2;
			Projectile.penetrate = 999;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = false;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Caliburn/CaliburnTypeB"; }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[Projectile.type];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldRot.Length - 1; k >= 0; k -= 1)
			{

				if (GetType() == typeof(CaliburnHomingSwordTrue))
				{
					lightColor = Main.hslToRgb(((gtime+ (k*0.5f)) / 15f) % 1f, 0.9f, 0.75f);
				}


				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, ((lightColor * alphaz) * (appear))*0.2f, oldRot[k], drawOrigin, Projectile.scale, Projectile.direction < 0 ? SpriteEffects.None : SpriteEffects.None, 0f);
			}
			return false;
		}

		public override bool? CanHitNPC(NPC target)
		{
				if (Projectile.penetrate < 600)
				return false;
			return base.CanHitNPC(target);
		}

		public virtual void trailingeffect()
		{

			if (gtime<5f)
			gtime = Main.GlobalTimeWrappedHourly;

			Rectangle hitbox = new Rectangle((int)Projectile.position.X - 10, (int)Projectile.position.Y - 10, Projectile.height + 10, Projectile.width + 10);

			for (int k = oldRot.Length - 1; k > 0; k--)
			{
				oldRot[k] = oldRot[k - 1];
				oldPos[k] = oldPos[k - 1];

				if (Main.rand.Next(0, 10) == 1)
				{
					int typr = Mod.Find<ModDust>("TornadoDust").Type;
					bool itt;
					itt = (GetType() == typeof(CaliburnHomingSwordTrue));
					if (itt)
					{
						typr = 124;
					}

					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, typr);
					Main.dust[dust].scale = 0.75f * appear;
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Vector2 normvel = Projectile.velocity;
					normvel.Normalize(); normvel *= 16f;

					Main.dust[dust].velocity = (randomcircle / 1f) + (-normvel);
					Main.dust[dust].noGravity = true;
					if (itt)
					{
						Main.dust[dust].color = Main.hslToRgb((gtime / 15f)%1f, 0.75f, Main.rand.NextFloat(0.45f,0.75f));
					}

				}

			}

		}

		public override void AI()
		{

			trailingeffect();

			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45);

			oldRot[0] = Projectile.rotation;
			oldPos[0] = Projectile.Center;

			if (Projectile.timeLeft < 100 || Projectile.penetrate < 997) {
			Projectile.timeLeft = 100;
			Projectile.penetrate = 500;
			}
			Projectile.velocity *= 0.99f;
			Player owner = Main.player[Projectile.owner];
			if (Main.myPlayer == owner.whoAmI) {
				Vector2 diff = Main.MouseWorld - Projectile.Center;
				if (Projectile.penetrate < 997)
				diff = owner.Center - Projectile.Center;
				diff.Normalize();
				Projectile.velocity += diff*0.15f;
				Projectile.netUpdate = true;
			}

			if (Projectile.velocity.Length() < minspeed)
			{
				Projectile.velocity.Normalize(); Projectile.velocity *= minspeed;
			}

			if (Projectile.velocity.Length() > maxspeed)
			{
				Projectile.velocity.Normalize(); Projectile.velocity *= maxspeed;
			}

			if (Projectile.penetrate < 600)
				appear = Math.Max(appear - 0.0025f, 0.0f);

			if ((Projectile.penetrate < 600 && (owner.Center - Projectile.Center).Length() < 32) || appear<0.1)
				Projectile.Kill();

		}

	}


	public class CaliburnSpectralBlade : ModProjectile
	{
		private float[] oldRot = new float[3];
		private int subdamage;
		public float appear = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn");
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[Projectile.type];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldRot.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((Projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, (lightColor * alphaz)*Math.Min((float)Projectile.timeLeft/30f,Math.Min(appear,1f)), oldRot[k], drawOrigin, Projectile.scale, Projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			return false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.penetrate < 1000)
			return false;
			return base.CanHitNPC(target);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.penetrate = 50;
		}

		public override void AI()
		{
			if (Projectile.penetrate < 1000)
			{
				if (Projectile.timeLeft > 30)
					Projectile.timeLeft = 30;
				Projectile.velocity /= 1.15f;
			}


			Rectangle hitbox = new Rectangle((int)Projectile.position.X - 10, (int)Projectile.position.Y - 10, Projectile.height + 10, Projectile.width + 10);

			appear += 0.15f;
			for (int k = oldRot.Length - 1; k > 0; k--)
			{
				oldRot[k] = oldRot[k - 1];

				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 124);
				Main.dust[dust].scale = 0.5f;
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Vector2 normvel = Projectile.velocity;
				normvel.Normalize(); normvel *= 7f;


				if ((Projectile.timeLeft) < 31)
				{
					Main.dust[dust].scale *= ((float)Projectile.timeLeft / 30f);
					normvel *= ((float)Projectile.timeLeft / 30f);
				}

				Main.dust[dust].velocity = (randomcircle / 1f) + (-normvel);
				Main.dust[dust].noGravity = true;

			}
			oldRot[0] = Projectile.rotation;

			if (Projectile.hostile)
				Projectile.rotation += Projectile.velocity.Y * 0.05f;

			Projectile.rotation += Projectile.velocity.X*0.1f;
			Projectile.velocity=Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.velocity.X/3f));

		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			AIType = ProjectileID.Boulder;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.light = 0.5f;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 1000;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown=-1;
			Projectile.timeLeft = 150;
			Projectile.tileCollide = false;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Caliburn/CaliburnTypeA"; }
		}

	}


		public class CaliburnTypeC : CaliburnTypeA
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn");
			Tooltip.SetDefault("One of the lost swords\nFlings crystal shards from the blade.");
			Item.staff[Item.type] = true; 
		}
		
		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.crit = 15;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 3;
			Item.useAnimation = 21;
			Item.reuseDelay = 30;
			Item.useStyle = 1;
			Item.knockBack = 5;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.rare = 2;
	        Item.UseSound = SoundID.Item1;
	        Item.shoot=Mod.Find<ModProjectile>("SurtCharging").Type;
	        Item.shootSpeed=20f;
			Item.useTurn = false;
	     	Item.autoReuse = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/CaliburnBlade3").Value;
				Item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Color.White * 0.25f;
				};
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

				float speed = 1.5f;
				float numberProjectiles = 3;
				float rotation = MathHelper.ToRadians(16);
				//Main.PlaySound(SoundID.Item, player.Center,45);

			float speedvel = new Vector2(speedX, speedY).Length();

			Vector2 eree = player.itemRotation.ToRotationVector2();
			eree *= player.direction;
			speedX = eree.X* speedvel;
			speedY = eree.Y* speedvel;

			position += eree * 45f;

			for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				perturbedSpeed.RotatedBy(MathHelper.ToRadians(-45));
				perturbedSpeed *= Main.rand.NextFloat(0.8f, 1.2f);
				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.CrystalShard, (int)(damage * 0.25f), 0, player.whoAmI);
					Main.projectile[proj].DamageType = DamageClass.Melee;
					// Main.projectile[proj].magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
					Main.projectile[proj].hostile = false;
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].netUpdate = true;
					Main.projectile[proj].timeLeft = 180;
					Main.projectile[proj].localAI[0] = 1f;
					Main.projectile[proj].velocity = perturbedSpeed;
					IdgProjectile.Sync(proj);
				}

			return false;
		}


	public override void MeleeEffects(Player player, Rectangle hitbox)
	{

		for (int num475 = 0; num475 < 3; num475++)
		{
		int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.PinkCrystalShard);
		Main.dust[dust].scale=0.5f+(((float)num475)/3.5f);
		Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
		Main.dust[dust].velocity=(randomcircle/2f)+((player.itemRotation.ToRotationVector2()*5f).RotatedBy(MathHelper.ToRadians(-90)));
		Main.dust[dust].noGravity=true;
		}

		Lighting.AddLight(player.position, 0.9f, 0.1f, 0.5f);
	}
	
	


	}


}