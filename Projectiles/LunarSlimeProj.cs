using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using System.Linq;
using SGAmod.Effects;
using Terraria.Audio;

namespace SGAmod.Projectiles
{
    public class LunarSlimeProjectile : ModProjectile
    {
        bool remove = false;
        private Vector2[] oldPos = new Vector2[8];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lunar Slime");
        }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.Boulder);
            AIType = ProjectileID.Boulder;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1000;
            Projectile.light = 0.5f;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override string Texture
        {
            get { return ("SGAmod/Items/LunarRoyalGel"); }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            if (Projectile.ai[1] > 0)
                return false;

            Player owner = Main.player[Projectile.owner];

            Texture2D tex = Main.projectileTexture[Projectile.type];
            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 6) / 2f;

            //oldPos.Length - 1
            //for (int k = oldPos.Length - 1; k >= 0; k -= 2)
            int k = 0;

                Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 4f);
                Color color = (Main.hslToRgb((Projectile.ai[0] / 8f) % 1f, 1f, 0.9f)) * (1f - (float)(k + 1) / (float)(oldPos.Length + 2));
                int timing = (int)(Projectile.localAI[0] / 8f);
                timing %= 6;
                timing *= ((tex.Height) / 6);

            List<Vector2> list = oldPos.ToList();
            //list.Insert(0, projectile.Center);

            for (int z = 0; z < list.Count; z += 1)
            {
                float percent = (z / (float)list.Count) * (0f+(float)Math.Sin((owner.SGAPly().timer/20f)-(z/5f))*0.5f);
                list[z] += (owner.Center - list[z]) * percent;
            }

            TrailHelper trail = new TrailHelper("DefaultPass", SGAmod.ExtraTextures[21]);
            trail.color = delegate (float percent)
            {
                return Color.Pink;
            };
            trail.projsize = Vector2.Zero;
            trail.trailThickness = 2;
            trail.trailThicknessIncrease = 6;
            trail.DrawTrail(list, Projectile.Center);

            float angle = (float)(((1f + Projectile.ai[0] / 8f)) + 2.0 * Math.PI * (Projectile.ai[0] / ((double)8f)));

            spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 6), color, (float)Math.Sin(-angle), drawOrigin, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

        private void explode()
        {
            SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);
            int numProj = 2;
            float rotation = MathHelper.ToRadians(1);
            for (int i = 0; i < numProj; i++)
            {
                Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("SlimeBlast").Type, Projectile.damage*2, Projectile.knockBack, Projectile.owner, 0f, 0f);
            }
            List<int> types = new List<int>();
                types.Add(BuffID.Regeneration); types.Add(BuffID.RapidHealing); types.Add(BuffID.DryadsWard); types.Add(BuffID.ParryDamageBuff); types.Add(BuffID.Clairvoyance); types.Add(BuffID.Sharpened); types.Add(BuffID.AmmoBox);
            types.Add(BuffID.Honey); types.Add(BuffID.Invisibility); types.Add(BuffID.Ironskin);
            Player player = Main.player[Projectile.owner];
            if (!player.dead)
            player.AddBuff(types[Main.rand.Next(0,types.Count)],60*8);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.ai[1] > 0 || Projectile.localAI[0] < 130)
                return false;

            return base.CanHitNPC(target);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Projectile.penetrate < 995)
            {
                Projectile.penetrate = 1000;
                Projectile.ai[1] = 60 * 6;
                explode();
            }
            Projectile.localAI[0] = 100;
            target.immune[Projectile.owner] = 1;
        }

        public override void AI()
        {

            for (int i = 0; i < oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
            {
                if (oldPos[i] == default)
                    oldPos[i] = Projectile.Center;
            }

            if (Projectile.ai[1] < 1)
            {
                for (int i = 0; i < Main.maxProjectiles; i += 1)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj != null && proj.active)
                    {
                        if (proj.hostile && Projectile.damage>0)
                        {
                            Rectangle mecol = Projectile.Hitbox;
                            Rectangle themcol = proj.Hitbox;
                            if (themcol.Intersects(mecol) && proj.damage>1)
                            {
                                proj.damage = 1;
                                Projectile.penetrate -= proj.CanReflect() ? 0 : 2;
                                SoundEngine.PlaySound(29, (int)proj.position.X, (int)proj.position.Y, Main.rand.Next(66, 69), 1f, -0.6f);

                                if (Projectile.penetrate < 995)
                                {
                                    Projectile.penetrate = 1000;
                                    Projectile.ai[1] = 60 * 10;
                                    explode();
                                }
                            }

                        }
                    }
                }
            }

            for (int k = oldPos.Length - 1; k > 0; k--)
            {
                oldPos[k] = oldPos[k - 1];
            }
            oldPos[0] = Projectile.Center;

            if (Projectile.ai[1] > 0)
            {
                int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 72, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 20, Main.hslToRgb((float)(Projectile.ai[0] / 4f) % 1, 1f, 0.9f) * 0.2f, 1f);
                Main.dust[DustID2].noGravity = true;
            }

            Player player = Main.player[Projectile.owner];
            if (player == null)
            {
                remove = true;
                Projectile.Kill();
            }
            else
            {
                if (player.dead || (!player.GetModPlayer<SGAPlayer>().lunarSlimeHeart && Projectile.ai[1]<1))
                    return;
                Projectile.localAI[1] += 1;
                Projectile.timeLeft = 2;
                Projectile.ai[1] -= 1;
                Projectile.localAI[0] += 1;
                Projectile.ai[0] += 0.1f;

                    Projectile.damage = player.GetModPlayer<SGAPlayer>().lunarSlimeHeartdamage;

                double angle = ((1f + Projectile.ai[0] / 8f)) + 2.0 * Math.PI * (Projectile.ai[0] / ((double)8f));
                float dist = Math.Min(Projectile.localAI[0] * 2, 100f);
                Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist), (float)(Math.Sin(angle) * dist));


                Projectile.Center = player.Center + (thisloc);
                Projectile.velocity = thisloc;
                Projectile.velocity.Normalize();

            }



        }
    }

    public class SlimeBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blast");
        }

        public override void SetDefaults()
        {
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 30;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
        crit = false;
        }

        public override string Texture
        {
            get { return ("SGAmod/HavocGear/Projectiles/BoulderBlast"); }
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.01f) / 255f, ((255 - Projectile.alpha) * 0.025f) / 255f, ((255 - Projectile.alpha) * 0.25f) / 255f);
            bool flag15 = false;
            bool flag16 = false;
            if (Projectile.velocity.X < 0f && Projectile.position.X < Projectile.ai[0])
            {
                flag15 = true;
            }
            if (Projectile.velocity.X > 0f && Projectile.position.X > Projectile.ai[0])
            {
                flag15 = true;
            }
            if (Projectile.velocity.Y < 0f && Projectile.position.Y < Projectile.ai[1])
            {
                flag16 = true;
            }
            if (Projectile.velocity.Y > 0f && Projectile.position.Y > Projectile.ai[1])
            {
                flag16 = true;
            }
            if (flag15 && flag16)
            {
                Projectile.Kill();
            }
            float num461 = 25f;
            if (Projectile.ai[0] > 180f)
            {
                num461 -= (Projectile.ai[0] - 180f) / 2f;
            }
            if (num461 <= 0f)
            {
                num461 = 0f;
                Projectile.Kill();
            }
            num461 *= 0.7f;
            Projectile.ai[0] += 4f;
            int num462 = 0;
            while ((float)num462 < num461)
            {
                float num463 = (float)Main.rand.Next(-30, 31);
                float num464 = (float)Main.rand.Next(-30, 31);
                Vector2 stuff2 = new Vector2(num463, num464);
                stuff2.Normalize();
                stuff2*=(5f+Main.rand.NextFloat(0f,6f))*((float)Projectile.width/ 160f);
                int dustx = (Main.rand.NextBool()) ? Mod.Find<ModDust>("AcidDust") .Type: 184;
                if (Main.rand.NextBool())
                    dustx = (Main.rand.NextBool()) ? Mod.Find<ModDust>("HotDust") .Type: 43;
                int num467 = Dust.NewDust(new Vector2(Projectile.Center.X,Projectile.Center.Y), 0,0, dustx);
                Main.dust[num467].noGravity = true;
                Main.dust[num467].scale = 1f;
                Main.dust[num467].position.X = Projectile.Center.X;
                Main.dust[num467].position.Y = Projectile.Center.Y;
                Main.dust[num467].position.X += (float)Main.rand.Next(-10, 11);
                Main.dust[num467].position.Y += (float)Main.rand.Next(-10, 11);
                Main.dust[num467].velocity.X = stuff2.X;
                Main.dust[num467].velocity.Y = stuff2.Y;
                //Main.dust[num467].fadeIn = 0f;
                Main.dust[num467].noGravity = true;
                Main.dust[num467].color = Main.hslToRgb(Main.rand.NextFloat(0f, 1f), 0.7f, 1f);
                num462++;
            }
            return;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(Mod.Find<ModBuff>("ThermalBlaze").Type, 300);
            target.AddBuff(BuffID.Daybreak, 300);
            target.AddBuff(Mod.Find<ModBuff>("AcidBurn").Type, 180);
            target.AddBuff(BuffID.Frostburn, 300);
        }
    }

}