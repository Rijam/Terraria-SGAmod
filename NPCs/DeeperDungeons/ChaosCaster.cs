using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.DeeperDungeons
{
    public class ChaosCaster : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chaos Caster");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Tim]; //3
        }

        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 40;
            NPC.damage = 26;
            NPC.defense = 0;
            NPC.lifeMax = 65;
            NPC.value = 150f;
            NPC.aiStyle = 8;
            NPC.knockBackResist = 0.6f;
            AIType = NPCID.Tim;
            AnimationType = NPCID.Tim;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.buffImmune[BuffID.Confused] = false;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("ChaosCasterBanner").Type;
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
        }
    }
}