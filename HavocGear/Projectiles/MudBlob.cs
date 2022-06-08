using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
	public class MudBlob : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 3;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.damage = 34;
			Projectile.timeLeft = 1000;
			Projectile.light = 0.1f;
			AIType = -1;
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mud Blob");
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			for (int num654 = 0; num654 < 10; num654++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
				int num655 = Dust.NewDust(Projectile.position + Vector2.UnitX * -20f, Projectile.width + 40, Projectile.height, 184, Projectile.velocity.X + randomcircle.X * 8f, Projectile.velocity.Y+ randomcircle.Y*8f, 100, new Color(30, 30, 30, 20), 2f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].velocity *= 0.5f;
			}

			for (int num172 = 0; num172 < Main.maxNPCs; num172 += 1)
			{
				NPC target = Main.npc[num172];
				float damagefalloff = 1f - ((target.Center - Projectile.Center).Length() / 120f);
				if ((target.Center - Projectile.Center).Length() < 120f && !target.friendly && !target.dontTakeDamage)
				{
					target.AddBuff(BuffID.Oiled, 60 + (int)(60f * damagefalloff * 6f));
				}
			}

			return true;
		}

		public override void AI()
		{
			if (Projectile.localAI[1] == 0f)
			{
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			}
		}
	}
}