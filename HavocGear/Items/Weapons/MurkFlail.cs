using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
    public class MurkFlail : ModItem,IDankSlowText
    {
        public override void SetDefaults()
        {

            Item.width = 30;
            Item.height = 10;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = 3;
            Item.noMelee = true; 
            Item.useStyle = 5;
            Item.useAnimation = 20; 
            Item.useTime = 44;
            Item.knockBack = 8f;
            Item.damage = 35;
            Item.scale = 2f;
            Item.noUseGraphic = true; 
            Item.shoot = Mod.Find<ModProjectile>("MurkFlailBall").Type;
            Item.shootSpeed = 15.1f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
        }

    public override void SetStaticDefaults()
    {
            DisplayName.SetDefault("Mudrock Crasher");
            Tooltip.SetDefault("Impacts against walls at high enough speeds unleash several dank blasts which inflict Dank Slow");
        }

    }

}
