using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.Utilities;
using Terraria.ID;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
using Terraria.Graphics;
using Idglibrary;
using Terraria.Audio;

namespace SGAmod.NPCs.Hellion
{
    using System;
    using global::SGAmod.Effects;
    using global::SGAmod.Items;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Terraria;
    using Terraria.ID;
    using Terraria.ModLoader;

    public class HellionWorm : ModNPC
    {

        Texture2D tex;
        Texture2D armtex;
        float rotoffset = 0f;
        int aioffset = 9990;
        int noact = 0;
        int nomove = 0;
        int slowedSpeed = 0;
        bool onlyOnce = false;
        int chargeWarning = 0;
        int handsAttack = 0;
        float swapUp = 0;
        Vector2 chargeAt = Vector2.Zero;

        public Vector2 localdist;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellion Core Arm");
        }
        public override void SetDefaults()
        {
            NPC.width = 54;
            NPC.height = 54;
            NPC.damage = 100;
            NPC.defense = 20;
            NPC.lifeMax = 27000;
            NPC.knockBackResist = 0.0f;
            NPC.behindTiles = true;
            NPC.noTileCollide = true;
            NPC.netAlways = false;
            NPC.noGravity = true;
            NPC.dontCountMe = true;
            NPC.HitSound = SoundID.NPCHit1;
        }

        int phase = 0;

        public override string Texture
        {
            get { return ("Terraria/Projectile_" + ProjectileID.Starfury); }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(Mod.Find<ModBuff>("TechnoCurse").Type, 60 * 5);
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (phase > 1)
            {
                damage = (int)(damage * 0.25f);
            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            if (NPC.ai[1] > 0 && Main.npc[(int)NPC.ai[1]].active)
                damage = (int)(damage / 2f);
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {

            if (phase > 1)
            {
                if (NPC.ai[0] < 600)
                    damage = (int)(damage * 0.05f);
                damage = (int)(damage * 0.75f);
                if (projectile.penetrate != 1)
                {
                    //damage = (int)Math.Pow(damage, 0.925f);
                    damage = (int)(damage * 0.25f);
                }
            }
            if (NPC.ai[1] > 0)
            {
                damage = (int)(damage * 0.25f);
                if (projectile.penetrate != 1)
                    damage = (int)(damage * 0.25f);
            }
            else
            {
                //damage = (int)(damage * 5f);

            }
        }

        public virtual void KeepUpright(float dirX, float dirY)
        {
            localdist = new Vector2(dirX, dirY);
        }

        public override bool CheckActive()
        {
            return !Main.npc[(int)NPC.ai[3]].active;
        }

        public void WormHead(Player player)
        {
            if (Main.netMode != 1)
            {
                //make parts
                if (NPC.ai[0] == 3)
                {

                    /*npc.realLife = npc.whoAmI;

                    int latestNPC = npc.whoAmI;

                    int randomWormLength = Main.rand.Next(15, 15);
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        int latestNPC2 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("HellionWorm"), npc.whoAmI, 0, latestNPC);
                        Main.npc[(int)latestNPC2].realLife = (int)npc.ai[3];
                        Main.npc[(int)latestNPC2].ai[1] = latestNPC;
                        Main.npc[(int)latestNPC2].ai[3] = npc.ai[3];
                        Main.npc[(int)latestNPC2].netUpdate = true;
                        latestNPC = latestNPC2;
                    }
                    */

                    NPC.netUpdate = true;
                }

                //do ai

                Vector2 theplayerdir = player.Center - (NPC.Center);
                Vector2 orgrot = theplayerdir;
                float maxspeed = 100f;
                float friction = 0.97f;
                float acceerate = 0.4f;
                float acceeratedist = 4000f;
                bool facetoplayer = false;

                if (phase < 2)
                {

                    int timerMax = 800;
                    int timerTo = 400;

                    if (noact < 0)
                    {
                        int whento = phase < 1 ? 160 : 250;

                        chargeWarning = (int)Math.Max((NPC.ai[0] % whento) - (whento - 120), 0);
                        float chargespeedmul = MathHelper.Clamp(theplayerdir.Length() / 1000f, 0.5f, 1f);

                        if (NPC.ai[0] % whento > whento - 20)
                        {
                            swapUp = Main.rand.NextFloat(MathHelper.PiOver2 * 0.25f, MathHelper.PiOver2 * 0.75f) * (Main.rand.NextBool() ? 1f : -1f);
                            acceerate = 4f * chargespeedmul;
                        }
                        else
                        {
                            if (phase > 0)
                            {
                                theplayerdir = player.Center - (NPC.Center - ((NPC.ai[2] + MathHelper.ToRadians(NPC.ai[0]) * 2f).ToRotationVector2() * 200f));
                            }
                            else
                            {
                                theplayerdir = theplayerdir.RotatedBy(swapUp);
                            }

                        }


                        if (NPC.ai[0] % timerMax > timerTo)
                        {
                            chargeWarning = 0;
                            if (phase > 0)
                            {
                                theplayerdir = player.Center - (NPC.Center + ((NPC.ai[2] + MathHelper.ToRadians(NPC.ai[0])).ToRotationVector2() * 1000f));
                            }
                            else
                            {
                                theplayerdir = orgrot.RotatedBy(MathHelper.Pi * 0.025f*(float)Math.Sin((NPC.ai[0]/200f)*MathHelper.TwoPi));
                                Vector2 aplay = -theplayerdir + new Vector2(0, 0.01f);
                                if (theplayerdir.Length() < 0.2)
                                    theplayerdir = -Vector2.UnitY;
                                else
                                    theplayerdir.Normalize();
                                    float disttorbit = 420 + MathHelper.Clamp((((NPC.ai[0] % timerMax)-timerTo)-180)*2f, 0,720);
                                theplayerdir = player.Center - (NPC.Center + (theplayerdir * disttorbit));
                            }

                            friction = 0.92f;
                            acceerate = 2.5f;
                            acceeratedist = 1500f;
                            facetoplayer = true;

                            if (NPC.ai[0] % 10 == 0 && NPC.ai[0] % 80 < 40)
                                if (phase == 0 && NPC.ai[0] % timerMax > timerTo+160)
                                {
                                    float spread = 50 + (1f-(NPC.life/(float)NPC.lifeMax))*120f;
                                    int count = 2 + (NPC.life < (NPC.lifeMax * ((1f + HellionCore.beginphase[0]) / 2f)) ? 1 : 0);
                                    Idglib.Shattershots(NPC.Center, NPC.Center + NPC.rotation.ToRotationVector2(), new Vector2(0, 0), ModContent.ProjectileType<HellionCorePlasmaAttackButGreen>(), 50, 24, spread, count, true, 0, false, 400);
                                    Idglib.Shattershots(NPC.Center, NPC.Center + NPC.rotation.ToRotationVector2(), new Vector2(0, 0), ModContent.ProjectileType<HellionCoreArmWarning>(), 1, 1f, spread, count, true, 0, false, 60);
                                }
                            // if (npc.ai[0] % 125 == 0)
                            //     if (phase == 1)
                            //         Idglib.Shattershots(npc.Center, npc.Center + npc.rotation.ToRotationVector2(), new Vector2(0, 0), ProjectileID.DesertDjinnCurse, 25, 8, 20 + (400 - (npc.ai[0] % 400)) / 20, 1, true, 0, false, 400);
                        }
                        else
                        {
                            if (NPC.ai[0] % 180 == 0 && phase < 2)
                            {
                                Idglib.Shattershots(NPC.Center, NPC.Center + NPC.rotation.ToRotationVector2(), new Vector2(0, 0), ProjectileID.DemonScythe, 40, 5, 160, Hellion.GetHellion().phase < 1 ? 5 : 3, false, 0, false, 250);
                                Idglib.Shattershots(NPC.Center, NPC.Center + NPC.rotation.ToRotationVector2(), new Vector2(0, 0), ModContent.ProjectileType<PinkyWarning>(), 1, 1, 160, Hellion.GetHellion().phase < 1 ? 5 : 3, false, 0, false, 250);


                            }

                        }
                    }

                }
                else
                {
                    if (noact < 1)
                        facetoplayer = true;
                    for (int k = 0; k < NPC.buffTime.Length; k++)
                    {
                        if (NPC.buffTime[k] > 5)
                            NPC.buffTime[k] -= 2;
                    }

                    acceerate = 4f;
                    acceeratedist = 150000f;
                    friction = 0.93f;

                    NPC master = Main.npc[(int)NPC.ai[3]];

                    Vector2 hellcenter = master.Center;
                    Vector2 thehelldir = player.Center - (hellcenter);

                    Vector2 aplay = thehelldir;

                    if (aplay.Length() < 0.2)
                        aplay = Vector2.UnitY;
                    else
                        aplay.Normalize();

                    float ittz = 1400f + (float)Math.Sin((NPC.ai[2] * 171.9424f) + NPC.ai[0] / 15f) * 600f;
                    float ittz2 = aioffset % 2 == 0 ? 1f : -1f;
                    float angles = (float)Math.Cos(MathHelper.ToRadians((NPC.ai[2] * 351.724f) - NPC.ai[0] * (ittz2 * 8f))) * 46f;
                    Vector2 former = theplayerdir;

                    theplayerdir = (master.Center - (aplay * 500) + ((aplay * ittz).RotatedBy(angles))) - NPC.Center;

                    if ((NPC.ai[0] + aioffset * 8) % 1700 > 1280)
                    {
                        acceerate = 8f;
                        if (former.Length() > 0)
                            former.Normalize();
                        else
                            former = Vector2.UnitY;

                        theplayerdir = (master.Center - ((aplay * 3500) + ((former * (ittz * 2f)).RotatedBy(angles)))) - NPC.Center;

                        if ((NPC.ai[0] + aioffset * 6) % 1700 == 1600)
                        {

                            bool fastlaser = (NPC.ai[0] + aioffset * 8) % 1700 == 1500;

                            Vector2 where = NPC.Center - (new Vector2(((NPC.ai[0] % 40) == 0) ? 1f : -1f, 0f) * 80f);
                            Vector2 where2 = player.Center - NPC.Center;
                            if (aplay.Length() < 0.2)
                                where2 = Vector2.UnitY;
                            else
                                where2.Normalize();
                            Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
                            {
                                float val = current;
                                if (time < 100)
                                    val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.05f);
                                else
                                    val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.01f);

                                return val;
                            };
                            if (!fastlaser)
                            {
                                projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
                                {
                                    return current;
                                };
                            }
                            Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
                            {

                                Vector2 gothere333 = NPC.Center;
                                Vector2 slideover = gothere333 - projpos;
                                current = slideover / 2f;

                                current /= 1.125f;

                                Vector2 speedz = current;
                                float spzzed = speedz.Length();
                                if (spzzed > 100f && speedz.Length()>0.25f)
                                {
                                    speedz.Normalize();
                                    current = (speedz * spzzed);
                                }

                                return current;
                            };

                            slowedSpeed = 350;
                            Hellion.GetHellion().manualmovement = 50;
                            Func<float, bool> projectilepattern = (time) => (time == 20);

                            int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 100, 200, NPC.rotation, Mod.Find<ModProjectile>("HellionBeam").Type, projectilepattern, 10f, 160, true);
                            (Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilefacing = projectilefacing;
                            (Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilemoving = projectilemoving;
                            SoundEngine.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
                            Main.projectile[ize2].hide = true;
                            Main.projectile[ize2].netUpdate = true;

                            if ((NPC.Center - player.Center).Length() < 100)
                                Spawnlaserportal();

                        }

                    }
                    else
                    {
                        //Main.NewText(handsAttack);


                        if ((NPC.ai[0] + aioffset * 64) % 1200 > 800 && (NPC.ai[0] % 3000 > 1400))
                        {
                            int len = 720 * 720;
                            if (handsAttack > 0 || (player.Center - NPC.Center).LengthSquared() > len)
                            {
                                nomove = 60;

                                friction = 0.98f;

                                int delay = (int)(NPC.ai[0] + aioffset * 8);

                                Vector2 thingz = player.Center - NPC.Center;

                                if ((delay) % 180 == 0 && handsAttack < -180 && (NPC.localAI[3] >= 300) && Vector2.Dot(Vector2.Normalize(thehelldir),Vector2.Normalize(thingz))>0.5f)
                                {
                                    thingz.Normalize();
                                    chargeAt = thingz * 120f;
                                    //npc.velocity += thingz * 85f;
                                    Idglib.Shattershots(NPC.Center, player.Center, new Vector2(0, 0), ModContent.ProjectileType<HellionCoreArmWarning>(), 2, 12, 160, 1, true, 0, false, 60);
                                    handsAttack = 100;
                                }

                                if (handsAttack == 75)
                                {
                                    //Vector2 thingz = player.Center - npc.Center;
                                    //thingz.Normalize();
                                    NPC.velocity = Vector2.Normalize(chargeAt) * 128f;

                                }

                                if (handsAttack < 1)//((delay - 80) % 180 < 20)
                                {
                                    if (NPC.velocity.LengthSquared() > 20 * 20)
                                        NPC.velocity *= 0.80f;
                                }

                                /*if ((delay + 60) % 180 < 60 && delay%10 == 0)
                                {
                                    Vector2 thingz = player.Center - npc.Center;
                                    thingz.Normalize();
                                    //npc.velocity += thingz * 85f;
                                    Idglib.Shattershots(npc.Center, player.Center, new Vector2(0, 0), ModContent.ProjectileType<HellionCorePlasmaAttackButGreen>(), 50, 32, 160, 1, true, 0, false, 200);

                                }*/

                                if (former.Length() > 0.25f)
                                    former = Vector2.UnitY;
                                else
                                    former.Normalize();
                                theplayerdir = (master.Center - (aplay * 2500) + ((former * ittz).RotatedBy(angles))) - NPC.Center;
                            }
                        }
                    }


                }


                if (noact > 0)
                    acceerate = 0.025f;

                float dist = theplayerdir.Length();
                if (dist > 0.25f)
                {
                    theplayerdir.Normalize();
                    if (nomove < 1)
                    {
                        if (noact < 160 && nomove < 1)
                            NPC.velocity += theplayerdir * (acceerate + (dist / acceeratedist));
                        if (slowedSpeed > 0)
                            maxspeed = 2;
                        if (NPC.velocity.Length() > maxspeed)
                            NPC.velocity = theplayerdir * maxspeed;

                        if (NPC.velocity.Length() > maxspeed / 2 && maxspeed >= 25f && phase > 1)
                        {
                            if (NPC.localAI[3] > 0)
                                NPC.localAI[3] = Math.Min(NPC.localAI[3] * 0.96f, 300);
                            NPC.localAI[3] = MathHelper.Max(NPC.localAI[3] - 10, -200);
                        }

                    }
                }

                NPC.velocity *= friction;

                if (facetoplayer)
                    NPC.rotation = NPC.rotation.AngleLerp((player.Center - NPC.Center).ToRotation(), 0.075f);
                else
                    NPC.rotation = NPC.rotation.AngleLerp(theplayerdir.ToRotation(), 0.075f);


            }

        }
        

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (NPC.localAI[3] < 300 || NPC.localAI[2]<300)
                return false;

            return (nomove > 0 || phase < 2);
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            base.OnHitByItem(player, item, damage, knockback, crit);
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            base.OnHitByProjectile(projectile, damage, knockback, crit);
        }

        public void WormSegment(Player player)
        {

            if (noact > 0)
                return;

            if (phase == 0)
            {

                if (NPC.ai[0] % 800 <= 400)
                {

                    /*if (npc.ai[0] % 160 == 30 + aioffset * 2)
                    {
                        Vector2 theplayerdir = player.Center - (npc.Center);
                        Idglib.Shattershots(npc.Center, player.Center, new Vector2(0, 0), ProjectileID.ShadowFlame, 30, 25, 60, 1, true, 0, false, 150);

                    }*/

                }
            }

            if (((NPC.ai[0] + (aioffset * 210))) % 600 == 400 && NPC.ai[0] % 800 < 400)
            {

                Spawnlaserportal();
            }

        }

        public void Spawnlaserportal()
        {
            Vector2 where = NPC.Center;
            Vector2 where2 = Main.player[NPC.target].Center - NPC.Center;
            Vector2 wheretogo2 = new Vector2(128f + ((((-NPC.ai[0] + aioffset) * 3.373475f) * 73.174f) % 96f), where2.ToRotation());
            where2.Normalize();
            Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
            {
                float val = current;
                val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.025f);

                return val;
            };
            Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
            {
                Vector2 wheretogo = new Vector2(wheretogo2.X, wheretogo2.Y);
                float angle = MathHelper.ToRadians(((wheretogo.Y + -time * 1.37f)));
                Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

                Vector2 gothere333 = NPC.Center + instore;
                Vector2 slideover = gothere333 - projpos;
                current = slideover / 2.5f;

                current /= 1.125f;

                Vector2 speedz = current;
                float spzzed = speedz.Length();
                speedz.Normalize();
                if (spzzed > 45f)
                    current = (speedz * spzzed);

                return current;
            };
            Func<float, bool> projectilepattern = (time) => (time % 70 == 0);
            if (phase < 1)
                projectilepattern = (time) => (time % 30 == 0);

            int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 20, phase < 1 ? 40 : 250, wheretogo2.Y, phase < 1 ? ModContent.ProjectileType<HellionCorePlasmaAttack>() : ProjectileID.DesertDjinnCurse,
                projectilepattern, phase < 1 ? 12f : 3f, phase < 1 ? 600 : 200);
            (Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilefacing = projectilefacing;
            (Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilemoving = projectilemoving;
            SoundEngine.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 45, 0.25f, 0.5f);
            Main.projectile[ize2].netUpdate = true;

        }

        public override void NPCLoot()
        {
            if (!Main.player[NPC.target].dead && !onlyOnce)// && SGAWorld.downedHellion>1)
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("ByteSoul").Type, 1);
            onlyOnce = true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (damage > 5000000)
            {
                NPCLoot();
            }
            else
            {
                if (tex != null)
                {
                    int size = tex.Width * tex.Height;
                    var datacolors2 = new Color[size];
                    tex.GetData(datacolors2);

                    NPC master = Main.npc[(int)NPC.ai[3]];

                    int losehp = (int)Math.Pow((master.lifeMax - master.life) / master.lifeMax, 1.05);

                    for (int i = 0; i < 1 + Math.Min(((damage / 3) + losehp) * 10, 1000); i += 1)
                    {
                        int ran1 = Main.rand.Next(tex.Width);
                        int ran2 = Main.rand.Next(tex.Height);

                        if (datacolors2[ran1 * ran2].A > 0)
                            datacolors2[ran1 * ran2] = Main.hslToRgb(Main.rand.NextFloat(), 0.75f, 0.4f);

                    }

                    tex.SetData(datacolors2);
                }
            }
        }

        public override bool PreAI()
        {

            handsAttack -= 1;
            NPC.localAI[3] += phase > 0 && noact > 0 ? 1 : (phase > 1 ? 3 : 1);
            NPC.dontTakeDamage = false;
            if (NPC.localAI[3] < 300)
            {
                NPC.dontTakeDamage = true;
                if (phase < 1)
                {
                    NPC.ai[0] -= 1;
                    noact = Math.Max(noact, 5);
                }
            }

            if (phase > 0 && noact > 0)
            {
                UnifiedRandom rando = new UnifiedRandom(NPC.whoAmI);
                NPC.localAI[3] -= 1;
                NPC.localAI[3] = Math.Min(NPC.localAI[3] * 0.99f, phase > 1 ? rando.Next(300, 1500) : 500);
            }

            if (phase < 2 || noact > 200)
            {
                /*for (int i = 0; i < 3; i++)
                {
                    if (Main.rand.Next(phase > 0 ? 120 : 0, 300) < npc.localAI[3])
                    {
                        float scale = MathHelper.Clamp((npc.localAI[3]) / 300f, 0f, 1f);
                        Vector2 spread = Main.rand.NextVector2Circular(640f, 640f) * (1.05f - scale);

                        ShadowParticle part = new ShadowParticle(
                            npc.Center + spread,
                            (npc.velocity) + Main.rand.NextVector2Circular(4f, 4f),
                            Vector2.One * scale * 0.16f,
                            30,
                            new Vector2(-0.01f, -0.01f),
                            new Vector2(0.975f, 0.975f),
                            Main.rand.NextFloat(MathHelper.TwoPi),
                            1f,
                            0.05f
                            );
                        part.alphaBoost += scale * 0.25f;
                        ShadowParticle.AddParticle(part);
                    }
                }*/
            }

            chargeWarning = 0;
            nomove -= 1;
            slowedSpeed -= 1;
            if (aioffset == 9990)
            {
                aioffset = (int)NPC.ai[0];
                NPC.ai[0] = 0;

                if (!Main.dedServ)
                {
                    Texture2D atex;

                    if (NPC.ai[2] > Math.PI)
                    {
                        rotoffset = 90f;
                        atex = ModContent.Request<Texture2D>("SGAmod/NPCs/Sharkvern/SharkvernHead");
                    }
                    else
                    {
                        /*int[,] spritetype = { { 223,180 },{ 224,0 },{ 516,0 },{ 67,0 },{ 24,0 },{15,0 },{819,0 },{ }


                        };*/
                        atex = SGAmod.HellionGores[Main.rand.Next(0, SGAmod.HellionGores.Count)];

                        rotoffset = Main.rand.Next(0, 360);
                        NPC.scale = Main.rand.NextFloat(1.5f, 2.5f);

                    }

                    tex = new Texture2D(Main.graphics.GraphicsDevice, atex.Width, atex.Height);

                    var datacolors2 = new Color[atex.Width * atex.Height];
                    atex.GetData(datacolors2);
                    tex.SetData(datacolors2);

                    int width = 20; int height = 300;

                    armtex = new Texture2D(Main.graphics.GraphicsDevice, width, height);
                    var dataColors = new Color[width * height];


                    //


                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x += 1)
                        {
                            int output = 32;

                            int inta = Main.rand.Next((int)x * -5, (int)x * 5);
                            output += inta;

                            dataColors[(int)x + y * width] = Main.hslToRgb((((y) + x) / 100f) % 1f, 0.75f, 0.5f);
                        }

                    }

                    armtex.SetData(dataColors);

                }
            }
            noact -= 1;

            Player player = Main.player[NPC.target];
            if (NPC.ai[3] > 0)
                NPC.realLife = (int)NPC.ai[3];
            if (NPC.target < 0 || NPC.target == byte.MaxValue || Main.player[NPC.target].dead)
            {
                NPC.TargetClosest(true);
                if (NPC.target < 0 || NPC.target == byte.MaxValue || Main.player[NPC.target].dead)
                    NPC.active = false;
            }
            else
            {
                NPC.ai[0] += 1;
            }
            if (!Main.npc[(int)NPC.ai[3]].active)
            {
                NPC.life = 0;
                NPC.HitEffect(0, 10000000.0);
                NPC.active = false;
                return false;
            }
            else
            {
                NPC.timeLeft = 500;

                NPC owner = Main.npc[(int)NPC.ai[3]];

                if (owner.life < (int)(owner.lifeMax * HellionCore.beginphase[0]) && phase == 0)
                {
                    phase = 1;
                    noact = 150;
                    NPC.ai[0] = (int)NPC.ai[2] * 350;
                    if (NPC.ai[2] > 0)
                    {
                        NPC.ai[1] = -1;
                    }
                }

                if (owner.life < (int)(owner.lifeMax * HellionCore.beginphase[1]) && phase == 1)
                {
                    NPC.localAI[3] += 10000;
                    phase = 2;
                    noact = 300;
                    NPC.ai[0] = aioffset * 3;

                    NPC.velocity = new Vector2(Main.rand.Next(-99999, 99999), Main.rand.Next(-99999, 99999));
                    if (NPC.velocity.Length() < 1)
                        NPC.velocity = Vector2.UnitY;
                    NPC.velocity.Normalize();
                    NPC.velocity *= Main.rand.NextFloat(15f, 64f);

                    NPC.ai[1] = -1;
                    NPC.ai[2] = Main.rand.NextFloat(0f, MathHelper.ToRadians(360));
                    NPC.netUpdate = true;
                }

            }

            if ((int)NPC.ai[1] <0)
            {
                /*if (Main.rand.Next(200, 300) < npc.localAI[3])
                {
                    float scale = MathHelper.Clamp((npc.localAI[3]) / 300f, 0f, 1f);
                    Vector2 spread = Main.rand.NextVector2Circular(64f, 64f) * (1.25f - scale);
                    ShadowParticle.AddParticle(new ShadowParticle(
                        npc.Center + spread,
                        (npc.velocity) + Main.rand.NextVector2Circular(2f, 2f),
                        Vector2.One * scale * 0.36f,
                        30,
                        new Vector2(-0.01f, -0.01f),
                        new Vector2(0.975f, 0.975f),
                        Main.rand.NextFloat(MathHelper.TwoPi),
                        1f,
                        0.05f
                        ));
                }*/

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[2] += 1;
                    //npc.life = 0;
                    //npc.HitEffect(0, 10.0);
                    //npc.active = false;
                    WormHead(player);

                    return false;
                }
            }

            //If we are a worm segment: below:

            if ((int)NPC.ai[1] > -1 && NPC.ai[1] < (double)Main.npc.Length && Main.npc[(int)NPC.ai[1]].type == Mod.Find<ModNPC>("HellionWorm").Type)
            {
                NPC.localAI[2] += 1;
                float scalie = (NPC.localAI[2] / 300f);
                if (scalie > 0f)
                {
                    Vector2 npcCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);

                    float dirX = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - npcCenter.X;
                    float dirY = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - npcCenter.Y;
                    KeepUpright(dirX, dirY);
                    NPC.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                    float length = ((float)Math.Sqrt(dirX * dirX + dirY * dirY));
                    float dist = (length - (float)NPC.width) / length;
                    float posX = dirX * dist;
                    float posY = dirY * dist;
                    NPC.velocity *= 0.90f;
                    NPC.position = NPC.position + (new Vector2(posX, posY) * MathHelper.Clamp(scalie * scalie* scalie, 0.005f, 0.25f));

                    WormSegment(player);
                }
                
            }

            return false;

        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            DrawMe(spriteBatch, drawColor);
            return false;
        }

        public void DrawMe(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor, float scale = 1f)
        {

            Idglib.coloroverride = Color.White;

            NPC myowner = Main.npc[(int)NPC.ai[3]];

            float alpha = MathHelper.Clamp((NPC.localAI[3] - 150) / 150f, 0.0f, 1f);

            if (NPC.ai[1] < 0 && phase < 2)
            {
                List<NPC> worms = new List<NPC>();
                NPC current = NPC;
                worms.Add(NPC);
                for (int i = 0; i < Main.npc.Length; i += 1)
                {
                    NPC thatNPC = Main.npc[i];
                    if (thatNPC.active && thatNPC.type == ModContent.NPCType<HellionWorm>())
                    {
                        if (thatNPC.ai[1] == current.whoAmI)
                        {
                            worms.Add(thatNPC);
                            current = thatNPC;
                        }
                    }
                }

                if (worms.Count > 1)
                {
                    Effects.TrailHelper trail = new Effects.TrailHelper("FadedBasicEffectAlphaPass", Mod.Assets.Request<Texture2D>("ElectricFireNoColor").Value);
                    trail.color = delegate (float percent)
                    {
                        NPC wormDude = worms[(int)Math.Min(((worms.Count-1) * percent), worms.Count - 1)];

                        return Main.hslToRgb((percent + Main.GlobalTimeWrappedHourly / 3f) % 1f, 0.75f, 0.75f) * MathHelper.Clamp(wormDude.localAI[3] / 300f, 0f, 1f) * MathHelper.Clamp((wormDude.localAI[2]+100) / 300f, 0f, 1f);
                    };

                    trail.projsize = Vector2.Zero;
                    trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -0.75f);
                    trail.coordMultiplier = new Vector2(1f, worms.Count/10f);
                    trail.trailThickness = 32f;
                    trail.trailThicknessIncrease = 0;
                    trail.doFade = false;
                    trail.connectEnds = false;
                    trail.strength = MathHelper.Clamp((NPC.localAI[3]-150) / 150f, 0f, 1f);

                    List<Vector2> listOfPoints = new List<Vector2>();
                    listOfPoints.AddRange(worms.Select(testby => testby.Center).ToList());

                    trail.DrawTrail(listOfPoints);
                }

            }


            //if (scale >= 0f)
            //{

            if (chargeWarning > 0 && Main.player[NPC.target] != null)
            {
                Vector2 there = (Main.player[NPC.target].Center - NPC.Center);
                float dist = there.Length();

                List<Vector2> toThem = new List<Vector2>();

                toThem.Add(NPC.Center);
                toThem.Add(Main.player[NPC.target].Center);

                TrailHelper trail = new TrailHelper("FadedBasicEffectPass", Mod.Assets.Request<Texture2D>("SmallLaser").Value);
                trail.projsize = Vector2.Zero;
                trail.coordOffset = new Vector2(0, -Main.GlobalTimeWrappedHourly*0.75f);
                trail.coordMultiplier = new Vector2(1f, 1f);
                trail.trailThickness = (float)chargeWarning / 10f;
                trail.trailThicknessIncrease = 0;
                trail.doFade = false;
                trail.strength = MathHelper.Clamp(chargeWarning / 52f, 0f, 1f);
                trail.color = delegate (float percent)
                {
                    return Color.Lerp(Color.Lime,Color.Red, MathHelper.Clamp(chargeWarning / 96f,0f,1f));
                };
                trail.DrawTrail(toThem);

                spriteBatch.Draw(Main.quicksIconTexture, (Main.player[NPC.target].Center-Vector2.Normalize(there)*72f) - Main.screenPosition, null, Color.White*MathHelper.Clamp(chargeWarning / 64f,0f,1f), 0, Main.quicksIconTexture.Size()/2f, 2f+ MathHelper.Clamp(chargeWarning / 32f, 0f, 2f), SpriteEffects.None, 0f);

                //spriteBatch.Draw(Main.blackTileTexture, (npc.Center - Main.screenPosition), new Rectangle(0, 0, 1, 8), Color.Red * MathHelper.Clamp((float)chargeWarning / 90f, 0f, 1f), there.ToRotation(), new Vector2(0, 4), new Vector2(dist, (float)chargeWarning / 30f), SpriteEffects.None, 0f);
            }

            if (armtex != null && phase > 1 && noact < 200)
            {
                Idglib.coloroverride = Color.White * alpha;
                int aval = noact > 0 ? 0 : (int)NPC.localAI[3];

                if (aval < 5)
                    aval = 5;

                if (myowner != null && myowner.active && Main.rand.Next(aval, aval + 200) > 250)
                {
                    Idglib.DrawSkeletronLikeArms(spriteBatch, armtex, NPC.Center, myowner.Center, 0f, 0f, MathHelper.Clamp((myowner.Center.X - NPC.Center.X) * 0.02f, -1, 1));
                    Vector2 drawPos = ((Idglib.skeletronarmjointpos - Main.screenPosition)) + new Vector2(0f, 0f);
                    Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/TPD").Value;
                    spriteBatch.Draw(texture, drawPos, null, Main.DiscoColor * alpha, NPC.spriteDirection + (NPC.ai[0] * 0.4f), new Vector2(16, 16), new Vector2(Main.rand.Next(15, 35) / 4f, Main.rand.Next(15, 35) / 4f), SpriteEffects.None, 0f);
                }
            }
            // }
            //else
            //{

            if (tex != null)
            {
                float distort = 1f - MathHelper.Clamp((NPC.localAI[3] - 0) / 300f, 0f, 1f);

                Texture2D texture = tex;//Main.npcTexture[npc.type];
                Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);

                Vector2 offset = Vector2.Zero;
                for (int i = 0; i < 1 + (alpha < 1 ? 10 : 0); i += 1)
                {
                    if (alpha < 1)
                        offset = Main.rand.NextVector2Circular(distort * 960f, distort * 960f);
                    Main.spriteBatch.Draw(texture, (NPC.Center + offset - Main.screenPosition) * scale, new Rectangle?(), drawColor * alpha, NPC.rotation + MathHelper.ToRadians(rotoffset), origin, NPC.scale * scale, localdist.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }
            //}
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
    }


    public class HellionMonolog : ModNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellion Monologging");
            Main.npcFrameCount[NPC.type] = 1;
        }
        public override void SetDefaults()
        {
            NPC.width = 96;
            NPC.height = 96;
            NPC.damage = 0;
            NPC.defense = 10;
            NPC.lifeMax = 750000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 0;
            NPC.boss = true;
            //AIType = NPCID.BlueSlime;
            //AnimationType = NPCID.BlueSlime;
            NPC.noTileCollide = false;
            NPC.noGravity = false;
            music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Silence");
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(0, 90, 0, 0);
            NPC.netAlways = true;
            NPC.chaseable = false;
            NPC.dontTakeDamage = true;
        }

        public override bool PreAI()
        {
            Player P = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(false);
                P = Main.player[NPC.target];
                if (!P.active || P.dead)
                {
                    float speed = ((-10f));
                    NPC.velocity = new Vector2(NPC.velocity.X, NPC.velocity.Y + speed);
                    NPC.active = false;
                }

            }
            else
            {
                NPC.ai[0] += 1;
                if (NPC.ai[0] == 1 && SGAWorld.downedHellion < 1)
                {
                    if (SGAWorld.NightmareHardcore < 1)
                        NPC.ai[1] = 1;
                    for (int i = 0; i < 8 + P.extraAccessorySlots; i += 1)
                    {
                        if (P.armor[i].type == ModContent.ItemType<Items.Accessories.DevPower>())
                        {
                            NPC.ai[1] = 0;
                            break;
                        }
                    }
                }
                if (NPC.ai[0] > 200 && (NPC.ai[0] < 540 || NPC.ai[1] == 0))
                {
                    NPC.velocity += (((P.Center + new Vector2(0, -200)) - NPC.Center) / 100f);
                }
                NPC.velocity /= 1.5f;

            }
            Hellion Hellinstance = new Hellion();



            if (NPC.ai[0] == 120)
                Hellinstance.HellionTaunt("hmmmm...");
            if (NPC.ai[0] == 260)
                Hellinstance.HellionTaunt("It seems you have destroyed my most powerful creation...");

            if (NPC.ai[1] == 1)
            {
                if (SGAWorld.downedHellion == -1 && NPC.ai[0] == 100)
                {
                    Hellinstance.HellionTaunt("You are still too weak...");
                    NPC.ai[0] = 461;
                }

                if (NPC.ai[0] == 460)
                    Hellinstance.HellionTaunt("But no matter...");

                if (NPC.ai[0] == 560)
                    Hellinstance.HellionTaunt("Only with the power of [The Secret of Souls] would you ever hope to succeed...");

                if (NPC.ai[0] == 600)
                {
                    Projectile.NewProjectile(NPC.Center, Vector2.Zero, SGAmod.Instance.Find<ModProjectile>("HellionTeleport").Type, 0, 2f);
                }

                if (NPC.ai[0] == 660)
                {
                    NPC.life = 0;
                    NPC.active = false;
                    SGAWorld.downedHellion = -1;
                }

            }
            else
            {

                if (NPC.ai[0] == 460)
                    Hellinstance.HellionTaunt("Your powerful, very powerful, maybe that's why he fled to you.");
                if (NPC.ai[0] == 700)
                    Hellinstance.HellionTaunt("With that kind of power, we could have been allies, But in the end...");
                if (NPC.ai[0] == 1000)
                    Hellinstance.HellionTaunt("I have put far too many resources into the project");
                if (NPC.ai[0] == 1100)
                    Hellinstance.HellionTaunt("And far too many slip ups to have let accured");
                if (NPC.ai[0] == 1300)
                    Hellinstance.HellionTaunt("So...");
                if (NPC.ai[0] == 1500)
                    Hellinstance.HellionTaunt("I think it's finally time, I ended this, myself");

                if (NPC.ai[0] == 1700)
                {
                    NPC.NewNPC((int)(NPC.position.X + (float)(NPC.width / 2)), (int)NPC.position.Y + NPC.height / 2, Mod.Find<ModNPC>("Hellion").Type, NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                    NPC.life = 0;
                    NPC.active = false;

                    if (SGAWorld.downedHellion < 1)
                    {
                        SGAWorld.downedHellion = 1;
                    }

                }

            }

            //					int prog = Projectile.NewProjectile(npc.Center, Vector2.Zero, SGAmod.Instance.ProjectileType("HellionTeleport"), 0, 2f);

            return false;
        }

        public override string Texture
        {
            get { return ("Terraria/Projectile_" + ProjectileID.Starfury); }
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Hellion.HellionTeleport(spriteBatch, NPC.Center, 1f, 96);
            return false;
        }

    }













    [AutoloadBossHead]
    public class HellionCore : Hellion
    {

        public static float[] beginphase = { 0.70f, 0.40f };
        int hellionmessages = 0;
        int repairs = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellion Core");
            Main.npcFrameCount[NPC.type] = 1;
        }
        public override void SetDefaults()
        {
            NPC.width = 96;
            NPC.height = 96;
            NPC.damage = 0;
            NPC.defense = 10;
            NPC.lifeMax = 750000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 0f;
            NPC.knockBackResist = 0f;
            NPC.aiStyle = 0;
            NPC.boss = true;
            //AIType = NPCID.BlueSlime;
            //AnimationType = NPCID.BlueSlime;
            NPC.noTileCollide = false;
            NPC.noGravity = false;
            music = MusicID.Boss3;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            //bossBag = mod.ItemType("SPinkyBag");
            NPC.value = Item.buyPrice(0, 90, 0, 0);
            NPC.netAlways = true;
            NPC.chaseable = false;
        }

        public override void NPCLoot()
        {

            int num154 = NPC.NewNPC((int)(NPC.position.X + (float)(NPC.width / 2)), (int)NPC.position.Y + NPC.height / 2, Mod.Find<ModNPC>("HellionMonolog").Type, NPC.whoAmI, SGAWorld.downedHellion < 1 || TheWholeExperience.Check() ? 0f : 1698f, 0f, 0f, 0f, 255);

        }

        //public override string BossHeadTexture => "SGAmod/NPCs/Hellion/HellionCore_Head_Boss";

        
        public override string Texture
        {
            get { return ("SGAmod/NPCs/Hellion/Hellioncore"); }
        }
        

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float clamper = MathHelper.Clamp((NPC.localAI[0] - 60) / 60f, 0f, 1f);
            Hellion.HellionTeleport(spriteBatch, NPC.Center, clamper, 96 * clamper);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void AdvancePhases()
        {
            if (hellionmessages == 0 && NPC.life < NPC.lifeMax * 0.9)
            {
                HellionTaunt("PIXEL HULL BREACHED! RELEASING REPAIR WRAITHS!", 1);
                hellionmessages = 2;
                repairs += 5;
            }
            if (hellionmessages == 2 && NPC.life < NPC.lifeMax * 0.8)
            {
                HellionTaunt("You ain't bad! But you won't last much longer");
                hellionmessages = 3;
            }
            if (hellionmessages == 3 && NPC.life < NPC.lifeMax * 0.75)
            {
                HellionTaunt("HULL SUSTAINED DAMAGE, SUGGESTED COURSE OF ACTION: DODGE!", 1);
                hellionmessages = 4;
            }
            if (hellionmessages == 4 && NPC.life < NPC.lifeMax * 0.65)
            {
                HellionTaunt("CORRUPTION DETECTED, FURTHER REPAIRS NEEDED!", 1);
                hellionmessages = 5;
                repairs += 5;
            }
            if (hellionmessages == 5 && NPC.life < NPC.lifeMax * 0.6)
            {
                HellionTaunt("ALERT: TERRARIAN PROVING OVERPOWERING, CHANCES OF SUCCESS DROPPING!", 1);
                HellionTaunt("RELEASING REMAINING REPAIR WRAITHS!", 1);
                hellionmessages = 6;
                repairs += 25;
            }
            if (hellionmessages == 6 && repairs < 1)
            {
                HellionTaunt("REPAIR WRAITHS DEPLETED", 1);
                hellionmessages = 7;
            }
            if (hellionmessages < 20 && NPC.life < NPC.lifeMax * 0.15)
            {
                HellionTaunt("SYSTEMS ERROR! SYSTEMS BADLY DAMAGED: RETREAT HIGHLY SUGGESTED!", 1);
                hellionmessages = 20;
            }

            if (NPC.life < (int)(NPC.lifeMax * HellionCore.beginphase[0]) && phase == 0)
            {
                phase = 1;
                HellionTaunt("Engauge Seperate Protocall!");
            }
            if (NPC.life < (int)(NPC.lifeMax * HellionCore.beginphase[1]) && phase == 1)
            {
                NPC.ai[0] = 0;
                phase = 2;
                HellionTaunt("Begin Planetary Ravanger mode!");
            }
            if (phase == 2 && NPC.ai[0] == 80)
                HellionTaunt("Time to meet your end!");
            if (phase == 2 && NPC.ai[0] == 200)
                HellionTaunt("Now witness the power that ended your friend's world!");

        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossLifeScale);
            NPC.damage = (int)(NPC.damage * 0.6f);
        }
        public override bool PreAI()
        {
            if (!Main.expertMode)
            {
                HellionTaunt("NO U, normie difficulty");
                NPC.active = false;
            }

            NPC.localAI[0] += 1;

            if (NPC.localAI[0] < 150)
            {
                for (int f = 0; f < 6; f++)
                {
                    float scale2 = (float)Math.Sin((NPC.localAI[0] * MathHelper.Pi) / 150);
                    if (Main.rand.Next(0, 30) < scale2 * 45f)
                    {
                        float direction = f + (NPC.localAI[0] / 60f);
                        //float scale = MathHelper.Clamp((npc.localAI[0]) / 300f, 0f, 1f);
                        Vector2 spread = Vector2.UnitX.RotatedBy((direction / 6f) * MathHelper.TwoPi) * (1f - scale2) * 640f;

                        ShadowParticle part = new ShadowParticle(
                            NPC.Center + spread,
                            (NPC.velocity) + Main.rand.NextVector2Circular(4f, 4f),
                            Vector2.One * scale2 * 0.75f,
                            30,
                            new Vector2(-0.01f, -0.01f),
                            new Vector2(0.975f, 0.975f),
                            Main.rand.NextFloat(MathHelper.TwoPi),
                            1f,
                            0.05f
                            );
                        part.alphaBoost += scale2 * 0.25f;
                        ShadowParticle.AddParticle(part);
                    }
                }
            }

            Player P = Main.player[NPC.target];
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(false);
                P = Main.player[NPC.target];
                if (!P.active || P.dead)
                {
                    float speed = ((-10f));
                    NPC.velocity = new Vector2(NPC.velocity.X, NPC.velocity.Y + speed);
                    NPC.active = false;
                }

            }
            else
            {
                if (NPC.localAI[3] == 0)
                {
                    manualmovement = 200;
                    NPC.Center = P.Center + new Vector2(0, -256f);
                    NPC.localAI[3] = 1;
                }
                AdvancePhases();
                NPC.netUpdate = true;
                NPC.timeLeft = 99999;
                manualmovement -= 1;

                NPC.ai[0] += 1;

                if (repairs > 0)
                {
                    if (NPC.ai[0] % 200 == 0)
                    {
                        int num154 = NPC.NewNPC((int)(NPC.position.X + (float)(NPC.width / 2)), (int)NPC.position.Y + NPC.height / 2, Mod.Find<ModNPC>("HealingDrone").Type, NPC.whoAmI, 0f, 0f, 0f, 0f, 255);
                        Main.npc[num154].target = NPC.target;
                        Main.npc[num154].lifeMax = (int)(NPC.lifeMax * 0.004);
                        Main.npc[num154].life = Main.npc[num154].lifeMax;
                        Main.npc[num154].netUpdate = true;

                        repairs -= 1;
                    }

                }


                if (NPC.ai[0] == 1)
                {
                    int latestNPC = -1;
                    for (int i = 0; i < 60; ++i)
                    {
                        int npc2;

                        npc2 = NPC.NewNPC((int)NPC.Center.X + Main.rand.Next(-640, 640), (int)NPC.Center.Y + Main.rand.Next(-640, 640), Mod.Find<ModNPC>("HellionWorm").Type);//NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("HellionWorm"), npc.whoAmI, 0, latestNPC);
                        Main.npc[(int)npc2].realLife = NPC.whoAmI;
                        Main.npc[(int)npc2].ai[3] = NPC.whoAmI;
                        Main.npc[(int)npc2].ai[0] = i;
                        Main.npc[(int)npc2].localAI[2] = -(200+(i * 6));
                        Main.npc[(int)npc2].localAI[3] = -(i * 4);

                        if (i % 20 == 0)
                            Main.npc[(int)npc2].ai[2] = (MathHelper.TwoPi) + MathHelper.ToRadians(i * (360 / 60));
                        Main.npc[(int)npc2].ai[1] = latestNPC;
                        Main.npc[(int)npc2].netUpdate = true;
                        latestNPC = npc2;

                    }
                }


                Vector2 gothere = NPC.Center + new Vector2(0, 1);
                int divider = 1;
                float accelerate = 250f;


                if (phase < 2)
                {
                    
                    for (int i = 0; i < Main.maxNPCs; i += 1)
                    {
                        if (Main.npc[i].active && Main.npc[i].type == Mod.Find<ModNPC>("HellionWorm").Type)
                        {
                            divider += 1;
                            gothere += Main.npc[i].Center;
                        }
                    }
                    if (divider>5)
                    gothere /= divider;
                    

                }
                else
                {
                    accelerate = 2500;
                    gothere = P.Center + new Vector2(600 * ((NPC.ai[0] % 1400 > 700) ? 1f : -1f), 0);

                    if (NPC.ai[0] < 600 && NPC.ai[0] >= 100)
                        NPC.life = (int)Math.Min(NPC.life + ((float)((NPC.lifeMax / 2f) * (1f - HellionCore.beginphase[1]) / 500f)), NPC.lifeMax);

                    if (NPC.ai[0] % 10 == 0 && NPC.ai[0] % 1850 > 1500)
                    {

                        Vector2 where = NPC.Center;
                        Vector2 wheretogo2 = new Vector2(320f + (NPC.ai[0] * 3739.6792f) % 960f, (NPC.ai[0] * 6834.392f) % 360f);
                        Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
                        {
                            float val = current;
                            val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.15f);

                            return val;
                        };
                        Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
                        {
                            Vector2 wheretogo = playerpos + new Vector2((float)Math.Cos(wheretogo2.Y), (float)Math.Sin(wheretogo2.Y)) * wheretogo2.X;

                            Vector2 gothere333 = wheretogo;
                            Vector2 slideover = gothere333 - projpos;
                            current = slideover / 2f;

                            current /= 3f;

                            Vector2 speedz = current;
                            float spzzed = speedz.Length();
                            speedz.Normalize();
                            if (spzzed > 25f)
                                current = (speedz * spzzed);

                            return current;
                        };
                        Func<float, bool> projectilepattern = (time) => (time > 120 && time % 80 == 0);

                        int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 75, 200, wheretogo2.Y, ProjectileID.DesertDjinnCurse, projectilepattern, 1f, 200);
                        (Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilefacing = projectilefacing;
                        (Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilemoving = projectilemoving;
                        SoundEngine.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
                        Main.projectile[ize2].netUpdate = true;

                    }

                    if (NPC.ai[0] % 20 == 0 && NPC.ai[0] % 900 > 650)
                    {

                        Vector2 where = NPC.Center;
                        Vector2 wheretogo2 = P.Center + new Vector2(320f + (NPC.ai[0] * 3739.6792f) % 960f, (NPC.ai[0] * 6834.392f) % 360f);
                        Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
                        {
                            float val = current;
                            val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.15f);

                            return val;
                        };
                        Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
                        {
                            Vector2 wheretogo = new Vector2(wheretogo2.X, wheretogo2.Y);

                            Vector2 gothere333 = wheretogo;
                            Vector2 slideover = gothere333 - projpos;
                            current = slideover / 5f;

                            current /= 3f;

                            Vector2 speedz = current;
                            float spzzed = speedz.Length();
                            speedz.Normalize();
                            if (spzzed > 30f)
                                current = (speedz * spzzed);

                            return current;
                        };
                        Func<float, bool> projectilepattern = (time) => (time > 120 && time % 30 == 0);

                        int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 50, 240, wheretogo2.Y, ModContent.ProjectileType<HellionCorePlasmaAttack>(), projectilepattern, 15f, 800);
                        (Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilefacing = projectilefacing;
                        (Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilemoving = projectilemoving;
                        SoundEngine.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
                        Main.projectile[ize2].netUpdate = true;

                    }


                    if (NPC.ai[0] % 400 < 30)
                        accelerate = 400;

                }

                Vector2 gogo = gothere - NPC.Center;
                Vector2 gogo2 = gogo; gogo2.Normalize();

                if ((NPC.ai[0] % 1200 < 900) && NPC.ai[0] % 1200 != 950)
                {
                    if (manualmovement < 1)
                        NPC.velocity += gogo / accelerate;

                }
                else
                {

                    if (NPC.ai[0] % 1200 == 950)
                    {

                        for (int rotz = 0; rotz < 360; rotz += 360 / 3)
                        {

                            Vector2 where = NPC.Center - (new Vector2(((NPC.ai[0] % 40) == 0) ? 1f : -1f, 0f) * 80f);
                            Vector2 wheretogo2 = new Vector2(96f, rotz);
                            Vector2 where2 = P.Center - NPC.Center;
                            where2.Normalize();
                            Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
                            {
                                float val = current;
                                if (time < 100)
                                    val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.04f);
                                else
                                    val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.015f);

                                return val;
                            };
                            Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
                            {
                                Vector2 wheretogo = new Vector2(wheretogo2.X, wheretogo2.Y);
                                float angle = MathHelper.ToRadians(((wheretogo.Y + time * 2f)));
                                Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

                                Vector2 gothere333 = Hellion.GetHellion().NPC.Center + instore;
                                Vector2 slideover = gothere333 - projpos;
                                current = slideover / 2f;

                                current /= 1.125f;

                                Vector2 speedz = current;
                                float spzzed = speedz.Length();
                                speedz.Normalize();
                                if (spzzed > 25f)
                                    current = (speedz * spzzed);

                                return current;
                            };
                            Func<float, bool> projectilepattern = (time) => (time == 20);

                            int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 100, 250, wheretogo2.Y, Mod.Find<ModProjectile>("HellionBeam").Type, projectilepattern, 3f, 200, true);
                            (Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilefacing = projectilefacing;
                            (Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilemoving = projectilemoving;
                            SoundEngine.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
                            Main.projectile[ize2].netUpdate = true;

                        }
                    }


                }
                NPC.velocity /= 1.02f;

            }
            return false;

        }

        public override bool CheckDead()
        {
            return true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }
    }

    public class HealingDrone : DPSDrones
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Repair Support Wraith");
            Main.npcFrameCount[NPC.type] = 1;
            NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
        }
        public override void SetDefaults()
        {
            NPC.width = 16;
            NPC.height = 16;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 10000;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.knockBackResist = 0.05f;
            NPC.aiStyle = -1;
            NPC.boss = false;
            AnimationType = 0;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.value = Item.buyPrice(0, 1, 0, 0);
        }
        public override void AI()
        {

            //npc.netUpdate = true;
            NPC master = Main.npc[(int)NPC.ai[1]];
            Player P = Main.player[NPC.target];
            Hellion hell = Hellion.GetHellion();
            NPC.localAI[0] += 1;

            if (hell == null)
            {
                NPC.active = false;
                return;
            }

            NPC guy = Main.npc[(int)NPC.ai[1]];

            bool checksem = master.active && !master.dontTakeDamage && !master.friendly && master.type != NPC.type && master.whoAmI != NPC.whoAmI
                 && !master.immortal && master.chaseable;

            if (master == null || master.active == false || !checksem)
            {
                List<int> stuffs = new List<int>();
                for (int i = 0; i < Main.maxNPCs; i += 1)
                {
                    if (Main.npc[i] != null)
                        if (Main.npc[i].active)
                            if (!Main.npc[i].dontTakeDamage && !Main.npc[i].friendly && Main.npc[i].type != NPC.type && !master.immortal && master.chaseable)
                                stuffs.Add(i);
                }

                if (stuffs.Count > 0)
                {
                    NPC.ai[1] = stuffs[(Main.rand.Next(0, stuffs.Count))];
                }
                else
                {
                    if (hell != null)
                    {
                        NPC.ai[1] = hell.NPC.whoAmI;
                    }
                    else
                    {
                        return;
                    }
                }

                //if (!P.active || P.dead)
                //    npc.active = false;

            }




            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(false);
                P = Main.player[NPC.target];

            }
            else
            {
                if (master != null)
                {


                    NPC.ai[0] += 1;
                    if (NPC.ai[0] == 1)
                    {
                        NPC.ai[1] = Main.rand.NextFloat(0.0025f, 0.0075f) * (Main.rand.Next(0, 2) == 1 ? 1f : -1f);
                        NPC.ai[2] = Main.rand.NextFloat(0f, MathHelper.ToRadians(360));
                        NPC.netUpdate = true;

                    }
                    NPC.ai[2] += NPC.ai[1];
                    Vector2 tothere = master.Center;
                    tothere += new Vector2((float)Math.Cos(NPC.ai[2]), (float)Math.Sin(NPC.ai[2])) * (200);
                    tothere -= NPC.Center;
                    tothere.Normalize();
                    NPC.velocity += tothere * 1f;

                    if (NPC.ai[0] % 180 == 0)
                    {
                        Idglib.Shattershots(NPC.position, master.Center, new Vector2(0, 0), ProjectileID.DD2DarkMageHeal, 50, 12, 0, 1, true, 0, true, 300);
                        if (master.type == Mod.Find<ModNPC>("HellionWorm").Type)
                            hell.NPC.life = Math.Min(hell.NPC.lifeMax, hell.NPC.life + 200);
                        if (master.type == Mod.Find<ModNPC>("Hellion").Type)
                            hell.NPC.life = Math.Min(hell.NPC.lifeMax, hell.NPC.life + 500);
                    }

                }


                float fric = 0.97f;
                NPC.velocity = NPC.velocity * fric;
            }

        }

    }


    public class HellionCorePlasmaAttack : ModProjectile
    {
        public virtual Color Color => Color.White;
        public virtual Color Color2 => Color.Purple;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellion's Plasma");
        }
        public override string Texture => "Terraria/Projectile_" + ProjectileID.NebulaBolt;

        public override void SetDefaults()
        {
            if (GetType() != typeof(Items.Weapons.Technical.EngineerSentryShotProj))
            Projectile.CloneDefaults(ProjectileID.SnowBallFriendly);
            Projectile.tileCollide = true;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 3;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.penetrate = -1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1;
            Projectile.ai[0] += 1;

            if (Projectile.ai[0] == 1)
            {
                var snd = SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 68);
                if (snd != null)
                {
                    snd.Pitch = -0.5f;
                }
            }

            Projectile.position -= Projectile.velocity * (1f - MathHelper.Clamp((Projectile.ai[0] - 120) / 120f, 0.10f, 1f));

            Projectile.rotation = Projectile.velocity.ToRotation();

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            Texture2D texture = Main.projectileTexture[Projectile.type];
            Texture2D textureGlow = ModContent.Request<Texture2D>("SGAmod/Glow");

            float realVelocity = (MathHelper.Clamp((Projectile.ai[0] - 300) / 300, 0f, 1f));
            float realAlpha = MathHelper.Clamp(Projectile.localAI[0] / 300, 0f, 1f);
            float timeLeft = Math.Min(Projectile.timeLeft / 50f, 1f);

            Vector2 drawOrigin2 = texture.Size() / 2f;
            Vector2 drawOrigin3 = textureGlow.Size() / 2f;

            //Color color = Color.White;
            //Color color2 = Color.Purple;

            float alpha = MathHelper.Clamp(Projectile.localAI[0] / 30f, 0f, 1f);

            float scale = 2f - MathHelper.Clamp(Projectile.localAI[0] / 70f, 0f, 1f);

            float scaledpre = MathHelper.Clamp((Projectile.localAI[0] - 20) / 45f, 0f, 1f) * scale;

            float detail = 1f;// + projectile.velocity.Length();

            float maxtrail = (float)(Projectile.oldPos.Length - 15f);
            float maxtrail2 = (float)(Projectile.oldPos.Length - 1f);

            for (float f = maxtrail2; f >= 3f; f -= 0.25f)
            {
                Vector2 pos = Vector2.Lerp(Projectile.oldPos[(int)f - 1], Projectile.oldPos[(int)f], f % 1f);
                float rot = Projectile.oldRot[(int)f - 1];
                spriteBatch.Draw(texture, pos + (Projectile.Hitbox.Size() / 2f) - Main.screenPosition, null, Color * timeLeft * (1f / detail) * (1f - (f / maxtrail2)) * alpha, rot + MathHelper.PiOver2, drawOrigin2, new Vector2(0.2f, 2.0f) * scaledpre, SpriteEffects.None, 0f);
            }

            for (float f = maxtrail; f >= 1f; f -= 0.5f)
            {
                Vector2 pos = Vector2.Lerp(Projectile.oldPos[(int)f - 1], Projectile.oldPos[(int)f], f % 1f);
                float rot = Projectile.oldRot[(int)f - 1];
                spriteBatch.Draw(texture, pos + (Projectile.Hitbox.Size() / 2f) - Main.screenPosition, null, Color * timeLeft * (1f / detail) * (1f - (f / maxtrail)) * alpha, rot + MathHelper.PiOver2, drawOrigin2, scaledpre, SpriteEffects.None, 0f);
            }
            for (float f = maxtrail; f >= 1f; f -= 0.5f)
            {
                Vector2 pos = Vector2.Lerp(Projectile.oldPos[(int)f - 1], Projectile.oldPos[(int)f], f % 1f);
                float rot = Projectile.oldRot[(int)f - 1];
                spriteBatch.Draw(textureGlow, pos + (Projectile.Hitbox.Size() / 2f) - Main.screenPosition, null, Color2 * timeLeft * (0.75f / detail) * (1f - (f / maxtrail)) * alpha, rot + MathHelper.PiOver2, drawOrigin3, new Vector2(0.4f, 1.0f) * scaledpre, SpriteEffects.None, 0f);
            }

            return false;
        }
    }

    public class HellionCorePlasmaAttackButGreen : HellionCorePlasmaAttack
    {
        public override Color Color => Color.White;
        public override Color Color2 => Color.Lime;
        public override string Texture => "Terraria/Item_" + ItemID.LivingCursedFireBlock;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellion's Plasma But Green");
        }
    }

    public class HellionCoreArmWarning : PinkyWarning
    {
        protected override Color color => Color.Lime;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warning Forever! (Very Green With Envy)");
        }
        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
            Projectile.width = 8;
            Projectile.height = 20;
            Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 1;
            AIType = -1;
            Projectile.aiStyle = -1;
            timeleft = 60;
        }

        public override bool PreKill(int timeLeft)
        {
            return true;
        }

        public override bool CanDamage()
        {
            return false;
        }
    }

}