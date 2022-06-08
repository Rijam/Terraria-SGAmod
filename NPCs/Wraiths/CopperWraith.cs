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
using SGAmod.Items;

namespace SGAmod.NPCs.Wraiths
{
	public class CopperArmorPiece : ModNPC
	{

		public int armortype = ItemID.CopperChainmail;
		public int attachedID = 0;
		public int CoreID = 0;
		public float friction = 0.75f;
		public float speed = 0.3f;
		public string attachedType = "CopperWraith";
		public bool selfdestruct;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(attachedID);
			//Texture2D Texz = new Texture2D(Main.graphics.GraphicsDevice, width, height, false, SurfaceFormat.Color);
			//Texz.SetData
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			attachedID = reader.Read();
		}

		public virtual void ArmorMalfunction()
		{
			selfdestruct = true;
			CopperArmorPiece myself = npc.ModNPC as CopperArmorPiece;
			NPC.velocity = new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-5, 5));
			NPC.StrikeNPCNoInteraction((int)(NPC.lifeMax * 0.15f), 0f, 0);
		}

		public override void NPCLoot()
		{

			if (Main.rand.Next(0, 3) < 2)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("CopperWraithNotch").Type);
		}

		public override bool CheckActive()
		{
			CopperArmorPiece myself = npc.ModNPC as CopperArmorPiece;
			int npctype = Mod.Find<ModNPC>(myself.attachedType).Type;
			NPC myowner = Main.npc[myself.attachedID];
			return (!myowner.active);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Armor Piece");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + armortype; }
		}
		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 0;
			NPC.defense = 0;
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

		public override bool PreAI()
		{
			if (Hellion.Hellion.GetHellion() != null)
			{
				if (Hellion.Hellion.GetHellion().NPC.ai[1] > 999)
					NPC.ai[0] = 0;

			}

			return true;
		}
		public override void AI()
		{
			int npctype = Mod.Find<ModNPC>(attachedType).Type;
			if (NPC.CountNPCS(npctype) > 0)
			{
				NPC myowner = Main.npc[NPC.FindFirstNPC(npctype)];
			}
			else { NPC.active = false; }

		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 0f;
			return null;
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Main.expertMode)
			{
				double damagemul = 1.0;
				if (projectile.penetrate > 1 || projectile.penetrate < 0)
					damagemul = 0.5;
				base.OnHitByProjectile(projectile, (int)(damage * damagemul), knockback, crit);
			}

		}

	}


	public class CopperArmorChainmail : CopperArmorPiece
	{
		public int armortype = ItemID.CopperChainmail;

		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 32;
			NPC.damage = 0;
			NPC.defense = 0;
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

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chainmail");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + (ItemID.CopperChainmail); }
		}
		public override void AI()
		{
			CopperArmorPiece myself = npc.ModNPC as CopperArmorPiece;
			NPC myowner = Main.npc[myself.attachedID];
			if (myowner.active == false)
			{
				myself.ArmorMalfunction();
			}
			else
			{
				NPC.velocity = NPC.velocity + (myowner.Center + new Vector2((float)NPC.ai[1], (float)NPC.ai[2]) - NPC.Center) * (myself.speed);
				NPC.velocity = NPC.velocity * myself.friction;
				NPC.rotation = (float)NPC.velocity.X * 0.1f;
				//npc.position=myowner.position;
				NPC.timeLeft = 999;
			}


		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.npcTexture[NPC.type];
			Vector2 drawPos = NPC.Center - Main.screenPosition;
			Color lights = lightColor;
			lights.A = (byte)(NPC.alpha);
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

			Vector2 drawoffset = new Vector2((float)Math.Sin(Main.GlobalTimeWrappedHourly * 1.61775f + ((float)NPC.whoAmI * 5.734575f)) * 7f, (float)Math.Cos(Main.GlobalTimeWrappedHourly * 1.246f + ((float)NPC.whoAmI * 5.734575f)) * 5f);

			if (GetType() == typeof(CopperArmorChainmail) || GetType() == typeof(CopperArmorGreaves) || GetType() == typeof(CopperArmorHelmet)
				|| GetType() == typeof(CobaltArmorChainmail) || GetType() == typeof(CobaltArmorGreaves) || GetType() == typeof(CobaltArmorHelmet))
			{
				Rectangle drawrect;
				texture = GetType() == typeof(CobaltArmorGreaves) ? Mod.Assets.Request<Texture2D>("NPCs/Wraiths/Cobalt_Wraith_resprite_leggys") .Value: Mod.Assets.Request<Texture2D>("NPCs/Wraiths/Copper_Wraith_resprite_leggys").Value;
				origin = new Vector2((float)texture.Width * 0.5f, -12);
				if (GetType() == typeof(CopperArmorChainmail) || GetType() == typeof(CobaltArmorChainmail))
				{
					texture = GetType() == typeof(CobaltArmorChainmail) ? Mod.Assets.Request<Texture2D>("NPCs/Wraiths/Cobalt_Wraith_resprite_chestplate") .Value: Mod.Assets.Request<Texture2D>("NPCs/Wraiths/Copper_Wraith_resprite_chestplate").Value;
					origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
				}
				if (GetType() == typeof(CopperArmorHelmet) || GetType() == typeof(CobaltArmorHelmet))
				{
					texture = GetType() == typeof(CobaltArmorHelmet) ? Mod.Assets.Request<Texture2D>("NPCs/Wraiths/Cobalt_Wraith_resprite_Helmet") .Value: Mod.Assets.Request<Texture2D>("NPCs/Wraiths/Copper_Wraith_resprite_Helmet_1").Value;
					int offset = (int)(Math.Min(((int)((float)Main.GlobalTimeWrappedHourly * 8f)) % 15f, 5) * ((float)texture.Height / 6f));
					drawrect = new Rectangle(0, offset, texture.Width, (int)(texture.Height / 6f));
					origin = new Vector2((float)texture.Width * 0.5f, ((float)(texture.Height / 6f) * 0.5f) + 20f);
				}
				else
				{
					drawrect = new Rectangle(0, 0, texture.Width, texture.Height);
				}

				SpriteEffects effect = SpriteEffects.None;
				Player theplayer = Main.LocalPlayer;
				if (theplayer.active && !theplayer.dead)
				{
					if (theplayer.Center.X < NPC.Center.X)
						effect = SpriteEffects.FlipHorizontally;
				}

				for (float speez = NPC.velocity.Length(); speez > 0f; speez -= 0.5f)
				{
					Vector2 speedz = (NPC.velocity); speedz.Normalize();
					spriteBatch.Draw(texture, drawPos + (speedz * speez * -2f) + drawoffset, drawrect, lights * 0.02f, NPC.rotation, origin, new Vector2(1f, 1f), effect, 0f);

				}

				spriteBatch.Draw(texture, drawPos + drawoffset, drawrect, lightColor, NPC.rotation, origin, new Vector2(1f, 1f), effect, 0f);

			}
			else
			{
				spriteBatch.Draw(texture, drawPos + (drawoffset / 3f), null, lightColor, NPC.rotation, origin, new Vector2(1f, 1f), SpriteEffects.None, 0f);
			}
			return false;
		}

	}

	public class CopperArmorHelmet : CopperArmorChainmail
	{
		public int armortype = ItemID.CopperHelmet;

		public override void SetDefaults()
		{
			base.SetDefaults();

		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helmet");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + (ItemID.CopperHelmet); }
		}

	}

	public class CopperArmorGreaves : CopperArmorChainmail
	{
		public int armortype = ItemID.CopperGreaves;

		public override void SetDefaults()
		{
			base.SetDefaults();
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Greaves");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + (ItemID.CopperGreaves); }
		}

	}

	public class CopperArmorBow : CopperArmorPiece
	{
		public int armortype = ItemID.CopperBow;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Bow");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + (ItemID.CopperBow); }
		}
		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 400;
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
			float speedmulti = 0.75f;
			if (!Main.expertMode)
				speedmulti = 0.5f;
			if (SGAmod.DRMMode)
				speedmulti = 1f;
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
					float locspeed = 0.25f * speedmulti;
					if (NPC.ai[0] % 900 > 550)
					{
						Vector2 cas = new Vector2(NPC.position.X - P.position.X, NPC.position.Y - P.position.Y);
						double dist = cas.Length();
						float rotation = (float)Math.Atan2(NPC.position.Y - (P.position.Y - (new Vector2(0, (float)(dist * 0.15f))).Y + (P.height * 0.5f)), NPC.position.X - (P.position.X + (P.width * 0.5f)));
						NPC.rotation = rotation;//npc.rotation+((rotation-npc.rotation)*0.1f);
						NPC.velocity = NPC.velocity * 0.86f;
						if (NPC.ai[0] % 50 == 0 && NPC.ai[0] % 900 > 650)
						{


							List<Projectile> one = Idglib.Shattershots(NPC.Center, NPC.Center + new Vector2(-15 * NPC.spriteDirection, 0), new Vector2(0, 0), Math.Abs(NPC.ai[1]) < 18 ? ProjectileID.DD2BetsyArrow : (SGAmod.DRMMode ? Mod.Find<ModProjectile>("UnmanedArrow") .Type: ProjectileID.WoodenArrowHostile), 7, 12, 0, 1, true, (Main.rand.Next(-100, 100) * 0.000f) - NPC.rotation, true, 300);
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
						locspeed = 0.5f * speedmulti;
					}
					NPC.velocity = NPC.velocity * 0.96f;
					itt.Normalize();
					NPC.velocity = NPC.velocity + (itt * locspeed);
					NPC.timeLeft = 999;
				}

			}
		}


	}

	public class CopperArmorSword : CopperArmorPiece
	{
		public int armortype = ItemID.CopperBroadsword;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Sword");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + (ItemID.CopperShortsword); }
		}
		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 5;
			NPC.defDamage = 5;
			NPC.defense = 0;
			NPC.lifeMax = 400;
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
			float speedmulti = 0.75f;
			if (!Main.expertMode)
				speedmulti = 0.5f;
			if (SGAWorld.NightmareHardcore > 0)
				speedmulti = 1f;
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
				float locspeed = 0.25f * speedmulti;
				if (NPC.ai[0] % 600 > 350)
				{

					NPC.damage = (int)NPC.defDamage * 3;
					itt = itt = (P.position - NPC.position + new Vector2(NPC.ai[1] * NPC.spriteDirection, -8));

					if (NPC.ai[0] % 180 == 0 && SGAWorld.NightmareHardcore > 0)
					{
						Vector2 zxx = itt;
						zxx += P.velocity * 3f;
						zxx.Normalize();
						NPC.velocity += zxx * 18;
					}
					NPC.rotation = NPC.rotation + (0.65f * NPC.spriteDirection);
				}
				else
				{
					NPC.damage = (int)NPC.defDamage;
					if (NPC.ai[0] % 300 < 60)
					{
						locspeed = 2.5f * speedmulti;
						NPC.velocity = NPC.velocity * 0.92f;
					}
					NPC.rotation = (float)NPC.velocity.X * 0.09f;
					locspeed = 0.5f * speedmulti;
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
		}


	}
























	[AutoloadBossHead]
	public class CopperWraith : ModNPC, ISGABoss
	{
		public string Trophy() => GetType() == typeof(CobaltWraith) ? "CobaltWraithTrophy" : "CopperWraithTrophy";
		public bool Chance() => Main.rand.Next(0, 10) == 0;
		public string RelicName() => GetType() == typeof(CobaltWraith) ? "Cobalt_Wraith" : "Copper_Wraith";
		public void NoHitDrops() { }
		public string MasterPet() => GetType() == typeof(CobaltWraith) ? "CobaltTack" : "CopperTack";
		public bool PetChance() => Main.rand.Next(4) == 0;

		public int level = 0;
		public Vector2 OffsetPoints = new Vector2(0f, 0f);
		public float speed = 0.2f;
		public bool coreonlymode = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Wraith");
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
		}
		public override void SetDefaults()
		{
			NPC.width = 16;
			NPC.height = 16;
			NPC.damage = 10;
			NPC.defense = 0;
			NPC.lifeMax = 400;
			NPC.HitSound = SoundID.NPCHit5;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.knockBackResist = 0.05f;
			NPC.aiStyle = -1;
			NPC.boss = true;
			music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Copperig");
			//music =MusicID.Boss5;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.value = Item.buyPrice(0, 0, 25, 0);
		}
		public override string Texture
		{
			get { return ("SGAmod/NPCs/TPD"); }
		}

		public override string BossHeadTexture => "SGAmod/NPCs/Wraiths/CopperWraith_Head_Boss";

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.6f);
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if ((projectile.maxPenetrate < 1 || projectile.maxPenetrate > 1) && projectile.damage > (GetType() == typeof(CobaltWraith) ? 20 : 30))
			{
				damage = (int)((float)damage * 0.25f);

			}
			else
			{
				//if (GetType() == typeof(CopperWraith))
				//{
				if (GetType() == typeof(CopperWraith))
					damage = (int)(damage * 1.5);
				if (projectile.maxPenetrate < 2 && projectile.maxPenetrate > -1)
					crit = true;
			}
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.LesserHealingPotion;
		}

		public override void NPCLoot()
		{
			List<int> types = new List<int>();

			/*WorldGen.CopperTierOre = 7;
			WorldGen.IronTierOre = 6;
			WorldGen.SilverTierOre = 9;
			WorldGen.GoldTierOre = 8;*/

			if (SGAWorld.craftwarning < 30)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("TrueCopperWraithNotch").Type);
			}
			if (Main.expertMode)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("WraithTargetingGamepad").Type);

			if (Main.rand.Next(7) == 0)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Armors.Vanity.CopperWraithMask>());
			}

			int shardtype = ModContent.ItemType<WraithFragment>();

			/*if (SGAWorld.WorldIsTin)
			{
				shardtype = mod.ItemType("WraithFragment2");
				//npc.GivenName = "Tin Wraith";
			}*/

			

			types.Insert(types.Count, shardtype);
			types.Insert(types.Count, SGAmod.WorldOres[0, SGAWorld.oretypesprehardmode[0] == TileID.Copper ? 1 : 0]); types.Insert(types.Count, SGAmod.WorldOres[0, SGAWorld.oretypesprehardmode[0] == TileID.Copper ? 1 : 0]);
			types.Insert(types.Count, SGAmod.WorldOres[1, SGAWorld.oretypesprehardmode[1] == TileID.Iron ? 1 : 0]); types.Insert(types.Count, SGAmod.WorldOres[1, SGAWorld.oretypesprehardmode[1] == TileID.Iron ? 1 : 0]);
			types.Insert(types.Count, SGAmod.WorldOres[2, SGAWorld.oretypesprehardmode[2] == TileID.Silver ? 1 : 0]); types.Insert(types.Count, SGAmod.WorldOres[2, SGAWorld.oretypesprehardmode[2] == TileID.Silver ? 1 : 0]);
			types.Insert(types.Count, SGAmod.WorldOres[3, SGAWorld.oretypesprehardmode[3] == TileID.Gold ? 1 : 0]);

			DropHelper.DropFixedItemQuanity(types.ToArray(), (Main.expertMode ? 50 : 30)*(Main.hardMode ? 2 : 1), NPC.Center);

			

			if (shardtype > 0)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, shardtype, (Main.expertMode ? 60 : 30) * (Main.hardMode ? 2 : 1));
			}

			/*
			WeightedItemSet[] sets = {
							new WeightedItemSet(new (int,int)[]{ (ItemID.CopperPlating, 15),(ItemID.CopperBar, 25)  }),
			new WeightedItemSet(new (int,int)[]{ (ItemID.SunStone, 1) }),
			new WeightedItemSet(new (int,int)[]{ (ItemID.MoonStone, 1) }),
			new WeightedItemSet(new (int,int)[]{ (ItemID.FragmentNebula, 10),(ItemID.FragmentSolar, 10),(ItemID.FragmentStardust, 10),(ItemID.FragmentVortex, 10) },3),

				};

			DropHelper.DropFromItemSets(npc.Center, sets,2);
			*/





			/*for (int f = 0; f < (Main.expertMode ? 50 : 30); f += 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, types[Main.rand.Next(0, types.Count)]);
			}*/

			Achivements.SGAAchivements.UnlockAchivement("Copper Wraith", Main.LocalPlayer);
			if (SGAWorld.downedWraiths < 1)
			{
				SGAWorld.downedWraiths = 1;
				Idglib.Chat("You may now craft bars without being attacked", 150, 150, 70);
			}
		}

		public virtual void SpawnMoreExpert()
		{
			int newguy = NPC.NewNPC((int)NPC.position.X, (int)NPC.position.Y, Mod.Find<ModNPC>("CopperArmorSword").Type); NPC newguy2 = Main.npc[newguy]; CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI; newguy2.ai[0] = 300f; newguy2.ai[1] = -64f; newguy2.ai[2] = 48f; newguy2.lifeMax = (int)NPC.lifeMax * 1; newguy2.life = (int)(NPC.lifeMax * (1)); newguy2.knockBackResist = 0.9f; newguy2.netUpdate = true;
			newguy = NPC.NewNPC((int)NPC.position.X - 400, (int)NPC.position.Y, Mod.Find<ModNPC>("CopperArmorSword").Type); newguy2 = Main.npc[newguy]; newguy3 = newguy2.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI; newguy2.ai[1] = 64f; newguy2.ai[2] = 48f; newguy2.lifeMax = (int)NPC.lifeMax * 1; newguy2.life = (int)(NPC.lifeMax * (1)); newguy2.knockBackResist = 0.9f; newguy2.netUpdate = true;
			newguy = NPC.NewNPC((int)NPC.position.X - 400, (int)NPC.position.Y, Mod.Find<ModNPC>("CopperArmorBow").Type); newguy2 = Main.npc[newguy]; newguy3 = newguy2.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI; newguy2.ai[0] = 450f; newguy2.ai[1] = 16f; newguy2.ai[2] = -64f; newguy2.lifeMax = (int)NPC.lifeMax * 1; newguy2.life = (int)(NPC.lifeMax * (1)); newguy2.knockBackResist = 0.2f; newguy2.netUpdate = true;
			newguy = NPC.NewNPC((int)NPC.position.X + 400, (int)NPC.position.Y, Mod.Find<ModNPC>("CopperArmorBow").Type); newguy2 = Main.npc[newguy]; newguy3 = newguy2.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI; newguy2.ai[1] = -16f; newguy2.ai[2] = -64f; newguy2.lifeMax = (int)NPC.lifeMax * 1; newguy2.life = (int)(NPC.lifeMax * (1)); newguy2.knockBackResist = 0.2f; newguy2.netUpdate = true;
			newguy = NPC.NewNPC((int)NPC.position.X + 400, (int)NPC.position.Y, Mod.Find<ModNPC>(level > 0 ? "CobaltArmorChainmail" : "CopperArmorChainmail").Type); newguy2 = Main.npc[newguy]; newguy3 = newguy2.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI; newguy2.lifeMax = (int)(NPC.lifeMax * 1.5f); newguy2.life = (int)(NPC.lifeMax * (1.5f)); newguy2.knockBackResist = 1f; newguy2.netUpdate = true;
		}
		public override void AI()
		{

			float speedmulti = 0.75f;
			if (GetType() == typeof(CopperWraith))
			{
				if (!Main.expertMode)
					speedmulti = 0.5f;
				if (SGAWorld.NightmareHardcore > 0)
					speedmulti = 1f;

			}

			if (GetType() == typeof(CobaltWraith))
			{
				speedmulti = 1.25f;
				if (!Main.expertMode)
					speedmulti = 1f;
				if (SGAWorld.NightmareHardcore > 0)
					speedmulti = 1.4f;

			}

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

				int expert = 0;
				if (Main.expertMode)
				{
					expert = 1;
				}
				NPC.ai[0] += 1;
				if (NPC.type == Mod.Find<ModNPC>("CobaltWraith").Type) { level = 1; }
				NPC.defense = (int)(((NPC.CountNPCS(Mod.Find<ModNPC>("CopperArmorChainmail").Type) * 6) + (NPC.CountNPCS(Mod.Find<ModNPC>("CopperArmorGreaves").Type)) * 3 + (NPC.CountNPCS(Mod.Find<ModNPC>("CopperArmorHelmet").Type) * 4)) * ((expert + 1) * 0.4f));
				if (NPC.CountNPCS(Mod.Find<ModNPC>("CopperArmorChainmail").Type) + NPC.CountNPCS(Mod.Find<ModNPC>("CobaltArmorChainmail").Type) < 1)
				{
					if (NPC.ai[0] > 50)
					{
						NPC.ai[0] = -500;
					}
				}
				if (NPC.life < NPC.lifeMax * 0.5f)
				{
					if (expert > 0)
					{
						if (NPC.ai[0] > -1500 && NPC.ai[1] == 0) { NPC.ai[0] = -2000; NPC.ai[1] = 1; }
						if (NPC.ai[0] == -1850)
						{
							List<Projectile> itz = Idglib.Shattershots(NPC.position, NPC.position + new Vector2(0, 200), new Vector2(0, 0), ProjectileID.DD2BetsyArrow, 10, 5, 360, 20, true, 0, true, 150);
							for (int f = 0; f < 20; f = f + 1)
							{
								itz[f].aiStyle = 0;
								itz[f].rotation = -((float)Math.Atan2((double)itz[f].velocity.Y, (double)itz[f].velocity.X));
							}
							SpawnMoreExpert();
						}
						if (NPC.ai[0] == -1800)
						{
							NPC.ai[0] = 0;
						}
					}
				}
				if ((NPC.ai[0] == 1 || NPC.ai[0] == -1) && NPC.ai[1] < 1)
				{
					float mul = (NPC.ai[0] < 0 ? 0.10f : 0.45f);
					if (NPC.CountNPCS(Mod.Find<ModNPC>("CopperArmorChainmail").Type) < 1) { int newguy = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y - 10, Mod.Find<ModNPC>(level > 0 ? "CobaltArmorChainmail" : "CopperArmorChainmail").Type); NPC newguy2 = Main.npc[newguy]; CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI; newguy2.lifeMax = (int)NPC.lifeMax * 2; newguy2.life = (int)(NPC.lifeMax * (2 * mul)); newguy2.knockBackResist = 0.85f; newguy2.netUpdate = true; }
					if (NPC.CountNPCS(Mod.Find<ModNPC>("CopperArmorSword").Type) < 1) { int newguy = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y - 10, Mod.Find<ModNPC>(level > 0 ? "CobaltArmorSword" : "CopperArmorSword").Type); NPC newguy2 = Main.npc[newguy]; CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI; newguy2.ai[1] = -32f; newguy2.ai[2] = -16f; newguy2.lifeMax = (int)(NPC.lifeMax * 1f); newguy2.life = (int)(NPC.lifeMax * (1f)); newguy2.knockBackResist = 0.75f; newguy2.netUpdate = true; }
					if (NPC.CountNPCS(Mod.Find<ModNPC>("CopperArmorHelmet").Type) < 1) { int newguy = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y - 10, Mod.Find<ModNPC>(level > 0 ? "CobaltArmorHelmet" : "CopperArmorHelmet").Type); NPC newguy2 = Main.npc[newguy]; CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI; newguy2.ai[2] = -12f; newguy2.lifeMax = (int)(NPC.lifeMax * 1.5f); newguy2.life = (int)(NPC.lifeMax * (1.5f * mul)); newguy2.knockBackResist = 0.8f; newguy2.netUpdate = true; }
					if (NPC.CountNPCS(Mod.Find<ModNPC>("CopperArmorGreaves").Type) < 1) { int newguy = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y - 10, Mod.Find<ModNPC>(level > 0 ? "CobaltArmorGreaves" : "CopperArmorGreaves").Type); NPC newguy2 = Main.npc[newguy]; CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI; newguy2.ai[2] = 12f; newguy2.lifeMax = (int)(NPC.lifeMax * 1.5f); newguy2.life = (int)(NPC.lifeMax * (1.5f * mul)); newguy2.knockBackResist = 0.8f; newguy2.netUpdate = true; }
					if (NPC.CountNPCS(Mod.Find<ModNPC>("CopperArmorBow").Type) < 1) { int newguy = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y - 10, Mod.Find<ModNPC>(level > 0 ? "CobaltArmorSword" : "CopperArmorBow").Type); NPC newguy2 = Main.npc[newguy]; CopperArmorPiece newguy3 = newguy2.ModNPC as CopperArmorPiece; newguy3.attachedID = NPC.whoAmI; newguy2.ai[1] = 32f; newguy2.ai[2] = -16f; newguy2.lifeMax = (int)(NPC.lifeMax * (1f)); newguy2.life = (int)(NPC.lifeMax * (1f)); newguy2.knockBackResist = 0.75f; newguy2.netUpdate = true; }
				}
				if (NPC.ai[0] > 1)
				{

					if (NPC.ai[0] % 600 < 250)
					{
						Vector2 itt = ((P.Center + OffsetPoints) - NPC.position); itt.Normalize();
						NPC.velocity = NPC.velocity + (itt * (speed * speedmulti));
					}
					NPC.velocity = NPC.velocity * 0.98f;

				}
				if (NPC.ai[0] < 0 && NPC.ai[0] > -2000)
				{
					if (NPC.ai[0] % 110 < -95)
					{
						NPC.velocity = new Vector2(Main.rand.Next(-20, 20), 0);
						if (NPC.ai[0] % 10 == 0)
						{
							Idglib.Shattershots(NPC.position, P.position, new Vector2(P.width, P.height), 100, 10, 8, 0, 1, true, 0, true, 300);
						}
					}
					else
					{
						Vector2 itt = ((P.Center + OffsetPoints + new Vector2(0, -250)) - NPC.position); itt.Normalize();
						float speedz = (float)level + 0.45f;
						NPC.velocity = NPC.velocity + ((itt * speedz) * speedmulti);
					}
					float fric = 0.96f + ((float)level * 0.01f);
					NPC.velocity = NPC.velocity * fric;
				}


			}
		}




		public override bool CanHitPlayer(Player ply, ref int cooldownSlot)
		{
			return true;
		}
		public override bool? CanBeHitByItem(Player player, Item item)
		{
			if (CanBeHitByPlayer(player))
			{
				return false;
			}
			else
			{
				return base.CanBeHitByItem(player, item);
			}
		}
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			//if (Main.npc[projectile.owner]!=null){return false;}else{
			//return CanBeHitByPlayer(Main.player[projectile.owner]);
			if (CanBeHitByPlayer(Main.player[projectile.owner]))
			{
				return false;
			}
			else
			{
				return base.CanBeHitByProjectile(projectile);
			}
		}
		private bool CanBeHitByPlayer(Player P)
		{
			//int npctype=mod.NPCType("CopperArmorChainmail");
			return NPC.ai[0] < -700 ? true : false;
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

