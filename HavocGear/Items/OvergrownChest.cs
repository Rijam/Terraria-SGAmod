using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items
{
	public class OvergrownChest : ModItem
	{
		public override void SetDefaults()
		{

			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("OvergrownChest").Type;
        }     

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Overgrown Chest");
      Tooltip.SetDefault("");
    }

	}
}   
