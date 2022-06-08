using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
	public class Landslide1 : ModProjectile
	{
        bool abovePlayer;
		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 2000;
            Projectile.tileCollide = false;
        }

        public void spinspin()
        {
            if (Projectile.ai[0] == 0)
            {
                Projectile.ai[0] = Main.rand.NextFloat(-0.2f, 0.2f);
            }
            else
            {
                Projectile.rotation += Projectile.ai[0];
            }



        }

        public override bool PreKill(int timeLeft)
        {
            for (int num654 = 0; num654 < 10; num654++)
            {
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
                int num655 = Dust.NewDust(Projectile.position + Vector2.UnitX * -20f, Projectile.width + 40, Projectile.height, DustID.t_Lihzahrd, Projectile.velocity.X + randomcircle.X * 8f, Projectile.velocity.Y + randomcircle.Y * 3f, 100, new Color(30, 30, 30, 20), 2f);
                Main.dust[num655].noGravity = true;
                Main.dust[num655].velocity *= 0.5f;
            }
            return true;
        }


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Landslide");
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (abovePlayer == true)
            {
                return false;
            }
            return true;
        }

        public override void AI()
        {
            spinspin();
            Projectile.velocity.X *= 0.98f;
            Projectile.velocity.Y *= 1.00025f;
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 0f)
            {
                if (player.position.Y > Projectile.position.Y)
                {
                    abovePlayer = true;
                    Projectile.tileCollide = false;
                }
                else if (player.position.Y < Projectile.position.Y)
                {
                    abovePlayer = false;
                    Projectile.tileCollide = true;
                }
            }
        }
	}

    public class Landslide2 : Landslide1
    {
        bool abovePlayer;
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 1000;
            Projectile.tileCollide = false;
        }
    }

    public class Landslide3 : Landslide1
    {
        bool abovePlayer;
        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 1000;
        }

        public override void AI()
        {
            spinspin();
            Projectile.velocity.X *= 0.95f;
            Projectile.velocity.Y *= 1.001f;
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0] == 0f)
            {
                if (player.position.Y > Projectile.position.Y)
                {
                    abovePlayer = true;
                    Projectile.tileCollide = false;
                }
                else if (player.position.Y < Projectile.position.Y)
                {
                    abovePlayer = false;
                    Projectile.tileCollide = true;
                }
                /*if (projectile.velocity.Y > 5)
                {
                    projectile.velocity.Y = 5;
                }*/
                if (Main.tile[(int)projectile.position.X / 16, (int)projectile.position.Y / 16].HasTile)
                {
                    Projectile.alpha = 150;
                }
                else
                {
                    Projectile.alpha = 0;
                }
            }
        }
    }
}