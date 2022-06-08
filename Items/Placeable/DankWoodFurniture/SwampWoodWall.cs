using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.DankWoodFurniture
{
	public class SwampWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug: Swamp Wall");
			Tooltip.SetDefault("Use this in case your Dank Shrines get messed up to fix them");
		}
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.createWall = Mod.Find<ModWall>("SwampWall").Type;
		}
	}

	public class SwampWoodWall : ModItem
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
			Item.useStyle = 1;
			Item.consumable = true;
			Item.createWall = Mod.Find<ModWall>("SwampWoodWall").Type;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swamp Wood Wall");
			Tooltip.SetDefault("'For when you wanna bit a bit closer to Nature'");
		}

		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(null, "DankWood").AddTile(TileID.LivingLoom).Register();
		}
	}
	
}
