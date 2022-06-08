using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using Terraria;
using System;
using SGAmod.Items.Accessories;
using SGAmod.Items.Consumables;
using Terraria.DataStructures;

namespace SGAmod.Items.Placeable.TechPlaceable
{
	public class NumismaticCrucibleItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Numismatic Crucible");
			Tooltip.SetDefault("Simulates npc deaths and outputs their drops\nPlace a filled Soul Jar into the machine to designate that type of enemy\nRequires Raw money inputed via coins to function\nMoney cost is relative to the money ammount dropped by the designated enemy\n'basically a mob farm block'");
		}

        public override string Texture => "SGAmod/Tiles/TechTiles/NumismaticCrucibleTile";

        public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 26;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 2, 50, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.TechTiles.NumismaticCrucibleTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<AdvancedPlating>(), 12).AddRecipeGroup("SGAmod:Tier1Bars", 12).AddIngredient(ModContent.ItemType<EnergizerBattery>(), 2).AddIngredient(ModContent.ItemType<Weapons.Technical.LaserMarker>(), 10).AddIngredient(ItemID.CookingPot, 1).AddTile(ModContent.TileType<Tiles.ReverseEngineeringStation>()).Register();
		}
	}
}