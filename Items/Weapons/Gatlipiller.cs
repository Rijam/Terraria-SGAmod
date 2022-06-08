using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class Gatlipiller : ModItem
	{
		public int delay = 45;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gatlipiller");
			Tooltip.SetDefault("Fires a 5-round burst that gets faster the longer you hold down the fire button\nBullets poison targets\nThe shots do not travel far\n50% to not consume ammo");
		}

		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 42;
			Item.height = 16;
			Item.useTime = 4;
			Item.useAnimation = 20;
			Item.reuseDelay = delay;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.value = 50000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item111;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 26f;
			Item.useAmmo = AmmoID.Bullet;
		}
		public override bool ConsumeAmmo(Player player)
		{
			return Main.rand.Next(100) < 50;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, 0);
		}
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(delay);
		}

		public override void NetReceive(BinaryReader reader)
		{
			delay=reader.ReadInt32();
		}

		public override void UpdateInventory(Player player)
		{
			if (player.itemAnimation < 1)
				delay = 45;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (delay>8 && !(player.itemAnimation < Item.useAnimation - 2))
				delay -= 5;
			Item.reuseDelay = delay;

			float speed = 1.5f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(4);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 48f;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].timeLeft = 100;
				Main.projectile[proj].knockBack = Item.knockBack;
				IdgProjectile.AddOnHitBuff(proj, BuffID.Poisoned, 60 * 6);
			}
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.IllegalGunParts, 1).AddIngredient(mod.ItemType("BiomassBar"), 10).AddIngredient(mod.ItemType("DankCore"), 2).AddIngredient(ItemID.Minishark, 1).AddTile(TileID.Anvils).Register();
		}

	}

}
