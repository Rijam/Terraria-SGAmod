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
	public class SGABanner : ModItem
	{
		/*public override void SetStaticDefaults() {
			DisplayName.SetDefault("Music Box (" + Name2[0] + ")");
			Tooltip.SetDefault(Idglib.ColorText(Color.PaleTurquoise, "'" + Name2[1] + "'") + Idglib.ColorText(Color.PaleGoldenrod, " : Composed by " + Name2[2]));
		}*/

		/// </summary>

		public override bool CloneNewInstances => true;

		private int placeStyle;

		public SGABanner(string internalname)
		{
			placeStyle = Banners.idToItem.Count;
			Banners.idToItem.Add(Banners.idToItem.Count, internalname);
		}

		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.rare = 1;
			Item.value = Item.buyPrice(0, 0, 10, 0);
			Item.createTile = Mod.Find<ModTile>("Banners").Type;
			Item.placeStyle = placeStyle;
		}
		public override bool Autoload(ref string name)
		{
			return false;
		}
	}
}