using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Tools
{
	public class DankWoodHammer : ModItem
	{
        public override void SetDefaults()
		{
			base.SetDefaults();

            Item.damage = 6;
            Item.hammer = 40;
            Item.width = 32;
			Item.height = 32;
            Item.DamageType = DamageClass.Melee;
            Item.useTurn = false;
            Item.rare = 0;
            Item.useStyle = 1;
            Item.useAnimation = 20;
           	Item.knockBack = 7;
	        Item.autoReuse = true;
            Item.useTime = 42;
            Item.consumable = false;
            Item.UseSound = SoundID.Item1;
        }

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dank Wood Hammer");
      Tooltip.SetDefault("");
    }

        
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "DankWood", 8).AddTile(TileID.WorkBenches).Register();
		}
	}
}   
