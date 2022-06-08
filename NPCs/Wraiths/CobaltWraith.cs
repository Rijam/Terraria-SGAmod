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

namespace SGAmod.NPCs.Wraiths
{
	public class CobaltArmorChainmail : CopperArmorChainmail
	{
		public int armortype = ItemID.CobaltBreastplate;
		public int mode = 0;

		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 32;
			NPC.damage = 0;
			NPC.defense = 5;
			NPC.lifeMax = 500;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath7;
			NPC.value = 0f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			AIType = -1;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			CopperArmorPiece myself = npc.ModNPC as CopperArmorPiece;
			myself.friction = myself.friction / 8;
			myself.speed = 0.54f;
			NPC.buffImmune[BuffID.Daybreak] = true;

		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(0, 5) == 0)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("CobaltWraithNotch").Type);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Breastplate");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public virtual void CobaltFloat(NPC myowner)
		{
			NPC.velocity = NPC.velocity + (myowner.Center + new Vector2((float)NPC.ai[1], (float)NPC.ai[2]) - NPC.Center) * (speed);
			NPC.velocity = NPC.velocity * 0.5f;
			NPC.rotation = (float)NPC.velocity.X * 0.1f;
			NPC.timeLeft = 999;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + armortype; }
		}
		public override void AI()
		{
			CopperArmorPiece myself = npc.ModNPC as CopperArmorPiece;
			int npctype = Mod.Find<ModNPC>(myself.attachedType).Type;
			NPC myowner = Main.npc[myself.attachedID];
			if (myowner.active == false)
			{
				myself.ArmorMalfunction();
			}
			else
			{
				CobaltFloat(myowner);
			}
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Main.expertMode)
			{
				double damagemul = 1.0;
				if (projectile.penetrate > 1)
					damagemul = 0.8;
				if (projectile.penetrate > 2 || projectile.penetrate < 0)
					damagemul = 0.6;
				base.OnHitByProjectile(projectile, (int)(damage * damagemul), knockback, crit);
			}

		}

	}

	public class CobaltArmorHelmet : CobaltArmorChainmail
	{
		public int armortype = ItemID.CobaltHelmet;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helmet");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + armortype; }
		}

	}

	public class CobaltArmorGreaves : CobaltArmorChainmail
	{
		public int armortype = ItemID.CobaltLeggings;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leggings");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + armortype; }
		}

	}

	public class CobaltBrickArmor : CobaltArmorChainmail
	{
		public int armortype = ItemID.CobaltBrickWall;

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.width = 48;
			NPC.height = 48;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath7;
			CopperArmorPiece myself = npc.ModNPC as CopperArmorPiece;
			myself.friction = 0.95f;
			myself.speed = 0.04f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leggings");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + armortype; }
		}

	}

	public class CobaltBrickArmorNexus : CobaltArmorChainmail
	{
		public int armortype = ItemID.CobaltBrick;
		public int madeturrets = 0;
		public int laserblast = -1000;

		public override void SetDefaults()
		{
			base.SetDefaults();
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leggings");
			Main.npcFrameCount[NPC.type] = 1;

		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(mode);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			mode = reader.ReadInt32();
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + armortype; }
		}
		public override void CobaltFloat(NPC myowner)
		{
			if (mode == 0)
			{
				base.CobaltFloat(myowner);
				return;
			}
			NPC ownerz = Main.npc[NPC.FindFirstNPC(Mod.Find<ModNPC>("CobaltWraith").Type)];
			Player P = Main.player[ownerz.target];

			NPC.velocity = NPC.velocity + (new Vector2(myowner.Center.X+(mode * 48f), P.Center.Y-480) - NPC.Center) * (speed);
			NPC.velocity /= 2f;
			NPC.rotation += Math.Sign(mode)*0.10f;
			NPC.timeLeft = 999;
		}
		public override void AI()
		{
			base.AI();
			madeturrets = madeturrets + 1;
			if (laserblast == -1000)
				laserblast = (int)Main.rand.Next(0, 50) - 300;
			int findthem = NPC.FindFirstNPC(Mod.Find<ModNPC>("CobaltWraith").Type);
			if (findthem >=0 && findthem < Main.maxNPCs)
			{
				NPC ownerz = Main.npc[findthem];
				if ((ownerz.ModNPC as CobaltWraith).raged == true)
				{
					laserblast = laserblast + 1;
					Player P = Main.player[ownerz.target];
					if (laserblast % 20 == 0 && laserblast % 400 > 200 && laserblast > 0 && P != null && Main.expertMode && (Main.netMode != 1))
					{
						if (mode == 0)
						{
							//if (laserblast % 400 > 300)
							//Idglib.Shattershots(npc.Center, npc.Center + new Vector2(Math.Sign(P.Center.X - npc.Center.X) * 8f, 0), Vector2.Zero, ProjectileID.WaterBolt, 20, 30, 0, 1, true, 0, true, 100);
						}
						else
						{
							Idglib.Shattershots(NPC.Center, NPC.Center + Vector2.UnitY, Vector2.Zero, ProjectileID.WaterBolt, 20, 30, 0, 1, true, 0, true, 100);
						}
					}
				}
			}
			if (madeturrets == 5)
			{
				//madeturrets=true;
				int nexus = NPC.NewNPC((int)NPC.position.X, (int)NPC.position.Y - 24, Mod.Find<ModNPC>(Main.rand.Next(0, 100) < 50 && mode == 0 ? "CobaltArmorBow" : "CobaltChainSawPiece").Type);
				NPC armpeice = Main.npc[nexus];
				CopperArmorPiece newguy3 = armpeice.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI;

				armpeice.lifeMax = (int)(NPC.lifeMax * 0.75f); armpeice.ai[1] = -16; armpeice.ai[1] = -0f; armpeice.life = (int)(NPC.lifeMax * (0.75f)); armpeice.knockBackResist = 1f; armpeice.netUpdate = true;
			}
		}

	}


	public class CobaltArmorBow : CobaltArmorChainmail
	{
		public int armortype = ItemID.CobaltRepeater;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Repeater");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + armortype; }
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 0;
			NPC.defense = 5;
			NPC.lifeMax = 500;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath7;
			NPC.value = 0f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			AIType = -1;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
		}
		public override void AI()
		{
			CopperArmorPiece myself = npc.ModNPC as CopperArmorPiece;
			int npctype = Mod.Find<ModNPC>(myself.attachedType).Type;
			NPC myowner = Main.npc[myself.attachedID];
			if (myowner.active == false)
			{
				myself.ArmorMalfunction();
			}
			else
			{
				Player P = Main.player[NPC.target];
				if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				{
					NPC.TargetClosest(false);
					P = Main.player[NPC.target];
					if (!P.active || P.dead)
					{
						NPC.active = false;
						Main.npc[(int)NPC.ai[1]].active = false;
					}
				}
				else
				{
					NPC.ai[0] += 1;
					Vector2 itt = (myowner.Center - NPC.Center + new Vector2(NPC.ai[1] * NPC.spriteDirection, NPC.ai[2]));
					if (NPC.ai[0] % 1500 > 1250)
					{
						itt = (P.position - NPC.position + new Vector2(3f * NPC.ai[1] * NPC.spriteDirection, NPC.ai[2] * 2f));
					}
					float locspeed = 0.25f;
					if (NPC.ai[0] % 900 > 550)
					{
						Vector2 cas = new Vector2(NPC.position.X - P.position.X, NPC.position.Y - P.position.Y);
						double dist = cas.Length();
						float rotation = (float)Math.Atan2(NPC.position.Y - (P.position.Y - (new Vector2(0, (float)(dist * 0.05f))).Y + (P.height * 0.5f)), NPC.position.X - (P.position.X + (P.width * 0.5f)));
						NPC.rotation = rotation;//npc.rotation+((rotation-npc.rotation)*0.1f);
						NPC.velocity = NPC.velocity * 0.86f;
						if (NPC.ai[0] % 20 == 0 && NPC.ai[0] % 900 > 650)
						{
							//NPC findthem = Main.npc[NPC.FindFirstNPC(mod.NPCType("CobaltWraith"))];
							//int arrowType = SGAWorld.NightmareHardcore > 0 && !(findthem.ModNPC as CobaltWraith).raged ? mod.ProjectileType("UnmanedArrow2") : ProjectileID.WoodenArrowHostile;
							int arrowType = ProjectileID.WoodenArrowHostile;
							List<Projectile> one = Idglib.Shattershots(NPC.Center, NPC.Center + new Vector2(-15 * NPC.spriteDirection, 0), new Vector2(0, 0), arrowType, 20, 20, 0, 1, true, (Main.rand.Next(-100, 100) * 0.000f) - NPC.rotation, true, 300);
							one[0].hostile = true;
							one[0].friendly = false;
							one[0].localAI[0] = P.whoAmI;
							one[0].netUpdate = true;
						}
						NPC.spriteDirection = 1;
					}
					else
					{
						if (Math.Abs(NPC.velocity.X) > 2) { NPC.spriteDirection = NPC.velocity.X > 0 ? -1 : 1; }
						NPC.rotation = (float)NPC.velocity.X * 0.09f;
						locspeed = 0.5f;
					}
					NPC.velocity = NPC.velocity * 0.96f;
					itt.Normalize();
					NPC.velocity = NPC.velocity + (itt * locspeed);
					NPC.timeLeft = 999;
				}

			}
		}

	}


	public class CobaltChainSawPiece : CobaltArmorChainmail
	{
		public int armortype = ItemID.CobaltChainsaw;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Chainsaw");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.CobaltChainsaw; }
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 26;
			NPC.defDamage = 6;
			NPC.defense = 5;
			NPC.lifeMax = 500;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath7;
			NPC.value = 0f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			AIType = -1;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
			return NPC.ai[0] % 300 > 150;

		}
        public override void AI()
		{
			CopperArmorPiece myself = npc.ModNPC as CopperArmorPiece;
			int npctype = Mod.Find<ModNPC>(myself.attachedType).Type;
			NPC myowner = Main.npc[myself.attachedID];
			if (myowner.active == false)
			{
				myself.ArmorMalfunction();
			}
			else
			{
				Player P = Main.player[NPC.target];
				if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				{
					NPC.TargetClosest(false);
					P = Main.player[NPC.target];
					if (!P.active || P.dead)
					{
						NPC.active = false;
						Main.npc[(int)NPC.ai[1]].active = false;
					}
				}
				else
				{
					NPC.ai[0] = NPC.ai[0] + 1;
					if (NPC.ai[0] % 300 < 150)
					{
						NPC.velocity = NPC.velocity + ((myowner.Center + new Vector2((float)NPC.ai[1], (float)NPC.ai[2]) - NPC.Center)) / (50);
					}
					else
					{
						NPC.velocity = NPC.velocity + (P.Center - NPC.Center) * (0.02f);
					}
					NPC.velocity = NPC.velocity * 0.5f;
					Vector2 dif = NPC.Center - myowner.Center;
					dif.Normalize();
					NPC.position = NPC.position + (dif * 5);

					NPC.rotation = (float)Math.Atan2(NPC.position.Y - (myowner.position.Y + (myowner.height * 0.5f)), NPC.position.X - (myowner.position.X + (myowner.width * 0.5f))) + 90f;


					NPC.timeLeft = 999;
				}

			}


		}

		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			if (Main.expertMode || Main.rand.Next(2) == 0)
			{
				player.AddBuff(BuffID.Bleeding, 60 * 10, true);
			}
			if (Main.expertMode || Main.rand.Next(4) == 0)
			{
				player.AddBuff(Mod.Find<ModBuff>("MassiveBleeding").Type, 60 * 4, true);
			}

		}

		/*public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
{
Vector2 drawPos = npc.Center - Main.screenPosition;
Color glowingcolors1 = Main.hslToRgb((float)lightColor.R*0.08f,(float)lightColor.G*0.08f,(float)lightColor.B*0.08f);
Texture2D texture = mod.GetTexture("Terraria/Projectile_" + ProjectileID.CobaltChainsaw);
spriteBatch.Draw(texture, drawPos, null, lightColor, npc.rotation, new Vector2(48,48),new Vector2(1,1), SpriteEffects.None, 0f);
return true;
}*/


	}

	public class CobaltArmorSword : CopperArmorChainmail
	{
		public int armortype = ItemID.CobaltSword;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Sword");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + armortype; }
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(0, 8) == 0)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("CobaltWraithNotch").Type);
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 20;
			NPC.defDamage = 20;
			NPC.defense = 25;
			NPC.lifeMax = 500;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath7;
			NPC.value = 0f;
			NPC.knockBackResist = -0.5f;
			NPC.aiStyle = -1;
			AIType = -1;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
		}
		public override void AI()
		{
			CopperArmorPiece myself = npc.ModNPC as CopperArmorPiece;
			int npctype = Mod.Find<ModNPC>(myself.attachedType).Type;
			NPC myowner = Main.npc[myself.attachedID];
			if (myowner.active == false)
			{
				myself.ArmorMalfunction();
			}
			else
			{
				Player P = Main.player[myowner.target];
				NPC.ai[0] += 1;
				NPC.spriteDirection = NPC.velocity.X > 0 ? -1 : 1;
				Vector2 itt = (myowner.Center - NPC.Center + new Vector2(NPC.ai[1] * NPC.spriteDirection, NPC.ai[2]));
				float locspeed = 0.25f;
				if (NPC.ai[0] % 600 > 350)
				{

					NPC.damage = (int)NPC.defDamage * 3;
					itt = itt = (P.position - NPC.position + new Vector2(NPC.ai[1] * NPC.spriteDirection, -8));

					if (NPC.ai[0] % 160 == 0 && SGAmod.DRMMode)
					{
						Vector2 zxx = itt;
						itt += P.velocity * 3f;
						zxx.Normalize();
						NPC.velocity += (zxx) * 18f;
					}

					NPC.rotation = NPC.rotation + (0.65f * NPC.spriteDirection);
					locspeed = 0.5f;
				}
				else
				{
					NPC.damage = (int)NPC.defDamage;
					if (NPC.ai[0] % 300 < 60)
					{

						locspeed = 1.0f;
						NPC.velocity = NPC.velocity * 0.92f;
					}
					NPC.rotation = (float)NPC.velocity.X * 0.09f;
					locspeed = 0.7f;
				}
				NPC.velocity = NPC.velocity * 0.96f;
				itt.Normalize();
				NPC.velocity = NPC.velocity + (itt * locspeed);
				NPC.timeLeft = 999;
			}


		}

		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			if (Main.expertMode || Main.rand.Next(2) == 0)
			{
				player.AddBuff(BuffID.Bleeding, 60 * 10, true);
			}
			if (Main.expertMode || Main.rand.Next(4) == 0)
			{
				player.AddBuff(BuffID.BrokenArmor, 60 * 8, true);
			}

		}

	}



















	[AutoloadBossHead]
	public class CobaltWraith : CopperWraith, ISGABoss
	{

		bool madearmor = false;
		public bool raged = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Wraith");
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
		}
		public override void SetDefaults()
		{
			NPC.width = 16;
			NPC.height = 16;
			NPC.damage = 40;
			NPC.defense = 0;
			NPC.lifeMax = 2000;
			NPC.HitSound = SoundID.NPCHit5;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.knockBackResist = 0.05f;
			NPC.aiStyle = -1;
			NPC.boss = true;
			music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Copperig");
			//music=MusicID.Boss5;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			CopperWraith myself = npc.ModNPC as CopperWraith;
			myself.level = 1;
			NPC.buffImmune[BuffID.Daybreak] = true;
			NPC.value = Item.buyPrice(0, 1, 0, 0);
		}
		public override string Texture
		{
			get { return ("SGAmod/NPCs/TPD"); }
		}

		public override string BossHeadTexture => "SGAmod/NPCs/Wraiths/CobaltWraith_Head_Boss";

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.6f);
		}


		public override void SpawnMoreExpert()
		{
			Idglib.Chat("Turn.... baacckk...", 5, 5, 60);
			int newguy = NPC.NewNPC((int)NPC.position.X + 400, (int)NPC.position.Y, Mod.Find<ModNPC>(level > 0 ? "CobaltArmorChainmail" : "CopperArmorChainmail").Type); NPC newguy2 = Main.npc[newguy]; CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI; newguy2.lifeMax = (int)(NPC.lifeMax * 2f); newguy2.life = (int)(NPC.lifeMax * (2f)); newguy2.knockBackResist = 1f;
			Main.npc[newguy].width += 16;
			Main.npc[newguy].height += 16;
			newguy2.netUpdate = true;
			raged = true;
			createarmorthings();
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
		}

		public override void NPCLoot()
		{

			/*WorldGen.oreTier1 = 107;
			WorldGen.oreTier3 = 111;*/


			List<int> types = new List<int>();
			types.Insert(types.Count, Mod.Find<ModItem>("WraithFragment4").Type);
			int ammount = 0;
			for (ammount = 0; ammount < 1; ammount += 1)
				types.Insert(types.Count, ItemID.Hellstone);
			for (ammount = 0; ammount < 2; ammount += 1)
				types.Insert(types.Count, SGAmod.WorldOres[4, SGAWorld.oretypeshardmode[0] == TileID.Cobalt ? 1 : 0]);
			for (ammount = 0; ammount < 2; ammount += 1)
				types.Insert(types.Count, SGAmod.WorldOres[5, SGAWorld.oretypeshardmode[1] == TileID.Mythril ? 1 : 0]);
			for (ammount = 0; ammount < 1; ammount += 1)
				types.Insert(types.Count, ItemID.SoulofLight);
			for (ammount = 0; ammount < 1; ammount += 1)
				types.Insert(types.Count, SGAmod.WorldOres[6, SGAWorld.oretypeshardmode[2] == TileID.Adamantite ? 1 : 0]);
			for (ammount = 0; ammount < 1; ammount += 1)
				types.Insert(types.Count, ItemID.SoulofNight);

			DropHelper.DropFixedItemQuanity(types.ToArray(), (Main.expertMode ? 100 : 50) *(NPC.downedMoonlord ? 2 : 1), NPC.Center);

			/*for (int f = 0; f < (Main.expertMode ? 100 : 50); f = f + 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, types[Main.rand.Next(0, types.Count)]);
			}*/

			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("WraithFragment4").Type, Main.expertMode ? 25 : 10);

			if (Main.rand.Next(7) == 0)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Armors.Vanity.CobaltWraithMask>());
			}

			Achivements.SGAAchivements.UnlockAchivement("Cobalt Wraith", Main.LocalPlayer);
			if (SGAWorld.downedWraiths < 2)
				SGAWorld.downedWraiths = 2;

		}

		public void createarmorthings()
		{
			for (float fx = -14f; fx < 15f; fx = fx + 28f)
			{
				int itz = NPC.NewNPC((int)NPC.position.X, (int)NPC.position.Y, Mod.Find<ModNPC>("CobaltBrickArmor").Type);
				NPC arm = Main.npc[itz];
				CopperArmorPiece newguy3 = arm.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI;
				arm.lifeMax = (int)(NPC.lifeMax * 1.25f); arm.ai[1] = -fx; arm.ai[2] = 0f; arm.life = (int)(NPC.lifeMax * (1.25f)); arm.knockBackResist = 1f;
				int lastone = itz;
				arm.netUpdate = true;

				for (int coounz = 1; coounz < 4; coounz += 1)
				{
					int nexus = NPC.NewNPC((int)NPC.position.X, (int)NPC.position.Y, Mod.Find<ModNPC>("CobaltBrickArmorNexus").Type);
					NPC armpeice = Main.npc[nexus];
					newguy3 = armpeice.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI;
					armpeice.lifeMax = (int)(NPC.lifeMax * 0.75f); armpeice.ai[1] = -186 * (fx > 0 ? 1 : -1) / 2; armpeice.ai[2] = -24f; armpeice.life = (int)(NPC.lifeMax * (0.75f)); armpeice.knockBackResist = 1f;
					(armpeice.ModNPC as CobaltArmorChainmail).mode = (int)(fx * (coounz+1));
					armpeice.netUpdate = true;
				}


				for (float fx2 = 5f; fx2 < 35f; fx2 = fx2 + 12f)
				{
					int newguyleggings = NPC.NewNPC((int)NPC.position.X + 0, (int)NPC.position.Y, Mod.Find<ModNPC>("CobaltBrickArmor").Type);
					NPC armpeice = armpeice = Main.npc[newguyleggings];
					newguy3 = armpeice.ModNPC as CopperArmorPiece; newguy3.attachedID = lastone; newguy3.speed = newguy3.speed / (1 + (fx2 / 300));
					lastone = newguyleggings;
					armpeice.lifeMax = (int)(NPC.lifeMax * 0.75f); armpeice.ai[1] = Main.rand.Next(-80, 80); armpeice.ai[2] = Main.rand.Next(-80, 80); armpeice.life = (int)(NPC.lifeMax * (0.75f)); armpeice.knockBackResist = 1f; armpeice.netUpdate = true;

					int nexus = NPC.NewNPC((int)NPC.position.X + 0, (int)NPC.position.Y, Mod.Find<ModNPC>("CobaltBrickArmorNexus").Type);
					armpeice = Main.npc[nexus];
					newguy3 = armpeice.ModNPC as CopperArmorPiece; newguy3.speed = newguy3.speed / (1 + (fx2 / 200)); newguy3.attachedID = newguyleggings;
					armpeice.lifeMax = (int)(NPC.lifeMax * 0.75f); armpeice.ai[1] = Main.rand.Next(-80, 80); armpeice.ai[2] = Main.rand.Next(-80, 80); armpeice.life = (int)(NPC.lifeMax * (0.75f)); armpeice.knockBackResist = 1f; armpeice.netUpdate = true;
				}

			}

		}
		public override void AI()
		{

			Player P = Main.player[NPC.target];
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(false);
				P = Main.player[NPC.target];
				if (!P.active || P.dead)
				{
					NPC.active = false;
					Main.npc[(int)NPC.ai[1]].active = false;
				}
			}
			else
			{
				if ((P.Center - NPC.Center).Length() < 700)
					NPC.timeLeft = Math.Max(NPC.timeLeft, 500);
				base.AI();
				if (NPC.ai[0] > 10) { NPC.ai[0]++; }
				if (NPC.ai[0] == 1)
				{
					int newguyleggings = 0;
					CopperArmorPiece newguy3 = null;
					/*==int newguy=NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("CobaltArmorChainmail"));
					myarmor=Main.npc[newguy];
					newguy3 = myarmor.ModNPC as CopperArmorPiece; newguy3.attachedID=npc.whoAmI;
					myarmor.lifeMax=(int)(npc.lifeMax*2f); myarmor.ai[2]=-4f; myarmor.life=(int)(npc.lifeMax*(2f)); myarmor.knockBackResist=1f;

					newguyleggings=NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("CobaltArmorGreaves"));
					temparmor=Main.npc[newguyleggings];
					newguy3 = temparmor.ModNPC as CopperArmorPiece; newguy3.attachedID=newguy;
					temparmor.lifeMax=(int)(npc.lifeMax*1.25f); temparmor.ai[2]=12f; temparmor.life=(int)(npc.lifeMax*(1.25f)); temparmor.knockBackResist=1f;

					newguyleggings=NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("CobaltArmorHelmet"));
					temparmor=Main.npc[newguyleggings];
					newguy3 = temparmor.ModNPC as CopperArmorPiece; newguy3.attachedID=newguy;
					temparmor.lifeMax=(int)(npc.lifeMax*1.25f); temparmor.ai[2]=-20f; temparmor.life=(int)(npc.lifeMax*(1.25f)); temparmor.knockBackResist=1f;*/
					if (madearmor == false)
					{
						madearmor = true;
						for (float fx = -14f; fx < 15f; fx = fx + 28f)
						{
							int itz = NPC.NewNPC((int)NPC.position.X, (int)NPC.position.Y, Mod.Find<ModNPC>("CobaltBrickArmor").Type);
							NPC arm = Main.npc[itz];
							newguy3 = arm.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI;
							arm.lifeMax = (int)(NPC.lifeMax * 1.25f); arm.ai[1] = -fx; arm.ai[2] = 0f; arm.life = (int)(NPC.lifeMax * (1.25f)); arm.knockBackResist = 1f;
							arm.netUpdate = true;
							int lastone = itz;

							int nexus = NPC.NewNPC((int)NPC.position.X, (int)NPC.position.Y, Mod.Find<ModNPC>("CobaltBrickArmorNexus").Type);
							NPC armpeice = Main.npc[nexus];
							newguy3 = armpeice.ModNPC as CopperArmorPiece; newguy3.attachedID = itz;
							armpeice.lifeMax = (int)(NPC.lifeMax * 0.75f); armpeice.ai[1] = -16 * (fx > 0 ? 1 : -1) / 2; armpeice.ai[2] = -24f; armpeice.life = (int)(NPC.lifeMax * (0.75f)); armpeice.knockBackResist = 1f;
							armpeice.netUpdate = true;



							for (float fx2 = 20f; fx2 < 75f; fx2 = fx2 + 18f)
							{
								newguyleggings = NPC.NewNPC((int)NPC.position.X + 0, (int)NPC.position.Y, Mod.Find<ModNPC>("CobaltBrickArmor").Type);
								armpeice = Main.npc[newguyleggings];
								newguy3 = armpeice.ModNPC as CopperArmorPiece; newguy3.attachedID = lastone; newguy3.speed = newguy3.speed / (1 + (fx2 / 300));
								lastone = newguyleggings;
								armpeice.lifeMax = (int)(NPC.lifeMax * 0.75f); armpeice.ai[1] = Main.rand.Next(-80, 80); armpeice.ai[2] = Main.rand.Next(-80, 80); armpeice.life = (int)(NPC.lifeMax * (0.75f)); armpeice.knockBackResist = 1f;
								armpeice.netUpdate = true;

								nexus = NPC.NewNPC((int)NPC.position.X + 0, (int)NPC.position.Y, Mod.Find<ModNPC>("CobaltBrickArmorNexus").Type);
								armpeice = Main.npc[nexus];
								newguy3 = armpeice.ModNPC as CopperArmorPiece; newguy3.speed = newguy3.speed / (1 + (fx2 / 200)); newguy3.attachedID = newguyleggings;
								armpeice.lifeMax = (int)(NPC.lifeMax * 0.75f); armpeice.ai[1] = Main.rand.Next(-80, 80); armpeice.ai[2] = Main.rand.Next(-80, 80); armpeice.life = (int)(NPC.lifeMax * (0.75f)); armpeice.knockBackResist = 1f;
								armpeice.netUpdate = true;
							}

						}


					}


				}

			}
		}



		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			int npctype = Mod.Find<ModNPC>("CopperArmorChainmail").Type;
			Vector2 drawPos = NPC.Center - Main.screenPosition;
			Color glowingcolors1 = Main.hslToRgb((float)lightColor.R * 0.08f, (float)lightColor.G * 0.08f, (float)lightColor.B * 0.08f);
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/TPD").Value;
			spriteBatch.Draw(texture, drawPos, null, glowingcolors1, NPC.spriteDirection + (NPC.ai[0] * 0.4f), new Vector2(16, 16), new Vector2(Main.rand.Next(1, 20) / 17f, Main.rand.Next(1, 20) / 17f), SpriteEffects.None, 0f);
			if (NPC.ai[0] > 0) { return false; }
			else
			{
				//Vector2 drawPos = npc.Center-Main.screenPosition;
				for (int a = 0; a < 30; a = a + 1)
				{
					spriteBatch.Draw(texture, drawPos, null, glowingcolors1, NPC.spriteDirection + (NPC.ai[0] * (1 - (a % 2) * 2)) * 0.4f, new Vector2(16, 16), new Vector2(Main.rand.Next(1, 100) / 17f, Main.rand.Next(1, 20) / 17f), SpriteEffects.None, 0f);
				}
				return true;
			}
		}


	}
}

