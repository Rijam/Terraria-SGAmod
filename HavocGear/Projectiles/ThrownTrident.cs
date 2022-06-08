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
    public class ThrownTrident : ModProjectile
    {
        public override void SetStaticDefaults()
	    {
		    DisplayName.SetDefault("Trident");
	    }
		
		public override void SetDefaults()
        { 
		 Projectile.damage = 15;      
            Projectile.width = 16;      
            Projectile.height = 16; 
            Projectile.friendly = false;     
            // projectile.melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;           
            Projectile.tileCollide = true;   
            Projectile.penetrate = 4;     
            Projectile.timeLeft = 500;  
            Projectile.light = 0.25f;   
            Projectile.extraUpdates = 1;
   		    Projectile.ignoreWater = true;   
        }
        public override void AI()           //this make that the projectile will face the corect way
        {                                                           // |
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;  
       	}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int num315 = 0; num315 < 15; num315 = num315 + 1)
            {
                int dustType = 15;//Main.rand.Next(139, 143);
                int dustIndex = Dust.NewDust(Projectile.Center+new Vector2(-16,-16), 32,32, dustType);//,0,5,0,new Color=Main.hslToRgb((float)(npc.ai[0]/300)%1, 1f, 0.9f),1f);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = Projectile.velocity.X*0.4f;
                dust.velocity.Y = Projectile.velocity.Y*0.4f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                dust.fadeIn = 0.25f;
                dust.noGravity = true;
                Color mycolor = Color.Aqua;//new Color(25,22,18);
                dust.color=mycolor;
                dust.alpha=20;
            }
		}
    }
}