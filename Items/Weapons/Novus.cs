using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Items.Tools;
using Terraria.Audio;


namespace SGAmod.Items.Weapons
{
	public class UnmanedStaff : UnmanedPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Staff");
			Tooltip.SetDefault("Casts homing Novus bolts");
			Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 5;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 5;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("UnmanedBolt").Type;
			Item.shootSpeed = 4f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("UnmanedBar"), 10).AddTile(TileID.Anvils).Register();
		}
	}

	public class UnmanedBow : UnmanedPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Bow");
			Tooltip.SetDefault("Converts wooden arrows into Novus arrows");
		}

		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 5;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item5;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 6f;
			Item.useAmmo = AmmoID.Arrow;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			speedX *= player.ArrowSpeed(); speedY *= player.ArrowSpeed();

			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = Mod.Find<ModProjectile>("UnmanedArrow").Type;
			}
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("UnmanedBar"), 10).AddTile(TileID.Anvils).Register();
		}
	}

	public class UnmanedSword : UnmanedPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Sword");
		}
		public override void SetDefaults()
		{
			Item.damage = 18;
			Item.DamageType = DamageClass.Melee;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("UnmanedBar"), 10).AddTile(TileID.Anvils).Register();
		}

	}
	public class UnmanedShuriken : UnmanedPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Shuriken");
			Tooltip.SetDefault("Returns to the player like a boomerang on enemy hit");
		}

		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.knockBack = 1;
			Item.value = 10;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Blue;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<UnmanedShurikenProj>();
			Item.shootSpeed = 10f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 16f;
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(100).AddIngredient(mod.ItemType("UnmanedBar"), 1).AddIngredient(ItemID.Shuriken, 100).AddTile(TileID.Anvils).Register();
		}
	}


	public class UnmanedShurikenProj : ModProjectile
	{

		double keepspeed = 0.0;
		float homing = 0.05f;
		public float beginhoming = 20f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Shuriken");
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public override string Texture => "SGAmod/Items/Weapons/UnmanedShuriken";

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Shuriken);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.hostile = false;
			Projectile.friendly = true;
			// projectile.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.penetrate = 3;
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			AIType = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			Player owner = Main.player[Projectile.owner];
			if (Projectile.aiStyle == 3 && (owner.MountedCenter - Projectile.Center).Length() < 64)
			{
				owner.QuickSpawnItem(ModContent.ItemType<UnmanedShuriken>(), 1);
				return true;
			}


			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			for (int num315 = 0; num315 < 15; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("NovusSparkle").Type, Projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), Projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 15f), 50, Main.hslToRgb(0.4f, 0f, 0.15f), 2.4f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
			}
			Projectile.type = ProjectileID.Shuriken;

			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!target.friendly && Projectile.aiStyle != 3)
			{
				Projectile.aiStyle = 3;
				Projectile.extraUpdates = 1;
				Projectile.netUpdate = true;
			}
		}

		public override void AI()
		{
			for (int num315 = 0; num315 < (Projectile.aiStyle == 3 ? 1 : 2); num315++)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("NovusSparkle").Type, 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.15f), 1.7f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.3f;
			}
		}
	}
}