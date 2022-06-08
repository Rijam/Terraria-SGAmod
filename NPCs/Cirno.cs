using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Idglibrary;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Events;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Effects;
using System.Linq;
using Terraria.Audio;


namespace SGAmod.NPCs
{
	[AutoloadBossHead]
	public class Cirno : ModNPC, ISGABoss
	{
		public string Trophy() => "CirnoTrophy";
		public bool Chance() => Main.rand.Next(0, 10) == 0;
		public string RelicName() => "Cirno";
		public void NoHitDrops() { }
		public string MasterPet() => "FrozenBow";
		public bool PetChance() => Main.rand.Next(4) == 0;

		int aicounter = 0;
		int frameid = 0;
		int framevar = 0;
		int stateaction = 0;
		int bobbing = 0;
		int aistate = 0;
		int card = 0;
		int attacktype = 0;
		int nightmareprog = 0;
		float damagetospellcard = 0.9f;
		bool nightmaremode => SGAWorld.NightmareHardcore > 0 || SGAmod.DRMMode;
		int mixup = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cirno");
			Main.npcFrameCount[NPC.type] = 20;
		}
		public override void SetDefaults()
		{
			NPC.width = 50;
			NPC.height = 60;
			NPC.damage = 90;
			NPC.defense = 10;
			NPC.lifeMax = 15000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(0, 20, 0, 0);
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			NPC.boss = true;
			AIType = NPCID.Wraith;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/SGAmod_Cirno_v2");
			bossBag = Mod.Find<ModItem>("CirnoBag").Type;
			NPC.coldDamage = true;
			NPC.value = Item.buyPrice(0, 15, 0, 0);
		}

		public int owner
		{
			get
			{
				return (int)NPC.ai[0];
			}
			set
			{
				NPC.ai[0] = value;
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.750f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.5f);
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
		}
		public override void NPCLoot()
		{
			if (NPC.boss)
			{


				List<Projectile> itz = Idglib.Shattershots(NPC.Center - new Vector2(NPC.spriteDirection * 20, 100), NPC.Center - new Vector2(NPC.spriteDirection * 20, 100), new Vector2(0, 0), Mod.Find<ModProjectile>("SnowCloud").Type, 40, 4f, 0, 1, true, 0, false, 1000);

				if (Main.expertMode)
				{
					NPC.DropBossBags();
				}
				else
				{
					string[] dropitems = { "Starburster", "Snowfall", "IceScepter", "RubiedBlade", "IcicleFall", "RodOfTheMistyLake", "Magishield" };
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>(dropitems[Main.rand.Next(0, dropitems.Length)]).Type);
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("CryostalBar").Type, Main.rand.Next(15, 25));
					if (Main.rand.Next(3)==0)
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("GlacialStone").Type);
					if (Main.rand.Next(7) == 0)
                    {
						Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Armors.Vanity.CirnoMask>());
					}
				}
			}
			Achivements.SGAAchivements.UnlockAchivement("Cirno", Main.LocalPlayer);
			if (SGAWorld.downedCirno == false)
			{
				SGAWorld.downedCirno = true;
				Idglib.Chat("The snowflakes have thawed from your wings", 50, 200, 255);
			}
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write((int)NPC.localAI[3]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			NPC.localAI[3] = reader.ReadInt32();
        }

        public override bool CheckDead()
		{
			if (NPC.localAI[3] < 360)
			{
				NPC.active = true;
				NPC.life = 5;
				NPC.localAI[3] = Math.Min(NPC.localAI[3] + 1, 5);
				NPC.netUpdate = true;
			}
			return NPC.localAI[3]>320;
		}

		public override void AI()
		{
			if (NPC.localAI[3]>0)
            {

				if (NPC.localAI[3] == 360)
				{
					Gore.NewGore(NPC.Center + new Vector2(0, -24), new Vector2(0, -18), Mod.GetGoreSlot("Gores/CirnoHeadGore"));
					Gore.NewGore(NPC.Center + new Vector2(0, -24), new Vector2(0, -18), Mod.GetGoreSlot("Gores/Cirno_bow_gib"), 1f);
					Gore.NewGore(NPC.Center + new Vector2(0, 8), new Vector2(-1, -0), Mod.GetGoreSlot("Gores/Cirno_leg_gib"), 1f);
					Gore.NewGore(NPC.Center + new Vector2(0, 8), new Vector2(1, -0), Mod.GetGoreSlot("Gores/Cirno_leg_gib"), 1f);
					Gore.NewGore(NPC.Center, new Vector2(-2, -1), Mod.GetGoreSlot("Gores/Cirno_arm_gib"), 1f);
					Gore.NewGore(NPC.Center, new Vector2(2, -1), Mod.GetGoreSlot("Gores/Cirno_arm_gib"), 1f);
					NPC.localAI[3] = 2000;
					NPC.StrikeNPCNoInteraction(100000, 0, 0);
					SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 117, 1f,-0.75f);
				}

				if (NPC.localAI[3] % (int)(40 / (1 + ((NPC.localAI[3] / 120f))))==0 || NPC.localAI[3] == 360)
				{
					SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 100,0.75f);
					NPC.velocity += Main.rand.NextVector2Circular(4f, 4f);
					if (sound != null && NPC.localAI[3]<400)
						sound.Pitch = -0.80f + (NPC.localAI[3] / 200f);

					for (int num654 = 0; num654 < 1 + NPC.localAI[3] / 8f; num654++)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						Dust num655 = Dust.NewDustPerfect(NPC.position + new Vector2(Main.rand.Next(NPC.width), Main.rand.Next(NPC.height)), 59, randomcircle * (12f + (NPC.localAI[3] > 350 ? Main.rand.NextFloat(8f,15f) : 0f)), 150, Color.Aqua, 2f+(NPC.localAI[3]>350 ? 2f : 0f));
						num655.noGravity = true;
						num655.noLight = true;
					}
				}

				NPC.localAI[3] += 1;
					NPC.velocity /= 1.15f;
				NPC.dontTakeDamage = true;
				return;
            }
			Player P = Main.player[NPC.target];
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active || (!Main.dayTime && GetType() == typeof(Cirno)))
			{
				NPC.TargetClosest(false);
				P = Main.player[NPC.target];
				if (!P.active || P.dead || !Main.dayTime)
				{
					float speed = ((-10f));
					NPC.velocity = new Vector2(NPC.velocity.X, NPC.velocity.Y + speed);
					NPC.active = false;
				}

			}
			else
			{
				if (Main.expertMode)
				{
					if (NPC.life < NPC.lifeMax * 0.20 && NPC.ai[3] == 0)
					{
						NPC.ai[3] = 1;
						NPC.SpawnOnPlayer(P.whoAmI, Mod.Find<ModNPC>("CirnoIceFairy").Type);
						NPC.SpawnOnPlayer(P.whoAmI, Mod.Find<ModNPC>("CirnoIceFairy2").Type);
					}
					NPC.defense = 20 + (NPC.CountNPCS(Mod.Find<ModNPC>("CirnoIceFairy").Type) * 50) + (NPC.CountNPCS(Mod.Find<ModNPC>("CirnoIceFairy2").Type) * 50);
				}

				bool snow = P.ZoneSnow;
				//npc.dontTakeDamage = (!snow);
				NPC.netUpdate = true;
				NPC.timeLeft = 99999;
				bobbing = bobbing + 1;
				NPC.spriteDirection = -NPC.direction;
				aicounter = aicounter + 1;

				Vector2 playerloc = P.Center;
				/*if (GetType() == typeof(CirnoHellion) && Hellion.Hellion.GetHellion() != null)
				{
					playerloc = Hellion.Hellion.GetHellion().npc.Center;
					npc.dontTakeDamage = true;
				}*/

				Vector2 dist = playerloc - NPC.Center;

				if (card > 0)
				{
					if (nightmaremode)
					{
						if (nightmareprog < card * 400)
							nightmareprog += 1;
						/*if (nightmareprog == 1)
						{
							

						}*/
						if (SGAWorld.CirnoBlizzard < card * 100 && nightmareprog % 3 == 0)
							SGAWorld.CirnoBlizzard += 1;
						if (!Main.dedServ)
						{
							ScreenShaderData shad = Filters.Scene["SGAmod:CirnoBlizzard"].GetShader();
							shad.UseColor(Color.Lerp(Color.Blue, Color.Turquoise, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly)));
						}
						Main.raining = true;
						Main.windSpeed = MathHelper.Clamp(Main.windSpeed + Math.Sign((P.Center.X - NPC.Center.X)) * (-0.002f / 3f), -0.4f, 0.4f);
						Main.maxRaining = Math.Min(Main.maxRaining + 0.001f, 0.10f);
						Main.rainTime = 5;
						Main.UseStormEffects = true;


						nightmareprog = Math.Min(nightmareprog, 2000);


						if (nightmareprog > 100)
						{
							for (int i = 0; i < Main.maxPlayers; i += 1)
							{
								if (Main.player[i].active && !Main.player[i].dead)
								{
									Main.player[i].AddBuff(BuffID.WindPushed, 2);
								}
							}
						}
						if (nightmareprog > 500)
						{
							ScreenObstruction.screenObstruction = Math.Min((nightmareprog - 500) / 600f, 0.5f);
							for (int i = 0; i < Main.maxPlayers; i += 1)
							{
								if (Main.player[i].active && !Main.player[i].dead)
								{
									//if (card>3)
									//Main.player[i].AddBuff(BuffID.Obstructed, (int)((Main.maxRaining-0.5f)*120f));
									Main.player[i].AddBuff(BuffID.Darkness, (int)((Main.maxRaining - 0.5f) * 60f));
								}
							}
						}

					}



				}

				if (aistate == 2)
				{
					spellcard(card, aicounter, P);
					float floater = (float)(Math.Sin(bobbing / 14f) * 4f);
					NPC.direction = -1;
					if (dist.X > 0)
					{
						NPC.direction = 1;
					}
					NPC.velocity = new Vector2(((playerloc.X - ((NPC.Center.X))) / 150), ((playerloc.Y - ((NPC.Center.Y + 120))) / 110) + floater);
					if (NPC.life < NPC.lifeMax * damagetospellcard)
					{
						aistate = 0;
						aicounter = 0;
						framevar = 0;
						stateaction = 0;
						frameid = 0;
						attacktype = 0;
						damagetospellcard = damagetospellcard - 0.15f;
					}
				}



				if (aistate == 3)
				{

					if (aicounter > 15)
					{
						int dustType = 113;
						int dustIndex = Dust.NewDust(NPC.Center + new Vector2(-16, -16), 32, 32, dustType);
						Dust dust = Main.dust[dustIndex];
						dust.velocity.X = dust.velocity.X - NPC.velocity.X;
						dust.velocity.Y = dust.velocity.Y - NPC.velocity.Y;
						dust.scale *= 3f + Main.rand.Next(-30, 31) * 0.01f;
						dust.fadeIn = 0f;
						dust.noGravity = true;

						NPC.velocity = NPC.velocity + (NPC.DirectionTo(playerloc+(P.velocity*3f)) * (0.75f+((float)bobbing/120f)));
						NPC.velocity *= 0.96f;
						float extraspeed = (bobbing / 180f);

						if (NPC.velocity.Length() > 6f+extraspeed)
						{
							NPC.velocity.Normalize();
							NPC.velocity = NPC.velocity * (6f+extraspeed);
						}
						if (NPC.velocity.X > 0)
						{
							NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X);
							NPC.direction = 1;
						}
						else
						{
							NPC.rotation = (float)Math.Atan2(-NPC.velocity.Y, -NPC.velocity.X);
							NPC.direction = -1;
						}
						if (bobbing > 220)
						{
							float aimer = Main.rand.Next(-1000, 1000);
							if (bobbing % 25 == 0)
							{
								List<Projectile> bolts = Idglib.Shattershots(new Vector2(NPC.Center.X, NPC.Center.Y), P.position, new Vector2(P.width, P.height), ProjectileID.IceBolt, 20, 32f, 25, 3, true, (float)(aimer / 8000), false, 60);
							}
						}
						if (snow)
						{
							aistate = 0;
							aicounter = 40;
							framevar = 0;
							stateaction = 0;
							frameid = 0;
							attacktype = 0;
						}


					}
					else
					{
						bobbing = 0;
						NPC.velocity = NPC.velocity * 0.9f;
					}
				}
				else
				{
					NPC.rotation = (float)0f;
				}


				if (aistate == 0)
				{
					if (NPC.life < NPC.lifeMax * damagetospellcard)
					{
						aistate = 2;
						aicounter = 0;
						stateaction = 0;
						attacktype = 0;
						card = card + 1;
						damagetospellcard = damagetospellcard - 0.20f;
					}

					if (aicounter > 90 && !snow && Main.rand.Next(0,60) == 0)
					{
						aicounter = 0;
						aistate = 3;
						framevar = 0;
						stateaction = 2;
						frameid = 14;
					}
					if (aicounter > 140 || (aicounter > 100 && card == 3))
					{


						aicounter = 0;
						aistate = 1;
						framevar = 0;
						stateaction = 1;
						frameid = 6;
						attacktype = (int)Main.rand.Next(0, 2);

					}

					//}
					float floater = (float)(Math.Sin(bobbing / 17f) * 9f);
					if (NPC.Center.X < playerloc.X)
					{
						NPC.direction = 1;
						NPC.velocity = new Vector2(2, ((playerloc.Y - NPC.Center.Y) / 12) + floater);
					}
					else
					{
						NPC.velocity = new Vector2(-2, ((playerloc.Y - NPC.Center.Y) / 12) + floater);
						NPC.direction = -1;
					}

					NPC.velocity.Normalize();
					NPC.velocity = NPC.velocity * 2;
					Vector2 offset = (new Vector2(playerloc.X, playerloc.Y) - NPC.Center);
					if (offset.Length() > 640)
					{
						NPC.velocity += (Vector2.Normalize(offset)*((offset.Length()-640f) / 96f));
					}
				}
				if (aistate == 1 || aistate == 10)
				{
					NPC.velocity = new Vector2(0, 0);

					if (aistate == 10)
					{

						if (aicounter < 5)
							mixup = Main.rand.Next(0, 3);
						if (aicounter > 19 && aicounter < 76 && aicounter % 3 == 0 && mixup == 0)
						{
							//float aimer = Main.rand.Next(0, 0);
							//Idglib.Shattershots(new Vector2(npc.Center.X + (npc.direction * 48), npc.Center.Y), P.position, new Vector2(P.width, P.height), 348, 10, (float)26, 0, 1, true, (float)(aimer / 8000), false, Main.rand.Next(100, 120));

							float circle = aicounter/30f;
									Vector2 offset = Vector2.Lerp(NPC.Center,P.MountedCenter-new Vector2(0,200), circle);
									int proj2 = Projectile.NewProjectile(offset,Vector2.UnitY*(aicounter % 6 == 0 ? 16f : 10f), Mod.Find<ModProjectile>("CirnoIceShardHinted").Type, 40, 4, 0);
									(Main.projectile[proj2].ModProjectile as CirnoIceShardHinted).CirnoStart = NPC.Center;
									Main.projectile[proj2].netUpdate = true;

						}

						if (mixup == 2)
						{

							if (aicounter == 19)
							{

								for (int num315 = 0; num315 < 60; num315 = num315 + 1)
								{
									int dust = Dust.NewDust(new Vector2(NPC.Center.X + (NPC.direction * 48), NPC.Center.Y), 0, 0, 113, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 25, Main.hslToRgb(0.6f, 0.9f, 1f), 3f);
									Main.dust[dust].noGravity = true;
								}
							}
							if (aicounter > 9 && aicounter < 66 && aicounter % 2 == 0)
							{
								float aimer = Main.rand.Next(0, 0);
								Idglib.Shattershots(P.position + new Vector2(Main.rand.Next(-20, 20), P.Center.Y + 200), P.position + new Vector2(Main.rand.Next(-20, 20), P.Center.Y - 500), Vector2.Zero, ProjectileID.IcewaterSpit, 20, (float)23, 0, 1, true, 0, false, Main.rand.Next(270, 370));
							}
						}

						if (aicounter == 19 && mixup == 1)
						{
							List<Projectile> itz = Idglib.Shattershots(NPC.Center - new Vector2(0, 30), NPC.Center - new Vector2(-NPC.direction * 20, 30), new Vector2(0, 0), Mod.Find<ModProjectile>("SnowCloud").Type, 40, 5f, 0, 1, true, 0, false, 500);
							itz[0].velocity = itz[0].velocity + new Vector2(0, -6);
						}

					}
					if (aistate == 1)
					{

						if (card == 0 || card > 1)
						{
							if (attacktype == 0)
							{
								if (aicounter == 20)
								{
									float rotter = Main.rand.NextFloat(MathHelper.TwoPi);
									for (float circle = 0; circle < MathHelper.TwoPi; circle += MathHelper.Pi / 6f)
									{
										//if (circle % MathHelper.Pi > 1f)
										//{
											Vector2 offset = Vector2.One.RotatedBy(circle + rotter);
											int proj2 = Projectile.NewProjectile(P.Center.X + (offset.X * 300f), P.Center.Y + (offset.Y * 300f), -offset.X * 5f, -offset.Y * 5f, Mod.Find<ModProjectile>("CirnoIceShardHinted").Type, 40, 4, 0);
											(Main.projectile[proj2].ModProjectile as CirnoIceShardHinted).CirnoStart = NPC.Center;
											Main.projectile[proj2].netUpdate = true;
										//}
									}
								}
							}
							if (attacktype == 1)
							{
								if (aicounter > 19 && aicounter < 46 && aicounter % 3 == 0)
								{
									float aimer = Main.rand.Next(-1000, 1000);
									List<Projectile> bolts = Idglib.Shattershots(new Vector2(NPC.Center.X + (NPC.direction * 48), NPC.Center.Y), P.position, new Vector2(P.width, P.height), Mod.Find<ModProjectile>("CirnoBolt").Type, 50, (float)Main.rand.Next(60, 80) / 10f, 0, 1, true, (float)(aimer / 8000), false, 200);
									CirnoBolt Cbolt = bolts[0].ModProjectile as CirnoBolt;
									Cbolt.homing = 0.04f;
									bolts[0].netUpdate = true;

									//Shattershots(npc.position,P.position,new Vector2(P.width,P.height),83,20,12,40,2,true,0);
								}
							}

						}

						if (card == 1 || card > 1)
						{
							if (attacktype == 0)
							{
								if (aicounter == 20)
								{
									Idglib.Shattershots(new Vector2(NPC.Center.X + (NPC.direction * 48), NPC.Center.Y), P.position, new Vector2(P.width, P.height), Mod.Find<ModProjectile>("CirnoBolt").Type, 50, 8f, 85, 4, false, 0, false, 450);
								}
							}

							if (attacktype == 1)
							{
								for (int i = 0; i < 2; i += 1)
								{
									if (aicounter > 19 && aicounter < 79 && aicounter % 2 == 0)
									{
										float rotter = Main.rand.NextFloat(MathHelper.TwoPi);
										Vector2 offset = Main.rand.NextVector2Circular(Main.rand.NextFloat(-320, 320), Main.rand.NextFloat(-480, 240));
										Vector2 playerAim = Vector2.Normalize(P.MountedCenter - NPC.Center);

										float speed = aicounter / 2f;
										int proj2 = Projectile.NewProjectile(NPC.Center.X + (offset.X), NPC.Center.Y + (offset.Y), playerAim.X * speed, playerAim.Y * speed, Mod.Find<ModProjectile>("CirnoIceShardHinted").Type, 40, 4, 0);
										(Main.projectile[proj2].ModProjectile as CirnoIceShardHinted).CirnoStart = NPC.Center;
										Main.projectile[proj2].netUpdate = true;

									}
								}
							}
						}
					}


					if (aicounter > 80)
					{
						frameid = 12;
					}
					if (aicounter == 90 || (aicounter == 40 && card == 3 && aistate != 10))
					{
						if (aistate == 10)
						{
							aistate = 2;
						}
						else
						{
							aistate = 0;
						}
						framevar = 0;
						stateaction = 0;
						frameid = 0;
						aicounter = 0;
					}
				}




			}
		}

		//		public override bool? CanBeHitByItem(Player player, Item item)
		//{
		//	return CanBeHitByPlayer(player);
		//}
		//public override bool? CanBeHitByProjectile(Projectile projectile)
		//{
		//	return CanBeHitByPlayer(Main.player[projectile.owner]);
		//}
		//private bool? CanBeHitByPlayer(Player P){
		//if (phase<60){
		//return false;
		//}else{
		//return true;
		//}
		//}

		/*		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			OnHit(player, damage);
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			OnHit(Main.player[projectile.owner], damage);
		}

		private void OnHit(Player player, int damage)
		{
			if (player.active)
			{
			Vector2 dist=player.Center-npc.Center;
			if (dist.Length()>600 || (aistate>1 && aicounter<50)){
				npc.life=npc.life+damage;
				damage=0;
			}else{
			damagetospellcard=damagetospellcard-damage;
			}
			}else{
			npc.life=npc.life+damage;
			}

		}
		*/

		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			//if (Main.expertMode || Main.rand.Next(2) == 0)
			//{
			player.AddBuff(BuffID.Chilled, 200, true);
			if (Main.expertMode)
				player.AddBuff(BuffID.Frostburn, 60 * 4, true);
			//}
		}




		public override void FindFrame(int frameHeight)
		{
			framevar = framevar + 1;
			if (stateaction == 0)
			{
				if (frameid > 6)
				{
					frameid = 0;
				}
				if (framevar > 4)
				{
					frameid = frameid + 1;
					framevar = 0;
					if (frameid > 5)
					{
						frameid = 0;
					}
				}
			}

			if (stateaction == 2)
			{
				if (aistate == 0)
				{
					stateaction = 0;
					frameid = 0;
				}
				if (framevar > 3)
				{
					framevar = 0;
					frameid = frameid + 1;
				}
				if (frameid > 19)
				{
					frameid = 16;
				}
			}

			if (stateaction == 1)
			{
				if (aicounter < 81)
				{
					if (framevar > 5)
					{
						framevar = 0;
						frameid = frameid + 1;
					}
					if (frameid > 12)
					{
						frameid = 11;
					}
				}
				else
				{
					frameid = 12;
				}
			}
			//npc.spriteDirection = (int)npc.ai[1]*14;
			if (frameid == 2) { frameid = 3; }
			NPC.frame.Y = frameid * 80;
		}


		private void spellcard(int mycardid, int counter, Player P)
		{

			if (counter > 50)
			{
				if (mycardid == 1)
				{

					if (counter % 6 == 0)
					{

						Vector2 vis = new Vector2(P.Center.X + Main.rand.Next(-800, 800), P.Center.Y);
						float aimer = Main.rand.Next(-1000, 1000);
						if ((counter % 600) > 540 && Main.expertMode)
						{
							Idglib.Shattershots(new Vector2(P.Center.X - 800, P.Center.Y), new Vector2(P.Center.X + 800, P.Center.Y), new Vector2(0, 0), Mod.Find<ModProjectile>("CirnoBolt").Type, 25, (float)Main.rand.Next(60, 80) / 8, 0, 1, true, (float)(aimer / 9000), false, 200);
							Idglib.Shattershots(new Vector2(P.Center.X + 800, P.Center.Y), new Vector2(P.Center.X - 800, P.Center.Y), new Vector2(0, 0), Mod.Find<ModProjectile>("CirnoBolt").Type, 25, (float)Main.rand.Next(60, 80) / 8, 0, 1, true, (float)(aimer / 9000), false, 200);
						}
						Idglib.Shattershots(new Vector2(vis.X, P.Center.Y - 600f), new Vector2(vis.X, vis.Y + 600f), new Vector2(0, 0), Mod.Find<ModProjectile>("CirnoIceShard").Type, 20, (float)Main.rand.Next(60, 80) / 10, 0, 1, true, (float)(aimer / 5000), false, 200);

					}

				}
				if (card == 2)
				{
					if (Main.expertMode)
					{

						if (mixup < 100)
						{
							mixup = 100;
							for (int i = -400; i <= 401; i = i + 800)
							{
								List<Projectile> itz = Idglib.Shattershots(NPC.Center - new Vector2(i * 3, 50), NPC.Center, new Vector2(0, -50), Mod.Find<ModProjectile>("SnowCloud").Type, 40, 2f, 0, 1, true, 0, false, 1000);
								itz[0].velocity = itz[0].velocity + new Vector2(0, -4);
							}
						}


						if (counter % 350 == 0)
						{
							SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 30, 1f, -0.5f);
							for (int i = -200; i <= 201; i = i + 50)
							{
								Idglib.Shattershots(new Vector2(P.Center.X - 600, P.Center.Y + i), P.Center + new Vector2(0, i), new Vector2(0, 0), Mod.Find<ModProjectile>("CirnoIceShard").Type, 30, (float)2, 0, 1, true, 0, false, 190);
								Idglib.Shattershots(new Vector2(P.Center.X + 600, P.Center.Y + i), P.Center + new Vector2(0, i), new Vector2(0, 0), Mod.Find<ModProjectile>("CirnoIceShard").Type, 30, (float)2, 0, 1, true, 0, false, 190);
							}
						}
					}
					if ((counter) % 180 == 0) { Idglib.Shattershots(new Vector2(NPC.Center.X + (NPC.direction * 48), NPC.Center.Y), P.position, new Vector2(P.width, P.height), ProjectileID.EnchantedBeam, 50, (float)13, 0, 1, true, 0, false, 120); }
					if ((counter - 40) % 180 == 0) { Idglib.Shattershots(new Vector2(NPC.Center.X + (NPC.direction * 48), NPC.Center.Y), P.position, new Vector2(P.width, P.height), Mod.Find<ModProjectile>("CirnoIceShard").Type, 20, (float)7, 135, 8, true, 0, false, 160); SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 30, 1f, 0f); }
					if ((counter - 80) % 180 == 0) { Idglib.Shattershots(new Vector2(NPC.Center.X + (NPC.direction * 48), NPC.Center.Y), P.position, new Vector2(P.width, P.height), Mod.Find<ModProjectile>("CirnoIceShard").Type, 20, (float)9, 85, 4, false, 0, false, 140); SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 30, 1f, 0.30f); }
				}

				if (card == 3)
				{
					if (counter % 8 == 0)
					{
						Idglib.Shattershots(new Vector2(NPC.Center.X + (NPC.direction * 48), NPC.Center.Y), P.position, new Vector2(P.width, P.height), 263, 35, (float)(counter / 8), 0, 1, true, (float)(counter - 140) / 70, true, 120);
						Idglib.Shattershots(new Vector2(NPC.Center.X + (NPC.direction * 48), NPC.Center.Y), P.position, new Vector2(P.width, P.height), 263, 35, (float)(counter / 8), 0, 1, true, -(float)(counter - 140) / 70, true, 120);
					}
					if (counter % 200 == 0)
					{
						aicounter = 0;
						aistate = 10;
						framevar = 0;
						stateaction = 1;
						frameid = 6;
						attacktype = 0;//(int)Main.rand.Next(0,2);
					}
				}


			}

		}


	}


	public class SnowCloud : ModProjectile
	{

		public virtual int projectileid => ProjectileID.SnowBallFriendly;
		public virtual Color colorcloud => Main.hslToRgb(0.6f, 0.8f, 0.8f);
		public virtual int rate => 5;


		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			//projectile.aiStyle = 1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.tileCollide=false;
		}

				public override string Texture
		{
			get { return "Terraria/Item_" + 5; }
		}

	public override bool? CanHitNPC(NPC target){
		return false;
	}
	public override bool CanHitPlayer(Player target){
		return false;
	}
		public override void AI()
		{
			Projectile.velocity = new Vector2(Projectile.velocity.X, Projectile.velocity.Y * 0.95f);
			int q = 0;
			for (q = 0; q < 4; q++)
			{

				int dust = Dust.NewDust(Projectile.position - new Vector2(100, 0), 200, 12, DustID.Smoke, 0f, Projectile.velocity.Y * 0.4f, 100, colorcloud, 3f);
				Main.dust[dust].noGravity = true;
				//Main.dust[dust].velocity *= 1.8f;
				//Main.dust[dust].velocity.Y -= 0.5f;
				//Main.playerDrawDust.Add(dust);
			}
			Projectile.ai[0]++;
			int target2 = Idglib.FindClosestTarget(Projectile.friendly ? 0 : 1, Projectile.position, new Vector2(0, 0));
			Entity target;
			target = Main.player[target2] as Player;
			if (Projectile.friendly)
			{
				target = Main.npc[target2] as NPC;
				//target=Main.player[target2];
			}

			if (target is Player)
			{
				Player targetasplayer = target as Player;
				if (targetasplayer != null && targetasplayer.ownedProjectileCounts[Mod.Find<ModProjectile>("SnowfallCloud").Type] > 0)
				{
					Projectile.Kill();
				}
			}

			if (target != null)
			{



				Vector2 dist = target.Center - Projectile.position;
				if (System.Math.Abs(dist.X) < 250)
				{
					if (Projectile.ai[0] % rate == 0)
					{
						List<Projectile> itz = Idglib.Shattershots(Projectile.Center + new Vector2(Main.rand.Next(-100, 100), 0), Projectile.Center + new Vector2(Main.rand.Next(-200, 200), 500), new Vector2(0, 0), projectileid, (int)Projectile.damage, 8f, 0, 1, true, 0, true, 220);
						itz[0].friendly = Projectile.friendly;
						itz[0].hostile = Projectile.hostile;
						itz[0].coldDamage = true;
						itz[0].netUpdate = true;
						// itz[0].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
						// itz[0].Throwing().thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
						itz[0].minion = true;
					}
				}


			}



		}

public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
{
return false;
}

}

	public class CirnoBolt : ModProjectile
	{

		public double keepspeed =0.0;
		public float homing=0.02f;
		Vector2 gothere;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cirno's Grace");
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.IceBolt; }
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.IceBolt);
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.coldDamage = true;
			Projectile.npcProj = true;
			AIType = 0;//ProjectileID.IceBolt;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Frostburn, 60 * 10);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(1,3)==1)
			target.AddBuff(BuffID.Frostburn, 60 * 5);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)gothere.X);
			writer.Write((double)gothere.Y);
			writer.Write((double)homing);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			gothere.X = (float)reader.ReadDouble();
			gothere.Y = (float)reader.ReadDouble();
			homing = (float)reader.ReadDouble();
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type=ProjectileID.IceBolt;
			for (int num315 = 0; num315 < 15; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 92, Projectile.velocity.X+(float)(Main.rand.Next(-20,20)/15f), Projectile.velocity.Y+(float)(Main.rand.Next(-20,20)/15f), 50, Main.hslToRgb(0.6f,0.9f, 1f), 2.4f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
			}
			return true;
		}

		public override void AI()
		{
		for (int num315 = 0; num315 < 2; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 92, Projectile.velocity.X, Projectile.velocity.Y, 50, Main.hslToRgb(0.6f,0.9f, 1f), 1.7f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.3f;
				//dust3.shader = GameShaders.Armor.GetShaderFromItemId(ItemID.MidnightRainbowDye);
			}

			if (Projectile.ai[0] < 1)
			{
				if (Projectile.hostile)
					homing *= 1f;
				Projectile.ai[1] = -1;
			}

			Projectile.ai[0]=Projectile.ai[0]+1;
		if (Projectile.ai[0]<2){
		keepspeed=(Projectile.velocity).Length();
		}
			if (gothere==null || Projectile.ai[0] % 40 == 0 || Projectile.ai[0] == 1)
			{
				int target3 = Idglib.FindClosestTarget(Projectile.friendly ? 0 : 1, Projectile.position, new Vector2(0, 0), true, true, true, Projectile);

				//if (target2 > 0) {
				Entity target;
				target = Main.player[target3] as Player;
				if (Projectile.friendly)
				{
					target = Main.npc[target3] as NPC;
					//target=Main.player[target2];
				}
				gothere = target.Center;
				Projectile.netUpdate = true;
			}
				if (gothere != null && (gothere - Projectile.Center).Length() < 1000f)
				{
					if (Projectile.ai[0] < (Main.expertMode == true ? 150f : 50f) || Projectile.friendly)
					{
						Projectile.velocity = Projectile.velocity + (Projectile.DirectionTo(gothere) * ((float)keepspeed * homing));
						Projectile.velocity.Normalize();
						Projectile.velocity = Projectile.velocity * (float)keepspeed;
					}
				}

		}



		}

	public class CirnoIceFairy : IceFairy
	{
		int shooting2=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("2nd Strongest Ice Fairy");
			Main.npcFrameCount[NPC.type] = 4;
		}
		public override string Texture
		{
			get { return("SGAmod/NPCs/IceFairy");}
		}
		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 40;
			NPC.damage = 0;
			NPC.defense = 4;
			NPC.lifeMax = 1800;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 0f;
			NPC.knockBackResist = 0.0f;
			NPC.aiStyle = 22;
			AIType = 0;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.value = 40000f;
			NPC.coldDamage = true;
		}

				public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0f;
		}

		public override void AI()
		{
		overAI(NPC);
		}

		public void overAI(NPC npc2)
		{
		base.AI();
		shooting2=shooting2+1;
		Player P = Main.player[NPC.target];
		npc2.ai[3]+=Main.rand.Next(-2,4);
		int npctype=Mod.Find<ModNPC>("Cirno").Type;
		if (NPC.CountNPCS(npctype)>0){
		NPC myowner=Main.npc[NPC.FindFirstNPC(npctype)];
			Vector2 here=myowner.Center;
		if (npc2.ai[3]%400>150){
		float leftorright = (NPC.type==Mod.Find<ModNPC>("CirnoIceFairy2").Type) ? -128f : 128f;
		if (npc2.ai[3]%300>200){
		here=P.Center;
		leftorright = (NPC.type==Mod.Find<ModNPC>("CirnoIceFairy2").Type) ? 220f : -220f;
		}
		float bobbing=-30f+(float)Math.Sin(NPC.ai[3]/31)*20f;
		npc2.velocity=(npc2.velocity+((here+new Vector2(leftorright,bobbing)-(npc2.position))*0.02f)*0.01f)*0.99f;
		NPC.aiStyle = -1;
		}else{
		NPC.aiStyle = 22;
		}
		if (shooting2%400>250){
		if (shooting2%15==0){
		SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 30, 1f, -0.25f+Main.rand.NextFloat()/2f);
		Idglib.Shattershots(NPC.Center,myowner.Center, new Vector2(0,0), Mod.Find<ModProjectile>("CirnoIceShard").Type, 30,15,0,1,true,0,false,180);
		}}
		npc2.timeLeft=99;
		}else{
		npc2.active=false;
		}
		}

		public override void NPCLoot()
		{
		//stuff	
        }


	}

	public class CirnoIceFairy2 : CirnoIceFairy
	{
		int shooting=100;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("3rd Strongest Ice Fairy");
			Main.npcFrameCount[NPC.type] = 4;
		}
		public override string Texture
		{
			get { return("SGAmod/NPCs/IceFairy");}
		}
		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 40;
			NPC.damage = 0;
			NPC.defense = 20;
			NPC.lifeMax = 1000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 0f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = 22;
			AIType = 0;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.value = 40000f;
		}

	}

	public class CirnoIceShard : ModProjectile
	{

		int fakeid = ProjectileID.FrostShard;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Baka Ice Shard");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.CloneDefaults(fakeid);
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 0;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1000;
			Projectile.coldDamage = true;
			Projectile.npcProj = true;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + fakeid; }
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = fakeid;
			return true;
		}

		public virtual void Moving()
		{
			if (Main.rand.Next(0, 2) == 1)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Ice);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.NextFloat(0.1f, 0.25f));
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}

		public override void AI()
		{
			Moving();
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Chilled, 60 * 2);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Chilled, 60 * 2);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (Projectile.localAI[0] < 100)
				Projectile.localAI[0] = 100 + Main.rand.Next(0, 3);
			Texture2D tex = SGAmod.HellionTextures[5];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 5) / 2f;
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
			int timing = (int)(Projectile.localAI[0] - 100);
			timing %= 5;
			timing *= ((tex.Height) / 5);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 5), lightColor*Projectile.Opacity, MathHelper.ToRadians(0) + Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

	}

	public class CirnoIceShardHinted : CirnoIceShard
	{
		float strength => Math.Min(1f-(Projectile.localAI[1] / 110f), 1f);
		public Vector2 bezspot1 = default;
		public Vector2 bezspot2 = default;
		public Vector2 CirnoStart = default;
		public Vector2[] oldPos = new Vector2[10];
		public float speedIncrease = 60f;
		public float homeInTime = 100;
		protected Vector2 VectorEffect = default;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stay Frosty!");
		}

		public override bool CanDamage()
		{
			return Projectile.localAI[1] >= 100;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = 1000;
		}

		public override void AI()
		{
			Projectile.localAI[1] += 1;
			if (Projectile.localAI[1] >= homeInTime+10)
			{
				base.AI();
            }
            else
            {
				Projectile.rotation = MathHelper.Lerp((Projectile.Center-VectorEffect).ToRotation()+MathHelper.PiOver2,Projectile.velocity.ToRotation()+MathHelper.PiOver2, Projectile.localAI[1]/ homeInTime);
			}
			Projectile.Opacity = ((Projectile.localAI[1]-30)/70f);
			Projectile.position -= Projectile.velocity * MathHelper.Clamp(1f-((Projectile.localAI[1] - homeInTime) / 60f),0f, 1f);


			if (Projectile.localAI[1] == 1)
			{
				bezspot1 = CirnoStart + Main.rand.NextVector2CircularEdge(200, 200);
				bezspot2 = Projectile.Center + Main.rand.NextVector2CircularEdge(500, 500);
				for (int k = oldPos.Length - 1; k > 0; k--)
				{
					oldPos[k] = CirnoStart;
				}
			}

			VectorEffect = IdgExtensions.BezierCurve(CirnoStart, CirnoStart, bezspot1, bezspot2, Projectile.Center, Math.Min(Projectile.localAI[1] / homeInTime, 1f));
			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = VectorEffect;

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				if (oldPos[k] == default)
				oldPos[k] = VectorEffect;
			}

			if (strength > 0)
			{
				TrailHelper trail = new TrailHelper("DefaultPass", Mod.Assets.Request<Texture2D>("noise").Value);
				trail.color = delegate (float percent)
				{
					return Color.Aqua;
				};
				trail.projsize = Projectile.Hitbox.Size() / 2f;
				trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -1f);
				trail.trailThickness = 2;
				trail.trailThicknessIncrease = 6;
				trail.capsize = new Vector2(4f, 0f);
				trail.strength = strength;
				trail.DrawTrail(oldPos.ToList(), Projectile.Center);
			}

			if (Projectile.Opacity > 0)
			{
				if (Projectile.localAI[0] < 100)
					Projectile.localAI[0] = 100 + Main.rand.Next(0, 3);
				Texture2D tex = SGAmod.HellionTextures[5];
				Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 5) / 2f;
				Vector2 drawPos = ((VectorEffect - Main.screenPosition)) + new Vector2(0f, 4f);
				int timing = (int)(Projectile.localAI[0] - homeInTime);
				timing %= 5;
				timing *= ((tex.Height) / 5);
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 5), lightColor * Projectile.Opacity, MathHelper.ToRadians(0) + Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}

			return false;
		}

	}

		/*public class CirnoHellion : Cirno
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cirno, Hellion Annointed");
			Main.npcFrameCount[npc.type] = 20;
		}

		public override string Texture
		{
			get { return "SGAmod/NPCs/Cirno"; }
		}
		public override void AI()
		{
			base.AI();
			Hellion.Hellion hell = Hellion.Hellion.GetHellion();
			if (hell == null)
			{
				npc.active = false;
				return;
			}
			npc.life = 10 + (int)(((float)hell.army.Count / (float)hell.armysize) * npc.lifeMax);
		}
		public override void SetDefaults()
		{
			npc.width = 50;
			npc.height = 60;
			npc.damage = 90;
			npc.defense = 30;
			npc.lifeMax = 25000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.buyPrice(0, 20, 0, 0);
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			npc.boss = true;
			AIType = NPCID.Wraith;
			AnimationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			music = MusicID.Boss2;
			bossBag = mod.ItemType("CirnoBag");
			npc.value = Item.buyPrice(0, 15, 0, 0);
		}
	}*/

}

