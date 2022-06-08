using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Accessories
{
	[AutoloadEquip(EquipType.Neck)]
	public class SerratedTooth : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Serrated Tooth");
            Tooltip.SetDefault("Dealing more damage than 5 times an enemy's defense inflicts Massive Bleeding\nbase duration is 1 second and increases further over an enemy's defense your attack is\nHowever this is hard capped at 5 seconds\n+10 Armor Penetration");
        }

        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 15, 0, 0);;
			Item.rare = 5;
			Item.accessory = true;
			Item.expert=true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
		player.armorPenetration+=10;
		player.GetModPlayer<SGAPlayer>().SerratedTooth = true;
		}
	}
}