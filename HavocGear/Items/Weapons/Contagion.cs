using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons 
{
    public class Contagion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Contagion");
            Tooltip.SetDefault("Shoots a piercing wave of acid");
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.damage = 40;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useStyle = 5;
            Item.useTime = 20;
            Item.knockBack = 3f;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            Item.maxStack = 1;
            Item.value = 100000;
            Item.rare = 6;
            Item.shoot = Mod.Find<ModProjectile>("ContagionProj").Type;
            Item.shootSpeed = 11f;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(null, "Mossthorn", 1).AddIngredient(null, "TidalWave", 1).AddIngredient(null, "VialofAcid", 12).AddIngredient(mod.ItemType("VirulentBar"), 10).AddTile(TileID.MythrilAnvil).Register();
        }
    }
}