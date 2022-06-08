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
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace SGAmod.NPCs.Cratrosity
{
	[AutoloadBossHead]

	public class Cratrogeddon : Cratrosity, ISGABoss
	{
		private int pmlphase = 0;
		private int pmlphasetimer = 0;
		private float aimAngle = 0;
		private List<int> summons = new List<int>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cratrogeddon");
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
		}

		public override string Texture
		{
			get { return "SGAmod/NPCs/Cratrosity/TitanCrate"; }
		}

		public override string BossHeadTexture => "SGAmod/NPCs/Cratrosity/TitanCrate";

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.SuperHealingPotion;
		}

		public override bool CheckActive()
		{
			return false;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.damage = 80;
			NPC.defense = 50;
			NPC.lifeMax = 40000;
			NPC.value = Item.buyPrice(1, 0, 0, 0);
			//music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Evoland 2 OST - Track 46 (Ceres Battle)");
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
		}

		public override void NPCLoot()
		{
			if (Main.expertMode)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("TerrariacoCrateKeyUber").Type);
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("MoneySign").Type, Main.rand.Next(40, Main.expertMode ? 85 : 65));
			if (Main.rand.Next(7) == 0)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Armors.Vanity.CratrogeddonMask>());
			}
			Achivements.SGAAchivements.UnlockAchivement("Cratrogeddon", Main.LocalPlayer);
			if (SGAWorld.downedCratrosityPML == false)
			{
				SGAWorld.AdvanceHellionStory();
			}
			SGAWorld.downedCratrosityPML = true;
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(pmlphasetimer);
            base.SendExtraAI(writer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			pmlphasetimer = reader.ReadInt32();
			base.ReceiveExtraAI(reader);
        }


        public override void OrderOfTheCrates(Player P)
		{
			if (pmlphase == 2 && pmlphasetimer > -30000)
			{
				int timer = 3000000 - pmlphasetimer;
				Vector2 playerdiff = P.MountedCenter - NPC.Center;
				nonaispin = nonaispin + 0.6f;
				for (int layer = 0; layer < phase; layer++)
				{
					for (int index = 0; index < cratesPerRing[layer].Count; index++)
					{
						CratrosityInstancedCrate crate = cratesPerRing[layer][index];

						Vector2 anglediff = P.MountedCenter - crate.crateVector;
						//Cratesangle[a, i] = (a % 2 == 0 ? 1 : -1) * ((nonaispin * 0.01f) * (1f + a / 3f)) + 2.0 * Math.PI * ((i / (double)Cratesperlayer[a]));
						//Cratesdist[a, i] = compressvar * 20f + ((float)a * 20f) * compressvar;
						int adder = (((layer * cratesPerRing[layer].Count) + index) * 10);
						float crateangle2 = anglediff.ToRotation();
						if ((timer + adder)%600 >= 300 && timer >= 600)
						{
							if ((timer + adder) % 600 == 300)
							{
								crate.crateAngle = crateangle2;

								SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_DarkMageAttack, NPC.Center);
								if (sound != null)
								{
									sound.Pitch = -0.25f;
								}

								for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
								{
									Vector2 offset = f.ToRotationVector2();
									int dust = Dust.NewDust(crate.crateVector + (offset * 32f), 0, 0, DustID.GoldFlame);
									Main.dust[dust].scale = 1.5f;
									Main.dust[dust].noGravity = true;
									Main.dust[dust].velocity = f.ToRotationVector2() * 4f;
								}
							}
							if ((timer + adder) % 600 > 330)
							{
								int counterdist = ((timer + adder) % 600)-330;
								float cratevelocitybutcastedasfloat = crate.crateAngle;
								Vector2 disttoplayer = P.MountedCenter - crate.crateVector;
								Vector2 velo = cratevelocitybutcastedasfloat.ToRotationVector2();
								if (disttoplayer.LengthSquared()>200*200)
									crate.crateAngle = (cratevelocitybutcastedasfloat.AngleLerp(disttoplayer.ToRotation(), 0.010f));

								crate.crateVector += velo * MathHelper.Clamp(counterdist/120f,0f,1f)*60f;

								for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 2f)
								{
									int dust = Dust.NewDust(crate.crateVector - new Vector2(16,16), 32, 32, DustID.GoldFlame);
									Main.dust[dust].scale = 1f;
									Main.dust[dust].noGravity = true;
									Main.dust[dust].velocity = (velo+Main.rand.NextVector2Circular(4f,4f)) * 1f;
								}

								Point point = crate.crateVector.ToPoint();
								Rectangle rect = new Rectangle(point.X - 16, point.Y - 16, 32, 32);
								foreach(Player player in Main.player.Where(testby => testby.active && !testby.dead && testby.Hitbox.Intersects(rect)))
                                {
									if (player!= null)
									player.Hurt(PlayerDeathReason.ByCustomReason(player.name + "Got smashed"), NPC.damage*2, NPC.direction);
                                }

							}

						}
						else
						{
							//Cratesangle[a, i] = (float)(Cratesangle[a, i])
							float dist = ((index * layer)+index)*32f;

							Vector2 goHere = (NPC.Center + (Vector2.UnitX).RotatedBy(anglediff.ToRotation()) * dist);

							float crateangle = crate.crateAngle;
							if ((timer + adder) > 150)
							{
								crate.crateAngle = (crateangle.AngleLerp((goHere - crate.crateVector).ToRotation(), 0.025f));
							}

							crate.crateVector += crateangle.ToRotationVector2()*(1f+Math.Min((goHere - crate.crateVector).Length()*0.01f,40f));
						}

						//Vector2 it = new Vector2(P.Center.X - crate.crateVector.X, P.Center.Y - crate.crateVector.Y);
						//it.Normalize();

					}
				}


			}
			else
			{
				base.OrderOfTheCrates(P);

			}
		}

		public override void CrateRelease(int phase)
		{
			base.CrateRelease(phase);
		}

		public override void FalseDeath(int phase)
		{
			pmlphase = pmlphase + 1;
			pmlphasetimer = 1100;
			if (pmlphase == 2) { pmlphasetimer = 3000000; }
			Idglib.Chat("Impressive, but not good enough", 144, 79, 16);
			if (pmlphase == 1)
			{
				summons.Insert(0, Mod.Find<ModNPC>("CratrosityCrateOfSlowing").Type);
			}
			if (pmlphase == 2)
			{
				summons.Insert(0, Mod.Find<ModNPC>("CratrosityCrateOfSlowing").Type);
				summons.Insert(0, Mod.Find<ModNPC>("CratrosityCrateOfPoisoned").Type);
			}
			if (pmlphase == 3)
			{
				summons.Insert(0, Mod.Find<ModNPC>("CratrosityCrateOfWitheredArmor").Type);
				summons.Insert(0, Mod.Find<ModNPC>("CratrosityCrateOfWitheredWeapon").Type);
			}
			if (pmlphase > 3)
			{
				summons.Insert(0, Mod.Find<ModNPC>("CratrosityCrateOfPoisoned").Type);
				summons.Insert(0, Mod.Find<ModNPC>("CratrosityCrateOfSlowing").Type);
				summons.Insert(0, Mod.Find<ModNPC>("CratrosityCrateOfWitheredArmor").Type);
				summons.Insert(0, Mod.Find<ModNPC>("CratrosityCrateOfWitheredWeapon").Type);
			}
			for (int ii = 0; ii < summons.Count; ii += 1)
			{
				int spawnedint = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, summons[0]); summons.RemoveAt(0);
				NPC him = Main.npc[spawnedint];
				him.life = (int)(NPC.life * 0.25f);
				him.lifeMax = (int)(NPC.lifeMax * 0.25f);
			}
		}

		public override void AI()
		{
			Player P = Target;
			Cratrosity origin = npc.ModNPC as Cratrosity;
			pmlphasetimer--;
			NPC.dontTakeDamage = false;

			if (NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.WoodenCrate.ToString()).Type) + NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.IronCrate.ToString()).Type) > 0 || (pmlphasetimer>0 && P.DistanceSQ(NPC.Center) > 1000 * 1000))
				NPC.dontTakeDamage = true;

			if (pmlphasetimer > 0)
			{
				NPC.localAI[0] = 5;

				//phase 1
				if (pmlphase == 1)
				{
					OrderOfTheCrates(P);
					origin.compressvargoal = 4f;
					origin.themode = 1;
					NPC.rotation = Idglib.LookAt(NPC.Center, P.Center);
					//npc.dontTakeDamage = true;
					NPC.velocity = (NPC.velocity * 0.97f);
					if (pmlphasetimer < 1000)
					{
						Vector2 it = new Vector2(P.Center.X - NPC.Center.X, P.Center.Y - NPC.Center.Y);
						it.Normalize();
						if (pmlphasetimer % 120 == 0)
						{
							NPC.velocity = (it * (30f - pmlphasetimer * 0.02f));
						}
						if (pmlphasetimer % 120 < 60 && pmlphasetimer % 20 == 0)
						{
							Idglib.Shattershots(NPC.Center, NPC.Center + it * 50, new Vector2(0, 0), ProjectileID.NanoBullet, 40, (float)6, 80, 3, true, 0, false, 600);
							Idglib.PlaySound(13, NPC.Center, 0);
						}
					}
				}
				//phase 2
				if (pmlphase == 2)
				{
					OrderOfTheCrates(P);
					NPC.velocity = (NPC.velocity * 0.77f);
					Vector2 it = new Vector2(P.Center.X - NPC.Center.X, P.Center.Y - NPC.Center.Y);
					it.Normalize();
					NPC.velocity += it * 0.3f;
					//npc.Opacity += (0.1f - npc.Opacity) / 30f;
				}

				//phase 3
				if (pmlphase == 3)
				{
					NPC.ai[0] = 0;

					if (pmlphasetimer > 1000)
					{
						int spawnedint = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, Mod.Find<ModNPC>("CratrosityNight").Type);
						spawnedint = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, Mod.Find<ModNPC>("CratrosityLight").Type);
						pmlphasetimer = 105;
						NPC.netUpdate = true;
						Idglib.Chat("Give in to Temptation!", 144, 79, 16);
					}

					if (NPC.CountNPCS(Mod.Find<ModNPC>("Cratrosity").Type) > 0)
					{
						pmlphasetimer = 100;
					}

					NPC.dontTakeDamage = true;
					base.OrderOfTheCrates(P);
					NPC.velocity = (NPC.velocity * 0.77f);
					Vector2 it = new Vector2(P.Center.X - NPC.Center.X, P.Center.Y - NPC.Center.Y);
					it.Normalize();
					NPC.velocity += it * 0.3f;
					NPC.Opacity += (0.1f - NPC.Opacity) / 30f;


				}

				//phase 4
				if (pmlphase > 3)
				{
					if (pmlphasetimer < 300)
						NPC.dontTakeDamage = false;

					NPC.velocity /= 1.4f;
					if (pmlphasetimer > 1000)
					{
						NPC.netUpdate = true;
						pmlphasetimer = 400;
					}

				}


			}
			else
			{
				NPC.rotation = NPC.rotation * 0.85f;
				NPC.Opacity += (1f - NPC.Opacity) / 30f;
				base.AI();
			}


			if (phase < 1)
			{
				int val;
				val = (int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.WoodenCrate.ToString()).Type)) +
(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.IronCrate.ToString()).Type)) +
(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.GoldenCrate.ToString()).Type)) +
(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.DungeonFishingCrate.ToString()).Type)) +
(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.JungleFishingCrate.ToString()).Type)) +
(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + Cratrosity.EvilCrateType.ToString()).Type)) +
(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.HallowedFishingCrate.ToString()).Type)) +
(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.FloatingIslandFishingCrate.ToString()).Type));
				val += NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrateOfWitheredWeapon").Type) +
					NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrateOfWitheredArmor").Type) +
					NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrateOfPoisoned").Type) +
					NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrateOfSlowing").Type);
				if (val > 0)
					NPC.dontTakeDamage = true;
			}



		}

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
			if ((pmlphase == 3 || pmlphase==2) && NPC.dontTakeDamage)
				return false;

            return base.CanHitPlayer(target, ref cooldownSlot);
        }



    }


	public class CratrosityCrateOfWitheredWeapon : CratrosityCrateOfSlowing
	{
		protected override int BuffType => BuffID.WitheredWeapon;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Crate Of Withered Weapon");
			Main.npcFrameCount[NPC.type] = 1;
		}
	}
	public class CratrosityCrateOfWitheredArmor : CratrosityCrateOfSlowing
	{
		protected override int BuffType => BuffID.WitheredArmor;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Crate Of Withered Armor");
			Main.npcFrameCount[NPC.type] = 1;
		}
	}

	public class CratrosityCrateOfPoisoned : CratrosityCrateOfSlowing
	{
		protected override int BuffType => BuffID.Venom;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Crate Of Venom");
			Main.npcFrameCount[NPC.type] = 1;
		}
	}

	public class CratrosityCrateOfSlowing : CratrosityCrate
	{
		protected virtual int BuffType => BuffID.Slow;
		public override string Texture
		{
			get { return "SGAmod/NPCs/Cratrosity/" + Name; }
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crate Of Slowing");
			Main.npcFrameCount[NPC.type] = 1;
			NPC.life = 150000;
			NPC.lifeMax = 150000;
		}


		public override void NPCLoot()
		{
			return;
		}

		/*public override string Texture
		{
			get { return "Terraria/Buff_" + BuffType; }
		}*/


		public override void AI()
		{
			base.AI();
			int npctype = Mod.Find<ModNPC>("Cratrogeddon").Type;
			if (Hellion.Hellion.GetHellion() != null)
				npctype = Mod.Find<ModNPC>("Hellion").Type;
			if (NPC.CountNPCS(npctype) > 0)
			{

				for (int i = 0; i <= Main.maxPlayers; i++)
				{
					Player thatplayer = Main.player[i];
					if (thatplayer.active && !thatplayer.dead)
					{
						thatplayer.AddBuff(BuffType, 2);
					}
				}

				NPC myowner = Main.npc[NPC.FindFirstNPC(npctype)];
				NPC.ai[0] += Main.rand.Next(0, 4);
				NPC.netUpdate = true;
				NPC.velocity = NPC.velocity * 0.95f;
				if (myowner.ai[0] % 350 > 250) { NPC.velocity = NPC.velocity * 0.45f; }
				if (myowner.ai[0] % 150 == 140)
				{
					Player P = Main.player[myowner.target];
					List<Projectile> itz = Idglib.Shattershots(NPC.Center, P.position, new Vector2(P.width, P.height), ModContent.ProjectileType<GlowingPlatinumCoin>(), 45, 8, 0, 1, true, 0, false, 220);
					itz[0].aiStyle = 5;
				}
			}
			else
			{
				NPC.active = false;

			}

		}

	}


}