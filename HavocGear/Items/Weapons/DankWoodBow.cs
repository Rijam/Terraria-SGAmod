using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class DankWoodBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 8;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 18;
			Item.height = 32;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 1;
			Item.value = 3000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Arrow;
		}

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dank Wood Bow");
      Tooltip.SetDefault("Wooden Arrows shot from this bow become Dank Arrows");
    }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			speedX *= player.ArrowSpeed(); speedY *= player.ArrowSpeed();
			if (type == ProjectileID.WoodenArrowFriendly)
				type = Mod.Find<ModProjectile>("DankArrow").Type;
			return true;
		}


		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "DankWood", 25).AddTile(TileID.WorkBenches).Register();
		}
	}
}
