using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SGAmod.Items.Armors.Novite
{

	[AutoloadEquip(EquipType.Head)]
	public class NoviteHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Helmet");
			Tooltip.SetDefault("5% increased Technological damage\n+1500 Max Electric Charge\n20% reduced Electric Consumption");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = 2;
			Item.defense = 3;
		}
		public override bool DrawHead()
		{
			return false;
		}
		public override void UpdateEquip(Player player)
		{
			player.SGAPly().techdamage += 0.05f;
			player.SGAPly().electricChargeCost *= 0.80f;
			player.SGAPly().electricChargeMax += 1500;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("NoviteBar"), 10).AddTile(TileID.Anvils).Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class NoviteChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Chestplate");
			Tooltip.SetDefault("10% increased Technological and Trap damage\n+1 passive Electric Charge Rate\n+2500 Max Electric Charge");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = 2;
			Item.defense = 5;
		}
		public override void UpdateEquip(Player player)
		{
			player.SGAPly().techdamage += 0.10f;
			player.SGAPly().TrapDamageMul += 0.10f;
			player.SGAPly().electricChargeMax += 2500;
			player.SGAPly().electricrechargerate += 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("NoviteBar"), 15).AddTile(TileID.Anvils).Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class NoviteLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Leggings");
			Tooltip.SetDefault("5% increased Technological damage\nCharge is built up by running around at high speeds (600/Second)\n+1000 Max Electric Charge");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = 2;
			Item.defense = 2;
		}
		public override void UpdateEquip(Player player)
		{
			player.SGAPly().techdamage += 0.05f;
			player.SGAPly().Noviteset = Math.Max(player.SGAPly().Noviteset, 1);
			player.SGAPly().electricChargeMax += 1000;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("NoviteBar"), 10).AddTile(TileID.Anvils).Register();
		}
	}


}