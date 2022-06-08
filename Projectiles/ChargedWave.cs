using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using Terraria.Audio;

namespace SGAmod.Projectiles
{

	public class ChargedWave : ModProjectile
	{

		double keepspeed=0.0;
		float homing=0.04f;
		public float beginhoming=5f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charged Beamshot");
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile=false;
			Projectile.friendly=true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
			AIType = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			for (int num315 = 0; num315 < 60; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 226, Projectile.velocity.X+(float)(Main.rand.Next(-250,250)/15f), Projectile.velocity.Y+(float)(Main.rand.Next(-250,250)/15f), 50, default(Color), 3.4f);
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
				dust3.noGravity = true;
				dust3.scale = 2.0f;
				dust3.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
			}
			return true;
		}

		public override void ModifyHitNPC (NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
		if (Main.rand.Next(0,100)<50)
		crit=true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		if (!target.friendly && target.realLife<0){
		int proj=Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("WaveBeamStun").Type, damage, knockback, Main.player[Projectile.owner].whoAmI);
		Main.projectile[proj].timeLeft=300;
		HalfVector2 half=new HalfVector2(target.position.X,target.position.Y);
		Main.projectile[proj].ai[0]=ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
		Main.projectile[proj].ai[1]=target.whoAmI;
		if (target.boss)
		Main.projectile[proj].timeLeft=80;
		Main.projectile[proj].netUpdate=true;
		Projectile.Kill();
		}}

		public override void AI()
		{
		for (int num315 = 0; num315 < 2; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 226, 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.15f), 1.7f);
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.3f;
				dust3.noGravity = true;
				dust3.scale = 1.8f;
				dust3.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
			}

		Projectile.ai[0]=Projectile.ai[0]+1;
		if (Projectile.ai[0]<2){
		keepspeed=(Projectile.velocity).Length();
		}
		NPC target=Main.npc[Idglib.FindClosestTarget(0,Projectile.Center,new Vector2(0f,0f),true,true,true,Projectile)];
		if (target!=null){
		if ((target.Center-Projectile.Center).Length()<500f){
		if (Projectile.ai[0]<(250f) && Projectile.ai[0]>(beginhoming)){
Projectile.velocity=Projectile.velocity+(Projectile.DirectionTo(target.Center)*((float)keepspeed*homing));
Projectile.velocity.Normalize();
Projectile.velocity=Projectile.velocity*(float)keepspeed;
}}




}

			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;



		}

	}

	public class WaveBeamStun : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stunning shot");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 24;
			Projectile.height = 24;
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

	public override bool? CanHitNPC(NPC target){return false;}

			public override string Texture
		{
			get { return("SGAmod/Projectiles/WaveProjectile");}
		}

		public override void AI()
		{

		NPC enemy=Main.npc[(int)Projectile.ai[1]];
		if (enemy==null)
		Projectile.Kill();
		if (enemy.active==false)
		Projectile.Kill();

		Vector2 lockhere = new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(Projectile.ai[0]) }.ToVector2();
		Projectile.position=enemy.position;
		enemy.position=lockhere;

		for (int num315 = 0; num315 < 1; num315 = num315 + 1)
			{
				Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
				int num316 = Dust.NewDust(new Vector2(enemy.position.X+(enemy.width/2), enemy.position.Y+(enemy.height/2))+((randomcircle*1.5f)*((float)Math.Pow(enemy.width*enemy.height,0.5))), 8, 8, 226, 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.15f), 1.7f);
				Dust dust3 = Main.dust[num316];
				dust3.velocity = -randomcircle*2;
				dust3.noGravity = true;
				dust3.scale = 0.25f;
				dust3.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
			}


		}

	}


}