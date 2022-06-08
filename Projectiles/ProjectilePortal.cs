using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Projectiles
{
	public class ProjectilePortal : ModProjectile
	{
		public float scale = 0f;
		public int counter = 0;
		private int counter2 = 0;
		public virtual int takeeffectdelay => 0;
		public virtual float damagescale => 1f;
		public virtual int penetrate => 1;
		public virtual int openclosetime => 30;
		public virtual int timeleftfirerate => 30;


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spawner");
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + 658; }
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			//projectile.aiStyle = 1;
			Projectile.friendly = true;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			Projectile.timeLeft = 100;
			Projectile.tileCollide = false;
			AIType = -1;
		}

		public virtual void Explode()
		{

			if (Projectile.timeLeft == timeleftfirerate && Projectile.ai[0] > 0)
			{
				Player owner = Main.player[Projectile.owner];
				if (owner != null && !owner.dead)
				{

					Vector2 gotohere = new Vector2();
					gotohere = Projectile.velocity;//Main.MouseScreen - projectile.Center;
					gotohere.Normalize();

					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(50))*Projectile.velocity.Length();
					int proj=Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), (int)Projectile.ai[0], (int)(Projectile.damage* damagescale), Projectile.knockBack/10f, owner.whoAmI);
					Main.projectile[proj].DamageType = DamageClass.Magic;
					Main.projectile[proj].timeLeft = 300;
					Main.projectile[proj].penetrate = penetrate;
					IdgProjectile.Sync(proj);
				}

			}

		}

		public override void AI()
		{
			Projectile.rotation += 0.1f;
			counter += 1;

				scale = Math.Min(Math.Min((float)(counter-takeeffectdelay) / openclosetime, 1), (float)Projectile.timeLeft / (float)openclosetime);

			if (scale > 0)
			{

				int dustType = 43;
				int dustIndex = Dust.NewDust(Projectile.Center + new Vector2(-16, -16), 32, 32, dustType);
				Dust dust = Main.dust[dustIndex];
				dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
				dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
				dust.scale *= (3f + Main.rand.Next(-30, 31) * 0.01f) * scale;
				dust.fadeIn = 0f;
				dust.noGravity = true;
				dust.color = Main.DiscoColor;//Main.hslToRgb(Main.DiscoColor., 1f, 0.9f);
					//GetSecondaryShader(player.dye[slot].dye, player);

				Projectile.position -= Projectile.velocity;

				Explode();

			}


		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			if (scale > 0)
			{
				Texture2D inner = SGAmod.ExtraTextures[98];
				Texture2D texture = SGAmod.ExtraTextures[99];
				Texture2D outer = SGAmod.ExtraTextures[101];
				spriteBatch.Draw(inner, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Magenta, lightColor, 0.6f), (float)Math.Sin((double)Projectile.rotation), new Vector2(inner.Width / 2, inner.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;
				spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Magenta, lightColor, 0.75f), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;
				spriteBatch.Draw(outer, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Magenta, lightColor, 0.75f), -Projectile.rotation, new Vector2(outer.Width / 2, outer.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;
			}
				return false;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = ProjectileID.ImpFireball;
			return true;
		}

	}
}