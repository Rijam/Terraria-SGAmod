using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class ForagersBlade : ModItem
	{
        public override void SetDefaults()
		{
			base.SetDefaults();

            Item.damage = 14;
            Item.width = 32;
			Item.height = 32;
            Item.DamageType = DamageClass.Melee;
            Item.useTurn = true;
            Item.rare = 0;
            Item.useStyle = 1;
            Item.useAnimation = 24;
           	Item.knockBack = 5;
            Item.useTime = 20;
            Item.consumable = false;
            Item.UseSound = SoundID.Item1;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Forager's Blade");
            Tooltip.SetDefault("Dealing the killing blow will skin Leather off organic enemies");
        }

    }
}   
