using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;


namespace SGAmod.Items.Armors.Vanity
{

	[AutoloadEquip(EquipType.Head)]
	public class MasterfullyCraftedHatOfTheDragonGods : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Masterfully Crafted Hat Of The Dragon Gods");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 10);
			Item.rare = 6;
			Item.vanity = true;
			Item.defense = 0;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 5f) % 1f, 0.45f, 0.65f);
			tooltips.Add(new TooltipLine(Mod, "Dedicated", Idglib.ColorText(c, "Dedicated to a stupid Heroforge meme")));
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class AncientUnmanedHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unmaned Hood");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = 4;
			Item.vanity = true;
			Item.defense = 0;
		}
		public override bool DrawHead()
		{
			return GetType() != typeof(AncientSpaceDiverHelmet) && GetType() != typeof(AncientUnmanedHood);
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 5f) % 1f, 0.45f, 0.65f);
			tooltips.Add(new TooltipLine(Mod, "Dedicated", Idglib.ColorText(c, "Dedicated to PhilBill44, and preserving his work")));
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class AncientUnmanedBreastplate : AncientUnmanedHood
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unmaned Breastplate");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = 4;
			Item.vanity = true;
			Item.defense = 0;
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class AncientUnmanedLeggings : AncientUnmanedHood
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unmaned Leggings");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = 4;
			Item.vanity = true;
			Item.defense = 0;
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class AncientSpaceDiverHelmet : AncientUnmanedHood
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Space Diver Helm");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = 6;
			Item.vanity = true;
			Item.defense = 0;
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			if (GetType() != typeof(AncientSpaceDiverLeggings))
			{
				SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
				if (!Main.dedServ)
					sgaplayer.armorglowmasks[0] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
			}
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class AncientSpaceDiverChestplate : AncientSpaceDiverHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Space Diver Chestplate");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = 6;
			Item.vanity = true;
			Item.defense = 0;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
				sgaplayer.armorglowmasks[1] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class AncientSpaceDiverLeggings : AncientSpaceDiverHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Space Diver Leggings");
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = 6;
			Item.vanity = true;
			Item.defense=0;
		}
	}


}