using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.DeeperDungeons
{
    public class DungeonBat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dungeon Bat");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.CaveBat]; //5
        }

        public override void SetDefaults()
        {
            NPC.width = 22;
            NPC.height = 18;
            NPC.damage = 20;
            NPC.defense = 0;
            NPC.lifeMax = 32;
            NPC.value = 100f;
            NPC.aiStyle = 14;
            NPC.knockBackResist = 0.3f;
            NPC.npcSlots = 0.5f;
            AIType = NPCID.CaveBat;
            AnimationType = NPCID.CaveBat;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath4;
            NPC.buffImmune[BuffID.Confused] = false;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("DungeonBatBanner").Type;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                //Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DungeonBat_Gore"), 1f);
                Gore.NewGore(NPC.position, NPC.velocity + new Vector2(NPC.spriteDirection * -8, 0), Mod.GetGoreSlot("Gores/DungeonBat_Gore"), 1f);
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(250) == 0) //0.4% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.ChainKnife);
            }
            if (Main.rand.Next(100) == 0) //1% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.DepthMeter);
            }
            if (Main.rand.Next(5) == 0) //20% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Bone);
            }
        }
    }
}