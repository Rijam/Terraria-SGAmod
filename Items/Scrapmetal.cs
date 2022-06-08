using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items
{
	public class Scrapmetal : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Scrap Metal");
			Tooltip.SetDefault("A peice of the never ending gravel wars");

		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.rare = 0;
			Item.value = 0;//Item.sellPrice(0, 0, 20, 0);		
		}
	}
}