using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Idglibrary;
using SGAmod.Items.Placeable;
using SGAmod.Items.Weapons.Vibranium;
using SGAmod.Items.Accessories;
using Terraria.Utilities;
using SGAmod.Items.Placeable.DankWoodFurniture;
using Terraria.Audio;

namespace SGAmod.HavocGear.Items
{
	public class MoistSand : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.MoistSand>();
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moist Sand");
			Tooltip.SetDefault("'expect nothing else from sand thrown into water'");
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<MoistSand>()).AddTile(TileID.Furnaces).ReplaceResult(ItemID.SandBlock);
			CreateRecipe(1).AddIngredient(ItemID.SandBlock).Register();
		}

	}
	public class BottledMud : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Mud");
			Tooltip.SetDefault("'brown and full of sedimental value'");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 14;
			Item.maxStack = 99;
			Item.value = 50;
			Item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bottle).AddRecipeGroup("SGAmod:Mud", 3).Register();
		}

	}
	public class VirulentBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virulent Bar");
			Tooltip.SetDefault("Condensed life essence in bar form");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 14;
			Item.maxStack = 99;
			Item.value = 1000;
			Item.rare = 5;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("VirulentBarTile").Type;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<BiomassBar>(), 1).AddIngredient(ModContent.ItemType<VirulentOre>(), 3).AddTile(TileID.Hellforge).Register();
		}

	}
	public class VirulentOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virulent Ore");
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 99;
			Item.value = 100;
			Item.rare = ItemRarityID.Pink;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("VirulentOre").Type;
		}

	}
	public class BiomassBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Photosyte Bar");
			Tooltip.SetDefault("A hardened bar made from parasitic biomass reacting from murky gel and moss");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 14;
			Item.maxStack = 99;
			Item.value = 100;
			Item.rare = ItemRarityID.Green;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("BiomassBarTile").Type;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/BiomassBar"); }
		}

		public override void AddRecipes()
		{
			CreateRecipe(3).AddIngredient(ModContent.ItemType < Biomass>(), 5).AddIngredient(ModContent.ItemType < MurkyGel>(),2).AddIngredient(ModContent.ItemType<DecayedMoss>(), 1).AddIngredient(ModContent.ItemType<Weapons.SwampSeeds>(), 2).AddIngredient(ModContent.ItemType<MoistSand>(), 1).AddTile(TileID.Furnaces).Register();
		}
	}
	public class Biomass : ModItem
	{
		public override void SetDefaults()
		{

			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.rare = ItemRarityID.Green;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("Biomass").Type;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Biomass"); }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Photosyte");
			Tooltip.SetDefault("'Parasitic plant matter'\nIs found largely infesting clouds where it can gain the most sunlight");
		}

	}
	public class DankWood : ModItem
	{
		public override void SetDefaults()
		{
			Item.value = 50;
			Item.rare = ItemRarityID.Blue;
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodBlock>();
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dank Wood");
			Tooltip.SetDefault("It smells odd...");
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWoodFence>(), 4).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<BrokenDankWoodFence>(), 4).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWoodWall>(), 4).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWoodPlatform>(), 2).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SwampWoodWall>(), 4).Register();
		}
	}
	public class DankCore : ModItem
	{
		public override void SetDefaults()
		{
			Item.value = 2500;
			Item.rare = 2;
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dank Core");
			Tooltip.SetDefault("'Dark, Dank, Dangerous...'");
		}

	}

	public class MurkyGel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Murky Gel");
			Tooltip.SetDefault("Extra sticky, stinky too");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/MurkyGel"); }
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 14;
			Item.height = 14;
			Item.value = 50;
			Item.rare = 3;
		}
	}
	public class FieryShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Shard");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.maxStack = 99;
			Item.value = 25;
			Item.rare = 3;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
			Item.alpha = 30;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Orange.ToVector3() * 0.55f * Main.essScale);
		}
	}
}

namespace SGAmod.Items
{

	public class Glowrock : ModItem, IRadioactiveItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowrock");
			Tooltip.SetDefault("These rocks seem to give the Asteriods a glow; Curious.\nExtract it via an Extractinator for some goodies!\nDoesn't have much other use, outside of illegal interests");
			ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
		}

        public override void ExtractinatorUse(ref int resultType, ref int resultStack)
        {
			if (Main.rand.Next(8) < 4)
				return;

			WeightedRandom<(int, int)> WR = new WeightedRandom<(int, int)>();
			
			if (NPC.downedPlantBoss)
			{
				WR.Add((ItemID.Ectoplasm, Main.rand.Next(1, 1)), 1);
			}

			if (NPC.downedMoonlord)
				WR.Add((ItemID.LunarOre, Main.rand.Next(1, 3)), 1);

			WR.Add((ItemID.SoulofLight, 1), 1);
			WR.Add((ItemID.SoulofNight, 1), 1);
			WR.Add((ItemID.DarkBlueSolution, Main.rand.Next(1, 9)),0.50);
			WR.Add((ItemID.BlueSolution, Main.rand.Next(1, 9)), 0.50);

			WR.needsRefresh = true;
			(int, int) thing = WR.Get();
			resultType = thing.Item1;
			resultStack = thing.Item2;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Blue.ToVector3() * 0.55f);
		}

        public int RadioactiveHeld()
        {
			return 2;
        }

        public int RadioactiveInventory()
        {
			return 1;
        }
    }

	public class CelestineChunk : ModItem, IRadioactiveItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestine Chunk");
			Tooltip.SetDefault("Inert and radioactive Luminite...");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
		}

		public override string Texture => "Terraria/Item_" + ItemID.LunarOre;

        public override Color? GetAlpha(Color lightColor)
        {
			return Color.Lerp(Color.DarkGray, Color.Gray, 0.50f + (float)Math.Sin(Main.GlobalTimeWrappedHourly / 2f) / 2f);
        }

        public override void AddRecipes()
        {
			CreateRecipe(2).AddIngredient(ItemID.LunarOre, 1).AddIngredient(this, 4).AddIngredient(ModContent.ItemType<IlluminantEssence>(), 1).AddTile(TileID.LunarCraftingStation).ReplaceResult(ItemID.LunarBar);
		}

        public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.White.ToVector3() * 0.55f);
		}

		public int RadioactiveHeld()
		{
			return 2;
		}

		public int RadioactiveInventory()
		{
			return 1;
		}
	}

	public class OverseenCrystal : ModItem, IRadioactiveItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Overseen Crystal");
			Tooltip.SetDefault("Celestial Shards manifested from Phaethon's creators; resonates with charged forgotten spirits\nMay be used to fuse several strong materials together with ease\nSurely a shady dealer will also be interested in trading for these...");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 1000;
			Item.rare = ItemRarityID.Blue;
		}

		public int RadioactiveHeld()
		{
			return 3;
		}

		public int RadioactiveInventory()
		{
			return 2;
		}

		public override void AddRecipes()
		{

			CreateRecipe(8).AddIngredient(ModContent.ItemType<UnmanedOre>(), 2).AddIngredient(ModContent.ItemType<NoviteOre>(), 2).AddIngredient(this, 4).AddTile(tileType).ReplaceResult(ModContent.GetInstance<PrismalOre>());

			CreateRecipe(2).AddIngredient(ModContent.ItemType<AncientFabricItem>(), 5).AddIngredient(ModContent.ItemType<AdvancedPlating>(), 2).AddIngredient(this, 2).AddTile(tileType).ReplaceResult(ModContent.GetInstance<VibraniumPlating>());

			CreateRecipe(2).AddIngredient(ItemID.SoulofLight, 1).AddIngredient(ItemID.SoulofNight, 1).AddIngredient(this, 2).AddTile(tileType).ReplaceResult(ModContent.GetInstance<OmniSoul>());

			CreateRecipe(1).AddIngredient(ItemID.FossilOre, 2).AddIngredient(this, 1).AddTile(tileType).ReplaceResult(ItemID.DefenderMedal);

			CreateRecipe(1).AddIngredient(ItemID.HallowedBar, 4).AddIngredient(ItemID.SoulofFright, 1).AddIngredient(ItemID.SoulofMight, 1).AddIngredient(ItemID.SoulofSight, 1).AddIngredient(this, 5).AddTile(tileType).ReplaceResult(ModContent.GetInstance<Consumables.DivineShower>());

			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:VanillaAccessory", 2);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 4);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(ModContent.GetInstance<StarCollector>());
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:VanillaAccessory", 1);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 3);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(ModContent.GetInstance<RustedBulwark>());
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:VanillaAccessory", 1);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 2);
			recipe.AddIngredient(ItemID.ArmorPolish, 1);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(ModContent.GetInstance<EnchantedShieldPolish>());
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:VanillaAccessory", 1);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 3);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(ModContent.GetInstance<MurkyCharm>());
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:VanillaAccessory", 2);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 4);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(ModContent.GetInstance<MagusSlippers>());
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:VanillaAccessory", 2);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 4);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(ModContent.GetInstance<MagusSlippers>());
			recipe.AddRecipe();*/

		}

	}
	public class VibraniumCrystal : ModItem,IRadioactiveItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Crystal");
			Tooltip.SetDefault("'Makes a humming sound while almost shaking out your hands'");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 500;
			Item.rare = ItemRarityID.Red;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("VibraniumCrystalTile").Type;
		}
        public override bool Autoload(ref string name)
        {
            return SGAmod.VibraniumUpdate;
        }

		public int RadioactiveHeld()
		{
			return 3;
		}

		public int RadioactiveInventory()
		{
			return 3;
		}

	}
	public class VibraniumPlating : VibraniumCrystal
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Plating");
			Tooltip.SetDefault("'Dark cold steel; it constantly vibrates to the touch'");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 400;
			Item.rare = ItemRarityID.Purple;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.VibraniumPlatingTile>();
		}
	}
	public class VibraniumBar : VibraniumText
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Bar");
			Tooltip.SetDefault("'This alloy is just barely stable enough to not phase out of existance'");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 2500;
			Item.rare = ItemRarityID.Purple;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("VibraniumBarTile").Type;
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(mod.ItemType("VibraniumCrystal"), 3).AddIngredient(mod.ItemType("VibraniumPlating"), 3).AddIngredient(ItemID.LunarBar, 2).AddIngredient(mod.ItemType("LunarRoyalGel"), 1).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}
		public override bool Autoload(ref string name)
		{
			return SGAmod.VibraniumUpdate;
		}
	}

	public class IceFairyDust : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Fairy Dust");
			Tooltip.SetDefault("It doesn't feel like it's from this universe");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 26;
			Item.height = 14;
			Item.value = 75;
			Item.rare = 5;
		}
	}
	public class FrigidShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Shard");
			Tooltip.SetDefault("Raw essence of ice");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 26;
			Item.height = 14;
			Item.value = 0;
			Item.rare = 1;
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Aqua.ToVector3() * 0.25f);
		}
	}	
	public class Fridgeflame : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fridgeflame");
			Tooltip.SetDefault("Alloy of hot and cold essences");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
			Item.maxStack = 99;
			Item.value = 200;
			Item.rare = 6;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.White.ToVector3() * 0.65f * Main.essScale);
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(mod.ItemType("FrigidShard"), 1).AddIngredient(mod.ItemType("FieryShard"), 1).AddTile(TileID.CrystalBall).Register();
		}
	}
	public class VialofAcid : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vial of Acid");
			Tooltip.SetDefault("Highly Corrosive");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 100;
			Item.rare = 2;
		}
	}
	public class OmniSoul : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omni Soul");
			Tooltip.SetDefault("'The essence of essences combined'");
			// ticksperframe, frameCount
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.SoulofSight);
			Item.width = refItem.width;
			Item.height = refItem.height;
			Item.maxStack = 999;
			Item.value = 1000;
			Item.rare = 6;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTimeWrappedHourly/3f)%1f, 0.85f, 0.50f);
		}

		public override void AddRecipes()
		{
			CreateRecipe(3).AddIngredient(ItemID.SoulofLight, 1).AddIngredient(ItemID.SoulofNight, 1).AddIngredient(ItemID.SoulofFlight, 1).AddIngredient(ItemID.SoulofFright, 1).AddIngredient(ItemID.SoulofMight, 1).AddIngredient(ItemID.SoulofSight, 1).AddTile(TileID.CrystalBall).Register();
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Main.hslToRgb((Main.GlobalTimeWrappedHourly / 3f)%1f, 0.85f, 0.80f).ToVector3() * 0.55f * Main.essScale);
		}
	}
	public class Entrophite : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Entrophite");
			Tooltip.SetDefault("Corrupted beyond the veils of life");
		}
		public override void SetDefaults()
		{
			Item.value = 100;
			Item.rare = ItemRarityID.Lime;
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Dimensions.Tiles.EntrophicOre>();
		}

	}

	public class WovenEntrophite : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Woven Entrophite");
			Tooltip.SetDefault("Suprisingly strong, after being interlaced with souls");
		}

		public override void SetDefaults()
		{
			Item.value = 250;
			Item.rare = ItemRarityID.Lime;
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("WovenEntrophiteTile").Type;
		}
		public override void AddRecipes()
		{
			CreateRecipe(10).AddIngredient(ModContent.ItemType<OmniSoul>(), 1).AddIngredient(ModContent.ItemType<Entrophite>(), 10).AddTile(TileID.Loom).Register();
		}

	}

	public class AdvancedPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Advanced Plating");
			Tooltip.SetDefault("Advanced for the land of Terraria's standards, that is");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 26;
			Item.height = 14;
			Item.value = 1000;
			Item.rare = ItemRarityID.Green;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.AdvancedPlatingTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(3).AddIngredient(mod.ItemType("NoviteBar"), 2).AddIngredient(ItemID.Wire, 10).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}
	}
	public class ManaBattery : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Battery");
			Tooltip.SetDefault("Encapsulated mana to be used as a form of energy for techno weapons");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 26;
			Item.value = 15000;
			Item.rare = 3;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("WraithFragment3"), 3).AddIngredient(ItemID.ManaCrystal, 1).AddIngredient(mod.ItemType("UnmanedBar"), 3).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}
	}
	public class PlasmaCell : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Cell");
			Tooltip.SetDefault("Heated plasmic energy resides within");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 20;
			Item.width = 26;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Yellow;
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(ModContent.ItemType<AdvancedPlating>(), 2).AddIngredient(ModContent.ItemType<WraithFragment4>(), 2).AddIngredient(ModContent.ItemType<ManaBattery>(), 1).AddIngredient(ItemID.MeteoriteBar, 5).AddIngredient(ModContent.ItemType<VialofAcid>(), 5).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<EmptyPlasmaCell>(), 1).AddIngredient(ItemID.MeteoriteBar, 2).AddIngredient(ModContent.ItemType<VialofAcid>(), 3).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<EmptyPlasmaCell>(), 1).AddIngredient(ModContent.ItemType<OverseenCrystal>(), 2).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();

		}
	}
	public class EmptyPlasmaCell : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empty Plasma Cell");
			Tooltip.SetDefault("Casing not yet filled with plasma");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 20;
			Item.width = 26;
			Item.height = 14;
			Item.value = Item.sellPrice(0,0,10,0);
			Item.rare = ItemRarityID.LightRed;
		}
	}
	public class CryostalBar: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryostal Bar");
			Tooltip.SetDefault("Condensed ice magic has formed into this bar");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 26;
			Item.height = 14;
			Item.value = 1000;
			Item.rare = 5;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("CryostalBarTile").Type;
		}
	}
	public class EldritchTentacle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eldritch Tentacle");
			Tooltip.SetDefault("Remains of an eldritch deity\nMay be used alongside fragments to craft all of Moonlord's drops");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 14;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 9;
		}
	}	
	public class IlluminantEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Essence");
			Tooltip.SetDefault("'Shards of Heaven'");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
        public override string Texture => "SGAmod/Items/IlluminantEssence";
        public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.HotPink.ToVector3() * 0.55f * Main.essScale);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 26;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 11;
		}
	}	
	public class AuroraTear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aurora Tear");
			Tooltip.SetDefault("'Auroric Energy from the Banshee, it seems to be inert...'");
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Lerp(Color.BlueViolet, Color.HotPink, (float)Math.Sin((Main.essScale-0.70f)/0.30f)).ToVector3() * 0.75f * Main.essScale);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 26;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 11;
		}
	}

	public class AuroraTearAwoken : ModItem, IAuroraItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Awoken Aurora Tear");
			Tooltip.SetDefault("'Bustling with awoken, Luminous energy'");
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Lerp(Color.BlueViolet, Color.HotPink, (float)Math.Sin((Main.essScale - 0.70f) / 0.30f)).ToVector3() * 0.85f * Main.essScale);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 30;
			Item.width = 26;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 2, 50, 0);
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.value = 0;
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item35;
		}
	}

	public class LunarRoyalGel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Royal Gel");
			Tooltip.SetDefault("From the moon-infused Pinky");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 6));
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 100000;
			Item.rare = 9;
		}
	}
	public class AncientFabricItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Fabric");
			Tooltip.SetDefault("Strands of Reality, predating back to the Big Bang");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.DarkRed.ToVector3() * 0.15f * Main.essScale);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 14;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.rare = 10;
		}
	}

	public class WatchersOfNull : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("01001110 01010101 01001100 01001100");
			Tooltip.SetDefault("'Essence of N0ll Watchers, watching...'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(7, 13));
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 32;
			Item.height = 32;
			Item.value = 100000;
			Item.rare = 10;
		}
	}

	public class CosmicFragment: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Fragment");
			Tooltip.SetDefault("The core of a celestial experiment; it holds unmatched power\nUsed to make Dev items");
			ItemID.Sets.ItemIconPulse[Item.type] = true;
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 16;
			Item.height = 16;
			Item.value = 0;
			Item.rare = 9;
			Item.expert=true;
		}

		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange *= 5;
		}

		public override bool GrabStyle(Player player)
		{
			Vector2 vectorItemToPlayer = player.Center - Item.Center;
			Vector2 movement = vectorItemToPlayer.SafeNormalize(default(Vector2)) * 0.1f;
			Item.velocity = Item.velocity + movement;
			Item.velocity = Collision.TileCollision(Item.position, Item.velocity, Item.width, Item.height);
			return true;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale);
		}

	}

	public class EmptyCharm: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empty Amulet");
			Tooltip.SetDefault("An empty amulet necklace, ready for enchanting");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 20;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = 0;
			Item.consumable = false;
		}
	}

	public class StarMetalMold: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Metal Mold");
			Tooltip.SetDefault("A mold used to make Wraith Cores, it seems fit to mold bars from heaven\nIs not consumed in crafting Star Metal Bars");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 20;
			Item.height = 20;
			Item.value = 0;
			Item.rare = 8;
			Item.consumable = false;
		}
	}

	public class StarMetalBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Metal Bar");
			Tooltip.SetDefault("'This bar is a glimming white sliver that shimmers with stars baring the color of pillars'");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 20;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 25, 0);
			Item.rare = 9;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("StarMetalBarTile").Type;
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(mod.ItemType("StarMetalMold"), 1).AddIngredient(ItemID.LunarOre, 1).AddRecipeGroup("Fragment", 4).Register();
		}

	}
	public class DrakeniteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Drakenite Bar");
			Tooltip.SetDefault("A Bar forged from the same powers that created Draken...");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 20;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = 9;
			Item.consumable = false;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("DrakeniteBarTile").Type;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.Mod == "Terraria" && line.Name == "ItemName")
				{
					line.OverrideColor = Color.Lerp(Color.DarkGreen, Color.White, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 8f));
				}
			}
		}
		public static Texture2D[] staticeffects = new Texture2D[32];
		public static void CreateTextures()
		{
			if (!Main.dedServ)
			{
				Texture2D atex = ModContent.Request<Texture2D>("SGAmod/Items/DrakeniteBarHalf");
				int width = atex.Width; int height = atex.Height;
				for (int index = 0; index < staticeffects.Length; index++)
				{
					Texture2D tex = new Texture2D(Main.graphics.GraphicsDevice, width, height);

					var datacolors2 = new Color[atex.Width * atex.Height];
					atex.GetData(datacolors2);
					tex.SetData(datacolors2);

					DrakeniteBar.staticeffects[index] = new Texture2D(Main.graphics.GraphicsDevice, width, height);
					Color[] dataColors = new Color[atex.Width * atex.Height];


					for (int y = 0; y < height; y++)
					{
						for (int x = 0; x < width; x += 1)
						{
							if (Main.rand.Next(0, 16) == 1)
							{
								int therex = (int)MathHelper.Clamp((x), 0, width);
								int therey = (int)MathHelper.Clamp((y), 0, height);
								if (datacolors2[(int)therex + therey * width].A > 0)
								{

									dataColors[(int)therex + therey * width] = Main.hslToRgb(Main.rand.NextFloat(0f, 1f) % 1f, 0.6f, 0.8f) * (0.5f);
								}
							}
							if (Main.rand.Next(0, 8) > Math.Abs(x-(index-8)))
							{
								int therex = (int)MathHelper.Clamp((x), 0, width);
								int therey = (int)MathHelper.Clamp((y), 0, height);
								if (datacolors2[(int)therex + therey * width].A > 0)
								{
									dataColors[(int)therex + therey * width] = Main.hslToRgb(((float)(index-8)/ (float)width) % 1f, 0.9f, 0.75f)*(0.80f*(1f-(Math.Abs((float)x - ((float)index -8f))/8f)));
								}
							}


						}

					}

					DrakeniteBar.staticeffects[index].SetData(dataColors);
				}
			}

		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor,
	Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = DrakeniteBar.staticeffects[(int)(Main.GlobalTimeWrappedHourly*20f)%DrakeniteBar.staticeffects.Length];
				Vector2 slotSize = new Vector2(52f, 52f);
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, drawPos, null, drawColor, 0f, textureOrigin, Main.inventoryScale*2f, SpriteEffects.None, 0f);
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.LunarBar, 1).AddIngredient(mod.ItemType("ByteSoul"), 10).AddIngredient(mod.ItemType("WatchersOfNull"), 1).AddIngredient(mod.ItemType("AncientFabricItem"), 25).AddIngredient(mod.ItemType("HopeHeart"), 1).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}

	}

	public class CopperWraithNotch: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Wraith Notch");
			Tooltip.SetDefault("Intact remains of the Copper Wraith's animated armor");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 14;
			Item.height = 14;
			Item.value = 20;
			Item.rare = ItemRarityID.White;
		}
	}
	public class CobaltWraithNotch: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Wraith Notch");
			Tooltip.SetDefault("Intact remains of the Cobalt Wraith's animated armor, stronger than before");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 14;
			Item.height = 14;
			Item.value = 200;
			Item.rare = ItemRarityID.Pink;
		}
	}
	public class LuminiteWraithNotch: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminite Wraith Notch");
			Tooltip.SetDefault("Intact remains of the Luminate Wraith's special armor");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 14;
			Item.height = 14;
			Item.value = 10000;
			Item.rare = ItemRarityID.Red;
		}
	}
	public class WraithFragment: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Wraith Shard");
			Tooltip.SetDefault("The remains of a weak wraith; it is light and conductive");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 14;
			Item.height = 14;
			Item.value = 5;
			Item.rare = ItemRarityID.White;
		}
	}
	public class WraithFragment2: WraithFragment
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tin Wraith Shard");
			Tooltip.SetDefault("The remains of a weak wraith; it is soft and malleable");
		}
	}

	public class WraithFragment3: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bronze Alloy Wraith Shard");
			Tooltip.SetDefault("Tin and copper combined through the fires of a hellforge; thus stronger than a standard shard");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 14;
			Item.height = 14;
			Item.value = 25;
			Item.rare = ItemRarityID.Orange;
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(mod.ItemType("WraithFragment"), 2).AddIngredient(ItemID.TinOre, 4).AddTile(TileID.Hellforge).Register();
			CreateRecipe(2).AddIngredient(mod.ItemType("WraithFragment2"), 2).AddIngredient(ItemID.CopperOre, 4).AddTile(TileID.Hellforge).Register();
			CreateRecipe(1).AddIngredient(this, 1).AddIngredient(ItemID.LivingFireBlock, 3).AddTile(TileID.Hellforge).ReplaceResult(mod.ItemType("FieryShard"));

		}
	}

	public class WraithFragment4 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Wraith Shard");
			Tooltip.SetDefault("The remains of a stronger wraith; applyable uses in alloys and highly resistant to corrosion");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 14;
			Item.height = 14;
			Item.value = 30;
			Item.rare = ItemRarityID.Green;
		}
	}

	public class UnmanedBar: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Bar");
			Tooltip.SetDefault("This alloy of Novus and the power of the wraiths have awakened some of its dorment power\nMay be interchanged for iron bars in some crafting recipes");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 99;
			Item.width = 16;
			Item.height = 16;
			Item.value = 25;
			Item.rare = ItemRarityID.Blue;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("UnmanedBarTile").Type;
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(mod.ItemType("UnmanedOre"), 4).AddRecipeGroup("SGAmod:BasicWraithShards",3).AddTile(TileID.Furnaces).Register();
		}
	}
	public class UnmanedOre: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Ore");
			Tooltip.SetDefault("Stone laden with doment power...");
		}
	public override void SetDefaults()
        {
		Item.width = 16;
		Item.height = 16;
		Item.maxStack = 999;
		Item.value = 10;
		Item.rare = ItemRarityID.Blue;
		Item.alpha = 0;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = true;
		Item.createTile = Mod.Find<ModTile>("UnmanedOreTile").Type;

		}
	}
	public class NoviteOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Ore");
			Tooltip.SetDefault("Brassy scrap metal from a time along ago, might be of electronical use...");
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.value = 10;
			Item.rare = ItemRarityID.Blue;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("NoviteOreTile").Type;

		}
	}
	public class NoviteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Bar");
			Tooltip.SetDefault("This Brassy alloy reminds you of 60s scifi");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 99;
			Item.width = 16;
			Item.height = 16;
			Item.value = 25;
			Item.rare = ItemRarityID.Blue;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("NoviteBarTile").Type;
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(mod.ItemType("NoviteOre"), 4).AddRecipeGroup("SGAmod:BasicWraithShards", 3).AddTile(TileID.Furnaces).Register();
		}
	}
	public class MoneySign : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Raw Avarice");
			Tooltip.SetDefault("'pure greed'");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 14;
			Item.height = 14;
			Item.value = 75000;
			Item.rare = ItemRarityID.Red;
		}
	}

	public class ByteSoul : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Byte");
			Tooltip.SetDefault("'remains of the Hellion Core'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
		}
		/*public override string Texture
		{
			get { return ("Terraria/Item_"+Main.rand.Next(0,2000)); }
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb(Main.rand.NextFloat(0f, 1f), 0.75f, 0.65f);
		}*/
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Main.hslToRgb((-Main.GlobalTimeWrappedHourly+(Item.whoAmI*7.4231f))%1f,0.92f,0.85f).ToVector3() * 0.5f);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 14;
			Item.height = 14;
			Item.value = 10000;
			Item.rare = ItemRarityID.Red;
		}
	}

	public class AssemblyStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Assembly Star");
			Tooltip.SetDefault("'Raw assembly code forged directly from Draken'\nCan be used to craft previously uncraftable items");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
		public override string Texture
		{
			get { return "Terraria/SunOrb"; }
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Orange*MathHelper.Clamp((float)(Math.Sin(Main.GlobalTimeWrappedHourly)/2)+1f,0,1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 14;
			Item.height = 14;
			Item.value = 0;
			Item.rare = ItemRarityID.Quest;
		}
	}

	public class StygianCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stygian Star");
			Tooltip.SetDefault("'Torn from Stygian Veins with a mining tool, this star is burning fabric made manifest...'");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = 50000;
			Item.maxStack = 10;
			Item.rare = ItemRarityID.Red;
		}
		public override string Texture
		{
			get { return "Terraria/Sun"; }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Magenta*0.50f;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Texture2D inner = Main.itemTexture[ModContent.ItemType<AssemblyStar>()];

			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);
			

			for (float i = 0; i < 1f; i += 0.10f)
			{
				spriteBatch.Draw(inner, drawPos, null, (Color.DarkMagenta * (1f - ((i + (Main.GlobalTimeWrappedHourly / 2f)) % 1f)) * 0.5f)*0.50f, i * MathHelper.TwoPi, textureOrigin, Main.inventoryScale * (0.5f + 1.75f * (((Main.GlobalTimeWrappedHourly / 2f) + i) % 1f)), SpriteEffects.None, 0f);
			}

			return true;
		}

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {

			Texture2D inner = Main.itemTexture[ModContent.ItemType<AssemblyStar>()];

			Vector2 slotSize = new Vector2(52f, 52f);
			Vector2 position = Item.Center-Main.screenPosition;

			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);

			for (float i = 0; i < 1f; i += 0.10f)
			{
				spriteBatch.Draw(inner, position, null, (Color.DarkMagenta * (1f - ((i + (Main.GlobalTimeWrappedHourly / 2f)) % 1f)) * 0.5f) * 0.50f, i * MathHelper.TwoPi, textureOrigin, 1f * (0.5f + 1.75f * (((Main.GlobalTimeWrappedHourly / 2f) + i) % 1f)), SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(Main.itemTexture[Item.type],position,null,alphaColor,rotation, Main.itemTexture[Item.type].Size()/2f, 128f/256f, SpriteEffects.None, 0f);

			return false;
		}

	}

	public class HopeHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hopeful Heart");
			Tooltip.SetDefault("'There is always hope in the darkness...'\nRestores 30 lost max HP when picked up\nIs collected if your barely missing any life instead\nCannot be picked up while a boss is alive");
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 14;
			Item.maxStack = 30;
			Item.rare = 8;
			Item.value = 1000;
		}
		public override string Texture
		{
			get { return "SGAmod/Items/Consumables/HopefulHeartItem"; }
		}
		public override bool CanPickup(Player player)
        {
            return !IdgNPC.bossAlive;
        }
        public override bool OnPickup(Player player)
        {
			if (player.GetModPlayer<IdgPlayer>().radationAmmount<5)
            {
				return true;
			}
			UseItem2(player);
			return false;
		}
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
			Lighting.AddLight(Item.Center / 16f, (Color.PaleGoldenrod * 0.5f).ToVector3());
        }
        public void UseItem2(Player player)
        {
			SoundEngine.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 4, 0.75f, -0.65f);
			player.HealEffect(30*Item.stack,true);
			player.GetModPlayer<IdgPlayer>().radationAmmount = Math.Max(player.GetModPlayer<IdgPlayer>().radationAmmount - (30 * Item.stack), 0);
			Item.TurnToAir();
        }

	}

	public class PrismalBar: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Bar");
			Tooltip.SetDefault("It radiates the true energy of Novus");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 20;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 40, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("PrismalBarTile").Type;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("PrismalOre"), 4).AddTile(TileID.AdamantiteForge).Register();
		}

	}

	public class PrismalOre: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Ore");
			Tooltip.SetDefault("The power inside is cracked wide open, ready to be used");
		}
	public override void SetDefaults()
        {
		Item.width = 16;
		Item.height = 16;
		Item.maxStack = 99;
		Item.value = 7500;
		Item.rare = ItemRarityID.Yellow;
		Item.alpha = 0;
		Item.useTurn = true;
		Item.autoReuse = true;
		Item.useAnimation = 15;
		Item.useTime = 10;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.consumable = true;
		Item.createTile = Mod.Find<ModTile>("PrismalTile").Type;

	}
		public override string Texture
		{
			get { return ("SGAmod/Items/PrismalOre2"); }
		}

		public override void AddRecipes()
		{
			CreateRecipe(16).AddIngredient(mod.ItemType("UnmanedOre"), 8).AddIngredient(mod.ItemType("NoviteOre"), 8).AddIngredient(mod.ItemType("WraithFragment3"), 1).AddIngredient(mod.ItemType("Fridgeflame"), 3).AddIngredient(mod.ItemType("OmniSoul"), 2).AddIngredient(ItemID.CrystalShard, 3).AddIngredient(ItemID.BeetleHusk, 1).AddTile(mod.GetTile("PrismalStation")).Register();
		}

	}

	public class HeliosFocusCrystal : ModItem
	{
		public virtual Color MainColor => Color.Black;
					public virtual Color BackColor => Color.Black;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helios Focus Prism");
			Tooltip.SetDefault("An intact focus prism used to empower Phaethon\nCould be useful for crafting your own empowerment devices");
		}
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.value = Item.sellPrice(0,5,0,0);
			Item.rare = ItemRarityID.Yellow;
			Item.maxStack = 30;
			//item.damage = 1;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTimeWrappedHourly/4f) % 1f, 1f, 0.75f);
		}
		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.DD2ElderCrystal); }
		}
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
			Color glowColor = MainColor == Color.Black ? lightColor : MainColor;

			float vel = Item.velocity.X / 6f;
			Vector2 drawPos = (Item.Center+new Vector2(0, Main.itemTexture[Item.type].Height-4)) - Main.screenPosition;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 6f)
			{
				Color glowColor2 = BackColor == Color.Black ? Main.hslToRgb(f / MathHelper.TwoPi, 1f, 0.75f) : BackColor;
				spriteBatch.Draw(Main.itemTexture[Item.type], (drawPos + (Vector2.UnitX.RotatedBy(f + Main.GlobalTimeWrappedHourly * 2f) * 3f).RotatedBy(vel)), null, glowColor2, vel, Main.itemTexture[Item.type].Size() / 2f, scale, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			spriteBatch.Draw(Main.itemTexture[Item.type], drawPos, null, glowColor, vel, Main.itemTexture[Item.type].Size() / 2f, scale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			SGAmod.FadeInEffect.Parameters["fadeColor"].SetValue(2f);
			SGAmod.FadeInEffect.Parameters["alpha"].SetValue(0.50f);
			SGAmod.FadeInEffect.CurrentTechnique.Passes["ColorToAlphaPass"].Apply();

			spriteBatch.Draw(Main.itemTexture[Item.type], drawPos, null, glowColor, vel, Main.itemTexture[Item.type].Size() / 2f, scale / 15f, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Color glowColor = MainColor == Color.Black ? drawColor : MainColor;

			Vector2 slotSize = new Vector2(52f, 52f) * scale;
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;

			slotSize.X /= 1.0f;
			slotSize.Y = -slotSize.Y / 4f;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 6f)
			{
				Color glowColor2 = BackColor == Color.Black ? Main.hslToRgb(f / MathHelper.TwoPi, 1f, 0.75f) : BackColor;
				spriteBatch.Draw(Main.itemTexture[Item.type], drawPos+(Vector2.UnitX.RotatedBy(f+Main.GlobalTimeWrappedHourly*2f)*3f), null, glowColor2, 0, Main.itemTexture[Item.type].Size() / 2f, Main.inventoryScale, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			spriteBatch.Draw(Main.itemTexture[Item.type], drawPos, null, glowColor, 0, Main.itemTexture[Item.type].Size() / 2f, Main.inventoryScale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			SGAmod.FadeInEffect.Parameters["fadeColor"].SetValue(2f);
			SGAmod.FadeInEffect.Parameters["alpha"].SetValue(0.50f);
			SGAmod.FadeInEffect.CurrentTechnique.Passes["ColorToAlphaPass"].Apply();

			spriteBatch.Draw(Main.itemTexture[Item.type], drawPos, null, glowColor, 0, Main.itemTexture[Item.type].Size() / 2f, Main.inventoryScale*1/15f, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			return false;
		}
	}

	public class EntropyTransmuter : ModItem
	{
		static internal int MaxEntropy = 100000;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Entropy Transmuter");
			Tooltip.SetDefault("As enemies die near you, the Transmuter absorbs their life essences\nWhich converts Converts Demonite or Crimtane ore in your inventory into Entrophite\nConverts a maximum of 20 per full charge");
		}
		public override void SetDefaults()
		{
			Item.value = 0;
			Item.rare = ItemRarityID.Green;
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 1;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "Entropy", "Entropy Collected: "+Main.LocalPlayer.GetModPlayer<SGAPlayer>().entropyCollected + "/" + MaxEntropy));
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("Entrophite"), 250).AddIngredient(mod.ItemType("StygianCore"), 1).AddIngredient(mod.ItemType("OmniSoul"), 15).AddIngredient(ItemID.Diamond, 1).AddTile(TileID.CrystalBall).Register();
		}
	}

	public class EALogo : ModItem
	{

		public override void SetDefaults()
		{
			Item.value = 1000000;
			Item.rare = ItemRarityID.Cyan;
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 1;
			Item.expert = true;
		}

		public override void UpdateInventory(Player player)
		{
			player.GetModPlayer<SGAPlayer>().EALogo = true;
			if (player.taxMoney >= Item.buyPrice(0, 10, 0, 0))
			{
				player.taxMoney = 0;
				player.QuickSpawnItem(ItemID.GoldCoin,10);
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("EA Logo");
			Tooltip.SetDefault("Lets you charge maximum micro-transactions against your town NPCs\nWhile in your inventory: you can reforge unique prefixes for accessories\nYou automatically collect taxes while you have a Tax Collector\nPicking up Hearts and Mana Stars gives you money\nPress the 'Collect Taxes' hotkey to collect a gold coin from your tax collector's purse\n'EA! It's NOT in the game, that's DLC!'");
		}

	}

	public class TheWholeExperience : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("'The Whole Experience'");
			Tooltip.SetDefault("While in your inventory, specific cutscenes and events will replay\nLuminite Wraith will be summoned in his pre-Moonlord stage\nKiller Fly Swarm will be summoned instead of Murk\nHellion will replay her monolog after Hellion Core");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
		public static bool Check()
        {
			foreach(Player player in Main.player)
            {
				if (player.HasItem(ModContent.ItemType<TheWholeExperience>()) || player.HasItem(ModContent.ItemType<TheWholeExperienceEX>()))
					return true;
            }
			return false;
		}
		public static bool CheckHigherTier(bool highertier = false)
		{
			foreach (Player player in Main.player)
			{
				if (player.HasItem(ModContent.ItemType<TheWholeExperienceEX>()))
					return true;
			}
			return false;
		}
		public override string Texture
		{
			get { return "Terraria/UI/Camera_7"; }
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 14;
			Item.height = 14;
			Item.value = 0;
			Item.rare = ItemRarityID.Quest;
		}
	}

	public class TheWholeExperienceEX : TheWholeExperience
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("'The Whole Experience EX'");
			Tooltip.SetDefault("Same effects as 'The Whole Experience', but now prevents leaving subworlds on death\nSubworld bosses will reset when you die and you respawn in place");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
		}
        public override Color? GetAlpha(Color lightColor)
        {
			return Main.hslToRgb(Main.GlobalTimeWrappedHourly%1f,1f,0.75f);
        }
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<TheWholeExperience>(), 1).AddIngredient(ModContent.ItemType<WatchersOfNull>(), 20).AddTile(TileID.LunarCraftingStation).Register();
		}

	}

		public class DungeonSplunker : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dungeon Spulunker");
			Tooltip.SetDefault("While in your inventory, allows you to use pickaxes in the Deeper Dungeons");
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTimeWrappedHourly * 0.916f) % 1f, 0.8f, 0.75f);
		}
		public override string Texture
		{
			get { return "Terraria/UI/Cursor_10"; }
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 14;
			Item.height = 14;
			Item.value = 0;
			Item.rare = ItemRarityID.Quest;
		}
	}
	public class ShadowLockBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow LockBox");
			Tooltip.SetDefault("Right click to open, must have a Shadow Key\n'Yes, this is literally just placeholder 1.4 content'");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 30;
			Item.width = 14;
			Item.height = 14;
			Item.value = 0;
			Item.rare = ItemRarityID.Quest;
		}
		public override bool CanRightClick()
		{
			return Main.LocalPlayer.HasItem(ItemID.ShadowKey);
		}

		public override void RightClick(Player player)
		{
			List<int> lootrare = new List<int> { ItemID.DarkLance, ItemID.Sunfury, ItemID.Flamelash, ItemID.FlowerofFire, ItemID.HellwingBow };

			player.QuickSpawnItem(lootrare[Main.rand.Next(lootrare.Count)]);
		}


	}
	public class HellionCheckpoint1 : ModItem
	{
		protected virtual Color color => Color.Lime;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lession 1: Pedant of Doubt");
			Tooltip.SetDefault("'And there without, is not without doubt, of your failure...'\nPlace your inventory to allow Reality's Sunder summon Hellion at post Goblin Army\n" + Idglib.ColorText(Color.Red, "Hellion will not drop her crown, and will drop 25% less items\nWill consume one when summoned"));
		}
        public override bool Autoload(ref string name)
        {
			return false;
        }
        public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.value = 0;
			Item.rare = -12;
			Item.expert = true;
			Item.maxStack = 30;
			//item.damage = 1;
		}
		public override string Texture => "Terraria/Item_"+ItemID.AlphabetStatue1;
		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb(Main.GlobalTimeWrappedHourly % 1f, 1f, 0.75f);
		}
		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange *= 32;
		}
		public override bool GrabStyle(Player player)
		{
			Vector2 vectorItemToPlayer = player.Center - Item.Center;
			Vector2 movement = vectorItemToPlayer.SafeNormalize(default(Vector2)) * 0.25f;
			Item.velocity = Item.velocity + movement;
			Item.velocity = Collision.TileCollision(Item.position, Item.velocity, Item.width, Item.height);
			return true;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D inner = Main.itemTexture[Item.type];

			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new Vector2(inner.Width, inner.Height) / 2f;

			for (float f = 0f; f < 4; f += 0.25f)
			{
				spriteBatch.Draw(inner, drawPos+new Vector2(Main.rand.NextFloat(-f,f), Main.rand.NextFloat(-f, f)), null, (Color)GetAlpha(drawColor)*0.10f, 0, textureOrigin, Main.inventoryScale * 1, SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(inner, drawPos+new Vector2((-0.50f+Main.GlobalTimeWrappedHourly%1)*16f* scale,0), null, color * 1f, 0, textureOrigin, Main.inventoryScale * 0.50f, SpriteEffects.None, 0f);

			return false;
		}
	}

	public class HellionCheckpoint2 : HellionCheckpoint1
	{
		protected override Color color => Color.Purple;
		public override string Texture => "Terraria/Item_" + ItemID.AlphabetStatue2;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lession 2: Rotten Desires");
			Tooltip.SetDefault("'And there without, is not without doubt, of your failure...'\nPlace your inventory to allow Reality's Sunder summon Hellion at post Pirate Army\n" + Idglib.ColorText(Color.Red, "Hellion will not drop her crown, and will drop 50% less items\nWill consume one when summoned"));
		}
	}

	public class HellionCheckpoint3 : HellionCheckpoint1
	{
		protected override Color color => Color.Red;
		public override string Texture => "Terraria/Item_" + ItemID.AlphabetStatue3;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lession 3: Climax of Eternity");
			Tooltip.SetDefault("'And there without, is not without doubt, of your failure...'\nPlace your inventory to allow Reality's Sunder summon Hellion at post Festive Moons Army\n" + Idglib.ColorText(Color.Red, "Hellion will not drop her crown, and will drop 75% less items\nWill consume one when summoned"));
		}
	}

	public class HellionCheckpoint4 : HellionCheckpoint1
	{
		protected override Color color => Color.Black;
		public override string Texture => "Terraria/Item_" + ItemID.AlphabetStatue4;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lession 4: Epilogue");
			Tooltip.SetDefault("'The End'\nPlace your inventory to allow Reality's Sunder summon Hellion with 1 HP\n" + Idglib.ColorText(Color.Red, "Hellion will not drop her crown, and will drop 90% less items\nWill consume one when summoned"));
		}
	}

	public class FinalGem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Final Gem");
			Tooltip.SetDefault("While in your inventory, empowers the Gucci Guantlet to its true full power\nFavorite to disable all the gems");
		}
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.value = 0;
			Item.rare = -12;
			Item.expert = true;
			Item.maxStack = 1;
			//item.damage = 1;
		}
        public override void UpdateInventory(Player player)
        {
			if (!Item.favorited)
			player.SGAPly().finalGem = 3;
        }

        public override string Texture
		{
			get { return ("Terraria/Extra_57"); }
		}

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			//texture mappedTexture;
			//float2 mappedTextureMultiplier;
			//float2 mappedTextureOffset;

			/*texture _VoronoiTex;
			float4 _CellColor = float4(1, .75, 0, 1); // Orange
			float4 _EdgeColor = float4(1, .5, 0, 1); // Yellow-Orange
			float2 _CellSize = float2(1.5, 2.0);
			float _ScrollSpeed = 0.04;
			float _FadeSpeed = 3;
			float _ColorScale = 1.5652475842498528; // .7*sqrt(5)
			float _Time; // Pass the time in seconds into here
			*/

			Texture2D tex = ModContent.Request<Texture2D>("SGAmod/voronoismol");

			SGAmod.VoronoiEffect.Parameters["_CellColor"].SetValue(Color.Black.ToVector4() * 1f);
			SGAmod.VoronoiEffect.Parameters["_EdgeColor"].SetValue(Color.Lerp(Color.Orange,Color.Yellow,0.50f).ToVector4() * 1f);
			SGAmod.VoronoiEffect.Parameters["_CellSize"].SetValue(new Vector2(1.5f,2f)*1f);
			SGAmod.VoronoiEffect.Parameters["_ScrollSpeed"].SetValue(Main.GlobalTimeWrappedHourly/40000f);
			SGAmod.VoronoiEffect.Parameters["_FadeSpeed"].SetValue(3f);
			SGAmod.VoronoiEffect.Parameters["_ColorScale"].SetValue(1.5652475842498528f);
			SGAmod.VoronoiEffect.Parameters["_Time"].SetValue(Main.GlobalTimeWrappedHourly*1f);

			SGAmod.VoronoiEffect.CurrentTechnique.Passes["Star"].Apply();

			spriteBatch.Draw(tex, Item.Center - Main.screenPosition, null, Color.White, 0, tex.Size() / 2f, 1f, SpriteEffects.None, 0f);


			return false;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			float maxsize = 20;
			Texture2D[] gems = Main.gemTexture;
			Texture2D myTex = Main.itemTexture[Item.type];
			spriteBatch.Draw(myTex, position + new Vector2(14f, 14f), frame, drawColor*0.25f, Main.GlobalTimeWrappedHourly / 1f, myTex.Size() / 2f, scale * 2.5f * Main.essScale, SpriteEffects.None, 0f);
			spriteBatch.Draw(myTex, position + new Vector2(14f, 14f), frame, drawColor * 0.25f, -Main.GlobalTimeWrappedHourly / 1f, myTex.Size() / 2f, scale * 2.5f * Main.essScale, SpriteEffects.None, 0f);

			for (int i = 0; i < maxsize; i += 1)
			{
				Texture2D inner = gems[i % gems.Length];
				Double Azngle = i+(Main.GlobalTimeWrappedHourly/8f);
				Vector2 here = new Vector2((float)Math.Cos(Azngle), (float)Math.Sin(Azngle)) * (i * 2f);
				float scaler = (1f - (float)((float)i / maxsize));
				spriteBatch.Draw(inner, position + (new Vector2(14f, 14f)) + here, null, Color.Lerp(drawColor, Color.MediumPurple, 0.25f) * scaler, Main.GlobalTimeWrappedHourly *= (i % 2 == 0 ? -1f : 1f), new Vector2(inner.Width / 2, inner.Height / 2), scale * scaler, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(myTex, position + new Vector2(14f, 14f), frame, drawColor, Main.GlobalTimeWrappedHourly/1f, myTex.Size()/2f, scale * 1.5f * Main.essScale, SpriteEffects.None, 0f);
			spriteBatch.Draw(myTex, position + new Vector2(14f, 14f), frame, drawColor, -Main.GlobalTimeWrappedHourly/1f, myTex.Size() / 2f, scale * 1.5f * Main.essScale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			return false;
		}
	}


}

