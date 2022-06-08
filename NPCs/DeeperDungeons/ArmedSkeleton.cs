using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.DeeperDungeons
{
    public class ArmedSkeleton : ModNPC
    {
        public override bool Autoload(ref string name) { return false; }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Armed Skeleton");
            Main.npcFrameCount[NPC.type] = 7;
        }

        public override void SetDefaults()
        {
            NPC.width = 24;
            NPC.height = 48;
            NPC.damage = 50;
            NPC.defense = 7;
            NPC.lifeMax = 80;
            NPC.value = 100f;
            NPC.aiStyle = 3;
            NPC.knockBackResist = 0.5f;
            AIType = NPCID.ArmedZombie;
            AnimationType = NPCID.ArmedZombie;
            NPC.HitSound = SoundID.NPCHit2;
            NPC.DeathSound = SoundID.NPCDeath2;
            NPC.buffImmune[BuffID.Confused] = false;
            banner = Item.NPCtoBanner(NPCID.Skeleton);
            bannerItem = Item.BannerToItem(banner);
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
        //For some reason, cloning the Armed Zombie doesn't include the extra melee reach. Copied from vanilla.
        public override void AI()
        {
            //Main.NewText("npc.ai[2] " + npc.ai[2]);
            Rectangle npcRect = NPC.Hitbox;
            if (NPC.ai[2] > 5f)
            {
                int num = 34;
                if (NPC.spriteDirection < 0)
                {
                    npcRect.X -= num;
                    npcRect.Width += num;
                }
                else
                {
                    npcRect.Width += num;
                }
            }
        }


        //Vanilla. NPCID 430 through 436 are the different armed zombies
        /*public static void GetMeleeCollisionData(Rectangle victimHitbox, int enemyIndex, ref int specialHitSetter, ref float damageMultiplier, ref Rectangle npcRect)
        {
            NPC nPC = Main.npc[enemyIndex];
            if (nPC.type >= 430 && nPC.type <= 436 && nPC.ai[2] > 5f)
            {
                int num = 34;
                if (nPC.spriteDirection < 0)
                {
                    npcRect.X -= num;
                    npcRect.Width += num;
                }
                else
                {
                    npcRect.Width += num;
                }
                damageMultiplier *= 1.25f;
            }
        }*/

        public override void NPCLoot()
        {
            if (Main.rand.Next(25) == 0) //4% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Hook);
            }
            //if (Main.rand.Next(150) == 0) //0.67% chance
            //{
            //    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CartonOfMilk);
            //}
            if (Main.rand.Next(100) == 0) //1% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.AncientIronHelmet);
            }
            if (Main.rand.Next(200) == 0) //0.5% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.AncientGoldHelmet);
            }
            if (Main.rand.Next(201) == 0) //0.49% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.BoneSword);
            }
            if (Main.rand.Next(500) == 0) //0.2% chance
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Skull);
            }
        }
    }
}