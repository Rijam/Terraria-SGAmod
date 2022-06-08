using SGAmod.HavocGear.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SGAmod.Items.Placeable.DankWoodFurniture
{
	public class DankWoodPlatform : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("It still smells funny...");
		}

		public override void SetDefaults()
		{
			Item.width = 8;
			Item.height = 10;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.createTile = TileType<Tiles.DankWoodFurniture.DankWoodPlatformTile>();
		}

		public override void AddRecipes() 
		{
			CreateRecipe(2).AddIngredient(ItemType<DankWood>()).AddTile(TileID.WorkBenches).Register();
		}
	}
}