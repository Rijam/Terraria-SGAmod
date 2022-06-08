using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using Terraria.Audio;

namespace SGAmod.Projectiles
{

	public class UnmanedBolt : ModProjectile
	{

		double keepspeed = 0.0;
		float homing = 0.05f;
		public float beginhoming = 20f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orb thingy from Asterism");
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
			AIType = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			for (int num315 = 0; num315 < 15; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("NovusSparkle").Type, Projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), Projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 15f), 50, Main.hslToRgb(0.4f, 0f, 0.15f), 2.4f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!target.friendly)
				Projectile.Kill();
		}

		public override void AI()
		{
			for (int num315 = 0; num315 < 2; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("NovusSparkle").Type, 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.15f), 1.7f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.3f;
			}

			Projectile.ai[0] = Projectile.ai[0] + 1;
			if (Projectile.ai[0] < 2)
			{
				keepspeed = (Projectile.velocity).Length();
			}
			NPC target = Main.npc[Idglib.FindClosestTarget(0, Projectile.Center, new Vector2(0f, 0f), true, true, true, Projectile)];
			if (target != null)
			{
				if ((target.Center - Projectile.Center).Length() < 500f)
				{
					if (Projectile.ai[0] < (250f) && Projectile.ai[0] > (beginhoming))
					{
						Projectile.velocity = Projectile.velocity + (Projectile.DirectionTo(target.Center) * ((float)keepspeed * homing));
						Projectile.velocity.Normalize();
						Projectile.velocity = Projectile.velocity * (float)keepspeed;
					}
				}

				if (Projectile.ai[0] > 250f)
				{
					Projectile.Kill();
				}
			}
		}
	}
}