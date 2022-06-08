using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Tools
{
	public class VirulentPickaxe : MangrovePickaxe
	{
		public override void SetStaticDefaults()
		{
       			DisplayName.SetDefault("Virulent Pickaxe");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "Faster jungle Item", "25% faster use speed in the jungle biome"));
		}

		public override float UseTimeMultiplier(Player player)
		{
			if (player.ZoneJungle)
				return 1.25f;
			return 1f;
		}

		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Melee;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 9;
			Item.useAnimation = 20;
			Item.pick = 180;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.value = 3000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

        	public override void AddRecipes()
        	{
			CreateRecipe(1).AddIngredient(null, "MangrovePickaxe", 1).AddIngredient(mod.ItemType("VirulentBar"), 5).AddTile(TileID.MythrilAnvil).Register();
		}
	}
	public class VirulentDrill : VirulentPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virulent Drill");
		}

		public override void SetDefaults()
		{
			Item.damage = 19;
			Item.DamageType = DamageClass.Melee;
			Item.width = 56;
			Item.height = 22;
			Item.useTime = 8;
			Item.useAnimation = 25;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.pick = 180;
			Item.tileBoost += 1;
			Item.useStyle = 5;
			Item.knockBack = 0;
			Item.value = 3000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item23;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("VirulentDrill").Type;
			Item.shootSpeed = 40f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("VirulentBar"), 15).AddTile(TileID.MythrilAnvil).Register();
		}
	}
	public class VirulentHamaxe : VirulentPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virulent Hamaxe");
		}

		public override void SetDefaults()
		{
			Item.damage = 42;
			Item.DamageType = DamageClass.Melee;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 30;
			Item.useAnimation = 20;
			Item.axe = 17;
			Item.hammer = 85;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 3000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "MangroveHammer", 1).AddIngredient(null, "MangroveAxe", 1).AddIngredient(mod.ItemType("VirulentBar"), 5).AddTile(TileID.MythrilAnvil).Register();

		}
	}
	public class VirulentJacksaw : VirulentPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virulent Jacksaw");
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
			Item.shoot = Mod.Find<ModProjectile>("VirulentJacksaw").Type;
			Item.shootSpeed = 40f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("VirulentBar"), 15).AddTile(TileID.MythrilAnvil).Register();
		}
	}


}