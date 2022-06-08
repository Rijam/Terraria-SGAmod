
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace SGAmod.Items.Armors.Acid
{

	[AutoloadEquip(EquipType.Head)]
	public class AcidHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fames Helm");
			Tooltip.SetDefault("6% increased Throwing crit chance and Throwing Velocity");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0,0,75,0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}

		public static void ActivateHungerOfFames(SGAPlayer sgaply)
        {
			if (sgaply.AddCooldownStack(60 * 60))
            {
				sgaply.Player.AddBuff(ModContent.BuffType<FamesHungerBuff>(),300);
				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, (int)sgaply.Player.Center.X, (int)sgaply.Player.Center.Y);
				if (sound != null)
				{
					sound.Pitch = 0.5f;
				}

				SoundEffectInstance sound2 = SoundEngine.PlaySound(SoundID.Zombie, (int)sgaply.Player.Center.X, (int)sgaply.Player.Center.Y, 35);
				if (sound2 != null)
				{
					sound2.Pitch = -0.5f;
				}

				for(int i = 0; i < 50; i += 1)
                {
					int dust = Dust.NewDust(sgaply.Player.Hitbox.TopLeft() + new Vector2(0, -8), sgaply.Player.Hitbox.Width, sgaply.Player.Hitbox.Height+8, ModContent.DustType<Dusts.AcidDust>());
					Main.dust[dust].scale = 2f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = (sgaply.Player.velocity * Main.rand.NextFloat(0.75f, 1f)) + Vector2.UnitX.RotatedBy(-MathHelper.PiOver2 + Main.rand.NextFloat(-1.2f, 1.2f))*Main.rand.NextFloat(1f,3f);
				}
			}
		}

        public override bool DrawHead()
        {
			return false;
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
			player.Throwing().thrownCrit += 6;
			player.Throwing().thrownVelocity += 0.06f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<VialofAcid>(), 16).AddRecipeGroup("SGAmod:NoviteNovusBars", 8).AddTile(TileID.Anvils).Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class AcidChestplate : AcidHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fames Chestplate");
			Tooltip.SetDefault("6% increased Throwing Damage and 10% increased Throwing Velocity");
		}
		public override bool DrawHead()
		{
			return true;
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 8;
		}
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			player.Throwing().thrownDamage += 0.06f;
			player.Throwing().thrownVelocity += 0.10f;
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[1] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowmasks[2] = "SGAmod/Items/GlowMasks/" + Name + "_ArmsGlow";
				sgaplayer.armorglowmasks[4] = "SGAmod/Items/GlowMasks/" + Name + "_GlowFemale";
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<VialofAcid>(), 24).AddRecipeGroup("SGAmod:NoviteNovusBars", 10).AddTile(TileID.Anvils).Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class AcidLeggings : AcidHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fames Leggings");
			Tooltip.SetDefault("6% increased Throwing Damage\n15% increased movement speed");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
				sgaplayer.armorglowmasks[3] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
		}
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			player.Throwing().thrownDamage += 0.06f;
			player.moveSpeed += 1.15f;
			player.accRunSpeed += 1.5f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<VialofAcid>(), 12).AddRecipeGroup("SGAmod:NoviteNovusBars", 6).AddTile(TileID.Anvils).Register();
		}
	}

	public class FamesHungerBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hunger of Fames");
			Description.SetDefault("Acidically consume everything, even yourself");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/HungerOfFamesBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			SGAPlayer sgaply = player.SGAPly();
			sgaply.acidSet = (sgaply.acidSet.Item1, true);
			player.lifeRegenTime = 0;
			player.lifeRegenCount = 0;

			int dust = Dust.NewDust(player.Hitbox.BottomLeft() + new Vector2(0, -12), player.Hitbox.Width, 12, ModContent.DustType<Dusts.AcidDust>());
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = (player.velocity * Main.rand.NextFloat(0.9f, 1f)) + Vector2.UnitX.RotatedBy(-MathHelper.PiOver2 + Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(1f, 3f);

		}
	}


}