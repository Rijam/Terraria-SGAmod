using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;


namespace SGAmod.Projectiles
{
    public class FieryRock : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Rock");
		}
		
		public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
            AIType = ProjectileID.Boulder;      
            Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 10;
			Projectile.light = 0.5f;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide=false;
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
                SoundEngine.PlaySound(SoundID.Item10,Projectile.Center);
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
			
           int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 20, default(Color), 1f);
            Main.dust[DustID2].noGravity = true;
            Player player = Main.player[Projectile.owner];
            if (Projectile.ai[0]>0 || !player.channel || player.dead){
            Projectile.tileCollide=true;
            Projectile.ai[0]+=1;
            }else{
            Projectile.timeLeft=600;
            Vector2 mousePos = Main.MouseWorld;

            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 diff = mousePos - player.Center;
                diff.Normalize();
                Projectile.velocity = diff;
                Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                Projectile.netUpdate = true;
                Projectile.Center = mousePos;
            }
            int dir = Projectile.direction;
            player.ChangeDir(dir);
            player.itemTime = 40;
            player.itemAnimation = 40;
            player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir);

            Projectile.Center=player.Center+(Projectile.velocity*76f);
            Projectile.velocity*=8f;

        }
		
		
		}
    }
}