using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;

namespace SGAmod.Items
{
	public class SwampKey : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 32;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Yellow;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (NPC.downedPlantBoss == false)
			{
				tooltips.Add(new TooltipLine(Mod, "PrePlantera", Language.GetText("LegacyTooltip.59").ToString()));
			}
			if (NPC.downedPlantBoss == true)
			{
				tooltips.Add(new TooltipLine(Mod, "PostPlantera", "Unlocks a Swamp Chest in the dungeon"));
			}
		}
	}
}
