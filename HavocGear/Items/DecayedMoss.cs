using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items
{
    public class DecayedMoss : ModItem
    {
        public override void SetDefaults()
        {

            Item.width = 22;
            Item.height = 22;
            Item.maxStack = 99;

            Item.rare = 1;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = 1;
            Item.UseSound = SoundID.Item81;
            Item.consumable = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Decayed Moss");
            Tooltip.SetDefault("Can be used to fertilize saplings");
        }


        public override bool CanUseItem(Player player)
        {
            return TileLoader.IsSapling(Main.tile[Player.tileTargetX, Player.tileTargetY].TileType);
        }

        public override bool? UseItem(Player player)
        {
            if (WorldGen.GrowTree(Player.tileTargetX, Player.tileTargetY))
            {
                WorldGen.TreeGrowFXCheck(Player.tileTargetX, Player.tileTargetY);
            }
            else
            {
                item.stack++;
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe(12).AddIngredient(ModContent.ItemType<Biomass>(), 2).AddIngredient(ModContent.ItemType<MoistSand>(), 4).AddIngredient(ModContent.ItemType<Weapons.SwampSeeds>(), 6).AddIngredient(ModContent.ItemType<DankCore>(), 1).AddTile(TileID.AlchemyTable).Register();
        }

    }
}