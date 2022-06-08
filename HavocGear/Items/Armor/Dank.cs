using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SGAmod.HavocGear.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]

    public class DankWoodHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dank Wood Helmet");
            Tooltip.SetDefault("4% increased critical strike chance\n15% DoT resistance");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 20;
            Item.value = 10000;
            Item.rare = 1;
            Item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.BoostAllDamage(0, 4);
            player.SGAPly().DoTResist *= 0.85f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(null, "DankWood", 50).AddIngredient(null, "DankCore", 1).AddTile(TileID.WorkBenches).Register();
        }
    }
    [AutoloadEquip(EquipType.Body)]
    public class DankWoodChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dank Wood Chestplate");
            Tooltip.SetDefault("8% increased item use rate, improved life regen\n25% DoT resistance");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 22;
            Item.value = 10000;
            Item.rare = 1;
            Item.lifeRegen = 1;
            Item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
            sgaplayer.UseTimeMul += 0.08f;
            sgaplayer.DoTResist *= 0.75f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(null, "DankWood", 30).AddIngredient(null, "DankCore", 1).AddTile(TileID.WorkBenches).Register();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class DankLegs : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dank Wood Leggings");
            Tooltip.SetDefault("20% Improved movement speed and faster acceleration\n10% DoT resistance");
        }

        public override void SetDefaults()
		{
			Item.width = 18;
            Item.height = 12;
			Item.value = 10000;
			Item.rare = 1;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
            player.moveSpeed += 0.2f;
            player.accRunSpeed += 0.05f;
            player.SGAPly().DoTResist *= 0.90f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(null, "DankWood", 25).AddIngredient(null, "DankCore", 1).AddTile(TileID.WorkBenches).Register();
        }
    }
}