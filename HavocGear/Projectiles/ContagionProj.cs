using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
	public class ContagionProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Contagion");
		}

		public override void SetDefaults()
		{
			Projectile.width = 42;
			Projectile.height = 42;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 5;
			Projectile.ownerHitCheck = true;
			Projectile.aiStyle = 19;
			Projectile.DamageType = DamageClass.Melee;  
			Projectile.timeLeft = 90;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Main.player[Projectile.owner].direction = Projectile.direction;
			Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
			Main.player[Projectile.owner].itemTime = Main.player[Projectile.owner].itemAnimation;
			Projectile.position.X = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - (float)(Projectile.width / 2);
			Projectile.position.Y = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - (float)(Projectile.height / 2);
			Projectile.position += Projectile.velocity * Projectile.ai[0];
			if (Main.rand.Next(4) == 0)
			{
				int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.AcidDust>(), 0f, 0f, 254, default(Color), 0.3f);
				Main.dust[dustIndex].velocity += Projectile.velocity * 0.5f;
				Main.dust[dustIndex].velocity *= 0.5f;
				return;
			}
			if(Projectile.ai[0] == 0f)
			{
				Projectile.ai[0] = 3f;
				Projectile.netUpdate = true;
			}
			if(Main.player[Projectile.owner].itemAnimation < Main.player[Projectile.owner].itemAnimationMax / 3)
			{
				Projectile.ai[0] -= 2.4f;
				if (Projectile.localAI[0] == 0f && Main.myPlayer == Projectile.owner)
				{
					Projectile.localAI[0] = 1f;
					Projectile.NewProjectile(Projectile.Center.X + Projectile.velocity.X, Projectile.Center.Y + Projectile.velocity.Y, Projectile.velocity.X * 1f, Projectile.velocity.Y * 1f, Mod.Find<ModProjectile>("AcidSpear").Type, (int)((double)Projectile.damage * 0.5), Projectile.knockBack * 0.85f, Projectile.owner, 0f, 0f);
				}
			}
			else
			{
				Projectile.ai[0] += 0.95f;
			}
			
			if(Main.player[Projectile.owner].itemAnimation == 0)
			{
				Projectile.Kill();
			}
			
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 2.355f;
			if(Projectile.spriteDirection == -1)
			{
				Projectile.rotation -= 1.57f;
			}
			Lighting.AddLight(Projectile.position, 0.3f, 0.5f, 0f);
		}
    }
}