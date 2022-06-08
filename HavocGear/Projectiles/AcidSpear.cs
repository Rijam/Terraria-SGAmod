using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;

namespace SGAmod.HavocGear.Projectiles
{
    public class AcidSpear : ModProjectile
    {
        public override void SetStaticDefaults()
	    {
		    DisplayName.SetDefault("Acid Spear");
	    }
		
		public override void SetDefaults()
        { 
            Projectile.width = 7;      
            Projectile.height = 22; 
            Projectile.friendly = true;     
            Projectile.DamageType = DamageClass.Melee;        
            Projectile.tileCollide = true;   
            Projectile.penetrate = 3;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 8;
	        Projectile.alpha = 200;     
            Projectile.timeLeft = 2000;  
            Projectile.light = 0.75f;   
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;   
        }
        public override void AI()         
        {                                                    
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;  
       	
		    if (Main.rand.Next(3) == 0)
			{
				int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.AcidDust>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 200, default(Color), 0.7f);
				Main.dust[dustIndex].velocity += Projectile.velocity * 0.3f;
				Main.dust[dustIndex].velocity *= 0.2f;
			}
		}

	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        	Player player = Main.player[Projectile.owner];
		target.immune[Projectile.owner] = 2;
		target.AddBuff(Mod.Find<ModBuff>("AcidBurn").Type, 100);
    	}	
    }
}