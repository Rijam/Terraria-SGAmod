using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items
{
	public class BrokenAlter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Broken Ancient Alter");
			Tooltip.SetDefault("A strange device, it seems to still be functional\nMaybe I could repair it to full power...");
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 14;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = Item.sellPrice(0, 12, 0, 0);
			Item.rare = 10;
			Item.createTile = Mod.Find<ModTile>("BrokenAlter").Type;
		}
		public override string Texture
        {
            get { return "SGAmod/Tiles/BrokenAlter"; }
        }
	}

}
