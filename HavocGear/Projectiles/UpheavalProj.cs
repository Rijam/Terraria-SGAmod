using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
    public class UpheavalProj : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Upheaval");
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 4.5f;
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 320f;
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 16f;
		}
       
	    public override void SetDefaults()
        {
        	Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.TheEyeOfCthulhu);
			Projectile.extraUpdates = 0;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = 99;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
        }
        
        public override void AI()
        {

        int dust;
        for (int i = 0; i < 3; i++){
        dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type);
        Main.dust[dust].scale = 0.8f;
        Main.dust[dust].noGravity = true;
        Main.dust[dust].velocity = Projectile.velocity*(float)(Main.rand.Next(20,100+(i*40))*0.005f);
    	}

        dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type);
        Main.dust[dust].scale = 1.4f;
        Main.dust[dust].noGravity = true;
        Main.dust[dust].velocity = Projectile.velocity*(float)(Main.rand.Next(20,100)*0.002f);

		Lighting.AddLight(Projectile.position, 0.6f, 0.5f, 0f);

            int[] array = new int[20];
			int num428 = 0;
			float num429 = 300f;
			bool flag14 = false;
			for (int num430 = 0; num430 < 200; num430++)
			{
				if (Main.npc[num430].CanBeChasedBy(Projectile, false))
				{
					float num431 = Main.npc[num430].position.X + (float)(Main.npc[num430].width / 2);
					float num432 = Main.npc[num430].position.Y + (float)(Main.npc[num430].height / 2);
					float num433 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num431) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num432);
					if (num433 < num429 && Collision.CanHitLine(Projectile.Center, 1, 1, Main.npc[num430].Center, 1, 1))
					{
						if (num428 < 20)
						{
							array[num428] = num430;
							num428++;
						}
						flag14 = true;
					}
				}
			}
			if (flag14)
			{
				int num434 = Main.rand.Next(num428);
				num434 = array[num434];
				float num435 = Main.npc[num434].position.X + (float)(Main.npc[num434].width / 2);
				float num436 = Main.npc[num434].position.Y + (float)(Main.npc[num434].height / 2);
				Projectile.localAI[0] += 1f;
				if (Projectile.localAI[0] > 64f)
				{
					Projectile.localAI[0] = 0f;
					float num437 = 6f;
					Vector2 value10 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
					value10 += Projectile.velocity * 4f;
					float num438 = num435 - value10.X;
					float num439 = num436 - value10.Y;
					float num440 = (float)Math.Sqrt((double)(num438 * num438 + num439 * num439));
					num440 = num437 / num440;
					num438 *= num440;
					num439 *= num440;
					int theproj=Projectile.NewProjectile(value10.X, value10.Y, num438, num439, Mod.Find<ModProjectile>("HotBoulder").Type, (int)((double)Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, 0f, 0f);
					Main.projectile[theproj].timeLeft=60;
					return;
				}
			}
        }
        
    }
}