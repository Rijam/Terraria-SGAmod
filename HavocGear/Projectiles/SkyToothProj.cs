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
    public class SkyToothProj : SnappyTooth
    {
        public override void SetStaticDefaults()
	    {
		    DisplayName.SetDefault("Sky Tooth");
	    }
		
		public override void SetDefaults()
        { 
            Projectile.width = 14;      
            Projectile.height = 20; 
            Projectile.friendly = true;     
            Projectile.DamageType = DamageClass.Magic;     
            Projectile.tileCollide = true;
            Projectile.penetrate = 4;     
            Projectile.timeLeft = 2000;  
            Projectile.light = 0.75f;
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
		}
    }
}