using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Murk
{
	public class SwampSlime : ModNPC
	{
		public override void SetDefaults()
		{
			NPC.width = 38;
			NPC.height = 32;
			NPC.damage = 14;
			NPC.defense = 6;
			NPC.lifeMax = 100;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 60f;
			NPC.knockBackResist = 1.1f;
			NPC.aiStyle = 1;
            NPC.alpha = 0;
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueSlime];
			AIType = NPCID.Crimslime;
			AnimationType = NPCID.BlueSlime;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("DankSlimeBanner").Type;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dank Slime");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 20; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
            {
                int dust = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, 0, NPC.velocity.X * 0f, NPC.velocity.Y * 0f, 80, default(Color), 1f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = false; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 1f;
            }
            if (NPC.life < 1)
            {
                Projectile.NewProjectile(NPC.Center, Vector2.Zero, ModContent.ProjectileType<LessStickyOgreBallOneTick>(),0,0);
            }

        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.AddBuff(ModContent.BuffType<MurkyDepths>(),60*(NPC.AnyNPCs(ModContent.NPCType<Murk>()) ? 5 : 15));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];
            return !spawnInfo.playerInTown && !NPC.BusyWithAnyInvasionOfSorts() && SGAWorld.downedMurk>1 && !spawnInfo.invasion && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse && spawnInfo.spawnTileY < Main.rockLayer && spawnInfo.Player.ZoneJungle ? 0.12f : 0f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(4)<1)
            Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Gel, Main.rand.Next(4));
            if (Main.rand.Next(1+NPC.CountNPCS(Mod.Find<ModNPC>("Murk").Type)*4) < 1)
            Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("MurkyGel").Type, Main.rand.Next(3));
            if (Main.rand.Next(2 + NPC.CountNPCS(Mod.Find<ModNPC>("Murk").Type) * 4) < 1)
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("Biomass").Type, Main.rand.Next(3));
        }
    }
}