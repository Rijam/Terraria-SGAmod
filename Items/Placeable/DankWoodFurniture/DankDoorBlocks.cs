using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SGAmod.HavocGear.Items;
using SGAmod.Items.Placeable;
using System.Linq;

namespace SGAmod.HavocGear.Items
{
	public class DankDoorItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Confuses hostile mobs when hit due to the released stench");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 28;
			Item.maxStack = 99;
			Item.rare = 1;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = 150;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankDoorClosed>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.WoodenDoor).AddIngredient(ModContent.ItemType<DankWood>(), 15).AddTile(TileID.WorkBenches).Register();
		}
	}

}