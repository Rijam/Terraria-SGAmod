using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.HavocGear.Items;

namespace SGAmod.Items.Placeable.DankWoodFurniture
{
    #region Dank Wood Workbench
    public class DankWoodWorkbench : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 14;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 150;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodWorkbench>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWood>(), 10).Register();
		}
	}
    #endregion
    #region Dank Wood Chair
    public class DankWoodChair : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 30;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 150;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodChair>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWood>(), 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	#endregion
	#region Dank Wood Toilet
	public class DankWoodToilet : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 26;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 150;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodToilet>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWood>(), 6).AddTile(TileID.Sawmill).Register();
		}
	}
	#endregion
	#region Dank Wood Table
	public class DankWoodTable : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodTable>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWood>(), 8).AddTile(TileID.WorkBenches).Register();
		}
	}
	#endregion
	#region Dank Wood Dresser
	public class DankWoodDresser : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("This is a modded dresser.");
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodDresser>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWood>(), 16).AddTile(TileID.Sawmill).Register();
		}
	}
	#endregion
	#region Dank Wood Bed
	public class DankWoodBed : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 20;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 2000;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodBed>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWood>(), 15).AddIngredient(ItemID.Silk, 5).AddTile(TileID.Sawmill).Register();
		}
	}
	#endregion
	#region Dank Wood Sofa
	public class DankWoodSofa : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 24;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodSofa>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWood>(), 5).AddIngredient(ItemID.Silk, 2).AddTile(TileID.Sawmill).Register();
		}
	}
	#endregion
	#region Dank Wood Lantern
	internal class DankWoodLantern : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
			Item.autoReuse = true;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodLantern>();
            Item.width = 10;
            Item.height = 24;
            Item.value = 150;
        }
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWood>(), 6).AddIngredient(ModContent.ItemType<DankWoodTorch>(), 1).AddTile(TileID.WorkBenches).Register();
		}
	}
    #endregion
    #region Dank Wood Lamp
    internal class DankWoodLamp : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodLamp>();
			Item.width = 10;
			Item.height = 24;
			Item.value = 500;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWoodTorch>(), 1).AddIngredient(ModContent.ItemType<DankWood>(), 3).AddTile(TileID.WorkBenches).Register();
		}
	}
	#endregion
	#region Dank Wood Bookcase
	public class DankWoodBookcase : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("This is a modded bookcase.");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 32;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodBookcase>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWood>(), 20).AddIngredient(ItemID.Book, 10).AddTile(TileID.Sawmill).Register();
		}
	}
	#endregion
	#region Dank Wood Torch
	public class DankWoodTorch : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("This is a modded torch.");
		}

		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 12;
			Item.maxStack = 999;
			Item.holdStyle = 1;
			Item.noWet = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodTorch>();
			Item.flame = true;
			Item.value = 50;
		}

		public override void HoldItem(Player player)
		{
			if (Main.rand.Next(player.itemAnimation > 0 ? 40 : 80) == 0)
			{
				Dust.NewDust(new Vector2(player.itemLocation.X + 16f * player.direction, player.itemLocation.Y - 14f * player.gravDir), 4, 4, DustID.Fire);
			}
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			Lighting.AddLight(position, 1f, 0.75f, 0.30f);
		}

		public override void PostUpdate()
		{
			if (!Item.wet)
			{
				Lighting.AddLight((int)((Item.position.X + Item.width / 2) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 1f, 0.75f, 0.30f);
			}
		}

		public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
		{
			dryTorch = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(3).AddIngredient(ModContent.ItemType<DankWood>(), 1).AddIngredient(ItemID.Gel, 1).Register();
			CreateRecipe(3).AddIngredient(ModContent.ItemType<DankCore>(), 1).AddIngredient(ItemID.Torch, 3).Register();
		}
	}
	#endregion
	#region Dank Wood Clock
	public class DankWoodClock : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodClock>();
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWood>(), 10).AddIngredient(ModContent.ItemType<NoviteBar>(), 3).AddIngredient(ItemID.Glass, 6).AddTile(TileID.Sawmill).Register();
		}
	}
	#endregion
	#region Dank Wood Piano
	public class DankWoodPiano : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodPiano>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Bone, 4).AddIngredient(ModContent.ItemType<DankWood>(), 15).AddIngredient(ItemID.Book, 1).AddTile(TileID.Sawmill).Register();
		}
	}
	#endregion
	#region Dank Wood Sink
	public class DankWoodSink : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 30;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodSink>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWood>(), 6).AddIngredient(ItemID.WaterBucket, 1).AddTile(TileID.WorkBenches).Register();
		}
	}
	#endregion
	#region Dank Wood Bathtub
	public class DankWoodBathtub : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 300;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodBathtub>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DankWood>(), 14).AddTile(TileID.Sawmill).Register();
		}
	}
	#endregion
	#region Novite Candle
	internal class NoviteCandle : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 20;
			Item.maxStack = 999;
			Item.holdStyle = 1;
			Item.noWet = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.NoviteCandle>();
			Item.flame = true;
			Item.value = 0; //candles have no value for some reason.
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<NoviteBar>(), 4).AddIngredient(ModContent.ItemType<DankWoodTorch>(), 1).AddTile(TileID.WorkBenches).Register();
		}
		public override void HoldItem(Player player)
		{
			if (Main.rand.Next(player.itemAnimation > 0 ? 40 : 80) == 0)
			{
				Dust.NewDust(new Vector2(player.itemLocation.X + 16f * player.direction, player.itemLocation.Y - 14f * player.gravDir), 4, 4, DustID.Fire);
			}
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			Lighting.AddLight(position, 1f, 0.75f, 0.30f);
		}

		public override void PostUpdate()
		{
			if (!Item.wet)
			{
				Lighting.AddLight((int)((Item.position.X + Item.width / 2) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 1f, 0.75f, 0.30f);
			}
		}

		public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
		{
			dryTorch = true;
		}
	}
	#endregion
	#region Novite Chandelier
	internal class NoviteChandelier : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.NoviteChandelier>();
			Item.width = 30;
			Item.height = 28;
			Item.value = 3000;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<NoviteBar>(), 4).AddIngredient(ModContent.ItemType<DankWoodTorch>(), 4).AddIngredient(ItemID.Chain).AddTile(TileID.Anvils).Register();
		}
	}
	#endregion
	#region Novite Candelabra
	internal class NoviteCandelabra : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.NoviteCandelabra>();
			Item.width = 22;
			Item.height = 28;
			Item.value = 1500;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<NoviteBar>(), 3).AddIngredient(ModContent.ItemType<DankWoodTorch>(), 3).AddTile(TileID.WorkBenches).Register();
		}
	}
	#endregion
}