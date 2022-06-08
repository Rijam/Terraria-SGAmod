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
	public class DormantSupernova : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dormant Supernova");
			Tooltip.SetDefault("Mana charged into this modified Novus staff can open a rift in the cosmos at its tip, from which countless novus bolts will appear\nHold left click to keep the portal open, increasing its rate of fire, but also consuming more mana");
			Item.staff[Item.type] = true; 
		}
		
		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Magic;
			Item.width = 34;    
			Item.mana = 10;
            Item.height = 24;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = 100000;
			Item.rare = 2;
	        Item.shootSpeed = 8f;
            Item.noMelee = true; 
			Item.shoot = Mod.Find<ModProjectile>("ProjectilePortalDSupernova").Type;
            Item.shootSpeed = 5f;
			Item.UseSound = SoundID.Item8;
			Item.channel = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("UnmanedStaff"), 1).AddIngredient(mod.ItemType("RedManaStar"), 1).AddIngredient(ItemID.MeteoriteBar, 5).AddIngredient(ItemID.ManaCrystal, 3).AddTile(TileID.Anvils).Register();
		}



	}

}
