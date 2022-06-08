using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Terraria.Audio;

namespace SGAmod.HavocGear.Projectiles
{
    public class SnappyTooth : ModProjectile
    {
        public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Snappy Tooth");
	}
		
	public override void SetDefaults()
        { 
	    Projectile.CloneDefaults(ProjectileID.Bullet);
	    AIType = ProjectileID.Bullet;
            Projectile.width = 9;      
            Projectile.height = 12;
            Projectile.friendly = true;     
            Projectile.DamageType = DamageClass.Ranged;        
            Projectile.tileCollide = true;   
            Projectile.penetrate = -1;     
            Projectile.timeLeft = 2000; 
            Projectile.ignoreWater = true;   
        }

        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
            for (int num315 = 0; num315 < 15; num315 = num315 + 1)
            {
                int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 51, Projectile.velocity.X+(float)(Main.rand.Next(-20,20)/15f), Projectile.velocity.Y+(float)(Main.rand.Next(-20,20)/15f), 50, Main.hslToRgb(0f, 0.15f, 0.8f)*0.75f, 0.75f);
                Dust dust3 = Main.dust[num316];
                dust3.velocity *= 0.75f;
            }
            return true;
        }

	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)

	{
        if (this.GetType().Name=="SnappyTooth")
		target.AddBuff(Mod.Find<ModBuff>("Gourged").Type, 240);
	}			
    }
}