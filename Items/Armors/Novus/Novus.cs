using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SGAmod.Items.Armors.Novus
{

	[AutoloadEquip(EquipType.Head)]
	public class UnmanedHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Hood");
			Tooltip.SetDefault("5% faster item use times");
		}
        public override string Texture => "SGAmod/Items/Armors/Novus/NovusHood";
        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = 2;
			Item.defense=2;
		}
        public override bool DrawHead()
        {
            return false;
        }
        public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod,typeof(SGAPlayer).Name) as SGAPlayer;
            sgaplayer.UseTimeMul+=0.05f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("UnmanedBar"), 10).AddTile(TileID.Anvils).Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class UnmanedBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Breastplate");
			Tooltip.SetDefault("5% increased crit chance");
		}
		public override string Texture => "SGAmod/Items/Armors/Novus/NovusChestplate";
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = 2;
			Item.defense=4;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Melee) += 5;
			player.GetCritChance(DamageClass.Ranged) += 5;
			player.GetCritChance(DamageClass.Magic) += 5;
			player.Throwing().thrownCrit += 5;
		}		
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("UnmanedBar"), 15).AddTile(TileID.Anvils).Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class UnmanedLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Leggings");
			Tooltip.SetDefault("5% increased movement speed\n10% increased acceleration and max running speed");
		}
		public override string Texture => "SGAmod/Items/Armors/Novus/NovusLeggings";
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = 2;
			Item.defense=2;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed *= 1.05f;
			player.accRunSpeed *= 1.1f;
			player.maxRunSpeed *= 1.1f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("UnmanedBar"), 10).AddTile(TileID.Anvils).Register();
		}
	}


}