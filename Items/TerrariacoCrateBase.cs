using SGAmod.HavocGear.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace SGAmod.Items
{
	public class TerrariacoCrateBase : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terraria Co Supply Crate");
			//Tooltip.SetDefault("<right> for goodies!");
			Tooltip.SetDefault("<right> to open\nYou need a [i:" + Mod.Find<ModItem>("TerrariacoCrateKey") .Type+ "] to open this\nDo NOT use other keys, or else...");

		}

		public override string Texture
		{
			get { return ("SGAmod/Items/TerrariacoCrateBase"); }
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 34;
			Item.rare = 0;
			Item.maxStack = SGAWorld.downedCratrosity ? 30 : 1;
			Item.consumable = false;
		}
		//player.CountItem(mod.ItemType("ModItem"))

		public virtual void CrateLoot(Player ply)
		{
			string[] dropitems = { "CrateBossWeaponMagic", "CrateBossWeaponMelee", "CrateBossWeaponRanged", "CrateBossWeaponThrown", "CrateBossWeaponSummon" };
			ply.QuickSpawnItem(Mod.Find<ModItem>(dropitems[Main.rand.Next(0, dropitems.Length)]).Type, 1);

			if (Main.expertMode)
			{
				ply.QuickSpawnItem(Mod.Find<ModItem>("IdolOfMidas").Type, 1);
			}
			int[] typeofloot = { ItemID.GoldRing, ItemID.LuckyCoin, ItemID.DiscountCard };
			if (Main.rand.Next(0, 2) == 1)
				ply.QuickSpawnItem(Main.rand.NextBool() ? ModContent.ItemType<Placeable.TechPlaceable.AureateVaultItem>() : Mod.Find<ModItem>("TF2Emblem").Type, 1);
			if (Main.rand.Next(0, 3) == 1)
				ply.QuickSpawnItem(typeofloot[Main.rand.Next(0, typeofloot.Length)], 1);
		}

        public override bool OnPickup(Player player)
        {
			Item.maxStack = SGAWorld.downedCratrosity ? 30 : 1;
			return true;
        }

        public override bool CanRightClick()
		{
			Player ply = Main.LocalPlayer;
			bool canclick = (ply.CountItem(ItemID.GoldenKey) > 0 || ply.CountItem(ItemID.LightKey) > 0 || ply.CountItem(ItemID.NightKey) > 0 || ply.CountItem(ModContent.ItemType<SwampChestKey>()) > 0 || ply.CountItem(ItemID.TempleKey) > 0 || ply.CountItem(ItemID.ShadowKey) > 0);

			return ((canclick) || ply.CountItem(Mod.Find<ModItem>("TerrariacoCrateKey").Type) > 0);
		}

		public override void RightClick(Player ply)
		{

			bool usedwrongkey = (ply.CountItem(ItemID.GoldenKey) > 0 || ply.CountItem(ItemID.LightKey) > 0 || ply.CountItem(ItemID.NightKey) > 0 || ply.CountItem(ModContent.ItemType<SwampChestKey>()) > 0);
			bool usedrightkey = (ply.CountItem(Mod.Find<ModItem>("TerrariacoCrateKey").Type) > 0);
			int whatkey = 0;
			if (usedrightkey == true)
			{
				CrateLoot(ply);
				whatkey = Mod.Find<ModItem>("TerrariacoCrateKey").Type;
				goto endhere;
			}
			if (usedwrongkey == true)
			{

				if (ply.CountItem(ItemID.GoldenKey) > 0) { whatkey = ItemID.GoldenKey; }
				if (ply.CountItem(ItemID.LightKey) > 0) { whatkey = ItemID.LightKey; }
				if (ply.CountItem(ItemID.NightKey) > 0) { whatkey = ItemID.NightKey; }
				if (ply.CountItem(ModContent.ItemType<SwampChestKey>()) > 0) { whatkey = ModContent.ItemType<SwampChestKey>(); }
				if (!NPC.AnyNPCs(Mod.Find<ModNPC>("Cratrosity").Type))
				{

					if (Main.netMode > 0)
					{
						Mod.Logger.Debug("Crate: Net Spawn");
						ModPacket packet = Mod.GetPacket();
						packet.Write((ushort)75);
						packet.Write(Mod.Find<ModNPC>(whatkey == ModContent.ItemType<SwampChestKey>() ? "CratrosityDank" : (whatkey == ItemID.LightKey ? "CratrosityLight" : (whatkey == ItemID.NightKey ? "CratrosityNight" : "Cratrosity"))).Type);
						packet.Write(-9999);
						packet.Write(-9999);
						packet.Write(ply.whoAmI);
						packet.Send();
						//packet.Send(-1, ply.whoAmI);
					}
					else
					{
						Mod.Logger.Debug("Crate: SP Spawn");
						NPC.SpawnOnPlayer(ply.whoAmI, Mod.Find<ModNPC>(whatkey == ModContent.ItemType<SwampChestKey>() ? "CratrosityDank" : (whatkey == ItemID.LightKey ? "CratrosityLight" : (whatkey == ItemID.NightKey ? "CratrosityNight" : "Cratrosity"))).Type);

					}
					SoundEngine.PlaySound(15, (int)ply.position.X, (int)ply.position.Y, 0);
				}
			}
			endhere:
			ply.ConsumeItem(whatkey);
			//player.QuickSpawnItem(ItemID.LifeCrystal, 15);
			//player.QuickSpawnItem(ItemID.LifeFruit, 20);
			//player.QuickSpawnItem(ItemID.ManaCrystal, 9);
			//player.QuickSpawnItem(ItemID.AnkhShield);
			//player.QuickSpawnItem(ItemID.FrostsparkBoots);
			//player.QuickSpawnItem(ItemID.LavaWaders);
			//player.QuickSpawnItem(ItemID.CellPhone);
			//player.QuickSpawnItem(ItemID.SuspiciousLookingTentacle);
		}
	}
}