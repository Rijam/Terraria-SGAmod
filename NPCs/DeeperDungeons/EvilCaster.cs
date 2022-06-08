using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.DeeperDungeons
{
    public class EvilCaster : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Evil Caster");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.DesertDjinn]; //16
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 48;
            NPC.damage = 40;
            NPC.defense = 7;
            NPC.lifeMax = 600;
            NPC.value = 500f;
            NPC.aiStyle = 8;
            NPC.knockBackResist = 0.3f;
            NPC.buffImmune[BuffID.CursedInferno] = true;
            NPC.buffImmune[BuffID.Ichor] = true;
            AIType = NPCID.DesertDjinn;
            AnimationType = NPCID.DesertDjinn;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.buffImmune[BuffID.Confused] = false;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("EvilCasterBanner").Type;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 16, 0), NPC.velocity, Mod.GetGoreSlot("Gores/EvilCaster_Head"), 1f);
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * -16, 0), NPC.velocity, Mod.GetGoreSlot("Gores/EvilCaster_Arm"), 1f);
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 8, 0), NPC.velocity, Mod.GetGoreSlot("Gores/EvilCaster_Arm"), 1f);
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 8, 0), NPC.velocity, Mod.GetGoreSlot("Gores/EvilCaster_Leg"), 1f);
            }
        }

        public override void NPCLoot()
        {
            
            if (Main.rand.Next(100) < 98) //98% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Bone, Main.rand.Next(1, 3));
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
            if (Main.rand.Next(2) == 0) //50% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.CursedFlame, Main.rand.Next(1, 2));
            }
            if (Main.rand.Next(2) == 0) //50% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Ichor, Main.rand.Next(1, 2));
            }
        }
    }
}