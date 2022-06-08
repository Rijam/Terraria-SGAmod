using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Dank
{
	public class Maggot : ModNPC
	{
        int counter, counter2 = 0;
		public override void SetDefaults()
		{
			NPC.width = 20;
			NPC.height = 20;
			NPC.damage = 0;
			NPC.defense = 100;
			NPC.lifeMax = 25;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 0f;
            NPC.noTileCollide = false;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 66;
			AIType = NPCID.Worm;
			AnimationType = NPCID.Worm;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("MaggotBanner").Type;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Maggot");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Worm];
        }

        public override bool PreAI()
        {
            if (counter++ == 1000)
            {
                NPC.Transform(Mod.Find<ModNPC>("MaggotFly").Type);
                counter = 0;
            }
            if (counter2++ == 15)
            {
                NPC.scale += 0.01f;
                counter2 = 0;
            }
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int spawn = Main.rand.Next(30);
            return Main.hardMode && (spawnInfo.spawnTileType==Mod.Find<ModTile>("MoistStone") .Type&& spawn<15) ? 1f : 0f;
        }
    }
}
