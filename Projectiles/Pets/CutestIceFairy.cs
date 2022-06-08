using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles.Pets
{
	public class CutestIceFairy : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cutest Ice Fairy");
			Main.projFrames[Projectile.type] = 8;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.DD2PetGato); //Has 8 frames. But doesn't seem to use the last 4 frames, even in vanilla. Vanilla: Projectile_703
															   //Modified AI to use the last 4 frames if the projectile is moving fast
			Projectile.aiStyle = -1;
			Projectile.width = 24;
			Projectile.height = 24;
			//AIType = ProjectileID.DD2PetGato;
			drawOffsetX = -12;
			drawOriginOffsetY -= 14;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.petFlagDD2Gato = false; // Relic from AIType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (player.HasBuff(ModContent.BuffType<Buffs.Pets.CutestIceFairyBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			Lighting.AddLight(Projectile.Center, 0.0f, 0.18f, 0.25f);

			//Taken from AI_144_DD2Pets() and modified
			float numIs3f = 3f;
			int numOfFrames = Main.projFrames[Projectile.type];
			Vector2 playerDirection = new Vector2(player.direction * 30, -20f);
			playerDirection.Y += (float)Math.Cos(Projectile.localAI[0] * ((float)Math.PI / 30f)) * 2f;
			Projectile.direction = (Projectile.spriteDirection = player.direction);
			Vector2 playerCenterAndDirection = player.MountedCenter + playerDirection;
			float distToProjCenter = Vector2.Distance(Projectile.Center, playerCenterAndDirection);
			if (distToProjCenter > 1000f)
			{
				Projectile.Center = player.Center + playerDirection;
			}
			Vector2 distPlayerToProj = playerCenterAndDirection - Projectile.Center;
			if (distToProjCenter < numIs3f)
			{
				Projectile.velocity *= 0.25f;
			}
			if (distPlayerToProj != Vector2.Zero)
			{
				if (distPlayerToProj.Length() < numIs3f * 0.5f)
				{
					Projectile.velocity = distPlayerToProj;
				}
				else
				{
					Projectile.velocity = distPlayerToProj * 0.1f;
				}
			}
			if (Projectile.velocity.Length() > 6f)
			{
				float num7 = Projectile.velocity.X * 0.08f + Projectile.velocity.Y * Projectile.spriteDirection * 0.02f;
				if (Math.Abs(Projectile.rotation - num7) >= (float)Math.PI)
				{
					if (num7 < Projectile.rotation)
					{
						Projectile.rotation -= (float)Math.PI * 2f;
					}
					else
					{
						Projectile.rotation += (float)Math.PI * 2f;
					}
				}
				float num8 = 12f;
				Projectile.rotation = (Projectile.rotation * (num8 - 1f) + num7) / num8;
				if (Projectile.frameCounter++ >= 2)
				{
					Projectile.frameCounter = 0;
					Projectile.frame++;
				}
			}
			else
			{
				if (Projectile.rotation > (float)Math.PI)
				{
					Projectile.rotation -= (float)Math.PI * 2f;
				}
				if (Projectile.rotation > -0.005f && Projectile.rotation < 0.005f)
				{
					Projectile.rotation = 0f;
				}
				else
				{
					Projectile.rotation *= 0.96f;
				}
				if (Projectile.frameCounter++ >= 4)
				{
					Projectile.frameCounter = 0;
					if (Projectile.frame++ >= numOfFrames)
					{
						Projectile.frame = 0;
					}
				}
			}
			if (Math.Abs(Projectile.velocity.X) > 6f)
			{
				if (Projectile.frame < 4)
				{
					Projectile.frame += 4;
				}
				if (Projectile.frame >= 8)
				{
					Projectile.frame = 4;
				}
			}
			else
			{
				if (Projectile.frame >= 4)
				{
					Projectile.frame = 0;
				}
			}
			Projectile.localAI[0] += 1f;
			if (Projectile.localAI[0] > 120f)
			{
				Projectile.localAI[0] = 0f;
			}
		}
	}
}