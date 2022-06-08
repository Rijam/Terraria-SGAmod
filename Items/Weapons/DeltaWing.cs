using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class DeltaWing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Delta Wing");
			Tooltip.SetDefault("Converts arrows into Windfall arrows" +
				"\nWindfall arrows grant back a small amount of WingTime to the player on hit, and are nearly lighter than air");
		}

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 5;
			Item.value = 100000;
			Item.rare = 4;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.shoot = ModContent.ProjectileType<Projectiles.WindfallArrow>();
			Item.shootSpeed = 9f;
			Item.useAmmo = AmmoID.Arrow;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("UnmanedBow"), 1).AddIngredient(ItemID.Feather, 10).AddIngredient(ItemID.SoulofFlight, 10).AddTile(TileID.MythrilAnvil).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			speedX *= player.ArrowSpeed(); speedY *= player.ArrowSpeed();

			type = ModContent.ProjectileType<Projectiles.WindfallArrow>();
			return true;
		}

	}

}