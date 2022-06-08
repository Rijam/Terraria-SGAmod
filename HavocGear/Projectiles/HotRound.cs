using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace SGAmod.HavocGear.Projectiles
{
	//Give explosion on expire
	public class HotRound : ModProjectile
	{

		public int stickin = -1;
		public Player P;
		public Vector2 offset;
		public float boomGlow = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orb thingy from Asterism");
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = 200;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 4 * 60;
			Projectile.scale = 0.75f;
			AIType = 0;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(stickin);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			stickin = reader.ReadInt32();
		}

		public override bool PreKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			for (int num315 = 0; num315 < 20; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type, Projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), Projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 10f), 50, Main.hslToRgb(0.4f, 0f, 0.95f), 1f);
				Main.dust[num316].noGravity = true;
				Vector2 velop = new Vector2(Projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), Projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 10f));
				Main.dust[num316].velocity = velop / 8f;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.75f;
			}

			if (timeLeft < 2 && stickin>=0)
			{
				var snd = SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
				if (snd != null)
				{
					snd.Pitch = 0.75f;
					if (SGAmod.ScreenShake < 20)
					SGAmod.AddScreenShake(16, 1280, Projectile.Center);
				}

				Projectile.NewProjectile(Projectile.Center, Vector2.Normalize(Projectile.velocity) * 2f, ModContent.ProjectileType<HeatedBlowBackShot>(), Projectile.damage * 3, Projectile.knockBack * 3f, Projectile.owner);

				for (float gg = 1f; gg < 7.26f; gg += 0.25f)
				{
					Vector2 velo = Main.rand.NextVector2CircularEdge(gg, gg) * 2f;
					Gore.NewGore(Projectile.Center + new Vector2(0, 0), velo, Main.rand.Next(61, 64), (5f - Math.Abs(gg)) / 3f);

					velo = Main.rand.NextVector2CircularEdge(gg, gg);
					int gorer = Gore.NewGore(Projectile.Center + new Vector2(0, 0), Vector2.Zero, Main.rand.Next(61, 64), (5f - Math.Abs(gg)) / 3f);
					if (gorer >= 0)
						Main.gore[gorer].velocity = (Vector2.Normalize(Projectile.velocity) * ((gg * 2f) + 3f)) + (velo / 1f);

				}
			}

			if (stickin > -1)
			{
				NPC himz = Main.npc[stickin];
				if (himz != null && himz.active)
				{
					himz.AddBuff(Mod.Find<ModBuff>("ThermalBlaze").Type, 60 * 3);
				}
			}
			return true;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.penetrate < 100)
				return false;
			return base.CanHitNPC(target);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			bool foundsticker = false;
			target.immune[Main.player[Projectile.owner].whoAmI] = 1;
			int numfound = 0;

			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (i != Projectile.whoAmI // Make sure the looped projectile is not the current javelin
					&& currentProjectile.active // Make sure the projectile is active
					&& currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
					&& currentProjectile.type == Projectile.type // Make sure the projectile is of the same type as this javelin
					&& currentprojectile.ModProjectile is HotRound HoterRound // Use a pattern match cast so we can access the projectile like an ExampleJavelinProjectile
					&& HoterRound.stickin == target.whoAmI)
				{
					numfound += 1;
					if (numfound > 2)
					{
						foundsticker = true;
						Projectile.Kill();
						break;
					}
				}

			}

			if (!foundsticker)
			{

				if (Projectile.penetrate > 1)
				{
					Projectile.penetrate = 50;
					offset = (target.Center - Projectile.Center);
					stickin = target.whoAmI;
					Projectile.netUpdate = true;
				}
			}
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() * 0.75f);

			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

			if (stickin > -1)
			{
				boomGlow = (2.75f - boomGlow)/(1f+(Projectile.timeLeft/30f));
				boomGlow = Math.Min(boomGlow, 1f);

				NPC himz = Main.npc[stickin];
				Projectile.tileCollide = false;

				if (himz != null && himz.active)
				{
					Projectile.Center = (himz.Center - offset) - (Projectile.velocity * 0.2f);
					if (GetType() == typeof(HotRound))
					himz.AddBuff(BuffID.OnFire, 3);
				}
				else
				{
					Projectile.Kill();
				}

				for (int num315 = 0; num315 < 3; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) + Vector2.Normalize(Projectile.velocity) * Main.rand.NextFloat(0f, 32f), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type, 0,0, 50, Main.hslToRgb(0.4f, 0f, 0.95f), boomGlow/2f);
					Main.dust[num316].noGravity = true;
					Vector2 velop = new Vector2(Projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), Projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 10f));
					Main.dust[num316].velocity = velop / 8f;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.75f;
				}

			}
            else
            {
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type, 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.95f), 0.9f);
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.3f;
				}
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[Projectile.type];

			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, tex.Size() / 2f, Projectile.scale, default, 0);

			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (stickin >= 0)
			{
				Texture2D tex = Main.projectileTexture[Projectile.type];
				Texture2D tex2 = ModContent.Request<Texture2D>("SGAmod/Glow");


				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.OrangeRed*boomGlow, Projectile.rotation, tex2.Size() / 2f, Projectile.scale, default, 0);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				Effect fadeIn = SGAmod.FadeInEffect;

				fadeIn.Parameters["alpha"].SetValue(boomGlow);
				fadeIn.Parameters["strength"].SetValue(boomGlow);
				fadeIn.Parameters["fadeColor"].SetValue(Color.Orange.ToVector3());
				fadeIn.Parameters["blendColor"].SetValue(Color.White.ToVector3());

				fadeIn.CurrentTechnique.Passes["FadeIn"].Apply();

				spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null,Color.White, Projectile.rotation, tex.Size()/2f, Projectile.scale, default, 0);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
		}

	}

	public class HeatedBlowBackShot : Dimensions.NPCs.SpaceBossBasicShot, IDrawAdditive
	{
        protected override Color BoomColor => Color.Lerp(Color.Red, Color.Orange, Projectile.timeLeft/120f);
		public override bool DrawFlash => true;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blow Back Shot");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 120;
			Projectile.light = 0.75f;
			Projectile.penetrate = -1;
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 5;
		}

		public override void AI()
		{
			Projectile.localAI[0] += 1;

			if (Projectile.localAI[0] == 1)
			{
				startOrg = Projectile.Center;
			}

			if (Projectile.ai[0] < 15f)
			{
				Projectile.ai[0] += 0.05f;
			}
		}

        public override bool PreKill(int timeLeft)
        {
			return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(Mod.Find<ModBuff>("ThermalBlaze").Type, 60 * 3);
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			return false;
		}

	}

}