using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SGAmod.HavocGear.Items
{
	// Example Soul of Light/Soul of Night style NPC summon
	public class SwampChestSummon : ModPlayer
	{
		public int LastChest = 0;

		// This doesn't make sense, but this is around where this check happens in Vanilla Terraria.
		public override void PreUpdateBuffs()
		{
			if (Main.netMode != 1)
			{
				if (Player.chest == -1 && LastChest >= 0 && Main.chest[LastChest] != null)
				{
					int x2 = Main.chest[LastChest].x;
					int y2 = Main.chest[LastChest].y;
					ChestItemSummonCheck(x2, y2, Mod);
				}
				LastChest = Player.chest;
			}
		}

		public static bool ChestItemSummonCheck(int x, int y, Mod mod)
		{
			if (Main.netMode == 1)
			{
				return false;
			}
			int num = Chest.FindChest(x, y);
			if (num < 0)
			{
				return false;
			}
			int numberExampleBlocks = 0;
			int numberOtherItems = 0;
			ushort tileType = Main.tile[Main.chest[num].x, Main.chest[num].y].TileType;
			int tileStyle = (int)(Main.tile[Main.chest[num].x, Main.chest[num].y].TileFrameX / 36);
			if (tileType == TileID.Containers && (tileStyle < 5 || tileStyle > 6))
			{
				for (int i = 0; i < 40; i++)
				{
					if (Main.chest[num].item[i] != null && Main.chest[num].item[i].type > 0)
					{
						if (Main.chest[num].item[i].type == Mod.Find<ModItem>("SwampChestKey").Type)
						{
							numberExampleBlocks += Main.chest[num].item[i].stack;
						}
						else
						{
							numberOtherItems++;
						}
					}
				}
			}
			if (numberOtherItems == 0 && numberExampleBlocks == 1)
			{
				if (Main.tile[x, y].TileType == 21)
				{
					if (Main.tile[x, y].TileFrameX % 36 != 0)
					{
						x--;
					}
					if (Main.tile[x, y].TileFrameY % 36 != 0)
					{
						y--;
					}
					int number = Chest.FindChest(x, y);
					for (int j = x; j <= x + 1; j++)
					{
						for (int k = y; k <= y + 1; k++)
						{
							if (Main.tile[j, k].TileType == 21)
							{
								Main.tile[j, k].HasTile;
							}
						}
					}
					for (int l = 0; l < 40; l++)
					{
						Main.chest[num].item[l] = new Item();
					}
					Chest.DestroyChest(x, y);
                    NetMessage.SendData(34, -1, -1, null, 1, (float)x, (float)y, 0f, number, 0, 0);
                    NetMessage.SendTileSquare(-1, x, y, 3);
                }
                int npcToSpawn = Mod.Find<ModNPC>("SwampBigMimic").Type;
                int npcIndex = NPC.NewNPC(x * 16 + 16, y * 16 + 32, npcToSpawn, 0, 0f, 0f, 0f, 0f, 255);
                Main.npc[npcIndex].whoAmI = npcIndex;
                NetMessage.SendData(23, -1, -1, null, npcIndex, 0f, 0f, 0f, 0, 0, 0);
                Main.npc[npcIndex].BigMimicSpawnSmoke();
            }
			return false;
		}
	}

	public class SwampChestKey : ModItem
	{
		public override void SetDefaults()
		{
			base.SetDefaults();


			Item.width = 14;
			Item.height = 18;
			// item.melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Item.rare = 5;
			Item.useStyle = 1;
			Item.useAnimation = 0;
			Item.maxStack = 30;
			Item.useTime = 0;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dank Key");
			Tooltip.SetDefault("'Charged with the essence of the murky depths'");
		}


		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.SoulofNight, 3).AddIngredient(ItemID.SoulofLight, 3).AddIngredient(null, "MurkyGel", 15).AddIngredient(null, "DankCore", 1).AddTile(TileID.WorkBenches).Register();
		}
	}

}
