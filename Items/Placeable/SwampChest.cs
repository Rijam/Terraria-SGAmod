using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable
{
	public class SwampChest : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 500;
			Item.createTile = ModContent.TileType<Tiles.SwampChest>();
		}
	}
	public class LockedSwampChest : ModItem
	{
		public override string Texture => "SGAmod/Items/Placeable/SwampChest";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug: Locked Swamp Chest");
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 500;
			Item.createTile = ModContent.TileType<Tiles.SwampChest>();
			Item.placeStyle = 1;
		}
	}
}