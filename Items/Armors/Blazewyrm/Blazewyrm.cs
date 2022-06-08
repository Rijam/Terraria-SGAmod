using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armors.Blazewyrm
{

	[AutoloadEquip(EquipType.Head)]
	public class BlazewyrmHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazewyrm Helm");
			Tooltip.SetDefault("1% increased Melee Apocalyptical Chance\n20% faster melee speed and 16% more melee damage");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
			Item.defense = 10;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			sgaplayer.armorglowmasks[0] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
		}
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod,typeof(SGAPlayer).Name) as SGAPlayer;
			player.meleeSpeed += 0.20f;
			player.GetDamage(DamageClass.Melee) += 0.16f;
			sgaplayer.apocalypticalChance[0] += 1.0;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.MoltenHelmet, 1).AddIngredient(mod.ItemType("UnmanedBar"), 6).AddIngredient(mod.ItemType("FieryShard"), 8).AddTile(TileID.MythrilAnvil).Register();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "accapocalypticaltext", SGAGlobalItem.apocalypticaltext));
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class BlazewyrmBreastplate : BlazewyrmHelm
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazewyrm Breastplate");
			Tooltip.SetDefault("1% increased Melee Apocalyptical Chance\n12% increased melee crit chance");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
			Item.defense = 14;
		}
		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Melee) += 12;
			player.SGAPly().apocalypticalChance[0] += 1.0;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[1] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowmasks[2] = "SGAmod/Items/GlowMasks/" + Name + "_ArmsGlow";
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.MoltenBreastplate, 1).AddIngredient(mod.ItemType("UnmanedBar"), 8).AddIngredient(mod.ItemType("FieryShard"), 12).AddTile(TileID.MythrilAnvil).Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class BlazewyrmLeggings : BlazewyrmHelm
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazewyrm Leggings");
			Tooltip.SetDefault("1% increased Melee Apocalyptical Chance\n25% increase to movement speed\nEven faster in lava");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;
			Item.defense = 8;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
				sgaplayer.armorglowmasks[3] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 1.25f;
			player.accRunSpeed += 1.5f;
			if (player.lavaWet)
			{
				player.moveSpeed *= 1.2f;
				player.accRunSpeed *= 1.2f;
			}
			player.SGAPly().apocalypticalChance[0] += 1.0;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.MoltenGreaves, 1).AddIngredient(mod.ItemType("UnmanedBar"), 6).AddIngredient(mod.ItemType("FieryShard"), 10).AddTile(TileID.MythrilAnvil).Register();
		}
	}


}