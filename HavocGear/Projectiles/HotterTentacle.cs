using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
    public class HotterTentacle : ModProjectile
    {
    	
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = 5;
            Projectile.MaxUpdates = 3;
            Projectile.DamageType = DamageClass.Magic;
        }

        public override void AI()
        {
        	if (Projectile.velocity.X != Projectile.velocity.X)
			{
				if (Math.Abs(Projectile.velocity.X) < 1f)
				{
					Projectile.velocity.X = -Projectile.velocity.X;
				}
				else
				{
					Projectile.Kill();
				}
			}
			if (Projectile.velocity.Y != Projectile.velocity.Y)
			{
				if (Math.Abs(Projectile.velocity.Y) < 1f)
				{
					Projectile.velocity.Y = -Projectile.velocity.Y;
				}
				else
				{
					Projectile.Kill();
				}
			}
        	Vector2 center10 = Projectile.Center;
			Projectile.scale = 1f - Projectile.localAI[0];
			Projectile.width = (int)(20f * Projectile.scale);
			Projectile.height = Projectile.width;
			Projectile.position.X = center10.X - (float)(Projectile.width / 2);
			Projectile.position.Y = center10.Y - (float)(Projectile.height / 2);
			if ((double)Projectile.localAI[0] < 0.1)
			{
				Projectile.localAI[0] += 0.01f;
			}
			else
			{
				Projectile.localAI[0] += 0.025f;
			}
			if (Projectile.localAI[0] >= 0.95f)
			{
				Projectile.Kill();
			}
			Projectile.velocity.X = Projectile.velocity.X + Projectile.ai[0] * 1.5f;
			Projectile.velocity.Y = Projectile.velocity.Y + Projectile.ai[1] * 1.5f;
			if (Projectile.velocity.Length() > 16f)
			{
				Projectile.velocity.Normalize();
				Projectile.velocity *= 16f;
			}
			Projectile.ai[0] *= 1.05f;
			Projectile.ai[1] *= 1.05f;
			if (Projectile.scale < 1f)
			{
				int num897 = 0;
				while ((float)num897 < Projectile.scale * 1f)
				{
					int num898 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type, 0f, 0f, 150, Color.Orange, 1.15f);
					Main.dust[num898].position = (Main.dust[num898].position + Projectile.Center) / 2f;
					Main.dust[num898].noGravity = true;
					Main.dust[num898].velocity *= 0.1f;
					Main.dust[num898].velocity -= Projectile.velocity * (1.3f - Projectile.scale);
					//Main.dust[num898].fadeIn = (float)(100 + projectile.owner);
					Main.dust[num898].scale += Projectile.scale * 0.75f;
					num897++;
				}
				return;
			}
        }

	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        	Player player = Main.player[Projectile.owner];
		target.immune[Projectile.owner] = 2;
		target.AddBuff(Mod.Find<ModBuff>("ThermalBlaze").Type, 180);
			Projectile proj = Projectile.NewProjectileDirect(Projectile.Center+Main.rand.NextVector2CircularEdge(200f,200f),Vector2.Zero,ProjectileID.SpiritFlame,Projectile.damage, 0f, Projectile.owner,-300f);
			proj.localAI[0] = 60f;
			proj.netUpdate = true;

		}
    }
}