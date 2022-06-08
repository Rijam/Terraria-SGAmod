using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Dusts;
using Terraria.Audio;

namespace SGAmod.Projectiles
{

	public class UnmanedArrow : ModProjectile
	{
		protected double keepspeed = 0.0;
		protected virtual float homing => 0.03f;
		protected virtual float homingdist => 400f;
		protected virtual float maxhoming => 250f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Arrow");
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public virtual float beginhoming
		{
			get
			{
				return 20f;
			}
		}

		protected virtual float gravity
		{
			get
			{
				return 0.1f;
			}
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = ProjectileID.WoodenArrowHostile;
			effects(1);
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!target.friendly)
				Projectile.Kill();
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)Projectile.localAI[0]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.localAI[0] = (float)reader.ReadDouble();
		}

		public virtual void effects(int type)
		{
			if (type == 0)
			{
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 1; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y) + positiondust, Projectile.width, Projectile.height, Mod.Find<ModDust>("NovusSparkle").Type, 0f, 0f, 50, Main.hslToRgb(0.83f, 0.5f, 0.25f), 0.8f);
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.3f;
				}
			}
			if (type == 1)
			{
				SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 15; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y) + positiondust, Projectile.width, Projectile.height, Mod.Find<ModDust>("NovusSparkle").Type, Projectile.velocity.X + (float)(Main.rand.Next(-50, 50) / 15f), Projectile.velocity.Y + (float)(Main.rand.Next(-50, 50) / 15f), 50, Main.hslToRgb(0.83f, 0.5f, 0.25f), 2.2f);
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.7f;
				}

			}

		}

		public override void AI()
		{
			effects(0);

			Projectile.ai[0] = Projectile.ai[0] + 1;
			Projectile.velocity.Y += gravity;
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;


			if (Projectile.ai[0] < (beginhoming) && !Projectile.hostile)
			{
				Projectile.localAI[0] = -1;
				return;
			}
			Entity target = null;

			if (Projectile.localAI[0] > -1)
			{
				if (Projectile.hostile)
				target = Main.player[(int)Projectile.localAI[0]];
				else
				target = Main.npc[(int)Projectile.localAI[0]];


			}

			Projectile.localAI[1] += 1;
			float previousspeed = Projectile.velocity.Length();
			if (Projectile.localAI[0] < 0 || Projectile.localAI[1]%30==0 || ((!target.active || (target is NPC && ((target as NPC).dontTakeDamage || (target as NPC).life <1))) && Projectile.localAI[1] % 10 == 0))
			{
				Entity target2;
				if (Projectile.hostile)
					target2 = Main.player[Idglib.FindClosestTarget(1, Projectile.Center, new Vector2(0f, 0f), true, true, true, Projectile)];
				else
					target2 = Main.npc[Idglib.FindClosestTarget(0, Projectile.Center, new Vector2(0f, 0f), true, true, true, Projectile)];
					Projectile.localAI[0] = target2.whoAmI;
					target = target2;
			}

			if (target != null)
				{
				if ((target.Center - Projectile.Center).Length() < homingdist)
				{
					if (Projectile.ai[0] < (maxhoming))
					{
						Projectile.velocity = Projectile.velocity + (Projectile.DirectionTo(target.Center) * ((float)previousspeed * homing));
						Projectile.velocity.Normalize();
						Projectile.velocity = Projectile.velocity * previousspeed;
					}
				}
				}


		}






	}
	public class UnmanedArrow2 : UnmanedArrow
	{
		protected override float homing => 0.06f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Notchvos Arrow");
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
			Projectile.penetrate = 2;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.velocity = Projectile.velocity * -1;
			Projectile.position += Projectile.velocity * 1f;
			target.immune[Projectile.owner] = 1;
			//base.OnHitNPC(target, damage, knockback, crit);
		}

		public override void effects(int type)
		{
			if (type == 0)
			{
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 1; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y) + positiondust, Projectile.width, Projectile.height, Mod.Find<ModDust>("NovusSparkleBlue").Type, 0f, 0f, 50, Color.Lerp(Color.AliceBlue, Color.White, Main.rand.NextFloat()), 1.3f);
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.3f;
				}
			}
			if (type == 1)
			{
				SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 15; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y) + positiondust, Projectile.width, Projectile.height, Mod.Find<ModDust>("NovusSparkleBlue").Type, Projectile.velocity.X + (float)(Main.rand.Next(-50, 50) / 15f), Projectile.velocity.Y + (float)(Main.rand.Next(-50, 50) / 15f), 50, Color.Lerp(Color.AliceBlue, Color.White, Main.rand.NextFloat()), 3f);
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.7f;
				}

			}

		}


	}

	public class PitchArrow : UnmanedArrow
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("pitched Arrow");
		}

		public float getvalue()
		{


		return (float)Projectile.timeLeft;
		}


		public override float beginhoming => 99990f;

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(0,100)<20)
			IdgNPC.AddBuffBypass(target.whoAmI, BuffID.Oiled, 60 * 10);
			target.AddBuff(BuffID.Oiled, 60 * 20);
		}

		public override void effects(int type)
		{
			if (type == 0)
			{
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 1; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y) + positiondust, Projectile.width, Projectile.height, 109, Projectile.velocity.X*0.4f, Projectile.velocity.Y * 0.4f, 50, Color.Lerp(Color.AliceBlue, Color.White, Main.rand.NextFloat()), 1.3f);
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.3f;
				}
			}
			if (type == 1)
			{
				SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 25; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y) + positiondust, Projectile.width, Projectile.height, 109, Projectile.velocity.X / 4f + (float)(Main.rand.Next(-100, 100) / 15f), Projectile.velocity.Y / 4f + (float)(Main.rand.Next(-100, 100) / 15f), 50, Color.Lerp(Color.AliceBlue, Color.White, Main.rand.NextFloat()), 1f);
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.7f;
				}

			}

		}


	}

	public class DosedArrow : UnmanedArrow2
	{

		protected override float homing => 0.09f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dosed Arrow");
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public override float beginhoming
		{
			get
			{
				return 1f;
			}
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
			Projectile.penetrate = 2;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(0, 100) < 50)
				if (target.GetGlobalNPC<SGAnpcs>().Combusted < 1)
					target.AddBuff(Mod.Find<ModBuff>("DosedInGas").Type, 60 * 5);
			if (Main.rand.Next(0, 100) < 20)
			IdgNPC.AddBuffBypass(target.whoAmI, BuffID.Oiled, 60 * 10);
			base.OnHitNPC(target, damage, knockback, crit);
			if (target.HasBuff(BuffID.OnFire))
			{
				Projectile.type = ProjectileID.HellfireArrow;
				Projectile.penetrate = 0;
				Projectile.Kill();
				return;
			}
		}

		public override bool PreKill(int timeLeft)
		{
			if (Projectile.type != ProjectileID.HellfireArrow)
			Projectile.type = ProjectileID.WoodenArrowFriendly;
			effects(1);
			return true;
		}


		public override void effects(int type)
		{
			if (type == 0)
			{
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 1; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 211, Projectile.velocity.X, Projectile.velocity.Y, 50, Color.DarkGreen, 0.8f);
					Main.dust[num316].noLight = true;
					Dust dust = Main.dust[num316];
					dust.velocity *= 0.2f;
					Dust dust9 = Main.dust[num316];
					dust9.velocity.Y = dust9.velocity.Y + 0.2f;
					dust = Main.dust[num316];
					dust.velocity += Projectile.velocity*Main.rand.NextFloat(1f/6f, 0.5f);
				}
			}
			if (type == 1)
			{
				SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 25; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y) + positiondust, Projectile.width, Projectile.height, 211, Projectile.velocity.X / 4f + (float)(Main.rand.Next(-100, 100) / 15f), Projectile.velocity.Y / 4f + (float)(Main.rand.Next(-100, 100) / 15f), 50, Color.DarkGreen, Main.rand.NextFloat(0.7f,1.2f));
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.7f;
				}

			}

		}

	}
	public class DankArrow : PitchArrow
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dank Arrow");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.type = ProjectileID.BoneArrow;
			if (Main.rand.Next(0, 100) < 50 && !target.boss && !target.buffImmune[BuffID.Poisoned])
				target.AddBuff(Mod.Find<ModBuff>("DankSlow").Type, 60 * 3);
		}

		public override bool PreKill(int timeLeft)
		{
			effects(1);
			return true;
		}


		public override void effects(int type)
		{
			if (type == 0)
			{
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 3; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(Projectile.position+positiondust, Projectile.width, Projectile.height, 184, 0f, 0f, 50, Main.hslToRgb(0.15f, 1f, 1.00f), 1.00f);
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity = (randomcircle * 1.25f * Main.rand.NextFloat());
					dust3.velocity += Projectile.velocity / 5f;
					//dust.velocity += projectile.velocity * Main.rand.NextFloat(1f / 6f, 0.5f);
				}
			}
			if (type == 1)
			{
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 112, 0.33f, -0.125f);
				//Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 25; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(Projectile.position + positiondust, Projectile.width, Projectile.height, 184, 0f, 0f, 50, Main.hslToRgb(0.15f, 1f, 1.00f), 1.00f);
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity = (randomcircle * 3f * Main.rand.NextFloat());
					dust3.velocity += Projectile.velocity / 2f;
				}

			}

		}
	}
	public class WraithArrow : PitchArrow
	{

		double keepspeed = 0.0;
		float homing = 0.06f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wraith Arrow");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.arrow = true;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.type = ProjectileID.DD2BetsyArrow;
			if (Main.rand.Next(0, 100) < 50)
				target.AddBuff(BuffID.BetsysCurse, 60 * 8);
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.damage /= 5;
			effects(1);
			return true;
		}


		public override void effects(int type)
		{
			if (type == 0)
			{
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 3; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(Projectile.position+positiondust, Projectile.width, Projectile.height, 211, Projectile.velocity.X, Projectile.velocity.Y, 50, Color.DarkGreen, 1.8f);
					Main.dust[num316].noGravity = true;
					Dust dust = Main.dust[num316];
					dust.velocity *= 0.05f;
					Dust dust9 = Main.dust[num316];
					dust9.velocity.Y = dust9.velocity.Y + 0.2f;
					dust = Main.dust[num316];
					//dust.velocity += projectile.velocity * Main.rand.NextFloat(1f / 6f, 0.5f);
				}
			}
			if (type == 1)
			{
				SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 25; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y) + positiondust, Projectile.width, Projectile.height, 211, Projectile.velocity.X / 4f + (float)(Main.rand.Next(-100, 100) / 15f), Projectile.velocity.Y / 4f + (float)(Main.rand.Next(-100, 100) / 15f), 50, Color.DarkGreen, Main.rand.NextFloat(0.7f, 4.2f));
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.7f;
				}

			}

		}
	}


		public class WindfallArrow : UnmanedArrow
	{
		private Vector2[] oldPos = new Vector2[5];
		private float[] oldRot = new float[5];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Windfall Arrow");
		}

		public override float beginhoming => 99990f;
		protected override float gravity => 0.025f;

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			base.SetDefaults();
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player owner=Main.player[Projectile.owner];
			if (owner!=null && !owner.dead)
			{
				owner.wingTime = owner.wingTime > owner.wingTimeMax ? owner.wingTimeMax : owner.wingTime + 10;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[Projectile.type];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 10) / 2f;

			//oldPos.Length - 1
			for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.White, lightColor, (float)k/5);
				float alphaz= (1f - (float)(k + 1) / (float)(oldPos.Length + 2));
				spriteBatch.Draw(tex, drawPos, null, color* alphaz, oldRot[k], drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void effects(int type)
		{
			if (type == 0)
			{
				for (int k = oldPos.Length - 1; k > 0; k--)
				{
					oldPos[k] = oldPos[k - 1];
					oldRot[k] = oldRot[k - 1];
				}
				oldPos[0] = Projectile.Center;
				oldRot[0] = Projectile.rotation;
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * Main.rand.NextFloat(-0.75f,0.75f);
				for (int num315 = 0; num315 < 1; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y) + positiondust, Projectile.width, Projectile.height, ModContent.DustType<TornadoDust>(), Projectile.velocity.X * 0.0f, Projectile.velocity.Y * 0.0f, 50, Color.Lerp(Color.AliceBlue, Color.White, Main.rand.NextFloat())*0.5f, 0.5f);
					Main.dust[num316].noGravity = true;
				}
			}
			if (type == 1)
			{
				SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 25; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y) + positiondust, Projectile.width, Projectile.height, 109, Projectile.velocity.X / 4f + (float)(Main.rand.Next(-100, 100) / 15f), Projectile.velocity.Y / 4f + (float)(Main.rand.Next(-100, 100) / 15f), 50, Color.Lerp(Color.AliceBlue, Color.White, Main.rand.NextFloat()), 1f);
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.7f;
				}

			}

		}


	}


}