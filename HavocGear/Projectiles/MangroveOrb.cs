using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using SGAmod.Dusts;
using Terraria.Audio;


namespace SGAmod.HavocGear.Projectiles
{

	public class MangroveStaffOrb : MangroveOrb
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Orb");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 320;
			Projectile.alpha = 100;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.light = 0.4f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.AmethystBolt;
		}
	}

		public class MangroveOrb : ModProjectile
	{
		double keepspeed = 0.0;
		float homing = 0.15f;
		public float beginhoming = 20f;
		public Player P;
		public NPC target2;

		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Projectiles/MangroveOrb"); }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Orb");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 320;
			Projectile.alpha = 100;
			Projectile.light = 0.4f;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			AIType = ProjectileID.AmethystBolt;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0)
			{
				Projectile.Kill();
			}
			else
			{
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[Projectile.type].Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[Projectile.type], drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void AI()
		{
			if (Projectile.timeLeft < 200)
				Projectile.aiStyle = 1;
			Lighting.AddLight(Projectile.position, 0.0f, 0.3f, 0.1f);
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

			if (Main.rand.Next(3) == 0)
			{
				int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.MangroveDust>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 200, default(Color), 0.7f);
				Main.dust[dustIndex].velocity += Projectile.velocity * 0.3f;
				Main.dust[dustIndex].velocity *= 0.2f;
			}

			Projectile.ai[0] = Projectile.ai[0] + 1;
			if (Projectile.ai[0] < 2)
			{
				keepspeed = (Projectile.velocity).Length();
			}
			if (target2 == null || !target2.active)
				target2 = Main.npc[Idglib.FindClosestTarget(0, Projectile.Center, new Vector2(0f, 0f), true, true, true, Projectile)];
			if (target2 != null)
			{
				if ((target2.Center - Projectile.Center).Length() < 800f)
				{
					if (Projectile.ai[0] > (beginhoming))
					{
						Projectile.velocity = Projectile.velocity + (Projectile.DirectionTo(target2.Center) * ((float)keepspeed * homing));
						Projectile.velocity.Normalize();
						Projectile.velocity = Projectile.velocity * (float)keepspeed;
					}
				}

			}
		}

	}
}
