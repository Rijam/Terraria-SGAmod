using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
    public class TornadoProj2 : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tornado");
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}
        
		public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 500;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.aiStyle = 36;
			Projectile.tileCollide = true;
			Main.projFrames[Projectile.type] = 6;  
        }

	public override bool? CanHitNPC(NPC target){
		if (Projectile.timeLeft<20)
		return false;
		else
		return base.CanHitNPC(target);
	}

	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
    {
    Projectile.timeLeft=(int)MathHelper.Clamp(Projectile.timeLeft,-1,19);
			target.immune[Projectile.owner] = 1;

	}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.timeLeft=(int)MathHelper.Clamp(Projectile.timeLeft,-1,19);
			return false;
		}

        public override void AI()
        {

		    int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y + 2f), Projectile.width + 2, Projectile.height + 2, Mod.Find<ModDust>("TornadoDust").Type, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 20, default(Color), Projectile.Opacity);
            Main.dust[DustID2].noGravity = true;
            Projectile.Opacity=MathHelper.Clamp(((float)Projectile.timeLeft/20),0f,1f);

        	if (Projectile.timeLeft>19){
			float num472 = Projectile.Center.X;
			float num473 = Projectile.Center.Y;
			float num474 = 400f;
			bool flag17 = false;
			for (int num475 = 0; num475 < 200; num475++)
			{
				if (Main.npc[num475].CanBeChasedBy(Projectile, false) && Collision.CanHitLine(Projectile.Center, 1, 1, Main.npc[num475].Center, 1, 1))
				{
					float num476 = Main.npc[num475].position.X + (float)(Main.npc[num475].width / 2);
					float num477 = Main.npc[num475].position.Y + (float)(Main.npc[num475].height / 2);
					float num478 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num476) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num477);
					if (num478 < num474)
					{
						num474 = num478;
						num472 = num476;
						num473 = num477;
						flag17 = true;
					}
				}
			}
			if (flag17)
			{
				float num483 = 6f;
				Vector2 vector35 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num484 = num472 - vector35.X;
				float num485 = num473 - vector35.Y;
				float num486 = (float)Math.Sqrt((double)(num484 * num484 + num485 * num485));
				num486 = num483 / num486;
				num484 *= num486;
				num485 *= num486;
				Projectile.velocity.X = (Projectile.velocity.X * 20f + num484) / 21f;
				Projectile.velocity.Y = (Projectile.velocity.Y * 20f + num485) / 21f;
				return;
			}

			}
        
		}


    }
}