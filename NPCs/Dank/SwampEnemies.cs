using Idglibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace SGAmod.NPCs.Dank
{

    public class FlySwarm : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Bee);
            NPC.width = 28;
            NPC.height = 28;
            NPC.damage = 8;
            NPC.defense = 2;
            NPC.lifeMax = 10;
            NPC.value = 0f;
            NPC.noGravity = true;
            NPC.aiStyle = 5;
            AIType = NPCID.Bee;
            AnimationType = NPCID.Bee;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("FlySwarmBanner").Type;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fly Swarm");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Bee];
        }

        public override bool CheckDead()
        {
            int y = (int)(NPC.position.Y);
            int x = (int)(NPC.position.X + 20);
            for (int i = 0; i < Main.rand.Next(4, 6); i++)
            {
                int num5 = NPC.NewNPC(x, y, Mod.Find<ModNPC>("Fly").Type, 0, 0f, 0f, 0f, 0f, 255);
                Main.npc[num5].velocity.X = Main.rand.Next(-3, 4);
                Main.npc[num5].velocity.Y = Main.rand.Next(-3, 4);
            }
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int spawn = Main.rand.Next(1, 6);
            return spawn == 3 && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType == Mod.Find<ModTile>("MoistStone") .Type&& spawnInfo.Player.SGAPly().DankShrineZone ? 2f : 0f;
        }
    }
    public class SwampBigMimic : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BigMimicHallow);
            NPC.damage = 126;
            NPC.defense = 34;
            NPC.lifeMax = 3500;
            NPC.value = 15000f;
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BigMimicHallow];
            AIType = NPCID.BigMimicHallow;
            AnimationType = NPCID.BigMimicHallow;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("DankMimicBanner").Type;
            NPC.rarity = 2;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dank Mimic");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BigMimicHallow];
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int rand = Main.rand.Next(1, 95);
            return Main.hardMode && rand == 45 && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType == Mod.Find<ModTile>("MoistStone") .Type&& spawnInfo.Player.SGAPly().DankShrineZone ? 0.75f : 0f;
        }

        public override void NPCLoot()
        {
            int rand = Main.rand.Next(5);
            if (rand == 0)
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("Treepeater").Type);
            }
            if (rand == 1)
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("SwampSovnya").Type);
            }        
            if (rand == 2)
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("SludgeBomb").Type, Main.rand.Next(40, 120));
            }        
            if (rand == 3)
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("EarthbreakerShield").Type);
            }         
            if (rand == 4)
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("StickySituationSummon").Type);
            }
        }
    }

    public class SwampLizard : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.width = 78;
            NPC.height = 27;
            NPC.damage = 73;
            NPC.defense = 34;
            NPC.lifeMax = 300;
            NPC.value = 100f;
            NPC.aiStyle = 3;
            AIType = NPCID.Unicorn;
            AnimationType = NPCID.BloodZombie; //Changed from Zombie to BloodZombie
            NPC.HitSound = SoundID.NPCHit1; //New addition
            // npc.DeathSound = SoundID.NPCDeath1; //New addition
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("GiantLizardBanner").Type;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 16, 0), NPC.velocity, Mod.GetGoreSlot("Gores/GiantLizard_head_gib"), 1f);
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * -16, 0), NPC.velocity, Mod.GetGoreSlot("Gores/GiantLizard_tail_gib"), 1f);
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 8, 0), NPC.velocity, Mod.GetGoreSlot("Gores/GiantLizard_leg_gib"), 1f);
                Gore.NewGore(NPC.Center + new Vector2(NPC.spriteDirection * 8, 0), NPC.velocity, Mod.GetGoreSlot("Gores/GiantLizard_leg_gib"), 1f);

            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Lizard");
            Main.npcFrameCount[NPC.type] = 9; //Changed from 8 to 9
        }

        public override void NPCLoot()
        {
            Microsoft.Xna.Framework.Audio.SoundEffectInstance snd = SoundEngine.PlaySound(SoundID.DD2_WyvernDeath, (int)NPC.Center.X, (int)NPC.Center.Y);
            if (snd != null)
            {
                snd.Pitch = -0.50f;
            }

            if (Main.rand.Next(100) == 0)
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.LizardEgg);
            }
            if (Main.rand.Next(5) == 0 && SGAWorld.downedMurk > 1)
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("Biomass").Type, Main.rand.Next(1, 12));
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int rand = Main.rand.Next(2);
            return Main.hardMode && rand == 1 && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType == Mod.Find<ModTile>("MoistStone") .Type&& spawnInfo.Player.SGAPly().DankShrineZone ? 1.25f : 0f;
        }
    }

    public class BlackLeech : ModNPC
    {
        Vector2 offset = default;
        public override void SetDefaults()
        {
            NPC.width = 18;
            NPC.height = 8;
            NPC.damage = 0;
            NPC.defense = 2;
            NPC.lifeMax = 5;
            NPC.noTileCollide = false;
            NPC.noGravity = true;
            NPC.npcSlots = 0.15f;
            NPC.HitSound = SoundID.NPCHit9;
            NPC.DeathSound = SoundID.NPCDeath11;
            NPC.value = 0f;
            NPC.aiStyle = 3;
            Main.npcFrameCount[NPC.type] = 2;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("BlackLeechBanner").Type;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black Leech");
            Main.npcFrameCount[NPC.type] = 2;
        }

        public override void AI()
        {
            NPC.rotation = NPC.velocity.ToRotation();
            Player player = Main.player[NPC.target]; NPC.TargetClosest(true);
            if (!NPC.lavaImmune)
            {
                if (NPC.Hitbox.Intersects(player.Hitbox))
                {
                    offset = player.Center - NPC.Center;
                    NPC.lavaImmune = true;
                }

                if (NPC.wet)
                {
                    NPC.noGravity = true;
                    if (player.wet)
                    {
                        if (player.position.X < NPC.position.X)
                        {
                            NPC.velocity.X -= 0.05f;
                            if (NPC.velocity.X < -3)
                            {
                                NPC.velocity.X = -3;
                            }
                        }
                        else
                        {
                            NPC.velocity.X += 0.05f;
                            if (NPC.velocity.X > 3)
                            {
                                NPC.velocity.X = 3;
                            }
                        }
                        if (player.position.Y < NPC.position.Y)
                        {
                            NPC.velocity.Y -= 0.05f;
                            if (NPC.velocity.Y < -3)
                            {
                                NPC.velocity.Y = -3;
                            }
                        }
                        else
                        {
                            NPC.velocity.Y += 0.05f;
                            if (NPC.velocity.Y < 3)
                            {
                                NPC.velocity.Y = 3;
                            }
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    NPC.noGravity = false;
                }
            }
            else if (!player.dead)
            { 
                if (NPC.lavaImmune)
                {
                    player.AddBuff(BuffID.Bleeding, 360);
                    player.AddBuff(BuffID.Dazed, 20);
                    player.AddBuff(ModContent.BuffType<Buffs.MassiveBleeding>(), 3);
                    NPC.Center = player.MountedCenter+offset;
                    NPC.velocity = Vector2.Zero;
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Bleeding, 360);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.water && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.Player.SGAPly().DankShrineZone ? 3.5f : 0f;
        }
    }

    public class MudMummy : ModNPC
    {
        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.Mummy);
            NPC.lifeMax = 600;
            NPC.value = 1500f;
            AIType = NPCID.Mummy;
            AnimationType = NPCID.Mummy;
            banner = NPC.type;
            bannerItem = Mod.Find<ModItem>("MudMummyBanner").Type;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mud Mummy");
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Mummy];
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(0, 3)==0)
            {
                int rand = Main.rand.Next(2);
                if (rand == 0)
                {
                    Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.LightShard);
                }
                if (rand == 1)
                {
                    Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.DarkShard);
                }
            }
            else
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("DankCore").Type, 1);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.rotation >= MathHelper.Pi)
            {
                NPC.localAI[3] += Math.Abs(NPC.velocity.X/8f);
                NPC.frame.Y = NPC.frame.Y + frameHeight*((int)(NPC.localAI[3])%8);
            }
        }

        public override bool PreAI()
        {
            Player target = Main.player[NPC.target];

            bool doRest = true;

            if (NPC.aiAction > 100)
                doRest = false;

            if (doRest)
            {
                NPC.rotation /= 2f;
                if (target != null && target.active && !target.dead)
                {
                    if (Collision.CanHitLine(NPC.Center, 1, 1, target.Center, 1, 1))
                    {
                        NPC.aiAction += 3;
                    }
                }
                if (Main.rand.Next(0, 3) == 0)
                {
                    if (Main.netMode != 1)
                    {
                        NPC.aiAction += Main.rand.Next(1, 4);
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, NPC.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
            }

            if (!doRest)
            {

                int directiontogo = NPC.aiAction > 5400 ? 1 : -1;

                if (NPC.aiAction < 5000 || (NPC.aiAction > 5400 || (NPC.aiAction > 5000 && NPC.velocity.Y<-400)))
                {
                    bool hit = false;
                    List<Vector2> there = new List<Vector2>();
                    Vector2 gotothere = Vector2.Zero;
                    for (int i = 0; i < 100; i += 1)
                    {
                        Point16 here = new Point16((int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16) + (i * directiontogo));
                        if (WorldGen.InWorld(here.X, here.Y))
                        {
                            there.Add(here.ToVector2());
                            if (!Collision.CanHitLine(NPC.Center, 1, 1, here.ToVector2() * 16, 1, 1))
                            {
                                hit = true;
                                gotothere = here.ToVector2() * 16;
                                break;
                            }

                        }

                    }

                    if (hit)
                    {
                        for (int i = 0; i < there.Count; i += 1)
                        {
                            int DustID2 = Dust.NewDust(there[i]*16 - new Vector2(NPC.width, NPC.height) / 2f, NPC.width, NPC.height, Mod.Find<ModDust>("MangroveDust").Type, Main.rand.NextFloat(-4f,4f), -0.20f + NPC.velocity.Y * 0.2f, 20, default(Color), 1f);
                            Main.dust[DustID2].noGravity = true;
                        }
                        NPC.Center = gotothere + new Vector2(0, (NPC.height+4) * -directiontogo);

                        NPC.aiAction = 5000;
                        if (directiontogo == 1)
                        {
                            NPC.aiAction = -400;
                        }

                    }
                    else
                    {
                        NPC.aiAction = -200;
                        return true;
                    }

                }

                NPC.rotation = MathHelper.Pi;
                NPC.aiAction += 1;
                NPC.velocity.Y -= 0.5f;
                NPC.velocity.X /= 1.05f;

                if (Math.Abs(NPC.velocity.X)<0.25 && NPC.velocity.Y <= 0 && !Collision.CanHitLine(NPC.Center,1,1,NPC.Center-new Vector2(0,24),1,1))
                {
                    NPC.velocity.Y = 5;
                }

                NPC.spriteDirection = -Math.Sign(target.Center.X - NPC.Center.X);

                NPC.velocity.X += Math.Sign(target.Center.X - NPC.Center.X) / 8f;

                if (target != null && target.active && !target.dead)
                {
                    if (!Collision.CanHitLine(NPC.Center, 1, 1, target.Center, 1, 1))
                    {
                        NPC.aiAction += 3;
                    }
                    else
                    {
                        if (Math.Abs(target.Center.X - NPC.Center.X) < 64 || Main.rand.Next(0,4) == 0)
                        {
                            if (NPC.aiAction % 80 == 0)
                            {
                                int num5 = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, Main.rand.Next(0,2) == 0 ? ModContent.NPCType<BlackLeech>() : ModContent.NPCType<Murk.Fly>(), 0, 0f, 0f, 0f, 0f, 255);
                                Main.npc[num5].velocity.X = Main.rand.Next(-3, 4);
                                Main.npc[num5].velocity.Y = Main.rand.Next(-3, 4);
                                if (Main.netMode == 2 && num5 < 200)
                                {
                                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num5, 0f, 0f, 0f, 0, 0, 0);
                                }
                            }
;
                        }
                        else
                        {
                            if (NPC.aiAction % 40 == 0)
                            {
                                List<Projectile> itz = Idglib.Shattershots(NPC.Center + new Vector2(0, 16), target.Center, Vector2.Zero, ProjectileID.MudBall, 25, 8f, 0, 1, true, 0, true, 300);
                                foreach (Projectile proj in itz)
                                {
                                    proj.aiStyle = -5;
                                }
                            }
                        }
                    }
                }

            }

            return doRest;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int x = spawnInfo.spawnTileX;
            int y = spawnInfo.spawnTileY;
            int tile = (int)Main.tile[x, y].TileType;
            if (spawnInfo.Player.ZoneJungle && Main.hardMode)
                return 0.01f;

            return Main.hardMode ? ((tile == Mod.Find<ModTile>("MoistStone") .Type|| TileID.Sets.Mud[tile]) ? 0.15f : 0f) : 0f;
        }
    }
}
