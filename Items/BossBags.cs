using Microsoft.Xna.Framework;
using SGAmod.HavocGear.Items.Accessories;
using SGAmod.HavocGear.Items.Weapons;
using SGAmod.Items.Weapons;
using SGAmod.Items.Armors.Vanity;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items
{
	public class MurkBossBag : ModItem
	{
		public override void SetDefaults()
		{

			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;

			Item.rare = 9;
			Item.expert = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}

		public override int BossBagNPC
		{
			get
			{
				return Mod.Find<ModNPC>("Murk").Type;
			}
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{

			int random = Main.rand.Next(7);
			if (random == 6)
			{
				player.QuickSpawnItem(ModContent.ItemType<SwarmGun>());
			}
			if (random == 5)
			{
				player.QuickSpawnItem(ModContent.ItemType<GnatStaff>(), 1);
			}
			if (random == 4)
			{
				player.QuickSpawnItem(ModContent.ItemType<SwarmGrenade>(), Main.rand.Next(40, 100));
			}
			if (random == 3)
			{
				player.QuickSpawnItem(ModContent.ItemType<Mudmore>());
			}
			if (random == 2)
			{
				player.QuickSpawnItem(ModContent.ItemType<MurkFlail>());
			}
			if (random == 1)
			{
				player.QuickSpawnItem(ModContent.ItemType<Mossthorn>());
			}
			if (random == 0)
			{
				player.QuickSpawnItem(ModContent.ItemType<Landslide>());
			}
			player.QuickSpawnItem(ModContent.ItemType<MudAbsorber>());
			player.QuickSpawnItem(ModContent.ItemType<MurkyGel>(), Main.rand.Next(50, 70));
			if (Main.rand.Next(7) == 0)
			{
				player.QuickSpawnItem(ModContent.ItemType<MurkMask>());
			}
		}
	}
	public class SharkvernBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.rare = 11;
			Item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override int BossBagNPC
		{
			get
			{
				return Mod.Find<ModNPC>("SharkvernHead").Type;
			}
		}

		public override void OpenBossBag(Player player)
		{

			List<int> types = new List<int>();
			types.Insert(types.Count, ItemID.SharkFin);
			types.Insert(types.Count, ItemID.Seashell);
			types.Insert(types.Count, ItemID.Starfish);
			types.Insert(types.Count, ItemID.SoulofFlight);
			types.Insert(types.Count, ItemID.Coral);

			/*
			for (int f = 0; f < (Main.expertMode ? 150 : 75); f = f + 1)
			{
				player.QuickSpawnItem(types[Main.rand.Next(0, types.Count)]);
			}*/

			DropHelper.DropFixedItemQuanity(types.ToArray(), Main.expertMode ? 150 : 75,Vector2.Zero,player);

			player.TryGettingDevArmor();
			int lLoot = (Main.rand.Next(0, 4));
			player.QuickSpawnItem(Mod.Find<ModItem>("SerratedTooth").Type);
			if (lLoot == 0)
			{
				player.QuickSpawnItem(Mod.Find<ModItem>("SkytoothStorm").Type);
			}
			if (lLoot == 1)
			{
				player.QuickSpawnItem(Mod.Find<ModItem>("Jaws").Type);
			}
			if (lLoot == 2)
			{
				player.QuickSpawnItem(Mod.Find<ModItem>("SnappyShark").Type);
				player.QuickSpawnItem(Mod.Find<ModItem>("SharkTooth").Type, 150);
			}
			if (lLoot == 3)
			{
				player.QuickSpawnItem(Mod.Find<ModItem>("SharkBait").Type, Main.rand.Next(60, 150));
			}
			player.QuickSpawnItem(Mod.Find<ModItem>("SharkTooth").Type, Main.rand.Next(100, 200));
			if (Main.rand.Next(7) == 0)
			{
				player.QuickSpawnItem(ModContent.ItemType<SharkvernMask>());
			}
		}
	}
}


namespace SGAmod.Items
{
	public class PhaethonBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 32;
			Item.height = 32;
			Item.expert = true;
			Item.rare = -12;
		}

		public override int BossBagNPC
		{
			get
			{
				return ModContent.NPCType<Dimensions.NPCs.SpaceBoss>();
			}
		}

		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(ModContent.ItemType<StarMetalMold>());
			player.QuickSpawnItem(ModContent.ItemType<Accessories.PhaethonEye>(), 1);
		}
	}	
	public class SpiderBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 32;
			Item.height = 32;
			Item.expert = true;
			Item.rare = -12;
		}

		public override int BossBagNPC
		{
			get
			{
				return Mod.Find<ModNPC>("SpiderQueen").Type;
			}
		}

		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(Mod.Find<ModItem>("VialofAcid").Type, Main.rand.Next(35, 60));
			player.QuickSpawnItem(Mod.Find<ModItem>("AlkalescentHeart").Type, 1);
			if (Main.rand.Next(0, 3) == 0)
				player.QuickSpawnItem(Mod.Find<ModItem>("CorrodedShield").Type, 1);
			if (Main.rand.Next(0, 3) == 0)
			player.QuickSpawnItem(Mod.Find<ModItem>("AmberGlowSkull").Type, 1);
			if (Main.rand.Next(7) == 0)
			{
				player.QuickSpawnItem(ModContent.ItemType<SpiderQueenMask>());
			}
		}

	}
	public class CirnoBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 32;
			Item.height = 32;
			Item.expert = true;
			Item.rare = -12;
		}

		public override int BossBagNPC
		{
			get
			{
				return Mod.Find<ModNPC>("Cirno").Type;
			}
		}


		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
		player.TryGettingDevArmor();

			string[] dropitems = { "Starburster", "Snowfall", "IceScepter", "RubiedBlade", "IcicleFall", "RodOfTheMistyLake", "Magishield"};
			player.QuickSpawnItem(Mod.Find<ModItem>(dropitems[Main.rand.Next(dropitems.Length)]).Type);
			player.QuickSpawnItem(Mod.Find<ModItem>("CryostalBar").Type,Main.rand.Next(25, 40));
			player.QuickSpawnItem(Mod.Find<ModItem>("CirnoWings").Type, 1);
			if (Main.rand.Next(3) == 0)
				player.QuickSpawnItem(Mod.Find<ModItem>("GlacialStone").Type, 1);
			if (Main.rand.Next(7) == 0)
			{
				player.QuickSpawnItem(ModContent.ItemType<CirnoMask>());
			}
		}

}
	public class SPinkyBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Right click to open");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 32;
			Item.height = 32;
			Item.expert = true;
			Item.rare = -12;
		}

		public override int BossBagNPC
		{
			get
			{
				return Mod.Find<ModNPC>("SPinky").Type;
			}
		}

		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.TryGettingDevArmor();
				player.QuickSpawnItem(Mod.Find<ModItem>("LunarRoyalGel").Type, Main.rand.Next(40, 60));
			Armors.Illuminant.IlluminantHelmet.IlluminantArmorDrop(2, player.Center);
			//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("LunarRoyalGel"));
			player.QuickSpawnItem(Mod.Find<ModItem>("LunarSlimeHeart").Type);
			if (Main.rand.Next(7) == 0)
			{
				player.QuickSpawnItem(ModContent.ItemType<SupremePinkyMask>());
			}
		}
	}
}
