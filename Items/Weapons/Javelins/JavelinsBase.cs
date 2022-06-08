using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Idglibrary;
using Idglibrary.Bases;

//using CalamityMod.Dusts;
using SGAmod.Buffs;
using Terraria.Audio;
using Terraria.DataStructures;

namespace SGAmod.Items.Weapons.Javelins
{
    public class JablinData
    {
        public string itemName;
        public int dropChance;

        public JablinData(string itemName,int dropChance)
        {
            this.itemName = itemName;
            this.dropChance = dropChance;
        }
    }

    public enum JavelinType : byte
    {
        Stone,
        Ice,
        Corruption,
        Crimson,
        Amber,
        Dynasty,
        Hallowed,
        Shadow,
        SanguineBident,
        TerraTrident,
        CrimsonCatastrophe,
        Thermal,
        SwampSovnya
    }

    public class StoneJavelin : ModItem, IJablinItem
    {

        public static List<JablinData> jablinData = new List<JablinData>();

        //public delegate int PerformCalculation(int x, int y);
        //public Action<string> messageTarget;
        public Func<int, int, bool> testForEquality = (x, y) => x == y;
        public virtual int Penetrate => 3;
        public virtual int PierceTimer => 100;
        public virtual float Stabspeed => 1.20f;
        public virtual float Throwspeed => 6f;
        public virtual float Speartype => 0;
        public virtual int[] Usetimes => new int[] { 30,10};
        public virtual string[] Normaltext => new string[] { "It's a jab-lin made from stone" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stone Jab-lin");
            StoneJavelin.jablinData.Add(new JablinData("StoneJavelin", 8));
            StoneJavelin.jablinData.Add(new JablinData("IceJavelin", 8));
            StoneJavelin.jablinData.Add(new JablinData("CrimsonJavelin", 8));
            StoneJavelin.jablinData.Add(new JablinData("CorruptionJavelin", 8));
            StoneJavelin.jablinData.Add(new JablinData("AmberJavelin", 8));
            StoneJavelin.jablinData.Add(new JablinData("DynastyJavelin", 8));
            StoneJavelin.jablinData.Add(new JablinData("PearlWoodJavelin", 8));
            StoneJavelin.jablinData.Add(new JablinData("ShadowJavelin", 8));
            StoneJavelin.jablinData.Add(new JablinData("SanguineBident", 0));
            StoneJavelin.jablinData.Add(new JablinData("TerraTrident", 0));
            StoneJavelin.jablinData.Add(new JablinData("CrimsonCatastrophe", 0));
            StoneJavelin.jablinData.Add(new JablinData("ThermalJavelin", 8));
            StoneJavelin.jablinData.Add(new JablinData("SwampSovnya", 0));

            //Tooltip.SetDefault("Shoots a spread of bullets");
        }

        public override void AddRecipes()
        {
            CreateRecipe(100).AddRecipeGroup("Wood", 5).AddRecipeGroup("SGAmod:Stone", 10).AddTile(TileID.WorkBenches).Register();
            //messageTarget = delegate(string s) { bool thisisit = testForEquality(2,6); };


            //messageTarget("this");
        }

        public void drawstuff(SpriteBatch spriteBatch, Vector2 position, Color drawColor, Color itemColor, float scale, bool inventory = true)
        {
            Texture2D textureSpear = (Texture2D)ModContent.Request<Texture2D>(JavelinProj.tex[(int)Speartype] + "Spear");
            Texture2D textureJave = (Texture2D)ModContent.Request<Texture2D>(JavelinProj.tex[(int)Speartype]);
            Vector2 slotSize = new Vector2(52f, 52f);
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
            Vector2 textureOrigin = new Vector2(textureSpear.Width / 2, textureSpear.Height / 2);

            spriteBatch.Draw(textureSpear, drawPos - new Vector2(8, 8), null, drawColor, 0f, textureOrigin, inventory ? scale : Main.inventoryScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(textureJave, drawPos - new Vector2(8, 8), null, drawColor, 0f, textureOrigin, inventory ? scale : Main.inventoryScale, SpriteEffects.FlipHorizontally, 0f);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            drawstuff(spriteBatch,position,drawColor,itemColor,scale);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            drawstuff(spriteBatch, (Item.Center-Main.screenPosition) - new Vector2(8, 8), lightColor, lightColor, scale,true);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
            if (tt != null)
            {
                string[] thetext = tt.Text.Split(' ');
                string newline = "";
                List<string> valuez = new List<string>();
                foreach (string text2 in thetext)
                {
                    valuez.Add(text2 + " ");
                }
                valuez.Insert(1, "Melee/Throwing ");
                foreach (string text3 in valuez)
                {
                    newline += text3;
                }
                tt.Text = newline;
            }

            tt = tooltips.FirstOrDefault(x => x.Name == "CritChance" && x.Mod == "Terraria");
            if (tt != null)
            {
                string[] thetext = tt.Text.Split(' ');
                string newline = "";
                List<string> valuez = new List<string>();
                int counter = 0;
                foreach (string text2 in thetext)
                {
                    counter += 1;
                    if (counter>1)
                    valuez.Add(text2 + " ");
                }
                int thecrit = Main.GlobalTimeWrappedHourly % 3f >= 1.5f ? (int)Main.LocalPlayer.GetCritChance(DamageClass.Melee) : ThrowingUtils.DisplayedCritChance(Item);
                string thecrittype = Main.GlobalTimeWrappedHourly % 3f >= 1.5f ? "Melee " : "Throwing ";
                valuez.Insert(0, thecrit+"% "+ thecrittype);
                foreach (string text3 in valuez)
                {
                    newline += text3;
                }
                tt.Text = newline;
            }

            foreach ( string line in Normaltext){
            tooltips.Add(new TooltipLine(Mod, "JavaLine", line));
            }
            tooltips.Add(new TooltipLine(Mod, "JavaLine1", "Left click to quickly jab like a spear (melee damage done, may break after using if consumable)"));
            tooltips.Add(new TooltipLine(Mod, "JavaLine1", "Right click to more slowly throw (throwing damage done, benefits from throwing velocity)"));
            if (Item.consumable)
            tooltips.Add(new TooltipLine(Mod, "JavaLine1", "Melee attacks have a solid 50% chance to not be consumed"));
            tooltips.Add(new TooltipLine(Mod, "JavaLine1", "Thrown jab-lins stick into foes and do extra damage"));
            tooltips.Add(new TooltipLine(Mod, "JavaLine1", "benefits from Throwing item saving chance and melee attack speed"));
        }

        public override bool ConsumeItem(Player player)
        {
            if (player.altFunctionUse != 2 && Main.rand.Next(0, 100) < 50)
                return false;
            return TrapDamageItems.SavingChanceMethod(player,true);
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.DayBreak);
            Item.damage = 12;
            Item.width = 24;
            Item.height = 24;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.noMelee = true;
            // item.melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
            // item.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
            // item.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
            // item.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
            Item.knockBack = 5;
            Item.reuseDelay = 1;
            Item.value = 100;
            Item.consumable = true;
            Item.rare = 1;
            Item.maxStack = 999;
            Item.autoReuse = false;
            Item.shoot = Mod.Find<ModProjectile>("JavelinProj").Type;
            Item.shootSpeed = 1f;

        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage) //Changed to a single StatModifier
        {
            //damage *= (player.GetDamage(DamageClass.Throwing) + player.GetDamage(DamageClass.Melee)) / 2f;
            damage += player.GetModPlayer<SGAPlayer>().JavelinBaseBundle ? 0.10f : 0f;
            damage += player.GetModPlayer<SGAPlayer>().JavelinSpearHeadBundle ? 0.15f : 0f;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public virtual void OnThrow(int type, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type2, ref int damage, ref float knockBack, JavelinProj jav)
        {
//nullz
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = (int)((Usetimes[0] * player.GetAttackSpeed(DamageClass.Melee)) /player.GetModPlayer<SGAPlayer>().ThrowingSpeed);
                Item.useAnimation = (int)((Usetimes[0] * player.GetAttackSpeed(DamageClass.Melee)) / player.GetModPlayer<SGAPlayer>().ThrowingSpeed);
                Item.shootSpeed = 1f;

            }
            else
            {
                if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("JavelinProjMelee").Type] > 0)
                    return false;
                SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
                Item.autoReuse = sgaply.jabALot;
                float speedz = (player.GetAttackSpeed(DamageClass.Melee)) *(sgaply.jabALot ? 0.80f : 1f);
                Item.useTime = Math.Max(6,(int)((Usetimes[1] * speedz)));
                Item.useAnimation = Math.Max(6, (int)((Usetimes[1] * speedz)));
                Item.shootSpeed = (1f / player.GetAttackSpeed(DamageClass.Melee)) *sgaply.UseTimeMultiplier(this.Item);
            }
                return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 normalizedspeed = velocity;
            normalizedspeed.Normalize();
            bool melee = true;
            float basemul = Item.shootSpeed;
            if (player.altFunctionUse == 2)
            {
                normalizedspeed *= (Throwspeed*player.Throwing().thrownVelocity);
                normalizedspeed.Y -= Math.Abs(normalizedspeed.Y* 0.1f);
                Vector2 perturbedSpeed = new Vector2(normalizedspeed.X, normalizedspeed.Y).RotatedByRandom(MathHelper.ToRadians(10));
                float scale = 1f - (Main.rand.NextFloat() * .01f);
                perturbedSpeed = perturbedSpeed * scale;
                velocity.X = normalizedspeed.X; velocity.Y = normalizedspeed.Y;
                Item.useStyle = 1;
                type = Mod.Find<ModProjectile>("JavelinProj").Type;
                melee = false;
            }
           else
            {
                normalizedspeed *= Stabspeed* basemul;
                Vector2 perturbedSpeed = new Vector2(normalizedspeed.X, normalizedspeed.Y).RotatedByRandom(MathHelper.ToRadians(10));
                float scale = 1f - (Main.rand.NextFloat() * .01f);
                perturbedSpeed = perturbedSpeed * scale;
                Item.useStyle = 5;
                if (player.SGAPly().jabALot)
                    normalizedspeed = normalizedspeed.RotatedByRandom(MathHelper.Pi * 0.25f);

                velocity.X = normalizedspeed.X; velocity.Y = normalizedspeed.Y;
                type = Mod.Find<ModProjectile>("JavelinProjMelee").Type;
            }

            int thisoned = Projectile.NewProjectile(null, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, Main.myPlayer); //GetSource_FromThis()
            Main.projectile[thisoned].ai[1] = Speartype;
            Main.projectile[thisoned].DamageType = DamageClass.Melee;
            //Main.projectile[thisoned].Throwing().DamageType = !DamageClass.Melee;
            if (player.altFunctionUse == 2 && Speartype==7)
            Main.projectile[thisoned].aiStyle = 18;
            if (player.altFunctionUse == 2)
            {
                (Main.projectile[thisoned].ModProjectile as JavelinProj).maxstick += (player.GetModPlayer<SGAPlayer>().JavelinSpearHeadBundle ? 1 : 0);
                (Main.projectile[thisoned].ModProjectile as JavelinProj).maxStickTime = PierceTimer;
            }

            Main.projectile[thisoned].netUpdate = true;
            if (!melee)
            Main.projectile[thisoned].penetrate = Penetrate;
            IdgProjectile.Sync(thisoned);

            if (player.altFunctionUse == 2)
            {
                OnThrow(1, player, ref position, ref velocity.X, ref velocity.Y, ref type, ref damage, ref knockback, (Main.projectile[thisoned].ModProjectile as JavelinProj));
            }
            else
            {
                OnThrow(0, player, ref position, ref velocity.X, ref velocity.Y, ref type, ref damage, ref knockback, (Main.projectile[thisoned].ModProjectile as JavelinProj));
            }

                return false;

        }
    }

    public class JavelinProj : ModProjectile
    {
        public int stickin = -1;
        public Player P;
        public Vector2 offset;
        public int maxstick = 1;
        public int maxStickTime = 100;
        public int javelinType
        {
            get { return (int)Projectile.ai[1]; }
        }
        static public string[] tex =
    {"SGAmod/Items/Weapons/Javelins/StoneJavelin",
        "SGAmod/Items/Weapons/Javelins/IceJavelin",
        "SGAmod/Items/Weapons/Javelins/CrimsonJavelin",
        "SGAmod/Items/Weapons/Javelins/CorruptionJavelin",
        "SGAmod/Items/Weapons/Javelins/AmberJavelin",
        "SGAmod/Items/Weapons/Javelins/DynastyJavelin",
        "SGAmod/Items/Weapons/Javelins/PearlWoodJavelin",
        "SGAmod/Items/Weapons/Javelins/ShadowJavelin",
        "SGAmod/Items/Weapons/Javelins/SanguineBident",
         "SGAmod/Items/Weapons/Javelins/TerraTrident",
         "SGAmod/Items/Weapons/Javelins/CrimsonCatastrophe",
         "SGAmod/Items/Weapons/Javelins/ThermalJavelin",
         "SGAmod/Items/Weapons/Javelins/SwampSovnya",
       };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Javelin");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(stickin);
            writer.Write(maxstick);
            writer.Write(maxStickTime);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            stickin = reader.ReadInt32();
            maxstick = reader.ReadInt32();
            maxStickTime = reader.ReadInt32();
        }
        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            // projectile.melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.tileCollide = true;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 300;
            Projectile.light = 0.25f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
        }

        public override string Texture
        {
            get { return ("Terraria/Projectile_"+ ProjectileID.HallowStar); }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (stickin>-1)
                return false;
            return base.CanHitNPC(target);
        }

        public override void Kill(int timeLeft)
        {

            if (Projectile.owner == Main.myPlayer)
            {
                int dropChance = StoneJavelin.jablinData[(int)Projectile.ai[1]].dropChance;
                if (dropChance < 1)
                    return;
                // Drop a javelin item, 1 in 18 chance (~5.5% chance)
                //Main.NewText(StoneJavelin.jablinData[(int)projectile.ai[1]].itemName);
                int item =
                Main.rand.NextBool(dropChance)
                    ? Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.getRect(), Mod.Find<ModItem>(StoneJavelin.jablinData[(int)Projectile.ai[1]].itemName).Type)
                    : 0;

                // Sync the drop for multiplayer
                // Note the usage of Terraria.ID.MessageID, please use this!
                if (Main.netMode == NetmodeID.MultiplayerClient && item >= 0)
                {
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item, 1f);
                }
            }
        }

        public static void JavelinOnHit(NPC target, Projectile projectile, ref double damage)
        {
        if (projectile.ai[1] == (int)JavelinType.Ice)//Ice
            {
                if (Main.rand.Next(0,4)==1)
                target.AddBuff(BuffID.Frostburn, 60 * (projectile.type==ModContent.ProjectileType<JavelinProj>() ? 2 : 3));
            }
            if (projectile.ai[1] == (int)JavelinType.Dynasty)//Dynasty
            {
                if (projectile.penetrate > 1)
                {
                    int thisoned = Projectile.NewProjectile(null, projectile.Center.X+ Main.rand.NextFloat(-64, 64), projectile.Center.Y - 800, Main.rand.NextFloat(-2,2), 14f, projectile.type, projectile.damage, projectile.knockBack, Main.player[projectile.owner].whoAmI); //Projectile.GetSource_OnHit(target)
                    Main.projectile[thisoned].ai[1] = projectile.ai[1];
                    Main.projectile[thisoned].DamageType = DamageClass.Throwing;
                    Main.projectile[thisoned].penetrate = projectile.penetrate-1;
                    Main.projectile[thisoned].netUpdate = true;
                }
            }

            if (projectile.ai[1] == (int)JavelinType.Hallowed)//Hallow
            {
                if (Main.rand.Next(0, projectile.ModProjectile.GetType() == typeof(JavelinProjMelee) ? 2 : 0) == 0)
                {

                    int thisoned = Projectile.NewProjectile(null, projectile.Center.X + Main.rand.NextFloat(-64, 64), projectile.Center.Y - 800, Main.rand.NextFloat(-2, 2), 14f, ProjectileID.HallowStar, (int)(projectile.damage * damage), projectile.knockBack, projectile.owner);
                    Main.projectile[thisoned].DamageType = DamageClass.Throwing;
                    Main.projectile[thisoned].penetrate = 2;
                    Main.projectile[thisoned].netUpdate = true;
                    IdgProjectile.Sync(thisoned);
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, thisoned);

                }
            }
            if (projectile.ai[1] == (int)JavelinType.Shadow)//Shadow
            {
                if (Main.rand.Next(0, 4) == 1)
                    target.AddBuff(BuffID.ShadowFlame, 60 * (projectile.type == ModContent.ProjectileType<JavelinProj>() ? 3 : 5));
            }
            if (projectile.ai[1] == (int)JavelinType.SanguineBident)//Sanguine Bident
            {
                int bleed = ModContent.BuffType<MassiveBleeding>();
                if (target.buffImmune[BuffID.Bleeding])
                {
                    damage += 0.25;
                }

                if (projectile.ModProjectile.GetType() == typeof(JavelinProj))
                {
                    if (target.active && target.life > 0 && Main.rand.Next(0,12)<(target.HasBuff(bleed) || target.HasBuff(BuffID.Bleeding) ? 8 : 1))
                    {
                        projectile.vampireHeal((int)(projectile.damage / 2f), projectile.Center, target);
                    }
                }
                else
                {
                    target.AddBuff(ModContent.BuffType<MassiveBleeding>(), 60 * 5);
                }
                projectile.netUpdate = true;


            }
            if (projectile.ai[1] == (int)JavelinType.TerraTrident)//Terra Trident
            {
                if (projectile.ModProjectile.GetType() == typeof(JavelinProj))
                {
                    
                    SoundEngine.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 60, 0.6f, 0.25f);
                    Vector2 velo = projectile.velocity; velo.Normalize();
                    int prog = Projectile.NewProjectile(null, projectile.Center.X + (velo.X * 20f), projectile.Center.Y + (velo.Y * 20f), velo.X * 6f, velo.Y * 6f, SGAmod.Instance.Find<ModProjectile>("TerraTridentProj").Type, (int)(projectile.damage*0.75), projectile.knockBack / 2f, Main.myPlayer);
                    Main.projectile[prog].penetrate = 3;
                    Main.projectile[prog].timeLeft /= 4;
                    // Main.projectile[prog].melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
                    Main.projectile[prog].DamageType = DamageClass.Throwing;
                    Main.projectile[prog].netUpdate = true;
                    IdgProjectile.Sync(prog);
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, prog);
                }
            }
            if (projectile.ai[1] == (int)JavelinType.CrimsonCatastrophe)//Crimson Catastrophe
            {
                int bleed = ModContent.BuffType<MassiveBleeding>();
                if (target.buffImmune[BuffID.Bleeding])
                {
                    damage += 0.50;
                }

                ModProjectile modproj = projectile.ModProjectile;
                if (modproj.GetType() == typeof(JavelinProj))
                {
                    (modproj as JavelinProj).maxStickTime = (int)((modproj as JavelinProj).maxStickTime/1.25);
                    foreach (NPC enemy in Main.npc.Where(enemy => enemy.active && enemy.active && enemy.life > 0 && !enemy.friendly && !enemy.dontTakeDamage && enemy.Distance(projectile.Center)<500 && (modproj as JavelinProj).stickin!= enemy.whoAmI && enemy.HasBuff(bleed)))
                    {
                        CrimsonCatastrophe.BloodyExplosion(enemy, projectile);
                    }
                }
                else
                {
                    target.AddBuff(bleed, 60 * 5);
                }
                projectile.netUpdate = true;


            }
            if (projectile.ai[1] == (int)JavelinType.Thermal)//Thermal
            {
                target.AddBuff(ModContent.BuffType<ThermalBlaze>(), 60 * (projectile.type == ModContent.ProjectileType<JavelinProj>() ? 2 : 5));
            }
            if (projectile.ai[1] == (int)JavelinType.SwampSovnya)//Swamp Sovnya
            {
                if (!target.buffImmune[BuffID.Poisoned])
                {
                    ModProjectile modproj = projectile.ModProjectile;
                    if (modproj.GetType() == typeof(JavelinProj))
                    {
                        if (Main.rand.Next(0, 100) < 50 && !target.boss)
                            target.AddBuff(ModContent.BuffType<DankSlow>(), (int)(60 * 3f));
                    }
                }
                else
                {
                    damage += 0.25f;
                }
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Projectile.ai[1] == (int)JavelinType.SwampSovnya)//Swamp Sovnya
            {
                if (target.buffImmune[BuffID.Poisoned])
                {
                    damage = (int)(damage * 1.25f);
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.aiStyle = -1;
            double mul = 1.00;
            JavelinProj.JavelinOnHit(target,Projectile,ref mul);
            damage = (int)(damage * mul);

            int foundsticker = 0;

            for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
            {
                Projectile currentProjectile = Main.projectile[i];
                if (i != Projectile.whoAmI // Make sure the looped projectile is not the current javelin
                    && currentProjectile.active // Make sure the projectile is active
                    && currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
                    && currentProjectile.type == Projectile.type // Make sure the projectile is of the same type as this javelin
                    && currentProjectile.ModProjectile is JavelinProj JavelinProjz // Use a pattern match cast so we can access the projectile like an ExampleJavelinProjectile
                    && JavelinProjz.stickin == target.whoAmI)
                {
                    foundsticker += 1;
                   //projectile.Kill();
                }

            }

            if (foundsticker<maxstick)
            {

                if (Projectile.penetrate > 1)
                {
                    offset = (target.Center - Projectile.Center);
                    stickin = target.whoAmI;
                    Projectile.netUpdate = true;
                }
            }

        }

        public override void AI()
        {
            Projectile.localAI[0] -= 1f;
            float facingleft = Projectile.velocity.X > 0 ? 1f : -1f;
            Projectile.rotation = ((float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f) +(float)((Math.PI)*-0.25f);

            if (Projectile.ignoreWater==true)
            {
                Projectile.ignoreWater = false;
                if (Main.dedServ)
                {
                    Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(JavelinProj.tex[(int)Projectile.ai[1]]);
                    int xx = (texture.Width - (int)(Projectile.width)) / 2;
                    int yy = (texture.Height - (int)(Projectile.height)) / 2;
                    Projectile.position.X -= (xx / 2);
                    Projectile.position.Y -= (yy / 2);
                    Projectile.width = texture.Width / 2;
                    Projectile.height = texture.Height / 2;
                }
            }

            if (stickin > -1)
            {
                NPC himz = Main.npc[stickin];
                Projectile.tileCollide = false;

                if ((himz != null && himz.active) && Projectile.penetrate>0)
                {
                    Projectile.Center = (himz.Center - offset) - (Projectile.velocity * 0.2f);
                    if (Projectile.timeLeft < 100)
                    {
                        Projectile.penetrate -= 1;
                        Projectile.timeLeft = 100+maxStickTime;
                        double damageperc = 0.75;

                        if (Main.player[Projectile.owner].GetModPlayer<SGAPlayer>().JavelinBaseBundle)
                            damageperc += 0.25;

                        JavelinProj.JavelinOnHit(himz,Projectile,ref damageperc);

                        int theDamage = (int)(Projectile.damage * damageperc);
                        himz.StrikeNPC(theDamage, 0,1);
                        Main.player[Projectile.owner].addDPS(theDamage);

                        if (Main.netMode != NetmodeID.SinglePlayer)
                        {
                            NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, himz.whoAmI, (float)(Projectile.damage * damageperc), 16f, (float)1, 0, 0, 0);//Net message changed. Idk if the paramters are changed
                        }
                    }
                }
                else
                {
                    Projectile.Kill();
                }

            }
            else
            {

                if (Projectile.ai[1] == (int)JavelinType.CrimsonCatastrophe)//Sanguine Bident
                {
                    if (stickin < 0)
                    {
                        for (float i = 2f; i > 0.25f; i -= Main.rand.NextFloat(0.25f,1.25f))
                        {
                            Vector2 position = Projectile.position + Vector2.Normalize(Projectile.velocity)*Main.rand.NextFloat(0f,48f);
                            position -= new Vector2(Projectile.width, Projectile.height) / 2f;
                            Dust dust = Dust.NewDustDirect(position, Projectile.height, Projectile.width, DustID.Torch,0, 0, 200, Scale: 0.5f);
                            dust.velocity = new Vector2((Projectile.velocity.X / 3f) * i, Projectile.velocity.Y * i);

                            dust = Dust.NewDustDirect(position, Projectile.height, Projectile.width, DustID.Blood, 0, 0, 200, Scale: 0.5f);
                            dust.velocity = new Vector2(Projectile.velocity.X * i, (Projectile.velocity.Y / 3f) * i);
                        }
                    }

                }
                else
                {
                    if (Main.rand.NextBool(3))
                    {
                        Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustID.LunarOre,
                        Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 200, Scale: 0.5f);
                        dust.velocity += Projectile.velocity * 0.3f;
                        dust.velocity *= 0.2f;
                    }
                }

                if (Projectile.ai[1] == (int)JavelinType.CrimsonCatastrophe)
                {
                    if (Projectile.aiStyle != 0)
                    {
                        float rotvalue = Projectile.timeLeft/300f;
                        Projectile.velocity = Projectile.velocity.RotatedBy(((Projectile.aiStyle + 100f) / 1200f) * -rotvalue);
                    }
                    return;
                }

                if (Projectile.velocity.Y<16f)
                if (Projectile.aiStyle < 1)
                Projectile.velocity = Projectile.velocity + new Vector2(0, 0.1f);
            }

        }

        public override bool PreKill(int timeLeft)
        {
            if (Projectile.ai[1] == (int)JavelinType.SanguineBident)
            {
                for (int num315 = -40; num315 < 43; num315 = num315 + 4)
                {
                    int dustType = DustID.LunarOre;//Main.rand.Next(139, 143);
                    int dustIndex = Dust.NewDust(Projectile.Center + new Vector2(-16, -16) + ((Projectile.rotation-MathHelper.ToRadians(45)).ToRotationVector2() * num315), 32, 32, dustType);//,0,5,0,new Color=Main.hslToRgb((float)(npc.ai[0]/300)%1, 1f, 0.9f),1f);
                    Dust dust = Main.dust[dustIndex];
                    dust.velocity.X = Projectile.velocity.X * 0.8f;
                    dust.velocity.Y = Projectile.velocity.Y * 0.8f;
                    dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                    dust.fadeIn = 0.25f;
                    dust.noGravity = true;
                    Color mycolor = Color.OrangeRed;//new Color(25,22,18);
                    dust.color = mycolor;
                    dust.alpha = 20;
                }
            }

            return true;
        }

        public override bool PreDraw(ref Color drawColor)//Warning for PreDraw! Spritebatch was removed as a parameter
        {
            bool facingleft = Projectile.velocity.X > 0;
            Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(JavelinProj.tex[(int)Projectile.ai[1]]);
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);


            if (Projectile.ai[1] == (int)JavelinType.CrimsonCatastrophe)
            {
                if (stickin >= 0)
                    drawColor = Color.Lerp(drawColor, Color.Red, 0.50f + (float)Math.Sin(Projectile.timeLeft / 15f) / 3f);

                float sticktime = (float)maxStickTime;
                float alpha = 1;
                if (Projectile.penetrate <= 1)
                    alpha *= MathHelper.Clamp((float)(Projectile.timeLeft-100) / 10f, 0f, 1f);

                drawColor *= alpha;

                if (stickin >= 0)
                {
                    for (float k = 0; k < MathHelper.TwoPi; k += MathHelper.Pi / 2f)
                    {
                        float angle = (k + (float)Projectile.whoAmI + Main.GlobalTimeWrappedHourly);
                        Vector2 drawPos = Projectile.Center+Vector2.Normalize(angle.ToRotationVector2())*260;
                        Color color = drawColor * MathHelper.Clamp(Projectile.localAI[0] / 10f, 0.25f, 1f);

                        //spriteBatch.Draw(texture, drawPos - Main.screenPosition, new Rectangle?(), color * 0.5f, angle - (3f*MathHelper.Pi/4f), origin, Projectile.scale, effect, 0);

                        angle = (k - (float)Projectile.whoAmI - Main.GlobalTimeWrappedHourly);
                        float timedelay = (float)(Projectile.timeLeft - 100) / sticktime;
                        float alpha2 = MathHelper.Clamp((float)Math.Sin((double)timedelay*Math.PI)*2f,0f, 1f);
                        float distancemul = 100f / sticktime;
                        drawPos = Projectile.Center + Vector2.Normalize(angle.ToRotationVector2()) * (Projectile.timeLeft-100)*(8f*distancemul);
                        color = drawColor * alpha2;

                        //spriteBatch.Draw(texture, drawPos - Main.screenPosition, new Rectangle?(), color * 0.5f,MathHelper.Pi + angle - (3f * MathHelper.Pi / 4f), origin, Projectile.scale, effect, 0);

                    }
                    return false;
                }

                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    drawColor = Color.Lerp(drawColor, Color.Red, 0.50f + (float)Math.Sin((Projectile.timeLeft + k) / 15f) / 3f);
                    Vector2 drawPos = new Vector2(Projectile.oldPos[k].X + Projectile.width / 2, Projectile.oldPos[k].Y + Projectile.height / 2) - Main.screenPosition;
                    Color color = Projectile.GetAlpha(drawColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                    //spriteBatch.Draw(texture, drawPos, new Rectangle?(), color * 0.5f* alpha, Projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f) - (((float)Math.PI / 2) * (facingleft ? 0f : -1f)), origin, Projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);
                }
                return false;
            }//Crimson Catastrophe


            if (Projectile.ai[1] == (int)JavelinType.SanguineBident || Projectile.ai[1] == (int)JavelinType.Thermal)//Sanguine Bident
            {           
                Texture2D glow = null;
                if (Projectile.ai[1] == (int)JavelinType.Thermal)
                {
                    glow = (Texture2D)ModContent.Request<Texture2D>("SGAmod/Items/GlowMasks/ThermalJavelin_Glow");
                    drawColor = Color.White;
                }
                    for (int k = 0; k < (stickin < 0 ? Projectile.oldPos.Length : 1); k++)
                    {
                        Vector2 drawPos = new Vector2(Projectile.oldPos[k].X + Projectile.width / 2, Projectile.oldPos[k].Y + Projectile.height / 2) - Main.screenPosition;
                        Color color = (Projectile.ai[1] == (int)JavelinType.Thermal ? Color.White : Projectile.GetAlpha(drawColor)) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                        //spriteBatch.Draw(texture, drawPos, new Rectangle?(), color * 0.5f, Projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f) - (((float)Math.PI / 2) * (facingleft ? 0f : -1f)), origin, Projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);
                    //if (glow != null)
                        //spriteBatch.Draw(glow, drawPos, new Rectangle?(), color * 0.5f, Projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f) - (((float)Math.PI / 2) * (facingleft ? 0f : -1f)), origin, Projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);

                    }
            }


            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), drawColor, Projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f) - (((float)Math.PI / 2)*(facingleft ? 0f : -1f)), origin, Projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);
            return false;
        }
    }

    public class JavelinProjMelee : ProjectileSpearBase
    {

        bool mousecurser;
        public bool hitboxchange = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Javelin");
        }

        public override string Texture
        {
            get { return ("SGAmod/Items/Weapons/Javelins/StoneJavelin"); }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            double blank = 1.0;
            JavelinProj.JavelinOnHit(target, Projectile,ref blank);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Projectile.ai[1] == (int)JavelinType.SwampSovnya)//Swamp Sovnya
            {
                if (Projectile.ai[1] == (int)JavelinType.SwampSovnya)//Swamp Sovnya
                {
                    if (target.buffImmune[BuffID.Poisoned])
                    {
                        damage = (int)(damage * 1.25f);
                    }
                }
                if (target.HasBuff(ModContent.BuffType<DankSlow>()))
                {
                    crit = true;
                    int index = target.FindBuffIndex(ModContent.BuffType<DankSlow>());
                    damage = (int)(damage * Math.Min(1 + (target.buffTime[index] / 180f),5f));
                    target.DelBuff(index);
                }
            }
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.scale = 1.2f;
            Projectile.knockBack = 1f;
            Projectile.aiStyle = 19;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 90;
            Projectile.hide = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

            movein = 3f;
            moveout = 3f;
            thrustspeed = 3.0f;
            this.hitboxchange = false;
        }

        public override bool PreAI()
        {
            if (Projectile.ignoreWater == false)
            {
                if (Projectile.ai[1] == (int)JavelinType.CrimsonCatastrophe)
                {
                    Projectile.extraUpdates = 1;
                    movein = 8f;
                    moveout = 5f;
                    thrustspeed = 5.0f;
                    Projectile.localNPCHitCooldown = 1;
                }

                    if (Main.dedServ)
                {
                    Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(JavelinProj.tex[(int)Projectile.ai[1]] + "Spear");
                    int xx = texture.Width - (int)((Projectile.width) / 1.5f);
                    int yy = texture.Height - (int)((Projectile.height) / 1.5f);
                    Projectile.position.X -= (int)((xx / 1.5f));
                    Projectile.position.Y -= (int)((yy / 1.5f));
                    Projectile.width = (int)(texture.Width / 1.5f);
                    Projectile.height = (int)(texture.Height / 1.5f);
                }
            }
            return true;
        }
        public override void MakeProjectile()
        {
            if (Projectile.ai[1] == (int)JavelinType.TerraTrident)
            {
                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 60, 0.6f, 0.25f);
                Vector2 velo = Projectile.velocity;velo.Normalize();
                velo *= 8f;
                Projectile.NewProjectile(null, Projectile.Center.X+ (velo.X*5f), Projectile.Center.Y + (velo.Y * 5f), velo.X, velo.Y, Mod.Find<ModProjectile>("TerraTridentProj").Type, (int)(Projectile.damage * 0.75), Projectile.knockBack, Main.myPlayer);
            }
        }

        public override void AI()
        {
            base.AI();

            if (Main.player[Projectile.owner] != null)
            {
                Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
                if (Projectile.owner == Main.myPlayer)
                {
                    mousecurser = (Main.MouseScreen.X - Projectile.Center.X) > 0;
                    Projectile.direction = mousecurser.ToDirectionInt();
                    Projectile.netUpdate = true;


                }
            }

        }

        public new float movementFactor
        {
            get { return Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }
        public override bool PreDraw(ref Color drawColor)
        {
            Player owner = Main.player[Projectile.owner];
            bool facingleft = Projectile.direction < 0f;
            Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(JavelinProj.tex[(int)Projectile.ai[1]]+"Spear");
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Vector2 drawpos = Projectile.Center;

            if (Projectile.ai[1] == (int)JavelinType.CrimsonCatastrophe)
            {
                drawpos += Vector2.Normalize(Projectile.Center - owner.Center) * 64f;
                drawColor *= 0.5f;// MathHelper.Clamp((float)(projectile.timeLeft-60f) / 25f,0, 1f);

                for (float i = 0; i < 1f && Projectile.ai[1] == 10; i += 0.02f)
                {
                    Vector2 offset = (owner.Center - Projectile.Center) * i;
                    drawColor *= (1f - (i / 8f));

                    if (facingleft)
                        Main.spriteBatch.Draw(texture, (drawpos + offset) - Main.screenPosition, new Rectangle?(), drawColor, ((Projectile.rotation - (float)Math.PI / 2) - (float)Math.PI / 2) + (facingleft ? (float)(1f * Math.PI) : 0f), origin, Projectile.scale, effect, 0);
                    if (!facingleft)
                        Main.spriteBatch.Draw(texture, (drawpos + offset) - Main.screenPosition, new Rectangle?(), drawColor, (Projectile.rotation - (float)Math.PI / 2) + (facingleft ? (float)(1f * Math.PI) : 0f), origin, Projectile.scale, SpriteEffects.None, 0);
                }

            }
            else
            {

                if (facingleft)
                    Main.spriteBatch.Draw(texture, (drawpos) - Main.screenPosition, new Rectangle?(), drawColor, ((Projectile.rotation - (float)Math.PI / 2) - (float)Math.PI / 2) + (facingleft ? (float)(1f * Math.PI) : 0f), origin, Projectile.scale, effect, 0);
                if (!facingleft)
                    Main.spriteBatch.Draw(texture, (drawpos) - Main.screenPosition, new Rectangle?(), drawColor, (Projectile.rotation - (float)Math.PI / 2) + (facingleft ? (float)(1f * Math.PI) : 0f), origin, Projectile.scale, SpriteEffects.None, 0);

            }



            return false;
        }

    }

}