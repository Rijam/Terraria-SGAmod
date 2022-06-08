using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using Terraria.Audio;


namespace SGAmod.Projectiles
{

    public class TwineOfFateClothier : TwineOfFate
    {
        bool remove = false;
        public override float theangle => (float)Math.PI;
        public override int myNPC => NPCID.Clothier;

        public override string Texture
        {
            get { return ("Terraria/Item_" + ItemID.ClothierVoodooDoll); }
        }

    }
    public class TwineOfFate : ModProjectile
    {
        bool remove = false;
        public virtual float theangle => 0f;
        public virtual int myNPC => NPCID.Guide;
        private Vector2[] oldPos = new Vector2[8];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doll");
        }

        public override bool CanDamage()
        {
            return NPC.CountNPCS(myNPC) > 0;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.Boulder);
            AIType = ProjectileID.Boulder;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1000;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override string Texture
        {
            get { return ("Terraria/Item_" + ItemID.GuideVoodooDoll); }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (NPC.CountNPCS(myNPC) > 0)
            {
                double angle = theangle + MathHelper.ToRadians(Projectile.ai[0]);
                float dist = Math.Min(Projectile.localAI[0] * 2, 96f);
                Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist), (float)(Math.Sin(angle) * dist));

                //spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 10), color, projectile.velocity.X * 0.04f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
                SGAUtils.DrawFishingLine(Projectile.position, Main.player[Projectile.owner].Center, new Vector2(20*Math.Sign(thisloc.X), 0), new Vector2(0, 0), 0f);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GetHit(int damage)
        {
            for (int i = 0; i < Main.maxNPCs; i += 1)
            {
                NPC npc = Main.npc[i];
                if (npc.type == myNPC)
                {
                    Vector2 previously = npc.Center;
                    npc.Center = Projectile.Center;
                    npc.StrikeNPC(damage, 0f, 1);
                    if (npc.life > 0 && NPC.CountNPCS(myNPC) > 0)
                    {
                        npc.Center = previously;
                    }
                    else
                    {
                        npc.netUpdate = true;
                    }
                }

            }

        }

        public override void AI()
        {
            Projectile.velocity *= 0.95f;
            if (Projectile.ai[1] < 1 && NPC.CountNPCS(myNPC) > 0)
            {
                for (int i = 0; i < Main.maxProjectiles; i += 1)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj != null && proj.active)
                    {
                        if (proj.hostile && proj.damage > 0)
                        {
                            Rectangle mecol = Projectile.Hitbox;
                            Rectangle themcol = proj.Hitbox;
                            if (themcol.Intersects(mecol) && proj.damage > 1)
                            {
                                Projectile.velocity = proj.velocity * 3;
                                proj.velocity *= -1f;
                                SoundEngine.PlaySound(SoundID.NPCHit, (int)proj.position.X, (int)proj.position.Y, 1, 1f, 0.25f);

                                Projectile.ai[1] = 30;
                                Projectile.damage = 1;
                                GetHit(proj.damage);
                            }

                        }
                    }
                }
            }


            Player player = Main.player[Projectile.owner];
            if (player == null)
            {
                remove = true;
                Projectile.Kill();
            }
            else
            {
                if (player.dead || (!player.GetModPlayer<SGAPlayer>().twinesoffate))
                    return;
                Projectile.localAI[1] += 1;
                Projectile.timeLeft = 2;
                Projectile.ai[1] -= 1;
                Projectile.localAI[0] += 1;
                Projectile.ai[0] -= 3f;

                double angle = theangle + MathHelper.ToRadians(Projectile.ai[0]);
                float dist = Math.Min(Projectile.localAI[0] * 2, 96f);
                Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist), (float)(Math.Sin(angle) * dist));


                Projectile.Center = player.Center + (thisloc);

            }



        }

    }

}