using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Effects;
using SGAmod.Dimensions;
using Terraria.DataStructures;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.Audio;

namespace SGAmod.NPCs.Sharkvern
{
    [AutoloadBossHead]
    public class SharkvernHead : SharkvernBase, ISGABoss
    {

        public string Trophy() => "SharkvernTrophy";
        public bool Chance() => Main.rand.Next(0, 10) == 0;
        public string RelicName() => "Sharkvern";
        public void NoHitDrops() { }
        public string MasterPet() => null;
        public bool PetChance() => false;

        private float Phase2Percent => RainFight ? 0.80f : 0.75f;

        public int sergedout=0;
        public int rage=0;
        public bool touchwater=false;
        public Vector2 Summoncenter=new Vector2(0,0);
        public List<int> averagey;
        public int timer = 0;
        public int timer2 = 0;
        public int doSharkRise = 0;
        public bool ramwater = true;
        public int whirlpoolAttackNum=0;
        public bool RainFight = false;
        public int leftNRight = 1;
        public NPC tail;


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sharkvern");
            Main.npcFrameCount[NPC.type] = 1;
        }
        
        public override void SetDefaults()
        {
            NPC.lifeMax = 100000;        
            NPC.damage = 75;    
            NPC.defense = 12;        
            NPC.knockBackResist = 0f;
            NPC.width = 52; 
            NPC.height = 52; 
            NPC.boss = true;
            music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Shark");
            NPC.lavaImmune = false;      
            NPC.noGravity = true;          
            NPC.noTileCollide = true;       
            NPC.HitSound = SoundID.NPCHit1;
            NPC.behindTiles = true;
            NPC.DeathSound = SoundID.NPCDeath1;
            Main.npcFrameCount[NPC.type] = 1;
            NPC.npcSlots = 1f;
            NPC.netAlways = true;
            bossBag = Mod.Find<ModItem>("SharkvernBag").Type;
            NPC.value = Item.buyPrice(0, 25, 0, 0);
            NPC.aiStyle = -1;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
        potionType=ItemID.GreaterHealingPotion;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !NPC.dontTakeDamage;
        }

        public override void NPCLoot()
        {

            if (Main.expertMode)
            {
               NPC.DropBossBags();
            }
            else
            {
                int lLoot = (Main.rand.Next(0,4));
                if (lLoot == 0)
                {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("SkytoothStormSpell").Type);
                }
                if (lLoot == 1)
                {
                    Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("Jaws").Type);
                }
                if (lLoot == 2)
                {
                    Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("SnappyShark").Type);
                    Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("SharkTooth").Type, 99);
                }
                if (lLoot == 3)
                {
                    Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("SharkBait").Type, Main.rand.Next(50, 100));
                }
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("SharkTooth").Type, Main.rand.Next(50,100));

                List<int> types=new List<int>();
                types.Insert(types.Count,ItemID.SharkFin); 
                types.Insert(types.Count,ItemID.Seashell); 
                types.Insert(types.Count,ItemID.Starfish); 
                types.Insert(types.Count,ItemID.SoulofFlight);
                types.Insert(types.Count,ItemID.Coral);

                DropHelper.DropFixedItemQuanity(types.ToArray(), 75, NPC.Center);

                /*
                for (int f = 0; f < (Main.expertMode ? 150 : 75); f=f+1){
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, types[Main.rand.Next(0,types.Count)]);
                }
                */

            }
            if(Main.rand.Next(7) == 0)
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("SharkvernMask").Type);
            }


            //if (SLWorld.currentSubworld!=null)
                //SubworldCache.AddCache("SGAmod", "SGAWorld", "downedSharkvern", SGAWorld.downedSharkvern);

            Achivements.SGAAchivements.UnlockAchivement("Sharkvern", Main.LocalPlayer);
            SGAWorld.downedSharkvern = true;
            if (RainFight)
            {
                SGAWorld.tidalCharmUnlocked = true;
                SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.NPCKilled, -1, -1, 20);
                if (sound != null)
                {
                    sound.Pitch = 0.25f;
                }
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                Gore.NewGore(NPC.Center + new Vector2(0,0), NPC.velocity/2f, Mod.GetGoreSlot("Gores/Sharkvern_head_gib"), 1f);
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(ramwater);
            writer.Write(timer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ramwater = reader.ReadBoolean();
            timer = reader.ReadInt32();
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.650f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.6f);
        }

        public static void DoStormThings(Player player, SharkvernHead shark)
        {
            if (!Main.expertMode)
                return;
            //Main.player[npc.target]

            if (shark == null)
            {
                bool one = (Main.raining);
                bool two = (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !SGAWorld.downedSharkvern && (SGAConfig.Instance.NegativeWorldEffects || SGAmod.DRMMode));
                bool three = (Main.netMode == NetmodeID.SinglePlayer && !Main.LocalPlayer.ZoneSnow && SGAmod.TotalCheating);
                int time = 2;

                if (one && (two || three))
                {
                    if (three)
                    {
                        time += (int)(SGAmod.PlayingPercent * 1800);
                    }

                    if (!SGAWorld.sharkvernMessage)
                    {
                        SGAWorld.sharkvernMessage = true;
                        Idglib.Chat("A torrential storm moves in from the oceans...", 50, 50, 255);

                    }

                    Main.maxRaining = 0.80f;

                    for (int k = 0; k < Main.maxPlayers; k++)
                    {
                        Player player2 = Main.player[k];
                        if (player2 != null)
                        {
                            if (player2.active && !player2.dead)
                            {
                                if (!player2.ZoneUnderworldHeight && Collision.CanHitLine(player2.Center, 1, 1, player2.Center - new Vector2(0, 1200), 1, 1))
                                    player2.AddBuff(ModContent.BuffType<SharkvernDrown>(), time, true);
                            }
                        }
                    }
                    goto endhere;
                }
                return;
            }

            if (player != null)
                Main.windSpeed = MathHelper.Clamp(Main.windSpeed + Math.Sign((player.Center.X - shark.NPC.Center.X)) * (-0.002f / 3f), -0.4f, 0.4f);

            if (Main.maxRaining > 0.3f)
            {
                for (int k = 0; k < Main.maxPlayers; k++)
                {
                    Player player2 = Main.player[k];
                    if (player2 != null)
                    {
                        if (player2.active && !player2.dead)
                        {
                            if (Collision.CanHitLine(player2.Center, 1, 1, player2.Center - new Vector2(0, 400), 1, 1))
                                player2.AddBuff(ModContent.BuffType<SharkvernDrown>(), 5+Math.Max(1,(int)(shark.rage / 4f)), true);
                        }
                    }
                }
            }
            float adder = 0;
            foreach (NPC npcx in Main.npc.Where(testby => testby.type == ModContent.NPCType<SharkvernCloudMiniboss>() && testby.active))
            {
                adder += npcx.ai[0] / 3200f;
            }
            if (shark.RainFight)
                adder = 15f;

            if (adder > 0)
            {
                Main.raining = true;
                Main.maxRaining = Math.Min(Main.maxRaining + 0.075f, MathHelper.Clamp(0f + adder,0f, shark.RainFight && shark.NPC.life<shark.NPC.lifeMax*0.25 ? 1f : 0.75f));
                Main.rainTime = 12;
                Main.UseStormEffects = true;
            }
            return;

        endhere:
            Main.UseStormEffects = true;

        }

        public override bool PreAI()
        {

            if (averagey == null)
                averagey = new List<int>();

            NPC.Opacity = MathHelper.Clamp(NPC.Opacity + (NPC.dontTakeDamage ? -0.01f : 0.02f), 0.2f, 1f);

            if (Main.netMode != 1)
            {
                if (NPC.ai[0] == 0)
                {
                    if (Main.raining && SGAWorld.downedSharkvern)
                    {
                        SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.NPCKilled, -1,-1,10);
                        if (sound != null)
                        {
                            sound.Pitch = 0.75f;
                        }
                        RainFight = true;
                    }

                    NPC.realLife = NPC.whoAmI;

                    int latestNPC = NPC.whoAmI;

                    int randomWormLength = 10;
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        //latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernNeck"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        //Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, Mod.Find<ModNPC>("SharkvernBody").Type, NPC.whoAmI, 0, latestNPC, ai3: NPC.whoAmI);
                        Main.npc[(int)latestNPC].realLife = NPC.whoAmI;
                        /*latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody2"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody3"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;*/
                    }
                    latestNPC = NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, Mod.Find<ModNPC>("SharkvernTail").Type, NPC.whoAmI, 0, latestNPC, ai3: NPC.whoAmI);
                    Main.npc[(int)latestNPC].realLife = NPC.whoAmI;
                    Main.npc[(int)latestNPC].ai[3] = NPC.whoAmI;
                    tail = Main.npc[(int)latestNPC];
                    NPC.netUpdate = true;
                }
            }
            NPC.ai[0] += 1;
            touchwater = false;

            int minTilePosX = (int)(NPC.position.X / 16.0) - 1;
            int maxTilePosX = (int)((NPC.position.X + NPC.width) / 16.0) + 2;
            int minTilePosY = (int)(NPC.position.Y / 16.0) - 1;
            int maxTilePosY = (int)((NPC.position.Y + NPC.height) / 16.0) + 2;
            if (minTilePosX < 0)
                minTilePosX = 0;
            if (maxTilePosX > Main.maxTilesX)
                maxTilePosX = Main.maxTilesX;
            if (minTilePosY < 0)
                minTilePosY = 0;
            if (maxTilePosY > Main.maxTilesY)
                maxTilePosY = Main.maxTilesY;

            bool collision = false;
            for (int i = minTilePosX - 5; i < maxTilePosX + 5; ++i)
            {
                for (int j = minTilePosY - 5; j < maxTilePosY + 10; ++j)
                {
                    if (WorldGen.InWorld(i,j) && Main.tile[i, j] != null)
                    {
                        if ((int)Main.tile[i, j].liquid > 64)
                        {
                            touchwater = true;
                        }
                    }
                }
            }

            if (ramwater == false)
            {
                for (int i = minTilePosX - 5; i < maxTilePosX + 5; ++i)
                {
                    for (int j = minTilePosY - 5; j < maxTilePosY; ++j)
                    {
                        if (WorldGen.InWorld(i, j))
                        {
                        Tile tile = Framing.GetTileSafely(i, j);

                            if (tile != null)
                            {
                                if ((int)tile.liquid > 240)
                                {
                                    if (ramwater == false)
                                    {
                                        if (NPC.velocity.Y > 6)
                                        {
                                            ramwater = true;
                                            for (float xx = 6f; xx < 30f; xx += 0.5f)
                                            {
                                                int proj2 = Projectile.NewProjectile(NPC.Center, new Vector2(Main.rand.NextFloat(-8f, 8f), -Main.rand.NextFloat(0, xx)), Mod.Find<ModProjectile>("RandomOceanCrap").Type, 30, 4);
                                                Main.projectile[proj2].friendly = false;
                                                Main.projectile[proj2].hostile = true;
                                                Main.projectile[proj2].netUpdate = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }

            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    //Main.tileSolidTop[(int)Main.tile[i, j].type]
                    if (Main.tile[i, j] != null && (Main.tile[i, j].HasUnactuatedTile && (Main.tileSolid[(int)Main.tile[i, j].TileType] && (int)Main.tile[i, j].TileFrameY == 0) || (int)Main.tile[i, j].liquid > 64))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(i * 16);
                        vector2.Y = (float)(j * 16);
                        if (NPC.position.X + NPC.width > vector2.X && NPC.position.X < vector2.X + 16.0 && (NPC.position.Y + NPC.height > (double)vector2.Y && NPC.position.Y < vector2.Y + 16.0))
                        {
                            collision = true;
                            if (Main.rand.Next(100) == 0 && Main.tile[i, j].HasUnactuatedTile)
                                WorldGen.KillTile(i, j, true, true, false);
                        }
                    }
                }
            }

            if (!collision)
            {
                Rectangle rectangle1 = new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height);
                int maxDistance = 1000;
                bool playerCollision = true;
                for (int index = 0; index < 255; ++index)
                {
                    if (Main.player[index].active)
                    {
                        Rectangle rectangle2 = new Rectangle((int)Main.player[index].position.X - maxDistance, (int)Main.player[index].position.Y - maxDistance, maxDistance * 2, maxDistance * 2);
                        if (rectangle1.Intersects(rectangle2))
                        {
                            playerCollision = false;
                            break;
                        }
                    }
                }
                if (playerCollision)
                    collision = true;
            }


            float speed = 15f + (Math.Min(Math.Max(0, rage / 100f), 6));

            float acceleration = 1f + (Math.Min(Math.Max(0, rage / 300f), 3));

            NPC.position = new Vector2(MathHelper.Clamp(NPC.position.X, 150f, (float)(Main.maxTilesX * 16) - 150f), NPC.position.Y);

            Vector2 npcCenter = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
            float targetXPos = Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2);
            float targetYPos = Main.player[NPC.target].position.Y + (Main.player[NPC.target].height / 2);

            DoStormThings(Main.player[NPC.target],this);

            timer += 1;
            timer2 += 1;

            NPC.dontTakeDamage = false;
            if (RainFight)
            {
                //npc.dontTakeDamage = NPC.CountNPCS(ModContent.NPCType<TidalElemental>()) > 0;
            }

            if (NPC.ai[0] < 300)
            {
                if (NPC.ai[0] < 5)
                {
                    NPC.Opacity = 0;
                }
                NPC.dontTakeDamage = true;
                targetYPos -= 600;
            }

            if (!(NPC.ai[3] < 1000 && NPC.ai[3] > -1) && NPC.life < NPC.lifeMax / 2 && (NPC.CountNPCS(NPCID.WyvernHead)>0 || (timer2%3000>2000 && timer2%3000<3000)))
            {
                targetYPos -= 800;
                if (timer2 % 3000 > 2300 && timer2 % 3000 < 2900)
                {
                    NPC.NewNPC((int)NPC.Center.X, (int)Math.Min(NPC.Center.Y, Main.player[NPC.target].Center.Y-1000), NPCID.WyvernHead);
                }
                if (NPC.CountNPCS(NPCID.WyvernHead) > 0)
                    timer2 = 2900;
                else
                    targetYPos -= 1600;

                NPC.dontTakeDamage = true;
            }
            else
            {
                if (doSharkRise < 10)
                {
                    if (timer % 1000 > 700)
                    {
                        collision = true;
                        targetYPos -= 800;
                        if (timer % 1000 == 849)
                        {
                            ramwater = false;
                            NPC.netUpdate = true;
                        }
                        if (timer % 1000 > 850)
                        {
                            targetYPos += 4800;
                        }
                    }
                }
            }

            if (doSharkRise > 9)
            {
                collision = true;
                targetXPos -= 1500*leftNRight;
                if (doSharkRise > 300)
                {
                    targetXPos += (doSharkRise - 300)*(3000*leftNRight);
                }
            }

            if (NPC.dontTakeDamage)
                SharkvernBody.DoDust(NPC);

            if (NPC.ai[3] > 0)
            {
                timer = 0;
                targetXPos = 3000f;
                if (NPC.Center.X > (Main.maxTilesX / 2) * 16)
                    targetXPos = (float)(Main.maxTilesX * 16) - (3000f);
                targetYPos = averagey[0];
                //targetYPos=(float)SGAWorld.RaycastDownWater((int)targetXPos/16,(int)(1),50)*16;
                Summoncenter = new Vector2(targetXPos, targetYPos - 500f);
                double angle = (NPC.ai[3] / 30f) + 2.0 * Math.PI;
                targetXPos += (float)((Math.Cos(angle) * 800f) * -1f);
                targetYPos += (float)((Math.Sin(angle) * 400f) * -1f) - 500f;
            }

            if (Main.player[NPC.target].dead)
            {
                targetYPos = 100000f;
            }
            else
            {
                NPC.timeLeft = 300;
            }

            float targetRoundedPosX = (float)((int)(targetXPos / 16.0) * 16);
            float targetRoundedPosY = (float)((int)(targetYPos / 16.0) * 16);
            npcCenter.X = (float)((int)(npcCenter.X / 16.0) * 16);
            npcCenter.Y = (float)((int)(npcCenter.Y / 16.0) * 16);
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);

            if (!collision && NPC.ai[3] < 1)
            {
                NPC.TargetClosest(true);
                NPC.velocity.Y = NPC.velocity.Y + 0.11f;
                if (NPC.velocity.Y > speed)
                    NPC.velocity.Y = speed;
                if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.4)
                {
                    if (NPC.velocity.X < 0.0)
                        NPC.velocity.X = NPC.velocity.X - acceleration * 1.1f;
                    else
                        NPC.velocity.X = NPC.velocity.X + acceleration * 1.1f;
                }
                else if (NPC.velocity.Y == speed)
                {
                    if (NPC.velocity.X < dirX)
                        NPC.velocity.X = NPC.velocity.X + acceleration;
                    else if (NPC.velocity.X > dirX)
                        NPC.velocity.X = NPC.velocity.X - acceleration;
                }
                else if (NPC.velocity.Y > 4.0)
                {
                    if (NPC.velocity.X < 0.0)
                        NPC.velocity.X = NPC.velocity.X + acceleration * 0.9f;
                    else
                        NPC.velocity.X = NPC.velocity.X - acceleration * 0.9f;
                }
            }

            else
            {
                if (timer % 1000 <= 700)
                {
                    if (NPC.soundDelay == 0)
                    {
                        float num1 = length / 40f;
                        if (num1 < 10.0)
                            num1 = 10f;
                        if (num1 > 20.0)
                            num1 = 20f;
                        NPC.soundDelay = (int)num1;

                        Tile tilehere=null;
                        Point coords = NPC.Center.ToTileCoordinates();
                        if (WorldGen.InWorld(coords.X, coords.Y))
                        tilehere = Framing.GetTileSafely(NPC.Center.ToTileCoordinates());

                        if (tilehere != null)
                        {
                            if (tilehere.HasTile && tilehere.liquid < 50)
                            {
                                SoundEngine.PlaySound(SoundID.Roar, (int)NPC.position.X, (int)NPC.position.Y, 1);
                            }
                            else if (tilehere.liquid >= 50)
                            {
                                SoundEffectInstance snd = SoundEngine.PlaySound(SoundID.Splash, (int)NPC.position.X, (int)NPC.position.Y, 1);
                                if (snd != null)
                                {
                                    snd.Pitch = 0.50f;
                                }
                            }
                        }
                    }
                }
                float absDirX = Math.Abs(dirX);
                float absDirY = Math.Abs(dirY);
                float newSpeed = speed / length;
                dirX = dirX * newSpeed;
                dirY = dirY * newSpeed;
                if (NPC.velocity.X > 0.0 && dirX > 0.0 || NPC.velocity.X < 0.0 && dirX < 0.0 || (NPC.velocity.Y > 0.0 && dirY > 0.0 || NPC.velocity.Y < 0.0 && dirY < 0.0))
                {
                    if (NPC.velocity.X < dirX)
                        NPC.velocity.X = NPC.velocity.X + acceleration;
                    else if (NPC.velocity.X > dirX)
                        NPC.velocity.X = NPC.velocity.X - acceleration;
                    if (NPC.velocity.Y < dirY)
                        NPC.velocity.Y = NPC.velocity.Y + acceleration;
                    else if (NPC.velocity.Y > dirY)
                        NPC.velocity.Y = NPC.velocity.Y - acceleration;
                    if (Math.Abs(dirY) < speed * 0.2 && (NPC.velocity.X > 0.0 && dirX < 0.0 || NPC.velocity.X < 0.0 && dirX > 0.0))
                    {
                        if (NPC.velocity.Y > 0.0)
                            NPC.velocity.Y = NPC.velocity.Y + acceleration * 2f;
                        else
                            NPC.velocity.Y = NPC.velocity.Y - acceleration * 2f;
                    }
                    if (Math.Abs(dirX) < speed * 0.2 && (NPC.velocity.Y > 0.0 && dirY < 0.0 || NPC.velocity.Y < 0.0 && dirY > 0.0))
                    {
                        if (NPC.velocity.X > 0.0)
                            NPC.velocity.X = NPC.velocity.X + acceleration * 2f;
                        else
                            NPC.velocity.X = NPC.velocity.X - acceleration * 2f;
                    }
                }
                else if (absDirX > absDirY)
                {
                    if (NPC.velocity.X < dirX)
                        NPC.velocity.X = NPC.velocity.X + acceleration * 1.1f;
                    else if (NPC.velocity.X > dirX)
                        NPC.velocity.X = NPC.velocity.X - acceleration * 1.1f;
                    if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
                    {
                        if (NPC.velocity.Y > 0.0)
                            NPC.velocity.Y = NPC.velocity.Y + acceleration;
                        else
                            NPC.velocity.Y = NPC.velocity.Y - acceleration;
                    }
                }
                else
                {
                    if (NPC.velocity.Y < dirY)
                        NPC.velocity.Y = NPC.velocity.Y + acceleration * 1.1f;
                    else if (NPC.velocity.Y > dirY)
                        NPC.velocity.Y = NPC.velocity.Y - acceleration * 1.1f;
                    if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < speed * 0.5)
                    {
                        if (NPC.velocity.X > 0.0)
                            NPC.velocity.X = NPC.velocity.X + acceleration;
                        else
                            NPC.velocity.X = NPC.velocity.X - acceleration;
                    }
                }
            }

            NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 1.57f;
            Vector2 capvelo = NPC.velocity;
            capvelo.Normalize();
            float speedmove = 15f - (length / 800f);
            if (NPC.ai[3] > 0)
                speedmove = speedmove / 2f;
            if (NPC.velocity.Length() < capvelo.Length() * speedmove) { NPC.velocity = capvelo * speedmove; }



            if (collision)
            {
                if (NPC.localAI[0] != 1)
                    NPC.netUpdate = true;
                NPC.localAI[0] = 1f;
            }
            else
            {
                if (NPC.localAI[0] != 0.0)
                    NPC.netUpdate = true;
                NPC.localAI[0] = 0.0f;
            }
            if ((NPC.velocity.X > 0.0 && NPC.oldVelocity.X < 0.0 || NPC.velocity.X < 0.0 && NPC.oldVelocity.X > 0.0 || (NPC.velocity.Y > 0.0 && NPC.oldVelocity.Y < 0.0 || NPC.velocity.Y < 0.0 && NPC.oldVelocity.Y > 0.0)) && !NPC.justHit)
                NPC.netUpdate = true;

            int num254 = (int)(NPC.position.X + (float)(NPC.width / 2)) / 16;
            int num255 = (int)(NPC.position.Y + (float)(NPC.height / 2)) / 16;
            /*if (Main.tile[num254, num255 - 1] == null)
            {
                Main.tile[num254, num255 - 1] = new Tile();
            }
             if (Main.tile[num254, num255 - 1].active())
             {
            if (Main.tile[num254, num255 - 1].liquid > 128)
            {
            rage=0;
            }}*/

            rage = rage + (length < 2000 && NPC.ai[3] < 1 ? 1 : 0);
            if (touchwater == true)
            {
                rage = Math.Max(-250, rage - 5);
                if (Main.expertMode)
                    rage = ((int)Math.Max(((1f - ((float)NPC.life / (float)NPC.lifeMax)) * 350f) - 150f, rage - 4));
            }
            if (RainFight)
                rage = 400;

            NPC.damage = Math.Max(NPC.defDamage, Math.Min(120, (int)rage / 4));
            sergedout = sergedout - 1;
            bool anyalive = false;

            if (NPC.ai[3] < -399)
            {
                NPC.ai[3] -= 1;
                if (NPC.ai[3] < -60 * 60)
                {
                    NPC.ai[3] = 0;
                }
            }

            if (NPC.life > NPC.lifeMax * Phase2Percent)
            {
                averagey.Add((int)NPC.Center.Y);
            }

            if (NPC.ai[3] < 1000 && NPC.ai[3] > -1 && NPC.life < (int)(NPC.lifeMax * Phase2Percent))
            {
                NPC.ai[3] += 1;

                if (averagey.Count > 2)
                {
                    int thereat = 0;
                    for (int i = 0; i < averagey.Count; i += 1)
                    {
                        thereat += averagey[i];
                    }
                    thereat = (int)(thereat / averagey.Count);
                    averagey.Clear();
                    averagey.Add(thereat);
                }

                if (NPC.ai[3] > 150)
                {
                    int attackrate = whirlpoolAttackNum > 1 ? 120 : 160;
                    if (NPC.ai[3] % attackrate == 0 && Main.netMode != 1 && whirlpoolAttackNum % 2 == 0)
                    {
                            int him = NPC.NewNPC((int)Summoncenter.X, (int)Summoncenter.Y, Mod.Find<ModNPC>(RainFight ? "TidalElemental" : "SharvernMinion").Type);
                            Main.npc[him].ai[3] = Main.rand.Next(0, 2000);
                    }

                    if ((NPC.ai[3]+80) % attackrate == 0 && Main.netMode != 1 && whirlpoolAttackNum%2==1)
                    {
                            NPC.NewNPC((int)Summoncenter.X, (int)Summoncenter.Y, Mod.Find<ModNPC>("SharkvernCloudMiniboss").Type);
                    }

                    for (int i = 0; i < 5; ++i)
                    {
                        double devider = (i / ((double)5f));
                        double angle = (NPC.ai[3] / 15) + 2.0 * Math.PI * devider;
                        Vector2 thecenter = new Vector2((float)((Math.Cos(angle) * 150)), (float)((Math.Sin(angle) * 80)));
                        thecenter = thecenter.RotatedByRandom(MathHelper.ToRadians(10));
                        int DustID2 = Dust.NewDust(Summoncenter + (thecenter * 4.5f), 0, 0, Mod.Find<ModDust>("TornadoDust").Type, thecenter.X * 0.8f, thecenter.X * 0.8f, 20, default(Color), 2.5f);
                        Main.dust[DustID2].noGravity = true;
                        Main.dust[DustID2].velocity = new Vector2(thecenter.X * 0.2f, thecenter.Y * 0.2f) * -1f;
                    }

                    for (int i = 0; i < 10; ++i)
                    {
                        double devider = (i / ((double)10f));
                        double angle = (NPC.ai[3] / 30) + 2.0 * Math.PI * devider;
                        Vector2 thecenter = new Vector2((float)((Math.Cos(angle) * 150)), (float)((Math.Sin(angle) * 80)));
                        int DustID2 = Dust.NewDust(Summoncenter + (thecenter * 0.5f), 0, 0, Mod.Find<ModDust>("TornadoDust").Type, thecenter.X * 0.7f, thecenter.X * 0.7f, 20, default(Color), 1.5f);
                        Main.dust[DustID2].noGravity = true;
                        Main.dust[DustID2].velocity = new Vector2(thecenter.X * 0.04f, thecenter.Y * 0.04f);
                        Main.dust[DustID2].color = Color.Aqua;
                    }

                    for (int i = 0; i < 360; i += Main.rand.Next(10, 45))
                    {
                        double devider = (i / ((double)360f));
                        double angle = 2.0 * Math.PI * devider;
                        Vector2 thecenter = new Vector2((float)((Math.Cos(angle) * 150)), (float)((Math.Sin(angle) * 80)));
                        int DustID2 = Dust.NewDust(Summoncenter + (thecenter * 0.5f), 0, 0, Mod.Find<ModDust>("TornadoDust").Type, thecenter.X * 0.7f, thecenter.X * 0.7f, 20, default(Color), 3f);
                        Main.dust[DustID2].noGravity = true;
                        Main.dust[DustID2].velocity = new Vector2(0, 0);
                        Main.dust[DustID2].color = Color.Aqua;
                    }
                }

                if (NPC.ai[3] > 999)
                {
                    NPC.ai[3] = -1000;
                    whirlpoolAttackNum += 1;
                }
            }
            else
            {

                if (NPC.life < NPC.lifeMax*Phase2Percent)
                {
                    if (doSharkRise < 10)
                        doSharkRise += 1;
                }

                if (doSharkRise > 9)
                {
                    doSharkRise += 1;
                    timer -= 1;
                }
                if (doSharkRise > 300)
                {
                    if (doSharkRise % 15 == 0)
                    {
                        Projectile.NewProjectileDirect(new Vector2(NPC.Center.X, Main.player[NPC.target].Center.Y-128f), Vector2.UnitY * 4f, ModContent.ProjectileType<SharkDropProj>(), 50, 5f);
                    }
                }
                if (doSharkRise > 600)
                {
                    leftNRight *= -1;
                    doSharkRise = -1300;
                }
            }


            if (rage == 700 && Main.netMode != 1)
                Idglib.Chat("The Sharkvern beckens you to return it to the sea!", 50, 50, 255);

            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player player = Main.player[k];
                if (player != null)
                {
                    if (player.active && !player.dead && player.Distance(NPC.Center)<5000)
                    {
                        anyalive = true;
                    }
                }
            }

            if (anyalive == false)
            {
                NPC.velocity.Y += 1;
                NPC.timeLeft = Math.Min(NPC.timeLeft,200);
                //npc.life = 0;
                //npc.active = false;
            }

            return false;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
                if (rage>200)
                player.AddBuff(BuffID.Bleeding, Math.Min(rage, 500), true);
                if (rage>300)
                player.AddBuff(Mod.Find<ModBuff>("MassiveBleeding").Type, Math.Min(rage,300), true);         
            }
       
       public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[NPC.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            //Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor*npc.Opacity, npc.rotation, origin, npc.scale, npc.velocity.X>0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

            List<(Vector3,Color)> SharkPoints = new List<(Vector3, Color)>();

            int startThere = tail.whoAmI;
            while(Main.npc[startThere].active && (Main.npc[startThere].type == ModContent.NPCType<SharkvernTail>() || Main.npc[startThere].type == ModContent.NPCType<SharkvernHead>() || Main.npc[startThere].type == ModContent.NPCType<SharkvernBody>()
                 || Main.npc[startThere].type == ModContent.NPCType<SharkvernBody2>() || Main.npc[startThere].type == ModContent.NPCType<SharkvernBody3>() || Main.npc[startThere].type == ModContent.NPCType<SharkvernNeck>()))
            {
                NPC guy = Main.npc[startThere];
                bool isshark = guy.ModNPC != null && guy.ModNPC is SharkvernBase sharka;
                Color color = isshark ? (guy.ModNPC as SharkvernBase).sharkGlowColor : Color.Transparent;
                SharkPoints.Add((new Vector3(guy.Center.X, guy.Center.Y, guy.rotation), color));
                startThere = (int)guy.ai[1];
                if (guy == NPC)
                    break;
            }

            List<Vector2> FinalPoints = new List<Vector2>();
            List<Color> FinalPointsColor = new List<Color>();

            foreach ((Vector3,Color) vec3 in SharkPoints)
            {
                FinalPoints.Add(new Vector2(vec3.Item1.X, vec3.Item1.Y));
                FinalPointsColor.Add(vec3.Item2);
            }
            FinalPoints.Reverse();

            TrailHelper trail = new TrailHelper("BasicEffectPass", Mod.Assets.Request<Texture2D>("NPCs/Sharkvern/SharkvernWhole").Value);//NPCs/Sharkvern/SharkvernWhole
            trail.color = delegate (float percent)
            {
                int index = (int)(percent * (float)(FinalPoints.Count - 1));
                Vector2 there = FinalPoints[index];
                Color colorz = FinalPointsColor[(FinalPoints.Count - index) - 1];

                return (colorz == Color.Transparent ? Lighting.GetColor((int)there.X >> 4, (int)there.Y >> 4) : colorz)*NPC.Opacity;
            };
            trail.projsize = Vector2.Zero;
            //trail.coordOffset = new Vector2(0, 0);
            //trail.coordMultiplier = new Vector2(1f, 1f);
            trail.trailThickness = 32;
            trail.doFade = false;
            trail.trailThicknessIncrease = 0;
            trail.strength = 1f;
            trail.DrawTrail(FinalPoints, NPC.Center);


            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.9f;   
            return null;
        }
    }

        public class SharvernMinion : ModNPC
    {

        int framevar=0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Shark");
            Main.npcFrameCount[NPC.type] = 4;
        }

        public override bool CheckActive()
        {
            return NPC.CountNPCS(Mod.Find<ModNPC>("SharkvernHead").Type)<1;
        }

        public override string Texture
        {
            get { return "Terraria/Npc_" + NPCID.Shark; }
        }
        
        public override void SetDefaults()
        {
        NPC.CloneDefaults(NPCID.Shark);
        NPC.damage=50;
        NPC.noGravity = true; 
        NPC.knockBackResist = 0.9f;
        NPC.lifeMax = 800;
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.ai[3]%10==0)
            framevar=framevar+1;
            if (framevar>3)
            framevar=0;
            NPC.frame.Y=framevar*frameHeight;
        }

           public override void AI()
        {    
            NPC.spriteDirection=NPC.velocity.X>0 ? 1 : -1;
        if (!Main.player[NPC.target].dead){
            Vector2 mix=Main.player[NPC.target].Center-NPC.Center;
            mix.Normalize();
            NPC.velocity+=mix*0.03f;
            //npc.velocity+=new Vector2(0,-0.20f);
            NPC.ai[3]+=1;

            if (NPC.ai[3]%800>600){

            NPC.noTileCollide=true;
            int DustID2 = Dust.NewDust(NPC.Center, 0, 0, Mod.Find<ModDust>("TornadoDust").Type, (NPC.velocity.X*-2f)-0.5f, NPC.velocity.Y*-0.5f, 20, default(Color), 1.5f);
            Main.dust[DustID2].noGravity = true;
            if (NPC.ai[3]%800<785)
            NPC.velocity*=0.75f;
            NPC.velocity+=mix*2f;
            }else{
            if (NPC.ai[3]%600==0 && Main.expertMode){
            //List<Projectile> itz=Idglib.Shattershots(npc.Center,Main.player[npc.target].Center,new Vector2(0,0),ProjectileID.SapphireBolt,50,8f,1,1,true,0,true,300);
            }
            NPC.noTileCollide=false;
            }
            Vector2 capvelo=NPC.velocity;
            capvelo.Normalize();
            NPC.velocity=capvelo*Math.Min(NPC.velocity.Length(),20f);
            NPC.velocity*=0.97f;



             }
        }
    }

    public class RandomOceanCrap : ModProjectile
    {

        int fakeid = ProjectileID.FrostShard;
        int fishtype = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Random Ocean Crap");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.CloneDefaults(fakeid);
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Throwing;
            Projectile.extraUpdates = 0;
            Projectile.tileCollide = true;
            Projectile.aiStyle = -1;
            int[] types = { ItemID.Fish, ItemID.Trout, ItemID.TundraTrout, ItemID.ReaverShark, ItemID.Goldfish, ItemID.Ebonkoi,ItemID.CrimsonTigerfish, ItemID.FishStatue, ItemID.OldShoe, ItemID.MirageFish, ItemID.PrincessFish,ItemID.FrostDaggerfish };
            fishtype = types[Main.rand.Next(types.Length)];
        }

        public override string Texture
        {
            get { return "Terraria/Projectile_" + fakeid; }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.itemTexture[fishtype];
            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height)/2;
            Vector2 drawPos = ((Projectile.Center - Main.screenPosition));
            spriteBatch.Draw(tex, drawPos,null, lightColor, MathHelper.ToRadians(0) + Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool PreKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33);
                Main.dust[dust].scale = 2.50f;
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity *= 2f;
            }
            return true;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.20f;
            if (Main.rand.Next(0, 2) == 1)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33);
                Main.dust[dust].scale = 0.8f;
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity = Projectile.velocity * 0.4f;
            }
            Projectile.rotation += ((float)Projectile.velocity.Length()*Math.Sign(Projectile.velocity.X))*0.01f;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Wet, 60 * 5);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Wet, 60 * 5);
        }

    }

    public class SharkvernDrown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drowning Presence");
            Description.SetDefault("You litterally cannot breath!\nAll forms of infinite water breathing are disabled");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "SGAmod/Buffs/SharkvernDrowningDebuff";
            return true;
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            if (!Main.gameMenu && SGAPocketDim.WhereAmI != null)
            {
                if (SGAPocketDim.WhereAmI == typeof(SpaceDim))
                {
                    tip += "\nInflicted by the vaccum of space";
                    return;
                }

            }
                if (NPC.CountNPCS(ModContent.NPCType<SharkvernHead>()) < 1 && !SGAWorld.downedSharkvern)
                tip += "\nBeat Sharkvern to remove this effect";
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<SGAPlayer>().permaDrown = true;
            player.GetModPlayer<SGAPlayer>().drownRate += 2;
        }

    }

    public class SharkDropProj : ModProjectile
    {

        public Player P;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shark Drop");
        }

        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 300;
            Projectile.timeLeft = 300;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            AIType = ProjectileID.WoodenArrowFriendly;
        }

        public override string Texture
        {
            get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.NewProjectileDirect(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<SharkDropProjDelay>(), Projectile.damage, 5f);

            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void AI()
        {

            Point16 loc = new Point16((int)Projectile.Center.X >> 4, (int)Projectile.Center.Y >> 4);
            if (WorldGen.InWorld(loc.X, loc.Y))
            {
                Tile tile = Main.tile[loc.X, loc.Y];
                if (tile != null)
                    if (tile.liquid > 64)
                        Projectile.Kill();
            }

        }
    }

    public class SharkDropProjDelay : PinkyWarning
    {
        protected override Color color => Color.Aqua;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shark Delay");
        }

        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 0;
            Projectile.timeLeft = 60;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            AIType = ProjectileID.WoodenArrowFriendly;
            timeleft = 60f;
        }
        public override string Texture
        {
            get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
        }
        public override bool CanDamage()
        {
            return false;
        }

        public override bool PreKill(int timeLeft)
        {
            Player playerclosest = Main.player[Player.FindClosest(Projectile.Center,1,1)];

            if (!Main.dedServ)
            {
                WaterShaderData watershader = (WaterShaderData)Filters.Scene["WaterDistortion"].GetShader();
                Vector2 ripplePos = Projectile.Center;
                Color waveData = Color.Magenta * 5f;
                watershader.QueueRipple(ripplePos, waveData, Vector2.One * 32f, RippleShape.Circle, -(float)Math.PI / 2f);
            }

            float jumpHeight = ((playerclosest.Center.Y-500) - Projectile.Center.Y);
            Vector2 velo = new Vector2(0, (float)Math.Sqrt(-2.0f * 0.25f * jumpHeight));

            Projectile.NewProjectileDirect(Projectile.Center, Vector2.UnitY * -(velo), ModContent.ProjectileType<FlyingSharkProjBossProj>(), Projectile.damage, 5f);

            for (int i = 0; i < 40; i += 1)
            {
                int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33);
                Main.dust[dust2].scale = 2.5f;
                Main.dust[dust2].noGravity = false;
                Main.dust[dust2].velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-32f, -8f));
            }

            SoundEngine.PlaySound(SoundID.Splash, (int)Projectile.position.X, (int)Projectile.position.Y, 1, 1f, 0.25f);

            return true;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1;
            for (int i = 0; i < 10; i += 1)
            {
                int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33);
                Main.dust[dust2].scale = 2.5f;
                Main.dust[dust2].alpha = 200;
                Main.dust[dust2].noGravity = false;
                Main.dust[dust2].velocity = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-6f, 1f));
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float there = Projectile.velocity.ToRotation() - MathHelper.ToRadians(-90);
            //if (projectile.ai[0] < 120)
            //{
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            spriteBatch.Draw(Main.extraTexture[60], Projectile.Center+new Vector2(0,12) - Main.screenPosition, null, color * MathHelper.Clamp(Projectile.timeLeft / 30f, 0f, 0.9f), 0, (Main.extraTexture[60].Size() / 2f) + new Vector2(0, 12), new Vector2(0.75f, 1f+Projectile.ai[0]/5f), SpriteEffects.None, 0f);
            //}

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }

    public class FlyingSharkProjBossProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hungry Hungry Shark");
        }

        public override string Texture
        {
            get { return ("Terraria/NPC_" + NPCID.Shark); }
        }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.WaterBolt);
            Projectile.extraUpdates = 0;
            Projectile.width = 24;
            Projectile.height = 64;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.scale = 1f;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33);
            Main.dust[dust].scale = 2f;
            Main.dust[dust].noGravity = false;
            Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-1f, 1f));

            Projectile.localAI[0] += 1;
            Projectile.velocity.Y += 0.25f;
            Projectile.rotation = MathHelper.PiOver2;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            Texture2D tex = Main.npcTexture[ModContent.NPCType<SharvernMinion>()];
            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
            Vector2 drawPos = ((Projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
            Color color = Projectile.GetAlpha(lightColor) * 1f; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
            int timing = (int)(Projectile.localAI[0] / 8f);
            timing %= 4;
            timing *= ((tex.Height) / 4);
            float yspeed = Projectile.velocity.Y;
            if (Math.Abs(Projectile.velocity.Y) > 2f)
            {
                yspeed = (Math.Sign(yspeed) * 2f) + yspeed / 5f;
            }
            spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 4),
                color * MathHelper.Clamp((float)Projectile.timeLeft / 30f, 0f, (float)Math.Min(Projectile.localAI[0] / 15f, 1f)), Projectile.rotation
                , drawOrigin, Projectile.scale, Projectile.velocity.X < 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }

    }
}
