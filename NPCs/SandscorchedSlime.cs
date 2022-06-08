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

	public class SandscorchedSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandscorched Slime");
			Main.npcFrameCount[NPC.type] = 2;
		}

		public override void SetDefaults()
		{
			NPC.lifeMax = 800;
			NPC.defense = 22;
			NPC.damage = 58;
			NPC.scale = 1f;
			NPC.width = 36;
			NPC.height = 26;
			AnimationType = 1;
			NPC.aiStyle = 1;
			NPC.knockBackResist = 0.4f;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.CursedInferno] = true;
			NPC.buffImmune[BuffID.ShadowFlame] = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[Mod.Find<ModBuff>("ThermalBlaze").Type] = true;
			NPC.npcSlots = 0.1f;
			NPC.netAlways = true;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = Item.buyPrice(0, 0, 5);
			banner = NPC.type;
			bannerItem = Mod.Find<ModItem>("SandscorchedSlimeBanner").Type;
		}

		public override void AI()
		{
			for (int k = 0; k < 1; k++)
            		{
                		int dust = Dust.NewDust(NPC.position - new Vector2(8f, 8f), NPC.width + 5, NPC.height + 4, Mod.Find<ModDust>("HotDust").Type, 0.6f, 0.5f, 0, default(Color), 1.0f);
				Main.dust[dust].noGravity = true;				
				Main.dust[dust].velocity *= 0.0f;
            		}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("FieryShard").Type, Main.rand.Next(1, 3));
		}
        	
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(Mod.Find<ModBuff>("ThermalBlaze").Type, 200, true);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY-3];
			return !spawnInfo.playerInTown && !NPC.BusyWithAnyInvasionOfSorts() && !NPC.BusyWithAnyInvasionOfSorts() && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse && spawnInfo.spawnTileY < Main.rockLayer && spawnInfo.Player.ZoneDesert && !spawnInfo.Player.ZoneBeach && Main.hardMode ? 0.25f : 0f;
		}

        }
}