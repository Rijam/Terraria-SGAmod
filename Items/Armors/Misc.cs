using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;


namespace SGAmod.Items.Armors
{

	[AutoloadEquip(EquipType.Head)]
	public class AncientHallowedVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Visor");
			Tooltip.SetDefault("12% increased throwing damage\n10% increased throwing crit\n20% increased throwing velocity");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0,5);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 16;
		}
		public override void UpdateEquip(Player player)
		{
			player.Throwing().thrownDamage += 0.12f;
			player.Throwing().thrownCrit += 10;
			player.Throwing().thrownVelocity += 0.20f;

		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.HallowedBar, 12).AddTile(GetType() == typeof(HallowedVisor) ? TileID.DemonAltar : TileID.MythrilAnvil).Register();
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class HallowedVisor : AncientHallowedVisor
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Future Hallowed Visor");
			Tooltip.SetDefault("12% increased throwing damage\n10% increased throwing crit\n20% increased throwing velocity\n'Reverse 1.4 lol'");
		}
	}

}