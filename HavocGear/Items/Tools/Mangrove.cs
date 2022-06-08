using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Tools
{
	public class MangrovePickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Pickaxe");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "Faster jungle Item", "15% faster use speed in the jungle biome"));
		}

		public override float UseTimeMultiplier(Player player)
		{
			if (player.ZoneJungle)
			return 1.15f;
			return 1f;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Item.useStyle == 1)
			{
				if (Main.rand.Next(5) == 0)
				{
					SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.GrassBlades, 0,0,100, Color.Yellow, 1f);

				}
			}
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.MoltenPickaxe);
			Item.damage = 8;
			Item.DamageType = DamageClass.Melee;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 18;
			Item.useAnimation = 20;
			Item.pick = 56;
			Item.useStyle = 1;
			Item.knockBack = 3;
			Item.value = 3000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "BiomassBar", 8).AddIngredient(null, "DankWood", 15).AddTile(TileID.Anvils).Register();
		}
	}
	public class MangroveAxe : MangrovePickaxe
	{
		public override void SetStaticDefaults()
		{
       			DisplayName.SetDefault("Mangrove Axe");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.MoltenHamaxe);
			Item.damage = 12;
			Item.DamageType = DamageClass.Melee;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 30;
			Item.useAnimation = 20;
			Item.axe = 12;
			Item.hammer = 0;
			Item.useStyle = 1;
			Item.knockBack = 5;
			Item.value = 3000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

        	public override void AddRecipes()
        	{
            		CreateRecipe(1).AddIngredient(null, "BiomassBar", 7).AddIngredient(null, "DankWood", 15).AddTile(TileID.Anvils).Register();
        	}
	}

	public class MangroveHammer : MangrovePickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Hammer");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.MoltenHamaxe);
			Item.damage = 14;
			Item.DamageType = DamageClass.Melee;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 30;
			Item.useAnimation = 20;
			Item.hammer = 59;
			Item.axe = 0;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 3000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "BiomassBar", 7).AddIngredient(null, "DankWoodHammer", 1).AddTile(TileID.Anvils).Register();
		}
	}

}