using SGAmod.Buffs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
    public class RustworkBlade : ModItem,IRustBurnText
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rustwork Blade");
            Tooltip.SetDefault("Applies Rustburn on hit\nThe Debuff duration scales up with the weapon's damage");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();

            Item.damage = 5;
            Item.width = 19;
            Item.height = 22;
            Item.DamageType = DamageClass.Melee;
            Item.rare = 2;
            Item.useStyle = 1;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.knockBack = 3;
            Item.value = 5000;
            Item.consumable = false;
            Item.UseSound = SoundID.Item19;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            RustBurn.ApplyRust(target, (2 + damage) * 80);
        }

    }
}
