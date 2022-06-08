using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items
{
	[AutoloadEquip(EquipType.Head)]
    public class SharkvernMask : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharkvern Mask");
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
			Item.rare = 1;
		}
    }
}