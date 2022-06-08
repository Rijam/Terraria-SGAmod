
using System.Linq;
using System;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;

namespace SGAmod.Dimensions.NPCs
{
	public class DungeonPortalDeeperDungeons : DungeonPortal
	{
		bool fallendown = false;
		public override bool Autoload(ref string name)
		{
			name = "Stranger Portal";
			return Mod.Properties.Autoload;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
		}

		public override void AI()
		{
			if (SGAPocketDim.WhereAmI == null)
			{
				NPC.active = false;
				return;
			}


			base.AI();
			if (!fallendown)
			{
				for (int i = 0; i < 200; i += 1)
				{
					Point wearehere = ((NPC.Center + new Vector2(0, 96)) / 16).ToPoint();
					if (WorldGen.InWorld(wearehere.X, wearehere.Y))
					{
						Tile tile = Framing.GetTileSafely(wearehere.X, wearehere.Y);
						if (tile != null)
						{
							if (tile.HasTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]))
							{
								fallendown = true;
								return;
							}
							else
							{
								NPC.position.Y += 2;
							}
						}
					}
					else
					{
						fallendown = true;
					}
				}
			}
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.rarity = 1;
		}
	}


	[AutoloadHead]
	public class DungeonPortal : ModNPC
	{

		public override bool Autoload(ref string name)
		{
			name = "Strange Portal";
			return Mod.Properties.Autoload;
		}

        public override void BossHeadSlot(ref int index)
        {
			if (!SGAWorld.portalcanmovein)
				index = -1;
        }
        public override string HeadTexture => "SGAmod/Dimensions/NPCs/DungeonPortal_Head";

        public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
			// DisplayName.SetDefault("Example Person");
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.ExtraFramesCount[NPC.type] = 1;
			NPCID.Sets.AttackFrameCount[NPC.type] = 1;
			NPCID.Sets.DangerDetectRange[NPC.type] = 0;
			NPCID.Sets.AttackType[NPC.type] = 0;
			NPCID.Sets.AttackTime[NPC.type] = 90;
			NPCID.Sets.AttackAverageChance[NPC.type] = 30;
			NPCID.Sets.HatOffsetY[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true;
			NPC.friendly = true;
			NPC.width = 32;
			NPC.height = 50;
			NPC.aiStyle = 7;
			NPC.damage = 0;
			NPC.noGravity = true;
			NPC.defense = 15;
			NPC.lifeMax = 1000000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			//npc.immortal = true;
			AnimationType = NPCID.Guide;
			NPC.homeless = true;
			NPC.SGANPCs().overallResist = 0;
			//npc.rarity = 1;
			Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 2) % 1f, 0.5f, 0.35f);

		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (GetType() != typeof(DungeonPortal))
				return 0;

			//Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];
			return !spawnInfo.playerInTown && !NPC.BusyWithAnyInvasionOfSorts() && NPC.downedBoss3 && !spawnInfo.invasion && spawnInfo.Player.ZoneDungeon && NPC.CountNPCS(ModContent.NPCType<DungeonPortal>())<1 &&
				(spawnInfo.spawnTileType==TileID.BlueDungeonBrick || spawnInfo.spawnTileType == TileID.PinkDungeonBrick || spawnInfo.spawnTileType == TileID.GreenDungeonBrick) ? 0.02f : 0f;
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return false;
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return false;
		}

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
			return false;
		}

		public override bool CheckConditions(int left, int right, int top, int bottom)
		{
			return SGAWorld.portalcanmovein;
		}

		public override string TownNPCName()
		{
			return "";
		}

		public override string Texture
		{
			get
			{
				return "SGAmod/Projectiles/FieryRock";
			}
		}

		BlendState blind = new BlendState
		{

			ColorSourceBlend = Blend.Zero,
			ColorDestinationBlend = Blend.InverseSourceColor,

			AlphaSourceBlend = Blend.Zero,
			AlphaDestinationBlend = Blend.InverseSourceColor

		};

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>("SGAmod/Projectiles/FieryRock");
				Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;
				Vector2 drawPos = ((NPC.Center - Main.screenPosition));
				Color color = Color.Lerp(drawColor,Color.White,0.5f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Texture, blind, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			for (float valez = 34f; valez > 4f; valez -= 0.2f)
			{
				spriteBatch.Draw(tex, drawPos, null, color * 0.05f, (((NPC.localAI[0]+ valez*0.05f) / 45f) * ((36f - valez)/15f)), drawOrigin, NPC.scale * valez,SpriteEffects.FlipHorizontally, 0f);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			for (float valez = 0.1f; valez < 10f; valez += 0.2f)
			{
				spriteBatch.Draw(tex, drawPos, null, color*0.05f, (((NPC.localAI[0] / 15f)* (6f-valez))+ (valez/3.12612f))/3f, drawOrigin, (NPC.scale* valez)*0.75f,SpriteEffects.FlipVertically, 0f);
			}
			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

			if (BirthdayParty.PartyIsUp)
			{

				UnifiedRandom rando = new UnifiedRandom(NPC.whoAmI);

				for (int i = 0; i < 5; i += 1)
				{

					int[] offsets = { 0, 16, 17, 18, 19 };

					float anglz = (Main.GlobalTimeWrappedHourly* rando.NextFloat(0.25f, 2f)* (rando.NextBool() ? 1f : -1f));

					float dist = 24f + rando.NextFloat(0f,72f)+(float)Math.Sin(Main.GlobalTimeWrappedHourly*(rando.NextFloat(0.25f,1.25f)))* rando.NextFloat(8f, 54f);

					Vector2 angelz = new Vector2((float)Math.Cos(anglz), (float)Math.Sin(anglz)) * dist;
					float angle = Main.GlobalTimeWrappedHourly* rando.NextFloat(0.25f, 3f);

					tex = Main.extraTexture[72];

					int scalehalf = (tex.Width / 20);

					spriteBatch.Draw(tex, NPC.Center+ angelz - Main.screenPosition, new Rectangle((int)(scalehalf) * offsets[i], 0, (int)(tex.Width / 20), tex.Height), Color.Gray, angle, new Vector2(scalehalf/2f, tex.Height/2f), new Vector2(1, 1), SpriteEffects.None, 0f);
				}
			}

			return false;
		}

		public override bool PreAI()
		{
			NPC.dontTakeDamage = true;
			NPC.localAI[0] += 1f;
			NPC.ai[0] = 0;
			NPC.ai[1] = 0;
			NPC.ai[2] = 0;
			NPC.ai[3] = 0;
			NPC.velocity = Vector2.Zero;
			return true;
		}

		// Consider using this alternate approach to choosing a random thing. Very useful for a variety of use cases.
		// The WeightedRandom class needs "using Terraria.Utilities;" to use
		public override string GetChat()
		{
			if (Main.netMode > 0)
				return "Not accessible in Multiplayer";
			if (SGAPocketDim.WhereAmI == typeof(DeeperDungeon))
			{
				return "Go deeper to Floor " + (SGAWorld.dungeonlevel + 2)+"?";
			}
			string extrachat = " (Highest floor completed: " + SGAWorld.highestDimDungeonFloor + ")";
			if (BirthdayParty.PartyIsUp)
				return "Are you sure you wish to leave the party?" + extrachat;
			return "Do you dare enter?" + extrachat;
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{
			//button = Language.GetTextValue("LegacyInterface.28");
			if (Main.netMode < 1)
				button = "Enter";
			//else
				//button = "Buy Loot";

		}

		public override void SetupShop(Chest shop, ref int nextSlot)
		{

		shop.item[nextSlot].SetDefaults(SGAmod.Instance.Find<ModItem>("RingOfRespite").Type);
		shop.item[nextSlot].shopCustomPrice = 75;
		shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
		nextSlot++;

			shop.item[nextSlot].SetDefaults(SGAmod.Instance.Find<ModItem>("NinjaSash").Type);
			shop.item[nextSlot].shopCustomPrice = 100;
			shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
			nextSlot++;

			shop.item[nextSlot].SetDefaults(SGAmod.Instance.Find<ModItem>("StoneBarrierStaff").Type);
			shop.item[nextSlot].shopCustomPrice = 50;
			shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
			nextSlot++;

			shop.item[nextSlot].SetDefaults(SGAmod.Instance.Find<ModItem>("DiesIraeStone").Type);
			shop.item[nextSlot].shopCustomPrice = 50;
			shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
			nextSlot++;

			shop.item[nextSlot].SetDefaults(SGAmod.Instance.Find<ModItem>("MagusSlippers").Type);
			shop.item[nextSlot].shopCustomPrice = 60;
			shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
			nextSlot++;

			shop.item[nextSlot].SetDefaults(SGAmod.Instance.Find<ModItem>("YoyoTricks").Type);
			shop.item[nextSlot].shopCustomPrice = 75;
			shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
			nextSlot++;

			shop.item[nextSlot].SetDefaults(SGAmod.Instance.Find<ModItem>("BeserkerAuraStaff").Type);
			shop.item[nextSlot].shopCustomPrice = 75;
			shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
			nextSlot++;

			shop.item[nextSlot].SetDefaults(SGAmod.Instance.Find<ModItem>("EnchantedFury").Type);
			shop.item[nextSlot].shopCustomPrice = 100;
			shop.item[nextSlot].shopSpecialCurrency = SGAmod.ScrapCustomCurrencyID;
			nextSlot++;
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				if (Main.netMode < 1)
				{
					if (SGAPocketDim.WhereAmI == null)
                    {
						SGAWorld.dungeonlevel = SGAConfigDeeperDungeon.Instance.SetDungeonFloors != null ? SGAConfigDeeperDungeon.Instance.SetDungeonFloors.floor : 0;
						DeeperDungeon.hardMode = Main.hardMode;
						DeeperDungeon.postPlantera = NPC.downedPlantBoss;
					}
					SGAPocketDim.EnterSubworld(Mod.GetType().Name + "_DeeperDungeon", true);
				}
				else
				{
					//shop = true;
				}
			}
		}

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 35;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 5000;
			randExtraCooldown = 10;
		}

		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			projType = ProjectileID.Flames;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 3f;
		}

	}

}