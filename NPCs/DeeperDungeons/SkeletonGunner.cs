using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace SGAmod.NPCs.DeeperDungeons
{
    public class SkeletonGunner : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skeleton Gunner");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.PirateDeadeye]; //20
        }

        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 48;
            NPC.damage = 40;
            NPC.defense = 9;
            NPC.lifeMax = 120;
            NPC.value = 500f;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0.4f;
            AIType = NPCID.PirateDeadeye;
            AnimationType = NPCID.PirateDeadeye;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.buffImmune[BuffID.Confused] = false;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("SkeletonGunnerBanner").Type;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 16, 0), NPC.velocity, 42, 1f); //Skeleton head gore
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * -16, 0), NPC.velocity, 43, 1f); //Skeleton arm gore
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 8, 0), NPC.velocity, 43, 1f); //Skeleton arm gore
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 8, 0), NPC.velocity, 44, 1f); //Skeleton leg gore

            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(100) <= 98) //98% chance
            {
                for (int i = 0; i <= Main.rand.Next(1, 3); i++)
                {
                    Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Bone);
                }
            }
            if (Main.rand.Next(65) == 0) //1.53% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.GoldenKey);
            }
            if (Main.rand.Next(250) == 0) //0.4% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.BoneWand);
            }
            if (Main.rand.Next(300) == 0) //0.33% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.ClothierVoodooDoll);
            }
            if (Main.rand.Next(100) == 0) //1% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.TallyCounter);
            }
            //additionally, drop the Handgun
            if (Main.rand.Next(50) == 0) //2% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Handgun);
            }
        }
		//Copied from aiStyle 3. Originally ~3800 lines long
		public override void AI()
		{
			bool flagNPCVel0OrJustHit = false;
			if (NPC.velocity.X == 0f)
			{
				flagNPCVel0OrJustHit = true;
			}
			if (NPC.justHit)
			{
				flagNPCVel0OrJustHit = false;
			}
			int num41 = 60;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = true;
			if (!flag6 && flag7)
			{
				if (NPC.velocity.Y == 0f && ((NPC.velocity.X > 0f && NPC.direction < 0) || (NPC.velocity.X < 0f && NPC.direction > 0)))
				{
					flag4 = true;
				}
				if (NPC.position.X == NPC.oldPosition.X || NPC.ai[3] >= (float)num41 || flag4)
				{
					NPC.ai[3] += 1f;
				}
				else if ((double)Math.Abs(NPC.velocity.X) > 0.9 && NPC.ai[3] > 0f)
				{
					NPC.ai[3] -= 1f;
				}
				if (NPC.ai[3] > (float)(num41 * 10))
				{
					NPC.ai[3] = 0f;
				}
				if (NPC.justHit)
				{
					NPC.ai[3] = 0f;
				}
				if (NPC.ai[3] == (float)num41)
				{
					NPC.netUpdate = true;
				}
			}
			if (NPC.ai[3] < (float)num41)
			{
				if (Main.rand.Next(1000) == 0)
				{
					SoundEngine.PlaySound(SoundID.ZombieMoan, (int)NPC.position.X, (int)NPC.position.Y);//idle sound
				}
				NPC.TargetClosest();
			}
			if (true)
			{
				bool flag16 = false;
				bool flag17 = false;
				bool flag18 = true;
				int num145 = -1;
				int num146 = -1;
				if (NPC.confused)
				{
					NPC.ai[2] = 0f;
				}
				else
				{
					if (NPC.ai[1] > 0f)
					{
						NPC.ai[1] -= 1f;
					}
					if (NPC.justHit)
					{
						NPC.ai[1] = 30f;
						NPC.ai[2] = 0f;
					}
					int num147 = 40;
					int num148 = num147 /2;
					if (NPC.ai[2] > 0f)
					{
						if (flag18)
						{
							NPC.TargetClosest();
						}
						if (NPC.ai[1] == num148)
						{
							float num149 = 11f;
							Vector2 vector31 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
							float projectileSpeedX = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector31.X;
							float num151 = 0f;
							float projectileSpeedY = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector31.Y - num151;
							projectileSpeedX += (float)Main.rand.Next(-40, 41);
							projectileSpeedY += (float)Main.rand.Next(-40, 41);
							float num153 = (float)Math.Sqrt(projectileSpeedX * projectileSpeedX + projectileSpeedY * projectileSpeedY);
							NPC.netUpdate = true;
							num153 = num149 / num153;
							projectileSpeedX *= num153;
							projectileSpeedY *= num153;
							int projectileDamage = 20;
							int projectileType = ProjectileID.BulletDeadeye;
							vector31.X += projectileSpeedX;
							vector31.Y += projectileSpeedY;
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								Projectile.NewProjectile(vector31.X, vector31.Y, projectileSpeedX, projectileSpeedY, projectileType, projectileDamage, 1.0f, Main.myPlayer);
							}
							if (Math.Abs(projectileSpeedY) > Math.Abs(projectileSpeedX) * 2f)
							{
								if (projectileSpeedY > 0f)
								{
									NPC.ai[2] = 1f;
								}
								else
								{
									NPC.ai[2] = 5f;
								}
							}
							else if (Math.Abs(projectileSpeedX) > Math.Abs(projectileSpeedY) * 2f)
							{
								NPC.ai[2] = 3f;
							}
							else if (projectileSpeedY > 0f)
							{
								NPC.ai[2] = 2f;
							}
							else
							{
								NPC.ai[2] = 4f;
							}
						}
						if ((NPC.velocity.Y != 0f && !flag17) || NPC.ai[1] <= 0f)
						{
							NPC.ai[2] = 0f;
							NPC.ai[1] = 0f;
						}
						else if (!flag16 || (num145 != -1 && NPC.ai[1] >= (float)num145 && NPC.ai[1] < (float)(num145 + num146) && (!flag17 || NPC.velocity.Y == 0f)))
						{
							NPC.velocity.X *= 0.9f;
							NPC.spriteDirection = NPC.direction;
						}
					}
					if ((NPC.ai[2] <= 0f || flag16) && (NPC.velocity.Y == 0f || flag17) && NPC.ai[1] <= 0f && !Main.player[NPC.target].dead)
					{
						bool flag20 = Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height);
						if (Main.player[NPC.target].stealth == 0f && Main.player[NPC.target].itemAnimation == 0)
						{
							flag20 = false;
						}
						if (flag20)
						{
							float num159 = 10f;
							Vector2 vector32 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
							float num160 = Main.player[NPC.target].position.X + (float)Main.player[NPC.target].width * 0.5f - vector32.X;
							float num161 = Math.Abs(num160) * 0.1f;
							float num162 = Main.player[NPC.target].position.Y + (float)Main.player[NPC.target].height * 0.5f - vector32.Y - num161;
							num160 += (float)Main.rand.Next(-40, 41);
							num162 += (float)Main.rand.Next(-40, 41);
							float num163 = (float)Math.Sqrt(num160 * num160 + num162 * num162);
							float num164 = 700f;
							if (num163 < num164)
							{
								NPC.netUpdate = true;
								NPC.velocity.X *= 0.5f;
								num163 = num159 / num163;
								num160 *= num163;
								num162 *= num163;
								NPC.ai[2] = 3f;
								NPC.ai[1] = num147;
								if (Math.Abs(num162) > Math.Abs(num160) * 2f)
								{
									if (num162 > 0f)
									{
										NPC.ai[2] = 1f;
									}
									else
									{
										NPC.ai[2] = 5f;
									}
								}
								else if (Math.Abs(num160) > Math.Abs(num162) * 2f)
								{
									NPC.ai[2] = 3f;
								}
								else if (num162 > 0f)
								{
									NPC.ai[2] = 2f;
								}
								else
								{
									NPC.ai[2] = 4f;
								}
							}
						}
					}
					if (NPC.ai[2] <= 0f || (flag16 && (num145 == -1 || !(NPC.ai[1] >= (float)num145) || !(NPC.ai[1] < (float)(num145 + num146)))))
					{
						float num165 = 2f;
						float num166 = 0.1f;
						float num167 = 0.8f;
						if (NPC.velocity.X < 0f - num165 || NPC.velocity.X > num165)
						{
							if (NPC.velocity.Y == 0f)
							{
								NPC.velocity *= num167;
							}
						}
						else if (NPC.velocity.X < num165 && NPC.direction == 1)
						{
							NPC.velocity.X += num166;
							if (NPC.velocity.X > num165)
							{
								NPC.velocity.X = num165;
							}
						}
						else if (NPC.velocity.X > 0f - num165 && NPC.direction == -1)
						{
							NPC.velocity.X -= num166;
							if (NPC.velocity.X < 0f - num165)
							{
								NPC.velocity.X = 0f - num165;
							}
						}
					}
				}
			}
			bool flag22 = false;
			if (NPC.velocity.Y == 0f)
			{
				int num171 = (int)(NPC.position.Y + (float)NPC.height + 7f) / 16;
				int num172 = (int)NPC.position.X / 16;
				int num173 = (int)(NPC.position.X + (float)NPC.width) / 16;
				for (int num174 = num172; num174 <= num173; num174++)
				{
					if (Main.tile[num174, num171] == null)
					{
						return;
					}
					if (Main.tile[num174, num171].HasUnactuatedTile && Main.tileSolid[Main.tile[num174, num171].TileType])
					{
						flag22 = true;
						break;
					}
				}
			}
			if (NPC.velocity.Y >= 0f)
			{
				int num175 = 0;
				if (NPC.velocity.X < 0f)
				{
					num175 = -1;
				}
				if (NPC.velocity.X > 0f)
				{
					num175 = 1;
				}
				Vector2 vector34 = NPC.position;
				vector34.X += NPC.velocity.X;
				int num176 = (int)((vector34.X + (float)(NPC.width / 2) + (float)((NPC.width / 2 + 1) * num175)) / 16f);
				int num177 = (int)((vector34.Y + (float)NPC.height - 1f) / 16f);
				if (Main.tile[num176, num177] == null)
				{
					Main.tile[num176, num177] = new Tile();
				}
				if (Main.tile[num176, num177 - 1] == null)
				{
					Main.tile[num176, num177 - 1] = new Tile();
				}
				if (Main.tile[num176, num177 - 2] == null)
				{
					Main.tile[num176, num177 - 2] = new Tile();
				}
				if (Main.tile[num176, num177 - 3] == null)
				{
					Main.tile[num176, num177 - 3] = new Tile();
				}
				if (Main.tile[num176, num177 + 1] == null)
				{
					Main.tile[num176, num177 + 1] = new Tile();
				}
				if (Main.tile[num176 - num175, num177 - 3] == null)
				{
					Main.tile[num176 - num175, num177 - 3] = new Tile();
				}
				if ((float)(num176 * 16) < vector34.X + (float)NPC.width && (float)(num176 * 16 + 16) > vector34.X && ((Main.tile[num176, num177].HasUnactuatedTile && !Main.tile[num176, num177].topSlope() && !Main.tile[num176, num177 - 1].topSlope() && Main.tileSolid[Main.tile[num176, num177].TileType] && !Main.tileSolidTop[Main.tile[num176, num177].TileType]) || (Main.tile[num176, num177 - 1].IsHalfBlock && Main.tile[num176, num177 - 1].HasUnactuatedTile)) && (!Main.tile[num176, num177 - 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num176, num177 - 1].TileType] || Main.tileSolidTop[Main.tile[num176, num177 - 1].TileType] || (Main.tile[num176, num177 - 1].IsHalfBlock && (!Main.tile[num176, num177 - 4].HasUnactuatedTile || !Main.tileSolid[Main.tile[num176, num177 - 4].TileType] || Main.tileSolidTop[Main.tile[num176, num177 - 4].TileType]))) && (!Main.tile[num176, num177 - 2].HasUnactuatedTile || !Main.tileSolid[Main.tile[num176, num177 - 2].TileType] || Main.tileSolidTop[Main.tile[num176, num177 - 2].TileType]) && (!Main.tile[num176, num177 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num176, num177 - 3].TileType] || Main.tileSolidTop[Main.tile[num176, num177 - 3].TileType]) && (!Main.tile[num176 - num175, num177 - 3].HasUnactuatedTile || !Main.tileSolid[Main.tile[num176 - num175, num177 - 3].TileType]))
				{
					float num178 = num177 * 16;
					if (Main.tile[num176, num177].IsHalfBlock)
					{
						num178 += 8f;
					}
					if (Main.tile[num176, num177 - 1].IsHalfBlock)
					{
						num178 -= 8f;
					}
					if (num178 < vector34.Y + (float)NPC.height)
					{
						float num179 = vector34.Y + (float)NPC.height - num178;
						float num180 = 16.1f;
						if (num179 <= num180)
						{
							NPC.gfxOffY += NPC.position.Y + (float)NPC.height - num178;
							NPC.position.Y = num178 - (float)NPC.height;
							if (num179 < 9f)
							{
								NPC.stepSpeed = 1f;
							}
							else
							{
								NPC.stepSpeed = 2f;
							}
						}
					}
				}
			}
			if (flag22)
			{
				int num181 = (int)((NPC.position.X + (float)(NPC.width / 2) + (float)(15 * NPC.direction)) / 16f);
				int num182 = (int)((NPC.position.Y + (float)NPC.height - 15f) / 16f);
				if (Main.tile[num181, num182] == null)
				{
					Main.tile[num181, num182] = new Tile();
				}
				if (Main.tile[num181, num182 - 1] == null)
				{
					Main.tile[num181, num182 - 1] = new Tile();
				}
				if (Main.tile[num181, num182 - 2] == null)
				{
					Main.tile[num181, num182 - 2] = new Tile();
				}
				if (Main.tile[num181, num182 - 3] == null)
				{
					Main.tile[num181, num182 - 3] = new Tile();
				}
				if (Main.tile[num181, num182 + 1] == null)
				{
					Main.tile[num181, num182 + 1] = new Tile();
				}
				if (Main.tile[num181 + NPC.direction, num182 - 1] == null)
				{
					Main.tile[num181 + NPC.direction, num182 - 1] = new Tile();
				}
				if (Main.tile[num181 + NPC.direction, num182 + 1] == null)
				{
					Main.tile[num181 + NPC.direction, num182 + 1] = new Tile();
				}
				if (Main.tile[num181 - NPC.direction, num182 + 1] == null)
				{
					Main.tile[num181 - NPC.direction, num182 + 1] = new Tile();
				}
				Main.tile[num181, num182 + 1].IsHalfBlock;
				if (Main.tile[num181, num182 - 1].HasUnactuatedTile && (TileLoader.IsClosedDoor(Main.tile[num181, num182 - 1]) || Main.tile[num181, num182 - 1].TileType == 388) && flag5)
				{
					NPC.ai[2] += 1f;
					NPC.ai[3] = 0f;
					if (NPC.ai[2] >= 60f)
					{
						NPC.velocity.X = 0.5f * (float)(-NPC.direction);
						int num183 = 5;
						if (Main.tile[num181, num182 - 1].TileType == 388)
						{
							num183 = 2;
						}
						NPC.ai[1] += num183;
						NPC.ai[2] = 0f;
						bool flag23 = false;
						if (NPC.ai[1] >= 10f)
						{
							flag23 = true;
							NPC.ai[1] = 10f;
						}
						WorldGen.KillTile(num181, num182 - 1, fail: true);
						if ((Main.netMode != NetmodeID.MultiplayerClient || !flag23) && flag23 && Main.netMode != NetmodeID.MultiplayerClient)
						{
							if (TileLoader.OpenDoorID(Main.tile[num181, num182 - 1]) >= 0)
							{
								bool flag24 = WorldGen.OpenDoor(num181, num182 - 1, NPC.direction);
								if (!flag24)
								{
									NPC.ai[3] = num41;
									NPC.netUpdate = true;
								}
								if (Main.netMode == NetmodeID.Server && flag24)
								{
									NetMessage.SendData(MessageID.ChangeDoor, -1, -1, null, 0, num181, num182 - 1, NPC.direction);
								}
							}
							if (Main.tile[num181, num182 - 1].TileType == 388)
							{
								bool flag25 = WorldGen.ShiftTallGate(num181, num182 - 1, closing: false);
								if (!flag25)
								{
									NPC.ai[3] = num41;
									NPC.netUpdate = true;
								}
								if (Main.netMode == NetmodeID.Server && flag25)
								{
									NetMessage.SendData(MessageID.ChangeDoor, -1, -1, null, 4, num181, num182 - 1);
								}
							}
						}
					}
				}
				else
				{
					int num184 = NPC.spriteDirection;
					if ((NPC.velocity.X < 0f && num184 == -1) || (NPC.velocity.X > 0f && num184 == 1))
					{
						if (NPC.height >= 32 && Main.tile[num181, num182 - 2].HasUnactuatedTile && Main.tileSolid[Main.tile[num181, num182 - 2].TileType])
						{
							if (Main.tile[num181, num182 - 3].HasUnactuatedTile && Main.tileSolid[Main.tile[num181, num182 - 3].TileType])
							{
								NPC.velocity.Y = -8f;
								NPC.netUpdate = true;
							}
							else
							{
								NPC.velocity.Y = -7f;
								NPC.netUpdate = true;
							}
						}
						else if (Main.tile[num181, num182 - 1].HasUnactuatedTile && Main.tileSolid[Main.tile[num181, num182 - 1].TileType])
						{
							NPC.velocity.Y = -6f;
							NPC.netUpdate = true;
						}
						else if (NPC.position.Y + (float)NPC.height - (float)(num182 * 16) > 20f && Main.tile[num181, num182].HasUnactuatedTile && !Main.tile[num181, num182].topSlope() && Main.tileSolid[Main.tile[num181, num182].TileType])
						{
							NPC.velocity.Y = -5f;
							NPC.netUpdate = true;
						}
						else if (NPC.directionY < 0 && NPC.type != NPCID.Crab && (!Main.tile[num181, num182 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num181, num182 + 1].TileType]) && (!Main.tile[num181 + npc.direction, num182 + 1].HasUnactuatedTile || !Main.tileSolid[Main.tile[num181 + NPC.direction, num182 + 1].TileType]))
						{
							NPC.velocity.Y = -8f;
							NPC.velocity.X *= 1.5f;
							NPC.netUpdate = true;
						}
						else if (flag5)
						{
							NPC.ai[1] = 0f;
							NPC.ai[2] = 0f;
						}
						if (NPC.velocity.Y == 0f && flagNPCVel0OrJustHit && NPC.ai[3] == 1f)
						{
							NPC.velocity.Y = -5f;
						}
					}
				}
			}
			else if (flag5)
			{
				NPC.ai[1] = 0f;
				NPC.ai[2] = 0f;
			}
			if (Main.netMode == NetmodeID.MultiplayerClient || NPC.type != NPCID.ChaosElemental || !(NPC.ai[3] >= (float)num41))
			{
				return;
			}
			int num185 = (int)Main.player[NPC.target].position.X / 16;
			int num186 = (int)Main.player[NPC.target].position.Y / 16;
			int num187 = (int)NPC.position.X / 16;
			int num188 = (int)NPC.position.Y / 16;
			int num189 = 20;
			int num190 = 0;
			bool flag26 = false;
			if (Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) + Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 2000f)
			{
				num190 = 100;
				flag26 = true;
			}
			while (!flag26 && num190 < 100)
			{
				num190++;
				int num191 = Main.rand.Next(num185 - num189, num185 + num189);
				for (int num192 = Main.rand.Next(num186 - num189, num186 + num189); num192 < num186 + num189; num192++)
				{
					if ((num192 < num186 - 4 || num192 > num186 + 4 || num191 < num185 - 4 || num191 > num185 + 4) && (num192 < num188 - 1 || num192 > num188 + 1 || num191 < num187 - 1 || num191 > num187 + 1) && Main.tile[num191, num192].HasUnactuatedTile)
					{
						bool flag27 = true;
						if (Main.tile[num191, num192 - 1].lava())
						{
							flag27 = false;
						}
						if (flag27 && Main.tileSolid[Main.tile[num191, num192].TileType] && !Collision.SolidTiles(num191 - 1, num191 + 1, num192 - 4, num192 - 1))
						{
							NPC.position.X = num191 * 16 - NPC.width / 2;
							NPC.position.Y = num192 * 16 - NPC.height;
							NPC.netUpdate = true;
							NPC.ai[3] = -120f;
						}
					}
				}
			}
		}
	}
}