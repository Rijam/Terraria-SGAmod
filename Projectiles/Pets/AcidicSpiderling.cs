using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles.Pets
{
	public class AcidicSpiderling : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acidic Spiderling");
			Main.projFrames[Projectile.type] = 11;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.VenomSpider); //a little buggy because its a minion and not a pet
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.friendly = true;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			//AIType = ProjectileID.VenomSpider;
			AIType = 0;
			//drawOriginOffsetY -= 6;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			//player.petFlagDD2Gato = false; // Relic from AIType
			//player.spiderMinion = false;
			//player.numMinions -= 1;
			return true;
		}

		/*
		 * Frames
		 * 0-3 walking on floor
		 * 4-7 walking on wall
	     * 8-10 flying
		 */

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (player.HasBuff(ModContent.BuffType<Buffs.Pets.AcidicSpiderlingBuff>()))
			{
				Projectile.timeLeft = 2;
			}



			//Taken from AI style 26
			//stills needs a lot of cleaning up. Originally was >1200 lines, now <850
			//Known bugs:
			//	doesn't teleport to the player when they are too far away.
			//	Doesn't start flying to stay near the play if the player is far away vertically (seems to happen with the spider summons too)
			//	Faces right when then player is idle and facing the left (only on floor)
			if (!Main.player[Projectile.owner].active)
			{
				Projectile.active = false;
				return;
			}
			bool flag1 = false;
			bool flag2 = false;
			bool flag3PlayerAboveProjectile = false;
			bool flag4ProjectileInSolidTile = false;
			int num;
			num = 10;
			int num2 = 40 * (Projectile.minionPos + 1) * Main.player[Projectile.owner].direction;
			if (Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) < Projectile.position.X + (float)(Projectile.width / 2) - (float)num + (float)num2)
			{
				flag1 = true;
			}
			else if (Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) > Projectile.position.X + (float)(Projectile.width / 2) + (float)num + (float)num2)
			{
				flag2 = true;
			}
			if (Projectile.ai[0] != 0f)
			{
				float num40 = 0.2f;
				int num41 = 200;
				Projectile.tileCollide = false;
				Vector2 vector7 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
				float num42 = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - vector7.X;
				num42 -= (float)(40 * Main.player[Projectile.owner].direction);
				bool flag6ChaseNPC = false;
				int num44 = -1;
				//For chasing other NPCs to attack
				/*for (int j = 0; j < 200; j++)
				{
					if (!Main.npc[j].CanBeChasedBy(projectile))
					{
						continue;
					}
					float num45 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
					float num46 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
					if (Math.Abs(Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - num45) + Math.Abs(Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - num46) < num43)
					{
						if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[j].position, Main.npc[j].width, Main.npc[j].height))
						{
							num44 = j;
						}
						flag6ChaseNPC = true;
						break;
					}
				}*/
				if (!flag6ChaseNPC)
				{
					num42 -= (float)(40 * Projectile.minionPos * Main.player[Projectile.owner].direction);
				}
				if (flag6ChaseNPC && num44 >= 0)
				{
					Projectile.ai[0] = 0f;
				}
				float num47 = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - vector7.Y;
				float num48 = (float)Math.Sqrt(num42 * num42 + num47 * num47);
				float num49 = 10f;
				if (num48 < (float)num41 && Main.player[Projectile.owner].velocity.Y == 0f && Projectile.position.Y + (float)Projectile.height <= Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
					if (Projectile.velocity.Y < -6f)
					{
						Projectile.velocity.Y = -6f;
					}
				}
				if (num48 < 60f)
				{
					num42 = Projectile.velocity.X;
					num47 = Projectile.velocity.Y;
				}
				else
				{
					num48 = num49 / num48;
					num42 *= num48;
					num47 *= num48;
				}
				if (Projectile.velocity.X < num42)
				{
					Projectile.velocity.X += num40;
					if (Projectile.velocity.X < 0f)
					{
						Projectile.velocity.X += num40 * 1.5f;
					}
				}
				if (Projectile.velocity.X > num42)
				{
					Projectile.velocity.X -= num40;
					if (Projectile.velocity.X > 0f)
					{
						Projectile.velocity.X -= num40 * 1.5f;
					}
				}
				if (Projectile.velocity.Y < num47)
				{
					Projectile.velocity.Y += num40;
					if (Projectile.velocity.Y < 0f)
					{
						Projectile.velocity.Y += num40 * 1.5f;
					}
				}
				if (Projectile.velocity.Y > num47)
				{
					Projectile.velocity.Y -= num40;
					if (Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y -= num40 * 1.5f;
					}
				}
				// set sprite direction
				if ((double)Projectile.velocity.X > 0.5)
				{
					Projectile.spriteDirection = -1;
				}
				else if ((double)Projectile.velocity.X < -0.5)
				{
					Projectile.spriteDirection = 1;
				}
				int num51 = (int)(Projectile.Center.X / 16f);
				int num52 = (int)(Projectile.Center.Y / 16f);
				if (Main.tile[num51, num52] != null && Main.tile[num51, num52].WallType > 0)
				{
					Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
					Projectile.frameCounter += (int)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y));
					if (Projectile.frameCounter > 5)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame > 7)
					{
						Projectile.frame = 4;
					}
					if (Projectile.frame < 4)
					{
						Projectile.frame = 7;
					}
				}
				else
				{
					Projectile.frameCounter++;
					if (Projectile.frameCounter > 2)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame < 8 || Projectile.frame > 10)
					{
						Projectile.frame = 8;
					}
					Projectile.rotation = Projectile.velocity.X * 0.1f;
				}
				//The Spiders don't usually have this dust, but I think its cool.

				int num55 = Dust.NewDust(new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 4f, Projectile.position.Y + (float)(Projectile.height / 2) - 4f) - Projectile.velocity, 8, 8, 163, (0f - Projectile.velocity.X) * 0.5f, Projectile.velocity.Y * 0.5f, 50, default(Color), 1.7f); //DustID.PoionStaff
				Main.dust[num55].velocity.X = Main.dust[num55].velocity.X * 0.2f;
				Main.dust[num55].velocity.Y = Main.dust[num55].velocity.Y * 0.2f;
				Main.dust[num55].noGravity = true;

				return;
			}
			bool flag7 = false;
			Vector2 vector9 = Vector2.Zero;
			bool flag8IsOnWall = false;

			//The position behind the player. "40 * projectile.minionPos;" in vanilla
			float num80 = 60;
			int num81 = 60;
			Projectile.localAI[0] -= 1f;
			if (Projectile.localAI[0] < 0f)
			{
				Projectile.localAI[0] = 0f;
			}
			if (Projectile.ai[1] > 0f)
			{
				Projectile.ai[1] -= 1f;
			}
			else
			{
				float num82 = Projectile.position.X;
				float num83 = Projectile.position.Y;
				float num84 = 100000f;
				float num85 = num84;
				int num86 = -1;
				//For chasing other NPCs to attack
				/*NPC ownerMinionAttackTargetNPC2 = projectile.OwnerMinionAttackTargetNPC;
				if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(projectile))
				{
					float x = ownerMinionAttackTargetNPC2.Center.X;
					float y = ownerMinionAttackTargetNPC2.Center.Y;
					float num87 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - x) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - y);
					if (num87 < num84)
					{
						if (num86 == -1 && num87 <= num85)
						{
							num85 = num87;
							num82 = x;
							num83 = y;
						}
						if (Collision.CanHit(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
						{
							num84 = num87;
							num82 = x;
							num83 = y;
							num86 = ownerMinionAttackTargetNPC2.whoAmI;
						}
					}
				}
				if (num86 == -1)
				{
					for (int m = 0; m < 200; m++)
					{
						if (!Main.npc[m].CanBeChasedBy(projectile))
						{
							continue;
						}
						float num88 = Main.npc[m].position.X + (float)(Main.npc[m].width / 2);
						float num89 = Main.npc[m].position.Y + (float)(Main.npc[m].height / 2);
						float num90 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num88) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num89);
						if (num90 < num84)
						{
							if (num86 == -1 && num90 <= num85)
							{
								num85 = num90;
								num82 = num88;
								num83 = num89;
							}
							if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[m].position, Main.npc[m].width, Main.npc[m].height))
							{
								num84 = num90;
								num82 = num88;
								num83 = num89;
								num86 = m;
							}
						}
					}
				}*/
				if (!Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.tileCollide = true;
				}
				if (num86 == -1 && num85 < num84)
				{
					num84 = num85;
				}
				else if (num86 >= 0)
				{
					flag7 = true;
					vector9 = new Vector2(num82, num83) - Projectile.Center;
					if (Main.npc[num86].position.Y > Projectile.position.Y + (float)Projectile.height)
					{
						int num91 = (int)(Projectile.Center.X / 16f);
						int num92 = (int)((Projectile.position.Y + (float)Projectile.height + 1f) / 16f);
						if (Main.tile[num91, num92] != null && Main.tile[num91, num92].HasTile && TileID.Sets.Platforms[Main.tile[num91, num92].TileType])
						{
							Projectile.tileCollide = false;
						}
					}
					Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
					Rectangle value = new Rectangle((int)Main.npc[num86].position.X, (int)Main.npc[num86].position.Y, Main.npc[num86].width, Main.npc[num86].height);
					int num93 = 10;
					value.X -= num93;
					value.Y -= num93;
					value.Width += num93 * 2;
					value.Height += num93 * 2;
					if (rectangle.Intersects(value))
					{
						flag8IsOnWall = true;
						Vector2 vector10 = Main.npc[num86].Center - Projectile.Center;
						if (Projectile.velocity.Y > 0f && vector10.Y < 0f)
						{
							Projectile.velocity.Y *= 0.5f;
						}
						if (Projectile.velocity.Y < 0f && vector10.Y > 0f)
						{
							Projectile.velocity.Y *= 0.5f;
						}
						if (Projectile.velocity.X > 0f && vector10.X < 0f)
						{
							Projectile.velocity.X *= 0.5f;
						}
						if (Projectile.velocity.X < 0f && vector10.X > 0f)
						{
							Projectile.velocity.X *= 0.5f;
						}
						if (vector10.Length() > 14f)
						{
							vector10.Normalize();
							vector10 *= 14f;
						}
						Projectile.rotation = (Projectile.rotation * 5f + vector10.ToRotation() + (float)Math.PI / 2f) / 6f;
						Projectile.velocity = (Projectile.velocity * 9f + vector10) / 10f;
						for (int n = 0; n < 1000; n++)
						{
							if (Projectile.whoAmI != n && Projectile.owner == Main.projectile[n].owner && (Main.projectile[n].type >= 390 && Main.projectile[n].type <= 392 || true) && (Main.projectile[n].Center - Projectile.Center).Length() < 15f)
							{
								float num94 = 0.5f;
								if (Projectile.Center.Y > Main.projectile[n].Center.Y)
								{
									Main.projectile[n].velocity.Y -= num94;
									Projectile.velocity.Y += num94;
								}
								else
								{
									Main.projectile[n].velocity.Y += num94;
									Projectile.velocity.Y -= num94;
								}
								if (Projectile.Center.X > Main.projectile[n].Center.X)
								{
									Projectile.velocity.X += num94;
									Main.projectile[n].velocity.X -= num94;
								}
								else
								{
									Projectile.velocity.X -= num94;
									Main.projectile[n].velocity.Y += num94;
								}
							}
						}
					}
				}
				float num95;
				num95 = 500f;
				if ((double)Projectile.position.Y > Main.worldSurface * 16.0)
				{
					num95 = 250f;
				}
				if (num84 < num95 + num80 && num86 == -1)
				{
					float num96 = num82 - (Projectile.position.X + (float)(Projectile.width / 2));
					if (num96 < -5f)
					{
						flag1 = true;
						flag2 = false;
					}
					else if (num96 > 5f)
					{
						flag2 = true;
						flag1 = false;
					}
				}
				bool flag9 = false;
				if (Projectile.localAI[1] > 0f)
				{
					flag9 = true;
					Projectile.localAI[1] -= 1f;
				}
				if (num86 >= 0 && num84 < 800f + num80)
				{
					Projectile.friendly = true;
					Projectile.localAI[0] = num81;
					float num97 = num82 - (Projectile.position.X + (float)(Projectile.width / 2));
					if (num97 < -10f)
					{
						flag1 = true;
						flag2 = false;
					}
					else if (num97 > 10f)
					{
						flag2 = true;
						flag1 = false;
					}
					if (num83 < Projectile.Center.Y - 100f && num97 > -50f && num97 < 50f && Projectile.velocity.Y == 0f)
					{
						float num98 = Math.Abs(num83 - Projectile.Center.Y);
						if (num98 < 120f)
						{
							Projectile.velocity.Y = -10f;
						}
						else if (num98 < 210f)
						{
							Projectile.velocity.Y = -13f;
						}
						else if (num98 < 270f)
						{
							Projectile.velocity.Y = -15f;
						}
						else if (num98 < 310f)
						{
							Projectile.velocity.Y = -17f;
						}
						else if (num98 < 380f)
						{
							Projectile.velocity.Y = -18f;
						}
					}
					if (flag9)
					{
						Projectile.friendly = false;
						if (Projectile.velocity.X < 0f)
						{
							flag1 = true;
						}
						else if (Projectile.velocity.X > 0f)
						{
							flag2 = true;
						}
					}
				}
				else
				{
					Projectile.friendly = false;
				}
			}
			if (Projectile.ai[1] != 0f)
			{
				flag1 = false;
				flag2 = false;
			}
			else if (true)
			{
				int num99 = (int)(Projectile.Center.X / 16f);
				int num100 = (int)(Projectile.Center.Y / 16f);
				if (Main.tile[num99, num100] != null && Main.tile[num99, num100].WallType > 0)
				{
					flag1 = (flag2 = false);
				}
			}
			if (!flag8IsOnWall)
			{
				Projectile.rotation = 0f;
			}
			float num101;
			float num102;
			num102 = 6f;
			num101 = 0.2f;
			if (num102 < Math.Abs(Main.player[Projectile.owner].velocity.X) + Math.Abs(Main.player[Projectile.owner].velocity.Y))
			{
				num102 = Math.Abs(Main.player[Projectile.owner].velocity.X) + Math.Abs(Main.player[Projectile.owner].velocity.Y);
				num101 = 0.3f;
			}
			num101 *= 2f;
			if (flag1)
			{
				if ((double)Projectile.velocity.X > -3.5)
				{
					Projectile.velocity.X -= num101;
				}
				else
				{
					Projectile.velocity.X -= num101 * 0.25f;
				}
			}
			else if (flag2)
			{
				if ((double)Projectile.velocity.X < 3.5)
				{
					Projectile.velocity.X += num101;
				}
				else
				{
					Projectile.velocity.X += num101 * 0.25f;
				}
			}
			else
			{
				Projectile.velocity.X *= 0.9f;
				if (Projectile.velocity.X >= 0f - num101 && Projectile.velocity.X <= num101)
				{
					Projectile.velocity.X = 0f;
				}
			}
			if (flag1 || flag2)
			{
				int num103 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
				int j2 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
				if (flag1)
				{
					num103--;
				}
				if (flag2)
				{
					num103++;
				}
				num103 += (int)Projectile.velocity.X;
				if (WorldGen.SolidTile(num103, j2))
				{
					flag4ProjectileInSolidTile = true;
				}
			}
			if (Main.player[Projectile.owner].position.Y + (float)Main.player[Projectile.owner].height - 8f > Projectile.position.Y + (float)Projectile.height)
			{
				flag3PlayerAboveProjectile = true;
			}
			Collision.StepUp(ref Projectile.position, ref Projectile.velocity, Projectile.width, Projectile.height, ref Projectile.stepSpeed, ref Projectile.gfxOffY);
			if (Projectile.velocity.Y == 0f)
			{
				if (!flag3PlayerAboveProjectile && (Projectile.velocity.X < 0f || Projectile.velocity.X > 0f))
				{
					int num104 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
					int j3 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16 + 1;
					if (flag1)
					{
						num104--;
					}
					if (flag2)
					{
						num104++;
					}
					WorldGen.SolidTile(num104, j3);
				}
				if (flag4ProjectileInSolidTile)
				{
					int num105 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
					int num106 = (int)(Projectile.position.Y + (float)Projectile.height) / 16 + 1;
					if (WorldGen.SolidTile(num105, num106) || Main.tile[num105, num106].IsHalfBlock || Main.tile[num105, num106].slope() > 0)
					{
						try
						{
							num105 = (int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16;
							num106 = (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16;
							if (flag1)
							{
								num105--;
							}
							if (flag2)
							{
								num105++;
							}
							num105 += (int)Projectile.velocity.X;
							if (!WorldGen.SolidTile(num105, num106 - 1) && !WorldGen.SolidTile(num105, num106 - 2))
							{
								Projectile.velocity.Y = -5.1f;
							}
							else if (!WorldGen.SolidTile(num105, num106 - 2))
							{
								Projectile.velocity.Y = -7.1f;
							}
							else if (WorldGen.SolidTile(num105, num106 - 5))
							{
								Projectile.velocity.Y = -11.1f;
							}
							else if (WorldGen.SolidTile(num105, num106 - 4))
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
			if (Projectile.velocity.X > num102)
			{
				Projectile.velocity.X = num102;
			}
			if (Projectile.velocity.X < 0f - num102)
			{
				Projectile.velocity.X = 0f - num102;
			}
			if (Projectile.velocity.X < 0f)
			{
				Projectile.direction = -1;
			}
			if (Projectile.velocity.X > 0f)
			{
				Projectile.direction = 1;
			}
			if (Projectile.velocity.X > num101 && flag2)
			{
				Projectile.direction = 1;
			}
			if (Projectile.velocity.X < 0f - num101 && flag1)
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
			int num118 = (int)(Projectile.Center.X / 16f);
			int num119 = (int)(Projectile.Center.Y / 16f);
			if (Main.tile[num118, num119] != null && Main.tile[num118, num119].WallType > 0)
			{
				Projectile.position.Y += Projectile.height;
				Projectile.height = 34;
				Projectile.position.Y -= Projectile.height;
				Projectile.position.X += Projectile.width / 2;
				Projectile.width = 34;
				Projectile.position.X -= Projectile.width / 2;
				float num120 = 9f;
				float num121 = 40 * (Projectile.minionPos + 1);
				Vector2 vector12 = Main.player[Projectile.owner].Center - Projectile.Center;
				if (flag7)
				{
					vector12 = vector9;
					num121 = 10f;
				}
				else if (!Collision.CanHitLine(Projectile.Center, 1, 1, Main.player[Projectile.owner].Center, 1, 1))
				{
					Projectile.ai[0] = 1f;
				}
				if (vector12.Length() < num121)
				{
					Projectile.velocity *= 0.9f;
					if ((double)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) < 0.1)
					{
						Projectile.velocity *= 0f;
					}
				}
				else if (vector12.Length() < 800f || !flag7)
				{
					Projectile.velocity = (Projectile.velocity * 9f + Vector2.Normalize(vector12) * num120) / 10f;
				}
				if (vector12.Length() >= num121)
				{
					Projectile.spriteDirection = Projectile.direction;
					Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
				}
				else
				{
					Projectile.rotation = vector12.ToRotation() + (float)Math.PI / 2f;
				}
				Projectile.frameCounter += (int)(Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y));
				if (Projectile.frameCounter > 5)
				{
					Projectile.frame++;
					Projectile.frameCounter = 0;
				}
				if (Projectile.frame > 7)
				{
					Projectile.frame = 4;
				}
				if (Projectile.frame < 4)
				{
					Projectile.frame = 7;
				}
				return;
			}
			if (!flag8IsOnWall)
			{
				Projectile.rotation = 0f;
			}
			if (Projectile.direction == -1)
			{
				Projectile.spriteDirection = 1;
			}
			if (Projectile.direction == 1)
			{
				Projectile.spriteDirection = -1;
			}
			Projectile.position.Y += Projectile.height;
			Projectile.height = 30;
			Projectile.position.Y -= Projectile.height;
			Projectile.position.X += Projectile.width / 2;
			Projectile.width = 30;
			Projectile.position.X -= Projectile.width / 2;
			if (!flag7 && !Collision.CanHitLine(Projectile.Center, 1, 1, Main.player[Projectile.owner].Center, 1, 1))
			{
				Projectile.ai[0] = 1f;
			}
			if (!flag8IsOnWall && Projectile.frame >= 4 && Projectile.frame <= 7)
			{
				Vector2 vector13 = Main.player[Projectile.owner].Center - Projectile.Center;
				if (flag7)
				{
					vector13 = vector9;
				}
				float num122 = 0f - vector13.Y;
				if (!(vector13.Y > 0f))
				{
					if (num122 < 120f)
					{
						Projectile.velocity.Y = -10f;
					}
					else if (num122 < 210f)
					{
						Projectile.velocity.Y = -13f;
					}
					else if (num122 < 270f)
					{
						Projectile.velocity.Y = -15f;
					}
					else if (num122 < 310f)
					{
						Projectile.velocity.Y = -17f;
					}
					else if (num122 < 380f)
					{
						Projectile.velocity.Y = -18f;
					}
				}
			}
			if (flag8IsOnWall)
			{
				Projectile.frameCounter++;
				if (Projectile.frameCounter > 3)
				{
					Projectile.frame++;
					Projectile.frameCounter = 0;
				}
				if (Projectile.frame >= 8)
				{
					Projectile.frame = 4;
				}
				if (Projectile.frame <= 3)
				{
					Projectile.frame = 7;
				}
			}
			else if (Projectile.velocity.Y >= 0f && (double)Projectile.velocity.Y <= 0.8)
			{
				if (Projectile.velocity.X == 0f)
				{
					Projectile.frame = 0;
					Projectile.frameCounter = 0;
				}
				else if ((double)Projectile.velocity.X < -0.8 || (double)Projectile.velocity.X > 0.8)
				{
					Projectile.frameCounter += (int)Math.Abs(Projectile.velocity.X);
					Projectile.frameCounter++;
					if (Projectile.frameCounter > 5)
					{
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					if (Projectile.frame > 2)
					{
						Projectile.frame = 0;
					}
				}
				else
				{
					Projectile.frame = 0;
					Projectile.frameCounter = 0;
				}
			}
			else
			{
				Projectile.frameCounter = 0;
				Projectile.frame = 3;
			}
			Projectile.velocity.Y += 0.4f;
			if (Projectile.velocity.Y > 10f)
			{
				Projectile.velocity.Y = 10f;
			}
		}
	}
}