using System;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.HavocGear.Items.Accessories;
using Idglibrary;

using Terraria.Localization;
using SGAmod.Items.Weapons;
using SGAmod.Buffs;
using Terraria.Utilities;
using System.Linq;
using SGAmod.Items.Mounts;
using SGAmod.Items.Consumables;

namespace SGAmod.Items.Accessories
{
	public class RestorationFlower : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Restoration Flower");
			Tooltip.SetDefault("Restoration Potions (and Strange Brew) restore 50% more Health & Mana\nRecovering over the max health/mana with a potion grants bonuses\nThese last depending on how much excess overhealing you did");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.ManaFlower);
			item.width = 24;
			item.height = 24;
			item.faceSlot = 6;
			item.rare += 1;
			item.value = Item.sellPrice(0, 2, 50, 0); ;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.restorationFlower = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.JungleRose, 1);
			recipe.AddIngredient(ItemID.RestorationPotion, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
	public class LifeFlower : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Life Flower");
			Tooltip.SetDefault("You consume healing potions instead of dying when taking fatal damage\nEffects of Obsidian Rose");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.ManaFlower);
			item.width = 24;
			item.height = 24;
			item.faceSlot = 6;
			item.rare += 1;
			item.value = Item.sellPrice(0, 2, 50, 0); ;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.LifeFlower = true;
			player.lavaRose = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ObsidianRose, 1);
			recipe.AddIngredient(ItemID.HealingPotion, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class LifeforceQuintessence : LifeFlower
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lifeforce Quintessence");
			Tooltip.SetDefault("You consume healing potions instead of dying when taking fatal damage\nUse mana potions when needed\nEffects of Obsidian Rose/Restoration Flower, 5% reduced mana costs");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.ManaFlower);
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightPurple;
			item.faceSlot = 6;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.SGAPly().restorationFlower = true;
			player.manaFlower = true;
			player.manaCost -= 0.05f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ManaFlower, 1);
			recipe.AddIngredient(mod.ItemType("LifeFlower"), 1);
			recipe.AddIngredient(mod.ItemType("RestorationFlower"), 1);
			recipe.AddIngredient(ItemID.LifeforcePotion, 1);
			recipe.AddIngredient(ItemID.DD2EnergyCrystal, 15);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.HandsOn)]
	public class PrismalGuantlet : ModItem
	{
		public static string tip = "15% increased melee damage and melee speed, 8% increased melee crit\n+5 armor penetration, Inflict OnFire! and Frostburn on hit and melee hits count as Cold Damage";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Gauntlet");
			Tooltip.SetDefault(tip);
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Cyan;
			item.value = Item.sellPrice(0, 15, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.kbGlove = true;
			player.meleeSpeed += 0.15f;
			player.meleeDamage += 0.15f;
			player.meleeCrit += 10;
			player.magmaStone = true;
			player.armorPenetration += 5;
			player.SGAPly().glacialStone = true;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FireGauntlet, 1);
			recipe.AddIngredient(ModContent.ItemType<GlacialStone>(), 1);
			recipe.AddIngredient(ItemID.SharkToothNecklace, 1);
			recipe.AddIngredient(ItemID.DestroyerEmblem, 1);
			recipe.AddIngredient(ModContent.ItemType<PrismalBar>(), 12);
			recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.Weapons.SharkTooth>(), 50);
			recipe.AddIngredient(ModContent.ItemType<Fridgeflame>(), 8);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.HandsOn)]
	public class YoyoGauntlet : PrismalGuantlet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yoyo Gauntlet");
			Tooltip.SetDefault(PrismalGuantlet.tip+"\nYoyos are 25% faster and do more damage at higher ranges\nYoyo Bag with fabulous rainbow strings!");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Purple;
			item.value = Item.sellPrice(0, 25, 0, 0);
			item.accessory = true;
			item.stringColor = 27;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.counterWeight = 556 + Main.rand.Next(6);
			player.yoyoGlove = true;
			player.yoyoString = true;
			player.SGAPly().YoyoTricks = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PrismalGuantlet>(), 1);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ItemID.YoyoBag, 1);
			recipe.AddIngredient(ItemID.RainbowString, 1);
			recipe.AddIngredient(ModContent.ItemType<YoyoTricks>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
	public class Photosynthesizer : MudAbsorber
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Photosynthesizer");
			Tooltip.SetDefault("Increased life regen while on the surface at day\n5% of the sum of all damage types is added to your current weapon's attack\nBeing near mud greatly increases your stats and life regen");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Lime;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.accessory = true;
			item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			sgaply.Dankset = 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("MudAbsorber"), 1);
			recipe.AddIngredient(mod.ItemType("BottledMud"), 15);
			recipe.AddIngredient(mod.ItemType("DankCore"), 2);
			recipe.AddIngredient(mod.ItemType("DankWoodHelm"), 1);
			recipe.AddIngredient(mod.ItemType("DankWoodChest"), 1);
			recipe.AddIngredient(mod.ItemType("DankLegs"), 1);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 10);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.Neck)]
	public class BloodCharmPendant : LifeFlower
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Charm Pendant");
			Tooltip.SetDefault("Increases life regen and length of invincibility after taking damage\nStars rain down after taking damage\nReduced cooldown on potions\nEffects of Necklace O' Nerve");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.lifeRegen = 3;
			item.rare = ItemRarityID.Lime;
			item.value = Item.sellPrice(0, 15, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			ModContent.GetInstance<NeckONerves>().UpdateAccessory(player, hideVisual);
			player.pStone = true;
			player.starCloak = true;
			player.longInvince = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("NeckONerves"), 1);
			recipe.AddIngredient(ItemID.CharmofMyths, 1);
			recipe.AddIngredient(ItemID.StarVeil, 1);
			recipe.AddIngredient(ItemID.HallowedBar, 6);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.Back)]
	public class PortableHive : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Portable Hive");
			Tooltip.SetDefault("Bees become enhanced and very aggressive\nSummons up to five enhanced bees to attack foes\nDamage is based on defense and summoner values\nToggle visibity to enable/disable the agressive bee movement\nEffects of Hive Pack, Honey Comb, and all bees do more damage, increases Summon damage by 10%\nGetting hit may splash all nearby players with the Honeyed Buff");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.value = Item.sellPrice(gold: 5);
			item.rare = -12;
			item.expert = true;
			item.accessory = true;
			item.damage = 1;
			item.summon = true;
			item.shieldSlot = 5;
			item.backSlot = 9;
			item.knockBack = 1f;
			item.backSlot = 9;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HiveBackpack, 1);
			recipe.AddIngredient(ItemID.HoneyComb, 1);
			recipe.AddIngredient(ItemID.CrispyHoneyBlock, 10);
			recipe.AddIngredient(ItemID.BeeWax, 10);
			recipe.AddIngredient(ItemID.HoneyBucket, 2);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			flat = (float)((player.statDefense * 0.2) * (0.5f + ((player.minionDamage - 1f) * 8f)));
			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().beefield = 5;
			if (!hideVisual)
				player.GetModPlayer<SGAPlayer>().beefieldtoggle = 5;

			player.minionDamage += 0.1f;

			player.strongBees = true;
			//Item shield = new Item();
			//shield.SetDefaults(ItemID.EoCShield);
			player.bee = true;

			float flat = 0f;
			if (GetType() == typeof(PortableHive))
				flat = (float)((player.statDefense * 0.2) * (0.5f + ((player.minionDamage - 1f) * 8f)));

			if (1f + (flat * 1f) > player.GetModPlayer<SGAPlayer>().beedamagemul)
				player.GetModPlayer<SGAPlayer>().beedamagemul = 1f + (flat * 1f);

		}

	}

	public class DevPower : ModItem
	{

		float[] effects = new float[20];
		float[] effectrotationadder = new float[20];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Secrets");
			Tooltip.SetDefault("While worn, it will unlock the true nature of so called 'Vanity' Dev Armors in your inventory...\nCombines the effects of:\n-Blood Charm Pendant\n-Lifeforce Quintessence\n-Havoc's Fragmented Remains\n-Portable Hive\ntoggle visiblity to disable bee spawning of Portable Hive");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.lifeRegen = 5;
			item.rare = 12;
			item.damage = 1;
			item.summon = true;
			item.value = Item.sellPrice(5, 0, 0, 0);
			item.accessory = true;
			item.expert = true;
			/*for (int i = 0; i < effects.Length; i += 1)
			{
				effects[i] = i*2f;
				effectsangle[i] = Main.rand.NextFloat(MathHelper.ToRadians(360));
				effectrotation[i] = Main.rand.NextFloat(-2,2f)/10f;
				effectrotationadder[i] = Main.rand.NextFloat((float)-Math.PI, (float)Math.PI);
			}*/
		}

		public override string Texture
		{
			get { return ("Terraria/Extra_57"); }
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Texture2D inner = SGAmod.ExtraTextures[57];

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			for (int i = 0; i < 20; i += 1)
			{
				effectrotationadder[i] += 1f;//effectrotation[i];
				Double Azngle = i;
				Vector2 here = new Vector2((float)Math.Cos(Azngle), (float)Math.Sin(Azngle)) * (i * 2f);
				float scaler = (1f - (float)((float)i / (float)effects.Length));
				spriteBatch.Draw(inner, position + (new Vector2(14f, 14f)) + here, null, Color.Lerp(drawColor, Color.MediumPurple, 0.25f) * scaler, Main.GlobalTime *= (i % 2 == 0 ? -1f : 1f), new Vector2(inner.Width / 2, inner.Height / 2), scale * scaler, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(Main.itemTexture[item.type], position + new Vector2(14f, 14f), frame, drawColor, 0, new Vector2(inner.Width / 2, inner.Height / 2), scale * 1.5f * Main.essScale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					line.overrideColor = Color.Lerp(Color.DarkMagenta, Color.SteelBlue, 0.5f + (float)Math.Sin(Main.GlobalTime * 2f));
				}
			}
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			flat = (float)((player.statDefense * 0.10) * (1.1f + (player.minionDamage - 1f) * 50f));
			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().devpower = 3;
			ModContent.GetInstance<BloodCharmPendant>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<LifeforceQuintessence>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<PortableHive>().UpdateAccessory(player, false);
			ModContent.GetInstance<Havoc>().UpdateAccessory(player, false);
			if (hideVisual == true)
			{
				player.GetModPlayer<SGAPlayer>().beefield = 3;
			}
			float flat = (float)((player.statDefense * 0.25) * (2f + (player.minionDamage - 1f) * 50f));

			if (1f + (flat * 1f) > player.GetModPlayer<SGAPlayer>().beedamagemul)
				player.GetModPlayer<SGAPlayer>().beedamagemul = 1f + (flat * 1f);


		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BloodCharmPendant>(), 1);
			recipe.AddIngredient(ModContent.ItemType<LifeforceQuintessence>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Havoc>(), 1);
			recipe.AddIngredient(ModContent.ItemType<PortableHive>(), 1);
			recipe.AddIngredient(ModContent.ItemType<LunarRoyalGel>(), 25);
			recipe.AddIngredient(ModContent.ItemType<MoneySign>(), 15);
			recipe.AddIngredient(ModContent.ItemType<EldritchTentacle>(), 20);
			recipe.AddIngredient(ModContent.ItemType<ByteSoul>(), 50);
			/*recipe.AddIngredient(ItemID.ShroomiteBar, 30);
			recipe.AddIngredient(ItemID.SpectreBar, 30);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 30);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 30);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 30);
			recipe.AddIngredient(mod.ItemType("Entrophite"), 250);
			recipe.AddIngredient(mod.ItemType("StygianCore"), 4);*/
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
	public class GeyserInABottle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geyser In A Bottle");
			Tooltip.SetDefault("Creates an eruption midair when you jump!\nNo damage taken from Geysers, counts as Trap Damage\nGrants a small amount of lava immunity");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			int y_bottom_edge = (int)(player.position.Y + (float)player.height + 16f) / 16;
			int x_edge = (int)(player.Center.X) / 16;

			Tile mytile = Framing.GetTileSafely(x_edge, y_bottom_edge);

			if (mytile.active() && player.velocity.Y == 0)
			{
				player.GetModPlayer<SGAPlayer>().GeyserInABottle = true;

			}
			player.lavaMax += 100;
			player.GetModPlayer<SGAPlayer>().GeyserInABottleActive = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GeyserTrap, 1);
			recipe.AddIngredient(ItemID.Obsidian, 10);
			recipe.AddIngredient(ItemID.CloudinaBottle, 1);
			recipe.AddIngredient(ItemID.LavaBucket, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class AvariceRing : AvariceRingWeaker, INoHitItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Absolute Ring Of Untouchable Avarice");
			Tooltip.SetDefault("Enemies you kill drop double loot if you no-hit them");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Yellow;
			item.value = Item.buyPrice(0, 50, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
				player.SGAPly().avariceRing = 2;
		}
	}
	public class AvariceRingWeaker : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ring Of Untouchable Avarice");
			Tooltip.SetDefault("'A silver replica of the real ring...'\nEnemies you kill drop double loot 20% of the time, if you no-hit them");
		}

		public override bool CanEquipAccessory(Player player, int slot)
		{
			return player.armor.Where(testby => testby.type == ModContent.ItemType<AvariceRing>() || testby.type == ModContent.ItemType<AvariceRingWeaker>()).Count() < 1;
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Yellow;
			item.value = Item.buyPrice(0, 50, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().avariceRing = 1;
		}
	}


	public class SybariteGem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sybarite Gem");
			Tooltip.SetDefault("Apply Midas to yourself for 5 seconds while at full health\nGrants 50% increased Apocalyptical Strength while afflicted with Midas\nScoring an Apocalyptical creates a Coin-splosion\nThe amount is based off their worth times damage divided by max health times Apocalyptical Strength\nCoin projectiles have a chance to be recovered after being expended\n'Eternally Priceless... People would pay alot for this, even their own lives.'");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 52;
			item.rare = ItemRarityID.LightPurple;
			item.value = Item.buyPrice(gold: 10);
			item.accessory = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "accapocalypticaltext", SGAGlobalItem.apocalypticaltext));
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().SybariteGem = true;
			player.GetModPlayer<SGAPlayer>().apocalypticalStrength += player.HasBuff(BuffID.Midas) ? 0.50f : 0.0f;

		}

		public override void AddRecipes()
		{
			//nil
		}
	}
	public class IdolOfMidas : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Midas Insignia");
			Tooltip.SetDefault("One of the many treasures this greed infested abomination stole....\nPicking up coins grants small buffs depending on the coin\ndefensive/movement buffs while facing left, offensive buffs while facing right, gold and platinum coins give you both\nIncreased damage with more money you have sacrificed (this caps at 25% at 10 platinum)\n15% increased damage against enemies afflicted with Midas\nShop prices are 20% cheaper\n" + Idglib.ColorText(Color.Red, "Any coins picked up are sacrificed to the Planes of Wealth"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 52;
			item.rare = ItemRarityID.Lime;
			item.value = Item.sellPrice(0, 25, 0, 0); ;
			item.accessory = true;
			item.expert = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "sacrificedMoney", Main.LocalPlayer.SGAPly().MoneyCollected));
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			bool left = player.direction < 0;
			//int coincount = player.CountItem(ItemID.CopperCoin) + (player.CountItem(ItemID.SilverCoin) * 100) + (player.CountItem(ItemID.GoldCoin) * 10000) + (player.CountItem(ItemID.PlatinumCoin) * 1000000);
			int coincount = player.SGAPly().midasMoneyConsumed;
			float howmuch = Math.Min(0.25f, (coincount / 10000000f) / 4f);
			player.BoostAllDamage(howmuch, 5);
			if (GetType() == typeof(CorperateEpiphany))
				player.GetModPlayer<SGAPlayer>().MidasIdol = hideVisual ? 3 : left ? 1 : 2;
			else
				player.GetModPlayer<SGAPlayer>().MidasIdol = left ? 1 : 2;
		}

	}

	public class CorperateEpiphany : IdolOfMidas, INoHitItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corporate Epiphany");
			Tooltip.SetDefault("'Money Money Money!\nMust be funny? In a rich man's world?'\n" +
				"Combined Effects of Idol of Midas, Sybarite Gem, Avarice Ring, and Omni-Magnet (hide to disable Idol of Midas's coin collecting)\n" +
				"Includes EALogo's ability. but only while worn as an accessory");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					line.overrideColor = Color.Lerp(Color.DarkRed, Color.Gold, 0.5f + (float)Math.Sin(Main.GlobalTime * 1f));
				}
			}
		}
		public override void SetDefaults()
		{
			item.value = 3000000;
			item.rare = ItemRarityID.Purple;
			item.width = 24;
			item.height = 24;
			item.accessory = true;
			item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<OmniMagnet>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<EALogo>().UpdateInventory(player);
			ModContent.GetInstance<SybariteGem>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<AvariceRing>().UpdateAccessory(player, hideVisual);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<IdolOfMidas>(), 1);
			recipe.AddIngredient(ModContent.ItemType < SybariteGem>(), 1);
			recipe.AddIngredient(ModContent.ItemType < AvariceRing>(), 1);
			recipe.AddIngredient(ModContent.ItemType < EALogo>(), 1);
			recipe.AddIngredient(ModContent.ItemType < OmniMagnet>(), 1);
			recipe.AddIngredient(ModContent.ItemType <MoneySign>(), 15);
			recipe.AddIngredient(ModContent.ItemType <Entrophite>(), 200);
			recipe.AddIngredient(ModContent.ItemType <StygianCore>(), 3);
			recipe.AddIngredient(ModContent.ItemType <CalamityRune>(), 2);
			recipe.AddIngredient(ItemID.GoldDust, 200);
			recipe.AddIngredient(ItemID.PlatinumCoin, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}


	public class BlinkTechGear : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tech Master's Gear");
			Tooltip.SetDefault("'Mastery over your advancements has led you to create this highly advanced suit'\nHold UP and press left or right to blink teleport, this gives you 2 seconds of chaos state\nCannot blink with more than 6 seconds of Chaos State\nhide accessory to disable blinking\nGrants the effects of:\n-Prismal Core, Plasma Pack, and Fridgeflame Canister\n-Jindosh Buckler (Both Evil Types)\n-Rusted Bulwark's effects are doubled");
		}

		public override void SetDefaults()
		{
			item.value = 1500000;
			item.rare = ItemRarityID.Red;
			item.width = 24;
			item.defense = 5;
			item.height = 24;
			item.accessory = true;
			item.waistSlot = 10;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			//player.meleeDamage += 0.05f; player.magicDamage += 0.05f; player.rangedDamage += 0.05f; player.minionDamage += 0.05f; player.Throwing().thrownDamage += 0.05f;
			player.BoostAllDamage(0.05f, 5);
			//player.meleeCrit += 5; player.magicCrit += 5; player.rangedCrit += 5; player.Throwing().thrownCrit += 5;
			sgaply.maxblink += hideVisual ? 0 : 60 * 6;
			//sgaply.boosterPowerLeftMax += (int)(10000f * 0.15f);
			//sgaply.boosterrechargerate += 3;
			//sgaply.electricChargeMax += 1500;
			//sgaply.electricrechargerate += 1;
			//sgaply.plasmaLeftInClipMax += (int)((float)SGAPlayer.plasmaLeftInClipMaxConst * 0.20f);
			//sgaply.techdamage += 0.05f;
			ModContent.GetInstance<PlasmaPack>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<FridgeFlamesCanister>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<PrismalCore>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<JindoshBuckler>().UpdateAccessoryThing(player, hideVisual, true);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 6);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 32);
			recipe.AddIngredient(mod.ItemType("PlasmaCell"), 3);
			recipe.AddIngredient(mod.ItemType("PrismalCore"), 1);
			recipe.AddIngredient(mod.ItemType("PlasmaPack"), 1);
			recipe.AddIngredient(mod.ItemType("JindoshBuckler"), 1);
			recipe.AddIngredient(mod.ItemType("FridgeFlamesCanister"), 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.Back)]
	public class JavelinBaseBundle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bundle of Jab-lin Bases");
			Tooltip.SetDefault("Improves the damage over time of javelins by 25%\nJavelin damage increased by 10%");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Green;
			item.value = Item.sellPrice(0, 0, 2, 0); ;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().JavelinBaseBundle = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:EvilJavelins", 1);
			recipe.AddIngredient(mod.ItemType("IceJavelin"), 1);
			recipe.AddIngredient(mod.ItemType("StoneJavelin"), 1);
			recipe.AddIngredient(mod.ItemType("AmberJavelin"), 1);
			recipe.AddIngredient(ItemID.RopeCoil, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
	public class JavelinSpearHeadBundle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bundle of Jab-lin Spear Heads");
			Tooltip.SetDefault("The amount of javelins you can stick into a target increases by 1\nJavelin damage increased by 15%");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(0, 0, 2, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().JavelinSpearHeadBundle = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:EvilJavelins", 1);
			recipe.AddIngredient(mod.ItemType("IceJavelin"), 1);
			recipe.AddIngredient(mod.ItemType("StoneJavelin"), 1);
			recipe.AddIngredient(mod.ItemType("AmberJavelin"), 1);
			recipe.AddIngredient(mod.ItemType("ShadowJavelin"), 1);
			recipe.AddIngredient(mod.ItemType("PearlWoodJavelin"), 1);
			recipe.AddIngredient(ItemID.RopeCoil, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
	[AutoloadEquip(EquipType.Back)]
	public class JavelinBundle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bundle of Jab-lin Parts");
			Tooltip.SetDefault("'Worthless money-wise, but won't discount your life'\nThe amount of javelins you can stick into a target increases by 1\nImproves the damage over time of javelins by 25%\nJavelin damage increased by 25%\nEffects of Jabb-a-wacky (hide to disable)\nDoesn't stack with component parts");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightPurple;
			item.value = Item.buyPrice(0, 0, 20, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().JavelinSpearHeadBundle = true;
			player.GetModPlayer<SGAPlayer>().JavelinBaseBundle = true;
			if (!hideVisual)
				ModContent.GetInstance<Jabbawacky>().UpdateAccessory(player, hideVisual);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("JavelinSpearHeadBundle"), 1);
			recipe.AddIngredient(mod.ItemType("JavelinBaseBundle"), 1);
			recipe.AddIngredient(mod.ItemType("Jabbawacky"), 1);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 10);
			recipe.AddIngredient(ItemID.RopeCoil, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class Fieryheart : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Heart");
			Tooltip.SetDefault("'The best friendships burn the brightest...'\nAll attacks inflict a short amount of Daybroken\nAs you gain more Total Expertise, this increases even further\nYou gain max life based on Total Expertise");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 4));
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.value = 100000;
			item.rare = -12;
			item.expert = true;
			item.accessory = true;
			item.defense = 0;
			//item.damage = 1;
			item.summon = true;
			item.knockBack = 1f;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().FieryheartBuff = Math.Max(player.GetModPlayer<SGAPlayer>().FieryheartBuff, 5);
			player.statLifeMax2 += (int)(player.SGAPly().ExpertiseCollectedTotal / (GetType() == typeof(Blazingheart) ? 150f : 200f));
		}
	}
	public class Blazingheart : Fieryheart
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazing Heart");
			Tooltip.SetDefault("'Nothing will extinish the fire in our hearts!'\nAll effects of the Fiery Heart\nBut your daybreak attacks now bypass enemy immunities\nAlso, max life is improved");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.value = 500000;
			item.rare = -12;
			item.expert = true;
			item.accessory = true;
			item.defense = 0;
			//item.damage = 1;
			item.summon = true;
			item.knockBack = 1f;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.GetModPlayer<SGAPlayer>().FieryheartBuff = 30;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Texture2D inner = Main.itemTexture[ModContent.ItemType<AssemblyStar>()];

			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);

			for (float i = 0; i < 1f; i += 0.20f)
			{
				spriteBatch.Draw(inner, drawPos, null, Color.White * (1f - ((i + (Main.GlobalTime / 2f)) % 1f)) * 0.75f, i * MathHelper.TwoPi, textureOrigin, Main.inventoryScale * (0.5f + 1.75f * (((Main.GlobalTime / 2f) + i) % 1f)), SpriteEffects.None, 0f);
			}
			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Fieryheart"), 1);
			recipe.AddIngredient(mod.ItemType("HopeHeart"), 10);
			recipe.AddIngredient(ItemID.LihzahrdDoor);
			recipe.AddTile(TileID.LihzahrdAltar);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class Rainbowheart : Blazingheart
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("[i: " + item.type + "]");
			Tooltip.SetDefault("'Our fires burn hotter than the golden sun'\nRadiation Resistance now also allow you to resist Limbo Fading\nAll effects of the Blazing Heart and Alkalescent Heart");
		}
		public override string Texture => "SGAmod/GreyHeart";
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.value = 1500000;
			item.rare = -12;
			item.expert = true;
			item.accessory = true;
			item.defense = 0;
			//item.damage = 1;
			item.summon = true;
			item.knockBack = 1f;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb(Main.GlobalTime % 1f, 1f, 0.75f);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.SGAPly().FieryheartBuff = 30;
			player.SGAPly().alkalescentHeart = true;
			player.GetModPlayer<IdgPlayer>().resistLimbo = true;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Texture2D inner = Main.itemTexture[ModContent.ItemType<AssemblyStar>()];
			Texture2D inner2 = Main.itemTexture[item.type];

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			Effect hallowed = SGAmod.HallowedEffect;

			hallowed.Parameters["prismAlpha"].SetValue(1f);
			hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("Perlin"));
			hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, Main.GlobalTime / 1f, Main.GlobalTime / 2f));
			hallowed.Parameters["overlayAlpha"].SetValue(1f);
			hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2.5f, 2.5f, 0f));
			hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
			hallowed.Parameters["rainbowScale"].SetValue(1f);
			hallowed.Parameters["overlayScale"].SetValue(new Vector2(1, 1));

			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);

			for (float i = 0; i < 1f; i += 0.20f)
			{
				spriteBatch.Draw(inner, drawPos, null, Color.White * (1f - ((i + (Main.GlobalTime / 2f)) % 1f)) * 0.75f, i * MathHelper.TwoPi, textureOrigin, Main.inventoryScale * (0.5f + 1.75f * (((Main.GlobalTime / 2f) + i) % 1f)), SpriteEffects.None, 0f);
				hallowed.Parameters["prismColor"].SetValue(Main.hslToRgb(i % 1f, 1f, 0.75f).ToVector3());
				hallowed.Parameters["alpha"].SetValue((1f - ((i + (Main.GlobalTime / 2f)) % 1f)) * 0.75f);
				hallowed.CurrentTechnique.Passes["Prism"].Apply();
			}

			hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("Perlin"));
			hallowed.Parameters["overlayProgress"].SetValue(new Vector3(Main.GlobalTime / 4f, 0, Main.GlobalTime / 4f));
			hallowed.Parameters["overlayAlpha"].SetValue(0.5f);
			hallowed.Parameters["overlayStrength"].SetValue(new Vector3(-2.5f, 0f, 0f));
			hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
			hallowed.Parameters["prismColor"].SetValue(((Color)GetAlpha(itemColor)).ToVector3());
			hallowed.Parameters["alpha"].SetValue(1f);
			hallowed.Parameters["rainbowScale"].SetValue(1f);
			hallowed.Parameters["overlayScale"].SetValue(new Vector2(0.5f, 0.5f));
			hallowed.CurrentTechnique.Passes["Prism"].Apply();

			spriteBatch.Draw(inner2, drawPos, null, Color.White, 0, inner2.Size() / 2f, Main.inventoryScale * 1, SpriteEffects.None, 0f);

			hallowed.Parameters["rainbowScale"].SetValue(1f);
			hallowed.Parameters["overlayScale"].SetValue(new Vector2(1, 1));

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Blazingheart"), 1);
			recipe.AddIngredient(mod.ItemType("AlkalescentHeart"), 1);
			recipe.AddIngredient(mod.ItemType("AncientFabricItem"), 25);
			recipe.AddIngredient(mod.ItemType("StygianCore"), 1);
			recipe.AddTile(TileID.LihzahrdAltar);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class LunarSlimeHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Slime Heart");
			Tooltip.SetDefault("Heart of the supreme lunar princess" +
				"\nSummons an array of lunar gels that damage enemies and nearly nullify projectile damage\nWhen a projectile is dampened or hits an enemy 5 times, the gel explodes into a damaging debuffing nova\nThis grants the player a random buff for 8 seconds\nWhen the gel explodes from canceling out projectiles remains inactive for 10 seconds, otherwise only 6 seconds\nBase damage is based on your defense times the sum of your damage multipliers: (melee+thrown+summon+magic+ranged)*defense\nEach buff the player has grants +1 defense\ndebuffs grant 4 defense");
		}
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.value = Item.sellPrice(platinum: 1);
			item.rare = -12;
			item.expert = true;
			item.accessory = true;
			item.damage = 1;
			item.knockBack = 1f;
		}


		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			flat = (float)(player.statDefense * (player.minionDamage + player.rangedDamage + player.meleeDamage + player.Throwing().thrownDamage + player.magicDamage));
			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().lunarSlimeHeart = true;
		}
	}

	public class OmegaSigil : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omega Sigil");
			Tooltip.SetDefault("'it's time to put an end...'\n2% increased Apocalyptical Chance\nWhile you have max HP, gain an extra 3% Apocalyptical Chance\n10% chance to dodge attacks that would kill you\nIs not consumed when making Wrath Arrows");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "accapocalypticaltext", SGAGlobalItem.apocalypticaltext));
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			for (int i = 0; i < player.GetModPlayer<SGAPlayer>().apocalypticalChance.Length; i += 1)
				player.GetModPlayer<SGAPlayer>().apocalypticalChance[i] += 2.0;
			player.GetModPlayer<SGAPlayer>().OmegaSigil = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 32;
			item.height = 32;
			item.value = 1500000;
			item.rare = ItemRarityID.Red;
			item.accessory = true;
		}
	}
	public class BrokenImmortalityCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flawed 1mm0rtal1ty Pr0t0call");
			Tooltip.SetDefault("'A fragment of Hellion's sheer power... Too bad it hardly even works'\nGrants 1000 defense! But getting hit causes you to lose it for 10 seconds");
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return lightColor = Main.hslToRgb((Main.GlobalTime / 15f) % 1f, 0.85f, 0.45f);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().BIP = true;
			if (!player.HasBuff(mod.BuffType("BIPBuff")))
				player.statDefense += 1000;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/StarMetalMold2"); }
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(mod.ItemType("ByteSoul"), 100);
			recipe.AddIngredient(mod.ItemType("ByteSoul"), 50);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 50);
			recipe.AddIngredient(mod.ItemType("WraithFragment4"), 100);
			recipe.AddIngredient(mod.ItemType("EldritchTentacle"), 25);
			recipe.AddIngredient(ItemID.FallenStar, 15);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 32;
			item.height = 32;
			item.value = 1500000;
			item.rare = ItemRarityID.Red;
			item.accessory = true;
		}
	}

	public class PrimordialSkull : ModItem
	{
		public static string Text => "While you have On-Fire!, you gain the Inferno buff and resist 50% contact damage\nFurthermore, enemies who also are on fire will take 25% increased damage from you during this\nImmunity against Thermal Blaze";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Primordial Skull");
			Tooltip.SetDefault(Text+"\nThe skull also gives you 25% DoT resist while OnFire!\nA most sinister looking skull, I wonder what else it's for?");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/PrimordialSkull"); }
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.HasBuff(BuffID.OnFire))
			{
				player.AddBuff(BuffID.Inferno, 2);
				player.GetModPlayer<SGAPlayer>().PrimordialSkull = true;
				if (GetType() == typeof(PrimordialSkull))
                {
					player.SGAPly().DoTResist *= 0.75f;
                }
			}
			player.buffImmune[mod.BuffType("ThermalBlaze")] = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 26;
			item.accessory = true;
			item.height = 14;
			item.value = 500000;
			item.rare = ItemRarityID.LightPurple;
			item.expert = false;
		}
	}

	[AutoloadEquip(EquipType.Face)]
	public class AmberGlowSkull : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corroded Skull");
			Tooltip.SetDefault("'It seems suprisingly intact, yet corroded by the Spider Queen'\nGrants immunity against Acid Burn\nGrants 25% increased radiation resistance");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.buffImmune[mod.BuffType("AcidBurn")] = true;
			player.GetModPlayer<IdgPlayer>().radresist += 0.25f;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 26;
			item.defense = 0;
			item.accessory = true;
			item.height = 14;
			item.value = 5000;
			item.rare = ItemRarityID.Green;
			item.expert = false;
		}
	}
	public class AlkalescentHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alkalescent Heart");
			Tooltip.SetDefault("'The Spider Queen's toxic blood pumps through you!'\nDealing crits debuff enemies, doing more damage while they are debuffed as follows:\nWhile not poisoned, poison enemies\nWhile poisoned, do 5% more damage and next crit Venoms\n" +
				"While Venomed, do 10% more damage and next crit Acid Burns\nWhile Acid Burned, do 15% more damage" +
				"\nThese damage boosts do not stack; highest takes priority\nMinions may infict this based off your highest crit chance");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().alkalescentHeart = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 26;
			item.defense = 0;
			item.accessory = true;
			item.height = 14;
			item.value = Item.buyPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Green;
			item.expert = true;
		}
	}
	public class UndyingValor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Undying Valor");
			Tooltip.SetDefault("'The Caliburns lend you the strength to keep fighting to the end'\nAll damage taken is converted into a DoT stack");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}

		public override string Texture => "Terraria/Item_3857";
		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.defense = 0;
			item.accessory = true;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.height = 16;
			item.expert = true;
			item.rare = ItemRarityID.Quest;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().undyingValor = true;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = mod.GetTexture("Glow");

				Texture2D[] tex1 = { mod.GetTexture("Items/Weapons/Caliburn/CaliburnTypeA"), mod.GetTexture("Items/Weapons/Caliburn/CaliburnTypeB"), mod.GetTexture("Items/Weapons/Caliburn/CaliburnTypeC") };

				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);

				List<float> valuesz = new List<float>();
				List<(int,float)> valuesz2 = new List<(int, float)>();

				for (int i = 0; i < 3; i += 1)
				{
					valuesz2.Add((i, (((i / 3f) * MathHelper.TwoPi)+ Main.GlobalTime)));
				}


				for (int i = 0; i < 50; i += 1)
				{
					valuesz.Add(((i / 50f) + Main.GlobalTime / 2f) % 1f);
				}


				valuesz = valuesz.OrderBy(testby => testby).ToList();

				valuesz2 = valuesz2.OrderBy(testby => testby.Item2).ToList();


				foreach ((int, float) fot in valuesz2)
				{
					float scalezx = 1f + (float)Math.Sin(fot.Item2) / 5f;
					if (scalezx <= 1f)
					{
						spriteBatch.Draw(tex1[fot.Item1], item.Center + (Vector2.UnitX.RotatedBy(fot.Item2)*new Vector2(24f,5f)).RotatedBy(item.velocity.X / 6f) - Main.screenPosition, null, Color.White, -MathHelper.PiOver4+(item.velocity.X / 6f) + (float)Math.Sin((scalezx-1f)) * 0.75f, tex1[fot.Item1].Size()/2f, scalezx * 0.60f, SpriteEffects.None, 0f);
					}
				}

				foreach (float fot in valuesz)
                {
					spriteBatch.Draw(texture, item.Center + new Vector2(0, 0) - Main.screenPosition, null, Main.hslToRgb((Main.GlobalTime-fot*1.5f)%1f,1f,0.65f) * (1f-fot), item.velocity.X/6f, textureOrigin, new Vector2(1f, 1.75f) * (fot* fot), SpriteEffects.None, 0f);
				}

				foreach ((int, float) fot in valuesz2)
				{
					float scalezx = 1f + (float)Math.Sin(fot.Item2) / 5f;
					if (scalezx > 1f)
					{
						spriteBatch.Draw(tex1[fot.Item1], item.Center + (Vector2.UnitX.RotatedBy(fot.Item2) * new Vector2(24f, 5f)).RotatedBy(item.velocity.X / 6f) - Main.screenPosition, null, Color.White, -MathHelper.PiOver4 + (item.velocity.X / 6f) + (float)Math.Sin((scalezx - 1f)) * 0.75f, tex1[fot.Item1].Size() / 2f, scalezx*0.60f, SpriteEffects.None, 0f);
					}
				}

			}
			return false;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = mod.GetTexture("Glow");

				Texture2D[] tex1 = { mod.GetTexture("Items/Weapons/Caliburn/CaliburnTypeA"), mod.GetTexture("Items/Weapons/Caliburn/CaliburnTypeB"), mod.GetTexture("Items/Weapons/Caliburn/CaliburnTypeC") };

				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);

				List<float> valuesz = new List<float>();
				List<(int, float)> valuesz2 = new List<(int, float)>();

				for (int i = 0; i < 3; i += 1)
				{
					valuesz2.Add((i, (((i / 3f) * MathHelper.TwoPi) + Main.GlobalTime)));
				}

				position = position + new Vector2(13f, 13f) * Main.inventoryScale;



				for (int i = 0; i < 50; i += 1)
				{
					valuesz.Add(((i / 50f) + Main.GlobalTime / 2f) % 1f);
				}


				valuesz = valuesz.OrderBy(testby => testby).ToList();

				valuesz2 = valuesz2.OrderBy(testby => testby.Item2).ToList();


				foreach ((int, float) fot in valuesz2)
				{
					float scalezx = 1f + (float)Math.Sin(fot.Item2) / 5f;
					if (scalezx <= 1f)
					{
						spriteBatch.Draw(tex1[fot.Item1], position + (Vector2.UnitX.RotatedBy(fot.Item2) * new Vector2(24f, 5f)), null, Color.White, -MathHelper.PiOver4 + (float)Math.Sin((scalezx - 1f)) * 0.75f, tex1[fot.Item1].Size() / 2f, scalezx * 0.60f, SpriteEffects.None, 0f);
					}
				}

				foreach (float fot in valuesz)
				{
					spriteBatch.Draw(texture, position + new Vector2(0, 0), null, Main.hslToRgb((Main.GlobalTime - fot * 1.5f) % 1f, 1f, 0.65f) * (1f - fot), 0, textureOrigin, new Vector2(1f, 1.75f) * (fot * fot), SpriteEffects.None, 0f);
				}

				foreach ((int, float) fot in valuesz2)
				{
					float scalezx = 1f + (float)Math.Sin(fot.Item2) / 5f;
					if (scalezx > 1f)
					{
						spriteBatch.Draw(tex1[fot.Item1], position + (Vector2.UnitX.RotatedBy(fot.Item2) * new Vector2(24f, 5f)), null, Color.White, -MathHelper.PiOver4 + (float)Math.Sin((scalezx - 1f)) * 0.75f, tex1[fot.Item1].Size() / 2f, scalezx * 0.60f, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
	}	
	public class PhaethonEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowspirit's Eye");
			Tooltip.SetDefault("'an eye of Phaethon, grants its wearing protection of the cosmos...'" +
				"\nGrants a 1/3 chance of converting debuffs into Action Cooldown Stacks\nLethal damage is converted into an Action Cooldown Stack\nGrants ALL sense potion effects (hide to disable)\nUpgrades the Caliburn Compass to detect Rare creatures from anywhere\nVisual buffs work while in inventory");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}

		public override string Texture => "SGAmod/Doom_Harbinger_Resprite_pupil";
		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.defense = 0;
			item.accessory = true;
			item.height = 16;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Quest;
			item.expert = true;
		}

        public override void UpdateInventory(Player player)
        {
			player.SGAPly().phaethonEye = 3;
			player.dangerSense = true;
				player.detectCreature = true;
				player.findTreasure = true;
				player.nightVision = true;
				player.sonarPotion = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{

			if (!hideVisual)
			{
				UpdateInventory(player);
			}
			player.SGAPly().phaethonEye = 10;

		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[item.type];
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, item.Center + new Vector2(0, 0) - Main.screenPosition, null, Color.White * 0.75f, 0f, textureOrigin, 2.25f - Main.essScale, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, item.Center + new Vector2(0, 0) - Main.screenPosition, null, Color.White * 0.75f, 0f, textureOrigin, 0.75f * Main.essScale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[item.type];
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				Vector2 loc = position + new Vector2(13f, 13f) * Main.inventoryScale;
				spriteBatch.Draw(texture, loc, null, Color.White * 0.75f, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, loc, null, Color.White * 0.75f, 0f, textureOrigin, 0.75f * Main.essScale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}

	public class PhaethonEyeProcEffect : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phaethon Eye Proc Effect");
		}

		public override string Texture
		{
			get { return ("SGAmod/Doom_Harbinger_Resprite_pupil"); }
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 90;
			aiType = 0;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			projectile.ai[0] += 1;
			if (projectile.ai[0] == 1)
			{
				Microsoft.Xna.Framework.Audio.SoundEffectInstance snd = Main.PlaySound(SoundID.NPCHit, (int)projectile.Center.X, (int)projectile.Center.Y, 5);

				if (snd != null)
				{
					snd.Pitch = Main.rand.NextFloat(-0.75f, -0.50f);
				}
			}

			Vector2 wegohere = player.MountedCenter;

			projectile.Center = wegohere;

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Vector2 size = tex.Size() / 2f;

			UnifiedRandom unirand = new UnifiedRandom(projectile.whoAmI);

			for (int i = 0; i < 16; i += 1)
			{
				float percent = 1f - (projectile.timeLeft / 90f);
				float dist = percent * (72f - ((72f / (1f + (projectile.ai[0]) * 2f))) + unirand.NextFloat(32, 160f) + (float)Math.Pow(MathHelper.Clamp(projectile.ai[0] - 50, 0f, 1000), 1.1));

				float angle = unirand.NextFloat(MathHelper.TwoPi) - ((percent * (projectile.ai[0] - 20f)) * (unirand.NextFloat(0.01f, 0.2f)));


				for (float f = 0; f < 0.1f; f += 0.01f)
				{
					Vector2 offseteffect = Vector2.One.RotatedBy(angle - f) * dist;

					spriteBatch.Draw(tex, projectile.Center + offseteffect - Main.screenPosition, null, Color.White * 0.60f * MathHelper.Clamp((float)Math.Sin(percent * MathHelper.Pi) * 2f, 0f, 1f) * (1f - (f * 10f)), angle, size, new Vector2(1f, 1f), projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
				}
			}

			return false;
		}
	}

	public class DraconicClawNecklace : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Draconic Claw Necklace");
			Tooltip.SetDefault("'The trimmed claws make a good trophy...'\nEnhances max flight time of Avarice Lord\nAlso effectively infinite flight by holding up\n"+Idglib.ColorText(Color.Red, "this will drain HP from you , if you have health to spare...")+"\n+50 Armor Penetration with Avarice Lord");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.Red;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<DergWingsPlayer>().draconicBoost = true;
		}
	}

	public class CalamityRune : SybariteGem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Calamity Rune");
			Tooltip.SetDefault("'The unrelenting heat of dismay shimmers, charged within this rune'\nApocalyptical Strength increased by 50%\nYou create fiery explosions on enemies when you score an Apocalyptical\nThese hit only once per enemy and inflict Thermal Blaze, short Daybreak, and Everlasting Suffering\nEverlasting Suffering increases enemy damage over time by 250% and makes other debuffs last until it ends\nDamage done, as well as Daybreak and Everlasting Suffering duration is boosted by your Apocalyptical Strength");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/CalamityRune"); }
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().apocalypticalStrength += 0.50f;
			player.GetModPlayer<SGAPlayer>().CalamityRune = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Red;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentSolar, 3);
			recipe.AddIngredient(mod.ItemType("Entrophite"), 25);
			recipe.AddIngredient(mod.ItemType("StygianCore"), 1);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 4);
			recipe.AddIngredient(mod.ItemType("Fridgeflame"), 5);
			recipe.AddIngredient(ItemID.HellstoneBar, 2);
			recipe.AddIngredient(ItemID.CrystalBall, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();*/
		}
	}

	public class RadSuit : SybariteGem,IRadioactiveDebuffText
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rad Suit");
			Tooltip.SetDefault("'Deflects punching at a molecular level!'\nGrants 50% increased radiation resistance and immunity to Radiation One\nYou create bursts of Radiation on enemies when you score an Apocalyptical\n"+Idglib.ColorText(Color.Red, "But only on enemies who are irradiated")+"\nDamage done is boosted by your Apocalyptical Strength\nIncreased effects of Handling Gloves");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Accessories/RadSuit"); }
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().RadSuit = true;
			player.GetModPlayer<IdgPlayer>().radresist += 0.50f;
			ModContent.GetInstance<HandlingGloves>().UpdateAccessory(player, hideVisual);
			player.buffImmune[ModLoader.GetMod("IDGLibrary").GetBuff("RadiationOne").Type] = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 2);
			item.rare = ItemRarityID.Red;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarOre, 20);
			recipe.AddIngredient(ModContent.ItemType<StarMetalBar>(), 16);
			recipe.AddIngredient(ItemID.LeadBar, 10);
			recipe.AddIngredient(ItemID.Leather, 20);
			recipe.AddIngredient(ItemID.SWATHelmet, 1);
			recipe.AddTile(ModContent.TileType<Tiles.ReverseEngineeringStation>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarOre, 50);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 25);
			recipe.AddIngredient(ItemID.LeadBar, 10);
			recipe.AddIngredient(ItemID.TatteredCloth, 5);
			recipe.AddTile(ModContent.TileType<Tiles.ReverseEngineeringStation>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	public class YoyoTricks : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Professional's Drop");
			Tooltip.SetDefault("Increases Yoyo velocity by 25%, does more damage the further out on the line it is\nCaps at 15% boost at max range\nHaving a longer reach can boost this even higher");
		}
		public override string Texture
		{
			get { return ("Terraria/OneDropLogo"); }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTime / 5f) % 1f, 0.50f, 0.75f);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().YoyoTricks = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 2);
			item.rare = ItemRarityID.LightRed;
			item.accessory = true;
		}
	}

	public class FridgeFlamesCanister : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fridgeflame Canister");
			Tooltip.SetDefault("Flamethrowers shoot Frostflames alongside normal flames\nThis also applies to any weapon that uses gel as its ammo\nFrostflames do not cause immunity frames and do 75% of the weapon's base damage\n5% increased Technological Damage\n+1 passive Electric Charge Rate in Underworld/Ice Biome\n+2 passive Electric Charge Rate While afflicted with OnFire! or Frostburn");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().FridgeflameCanister = true;
			player.GetModPlayer<SGAPlayer>().techdamage += 0.05f;
			if (player.ZoneSnow || player.ZoneUnderworldHeight)
            {
				player.GetModPlayer<SGAPlayer>().electricrechargerate += 1;
			}
			if (player.HasBuff(BuffID.OnFire) || player.HasBuff(BuffID.Frostburn))
				player.GetModPlayer<SGAPlayer>().electricrechargerate += 2;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Glass, 20);
			recipe.AddIngredient(mod.ItemType("Fridgeflame"), 10);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
	public class IceFlames : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Icey Flames");
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 90;
			projectile.extraUpdates = 2;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 10;
			projectile.coldDamage = true;
		}

		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Projectiles/BoulderBlast"); }
		}

		public override void AI()
		{
			int i = Main.rand.Next(0, 2);
			//for (int i = 0; i < 1; i += 1)
			//{
			int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 185, projectile.velocity.X * (i * 0.5f), projectile.velocity.Y * (i * 0.5f), 20, default(Color), 1.75f);
			Main.dust[DustID2].noGravity = true;
			//}

		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Frostburn, 60 * 4);
		}

	}

	[AutoloadEquip(EquipType.Back)]
	public class Transformer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Transformer");
			Tooltip.SetDefault("Electric Bolts will gain an additional arc\n+2000 Max Electric Charge\n+1 passive Electric Charge Rate, per Active Cooldown Stack");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			sgaply.electricrechargerate += player.HasBuff(BuffID.Electrified) ? 10 : 2;
			sgaply.electricChargeMax += 2000;
			sgaply.transformerAccessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<ManaBattery>(), 3);
			recipe.AddIngredient(ModContent.ItemType<VialofAcid>(), 12);
			recipe.AddIngredient(ModContent.ItemType<NoviteBar>(), 6);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class BlinkTech : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blink Tech Canister");
			Tooltip.SetDefault("Enables a short ranged blink teleport, hide to disable blinking\nHold UP and press left or right to teleport in the direction\ngives chaos state for 2 seconds, blinking not possible while you have chaos state\n5% increased Technological Damage\n+1500 Max Electric Charge, +1 passive Electric Charge Rate");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (!hideVisual)
			player.SGAPly().maxblink += 3;
			player.SGAPly().techdamage += 0.05f;
			player.SGAPly().electricChargeMax += 1500;
			player.SGAPly().electricrechargerate += 1;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Accessories/Canister4"); }
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(silver: 50);
			item.rare = ItemRarityID.LightRed;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Glass, 10);
			recipe.AddIngredient(ItemID.MeteoriteBar, 3);
			recipe.AddIngredient(mod.ItemType("ManaBattery"), 1);
			recipe.AddIngredient(mod.ItemType("VialofAcid"), 12);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}


	}
	public class HeartOfEntropy : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart of Entropy");
			Tooltip.SetDefault("You spawn vampric healing projectiles when you score an Apocalyptical\nThese heal based on damage done and are boosted by your Apocalyptical Strength\n1% increased Apocalyptical Chance\n'It lashes at at your soul to bring nothing but dismay'");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "accapocalypticaltext", SGAGlobalItem.apocalypticaltext));
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().HoE = true;
			for (int i = 0; i < player.GetModPlayer<SGAPlayer>().apocalypticalChance.Length; i += 1)
				player.GetModPlayer<SGAPlayer>().apocalypticalChance[i] += 1.0;
		}

		/*public override string Texture
		{
			get { return ("Terraria/Item_"+ItemID.DemonHeart); }
		}*/

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 32;
			item.defense = 0;
			item.accessory = true;
			item.height = 32;
			item.value = Item.sellPrice(gold: 5);
			item.rare = ItemRarityID.Lime;
			item.expert = false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(mod.ItemType("OmniSoul"), 5);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(mod.ItemType("Entrophite"), 75);
			recipe.AddIngredient(mod.ItemType("StygianCore"), 2);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.Shoes)]
	public class GravityStabilizerBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gravity Stabilizer Boots");
			Tooltip.SetDefault("Hold DOWN to fall faster in normal gravity\nBut only fall at normal speed in low gravity");
		}

        public override bool Autoload(ref string name)
        {
            SGAPlayer.PostPostUpdateEquipsEvent += SGAPlayer_PostPostUpdateEquipsEvent;
			return true;
        }

		private void SGAPlayer_PostPostUpdateEquipsEvent(SGAPlayer sgaply)
		{
			if (sgaply.player.velocity.Y != 0 && sgaply.player.controlDown && !sgaply.player.controlJump && sgaply.gravBoots == true)
			{
				if (sgaply.player.gravity >= Player.defaultGravity)
					sgaply.player.maxFallSpeed += 5;
				sgaply.player.gravity = sgaply.player.gravity < Player.defaultGravity ? Player.defaultGravity : sgaply.player.gravity + 1;
			}
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().gravBoots = true;
		}

        public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 26;
			item.defense = 0;
			item.accessory = true;
			item.height = 14;
			item.value = Item.sellPrice(silver: 75);
			item.rare = ItemRarityID.LightPurple;
			item.expert = false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 12);
			recipe.AddIngredient(ModContent.ItemType<Glowrock>(), 50);
			recipe.AddIngredient(ItemID.GravitationPotion, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.Shoes)]
	public class SoulSparkBoots : ModItem
	{
		public static int boots = -1;
		public static string SSBText => "All effects of Frostspark boots and Lava Waders improved\nJump Height significantly boosted, Knockback Immunity, and no Fall Damage suffered\nDouble Jump ability (with no wing time)\nHold DOWN to stabilize gravity\nToggle visibility to disable frog leg";
		public static string DemonSteppersText => "Immunity to Thermal Blaze, Acid Burn, Chilled, and Frozen\nGrants 25% increased radiation resistance\n" + PrimordialSkull.Text + "\nOn Fire! doesn't hurt you and slightly heals you instead";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SoulSpark Boots");
			Tooltip.SetDefault("'Obligatory Hardmode boots'\n"+SSBText);
		}

		public override bool Autoload(ref string name)
		{
			SGAPlayer.PostPostUpdateEquipsEvent += SGAPlayer_PostPostUpdateEquipsEvent;
			return true;
		}

		private void SGAPlayer_PostPostUpdateEquipsEvent(SGAPlayer player)
		{
			if (player.demonsteppers > 0)
			{
				if (player.demonsteppers > 1)
				{
					player.player.buffImmune[BuffID.OnFire] = false;
					player.player.buffImmune[BuffID.Chilled] = true;
					player.player.buffImmune[BuffID.Frozen] = true;
				}
				if (player.player.wingTime < 1)
				{
					player.player.doubleJumpCloud = true;
				}
			}
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.GetModPlayer<SGAPlayer>().demonsteppers<1)
			{
				player.GetModPlayer<SGAPlayer>().demonsteppers = 1;
				player.rocketBoots += 2;
				if (!player.GetModPlayer<SGAPlayer>().Walkmode)
				{
					player.accRunSpeed += 5f;
					player.runAcceleration += 0.05f;
					player.jumpSpeedBoost += 2.4f;
				}
				player.iceSkate = true;
				player.lavaMax += 500;
				player.waterWalk = true;
				player.fireWalk = true;
				player.jumpBoost = true;
				player.noFallDmg = true;
				player.autoJump = true;
				player.extraFall += 15;
				player.noKnockback = true;

			}

			ModContent.GetInstance<GravityStabilizerBoots>().UpdateAccessory(player, hideVisual);

		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string s = "Not Binded!";
			foreach (string key in SGAmod.WalkHotKey.GetAssignedKeys())
			{
				s = key;
			}
			tooltips.Add(new TooltipLine(mod, "DemonSteppersSpeed", "Toggle the speed/frog leg boost with the 'Walk Mode' hotkey (" + s + ")"));
			if (item.wingSlot > 0)
			{
				tooltips.Add(new TooltipLine(mod, "DemonSteppersSpeed", "Also effects the flying speed"));
			}
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 26;
			item.defense = 0;
			item.accessory = true;
			item.height = 14;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.Pink;
			item.expert = false;
			if (GetType() == typeof(DemonSteppers) || GetType() == typeof(SoulSparkBoots))
			{
				boots = item.shoeSlot;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GravityStabilizerBoots>(), 1);
			recipe.AddIngredient(ItemID.FrostsparkBoots, 1);
			recipe.AddIngredient(ItemID.LavaWaders, 1);
			recipe.AddRecipeGroup("SGAmod:HorseshoeBalloons", 1);
			recipe.AddIngredient(ItemID.FrogLeg, 1);
			recipe.AddIngredient(ModContent.ItemType<AmberGlowSkull>(), 1);
			recipe.AddIngredient(ModContent.ItemType<CobaltHorseshoe>(), 1);
			recipe.AddIngredient(ItemID.SoulofLight, 12);
			recipe.AddIngredient(ItemID.SoulofNight, 12);
			recipe.AddIngredient(ModContent.ItemType<DivineShower>(), 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.Shoes)]
	public class DemonSteppers : SoulSparkBoots
	{
		public static new int boots = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demon Steppers");
			Tooltip.SetDefault("'Obligatory Hardmode boots, but Better!'"+SoulSparkBoots.SSBText+"\n" + SoulSparkBoots.DemonSteppersText);
		}

        public override bool Autoload(ref string name)
        {
			return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.GetModPlayer<SGAPlayer>().NoFireBurn = 3;
			if (player.GetModPlayer<SGAPlayer>().demonsteppers<2)
			{
				player.GetModPlayer<SGAPlayer>().demonsteppers = 2;
			}

			ModContent.GetInstance<PrimordialSkull>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<AmberGlowSkull>().UpdateAccessory(player, hideVisual);
		}

        public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 26;
			item.defense = 0;
			item.accessory = true;
			item.height = 14;
			item.value = Item.sellPrice(gold: 25);
			item.rare = ItemRarityID.Lime;
			item.expert = false;
			if (GetType() == typeof(DemonSteppers) || GetType() == typeof(SoulSparkBoots))
			{
				boots = item.shoeSlot;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SoulSparkBoots>(), 1);
			recipe.AddIngredient(ItemID.HandWarmer, 1);
			recipe.AddIngredient(ModContent.ItemType < AmberGlowSkull>(), 1);
			recipe.AddIngredient(ModContent.ItemType < PrimordialSkull>(), 1);
			recipe.AddIngredient(ModContent.ItemType < Entrophite>(), 100);
			recipe.AddIngredient(ModContent.ItemType < StygianCore>(), 3);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 40);
			recipe.AddIngredient(ItemID.HellstoneBar, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
	public class BoosterMagnet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Booster Magnet");
			Tooltip.SetDefault("Attracts Nebula Boosters from a longer range");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().BoosterMagnet = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 32;
			item.height = 32;
			item.value = Item.sellPrice(gold: 3);
			item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FragmentNebula, 10);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class HeartreachMagnet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heartreach Magnet");
			Tooltip.SetDefault("Attracts Hearts from a longer range");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.lifeMagnet = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 32;
			item.height = 32;
			item.value = Item.sellPrice(gold: 2);
			item.rare = ItemRarityID.Lime;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HeartLantern, 1);
			recipe.AddIngredient(ItemID.HeartreachPotion, 5);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HeartLantern, 1);
			recipe.AddIngredient(ItemID.LifeFruit, 5);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class OmniMagnet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omni Magnet");
			Tooltip.SetDefault("Increased grab range for Nebula Boosters, Coins, Hearts, Mana Stars, and slightly for anything else\nEnemies drop coins on hit and NPCs offer you a discount\n'Suck it all up!'");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().graniteMagnet = true;
			player.GetModPlayer<SGAPlayer>().BoosterMagnet = true;
			player.manaMagnet = true;
			player.lifeMagnet = true;
			player.coins = true;
			player.discount = true;
			player.goldRing = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 32;
			item.height = 32;
			item.value = Item.sellPrice(gold: 75);
			item.rare = ItemRarityID.Red;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 12);
			recipe.AddIngredient(mod.ItemType("CobaltMagnet"), 1);
			recipe.AddIngredient(mod.ItemType("BoosterMagnet"), 1);			
			recipe.AddIngredient(mod.ItemType("HeartreachMagnet"), 1);
			recipe.AddIngredient(ItemID.GreedyRing, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class GreenApocalypse : DarkApocalypse
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Herald of Death");
			Tooltip.SetDefault("'And now, the 4th seal is broken'\nGrants 50% increased Apocalyptical Strength\nAnd 3% throwing Apocalyptical Chance while mounted\n" + Idglib.ColorText(Color.Red, "But your damage taken is slightly increased"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}

		public override void Tooltipsfunc(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "saddletext", "'and now, the 4th seal is broken'"));
			tooltips.Add(new TooltipLine(mod, "saddletext", "grants 50% increased Apocalyptical Strength"));
			tooltips.Add(new TooltipLine(mod, "saddletext", "And 3% throwing Apocalyptical Chance while mounted"));
			tooltips.Add(new TooltipLine(mod, "saddletext", Idglib.ColorText(Color.Red, "But your damage taken is slightly increased")));
		}

		public override void OnWear(SGAPlayer player)
		{
			player.damagetaken += 0.1f;
			player.apocalypticalChance[3] += 3.0;
		}
	}

	public class FireApocalypse : DarkApocalypse
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Herald of War");
			Tooltip.SetDefault("'Then the 2nd seal was broken'\nGrants 50% increased Apocalyptical Strength\nAnd 3% melee Apocalyptical Chance while mounted\n" + Idglib.ColorText(Color.Red, "But enemy spawn rates are increased"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}

		public override void Tooltipsfunc(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "saddletext", "'then the 2nd seal was broken'"));
			tooltips.Add(new TooltipLine(mod, "saddletext", "grants 50% increased Apocalyptical Strength"));
			tooltips.Add(new TooltipLine(mod, "saddletext", "And 3% melee Apocalyptical Chance while mounted"));
			tooltips.Add(new TooltipLine(mod, "saddletext", Idglib.ColorText(Color.Red, "But enemy spawn rates are increased")));
		}

		public override void OnWear(SGAPlayer player)
		{
			player.morespawns += 0.5f;
			player.apocalypticalChance[0] += 3.0;
		}
	}

	public class WhiteApocalypse : DarkApocalypse
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Herald of Pestilence");
			Tooltip.SetDefault("'The 1st seal is broken'\nGrants 50% increased Apocalyptical Strength\nAnd 3% magic Apocalyptical Chance while mounted\n" + Idglib.ColorText(Color.Red, "But may inflict self-bleeding randomly"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}
		public override void Tooltipsfunc(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "saddletext", "'The 1st seal is broken'"));
			tooltips.Add(new TooltipLine(mod, "saddletext", "Grants 50% increased Apocalyptical Strength"));
			tooltips.Add(new TooltipLine(mod, "saddletext", "And 3% magic Apocalyptical Chance while mounted"));
			tooltips.Add(new TooltipLine(mod, "saddletext", Idglib.ColorText(Color.Red, "But may inflict bleeding randomly")));

		}

		public override void OnWear(SGAPlayer player)
		{
			if (Main.rand.Next(0, 300) == 1)
				player.player.AddBuff(BuffID.Bleeding, 200);
			player.apocalypticalChance[2] += 3.0;
		}
	}

	public class DarkApocalypse : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Herald of Famine");
			//Tooltip.SetDefault("'thus the 3nd seal was broken'\ngrants 50% increased Apocalyptical Strength\nAnd 4% ranged Apocalyptical chance while mounted\n" + Idglib.ColorText(Color.Red,"But will nullify the effects of Well Fed"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}

		public virtual void Tooltipsfunc(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "saddletext", "'Thus the 3nd seal was broken'"));
			tooltips.Add(new TooltipLine(mod, "saddletext", "Grants 50% increased Apocalyptical Strength"));
			tooltips.Add(new TooltipLine(mod, "saddletext", "And 3% ranged Apocalyptical Chance while mounted"));
			tooltips.Add(new TooltipLine(mod, "saddletext", Idglib.ColorText(Color.Red, "But will nullify the effects of Well Fed")));

		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (GetType() == typeof(DarkApocalypse))
				Tooltipsfunc(tooltips);

			tooltips.Add(new TooltipLine(mod, "saddletext", Idglib.ColorText(Color.Red, "Limited to 1 saddle at a time")));
			tooltips.Add(new TooltipLine(mod, "saddletext", SGAGlobalItem.apocalypticaltext));
		}

		public virtual void OnWear(SGAPlayer player)
		{
			player.player.buffImmune[BuffID.WellFed] = true;
			player.apocalypticalChance[1] += 3.0;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (player.mount.Active)
			{
				OnWear(sgaplayer);
				sgaplayer.apocalypticalStrength += 0.50f;
			}
		}

		public override bool CanEquipAccessory(Player player, int slot)
		{
			bool canequip = true;
			for (int x = 3; x < 8 + player.extraAccessorySlots; x++)
			{
				if (player.armor[x].modItem != null)
				{
					Type myclass = player.armor[x].modItem.GetType();
					if (myclass.BaseType == typeof(DarkApocalypse) || myclass == typeof(DarkApocalypse))
					{

						//if (myType==mod.ItemType("MiningCharmlv1") || myType==mod.ItemType("MiningCharmlv2") || myType == mod.ItemType("MiningCharmlv3")){
						canequip = false;
						break;
					}
				}
			}
			return canequip;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SlimySaddle, 1);
			recipe.AddIngredient(ModContent.ItemType<WovenEntrophite>(), 50);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HardySaddle, 1);
			recipe.AddIngredient(ModContent.ItemType<WovenEntrophite>(), 40);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class ApocalypticCrystalBringer : OmegaSigil
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cataclysmic Catalyst");
			Tooltip.SetDefault("'You wield power not even The Council could ever have...'\n2% (5% at max health) increased Apocalyptical Chance, 50% increased Apocalyptical Strength\n10% chance to dodge attacks that would kill you\nJust Blocks against contact damage trigger Apocalypticals");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 20));
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Accessories/ApocalypticCrystalBringer"); }
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{

			Item itz = new Item();
			bool defined = false;
			if (Main.LocalPlayer.HeldItem != null)
			{
				if (Main.LocalPlayer.HeldItem.damage > 0)
				{
					itz.SetDefaults(ModContent.ItemType<GreenApocalypse>());
					defined = true;
				}
				if (Main.LocalPlayer.HeldItem.ranged)
				{
					itz.SetDefaults(ModContent.ItemType<DarkApocalypse>());
					defined = true;
				}
				if (Main.LocalPlayer.HeldItem.melee)
				{
					itz.SetDefaults(ModContent.ItemType<FireApocalypse>());
					defined = true;
				}
				if (Main.LocalPlayer.HeldItem.magic)
				{
					itz.SetDefaults(ModContent.ItemType<WhiteApocalypse>());
					defined = true;
				}
			}

			tooltips.Add(new TooltipLine(mod, "Saddletext", "--You copy the Saddle of your weapon's damage type--"));
			if (defined)
			{
				(itz.modItem as DarkApocalypse).Tooltipsfunc(tooltips);
			}

			tooltips.Add(new TooltipLine(mod, "accapocalypticaltext", SGAGlobalItem.apocalypticaltext));
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().apocalypticalStrength += 0.50f;
			player.SGAPly().diesIraeStone = true;
			base.UpdateAccessory(player, hideVisual);

			Item itz = new Item();
			bool defined = false;
			if (!Main.LocalPlayer.HeldItem.IsAir && Main.LocalPlayer.HeldItem.damage > 0 && SGAWorld.modtimer > 120)
			{
				if (Main.LocalPlayer.HeldItem.ranged)
				{
					itz.SetDefaults(ModContent.ItemType<DarkApocalypse>());
					defined = true;
				}
				if (Main.LocalPlayer.HeldItem.melee)
				{
					itz.SetDefaults(ModContent.ItemType<FireApocalypse>());
					defined = true;
				}
				if (Main.LocalPlayer.HeldItem.magic)
				{
					itz.SetDefaults(ModContent.ItemType<WhiteApocalypse>());
					defined = true;
				}
				if (Main.LocalPlayer.HeldItem.Throwing() != null)
				{
					if (!Main.LocalPlayer.HeldItem.IsAir && ((Main.LocalPlayer.HeldItem.modItem != null && Main.LocalPlayer.HeldItem.Throwing().thrown) || Main.LocalPlayer.HeldItem.thrown))
					{
						itz.SetDefaults(ModContent.ItemType<GreenApocalypse>());
						defined = true;
					}
				}
			}


			if (defined)
			{
				(itz.modItem as DarkApocalypse).UpdateAccessory(player, hideVisual);
			}

		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 24;
			item.height = 32;
			item.value = 2000000;
			item.rare = ItemRarityID.Purple;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DarkApocalypse>(), 1);
			recipe.AddIngredient(ModContent.ItemType<FireApocalypse>(), 1);
			recipe.AddIngredient(ModContent.ItemType<GreenApocalypse>(), 1);
			recipe.AddIngredient(ModContent.ItemType<WhiteApocalypse>(), 1);
			recipe.AddIngredient(ModContent.ItemType<DiesIraeStone>(), 1);
			recipe.AddIngredient(ModContent.ItemType<OmegaSigil>(), 1);
			recipe.AddIngredient(ModContent.ItemType<MoneySign>(), 15);
			recipe.AddIngredient(ModContent.ItemType<StygianCore>(), 3);
			recipe.AddIngredient(ModContent.ItemType<WovenEntrophite>(), 100);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class MVMUpgrade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("MVM Upgrade");
			Tooltip.SetDefault("Upgrades the effects of the Gas Passer and Jarate\nGas Passer Gains Explode on Ignite\nEnemies Take 350 Classless damage when ignited\nJarate gains the ability to slow enemies when Soddened\n'100+ tour elitist be like: GET UPGRADES OR KICK! REEEEE!'");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Yellow;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			sgaply.MVMBoost = true;
		}

	}

	[AutoloadEquip(EquipType.Waist)]
	public class ThrowerPouch : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thrower's Pouch");
			Tooltip.SetDefault("5% increased throwing damage, 3% increased throwing crit chance\n10% increase Throwing Velocity\n+5% increased chance to not consume throwing items");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 0, 25, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			player.Throwing().thrownDamage += 0.05f;
			player.Throwing().thrownCrit += 3;
			sgaply.Thrownsavingchance += 0.05f;
			player.Throwing().thrownVelocity += 0.10f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("IronBar", 1);
			recipe.AddIngredient(ItemID.Chain, 2);
			recipe.AddIngredient(ItemID.Leather, 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	/*
	public class TidalCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Charm");
			Tooltip.SetDefault("Increases your max Breath by 5 bubbles\nHaving less breath boosts your defense\nThis boost increases through progression as you beat SGAmod bosses");
		}

        public override bool Autoload(ref string name)
        {
			SGAPlayer.PostUpdateEquipsEvent += DefenseBoost;
			return true;
        }

        public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(0, 0, 75, 0);
			item.accessory = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "tidalcharm", Idglib.ColorText(Color.DeepSkyBlue, "Allows you to 'swim' in air while it is raining (hide the accessory to disable)")));
			tooltips.Add(new TooltipLine(mod, "tidalcharm", Idglib.ColorText(Color.DeepSkyBlue, "Most status effects that trigger while wet will trigger here")));
			if (!SGAWorld.tidalCharmUnlocked)
				tooltips.Add(new TooltipLine(mod, "tidalcharm", Idglib.ColorText(Color.DarkBlue, "Its full potential is locked behind the Tempest Sharkvern")));
			else
				tooltips.Add(new TooltipLine(mod, "tidalcharm", Idglib.ColorText(Color.DeepSkyBlue, "Tempest Sharkvern's defeat allows this to take effect when it is not raining")));
		}

		public void DefenseBoost(SGAPlayer sgaply)
		{
			if (sgaply.tidalCharm >= 2)
			{
				Player player = sgaply.player;
				int defensegiven = 8;
				defensegiven += SGAWorld.downedWraiths * 2;
				if (SGAWorld.downedSpiderQueen)
					defensegiven += 3;
				if (SGAWorld.downedMurk > 1)
					defensegiven += SGAWorld.GennedVirulent ? 6 : 3;
				if (SGAWorld.downedSharkvern)
					defensegiven += 3;
				if (SGAWorld.downedCratrosity)
					defensegiven += 3;
				if (SGAWorld.downedTPD)
					defensegiven += 3;
				if (SGAWorld.downedHarbinger)
					defensegiven += 3;
				if (SGAWorld.downedCratrosityPML)
					defensegiven += 3;
				if (SGAWorld.downedSPinky)
					defensegiven += 3;

				player.statDefense += Math.Max((int)((1f - ((float)player.breath / (float)player.breathMax)) * defensegiven), 0);
			}

		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.breathMax += 100;
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			if (!hideVisual && ((Main.raining) || SGAWorld.tidalCharmUnlocked))
			sgaply.tidalCharm = 2;

		}

	}
	*/


	public class TwinesOfFate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twines Of Fate");
			Tooltip.SetDefault("Spin a Guide and Clothier Voodoo Doll pair around you on a string\nThey reflect projectiles and take hits that direct damage to your NPCs\nReflected projectiles do practically no damage and the dolls suffer a cooldown\nThe dolls cease to function if their respective NPCs die");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.accessory = true;
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.GuideVoodooDoll); }
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			sgaply.twinesoffate = true;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[ItemID.GuideVoodooDoll];
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, item.Center + new Vector2(-6, 0) - Main.screenPosition, null, lightColor, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(Main.itemTexture[ItemID.ClothierVoodooDoll], item.Center + new Vector2(6, 0) - Main.screenPosition, null, lightColor, 0f, textureOrigin, 1f, SpriteEffects.FlipHorizontally, 0f);
			}
			return false;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[ItemID.GuideVoodooDoll];
				Vector2 slotSize = new Vector2(52f, 52f);
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, drawPos + new Vector2(-6, 0), null, drawColor, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);
				spriteBatch.Draw(Main.itemTexture[ItemID.ClothierVoodooDoll], drawPos + new Vector2(6, 0), null, drawColor, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.FlipHorizontally, 0f);
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WhiteString, 2);
			recipe.AddIngredient(ItemID.GuideVoodooDoll, 1);
			recipe.AddIngredient(ItemID.ClothierVoodooDoll, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.HandsOn)]
	public class RingOfRespite : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ring Of Respite");
			Tooltip.SetDefault("Grants an additional free Action Cooldown Stack\nDoes not stack with items crafted from a Ring Of Respite");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Orange;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (!player.SGAPly().noactionstackringofrespite)
			{
				player.SGAPly().MaxCooldownStacks += 1;
				player.SGAPly().noactionstackringofrespite = true;
			}
		}

	}
	public class VoidEmbracers : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Em'Bracers");
			Tooltip.SetDefault("'you don't shrivel up, you embrace your lost vitality'\nGrants 1 defense for every 25 HP lost\nGrants 1 defense for every 10 max HP lost\nIncreases max Regen Pool by 300, and 4 for every defense");
		}

        public override bool Autoload(ref string name)
        {
			SGAPlayer.PostUpdateEquipsEvent += PostUpdateAccessories;
			return true;
        }

        public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Lime;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.accessory = true;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
			player.SGAPly().voidEmbrancers = true;
		}

        private void PostUpdateAccessories(SGAPlayer sgaply)
		{
			if (sgaply.voidEmbrancers)
			{
				sgaply.player.statDefense += (int)(sgaply.player.GetModPlayer<IdgPlayer>().radationAmmount / 10f);
				sgaply.player.statDefense += (int)((sgaply.player.statLifeMax2 - sgaply.player.statLife) / 25);
				sgaply.healingPointsStatBoost += 300+sgaply.player.statDefense*4;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<UnmanedBar>(), 8);
			recipe.AddIngredient(ModContent.ItemType<WovenEntrophite>(), 40);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class DiesIraeStone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dies Irae Stone");
			Tooltip.SetDefault("'Judgement is at hand this day...'\nJust Blocking enemy contact damage with SGAmod Shields triggers Apocalypticals\nShield of Cthulhu bonking also triggers Apocalypticals");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Orange;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().diesIraeStone = true;
		}

	}

	[AutoloadEquip(EquipType.Neck)]
	public class NeckONerves : RingOfRespite
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Necklace O' Nerve");
			Tooltip.SetDefault("Increases: life regen, and grants an additional free Cooldown Stack\nIncreases mana by 20 (Corruption Worlds)\nIncreased movement speed when struck (Crimson Worlds)");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.lifeRegen = 1;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			if (WorldGen.crimson)
				player.panic = true;
			else
				player.statManaMax2 += 20;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BandofStarpower, 1);
			recipe.AddIngredient(ItemID.BandofRegeneration, 1);
			recipe.AddIngredient(mod.ItemType("RingOfRespite"), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PanicNecklace, 1);
			recipe.AddIngredient(ItemID.BandofRegeneration, 1);
			recipe.AddIngredient(mod.ItemType("RingOfRespite"), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.Back)]
	public class PlasmaPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Pack");
			Tooltip.SetDefault("20% increased Plasma capacity\n+3 Booster recharge rate\n+5000 Max Electric Charge, +3 passive Electric Charge Rate\nGrants the other effects of Blink Tech Canister and Transformer");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = ItemRarityID.Yellow;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			sgaply.boosterrechargerate += 3;
			sgaply.plasmaLeftInClipMax += (int)((float)SGAPlayer.plasmaLeftInClipMaxConst * 0.20f);
			sgaply.electricChargeMax += 1500;
			player.SGAPly().electricrechargerate += 2;
			ModContent.GetInstance<BlinkTech>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<Transformer>().UpdateAccessory(player, hideVisual);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BlinkTech>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Transformer>(), 1);
			recipe.AddIngredient(ModContent.ItemType<PlasmaCell>(), 2);
			recipe.AddIngredient(ModContent.ItemType<PrismalBar>(), 8);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.Back)]
	public class GunBarrelParts : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gun Barrel Parts");
			Tooltip.SetDefault("Revolvers reload 20% faster\n6% increased Bullet Damage");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			sgaply.RevolverSpeed += 0.20f;
			player.bulletDamage += 0.06f;
		}
	}

	[AutoloadEquip(EquipType.HandsOff)]
	public class SecondCylinder : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Second Cylinder");
			Tooltip.SetDefault("Grants 2 extra bullets per clip while using Revolvers\n6% increased Bullet Damage");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			player.bulletDamage += 0.06f;
			sgaply.ammoLeftInClipMaxStack += 2;
		}

	}
	[AutoloadEquip(EquipType.Waist)]
	public class GunsmithsBeltofTools : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gunsmith's Belt of Tools");
			Tooltip.SetDefault("Grants 2 extra bullets per clip while using Revolvers\nRevolvers reload 20% faster\n10% increased Bullet Damage");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = 5;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			sgaply.RevolverSpeed += 0.20f;
			sgaply.ammoLeftInClipMaxStack += 2;
			player.bulletDamage += 0.10f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("SecondCylinder"), 1);
			recipe.AddIngredient(mod.ItemType("GunBarrelParts"), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}


	[AutoloadEquip(EquipType.Neck)]
	public class PeacekeepersDuster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Peacekeeper's Duster");
			Tooltip.SetDefault("Take 20% less damage while reloading your revolver\nHostile bullets do 50% less damage to you\nGain bonus Life Regen while in your town (1 per npc up to 10 max)\n'Keepin order will never be easier with this'");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = 6;
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.accessory = true;
		}

		public override void HoldItem(Player player)
		{
			if (item.value == Item.buyPrice(0, 10, 0, 0))
				Item.sellPrice(0, 2, 50, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			sgaply.Duster = true;
			if (player.townNPCs > 0)
				player.lifeRegen += (int)Math.Min(player.townNPCs, 10);
			if (player.itemAnimation < 1 && sgaply.ReloadingRevolver > 0)
				sgaply.damagetaken /= 1.20f;
		}

	}


	[AutoloadEquip(EquipType.Shoes)]
	public class SparingSpurs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sparing Spurs");
			Tooltip.SetDefault("Gain movement speed, acceleration, and 8 defense while reloading your revolver\nGrants a Shield of Cthulhu Dash and 3 defense while firing your revolver\nFall Damage, fireblock, and (if grounded) knockback immunity\n'Ya ready to dance pardner?'");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = 3;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.accessory = true;
			item.defense = 4;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();

			if (sgaply.IsRevolver(player.HeldItem) > 0)
			{
				if (player.itemAnimation > 0)
				{
					player.dash = 2;
				}
			}

			if (player.itemAnimation < 1 && sgaply.ReloadingRevolver > 0)
			{
				if (Math.Abs(player.velocity.X) > 2f && player.velocity.Y == 0.0 && !player.mount.Active && !player.mount.Cart)
				{
					float posX;
					if (player.direction > 0)
					{
						posX = player.position.X + (player.direction);
					}
					else
					{
						posX = player.position.X + (player.direction) + 16;
					}
					Vector2 pos = new Vector2(posX, player.position.Y + 36);
					int dust = Dust.NewDust(pos - new Vector2(12, 6), 24, 12, DustID.Sandstorm, (player.velocity.X) + Math.Sign(player.direction) * -12f, player.velocity.Y - 5f, 0, Color.Yellow, 2f);
					int slot = -1;
					for (int l = 3; l < 8 + player.extraAccessorySlots; l++)
					{
						if (player.armor[l].type == item.type)
						{
							slot = l;
							break;
						}
					}
					Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(player.dye[slot].dye, player);
					Main.dust[dust].noGravity = true;
				}
				player.accRunSpeed += 4.0f;
				player.runAcceleration += 0.50f;
				player.moveSpeed += 0.75f;
			}
			player.noFallDmg = true;
			player.fireWalk = true;
			if (player.velocity.Y == 0)
				player.noKnockback = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CobaltHorseshoe>(), 1);
			recipe.AddIngredient(ItemID.ShoeSpikes, 1);
			recipe.AddIngredient(ItemID.Spike, 15);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.Face)]
	public class HighNoon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("High Noon");
			Tooltip.SetDefault("Damage is increased during the day\nBase damage increase reaches its peak of 10% increased damage at noon\nAdditional 6% bullet damage at noon");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = 3;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (Main.dayTime)
			{
				float scale = (GetType() == typeof(HighNoon) ? 15000f : 25000f);
				float peak = MathHelper.Clamp(1f - ((float)Math.Abs(27000.00 - Main.time) / scale), 0f, 1f);
				float ammount = 0.10f * peak;
				player.BoostAllDamage(ammount, 0);
				player.bulletDamage += (0.06f * peak);
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sunglasses, 1);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 8);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.Face)]
	public class DuelingDeity : HighNoon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dueling Deity's Shades");
			Tooltip.SetDefault("Scoring an Apocalyptical summons 2 bullets to strike your opponent\nThese projectiles are consumed from your ammo slots and do 50% more damage but cannot crit\nThey do not count as ranged damage either\nEffects of High Noon, span of day improved");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = 7;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			sgaply.dualityshades = true;
			base.UpdateAccessory(player, hideVisual);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("HighNoon"), 1);
			recipe.AddRecipeGroup("SGAmod:IchorOrCursed", 12);
			recipe.AddIngredient(mod.ItemType("Entrophite"), 50);
			recipe.AddIngredient(mod.ItemType("StygianCore"), 2);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}


	//Set Bonus stuff
	//Damaging an enemy marks them for a dual with you\nYour damage against your enemy is increased by 25%\nHowever your damage against other enemies is reduced by 50%\nThe duel ends when you damage a different enemy or your enemy dies\nThere is a cooldown between duels, and only ranged damage can start and stop duels"



	[AutoloadEquip(EquipType.Waist, EquipType.Neck)]
	public class GunslingerLegend : SparingSpurs
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gunslinger of Song and Legend");
			Tooltip.SetDefault("'Riding high, in the sky...'\n'Its that time; its high Noon'");
		}

		public override void SetDefaults()
		{
			item.defense = 4;
			item.width = 18;
			item.height = 24;
			item.rare = ItemRarityID.Cyan;
			item.value = Item.sellPrice(0, 75, 0, 0);
			item.accessory = true;
		}

        public override bool Autoload(ref string name)
        {
            SGAnpcs.DoModifiesLateEvent += SGAnpcs_DoModifiesLateEvent;
			return true;
        }

        private void SGAnpcs_DoModifiesLateEvent(NPC npc, Player player, Projectile projectile, Item item, ref int sourcedamage, ref int damage, ref float knockback, ref bool crit)
        {
			SGAPlayer moddedplayer = player.SGAPly();
			if (moddedplayer.gunslingerLegend && moddedplayer.gunslingerLegendtarget > -1)
			{
				if (npc.whoAmI == moddedplayer.gunslingerLegendtarget)
				{
					damage = (int)((float)damage * 1.75f);
				}
				else
				{
					damage = (int)((float)damage * 0.25f);
				}
			}
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string s = "Not Binded!";
			foreach (string key in SGAmod.GunslingerLegendHotkey.GetAssignedKeys())
			{
				s = key;
			}
			tooltips.Add(new TooltipLine(mod, "GunSlingerLegendTooltip", "Press the 'Gunslinger Legend' hotkey to challenge the enemy nearest your mouse curser to a duel (" + s + ")"));
			tooltips.Add(new TooltipLine(mod, "GunSlingerLegendTooltip", "You do 75% increased damage against this target, but 75% reduced damage to anything else"));
			tooltips.Add(new TooltipLine(mod, "GunSlingerLegendTooltip", Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 30 seconds")));
			tooltips.Add(new TooltipLine(mod, "GunSlingerLegendTooltip", "Combines the Effects of:"));
			tooltips.Add(new TooltipLine(mod, "GunSlingerLegendTooltip", "-Sparing Spurs and Peacekeeper's Duster"));
			tooltips.Add(new TooltipLine(mod, "GunSlingerLegendTooltip", "-Gunsmith's Belt of Tools and Dueling Deity's Shades"));
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			ModContent.GetInstance<GunsmithsBeltofTools>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<DuelingDeity>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<PeacekeepersDuster>().UpdateAccessory(player, hideVisual);
			sgaply.gunslingerLegend = true;
			base.UpdateAccessory(player, hideVisual);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("PeacekeepersDuster"), 1);
			recipe.AddIngredient(mod.ItemType("DuelingDeity"), 1);
			recipe.AddIngredient(mod.ItemType("GunsmithsBeltofTools"), 1);
			recipe.AddIngredient(mod.ItemType("SparingSpurs"), 1);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 20);
			recipe.AddIngredient(ItemID.LunarBar, 15);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}


	[AutoloadEquip(EquipType.Waist)]
	public class NinjaSash : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ninja's Stash");
			Tooltip.SetDefault("Scoring crits with your throwing weapons summon shurikens and throwing knives to strike hit enemies\nThese summoned attacks cannot crit or trigger Thrower damage\nFurthermore, they are limited by your throwing damage\nFinally they are consumed from the player's inventory on use");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			sgaply.ninjaSash = Math.Max(sgaply.ninjaSash, 1);
		}

	}

	[AutoloadEquip(EquipType.Waist)]
	public class ShinSash : ThrowerPouch
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shin Sash");
			Tooltip.SetDefault("Press 'Shin Sash' key to throw out an explosive short fused smoke bomb\nSmoke bombed enemies are highly likely to miss (Ninja Dodge)\nFriendly players nearby the explosion Ninja Dodge and gain IFrames\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 60 seconds each") + "\nThrowing damage is increased by 10% and crit chance by 5%\nEffects of Ninja Sash and Thrower Pouch");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightPurple;
			item.value = Item.sellPrice(0, 8, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			sgaply.ninjaSash = Math.Max(sgaply.ninjaSash, 2);
			player.Throwing().thrownDamage += 0.10f;
			player.Throwing().thrownCrit += 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("NinjaSash"), 1);
			recipe.AddIngredient(mod.ItemType("ThrowerPouch"), 1);
			recipe.AddIngredient(ItemID.Katana, 1);
			recipe.AddIngredient(ItemID.SmokeBomb, 100);
			recipe.AddIngredient(mod.ItemType("OmniSoul"), 15);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 8);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class ShinobiShiv : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shinobi's Shadow");
			Tooltip.SetDefault("When you Ninja Dodge an attack, you turn invisible and gain Striking moment for 4 seconds");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = 6;
			item.value = Item.buyPrice(0, 60, 0, 0);
			item.accessory = true;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Black * 0.50f;
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.WinterCape); }
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			sgaply.shinobj = 3;
		}

	}


	[AutoloadEquip(EquipType.Waist)]
	public class KouSash : ShinSash
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kou Sash");
			Tooltip.SetDefault("'A true Assassin's attire'\nScoring an Apocalyptical imbues all your currently thrown projectiles\nImbued projectiles gain 50% increased damage and inflict Moonlight Curse\nThrowing damage is increased by 10% and crit chance by 5%\nShin Sash's Ninja Bombs can now effect even enemies immune to debuffs\nCombines the effects of:\n-Shin Sash\n-Master Ninja Gear\n-Gi\n-Shinobi's Shadow\n-Bundle of Jab-lin Parts");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Cyan;
			item.defense = 4;
			item.handOnSlot = 11;
			item.handOffSlot = 6;
			item.shoeSlot = 14;
			item.value = Item.sellPrice(0, 25, 0, 0);
			item.accessory = true;
			item.handOnSlot = 11;
			item.handOffSlot = 6;
			item.shoeSlot = 14;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.blackBelt = true;
			player.dash = 1;
			player.spikedBoots = 2;
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			ModContent.GetInstance<ShinobiShiv>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<JavelinBundle>().UpdateAccessory(player, hideVisual);
			sgaply.ninjaSash = Math.Max(sgaply.ninjaSash, 3);

			player.Throwing().thrownCrit += 5;//5%
			player.Throwing().thrownDamage += 0.10f;//15%

			player.BoostAllDamage(0.05f, 5);//Gi

			//player.meleeDamage += 0.05f; player.rangedDamage += 0.05f; player.magicDamage += 0.05f; player.minionDamage += 0.05f;
			//player.rangedCrit += 5; player.magicCrit += 5; player.meleeCrit += 5;
			//SGAmod.BoostModdedDamage(player, 0.05f, 5);

			player.meleeSpeed += 0.10f;
			player.moveSpeed += 0.10f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ShinSash"), 1);
			recipe.AddIngredient(ItemID.MasterNinjaGear, 1);
			recipe.AddIngredient(ItemID.Gi, 1);
			recipe.AddIngredient(mod.ItemType("ShinobiShiv"), 1);
			recipe.AddIngredient(mod.ItemType("JavelinBundle"), 1);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 12);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 16);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
	public class SnakeEyes : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snake Eyes");
			Tooltip.SetDefault("All crit chance is reduced by 10%\nEvery non-crit increases damage by 1% up to 100%\nGetting a Crit resets the bonus\nAttacks have 1% to false crit, also resetting the bonus");
		}

        public override bool Autoload(ref string name)
        {
			SGAPlayer.PostUpdateEquipsEvent += AfterEquip;
			return true;
        }

        public static void AfterEquip(SGAPlayer sgaplayer)
		{
			if (!sgaplayer.snakeEyes.Item1)
            {
				sgaplayer.snakeEyes.Item2 = 0;
			}
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.BoostAllDamage(0, -10);
			player.SGAPly().snakeEyes = (true, player.SGAPly().snakeEyes.Item2);
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}
	}
	public class RussianRoulette : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Russian Roulette");
			Tooltip.SetDefault("ammo using weapons' damage is increased by 50% (100% for Revolvers)\n"+Idglib.ColorText(Color.Red,"But there's a 1/6th chance of shooting yourself instead"));
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().russianRoulette = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}
	}
	public class CardDeckPersona : Weapons.Almighty.TheJoker
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hearts of the Cards");
			Tooltip.SetDefault("Cooldown Stack times on Almighty cards are reduced by half\nYou have a chance to pull A Joker whenever you use a card\nJokers steal one of your hearts and enter your inventory\nSuccessfully using a card causes you to get your hearts back\n'Lookin' cool Joker!'");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().personaDeck = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
	}

	public class LiquidGambling : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Liquified Gambling");
			Tooltip.SetDefault("'You don't want to question this substance...'\nTag a swig if you want to test your luck...\n"+ Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 60 seconds each") +"\nGreatly speeds up the opening of Contraband Crates when worn\nAlso works while in inventory");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			UpdateInventory(player);
		}

        public override void UpdateInventory(Player player)
        {
			player.SGAPly().liquidGambling = 5;
		}

        public override bool CanUseItem(Player player)
        {
			return player.SGAPly().AddCooldownStack(60 * 60, 1);
        }

        public override bool UseItem(Player player)
        {
			List<int> possibleEvents = new List<int>();
			possibleEvents.Add(0);
			possibleEvents.Add(1);
			possibleEvents.Add(2);
			if (!NPC.BusyWithAnyInvasionOfSorts())
			possibleEvents.Add(3);
			possibleEvents.Add(4);
			possibleEvents.Add(5);

			possibleEvents = possibleEvents.OrderBy(testby => Main.rand.Next()).ToList();

			switch (possibleEvents[0])
			{

				case 5:

					Item item = new Item();
					item.SetDefaults(ItemID.GenderChangePotion);
					player.inventory[player.inventory.Length - 1] = item;
					player.selectedItem = player.inventory.Length - 1;
					player.controlUseItem = true;
					ItemLoader.UseItem(player.inventory[player.inventory.Length - 1], player);
					break;

				case 4:

					Idglib.Chat("And so... I shall make it rain!"+ "[i: " + ModContent.ItemType<MoneySign>() + "]",220,0,220);

					List<int> items = new List<int>() {ItemID.FossilOre,ItemID.SilverCoin,ItemID.CopperCoin,ItemID.GoldOre,ItemID.PlatinumOre };
					if (Main.hardMode) 
					{
						items.Add(ItemID.AdamantiteOre);
						items.Add(ItemID.TitaniumOre);
					}
					for (int i = 0; i < 100; i += 1)
					{
						Item.NewItem(player.Center + new Vector2(Main.rand.Next(-1200, 1200), Main.rand.Next(-2600, -1600)), items[Main.rand.Next(items.Count)]);
					}
					break;
				case 3:
					Main.StartInvasion(Main.hardMode ? Main.rand.Next(1,4) : 1);
					break;
				case 2:
					Vector2 where = player.Center + new Vector2(0, -64);
					NPC.NewNPC((int)where.X, (int)where.Y, NPCID.DD2EterniaCrystal);
					//Terraria.GameContent.Events.DD2Event.StartInvasion();
					break;
				case 1:
					Projectile.NewProjectile(player.Center+new Vector2(0, -64),-Vector2.UnitY, ProjectileID.CoinPortal, 50000, 10f);
					break;

				default:
					player.DropSelectedItem();
					player.QuickSpawnItem(ItemID.RedPotion,1);
					player.AddBuff(ModContent.BuffType<Idglibrary.Buffs.CurseOfRed>(), 60 * 2);
					break;
			}
			return true;
        }

        public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(silver: 50);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;

			item.useStyle = ItemUseStyleID.EatingUsing;
			item.UseSound = SoundID.Item3;
			item.useAnimation = 15;
			item.useTime = 15;
			item.noMelee = true;
		}
	}

	public class HighStakesSet : LiquidGambling
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("High Stakes Set");
			Tooltip.SetDefault("'Who knows... Where the whims of fate may lead us.'\nDamage taken and given will be between half or doubled\nSelf caused damage will be greatly reduced by 75%\nIncludes the effects of all the gambling conponents");
		}

		public override bool Autoload(ref string name)
		{
            SGAnpcs.DoModifiesLateEvent += SGAnpcs_DoModifiesLateEvent;
			return true;
		}

		private void SGAnpcs_DoModifiesLateEvent(NPC npc, Player player, Projectile projectile, Item item, ref int sourcedamage, ref int damage, ref float knockback, ref bool crit)
		{
			if (player != null && player.SGAPly().highStakesSet)
			{
				damage = (int)(damage * Main.rand.NextFloat(0.50f, 2.00f));
			}
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			if (GetType() == typeof(HighStakesSet))
			{
				player.SGAPly().highStakesSet = true;
				ModContent.GetInstance<CardDeckPersona>().UpdateAccessory(player, hideVisual);
				ModContent.GetInstance<RussianRoulette>().UpdateAccessory(player, hideVisual);
				ModContent.GetInstance<SnakeEyes>().UpdateAccessory(player, hideVisual);
			}
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 5);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SnakeEyes>(), 1);
			recipe.AddIngredient(ModContent.ItemType<RussianRoulette>(), 1);
            recipe.AddIngredient(ModContent.ItemType<CardDeckPersona>(), 1);
			recipe.AddIngredient(ModContent.ItemType<LiquidGambling>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TerrariacoCrateBase>(), 1);
			recipe.AddIngredient(ItemID.LockBox, 1);
			recipe.AddIngredient(ItemID.Stake, 100);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class CobwebRepellent : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobweb Repellent");
			Tooltip.SetDefault("Breaks cobwebs 3 times as fast when stuck on them");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().cobwebRepellent = Math.Max(player.SGAPly().cobwebRepellent, (byte)1);
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 26;
			item.defense = 0;
			item.accessory = true;
			item.height = 14;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Blue;
		}
	}

	public class ExperimentalPathogen : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Experimental Pathogen");
			Tooltip.SetDefault("'takes advantage of a weakened body, allowing further damage'\n'But it weakens you as well'\nGain 6% increased damage against enemies per buff/debuff applied to them\n"+Idglib.ColorText(Color.Red,"However, you suffer 5% more DoT damage per debuff on yourself")+ "\nStacks up to a max of 5 buffs/debuffs\nFurthermore, attacks have a 25% chance to extend enemy debuffs");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().experimentalPathogen = true;
			float debuffs = 0f;

			for (int i = 0; i < player.buffTime.Length; i += 1)
			{
				if (player.buffTime[i] > 0 && player.buffType[i] >= 0 && Main.debuff[player.buffType[i]])
                {
					debuffs++;
					if (debuffs >= 5)
						break;
				}
			}
			player.SGAPly().DoTResist += 0.05f * debuffs;
		}

		public override bool Autoload(ref string name)
		{
			SGAnpcs.DoModifiesLateEvent += SGAnpcs_DoModifiesLateEvent;
			return true;
		}

		private void SGAnpcs_DoModifiesLateEvent(NPC npc, Player player, Projectile projectile, Item item, ref int sourcedamage, ref int damage, ref float knockback, ref bool crit)
		{
			if (player != null && player.SGAPly().experimentalPathogen)
			{
				int dam = sourcedamage;

				if (Main.rand.Next(100) < 25)
                {
					List<int> buffIndex = new List<int>();
					for (int i = 0; i<npc.buffTime.Length; i++)
                    {
						if (npc.buffTime[i] > 0 && Main.debuff[npc.buffType[i]])
						{
							buffIndex.Add(i);
						}
                    }
					if (buffIndex.Count > 0)
					{
						buffIndex = buffIndex.OrderBy(testby => Main.rand.Next()).ToList();
						int extendTime = Math.Min(10+(sourcedamage / 3), 180);
						npc.buffTime[buffIndex[0]] = (int)Math.Min(npc.buffTime[buffIndex[0]]+extendTime,Math.Max(npc.buffTime[buffIndex[0]],300));
					}
                }

				int increase = 0;

				for (int i = 0; i < npc.buffType.Length; i += 1)
				{
					if (npc.buffTime[i] > 30)
						increase++;
					if (increase >= 5)
						break;
				}

				damage = (int)(dam * (1f + Math.Min(increase * 0.06f, 1f)));
			}
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}

	}
	public class ConcussionDevice : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Concussion Device");
			Tooltip.SetDefault("'This device will provide a stronger blow to those who can normally take it'\nDo more damage the more knockback immunity an enemy has; Up to a max of 20%\nAdditionally: Effectiveness is improved by 5% of your attack's knockback");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 4));
		}

		public override bool Autoload(ref string name)
		{
			SGAnpcs.DoModifiesLateEvent += SGAnpcs_DoModifiesLateEvent;
			return true;
		}

		private void SGAnpcs_DoModifiesLateEvent(NPC npc, Player player, Projectile projectile, Item item, ref int sourcedamage, ref int damage, ref float knockback, ref bool crit)
		{
			if (player != null && player.SGAPly().concussionDevice)
			{
				int dam = sourcedamage;
				float effectiveness = Math.Max((1f - npc.knockBackResist) * 0.20f, 0);
				damage = (int)(dam * (1f + effectiveness));
				player.SGAPly().concussionDeviceEffectiveness = 1f-npc.knockBackResist;
			}
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().concussionDevice = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}
	}

	public class TheBountyHunter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Bounty Hunter's Mark");
			Tooltip.SetDefault("Reciving a Banner drop causes the killed enemy to drop 10 times loot\nThis will also reduce banner requirements by 10 kills");
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
	}

	public class TheHitList : TheBountyHunter
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Hit List");
			Tooltip.SetDefault("Grants a banner buff against the last enemy hit\nRequires atleast 50 kills of that enemy and they must have a banner drop\nReciving a Banner drop causes the killed enemy to drop 10 times loot\nThis will also reduce banner requirements by 10 kills");
		}

        public override string Texture => "Terraria/Item_"+ItemID.ThePlan;

        public override void UpdateInventory(Player player)
        {
			player.accJarOfSouls = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);

			player.accJarOfSouls = true;

			if (player.lastCreatureHit > 0 && NPC.killCount[player.lastCreatureHit] >= 50)
			{
				player.NPCBannerBuff[player.lastCreatureHit] = true;
			}
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TallyCounter, 1);
			recipe.AddIngredient(ItemID.TatteredCloth, 4);
			recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.DankCore>(), 2);
			recipe.AddIngredient(ModContent.ItemType<TheBountyHunter>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}

	public class NoviteCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Core");
			Tooltip.SetDefault("'Core of modern technology, could be put to good use'\n10% increased Technological and Trap damage\nCharge is built up by running around at high speeds (600/Second)\n20% reduced Electric Consumption, +2500 Max Electric Charge, +2 passive Electric Charge Rate");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().techdamage += 0.10f;
			player.SGAPly().TrapDamageMul += 0.10f;
			player.SGAPly().electricChargeMax += 2500;
			player.SGAPly().electricrechargerate += 2;
			player.SGAPly().electricChargeCost *= 0.80f;

			player.SGAPly().Noviteset = Math.Max(player.SGAPly().Noviteset, 1);
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 2);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("NoviteBar"), 12);
			recipe.AddIngredient(mod.ItemType("NoviteHelmet"), 1);
			recipe.AddIngredient(mod.ItemType("NoviteChestplate"), 1);
			recipe.AddIngredient(mod.ItemType("NoviteLeggings"), 1);
			recipe.AddIngredient(ItemID.HallowedBar, 8);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class NovusCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Core");
            Tooltip.SetDefault("'Core of untapped magic, could be put to good use'\nAny item that uses Novus Bars or Fiery Shards in its crafting recipe get both the following:\n10% increased damage and emits Novus particles\n5% faster use time on all items\nIncludes all other effects of Novus set bonus\nGain an additional free Cooldown Stack (only one from crafting tree)");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().Novusset = 5;
			if (!player.SGAPly().novusStackBoost)
			{
				player.SGAPly().MaxCooldownStacks += 1;
				player.SGAPly().novusStackBoost = true;
			}
			player.SGAPly().UseTimeMul += 0.05f;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 2);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 12);
			recipe.AddIngredient(mod.ItemType("UnmanedHood"), 1);
			recipe.AddIngredient(mod.ItemType("UnmanedBreastplate"), 1);
			recipe.AddIngredient(mod.ItemType("UnmanedLeggings"), 1);
			recipe.AddIngredient(ItemID.HallowedBar, 8);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
	public class NoviteChip : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Command Chip");
			Tooltip.SetDefault("Increases your max number of sentries\nImproves Max Electric Charge by 1000");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxTurrets += 1;
			player.SGAPly().electricChargeMax += 1000;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(silver: 50);
			item.rare = ItemRarityID.Blue;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("NoviteBar"), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
	public class NovusSummoning : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Summoning Orb");
			Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.PygmyNecklace"));
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions += 1;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(silver: 50);
			item.rare = ItemRarityID.Blue;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Neck)]
	public class AuraclesInsight : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Auracle's Insight");
			Tooltip.SetDefault("Auras are boosted by 1 power level\n10% increased minion damage");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.minionDamage += 0.10f;
			(float, float) boost = (player.SGAPly().auraBoosts.Item1+1f, player.SGAPly().auraBoosts.Item2);
			player.SGAPly().auraBoosts = boost;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 2);
			item.rare = ItemRarityID.Blue;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrystalShard, 25);
			recipe.AddIngredient(ModContent.ItemType<CryostalBar>(), 8);
			recipe.AddIngredient(ItemID.Chain, 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.Neck)]
	public class PrismalNecklace : AuraclesInsight
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Necklace");
			Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.PygmyNecklace") + " (by 2), and sentries by 1\n25% increased minion damage\nImproves Max Electric Charge by 1000\nAuras are boosted by 1 power level\n" + Idglib.ColorText(Color.Red, "All other damage and crit chance is reduced by 10%"));
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player,hideVisual);
			player.maxMinions += 2;
			player.maxTurrets += 1;
			player.minionDamage += 0.35f;
			player.minionKB += 2f;
			player.BoostAllDamage(-0.10f, -10);
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(silver: 50);
			item.rare = ItemRarityID.Cyan;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NovusSummoning>(), 1);
			recipe.AddIngredient(ModContent.ItemType< NoviteChip>(), 1);
			recipe.AddIngredient(ItemID.SummonerEmblem, 1);
			recipe.AddIngredient(ItemID.PygmyNecklace, 1);
			recipe.AddIngredient(ModContent.ItemType<PrismalBar>(), 10);
			recipe.AddIngredient(ModContent.ItemType<AuraclesInsight>(), 1);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 32);

			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}


	[AutoloadEquip(EquipType.HandsOn)]
	public class BustlingFungus : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bustling Fungus");
			Tooltip.SetDefault("Standing still creates an aura of fungal healing around you\nYour stationary sentries also create this aura");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().bustlingFungus.Item1 = true;
			if (player.SGAPly().bustlingFungus.Item2 == 180)
			Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Items.Accessories.BungalHealingAura>(), 0, 0, player.whoAmI);
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 26;
			item.defense = 0;
			item.accessory = true;
			item.height = 14;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
		}
	}

	public class BungalHealingAura : ModProjectile
	{
		public class ShroomEffects
        {
			public Vector2 position;
			public Vector2 velocity;
			public float fadeIn = 0f;
			public float fadeInRate;
			public int timeLeft = 0;
			public int timeMax = 0;
			public int timeAdd = 0;

			public ShroomEffects(Vector2 position,Vector2 velocity,int time,float fadeInRate)
            {
				this.position = position;
				this.velocity = velocity;
				this.fadeInRate = fadeInRate;
				this.timeMax = time;
				this.timeLeft = time;
			}

			public void Update()
            {
				position += velocity;
				fadeIn += fadeInRate;
				timeAdd++;
				timeLeft -= 1;
			}
			public void Draw()
            {

            }

		}

		protected Effect effect => SGAmod.TrailEffect;
		public virtual float RingSize
		{

			get
			{
				Player player = Main.player[projectile.owner];

				return (200f* (1f+(player.SGAPly().auraBoosts.Item1/2f)) * MathHelper.Clamp(projectile.timeLeft / (float)MaxTime, 0f, 1f));

			}
		}
		public virtual int MaxSize => 160;
		public virtual int MaxTime => 30;
		public virtual int HealingRate
		{

			get
			{
				Player player = Main.player[projectile.owner];
				return (int)(10f * (1f + (player.SGAPly().auraBoosts.Item1 / 2f)));
			}

		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bungal Healing Aura");
		}

		public override string Texture => "SGAmod/HopefulHeart";

		public override void SetDefaults()
		{
			projectile.width = 160;
			projectile.height = 160;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.alpha = 40;
			projectile.timeLeft = 30;
			projectile.extraUpdates = 0;
			projectile.ignoreWater = true;
			projectile.damage = 20;
		}

		public override void AI()
		{

			if (projectile.timeLeft > 28)
			{
				float realsize = (RingSize * 0.90f);
				bool playerbased = projectile.ai[1] <= 0 && (projectile.owner < Main.maxPlayers && Main.player[projectile.owner].velocity.Length() < 0.05f);
				bool projbased = projectile.ai[1] > 0 && Main.projectile[(int)projectile.ai[1] - 1].active && Main.projectile[(int)projectile.ai[1] - 1].sentry && Main.projectile[(int)projectile.ai[1] - 1].velocity.Length() < 0.25f;

				if (playerbased || projbased)
				{
					Player playz = Main.player[projectile.owner];
					if (playz.SGAPly().bustlingFungus.Item1)
					{
						projectile.timeLeft = 30;
						Projectile masterproj = projbased ? Main.projectile[(int)projectile.ai[1] - 1] : null;

						Entity ent = projbased ? (Entity)masterproj : (Entity)Main.player[projectile.owner];
						projectile.Center = ent.Center;
					}


					if (projectile.localAI[0] > MaxTime && projectile.timeLeft > 29)
					{
						foreach (Player player in Main.player.Where(testby => testby.active && testby.IsAlliedPlayer(playz) && (testby.Center - projectile.Center).Length() < realsize))
						{
							player.SGAPly().postLifeRegenBoost += HealingRate;
						}
					}
				}
			}

			projectile.localAI[0] += 1;

		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D mainTex = Main.projectileTexture[projectile.type];
			Effect RadialEffect = SGAmod.RadialEffect;

			float alpha = 1f;

			Vector2 half = mainTex.Size() / 2f;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("SmallLaserHorz"));
			RadialEffect.Parameters["alpha"].SetValue(0.70f * alpha);
			RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime * 0.075f, 0.20f));
			RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(1f, 2f));
			RadialEffect.Parameters["ringScale"].SetValue(0.008f * alpha);
			RadialEffect.Parameters["ringOffset"].SetValue(((projectile.timeLeft / (float)MaxSize)) * 0.9f);
			RadialEffect.Parameters["ringColor"].SetValue(Color.Lime.ToVector3());
			RadialEffect.Parameters["tunnel"].SetValue(false);

			RadialEffect.CurrentTechnique.Passes["RadialAlpha"].Apply();

			Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.Lime, 0, half, RingSize * 0.5f * MathHelper.Clamp(projectile.localAI[0] / (float)MaxTime, 0f, 1f), default, 0);

			RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("SmallLaserHorz"));
			RadialEffect.Parameters["alpha"].SetValue(1.15f * alpha);
			RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime * 0.125f, -Main.GlobalTime * 0.275f));
			RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(2f, 5f));
			RadialEffect.Parameters["ringScale"].SetValue(0.032f * alpha);

			RadialEffect.CurrentTechnique.Passes["RadialAlpha"].Apply();

			Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.Lime, 0, half, RingSize * 0.5f*MathHelper.Clamp(projectile.localAI[0]/ (float)MaxTime, 0f,1f), default, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}
	}

	public class NurseHealingAura : BungalHealingAura
	{
		int nurseRecentlyHit = 9999;
		float shrinker1 = 1f;
		int shrinker2 = 0;
		public override float RingSize
		{

			get
			{
				Player player = Main.player[projectile.owner];

				return (120);

			}
		}
		public override int MaxSize => 160;
		public override int MaxTime => 30;
		public override int HealingRate
		{

			get
			{
				Player player = Main.player[projectile.owner];
				return (int)((projectile.damage)* shrinker1);
			}

		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nurse Healing Aura");
		}

		public override void AI()
		{

			shrinker1 = MathHelper.SmoothStep(shrinker1,shrinker2 > 0 ? 0.25f : 1f,0.15f);
			shrinker2 -= 1;

			if (projectile.timeLeft > 28 && projectile.localAI[1]<600)
			{
				float realsize = (RingSize * 0.90f);
				int nursehere = NPC.FindFirstNPC(NPCID.Nurse);

				if (nursehere>=0)
				{

					NPC nurse = Main.npc[nursehere];

					if (nurse.life < nurseRecentlyHit - 20)
                    {
						shrinker2 = 150;
                    }

					Player playz = Main.player[projectile.owner];
						projectile.timeLeft = 30;
						projectile.Center = nurse.Center;			

					if (projectile.localAI[0] > MaxTime && projectile.timeLeft > 29)
					{
						foreach (Player player in Main.player.Where(testby => testby.active && testby.IsAlliedPlayer(playz) && (testby.Center - projectile.Center).Length() < realsize))
						{
							//if (projectile.localAI[0]%3==0)
							if (projectile.localAI[0]%(1+(int)((40f/(float)HealingRate)))==0 || projectile.localAI[0]%2==0)
							player.SGAPly()._healingPointsLeft += 1;
							player.SGAPly().postLifeRegenBoost += HealingRate;
						}
					}

					nurseRecentlyHit = Math.Max(nurse.life, nurseRecentlyHit);
					nurseRecentlyHit = (int)Math.Ceiling(MathHelper.SmoothStep(nurseRecentlyHit, nurse.life, 0.15f));
				}
			}

			projectile.localAI[1] += 1;
			projectile.localAI[0] += 1;
		}

        public override string Texture => "Terraria/Projectile_"+ ProjectileID.NurseSyringeHeal;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Effect RadialEffect = SGAmod.RadialEffect;

			float alpha = 1f;

			float size = MathHelper.Clamp(projectile.localAI[0] / (float)MaxTime, 0f, 1f);

			Texture2D mainTex = Main.projectileTexture[projectile.type];
			Vector2 half = mainTex.Size() / 2f;

			float mather = MathHelper.Clamp(projectile.timeLeft / (float)MaxTime, 0f, 1f);

			for (float ff = 0; ff < MathHelper.TwoPi; ff += MathHelper.TwoPi / 20f)
			{
				float ff2 = ff + Main.GlobalTime / 8f;
				Vector2 loc = (Vector2.UnitX.RotatedBy(ff2)) * size * mather * (RingSize*0.85f);

				Main.spriteBatch.Draw(mainTex, projectile.Center + loc - Main.screenPosition, null, Color.White * size * mather, ff2 + MathHelper.PiOver2, half, size* mather, default, 0);
			}

			mainTex = Main.projectileTexture[ModContent.ProjectileType<BungalHealingAura>()];
			half = mainTex.Size() / 2f;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("GlowOrb"));
			RadialEffect.Parameters["alpha"].SetValue(0.70f * alpha);
			RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime * 0.025f, 0.20f));
			RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(1f, 2f));
			RadialEffect.Parameters["ringScale"].SetValue(0.166f * alpha);
			RadialEffect.Parameters["ringOffset"].SetValue(((projectile.timeLeft / (float)MaxSize)) * 0.9f);
			RadialEffect.Parameters["ringColor"].SetValue(Color.OrangeRed.ToVector3()*0.25f);
			RadialEffect.Parameters["tunnel"].SetValue(false);

			RadialEffect.CurrentTechnique.Passes["RadialAlpha"].Apply();

			Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.White, 0, half, RingSize * 0.5f * size, default, 0);

			RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("SmallLaserHorz"));
			RadialEffect.Parameters["alpha"].SetValue(1.15f * alpha);
			RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime * 0.125f, -Main.GlobalTime * 0.675f));
			RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(2f, 8f));
			RadialEffect.Parameters["ringColor"].SetValue(Color.Red.ToVector3() * 0.5f);
			RadialEffect.Parameters["ringScale"].SetValue(0.164f * alpha);

			RadialEffect.CurrentTechnique.Passes["RadialAlpha"].Apply();

			Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.White, 0, half, RingSize * 0.5f * size, default, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}
	}



	[AutoloadEquip(EquipType.HandsOn, EquipType.HandsOff, EquipType.Back)]
	public class ArmchairGeneral : PrismalNecklace
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Armchair General");
			Tooltip.SetDefault("'Lead not by example'\nWhen standing still you get in your chair, and emit a healing fungal aura\nYour stationary sentries also emit this aura\nWhile in your chair, you gain knockback immunity, defence, and regen, but can't move\nHide accessory to disable getting into the chair\nIncreases your maximum minions by 3, and sentries by 2\n50% Increased Summon damage and increased Minion knockback\nIncreases max electrical charge by 1000 and boosts auras by 1 level\n\n" + Idglib.ColorText(Color.Red,"All other damage and crit chance is reduced by 25%"));
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);

			if (!hideVisual && player.SGAPly().bustlingFungus.Item2 == 200 && (player.mount == null || !player.mount.Active))
			{
				player.mount.SetMount(ModContent.MountType<GeneralsArmchairMount>(), player);
				//player.AddBuff(ModContent.BuffType<GeneralsArmchairBuff>(), 10);
			}

			ModContent.GetInstance<BustlingFungus>().UpdateAccessory(player, hideVisual);
			player.maxMinions += 1;
			player.minionKB += 1;
			player.dd2Accessory = true;//10%
			player.minionDamage += 0.30f;
			player.BoostAllDamage(-0.15f,-15);
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 26;
			item.defense = 0;
			item.accessory = true;
			item.height = 14;
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.expert = true;
			item.rare = ItemRarityID.Cyan;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PrismalNecklace>(), 1);
			recipe.AddIngredient(ModContent.ItemType<BustlingFungus>(), 1);
			recipe.AddRecipeGroup("SGAmod:DD2Accessories", 1);
			recipe.AddIngredient(ItemID.ShinyStone, 1);
			recipe.AddIngredient(ItemID.PapyrusScarab, 1);
			recipe.AddIngredient(ModContent.ItemType<LunarRoyalGel>(), 12);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class PrismalCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Core");
			Tooltip.SetDefault("'Core of Mettle and Magic'\n+2 Booster recharge rate, 10% increased Booster Capacity\nCombines the effects of both Novite and Novus Cores");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			ModContent.GetInstance<NoviteCore>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<NovusCore>().UpdateAccessory(player, hideVisual);
			player.SGAPly().boosterrechargerate += 2;
			player.SGAPly().boosterPowerLeftMax += (int)(10000f * 0.10f);
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 2);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 16);
			recipe.AddIngredient(mod.ItemType("NoviteCore"), 1);
			recipe.AddIngredient(mod.ItemType("NovusCore"), 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
	public class GlacialStone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glacial Stone");
			Tooltip.SetDefault("Melee attacks inflict Frostburn\nYour melee projectiles become Cold Damage");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().glacialStone = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 3);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 4);
			recipe.AddIngredient(ItemID.MagmaStone, 1);
			recipe.AddIngredient(ItemID.FrostCore, 1);
			recipe.AddTile(ItemID.CrystalBall);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();*/
		}
	}

	public class Refractor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Refractor");
			Tooltip.SetDefault("Reflect damage taken back at nearby enemies as beams of light\nDoes damage taken before damage scaling, times 3, times thorns level\nDamage scales with thorns effect\nTake 10% (multiplicative) of 25% of the damage less per enemies affected\nRaises thorns by 2X");
		}

        public override bool Autoload(ref string name)
        {
            SGAPlayer.FirstHurtEvent += SGAPlayer_PreHurtEvent;
			//SGAPlayer.AfterTheHitEvent += SGAPlayer_AfterTheHitEvent;
			return true;
        }
		private void DoMoreDamageAndTakeLess(SGAPlayer player, PlayerDeathReason damageSource, ref int damage, ref int hitDirection, bool pvp, bool quiet, ref bool Crit, int cooldownCounter)
		{
			if (player != null && damageSource != null && player.refractor && (cooldownCounter < 0 || (cooldownCounter >= 0 && player.player.hurtCooldowns[cooldownCounter] <= 0)))
			{
				Player ply = player.player;
				int damageTaken = (int)(damage * 0.75);
				damage -= damageTaken;
				int damageToDo = damage * 3;
				foreach (NPC npc in Main.npc.Where(testby => testby.active && !testby.friendly && !testby.dontTakeDamage && (testby.Center - ply.Center).Length() < 640))
				{
					Projectile proj = Projectile.NewProjectileDirect(ply.Center, Vector2.Zero, ModContent.ProjectileType<RefractorLaserProj>(), (int)(damageToDo * ply.thorns), 10, ply.whoAmI, Main.rand.NextFloat(1f), npc.whoAmI);
					proj.penetrate = -1;
					proj.netUpdate = true;
					damage = (int)(damage * 0.90f);
				}
				damage += damageTaken;
			}
		}

		private void SGAPlayer_PreHurtEvent(SGAPlayer player, PlayerDeathReason damageSource, ref int damage, ref int hitDirection, bool pvp, bool quiet, ref bool Crit, int cooldownCounter)
		{
			DoMoreDamageAndTakeLess(player, damageSource, ref damage, ref hitDirection, pvp, quiet, ref Crit, cooldownCounter);
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.thorns += 2f;
			player.SGAPly().refractor = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 2);
			item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HeliosFocusCrystal>(), 2);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 20);
			recipe.AddIngredient(ItemID.LightShard, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class RefractorLaserProj : NoteLaserProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Refractor Laser Proj");
		}
	}

		[AutoloadEquip(EquipType.Back)]
	public class NormalQuiver : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Normal Quiver");
			Tooltip.SetDefault("'What? Expecting a MAGIC one?'\n10% increased Arrow Damage");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.arrowDamage += 0.10f;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(gold: 2);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}
	}

	[AutoloadEquip(EquipType.Neck)]
	public class MurkyCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Murky Charm");
			Tooltip.SetDefault("Reduces the life lost from drowning\nGrants immunity to Murky Depths and Swamp Rot");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.SGAPly();
			if (!sgaply.murkyCharm)
			{
				sgaply.murkyCharm = true;
				sgaply.drownRate -= 1;
				player.buffImmune[ModContent.BuffType<NPCs.Murk.MurkyDepths>()] = true;
				player.buffImmune[ModContent.BuffType<NPCs.Murk.PoisonStack>()] = true;
			}
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.sellPrice(silver: 25);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
		}

	}
	[AutoloadEquip(EquipType.Shoes)]
	public class MagusSlippers : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magus Slippers");
			Tooltip.SetDefault("Removes the movement penality from mana regeneration\n'So magical cozy! No wonder mages can focus!'");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.SGAPly();
			sgaply.magusSlippers = true;
			//placeholder
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(gold: 2);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
		}
	}

	[AutoloadEquip(EquipType.Shoes)]
	public class DruidicSneakers : MagusSlippers
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Druidic Sneakers");
			Tooltip.SetDefault("Mana herbs sometimes grow on grass where you walk\nWorks on Grass, Jungle Grass, and Hallowed Grass\nHarvesting these plants yields Mana Stars\nEffects of Magus Slippers\n'Eco Friendly!'");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.SGAPly();
			base.UpdateAccessory(player, hideVisual);
			//Just a bit chunk of vanilla code, urgh
			if (player.velocity.Y == 0f && player.grappling[0] == -1)
			{
				int num2 = (int)player.Center.X / 16;
				int num3 = (int)(player.position.Y + (float)player.height - 1f) / 16;
				if (Main.tile[num2, num3] == null)
				{
					Main.tile[num2, num3] = new Tile();
				}
				if (!Main.tile[num2, num3].active() && Main.tile[num2, num3].liquid == 0 && Main.tile[num2, num3 + 1] != null && WorldGen.SolidTile(num2, num3 + 1))
				{
					Main.tile[num2, num3].frameY = 0;
					Main.tile[num2, num3].slope(0);
					Main.tile[num2, num3].halfBrick(halfBrick: false);
					if (Main.tile[num2, num3 + 1].type == TileID.Grass || Main.tile[num2, num3 + 1].type == TileID.JungleGrass || Main.tile[num2, num3 + 1].type == TileID.HallowedGrass)
					{
						if (Main.rand.Next(2) == 0)
						{
							Main.tile[num2, num3].active(active: true);
							Main.tile[num2, num3].type = 3;
							Main.tile[num2, num3].frameX = (short)(18 * Main.rand.Next(6, 11));
							while (Main.tile[num2, num3].frameX == 144)
							{
								Main.tile[num2, num3].frameX = (short)(18 * Main.rand.Next(6, 11));
							}
						}
						else if (Main.rand.Next(6) == 0)
						{
							Main.tile[num2, num3].active(active: true);
							Main.tile[num2, num3].type = (ushort)mod.TileType("ManaHerb");
							Main.tile[num2, num3].frameX = (short)(18 * Main.rand.Next(9, 14));
						}
						else
						{
							Main.tile[num2, num3].active(active: true);
							Main.tile[num2, num3].type = 73;
							Main.tile[num2, num3].frameX = (short)(18 * Main.rand.Next(6, 21));
							while (Main.tile[num2, num3].frameX == 144)
							{
								Main.tile[num2, num3].frameX = (short)(18 * Main.rand.Next(6, 21));
							}
						}
						if (Main.netMode == 1)
						{
							NetMessage.SendTileSquare(-1, num2, num3, 1);
						}
					}
					/*else if (Main.tile[num2, num3 + 1].type == 109)
					{
						if (Main.rand.Next(2) == 0)
						{
							Main.tile[num2, num3].active(active: true);
							Main.tile[num2, num3].type = 110;
							Main.tile[num2, num3].frameX = (short)(18 * Main.rand.Next(4, 7));
							while (Main.tile[num2, num3].frameX == 90)
							{
								Main.tile[num2, num3].frameX = (short)(18 * Main.rand.Next(4, 7));
							}
						}
						else
						{
							Main.tile[num2, num3].active(active: true);
							Main.tile[num2, num3].type = 113;
							Main.tile[num2, num3].frameX = (short)(18 * Main.rand.Next(2, 8));
							while (Main.tile[num2, num3].frameX == 90)
							{
								Main.tile[num2, num3].frameX = (short)(18 * Main.rand.Next(2, 8));
							}
						}
						if (Main.netMode == 1)
						{
							NetMessage.SendTileSquare(-1, num2, num3, 1);
						}
					}
					else if (Main.tile[num2, num3 + 1].type == 60)
					{
						Main.tile[num2, num3].active(active: true);
						Main.tile[num2, num3].type = 74;
						Main.tile[num2, num3].frameX = (short)(18 * Main.rand.Next(9, 17));
						if (Main.netMode == 1)
						{
							NetMessage.SendTileSquare(-1, num2, num3, 1);
						}
					}*/
				}

				//placeholder
			}
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(gold: 2);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FlowerBoots, 1);
			recipe.AddIngredient(ModContent.ItemType<MagusSlippers>(), 1);
			recipe.AddIngredient(ItemID.ManaCrystal, 1);
			recipe.AddIngredient(ItemID.LightShard, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class EnchantedShieldPolish : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Shield Polish");
			Tooltip.SetDefault("Scoring a Just Block with a shield restores some mana\nThe amount of damage shields reduce is increased by 5%\nYour shields gain 10% more damage and Bashes gain additional Magic damage");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.SGAPly();
			sgaply.shieldDamageReduce += 0.05f;
			sgaply.shieldDamageBoost += 0.10f;

			sgaply.enchantedShieldPolish = true;
			//placeholder
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(gold: 2);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}

	}

	public class StarCollector : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Collector");
			Tooltip.SetDefault("Picking up Mana stars reduces Mana Sickness by 1 second\nFurthermore, if your mana is full, heal 5 HP");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.SGAPly();
			sgaply.starCollector = true;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(silver: 30);
			item.rare = ItemRarityID.Orange;
			item.accessory = true;
		}

    }

	[AutoloadEquip(EquipType.Back)]
	public class MagicianGear : DruidicSneakers
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magician's Gear");
			Tooltip.SetDefault("'For when you want to take the show on the road!'\n+20% magic damage and 5% crit chance if not Mana Sick\n"+Language.GetTextValue("ItemTooltip.CelestialMagnet")+"\nGrants the effects of:\n-Star Collector\n-Enchanted Shield Polish\n-Druidic Sneakers");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			SGAPlayer sgaply = player.SGAPly();
			sgaply.starCollector = true;
			ModContent.GetInstance<EnchantedShieldPolish>().UpdateAccessory(player, hideVisual);
			player.manaMagnet = true;
			if (!player.manaSick)
			{
				player.magicDamage += 0.20f;
				player.magicCrit += 5;
			}
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = Item.buyPrice(gold: 3);
			item.rare = ItemRarityID.Red;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DruidicSneakers>(), 1);
			recipe.AddIngredient(ModContent.ItemType<StarCollector>(), 1);
			recipe.AddIngredient(ModContent.ItemType<EnchantedShieldPolish>(), 1);
			recipe.AddIngredient(ItemID.CelestialEmblem, 1);
			recipe.AddIngredient(ItemID.Bunny, 1);
			recipe.AddIngredient(ItemID.TaxCollectorsStickOfDoom, 1);
			recipe.AddIngredient(ItemID.MagicHat, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Face)]
	public class TerraDivingGear : ModItem
	{
		protected string allText => Language.GetTextValue("ItemTooltip.ArcticDivingGear") + "\n" + Language.GetTextValue("ItemName.BreathingReed") + " " + Language.GetTextValue("ItemTooltip.BreathingReed") + "\n" +
			((GetType() == typeof(PrismalDivingGear)) ? Language.GetTextValue("ItemTooltip.FlipperPotion")+"\n" : "") + "Hold DOWN to fall faster in liquids\nReels in air bubbles from further away"+(GetType() == typeof(TerraDivingGear) ? "\n'Sometimes known as Dergon Diving Gear'" : "");
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Breathing Gear");
			Tooltip.SetDefault(allText);
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Lime;
			item.value = Item.buyPrice(0, 2, 50, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.arcticDivingGear = true;
			player.accFlipper = true;
			player.accDivingHelm = true;
			player.iceSkate = true;
			player.SGAPly().terraDivingGear = true;
			if (player.controlDown)
				ModContent.GetInstance<PocketRocks>().UpdateAccessory(player, hideVisual);

			if (player.wet)
			{
				if (GetType() == typeof(PrismalDivingGear))
					Lighting.AddLight((int)player.Center.X / 16, (int)player.Center.Y / 16, 0.75f, 0.2f, 0.95f);
				else
				Lighting.AddLight((int)player.Center.X / 16, (int)player.Center.Y / 16, 0.27f, 0.85f, 0.53f);
			}

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ArcticDivingGear, 1);
			recipe.AddIngredient(ModContent.ItemType<PocketRocks>(), 1);
			recipe.AddIngredient(ItemID.BreathingReed, 1);
			recipe.AddIngredient(ItemID.SoulofLight, 4);
			recipe.AddIngredient(ItemID.SoulofNight, 4);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
	public class BottledLiquidEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Liquid Essence");
			Tooltip.SetDefault("Improved Life and Mana regen while wet");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.wet)
			{
				player.manaRegenBonus += 30;
				player.lifeRegen += 1;
			}
			//ModContent.GetInstance<BlinkTech>().UpdateAccessory(player, hideVisual);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("BottledMud"), 50);
			recipe.needLava = true;
			recipe.needWater = true;
			recipe.needHoney = true;
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class NoviteAirTank : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Air Tank");
			Tooltip.SetDefault("+5 max Breath Bubbles\nGrants defense per missing breath, becomes stronger with better breathing gear");
		}

		public override bool Autoload(ref string name)
		{
			if (GetType() == typeof(NoviteAirTank))
			{
                SGAPlayer.PostPostUpdateEquipsEvent += SGAPlayer_PostUpdateEquipsEvent;

			}
			return true;
		}

        private void SGAPlayer_PostUpdateEquipsEvent(SGAPlayer sgaply)
        {
            if (sgaply.airTank)
            {

				float scaler = 1f + (sgaply.player.arcticDivingGear ? 0.5f : 0f) + (sgaply.terraDivingGear ? 0.75f : 0) + (sgaply.prismalDivingGear ? 0.75f : 0);
				int defensegiven = (int)((sgaply.player.breathMax/20)*scaler);

				sgaply.player.statDefense += Math.Max((int)((1f - ((float)sgaply.player.breath / (float)sgaply.player.breathMax)) * defensegiven), 0);

			}
        }

        public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.SGAPly().airTank)
				return;

			player.SGAPly().airTank = true;
			player.breathMax += 100;
			player.ignoreWater = true;
			//ModContent.GetInstance<BlinkTech>().UpdateAccessory(player, hideVisual);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NoviteBar>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Back)]
	public class PrismalAirTank : NoviteAirTank
	{
		internal static sbyte backItem = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Air Tank");
			Tooltip.SetDefault("+5 max Breath Bubbles, "+ Language.GetTextValue("ItemTooltip.FlipperPotion")+ "\nGrants more defense per missing breat, becomes stronger with better breathing gear\nImproved Life and Mana regen while wet\nGrants an additional free Action Cooldown Stack while wet");
			backItem = item.backSlot;
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 24;
			item.rare = ItemRarityID.Lime;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			if (player.wet)
			{
				player.SGAPly().MaxCooldownStacks += 1;
				ModContent.GetInstance<BottledLiquidEssence>().UpdateAccessory(player, hideVisual);
			}

			player.ignoreWater = true;

			//ModContent.GetInstance<BlinkTech>().UpdateAccessory(player, hideVisual);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PrismalBar>(), 8);
			recipe.AddIngredient(ModContent.ItemType<NoviteAirTank>(), 1);
			recipe.AddIngredient(ModContent.ItemType<BottledLiquidEssence>(), 1);
			recipe.AddIngredient(ItemID.FlipperPotion, 5);
			recipe.AddTile(mod.GetTile("PrismalStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Face)]
	public class PrismalDivingGear : TerraDivingGear
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Breathing Gear");
			Tooltip.SetDefault(allText + "\nEffects of Terra Diving Gear, Murky Charm, and Prismal Air Tank");
		}

		public override bool DrawHead()
		{
			return false;
		}

		public override void SetDefaults()
		{
			item.backSlot = PrismalAirTank.backItem;
			item.width = 18;
			item.height = 24;
			item.rare = ItemRarityID.Yellow;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().prismalDivingGear = true;
			base.UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<PrismalAirTank>().UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<MurkyCharm>().UpdateAccessory(player, hideVisual);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TerraDivingGear>(), 1);
			recipe.AddIngredient(ModContent.ItemType<PrismalAirTank>(), 1);
			recipe.AddIngredient(ModContent.ItemType<MurkyCharm>(), 1);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 30);
			recipe.AddIngredient(ModContent.ItemType<PrismalBar>(), 12);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class WraithTargetingGamepad : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wraith Targeting Gamepad");
			Tooltip.SetDefault("Enables Terraria Gamepad auto-aiming with keyboard controls while worn");
		}

		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.ManaFlower);
			item.width = 24;
			item.height = 24;
			item.rare += 1;
			item.value = Item.sellPrice(0, 0, 25, 0);
			item.accessory = true;
			item.expert = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string s = "Not Binded!";
			foreach (string key in SGAmod.ToggleGamepadKey.GetAssignedKeys())
			{
				s = key;
			}

			tooltips.Add(new TooltipLine(mod, "uncraft", Idglib.ColorText(Color.CornflowerBlue, "Press the 'Toggle Aiming Style' (" + s + ") Hotkey to toggle modes")));
		}

		public override string Texture => "Terraria/UI/HotbarRadial_2";

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.gamePadAutoAim = 2;
			//Terraria.GameContent.Events.DD2Event.LaneSpawnRate = 9;
		}

	}
	[AutoloadEquip(EquipType.Shield)]
	public class RustedBulwark : ModItem,IRustBurnText
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rusted Bulwark");
			Tooltip.SetDefault("Halves Knockback taken\nwhen below half health, grants:\n+1 defense and 4% increased blocking damage with shields\nScoring a Just Block will Rustburn the attacking enemy\n'Has seen better days...'");
		}

		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.ManaFlower);
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.White;
			item.value = Item.buyPrice(0, 0, 20, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (player.statLife < player.statLifeMax2 / 2)
			{
				player.statDefense += 1;
				sgaplayer.shieldDamageReduce += 0.04f;
			}
			sgaplayer.knockbackTaken *= 0.5f;
			sgaplayer.rustedBulwark = true;
		}

	}
	public class Jabbawacky : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jabb-a-wacky");
			Tooltip.SetDefault("'Skeleton Merchant's personal hook; puts a little el-bone grease into your jabs'\nIncreases Jab-lin jabbing speed by 20% and grants autofire\n"+ Idglib.ColorText(Color.Red, "Your precision gets a little off thou..."));
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.Hook); }
		}

		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.ManaFlower);
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Green;
			item.value = Item.buyPrice(0, 1, 50, 0);
			item.accessory = true;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTime / 4f) % 1f, 0.25f, 0.50f);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.jabALot = true;
		}

	}
	public class TPDCPU : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("T.P.D.C.P.U");
			Tooltip.SetDefault("'Twin Prime Destroyers Coolent Processing Unit'\nReduces all new Cooldown Stacks by 25%\nBarrier recharge delay is reduced by 2 seconds\nGrants Immunity to Shield Break\nRemoves the negative effects of most Enchanted Amulets\nAmulet of Diehard Cataclysm is not affected");
		}

		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.ManaFlower);
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Yellow;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.accessory = true;
			item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.tpdcpu = true;
			sgaplayer.actionCooldownRate -= 0.25f;
			//Terraria.GameContent.Events.DD2Event.LaneSpawnRate = 9;
		}

	}

	public class CobaltHorseshoe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Horseshoe");
			Tooltip.SetDefault(Language.GetTextValue("ItemTooltip.CobaltShield")+"\n"+ Language.GetTextValue("ItemTooltip.ObsidianHorseshoe"));
		}

		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.ManaFlower);
			item.width = 24;
			item.height = 24;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.fireWalk = true;
			player.noKnockback = true;
			player.noFallDmg = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CobaltShield, 1);
			recipe.AddIngredient(ItemID.ObsidianHorseshoe, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class AversionCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aversion Charm");
			Tooltip.SetDefault("Allows you to Ninja Dodge traps\n"+Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 30 seconds each"));
		}

		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.ManaFlower);
			item.width = 24;
			item.height = 24;
			item.rare = 2;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.aversionCharm = true;
		}

	}

	[AutoloadEquip(EquipType.Waist)]
	public class PocketRocks : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pocket Rocks");
			Tooltip.SetDefault("'Rocks in your pockets!'\nMakes you fall faster in liquids when worn");
		}

		public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.ManaFlower);
			item.width = 12;
			item.height = 24;
			item.rare = 1;
			item.value = Item.buyPrice(0, 0, 50, 0);
			item.accessory = true;

			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.damage = 5;
			item.shootSpeed = 10f;
			item.shoot = ModContent.ProjectileType<Weapons.RockProj>();
			item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			item.width = 12;
			item.height = 24;
			item.knockBack = 2;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 8;
			item.useTime = 8;
			item.noUseGraphic = true;
			item.noMelee = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.wet)
			{
				player.maxFallSpeed += 10f;
				if (player.velocity.Y > 0)
					player.velocity.Y += 0.2f;
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].Throwing().thrown = true;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			Main.projectile[probg].netUpdate = true;
			IdgProjectile.Sync(probg);
			return false;
		}

	}

	public class FluidDisplacer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fluid Displacer");
			Tooltip.SetDefault("Displaces fluids around you, allowing you move through them freely\nConsumes Electric Charge to prevent the player from submerging\nConsumes far more to prevent lava submerging\nTriggers a Shield Break when Electric Charge runs out\n"+Idglib.ColorText(Color.Red,"Removing this accessory during Shield Break will cause great damage!"));
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.value = 50000;
			item.maxStack = 1;
			item.accessory = true;
			item.rare = ItemRarityID.Orange;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().tidalCharm = -10;
			player.SGAPly().ShieldTypeDelay = 5;
			player.SGAPly().ShieldType = 1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 8);
			recipe.AddIngredient(mod.ItemType("LaserMarker"), 4);
			recipe.AddIngredient(ItemID.WireBulb, 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D inner = mod.GetTexture("Items/GlowMasks/Floudglow");

			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);
			spriteBatch.Draw(Main.itemTexture[item.type], drawPos, null, drawColor, 0, Main.itemTexture[item.type].Size() / 2f, Main.inventoryScale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			spriteBatch.Draw(inner, drawPos, null, Color.White, 0, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, default, default, default, default, null, Main.UIScaleMatrix);

			return false;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{

			Vector2 position = item.Center - Main.screenPosition;
			spriteBatch.Draw(Main.itemTexture[item.type], position, null, alphaColor, rotation, Main.itemTexture[item.type].Size() / 2f, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			Texture2D inner = mod.GetTexture("Items/GlowMasks/Floudglow");


			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);

			spriteBatch.Draw(inner, position, null, Color.White, rotation, textureOrigin, scale*Main.essScale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.ZoomMatrix);

			return false;
		}

	}

	public class GraniteMagnet : Weapons.Shields.CorrodedShield, IHitScanItem
	{
		public override string ShowPercentText => "none";
		public override bool CanBlock => false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Magnet");
			Tooltip.SetDefault("Point at grounded items to attract them from a larger distance\nCan be held out like a torch and used normally by holding shift\nCan be worn to provide a minor increase to item grab radius and grab speed");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 12;
			item.noUseGraphic = true;
			item.value = Item.buyPrice(0, 0, 75, 0);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 1;
			item.accessory = true;
			item.useTurn = false;
			item.autoReuse = false;
			item.noMelee = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().graniteMagnet = true;
		}

	}

	public class CobaltMagnet : GraniteMagnet, IHitScanItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Magnet");
			Tooltip.SetDefault("Point at grounded items to quickly attract them from a larger distance\nCan be held out like a torch and used normally by holding shift\nwhile worn:\n-minor increase to item grab radius and grab speed\n-Effects of Celestial Magnet");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 12;
			item.noUseGraphic = true;
			item.value = Item.buyPrice(0, 2, 5, 0);
			item.rare = ItemRarityID.Pink;
			item.maxStack = 1;
			item.accessory = true;
			item.useTurn = false;
			item.autoReuse = false;
			item.noMelee = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.SGAPly().graniteMagnet = true;
			player.manaMagnet = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CelestialMagnet, 1);
			recipe.AddIngredient(ModContent.ItemType<GraniteMagnet>(), 1);
			recipe.AddIngredient(ItemID.CobaltBar, 5);
			recipe.AddIngredient(ItemID.MeteoriteBar, 8);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}


	}

	public class CobaltMagnetProj : GraniteMagnetProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CobaltMagnetProj");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Accessories/CobaltMagnet"); }
		}

	}

	public class GraniteMagnetProj : Weapons.Technical.LaserMarkerProj
    {
		protected override float VisualAngle => -MathHelper.PiOver2;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("GraniteMagnetProj");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.light = 0.0f;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Accessories/GraniteMagnet"); }
		}

		public override void AI()
		{

			Player player = Main.player[projectile.owner];
			bool heldone = player.HeldItem.type != mod.ItemType(ItemName);
			if (projectile.ai[0] > 0 || (player.HeldItem == null || heldone) || player.itemAnimation > 0 || player.dead)
			{
				projectile.Kill();
				return;
			}
			else
			{
				if (projectile.timeLeft < 3)
					projectile.timeLeft = 3;
				Vector2 mousePos = Main.MouseWorld;

				if (projectile.owner == Main.myPlayer)
				{
					Vector2 diff = mousePos - player.Center;
					projectile.velocity = diff;
					projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
					projectile.netUpdate = true;
					projectile.Center = mousePos;
				}
				int dir = projectile.direction;
				player.ChangeDir(dir);

				Vector2 direction = (projectile.velocity);
				Vector2 directionmeasure = direction;

				player.heldProj = projectile.whoAmI;

				projectile.velocity.Normalize();
				projectile.rotation = projectile.velocity.ToRotation()-(MathHelper.Pi/2f)* dir;

				player.bodyFrame.Y = player.bodyFrame.Height * 3;
				if (directionmeasure.Y - Math.Abs(directionmeasure.X) > 25)
					player.bodyFrame.Y = player.bodyFrame.Height * 4;
				if (directionmeasure.Y + Math.Abs(directionmeasure.X) < -25)
					player.bodyFrame.Y = player.bodyFrame.Height * 2;
				if (directionmeasure.Y + Math.Abs(directionmeasure.X) < -160)
					player.bodyFrame.Y = player.bodyFrame.Height * 5;
				player.direction = (directionmeasure.X > 0).ToDirectionInt();

				projectile.Center = player.Center + (projectile.velocity * 10f);
				projectile.velocity *= 8f;

				Vector2 magnetHere = projectile.Center + Vector2.Normalize(projectile.velocity) * 48f;

				foreach (Item item in Main.item)
				{

					Vector2 vectorItemToPlayer = magnetHere - item.Center;
					Vector2 vectorItemToPlayer2 = projectile.Center - item.Center;
					if (vectorItemToPlayer2.Length() < 600 && Vector2.Dot(Vector2.Normalize(vectorItemToPlayer2), Vector2.Normalize(projectile.velocity)) < -0.9f && Collision.CanHitLine(magnetHere, 1, 1, item.Center, 1, 1))
					{
						Vector2 movement = vectorItemToPlayer.SafeNormalize(default(Vector2)) * (GetType() == typeof(CobaltMagnetProj) ? 1f : 0.45f);
						item.velocity = item.velocity + movement;
						item.velocity = Collision.TileCollision(item.position, item.velocity, item.width, item.height,true);
					}
				}


			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			base.PreDraw(spriteBatch, drawColor);

			/*
			bool facingleft = projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.None;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), drawColor * projectile.Opacity, projectile.rotation + (facingleft ? 0 : MathHelper.Pi), origin, projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);
			*/

			return false;
		}

	}

}