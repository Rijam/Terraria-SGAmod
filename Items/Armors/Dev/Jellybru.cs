using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Armors.Dev
{

	[AutoloadEquip(EquipType.Head)]
	public class JellybruHelmet : MisterCreeperHead, IDevArmor
	{
		public override bool Autoload(ref string name)
		{
			if (GetType() == typeof(JellybruHelmet))
			{
				SGAPlayer.PostUpdateEquipsEvent += SetBonus;
			}
			return true;
		}
		public override Color AwakenedColors => Color.Purple;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jellybru's Mask");
		}
		public override void InitEffects()
		{
			Item.defense = 8;
			Item.rare = ItemRarityID.Purple;
		}

		public static void SetBonus(SGAPlayer sgaplayer)
		{
			if (sgaplayer.jellybruSet)
			{
				Player player = sgaplayer.Player;

				float thepercent = 0.5f;

				int percentLife = (int)((player.statLifeMax2) * thepercent);

				percentLife = (int)((percentLife * 2) * player.GetDamage(DamageClass.Magic));

				sgaplayer.energyShieldAmmountAndRecharge.Item2 += percentLife;
				//Main.NewText(sgaplayer.energyShieldReservation);
				sgaplayer.energyShieldReservation += (1f - sgaplayer.energyShieldReservation) * thepercent;

				sgaplayer.ShieldType = 1001;

				if (!sgaplayer.EnergyDepleted)
				{
					Item itemxx = new Item();
					itemxx.SetDefaults(ItemID.AnkhCharm);
					bool falsebool = false; bool falsebool2 = false; bool falsebool3 = false;
					player.VanillaUpdateAccessory(0,itemxx,false,ref falsebool,ref falsebool2, ref falsebool3);
				}
			}
		}

		public override bool DrawHead()
		{
			return false;
		}

		public static Color IDGGlow(Player player, int index)
		{
			return Main.hslToRgb(((Main.GlobalTimeWrappedHourly + (index / 2f)) * 1.25f) % 1f, 0.8f, 0.75f) * 0.75f;
		}

		public override void AddEffects(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.14f;
			player.GetCritChance(DamageClass.Magic) += 14;
			player.statManaMax2 += 50;
			player.manaRegenBonus += player.SGAPly().EnergyDepleted ? 250 : 100;
			player.manaRegenDelayBonus += 2;
			player.SGAPly().DoTResist += 0.30f;
		}

		public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "Jellybru", "+14% magic damage and crit chance, +50 Mana"));
			tooltips.Add(new TooltipLine(Mod, "Jellybru", "Mana regen is greatly improved, Regen delay is reduced"));
			tooltips.Add(new TooltipLine(Mod, "Jellybru", Idglib.ColorText(Color.Red, "30% Increased DoT damage taken")));
			tooltips.Add(new TooltipLine(Mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "--When Shield Down--")));
			tooltips.Add(new TooltipLine(Mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "Mana regen is improved to extreme levels!")));
			return tooltips;
		}

		public override void UpdateEquip(Player player)
		{
			if (!Item.vanity)
			{
				AddEffects(player);
			}
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = 1;
			Item.defense = 0;
			Item.vanity = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!Item.vanity)
				tooltips = AddText(tooltips);
			tooltips.Add(new TooltipLine(Mod, "Jellybru", "Great for impersonating..."));
			Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 4) % 1f, 0.4f, 0.45f);
			tooltips.Add(new TooltipLine(Mod, "Jellybru Dev Item", Idglib.ColorText(c, "Jellybru's dev armor")));
		}
	}

	//sgaplayer.armorglowcolor[0] = delegate (Player player2, int index)
				//{
				////	return IDGHead.IDGGlow(player2, index);
				//};

[AutoloadEquip(EquipType.Body)]
		public class JellybruChestplate : JellybruHelmet, IDevArmor
	{
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Jellybru's Purpled Padding");
			}
			public override void SetDefaults()
			{
				Item.width = 18;
				Item.height = 18;
				Item.value = 10000;
				Item.rare = 1;
				Item.defense = 0;
				Item.vanity = true;
			}
			public override void InitEffects()
			{
				Item.defense = 10;
				Item.rare = ItemRarityID.Purple;
				Item.lifeRegen = 3;
			}
			public override void AddEffects(Player player)
			{
			SGAPlayer sgaply = player.SGAPly();
				player.GetDamage(DamageClass.Magic) += 0.16f;
				player.manaCost *= 0.80f;
				player.statManaMax2 += 100;
			player.SGAPly().DoTResist += 0.50f;

			if (sgaply.EnergyDepleted)
			{
				player.nebulaLevelDamage = 3;
				player.nebulaLevelLife = 3;
				player.nebulaLevelMana = 3;
			}
		}
		public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
			{
			tooltips.Add(new TooltipLine(Mod, "Jellybru", "+16% magic damage, +100 Mana"));
			tooltips.Add(new TooltipLine(Mod, "Jellybru", "magic cost reduced by 20%, minorly increased Life Regen"));
			tooltips.Add(new TooltipLine(Mod, "Jellybru", Idglib.ColorText(Color.Red, "50% Increased DoT damage taken")));
			tooltips.Add(new TooltipLine(Mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "--When Shield Down--")));
			tooltips.Add(new TooltipLine(Mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "Gain the powers of the nebula pillar!")));
			return tooltips;
			}

	}

		[AutoloadEquip(EquipType.Legs)]
		public class JellybruLeggings : JellybruHelmet, IDevArmor
	{
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Jellybru's Bre's Breeches");
			}
			public override void SetDefaults()
			{
				Item.width = 18;
				Item.height = 18;
				Item.value = 10000;
				Item.rare = 1;
				Item.defense = 0;
				Item.vanity = true;
			}

			public override void InitEffects()
			{
			Item.defense = 6;
			Item.rare = ItemRarityID.Purple;
		}
		public override void AddEffects(Player player)
		{

			SGAPlayer sgaplayer = player.SGAPly();

			player.GetDamage(DamageClass.Magic) += 0.15f;
			player.statManaMax2 += 50;
			player.moveSpeed += sgaplayer.EnergyDepleted ? 6f : 2f;
			player.accRunSpeed += sgaplayer.EnergyDepleted ? 6f : 2f;
			player.SGAPly().DoTResist += 0.20f;

		}
		public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "Jellybru", "+15% magic damage, +50 Mana"));
			tooltips.Add(new TooltipLine(Mod, "Jellybru", "Movement and horizontal flight speed increased"));
			tooltips.Add(new TooltipLine(Mod, "Jellybru", Idglib.ColorText(Color.Red, "20% Increased DoT damage taken")));
			tooltips.Add(new TooltipLine(Mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "--When Shield Down--")));
			tooltips.Add(new TooltipLine(Mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "Gain a great speed increase!")));
			return tooltips;
		}
	}
}