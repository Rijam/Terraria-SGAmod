using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SGAmod.Items.Weapons
{
	public class Sunbringer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun Bringer");
			//Tooltip.SetDefault("Gotta start somewhere you know");
		}
		public override void SetDefaults()
		{
            Item.CloneDefaults(164);
			Item.damage = 12;
			Item.useTime = 62;
			Item.useAnimation = 62;
			Item.knockBack = 6;
			Item.value = 75000;
			Item.rare = 5;
			Item.shoot = Mod.Find<ModProjectile>("SunbringerFlare").Type;
			Item.useAmmo = AmmoID.Flare;
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("SunbringerFlare").Type, damage, knockBack, player.whoAmI);
			return false;
		} 
	}
}
