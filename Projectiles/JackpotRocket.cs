using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using Terraria.Audio;

namespace SGAmod.Projectiles
{

	public class JackpotRocket : ModProjectile
	{

		double keepspeed=0.0;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jackpot Rocket");
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile=false;
			Projectile.friendly=true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		target.immune[Projectile.owner] = 15;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type=ProjectileID.RocketI;
			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
			for (int num315 = 0; num315 < 40; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X-1, Projectile.position.Y)+positiondust, Projectile.width, Projectile.height, 31, Projectile.velocity.X+(float)(Main.rand.Next(-250,250)/15f), Projectile.velocity.Y+(float)(Main.rand.Next(-250,250)/15f), 50, Main.hslToRgb(0.15f, 1f, 1.00f), 2.5f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
			}
		int [] types = {ProjectileID.CopperCoin,ProjectileID.SilverCoin,Mod.Find<ModProjectile>("FallingGoldCoin").Type,ProjectileID.PlatinumCoin};
			for (int num315 = 1; num315 < 13; num315 = num315 + 1)
			{
				Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
				float velincrease=((float)(num315+8)/2f);
                int thisone=Projectile.NewProjectile(Projectile.Center.X-Projectile.velocity.X, Projectile.Center.Y-Projectile.velocity.Y, randomcircle.X*velincrease, randomcircle.Y*velincrease, types[2], (int)(Projectile.damage*0.25), Projectile.knockBack, Projectile.owner, 0.0f, 0f);
                Main.projectile[thisone].DamageType=DamageClass.Ranged;
                Main.projectile[thisone].friendly=Projectile.friendly;
                Main.projectile[thisone].hostile=Projectile.hostile;
                IdgProjectile.AddOnHitBuff(thisone,BuffID.Midas,60*10);
				Main.projectile[thisone].netUpdate = true;
				IdgProjectile.Sync(thisone);
            }

            int theproj=Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("Explosion").Type, (int)((double)Projectile.damage * 0.75f), Projectile.knockBack, Projectile.owner, 0f, 0f);
			Main.projectile[theproj].DamageType=DamageClass.Ranged;
			IdgProjectile.AddOnHitBuff(theproj,BuffID.Midas,60*10);

			return true;
		}

		public override void AI()
		{
		Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
		for (int num315 = 0; num315 < 3; num315 = num315 + 1)
			{
				Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X-1, Projectile.position.Y)+positiondust, Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type, 0f, 0f, 50, Main.hslToRgb(0.10f, 0.5f, 0.75f), 0.8f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity = (-Projectile.velocity)+(randomcircle*(0.5f))*((float)num315/3f);
				dust3.velocity.Normalize();
			}

		for (int num315 = 1; num315 < 16; num315 = num315 + 1)
			{
				if (Main.rand.Next(0,100)<25){
				Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X-1, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 50,Main.hslToRgb(0.15f, 1f, 1.00f), 0.33f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity = (randomcircle*2.5f*Main.rand.NextFloat())+(Projectile.velocity);
				dust3.velocity.Normalize();
			}}

		Projectile.ai[0]=Projectile.ai[0]+1;
		Projectile.velocity.Y+=0.1f;
		Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f; 
		}


	}

	public class FallingGoldCoin : NPCs.Cratrosity.GlowingGoldCoin, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Coin");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 180;
		}

		public override void AI()
		{
			Projectile.localAI[0] += 1;
			Projectile.velocity.Y+=0.2f;
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}

	}

}