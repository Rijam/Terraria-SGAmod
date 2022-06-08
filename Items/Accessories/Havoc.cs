using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;

namespace SGAmod.Items.Accessories
{
	public class Havoc : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Havoc's Fragmented Remains");
            Tooltip.SetDefault("'The remains of PeopleMCNugget's dreams of Havoc mod'\n'now at your will and prehaps, can be put back together'\n25% more damage with Havoc weapons\n3 defense per Havoc accessory equipped (8 in hardmode)\nEffects of Photosynthesizer and Serrated Tooth");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly/4f)%1f, 0.45f, 0.65f);
			tooltips.Add(new TooltipLine(Mod,"Dedicated", Idglib.ColorText(c,"Dedicated to PeopleMCNugget")));
			c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 3.17f) % 1f, 0.65f, 0.45f);
			tooltips.Add(new TooltipLine(Mod, "Dedicated2", Idglib.ColorText(c, "And the amazing spriters who helped him")));
		}

        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(1, 0, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.accessory = true;
			Item.expert=true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
		player.GetModPlayer<SGAPlayer>().Havoc = 1;
		Mod.GetItem("Photosynthesizer").UpdateAccessory(player,hideVisual);
		Mod.GetItem("SerratedTooth").UpdateAccessory(player,hideVisual);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("SharkTooth"), 100).AddIngredient(mod.ItemType("MurkyGel"), 100).AddIngredient(mod.ItemType("Photosynthesizer"), 1).AddIngredient(mod.ItemType("SerratedTooth"), 1).AddIngredient(mod.ItemType("PrismalBar"), 15).AddTile(TileID.LunarCraftingStation).Register();
		}
	}
}