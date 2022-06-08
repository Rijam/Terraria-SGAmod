using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items
{
	public class TerrariacoCrateKey: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terraria Co Supply Crate Key");
			Tooltip.SetDefault("Use this to open a Terraria Co Supply Crate");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 24;
			Item.height = 24;
			Item.value = 0;
			Item.rare = 10;
		}
	}

	public class TerrariacoCrateKeyUber : TerrariacoCrateKey
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terraria Co Supply Crate Key?");
			Tooltip.SetDefault("Something is very strange about this key...\nMaybe try using it on that strange crate?");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/TerrariacoCrateKey"); }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.DiscoColor;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 24;
			Item.height = 24;
			Item.value = 0;
			Item.rare = 10;
			Item.expert = true;
		}
	}

}
