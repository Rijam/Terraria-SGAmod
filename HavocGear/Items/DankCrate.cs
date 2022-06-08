using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;

namespace SGAmod.HavocGear.Items
{
    public class DankCrate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 26;
            Item.rare = 2;
            Item.maxStack = 99;
            // item.melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
        }

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dank Crate");
      Tooltip.SetDefault(Language.GetTextValue("CommonItemTooltip.RightClickToOpen"));
    }


        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            int pick = Main.rand.Next(8);
		if (!Main.hardMode)
            player.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(15, 80));
		else
            player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(1, 3));

        if (Main.rand.Next(15) == 0)
                player.QuickSpawnItem(Mod.Find<ModItem>("MurkyCharm").Type);

            if (pick == 0)
                player.QuickSpawnItem(Main.hardMode ? ItemID.CobaltBar : ItemID.IronBar, Main.rand.Next(3, 9));
            else if (pick == 1)
                player.QuickSpawnItem(Main.hardMode ? ItemID.PalladiumBar : ItemID.LeadBar, Main.rand.Next(3, 9));
            else if (pick == 2)
                player.QuickSpawnItem(Main.hardMode ? ItemID.MythrilBar : ItemID.SilverBar, Main.rand.Next(3, 9));
            else if (pick == 3)
                player.QuickSpawnItem(Main.hardMode ? ItemID.OrichalcumBar : ItemID.TungstenBar, Main.rand.Next(3, 9));
            else if (pick == 4)
                player.QuickSpawnItem(Main.hardMode ? ItemID.AdamantiteBar : ItemID.PlatinumBar, Main.rand.Next(3, 9));
            else if (pick == 5)
                player.QuickSpawnItem(Main.hardMode ? ItemID.TitaniumBar : ItemID.GoldBar, Main.rand.Next(3, 9));
            else if (pick == 6)
                player.QuickSpawnItem(SGAWorld.WorldIsNovus ? Mod.Find<ModItem>("UnmanedBar") .Type: Mod.Find<ModItem>("NoviteBar").Type, Main.rand.Next(3, 9));
            else if (pick == 7)
                player.QuickSpawnItem(Mod.Find<ModItem>("TransmutationPowder").Type, Main.rand.Next(4, 11));

            pick = Main.rand.Next(5);
            if (pick == 0)
                player.QuickSpawnItem(Mod.Find<ModItem>("DankWood").Type, Main.rand.Next(12, 36)); 
            else if (pick == 1)
                player.QuickSpawnItem(Mod.Find<ModItem>("DankCore").Type, Main.rand.Next(1, 2)); 
            else if (pick == 2)
                player.QuickSpawnItem(Mod.Find<ModItem>("DecayedMoss").Type, Main.rand.Next(2, 5));    
            else if (pick == 3)
                player.QuickSpawnItem(Mod.Find<ModItem>("Biomass").Type, Main.rand.Next(4, 16));    
            else if (pick == 4)
                player.QuickSpawnItem(SGAWorld.WorldIsNovus ? Mod.Find<ModItem>("UnmanedOre") .Type: Mod.Find<ModItem>("NoviteOre").Type, Main.rand.Next(4, 16));    

        }
	}
}
