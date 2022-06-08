using IL.Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SGAmod.Items.Placeable;

namespace SGAmod.Tiles
{
	public class BossTrophies : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 6;
			TileObjectData.addTile(Type);
			dustType = 7;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Trophy");
			AddMapEntry(new Color(120, 85, 60), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int item = 0;
			if (frameY / 54 == 0) //PreHardmode-1st row
			{
				switch (frameX / 54)
				{
					case 0:
						item = Mod.Find<ModItem>("CopperWraithTrophy").Type;
						break;
					case 1:
						item = Mod.Find<ModItem>("CaliburnATrophy").Type;
						break;
					case 2:
						item = Mod.Find<ModItem>("CaliburnBTrophy").Type;
						break;
					case 3:
						item = Mod.Find<ModItem>("CaliburnCTrophy").Type;
						break;
					case 4:
						item = Mod.Find<ModItem>("SpiderQueenTrophy").Type;
						break;
					case 5:
						item = Mod.Find<ModItem>("MurkTrophy").Type;
						break;
				}
			}
			if (frameY / 54 == 1) //Hardmode-2nd row
			{
				switch (frameX / 54)
				{
					case 0:
						item = Mod.Find<ModItem>("CirnoTrophy").Type;
						break;
					case 1:
						item = Mod.Find<ModItem>("CobaltWraithTrophy").Type;
						break;
					case 2:
						item = Mod.Find<ModItem>("SharkvernTrophy").Type;
						break;
					case 3:
						item = Mod.Find<ModItem>("CratrosityTrophy").Type;
						break;
					case 4:
						item = Mod.Find<ModItem>("TwinPrimeDestroyersTrophy").Type;
						break;
					case 5:
						item = Mod.Find<ModItem>("DoomHarbingerTrophy").Type;
						break;
				}

			}
			if (frameY / 54 == 2) //PML-3rd row
			{
				switch (frameX / 54)
				{
					case 0:
						item = Mod.Find<ModItem>("LuminiteWraithTrophy").Type;
						break;
					case 1:
						item = Mod.Find<ModItem>("CratrogeddonTrophy").Type;
						break;
					case 2:
						item = Mod.Find<ModItem>("SupremePinkyTrophy").Type;
						break;
					case 3:
						item = Mod.Find<ModItem>("HellionTrophy").Type;
						break;
					case 4:
						item = Mod.Find<ModItem>("PhaethonTrophy").Type;
						break;
					case 5:
						item = Mod.Find<ModItem>("PrismicBansheeTrophy").Type;
						break;
				}
			}
			if (frameY / 54 == 3) //4th row
			{
				switch (frameX / 54)
				{
					case 0:
						item = Mod.Find<ModItem>("TinWraithTrophy").Type;
						break;
					case 1:
						item = Mod.Find<ModItem>("PalladiumWraithTrophy").Type;
						break;
				}
			}

			if (item > 0)
			{
				Item.NewItem(i * 16, j * 16, 48, 48, item);
			}
		}
	}
}