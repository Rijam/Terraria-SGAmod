using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class HeatBeater : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoots a spread of bullets");
		}

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 56;
            Item.height = 28;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.value = 10000;
            Item.rare = 5;
            Item.UseSound = SoundID.Item38;
            Item.autoReuse = true;
            Item.shoot = 10;
            Item.shootSpeed = 16f;
            Item.useAmmo = AmmoID.Bullet;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/HeatBeater_Glow").Value;
            }
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, -4);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Shotgun, 1).AddIngredient(null, "FieryShard", 10).AddIngredient(mod.ItemType("UnmanedBar"), 10).AddTile(TileID.MythrilAnvil).Register();
        }

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 3 + Main.rand.Next(2);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale; 
				int prog=Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                IdgProjectile.Sync(prog);
			}
			return false;
		}
	}
}
