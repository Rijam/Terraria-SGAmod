using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles.Pets
{
	public class SlimeDuchess : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slime Duchess");
			Main.projFrames[Projectile.type] = 12;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.KingSlimePet); in 1.4 //Vanilla Projectile_881
			Projectile.netImportant = true;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft *= 5;
			Projectile.width = 16;
			Projectile.height = 16;
			//AIType = ProjectileID.KingSlimePet;
			//AIType = 0;
			drawOffsetX = 0;
			drawOriginOffsetY -= 1;
		}

		/*public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.petFlagKingSlimePet = false; // Relic from AIType
			return true;
		}*/

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (player.HasBuff(ModContent.BuffType<Buffs.Pets.SlimeDuchessBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			//Taken from 1.4's AI_026

			#region AI
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			int num = 85;
			if (player.position.X + (player.width / 2) < Projectile.position.X + (Projectile.width / 2) - num)
			{
				flag1 = true;
			}
			else if (player.position.X + (player.width / 2) > Projectile.position.X + (Projectile.width / 2) + num)
			{
				flag2 = true;
			}
			if (Projectile.ai[1] == 0f)
			{
				int num69 = 500;
				if (player.rocketDelay2 > 0)
				{
					Projectile.ai[0] = 1f;
				}
				Vector2 val8 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
				float num70 = player.position.X + (player.width / 2) - val8.X;
				float num71 = player.position.Y + (player.height / 2) - val8.Y;
				float num72 = (float)Math.Sqrt(num70 * num70 + num71 * num71);
				if (num72 > 2000f)
				{
					Projectile.position.X = player.position.X + (player.width / 2) - (Projectile.width / 2);
					Projectile.position.Y = player.position.Y + (player.height / 2) - (Projectile.height / 2);
				}
				else if (num72 > num69 || (Math.Abs(num71) > 300f && ((true && true && (false || true)) || !(Projectile.localAI[0] > 0f))))
				{
					if (num71 > 0f && Projectile.velocity.Y < 0f)
					{
						Projectile.velocity.Y = 0f;
					}
					if (num71 < 0f && Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y = 0f;
					}
					Projectile.ai[0] = 1f;
				}
			}
			if (Projectile.ai[0] != 0f)
			{
				float num73 = 0.2f;
				int num74 = 200;
				Projectile.tileCollide = false;
				Vector2 val9 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
				float num75 = player.position.X + (player.width / 2) - val9.X;
				float num81 = player.position.Y + (player.height / 2) - val9.Y;
				float num82 = (float)Math.Sqrt(num75 * num75 + num81 * num81);
				float num84 = 10f;
				if (num82 < num74 && player.velocity.Y == 0f && Projectile.position.Y + Projectile.height <= player.position.Y + player.height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
					if (Projectile.velocity.Y < -6f)
					{
						Projectile.velocity.Y = -6f;
					}
				}
				if (num82 < 60f)
				{
					num75 = Projectile.velocity.X;
					num81 = Projectile.velocity.Y;
				}
				else
				{
					num82 = num84 / num82;
					num75 *= num82;
					num81 *= num82;
				}
				if (Projectile.velocity.X < num75)
				{
					Projectile.velocity.X += num73;
					if (Projectile.velocity.X < 0f)
					{
						Projectile.velocity.X += num73 * 1.5f;
					}
				}
				if (Projectile.velocity.X > num75)
				{
					Projectile.velocity.X -= num73;
					if (Projectile.velocity.X > 0f)
					{
						Projectile.velocity.X -= num73 * 1.5f;
					}
				}
				if (Projectile.velocity.Y < num81)
				{
					Projectile.velocity.Y += num73;
					if (Projectile.velocity.Y < 0f)
					{
						Projectile.velocity.Y += num73 * 1.5f;
					}
				}
				if (Projectile.velocity.Y > num81)
				{
					Projectile.velocity.Y -= num73;
					if (Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y -= num73 * 1.5f;
					}
				}
				if (Projectile.velocity.X > 0.5)
				{
					Projectile.spriteDirection = -1;
				}
				else if (Projectile.velocity.X < -0.5)
				{
					Projectile.spriteDirection = 1;
				}
				//Gore for when it transforms from walking to flying
				/*int num86 = 1226; //1226 The Crown
				if (projectile.type == 934)//Slime Princess
				{
					num86 = 1261;
				}
				if (projectile.frame < 6 || projectile.frame > 11)
				{
					Gore.NewGore(new Vector2(projectile.Center.X, projectile.position.Y), projectile.velocity * 0.5f, num86);
				}*/
				Projectile.frameCounter++;
				if (Projectile.frameCounter > 4)
				{
					Projectile.frame++;
					Projectile.frameCounter = 0;
				}
				if (Projectile.frame < 6 || Projectile.frame > 11)
				{
					Projectile.frame = 6;
				}
				Vector2 v5 = Projectile.velocity;
				v5.Normalize();
				Projectile.rotation = v5.ToRotation() + (float)Math.PI / 2f;
			}
			else
			{
				if (Projectile.ai[1] != 0f)
				{
					flag1 = false;
					flag2 = false;
				}
				Projectile.rotation = 0f;
				Projectile.tileCollide = true;
				float num152 = 6f;
				float num151 = 0.2f;
				if (num152 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
				{
					num152 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
					num151 = 0.3f;
				}
				if (flag1)
				{
					if (Projectile.velocity.X > -3.5)
					{
						Projectile.velocity.X -= num151;
					}
					else
					{
						Projectile.velocity.X -= num151 * 0.25f;
					}
				}
				else if (flag2)
				{
					if (Projectile.velocity.X < 3.5)
					{
						Projectile.velocity.X += num151;
					}
					else
					{
						Projectile.velocity.X += num151 * 0.25f;
					}
				}
				else
				{
					Projectile.velocity.X *= 0.9f;
					if (Projectile.velocity.X >= 0f - num151 && Projectile.velocity.X <= num151)
					{
						Projectile.velocity.X = 0f;
					}
				}
				if (flag1 || flag2)
				{
					int num153 = (int)(Projectile.position.X + (Projectile.width / 2)) / 16;
					int j2 = (int)(Projectile.position.Y + (Projectile.height / 2)) / 16;
					if (Projectile.type == 236)
					{
						num153 += Projectile.direction;
					}
					if (flag1)
					{
						num153--;
					}
					if (flag2)
					{
						num153++;
					}
					num153 += (int)Projectile.velocity.X;
					if (WorldGen.SolidTile(num153, j2))
					{
						flag4 = true;
					}
				}
				if (player.position.Y + player.height - 8f > Projectile.position.Y + Projectile.height)
				{
					flag3 = true;
				}
				if (Projectile.type == 268 && Projectile.frameCounter < 10)
				{
					flag4 = false;
				}
				if (Projectile.type == 860 && Projectile.velocity.X != 0f)
				{
					flag4 = true;
				}
				if (Projectile.velocity.X != 0f)
				{
					flag4 = true;
				}
				Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
				if (Projectile.velocity.Y == 0f)
				{
					if (!flag3 && (Projectile.velocity.X < 0f || Projectile.velocity.X > 0f))
					{
						int i3 = (int)(Projectile.position.X + (Projectile.width / 2)) / 16;
						int j3 = (int)(Projectile.position.Y + (Projectile.height / 2)) / 16 + 1;
						if (flag1)
						{
							i3--;
						}
						if (flag2)
						{
							i3++;
						}
						WorldGen.SolidTile(i3, j3);
					}
					if (flag4)
					{
						int num154 = (int)(Projectile.position.X + (Projectile.width / 2)) / 16;
						int num155 = (int)(Projectile.position.Y + Projectile.height) / 16;
						if (WorldGen.SolidTileAllowBottomSlope(num154, num155) || Main.tile[num154, num155].IsHalfBlock || Main.tile[num154, num155].slope() > 0)
						{
							try
							{
								num154 = (int)(Projectile.position.X + (Projectile.width / 2)) / 16;
								num155 = (int)(Projectile.position.Y + (Projectile.height / 2)) / 16;
								if (flag1)
								{
									num154--;
								}
								if (flag2)
								{
									num154++;
								}
								num154 += (int)Projectile.velocity.X;
								if (!WorldGen.SolidTile(num154, num155 - 1) && !WorldGen.SolidTile(num154, num155 - 2))
								{
									Projectile.velocity.Y = -5.1f;
								}
								else if (!WorldGen.SolidTile(num154, num155 - 2))
								{
									Projectile.velocity.Y = -7.1f;
								}
								else if (WorldGen.SolidTile(num154, num155 - 5))
								{
									Projectile.velocity.Y = -11.1f;
								}
								else if (WorldGen.SolidTile(num154, num155 - 4))
								{
									Projectile.velocity.Y = -10.1f;
								}
								else
								{
									Projectile.velocity.Y = -9.1f;
								}
							}
							catch
							{
								Projectile.velocity.Y = -9.1f;
							}
						}
					}
				}
				if (Projectile.velocity.X > num152)
				{
					Projectile.velocity.X = num152;
				}
				if (Projectile.velocity.X < 0f - num152)
				{
					Projectile.velocity.X = 0f - num152;
				}
				if (Projectile.velocity.X < 0f)
				{
					Projectile.direction = -1;
				}
				if (Projectile.velocity.X > 0f)
				{
					Projectile.direction = 1;
				}
				if (Projectile.velocity.X > num151 && flag2)
				{
					Projectile.direction = 1;
				}
				if (Projectile.velocity.X < 0f - num151 && flag1)
				{
					Projectile.direction = -1;
				}
				if (Projectile.direction == -1)
				{
					Projectile.spriteDirection = 1;
				}
				if (Projectile.direction == 1)
				{
					Projectile.spriteDirection = -1;
				}
				Projectile.spriteDirection = 1;
				if (player.Center.X < Projectile.Center.X)
				{
					Projectile.spriteDirection = -1;
				}
				if (Projectile.velocity.Y > 0f)
				{
					Projectile.frameCounter++;
					if (Projectile.frameCounter > 2)
					{
						Projectile.frame++;
						if (Projectile.frame >= 2)
						{
							Projectile.frame = 2;
						}
						Projectile.frameCounter = 0;
					}
				}
				else if (Projectile.velocity.Y < 0f)
				{
					Projectile.frameCounter++;
					if (Projectile.frameCounter > 2)
					{
						Projectile.frame++;
						if (Projectile.frame >= 5)
						{
							Projectile.frame = 0;
						}
						Projectile.frameCounter = 0;
					}
				}
				else if (Projectile.frame == 0)
				{
					Projectile.frame = 0;
				}
				else if (++Projectile.frameCounter > 3)
				{
					Projectile.frame++;
					if (Projectile.frame >= 6)
					{
						Projectile.frame = 0;
					}
					Projectile.frameCounter = 0;
				}
				if (Projectile.wet && player.position.Y + player.height < Projectile.position.Y + Projectile.height && Projectile.localAI[0] == 0f)
				{
					if (Projectile.velocity.Y > -4f)
					{
						Projectile.velocity.Y -= 0.2f;
					}
					if (Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y *= 0.95f;
					}
				}
				else
				{
					Projectile.velocity.Y += 0.4f;
				}
				if (Projectile.velocity.Y > 10f)
				{
					Projectile.velocity.Y = 10f;
				}
			}
			#endregion
		}
	}
}