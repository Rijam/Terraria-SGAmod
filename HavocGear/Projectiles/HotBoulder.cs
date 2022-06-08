using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;


namespace SGAmod.HavocGear.Projectiles
{
    public class HotBoulder : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hot Boulder");
		}
		
		public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
            AIType = ProjectileID.Boulder;      
            Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.light = 0.5f;
            Projectile.width = 16;
            Projectile.height = 16;
        }
       
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
            else
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = -oldVelocity.X;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }

	    public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item45,Projectile.Center);
        	int numProj = 2;
            float rotation = MathHelper.ToRadians(1);
            for (int i = 0; i < numProj; i++)
            {
                Vector2 perturbedSpeed = new Vector2(Projectile.velocity.X, Projectile.velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
                Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("BoulderBlast").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            }
            return true;
        }
		
		public override void AI()
        {
			
           int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width + 2, Projectile.height + 2, Mod.Find<ModDust>("HotDust").Type, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 20, default(Color), 1f);
            Main.dust[DustID2].noGravity = true;
		
		
		}
    }
}