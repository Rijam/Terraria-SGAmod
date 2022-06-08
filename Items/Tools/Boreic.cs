using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Tools
{
	public class BoreicPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boreic Pickaxe");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "Faster Icey Item", "25% faster use speed in the snow biome"));
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.MythrilPickaxe);
			Item.useTime = 10;
		}
		
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("CryostalBar"), 10).AddIngredient(mod.ItemType("FrigidShard"), 10).AddTile(TileID.IceMachine).Register();
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(7) == 0)
			{
				SGAPlayer sgaplayer = player.GetModPlayer(Mod,typeof(SGAPlayer).Name) as SGAPlayer;
				int num316 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 92, 0f, 0f, 50, Main.hslToRgb(0.6f, 0.9f, 1f), 0.5f);
				Main.dust[num316].noGravity = true;
			}
		}

		public override float UseTimeMultiplier(Player player)
		{
			if (player.ZoneSnow)
			return 1.25f;
			return 1f;
		}

	}

	public class BoreicDrill : BoreicPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boreic Drill");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.MythrilDrill);
			Item.shoot = Mod.Find<ModProjectile>("BoreicDrillProj").Type;
			Item.useTime = 10;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("CryostalBar"), 8).AddIngredient(mod.ItemType("FrigidShard"), 8).AddTile(TileID.IceMachine).Register();
		}

	}
		public class BoreicHamaxe : BoreicPickaxe
		{
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Boreic Hamaxe");
			}

			public override void SetDefaults()
			{
				Item.CloneDefaults(ItemID.MythrilWaraxe);
				Item.hammer = 50;
			Item.damage = 20;
			Item.useTime = 10;
			Item.useAnimation = 20;
		}

			public override void AddRecipes()
			{
				CreateRecipe(1).AddIngredient(mod.ItemType("CryostalBar"), 12).AddIngredient(mod.ItemType("FrigidShard"), 8).AddTile(TileID.IceMachine).Register();
			}

		}

	public class BoreicJacksaw : BoreicPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boreic Jacksaw");
		}

		public override void SetDefaults()
		{
			Item.damage = 26;
			Item.DamageType = DamageClass.Melee;
			Item.width = 56;
			Item.height = 22;
			Item.useTime = 7;
			Item.useAnimation = 25;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.axe = 17;
			Item.hammer = 85;
			Item.tileBoost++;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = 3000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item23;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("BoreicJacksawProj").Type;
			Item.shootSpeed = 40f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("CryostalBar"), 12).AddIngredient(mod.ItemType("FrigidShard"), 8).AddTile(TileID.IceMachine).Register();
		}
	}

	public class BoreicDrillProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boreic Drill");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Tools/BoreicDrillProj"); }
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.MythrilDrill);
		}

	}

	public class BoreicJacksawProj : ModProjectile
	{
		public override string Texture
		{
			get { return ("SGAmod/Items/Tools/BoreicJacksawProj"); }
		}
		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 56;
			Projectile.aiStyle = 20;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = DamageClass.Melee;
		}
	}

}