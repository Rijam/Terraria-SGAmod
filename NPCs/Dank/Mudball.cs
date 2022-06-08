using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Dank
{
	public class MudBall : ModNPC
	{
		public override void SetDefaults()
		{
			NPC.width = 26;
			NPC.height = 32;
			NPC.damage = 21;
			NPC.defense = 7;
			NPC.lifeMax = 32;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 40f;
			NPC.knockBackResist = 0.7f;
			NPC.aiStyle = 2;
            AnimationType = NPCID.DemonEye;
            AIType = NPCID.DemonEye;
			banner = NPC.type;
			bannerItem = Mod.Find<ModItem>("MudBallBanner").Type;
		}

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mudball");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
			for (int num654 = 0; num654 < (NPC.life<1 ? 16 : 1); num654++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
				int num655 = Dust.NewDust(NPC.position + Vector2.UnitX * -20f, NPC.width + 40, NPC.height, 184, NPC.velocity.X + randomcircle.X * 8f, NPC.velocity.Y + randomcircle.Y * 8f, 100, new Color(30, 30, 30, 20), 2f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].velocity *= 0.5f;
			}
		}

        public override void NPCLoot()
        {
			for (int num172 = 0; num172 < Main.maxPlayers; num172 += 1)
			{
				Player target = Main.player[num172];
				if (target != null && target.active)
				{
					float damagefalloff = 1f - ((target.Center - NPC.Center).Length() / 120f);
					if ((target.Center - NPC.Center).Length() < 90f)
					{
						target.AddBuff(BuffID.OgreSpit, 60 + (int)(60f * damagefalloff * 5f));
					}
				}
			}
			if (Main.rand.Next(4) == 0 && SGAWorld.downedMurk > 1)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("DankWood").Type, Main.rand.Next(1,16));
			}
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int spawn = Main.rand.Next(1,3);
			return spawn == 2 && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType==Mod.Find<ModTile>("MoistStone") .Type&& spawnInfo.Player.SGAPly().DankShrineZone ? 1.60f : 0f;
		}
	}
}
