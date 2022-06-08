using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Projectiles
{

	public class WaveBeam : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Beam");
		}

	public override bool? CanHitNPC(NPC target){return false;}

			public override string Texture
		{
			get { return("SGAmod/Projectiles/WaveProjectile");}
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile=false;
			Projectile.friendly=true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			AIType = 0;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void AI()
		{
			Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 3f;

			if (Projectile.ai[0] == 0) {
				Projectile.ai[0] = 1;
				int proj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, Mod.Find<ModProjectile>("WaveProjectile").Type, Projectile.damage, Projectile.knockBack, Projectile.owner);
				Main.projectile[proj].timeLeft = Projectile.timeLeft;
				Main.projectile[proj].penetrate = 1;
				Main.projectile[proj].ai[0] = (float)Math.PI;
				Main.projectile[proj].ai[1] = (float)Projectile.whoAmI;

				proj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, Mod.Find<ModProjectile>("WaveProjectile").Type, Projectile.damage, Projectile.knockBack, Projectile.owner);
				Main.projectile[proj].timeLeft = Projectile.timeLeft;
				Main.projectile[proj].penetrate = 1;
				Main.projectile[proj].ai[0] = (float)-Math.PI;
				Main.projectile[proj].ai[1] = (float)Projectile.whoAmI;
				//IdgProjectile.AddOnHitBuff(proj,BuffID.OnFire,60*10);
			}
		}



	}

	public class WaveProjectile : ModProjectile
	{

		Vector2 facing;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Beam");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile=false;
			Projectile.friendly=true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			AIType = 0;
		}

		public override bool PreKill(int timeLeft)
		{

		for (int num315 = 0; num315 < 25; num315 = num315 + 1)
			{
					int num622 = Dust.NewDust(new Vector2(Projectile.position.X-1,Projectile.position.Y), Projectile.width, Projectile.height, 185, 0f, 0f, 100, default(Color), 0.75f);
					Main.dust[num622].velocity *= 1f;

					Main.dust[num622].noGravity = true;
					Main.dust[num622].color=Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly/5)%1, 0.9f, 1f);
					Main.dust[num622].color.A=10;
					Main.dust[num622].velocity.X = facing.X + (Main.rand.Next(-250, 251) * 0.025f);
					Main.dust[num622].velocity.Y = facing.Y + (Main.rand.Next(-250, 251) * 0.025f);
					Main.dust[num622].alpha = 200;
			}

			return true;
		}

		public override void AI()
		{
		Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 3f;
		Projectile.ai[0]+=0.2f*(Projectile.ai[0]>0 ? 1f : -1f);

		Projectile.Center=Main.projectile[(int)Projectile.ai[1]].Center;
		double angle=((double)Projectile.ai[0])+ 2.0* Math.PI;

		float veladd2=(Main.projectile[(int)Projectile.ai[1]].velocity).ToRotation()+(float)(Math.PI/2.0);
		Vector2 veladd=new Vector2((float)Math.Cos(veladd2),(float)Math.Sin(veladd2))*(64f*(float)Math.Sin(angle));


		Projectile.velocity=Main.projectile[(int)Projectile.ai[1]].velocity+(veladd);

		facing=Main.projectile[(int)Projectile.ai[1]].velocity+(-veladd/12f);


		for (int num315 = 0; num315 < 1; num315 = num315 + 1)
			{
					int num622 = Dust.NewDust(new Vector2(Projectile.position.X-1,Projectile.position.Y)+Projectile.velocity, Projectile.width, Projectile.height, 185, 0f, 0f, 100, default(Color), 0.75f);
					Main.dust[num622].velocity *= 1f;

					Main.dust[num622].noGravity = true;
					Main.dust[num622].color=Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly/5)%1, 0.9f, 1f);
					Main.dust[num622].color.A=10;
					Main.dust[num622].velocity.X = facing.X/5 + (Main.rand.Next(-50, 51) * 0.025f);
					Main.dust[num622].velocity.Y = facing.Y/5 + (Main.rand.Next(-50, 51) * 0.025f);
					Main.dust[num622].alpha = 200;
			}

			Projectile.rotation = (float)Math.Atan2((double)facing.Y, (double)facing.X) + 1.57f; 



		}


	}

}