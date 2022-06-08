using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.TechPlaceable
{	
	public class DropperItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dropper");
			Tooltip.SetDefault("Drops items sent into it via hoppers\nHammer the dropper to change its output direction\nDroppers can be disabled by sending a wire signal to them");
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
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = Item.buyPrice(silver: 10);
			Item.rare = 1;
			Item.createTile = Mod.Find<ModTile>("DropperTile").Type;
			Item.placeStyle = 0;
			Item.mech = true;
		}
        public override string Texture => "SGAmod/Items/Placeable/TechPlaceable/DropperItem";

        public override void AddRecipes()
		{
			CreateRecipe(3).AddIngredient(mod.ItemType("UnmanedBar"), 3).AddIngredient(mod.ItemType("NoviteBar"), 3).AddIngredient(ItemID.IronCrate, 1).AddTile(TileID.HeavyWorkBench).Register();
		}
	}
}