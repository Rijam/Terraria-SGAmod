using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using Terraria.Utilities;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace SGAmod.NPCs.Sharkvern
{
	public class AquaSurge : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aqua Surge");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 22;
			NPC.height = 28;
			NPC.damage = 26;
			NPC.defense = 18;
			NPC.lifeMax = 300;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			AnimationType = ItemID.SoulofNight;
			NPC.value = 60f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 44;
			AIType = NPCID.Wraith;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = NPC.direction;
		}

		public override void AI()
		{
			NPC.ai[0]++;
			Player P = Main.player[NPC.target];
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(true);
			}
			NPC.netUpdate = true;

			NPC.ai[1]++;
			if (NPC.ai[1] % 100 == 0 && Main.netMode != 1)
			{
				float Speed = 15f;
				Vector2 vector8 = new Vector2(NPC.position.X + (NPC.width / 2), NPC.position.Y + (NPC.height / 2));
				int damage = 35;
				int type = Mod.Find<ModProjectile>("WaterTornado").Type;
				SoundEngine.PlaySound(23, (int)NPC.position.X, (int)NPC.position.Y, 17);
				float rotation = (float)Math.Atan2(vector8.Y - (P.position.Y + (P.height * 0.5f)), vector8.X - (P.position.X + (P.width * 0.5f)));
				int num54 = Projectile.NewProjectile(vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, 0);
				Main.projectile[num54].damage = (int)(NPC.damage / 2);
				NPC.netUpdate = true;
			}

			/*if (npc.ai[1] % 500 == 0 && npc.ai[1] > 0 && Main.expertMode)
			{
				List<Projectile> itz = Idglib.Shattershots(npc.Center, npc.Center + new Vector2(0, 200), new Vector2(0, 0), ProjectileID.WaterBolt, (int)(npc.damage), 4f, 180, 5, false, 0, true, 300);
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 21);
			}*/

			return;

			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (!player.dead)
				{
					return;
				}
				else
				{
					NPC.life = 0;
					NPC.active = false;
					return;
				}
			}

		}

	}


	public class SharkvernCloudMiniboss : ModNPC
	{
		int[] lightningDelay = new int[24];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Storm Keeper");
			Main.npcFrameCount[NPC.type] = 4;
		}
		public override string Texture => "SGAmod/NPCs/Sharkvern/AquaSurge";

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.650f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.6f);
		}
        public override bool CheckActive()
        {
            return NPC.CountNPCS(ModContent.NPCType<SharkvernHead>())<1;
        }

        public override void SetDefaults()
		{
			NPC.width = 160;
			NPC.height = 48;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 4000;
			NPC.noGravity = true;
			//npc.HitSound = SoundID.NPCHit1;
			//npc.DeathSound = SoundID.NPCDeath1;
			AnimationType = ItemID.SoulofNight;
			NPC.value = 1500f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			NPC.dontTakeDamage = true;
		}

        public override void HitEffect(int hitDirection, double damage)
        {
			if (NPC.life > 0)
			{
				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.NPCHit, (int)NPC.Center.X, (int)NPC.Center.Y,30);
				if (sound != null)
				{
					sound.Pitch = -0.75f;
				}
			}
			else
			{
				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.NPCKilled, (int)NPC.Center.X, (int)NPC.Center.Y, 33);
				if (sound != null)
				{
					sound.Pitch = -0.75f;
				}
				for (int i = 16; i < 110; i += 2)
				{
					float devider = Main.rand.NextFloat(0, 1f);
					float angle = MathHelper.TwoPi * devider;
					Vector2 thecenter = new Vector2((float)((Math.Cos(angle) * i)), (float)((Math.Sin(angle) * i)));
					thecenter = thecenter.RotatedByRandom(MathHelper.TwoPi);
					thecenter.Y *= 0.4f;
					int DustID2 = Dust.NewDust(NPC.Center + (thecenter * 2.5f), 0, 0, SGAmod.Instance.Find<ModDust>("TornadoDust").Type, thecenter.X * 0.2f, thecenter.X * 0.2f, 255 - (int)(NPC.Opacity * 255f), default(Color), 1.5f);
					Main.dust[DustID2].noGravity = true;
					Main.dust[DustID2].velocity = new Vector2(thecenter.X * 0.2f, thecenter.Y * 0.2f) * -1f;
				}

			}
		}

        public override void AI()
		{
			NPC.localAI[0]++;
			Player P = Main.player[NPC.target];
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(true);
			}
			int shark = NPC.FindFirstNPC(ModContent.NPCType<SharkvernHead>());
			if (introEffect >= 1f)
			{
				int timer = 600;
				NPC.dontTakeDamage = false;
				NPC.ai[0]++;
				if (P.active && !P.dead && shark >= 0 && Main.npc[shark].active)
				{
					if (NPC.ai[0] % timer < 300)
					{
						if (NPC.Distance(Main.npc[shark].Center) > 1280)
						{
							NPC.velocity += Vector2.Normalize(Main.npc[shark].Center - NPC.Center) * 0.15f;
						}
                    }
                    else
                    {
						if (NPC.ai[0] % timer < 420)
						{
							NPC.velocity += Vector2.Normalize(P.MountedCenter + new Vector2(0, -200) - NPC.Center) * 2.75f;
						}
						else
						{
							if (NPC.ai[0] % timer < 470)
							{
								for (int i = 0; i < lightningDelay.Length; i += 1)
								{
									if (Main.rand.Next(0, 35) == 0)
										lightningDelay[i] = 30;
								}
							}
						}

						if (NPC.ai[0] % timer == 470)
                        {
							int proj = Projectile.NewProjectile((int)NPC.Center.X, (int)NPC.Center.Y, 0,16, ProjectileID.CultistBossLightningOrbArc, 75, 15f);
							Main.projectile[proj].ai[0] = MathHelper.PiOver2;
							Main.projectile[proj].netUpdate = true;

							for (int i = 0; i < lightningDelay.Length; i += 1)
							{
								lightningDelay[i] = 60;
							}

							SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 122);
							if (sound != null)
							{
								sound.Pitch = 0.95f;
							}

						}
						NPC.velocity *= 0.92f;

					}
					NPC.velocity *= 0.98f;
				}
				else
				{
					NPC.velocity -= Vector2.UnitY * 0.10f;
				}
			}
			for (int i = 0; i < lightningDelay.Length; i += 1)
			{
				lightningDelay[i] -= 1;
				if (Main.rand.Next(0, 200) < 3 && lightningDelay[i] < -60 && introEffect >= 1f)
				{
					lightningDelay[i] = Main.rand.Next(5,30);
				}
			}
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.frame.Y = (((int)(NPC.localAI[0] / 6f) % 4) * frameHeight);
		}
		float introEffect => MathHelper.Clamp(NPC.localAI[0] / 160f, 0f, 1f);

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (!SGAConfigClient.Instance.SpecialBlending)
			Draw(spriteBatch,lightColor);
			return false;
		}

		public void Draw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.npcTexture[NPC.type];
			spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, NPC.frame, Color.Blue * 0.25f* introEffect, NPC.localAI[0]/10f, new Vector2(tex.Width, tex.Height/4f) / 2f, 2f, SpriteEffects.None, 0f);
			spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, NPC.frame, Color.Blue * 0.50f* introEffect, MathHelper.Pi-NPC.localAI[0]/10f, new Vector2(tex.Width, tex.Height / 4f) / 2f, 1f, SpriteEffects.None, 0f);
			DrawCloudWisp(NPC, NPC.Center, NPC.localAI[0], spriteBatch, lightColor);
		}

		public void DrawCloudWisp(Entity id, Vector2 drawWhere, float timer, SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.cloudTexture[0];
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

			UnifiedRandom rando = new UnifiedRandom(id.whoAmI);

			List<Vector4> drawThere = new List<Vector4>();

			for (int i = 0; i < lightningDelay.Length; i += 1)
			{
				Vector3 offset = new Vector3((i*2)+8, 0, 0);

				float mulEffect = timer / 2f;

				Matrix matrix = Matrix.CreateRotationZ((rando.NextFloat(-0.25f, 0.25f) * mulEffect) + rando.NextFloat(MathHelper.TwoPi)) *
					Matrix.CreateRotationY((rando.NextFloat(-0.25f, 0.25f) * mulEffect) + rando.NextFloat(MathHelper.TwoPi)) *
					Matrix.CreateRotationX((rando.NextFloat(-0.25f, 0.25f) * mulEffect) + rando.NextFloat(MathHelper.TwoPi));

				offset = Vector3.Transform(offset, matrix);

				drawThere.Add(new Vector4(offset.X, offset.Y, offset.Z,i));

			}

			//List<Vector2> drawThereCopy = new List<Vector2>(drawThere);

			foreach (Vector4 positionittlerv4 in drawThere.OrderBy(testnpc => 100000 - testnpc.LengthSquared()))
			{
				Vector3 positionittler = new Vector3(positionittlerv4.X, positionittlerv4.Y, positionittlerv4.Z);
				Vector3 position = positionittler*(6f - introEffect * 5f);
				Vector3 posa = Vector3.Normalize(position);
				if (posa.Z > 0)
				{
					float scaler = 0.75f + (posa.Z * 0.25f);
					Color color = Color.Lerp(Color.Gray,Color.Yellow,MathHelper.Clamp(lightningDelay[(int)positionittlerv4.W]/30f,0f,1f));
					spriteBatch.Draw(texture, drawWhere + new Vector2(position.X, position.Y) - Main.screenPosition, null, color * posa.Z * introEffect, 0f, origin, new Vector2(scaler*0.75f, scaler), SpriteEffects.None, 0f);
				}
			}
		}
	}
}