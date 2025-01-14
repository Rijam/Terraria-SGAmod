
using Microsoft.Xna.Framework;
using SGAmod.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armors.Illuminant
{

	[AutoloadEquip(EquipType.Head)]
	public class IlluminantHelmet : ModItem
	{

		public static void ActivateAbility(SGAPlayer sgaplayer)
		{
			if (sgaplayer.illuminantSet.Item1 <= 4)
				return;

				if (sgaplayer.AddCooldownStack(60 * 60, 3))
			{
				int time = 60 * 10;
                while (sgaplayer.AddCooldownStack(60 * 60, 1))
                {
					time += 60 * 10;
				}

				sgaplayer.player.AddBuff(ModContent.BuffType<IlluminantBuff>(), time);
				Point spot = sgaplayer.player.Center.ToPoint();

				Main.PlaySound(SoundID.Item, spot.X, spot.Y, 78, 1f, -0.8f);
				var snd = Main.PlaySound(SoundID.DD2_EtherianPortalOpen, (int)spot.X, (int)spot.Y);
				if (snd != null)
				{
					snd.Pitch = -0.80f;
				}
			}
		}

		public static void IlluminantArmorDrop(int ammount,Vector2 where)
        {
			List<int> armors = new List<int> {ModContent.ItemType<IlluminantHelmet>(), ModContent.ItemType<IlluminantChestplate>(), ModContent.ItemType<IlluminantLeggings>() };
			armors = armors.OrderBy(testby => Main.rand.Next()).ToList();
			for (int i = 0; i < ammount; i += 1)
            {
				Item.NewItem(where, Vector2.Zero, armors[0]);
				armors.RemoveAt(0);
			}
        }

		public override bool Autoload(ref string name)
        {
			if (GetType() == typeof(IlluminantHelmet))
			SGAPlayer.PostUpdateEquipsEvent += SetBonus;
			return true;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Helmet");
			Tooltip.SetDefault("Gain an additional free Cooldown Stack");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0,50,0,0);
			item.rare = ItemRarityID.Purple;
			item.defense = 24;
		}

		public static void SetBonus(SGAPlayer sgaplayer)
        {
			if (sgaplayer.illuminantSet.Item1>0)
            {
				Player player = sgaplayer.player;

				Lighting.AddLight(player.MountedCenter, Color.HotPink.ToVector3() * (0.4f+(float)Math.Sin(Main.GlobalTime*3f)*0.3f) * sgaplayer.illuminantSet.Item2);

				if (sgaplayer.illuminantSet.Item1 > 4)
				{
					player.SGAPly().healingPointsStatBoost += 500;

					float bonusRate = player.HasBuff(ModContent.BuffType<IlluminantBuff>()) ? 2f : 1;
					//Main.NewText(sgaplayer.illuminantSet.Item2);
					player.BoostAllDamage(sgaplayer.activestacks * 0.04f* bonusRate, (int)(sgaplayer.activestacks*2* bonusRate));
					player.minionDamage += sgaplayer.activestacks * 0.02f* bonusRate;
					player.lifeRegen += (int)(sgaplayer.activestacks*2* bonusRate);

					sgaplayer.actionCooldownRate -= 0.20f;
					/*
					for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
					{
						Item thisitem = player.armor[i];
						if (thisitem.prefix > 0)
						{
							int itemtype = thisitem.type;
							thisitem.type = ItemID.None;
							player.VanillaUpdateEquip(thisitem);
							bool blank1 = false; bool blank2 = false; bool blank3 = false;
							//PlayerHooks.UpdateEquips(player, ref blank1, ref blank2, ref blank3);
							thisitem.type = itemtype;
						}
					}
					*/
				}
			}
			sgaplayer.illuminantSet.Item2 = 0;

		}

		public Color ArmorGlow(Player player, int index)
		{
			float mathy = (float)((Main.GlobalTime*4f) + (index / 1f));
			return Color.White*((0.4f)+((float)System.Math.Sin(mathy)*0.3f));
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.illuminantSet.Item2 += 1;
			sgaplayer.armorglowmasks[0] = "SGAmod/Items/Armors/Illuminant/" + Name+ "_Head";
			sgaplayer.armorglowcolor[0] = ArmorGlow;
		}
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod,typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.illuminantSet.Item1 += 1;
			sgaplayer.MaxCooldownStacks += 1;
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class IlluminantChestplate : IlluminantHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Chestplate");
			Tooltip.SetDefault("Gain an additional free Cooldown Stack");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 60, 0, 0);
			item.rare = ItemRarityID.Purple;
			item.defense = 35;
			item.lifeRegen = 3;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.illuminantSet.Item2 += 1;
			sgaplayer.armorglowmasks[1] = "SGAmod/Items/Armors/Illuminant/" + Name + "_Body";
				sgaplayer.armorglowmasks[2] = "SGAmod/Items/Armors/Illuminant/" + Name + "_Arms";
			sgaplayer.armorglowcolor[1] = ArmorGlow;
			sgaplayer.armorglowcolor[2] = ArmorGlow;
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class IlluminantLeggings : IlluminantHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Leggings");
			Tooltip.SetDefault("Gain an additional free Cooldown Stack");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 50, 0, 0);
			item.rare = ItemRarityID.Purple;
			item.defense = 16;
			item.lifeRegen = 2;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.illuminantSet.Item2 += 1;
			sgaplayer.armorglowmasks[3] = "SGAmod/Items/Armors/Illuminant/" + Name + "_Legs";
			sgaplayer.armorglowcolor[3] = ArmorGlow;
		}
	}

	public class IlluminantBuff : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/BuffTemplate";
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Illumination");
			Description.SetDefault("Cooldown Stacks do not Decay, Illuminant set bonus is stronger");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().noCooldownRate = true;
		}
	}

}