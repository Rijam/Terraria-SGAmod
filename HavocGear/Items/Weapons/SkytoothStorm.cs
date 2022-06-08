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
	public class SkytoothStorm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skytooth Storm");
			Tooltip.SetDefault("Rains down sky teeth from the heavens!");
		}
		
		public override void SetDefaults()
		{
			Item.damage = 55;
			Item.DamageType = DamageClass.Magic;
			Item.width = 34;    
			Item.mana = 7;
            Item.height = 24;
			Item.useTime = 10;
			Item.useAnimation = 30;
			Item.useStyle = 5;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = 5;
	        Item.shootSpeed = 8f;
            Item.noMelee = true; 
			Item.shoot = Mod.Find<ModProjectile>("SkyToothProj").Type;
			Item.UseSound = SoundID.Item1;		
			Item.autoReuse = true;
		    Item.useTurn = true;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
		    int numberProjectiles = 2 + Main.rand.Next(3);  
            for (int index = 0; index < numberProjectiles; ++index)
            {
                Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
                vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
                vector2_1.Y -= (float)(100 * index);
                float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
                float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
                if ((double)num13 < 0.0) num13 *= -1f;
                if ((double)num13 < 20.0) num13 = 20f;
                float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
                float num15 = Item.shootSpeed / num14;
                float num16 = num12 * num15;
                float num17 = num13 * num15;
                float SpeedX = num16 + (float)Main.rand.Next(-40, 41) * 0.02f;  
                float SpeedY = num17 + (float)Main.rand.Next(-40, 41) * 0.02f;  
                Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, type, damage, knockBack, Main.myPlayer, 0.0f, (float)Main.rand.Next(5));
            }
           return false;
		}
				
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(1) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, Mod.Find<ModDust>("TornadoDust").Type);
            }
        }
	
	}
}