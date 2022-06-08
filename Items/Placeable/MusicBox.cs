using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SGAmod.Tiles;
using Idglibrary;
using static Terraria.ModLoader.ModContent;

namespace SGAmod.Items.Placeable
{
	public class SGAItemMusicBox : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Music Box (" + Name2[0] + ")");
			Tooltip.SetDefault(Idglib.ColorText(Color.PaleTurquoise, "'" + Name2[1] + "'") + Idglib.ColorText(Color.PaleGoldenrod, " : Composed by " + Name2[2]));
		}

		public override bool CloneNewInstances => true;

		string internalname;
		string[] Name2;

		public SGAItemMusicBox(string internalname2, string Name3, string Title, string Author)
		{
			Name2 = new string[3] { Name3, Title, Author };
			internalname = internalname2;
			SGAmod.Instance.AddTile(internalname2, new SGATileMusicBox(internalname2),"SGAmod/Tiles/"+ internalname2);
		}

		public override void SetDefaults() {
			Item.useStyle = 1;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = SGAmod.Instance.Find<ModTile>(internalname).Type;
			Item.width = 24;
			Item.height = 24;
			Item.rare = 4;
			Item.value = 100000;
			Item.accessory = true;
		}
		public override bool Autoload(ref string name)
		{
			return false;
		}
	}
}

namespace SGAmod.Tiles
{
	public class SGATileMusicBox : ModTile
	{
		int itemID => SGAmod.Instance.Find<ModItem>(internalname).Type;
		string internalname;

		public SGATileMusicBox(string internalname2)
		{
			internalname = internalname2;

		}

		public override bool Autoload(ref string name, ref string texture)
		{
			return false;
		}


		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Music Box");
			AddMapEntry(new Color(200, 200, 200), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 16, 48, itemID);
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = itemID;
		}
	}
}