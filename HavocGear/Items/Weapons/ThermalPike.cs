using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class ThermalPike : Mossthorn
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Pike");
			Tooltip.SetDefault("Pierces half of the enemy's defense");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.damage = 24;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.useTurn = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 16;
			Item.useStyle = 5;
			Item.useTime = 12;
			Item.knockBack = 3.2f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.height = 44;
			Item.maxStack = 1;
			Item.value = 75000;
			Item.rare = 6;
			Item.shoot = Mod.Find<ModProjectile>("ThermalPikeProj").Type;
			Item.shootSpeed = 10f;
		}

        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "FieryShard", 12).AddIngredient(mod.ItemType("UnmanedBar"), 10).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}