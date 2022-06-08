using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod.HavocGear.Items.Weapons 
{
    public class DjinnsInferno : ModItem
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Djinn's Inferno");
			Tooltip.SetDefault("Spews fiery tendrils\nOrange tendrils spawn Spirit Flames on hit");
		}
	
        public override void SetDefaults()
        {
            Item.damage = 50;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 5;
            Item.useAnimation = 20;
            Item.useStyle = 5;
            Item.noMelee = true; 
            Item.knockBack = 3.5f;
            Item.value = 10000;
            Item.rare = 6;
            Item.UseSound = SoundID.Item103;
            Item.autoReuse = true;
            Item.shoot = Mod.Find<ModProjectile>("HeatTentacle").Type;
            Item.shootSpeed = 17f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/DjinnsInferno_Glow").Value;
			}
		}
    
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
	    {
    	    int i = Main.myPlayer;
		    int num73 = damage;
		    float num74 = knockBack;
    	    num74 = player.GetWeaponKnockback(Item, num74);
    	    player.itemTime = Item.useTime;
    	    Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
    	    float num78 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
		    float num79 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
    	    Vector2 value2 = new Vector2(num78, num79);
		    value2.Normalize();
		    Vector2 value3 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
		    value3.Normalize();
		    value2 = value2 * 4f + value3;
		    value2.Normalize();
		    value2 *= Item.shootSpeed;
		    int projChoice = Main.rand.Next(5);
		    float num91 = (float)Main.rand.Next(10, 160) * 0.001f;
		    if (Main.rand.Next(2) == 0)
		    {
			    num91 *= -1f;
		    }
		    float num92 = (float)Main.rand.Next(10, 160) * 0.001f;
		    if (Main.rand.Next(2) == 0)
		    {
			    num92 *= -1f;
		    }
		    if (projChoice == 0)
		    {
		 	    Projectile.NewProjectile(vector2.X, vector2.Y, value2.X, value2.Y, Mod.Find<ModProjectile>("HotterTentacle").Type, (int)((double)num73 * 1.5f), num74, i, num92, num91);
		    }
		    else
		    {
			    Projectile.NewProjectile(vector2.X, vector2.Y, value2.X, value2.Y, Mod.Find<ModProjectile>("HeatTentacle").Type, num73, num74, i, num92, num91);
		    }
    	    return false;
	    }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.ShadowFlameHexDoll, 1).AddIngredient(ItemID.SpiritFlame, 1).AddIngredient(ItemID.HallowedBar, 8).AddIngredient(null, "FieryShard", 10).AddIngredient(mod.ItemType("UnmanedBar"), 10).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}