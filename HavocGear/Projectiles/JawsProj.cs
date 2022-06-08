using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
    public class JawsProj : ModProjectile
    {
    	int started=0;
    	Projectile [] orbitors={null,null,null,null,null,null};
    	int [] spinners={-6,-6,-6,-6,-6,-6};
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jaws");
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 240f;
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 15f;
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 3f;
		}
       
	    public override void SetDefaults()
        {
        	Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Amarok);
			Projectile.extraUpdates = 0;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = 99;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
        }

		public override void SendExtraAI(System.IO.BinaryWriter writer)
		{
			for (int i = 0; i < orbitors.Length; i += 1)
			{
				writer.Write((ushort)orbitors[i].whoAmI);
			}
		}

		public override void ReceiveExtraAI(System.IO.BinaryReader reader)
		{
			for (int i = 0; i < orbitors.Length; i += 1)
			{
				int theyare = (int)reader.ReadUInt16();
				Projectile proj = Main.projectile[theyare];
				if (proj != null && proj.active && proj.type == ModContent.ProjectileType<SnappyTooth>())
				orbitors[i] = Main.projectile[theyare];
			}
		}

		public override void AI()
        {
			for (int k = 0; k < 5; k += 1)
			{

				if (spinners[k] == -6)
				{
					spinners[k] = 1;
					int newb = Projectile.NewProjectile(Projectile.Center, new Vector2(0f, 0f), ModContent.ProjectileType<SnappyTooth>(), (int)(Projectile.damage * 1f), Projectile.knockBack, Main.myPlayer, 0f, (float)Main.player[Projectile.owner].whoAmI);
					Main.projectile[newb].penetrate = 4;
					// Main.projectile[newb].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
					Main.projectile[newb].DamageType = DamageClass.Melee;
					Main.projectile[newb].usesIDStaticNPCImmunity = true;
					Main.projectile[newb].idStaticNPCHitCooldown = 10;
					Main.projectile[newb].netUpdate = true;
					orbitors[k] = Main.projectile[newb];
					Projectile.netUpdate = true;
				}
				else
				{
					if (orbitors[k] != null)
					{
						if (orbitors[k].type == Mod.Find<ModProjectile>("SnappyTooth").Type)
						{
							double anglez = (k / ((double)5f));
							double angle = ((float)(started / 5f)) + 2.0 * Math.PI * anglez;
							Vector2 loc = new Vector2(-1f + (float)((Math.Cos(angle) * 16f)), (float)((Math.Sin(angle) * 16f)));
							Vector2 gohere = Projectile.Center + loc;
							orbitors[k].Center = gohere + Projectile.velocity;
							orbitors[k].timeLeft = 3;
							orbitors[k].velocity = loc * 0.05f;

						}
					}
				}
			}



		started+=1;
		}


    }
}