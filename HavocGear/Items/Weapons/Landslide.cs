using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class Landslide : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 20;
			Item.width = 28;
			Item.height = 30;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = 4;
			Item.noMelee = true;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = 1;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
			Item.shoot = 1;
            Item.shootSpeed = 10f;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Landslide");
            Tooltip.SetDefault("");
        }


        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < Main.rand.Next(6, 13); i++)
            {
                string[] projectileArray = { "Landslide1", "Landslide2", "Landslide3" };
                Projectile.NewProjectile(Main.MouseWorld.X + (Main.rand.Next(-20, 21)), player.position.Y - 600, Main.rand.Next(-2, 3), Main.rand.Next(12, 16), Mod.Find<ModProjectile>(projectileArray[Main.rand.Next(projectileArray.Length)]).Type, Item.damage, 0f, Main.myPlayer, 0f, 0f);
            }
            return false;
        }
	}
}
