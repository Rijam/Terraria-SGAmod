using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

using SGAmod.NPCs.Cratrosity;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class CrateBossWeaponMelee : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Midas Touch");
			Tooltip.SetDefault("Attacks always crit and deal double damage VS enemies debuffed with Midas\nDoesn't work with a Flask of Gold");
		}
		public override void SetDefaults()
		{
			Item.damage = 70;
			Item.DamageType = DamageClass.Melee;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = 1;
			Item.knockBack = 3;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = 7;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 124);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
				Main.dust[dust].noGravity = true;
				//Main.dust[dust].velocity.Normalize();
				//Main.dust[dust].velocity+=new Vector2(player.velocity.X/4,0f);
				//Main.dust[dust].velocity*=(((float)Main.rand.Next(0,100))/30f);
			}

			Lighting.AddLight(player.position, 0.1f, 0.1f, 0.9f);
		}

		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (target.HasBuff(BuffID.Midas) && !player.HasBuff(BuffID.WeaponImbueGold))
			{
				crit = true;
				damage *= 2;
			}
		}
	}
	public class CrateBossWeaponMagic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Philanthropist's Shower");
			Tooltip.SetDefault("Consumes Coins from the player's inventory\nDamage increases with higher value coins; more valuable coins are used first\nInflicts Midas on enemies\n'and so, I shall make it rain!'");
		}

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Magic;
			Item.width = 34;
			Item.mana = 8;
			Item.height = 24;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 100000;
			Item.rare = ItemRarityID.Lime;
			Item.shootSpeed = 8f;
			Item.noMelee = true;
			Item.shoot = 14;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.useTurn = false;
			Item.staff[Item.type] = true;
		}

		public override bool CanUseItem(Player player)
		{
			return (player.CountItem(ItemID.CopperCoin) + player.CountItem(ItemID.SilverCoin) + player.CountItem(ItemID.GoldCoin) + player.CountItem(ItemID.PlatinumCoin) > 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			int taketype = 3;
			int[] types = { ItemID.CopperCoin, ItemID.SilverCoin, ItemID.GoldCoin, ItemID.PlatinumCoin };
			int silver = player.CountItem(ItemID.SilverCoin);
			int gold = player.CountItem(ItemID.GoldCoin);
			int plat = player.CountItem(ItemID.PlatinumCoin);
			taketype = plat > 0 ? 3 : (gold > 0 ? 2 : (silver > 0 ? 1 : 0));
			int coincount = player.CountItem(types[taketype]);
			if (player.CountItem(types[taketype]) > 0)
			{
				player.ConsumeItem(types[taketype]);
				float[,] typesproj = { { ModContent.ProjectileType<GlowingCopperCoinPlayer>(), 1f }, { ModContent.ProjectileType<GlowingSilverCoinPlayer>(), 1.25f }, { ModContent.ProjectileType<GlowingGoldCoinPlayer>(), 1.75f }, { ModContent.ProjectileType<GlowingPlatinumCoinPlayer>(), 2.5f } };

				int numberProjectiles = 8 + Main.rand.Next(5);
				for (int index = 0; index < numberProjectiles; index = index + 1)
				{
					Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
					vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
					vector2_1.Y -= (float)(100 * (index / 4));
					float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
					float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
					if ((double)num13 < 0.0) num13 *= -1f;
					if ((double)num13 < 20.0) num13 = 20f;
					float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
					float num15 = Item.shootSpeed / num14;
					float num16 = num12 * num15;
					float num17 = num13 * num15;
					float morespeed = 0.75f + ((float)taketype * 0.2f);
					float SpeedX = (num16 * morespeed) + (float)Main.rand.Next(-40, 41) * 0.02f;
					float SpeedY = (num17 * morespeed) + (float)Main.rand.Next(-40, 41) * 0.02f;
					int thisone = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, (int)typesproj[taketype, 0], (int)(typesproj[taketype, 1] * (float)damage), knockBack, Main.myPlayer, 0.0f, 0f);
					Main.projectile[thisone].friendly = true;
					Main.projectile[thisone].hostile = false;
					Main.projectile[thisone].DamageType = DamageClass.Magic;
					// Main.projectile[thisone].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
					IdgProjectile.AddOnHitBuff(thisone, BuffID.Midas, 60 * 10);
				}



			}
			return false;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(1) == 0)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 15);
			}
		}

	}

	public class CrateBossWeaponRanged : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Jackpot");
			Tooltip.SetDefault("Launches money-filled rockets that explode into coins!\nInflicts Midas on enemies\n'Once was a pre-order bonus!'");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.SnowmanCannon);
			Item.damage = 40;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.knockBack = 6;
			Item.value = 100000;
			Item.DamageType = DamageClass.Ranged;
			Item.rare = 7;
			Item.shootSpeed = 14f;
			Item.noMelee = true;
			Item.useAmmo = AmmoID.Rocket;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -6);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float speed = 8f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(8);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			float rooffset = player.direction * MathHelper.PiOver2 * -0.15f;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy((MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				position = position.RotatedBy(rooffset, player.MountedCenter);
				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("JackpotRocket").Type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].timeLeft = 600;
				Main.projectile[proj].knockBack = Item.knockBack;
			}
			return false;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(1) == 0)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 15);
			}
		}


	}

	class CrateBossWeaponThrown : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Avarice");
			Tooltip.SetDefault("Throw coins that influx on one enemy, losing 50% damage per hit\nInflicts Midas on the 1st hit, and Shadowflame on 2nd hit\nHitting the 3rd will inflict Mircotransactions\nthis depletes enemy health and wealth\n'Greed is it's own corruption'");
		}

		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.damage = 75;
			Item.shootSpeed = 3f;
			Item.shoot = Mod.Find<ModProjectile>("AvariceCoin").Type;
			Item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			Item.width = 8;
			Item.height = 28;
			Item.maxStack = 1;
			Item.knockBack = 9;
			Item.consumable = false;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(0, 3, 0, 0);
			Item.rare = 7;
		}


		public override bool CanUseItem(Player player)
		{
			return true;
		}

	}

	public class AvariceCoin : NPCs.Cratrosity.GlowingGoldCoin
	{

		int fakeid = ProjectileID.GoldCoin;
		public int guyihit = -10;
		public int cooldown = -10;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Coin");
		}

		public override void SetDefaults()
		{
			Projectile.aiStyle = 18;
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 3;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			guyihit = -1;
			cooldown = -1;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (guyihit < 0)
			{
				return null;
			}
			else
			{
				if (guyihit != target.whoAmI && cooldown > 0)
					return false;
				if (cooldown > 0)
					return false;
			}
			return null;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(cooldown);
			writer.Write((short)guyihit);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			cooldown = reader.ReadInt32();
			guyihit = reader.ReadInt16();
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (guyihit < 1)
				guyihit = target.whoAmI;
			cooldown = 15;
			Projectile.tileCollide = false;
			Projectile.damage /= 2;
			target.immune[Projectile.owner] = 2;
			target.AddBuff(BuffID.Midas, 60 * 10);
			if (Projectile.penetrate < 3)
			target.AddBuff(BuffID.ShadowFlame, 60 * 10);
			if (Projectile.penetrate<2)
				target.AddBuff(ModContent.BuffType<Buffs.Microtransactions>(), 60 * 10);
			Projectile.netUpdate = true;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = fakeid;
			return true;
		}

		public override void AI()
		{
			Projectile.localAI[0] += 1;
			cooldown -= 1;
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
			if (guyihit > -1)
			{
				if (cooldown == 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Projectile.Center = Main.npc[guyihit].Center + (randomcircle * 256f);
					Projectile.velocity = -randomcircle * 8f;
				}
				if (Main.npc[guyihit].active == false || Main.npc[guyihit].life < 1)
				{
					guyihit = -10;
				}

			}
		}

	}

	public class CrateBossWeaponSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prosperity Rod");
			Tooltip.SetDefault("Summons Midas Portals to shower your enemies in wealth, painfully\nOrdering your minions to attack a target will move the center of the circle to the target\nThe portals will gain an extra weaker attack VS the closest enemy\nAttacks inflict Midas\n'money money, it acts so funny...'");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.knockBack = 3f;
			Item.mana = 10;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = 1;
			Item.value = Item.buyPrice(0, 4, 0, 0);
			Item.rare = 7;
			Item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = Mod.Find<ModBuff>("MidasMinionBuff").Type;
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = Mod.Find<ModProjectile>("MidasPortal").Type;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(Item.buffType, 2);

			return true;
		}

	}

	public class MidasPortal : ModProjectile
	{
		protected float idleAccel = 0.05f;
		protected float spacingMult = 1f;
		protected float viewDist = 400f;
		protected float chaseDist = 200f;
		protected float chaseAccel = 6f;
		protected float inertia = 40f;
		protected float shootCool = 90f;
		protected float shootSpeed;
		protected int shoot;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Midas Portal");
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[Projectile.type] = 1;
			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[Projectile.type] = true;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minion = true;
			// Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			Projectile.minionSlots = 1f;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			Projectile.penetrate = -1;
			Projectile.timeLeft = 60;
		}


		// Here you can decide if your minion breaks things like grass or pots
		public override bool? CanCutTiles()
		{
			return false;
		}

		// This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage()
		{
			return false;
		}

		public virtual void CreateDust()
		{
		}

		public virtual void SelectFrame()
		{
		}

		public override void AI()
		{
			//if (projectile.owner == null || projectile.owner < 0)
			//return;


			Player player = Main.player[Projectile.owner];
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<MidasMinionBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<MidasMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			Vector2 gothere = player.Center;
			Projectile.localAI[0] += 1;

			int target2 = Idglib.FindClosestTarget(0, Projectile.Center, new Vector2(0f, 0f), true, true, true, Projectile);
			NPC them = Main.npc[target2];
			NPC oldthem = null;

			if (player.HasMinionAttackTargetNPC)
			{
				oldthem = them;
				them = Main.npc[player.MinionAttackTargetNPC];
				gothere = them.Center;
			}

			if (them != null && them.active)
			{
				if ((them.Center - Projectile.Center).Length() < 500 && Collision.CanHitLine(new Vector2(Projectile.Center.X, Projectile.Center.Y), 1, 1, new Vector2(them.Center.X, them.Center.Y), 1, 1) && them.CanBeChasedBy())
				{
					Projectile.ai[0] += 1;

					if (Projectile.ai[0] % 20 == 0)
					{
						SoundEngine.PlaySound(18, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0, 1f, 0.25f);
						int thisoned = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<GlowingGoldCoin>(), Projectile.damage, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
						Main.projectile[thisoned].minion = true;
						Main.projectile[thisoned].velocity = (them.Center - Projectile.Center);
						Main.projectile[thisoned].velocity.Normalize(); Main.projectile[thisoned].velocity *= 12f; Main.projectile[thisoned].velocity = Main.projectile[thisoned].velocity.RotateRandom(MathHelper.ToRadians(15));
						Main.projectile[thisoned].penetrate = 1;
						// Main.projectile[thisoned].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
						Main.projectile[thisoned].friendly = true;
						Main.projectile[thisoned].hostile = false;
						Main.projectile[thisoned].netUpdate = true;
						IdgProjectile.AddOnHitBuff(thisoned, BuffID.Midas, 60 * 5);
						IdgProjectile.Sync(thisoned);
					}

				}

				if (oldthem != null)
				{
					if ((oldthem.Center - Projectile.Center).Length() < 500 && Collision.CanHitLine(new Vector2(Projectile.Center.X, Projectile.Center.Y), 1, 1, new Vector2(oldthem.Center.X, oldthem.Center.Y), 1, 1) && oldthem.CanBeChasedBy())
					{

						if (Projectile.ai[0] % 35 == 0)
						{
							SoundEngine.PlaySound(18, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0, 0.75f, -0.5f);
							int thisoned = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<GlowingSilverCoin>(), (int)((float)Projectile.damage * 0.75f), Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
							Main.projectile[thisoned].minion = true;
							Main.projectile[thisoned].velocity = (oldthem.Center - Projectile.Center);
							Main.projectile[thisoned].velocity.Normalize(); Main.projectile[thisoned].velocity *= 10f; Main.projectile[thisoned].velocity = Main.projectile[thisoned].velocity.RotateRandom(MathHelper.ToRadians(15));
							Main.projectile[thisoned].penetrate = 1;
							// Main.projectile[thisoned].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
							Main.projectile[thisoned].friendly = true;
							Main.projectile[thisoned].hostile = false;
							Main.projectile[thisoned].netUpdate = true;
							IdgProjectile.AddOnHitBuff(thisoned, BuffID.Midas, 60 * 2);
							IdgProjectile.Sync(thisoned);
						}
					}

				}


			}

			int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 124);
			Main.dust[dust].scale = 0.7f;
			Main.dust[dust].velocity = Projectile.velocity * 0.2f;
			Main.dust[dust].noGravity = true;

			float us = 0f;
			float maxus = 0f;

			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (currentProjectile.active // Make sure the projectile is active
				&& currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
				&& currentProjectile.type == Projectile.type)
				{ // Make sure the projectile is of the same type as this javelin

					if (i == Projectile.whoAmI)
						us = maxus;
					maxus += 1f;

				}

			}
			Vector2 there = player.Center;

			double angles = MathHelper.ToRadians((float)((us / maxus) * 360.00) - 90f);
			float dist = 256f;//Main.rand.NextFloat(54f, 96f);
			Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles)) * dist;
			Vector2 where = gothere + here;

			if ((where - Projectile.Center).Length() > 8f)
			{
				Projectile.velocity += (where - Projectile.Center) * 0.005f;
				Projectile.velocity *= 0.975f;
			}
			float maxspeed = Math.Min(Projectile.velocity.Length(), 16);
			Projectile.velocity.Normalize();
			Projectile.velocity *= maxspeed;



			Lighting.AddLight(Projectile.Center, Color.Yellow.ToVector3() * 0.78f);

		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Javelins/StoneJavelin"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = SGAmod.ExtraTextures[95];

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
			Color color = Color.Lerp((Projectile.GetAlpha(lightColor) * 0.5f), Color.White, 0.5f); //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = (int)(Projectile.localAI[0] / 8f);
			timing %= 4;
			timing *= ((tex.Height) / 4);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 4), color, Projectile.velocity.X * 0.04f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

	}
	public class MidasMinionBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Midas Portal");
			Description.SetDefault("Portals from Planes of Wealth will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/MidasMinionBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("MidasPortal").Type] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}

	public class CrateBossWeaponMeleeOld : CrateBossWeaponMelee
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fortune Falchion");
			Tooltip.SetDefault("Sucks in nearby money when held\nHitting enemies debuffed with Midas increases the money they drop by 5% of the weapon's value\n^This stacks for each hit\nEquiping the [i:" + Mod.Find<ModItem>("IdolOfMidas") .Type+ "] greatly improves this weapon's effects");
		}
		public override void SetDefaults()
		{
			Item.damage = 32;
			Item.DamageType = DamageClass.Melee;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = 1;
			Item.knockBack = 3;
			Item.value = Item.buyPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.FalconBlade, 1).AddRecipeGroup("SGAmod:Tier4Bars", 10).AddTile(TileID.MythrilAnvil).Register();
		}

		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (target.HasBuff(BuffID.Midas))
			{
				target.value += (int)(Item.value * (player.SGAPly().MidasIdol > 0 ? 0.20f : 0.05f));
			}
		}

	}

	public class GlowingCopperCoinPlayer : GlowingCopperCoin, IDrawAdditive
	{
		protected override int FakeID2 => ProjectileID.CopperCoin;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Copper Coin");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.friendly = true;
			Projectile.hostile = false;
		}
		public override string Texture
		{
			get { return "Terraria/Coin_" + 0; }
		}
	}
	public class GlowingSilverCoinPlayer : GlowingCopperCoinPlayer, IDrawAdditive
	{
		protected override int FakeID2 => ProjectileID.SilverCoin;
		protected override Color GlowColor => Color.Silver;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Silver Coin");
		}
		public override string Texture
		{
			get { return "Terraria/Coin_" + 1; }
		}
	}
	public class GlowingGoldCoinPlayer : GlowingCopperCoinPlayer, IDrawAdditive
	{
		protected override int FakeID2 => ProjectileID.GoldCoin;
		protected override Color GlowColor => Color.Gold;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Gold Coin");
		}
		public override string Texture
		{
			get { return "Terraria/Coin_" + 2; }
		}
	}
	public class GlowingPlatinumCoinPlayer : GlowingCopperCoinPlayer, IDrawAdditive
	{
		protected override int FakeID2 => ProjectileID.PlatinumCoin;
		protected override Color GlowColor => new Color(229, 228, 226);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Platinum Coin");
		}
		public override string Texture
		{
			get { return "Terraria/Coin_" + 3; }
		}
	}

}