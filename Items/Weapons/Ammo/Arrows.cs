using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons.Ammo
{
	public class UnmanedArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Arrow");
			Tooltip.SetDefault("Arrows slightly home in on nearby enemies");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/UnmanedArrow"); }
		}
		public override void SetDefaults()
		{
			Item.damage = 9;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 1.5f;
			Item.value = 10;
			Item.rare = 1;
			Item.shoot = Mod.Find<ModProjectile>("UnmanedArrow").Type;   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 2.5f;                  //The speed of the projectile
			Item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(ItemID.WoodenArrow, 50).AddIngredient(mod.ItemType("CopperWraithNotch"), 1).AddIngredient(mod.ItemType("UnmanedBar"), 1).AddTile(TileID.Anvils).Register();
		}
	}
	public class UnmanedArrow2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Notchvos Arrow");
			Tooltip.SetDefault("Improved arrows that better home in on nearby enemies\nArrows travel in the reverse direction after hitting an enemy\nCan hit a total of 2 times, ignores invulnerability frames");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/UnmanedArrow2"); }
		}
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 2f;
			Item.value = 25;
			Item.rare = 3;
			Item.shoot = Mod.Find<ModProjectile>("UnmanedArrow2").Type;   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 3.5f;                  //The speed of the projectile
			Item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(mod.ItemType("UnmanedArrow"), 50).AddIngredient(mod.ItemType("CobaltWraithNotch"), 2).AddTile(TileID.MythrilAnvil).Register();
		}
	}

	public class PitchArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pitched Arrow");
			Tooltip.SetDefault("Inflicts Oiled on enemies\nHas a small chance to affect immune enemies for half the debuff time\nOiled enemies will be ignited for an extended time from Thermal Blaze and take even more damage");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/PitchArrow"); }
		}
		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 2f;
			Item.value = 25;
			Item.rare = 3;
			Item.shoot = Mod.Find<ModProjectile>("PitchArrow").Type;   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 3.5f;                  //The speed of the projectile
			Item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(ItemID.WoodenArrow, 50).AddIngredient(mod.ItemType("BottledMud"), 1).AddIngredient(mod.ItemType("MurkyGel"), 3).AddTile(TileID.Anvils).Register();
		}
	}

	public class DosedArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Doused Arrow");
			Tooltip.SetDefault("Has a chance to inflict Doused on enemies\nUse in combo with burning weapons to ignite foes\nExplodes against burning targets, otherwise it will bounce off them once\nSometimes inflict Oiled (bypassing immunity) and homes in on enemies");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/DosedArrow"); }
		}
		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 4f;
			Item.value = 500;
			Item.rare = 7;
			Item.shoot = Mod.Find<ModProjectile>("DosedArrow").Type;   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 12f;                  //The speed of the projectile
			Item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe(200).AddIngredient(mod.ItemType("PitchArrow"), 100).AddIngredient(mod.ItemType("UnmanedArrow2"), 100).AddIngredient(mod.ItemType("GasPasser"), 1).AddIngredient(mod.ItemType("LuminiteWraithNotch"), 1).AddTile(TileID.LunarCraftingStation).Register();
		}
	}

	public class WraithArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrath Arrow");
			Tooltip.SetDefault("Inflicts Betsy's Curse, and explodes like Aerial Bane Arrows on hit (no extra arrows are spawned however)\n'Because of course we need this too :-p'");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/WraithArrow"); }
		}
		public override void SetDefaults()
		{
			Item.damage = 15;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 4f;
			Item.value = 500;
			Item.rare = ItemRarityID.Lime;
			Item.shoot = Mod.Find<ModProjectile>("WraithArrow").Type;   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 18f;                  //The speed of the projectile
			Item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe(250).AddIngredient(ItemID.HellfireArrow, 250).AddIngredient(ItemID.WrathPotion, 1).AddIngredient(ItemID.DefenderMedal, 5).AddIngredient(mod.ItemType("StarMetalBar"), 2).AddIngredient(mod.ItemType("OmegaSigil"), 1).AddTile(TileID.LunarCraftingStation).Register();
		}
	}
	public class DankArrow : ModItem,IDankSlowText
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dank Arrow");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			Item.damage = 5;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 0f;
			Item.value = 50;
			Item.rare = 1;
			Item.shoot = Mod.Find<ModProjectile>("DankArrow").Type;   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 5f;                  //The speed of the projectile
			Item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			CreateRecipe(250).AddIngredient(mod.ItemType("DankWood"), 25).AddIngredient(mod.ItemType("DankCore"), 1).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class PrismicArrow : PrismicBullet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lesser Prismic Arrow");
			Tooltip.SetDefault("Shots cycle through your 2nd and 3rd ammo slots while placed in your first\nDefaults to a weak wooden arrow\nHas a 66% to not consume the fired ammo type");
		}
		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.WoodenArrow); }
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodenArrow);
			Item.damage = 2;
			Item.DamageType = DamageClass.Ranged;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.value = 100;
			Item.rare = 5;
			Item.shoot = ProjectileID.WoodenArrowFriendly;   //The projectile shoot when your weapon using this ammo
			Item.ammo = AmmoID.Arrow;
		}

		public override void AddRecipes()
		{
			CreateRecipe(250).AddRecipeGroup("SGAmod:Tier1Bars", 1).AddRecipeGroup("SGAmod:Tier2Bars", 1).AddRecipeGroup("SGAmod:Tier3Bars", 1).AddRecipeGroup("SGAmod:Tier4Bars", 1).AddIngredient(mod.ItemType("WraithFragment3"), 2).AddIngredient(ItemID.WoodenArrow, 250).AddTile(TileID.ImbuingStation).Register();
		}
	}

	public class PrismalArrow : PrismalBullet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Arrow");
			Tooltip.SetDefault("Highly increased damage over its precursor\nCycles through your ammo slots when placed in your first; defaults to Wooden Arrows\nHas a 75% to not consume the fired ammo type");
		}
		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.WoodenArrow); }
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodenArrow);
			Item.damage = 18;
			Item.DamageType = DamageClass.Ranged;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.value = 1000;
			Item.rare = 10;
			Item.shoot = ProjectileID.WoodenArrowFriendly;   //The projectile shoot when your weapon using this ammo
			Item.ammo = AmmoID.Arrow;
		}
		public override void AddRecipes()
		{
			CreateRecipe(250).AddIngredient(mod.ItemType("PrismalBar"), 1).AddIngredient(mod.ItemType("PrismicArrow"), 250).AddTile(TileID.ImbuingStation).Register();
		}
	}
}
