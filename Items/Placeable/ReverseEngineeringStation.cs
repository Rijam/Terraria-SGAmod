using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using Terraria;

namespace SGAmod.Items.Placeable
{
	public class ReverseEngineeringStation : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reverse Engineering Station");
			Tooltip.SetDefault("Allows weaponization of unusual tidbits and crafting of advanced machinery\nSome formerly uncraftable items may be crafted here\nDoubles as a Tinkerer's Workbench");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 26;
			Item.height = 14;
			Item.value = Item.sellPrice(0,0,75,0);
			Item.rare = ItemRarityID.Blue;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("ReverseEngineeringStation").Type;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string s = "Not Binded!";
			foreach (string key in SGAmod.ToggleRecipeHotKey.GetAssignedKeys())
			{
				s = key;
			}

			tooltips.Add(new TooltipLine(Mod, "uncraft", Idglib.ColorText(Color.CornflowerBlue, "Allows you to uncraft non-favorited held items on right click")));
			tooltips.Add(new TooltipLine(Mod, "uncraft", Idglib.ColorText(Color.CornflowerBlue, "Press the 'Toggle Recipe' (" + s + ") Hotkey to swap between recipes")));
			tooltips.Add(new TooltipLine(Mod, "uncraft", Idglib.ColorText(Color.CornflowerBlue, "There is a net loss in materials on uncraft, this can however be reduced")));
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.TinkerersWorkshop, 1).AddIngredient(ItemID.MeteoriteBar, 8).AddIngredient(ModContent.ItemType<Consumables.EnergizerBattery>(), 5).AddIngredient(ModContent.ItemType<Weapons.Technical.LaserMarker>(), 10).AddIngredient(ModContent.ItemType<VialofAcid>(), 25).AddRecipeGroup("SGAmod:EvilBossMaterials", 15).AddTile(TileID.WorkBenches).Register();
			CreateRecipe(1).AddIngredient(ItemID.TinkerersWorkshop, 1).AddIngredient(ItemID.MeteoriteBar, 3).AddIngredient(ModContent.ItemType<VialofAcid>(), 8).AddRecipeGroup("SGAmod:PressurePlates", 2).AddRecipeGroup("SGAmod:EvilBossMaterials", 8).AddRecipeGroup("SGAmod:TechStuff", 1).AddTile(TileID.WorkBenches).Register();
		}
	}
}