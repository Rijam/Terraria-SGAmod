using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Dank
{
	public class SwampBat : ModNPC
	{
		public override void SetDefaults()
		{
			NPC.width = 30;
			NPC.height = 22;
			NPC.damage = 15;
			NPC.defense = 5;
			NPC.lifeMax = 29;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath4;
            NPC.value = 600f;
            NPC.noGravity = true;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 14;
            AnimationType = NPCID.CaveBat;
            AIType = NPCID.CaveBat;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("SwampBatBanner").Type;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swamp Bat");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.CaveBat];
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                Gore.NewGore(NPC.Center, NPC.velocity, Mod.GetGoreSlot("Gores/SwampBat_gib"), 1f);
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(25) == 0)
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.ChainKnife);
            }
            if (Main.rand.Next(4) == 0 && SGAWorld.downedMurk>1)
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("MurkyGel").Type, Main.rand.Next(6));
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType == Mod.Find<ModTile>("MoistStone") .Type&& spawnInfo.Player.SGAPly().DankShrineZone ? 0.75f : 0f;
        }
    }
}
