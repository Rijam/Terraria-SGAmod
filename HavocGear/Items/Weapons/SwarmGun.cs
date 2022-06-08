using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Items.Weapons.SeriousSam;
using Idglibrary;
using SGAmod.Items.Weapons.Technical;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class SwarmGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Buzz Blaster");
			Tooltip.SetDefault("Charges up a slow moving, concentrated fly swarm!\nCharging longer creates more flies");
		}

		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.DamageType = DamageClass.Magic;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 70;
			Item.useAnimation = 70;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.mana = 15;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Orange;
			//item.UseSound = SoundID.Item99;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SwarmGunCharging>();
			Item.shootSpeed = 3f;
			Item.noUseGraphic = false;
			Item.channel = true;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, -0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float rotation = MathHelper.ToRadians(0);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 2f;
			if (player.ownedProjectileCounts[Item.shoot] < 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, Item.shoot, damage, knockBack, player.whoAmI);
			}
			return false;
		}

	}

	public class SwarmGunCharging : NovaBlasterCharging
	{

		public override int chargeuptime => 180;
		public override float velocity => 40f;
		public override float spacing => 80f;
		public override int fireRate => 30;
		int chargeUpTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Buzz Charging");
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

		public override void ChargeUpEffects()
		{
			chargeUpTimer += 1;

			if (Projectile.ai[0] < chargeuptime)
			{
				if (chargeUpTimer % 4 == 0)
				{
					float perc = MathHelper.Clamp(Projectile.ai[0] / (float)chargeuptime, 0f, 1f);
					//SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_LightningAuraZap, (int)projectile.Center.X, (int)projectile.Center.Y);
					SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_KoboldHurt, (int)Projectile.Center.X, (int)Projectile.Center.Y);
					if (sound != null)
					{
						sound.Pitch = -0.5f + perc;
					}
				}
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 3) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y) + randomcircle * 20, 0, 0, 184, 0f, 0f, 100, Color.Green, 0.75f);

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
					if (Main.rand.Next(0, 2) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y), 0, 0, 184, 0f, 0f, 100, Color.Green, 0.75f);

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
				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 97);
				if (sound != null)
				{
					sound.Pitch += 0.5f;
				}

				for (int num315 = 0; num315 < 35; num315 = num315 + 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y), 0, 0, 195, 0f, 0f, 100, Color.Green, 0.5f);

					Main.dust[num622].scale = 2.8f;
					Main.dust[num622].noGravity = true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = randomcircle.X * 4f;
					Main.dust[num622].velocity.Y = randomcircle.Y * 4f;
					Main.dust[num622].alpha = 150;
				}
			}

		}

		public override bool DoChargeUp()
		{
			return player.CheckMana(player.HeldItem, Projectile.ai[0]%3==0 ? 1 : 0, true);
		}

		public override void FireWeapon(Vector2 direction)
		{
			float perc = MathHelper.Clamp(Projectile.ai[0] / (float)chargeuptime, 0f, 1f);

			float speed = 1f;

			Vector2 perturbedSpeed = (new Vector2(direction.X, direction.Y) * speed);

			Projectile.Center += Projectile.velocity;

			int damage = (int)(Projectile.damage * (Projectile.ai[0] / chargeuptime));

			if (Projectile.ai[0] >= chargeuptime/2 || !player.CheckMana(player.HeldItem, 5))
			{
				int type = ModContent.ProjectileType<SwarmGunProjectile>();
				Vector2 center = Projectile.Center - Vector2.Normalize(perturbedSpeed) * 32f;
				int prog = Projectile.NewProjectile(center.X, center.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, Projectile.knockBack, player.whoAmI,ai1: 0+(perc*20));
				Main.projectile[prog].penetrate = (int)(((perc-0.5f)*2f) * 50);
				Main.projectile[prog].netUpdate = true;

				SoundEngine.PlaySound(SoundID.Item73, player.Center);
				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Item, (int)center.X, (int)center.Y, 97);

				if (sound != null)
				{
					sound.Pitch -= 0.5f;
				}

				for (int num315 = 0; num315 < 35; num315 = num315 + 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(center, 0, 0, 195, 0f, 0f, 184, default(Color), 0.5f);

					Main.dust[num622].scale = 3.2f;
					Main.dust[num622].noGravity = true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = randomcircle.X * Main.rand.NextFloat(4f, 8f);
					Main.dust[num622].velocity.Y = randomcircle.Y * Main.rand.NextFloat(4f, 8f);
					Main.dust[num622].alpha = 50;
				}

				for (int num315 = 0; num315 < 20; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 2) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(center, 0, 0, 195, 0f, 0f, 100, Color.Lime, 0.75f);

						Main.dust[num622].scale = 1.5f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						randomcircle *= Main.rand.NextFloat(-2f, 4f + num315 / 4f);
						Main.dust[num622].velocity.X = randomcircle.X;
						Main.dust[num622].velocity.Y = randomcircle.Y;
						Main.dust[num622].alpha = 100;
					}
				}

			}
			Projectile.Kill();
		}

	}

	public class SwarmGunProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("SWARM!");
			//ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
		}

		public override string Texture => "SGAmod/NPCs/Dank/FlySwarm";

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 2000;
			Projectile.penetrate = 10;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8;
		}

		private void ReleaseFlies(int ammount)
		{
			for (int i = 0; i < ammount; i += 1)
			{
				Projectile.NewProjectile(Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.TwoPi), ModContent.ProjectileType<FlyProjectileMagic>(), Projectile.damage, 0f, Projectile.owner);
			}
			Projectile.penetrate -= 1;

			if (Projectile.penetrate < 1 && Projectile.timeLeft > 20)
			{
				Projectile.timeLeft = 20;
			}
		}

        public override bool PreKill(int timeLeft)
        {
			ReleaseFlies((int)(Projectile.penetrate+Projectile.ai[1]));
			return true;
        }

        public override void AI()
		{

			Player player = Main.player[base.Projectile.owner];
			Projectile.ai[0] += 1;
			if (Projectile.ai[0] > 5 && player != null)
			{
				if (Projectile.ai[0] % 20 == 0)
				{
					ReleaseFlies(1);
				}

			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texa = Main.projectileTexture[Projectile.type];
			int texsize = texa.Height / 4;
			Rectangle rect = new Rectangle(0, texsize * (int)((Projectile.ai[0] / 10f) % 4), texa.Width, texsize);
			spriteBatch.Draw(texa, Projectile.Center - Main.screenPosition, rect, lightColor * MathHelper.Clamp(Projectile.ai[0] / 10f, 0f, Math.Min(Projectile.timeLeft / 20f, 1f)), 0f, new Vector2(texa.Width, texa.Height/4f) / 2f, new Vector2(1, 1), SpriteEffects.None, 0f);
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

	public class FlyProjectileMagic : FlyProjectileThrown
	{

		public override string Texture => "SGAmod/NPCs/Dank/Fly";

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 200;
			Projectile.penetrate = 3;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 8;
		}
	}
}
