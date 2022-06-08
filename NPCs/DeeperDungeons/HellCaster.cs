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
    public class HellCaster : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hell Caster");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.FireImp]; //10
        }

        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 48;
            NPC.damage = 30;
            NPC.defense = 9;
            NPC.lifeMax = 75;
            NPC.value = 150f;
            NPC.aiStyle = 8;
            NPC.knockBackResist = 0.4f;
            NPC.buffImmune[BuffID.OnFire] = true;
            AIType = NPCID.FireImp;
            AnimationType = NPCID.FireImp;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.buffImmune[BuffID.Confused] = false;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("HellCasterBanner").Type;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * -16, 0), NPC.velocity, Mod.GetGoreSlot("Gores/HellCaster_Arm"), 1f);
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 8, 0), NPC.velocity, Mod.GetGoreSlot("Gores/HellCaster_Arm"), 1f);
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 8, 0), NPC.velocity, Mod.GetGoreSlot("Gores/HellCaster_Leg"), 1f);
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool())
            {
                NPC.NewNPC((int)NPC.position.X, (int)NPC.position.Y, Mod.Find<ModNPC>("FlamingSkull").Type);
                SoundEngine.PlaySound(SoundID.Zombie, NPC.position, 53);
            }
            else
            {
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 16, 0), NPC.velocity, Mod.GetGoreSlot("Gores/HellCaster_Head"), 1f);
            }
            
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