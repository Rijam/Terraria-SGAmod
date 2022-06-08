using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Projectiles;

namespace SGAmod.HavocGear.Projectiles
{
	public class MurkFlailBall : ModProjectile
	{
		public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.aiStyle = 15; 
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            if (oldVelocity.Length() > 12)
            {
                for (int num315 = 1; num315 < 13; num315 = num315 + 1)
                {
                    Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                    float velincrease = ((float)(num315 + 8) / 2f);
                    int thisone = Projectile.NewProjectile(Projectile.Center.X - Projectile.velocity.X, Projectile.Center.Y - Projectile.velocity.Y, randomcircle.X * velincrease, randomcircle.Y * velincrease, ModContent.ProjectileType<DankBlast>(), (int)(Projectile.damage * 0.50), 0f, Projectile.owner, 0.0f, 0f);
                    Main.projectile[thisone].friendly = Projectile.friendly;
                    Main.projectile[thisone].hostile = Projectile.hostile;
                    Main.projectile[thisone].netUpdate = true;
                    IdgProjectile.Sync(thisone);
                }

            }
            return true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mudrock Spikeball");
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("SGAmod/HavocGear/Projectiles/MurkFlailChain");
 
            Vector2 position = Projectile.Center;
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = Projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1.35f, SpriteEffects.None, 0.0f);
                }
            }
            return true;
        }
	}

    public class DankBlast : DankArrow
    {

        double keepspeed = 0.0;
        float homing = 0.06f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dank Blast");
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(0, 100) < 50 && !target.boss)
                target.AddBuff(Mod.Find<ModBuff>("DankSlow").Type, (int)(60 * 2.5f));
        }

        public override string Texture
        {
            get { return ("Terraria/Projectile_" + 14); }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.arrow = false;
            Projectile.timeLeft = 300;
            AIType = ProjectileID.WoodenArrowFriendly;
        }

    }
    }