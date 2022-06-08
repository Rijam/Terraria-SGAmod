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

namespace SGAmod.NPCs
{

	public class SandscorchedGolem : ModNPC
	{
		float framevar=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandscorched Golem");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.lifeMax = 1000;
			NPC.defense = 22;
			NPC.damage = 65;
			NPC.scale = 1f;
			NPC.width = 48;
			NPC.height = 56;
			AnimationType = -1;
			NPC.aiStyle = 3;
			NPC.knockBackResist = 0.4f;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.ShadowFlame] = true;
			NPC.buffImmune[BuffID.CursedInferno] = true;
			NPC.buffImmune[Mod.Find<ModBuff>("ThermalBlaze").Type] = true;
			NPC.npcSlots = 0.1f;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = Item.buyPrice(0, 0, 50);
			banner = NPC.type;
			bannerItem = Mod.Find<ModItem>("SandscorchedGolemBanner").Type;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life < 1)
			{
				Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * -4, 6), NPC.velocity, Mod.GetGoreSlot("Gores/SandscorchedGolem_leg_gib"), 1f);
				Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * -4, -2), NPC.velocity, Mod.GetGoreSlot("Gores/SandscorchedGolem_arm_gib"), 1f);
				Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 4, -2), NPC.velocity, Mod.GetGoreSlot("Gores/SandscorchedGolem_arm_gib"), 1f);
				Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 4, 6), NPC.velocity, Mod.GetGoreSlot("Gores/SandscorchedGolem_leg_gib"), 1f);
				Gore.NewGore(NPC.Center, NPC.velocity, Mod.GetGoreSlot("Gores/SandscorchedGolem_chest_gib"), 1f);
				Gore.NewGore(NPC.Center + new Vector2(0, -18), NPC.velocity, Mod.GetGoreSlot("Gores/SandscorchedGolem_head_gib"), 1f);
			}
		}

		public override void AI()
		{
			NPC.TargetClosest(false);
			for (int k = 0; k < 1; k++)
            		{
                		int dust = Dust.NewDust(NPC.position - new Vector2(8f, 8f), NPC.width + 6, NPC.height + 8, Mod.Find<ModDust>("HotDust").Type, 0.6f, 0.5f, 0, default(Color), 1.0f);
				Main.dust[dust].noGravity = true;				
				Main.dust[dust].velocity *= 0.0f;
            		}
            		Player target=Main.player[NPC.target];
            	if (target!=null && (!target.dead)){
            	NPC.spriteDirection=target.position.X-NPC.position.X>0f ? 1 : -1;
            	}else{
            	NPC.spriteDirection=NPC.velocity.X>0 ? 1 : -1;
           		}
           		NPC.velocity+=new Vector2((float)NPC.spriteDirection*0.25f,0f);
           		 NPC.velocity=new Vector2(MathHelper.Clamp(NPC.velocity.X/1.05f,-10f,10f),NPC.velocity.Y);
			if (NPC.velocity.Length()<2)
				NPC.velocity.Y =-5f;

			framevar +=Math.Abs(NPC.velocity.X)*0.05f;
		}

				public override void FindFrame(int frameHeight)
		{
			NPC.frame.Y=(((int)framevar%4))*NPC.height;
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("FieryShard").Type, Main.rand.Next(3, 13));
		}

		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(Mod.Find<ModBuff>("ThermalBlaze").Type, 200, true);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY-3];
			return !spawnInfo.playerInTown && !NPC.BusyWithAnyInvasionOfSorts() && !NPC.BusyWithAnyInvasionOfSorts() && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse && spawnInfo.spawnTileY < Main.rockLayer && spawnInfo.Player.ZoneDesert && !spawnInfo.Player.ZoneBeach && Main.hardMode ? 0.15f : 0f;
		}


        }
}