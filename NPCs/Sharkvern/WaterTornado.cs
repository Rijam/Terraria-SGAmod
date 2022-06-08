using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Sharkvern
{
    public class WaterTornado : ModProjectile
    {
    	public float timeToCount = 150f;
    	
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 500;
			Projectile.aiStyle = 36;
            Main.projFrames[Projectile.type] = 6;
            Projectile.damage = 18;
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Tornado");
		}
		
		public override void AI()
        {
            Projectile.Opacity=MathHelper.Clamp(((float)Projectile.timeLeft/timeToCount),0f,1f);
           int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width + 2, Projectile.height + 2, Mod.Find<ModDust>("TornadoDust").Type, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 20, default(Color)*Projectile.Opacity, 1f);
            Main.dust[DustID2].noGravity = true;
		
		}

	}
}