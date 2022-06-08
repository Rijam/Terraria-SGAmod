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
using SGAmod.Items.Weapons;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using Terraria.Graphics.Shaders;
using Terraria.Audio;

namespace SGAmod.NPCs.Cratrosity
{
	public class CratrosityInstancedCrate
    {
		public int crateType;
		public int crateRing;
		public float crateAngle=0;
		public float crateDist;
		public Vector2 crateVector;
		public int crateState = 0;
		public int crateTimer = 0;
		public Cratrosity boss;

		public CratrosityInstancedCrate(Cratrosity boss,int crateType,float crateDist,int crateRing)
        {
			this.crateType = crateType;
			this.crateDist = crateDist;
			this.crateRing = crateRing;
			this.boss = boss;
		}

		public bool Update()
        {
			crateTimer += 1;
			return true;
		}

		public void Release(float cratehp)
        {
			NPC npc = boss.NPC;
			Mod mod = boss.Mod;
			int crateToSpawn = Mod.Find<ModNPC>("CratrosityCrate" + crateType.ToString()).Type;
			if (crateType == ModContent.ItemType<HavocGear.Items.DankCrate>())
				crateToSpawn = Mod.Find<ModNPC>("CratrosityCrateDankCrate").Type;

			int spawnedint = NPC.NewNPC((int)crateVector.X, (int)crateVector.Y, crateToSpawn);
			NPC spawned = Main.npc[spawnedint];
			Item cratetypeitem = new Item();
			cratetypeitem.SetDefaults(crateType);

			spawned.value = cratetypeitem.value;
			spawned.damage = (int)spawned.damage * (npc.damage / boss.defaultdamage);
			if (crateType == ItemID.WoodenCrate)
			{
				spawned.aiStyle = 10;
				spawned.knockBackResist = 0.98f;
				spawned.lifeMax = (int)cratehp / 5;
				spawned.life = (int)cratehp / 5;
				spawned.damage = (int)50 * (npc.damage / boss.defaultdamage);
				return;
			}
			if (crateType == ItemID.IronCrate)
			{
				spawned.aiStyle = 23;
				spawned.knockBackResist = 0.99f;
				spawned.lifeMax = (int)(cratehp * 0.30);
				spawned.life = (int)(cratehp * 0.30);
				spawned.damage = (int)60 * (npc.damage / boss.defaultdamage);
				return;
			}
			if (crateType == ItemID.DungeonFishingCrate || crateType == ItemID.JungleFishingCrate)
			{
				spawned.aiStyle = crateType == ItemID.DungeonFishingCrate ? 56 : 86;
				spawned.knockBackResist = crateType == ItemID.DungeonFishingCrate ? 0.92f : 0.96f;
				spawned.lifeMax = crateType == ItemID.DungeonFishingCrate ? (int)(cratehp * 0.4) : (int)(cratehp * 0.30);
				spawned.life = crateType == ItemID.DungeonFishingCrate ? (int)(cratehp * 0.4) : (int)(cratehp * 0.30);
				spawned.damage = (crateType == ItemID.DungeonFishingCrate ? (int)(80) : (int)(80)) * ((int)npc.damage / boss.defaultdamage);
				return;
			}
			if (crateType == ItemID.HallowedFishingCrate || crateType == Cratrosity.EvilCrateType)
			{
				spawned.aiStyle = crateType == ItemID.HallowedFishingCrate ? 63 : 62;
				spawned.knockBackResist = crateType == ItemID.HallowedFishingCrate ? 0.92f : 0.96f;
				spawned.lifeMax = crateType == ItemID.HallowedFishingCrate ? (int)(cratehp * 0.60) : (int)(cratehp * 0.45);
				spawned.life = crateType == ItemID.HallowedFishingCrate ? (int)(cratehp * 0.60) : (int)(cratehp * 0.45);
				spawned.damage = (crateType == ItemID.HallowedFishingCrate ? (int)(100) : (int)(40)) * ((int)npc.damage / boss.defaultdamage);
				return;
			}
			if (crateType == ModContent.ItemType<HavocGear.Items.DankCrate>())
			{
				spawned.aiStyle = 49;
				spawned.knockBackResist = 0.85f;
				spawned.lifeMax = (int)(cratehp * 0.65);
				spawned.life = (int)(cratehp * 0.65);
				spawned.damage = (int)(45 * (npc.damage / boss.defaultdamage));
				return;
			}
			//if (layer == 0)
			//{
				spawned.knockBackResist = crateType == ItemID.HallowedFishingCrate ? 0.92f : 0.96f;
				spawned.lifeMax = (int)(cratehp * 0.65);
				spawned.life = (int)(cratehp * 0.65);
			//}
		}

		public void Draw(SpriteBatch spriteBatch,Color drawColor)
        {
			Texture2D texture = Main.itemTexture[crateType];
			Vector2 drawPos = crateVector - Main.screenPosition;
			spriteBatch.Draw(texture, drawPos, null, drawColor, crateAngle, texture.Size() / 2f, new Vector2(1, 1), SpriteEffects.None, 0f);
		}

	}



	[AutoloadBossHead]
	public class Cratrosity : ModNPC, ISGABoss
	{
		public string Trophy() => GetType() == typeof(Cratrosity) ? "CratrosityTrophy" : "CratrogeddonTrophy";
		public bool Chance() => Main.rand.Next(0, 10) == 0;

		public string RelicName() => GetType() == typeof(Cratrosity) ? "Cratrosity" : "Cratrogeddon";
		public void NoHitDrops()
		{
			Item.NewItem(NPC.position,NPC.Hitbox.Size(),ModContent.ItemType<Items.Accessories.AvariceRing>());
		}
		public string MasterPet() => null;
		public bool PetChance() => false;

		public Vector2 offsetype = new Vector2(0, 0);
		public int phase = 5;
		public int defaultdamage = 60;
		public int themode = 0;
		public float compressvar = 1;
		public float compressvargoal = 1;
		public int doCharge = 0;
		public bool init = false;
		public static int EvilCrateType => WorldGen.crimson ? ItemID.CrimsonFishingCrate : ItemID.CorruptFishingCrate;
		Vector2 theclostestcrate = new Vector2(0, 0);
		public int[] Cratesperlayer = new int[5] { 0, 0, 0, 0, 0};
		public List<List<CratrosityInstancedCrate>> cratesPerRing = new List<List<CratrosityInstancedCrate>>();
		public int postmoonlord = 0;
		public float nonaispin = 0f;
		public bool firstTimeSetup = true;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cratrosity");
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
		}

		public override bool Autoload(ref string name)
		{
			return base.Autoload(ref name);
		}

		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 50;
			NPC.defense = 50;
			NPC.lifeMax = 10000;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath37;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			NPC.boss = true;
			//music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Evoland 2 OST - Track 46 (Ceres Battle)");
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			theclostestcrate = NPC.Center;
			music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/SGAmod_Cratrosity");
			NPC.value = Item.buyPrice(0, 10, 0, 0);
		}

		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.GoldenCrate; }
		}

		public override string BossHeadTexture => "Terraria/Item_" + ItemID.GoldenCrate;


		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.6f);
		}

		public override bool CheckActive()
		{
			return false;
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(doCharge);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			doCharge = reader.ReadInt32();
        }

        public virtual void FalseDeath(int phase)
		{
			//nothing here

		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return NPC.localAI[0] > 0 && doCharge<1;
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("TerrariacoCrateKey").Type);
			if (Main.rand.Next(7) == 0)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Armors.Vanity.CratrosityMask>());
			}
			Achivements.SGAAchivements.UnlockAchivement("Cratrosity", Main.LocalPlayer);
			if (SGAWorld.downedCratrosity == false)
			{
				Idglib.Chat("The hungry video game industry has been tamed! New items are available for buying", 244, 179, 66);
			}
			SGAWorld.downedCratrosity = true;
		}


		public override bool CheckDead()
		{
			if (NPC.life < 1 && phase > 0)
			{
				NPC.life = NPC.lifeMax;
				phase -= 1;
				if (phase < 4 && GetType() == typeof(Cratrosity))
                {
					NPC.localAI[3] = 1;
					NPC.frameCounter = 1;
                }
				Cratrosity origin = npc.ModNPC as Cratrosity;
				CrateRelease(phase);
				FalseDeath(phase);
				if (origin.postmoonlord > 0)
				{
					//do stuff here
				}
				NPC.active = true;
				return false;
			}
			else { return true; }
		}

        public override bool? CanBeHitByItem(Player player, Item item)
        {
			return false;
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
			return false;
		}

        public override bool PreAI()
        {
			bool newoneneeded = true;
			if (NPC.CountNPCS(ModContent.NPCType<CratrosityHitBox>()) > 0)
			{
				foreach (NPC npc2 in Main.npc.Where(testby => testby.active && testby.type == ModContent.NPCType<CratrosityHitBox>()))
				{
					if (npc2.realLife == NPC.whoAmI && npc2.type == ModContent.NPCType<CratrosityHitBox>())
					{
						newoneneeded = false;
						break;
					}
				}
			}

			if (newoneneeded)
			{
				//npc.dontTakeDamage = true;
				int npc2 = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<CratrosityHitBox>());
				Main.npc[npc2].realLife = NPC.whoAmI;
				Main.npc[npc2].netUpdate = true;
				//Main.NewText("new one");
				init = true;
			}

			return true;
        }

		public Player Target => Main.player[NPC.target];

        public override void AI()
		{

			doCharge -= 1;
			Player P = Target;
			NPC.localAI[0] -= 1;
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(false);
				P = Main.player[NPC.target];
				if (!P.active || P.dead)
				{
					float speed = ((-10f));
					NPC.velocity = new Vector2(NPC.velocity.X, NPC.velocity.Y + speed);
					NPC.active = false;
				}

			}
			else
			{

				if (NPC.localAI[3] > 0)
				{
					if (NPC.localAI[3] < 100)
					{
						NPC.dontTakeDamage = true;
						NPC.velocity *= 0.85f;
						NPC.localAI[3] += 1;
						if (phase > 2 && NPC.localAI[3]>=0)
						{
							NPC.frameCounter = 16f * (1f - ((NPC.localAI[3]) / 100f));
                        }
                        else
                        {
							NPC.frameCounter = 19 + (NPC.localAI[3]/3f) % 3;
						}

						if (NPC.localAI[3] == 50)
						{
							NPC.extraValue += Item.buyPrice(0, 1, 0, 0);
							NPC.moneyPing(NPC.Center);

						}

						goto gohere;
					}
					else
					{
						
						if (NPC.localAI[3] < 200)
						{
							//if (npc.frameCounter < 5)
							//{
							NPC.frameCounter += 0.25f;
							NPC.frameCounter %= 5;
							//if (npc.frameCounter >= 5 && npc.frameCounter < 8)
							//	npc.frameCounter = 5 - npc.frameCounter;
							//}
						}
						
						if (NPC.localAI[3] >= 200 && NPC.localAI[3] < 400)
						{
							NPC.localAI[3]++;
							//if (npc.frameCounter < 5)
							//{
							if (NPC.frameCounter < 18)
								NPC.frameCounter = 18;

							if (NPC.frameCounter < 21)
								NPC.frameCounter += 0.25f;

							if (NPC.frameCounter > 18 && NPC.localAI[3]>215)
								NPC.frameCounter -= 0.25f;

							if (NPC.localAI[3] > 230)
								NPC.localAI[3] = 100;
							//}
						}
						if (NPC.localAI[3] >= 400)
						{
							NPC.localAI[3]++;
							NPC.frameCounter = 21f * (((NPC.localAI[3]-400) / 100f));
							if (NPC.localAI[3] > 460)
								NPC.localAI[3] = 201;
						}
						
						

					}
					NPC.dontTakeDamage = false;
				}


				NPC.ai[0] += 1f;
				Vector2 gohere = new Vector2(P.Center.X, P.Center.Y - 220) + offsetype;
				float thespeed = 0.01f;
				float friction = 0.98f - phase * 0.04f;
				float friction2 = 0.99f - phase * 0.0075f;
				themode -= 1;
				NPC.ai[1] += Main.rand.Next(0, 5);
				if (NPC.ai[1] % 2000 > 850)
				{
					if (System.Math.Abs(NPC.ai[2]) < 300)
					{
						compressvargoal = 1;
						int theammount = (NPC.ai[2] > 0 ? 1 : -1) * (offsetype.X > 0 ? 1 : -1);
						if (NPC.ai[1] % 2000 < 1100)
						{
							gohere = new Vector2(P.Center.X + (theammount * 800), P.Center.Y - 220);
							NPC.velocity = (NPC.velocity + ((gohere - NPC.Center) * thespeed)) * 0.98f;
						}
						else
						{
							if (phase < 1)
								theclostestcrate = Vector2.Zero;

							NPC.velocity = new Vector2(-theammount * ((GetType() == typeof(Cratrogeddon)) ? 15 : 10), 0);
							if (NPC.ai[0] % 15 == 0)
							{
								List<Projectile> itz = Idglib.Shattershots(NPC.Center, P.Center + new Vector2(0, P.Center.Y > NPC.Center.Y ? 600 : -600), new Vector2(0, 0), ModContent.ProjectileType<GlowingCopperCoin>(), (int)(NPC.damage * (10.00 / defaultdamage)), 10, 0, 1, true, 0, true, 100);
							}
							if (NPC.ai[0] % 40 == 0)
							{
								List<Projectile> itz = Idglib.Shattershots(theclostestcrate, P.Center, new Vector2(0, 0), ModContent.ProjectileType<GlowingGoldCoin>(), (int)(NPC.damage * (30.00 / defaultdamage)), 10, 0, 1, true, 0, true, 200);
							}
							if (((NPC.ai[0] + 20) % 40 == 0) && Main.expertMode)
							{
								List<Projectile> itz = Idglib.Shattershots(theclostestcrate, P.Center + new Vector2(0, P.Center.Y > theclostestcrate.Y ? 600 : -600), new Vector2(0, 0), ModContent.ProjectileType<GlowingSilverCoin>(), (int)(NPC.damage * (20.00 / defaultdamage)), 10, 0, 1, true, 0, true, 200);
								SGAprojectile modeproj = itz[0].GetGlobalProjectile<SGAprojectile>();
								//modeproj.splittingcoins = true;
								//modeproj.splithere = P.Center;
							}
							if (phase < 4)
							{
								if (NPC.ai[0] % 8 == 0)
								{
									Idglib.Shattershots(NPC.Center, NPC.Center + new Vector2(-NPC.velocity.X, 0), new Vector2(0, 0), ModContent.ProjectileType<GlowingSilverCoin>(), (int)(NPC.damage * (20.00 / defaultdamage)), 25, 0, 1, true, 0, false, 40);
								}
							}
							themode = 300;
							if (offsetype.X >= 0)
							{
								NPC.localAI[0] = 5;
								if (NPC.Center.X < P.Center.X - 700)
								{
									NPC.ai[2] = Math.Abs(NPC.ai[2]);
								}
								if (NPC.Center.X > P.Center.X + 700)
								{
									NPC.ai[2] = -Math.Abs(NPC.ai[2]);
								}
							}
							else
							{
								if (NPC.Center.X < P.Center.X - 700)
								{
									NPC.ai[2] = -Math.Abs(NPC.ai[2]);
								}
								if (NPC.Center.X > P.Center.X + 700)
								{
									NPC.ai[2] = Math.Abs(NPC.ai[2]);
								}
							}
							//npc.ai[1]=1600+(2000-1600);
							//}
						}

					}
					else
					{
						Vector2 gogo = P.Center - NPC.Center; gogo.Normalize(); gogo = gogo * (8 - phase * 1);
						if (GetType() == typeof(Cratrogeddon))
							gogo *= 1.25f;
						NPC.velocity = gogo;
						compressvargoal = 2;
						NPC.localAI[0] = 5;
					}
				}
				else
				{
					NPC.ai[2] = Main.rand.Next(-600, 600);
					if (NPC.ai[0] % 600 < 350)
					{
						NPC.velocity = (NPC.velocity + (((gohere) - NPC.Center) * thespeed)) * friction;
						compressvargoal = 1;

						switch (phase)
						{
							case 5:
								{
									if (NPC.ai[0] % 30 == 0)
									{
										Idglib.Shattershots(NPC.Center, P.position, new Vector2(P.width, P.height), ModContent.ProjectileType<GlowingCopperCoin>(), (int)(NPC.damage * (10f / (float)defaultdamage)), 10, 0, 1, true, 0, true, 150);
									}
									break;
								}
							case 4:
								{
									if (NPC.ai[0] % 10 == 0)
									{
										Idglib.Shattershots(NPC.Center, P.position, new Vector2(P.width, P.height), ModContent.ProjectileType<GlowingSilverCoin>(), (int)(NPC.damage * (20f / (float)defaultdamage)), 14, 0, 1, true, 0, true, 100);
									}
									break;
								}
							case 3:
								{
									if (NPC.ai[0] % 3 == 0 && NPC.ai[0] % 50 > 38)
									{
										Idglib.Shattershots(NPC.Center, P.position, new Vector2(P.width, P.height), ModContent.ProjectileType<GlowingGoldCoin>(), (int)(NPC.damage * (30f / (float)defaultdamage)), 16, 0, 1, true, 0, true, 90);
									}
									if (NPC.ai[0] % 8 == 0 && Main.expertMode)
									{
										List<Projectile> itz = Idglib.Shattershots(NPC.Center, NPC.Center + new Vector2(0, -5), new Vector2(0, 0), ModContent.ProjectileType<GlowingSilverCoin>(), (int)(NPC.damage * (20f / (float)defaultdamage)), 7, 360, 2, true, NPC.ai[0] / 20, true, 300);
									}
									break;
								}
							case 2:
								{

									if (NPC.ai[0] % 5 == 0)
									{

										Idglib.Shattershots(NPC.Center, NPC.Center + new Vector2((P.velocity.X * 4f) + Main.rand.NextFloat(-24f, 24f), -96f), Vector2.Zero, ModContent.ProjectileType<GlowingGoldCoinHoming>(), (int)(NPC.damage * (30f / (float)defaultdamage)), Main.rand.NextFloat(6f, 10f), 0, 1, true, 0, true, 600);
									}
								}
								break;

						}

						doCharge = -100000;
					}
					else
					{
						NPC.localAI[0] = 5;
						compressvargoal = 0.4f;
						Vector2 gogo = P.Center - NPC.Center; gogo.Normalize(); gogo = gogo * (30 - phase * 2);

						if (doCharge < -1000)
						{
							doCharge = 120;
						}

						if (doCharge > 0)
						{
							NPC.ai[0] -= 1;
							if (doCharge == 60)
							{
								if (NPC.localAI[3]>0)
								NPC.localAI[3] = 400;


								SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_DarkMageAttack, NPC.Center);
								if (sound != null)
								{
									sound.Pitch = -0.25f;
								}

								for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
								{
									Vector2 offset = f.ToRotationVector2();
									int dust = Dust.NewDust(NPC.Center + (offset * 32f), 0, 0, DustID.GoldFlame);
									Main.dust[dust].scale = 1.5f;
									Main.dust[dust].noGravity = true;
									Main.dust[dust].velocity = f.ToRotationVector2() * 4f;
								}
							}

							NPC.velocity /= 1.5f;
						}
						else
						{

							if (GetType() == typeof(Cratrogeddon))
								gogo *= 0.4f;

							float tiev = (GetType() == typeof(Cratrogeddon)) ? NPC.ai[0] % (50 + (phase * 3)) : NPC.ai[0] % (25 + (phase * 10));

							if (tiev < (GetType() == typeof(Cratrogeddon) ? 10 : 1))
							{
								NPC.velocity = (NPC.velocity + gogo);

								if (NPC.localAI[3] > 0)
									NPC.localAI[3] = 201;

								if (NPC.velocity.Length() > 30)
								{
									NPC.velocity.Normalize();
									NPC.velocity *= 30f;
								}

							}
							NPC.velocity = NPC.velocity * friction2;
						}
					}
				}
			}

			gohere:

			NPC.defense = (int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.WoodenCrate.ToString()).Type)) * 5 +
			(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.IronCrate.ToString()).Type)) * 6 +
			(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.GoldenCrate.ToString()).Type)) * 6 +
			(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.DungeonFishingCrate.ToString()).Type)) * 10 +
			(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.JungleFishingCrate.ToString()).Type)) * 10 +
			(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + EvilCrateType.ToString()).Type)) * 10 +
			(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.HallowedFishingCrate.ToString()).Type)) * 10 +
			NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrateDankCrate").Type) * 10 +
			(int)(NPC.CountNPCS(Mod.Find<ModNPC>("CratrosityCrate" + ItemID.FloatingIslandFishingCrate.ToString()).Type)) * (30);
			NPC.defense *= Main.expertMode ? 4 : 2;
			NPC.defense += Main.expertMode ? 20 : 0;
			OrderOfTheCrates(P);



		}

		public virtual void OrderOfTheCrates(Player P)
		{
			if (firstTimeSetup)
            {
				for (int layer = 0; layer < phase; layer++)
				{
					Cratesperlayer[layer] = 4 + (layer * 4);
					List<CratrosityInstancedCrate> cratesInThisLayer = new List<CratrosityInstancedCrate>();

					for (int i = 0; i < Cratesperlayer[layer]; i = i + 1)
					{
						int crateType = ItemID.WoodenCrate;
						if (layer == 3)
						{
							crateType = ItemID.IronCrate;
							if (NPC.ai[3] > 300000 && i % 2 == 0)
							{
								crateType = ModContent.ItemType<HavocGear.Items.DankCrate>();
							}
							else if (NPC.ai[3] > 100000 && i % 2 == 0)
							{
								crateType = ItemID.HallowedFishingCrate;
							}
							else
							{
								if (NPC.ai[3] < -100000 && i % 2 == 0)
								{
									crateType = EvilCrateType;
								}
							}
						}
						if (layer == 2)
						{
							crateType = (i % 2 == 0) ? ItemID.JungleFishingCrate : ItemID.DungeonFishingCrate;
						}
						if (layer == 1)
						{
							crateType = (i % 2 == 0) ? ItemID.HallowedFishingCrate : (EvilCrateType);
						}
						if (layer == 0)
						{
							crateType = ItemID.FloatingIslandFishingCrate;
						}
						cratesInThisLayer.Add(new CratrosityInstancedCrate(this, crateType, 8, layer));
					}
					cratesPerRing.Add(cratesInThisLayer);

				}
				firstTimeSetup = false;
			}


			nonaispin = nonaispin + 1f;
			compressvar += (compressvargoal - compressvar) / 20;
			float themaxdist = 99999f;
			for (int layer = 0; layer < phase; layer++)
			{
				for (int index = 0; index < cratesPerRing[layer].Count; index++)
				{
					CratrosityInstancedCrate crate = cratesPerRing[layer][index];

					if (!crate.Update())
						continue;

					crate.crateAngle = (layer % 2 == 0 ? 1 : -1) * ((nonaispin * 0.01f) * (1f + layer / 3f)) + MathHelper.TwoPi * ((index / (float)cratesPerRing[layer].Count));
					crate.crateDist = compressvar * 20f + ((float)layer * 20f) * compressvar;
					//Cratesangle[a, i] = (a % 2 == 0 ? 1 : -1) * ((nonaispin * 0.01f) * (1f + a / 3f)) + 2.0 * Math.PI * ((i / (double)Cratesperlayer[a]));
					//Cratesdist[a, i] = compressvar * 20f + ((float)a * 20f) * compressvar;

					float theexpand = 0f;

					if (themode > 0)
					{
						theexpand = (((index / 1f) * (layer + 1f))) * (themode / 30f);
					}

					crate.crateVector += ((crate.crateDist * (new Vector2((float)Math.Cos(crate.crateAngle), (float)Math.Sin(crate.crateAngle))) + NPC.Center) - crate.crateVector) / (theexpand + (Math.Max((((compressvar) - 1) * (2 + (layer * 1))), 1)));
					//if (index)
					//crate.crateVector += Main.rand.NextVector2Circular(32, 32);

					float sinner = NPC.ai[0] + ((float)(index * 5) + (layer * 14));
					float sinner2 = (float)(10f + (Math.Sin(sinner / 30f) * 7f));

					if (compressvar > 1.01)
					{
						int[] projtype = { ModContent.ProjectileType<GlowingPlatinumCoin>(), ModContent.ProjectileType<GlowingPlatinumCoin>(), ModContent.ProjectileType<GlowingPlatinumCoin>(), ModContent.ProjectileType<GlowingGoldCoin>(), ModContent.ProjectileType<GlowingSilverCoin>(), ModContent.ProjectileType<GlowingCopperCoin>() };
						int[] projdamage = { 60, 50, 40, 30, 20, 10 };
						float[] projspeed = { 1f, 1f, 1f, 9f, 8f, 7f };
						if (layer == phase - 1)
						{
							if (sinner2 < 4 && (NPC.ai[0] + (index * 4)) % 30 == 0)
							{
								List<Projectile> itz = Idglib.Shattershots(crate.crateVector, P.position, new Vector2(P.width, P.height), projtype[layer + 1], (int)((double)NPC.damage * ((double)projdamage[layer + 1] / (double)defaultdamage)), projspeed[layer + 1], 0, 1, true, 0, false, 110);
								if (projtype[layer + 1] == ModContent.ProjectileType<GlowingPlatinumCoin>()) { itz[0].aiStyle = 18; IdgProjectile.AddOnHitBuff(itz[0].whoAmI, BuffID.ShadowFlame, 60 * 10); }
							}
						}

						crate.crateVector += ((P.Center - crate.crateVector) / (sinner2)) * ((Math.Max(compressvar - 1, 0) * 1));
					}
					float dist = (P.Center - crate.crateVector).Length();
					if (dist < themaxdist)
					{
						themaxdist = dist;
						theclostestcrate = crate.crateVector;
					}
				}
			}

		}

		/*public override bool CanHitPlayer(Player ply, ref int cooldownSlot){
			return true;
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
		return true;
		}*/

		public virtual void CrateRelease(int layer)
		{
			float cratehp = NPC.lifeMax * 0.50f;
			List<CratrosityInstancedCrate> cratesInThisLayer = cratesPerRing[layer];

			for (int i = 0; i < cratesInThisLayer.Count; i = i + 1)
			{
				cratesInThisLayer[i].Release(cratehp);
			}
			NPC.lifeMax = (int)(NPC.lifeMax * 0.75f);
			NPC.life = NPC.lifeMax;
			cratesPerRing.RemoveAt(layer);
		}

		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(Mod.Find<ModBuff>("MoneyMismanagement").Type, 250, true);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (firstTimeSetup)
				return false;

			for (int layer = 0; layer < phase; layer++)
			{
				List<CratrosityInstancedCrate> cratesInThisLayer = cratesPerRing[layer];
				for (int i = 0; i < cratesInThisLayer.Count; i++)
				{
					Point there = (cratesInThisLayer[i].crateVector / 16).ToPoint();
					cratesInThisLayer[i].Draw(spriteBatch,Lighting.GetColor(there.X, there.Y,lightColor));
					//Color glowingcolors1 = Main.hslToRgb((float)(npc.ai[0] / 30) % 1, 1f, 0.9f);
					//Texture2D texture = mod.GetTexture("Items/IronSafe");
					//Texture2D texture = Main.itemTexture[Cratestype[a, i]];
					//Main.getTexture("Terraria/Item_" + ItemID.IronCrate);
					//Vector2 drawPos = npc.Center-Main.screenPosition;
					//Vector2 drawPos = Cratesvector[a, i] - Main.screenPosition;
					//spriteBatch.Draw(texture, drawPos, null, lightColor, (float)Cratesangle[a, i], new Vector2(16, 16), new Vector2(1, 1), SpriteEffects.None, 0f);
				}
			}

			if (GetType() != typeof(Cratrogeddon) && (NPC.localAI[3] > 0 || phase<3))
			{
				Texture2D trueFormTexture = Mod.Assets.Request<Texture2D>("NPCs/Cratrosity/Cratosity").Value;
				int width = trueFormTexture.Width;
				int height = trueFormTexture.Height;
				int frames = 22;
				int framesHeight = height / frames;
				int frame = (int)NPC.frameCounter;

				float direction = Math.Sign(NPC.velocity.X);
				NPC.frame = new Rectangle(0, frame * framesHeight, width/2, framesHeight);
				Rectangle frame2 = new Rectangle(width/2, frame * framesHeight, width / 2, framesHeight);

				Main.spriteBatch.Draw(trueFormTexture, NPC.Center - Main.screenPosition, NPC.frame, lightColor, (float)Math.Pow(Math.Abs(NPC.velocity.X/60f),0.75f)* direction, NPC.frame.Size()/ 2f, NPC.scale, default, 0);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				ArmorShaderData stardustsshader3 = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye);
				DrawData value28 = new DrawData(trueFormTexture, new Vector2(0, 0), frame2, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
						stardustsshader3.UseColor(Color.Blue.MultiplyRGB(lightColor).ToVector3());
				stardustsshader3.UseOpacity(0.5f);
				stardustsshader3.UseSaturation(0f);
				stardustsshader3.UseSecondaryColor(Color.Aqua);
				stardustsshader3.Apply(null, new DrawData?(value28));

						Main.spriteBatch.Draw(trueFormTexture, NPC.Center - Main.screenPosition, frame2, Color.White, (float)Math.Pow(Math.Abs(NPC.velocity.X / 60f), 0.75f) * direction, NPC.frame.Size() / 2f, NPC.scale, default, 0);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				return false;
			}

			Texture2D mainTex = GetType() == typeof(Cratrogeddon) ? Mod.Assets.Request<Texture2D>("NPCs/Cratrosity/TitanCrate") .Value: Main.itemTexture[ItemID.GoldenCrate];
			Main.spriteBatch.Draw(mainTex, NPC.Center - Main.screenPosition, null, lightColor, NPC.rotation, mainTex.Size() / 2f, NPC.scale, default, 0);

			return false;
		}


	}

	public class CratrosityHitBox : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cratrosity");
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
		}

		public override bool Autoload(ref string name)
		{
			return base.Autoload(ref name);
		}

		public override void SetDefaults()
		{
			NPC.width = 160;
			NPC.height = 160;
			NPC.damage = 0;
			NPC.defense = 50;
			NPC.lifeMax = 5000000;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath37;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			NPC.hide = true;
			//music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Evoland 2 OST - Track 46 (Ceres Battle)");
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
		}
		public override string Texture
		{
			get { return "Terraria/Coin_" + 0; }
		}

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
			return false;
        }

        public override void AI()
        {
			if (NPC.realLife >= 0)
			{
				if (NPC.realLife < 1)
                {
					//npc.StrikeNPCNoInteraction(100000, 0, 0, noEffect: true);
					return;
				}
				NPC.dontTakeDamage = Main.npc[NPC.realLife].dontTakeDamage;
				NPC.Center = Main.npc[NPC.realLife].Center;
				NPC.defense = Main.npc[NPC.realLife].defense;
				ModNPC modnpc = Main.npc[NPC.realLife].ModNPC;
				if (modnpc != null)
				{
					if (!Main.npc[NPC.realLife].ModNPC.GetType().IsSubclassOf(typeof(Cratrosity)))
					{
						//npc.StrikeNPCNoInteraction(100000, 0, 0, noEffect: true);
						return;
					}
					else
					{
						Cratrosity crate = Main.npc[NPC.realLife].ModNPC as Cratrosity;
						NPC.width = 80 + (crate.phase * 16);
						NPC.height = 80 + (crate.phase * 16);
					}
				}
			}
			//Main.NewText("exists "+ npc.Center);
		}


    }
		public class GlowingCopperCoin : ModProjectile,IDrawAdditive
	{
		protected virtual int FakeID2 => ProjectileID.CopperCoin;
		protected virtual Color GlowColor => new Color(184, 115, 51);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Copper Coin");
		}

		public override string Texture
		{
			get { return "Terraria/Coin_" + 0; }
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(FakeID2);
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
		}

        public override bool PreKill(int timeLeft)
		{
			Projectile.type = FakeID2;
			return true;
		}

		public override void AI()
		{
			Projectile.localAI[0] += 1;
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}

		public void DrawAdditive(SpriteBatch spriteBatch)
		{

			Texture2D tex = Main.projectileTexture[Projectile.type];
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			int texHeight = tex.Height / 8;
			Vector2 offset = new Vector2(tex.Width, texHeight / 8) / 2f;
			int index = (int)(Projectile.localAI[0] / 6f) % 8;

			spriteBatch.Draw(tex, drawPos, new Rectangle(0, texHeight * index, tex.Width, texHeight), Color.Lerp(GlowColor, Color.White, 0.50f)*0.50f, Projectile.rotation, offset, Projectile.scale+0.25f, SpriteEffects.None, 0f);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Texture2D tex = Main.projectileTexture[Projectile.type];
			Vector2 drawPos = Projectile.Center-Main.screenPosition;
			int texHeight = tex.Height / 8;
			Vector2 offset = new Vector2(tex.Width, texHeight / 8) / 2f;
			int index = (int)(Projectile.localAI[0]/6f)%8;

			spriteBatch.Draw(tex, drawPos, new Rectangle(0, texHeight*index, tex.Width, texHeight), Color.Lerp(lightColor,Color.White,0.50f), Projectile.rotation, offset, Projectile.scale, SpriteEffects.None, 0f);
			Texture2D tex2 = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];
			spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, GlowColor * 0.75f, Projectile.rotation+MathHelper.Pi, tex2.Size() / 2f, Projectile.scale / 1.5f, default, 0);
			return false;
        }
    }
	public class GlowingSilverCoin : GlowingCopperCoin, IDrawAdditive
	{
		protected override int FakeID2 => ProjectileID.SilverCoin;
		protected override Color GlowColor => Color.Silver;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Silver Coin");
		}
		public override string Texture
		{
			get { return "Terraria/Coin_" + 1; }
		}
	}
	public class GlowingGoldCoin : GlowingCopperCoin, IDrawAdditive
	{
		protected override int FakeID2 => ProjectileID.GoldCoin;
		protected override Color GlowColor => Color.Gold;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Gold Coin");
		}
		public override string Texture
		{
			get { return "Terraria/Coin_" + 2; }
		}
	}
	public class GlowingGoldCoinHoming : GlowingGoldCoin, IDrawAdditive
	{
		public override void AI()
        {
			base.AI();
			if (Projectile.localAI[0] >= 0f)
				Projectile.localAI[0] += 1;
			if (Projectile.ai[1] < 1000)
            {
				Projectile.ai[0] = Main.rand.Next(60, 200);
				Projectile.ai[1] = 1000+(int)Player.FindClosest(Projectile.Center, 0, 0);
				Projectile.netUpdate = true;

			}
            else
            {
				Player player = Main.player[(int)Projectile.ai[1]-1000];
				if (Projectile.localAI[0] > Projectile.ai[0] && Projectile.localAI[0] >= 0)
                {
					Vector2 dotter = player.Center - Projectile.Center;
					float speed = Projectile.velocity.Length();
					Projectile.velocity = (Projectile.velocity.ToRotation().AngleLerp(dotter.ToRotation(), 0.05f)).ToRotationVector2()*speed;
					if (Vector2.Dot(Vector2.Normalize(dotter), Vector2.Normalize(Projectile.velocity)) > 0.650f)
                    {
						Projectile.localAI[0] = -10000;
					}

                }

            }

        }
	}
	public class GlowingPlatinumCoin : GlowingCopperCoin, IDrawAdditive
	{
		protected override int FakeID2 => ProjectileID.PlatinumCoin;
		protected override Color GlowColor => new Color(229, 228, 226);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Platinum Coin");
		}
		public override string Texture
		{
			get { return "Terraria/Coin_" + 3; }
		}
	}
}