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
	public class Dragonhead : EquipTexture
	{
		public override bool DrawHead()
		{
			return false;
		}
	}

	[AutoloadEquip(EquipType.Head)]
		public class IDGHead : MisterCreeperHead, IDevArmor
	{
		public override Color AwakenedColors => Color.Lime;
		public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("IDGCaptainRussia94's Dergon Disguise");
			}
			public override void InitEffects()
			{
				Item.defense = 25;
				Item.rare = 10;
			}

		public override bool DrawHead()
		{
			return false;
		}

		public static Color IDGGlow(Player player,int index)
		{
			 return Main.hslToRgb(((Main.GlobalTimeWrappedHourly+ (index/2f)) * 1.25f) % 1f, 0.8f, 0.75f)*0.75f;
		}

			public override void AddEffects(Player player)
			{
				player.maxMinions += 1;
				player.GetDamage(DamageClass.Summon) += 0.30f;
				player.GetDamage(DamageClass.Ranged) += 0.25f;
				player.GetCritChance(DamageClass.Ranged) += 15;
				player.maxTurrets += 1;
				player.GetModPlayer<SGAPlayer>().summonweaponspeed += 0.50f;
			}

			public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
			{
				tooltips.Add(new TooltipLine(Mod, "IDG", "+1 max minions, +1 max sentries"));
				tooltips.Add(new TooltipLine(Mod, "IDG", "25% increased ranged damage, 15% increased ranged crit chance"));
				tooltips.Add(new TooltipLine(Mod, "IDG", "30% increased summon damage, Summon weapons are used 50% faster"));
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
				Item.defense=0;
				Item.vanity = true;
			}
			public override void ModifyTooltips(List<TooltipLine> tooltips)
			{
				if (!Item.vanity)
				tooltips=AddText(tooltips);
				tooltips.Add(new TooltipLine(Mod, "IDG", "Great for impersonating a derg who uses code to cover his bad spriting skills"));
				Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 4) % 1f, 0.4f, 0.45f);
				tooltips.Add(new TooltipLine(Mod, "IDG Dev Item", Idglib.ColorText(c, "IDGCaptainRussia94's dev armor")));
			}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[0] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowcolor[0] = IDGHead.IDGGlow;
			}
		}
	}

	//sgaplayer.armorglowcolor[0] = delegate (Player player2, int index)
				//{
				////	return IDGHead.IDGGlow(player2, index);
				//};

[AutoloadEquip(EquipType.Body)]
		public class IDGBreastplate : IDGHead, IDevArmor
	{
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("IDGCaptainRussia94's Scaled Suit");
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
				Item.defense = 40;
				Item.rare = 10;
				Item.lifeRegen = 5;
			}
			public override void AddEffects(Player player)
			{
				player.maxMinions += 2;
				player.bulletDamage += 0.10f;
				player.rocketDamage += 0.20f;
				player.minionKB += 0.50f;
				player.GetModPlayer<SGAPlayer>().TrapDamageMul += 0.25f;
				player.GetModPlayer<SGAPlayer>().techdamage += 0.15f;
			}
			public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
			{
				tooltips.Add(new TooltipLine(Mod, "IDG", "50% increased minion knockback, +2 max minions"));
				tooltips.Add(new TooltipLine(Mod, "IDG", "10% increased bullet damage, 20% increased rocket damage"));
				tooltips.Add(new TooltipLine(Mod, "IDG", "15% increased Tech damage, 25% increased Trap damage"));
				tooltips.Add(new TooltipLine(Mod, "IDG", "Moderately increased Life Regen"));
				return tooltips;
			}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[1] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowcolor[1] = IDGHead.IDGGlow;
				sgaplayer.armorglowmasks[2] = "SGAmod/Items/GlowMasks/" + Name + "_ArmsGlow";
				sgaplayer.armorglowcolor[2] = IDGHead.IDGGlow;
			}
		}

	}

		[AutoloadEquip(EquipType.Legs)]
		public class IDGLegs : IDGHead, IDevArmor
	{
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("IDGCaptainRussia94's Dragon Dressings");
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
				Item.defense = 20;
				Item.rare = 10;
			}

		public override bool Autoload(ref string name)
		{
			SGAPlayer.PostPostUpdateEquipsEvent += SGAPlayer_PostPostUpdateEquipsEvent;
			return true;
		}

		private void SGAPlayer_PostPostUpdateEquipsEvent(SGAPlayer sgaplayer)
		{
			Player player = sgaplayer.Player;
			if (!player.armor[2].vanity && player.armor[2].type == Item.type)
			{
				player.wingTimeMax = (int)(player.wingTimeMax * 1.20f);
			}
		}

		public override void AddEffects(Player player)
			{
				player.moveSpeed += 2f;
				player.accRunSpeed += 2f;
				player.wingTimeMax = (int)((float)player.wingTimeMax*(1.20f));
				player.maxMinions += 1;
				player.statManaMax2 += 60;
				player.ammoCost80 = true;
				player.GetModPlayer<SGAPlayer>().boosterPowerLeftMax += (int)(10000f * 0.20f);
		}
			public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
			{
				tooltips.Add(new TooltipLine(Mod, "IDG", "+1 max minions, +60 Mana, 20% chance to not consume ammo"));
				tooltips.Add(new TooltipLine(Mod, "IDG", "Movement speed increased and Flight time improved by 20%"));
				tooltips.Add(new TooltipLine(Mod, "IDG", "20% increased Booster capacity"));
				return tooltips;
			}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[3] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowcolor[3] = IDGHead.IDGGlow;
			}
		}

	}
	public class DigiCurse : ModBuff
	{

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/AcidBurn";
			return true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Digi Curse");
			Description.SetDefault("You Take more damage");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().damagetaken += 0.10f;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().damagemul += 0.10f;
		}
	}

}