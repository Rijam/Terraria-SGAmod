using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Idglibrary;
using SGAmod.Effects;
using System.Linq;
using Terraria.Audio;

namespace SGAmod.Projectiles
{
    public class MoonlightWaveLv1 : ModProjectile
    {
        virtual protected int extraparticles => 0;
        Effect effect => SGAmod.TrailEffect;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonlight Wave");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = 8;
            Projectile.alpha = 40;
            Projectile.timeLeft = 500;
            Projectile.light = 0.75f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            //VertexBuffer vertexBuffer;

            /*basicEffect.World = WVP.World();
            basicEffect.View = WVP.View(Main.GameViewMatrix.Zoom);
            basicEffect.Projection = WVP.Projection();
            basicEffect.VertexColorEnabled = true;
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = SGAmod.ExtraTextures[21];*/
            /*effect.Parameters["WorldViewProjection"].SetValue(WVP.View(Main.GameViewMatrix.Zoom) * WVP.Projection());
            effect.Parameters["imageTexture"].SetValue(SGAmod.ExtraTextures[21]);
            effect.Parameters["coordOffset"].SetValue(new Vector2(0, Main.GlobalTimeWrappedHourly * -1f));
            effect.Parameters["coordMultiplier"].SetValue(1f);
            effect.Parameters["strength"].SetValue(1f);
            string pass = "DefaultPass";

            int totalcount = projectile.oldPos.Length;

            for(int i=0;i< projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
            {
                if (projectile.oldPos[i]==default)
                projectile.oldPos[i] = projectile.position;
            }

                VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[((totalcount + 1) * 6)];

                Vector3[] prevcoords = { Vector3.One, Vector3.One };

            float coordprogress = 0;
            for (int k = 1; k < totalcount; k += 1)
                {
                    float fraction = (float)k / (float)totalcount;
                    float fractionPlus = (float)(k + 1) / (float)totalcount;

                    Vector2 size = new Vector2(projectile.width, projectile.height) / 2f;
                    Vector2 trailloc = projectile.oldPos[k] + size;
                    Vector2 prev2 = projectile.oldPos[k - 1] + size;
                if (prev2 == default)
                    prev2 = trailloc;

                //You want prims, you get prims!

                float thickness = 13+(1f-(k/(float)projectile.oldPos.Length))*16;

                    Vector2 normal = Vector2.Normalize(trailloc - prev2);
                    Vector3 left = (normal.RotatedBy(MathHelper.Pi / 2f) * (thickness)).ToVector3();
                    Vector3 right = (normal.RotatedBy(-MathHelper.Pi / 2f) * (thickness)).ToVector3();

                    Vector3 drawtop = (trailloc - Main.screenPosition).ToVector3();
                    Vector3 drawbottom = (prev2 - Main.screenPosition).ToVector3();

                    if (prevcoords[0] == Vector3.One)
                    {
                        prevcoords = new Vector3[2] { drawbottom + left, drawbottom + right };
                    }

                    Color color = Color.Lerp(Color.White, Color.White * 0f, fraction);
                    Color color2 = Color.Lerp(Color.White, Color.White * 0f, fractionPlus);

                    vertices[0 + (k * 6)] = new VertexPositionColorTexture(prevcoords[0], color, new Vector2(0, fractionPlus));
                    vertices[1 + (k * 6)] = new VertexPositionColorTexture(drawtop + right, color2, new Vector2(1, fraction));
                    vertices[2 + (k * 6)] = new VertexPositionColorTexture(drawtop + left, color2, new Vector2(0, fraction));

                    vertices[3 + (k * 6)] = new VertexPositionColorTexture(prevcoords[0], color, new Vector2(0, fractionPlus));
                    vertices[4 + (k * 6)] = new VertexPositionColorTexture(prevcoords[1], color, new Vector2(1, fractionPlus));
                    vertices[5 + (k * 6)] = new VertexPositionColorTexture(drawtop + right, color2, new Vector2(1, fraction));

                    prevcoords = new Vector3[2] { drawtop + left, drawtop + right };

                    //Idglib.DrawTether(SGAmod.ExtraTextures[21], prev2, goto2, 1f, 0.25f, 1f, Color.Magenta);

                }

                vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
                vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

                Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;



                effect.CurrentTechnique.Passes[pass].Apply();
                    Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, ((totalcount + 1) * 2));

            */

            for (int i = 0; i < Projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
            {
                if (Projectile.oldPos[i] == default)
                    Projectile.oldPos[i] = Projectile.position;
            }

            TrailHelper trail = new TrailHelper("BasicEffectPass", Main.extraTexture[21]);
            trail.projsize = Projectile.Hitbox.Size() / 2f;
            trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -1f);
            trail.trailThickness = 13;
            trail.trailThicknessIncrease = 15;
            trail.DrawTrail(Projectile.oldPos.ToList(),Projectile.Center);



            Texture2D texture = Main.projectileTexture[Mod.Find<ModProjectile>(this.GetType().Name).Type];
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

            spriteBatch.Draw(texture, Projectile.Center + new Vector2(1, 0) - Main.screenPosition, null, Color.White, Projectile.rotation, origin, new Vector2(1f, 1f), Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }

        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item94, Projectile.Center);
            SoundEngine.PlaySound(SoundID.Item89, Projectile.Center);
            for (float num475 = -6 - extraparticles; num475 < 6 + extraparticles; num475 += 0.3f)
            {
                float anglehalf = (float)(((double)Projectile.velocity.ToRotation()) + 2.0 * Math.PI);
                Vector2 startloc2 = Projectile.velocity;
                startloc2.Normalize();
                Vector2 startloc = (Projectile.Center + (startloc2 * 12f));
                int dust = Dust.NewDust(new Vector2(startloc.X, startloc.Y), 0, 0, 185);

                float anglehalf2 = anglehalf + ((float)Math.PI / 2f);
                Main.dust[dust].position += anglehalf2.ToRotationVector2() * (float)((Main.rand.Next(-200, 200) / 10f));

                Main.dust[dust].scale = 2f - Math.Abs(num475) / 4f;
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                Main.dust[dust].velocity = (randomcircle / 3f);
                Main.dust[dust].velocity += (Projectile.velocity * num475);
                Main.dust[dust].noGravity = true;
            }

            return true;
        }

        public override void AI()
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y * Projectile.spriteDirection, (double)Projectile.velocity.X * Projectile.spriteDirection) + 1.57f;

            for (float num475 = -2; num475 < 3; num475 += 2)
            {
                float anglehalf = (float)(((double)Projectile.velocity.ToRotation()) + 2.0 * Math.PI);
                Vector2 startloc2 = Projectile.velocity;
                startloc2.Normalize();
                Vector2 startloc = (Projectile.Center + (startloc2 * 8f));
                int dust = Dust.NewDust(new Vector2(startloc.X, startloc.Y), 0, 0, 185);

                float anglehalf2 = anglehalf + ((float)Math.PI / 2f);
                Main.dust[dust].position += anglehalf2.ToRotationVector2() * (float)((Main.rand.Next(-200, 200) / 10f));

                Main.dust[dust].scale = 1.2f - Math.Abs(num475) / 5f;
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                Main.dust[dust].velocity = (randomcircle / 3f);
                Main.dust[dust].velocity += Projectile.velocity / 2f;
                Main.dust[dust].noGravity = true;
            }


            for (float num475 = -2f; num475 < 3f; num475 += 4f)
            {

                float anglehalf = (float)(((double)Projectile.velocity.ToRotation()) + 2.0 * Math.PI);
                Vector2 startloc = (Projectile.Center + (Projectile.velocity * 1f));
                int dust = Dust.NewDust(new Vector2(startloc.X, startloc.Y), 0, 0, 185);
                Main.dust[dust].scale = 1f;
                Main.dust[dust].velocity = Projectile.velocity;
                Main.dust[dust].noGravity = true;
                if (Math.Abs(num475) > 0)
                {
                    anglehalf += ((float)Math.PI / 2f);
                    Main.dust[dust].velocity += anglehalf.ToRotationVector2() * ((num475 * 4f) / 1.25f);
                }
            }

        }


        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(Mod.Find<ModBuff>("MoonLightCurse").Type, 60 * 8);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (this.GetType() == typeof(MoonlightWaveLv2))
                target.AddBuff(Mod.Find<ModBuff>("MoonLightCurse").Type, 60 * 5);
        }

    }

    public class MoonlightWaveLv2 : MoonlightWaveLv1
    {
        virtual protected int extraparticles => 6;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonlight Wave");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = 12;
            Projectile.alpha = 40;
            Projectile.timeLeft = 500;
            Projectile.light = 1.15f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }

        public override void AI()
        {
            base.AI();
            if (Projectile.ai[0] < 1)
            {
                Projectile.ai[0] = 1;
                SoundEngine.PlaySound(SoundID.Item30, Projectile.Center);
            }
        }

    }

    public class MoonlightWaveLv3 : MoonlightWaveLv1
    {
        virtual protected int extraparticles => 16;
        double keepspeed = 0.0;
        float homing = 0.07f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonlight Wave");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 52;
            Projectile.height = 52;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.alpha = 40;
            Projectile.timeLeft = 600;
            Projectile.light = 1.5f;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            SoundEngine.PlaySound(SoundID.Item119, Projectile.Center);
        }

        public override bool PreKill(int timeLeft)
        {
            base.PreKill(timeLeft);
            SoundEngine.PlaySound(SoundID.Item124, Projectile.Center);
            SoundEngine.PlaySound(SoundID.NPCDeath60, Projectile.Center);
            return true;
        }

        public override void AI()
        {
            base.AI();
            if (Projectile.ai[0] < 1)
            {
                Projectile.ai[0] = 1;
                keepspeed = (Projectile.velocity).Length();
                SoundEngine.PlaySound(SoundID.Item119, Projectile.Center);
            }

            Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();

            NPC target = Main.npc[Idglib.FindClosestTarget(0, Projectile.Center, new Vector2(0f, 0f), false, true, true, Projectile)];
            if (target != null)
            {
                if ((target.Center - Projectile.Center).Length() < 1000f)
                {
                    Projectile.velocity = Projectile.velocity + (Projectile.DirectionTo(target.Center) * ((float)keepspeed * homing));
                    Projectile.velocity.Normalize();
                    Projectile.velocity = Projectile.velocity * (float)keepspeed;
                }
            }


        }

    }


}