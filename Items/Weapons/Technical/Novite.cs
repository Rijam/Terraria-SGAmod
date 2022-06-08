using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Projectiles;
using Terraria.Audio;

namespace SGAmod.Items.Weapons.Technical
{
	public class NoviteKnife : SeriousSamWeapon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Knife");
			Tooltip.SetDefault("Instantly hits against targets where you swing\nHitting some types of targets in the rear will backstab, automatically becoming a crit\nHolding this weapon increases your movement speed and jump height");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();

			Item.damage = 36;
			Item.crit = 25;
			Item.width = 48;
			Item.height = 48;
			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.rare = ItemRarityID.Green;
			Item.value = 2500;
			Item.useStyle = 1;
			Item.useAnimation = 50;
			Item.useTime = 50;
			Item.knockBack = 8;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.consumable = false;
			Item.noMelee = true;
			Item.shootSpeed = 2f;
			Item.shoot = ModContent.ProjectileType<NoviteStab>();
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Weapons/Technical/NoviteKnife").Value;
				Item.GetGlobalItem<ItemUseGlow>().angleAdd = MathHelper.ToRadians(-20);
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("NoviteBar"), 10).AddTile(TileID.Anvils).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Knife").WithVolume(.7f).WithPitchVariance(.15f), player.Center);
			return true;
		}

	}

	public class NoviteStab : ModProjectile,ITrueMeleeProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 40;
			Projectile.extraUpdates = 40;
			AIType = -1;
			Main.projFrames[Projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stab");
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[Projectile.owner];
			if (target.spriteDirection== player.direction)
			{
				if ((target.aiStyle > 1 && target.aiStyle < 10) || target.aiStyle == 14 || target.aiStyle == 16 || target.aiStyle == 26 || target.aiStyle == 39 || target.aiStyle == 41 || target.aiStyle == 44)
					crit = true;

			}
		}

		public override void AI()
		{
			Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
		}
	}

	public class NoviteBlaster : SeriousSamWeapon, ITechItem
	{
		private bool altfired = false;
		public float ElectricChargeScalingPerUse() => 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Blaster");
			Tooltip.SetDefault("Fires a piercing bolt of electricity\nConsumes Electric Charge, 50 up front, 200 over time to charge up\nHold the fire button to charge a stronger, more accurate shot\nCan deal up to 3X damage and chain once at max charge");
		}

		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.DamageType = DamageClass.Magic;
			Item.width = 32;
			Item.height = 62;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.value = Item.buyPrice(0, 0, 25, 0);
			Item.rare = 2;
			//item.UseSound = SoundID.Item99;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 50f;
			Item.noUseGraphic = false;
			Item.channel = true;
			Item.reuseDelay = 5;
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().ConsumeElectricCharge(50, 0, consume: false);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("NoviteBar"), 10).AddTile(TileID.Anvils).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float rotation = MathHelper.ToRadians(0);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[ModContent.ProjectileType<NovaBlasterCharging>()] < 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<NovaBlasterCharging>(), damage, knockBack, player.whoAmI);
				player.SGAPly().ConsumeElectricCharge(50, 100);
			}
			return false;
		}

	}

	public class NovaBlasterCharging : ModProjectile
	{
		protected bool buttonReleased = false;
		public virtual int chargeuptime => 100;
		public virtual float velocity => 32f;
		public virtual float spacing => 24f;
		public virtual int fireRate => 5;
		public virtual int FireCount => 1;
		//public virtual float ForcedLock => 1f;
		public virtual (float,float) AimSpeed => (1f,0f);
		public int firedCount = 0;
		protected Player player;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Blaster Charging");
		}

        public override bool CanDamage()
        {
			return false;
        }

        public override string Texture
		{
			get { return ("SGAmod/Projectiles/WaveProjectile"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			AIType = 0;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public virtual void ChargeUpEffects()
		{

			if (Projectile.ai[0] < chargeuptime)
			{
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 5) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y) + randomcircle * 20, 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.75f);

						Main.dust[num622].scale = 1f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = -randomcircle.X;
						Main.dust[num622].velocity.Y = -randomcircle.Y;
						Main.dust[num622].alpha = 150;
					}
				}
			}
			else
			{
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 5) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y), 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.75f);

						Main.dust[num622].scale = 1.5f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = randomcircle.X * 2;
						Main.dust[num622].velocity.Y = randomcircle.Y * 2;
						Main.dust[num622].alpha = 100;
					}
				}
			}


			if (Projectile.ai[0] == chargeuptime)
			{
				for (int num315 = 0; num315 < 15; num315 = num315 + 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y), 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.5f);

					Main.dust[num622].scale = 2.8f;
					Main.dust[num622].noGravity = true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = randomcircle.X * 4f;
					Main.dust[num622].velocity.Y = randomcircle.Y * 4f;
					Main.dust[num622].alpha = 150;
				}
			}

		}

		public virtual void FireWeapon(Vector2 direction)
        {
			float perc = MathHelper.Clamp(Projectile.ai[0] / (float)chargeuptime, 0f, 1f);

			float speed = 3f + perc * 3f;

			Vector2 perturbedSpeed = (new Vector2(direction.X, direction.Y) * speed); // Watch out for dividing by 0 if there is only 1 projectile.

			Projectile.Center += Projectile.velocity;

			int prog = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<CBreakerBolt>(), (int)((float)Projectile.damage * (1f + (perc * 2f))), Projectile.knockBack, player.whoAmI, perc >= 0.99f ? 1 : 0, 0.50f + (perc * 0.20f));
			Main.projectile[prog].localAI[0] = (perc * 0.90f);
			Main.projectile[prog].DamageType = DamageClass.Magic;
			// Main.projectile[prog].melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Main.projectile[prog].netUpdate = true;

			IdgProjectile.Sync(prog);
			SoundEngine.PlaySound(SoundID.Item91, player.Center);

			if (firedCount>=FireCount)
			Projectile.Kill();
		}

		public virtual bool DoChargeUp()
        {
			return player.SGAPly().ConsumeElectricCharge(2, 60);
        }

		public override void AI()
		{
			Projectile.localAI[0] += 1;
			player = Main.player[Projectile.owner];

			if (player == null)
				Projectile.Kill();
			if (player.dead)
				Projectile.Kill();
			Projectile.timeLeft = 2;

			/*if (firedCount < FireCount && channeling)
			{
				player.itemTime = 6;
				player.itemAnimation = 6;
			}*/

			Vector2 direction = (Main.MouseWorld - player.MountedCenter);
			Vector2 directionmeasure = direction;
			direction.Normalize();

			bool cantchargeup = false;

			if (Projectile.ai[0] < chargeuptime + 1 && firedCount<1)
			{
				if (DoChargeUp())
					Projectile.ai[0] += 1;
				else
					cantchargeup = true;
			}

			bool channeling = (((player.channel && !buttonReleased) || (Projectile.ai[0] < 5 && !cantchargeup)) && !player.noItems && !player.CCed);
			bool aiming = true;// firedCount < FireCount;

			if (aiming || channeling)
			{
				Vector2 mousePos = Main.MouseWorld;
				if (Projectile.owner == Main.myPlayer)
				{
					Vector2 diff = mousePos - player.MountedCenter;
					diff.Normalize();
					Projectile.velocity = Vector2.Lerp(Vector2.Normalize(Projectile.velocity),diff, channeling ? AimSpeed.Item1 : AimSpeed.Item2);
					Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
					Projectile.netUpdate = true;
					Projectile.Center = mousePos;
				}
				int dir = Projectile.direction;
				player.ChangeDir(dir);

				player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir);
				Projectile.Center = player.MountedCenter + Projectile.velocity * velocity;

				if (channeling)
				{
					player.itemTime = fireRate;
					player.itemAnimation = fireRate;
				}
			}

			Projectile.Center = player.MountedCenter + Vector2.Normalize(Projectile.velocity) * spacing;

			if (Projectile.ai[0] > 10)
			{

				ChargeUpEffects();



				if (!channeling && player.itemTime<fireRate && firedCount < FireCount)
				{
					buttonReleased = true;
					firedCount += 1;
					player.itemTime = fireRate*(firedCount< FireCount ? 2 : 1);
					player.itemAnimation = fireRate * (firedCount < FireCount ? 2 : 1);
					FireWeapon(Vector2.Normalize(Projectile.velocity));
				}

			}
		}

	}

	public class NoviteTowerSummon : SeriousSamWeapon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Tesla Tower");
			Tooltip.SetDefault("Summons a Tesla Tower to zap enemies\nConsumes 25 Electric Charge per zap");
		}

		public override void SetDefaults()
		{
			Item.damage = 13;
			Item.DamageType = DamageClass.Summon;
			Item.sentry = true;
			Item.width = 24;
			Item.height = 30;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.noMelee = true;
			Item.knockBack = 2f;
			Item.value = Item.buyPrice(0, 0, 25, 0);
			Item.rare = 1;
			Item.autoReuse = false;
			Item.shootSpeed = 0f;
			Item.UseSound = SoundID.Item78;
			Item.shoot = ModContent.ProjectileType<NoviteTower>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse != 2)
			{
				position = Main.MouseWorld;
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
				player.UpdateMaxTurrets();
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("NoviteBar"), 10).AddTile(TileID.Anvils).Register();
		}
	}

	public class NoviteTower : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Novite Tower");
			//ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 52;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.sentry = true;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{

			if (Projectile.ai[0] == 0)
			{
				for (int i = 0; i < 4000; i += 1)
				{
					if (!Collision.CanHitLine(new Vector2(Projectile.Center.X, Projectile.position.Y + Projectile.height), 1, 1, new Vector2(Projectile.Center.X, Projectile.position.Y + Projectile.height + 2), 1, 1))
					{
						break;
					}
					Projectile.position.Y += 1;
				}
			}

			Player player = Main.player[base.Projectile.owner];
			Projectile.ai[0] += 1;
			if (Projectile.ai[0] > 30) {
				if (Projectile.ai[0] % 20 == 0)
				{
					NPC target = Main.npc[Idglib.FindClosestTarget(0, Projectile.Center - new Vector2(0f, 20f), new Vector2(0f, 0f), true, true, true, Projectile)];
					if (target != null && target.active && target.life>0 && Vector2.Distance(target.Center, Projectile.Center) < 300)
					{
						if (player.SGAPly().ConsumeElectricCharge(25, 60))
						{
							Vector2 there = Projectile.Center - new Vector2(3f, 20f);
							Vector2 Speed = (target.Center - there);
							Speed.Normalize(); Speed *= 2f;
							int prog = Projectile.NewProjectile(there.X, there.Y, Speed.X, Speed.Y, ModContent.ProjectileType<CBreakerBolt>(), Projectile.damage, 1f, player.whoAmI, 0);
							Main.projectile[prog].minion = true;
							// Main.projectile[prog].melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
							Main.projectile[prog].usesLocalNPCImmunity = true;
							Main.projectile[prog].localNPCHitCooldown = -1;
							IdgProjectile.Sync(prog);
							SoundEngine.PlaySound(SoundID.Item93, player.Center);
						}

					}

				}

				for (int num315 = 0; num315 < 3; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 15) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(3f, 20f) + randomcircle * 8, 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.75f);

						Main.dust[num622].scale = 1f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = randomcircle.RotatedBy(MathHelper.ToRadians(-90)).X;
						Main.dust[num622].velocity.Y = randomcircle.RotatedBy(MathHelper.ToRadians(-90)).Y;
						Main.dust[num622].alpha = 150;
					}
				}

			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texa = Main.projectileTexture[Projectile.type];
			spriteBatch.Draw(texa, Projectile.Center-Main.screenPosition, null, lightColor*MathHelper.Clamp(Projectile.ai[0]/30f,0f,1f), 0f, new Vector2(texa.Width, texa.Height)/2f, new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = Vector2.Zero;
			return false;
		}

		public override bool CanDamage()
		{
			return false;
		}
	}

}
