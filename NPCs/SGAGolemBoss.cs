using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using Terraria.Audio;

namespace SGAmod.NPCs
{
	public class SGAGolemBoss : ModNPC
	{
		int oldtype=0;
		int phase=0;
		int minlife=9999;
		int anticheese=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Simply better Golem");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 3000000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.2f;
			NPC.aiStyle = -1;
			NPC.boss=false;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.value = 1440000f;
			NPC.dontTakeDamage=true;
			NPC.immortal=true;
		}
				public override string Texture
		{
			get { return("SGAmod/NPCs/TPD");}
		}


		public override void NPCLoot()
		{


        }

		public override void AI()
		{
			NPC.timeLeft = 30;
			if (oldtype < 1)
			{
				oldtype = NPC.type;
			}
			NPC.netUpdate = true;
			Player P = Main.player[NPC.target];
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || NPC.CountNPCS(NPCID.Golem) < 1)
			{
				NPC.TargetClosest(false);
				P = Main.player[NPC.target];
				if (NPC.CountNPCS(NPCID.Golem) < 1)
				{
					NPC.active = false;
				}
			}
			else
			{

				NPC.ai[0] += 1;
				int npctype = NPCID.Golem;
				if (NPC.CountNPCS(npctype) > 0)
				{
					NPC myowner = Main.npc[NPC.FindFirstNPC(NPCID.Golem)];
					NPC.position = myowner.position;
					if (anticheese % 500 < 400)
					{
						if (!Collision.CanHitLine(new Vector2(myowner.Center.X, myowner.Center.Y), 1, 1, new Vector2(P.Center.X, P.Center.Y), 1, 1))
							anticheese += 1;
					}
					else
					{

					}
					if (NPC.CountNPCS(NPCID.GolemHeadFree) > 0)
					{
						NPC myhead = Main.npc[NPC.FindFirstNPC(NPCID.GolemHeadFree)];
						if (phase == 0)
						{
							phase = 1;
							myhead.lifeMax = myowner.lifeMax;
							myhead.life = myowner.lifeMax;
							myhead.dontTakeDamage = false;
						}
					}
					if (NPC.CountNPCS(NPCID.GolemHeadFree) < 1 && phase == 1)
						phase = 2;
					if (phase == 0)
						minlife = myowner.life;
					if (phase == 1)
					{
						myowner.life = minlife;
						if (Main.expertMode)
							minlife += 1;
					}

					Vector2 angledif = P.Center - myowner.Center; angledif.Normalize();
					float angle = NPC.velocity.ToRotation().AngleLerp(angledif.ToRotation(), (0.50f - ((float)myowner.life / (float)myowner.lifeMax) * 0.25f) * 0.25f);
					if (NPC.ai[0] % 900 > 600 && NPC.ai[0] % 900 < 800)
					{
						for (int a = 0; a < 2000; a += Main.rand.Next(36, 64))
						{
							Vector2 there = NPC.velocity * a;
							int num622 = Dust.NewDust(new Vector2(myowner.Center.X, myowner.Center.Y) + there, 0, 0, 6, 0f, 0f, 100, default(Color), 1f);

						}
					}

					if (NPC.ai[0] % 900 > 800)
					{
						if (NPC.ai[0] % 20 == 0)
							Idglib.Shattershots(myowner.Center, myowner.Center + NPC.velocity, Vector2.Zero, ProjectileID.CultistBossFireBall, 40, 12, 90, 1, true, 0, false, 220);
						if (NPC.ai[0] % 3 == 0)
						{
							for (int i = 0; i < 1000; i += 400)
							{
								List<Projectile> itz = Idglib.Shattershots(myowner.Center + (NPC.velocity * i), myowner.Center + (NPC.velocity * i) + NPC.velocity, Vector2.Zero, ProjectileID.FlamesTrap, 40, 12, 90, 1, true, 0, false, 220);
								itz[0].friendly = false;
								itz[0].hostile = true;
							}

						}
					}
					else
					{
						NPC.velocity = angle.ToRotationVector2();
					}

					if (phase == 1 && NPC.ai[0] % 300 == 0)
					{
						List<Projectile> itz = Idglib.Shattershots(myowner.Center, P.position, new Vector2(P.width, P.height), ProjectileID.CultistBossFireBall, 40, 8, 140, 2, true, 0, false, 220);
						//itz[0].aiStyle=5;
					}

					if (phase > 0 && NPC.ai[0] % 900 > 700)
					{
						bool cond = NPC.ai[0] % 900 == 899;
						for (int a = 0; a < 20 + (cond ? 60 : 0); a++)
						{
							Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
							Vector2 vecr = randomcircle * 240;
							vecr *= (1f - (900f / (NPC.ai[0] % 900)));

							randomcircle *= (cond ? Main.rand.NextFloat(3f, 10f) : 1f);
							int num622 = Dust.NewDust(new Vector2(myowner.Center.X, myowner.Center.Y) + vecr, 0, 0, 6, randomcircle.X, randomcircle.Y, 100, default(Color), 0.5f + (((NPC.ai[0] - 400f) % 900) / 300f));
							Main.dust[num622].velocity = randomcircle;
							Main.dust[num622].noGravity = true;
						}
						if (cond)
							SoundEngine.PlaySound(SoundID.Item, (int)myowner.Center.X, (int)myowner.Center.Y, 74, 1f, 0f);
					}


					if (phase > 0 && NPC.ai[0] % 900 == 0)
					{
						for (int i = 0; i < 3; i++)
						{
							List<Projectile> itz = Idglib.Shattershots(myowner.Center, myowner.Center, new Vector2(0, 0), Mod.Find<ModProjectile>("Ringproj").Type, 60, 14, 0, 1, true, Main.rand.Next(-360, 360), false, 420);
							itz[0].timeLeft = 200;
							itz[0].tileCollide = true;
							//itz[0].aiStyle=5;
						}
					}
					if ((phase > 0 && (NPC.ai[0] * (phase == 2 ? 1.5f : 1f)) % 600 > (phase == 2 ? 450 : 500) && myowner.velocity.Y < 0) || anticheese % 500 > 399)
					{
						if (myowner.velocity.Y < 0)
						{
							myowner.noTileCollide = true;
							myowner.velocity = myowner.velocity + (new Vector2(P.Center.X > myowner.Center.X ? 1 : -1, -0.25f));
							if (anticheese % 500 > 399)
								anticheese += 1;
							//itz[0].aiStyle=5;
						}
					}


					if (phase == 2)
					{
						if (System.Math.Abs(myowner.velocity.Y) < 1)
						{
							if ((NPC.ai[0]) % 600 > 400 && (NPC.ai[0]) % 10 == 0)
							{
								List<Projectile> itz = Idglib.Shattershots(myowner.Center, P.position + new Vector2((float)Main.rand.Next(-150, 150), -700f), new Vector2(P.width, P.height), ProjectileID.DD2BetsyFireball, 70, 30, 0, 1, true, 0, false, 220);
								itz[0].timeLeft = 600;
								itz[0].tileCollide = false;
								SGAprojectile modeproj = itz[0].GetGlobalProjectile<SGAprojectile>();
								modeproj.raindown = true;
								modeproj.splithere = P.Center + new Vector2(0, 200);
							}
						}
					}
					if (myowner.velocity.Y > 0 && (myowner.Center.Y > P.Center.Y - 100))
						myowner.noTileCollide = false;
					else
						myowner.noTileCollide = true;

					if (myowner.velocity.Y < -1)
						myowner.noTileCollide = true;

				}



			}

		}
		




		public override bool CanHitPlayer(Player ply, ref int cooldownSlot){
			return false;
		}
		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return CanBeHitByPlayer(player);
		}
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return CanBeHitByPlayer(Main.player[projectile.owner]);
		}
		private bool? CanBeHitByPlayer(Player P){
		return false;
		}


public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
{
return false;
}


	}
}

