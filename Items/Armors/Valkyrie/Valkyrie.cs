
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Tiles;
using SGAmod.Tiles.TechTiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace SGAmod.Items.Armors.Valkyrie
{

	[AutoloadEquip(EquipType.Head)]
	public class ValkyrieHelm : ModItem,IAuroraItem
	{
        public override bool Autoload(ref string name)
        {
			if (GetType() == typeof(ValkyrieHelm))
			{
				SGAPlayer.PostUpdateEquipsEvent += SetBonus;
                SGAPlayer.PostPostUpdateEquipsEvent += SGAPlayer_PostPostUpdateEquipsEvent;
			}
			return true;
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valkyrie Helm");
			Tooltip.SetDefault("15% increased throwing damage, and 20% increased crit chance and velocity\nGrants life regeneration");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0,50,0,0);
			Item.rare = ItemRarityID.Cyan;
			Item.defense = 20;
			Item.lifeRegen = 2;
		}

		public static void ActivateRagnorok(SGAPlayer sgaply)
		{
			if (sgaply.AddCooldownStack(60 * 150,2))
			{
				sgaply.Player.AddBuff(ModContent.BuffType<RagnarokBuff>(), (int)(120*System.Math.Min(sgaply.Player.lifeRegen, sgaply.Player.lifeRegenTime * 0.01f)));
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

		public static void SetBonus(SGAPlayer sgaplayer)
		{
			if (sgaplayer.valkyrieSet.Item1)
			{
				Player player = sgaplayer.Player;

				if (!sgaplayer.valkyrieSet.Item3)
				sgaplayer.valkyrieSet.Item2 += (System.Math.Min(player.lifeRegen, player.lifeRegenTime * 0.01f) - sgaplayer.valkyrieSet.Item2)/30f;
				player.Throwing().thrownDamage += (sgaplayer.valkyrieSet.Item2+(sgaplayer.valkyrieSet.Item3 ? player.lifeRegen : 0)) * 0.02f;

				if (player.Male)
					player.endurance += 0.15f;

			}
		}
		private void SGAPlayer_PostPostUpdateEquipsEvent(SGAPlayer sgaplayer)
		{
			if (sgaplayer.valkyrieSet.Item1)
			{
				Player player = sgaplayer.Player;

				if (!player.Male)
					player.wingTimeMax = (int)(player.wingTimeMax * 1.20f);
			}
		}
		public Color ArmorGlow(Player player, int index)
		{
			float peak = MathHelper.Clamp(1f - ((float)System.Math.Abs(27000.00 - Main.time) / 60000f), 0f, 1f);
			//float valx = (float)System.Math.Sin(((Main.time / Main.dayLength)));
			//Main.NewText(valx);
			return Color.White * MathHelper.Clamp(1f- peak, 0f,1f);
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod,typeof(SGAPlayer).Name) as SGAPlayer;
			player.Throwing().thrownVelocity += 0.25f;
			player.Throwing().thrownDamage += 0.15f;
			player.Throwing().thrownCrit += 20;
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ && !Main.dayTime)
				sgaplayer.armorglowmasks[0] = "SGAmod/Items/Armors/Valkyrie/" + Name + "_Head";
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<AuroraTearAwoken>(), 1).AddIngredient(ModContent.ItemType<IlluminantEssence>(), 25).AddIngredient(ModContent.ItemType<StarMetalBar>(), 12).AddIngredient(ModContent.ItemType<LuminiteWraithNotch>(), 1).AddTile(ModContent.TileType<LuminousAlter>()).Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class ValkyrieBreastplate : ValkyrieHelm, IAuroraItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valkyrie Breastplate");
			Tooltip.SetDefault("10% increased throwing damage, velocity, and throwing attack speed\n75% chance to not consume Throwing Items\nGrants life regeneration");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 50, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.defense = 30;
			Item.lifeRegen = 3;
		}
		public override void UpdateEquip(Player player)
		{
			player.Throwing().thrownVelocity += 0.10f;
			player.Throwing().thrownDamage += 0.10f;
			player.SGAPly().Thrownsavingchance += 0.75f;
			player.SGAPly().ThrowingSpeed += 0.20f;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ && !Main.dayTime)
			{
				sgaplayer.armorglowmasks[1] = "SGAmod/Items/Armors/Valkyrie/" + Name + "_Body";
				sgaplayer.armorglowmasks[2] = "SGAmod/Items/Armors/Valkyrie/" + Name + "_Arms";
			}
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class ValkyrieLeggings : ValkyrieHelm, IAuroraItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valkyrie Leggings");
			Tooltip.SetDefault("10% increased throwing damage\n25% increase to movement speed\nFlight time and movement speed improved by 15% at night\nGrants life regeneration");
		}

        public override bool Autoload(ref string name)
        {
            SGAPlayer.PostPostUpdateEquipsEvent += SGAPlayer_PostPostUpdateEquipsEvent;
			return true;
        }

        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 50, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.defense = 15;
			Item.lifeRegen = 2;
		}
		private void SGAPlayer_PostPostUpdateEquipsEvent(SGAPlayer player)
		{
			if (!Main.dayTime && player.Player.armor[2].type == ModContent.ItemType<ValkyrieLeggings>())
				player.Player.wingTimeMax = (int)(player.Player.wingTimeMax * 1.15f);
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 1.25f*(!Main.dayTime ? 1.15f : 1f);
			player.accRunSpeed += 1.5f * (!Main.dayTime ? 1.15f : 1f);
			player.Throwing().thrownDamage += 0.10f;

		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ && !Main.dayTime)
				sgaplayer.armorglowmasks[3] = "SGAmod/Items/Armors/Valkyrie/" + Name + "_Legs";
		}
	}

	public class RagnarokBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ragnar�k");
			Description.SetDefault("The end is close!");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/RagnarokBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			SGAPlayer sgaply = player.SGAPly();
			sgaply.valkyrieSet.Item3 = true;
			player.lifeRegenTime = 0;
			player.lifeRegenCount = 0;
			sgaply.apocalypticalChance[3] += 3;
			sgaply.ThrowingSpeed += 0.4f;


			int dust = Dust.NewDust(player.Hitbox.TopLeft() + new Vector2(0, -8), player.Hitbox.Width, player.Hitbox.Height+16, 36);
			Main.dust[dust].scale = 3f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].alpha = 240;
			Main.dust[dust].velocity = (player.velocity * Main.rand.NextFloat(0.4f, 1.2f)) + Vector2.UnitX.RotatedBy(-MathHelper.PiOver2 + Main.rand.NextFloat(-0.6f, 0.6f)) * Main.rand.NextFloat(1f, 4f);

			dust = Dust.NewDust(player.Hitbox.TopLeft() + new Vector2((player.Hitbox.Width/2) - 8, 4), 16, 0, DustID.AncientLight);
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].alpha = 240;
			Main.dust[dust].velocity = (player.velocity * Main.rand.NextFloat(0.4f, 1.2f)) + Vector2.UnitX.RotatedBy(-MathHelper.PiOver2 + Main.rand.NextFloat(-0.6f, 0.6f)) * Main.rand.NextFloat(1f, 5f);

			dust = Dust.NewDust(player.Hitbox.TopLeft() + new Vector2((player.Hitbox.Width / 2) - 8, 4), 16, 0, 182);
			Main.dust[dust].scale = 0.25f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].alpha = 200;
			Main.dust[dust].fadeIn = 1f;
			Main.dust[dust].velocity = (player.velocity * Main.rand.NextFloat(0.75f, 1.2f)) + Vector2.UnitX.RotatedBy(-MathHelper.PiOver2 + Main.rand.NextFloat(-0.6f, 0.6f)) * Main.rand.NextFloat(0f, 1f);
		}
	}

}