using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Dank
{
    public class MaggotFly : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BeeSmall);
            NPC.width = 40;
            NPC.height = 30;
            NPC.damage = 84;
            NPC.defense = 25;
            NPC.lifeMax = 500;
            NPC.knockBackResist = 0f;
            NPC.value = 1500f;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.noGravity = true;
            NPC.aiStyle = 5;
            AIType = NPCID.BeeSmall;
            AnimationType = NPCID.BeeSmall;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("MaggotFlyBanner").Type;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 4, 0), NPC.velocity, Mod.GetGoreSlot("Gores/MaggotFly_gib"), 1f);
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Maggot Fly");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BeeSmall];
        }
    }
}
