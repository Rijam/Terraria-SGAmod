using Terraria.ModLoader;
using Terraria.ID;
using SGAmod.HavocGear.Items;

namespace SGAmod.Items.Placeable.DankWoodFurniture
{
	public class DankWoodWall : ModItem
	{
		public override void SetDefaults() {
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Tiles.DankWoodFurniture.DankWoodWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<DankWood>(), 1).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class DankWoodFence : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Tiles.DankWoodFurniture.DankWoodFence>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<DankWood>(), 1).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class BrokenDankWoodFence : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<Tiles.DankWoodFurniture.BrokenDankWoodFence>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<DankWood>(), 1).AddTile(TileID.WorkBenches).Register();
		}
	}
}