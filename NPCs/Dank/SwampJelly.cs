using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Dank
{
	public class SwampJelly : ModNPC
	{
		public override void SetDefaults()
		{
			NPC.width = 26;
			NPC.height = 28;
			NPC.damage = 24;
			NPC.defense = 2;
			NPC.lifeMax = 18;
            NPC.HitSound = SoundID.NPCHit25;
            NPC.DeathSound = SoundID.NPCDeath18;
            NPC.noGravity = true;
			NPC.value = 80f;
			NPC.knockBackResist = 0.5f;
			NPC.aiStyle = 18;
			AIType = NPCID.GreenJellyfish;
			AnimationType = NPCID.GreenJellyfish;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("SwampJellyBanner").Type;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swamp Jellyfish");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.GreenJellyfish];
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Glowstick, Main.rand.Next(4));
            if (Main.rand.Next(50) < 1)
                Item.NewItem(NPC.position, NPC.Hitbox.Size(), ModContent.ItemType<Items.Accessories.MurkyCharm>());
        }

        public override bool CheckDead()
        {
            Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/SwampJelly_1"), 1f);
            Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/SwampJelly_2"), 1f);
            Gore.NewGore(NPC.position, NPC.velocity, Mod.GetGoreSlot("Gores/SwampJelly_3"), 1f);
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            int x = spawnInfo.spawnTileX;
            int y = spawnInfo.spawnTileY;
			return SGAUtils.NoInvasion(spawnInfo) && spawnInfo.water && spawnInfo.Player.SGAPly().DankShrineZone ? 2f : 0f;
		}
	}
}
