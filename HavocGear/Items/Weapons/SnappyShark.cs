using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class SnappyShark : ModItem
	{
	public override void SetStaticDefaults()
		{	
			DisplayName.SetDefault("Snappy Shark");
			Tooltip.SetDefault("Shoots gouging teeth which halves enemy defense\n30% chance to not consume teeth");
		}

		public override bool ConsumeAmmo(Player player)
		{
			if (Main.rand.Next(0, 100) < 30)
				return false;
			return base.ConsumeAmmo(player);
		}

		public override void SetDefaults()
		{
	Item.CloneDefaults(ItemID.Megashark);
			Item.damage = 35;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 52;
			Item.height = 28;
			Item.useTime = 9;
			Item.knockBack = 4;
			Item.value = 10000;
			Item.rare = 5;
			Item.UseSound = SoundID.Item41;
			Item.autoReuse = true;
            Item.useAmmo = Mod.Find<ModItem>("SharkTooth").Type;
			Item.shoot = Mod.Find<ModProjectile>("SnappyTooth").Type;
		}		

		public override bool Shoot (Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
	        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			return true;
		}		
		
		public override Vector2? HoldoutOffset()
		{
	        return new Vector2(0, 3);
		}
	}
}
