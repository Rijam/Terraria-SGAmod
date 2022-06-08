using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class DankWaveStaff : ModItem
	{
		public override void SetDefaults()
		{

			Item.damage = 12;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 8;
			Item.width = 50;
			Item.height = 52;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = 2;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("SwampWave").Type;
			Item.shootSpeed = 10f;
        }

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dank Wave Staff");
      Tooltip.SetDefault("Shoots a short piercing wave");
		Item.staff[Item.type] = true;
	}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "DankWood", 15).AddIngredient(null, "DankCore", 1).AddIngredient(null, "VialofAcid", 8).AddTile(TileID.WorkBenches).Register();
		}

	}
}
