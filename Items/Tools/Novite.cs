using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Items.Weapons.SeriousSam;

namespace SGAmod.Items.Tools
{
	public class NoviteDrill : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Drill");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.MythrilDrill);
			Item.shoot = ModContent.ProjectileType<NoviteDrillProj>();
			Item.useAnimation = 40;
			Item.value = Item.buyPrice(0, 0, 50, 0);
			Item.useTime = 12;
			Item.damage = 8;
			Item.pick = 55;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("NoviteBar"), 12).AddTile(TileID.Anvils).Register();
		}

	}

	public class NoviteDrillProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Drill");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Tools/NoviteDrillProj"); }
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.MythrilDrill);
		}

	}

	public class NoviteChainsaw : NoviteDrill
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Chainsaw");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.MythrilDrill);
			Item.damage = 7;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.tileBoost = -2;
			Item.axe = 15;
			Item.pick = 0;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<NoviteChainsawProj>();
		}

	}

	public class NoviteChainsawProj : NoviteDrillProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Chainsaw");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Tools/NoviteChainsawProj"); }
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.MythrilDrill);
		}

	}

}