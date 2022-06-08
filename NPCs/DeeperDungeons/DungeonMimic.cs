using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.DeeperDungeons
{
    public class DungeonMimic : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dungeon Mimic");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Mimic];
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Mimic);
            AIType = NPCID.Mimic;
            AnimationType = NPCID.Mimic;
            NPC.buffImmune[BuffID.Confused] = false;
            banner = Item.NPCtoBanner(NPCID.Mimic);
            bannerItem = Item.BannerToItem(banner);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 16, 0), NPC.velocity, Main.rand.Next(61, 63), 1f); //Smoke gore
            }
        }

        public override void NPCLoot()
        {
            List<int> itemx = Dimensions.DeeperDungeon.CommonItems.ToList();
            itemx.AddRange(Dimensions.DeeperDungeon.RareItems);
            int itemz = itemx[Main.rand.Next(itemx.Count)];

            Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, itemz, itemz == ModContent.ItemType<Items.Weapons.Almighty.Megido>() ? Main.rand.Next(1, 3) : 1);

        }
    }
}