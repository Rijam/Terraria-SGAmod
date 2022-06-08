using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
    public class DankWoodSword : ModItem
	{
        public override void SetDefaults()
		{
			base.SetDefaults();

            Item.damage = 10;
            Item.width = 32;
			Item.height = 32;
            Item.DamageType = DamageClass.Melee;
            Item.useTurn = false;
            Item.rare = 1;
            Item.useStyle = 1;
            Item.useAnimation = 20;
           	Item.knockBack = 3;
            Item.useTime = 20;
            Item.consumable = false;
            Item.UseSound = SoundID.Item1;
        }

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dank Wood Sword");
      Tooltip.SetDefault("Crits against foes slowed by Dank Slow");
    }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (target.GetGlobalNPC<SGAnpcs>().DankSlow)
                crit = true;
        }


        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "DankWood", 25).AddTile(TileID.WorkBenches).Register();
		}
	}
}   
