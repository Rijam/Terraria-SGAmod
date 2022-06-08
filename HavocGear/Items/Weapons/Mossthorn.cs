using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class Mossthorn : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mossthorn");
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 48;
			Item.damage = 30;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.useTurn = false;
			Item.noUseGraphic = true;
			Item.useAnimation = 10;
			Item.useStyle = 5;
			Item.useTime = 11;
			Item.knockBack = 4.5f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = 3;
			Item.shoot = Mod.Find<ModProjectile>("MossthornProj").Type;
			Item.shootSpeed = 4.5f;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
	}

}