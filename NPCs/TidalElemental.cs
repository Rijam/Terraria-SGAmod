using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.NPCs
{

	public class TidalElemental : ModNPC
	{
		private int framevar = 0;
		public int outofwater = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Elemental");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.lifeMax = 800;
			NPC.defense = 4;
			NPC.damage = 32;
			NPC.scale = 1f;
			NPC.width = 86;
			NPC.height = 86;
			AnimationType = -1;
			NPC.aiStyle = -1;
			NPC.knockBackResist = 0.4f;
			NPC.npcSlots = 0.1f;
			NPC.netAlways = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = Item.buyPrice(gold: 1);
			NPC.noTileCollide = false;
			NPC.noGravity = true;
			NPC.netUpdate = true;
			banner = NPC.type;
			bannerItem = Mod.Find<ModItem>("TidalElementalBanner").Type;
			NPC.rarity = 1;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life < 1)
			{
				Gore.NewGore(NPC.Center + new Vector2(-NPC.spriteDirection*16,24), NPC.velocity, Mod.GetGoreSlot("Gores/TidalElemental_tail_gib"), 1f);
				Gore.NewGore(NPC.Center + new Vector2(0, -16), NPC.velocity, Mod.GetGoreSlot("Gores/TidalElemental_head_gib"), 1f);
				Gore.NewGore(NPC.Center, NPC.velocity, Mod.GetGoreSlot("Gores/TidalElemental_arm_gib"), 1f);
			}
		}

		public override void AI()
		{

			int num254 = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
			int num255 = (int)(NPC.position.Y + (float)(NPC.height / 2)) / 16;
			NPC.ai[0] = NPC.ai[0] + 1;
			NPC.ai[1] = 0;
			if (Main.tile[num254, num255 + 1] != null && Main.tile[num254, num255 + 1].liquid > 64)
			{
				NPC.ai[1] = 1;
			}
			if (Main.tile[num254, num255 - 1] == null)
			{
				Main.tile[num254, num255 - 1] = new Tile();
			}
			if (Main.tile[num254, num255 + 1] == null)
			{
				Main.tile[num254, num255 + 1] = new Tile();
			}
			if (Main.tile[num254, num255 + 2] == null)
			{
				Main.tile[num254, num255 + 2] = new Tile();
			}
			num255 = (int)(NPC.position.Y + (float)(NPC.height)) / 16;
			if (Main.tile[num254, num255 + 1].HasTile)
			{
				if (Main.tile[num254, num255 + 1].liquid < 64)
				{
					NPC.ai[1] = -1;
				}
			}

			if (NPC.ai[1] == -1 && outofwater < 300)
			{
				NPC.velocity = new Vector2(NPC.velocity.X, NPC.velocity.Y - Main.rand.Next(6, 15));
			}


			Player P = Main.player[NPC.target];
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(false);
				P = Main.player[NPC.target];
				if (!P.active || P.dead)
				{
					NPC.spriteDirection = NPC.velocity.X > 0 ? 1 : -1;
				}

			}
			else
			{
				bool RageShark = false;
				foreach(NPC npc in Main.npc.Where(testby => testby.active && testby.type == ModContent.NPCType<Sharkvern.SharkvernHead>()))
                {
					if ((npc.ModNPC as Sharkvern.SharkvernHead).RainFight)
						RageShark = true;
                }
				if (RageShark)
				{
					NPC.damage = 80;
					NPC.defense = 30;
				}
				Vector2 dista = P.Center - NPC.Center;
				Vector2 dista2 = P.Center - NPC.Center;
				//Vector2 dista2=dista;
				dista2.Normalize();
				NPC.velocity += (dista2 * (NPC.ai[1] == 1 ? 0.2f : 0.4f));
				NPC.spriteDirection = dista2.X > 0 ? 1 : -1;
				if (outofwater < 300)
				{
					outofwater += (NPC.ai[1] == 0 ? 1 : 0) + ((Collision.CanHitLine(new Vector2(NPC.Center.X, NPC.Center.Y), 16, 32, new Vector2(P.Center.X, P.Center.Y), 16, 32)) ? 0 : 1)+(RageShark ? 1 : 0);
				}
				if (NPC.ai[1] == -1 && outofwater > 299 && dista.Y < -30f)
				{
					NPC.velocity = new Vector2(NPC.velocity.X, NPC.velocity.Y - Main.rand.Next(6, 15));
				}
				if (NPC.ai[0] % 400 > 250 && NPC.ai[0] % 60 == 0 && Collision.CanHitLine(new Vector2(NPC.Center.X, NPC.Center.Y), 4, 4, new Vector2(P.Center.X, P.Center.Y), 4, 4))
				{
					List<Projectile> itz = Idglib.Shattershots(NPC.Center, P.position, new Vector2(P.width, P.height), Mod.Find<ModProjectile>("ThrownTrident").Type, RageShark ? 60 : 20, 8, 0, 1, true, 0, true, 400);
					itz[0].damage /= 2;
				}

			}

			if (outofwater > 299)
			{
				outofwater += 1;
				if (outofwater == 300)
				{
					Vector2 dista2 = P.Center - NPC.Center;
					NPC.velocity = new Vector2(NPC.velocity.X, dista2.Y > -100 ? 16f : -16f);
				}
				if (outofwater > 350)
					NPC.ai[1] = 1;
				if (outofwater > 500)
				{
					outofwater = 0;

				}
				NPC.noTileCollide = true;
			}
			else
			{
				NPC.noTileCollide = false;
			}
			NPC.velocity = NPC.velocity * 0.98f;

			if (NPC.velocity.Length() > 14)
			{
				NPC.velocity.Normalize();
				NPC.velocity = NPC.velocity * 14;
			}
			//Main.NewText(""+outofwater,255,255,255);
			if (NPC.ai[1] == 1)
			{
				NPC.rotation = 0f;
			}
			if (NPC.ai[1] == 0)
			{
				NPC.velocity = new Vector2(NPC.velocity.X, NPC.velocity.Y + 1f);
				NPC.rotation = (NPC.velocity.Y * 0.03f) * NPC.spriteDirection;
			}
			NPC.noGravity = NPC.ai[1] == 1 ? true : false;
		}

		public override void FindFrame(int frameHeight)
		{

			if (NPC.ai[0] % 15 == 0)
				framevar = framevar + 1;
			if (framevar > 3)
				framevar = 0;
			NPC.frame.Y = framevar * NPC.height;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY - 3];
			bool canspawn = tile.liquid > 63 ? true : false;
			//!NPC.BusyWithAnyInvasionOfSorts() 
			return !spawnInfo.playerInTown && !spawnInfo.invasion && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse && spawnInfo.spawnTileY < Main.rockLayer && spawnInfo.Player.ZoneBeach && canspawn && NPC.downedBoss1 ? 0.05f : 0f;
		}

		public override void NPCLoot()
		{
			int pick = Main.rand.Next(0, 4);
			if (pick == 0)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("StarfishBlaster").Type, 1);
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, 2626, Main.rand.Next(50, 100));
			}
			if (pick == 1)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("ThrownTrident").Type, 1);
			}
			if (pick == 2)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("TidalWave").Type, 1);
			}
			if (pick == 3)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("TidalCharm").Type, 1);
			}
		}
	}
}