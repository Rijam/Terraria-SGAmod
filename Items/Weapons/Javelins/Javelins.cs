using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Terraria.Utilities;
using Idglibrary;
using Idglibrary.Bases;
using Terraria.Audio;


namespace SGAmod.Items.Weapons.Javelins
{
    public class CrimsonCatastrophe : SanguineBident, IJablinItem
    {

        public override float Stabspeed => 10f;
        public override float Throwspeed => 25f;
        public override int Penetrate => 8;
        public override float Speartype => 10;
        public override int[] Usetimes => new int[] { 25, 15 };
        public override string[] Normaltext => new string[] { "A twisted form of thrown blood lust that explodes your foe's blood out from their wounds", "Throws 3 Jab-lins that inflict area damage against foes that are Massively Bleeding", "In Addition, crits against enemies afflicted with Everlasting Suffering", "On proc the Bleeding and Everlasting Suffering are removed, with a delay before retrigger", "Doesn't effect the enemy the Jab-lin is stuck to","50% bonus damage against bleeding immune enemies", "Primary Fire flies far and fast, and inflicts Massive Bleeding", "Is considered a Jab-lin, but non consumable and able to have prefixes" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Catastrophe");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 320;
            Item.width = 32;
            Item.crit = 10;
            Item.height = 32;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 50);
            Item.rare = ItemRarityID.Red;
            Item.consumable = false;
            Item.maxStack = 1;
            Item.UseSound = SoundID.Item1;
        }

        public static void BloodyExplosion(NPC enemy, Projectile projectile)
        {
            if (enemy.GetGlobalNPC<SGAnpcs>().crimsonCatastrophe > 0)
                return;

            int bleed = ModContent.BuffType<Buffs.MassiveBleeding>();
            int els = ModContent.BuffType <Buffs.EverlastingSuffering>();
            float damazz = (Main.DamageVar((float)projectile.damage * 2f));
            bool crit = false;

            int buffindex = enemy.FindBuffIndex(bleed);
            if (buffindex >= 0)
                enemy.DelBuff(buffindex);

            if (enemy.HasBuff(els))
            {
                buffindex = enemy.FindBuffIndex(els);
                crit = true;
                enemy.DelBuff(buffindex);
            }

            projectile.localAI[0] = 20;
            enemy.GetGlobalNPC<SGAnpcs>().crimsonCatastrophe = 60;

            for (int num315 = -40; num315 < 43; num315 = num315 + (crit ? 1 : 3))
            {
                int dustType = DustID.LunarOre;//Main.rand.Next(139, 143);
                int dustIndex = Dust.NewDust(enemy.Center + new Vector2(-16, -16) + ((Main.rand.NextFloat(0, MathHelper.TwoPi)).ToRotationVector2() * num315), 32, 32, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = projectile.velocity.X * (num315 * 0.02f);
                dust.velocity.Y = projectile.velocity.Y * (num315 * 0.02f);
                dust.velocity.RotateRandom(Math.PI / 2.0);
                dust.scale *= 1f + Main.rand.NextFloat(0.2f, 0.5f) / (1f + (num315 / 15f)) + (crit ? 1f : 0f);
                dust.fadeIn = 0.25f;
                dust.noGravity = true;
                Color mycolor = crit ? Color.CornflowerBlue : Color.OrangeRed;
                dust.color = mycolor;
                dust.alpha = 20;
            }

            enemy.StrikeNPC((int)damazz, 0f, 0, crit, true, true);

            if (Main.netMode != 0)
            {
                NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, enemy.whoAmI, damazz, 16f, (float)1, 0, 0, 0);
            }

        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(mod.ItemType("SanguineBident"), 1).AddIngredient(mod.ItemType("TerraTrident"), 1).AddIngredient(ItemID.DayBreak, 1).AddIngredient(mod.ItemType("StygianCore"), 2).AddIngredient(mod.ItemType("LunarRoyalGel"), 15).AddTile(TileID.LunarCraftingStation).Register();
        }


    }

    public class TerraTrident : SanguineBident, IJablinItem
    {

        public override float Stabspeed => 3.6f;
        public override float Throwspeed => 10f;
        public override int Penetrate => 4;
        public override float Speartype => 9;
        public override int[] Usetimes => new int[] { 25, 10 };
        public override string[] Normaltext => new string[] { "Jabs launch Terra Forks that do not cause immunity frames", "Weaker Terra Forks are unleashed through impaled targets", "These weaker forks pierce less times and travel short distances", "Is considered a Jab-lin, but non consumable and able to have prefixes" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Trident");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 42;
            Item.width = 32;
            Item.height = 32;
            Item.crit = 10;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.consumable = false;
            Item.maxStack = 1;
            Item.UseSound = SoundID.Item1;
        }
        public override void OnThrow(int type, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type2, ref int damage, ref float knockBack, JavelinProj madeproj)
        {
            //nothing
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(mod.ItemType("Contagion"), 1).AddIngredient(mod.ItemType("ThrownTrident"), 1).AddIngredient(ItemID.UnholyTrident, 1).AddIngredient(ItemID.Gungnir, 1).AddIngredient(mod.ItemType("ThermalJavelin"), 300).AddIngredient(mod.ItemType("OmniSoul"), 12).AddIngredient(ItemID.BrokenHeroSword, 1).AddTile(TileID.MythrilAnvil).Register();
        }

    }

    public class TerraTridentProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Trident");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.penetrate = 5;
            Projectile.alpha = 120;
            Projectile.timeLeft = 500;
            Projectile.light = 0.75f;
            Projectile.extraUpdates = 2;
            Projectile.localNPCHitCooldown = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.ignoreWater = true;
        }
        public override bool PreKill(int timeLeft)
        {
            for (int i = 0; i < 25; i += 1)
            {
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.AncientLight, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, default(Color), 1.5f);
                Main.dust[dustIndex].velocity += Projectile.velocity * 0.3f;
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].color = Color.Lime;
            }
            SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 10, 1f, 0.25f);
            return true;
        }
        public override void AI()
        {
            Projectile.rotation = ((float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f) - MathHelper.ToRadians(45);

            if (Main.rand.Next(1) == 0)
            {
                float velmul = Main.rand.NextFloat(0.1f, 0.25f);
                int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.AncientLight, Projectile.velocity.X * velmul, Projectile.velocity.Y * velmul, 200, default(Color), 0.7f);
                Main.dust[dustIndex].velocity += Projectile.velocity * 0.3f;
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].color = Color.Lime;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[Projectile.type].Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[Projectile.type], drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }

    public class SanguineBident : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 3.6f;
        public override float Throwspeed => 10f;
        public override int Penetrate => 3;
        public override float Speartype => 8;
        public override int[] Usetimes => new int[] { 25, 10 };
        public override string[] Normaltext => new string[] { "Launch 3 projectiles on throw at foes", "Impaled targets may leach life, more likely to leach from bleeding targets", "Melee strikes will make enemies bleed", "25% bonus damage against bleeding immune enemies", "Is considered a Jab-lin, but non consumable and able to have prefixes" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sanguine Bident");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 50;
            Item.width = 32;
            Item.height = 32;
            Item.knockBack = 5;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = 7;
            Item.consumable = false;
            Item.maxStack = 1;
        }
        public override void OnThrow(int type, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type2, ref int damage, ref float knockBack, JavelinProj madeproj)
        {
            if (type == 1)
            {
                Vector2 normalizedspeed = new Vector2(speedX, speedY);
                normalizedspeed.Normalize();
                normalizedspeed *= (Throwspeed * player.Throwing().thrownVelocity);
                normalizedspeed.Y -= Math.Abs(normalizedspeed.Y * 0.1f);
                if (player.altFunctionUse == 2)
                {
                    for (int i = -15; i < 16; i += 30)
                    {
                        Vector2 perturbedSpeed = ((new Vector2(normalizedspeed.X, normalizedspeed.Y)).RotatedBy(MathHelper.ToRadians(i))).RotatedByRandom(MathHelper.ToRadians(10)) * 0.85f;
                        float scale = 1f - (Main.rand.NextFloat() * .01f);
                        perturbedSpeed = perturbedSpeed * scale;
                        type2 = Mod.Find<ModProjectile>("JavelinProj").Type;

                        int thisoneddddd = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type2, damage, knockBack, Main.myPlayer);
                        Main.projectile[thisoneddddd].ai[1] = Speartype;
                        // Main.projectile[thisoneddddd].melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
                        Main.projectile[thisoneddddd].Throwing().DamageType = DamageClass.Throwing;

                        if (Speartype == (int)JavelinType.CrimsonCatastrophe)
                            Main.projectile[thisoneddddd].aiStyle = (-100 + i);


                       (Main.projectile[thisoneddddd].ModProjectile as JavelinProj).maxstick = madeproj.maxstick;
                        (Main.projectile[thisoneddddd].ModProjectile as JavelinProj).maxStickTime = madeproj.maxStickTime;
                        Main.projectile[thisoneddddd].penetrate = madeproj.Projectile.penetrate;
                        Main.projectile[thisoneddddd].netUpdate = true;
                        IdgProjectile.Sync(thisoneddddd);

                    }
                }

            }
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            switch (rand.Next(16))
            {
                case 1:
                    return PrefixID.Demonic;
                case 2:
                    return PrefixID.Frenzying;
                case 3:
                    return PrefixID.Dangerous;
                case 4:
                    return PrefixID.Savage;
                case 5:
                    return PrefixID.Furious;
                case 6:
                    return PrefixID.Terrible;
                case 7:
                    return PrefixID.Awful;
                case 8:
                    return PrefixID.Dull;
                case 9:
                    return PrefixID.Unhappy;
                case 10:
                    return PrefixID.Unreal;
                case 11:
                    return PrefixID.Shameful;
                case 12:
                    return PrefixID.Heavy;
                case 13:
                    return PrefixID.Zealous;
                case 14:
                    return Mod.Find<ModPrefix>("Tossable").Type;
                case 15:
                    return Mod.Find<ModPrefix>("Impacting").Type;
                default:
                    return Mod.Find<ModPrefix>("Olympian").Type;
            }
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Trident, 1).AddIngredient(mod.ItemType("CrimsonJavelin"), 150).AddIngredient(ItemID.Vertebrae, 10).AddIngredient(ItemID.Ectoplasm, 8).AddIngredient(mod.ItemType("StygianCore"), 1).AddTile(TileID.MythrilAnvil).Register();
        }

    }

    public class SwampSovnya: SanguineBident, IJablinItem,IDankSlowText
    {
        public override float Stabspeed => 4.00f;
        public override float Throwspeed => 10f;
        public override int Penetrate => 5;
        public override float Speartype => 12;
        public override int[] Usetimes => new int[] { 25, 6 };
        public override string[] Normaltext => new string[] {"'Hunt or be hunted'", "Thrown Jab-libs inflict Dank Slow", "Jabs crit slowed targets and remove the debuff, increasing damage based on slow up to 5X","Additionally, this weapon does 25% increased direct and DOT damage to poison-immune enemies", "Is considered a Jab-lin, but non consumable and able to have prefixes" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swamp Sovnya");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 60;
            Item.width = 32;
            Item.height = 32;
            Item.crit = 0;
            Item.knockBack = 3;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Lime;
            Item.consumable = false;
            Item.maxStack = 1;
            Item.UseSound = SoundID.Item1;
        }
        public override void OnThrow(int type, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type2, ref int damage, ref float knockBack, JavelinProj madeproj)
        {
            //nothing
        }
        public override void AddRecipes()
        {
            //nothing
        }

    }

    public class ThermalJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 4.00f;
        public override float Throwspeed => 14f;
        public override int Penetrate => 5;
        public override float Speartype => 11;
        public override int[] Usetimes => new int[] { 20, 6 };
        public override string[] Normaltext => new string[] { "Applies Thermal Blaze to your enemies"};
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thermal Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 56;
            Item.width = 24;
            Item.height = 24;
            Item.knockBack = 4;
            Item.value = 50;
            Item.rare = 5;
        }
        public override void AddRecipes()
        {
            CreateRecipe(100).AddIngredient(mod.ItemType("UnmanedBar"), 2).AddIngredient(mod.ItemType("FieryShard"), 1).AddTile(TileID.MythrilAnvil).Register();
        }

    }

    public class ShadowJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 1.70f;
        public override float Throwspeed => 1f;
        public override int Penetrate => 5;
        public override float Speartype => 7;
        public override int[] Usetimes => new int[] { 25, 7 };
        public override string[] Normaltext => new string[] { "Made from evil Jab-lins and the dark essence emitted by a shadow key, attacks may inflict Shadowflame","The Shadow Key is NOT consumed on craft!", "Javelins accelerates forward, is not affected by gravity until it hits a target" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 32;
            Item.width = 24;
            Item.height = 24;
            Item.knockBack = 4;
            Item.value = 40;
            Item.rare = 4;
        }
        public override void AddRecipes()
        {
            CreateRecipe(50).AddIngredient(ItemID.ShadowKey, 1).AddRecipeGroup("SGAmod:EvilJavelins", 50).AddTile(TileID.WorkBenches).Register();
        }

    }

    public class PearlWoodJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 3.00f;
        public override float Throwspeed => 13f;
        public override int Penetrate => 5;
        public override float Speartype => 6;
        public override int[] Usetimes => new int[] { 20, 8 };
        public override string[] Normaltext => new string[] { "The Hallow's wrath makes stars fall down on jabbed or impaled targets","Stars scale damage with your Damage over Time boosts" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PearlWood Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 36;
            Item.width = 24;
            Item.height = 24;
            Item.knockBack = 4;
            Item.value = 50;
            Item.rare = 5;
        }
        public override void AddRecipes()
        {
            CreateRecipe(200).AddIngredient(ItemID.Pearlwood, 10).AddIngredient(ItemID.CrystalShard, 3).AddIngredient(ItemID.UnicornHorn, 1).AddTile(TileID.WorkBenches).Register();
        }

    }


    public class DynastyJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 1.70f;
        public override float Throwspeed => 10f;
        public override int Penetrate => 4;
        public override float Speartype => 5;
        public override int[] Usetimes => new int[] { 35, 12 };
        public override string[] Normaltext => new string[] { "The Battle calls!", "Summons extra Dynasty Javelins to fall from the sky when they damage an enemy" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dynasty Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 28;
            Item.width = 24;
            Item.height = 24;
            Item.knockBack = 4;
            Item.value = 30;
            Item.rare = 3;
        }
        public override void AddRecipes()
        {
            //null
        }
    }

    public class AmberJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 1.70f;
        public override float Throwspeed => 10f;
        public override int Penetrate => 8;
        public override float Speartype => 4;
        public override int[] Usetimes => new int[] { 25, 12 };
        public override string[] Normaltext => new string[] { "Made from sandy materials, Sticks into targets for longer" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Amber Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 20;
            Item.width = 24;
            Item.height = 24;
            Item.knockBack = 4;
            Item.value = 30;
            Item.rare = 3;
        }
        public override void AddRecipes()
        {
            CreateRecipe(250).AddIngredient(ItemID.PalmWood, 10).AddIngredient(ItemID.FossilOre, 2).AddIngredient(ItemID.Amber, 1).AddTile(TileID.WorkBenches).Register();
        }

    }

        public class CorruptionJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 1.50f;
        public override float Throwspeed => 9f;
        public override float Speartype => 3;
        public override int Penetrate => 4;
        public override int[] Usetimes => new int[] { 30, 10 };
        public override string[] Normaltext => new string[] { "Made from corrupt materials" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corruption Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 15;
            Item.width = 24;
            Item.height = 24;
            Item.knockBack = 4;
            Item.value = 25;
            Item.rare = 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe(150).AddIngredient(ItemID.Ebonwood, 5).AddIngredient(ItemID.EbonstoneBlock, 10).AddIngredient(ItemID.DemoniteBar, 1).AddTile(TileID.WorkBenches).Register();
        }

    }

    public class CrimsonJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 1.20f;
        public override float Throwspeed => 8f;
        public override float Speartype => 2;
        public override int Penetrate => 4;
        public override int[] Usetimes => new int[] { 40, 15 };
        public override string[] Normaltext => new string[] { "Made from bloody materials" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 20;
            Item.width = 24;
            Item.height = 24;
            Item.knockBack = 4;
            Item.value = 25;
            Item.rare = 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe(150).AddIngredient(ItemID.Shadewood, 5).AddIngredient(ItemID.CrimstoneBlock, 10).AddIngredient(ItemID.CrimtaneBar, 1).AddTile(TileID.WorkBenches).Register();
        }

    }

    public class IceJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 1.20f;
        public override float Throwspeed => 6f;
        public override float Speartype => 1;
        public override int[] Usetimes => new int[] { 40, 15 };
        public override string[] Normaltext => new string[] { "Made from cold materials, attacks may inflict Frostburn" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 14;
            Item.width = 24;
            Item.height = 24;
            Item.knockBack = 5;
            Item.value = 15;
            Item.rare = 2;
        }
        public override void AddRecipes()
        {
            CreateRecipe(150).AddIngredient(ItemID.BorealWood, 5).AddIngredient(ItemID.IceBlock, 10).AddIngredient(mod.ItemType("FrigidShard"), 1).AddTile(TileID.WorkBenches).Register();
        }

    }


}
