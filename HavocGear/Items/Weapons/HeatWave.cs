using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class HeatWave : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heat Wave");
			Tooltip.SetDefault("Summons heated crystals that lodge into foes, setting them ablaze" +
				"\nAfter a while the crystal explodes, releasing a back blast of energy" +
	"\nOnly 3 crystals may be lodged into a foe at a time");
		}

		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 8;
			Item.width = 34;      
            Item.height = 24;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = 6;
	        Item.shootSpeed = 14f;
            Item.noMelee = true; 
			Item.shoot = Mod.Find<ModProjectile>("HotRound").Type;
			Item.UseSound = SoundID.Item9;		
			Item.autoReuse = true;
		    Item.useTurn = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position += new Vector2(speedX, speedY) * 1;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			return true;
		}
		public override void AddRecipes()
        	{
            CreateRecipe(1).AddIngredient(null, "FieryShard", 12).AddIngredient(mod.ItemType("UnmanedBar"), 10).AddTile(TileID.MythrilAnvil).Register();
        	}
	}
}