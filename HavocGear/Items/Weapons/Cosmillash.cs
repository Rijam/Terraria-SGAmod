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
	public class Cosmillash : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmillash");
			Tooltip.SetDefault("Shoots 3 homing quasar beams that explode on hit, dealing damage in a large area" +
				"\nExplosion doesn't crit, damage falls off over distance");
		}
		public override void SetDefaults()
		{
			Item.damage = 130;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 12;
			Item.width = 22;
			Item.height = 22;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 5;
			Item.knockBack = 10;
			Item.value = 1000000;
			Item.rare = 10;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 30;
			Item.crit = 10;
			Item.staff[Item.type] = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/Cosmillash_Glow").Value;
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = Mod.Find<ModProjectile>("QuasarOrb").Type;
			float numberProjectiles = 3; // 3, 4, or 5 shots
			float rotation = MathHelper.ToRadians(Main.rand.Next(33));
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .25f; // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 4);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("ShadeflameStaff"), 1).AddIngredient(ItemID.FragmentNebula, 8).AddIngredient(mod.ItemType("StarMetalBar"), 12).AddIngredient(mod.ItemType("IlluminantEssence"), 20).AddIngredient(ItemID.SoulofNight, 6).AddTile(TileID.LunarCraftingStation).Register();
		}
	}
}