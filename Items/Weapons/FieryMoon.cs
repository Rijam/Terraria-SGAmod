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
	public class FieryMoon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Moon");
			Tooltip.SetDefault("Launches molten rock that can pierce several times before exploding into a thermal nova\nHold left click to keep the ball attached to the end of the staff");
			Item.staff[Item.type] = true; 
		}
		
		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Magic;
			Item.width = 34;    
			Item.mana = 25;
            Item.height = 24;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = 1000000;
			Item.rare = 5;
	        Item.shootSpeed = 8f;
            Item.noMelee = true; 
			Item.shoot = Mod.Find<ModProjectile>("FieryRock").Type;
            Item.shootSpeed = 7f;
			Item.UseSound = SoundID.Item8;
			Item.channel = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "FieryShard", 10).AddIngredient(mod.ItemType("UnmanedBar"), 10).AddTile(TileID.MythrilAnvil).Register();
		}

	}

}