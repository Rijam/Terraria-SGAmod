using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using Idglibrary;
using Terraria.Audio;


namespace SGAmod.Items.Armors.Dev
{

	[AutoloadEquip(EquipType.Head)]
	public class MisterCreeperHead : ModItem, IDevArmor
	{
		public virtual Color AwakenedColors => Color.Orange;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mister Creeper's Crowning Attire");
		}
		public virtual void InitEffects()
		{
			Item.defense = 40;
			Item.rare = 10;
		}
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag["vanity"] = Item.vanity;
			return tag;
		}
		public override void Load(TagCompound tag)
		{
			Item.vanity = tag.GetBool("vanity");
			if (!Item.vanity)
			{
				Item.vanity = false;
				InitEffects();
			}
		}
		public override void NetSend(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = Item.vanity;
			writer.Write(flags);
		}

		public override void NetReceive(BinaryReader reader)
		{
			bool beforevanity = Item.vanity;
			Item.vanity = reader.ReadBoolean();
			if (beforevanity != Item.vanity && Item.vanity==false)
			{
			InitEffects();
			}

		}
		public virtual void AddEffects(Player player)
		{
			player.Throwing().thrownCrit += 22;
			player.Throwing().thrownCost33 = true;
			player.Throwing().thrownDamage += 0.10f;
			player.GetCritChance(DamageClass.Melee) += 18;
			player.GetDamage(DamageClass.Melee) += 0.10f;
			player.meleeSpeed += 0.25f;
			player.SGAPly().ThrowingSpeed += 0.15f;
			player.BoostAllDamage(-0.10f, -10);
			player.statLifeMax2 += 75;
			player.SGAPly().DoTResist *= 0.75f;
		}
		public virtual List<TooltipLine> AddText(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", "8% Increased Melee Crit, 12% increased throwing Crit"));
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", "33% to not consume thrown items, 25% increased melee swing speed"));
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", "15% increased throwing rate, 75 increased max life"));
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", "25% DoT resistance"));
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", Idglib.ColorText(Color.Red, "10% reduced non-melee/throwing damage and crit chance")));
			return tooltips;
		}

		public override void UpdateInventory(Player player)
		{
			if (Item.vanity)
			{
				if (player.GetModPlayer<SGAPlayer>().devpower>0)
				{
					Item.vanity = false;
					//Client Side
					if (Main.myPlayer == player.whoAmI)
					{
						CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), AwakenedColors, "???!!!", false, false);
						SoundEngine.PlaySound(29, (int)player.position.X, (int)player.position.Y, 105, 1f, -0.6f);
					}
					InitEffects();
				}
			}
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
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", "Great for impersonating an explosive fella who draws too many swords"));
			Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 4) % 1f, 0.4f, 0.45f);
			tooltips.Add(new TooltipLine(Mod, "IDG Dev Item", Idglib.ColorText(c, "MisterCreeper's (Legecy) Dev Armor")));
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class MisterCreeperBody : MisterCreeperHead, IDevArmor
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mister Creeper's Slick Suit");
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
			Item.defense = 50;
			Item.rare = 10;
			Item.lifeRegen = 10;
		}
		public override void AddEffects(Player player)
		{
			player.noKnockback = true;
			player.endurance += 0.20f;
			player.Throwing().thrownDamage += 0.42f;
			player.GetDamage(DamageClass.Melee) += 0.35f;

			player.Throwing().thrownCrit += 10;
			player.GetCritChance(DamageClass.Melee) += 10;

			player.BoostAllDamage(-0.10f, -10);

			player.statLifeMax2 += 100;
			player.SGAPly().DoTResist *= 0.35f;
		}
		public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", "25% increased melee damage, 32% increased throwing damage"));
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", "Immunity to Knockback, greatly increased Life Regen"));
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", "20% improved Endurance, 100 increased max life"));
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", "35% DoT resistance"));
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", Idglib.ColorText(Color.Red, "10% reduced non-melee/throwing damage and crit chance")));
			return tooltips;
		}

	}

	[AutoloadEquip(EquipType.Legs)]
	public class MisterCreeperLegs : MisterCreeperHead, IDevArmor
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mister Creeper's Business Casual");
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

        public override void InitEffects()
		{
			Item.defense = 30;
			Item.rare = 10;
		}
		public override void AddEffects(Player player)
		{
			player.moveSpeed += 3f;
			player.accRunSpeed += 3f;
			player.GetModPlayer<SGAPlayer>().Noselfdamage = true;
			player.BoostAllDamage(-0.10f,-10);

			player.Throwing().thrownCrit += 10;
			player.GetCritChance(DamageClass.Melee) += 10;
			player.GetDamage(DamageClass.Melee) += 0.10f;
			player.Throwing().thrownDamage += 0.10f;

			player.statLifeMax2 += 75;
			player.SGAPly().DoTResist *= 0.85f;

		}
		public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", "Movement speed increased and Flight time improved by 20%"));
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", "you don't take ANY self damage (includes fall and explosive damage)"));
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 60 seconds")));
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", "75 increased max life"));
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", "15% DoT resistance"));
			tooltips.Add(new TooltipLine(Mod, "MisterCreeper", Idglib.ColorText(Color.Red, "10% reduced non-melee/throwing damage and crit chance")));
			return tooltips;
		}

	}


}