using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Projectiles
{

	public class GasCloud : ModProjectile
	{

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			//projectile.aiStyle = 1;
			Projectile.friendly = true;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.tileCollide=false;
		}

				public override string Texture
		{
			get { return "Terraria/Item_" + 5; }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override bool? CanHitNPC(NPC target){
		return false;
	}
	public override bool CanHitPlayer(Player target){
		return false;
	}
	public override void AI()
	{
		Projectile.velocity=new Vector2(Projectile.velocity.X,Projectile.velocity.Y*0.95f);
		int q=0;
			for (q = 0; q < 1; q++)
				{

					int dust = Dust.NewDust(Projectile.position-new Vector2(50,0), 100, 40, 74, 0f, Projectile.velocity.Y * 0.4f, 100, Color.DarkGreen*0.15f, 2.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].fadeIn = 2f;
					//Main.dust[dust].velocity *= 1.8f;
					//Main.dust[dust].velocity.Y -= 0.5f;
					//Main.playerDrawDust.Add(dust);
				}

            int minTilePosX = (int)(Projectile.position.X / 16.0) - 1;
            int maxTilePosX = (int)((Projectile.position.X + 2) / 16.0) + 2;
            int minTilePosY = (int)(Projectile.position.Y / 16.0) - 1;
            int maxTilePosY = (int)((Projectile.position.Y + 2) / 16.0) + 2;

            int whereisity;
            whereisity=Idglib.RaycastDown(minTilePosX+1,minTilePosY);
            //Main.NewText(""+(whereisity-minTilePosY),255,255,255);
            Projectile.position.Y+=whereisity-minTilePosY>2 ? 1 : 0;


                Rectangle rectangle1 = new Rectangle((int)Projectile.Center.X-40, (int)Projectile.Center.Y-60, 100, 40);
                int maxDistance = 50;
                bool playerCollision = false;
                for (int index = 0; index < Main.maxNPCs; ++index)
                {
                    if (Main.npc[index].active)
                    {
                        Rectangle rectangle2 = new Rectangle((int)Main.npc[index].position.X - Main.npc[index].width, (int)Main.npc[index].position.Y - Main.npc[index].height, Main.npc[index].height * 2, Main.npc[index].width*2);
                        if (rectangle1.Intersects(rectangle2))
                        {
						if (Main.npc[index].GetGlobalNPC<SGAnpcs>().Combusted<1)
						IdgNPC.AddBuffBypass(index,Mod.Find<ModBuff>("DosedInGas").Type, 60*8, true);
                        }
                    }
                }

	}


}


	/*public class GasCloudGore : ModGore
	{
		public override void OnSpawn(Gore gore)
		{
			gore.velocity = new Vector2(Main.rand.NextFloat() - 0.5f, Main.rand.NextFloat() * MathHelper.TwoPi);
			gore.numFrames = 8;
			gore.frame = (byte)Main.rand.Next(8);
			gore.frameCounter = (byte)Main.rand.Next(8);
			updateType = 910;
		}
	}*/




}