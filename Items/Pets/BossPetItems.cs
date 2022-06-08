using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Projectiles.Pets;
using SGAmod.Buffs.Pets;

namespace SGAmod.Items.Pets
{
	public class FrozenBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frozen Bow");
			Tooltip.SetDefault("Summons a pet ice fairy");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 28;
			Item.height = 26;
			Item.shoot = ModContent.ProjectileType<CutestIceFairy>();
			Item.buffType = ModContent.BuffType<CutestIceFairyBuff>();
			Item.rare = ItemRarityID.LightRed;
			Item.expert = true; //Change to Master Mode in 1.4
			Item.value = 250000;
			Item.UseSound = SoundID.Item25;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
	public class SpiderlingEggs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiderling Eggs");
			Tooltip.SetDefault("Summons a pet acidic spiderling");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 30;
			Item.height = 26;
			Item.shoot = ModContent.ProjectileType<AcidicSpiderling>();
			Item.buffType = ModContent.BuffType<AcidicSpiderlingBuff>();
			Item.rare = ItemRarityID.Green;
			Item.expert = true;
			Item.value = 250000;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
	public class CopperTack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Tack");
			Tooltip.SetDefault("Summons a pet Copper Wraith");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 20;
			Item.height = 22;
			Item.shoot = ModContent.ProjectileType<MiniCopperWraith>();
			Item.buffType = ModContent.BuffType<MiniCopperWraithBuff>();
			Item.rare = ItemRarityID.Blue;
			Item.expert = true;
			Item.value = 250000;
			Item.UseSound = SoundID.Item46;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
	public class TinTack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tin Tack");
			Tooltip.SetDefault("Summons a pet Tin Wraith");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 20;
			Item.height = 22;
			Item.shoot = ModContent.ProjectileType<MiniTinWraith>();
			Item.buffType = ModContent.BuffType<MiniTinWraithBuff>();
			Item.rare = ItemRarityID.Blue;
			Item.expert = true;
			Item.value = 250000;
			Item.UseSound = SoundID.Item46;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
	public class CobaltTack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Tack");
			Tooltip.SetDefault("Summons a pet Cobalt Wraith");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 20;
			Item.height = 22;
			Item.shoot = ModContent.ProjectileType<MiniCobaltWraith>();
			Item.buffType = ModContent.BuffType<MiniCobaltWraithBuff>();
			Item.rare = ItemRarityID.LightRed;
			Item.expert = true;
			Item.value = 250000;
			Item.UseSound = SoundID.Item46;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
	public class PalladiumTack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Palladium Tack");
			Tooltip.SetDefault("Summons a pet Palladium Wraith");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 20;
			Item.height = 22;
			Item.shoot = ModContent.ProjectileType<MiniPalladiumWraith>();
			Item.buffType = ModContent.BuffType<MiniPalladiumWraithBuff>();
			Item.rare = ItemRarityID.LightRed;
			Item.expert = true;
			Item.value = 250000;
			Item.UseSound = SoundID.Item46;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
	public class LuminiteTack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminite Tack");
			Tooltip.SetDefault("Summons a pet Luminite Wraith");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 20;
			Item.height = 22;
			Item.shoot = ModContent.ProjectileType<MiniLuminiteWraith>();
			Item.buffType = ModContent.BuffType<MiniLuminiteWraithBuff>();
			Item.rare = ItemRarityID.Purple;
			Item.expert = true;
			Item.value = 250000;
			Item.UseSound = SoundID.Item46;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
	public class VisitantStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Visitant Star");
			Tooltip.SetDefault("Summons a pet Prismic Visitant");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 22;
			Item.height = 30;
			Item.shoot = ModContent.ProjectileType<PrismicVisitant>();
			Item.buffType = ModContent.BuffType<PrismicVisitantBuff>();
			Item.rare = ItemRarityID.Purple;
			Item.expert = true;
			Item.value = 250000;
			Item.UseSound = SoundID.Item25;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
	public class ImperatorialOdious : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imperatorial Odious");
			Tooltip.SetDefault("Summons a pet Slime Baron");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 18;
			Item.height = 28;
			Item.shoot = ModContent.ProjectileType<SlimeBaron>();
			Item.buffType = ModContent.BuffType<SlimeBaronBuff>();
			Item.rare = ItemRarityID.Orange;
			Item.expert = true;
			Item.value = 250000;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
	public class MonarchicalCate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Monarchical Cate");
			Tooltip.SetDefault("Summons a pet Slime Duchess");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DD2PetGato);
			Item.width = 26;
			Item.height = 28;
			Item.shoot = ModContent.ProjectileType<SlimeDuchess>();
			Item.buffType = ModContent.BuffType<SlimeDuchessBuff>();
			Item.rare = ItemRarityID.Purple;
			Item.expert = true;
			Item.value = 250000;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}
	}
}