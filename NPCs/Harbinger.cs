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

namespace SGAmod.NPCs
{
	public class Harbinger : ModNPC, ISGABoss
	{
		public string Trophy() => "DoomHarbingerTrophy";
		public bool Chance() => Main.rand.Next(0, 10) == 0;
		public string RelicName() => "Doom_Harbinger";
		public void NoHitDrops() { }
		public string MasterPet() => null;
		public bool PetChance() => false;

		int oldtype=0;
		int [] orbitors=new int[20];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Doom Harbinger");
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
		}
        public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 0;
			NPC.defense = 50;
			NPC.lifeMax = 25000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.2f;
			NPC.aiStyle = -1;
			NPC.boss=true;
			music=MusicID.Boss5;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.value = Item.buyPrice(0, 50, 0, 0);
		}

        public override void BossLoot(ref string name, ref int potionType)
        {
        potionType=ItemID.GreaterHealingPotion;
        }	

		public override string Texture
		{
			get { return("SGAmod/NPCs/TPD");}
		}	

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.6f);
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return true;
		}

		public override void NPCLoot()
		{

			if (Math.Abs(NPC.ai[2]) > 200)
			{
				if (NPC.ai[0]>6)
				{
					NPC.type = oldtype;
					NPC myguy = Main.npc[(int)NPC.ai[1]];
					myguy.active = false;
					Achivements.SGAAchivements.UnlockAchivement("Harbinger", Main.LocalPlayer);
					if (SGAWorld.downedHarbinger == false)
					{
						Idglib.Chat("Your end is nigh...", 15, 15, 150);
						//Idglib.Chat("Robbed figures have been seen near the dungeon.", 20, 20, 125);
						SGAWorld.downedHarbinger = true;
					}
				}
				else
				{
				NPC.boss = false;

				}

			}
			else
			{
				NPC.boss = false;
				Idglib.Chat("You are not ready for this...", 15, 15, 150);
				for (int i = 180; i <= 361; i+=180)
				{
					int harbinger2 = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, NPC.type);
					Main.npc[harbinger2].ai[2] = MathHelper.ToRadians(NPC.ai[2]+i);
					Main.npc[harbinger2].netUpdate = true;
				}

			}
        }

		public override void AI()
		{
			if (oldtype < 1)
			{
				oldtype = NPC.type;
			}
			NPC.netUpdate = true;
			Player P = Main.player[NPC.target];
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || Main.dayTime)
			{
				NPC.TargetClosest(false);
				P = Main.player[NPC.target];
				if (!P.active || P.dead || Main.dayTime)
				{
					NPC.active = false;
					Main.npc[(int)NPC.ai[1]].active = false;
				}
			}
			else
			{

                double angleval = NPC.ai[2];
				Vector2 itt = P.Center + new Vector2((float)Math.Cos(angleval), (float)Math.Sin(angleval)) * 300f;

				if (Math.Abs(NPC.ai[2]) > 0)
				{
					NPC.ai[2] += 1f;
					NPC.GivenName = "Doom Harbingers";
					float speedz = Main.npc[(int)NPC.ai[1]].velocity.Length();
					Vector2 poz = Main.npc[(int)NPC.ai[1]].position;
					if (NPC.CountNPCS(NPC.type) > 1)
						Main.npc[(int)NPC.ai[1]].position += (((itt - poz) / 10f) * (speedz / 30f));


				}




				NPC.ai[0] += 1;
				if (NPC.ai[0] == 2)
				{
					//npc.ai[2]=NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.MoonLordHead);
					//Main.npc[(int)npc.ai[2]].aiStyle=-1;
					//Main.npc[(int)npc.ai[2]].timeLeft=999999;
					//Main.npc[(int)npc.ai[2]].dontTakeDamage=true;
					//Main.npc[(int)npc.ai[2]].boss=false;
					NPC.type = NPCID.MoonLordCore;
					NPC.ai[1] = NPC.NewNPC((int)P.Center.X, (int)P.Center.Y - 200, NPCID.MoonLordFreeEye);
					Main.npc[(int)NPC.ai[1]].ai[3] = NPC.whoAmI;
					Main.npc[(int)NPC.ai[1]].lifeMax = NPC.lifeMax;
					Main.npc[(int)NPC.ai[1]].life = NPC.lifeMax;
					Main.npc[(int)NPC.ai[1]].defense = NPC.defense;
                    Main.npc[(int)NPC.ai[1]].defDefense = NPC.defDefense;
					Main.npc[(int)NPC.ai[1]].damage = 0;
					Main.npc[(int)NPC.ai[1]].defDamage = 0;
					Main.npc[(int)NPC.ai[1]].netUpdate = true;
				}
				//Main.npc[(int)npc.ai[1]].active=true;
				//Main.npc[(int)npc.ai[1]].aiStyle=1;
				NPC myguy = Main.npc[(int)NPC.ai[1]];
				if (myguy.active == false)
				{
					NPC.type = oldtype;
					NPC.StrikeNPCNoInteraction(9999, 0f, 0, false, false, false);
				}
				else
				{
					if (NPC.ai[0] > 1)
					{
						if (NPC.life > (int)(NPC.lifeMax / 1.5) || NPC.CountNPCS(NPC.type) > 1)
							NPC.ai[0] = Math.Min(NPC.ai[0] + 1, 5);
						if (NPC.ai[0] > 60)
						{
							if (NPC.ai[0] == 65)
							{
								Idglib.Chat(NPC.ai[2] > 0 ? "VERY Impressive, you just might stand a chance..." : "Most impressive child...", 15, 15, 150);
								if (NPC.ai[2] > 0)
								{
									myguy.life = (int)(NPC.lifeMax / 1.55);
								}
							}

							if (NPC.ai[0] % 600 == 62)
							{
								for (int i = 0; i < 5; i++)
								{
									int newb = Projectile.NewProjectile(NPC.Center, NPC.Center, ProjectileID.PhantasmalSphere, 60, 5, Main.myPlayer, 0f, (float)NPC.whoAmI);
									orbitors[i] = newb;
									Main.projectile[orbitors[i]].timeLeft = 1000;
								}
							}


							if (NPC.ai[0] > 65)
							{
								for (int i = 0; i < 5; i++)
								{
									if (orbitors[i] > 0 && Main.projectile[orbitors[i]].active)
									{
										if (orbitors[i] > 0 && Main.projectile[orbitors[i]] != null && Main.projectile[orbitors[i]].timeLeft > 120)
										{
											double angle = (NPC.ai[0] / 5) + 2.0 * Math.PI * (i / ((double)5f));
											float timeleft = Main.projectile[orbitors[i]].timeLeft;
											Main.projectile[orbitors[i]].Center = NPC.Center + new Vector2((float)(Math.Cos(angle) * (150f - (150f / (timeleft - 1450f)))), (float)(Math.Sin(angle) * (150f - (150f / (timeleft - 1450f)))));
											Main.projectile[orbitors[i]].velocity = new Vector2((float)(Math.Cos(angle) * 8f), (float)(Math.Sin(angle) * 8f));
										}
									}
								}
							}


							if (NPC.ai[0] % 600 == 62 && NPC.ai[2] > 5)
							{
								for (int i = 6; i <= 10; i++)
								{
									int newb = Projectile.NewProjectile(NPC.Center, NPC.Center, ProjectileID.PhantasmalSphere, 60, 5, Main.myPlayer, 0f, (float)NPC.whoAmI);
									orbitors[i] = newb;
									Main.projectile[orbitors[i]].timeLeft = 1000;
								}
							}

							if (NPC.ai[0] > 65 && NPC.ai[2] > 5)
							{
								for (int i = 6; i <= 10; i++)
								{
									if (orbitors[i] > 0 && Main.projectile[orbitors[i]].active)
									{
										if (orbitors[i] > 0 && Main.projectile[orbitors[i]] != null && Main.projectile[orbitors[i]].timeLeft > 120)
										{
											double angle = (-NPC.ai[0] / 45f) + 2.0 * Math.PI * (i / ((double)5f));
											float timeleft = Main.projectile[orbitors[i]].timeLeft;
											Main.projectile[orbitors[i]].Center = NPC.Center + new Vector2((float)(Math.Cos(angle) * (550f - (550f / (timeleft - 1450f)))), (float)(Math.Sin(angle) * (550f - (550f / (timeleft - 1450f)))));
											Main.projectile[orbitors[i]].velocity = new Vector2((float)(Math.Cos(angle) * 24f), (float)(Math.Sin(angle) * 24f));
										}
									}
								}
							}


							for (int k = 0; k < Main.maxPlayers; k++)
							{
								Player player = Main.player[k];
								if (player != null && player.active)
								{
									player.AddBuff(BuffID.MoonLeech, 30, true);
								}
							}
						}

						if (NPC.ai[0] > 1)
						{
							NPC.position = myguy.Center;
							NPC.life = System.Math.Max(myguy.life, 200);
							myguy.dontTakeDamage = false;
							myguy.aiStyle = 81;
							//myguy.Name="Doom Harbinger";
						}
					}
					NPC.timeLeft = 30;
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

