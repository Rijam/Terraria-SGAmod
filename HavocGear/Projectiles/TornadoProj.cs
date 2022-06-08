using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
    public class TornadoProj : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tornado");
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 5f;
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 350f;
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 17f;
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
        
       	public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
       	{

       			Texture2D tex = Mod.Assets.Request<Texture2D>("HavocGear/Projectiles/TornadoProj2").Value;
       			Vector2 drawOrigin = new Vector2(tex.Width,tex.Height/6)/2f;
				Vector2 drawPos = ((Projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
				Color color = Projectile.GetAlpha(lightColor)*0.5f; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				int timing=(int)(Projectile.localAI[0]/8f);
				timing %=6;
				timing*=((tex.Height)/6);
				spriteBatch.Draw(tex, drawPos, new Rectangle(0,timing,tex.Width,(tex.Height-1)/6), color, Projectile.velocity.X*0.04f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
       	}

        public override void AI()
        {

		    int DustID2 = Dust.NewDust(new Vector2(Projectile.Center.X-16f, Projectile.Center.Y - 12f), 32, 24, Mod.Find<ModDust>("TornadoDust").Type, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 20, default(Color)*0.5f, 1f);
            Main.dust[DustID2].noGravity = true;

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
				if (Projectile.localAI[0] > 32f)
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
					Projectile.NewProjectile(value10.X, value10.Y, num438, num439, Mod.Find<ModProjectile>("TornadoProj2").Type, (int)((double)Projectile.damage * 0.75f), Projectile.knockBack, Projectile.owner, 0f, 0f);
					return;
				}
			}
        }
        
    }
}