using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.TechPlaceable
{
	public class AureateVaultItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aureate Vault");
			Tooltip.SetDefault("'Entry point to the Planes of Wealth, but you can't reach inside'\nGrants nearby players Aureation Aura, which gilds nearby NPCs\nCan be right clicked to collect any money sacrificed to the Midas Insignia\nInfinite Gold can be exctracted over time, but not by hand");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "sacrificedMoney", Main.LocalPlayer.SGAPly().MoneyCollected));
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
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = ItemRarityID.Yellow;
			Item.createTile = Mod.Find<ModTile>("AureateVault").Type;
			Item.placeStyle = 0;
		}
	}
}