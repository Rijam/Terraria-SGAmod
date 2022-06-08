using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons 
{
    public class TidalWave : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tidal Wave");
            Tooltip.SetDefault("Shoots a short range water ball\nThe water balls do not expire in water and crit wet enemies");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.damage = 15;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 30;
            Item.useStyle = 5;
            Item.useTime = 30;
            Item.knockBack = 7f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.maxStack = 1;
            Item.value = 800;
            Item.rare = 1;
            Item.shoot = Mod.Find<ModProjectile>("TidalWaveProj").Type;
            Item.shootSpeed = 9f;
        }
    }
}