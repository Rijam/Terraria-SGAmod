using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.TechPlaceable
{	
	public class SiftingFunnelItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sifting Funnel");
			Tooltip.SetDefault("'Contains several layers of sieves meshes'\nExtracts items that pass through the funnel\nCan only process 5 items from a stack at a time\nRetains all previous functionality of a Hopper");
		}
        public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemRarityID.LightRed;
			Item.consumable = true;
			Item.value = Item.buyPrice(silver: 10);
			Item.rare = ItemRarityID.LightRed;
			Item.createTile = Mod.Find<ModTile>("ShiftingFunnelTile").Type;
			Item.placeStyle = 0;
		}
        public override string Texture => "SGAmod/Items/Placeable/TechPlaceable/SiftingFunnelItem";

        public override void AddRecipes()
		{
			CreateRecipe(3).AddIngredient(ItemID.Extractinator, 1).AddIngredient(ModContent.ItemType<HopperItem>(), 3).AddIngredient(ItemID.MetalShelf, 10).AddTile(ModContent.TileType<Tiles.ReverseEngineeringStation>()).Register();
		}
	}
}