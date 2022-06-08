using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.Utilities;
using System.Linq;

namespace SGAmod.Items.Armors.Vanity
{
	[AutoloadEquip(EquipType.Head)]
	public class ContraMerchHat : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Contraband Merchant's Hat");
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 10;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
			drawAltHair = true;
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class ContraMerchCoat : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Contraband Merchant's Coat");
		}
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 32;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			// The equipSlot is added in SGAmod.cs --> Load hook
			equipSlot = Mod.GetEquipSlot("ContraMerchCoat_Legs", EquipType.Legs);
			//Legs become "invisible". Idk how to fix it.
		}

		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
			drawArms = false;
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class ContraMerchShoes : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Contraband Merchant's Shoes");
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 10;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;//1
			Item.vanity = true;
			Item.defense = 0;
		}
	}
}