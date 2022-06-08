
using Idglibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace SGAmod.Items.Armors.JungleTemplar
{

	[AutoloadEquip(EquipType.Head)]
	public class JungleTemplarHelmet : ModItem
	{

		public override bool Autoload(ref string name)
        {
			if (GetType() == typeof(JungleTemplarHelmet))
			{
				SGAPlayer.PreUpdateMovementEvent += SetBonusMovement;
			}
			return true;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Templar Helmet");
			Tooltip.SetDefault("20% increased Throwing crit chance and 10% increased Technological damage\n25% increase to not consume throwable\n+2000 Max Electric Charge\n+3 electric charge regen rate if exposed to the sun\n" + Idglib.ColorText(Color.Red, "5% less overall crit chance"));
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0,15,0,0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 25;
		}
        public override bool DrawHead()
        {
			return GetType() == typeof(JungleTemplarHelmet) ? false : base.DrawHead();
        }
		public static void ActivatePrecurserPower(SGAPlayer sgaply)
		{
			if (sgaply.Player.HasBuff(ModContent.BuffType<PrecurserPowerBuff>()))
			{
				sgaply.Player.DelBuff(sgaply.Player.FindBuffIndex(ModContent.BuffType<PrecurserPowerBuff>()));

				SoundEffectInstance sound2 = SoundEngine.PlaySound(SoundID.Zombie, (int)sgaply.Player.Center.X, (int)sgaply.Player.Center.Y, 35);
				if (sound2 != null)
				{
					sound2.Pitch = 0.5f;
				}
			}
			else 
			{
				if (sgaply.AddCooldownStack(60 * 80, 2))
				{
					sgaply.Player.AddBuff(ModContent.BuffType<PrecurserPowerBuff>(), 1200);
					SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, (int)sgaply.Player.Center.X, (int)sgaply.Player.Center.Y);
					if (sound != null)
					{
						sound.Pitch = 0.85f;
					}

					SoundEffectInstance sound2 = SoundEngine.PlaySound(SoundID.Zombie, (int)sgaply.Player.Center.X, (int)sgaply.Player.Center.Y, 35);
					if (sound2 != null)
					{
						sound2.Pitch = -0.5f;
					}

					for (int i = 0; i < 50; i += 1)
					{
						int dust = Dust.NewDust(sgaply.Player.Hitbox.TopLeft() + new Vector2(0, -8), sgaply.Player.Hitbox.Width, sgaply.Player.Hitbox.Height + 8, DustID.AncientLight);
						Main.dust[dust].scale = 2f;
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity = (sgaply.Player.velocity * Main.rand.NextFloat(0.75f, 1f)) + Vector2.UnitX.RotatedBy(-MathHelper.PiOver2 + Main.rand.NextFloat(-1.2f, 1.2f)) * Main.rand.NextFloat(1f, 3f);
					}
				}
			}
		}

		public static void SetBonusMovement(SGAPlayer sgaplayer)
		{
			if (sgaplayer.jungleTemplarSet.Item1)
			{
				if (sgaplayer.Player.velocity.Y != 0)
                {
					float gravity = sgaplayer.Player.velocity.Y > 0.50f ? 0.50f : 0.25f;
					sgaplayer.Player.velocity += Vector2.UnitY * sgaplayer.Player.gravDir * Player.defaultGravity * gravity;
                }
                else
                {
					sgaplayer.Player.thorns += 3f;
					sgaplayer.Player.noKnockback = true;
                }
			}
		}

		public static void SetBonus(SGAPlayer sgaplayer)
		{

			sgaplayer.Player.powerrun = true;

			if (sgaplayer.ShieldType == 0)
				sgaplayer.ShieldType = 100;

			if (sgaplayer.Player.velocity.Y > 0.50f)
			{
				sgaplayer.Player.maxFallSpeed += 5;
			}

			if (sgaplayer.jungleTemplarSet.Item2)
			{
				sgaplayer.Player.Throwing().GetDamage(DamageClass.Throwing) *= sgaplayer.techdamage;

				if (sgaplayer.ConsumeElectricCharge(8, 300, true))
				{
					sgaplayer.Player.shinyStone = true;
				}
			}

			if (sgaplayer.timer>300 && sgaplayer.Player.lavaTime > 120 && !sgaplayer.ConsumeElectricCharge(1000, 0, false, false))
            {
				sgaplayer.Player.AddBuff(ModContent.BuffType<Buffs.LavaBurn>(),30*(Main.expertMode ? 1 : 2));
			}

		}

		public Color ArmorGlow(Player player, int index)
		{
			return Color.White * (player.SGAPly().jungleTemplarSet.Item2 ? 1f : 0.5f);
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.SGAPly();
			sgaplayer.techdamage += 0.10f;
			sgaplayer.electricChargeMax += 2000;
			player.Throwing().thrownCrit += 20;
			sgaplayer.Thrownsavingchance += 0.25f;
			player.BoostAllDamage(0, -5);

			if (Collision.CanHitLine(player.Center, 0, 0, new Vector2(player.Center.X, 0), 0, 0))
            {
				sgaplayer.electricrechargerate += 3;
			}

		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			if (GetType() == typeof(JungleTemplarHelmet))
			{
				SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
				sgaplayer.armorglowmasks[0] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowcolor[0] = ArmorGlow;
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<AdvancedPlating>(), 8).AddIngredient(ItemID.LihzahrdPowerCell).AddIngredient(ItemID.LihzahrdBrick, 50).AddTile(TileID.LihzahrdAltar).Register();
		}

	}

	[AutoloadEquip(EquipType.Body)]
	public class JungleTemplarChestplate : JungleTemplarHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Templar Chestplate");
			Tooltip.SetDefault("25% increased throwing damage, +2 electric charge regen rate\nMax Lava time is increased by 10 seconds\n" + Idglib.ColorText(Color.Red, "5% less overall crit chance"));
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 32;
			Item.lifeRegen = 0;
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.SGAPly();
			player.lavaMax += 600;
			//sgaplayer.lavaBurn = true;
			sgaplayer.techdamage += 0.10f;
			player.Throwing().thrownDamage += 0.25f;
			sgaplayer.electricrechargerate += 2;
			player.BoostAllDamage(0, -5);
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.armorglowmasks[1] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
			sgaplayer.armorglowmasks[4] = "SGAmod/Items/GlowMasks/" + Name + "_GlowFemale";
			sgaplayer.armorglowcolor[1] = ArmorGlow;
			sgaplayer.armorglowcolor[4] = ArmorGlow;
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class JungleTemplarLeggings : JungleTemplarHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Templar Leggings");
			Tooltip.SetDefault("15% increased Throwing crit chance\n20% boost to throwing item use speed while grounded\nMax Lava time is increased by 5 seconds\nBeing submerged in lava grants +5 electric charge regen rate\n" + Idglib.ColorText(Color.Red, "5% less overall crit chance"));
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 18;
			Item.lifeRegen = 0;
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.SGAPly();
			player.lavaMax += 600;
			if (player.velocity.Y == 0)
			sgaplayer.ThrowingSpeed += 0.20f;
			player.Throwing().thrownCrit += 15;
			player.BoostAllDamage(0, -5);

			if (player.lavaWet)
            {
				sgaplayer.electricrechargerate += 5;
            }
		}
	}

	public class PrecurserPowerBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Precursor's Power");
			Description.SetDefault("Ancient energy powers you up!\nGain shiny stone recovery even while moving, but consumes electric charge!\nExtreme energy causes harm if not wearing the armor set");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/PrecursorsPowerBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			SGAPlayer sgaply = player.SGAPly();
			sgaply.jungleTemplarSet.Item2 = true;

			if (!sgaply.jungleTemplarSet.Item1)
            {
				sgaply.badLifeRegen -= 50;
            }

			int dust = Dust.NewDust(player.Hitbox.TopLeft() + new Vector2(0, -8), player.Hitbox.Width, player.Hitbox.Height + 16, 36);
			Main.dust[dust].scale = 3f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].alpha = 240;
			Main.dust[dust].velocity = (player.velocity * Main.rand.NextFloat(0.4f, 1.2f)) + Vector2.UnitX.RotatedBy(-MathHelper.PiOver2 + Main.rand.NextFloat(-0.6f, 0.6f)) * Main.rand.NextFloat(1f, 4f);


		}
	}

}