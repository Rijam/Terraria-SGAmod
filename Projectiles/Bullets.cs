using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Dusts;
using SGAmod.Effects;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Terraria.DataStructures;
using Terraria.Audio;

namespace SGAmod.Projectiles
{

	public class BlazeBullet : ModProjectile
	{

		double keepspeed = 0.0;
		float homing = 0.03f;
		public Player P;
		private Vector2[] oldPos = new Vector2[6];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazing Bullet");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 5;
			AIType = ProjectileID.Bullet;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + 14); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

			//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			//GameShaders.Armor.GetShaderFromItemId(ItemID.SolarDye).Apply(null);

			Texture2D tex = Mod.Assets.Request<Texture2D>("Items/Weapons/Ammo/BlazeBullet").Value;
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.White, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldPos.Length + 2)) * (k != oldPos.Length - 1 ? 0.5f : 1f);
				spriteBatch.Draw(tex, drawPos, null, color * alphaz, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}

			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			effects(1);
			Projectile.type = ProjectileID.Bullet;
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(0, 10) == 1)
				target.AddBuff(Mod.Find<ModBuff>("ThermalBlaze").Type, 60 * 3);
		}

		public virtual void effects(int type)
		{
			if (type == 1)
			{
				SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
				Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 5; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y) + positiondust, Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type, Projectile.velocity.X + (float)(Main.rand.Next(-50, 50) / 15f), Projectile.velocity.Y + (float)(Main.rand.Next(-50, 50) / 15f), 50, Main.hslToRgb(0.83f, 0.5f, 0.25f), 0.25f);
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.7f;
				}

			}

		}

		public override void AI()
		{

			if (Main.rand.Next(0, 8) == 1)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type);
				Main.dust[dust].scale = 0.4f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			}

			Projectile.position -= Projectile.velocity * 0.8f;

			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = Projectile.Center;

			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}



	}


	public class AcidBullet : ModProjectile
	{
		private Vector2[] oldPos = new Vector2[6];
		private float[] oldRot = new float[6];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Round");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 300;
			Projectile.extraUpdates = 5;
			AIType = ProjectileID.Bullet;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = ProjectileID.CursedBullet;
			return true;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + 14); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Mod.Assets.Request<Texture2D>("Items/Weapons/Ammo/AcidBullet").Value;
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldPos.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, color * alphaz, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void AI()
		{

			if (Main.rand.Next(0, 4) == 1)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("AcidDust").Type);
				Main.dust[dust].scale = 0.5f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			}

			Projectile.position -= Projectile.velocity * 0.8f;

			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = Projectile.Center;

			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(0, 3) < 2)
				target.AddBuff(Mod.Find<ModBuff>("AcidBurn").Type, 30 * (Main.rand.Next(0, 3) == 1 ? 2 : 1));
		}

	}

	public class SeekerBullet : ModProjectile
	{
		private Vector2[] oldPos = new Vector2[20];
		private float[] oldRot = new float[20];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seeker Bullet");
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 1200;
			Projectile.extraUpdates = 10;
			AIType = ProjectileID.Bullet;
		}

		public override bool PreKill(int timeLeft)
		{
			if (timeLeft > 0)
				Projectile.type = ProjectileID.SandBallGun;
			return true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/SeekerBullet"); }
		}

		public override void AI()
		{
			bool seeking = false;
			NPC target = null;
			if (Projectile.ai[1] <= 0)
			{
				Projectile.ai[1] = Projectile.velocity.Length();
			}
			Player owner = Main.player[Projectile.owner];

			if (owner.HasMinionAttackTargetNPC)
			{
				target = Main.npc[owner.MinionAttackTargetNPC];
				if (!target.dontTakeDamage && (target.Center - Projectile.Center).LengthSquared() < 600 * 600)
				{
					seeking = true;
				}
			}

			if (seeking)
			{
				Vector2 velocityShift = Vector2.Normalize(target.Center - Projectile.Center);
				Projectile.velocity += velocityShift * 0.15f;
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MathHelper.Clamp(Projectile.velocity.Length(), 0, Projectile.ai[1]);

				if (Main.rand.Next(0, 4) == 1)
				{
					int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 32);
					Main.dust[dust].scale = 0.5f;
					Main.dust[dust].noGravity = false;
					Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
				}
			}

			Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, 1f);

			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = Projectile.Center;

			Projectile.position -= Projectile.velocity * 0.9f;

			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//if (Main.rand.Next(0, 3) < 2)
			//	target.AddBuff(mod.BuffType("AcidBurn"), 30 * (Main.rand.Next(0, 3) == 1 ? 2 : 1));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Mod.Assets.Request<Texture2D>("Items/Weapons/Ammo/SeekerBullet").Value;
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.White, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = 1f - (k / (float)(oldPos.Length));
				spriteBatch.Draw(tex, drawPos, null, color * alphaz * Projectile.Opacity, Projectile.rotation, drawOrigin, Projectile.scale * 0.75f, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, 0, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

	}

	public class SoundboundBullet : ModProjectile
	{
		private Vector2[] oldPos = new Vector2[30];
		private float[] oldRot = new float[30];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soundbound Bullet");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 900;
			Projectile.extraUpdates = 4;
			Projectile.penetrate = 2;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			AIType = ProjectileID.Bullet;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = ProjectileID.LostSoulFriendly;
			return true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/SoulboundBullet"); }
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Player owner = Main.player[Projectile.owner];

			for (float f = 0; f < 20f; f += 1)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - (Vector2.Normalize(Projectile.velocity) * f * 5f), Projectile.width, Projectile.height, 186);
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = Projectile.velocity * Main.rand.NextFloat(0.01f, 0.1f);
			}

			owner.Hurt(PlayerDeathReason.ByOther(5), (int)(owner.statDefense * 2.00), 0, cooldownCounter: 1);

			SoundEffectInstance snd = SoundEngine.PlaySound(SoundID.NPCKilled, (int)Projectile.Center.X, (int)Projectile.Center.Y, 39);
			if (snd != null)
			{
				snd.Pitch = -0.50f;
			}

			Projectile.Kill();

			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[Projectile.owner];
			if (owner.active && !owner.dead && owner.statLife < owner.statLifeMax2 * 0.25f)
			{
				//projectile.vampireHeal(50, target.Center);
			}

			Projectile.tileCollide = false;
			Projectile.ai[0] = Main.rand.NextFloat(-1f, 1f) * 0.75f;
			Projectile.ai[1] = MathHelper.TwoPi * 500;
			Projectile.localAI[0] = Main.rand.NextFloat(-1f, 1f) * 0.10f;
			Projectile.localAI[1] = Main.rand.NextFloat(0.1f, 1f) * 0.01f;
			Projectile.netUpdate = true;

			owner.MinionAttackTargetNPC = target.whoAmI;

			SoundEffectInstance snd = SoundEngine.PlaySound(SoundID.NPCKilled, (int)Projectile.Center.X, (int)Projectile.Center.Y, 39);
			if (snd != null)
			{
				snd.Pitch = 0.25f;
			}
			Projectile.damage = 0;
		}

		public override void AI()
		{

			if (oldPos[0] == default)
			{
				for (int i = 0; i < oldPos.Length; i += 1)
				{
					oldPos[i] = Projectile.Center;
				}
			}

			Player owner = Main.player[Projectile.owner];

			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}

			oldPos[0] = Projectile.Center;


			if (Projectile.ai[0] != 0)
			{
				Projectile.velocity += Vector2.Normalize(owner.Center - Projectile.Center).RotatedBy(Projectile.ai[0] + (float)Math.Sin(Projectile.ai[1]) * Projectile.localAI[0]) * 0.35f;
				Projectile.velocity *= 0.990f;

				Projectile.velocity += Vector2.Normalize(owner.Center - Projectile.Center) * 1f * (1f - Projectile.Opacity);

				Projectile.Opacity -= 0.005f;
				Projectile.ai[1] += Projectile.localAI[1];

				if (Projectile.Opacity <= 0)
				{

					if (owner.active && !owner.dead && owner.statLife < owner.statLifeMax2 * 0.25f)
					{
						bool magic = Projectile.magic;
						Projectile.DamageType = DamageClass.Magic;
						Projectile.ghostHeal(25, Projectile.Center);
						Projectile.DamageType = magic;
					}

					Projectile.Kill();
				}
			}


			Projectile.position -= Projectile.velocity * 0.5f;
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Mod.Assets.Request<Texture2D>("Items/Weapons/Ammo/SoulboundBullet").Value;
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			/*for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldPos.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, color * alphaz, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}*/

			//TrailHelper trailEffect = new TrailHelper("BasicEffectAlphaPass", Main.extraTexture[21]);
			TrailHelper trailEffect = new TrailHelper("DefaultPass", Mod.Assets.Request<Texture2D>("noise").Value);
			Color color = Color.Lerp(Color.CornflowerBlue, Color.PaleTurquoise, Projectile.Opacity);
			trailEffect.color = delegate (float percent)
			{
				return color;
			};
			trailEffect.strength = Projectile.Opacity * MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, 1f) * 2f;
			trailEffect.trailThickness = 2f;
			trailEffect.coordMultiplier = new Vector2(1f, 2f);
			trailEffect.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -2f);
			trailEffect.trailThicknessIncrease = 2f;
			trailEffect.capsize = new Vector2(4f, 4f);

			trailEffect.DrawTrail(oldPos.ToList());

			//if (projectile.ai[0] == 0)
			//spriteBatch.Draw(tex, projectile.Center-Main.screenPosition, null, Color.AliceBlue * 1f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);


			return false;
		}

	}


	public class NoviteBullet : ModProjectile
	{
		private Vector2[] oldPos = new Vector2[6];
		private float[] oldRot = new float[6];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Round");
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 3000;
			Projectile.extraUpdates = 5;
			AIType = ProjectileID.Bullet;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = ProjectileID.CursedBullet;
			return true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/NoviteBullet"); }
		}

		public override void AI()
		{

			if (Main.rand.Next(0, 20) == 1)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.GoldCoin);
				Main.dust[dust].scale = 0.20f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(0, 100) * 0.01f);
			}

			Projectile.position -= Projectile.velocity * 0.8f;

			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = Projectile.Center;

			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

			Player player = Main.player[Projectile.owner];

			if (Projectile.ai[0] == 0 && player != null)
			{
				List<NPC> enemies = SGAUtils.ClosestEnemies(Projectile.Center, 300);

				if (enemies != null && enemies.Count > 0 && player.SGAPly().ConsumeElectricCharge(30, 30))
				{
					NPC enemy = enemies[0];
					Projectile.ai[0] = 1;
					Projectile.velocity = Vector2.Normalize(enemy.Center - Projectile.Center) * Projectile.velocity.Length();
					SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 67, 0.25f, 0.5f);
				}

			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[Projectile.type];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.White, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldPos.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, color * alphaz, Projectile.rotation, drawOrigin, Projectile.scale * alphaz, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//null
		}



	}

	public class AimBotBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aimbot Bullet");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Bullet);
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 400;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 400;
			Projectile.aiStyle = -1;
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{

			if (Projectile.ai[0] < 1)
			{
				int dir = 1;
				if (Main.myPlayer == Projectile.owner)
				{
					List<NPC> targets = SGAUtils.ClosestEnemies(Main.player[Projectile.owner].Center, 2000, Main.MouseWorld);

					if (targets != null && targets.Count > 0)
					{
						NPC target = targets[0];// Main.npc[Idglib.FindClosestTarget(0, Main.MouseWorld, new Vector2(0f, 0f), true, true, true, projectile)];
						if (target != null)
						{
							Vector2 projvel = Projectile.velocity;
							Projectile.velocity = target.Center - Projectile.Center;
							Projectile.velocity.Normalize(); Projectile.velocity *= 8f;
							IdgProjectile.Sync(Projectile.whoAmI);
							Projectile.netUpdate = true;

							Vector2 pos = Vector2.Normalize(target.position + new Vector2(Main.rand.Next(0, target.width), Main.rand.Next(0, target.height)) - Projectile.Center);

							Projectile.NewProjectile(Projectile.Center, pos * Projectile.velocity.Length() * 400, ModContent.ProjectileType<AimbotBulletEffect>(), 0, 0);

                        }
                    }
                    else
                    {
						Vector2 pos = Vector2.Normalize(Projectile.velocity);
						Projectile.NewProjectile(Projectile.Center, pos * Projectile.velocity.Length() * 400, ModContent.ProjectileType<AimbotBulletEffect>(), 0, 0);
					}
				}
				dir = Math.Sign(Projectile.velocity.X);
				Main.player[Projectile.owner].ChangeDir(dir);
				Main.player[Projectile.owner].itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir);
				//Main.player[projectile.owner].itemRotation = projectile.velocity.ToRotation() * Main.player[projectile.owner].direction;

			}
			Projectile.ai[0] += 1;

			/*
			Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= 0.1f;
			int num655 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 206, projectile.velocity.X + randomcircle.X * 8f, projectile.velocity.Y + randomcircle.Y * 8f, 100, new Color(30, 30, 30, 20), 1f);
			Main.dust[num655].noGravity = true;
			Main.dust[num655].velocity *= 0.5f;
			*/
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.damage = (int)(Projectile.damage * 1.20f);
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + ProjectileID.MoonlordBullet); }
		}
	}

	public class AimbotBulletEffect : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("AimbotBullet's Zip");
		}
		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Starfury);
			AIType = -1;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 20;
			Projectile.tileCollide = false;
			// projectile.melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_591"; }
		}

		public override bool CanDamage()
		{
			return false;
		}
		public override void AI()
		{
			if (Projectile.localAI[0]<=0)
			Projectile.localAI[0] = Main.rand.NextFloat();

			Projectile.position -= Projectile.velocity;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Color color = Main.hslToRgb(Projectile.localAI[0] % 1f, 0.6f, 0.75f);
			Color color2 = Main.hslToRgb(Projectile.localAI[0] % 1f, 1f, 0.75f);

			float timeleft = MathHelper.Clamp(Projectile.timeLeft / 20f, 0f, 1f);

			Texture2D tex = Main.extraTexture[47];
			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, color * MathHelper.Clamp((Projectile.timeLeft-15) / 3f, 0f, 1f)*0.5f, Projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width / 2f, tex.Height), new Vector2(1f, Projectile.velocity.Length() / tex.Height), SpriteEffects.None, 0);

			Texture2D glowTex = Main.extraTexture[46];
			spriteBatch.Draw(glowTex, Projectile.Center+(Projectile.velocity*(1f-timeleft)) - Main.screenPosition, null, color2 * MathHelper.Clamp(Projectile.timeLeft / 12f, 0f, 1f), Projectile.velocity.ToRotation() + MathHelper.PiOver2, glowTex.Size()/2f, new Vector2(0.2f, 0.75f), SpriteEffects.None, 0);


			return false;
		}
	}


	public class TungstenBullet : ModProjectile
	{
		private Vector2[] oldPos = new Vector2[6];
		private float[] oldRot = new float[6];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tungsten Bullet");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Bullet);
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			AIType = ProjectileID.Bullet;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + 14); }
		}

	}

	public class PortalBullet : ProjectilePortal
	{
		public override int takeeffectdelay => 0;
		public override float damagescale => 1f;
		public override int penetrate => 1;
		public override int openclosetime => 8;


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
			Projectile.friendly = true;
			Projectile.timeLeft = 24;
			Projectile.tileCollide = false;
			AIType = -1;
		}

		public override void Explode()
		{

			if (Projectile.timeLeft == openclosetime && Projectile.ai[0] > 0)
			{
				Player owner = Main.player[Projectile.owner];
				if (owner != null && !owner.dead)
				{

					NPC target = Main.npc[Idglib.FindClosestTarget(0, Projectile.Center, new Vector2(0f, 0f), true, true, true, Projectile)];
					if (target != null && Vector2.Distance(target.Center, Projectile.Center) < 1500)
					{

						SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 67, 0.25f, 0.5f);

						Vector2 gotohere = new Vector2();
						gotohere = target.Center - Projectile.Center;//Main.MouseScreen - projectile.Center;
						gotohere.Normalize();

						Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(10)) * Projectile.velocity.Length();
						int proj = Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ProjectileID.BulletHighVelocity, Projectile.damage, Projectile.knockBack / 8f, owner.whoAmI);
						Main.projectile[proj].timeLeft = 180;
						//Main.projectile[proj].penetrate = 1;
						Main.projectile[proj].GetGlobalProjectile<SGAprojectile>().onehit = true; ;
						Main.projectile[proj].netUpdate = true;
						IdgProjectile.Sync(proj);

					}
				}

			}

		}

		public override void AI()
		{
			if (Projectile.ai[1] < 100)
			{
				Projectile.ai[1] = 100;
				Projectile.ai[0] = ProjectileID.BulletHighVelocity;
				Player owner = Main.player[Projectile.owner];
				if (owner != null && Main.myPlayer == owner.whoAmI)
				{
					Projectile.Center = Main.MouseWorld;
					Projectile.direction = Main.MouseWorld.X > owner.position.X ? 1 : -1;
				}
			}
			base.AI();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			if (scale > 0)
			{
				Texture2D texture = SGAmod.ExtraTextures[99];
				Texture2D outer = SGAmod.ExtraTextures[101];
				spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, lightColor, 0.75f) * 0.5f, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;
				spriteBatch.Draw(outer, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, lightColor, 0.75f) * 0.5f, -Projectile.rotation, new Vector2(outer.Width / 2, outer.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;


				outer = SGAmod.ExtraTextures[102];
				spriteBatch.Draw(outer, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, lightColor, 0.75f) * 0.5f, Projectile.rotation * 2, new Vector2(outer.Width / 2, outer.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;
				spriteBatch.Draw(outer, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, lightColor, 0.75f) * 0.5f, -Projectile.rotation * 2, new Vector2(outer.Width / 2, outer.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;


			}
			return false;
		}

	}
}