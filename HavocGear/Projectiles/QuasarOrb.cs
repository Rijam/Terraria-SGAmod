using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Items.Weapons;
using Idglibrary;
using Terraria.Audio;

namespace SGAmod.HavocGear.Projectiles
{
	public class QuasarOrb : ModProjectile
	{
		private Vector2[] oldPos = new Vector2[3];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quasar Orb");     //The English name of the projectile
			//ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
			//ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
		}

		public override void SetDefaults()
		{
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = true;       
			Projectile.width = 4;
			Projectile.height = 4;
			AIType = ProjectileID.Bullet;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.light = 0.5f;
			//projectile.alpha = 166;
			Projectile.scale = 1.2f;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 1;
		}
		
		public override void AI()
		{
			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = Projectile.Center;

			NPC target=Main.npc[Idglib.FindClosestTarget(0,Projectile.Center,new Vector2(0f,0f),true,true,true,Projectile)];

			Projectile.ai[0]+=1;
			if (target!=null){
			Vector2 gohere=(target.Center-Projectile.Center);
			float dist=gohere.Length();
			gohere.Normalize();
			if (dist<1000f && Projectile.ai[0]>5 && Projectile.ai[0]<5000f){
			Projectile.ai[0]+=200f;
			Projectile.velocity+=gohere/2f;


			}}
			if (Projectile.velocity.Length()<7f){
			Projectile.velocity.Normalize();
			Projectile.velocity*=7f;
			}
			//if(Main.rand.Next(6) == 0)
			//{

				int num126 = Dust.NewDust(Projectile.Center, 0, 0, 173, Projectile.velocity.X, Projectile.velocity.Y, 0, default(Color), 3.5f);
				Main.dust[num126].noGravity = true;
				Main.dust[num126].velocity = Projectile.Center - Main.dust[num126].position;
				Main.dust[num126].velocity*=0.25f;

				num126 = Dust.NewDust(new Vector2(Projectile.position.X,Projectile.position.Y)+new Vector2(10-Main.rand.Next(0,20),10-Main.rand.Next(0,20)), Projectile.width, Projectile.height, 173, Projectile.velocity.X, Projectile.velocity.Y, 0, default(Color), 2f);
				Main.dust[num126].noGravity = true;
				Main.dust[num126].velocity = Projectile.Center - Main.dust[num126].position;
				Main.dust[num126].velocity.Normalize();
				Main.dust[num126].velocity*=1f;
				Dust dust3 = Main.dust[num126];
				dust3.velocity *= -5f;
				dust3 = Main.dust[num126];
				dust3.velocity += Projectile.velocity / 2f;
			//}
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Projectile.timeLeft > 5)
			{
				damage = (int)(damage * 1.5f);
				Projectile.timeLeft = 90000;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		if (!target.friendly)
		Projectile.Kill();
		}

		public override bool PreKill(int timeLeft)
		{
			if (Projectile.timeLeft > 90000-10 && Projectile.timeLeft<90010)
			{
				Projectile.damage = (int)(Projectile.damage / 1.5f);
			}
			for (int num172 = 0; num172 < Main.maxNPCs; num172 +=1)
                    {
                    NPC target=Main.npc[num172];
				float damagefalloff = 1f-((target.Center - Projectile.Center).Length()/180f);
					if ((target.Center-Projectile.Center).Length()<180f && !target.friendly && !target.dontTakeDamage)// && ((target.ModNPC!=null && target.ModNPC.CanBeHitByProjectile(projectile)==true) || target.ModNPC==null))
                    {
					Player owner = Main.player[Projectile.owner];
					//if (owner.active)
					//owner.ApplyDamageToNPC(target, (int)(projectile.damage * damagefalloff), 0f, 1, false);
					float damazz = (Main.DamageVar((float)Projectile.damage) * damagefalloff);
					target.StrikeNPC((int)damazz, 0f,0,false,true,true);
					if (Main.netMode != 0)
					{
						NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, target.whoAmI, damazz, 16f, (float)1, 0, 0, 0);
					}
				}
			}

			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			float ascale= ((GetType() == typeof(QuasarOrbLessParticles) ? 1.5f : 1f));
			for (int num315 = 0; num315 < 300; num315 = num315 + (GetType()==typeof(QuasarOrbLessParticles) ? 4 : 2))
			{
				float makesmaller = (float)(1.00 - (num315 / 300.00)) * ascale;
				Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize(); Vector2 ogcircle=randomcircle; randomcircle*=(float)(num315/300.00);
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y)+randomcircle*240f, Projectile.width, Projectile.height, 173, Projectile.velocity.X+(float)(Main.rand.Next(-250,250)/15f), Projectile.velocity.Y+(float)(Main.rand.Next(-250,250)/15f), 0, default(Color), 4.4f*makesmaller);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
				dust3.velocity += ogcircle*makesmaller*3;
			}
			return true;
		}

    		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[Mod.Find<ModProjectile>(this.GetType().Name).Type];
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

			/*for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				float alpha = 1f - (float)(k + 1) / (float)(oldPos.Length + 2);
				spriteBatch.Draw(texture, oldPos[k] - Main.screenPosition,null, lightColor * alpha, (float)Main.rand.Next(-314,314)*0.01f, origin,new Vector2(1f,1f), SpriteEffects.None, 0f);
			}*/

			spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition,null, lightColor, (float)Main.rand.Next(-314,314)*0.01f, origin,new Vector2(1f,1f), SpriteEffects.None, 0f);
			return true;

		}
		
	}
}