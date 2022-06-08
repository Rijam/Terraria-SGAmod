using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.DeeperDungeons
{
    public class FlamingSkull : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flaming Skull");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.DemonEye]; //2
        }

        public override void SetDefaults()
        {
            NPC.width = 22;
            NPC.height = 22;
            NPC.damage = 35;
            NPC.defense = 0;
            NPC.lifeMax = 100;
            NPC.value = 100f;
            NPC.aiStyle = 2;
            NPC.knockBackResist = 0.2f;
            NPC.buffImmune[BuffID.OnFire] = true;
            AIType = NPCID.DemonEye;
            AnimationType = NPCID.DemonEye;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.buffImmune[BuffID.Confused] = false;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("FlamingSkullBanner").Type;
        }
        public override void PostAI()
        {
            if (Main.rand.NextBool())
            {
                Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Fire);
            }
            Lighting.AddLight(NPC.Center, 0.26f, 0.08f, 0.02f);
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                for (int i = 0; i < 30; i++)
                {
                    Dust killDust = Main.dust[Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Fire, 2, 2)];
                    killDust.noGravity = true;
                    Dust killDust2 = killDust;
                    killDust2.velocity *= 2f;
                }
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 16, 0), NPC.velocity, Mod.GetGoreSlot("Gores/HellCaster_Head"), 1f);
            }
        }
        public override Color? GetAlpha(Color newColor)
        {
            return new Color(255, 255, 255, 255);
        }
        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.OnFire, 120);
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