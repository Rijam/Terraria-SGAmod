using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons 
{
    public class SharkTooth : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shark Tooth");
            Tooltip.SetDefault("May be bought from the Arms Dealer if the player has a [i:" + Mod.Find<ModItem>("SnappyShark") .Type+ "] in their inventory\nDrops from Sharks after Sharkvern is defeated");
        }

        public override void SetDefaults()
        {

            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.consumable = true;
            Item.knockBack = 1.5f;
            Item.value = 100;
            Item.rare = 3;
            Item.ammo = Item.type;
        }
    }
}
