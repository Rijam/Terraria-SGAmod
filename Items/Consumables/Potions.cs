using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SGAmod.Generation;
using SGAmod;
using Idglibrary;
using SGAmod.HavocGear.Items;
using SGAmod.Buffs;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Tiles;
using System.Collections.Generic;
//using SubworldLibrary;

namespace SGAmod.Items.Consumables
{

	public class SpaceBenderPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Bender Potion");
			Tooltip.SetDefault("'Distance doesn't matter when you flatten space like paper'\nGrants unlimited flight\n" + Idglib.ColorText(Color.Red, "But flying when wingtime is too low will drain your health") + "\n" + Idglib.ColorText(Color.Red, "Mounts drain health as well"));
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Red;
			Item.value = 5000;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = SGAmod.Instance.Find<ModBuff>("WarmpedRealityBuff").Type;
			Item.buffTime = 60 * 600;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DragonsMightPotion>(), 1).AddRecipeGroup("Fragment", 3).AddIngredient(ModContent.ItemType<DrakeniteBar>(), 1).AddTile(TileID.LunarCraftingStation).Register();
		}
	}

	public class DeificHealingPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deific Healing Potion");
			Tooltip.SetDefault("'A source of healing from the same powers that made Draken'\nResets Life Regen to max when consumed");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Expert;
			Item.value = 5000;
			Item.potion = true;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.healLife = 300;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
		}

        public override bool? UseItem(Player player)
        {
			player.lifeRegenTime += 10000;
			player.lifeRegenCount += 10000;
			return true;
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return Main.hslToRgb((Main.GlobalTimeWrappedHourly*0.75f)%1f,1f,0.75f);
        }

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Consumables/DeificHealingPotionEffect").Value;
			Vector2 drawOrigin = texture.Size() / 2f;
			position += new Vector2(drawOrigin.X * scale, drawOrigin.Y * scale);

			spriteBatch.Draw(Main.itemTexture[Item.type], position, null, GetAlpha(itemColor) == null ? Color.White : (Color)GetAlpha(itemColor), 0f, drawOrigin, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, position, null, Color.White, 0f, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{

			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Consumables/DeificHealingPotionEffect").Value;
			Vector2 drawOrigin = texture.Size() / 2f;

			spriteBatch.Draw(Main.itemTexture[Item.type], Item.Center-Main.screenPosition, null, GetAlpha(lightColor) == null ? Color.White : (Color)GetAlpha(lightColor), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, lightColor, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.SuperHealingPotion, 1).AddIngredient(ModContent.ItemType<IlluminantEssence>(), 2).AddIngredient(ModContent.ItemType<OverseenCrystal>(), 2).AddIngredient(ModContent.ItemType<ByteSoul>(), 6).AddTile(TileID.Bottles).Register();
		}

	}	
	
	public class ToxicityPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxicity Potion");
			Tooltip.SetDefault("'60 IQ dialog of swearing intensifies'\nNearby enemies who are Stinky may infect other enemies when near you\nWhile you are Stinky and take damage, you spit out random swear words\nThis can rarely happen when you damage a Stinky enemy\nThese do area damage to ALL nearby NPCs\nThis is boosted by Thorns and if they are also Stinky\nFurthermore you also gain reduced aggro\nAll Town NPCs sell [i: " + ItemID.StinkPotion + "] to you while under this effect\n" + Idglib.ColorText(Color.Red, "Grants immunity to Intimacy and Lovestruct")+"\n"+Idglib.ColorText(Color.Red, "NPCs become unhappy talking to you and charge more money"));
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 5000;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = SGAmod.Instance.Find<ModBuff>("ToxicityPotionBuff").Type;
			Item.buffTime = 60 * 600;
		}

		public override void AddRecipes()
		{
			CreateRecipe(3).AddIngredient(ItemID.StinkPotion, 1).AddIngredient(ItemID.ThornsPotion, 1).AddIngredient(ItemID.ShadowScale, 8).AddIngredient(ItemID.VilePowder, 5).AddIngredient(ModContent.ItemType<MurkyGel>(), 6).AddTile(TileID.Bottles).Register();
		}
	}
	public class IntimacyPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Intimacy Potion");
			Tooltip.SetDefault("'Their hearts will be yours!'\nNearby enemies who are Lovestruct will lose health over time based on your Life Regen\nTown NPCs who are Lovestruct have reduced prices\nWhile you are Lovestruct all hearts heal 10 more health and draw more aggro\nNearby players who are Lovestruct gain 20% of your life regen to their own\nAll Town NPCs sell [i: " + ItemID.LovePotion + "] to you while under this effect\n" + Idglib.ColorText(Color.Red, "Grants immunity to Toxicity and Stinky"));
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 5000;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = SGAmod.Instance.Find<ModBuff>("IntimacyPotionBuff").Type;
			Item.buffTime = 60 * 600;
		}

		public override void AddRecipes()
		{
			CreateRecipe(3).AddIngredient(ItemID.LovePotion, 1).AddIngredient(ItemID.HeartreachPotion, 1).AddIngredient(ItemID.WarmthPotion, 1).AddIngredient(ItemID.TissueSample, 4).AddIngredient(ItemID.LifeCrystal, 1).AddTile(TileID.Bottles).Register();
		}
	}	
	public class TriggerFingerPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trigger Finger Potion");
			Tooltip.SetDefault("Increases the attack speed of non-autofire guns/Revolvers by 15%");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Orange;
			Item.value = 1000;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = SGAmod.Instance.Find<ModBuff>("TriggerFingerPotionBuff").Type;
			Item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			CreateRecipe(5).AddIngredient(ItemID.BottledWater, 5).AddIngredient(ItemID.DesertFossil, 3).AddIngredient(ItemID.IllegalGunParts, 1).AddTile(TileID.AlchemyTable).Register();
		}
	}
	public class TrueStrikePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Strike Potion");
			Tooltip.SetDefault("Boosts the damage of True Melee weapons by 20%\nThis includes held projectiles, like spears");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Orange;
			Item.value = 500;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = SGAmod.Instance.Find<ModBuff>("TrueStrikePotionBuff").Type;
			Item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Ale,1).AddIngredient(ModContent.ItemType <UnmanedOre>(), 2).AddIngredient(ModContent.ItemType <WraithFragment3>(), 1).AddTile(TileID.Bottles).Register();
		}
	}
	public class ClarityPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clarity Potion");
			Tooltip.SetDefault("Reduces Mana costs by 3%\nWhile you have mana sickness, this is increased to 10%");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Orange;
			Item.value = 500;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = SGAmod.Instance.Find<ModBuff>("ClarityPotionBuff").Type;
			Item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(ItemID.BottledWater,2).AddIngredient(ItemID.Sunflower, 1).AddIngredient(ItemID.ManaCrystal, 1).AddIngredient(ModContent.ItemType<HavocGear.Items.Biomass>(), 4).AddTile(TileID.Bottles).Register();
		}
	}
	public class TinkerPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tinker Potion");
			Tooltip.SetDefault("Greatly reduces the loss from uncrafting");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Orange;
			Item.value = 250;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = SGAmod.Instance.Find<ModBuff>("TinkerPotionBuff").Type;
			Item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(ModContent.ItemType < BottledMud>(), 2).AddIngredient(ModContent.ItemType <HavocGear.Items.VirulentOre>(), 2).AddIngredient(ModContent.ItemType < DankWood>(), 6).AddIngredient(ModContent.ItemType < DankCore>(), 1).AddIngredient(ModContent.ItemType < VialofAcid>(), 4).AddTile(ModContent.TileType<ReverseEngineeringStation>()).Register();
		}
	}
	public class TooltimePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tooltime Potion");
			Tooltip.SetDefault("Greatly increases the knockback of tools");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = 50;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = SGAmod.Instance.Find<ModBuff>("TooltimePotionBuff").Type;
			Item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType < BottledMud>(), 1).AddRecipeGroup("Wood", 5).AddIngredient(ItemID.Acorn, 2).AddIngredient(ItemID.TungstenOre, 1).AddTile(TileID.Bottles).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType < BottledMud>(), 1).AddRecipeGroup("Wood", 3).AddIngredient(ItemID.Acorn, 1).AddIngredient(ItemID.SilverOre, 1).AddTile(TileID.Bottles).Register();
		}
	}

	public class RagnarokBrew : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ragnarok's Brew");
			Tooltip.SetDefault("Boosts your Apocalyptical Chance and Strength as your HP drops");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "apocalypticaltext", SGAGlobalItem.apocalypticaltext));
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Lime;
			Item.value = 500;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = SGAmod.Instance.Find<ModBuff>("RagnarokBrewBuff").Type;
			Item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			CreateRecipe(3).AddIngredient(ItemID.BottledWater,3).AddIngredient(ModContent.ItemType < Entrophite>(), 15).AddIngredient(ModContent.ItemType < StygianCore>(), 1).AddIngredient(ModContent.ItemType < FieryShard>(), 2).AddIngredient(ModContent.ItemType < UnmanedOre>(), 5).AddTile(TileID.Bottles).Register();
		}
	}
	public class EnergyPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy Potion");
			Tooltip.SetDefault("'A bottled transformer for the road'\n25% increased passive Electric Charge Rate\nYour Electric Charge Recharge Delay is halved");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Green;
			Item.value = 500;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = SGAmod.Instance.Find<ModBuff>("EnergyPotionBuff").Type;
			Item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ItemID.Sunflower).AddIngredient(ModContent.ItemType < NoviteOre>(), 1).AddIngredient(ItemID.Meteorite).AddIngredient(ItemID.RainCloud).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}
	}	
	public class CondenserPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Condensing Potion");
			Tooltip.SetDefault("'Helps deal with overheating, allowing you to preform more actions'\nGrants an additonal free Cooldown Stack, however, all new Cooldown Stacks are 15% longer");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Green;
			Item.value = 500;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = SGAmod.Instance.Find<ModBuff>("CondenserPotionBuff").Type;
			Item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			CreateRecipe(3).AddIngredient(ItemID.BottledWater, 3).AddIngredient(ItemID.CookedMarshmallow).AddIngredient(ModContent.ItemType <FrigidShard>(), 3).AddTile(TileID.Bottles).Register();
		}
	}

	public class RadCurePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Radiation Cure Potion");
			Tooltip.SetDefault("'Radiation is not easy to cure, but thankfully, we have MAGIC!'\nGrants increased Radiation poisoning recovery while no bosses are alive\nGrants 50% increased radiation resistance");
		}

		public override bool CanUseItem(Player player)
		{
			return !(player.HasBuff(Idglib.Instance.Find<ModBuff>("RadCure").Type) && player.HasBuff(Idglib.Instance.Find<ModBuff>("LimboFading").Type));
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Lime;
			Item.value = 1000;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = Idglib.Instance.Find<ModBuff>("RadCure").Type;
			Item.buffTime = 60 * 60;
		}

		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(ItemID.RestorationPotion, 2).AddIngredient(ItemID.StrangeBrew).AddIngredient(ItemID.ChlorophyteOre, 4).AddIngredient(ModContent.ItemType<HavocGear.Items.Weapons.SwampSeeds>(), 2).AddTile(TileID.AlchemyTable).Register();
		}
	}

	public class DragonsMightPotion : ModItem, IPotionCantBeInfinite
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragon's Might Potion");
			Tooltip.SetDefault("30% increase to all damage types except Summon damage, which gets 50%" +
				"\nLasts 30 seconds, inflicts Weakness after it ends\nThis cannot be stopped by being immune\nCannot be used while Weakened\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 60 seconds"));
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.Lime;
			Item.value = 1000;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = ModContent.BuffType<DragonsMight>();
			Item.buffTime = 60*30;
		}

		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(ItemID.RestorationPotion,2).AddIngredient(ModContent.ItemType <OmniSoul>(), 2).AddIngredient(ModContent.ItemType < Fridgeflame>(), 2).AddIngredient(ModContent.ItemType < MurkyGel>(), 3).AddIngredient(ModContent.ItemType<Entrophite>(), 20).AddTile(TileID.AlchemyTable).Register();
		}

        public override bool CanUseItem(Player player)
        {
			return !player.HasBuff(ModContent.BuffType<WorseWeakness>()) && player.SGAPly().AddCooldownStack(60*60,testOnly: true);
        }

        public override void OnConsumeItem(Player player)
		{
			player.SGAPly().AddCooldownStack(60 * 60);
			//SLWorld.EnterSubworld("SGAmod_Blank");
			//RippleBoom.MakeShockwave(player.Center,8f,1f,10f,60,1f);
			//Achivements.SGAAchivements.UnlockAchivement("TPD", Main.LocalPlayer);
			//SGAmod.FileTest();
			//NormalWorldGeneration.PlaceCaiburnShrine(player.Center / 16f);
			//WorldGen.placeTrap((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f)+1, 0);
		}
	}

	public class IceFirePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fridgeflame Concoction");
			Tooltip.SetDefault("'A Potion of Ice and Fire'" +
				"\nGrants 25% reduced Damage-over-time caused by Debuffs\nGain an extra 15% less damage while you have Frostburn or OnFire! (Both do not stack)\n25% less damage from cold sources, Obsidian Rose effect\n" + Idglib.ColorText(Color.Red, "Removes Immunity to both Frostburn and OnFire!"));
		}
		
		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = 8;
			Item.value = 20000;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = Mod.Find<ModBuff>("IceFirePotionBuff").Type;
			Item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(ItemID.BottledHoney, 2).AddIngredient(ModContent.ItemType < CryostalBar>(), 1).AddIngredient(ModContent.ItemType < IceFairyDust>(), 1).AddIngredient(ModContent.ItemType < Fridgeflame>(), 3).AddTile(TileID.AlchemyTable).Register();
		}
	}
	public class PhalanxPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phalanx Potion");
			Tooltip.SetDefault("'Stronger Together'" +
				"\nIncreases your blocking angle on all shields by 15%\nFor every player nearby that has a shield out, you gain 8 defense");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = 1;
			Item.value = 250;
			Item.useStyle = ItemRarityID.Green;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = ModContent.BuffType<PhalanxPotionBuff>();
			Item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
		}
	}

	public class ReflexPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reflex Potion");
			Tooltip.SetDefault("Increases the period of time you can Just Block with your shield");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = 500;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = ModContent.BuffType<ReflexPotionBuff>();
			Item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.SwiftnessPotion, 1).AddIngredient(ModContent.ItemType<HavocGear.Items.MoistSand>(), 2).AddIngredient(ModContent.ItemType<HavocGear.Items.Weapons.SwampSeeds>(), 1).AddIngredient(ItemID.Ale, 1).AddTile(TileID.Bottles).Register();
		}
	}

	public class FuryPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fury Potion");
			Tooltip.SetDefault("Increases crit damage by 20%\nThis bonus falls to 10% if used with rage or wrath");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = 500;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = ModContent.BuffType<FuryPotionBuff>();
			Item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bottle, 1).AddIngredient(ModContent.ItemType<HavocGear.Items.Weapons.SwampSeeds>(), 3).AddIngredient(ItemID.Deathweed, 1).AddTile(TileID.Bottles).Register();
		}
	}


}