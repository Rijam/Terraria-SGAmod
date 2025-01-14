//#define BossDebug
using System.Linq;
using System;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using SGAmod.Effects;
using SGAmod.Dimensions;
using Microsoft.Xna.Framework.Audio;
using SGAmod.NPCs.Hellion;
using Terraria.Cinematics;
using Terraria.Graphics.Effects;
using SGAmod.Buffs;
using SGAmod.Dusts;

namespace SGAmod.Dimensions.NPCs
{
    public class SpaceBossEye
    {
        public Vector2 lookTo = default;
        public Vector2 previousLookTo = Vector2.Zero;
        public Vector2 currentLook = Vector2.Zero;
        public object boss;
        public float progress = 0f;
        public float movespeed = 1f;
        public float distanceScale = 300f;
        public int customID = 0;
        public float eyeMaxDistance = 12f;
        public float sleep = 1f;
        public float beamAlpha = 1f;
        public float eyeScale = 1f;
        public bool openUp = true;
        public bool exploded = false;

        public Vector2 offset = Vector2.Zero;
        public Vector2 Originaloffset = Vector2.Zero;

        public Vector2 BasePosition
        {
            get
            {
                if (boss is SpaceBoss spacez)
                    return spacez.npc.Center;
                if (boss is SpaceBossRock spacex)
                    return spacex.Position;

                return Vector2.Zero;
            }
        }

        public SpaceBossEye(Vector2 lookAt, object boss, Vector2 offset)
        {
            this.boss = boss;
            this.lookTo = lookAt;
            this.offset = offset;
            Originaloffset = offset;
        }

        public void NewLook(Vector2 lookat, float timetolook, bool resetprogress = true, bool resetlook = true)
        {
            if (resetlook)
                previousLookTo = currentLook;
            if (resetprogress)
                progress = 0f;

            if (boss is SpaceBoss spacez)
                lookTo = lookat - (spacez.npc.Center + offset);
            if (boss is SpaceBossRock spacex)
            {
                if (spacex.followBoss)
                    lookTo = lookat - (spacex.Position + spacex.bossOffset + offset);
                else
                    lookTo = lookat - (spacex.bossOffset + offset);
            }

            movespeed = timetolook;
        }

        public void Explode()
        {
            if (!exploded)
            {
                exploded = true;
                for (int i = 0; i < 64; i += 1)
                {
                    Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                    int dust = Dust.NewDust(new Vector2(BasePosition.X, BasePosition.Y) + offset * Main.rand.NextFloat(4f, 16f), 0, 0, DustID.AncientLight);
                    Main.dust[dust].scale = 1.5f;
                    Main.dust[dust].velocity = Vector2.Normalize(-offset) * (float)(2f * Main.rand.NextFloat(1.50f, 4.50f));
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].color = Color.Blue;
                }
                SoundEffectInstance sound = Main.PlaySound(SoundID.NPCKilled, (int)BasePosition.X, (int)BasePosition.Y, 12);
                if (sound != null)
                {
                    sound.Pitch = -0.5f;
                }

                sound = Main.PlaySound(SoundID.NPCKilled, (int)BasePosition.X, (int)BasePosition.Y, 58);
                if (sound != null)
                {
                    sound.Pitch = -0.5f;
                }

            }

        }

        public void Update()
        {
            if (sleep > 0)
            {
                if (!openUp)
                {
                    openUp = true;

                    SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)BasePosition.X, (int)BasePosition.Y, 32);
                    if (sound != null)
                    {
                        sound.Volume = 0.99f;
                        sound.Pitch -= 0.5f;
                    }
                    sound = Main.PlaySound(SoundID.DD2_DrakinBreathIn, (int)BasePosition.X, (int)BasePosition.Y);
                    if (sound != null)
                    {
                        sound.Volume = 0.99f;
                        sound.Pitch -= 0.5f;
                    }
                }
            }
            else
            {
                openUp = false;
            }
            progress += movespeed;
            currentLook = Vector2.Lerp(previousLookTo, lookTo, MathHelper.Clamp(progress, 0f, 1f));
        }

        public float EyeDistance => MathHelper.Clamp(currentLook.Length() / distanceScale, 0f, 1f);
        public float EyeAngle => currentLook.ToRotation();
    }

    public class SpaceBossRock
    {
        public Vector2 bossOffset = Vector2.Zero;
        public Vector2 lastPlace = Vector2.Zero;
        public SpaceBoss boss;
        public bool followBoss = false;
        protected float baserotation = 0f;
        protected float randomrotation = 0f;
        public float randomRotation2 = 0f;
        public Color color = Color.Gray;
        public float scale = 1f;
        public float beamAlpha = 0f;
        public SpaceBossEye eye;
        public int state = 0;
        public int timer = 0;
        public int stateTimer = 0;
        public int airReady = 0;
        public Vector2 velocity = Vector2.Zero;
        public MineableAsteriod grabrock;
        public float whiten = 0;
        public float bossRockDist;
        public int variation = 0;
        public int index = 0;
        public int indexMax = 0;

        public float Rotation
        {
            get
            {
                return baserotation + randomrotation;
            }
        }

        public Vector2 Position
        {
            get
            {
                if (followBoss)
                {
                    return boss.npc.Center + bossOffset.RotatedBy(boss.npc.rotation);
                }
                else
                {
                    return bossOffset;

                }
            }
        }

        public SpaceBossRock(Vector2 offset, SpaceBoss boss)
        {
            this.boss = boss;
            this.bossOffset = offset;
            randomrotation = Main.rand.NextFloat(MathHelper.TwoPi);
            randomRotation2 = Main.rand.NextFloat(-0.1f, 0.1f);
            scale = Main.rand.NextFloat(0.50f, 0.75f);
            bossRockDist = Main.rand.NextFloat(8f, 72f);
            variation = Main.rand.Next();
        }

        public void UpdateRotation(float phi)
        {
            baserotation += phi;
        }

        public void NebulaAttack()
        {


        }

        public void BossRocksUpdate()
        {

            Vector2 posa = Position;

            if (state < 1)
                stateTimer = 0;

            bool ragegrab = boss.TossRoids;
            bool installgrab = boss.installRoids > 0 && boss.npc.ai[0] < 11900;
            bool chance = Main.rand.Next(0, 1000) < boss.phase || (boss.TossRoids && Main.rand.Next(0, 100) < 1) || installgrab;
            chance = chance && boss.goingDark < 1;

            //Shadow Nebula Form
            if (state < 300 && boss.goingDark > 0)
            {
                Vector2 posser = Position;
                state = 300;
                grabrock = null;
                LockUnlock(false);
                bossOffset = posser;
            }
            if (state == 300)
            {
                stateTimer += 1;
                float maxTime = 300;
                float maxTimeScale = 600;
                float scaleDist = (96f + (MathHelper.SmoothStep(0f, 128f, stateTimer / maxTimeScale) * MathHelper.SmoothStep(0f, 640f, MathHelper.Clamp(stateTimer / maxTimeScale, 0f, 1f))));

                float gotoDist = MathHelper.Clamp(stateTimer / 120f, 0f, 1f);

                float DistToSpace = 120f + (float)Math.Sin(((stateTimer / 160f) + (index / (float)indexMax)) * MathHelper.TwoPi) * 72f;

                float spinRate = (stateTimer / 60f) * MathHelper.SmoothStep(1f, 0.25f, MathHelper.Clamp(stateTimer / 600f, 0f, 1f));

                float spinRate2 = MathHelper.Clamp(1f - (stateTimer / 420f), 0f, 1f);


                Vector2 gothere = Vector2.UnitX.RotatedBy(spinRate * ((index / (float)indexMax) * MathHelper.TwoPi)) *
                    (96f + (MathHelper.SmoothStep(0f, MathHelper.SmoothStep(DistToSpace, 360f, spinRate2), stateTimer / maxTime) * MathHelper.SmoothStep(0f, 1f, MathHelper.Clamp(stateTimer / maxTime, 0f, 1f))));

                gothere += boss.npc.Center;

                Vector2 diff = Position - boss.npc.Center;
                Vector2 diff2 = Vector2.Normalize(diff);

                Vector2 finalDist = Vector2.Lerp(bossOffset + (diff2 * 2f), gothere, MathHelper.SmoothStep(0f, 1f, gotoDist));
                bossOffset = finalDist;

                if (boss.goingDark < 1)
                {
                    velocity += Vector2.Normalize(diff) * 48f;
                    state = 102;
                    stateTimer = 0;
                }

            }

            //Celestial Celerity Form
            if (state < 400 && boss.celerityBuff > 0 && index % 4==0)
            {
                Vector2 posser = Position;
                state = 400;
                grabrock = null;
                LockUnlock(false);
                bossOffset = posser;
            }
            if (state == 400)//Boost mode, from Celestial Celerity
            {
                stateTimer += 1;
                if (boss.celerityBuff < 0)
                {
                    Vector2 diff = Position - boss.npc.Center;
                    velocity += Vector2.Normalize(diff) * 24f;
                    state = 102;
                    stateTimer = 0;
                }

                Vector2 vel = boss.npc.velocity;

                float timers = (stateTimer / 30f);

                float velspeed = vel.Length();

                if (velspeed <= 2f)
                {
                    float timerz = (index % 2 == 0 ? 1f : -1f);
                    vel = ((index / (float)indexMax)*(MathHelper.TwoPi* timerz) + (timers* timerz)).ToRotationVector2()*0.75f;
                }

                Vector2 gothere = Vector2.UnitX;

                float half = 0.50f;

                float angle = ((index/(float)indexMax)-0.50f);// (float)Math.Sin(((index / (float)indexMax) + timers) * MathHelper.TwoPi)+MathHelper.PiOver2;

                gothere = gothere.RotatedBy((angle * (MathHelper.PiOver2*1.25f)) + vel.ToRotation() + MathHelper.Pi) * (3f- MathHelper.Clamp(velspeed / 12f, 1f, 2.25f))*32f;

                bossOffset = Vector2.SmoothStep(bossOffset, boss.npc.Center + gothere, MathHelper.Clamp(stateTimer/600f,0f,MathHelper.Clamp(velspeed/8f,0.4f,2f))*0.47f);

                if (velspeed > 2)
                {

                    SpaceBoss.SpaceBossStardust dusts = new SpaceBoss.SpaceBossStardust(posa, Vector2.Normalize(gothere) * (velspeed/6f)+Main.rand.NextVector2Circular(3f,3f)*(velspeed/26f),Main.rand.Next(20,30));
                    dusts.scale = Vector2.One*0.6f;
                    dusts.fadeIn = 0.32f;
                    dusts.alpha = 1f;
                    dusts.fadeTime = Main.rand.Next(10,30);
                    boss.stardustEffectsOnTopOfRocks.Add(dusts);

                    /*
                    int dust = Dust.NewDust(posa, 0, 0, DustID.BlueCrystalShard);
                    Main.dust[dust].scale = 1f;
                    Main.dust[dust].alpha = 200;
                    Main.dust[dust].velocity = Vector2.Normalize(gothere) * (velspeed);
                    Main.dust[dust].noGravity = true;
                    */
                }
            }



               if (installgrab && (state == 100 || state == 200))
            {
                int dust = Dust.NewDust(posa, 0, 0, DustID.BlueCrystalShard);
                Main.dust[dust].scale = 1f;
                Main.dust[dust].alpha = 200;
                Main.dust[dust].noGravity = true;
            }

            //Main.NewText(installgrab);

            if (state < 1 && boss.state < 10 && chance)
            {
                Projectile[] roids = boss.AllAsteriods.Where(testby => testby.velocity.LengthSquared() < 20f && !testby.ignoreWater).OrderBy(testby => (testby.Center - posa).LengthSquared() + (testby.damage > 0 ? 1000000 : 0)).ToArray();

                //Main.NewText(roids.Length);

                if (roids.Count() > 0)
                {
                    MineableAsteriod firstones = roids[0].modProjectile as MineableAsteriod;
                    Vector2 diff = firstones.projectile.Center - posa;

                    if (diff.Length() < 1000 + (ragegrab ? 2000 : 0) + (installgrab ? 1000 : 0))
                    {
                        firstones.projectile.timeLeft = 600;
                        firstones.projectile.ignoreWater = true;
                        LockUnlock(false);
                        velocity += Vector2.Normalize(firstones.projectile.Center - Position).RotatedBy(MathHelper.Pi + (Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi) * 0.5f)) * 32f;
                        grabrock = firstones;

                        state = 100;
                    }
                }
            }

            if (state == 100)//Reach out and Grab Roid
            {
                if (grabrock == null)
                {
                    state = 101;
                    return;
                }
                Vector2 diff = grabrock.projectile.Center - posa;
                bossOffset += Vector2.Normalize(diff) * (ragegrab ? 48f : 24f);

                if (diff.Length() < 16f)
                {
                    state = installgrab ? 200 : 101;//if we're installing, go to state 200

                    if (installgrab)
                    {
                        SoundEffectInstance snd = Main.PlaySound(SoundID.DD2_SkeletonHurt, (int)Position.X, (int)Position.Y);
                        if (snd != null)
                        {
                            snd.Pitch = 0.50f;
                        }
                    }
                    else
                    {
                        SoundEffectInstance snd = Main.PlaySound(SoundID.DD2_CrystalCartImpact, (int)Position.X, (int)Position.Y);
                        if (snd != null)
                        {
                            snd.Pitch = -0.50f;
                        }
                    }
                }
            }

            if (state == 101)//Hold Roid for 30 frames before throwing it at the player
            {
                stateTimer += 1;
                if (grabrock == null)
                {
                    state = 102;
                    return;
                }

                bossOffset = grabrock.projectile.Center;

                if (stateTimer > 30)
                {
                    Vector2 toPlayer = Main.player[boss.npc.target].Center - posa;
                    velocity += Vector2.Normalize(toPlayer) * 32f;

                    grabrock.projectile.velocity = Vector2.Normalize(toPlayer) * 24f;
                    grabrock.projectile.damage = 50;
                    grabrock.projectile.hostile = true;
                    grabrock.projectile.friendly = false;
                    grabrock.projectile.ignoreWater = false;
                    state = 102;
                }
            }

            if (state == 200)//Spawn Enemy Roid
            {
                stateTimer += 1;
                if (grabrock == null)
                {
                    state = 102;
                    return;
                }

                bossOffset = grabrock.projectile.Center;

                if (stateTimer > 120)
                {
                    int npc = NPC.NewNPC((int)grabrock.projectile.Center.X, (int)grabrock.projectile.Center.Y, ModContent.NPCType<OverseenHeadAsteriod>());
                    grabrock.npc = npc;
                    grabrock.projectile.damage = 40;
                    grabrock.projectile.timeLeft = 99999;
                    grabrock.projectile.hostile = true;
                    grabrock.projectile.friendly = false;
                    grabrock.projectile.ignoreWater = false;
                    state = 102;

                    SoundEffectInstance snd = Main.PlaySound(SoundID.DD2_SkeletonSummoned, (int)Position.X, (int)Position.Y);
                    if (snd != null)
                    {
                        snd.Pitch = 0.50f;
                    }
                }
            }

            if (state == 102)//Throw Roid
            {

                Vector2 diff = boss.npc.Center - Position;
                bossOffset += Vector2.Normalize(diff) * (ragegrab ? 72f : 24f);

                //UnifiedRandom randomrock = new UnifiedRandom((int)(randomRotation2 * 32));
                if (diff.Length() < 8f + bossRockDist)
                {
                    //bossOffset = lastPlace;
                    LockUnlock(true);
                    state = 0;
                }

            }

            if (!followBoss)
                bossOffset += velocity;

            velocity *= 0.98f;

        }

        public void LockUnlock(bool lockin)
        {
            if (lockin)
            {

                SoundEffectInstance snd = Main.PlaySound(SoundID.DD2_OgreGroundPound, (int)Position.X, (int)Position.Y);
                if (snd != null)
                {
                    snd.Pitch = -0.50f;
                }
                snd = Main.PlaySound(SoundID.DD2_CrystalCartImpact, (int)Position.X, (int)Position.Y);
                if (snd != null)
                {
                    snd.Pitch = -0.50f;
                }

                this.bossOffset = -(boss.npc.Center - this.bossOffset);
                followBoss = true;

            }
            else
            {
                this.bossOffset = this.bossOffset + boss.npc.Center;

                SoundEffectInstance snd = Main.PlaySound(SoundID.DD2_CrystalCartImpact, (int)Position.X, (int)Position.Y);
                if (snd != null)
                {
                    snd.Pitch = 0.50f;
                }
                followBoss = false;
            }
            lastPlace = bossOffset;

        }
    }

    public class SpaceBossAI
    {
        public class ManagedSpaceBossAttack
        {
            public int timeLeft = 0;
            public int timeLeftMax = 0;
            public int timePassed = 0;
            public int damage = 30;
            SpaceBoss boss;
            public Vector3 position = Vector3.Zero;
            public Func<ManagedSpaceBossAttack, SpaceBoss, Vector3> Where = default;
            public Func<ManagedSpaceBossAttack, SpaceBoss, bool> Shoot = default;

            public ManagedSpaceBossAttack(SpaceBoss boss, int timeMax)
            {
                this.boss = boss;
                this.timeLeft = timeMax;
                this.timeLeftMax = timeMax;
            }

            public void Update()
            {
                position = Where(this, boss);
                if (Shoot(this, boss))
                {
                    Projectile.NewProjectile(new Vector2(position.X, position.Y), position.Z.ToRotationVector2() * 4f, ModContent.ProjectileType<SpaceBossTelegraphedBasicShot>(), damage, 1);
                }
                timePassed++;
                timeLeft -= 1;
            }

        }

        SpaceBoss boss;
        NPC npc;
        public List<int> deckOfAttacks = new List<int>();
        public List<ManagedSpaceBossAttack> managedAttacks = new List<ManagedSpaceBossAttack>();


        public SpaceBossAI(SpaceBoss boss)
        {
            this.boss = boss;
            npc = boss.npc;
        }

        enum SpaceBossAttackTypes
        {
            Nothing,
            XBeams,
            TossRoids,
            ShadowNebula,
            EolAttack,
            InstallRoids,
            Shield,
            SpecialMinion,
        }

        public void DrawDeck()
        {
            deckOfAttacks.Clear();

            //2 sets
            for (int i = 0; i < 2; i += 1)
            {
                deckOfAttacks.Add((int)SpaceBossAttackTypes.XBeams);

                deckOfAttacks.Add((int)SpaceBossAttackTypes.EolAttack);

                deckOfAttacks.Add((int)SpaceBossAttackTypes.TossRoids);

                if (boss.goingDark>0)//only during Shadow Nebula
                deckOfAttacks.Add((int)SpaceBossAttackTypes.SpecialMinion);

                if (boss.phase >= 1)
                {
                    for (int xx = 0; xx < 2; xx += 1)
                    {
                        deckOfAttacks.Add((int)SpaceBossAttackTypes.InstallRoids);
                    }

                }

                //Shuffle
                deckOfAttacks = deckOfAttacks.OrderBy(testby => Main.rand.Next()).ToList();
            }

            //Always put this one at the start
            if (boss.goingDark < -60 * 150 && boss.specialCooldown < 1 && boss.phase > 1 && Main.expertMode)
            {
                deckOfAttacks.Insert(0, (int)SpaceBossAttackTypes.ShadowNebula);
            }

            deckOfAttacks.Add((int)SpaceBossAttackTypes.Shield);
        }

        public bool ChooseAttack()
        {

            if (npc.life / (float)npc.lifeMax < boss.healthphase)//Heal if phase requires it
            {
                boss.phase += 1;//Advance phase
                boss.npc.ai[3] = 1;
                npc.ai[0] = boss.phase > 3 ? 25000 : 10000;//Shields up, but if in 4th phase, go into the final phase and teleport away
                DrawDeck();//Draw new deck of attacks
                return true;
            }

            if (npc.ai[0] > 1200 && npc.ai[0] < 10000)//Draw from the deck
            {

            tryagain:

                if (deckOfAttacks.Count<1)
                DrawDeck();//Draw new deck of attacks

                int attack = deckOfAttacks[0];//Top card
                deckOfAttacks.RemoveAt(0);//Discard drawn card

                if (attack == (int)SpaceBossAttackTypes.ShadowNebula || (!boss.removeDarknessCloud && boss.phase > 1 && Main.expertMode))//Choose: Shadow Nebula
                {
                    boss.specialCooldown = 80 * 60;
                    npc.ai[0] = 50000;//Shadow Nebula
                    return true;
                }
                if (boss.celerityBuff < -4500 && boss.phase > 1 && boss.goingDark < 0)//Cast Celestial Celerity with high priority over other moves if able
                {
                    npc.ai[0] = 12501;//Celestial Celerity
                    return true;
                }


                if (boss.celerityBuff<0 && ToEnemy.Length() > 3200 && boss.goingDark < 0)//Heal if too far away, regardless of what is drawn (Shadow Nebula will take priority thou), also ignored if under the buffs of Celestial Celerity
                {
                    npc.ai[0] = 10000;//Shields up
                    return true;
                }

                if (attack == (int)SpaceBossAttackTypes.XBeams)//Choose: X Beams
                {
                    SpaceBossRock[] nearbyrocks = boss.RocksAreas.Where(testby => (testby.Position - Main.player[npc.target].Center).LengthSquared() < (2000f * 2000f) && (testby.Position - Main.player[npc.target].Center).LengthSquared() > (640f * 640f)).ToArray();
                    if (nearbyrocks.Length > 2)//Need atleast 3 nearby to use this
                    {
                        npc.ai[0] = 10200;//X Beams
                        return true;
                    }
                    else
                    {
                        //Can't perform, try again
                        goto tryagain;
                    }
                }

                if (attack == (int)SpaceBossAttackTypes.EolAttack)//Choose: EoL attack
                {
                    SpaceBossEye[] eyesClosed = boss.Eyes.Where(testby => testby.sleep < 0).ToArray();
                    if (eyesClosed.Length < 3)//Need all eyes to be open
                    {
                        npc.ai[0] = 10900 + boss.phase * 30;//EoL attack
                        return true;
                    }
                    else
                    {
                        //Can't perform, try again
                        goto tryagain;
                    }
                }


                if (attack == (int)SpaceBossAttackTypes.SpecialMinion)//Choose: Special Minion
                {
                    //NPC[] closerMinions = Main.npc.Where(testby => testby.active && testby.type == ModContent.NPCType<SpaceBossShadowNebulaRepair>()).ToArray();
                    //NPC[] orbs = Main.npc.Where(testby => testby.active && testby.type == ModContent.NPCType<SpaceBossShadowNebulaEnemy>() && testby.life<testby.lifeMax).ToArray();

                    if (boss.goingDark>0)//Can have only 1
                    {
                        npc.ai[0] = 12000;//Special Minion
                        return true;
                    }
                    else
                    {
                        //Can't perform, try again
                        goto tryagain;
                    }
                }

                if (attack == (int)SpaceBossAttackTypes.InstallRoids)//Choose: Install Roids
                {
                    int num = 1800 * 1800;
                    Projectile[] roidsnearby = boss.AllAsteriods.Where(testby => (testby.Center - boss.npc.Center).LengthSquared() < (num)).ToArray();
                    if (roidsnearby.Length > 7 && NPC.CountNPCS(ModContent.NPCType<OverseenHeadAsteriod>()) < 4)//Need some asteriods nearby, and make sure there are not too many enemies already present
                    {
                        npc.ai[0] = 11700;
                        return true;
                    }
                    else
                    {
                        //Can't perform, try again
                        goto tryagain;
                    }
                }

                if (attack == (int)SpaceBossAttackTypes.TossRoids)//Choose: Toss Roids
                {
                    npc.ai[0] = 10500;//Toss Roids, can always perform
                    return true;
                }


                if (attack == (int)SpaceBossAttackTypes.Shield)//Choose: Shield
                {
                    //npc.ai[0] = 10000;//Shield, only at the end of the deck will we ever choose this
                    DrawDeck();//Draw new deck of attacks
                    goto tryagain;//On 2nd thought, nah, not yet atleast, maybe I'll do something special here later
                    return true;
                }

            }

            return false;

        }

        public void CreateManagedAttacks()
        {
            int pickedType = 0;

            float orbPercent = 1f;
            float maxHP = 1f;
            float norminalHP = 1f;
            foreach (NPC npc2 in Main.npc.Where(testby => testby.active && testby.type == ModContent.NPCType<SpaceBossShadowNebulaEnemy>()))
            {
                maxHP += npc2.lifeMax;
                norminalHP += npc2.life;
            }
            orbPercent = norminalHP / maxHP;

            //Attack varient 1: 2 waves of shots flying from offscreen
            if (pickedType == 0)
            {
                float maxAngle = orbPercent < 0.50f ? (MathHelper.TwoPi + 0.05f) : MathHelper.Pi;
                float anglePerAdd = MathHelper.PiOver2;
                float startingAngle = Main.rand.NextBool() ? (Main.rand.NextBool() ? MathHelper.Pi : 0) : (Main.rand.NextBool() ? MathHelper.PiOver2 : MathHelper.Pi + MathHelper.PiOver2) + (Main.rand.NextBool() ? MathHelper.PiOver4 : 0);
                float angleScale = orbPercent < 0.50f ? 1f : (Main.rand.NextBool() ? 1f : 2f);

                for (float f = 0; f < maxAngle; f += anglePerAdd)
                {
                    float flip = Main.rand.NextBool() ? 1f : -1f;

                    ManagedSpaceBossAttack attack = new ManagedSpaceBossAttack(boss, 300);

                    float angle = (f * angleScale) + startingAngle;

                    attack.Where = delegate (ManagedSpaceBossAttack managedAttack, SpaceBoss bossy)
                    {
                        Player player = Main.player[bossy.npc.target];
                        float percent = managedAttack.timeLeft / (float)managedAttack.timeLeftMax;

                        Vector2 offsetPoint = new Vector2((percent - 0.5f) * (2400f * flip), -2400f).RotatedBy(angle);

                        Vector3 pos = new Vector3(player.Center.X + offsetPoint.X, player.Center.Y + offsetPoint.Y, (angle) + MathHelper.PiOver2);


                        return pos;
                    };
                    attack.Shoot = delegate (ManagedSpaceBossAttack managedAttack, SpaceBoss bossy)
                    {
                        return managedAttack.timePassed % 30 == 0;
                    };
                    managedAttacks.Add(attack);
                }
            }

        }

        public void ManageActiveAttacks()
        {
            if (managedAttacks.Count > 0)
            {
                foreach (ManagedSpaceBossAttack attack in managedAttacks)
                {
                    attack.Update();
                }
                managedAttacks = managedAttacks.Where(testby => testby.timeLeft > 0).ToList();
            }
        }


        public void StateCelestialCelerity()
        {
            if (npc.ai[0]%20==0 && npc.ai[0] < 12750 && npc.ai[0] > 12550)
            {
                var snd = Main.PlaySound(SGAmod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/P5Loot").WithVolume(1f).WithPitchVariance(.10f), new Vector2(-1,-1));
                if (snd != null)
                {
                    snd.Pitch = MathHelper.Clamp((-0.5f)+ (npc.ai[0]-12500)/600f,-0.50f,0.90f);
                }
            }

            if (npc.ai[0] == 12510)
            {
                boss.celerityBuff = 1;
            }

            boss.celerityBuff += 5;

            if (boss.celerityBuff >300)
                boss.celerityBuff += 100;

            if (boss.celerityBuff > 3000)
            {
                boss.celerityBuff = 3000;
            }

            if (npc.ai[0] == 12800)
            {
                npc.ai[0] = 999;
            }
        }
        public void StateShadowNebula()
        {
            boss.state = 100;
            npc.life = (int)MathHelper.Clamp(npc.life + (int)(npc.lifeMax / 10000), 0, npc.lifeMax);
            boss.friction = 0.975f;

            if (npc.ai[0] > 50000)
            {
                npc.dontTakeDamage = true;
                if ((int)npc.ai[0] % 30 == 0 && npc.ai[0] < 50300)
                {
                    SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, (int)npc.Center.X, (int)npc.Center.Y);
                    if (sound != null)
                    {
                        sound.Pitch -= (npc.ai[0] - 50000f) / 600f;
                    }
                }

                for (int i = 48; i < 320; i += 16)
                {
                    Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                    int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + offset * Main.rand.NextFloat(32f, i + (npc.ai[0] - 50000) * 8f), 0, 0, 68);
                    Main.dust[dust].scale = 1.5f;
                    Main.dust[dust].velocity = Vector2.Normalize(offset) * (float)(8f * Main.rand.NextFloat(0.10f, 0.50f));
                    Main.dust[dust].noGravity = true;
                }
                if (npc.ai[0] == 50005)
                {
                    boss.goingDark = 60 * 10;
                }


                if (npc.ai[0] == 50260)
                {
                    SoundEffectInstance sound = Main.PlaySound(SoundID.Zombie, (int)npc.Center.X, (int)npc.Center.Y, 105);
                    if (sound != null)
                    {
                        sound.Pitch -= 0.85f;
                    }
                }

                if (npc.ai[0] == 50400)
                {
                    boss.removeDarknessCloud = true;
                    npc.ai[0] = 999;
                }

            }

            foreach (SpaceBossEye eyxxe in boss.Eyes)
            {
                if (Main.rand.Next(0, 75) == 0)
                    eyxxe.NewLook(npc.Center + new Vector2((Main.rand.Next(24, 160))).RotatedBy(MathHelper.TwoPi), 0.04f, true, true);
            }
        }

        public void StateInstallAsteriodOverheads()
        {
            boss.installRoids = 5;

            foreach (SpaceBossEye eye in boss.Eyes)
            {
                if (eye.customID != -1 && eye.customID != 1)
                {
                    Projectile[] closestroid = boss.AllAsteriods.OrderBy(testby => (testby.Center - ((SpaceBossRock)eye.boss).Position).Length()).ToArray();
                    if (closestroid.Length > 0)
                    {
                        Vector2 Offset = Vector2.Normalize(closestroid[0].Center - ((SpaceBossRock)eye.boss).Position);
                        eye.NewLook(((SpaceBossRock)eye.boss).Position + (Offset * 320f), 0.05f, true, true);
                    }
                }
            }

            if (npc.ai[0] > 11950)
            {
                foreach (SpaceBossRock rock in boss.Rocks)
                {
                    if (rock.state == 200)
                    {
                        npc.ai[0] = 11950;
                        break;
                    }

                }
            }

            if ((int)npc.ai[0] >= 12000-1)
            {
                npc.ai[0] = 1000;
            }
        }

        public void StateGrabAsteriodsAndSpin()
        {

            if (boss.TossRoids)
            {
                npc.rotation += boss.phase % 2 == 0 ? 0.15f : -0.15f;

                npc.velocity *= 0.98f;

                foreach (SpaceBossEye eyxxe in boss.Eyes)
                {
                    //if (eye.boss != this)
                    //{
                    if (eyxxe.customID != 1 && eyxxe.customID != -1)
                        eyxxe.NewLook(((SpaceBossRock)eyxxe.boss).Position + Vector2.Normalize(((SpaceBossRock)eyxxe.boss).Position - npc.Center) * 640f, 0.05f, true, true);
                    //}
                }
            }

            if ((int)npc.ai[0] == 10600)
            {
                if (boss.goingDark > 0)
                    CreateManagedAttacks();

                bool boom = true;

                for (int i = 0; i < (boom ? 64 : 2); i += 1)
                {
                    Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                    int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + offset * Main.rand.NextFloat(32f, 64f), 0, 0, DustID.BlueCrystalShard);
                    Main.dust[dust].scale = 2.5f;
                    Main.dust[dust].velocity = Vector2.Normalize(-offset) * (float)((boom ? 16f : -4f) * Main.rand.NextFloat(0.50f, 2.50f));
                    Main.dust[dust].noGravity = true;
                }

                SoundEffectInstance sound = Main.PlaySound(SoundID.Roar, (int)npc.Center.X, (int)npc.Center.Y, 0);
                if (sound != null)
                {
                    sound.Volume = 0.99f;
                    sound.Pitch -= 0.75f;
                }

            }
            if ((int)npc.ai[0] >= 10860)
            {
                npc.ai[0] = 1000;
            }
        }

        public void StateSummonSpecialMinion()
        {

            if ((int)npc.ai[0]%60 == 0 && npc.ai[0] < 12300)
            {
                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, -1, -1);
                if (sound != null)
                {
                    sound.Pitch = -0.5f+((npc.ai[0]-12000)/500f);
                    sound.Volume = 1.0f;
                }
            }

            if ((int)npc.ai[0] == 12300)
            {
                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_EtherianPortalDryadTouch, -1, -1);
                if (sound != null)
                {
                    sound.Pitch = 0.5f;
                    sound.Volume = 1.0f;
                }

                
                for (int i = 0; i < 180; i += 1)
                {
                    Vector2 offset = Main.rand.NextVector2Circular(1f, 1f);
                    Vector2 loca = new Vector2(npc.Center.X, npc.Center.Y) + offset * Main.rand.NextFloat(8f, 10f + i) / 5f;

                    SpaceBoss.SpaceBossStardust dusts = new SpaceBoss.SpaceBossStardust(loca, (Vector2.Normalize(offset) * (float)(2f * Main.rand.NextFloat(0.75f, 1.50f) * (i / 6f)))*0.250f, 40+(i/2));
                    dusts.scale = Vector2.One*1.5f;
                    dusts.fadeIn = 20;
                    dusts.fadeTime = 160;
                    boss.stardustEffectsOnTopOfRocks.Add(dusts);

                    int dust = Dust.NewDust(loca, 0, 0, ModContent.DustType<LeviDust>());
                    Main.dust[dust].scale = 2.5f;
                    Main.dust[dust].velocity = Vector2.Normalize(offset) * (float)(2f * Main.rand.NextFloat(0.75f, 1.50f)*(i/6f));
                    Main.dust[dust].noGravity = true;
                }
                

                NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<SpaceBossShadowNebulaRepair>());
            }

            foreach (SpaceBossEye eyxxe in boss.Eyes)
            {
                if (eyxxe.boss is SpaceBossRock rocky)
                {
                    if (eyxxe.customID != 1 && eyxxe.customID != -1)
                    {
                        Vector2 Offset = Vector2.Normalize(((SpaceBossRock)eyxxe.boss).Position - npc.Center);
                        eyxxe.NewLook(((SpaceBossRock)eyxxe.boss).Position - (Offset * 640f), 0.05f, true, true);


                        if (npc.ai[0] < 12300)
                        {

                            SpaceBoss.SpaceBossStardust dusts = new SpaceBoss.SpaceBossStardust(((SpaceBossRock)eyxxe.boss).Position, (-Offset * ((npc.ai[0] - 12000) / 300f) * 16f) + Main.rand.NextVector2Circular(4f, 4f), Main.rand.Next(10, 20));
                            dusts.fadeIn = 4;
                            dusts.fadeTime = 6;
                            dusts.alpha = 0.5f;
                            dusts.scale = Vector2.One * (npc.ai[0] - 12000) / 200f;
                            boss.stardustEffectsOnTopOfRocks.Add(dusts);

                            /*
                            int dust = Dust.NewDust(((SpaceBossRock)eyxxe.boss).Position, 0, 0, ModContent.DustType<LeviDust>());
                            Main.dust[dust].velocity = -Offset * ((npc.ai[0] - 12000) / 300f)*8f;
                            Main.dust[dust].scale = (npc.ai[0] - 12000) / 200f;
                            Main.dust[dust].alpha = 200;
                            Main.dust[dust].noGravity = true;
                            */
                        }


                    }

                }
            }


            if ((int)npc.ai[0] >= 12500 || boss.goingDark < 1)
            {
                npc.ai[0] = 1000;
            }
        }

        public void StateEolAttack()
        {

            if ((int)npc.ai[0] == 11000)
            {
                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_SonicBoomBladeSlash, -1, -1);
                if (sound != null)
                {
                    sound.Pitch = 0.5f;
                    sound.Volume = 1.0f;
                }
            }

            foreach (SpaceBossEye eyxxe in boss.Eyes)
            {
                if (eyxxe.boss is SpaceBossRock rocky)
                {

                    if (eyxxe.customID != 1 && eyxxe.customID != -1)
                    {
                        Vector2 Offset = Vector2.Normalize(((SpaceBossRock)eyxxe.boss).Position - npc.Center);
                        eyxxe.NewLook(((SpaceBossRock)eyxxe.boss).Position + (Offset * 640f), 0.05f, true, true);

                        if ((int)npc.ai[0] == 11000)
                        {
                            Projectile proj = Projectile.NewProjectileDirect(eyxxe.BasePosition, Offset * Main.rand.NextFloat(0.5f, 2f), ModContent.ProjectileType<SpaceBossHomingShot>(), 40, 12f);

                            if (proj != null)
                            {
                                proj.localAI[0] = -Main.rand.NextFloat(30, 90);
                                proj.timeLeft = Main.rand.Next(400, 600) + (int)proj.localAI[0];
                                proj.netUpdate = true;

                                for (int i = 0; i < 6; i += 1)
                                {
                                    Vector2 offset = proj.velocity.RotatedByRandom(MathHelper.Pi / 6f);
                                    int dust = Dust.NewDust(new Vector2(proj.Center.X, proj.Center.Y) + offset * Main.rand.NextFloat(8f, 64f), 0, 0, DustID.BlueCrystalShard);
                                    Main.dust[dust].scale = 2.5f;
                                    Main.dust[dust].velocity = Vector2.Normalize(offset) * (float)(2f * Main.rand.NextFloat(1f, 2.50f));
                                    Main.dust[dust].noGravity = true;
                                }

                            }
                            eyxxe.sleep = -Main.rand.NextFloat(3f, 5f);
                            eyxxe.openUp = false;

                        }
                        if (npc.ai[0] < 11000)
                        {
                            int dust = Dust.NewDust(((SpaceBossRock)eyxxe.boss).Position, 0, 0, DustID.BlueCrystalShard);
                            Main.dust[dust].scale = 1f;
                            Main.dust[dust].alpha = 200;
                            Main.dust[dust].noGravity = true;
                        }


                    }

                }
            }


            if ((int)npc.ai[0] >= 11200)
            {
                npc.ai[0] = 1000;
            }
        }

        public void StateCreateBeamX()//XBeams
        {

            foreach (SpaceBossEye eyxxe in boss.Eyes)
            {
                //if (eye.boss != this)
                //{
                if (eyxxe.customID != 1 && eyxxe.customID != -1)
                    eyxxe.NewLook(((SpaceBossRock)eyxxe.boss).Position + Vector2.Normalize(((SpaceBossRock)eyxxe.boss).Position - npc.Center) * 640f, 0.05f, true, true);
                //}
            }

            if ((int)npc.ai[0] == 10201)
            {

                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, -1, -1);
                if (sound != null)
                {
                    sound.Pitch -= 0.5f;
                }

                SpaceBossRock[] nearbyrocks = boss.RocksAreas.Where(testby => (testby.Position - Main.player[npc.target].Center).LengthSquared() < (2000f * 2000f) && (testby.Position - Main.player[npc.target].Center).LengthSquared() > (640f * 640f)).ToArray();

                foreach (SpaceBossRock rock in nearbyrocks)
                {
                    for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.PiOver2)
                    {
                        Vector2 vec = Vector2.UnitX;
                        int prog = Projectile.NewProjectile(rock.Position, vec.RotatedBy(f + rock.Rotation) * 32f, SGAmod.Instance.ProjectileType("SpaceBossBeam"), 50, 15f);
                    }
                }

            }
            if ((int)npc.ai[0] >= 10400)
            {
                npc.ai[0] = 1000;
            }
        }

        public void StateStartShield()
        {
            boss.state = 4;
            if ((int)npc.ai[0] == 10060)
            {
                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_WyvernScream, -1, -1);
                if (sound != null)
                {
                    sound.Volume = 0.99f;
                    sound.Pitch -= 0.5f;
                }
                sound = Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, -1, -1);
                if (sound != null)
                {
                    sound.Volume = 0.99f;
                    sound.Pitch += 0.5f;
                }
            }

            bool boom = (int)npc.ai[0] == 10120;

            if (boom)
            {
                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_WyvernDiveDown, npc.Center);
                if (sound != null)
                {
                    sound.Volume = 0.99f;
                    sound.Pitch -= 0.5f;
                }
            }

            for (int i = 0; i < (boom ? 64 : 2); i += 1)
            {
                Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + offset * Main.rand.NextFloat(32f, 64f), 0, 0, DustID.BlueCrystalShard);
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].velocity = Vector2.Normalize(-offset) * (float)((boom ? 16f : -4f) * Main.rand.NextFloat(0.50f, 2.50f));
                Main.dust[dust].noGravity = true;
            }

            boss.friction = 0.975f;

            if ((int)npc.ai[0] == 10120)
            {
                bool finalPhase = boss.npc.ai[3] == 2;
                if (boss.npc.ai[3] == 1)
                    boss.healthphase -= 0.25f;
                boss.LaunchTethers(finalPhase ? 5 : 3, finalPhase ? 3200 : 2000);
            }

            if (npc.ai[0] > 10120)
            {
                npc.ai[0] = 1000;
            }

        }

        public void StateGoToFurtherestSpot()
        {
            boss.state = 15;

            /*if (!Main.dedServ)
			{
				if (!Filters.Scene["SGAmod:ShockwaveBanshee"].IsActive())
				{
					//Main.NewText("work!");
					Filters.Scene.Activate("SGAmod:ShockwaveBanshee", npc.Center, new object[0]).GetShader().UseColor(5f, 2f, 10f).UseTargetPosition(npc.Center);
				}


				float progress = (npc.ai[0] - 25000) / 150f;

				float alpha = progress;

				Filters.Scene["SGAmod:ShockwaveBanshee"].GetShader().UseProgress(progress).UseOpacity(alpha).UseColor(3f, 5f, 500f).UseTargetPosition(npc.Center);
			}*/

            boss.friction = 0.900f;
            if (npc.ai[0] == 25140)
            {
                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_PhantomPhoenixShot, -1, -1);
                if (sound != null)
                {
                    sound.Volume = 0.99f;
                    sound.Pitch -= 0.5f;
                }
                sound = Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, -1, -1);
                if (sound != null)
                {
                    sound.Volume = 0.99f;
                    sound.Pitch += 0.5f;
                }
            }

            bool boom = (int)npc.ai[0] == 25150;

            if (boom)
            {
                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_DarkMageHealImpact, npc.Center);
                if (sound != null)
                {
                    sound.Volume = 0.99f;
                    sound.Pitch -= 0.5f;
                }
            }

            for (int i = 0; i < (boom ? 64 : 2); i += 1)
            {
                Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + offset * Main.rand.NextFloat(32f, 64f), 0, 0, DustID.BlueCrystalShard);
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].velocity = Vector2.Normalize(-offset) * (float)((boom ? 16f : -4f) * Main.rand.NextFloat(0.50f, 2.50f));
                Main.dust[dust].noGravity = true;
            }

            if (boom)
            {
                RippleBoom.MakeShockwave(npc.Center, 6f, 1f, 18f, 80, 1f, true);

                boss.npc.ai[3] = 2;//Starts the final phase

                boss.healthphase = -1;

                npc.ai[0] = 10000;//Shields up

                if (!Main.dedServ)
                {
                    if (!Filters.Scene["SGAmod:ShockwaveBanshee"].IsActive())
                    {
                        //Main.NewText("work!");
                        Filters.Scene.Deactivate("SGAmod:ShockwaveBanshee");
                    }
                }

                Vector2 gowhere = boss.RocksAreas.Where(testby => (testby.Position - npc.Center).Length() < 10000).OrderBy(testby => (Main.player[npc.target].Center - testby.Position).LengthSquared()).Reverse().ToArray()[0].Position;
                npc.Center = gowhere;

            }
        }



        public void StateShielded()
        {
            if (boss.npc.ai[0] % 100 == 0)
            {
                //RippleBoom.MakeShockwave(npc.Center, 8f, 1f, 24f, 80, 1f, true);
            }

            if (boss.npc.ai[3] == 1)
            {
                if (boss.phase < 4 && Main.expertMode)
                {
                    for (int i = 0; i < Math.Min(boss.phase, 3) * 2; i += 1)
                    {
                        int phaseCounter = -i * (50 + (boss.phase * 20));
                        NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<OverseenHeadBossShield>(), ai0: boss.phase > 3 ? 1 : 0, ai2: phaseCounter, ai3: 200 + i * 40);
                    }
                }
                boss.npc.ai[3] = 0;
            }
            npc.life = (int)MathHelper.Clamp(npc.life + (int)(npc.lifeMax / 20000), 0, npc.lifeMax);
            boss.friction = 0.975f;
            UnifiedRandom rando = new UnifiedRandom(npc.type);
            foreach (SpaceBossEye eyxxe in boss.Eyes)
            {
                if (Main.rand.Next(0, 75) == 0)
                    eyxxe.NewLook(npc.Center + new Vector2(rando.Next(24, 160)).RotatedBy(rando.NextFloat(-1f, 1f) * npc.localAI[0]), 0.04f, true, true);
            }
        }

        public void SecureArea()
        {
            Vector2[] thisthing;

            //choose where to go

            int getCloser = 0;

            if (boss.goingDark > 0)
            {
                getCloser = 1;
                thisthing = boss.visitedEmptySpaces.Where(testby => (testby - npc.Center).LengthSquared() < 4200 * 4200).OrderBy(testby => (testby - Main.player[npc.target].Center).LengthSquared()).ToArray();//Tries to move near the player
            }
            else
            {
                List<NPC> focusCrystals = Main.npc.Where(testby => testby.active && testby.type == ModContent.NPCType<StationFocusCrystal>()).ToList();
                if (boss.removeDarknessCloud && focusCrystals.Count > 0)
                {
                    getCloser = 2;
                    focusCrystals = focusCrystals.OrderBy(testby => (testby.Center - boss.npc.Center).LengthSquared()).ToList();
                    NPC crystalNearest = focusCrystals[0];
                    thisthing = boss.visitedEmptySpaces.Where(testby => (testby - npc.Center).LengthSquared() < 3200 * 3200).OrderBy(testby => (testby - crystalNearest.Center).LengthSquared()).ToArray();//Tries to move near any more existing focus crystals, to heal up
                }
                else
                {
                    thisthing = boss.visitedEmptySpaces.OrderBy(testby => (testby - npc.Center).LengthSquared()).ToArray();//just choses the nearest place to move to
                }
            }


            if (thisthing.Length > 0)
            {
                int index2 = 0;
                    index2 = Main.rand.Next(0, boss.goingDark > 0 ? Math.Min(3, boss.visitedEmptySpaces.Count / 300) : boss.visitedEmptySpaces.Count / 20);
                if (getCloser==2)
                    index2 = 0;

                boss.visitedEmptySpaces.Remove(thisthing[index2]);
                boss.goToEmptySpace = thisthing[index2];
            }

            SpaceBossRock rock = new SpaceBossRock(npc.Center, boss);
            rock.state = 1500;
            rock.airReady = -Main.rand.Next(300, 1500);
            boss.RocksAreas.Add(rock);
            boss.Rocks.Add(rock);

            SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 33);
            if (sound != null)
            {
                sound.Volume = 0.89f;
                sound.Pitch -= 0.5f;
            }
            boss.timeHere = 0;

            //Main.NewText("RocksAreas left " + boss.visitedEmptySpaces.Count);

            //Spawn Guys

            NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<OverseenHead>());

            if (boss.phase > 1 && boss.goingDark<1)
            {
                SpaceBossRock[] closerocksbutnottooclose = boss.RocksAreas.Where(testby => (testby.Position - Main.player[npc.target].Center).LengthSquared() < (3200f * 3200f) && (testby.Position - Main.player[npc.target].Center).LengthSquared() > (640f * 640f)).OrderBy(testby => (testby.Position - Main.player[npc.target].Center).LengthSquared()).ToArray();

                int index = Main.rand.Next(0, closerocksbutnottooclose.Length / 3);

                if (index >= 0 && closerocksbutnottooclose.Length > index)
                    NPC.NewNPC((int)closerocksbutnottooclose[index].Position.X, (int)closerocksbutnottooclose[index].Position.Y, ModContent.NPCType<OverseenHead>());
            }

            ChooseAttack();

        }

        public Vector2 ToEnemy => Main.player[npc.target].Center - npc.Center;

        public void StateMove()
        {

            //Forces Shadow Nebula on the start of phase 2 so it can't be skipped
            if (boss.goingDark < -60 * 150 && boss.specialCooldown < 1 && boss.phase > 1 && Main.expertMode && npc.aiAction < 100)
            {
                npc.aiAction = 100;
                boss.goToEmptySpace = boss.npc.Center;
                DrawDeck();
            }

            Vector2 dista = boss.goToEmptySpace - npc.Center;

            float movementSpeed = (boss.celerityBuff>0 ? MathHelper.Clamp(dista.Length()/2000f, 0.25f,1f) : 0.25f) + (Main.expertMode ? (boss.phase / 20f) : 0);
            boss.state = 2;
            npc.rotation += npc.velocity.X * 0.002f;

            boss.friction = 0.95f;

            if ((dista).Length() < 160)
            {
                if (npc.velocity.Length() < 4f)
                    boss.timeHere += 1;
                if (boss.timeHere > 80)// + (npc.life / (float)npc.lifeMax) * 60)
                {
                    SecureArea();
                }
            }
            else
            {
                if ((dista).Length() >= 0)
                    npc.velocity += (Vector2.Normalize(dista) * movementSpeed).RotatedBy(Math.Sin(npc.localAI[0] / 60f) * 1f);
            }

            foreach (SpaceBossEye eyxxe in boss.Eyes)
            {
                //if (eye.boss != this)
                //{
                if (eyxxe.customID != 1 && eyxxe.customID != -1 && npc.velocity.Length() > 3f && Vector2.Dot(Vector2.Normalize(eyxxe.BasePosition) - Vector2.Normalize(npc.Center), Vector2.Normalize(npc.velocity)) > 0f)
                    eyxxe.NewLook(npc.Center + npc.velocity * 16f, 0.05f, true, true);
                else
                    eyxxe.NewLook(Main.player[npc.target].MountedCenter, 0.05f, true, true);
                //}
            }

        }

        public void StateFinal()
        {

            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead)
                {
                    if (boss.countdownToTheEnd < 60 * 90)
                    {
                        player.AddBuff(boss.countdownToTheEnd < 60 * 60 ? BuffID.OnFire : ModContent.BuffType<Sunburn>(), 2);
                    }
                    if (boss.countdownToTheEnd < 60 * 40)
                    {
                        player.AddBuff(ModContent.BuffType<ThermalBlaze>(), 2);
                    }

                    if (boss.countdownToTheEnd < 60 * 30)
                    {
                        player.AddBuff(boss.countdownToTheEnd < 60 * 10 ? ModContent.BuffType<LavaBurn>() : ModContent.BuffType<LavaBurnLight>(), 2);
                    }

                    if (boss.countdownToTheEnd < 60 * 20)
                    {
                        player.AddBuff(BuffID.Burning, 2);
                    }

                    if (boss.countdownToTheEnd < -150)
                    {
                        player.KillMe(PlayerDeathReason.ByCustomReason("The sun crashed into " + player.name), 9001, 0);

                    }
                }
            }

            Player target = Main.player[npc.target];
            bool withinDist = ToEnemy.LengthSquared() < 1600 * 1600;

            if (npc.ai[0] % (Main.expertMode ? 30 : 60) == 0 && !withinDist)
            {
                Vector2 place = target.Center + (Vector2.Normalize(ToEnemy.RotatedBy(MathHelper.PiOver2)) * Main.rand.NextFloat(-400f, 400f)) + Vector2.Normalize(ToEnemy.RotatedBy(MathHelper.Pi)) * 1500f;

                int proj = Projectile.NewProjectile(place, Vector2.Normalize(ToEnemy) * Main.rand.NextFloat(2.8f, 3.2f), ModContent.ProjectileType<SpaceBossTelegraphedBasicShot>(), 30, 10);
                Main.projectile[proj].timeLeft = 720;
            }

            //boss.shieldeffect<1

            if (boss.TetherAsteriods.Count() < 5)
            {

                if (withinDist)
                {
                    if (npc.ai[0] % (Main.expertMode ? 30 : 80) == 0)
                    {
                        for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / (5f - (float)boss.TetherAsteriods.Count()))
                        {
                            Vector2 velo = Vector2.UnitX.RotatedBy((npc.ai[0] / 80f) + f);
                            Projectile.NewProjectile(npc.Center + velo * 24f, velo * 2, ModContent.ProjectileType<SpaceBossBasicShotAccelerate>(), 30, 10);
                        }
                    }
                }

                int timer = (Main.expertMode ? 160 : 320) + boss.TetherAsteriods.Count() * 60;

                if (npc.ai[0] % (120 + timer) == 0 && withinDist)
                {
                    foreach (Projectile rock in boss.TetherAsteriods)
                    {
                        int prog = Projectile.NewProjectile(npc.Center, Vector2.Normalize(rock.Center - npc.Center) * 32f, SGAmod.Instance.ProjectileType("SpaceBossBeam"), 50, 15f);
                    }
                }

                if ((npc.ai[0] + (timer / 2)) % (timer) == 0)
                {
                    int prog = Projectile.NewProjectile(npc.Center, Vector2.Normalize((target.MountedCenter + (target.velocity * 2f)) - npc.Center) * 32f, SGAmod.Instance.ProjectileType("SpaceBossBeam"), 50, 15f);
                }
            }


            if (npc.ai[0] % 250 == 0)
            {
                SpaceBossRock[] nearbyrocks = boss.RocksAreas.Where(testby => (testby.Position - Main.player[npc.target].Center).LengthSquared() < (3000f * 3000f) && (testby.Position - Main.player[npc.target].Center).LengthSquared() > (640f * 640f)).ToArray();

                if (nearbyrocks.Length > 0)
                {

                    SpaceBossRock randomrock = nearbyrocks[Main.rand.Next(nearbyrocks.Length)];

                    for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.PiOver2)
                    {
                        Vector2 vec = Vector2.UnitX;
                        int prog = Projectile.NewProjectile(randomrock.Position, vec.RotatedBy(f + randomrock.Rotation) * 32f, SGAmod.Instance.ProjectileType("SpaceBossBeam"), 50, 15f);
                    }
                }
            }

        }

        public void StateActive()
        {
            boss.state = 1;
            npc.ai[0] += 1;
            boss.celerityBuff -= 1;
            if (boss.goingDark > 0)
            {
                npc.chaseable = false;
            }

            npc.SGANPCs().overallResist = boss.DamageSoftCap <= 0.10f ? 0f : 1f;
            npc.SGANPCs().dotImmune = boss.DamageSoftCap <= 0.75f;

            //Main.NewText(boss.DamageSoftCap);

            if (npc.ai[0] == 310)
            {
                //Turn on!
                if (boss.visitedEmptySpaces.Count < 1 && boss.RocksAreas.Count < 1)
                {
                    npc.boss = true;
                    npc.ai[0] = 1000;
                    boss.goToEmptySpace = npc.Center;
                    boss.visitedEmptySpaces.AddRange(SpaceDim.EmptySpaces);
                }

                DrawDeck();

                SoundEffectInstance sound = Main.PlaySound(SoundID.NPCHit, (int)npc.Center.X, (int)npc.Center.Y, 57);
                if (sound != null)
                {
                    sound.Volume = 0.99f;
                    sound.Pitch -= 0.75f;
                }
            }

            foreach (SpaceBossEye eye in boss.Eyes)
            {
                eye.sleep += 0.015f;
            }

            if (npc.ai[0] > 1000)
            {

                ManageActiveAttacks();

                foreach (SpaceBossRock rockyplace in boss.RocksAreas)
                {
                    rockyplace.timer += 1;
                    rockyplace.airReady++;

                    if (rockyplace.airReady > 0)
                    {
                        foreach (Player player in Main.player)
                        {
                            if (player.DistanceSQ(rockyplace.Position) < 32 * 32)
                            {
                                SGAPlayer sgaplayer = player.SGAPly();

                                sgaplayer.RestoreBreath((int)MathHelper.Clamp((int)((rockyplace.airReady / 600f) * 30f), 0, 30));

                                rockyplace.airReady = -Main.rand.Next(1000, 2000);
                            }
                        }
                    }
                }

                npc.dontTakeDamage = boss.goingDark > 0;

                if (npc.ai[3] > 1)
                {
                    StateFinal();
                }
                else
                {
                    float vel = npc.velocity.Length();
                    if (boss.celerityBuff > 0 && vel > 1)
                    {
                        for (int duster = 0; duster < 1 + vel/3; duster += 1)
                        {
                            int dust = Dust.NewDust(npc.Center + Main.rand.NextVector2Circular(32f,32f), 0, 0, ModContent.DustType<LeviDust>());
                            Main.dust[dust].velocity = Main.rand.NextVector2Circular(2f, 2f) + (npc.velocity * Main.rand.NextFloat(0.25f, 0.75f));
                            Main.dust[dust].scale = MathHelper.Clamp(vel/24f,0f,1f);
                            Main.dust[dust].alpha = 200;
                            Main.dust[dust].noGravity = true;
                        }
                    }

                }

                foreach (SpaceBossRock rockyplace in boss.Rocks)
                {
                    rockyplace.BossRocksUpdate();
                }

                if (npc.ai[0] >= 50000 && npc.ai[0] < 51000)//Shadow Nebula
                {
                    StateShadowNebula();
                    return;
                }

                if (npc.ai[0] >= 25000 && npc.ai[0] < 25151)//Teleport Away and Shield Up
                {
                    StateGoToFurtherestSpot();
                    return;
                }

                if (npc.ai[0] >= 10000 && npc.ai[0] < 10200)//Shield Startup
                {
                    StateStartShield();
                    return;
                }

                if (npc.ai[0] >= 10900 && npc.ai[0] <= 11200)//Eol Attack
                {
                    StateEolAttack();
                    return;
                }

                if (npc.ai[0] >= 12000 && npc.ai[0] <= 12500)//Special Minion
                {
                    StateSummonSpecialMinion();
                    return;
                }

                if (npc.ai[0] >= 10200 && npc.ai[0] < 10400)//Beams
                {
                    StateCreateBeamX();
                    if (boss.phase < 3)
                        return;
                }

                if (npc.ai[0] > 11200 && npc.ai[0] < 12000)//Install Roids
                {
                    StateInstallAsteriodOverheads();
                    if (boss.phase < 2)
                        return;
                }

                if (npc.ai[0] > 12500 && npc.ai[0] <= 12800)//Celestial Celerity
                {
                    StateCelestialCelerity();
                        return;
                }


                if (npc.ai[0] >= 10500 && npc.ai[0] < 10900)//Roids grabbing
                {
                    StateGrabAsteriodsAndSpin();
                    //return;
                }

                if ((boss.TetherAsteriods.ToArray()).Length > 0)
                {
                    npc.dontTakeDamage = true;
                    boss.shieldeffect = MathHelper.Clamp(boss.shieldeffect + 1f, 0f, 15f);
                    boss.darknessAura = MathHelper.Clamp(boss.darknessAura - 0.025f, 0.5f, 1f);

                    /*
					if (boss.goingDark < -60 * 150 && boss.specialCooldown < 1 && boss.phase > 1 && Main.expertMode && npc.aiAction<100)
					{
						npc.aiAction = 100;
						boss.goToEmptySpace = boss.npc.Center;
						DrawDeck();
					}
					*/

                    StateShielded();
                }
                else
                {
                    boss.shieldeffect = MathHelper.Clamp(boss.shieldeffect - 1f, 0f, 30f);

                    if (boss.npc.ai[3] < 2)
                    {

                        if (boss.shieldeffect <= 0f)
                            StateMove();

                    }

                }

            }

        }

    }


    public class SpaceBoss : ModNPC, ISGABoss, IDrawThroughFog
    {
        public List<SpaceBossStardust> stardustEffects = new List<SpaceBossStardust>();
        public List<SpaceBossStardust> stardustEffectsOnTopOfRocks = new List<SpaceBossStardust>();

        public class SpaceBossStardust : HavocGear.Items.Weapons.StardusterCharging.StardusterProjectile
        {
            public float rotation = 0;
            public float rotationAdd = 0;
            public float fadeTime = 30f;
            public float fadeIn = 30f;
            public float alpha = 1f;
            public override Vector2 Position
            {
                get
                {
                    return position;
                }
            }
            public SpaceBossStardust(Vector2 position, Vector2 velocity, int timeLeft = 30) : base(position, velocity)
            {
                scale = Vector2.One;
                this.position = position;
                this.velocity = velocity;
                this.timeLeft = timeLeft;
                timeLeftMax = timeLeft;
                rando = Main.rand.Next();
                rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                rotationAdd = Main.rand.NextFloat(-1f, 1f) * 0.05f;
            }
            public override void Update()
            {
                rotation += rotationAdd;
                base.Update();
            }
        }
        public string Trophy() => "PhaethonTrophy";
        public bool Chance() => Main.rand.Next(0, 10) == 0;
        public string RelicName() => "Phaethon";
        public void NoHitDrops() { }
		public string MasterPet() => null;
		public bool PetChance() => false;

        public bool TossRoids => (int)npc.ai[0] > 10600 && (int)npc.ai[0] < 10800;

        public float darknessAura = 0.25f;
        public float timeHere = 0f;
        public float shieldeffect = 30f;
        public int phase = 0;
        public int state = 0;
        public float healthphase = 0.90f;
        public int countdownToTheEnd = 60 * 100;
        public int installRoids = 0;
        public float friction = 1f;
        public int celerityBuff = -100000;
        public int specialCooldown = 0;
        public bool removeDarknessCloud = false;
        public Vector2[] screenPos;
        public Vector2 startPosition;
        public static SpaceBoss instance;
        public bool NoHitMode => Items.TheWholeExperience.CheckHigherTier();

        public static Film film = new Film();

        public IEnumerable<Projectile> TetherAsteriods => Main.projectile.Where(testby => testby.active && testby.type == ModContent.ProjectileType<MineableAsteriodBossLock>());
        public IEnumerable<Projectile> AllAsteriods => Main.projectile.Where(testby => testby.active && testby?.modProjectile is IMineableAsteriod && testby.type != ModContent.ProjectileType<MineableAsteriodBossLock>());

        public List<SpaceBossEye> Eyes = new List<SpaceBossEye>();
        public List<SpaceBossRock> Rocks = new List<SpaceBossRock>();
        public List<SpaceBossRock> RocksAreas = new List<SpaceBossRock>();

        public Vector2 goToEmptySpace = default;
        public List<Vector2> visitedEmptySpaces = new List<Vector2>();
        public SpaceBossAI ai;
        public SoundEffectInstance Deathsnd;

        public int goingDark = -60 * 300;
        public float fadeOutAlpha = 1f;

        public float DamageSoftCap => 1f - MathHelper.Clamp(((npc.lifeMax * healthphase) - npc.life) / 1000f, 0f, 1f);

        public static bool CutSceneActive(ref bool cam) => cam || SpaceBoss.film.IsActive;

        public override bool Autoload(ref string name)
        {
            name = "Phaethon";
            SGAmod.PanCameraEvent += MoveCamera;
            SGAWorld.CutsceneActiveEvent += CutSceneActive;
            if (!SGAmod.SpaceBossActive)
                return false;

            return mod.Properties.Autoload;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
            // DisplayName.SetDefault("Example Person");
            Main.npcFrameCount[npc.type] = 1;
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;
        }

        public override void SetDefaults()
        {
            //phase = 2;

            npc.width = 96;
            npc.height = 96;
            npc.aiStyle = -1;
            npc.damage = 0;
            npc.noGravity = true;
            npc.defense = 30;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.rarity = ItemRarityID.Blue;
            npc.lifeMax = 100000;
            npc.friendly = false;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            npc.knockBackResist = 0f;
            bossBag = ModContent.ItemType<Items.PhaethonBag>();

            for (int i = BuffID.Count; i < Main.buffNoSave.Length; i += 1)
            {
                npc.buffImmune[i] = true;
            }

            instance = this;
        }

        public override bool CheckDead()
        {
            if (npc.ai[0] < 100000)
            {
                npc.ai[0] = 100000;
                npc.life = 10000;
                return false;
            }
            if (npc.ai[0] < 200000)
            {
                npc.life = 1000;
            }

            return npc.ai[0] > 200000;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.GreaterHealingPotion;

        }

        public override void NPCLoot()
        {

            RippleBoom.MakeShockwave(npc.Center, 8f, 1f, 24f, 80, 1f, true);

            SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_KoboldExplosion, -1, -1);

            if (sound != null)
            {
                sound.Pitch = -0.5f;
            }

            sound = Main.PlaySound(SoundID.NPCKilled, -1, -1, 10);
            if (sound != null)
            {
                sound.Pitch = -0.5f;
            }

            for (int i = 0; i < 10; i += 1)
            {
                int projtype = ModContent.ProjectileType<MineableAsteriodOverseenCrystal>();
                Vector2 velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                Projectile projectile = Projectile.NewProjectileDirect(npc.Center + (velocity * Main.rand.NextFloat(-8f, 20f)), velocity * Main.rand.NextFloat(8f, 16f), projtype, 0, 0);
                projectile.damage = 5;
            }

            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.StarMetalMold>());
            }

            if (Main.rand.Next(7) == 0)
            {
              Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Armors.Vanity.PhaethonMask>());
            }

            SGAWorld.downedSpaceBoss = true;
            SubworldCache.AddCache("SGAmod", "SGAWorld", "downedSpaceBoss", true, null);

            SpaceDim.crystalAsteriods = true;
        }

        public override bool CheckActive()
        {
            return npc.life < 1;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (goingDark > 0)
                return false;
            return null;
        }

        public void DamageAlter(ref int damage, object thing)
        {

            if (npc.life < npc.lifeMax * healthphase)
            {
                damage = (int)(damage * DamageSoftCap);
            }

            if (thing is Projectile proj)
            {
                if (ProjectileID.Sets.Homing[proj.type] == true)
                    damage = (int)(damage * 0.50);
            }

        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            DamageAlter(ref damage, player);
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            DamageAlter(ref damage, projectile);
        }

        public void Initiate()
        {
            if (Sleeping)
            {
#if BossDebug
				Main.NewText("I exist at " + npc.Center);
				Main.NewText("you are at " + Main.LocalPlayer.Center);
#endif
            }


            if (timePassed == 0)
            {
                startPosition = npc.Center;
                screenPos = new Vector2[10];
                for (int i = 0; i < screenPos.Length; i += 1)
                {
                    screenPos[i] = npc.Center - Main.screenPosition;
                }

                ai = new SpaceBossAI(this);

                int maxIndex = 32;

                foreach (SpaceStationStructure station in SpaceStationStructure.StationObjects)
                {
                    station.SpawnFocusCrystals();
                }

                for (int i = 0; i < maxIndex; i += 1)
                {

                    Vector2 offsetGaussian = SpaceDim.Gaussian2D(Main.rand.NextFloat(), Main.rand.NextFloat()) * (320f);
                    offsetGaussian += Vector2.Normalize(offsetGaussian) * 128f;

                    SpaceBossRock rock = new SpaceBossRock(npc.Center + offsetGaussian, this);

                    rock.index = i;
                    rock.indexMax = maxIndex;

                    Rocks.Add(rock);
                    SpaceBossEye eye3 = new SpaceBossEye(rock.bossOffset + Vector2.UnitY, rock, new Vector2(0, 0));
                    eye3.eyeScale = Main.rand.NextFloat(0.25f, 0.75f);
                    eye3.sleep = Main.rand.NextFloat(-2f, -0.5f);
                    eye3.beamAlpha = eye3.sleep;
                    rock.eye = eye3;
                    Eyes.Add(eye3);
                }

                SpaceBossEye eye = new SpaceBossEye(npc.Center + Vector2.UnitY, this, new Vector2(0, 0));
                eye.customID = 1;
                //SpaceBossEye eye2 = new SpaceBossEye(npc.Center + Vector2.UnitY, this, new Vector2(0, 0));
                //eye2.customID = -1;

                Eyes.Add(eye);
                //Eyes.Add(eye2);
                LaunchTethers(3, 5000);
            }
        }

        public override string Texture
        {
            get
            {
                return "SGAmod/Projectiles/FieryRock";
            }
        }

        BlendState blind = new BlendState
        {

            ColorSourceBlend = Blend.Zero,
            ColorDestinationBlend = Blend.InverseSourceColor,

            AlphaSourceBlend = Blend.Zero,
            AlphaDestinationBlend = Blend.InverseSourceColor

        };

        public bool Sleeping => npc.active && npc.ai[0] < 1 && npc.ai[0] >= 0;
        public bool WakingUp => npc.active && npc.ai[0] >= 1 && npc.ai[0] < 300;

        public bool WakingUpStartMusic => npc.ai[0] >= 200;
        public bool Activestate => npc.active && npc.ai[0] >= 300 && !DyingState;
        public int FinalTeleportState => (int)npc.ai[0] - 25000;



        public bool DyingState => npc.ai[0] >= 100000;
        public int timePassed = 0;
        public Projectile deathproj;

        public void StateDying()
        {
            npc.ai[0] += 1f;
            npc.dontTakeDamage = true;
            if (goingDark > 0)
                goingDark /= 4;

            fadeOutAlpha += (1 - fadeOutAlpha) / 5f;
            npc.velocity /= 2f;

            if (npc.ai[0] == 100001)
            {

                Deathsnd = Main.PlaySound(SoundID.Shatter, (int)npc.Center.X, (int)npc.Center.Y, 0);
                cutscenestartpos = new Vector3(Main.screenPosition.X, Main.screenPosition.Y, 1f);

                deathproj = Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ModContent.ProjectileType<SpaceBossExplode>(), 0, 0);

                scenecam = cutscenestartpos;
                //scenecamend = npc.Center - (new Vector2(Main.screenWidth, Main.screenHeight) / 2f);
                camlength = 20;
                camlength2 = 400;
                film = new Film();
                film.AppendSequence(camlength, FilmSetCamera);
                film.AppendSequence(camlength2 - camlength, FilmSetCamera);
                //for(int i=0;i<150;i++)
                film.AppendSequence(100, FilmSetCamera);

                film.AddSequence(0, film.AppendPoint, PerFrameSettings);

                CinematicManager.Instance.PlayFilm(film);

                foreach (SpaceBossRock rock in Rocks)
                {
                    rock.LockUnlock(false);
                }

            }

            if (deathproj != null && deathproj.active)
            {
                deathproj.Center = npc.Center;
            }

            if (Deathsnd != null)
            {
                Deathsnd.Volume = 0.99f;
                Deathsnd.Pitch = MathHelper.Clamp(0.8f - (npc.ai[0] - 100000f) / 45f, -0.9f, 0.9f);
            }


            UnifiedRandom randonx = new UnifiedRandom(npc.whoAmI);

            int time = (int)npc.ai[0] - 100000;

            foreach (SpaceBossRock rock in Rocks)
            {
                //if (rock.airReady > 0)
                //{
                int index = randonx.Next(60, 200);
                if (time > index)
                {
                    rock.bossOffset += Vector2.One.RotatedByRandom(MathHelper.TwoPi) * ((time - index) * 0.05f);
                    rock.bossOffset += Vector2.Normalize(npc.Center - rock.Position) * ((time - index) * 0.025f);
                    rock.whiten += 0.025f;
                }
                //}
            }

            UnifiedRandom rando = new UnifiedRandom(npc.type);
            foreach (SpaceBossEye eyxxe in Eyes)
            {
                int index = randonx.Next(40, 200);
                if (Main.rand.Next(0, time) > index)
                    eyxxe.NewLook(npc.Center + new Vector2(rando.Next(24, 160)).RotatedBy(rando.NextFloat(-1f, 1f) * npc.localAI[0]), 0.25f, true, true);

                if (time - index > 120 && eyxxe.customID == 0)
                {
                    eyxxe.Explode();
                }

            }

            if (npc.ai[0] == 100300)
            {
                npc.ai[0] = 250000;
                npc.StrikeNPCNoInteraction(1000000, 0, 0);
            }
        }


        public void StateSleeping()
        {
            if (TetherAsteriods.Count() < 1)
                npc.ai[0] = 1;

            foreach (SpaceBossRock rock in Rocks)
            {
                rock.UpdateRotation(rock.randomRotation2);
            }

            foreach (SpaceBossEye eye in Eyes)
            {
                if (eye.customID == 1 || eye.customID == -1)
                {
                    eye.NewLook(Main.LocalPlayer.MountedCenter, 1f, false, false);
                }
                else
                {
                    eye.NewLook(eye.BasePosition + Vector2.UnitY, 1f, false, false);
                    if (npc.ai[0] > 200)
                    {
                        eye.eyeScale = MathHelper.Clamp(eye.eyeScale + 0.05f, 0f, 2f);
                    }
                }
                //eye.lookTo = Main.LocalPlayer.MountedCenter;
            }
            npc.velocity += new Vector2(0, (float)Math.Cos(timePassed / 200) * 0.005f);
            npc.velocity *= 0.98f;
            SpaceDim.crystalAsteriods = false;

        }

        public static Vector3 cutscenestartpos;
        public static Vector3 scenecamend;
        public static Vector3 scenecam;
        public static int cutsceneposition = 0;
        public static int camlength = 300;
        public static int camlength2 = 120;


        private void FilmSetCamera(FrameEventData evt)
        {
            if (evt.Frame < 2)
                cutscenestartpos = new Vector3(Main.screenPosition.X, Main.screenPosition.Y, 1f);

            if (evt.AbsoluteFrame < camlength)
            {
                Vector2 diff = npc.Center - (new Vector2(Main.screenWidth, Main.screenHeight) / 2f);
                scenecamend = Vector3.Lerp(cutscenestartpos, new Vector3(diff.X, diff.Y, 1f), evt.Frame / (float)evt.Duration);
            }

            if (evt.AbsoluteFrame > camlength2)
            {
                Vector2 diff = Main.LocalPlayer.Center - (new Vector2(Main.screenWidth, Main.screenHeight) / 2f);
                scenecamend = Vector3.Lerp(cutscenestartpos, new Vector3(diff.X, diff.Y, 1f - MathHelper.Clamp(((evt.Frame / (float)evt.Duration) * 3f) - 2f, 0f, 1f)), (evt.Frame / (float)evt.Duration) * (1.25f));
            }

            scenecam += (scenecamend - scenecam) * 0.05f;
            //Main.screenPosition = scenecam;
        }

        private void PerFrameSettings(FrameEventData evt)
        {
            cutsceneposition = 2;
        }

        private void MoveCamera()
        {
            if (SpaceBoss.cutsceneposition > 0 && SpaceBoss.film.IsActive && SGAPocketDim.WhereAmI == typeof(SpaceDim))
            {
                Vector2 oldpos = Main.screenPosition;
                Main.screenPosition = Vector2.Lerp(new Vector2(SpaceBoss.scenecam.X, SpaceBoss.scenecam.Y), Main.screenPosition, MathHelper.Clamp(1f - SpaceBoss.scenecam.Z, 0f, 1f));
            }
        }

        public void StateWakingUp()
        {
            npc.SGANPCs().NoHit = true;

            bool canAdvance = true;

            npc.TargetClosestUpgraded(false, npc.Center);

            if (npc.ai[0] == 1)
            {

                camlength = 100;
                camlength2 = 500;
                cutscenestartpos = new Vector3(Main.screenPosition.X, Main.screenPosition.Y, 1f);

                scenecam = cutscenestartpos;
                film = new Film();
                //scenecamend = npc.Center - (new Vector2(Main.screenWidth, Main.screenHeight) / 2f);
                film.AppendSequence(camlength, FilmSetCamera);
                film.AppendSequence(camlength2 - camlength, FilmSetCamera);
                //for(int i=0;i<150;i++)
                film.AppendSequence(75, FilmSetCamera);

                film.AddSequence(0, film.AppendPoint, PerFrameSettings);

                CinematicManager.Instance.PlayFilm(film);
            }

            foreach (SpaceBossRock rock in Rocks)
            {
                rock.randomRotation2 *= 0.98f;
                rock.UpdateRotation(rock.randomRotation2);
            }

            foreach (SpaceBossEye eye in Eyes)
            {
                if (eye.customID == 1 || eye.customID == -1)
                {
                    eye.NewLook(npc.Center + Vector2.UnitY, 0.01f, true, true);
                }
            }

            if (npc.ai[0] > 120)
            {
                shieldeffect -= 1f;
                if (shieldeffect < 0)
                    shieldeffect = 0;

                UnifiedRandom randomrock = new UnifiedRandom(npc.whoAmI * 8);
                foreach (SpaceBossEye eye in Eyes)
                {
                    if (eye.boss != this)
                    {

                        eye.sleep += 0.015f;
                        if (!eye.openUp && eye.sleep > 0)
                        {
                            for (int i = 0; i < 10; i += 1)
                            {
                                SpaceBoss.SpaceBossStardust dusts = new SpaceBoss.SpaceBossStardust(eye.BasePosition, Main.rand.NextVector2Circular(1f, 1f) * (i / 20f), 50);
                                dusts.scale = Vector2.One * (1.5f + (i / 30f));
                                dusts.alpha = 0.75f;
                                dusts.fadeIn = 20;
                                dusts.fadeTime = 20;
                                stardustEffectsOnTopOfRocks.Add(dusts);
                            }
                        }
                        eye.beamAlpha += 0.015f;

                        if (eye.sleep > 1f)
                        {
                            eye.NewLook(Main.player[npc.target].MountedCenter, 0.01f, false, false);
                        }
                        else
                        {
                            eye.NewLook(eye.BasePosition + Vector2.UnitY, 0.01f, true, true);
                            eye.progress = 0f;
                        }

                        SpaceBossRock rock = (SpaceBossRock)eye.boss;

                        if (eye.sleep > 1.15f && !rock.followBoss)
                        {
                            Vector2 toboss = npc.Center - rock.bossOffset;
                            rock.bossOffset += Vector2.Normalize(toboss) * MathHelper.Clamp((eye.sleep - 1.5f) * 6f, 0f, 12f);
                            if (toboss.Length() < rock.bossRockDist)
                            {
                                rock.LockUnlock(true);
                            }
                            canAdvance = false;
                        }
                        else
                        {
                            //canAdvance = false;
                        }
                    }
                }
            }
            else
            {
                bool boom = (int)npc.ai[0] == 118;

                if (boom)
                {
                    SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_FlameburstTowerShot, npc.Center);
                    if (sound != null)
                    {
                        sound.Volume = 0.99f;
                        sound.Pitch += 0.5f;
                    }
                }

                float timeScaleUp = npc.ai[0] / 120f;

                for (int i = 0; i < (boom ? 150 : 3); i += 1)
                {

                    Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                    Vector2 vect = new Vector2(npc.Center.X, npc.Center.Y);
                    Vector2 vect2 = offset * Main.rand.NextFloat(16f, 32f);
                    Vector2 vect3 = Vector2.Normalize(-offset) * (float)((boom ? 4f : 1f) * Main.rand.NextFloat(0.50f, 2.50f));

                    SpaceBoss.SpaceBossStardust dusts = new SpaceBoss.SpaceBossStardust(vect + vect2 * (2.5f + (timeScaleUp * 2f)), boom ? vect3 : vect3 * (1f + (timeScaleUp * 2f)), boom ? Main.rand.Next(60, 90) : 25);
                    dusts.scale = Vector2.One * 1.00f;
                    dusts.fadeIn = boom ? 50 : 10;
                    dusts.fadeTime = boom ? 50 : 10;
                    stardustEffectsOnTopOfRocks.Add(dusts);


                    int dust = Dust.NewDust(vect2, 0, 0, DustID.BlueCrystalShard);
                    Main.dust[dust].scale = 0.5f;
                    Main.dust[dust].velocity = vect3;
                    Main.dust[dust].noGravity = true;

                }
            }


            if (npc.ai[0] < 280 || canAdvance)
                npc.ai[0] += 1f;

            if ((int)npc.ai[0] < 2)
            {
                foreach (SpaceBossEye eye in Eyes)
                {
                    eye.NewLook(npc.Center + Vector2.UnitY * 120f, 0.005f);
                }
            }

            npc.velocity *= 0.96f;
        }

        public void LaunchTethers(int ammount = 3, float maxRange = 2000)
        {
            List<FilledSpaceArea> vex = new List<FilledSpaceArea>(SpaceDim.FilledSpaces);
            Vector2 previousone = default;
            float angle = 0.80f;
            float fails = 1f;

            for (int i = 0; i < ammount; i += 1)
            {
                float angleCheck = (1f - (angle / fails));
                vex = vex.OrderBy(testby => (testby.position - npc.Center).LengthSquared() + (previousone == default ? 0 :
                Vector2.Dot(Vector2.Normalize(testby.position - npc.Center), Vector2.Normalize(previousone - npc.Center)) > angleCheck ? 999999 : 0)).ToList();

                Vector2 position1 = vex[0].position;
                Vector2 distanceToPoint = position1 - npc.Center;

                float dist = distanceToPoint.LengthSquared();


                if (dist > maxRange * maxRange || dist < 128 * 128)
                {
                    LaunchTethers((ammount - i) - 1);
                    return;
                }

                bool succeed = false;

                for (int tryAgain = 32; tryAgain < 128; tryAgain += 16)
                {
                    if (!succeed)
                    {
                        position1 = vex[0].position + Main.rand.NextVector2Circular(tryAgain, tryAgain);
                        distanceToPoint = position1 - npc.Center;
                        previousone = position1;

                        if (!Collision.CanHitLine(position1, 4, 4, npc.Center, 4, 4))
                        {
                            Projectile proj = Projectile.NewProjectileDirect(npc.Center, Vector2.Normalize(distanceToPoint) * 16f, ModContent.ProjectileType<MineableAsteriodBossLock>(), 0, 0);
                            if (proj != null)
                            {
                                proj.rotation = proj.velocity.ToRotation() - MathHelper.PiOver2;
                            }
                            fails = 1;
                            tryAgain = 1337;
                            succeed = true;
                            goto endhere;
                        }
                        else
                        {

                        }
                    }
                }

            endhere:
                if (!succeed)
                {
                    fails++;
                    i -= 1;
                }
                vex.RemoveAt(0);
            }
        }

        public void ManageDarknessAttack()
        {
            if (goingDark > 0)
            {
                SGAmod.fogDrawNPCsCounter = Math.Max(SGAmod.fogDrawNPCsCounter, 300);

                foreach (Player player in Main.player)
                {
                    if (player.active)
                    {
                        SGADimPlayer dply = player.GetModPlayer<SGADimPlayer>();
                        dply.noLightGrow = (int)MathHelper.Max(dply.noLightGrow, 5);
                        dply.lightSize = (int)(dply.lightSize + ((750 - dply.lightSize) * 0.0010f));
                        player.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationTwo").Type, 2);
                    }
                }

                foreach (SpaceBossRock rock in RocksAreas)
                {
                    if (rock.airReady > 0)
                    {
                        int size = (int)MathHelper.Clamp(rock.airReady * 2, 0, 1500);
                        Vector2 poz = rock.Position - Main.screenPosition;
                        if (poz.X > -size && poz.X < Main.screenWidth + size && poz.Y > -size && poz.Y < Main.screenHeight + size)
                            SGAmod.PostDraw.Add(new PostDrawCollection(new Vector3(rock.Position.X, rock.Position.Y, size)));
                    }
                }



                //Crystal Codes here

                if (npc.aiAction < 200)
                {
                    List<Vector2> checkPositions = new List<Vector2>(visitedEmptySpaces);

                    Vector2 previousLoc = npc.Center;
                    SpaceBossShadowNebulaEnemy previous = null;
                    int index = 0;

                    int otherIndex = 0;

                    foreach (SpaceBossRock rock in Rocks)
                    {
                        if (otherIndex % 2 == 0)
                        {
                            Vector2 rockPos = rock.Position;
                            int guy = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<SpaceBossShadowNebulaEnemy>());

                            NPC summonedStar = Main.npc[guy];
                            SpaceBossShadowNebulaEnemy starClass = summonedStar.modNPC as SpaceBossShadowNebulaEnemy;
                            starClass.masterBeforeMe = previous;
                            starClass.spawnedFrom = rock;

                            Vector2 somewhere = previousLoc;

                            if (index == 0)
                            {
                                starClass.emptySpace = Main.player[npc.target].Center + new Vector2(0, -128);
                            }
                            else
                            {
                                checkPositions = checkPositions.Where(testby => (testby - somewhere).LengthSquared() > 640f * 640f).OrderBy(testby => (testby - somewhere).LengthSquared() + (testby - npc.Center).LengthSquared()).ToList();

                                checkPositions.RemoveAt(0);
                                somewhere = checkPositions[0];

                                starClass.emptySpace = somewhere;
                            }

                            previous = starClass;
                            previousLoc = somewhere;

                            index += 1;
                        }

                        otherIndex += 1;
                    }

                    npc.aiAction = 200;
                }


                bool allowedToEnd = true;

                foreach (NPC guy in Main.npc.Where(testby => testby.active && testby.type == ModContent.NPCType<SpaceBossShadowNebulaEnemy>()))
                {
                    guy.ai[2] = 60;
                    //SpaceBossShadowNebulaEnemy modGuy = guy.modNPC as SpaceBossShadowNebulaEnemy;
                    if (guy.ai[3] < 1)
                    {
                        allowedToEnd = false;
                    }
                }

                if (!allowedToEnd)
                {
                    if (goingDark < 30)
                        goingDark = 30;
                }

            }


            fadeOutAlpha = MathHelper.Clamp(fadeOutAlpha + (goingDark > 0 ? -0.0025f : 0.01f), 0f, 1f);

            if (goingDark > -1200)
                goingDark -= 1;
        }

        public void RespawnBoss()
        {

            NPC.NewNPC((int)startPosition.X, (int)startPosition.Y, npc.type);
            npc.active = false;
            npc.type = 0;

            foreach (Projectile proj in TetherAsteriods)
            {
                proj.active = false;
            }

        }







        public override void AI()
        {
            Initiate();
            state = 0;
            npc.localAI[0] += 1;
            if (stardustEffects.Count > 0)
            {
                stardustEffects.ForEach(testby => testby.Update());
                stardustEffects = stardustEffects.Where(testby => testby.timeLeft > 0).ToList();
            }
            if (stardustEffectsOnTopOfRocks.Count > 0)
            {
                stardustEffectsOnTopOfRocks.ForEach(testby => testby.Update());
                stardustEffectsOnTopOfRocks = stardustEffectsOnTopOfRocks.Where(testby => testby.timeLeft > 0).ToList();
            }

            if (npc.ai[3] > 1 && npc.life > 0 && !DyingState)
            {
                countdownToTheEnd -= 1;
            }

            timePassed += 1;
            cutsceneposition -= 1;

            friction = 0.95f;
            npc.chaseable = true;

            installRoids -= 1;

            specialCooldown -= 1;

            for (int i = 1; i < screenPos.Length; i += 1)
            {
                screenPos[i] = screenPos[i - 1];
            }

            screenPos[0] = npc.Center - Main.screenPosition;

            foreach (SpaceBossEye eye in Eyes)
                eye.Update();

            if (DyingState)
            {
                StateDying();
                return;
            }

            if (Sleeping)
            {
                StateSleeping();
            }
            else
            {
                if (NoHitMode && Main.player[npc.target].dead)
                {
                    RespawnBoss();
                    return;
                }

                if (darknessAura < (phase > 1 ? 1f : 0.75f) && npc.ai[0] > 120)
                    darknessAura += 0.005f;

                if (WakingUp)
                    StateWakingUp();
                if (Activestate)
                    ai.StateActive();

            }

            ManageDarknessAttack();

            //installRoids = false;

            npc.velocity *= friction;
        }

        public static void DarknessNebulaEffect(Texture2D sunGlow, float scale, Vector2 position, float alphaik, int extraseed, float length = 10, float timescale = 1f)
        {

            if (alphaik > 0)
            {
                ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.TwilightDye);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                Texture2D stain = SGAmod.Instance.GetTexture("Doom_Harbinger_Resprite_pupil");

                DrawData value28 = new DrawData(sunGlow, new Vector2(240, 240), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 240, 240)), Microsoft.Xna.Framework.Color.White, MathHelper.PiOver2, stain.Size() / 2f, 1f, SpriteEffects.None, 0);

                List<(float, float, float)> ordering = new List<(float, float, float)>();

                UnifiedRandom rando = new UnifiedRandom(extraseed);

                for (float f = 0f; f < 1f; f += 1f / length)
                {
                    float time = Main.GlobalTime * timescale;
                    if (timescale < 0)
                        time = 100f - ((Main.GlobalTime * -timescale) % 100f);

                    float prog = MathHelper.Clamp((f + (time * 0.25f)) % (1f), 0f, 1f);
                    float alpha = MathHelper.Clamp((prog * 5f) * MathHelper.Clamp(1.5f - (prog * 1.5f), 0f, 1f), 0f, 1f);
                    ordering.Add((prog, alpha, rando.NextFloat(MathHelper.TwoPi)));
                }
                ordering = ordering.OrderBy(testby => testby.Item1).ToList();
                float scalee = scale;

                int index2 = 0;

                Vector2 half = sunGlow.Size() / 2f;

                foreach ((float, float, float) prog in ordering)
                {
                    //float prog = MathHelper.Clamp((f+(Main.GlobalTime*0.25f))%(1f),0f,1f);
                    shader.UseColor(Color.Lerp(Color.White, Color.Blue, MathHelper.Clamp(prog.Item1 * 2f - 1f, 0f, 1f)).ToVector3());
                    shader.UseOpacity(1f);
                    shader.Apply(null, new DrawData?(value28));

                    float angle = prog.Item3;

                    Vector2 pos = position - Main.screenPosition;// Vector2.Lerp(npc.Center-Main.screenPosition,new Vector2(Main.screenWidth, Main.screenHeight)/2f, prog.Item1 / 5f);

                    Main.spriteBatch.Draw(sunGlow, pos, null, Color.White * (prog.Item2 * alphaik), angle, half, (scalee) + (0.05f + prog.Item1 * 1.25f), SpriteEffects.None, 0f);

                    index2 += 1;
                }
            }

        }

        public void AlterSky(SpaceSky sky, int index, SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {

            if (Sleeping && (Main.LocalPlayer.Center - npc.Center).LengthSquared() > 2000 * 2000)
                return;

            if (index == 1 && Activestate)
            {
                Texture2D sunray = SGAmod.ExtraTextures[116];

                for (float f = 0; f < 1f; f += 0.25f)
                {
                    float indexsize = (f + Main.GlobalTime / 8f) % 1f;


                    spriteBatch.Draw(sunray, sky.sunPosition, null, (Color.White * (shieldeffect / 15f)) * sky.skyalpha * (1f - indexsize), ((npc.Center - Main.screenPosition) - sky.sunPosition).ToRotation() + MathHelper.PiOver2, new Vector2(sunray.Width / 2f, sunray.Height), Vector2.One * (indexsize * 3f), SpriteEffects.None, 0f);
                }

            }

            if (index == 3)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

                //Darkness Nebula

                ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.TwilightDye);

                Texture2D sunGlow = mod.GetTexture("GlowOrb");
                Vector2 half = sunGlow.Size() / 2f;
                Texture2D stain = mod.GetTexture("Stain");

                DrawData value28 = new DrawData(sunGlow, new Vector2(240, 240), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 600, 600)), Microsoft.Xna.Framework.Color.White, MathHelper.PiOver2, stain.Size() / 2f, 0.2f, SpriteEffects.None, 0);

                if (sky.darkalpha > 0)
                {

                    UnifiedRandom alwaysthesame = new UnifiedRandom(DimDungeonsProxy.DungeonSeeds - 5643);

                    for (float i = 0.03f; i <= 0.32f; i += 0.02f)
                    {
                        for (int xy = 0; xy < 2; xy += 1)
                        {

                            Vector2 scale = (Vector2.One * 1f) + (new Vector2(i) * 6f);
                            Vector2 loc = new Vector2(alwaysthesame.Next(-64, Main.screenWidth + 64), alwaysthesame.Next(-64, Main.screenHeight + 64));

                            shader.UseColor(Color.Lerp(Color.PaleTurquoise, Color.Blue, alwaysthesame.NextFloat(1f)).ToVector3());
                            shader.UseOpacity(1f);
                            shader.Apply(null, new DrawData?(value28));

                            float angle = alwaysthesame.NextFloat(MathHelper.TwoPi);

                            spriteBatch.Draw(sunGlow, loc, null, Color.White * sky.darkalpha, angle, half, scale, SpriteEffects.None, 0f);

                        }
                    }
                }

                value28 = new DrawData(sunGlow, new Vector2(240, 240), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 240, 240)), Microsoft.Xna.Framework.Color.White, MathHelper.PiOver2, stain.Size() / 2f, 1f, SpriteEffects.None, 0);


                //Sun absorb healing

                if (npc.ai[0] > 800 && shieldeffect > 0 && sky.skyalpha > 0)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


                    Texture2D tex = mod.GetTexture("Extra_57b");
                    Vector2 starhalf = tex.Size() / 2f;

                    List<(float, float, float, Vector2)> ordering = new List<(float, float, float, Vector2)>();

                    UnifiedRandom rando = new UnifiedRandom(npc.whoAmI);

                    for (float f = 0f; f < 1f; f += 1 / 120f)
                    {
                        float prog = MathHelper.Clamp((f + (Main.GlobalTime * 0.05f) + rando.NextFloat(1f)) % (1f), 0f, 1f);
                        float alpha = MathHelper.Clamp((prog * 5f) * MathHelper.Clamp(1.5f - (prog * 1.5f), 0f, 1f), 0f, 1f) * MathHelper.Clamp((float)Math.Sin((Main.GlobalTime * 0.1f) + rando.NextFloat(MathHelper.TwoPi)), 0f, 1f);
                        ordering.Add((prog, alpha, rando.NextFloat(MathHelper.TwoPi) + (Main.GlobalTime * rando.NextFloat(-0.5f, 0.5f)), rando.NextVector2Circular(320f, 320f)));
                    }
                    ordering = ordering.OrderBy(testby => testby.Item1).ToList();

                    int index2 = 0;

                    foreach ((float, float, float, Vector2) prog in ordering)
                    {

                        float angle = prog.Item3;

                        float percentToSun = (prog.Item1 * prog.Item1);
                        Vector2 pos = Vector2.Lerp(npc.Center - Main.screenPosition, sky.sunPosition, 1f - (percentToSun));

                        spriteBatch.Draw(tex, pos + (prog.Item4 * (percentToSun)), null, (Color.Lerp(Color.Yellow, Color.Blue, percentToSun) * (prog.Item2) * MathHelper.Clamp(percentToSun * 4f, 0f, 1f) * (shieldeffect / 15f)) * sky.skyalpha, angle, starhalf, (0.25f + percentToSun * 0.5f), SpriteEffects.None, 0f);

                        index2 += 1;
                    }
                }

                //Twilight Nebula

                if (!removeDarknessCloud)
                {
                    float alphaik = ((1f - sky.darkalpha) * 0.50f * darknessAura) * MathHelper.Clamp(1f - (npc.ai[0] - 100000f) / 180f, 0f, 1f);

                    if (alphaik > 0)
                    {
                        if (sky.darkalpha < 1 && (sky.darkalpha > 0 || (new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) - (npc.Center - Main.screenPosition)).LengthSquared() < 1440000))
                        {
                            DarknessNebulaEffect(sunGlow, (sky.darkalpha * 5f) * 6f, npc.Center, alphaik, npc.whoAmI, 10);
                        }
                    }
                }
            }
        }

        public void DrawGlowStuffs(SpriteBatch spriteBatch, int layer)
        {

            Texture2D glowtex = mod.GetTexture("Glow");
            Vector2 glowOrigin = glowtex.Size() / 2f;

            if (layer == 0)
            {
                foreach (SpaceBossStardust dust in stardustEffects)
                {
                    float alpha2 = MathHelper.Clamp(dust.timeStart / dust.fadeIn, 0f, Math.Min(dust.timeLeft / dust.fadeTime, 1f));
                    spriteBatch.Draw(glowtex, dust.Position - Main.screenPosition, null, Color.WhiteSmoke * alpha2 * dust.alpha * 0.75f, 0, glowOrigin, dust.scale, SpriteEffects.None, 0f);
                }
                foreach (SpaceBossStardust dust in stardustEffects)
                {
                    Texture2D starTex = Main.starTexture[dust.rando % Main.starTexture.Length];

                    float alpha2 = MathHelper.Clamp(dust.timeStart / dust.fadeIn, 0f, Math.Min(dust.timeLeft / dust.fadeTime, 1f));
                    spriteBatch.Draw(starTex, dust.Position - Main.screenPosition, null, Color.White * alpha2 * dust.alpha, 0, starTex.Size() / 2f, dust.scale, SpriteEffects.None, 0f);
                }
            }
            if (layer == 1)
            {
                foreach (SpaceBossStardust dust in stardustEffectsOnTopOfRocks)
                {
                    float alpha2 = MathHelper.Clamp(dust.timeStart / dust.fadeIn, 0f, Math.Min(dust.timeLeft / dust.fadeTime, 1f));
                    spriteBatch.Draw(glowtex, dust.Position - Main.screenPosition, null, Color.WhiteSmoke * alpha2 * dust.alpha * 0.75f, 0, glowOrigin, dust.scale, SpriteEffects.None, 0f);
                }
                foreach (SpaceBossStardust dust in stardustEffectsOnTopOfRocks)
                {
                    Texture2D starTex = Main.starTexture[dust.rando % Main.starTexture.Length];

                    float alpha2 = MathHelper.Clamp(dust.timeStart / dust.fadeIn, 0f, Math.Min(dust.timeLeft / dust.fadeTime, 1f));
                    spriteBatch.Draw(starTex, dust.Position - Main.screenPosition, null, Color.White * alpha2 * dust.alpha, 0, starTex.Size() / 2f, dust.scale, SpriteEffects.None, 0f);
                }
            }
        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            bool near = true;
            //if (Sleeping)
            //{
            int bufferRange = 64;
            Rectangle bufferrect = new Rectangle((int)Main.screenPosition.X - bufferRange, (int)Main.screenPosition.Y - bufferRange, (int)Main.screenPosition.X + Main.screenWidth + bufferRange, (int)Main.screenPosition.Y + Main.screenHeight + bufferRange);
            int bufferRange2 = 640;
            Rectangle bufferrect2 = new Rectangle((int)Main.screenPosition.X - bufferRange2, (int)Main.screenPosition.Y - bufferRange2, (int)Main.screenPosition.X + Main.screenWidth + bufferRange2, (int)Main.screenPosition.Y + Main.screenHeight + bufferRange2);

            DrawGlowStuffs(spriteBatch, 0);

            if (!bufferrect2.Contains(npc.Center.ToPoint()))
            {
                near = false;

                foreach (Projectile asteriod in TetherAsteriods)
                {
                    if (bufferrect.Contains(asteriod.Center.ToPoint()))
                    {
                        near = true;
                        break;
                    }

                }
            }

            if (!Sleeping && !near)
            {
                foreach (SpaceBossRock rock in Rocks)
                {
                    if (bufferrect.Contains(rock.Position.ToPoint()))
                    {
                        near = true;
                        break;
                    }
                }
            }

            //}

            if (!near)
                return false;

            Texture2D firerock = ModContent.GetTexture("SGAmod/Projectiles/FieryRock");
            Texture2D rocksmall = mod.GetTexture("Dimensions/Space/OverseenHead");
            Texture2D rocklarge = mod.GetTexture("Dimensions/Space/GlowAsteriod");
            Texture2D eyetex = mod.GetTexture("Doom_Harbinger_Resprite_pupil");
            Vector2 eyetexorig = eyetex.Size() / 2f;

            Vector2 firerockorig = firerock.Size() / 2f;
            Vector2 rocksmallorig = new Vector2(rocksmall.Width, rocksmall.Height / 2f);
            Vector2 rocklargeorig = new Vector2(rocklarge.Width, rocklarge.Height / 2f);

            Texture2D glowtex = mod.GetTexture("Glow");
            Vector2 glowOrigin = glowtex.Size() / 2f;


            if (celerityBuff > 0)
            {

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                float celerityBuffAlpha = MathHelper.Clamp(celerityBuff / 300f, 0f, 1f);
                float glowScale = 1f + celerityBuffAlpha*0.5f;
                Color glowColor = Color.CornflowerBlue * celerityBuffAlpha;

                foreach (SpaceBossRock rock in Rocks)
                {
                    float glowExtra = rock.state == 400 ? 1f : 0.80f;

                    spriteBatch.Draw(glowtex, rock.Position - Main.screenPosition, null, glowColor* glowExtra, 0, glowOrigin, glowScale, SpriteEffects.None, 0f);
                }

            }


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            //Rocks Lines



            if (!DyingState)
            {
                foreach (SpaceBossRock rock in Rocks)
                {
                    if (rock.eye != null)
                    {
                        float alpha = rock.eye.beamAlpha - 0.60f;
                        if (alpha <= 0 && rock.state < 1000)
                            continue;

                        if (rock.whiten > 0)
                        {
                            alpha = MathHelper.Clamp(1f - Main.rand.NextFloat(0f, rock.whiten), 0, 1);
                        }

                        List<Vector2> toThem = new List<Vector2>();

                        toThem.Add(rock.Position);
                        toThem.Add(npc.Center);

                        TrailHelper trail = new TrailHelper("FadedBasicEffectPass", Main.sunTexture);
                        trail.projsize = Vector2.Zero;
                        trail.coordOffset = new Vector2(0, 0f);
                        trail.coordMultiplier = new Vector2(1f, 0.5f);
                        trail.trailThickness = 16;
                        trail.trailThicknessIncrease = -12;
                        trail.doFade = false;
                        trail.color = delegate (float percent)
                        {
                            return Color.CornflowerBlue * (MathHelper.Clamp((alpha + percent) - 1f, 0f, 1f)) * fadeOutAlpha;
                        };
                        if (fadeOutAlpha > 0.01)
                            trail.DrawTrail(toThem, npc.Center);
                    }
                }
            }

            //Rock Areas
            if (RocksAreas.Count > 0)
            {

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                Texture2D sunText = Main.sunTexture;

                ArmorShaderData stardustsshader3 = GameShaders.Armor.GetShaderFromItemId(ItemID.VortexDye);

                foreach (SpaceBossRock rock in RocksAreas)
                {
                    Vector2 poz = rock.Position - Main.screenPosition;
                    int size = 128;
                    if (poz.X > -size && poz.X < Main.screenWidth + size && poz.Y > -size && poz.Y < Main.screenHeight + size)
                    {
                        //spriteBatch.Draw(Main.bubbleTexture, rock.Position - Main.screenPosition, null, rock.color, rock.Rotation + (rock.randomRotation2 * 674f), Main.bubbleTexture.Size() / 2f, MathHelper.Clamp(rock.timer / 300f, 0f, 1f), SpriteEffects.None, 0f);

                        DrawData value28 = new DrawData(sunText, new Vector2(240, 240), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 240, 240)), Microsoft.Xna.Framework.Color.White, 0, sunText.Size() / 2f, 1f, SpriteEffects.None, 0);
                        stardustsshader3.UseColor(Color.Lerp(rock.color, Color.Blue, 0.50f).ToVector3());
                        stardustsshader3.UseOpacity(0.5f);
                        stardustsshader3.Apply(null, new DrawData?(value28));
                        spriteBatch.Draw(sunText, rock.Position - Main.screenPosition, null, rock.color, rock.Rotation + (rock.randomRotation2 * 674f), sunText.Size() / 2f, MathHelper.Clamp(rock.timer / 300f, 0f, 1f), SpriteEffects.None, 0f);
                    }
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                Texture2D bubbletex = ModContent.GetTexture("SGAmod/Dimensions/Space/BubbleHuge");

                foreach (SpaceBossRock rock in RocksAreas)
                {
                    Vector2 poz = rock.Position - Main.screenPosition;
                    int size = 128;
                    if (rock.airReady > 0 && poz.X > -size && poz.X < Main.screenWidth + size && poz.Y > -size && poz.Y < Main.screenHeight + size)
                    {
                        spriteBatch.Draw(bubbletex, rock.Position - Main.screenPosition, null, rock.color * MathHelper.Clamp(rock.airReady / 120f, 0f, 1f), (float)Math.Sin(rock.Rotation) * 0.4f, bubbletex.Size() / 2f, MathHelper.Clamp(rock.airReady / 600f, 0f, 1f) * 1f, SpriteEffects.None, 0f);
                    }
                }
            }


            //'Real' Rocks
            if (fadeOutAlpha > 0)
            {

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                Effect fadeIn = SGAmod.FadeInEffect;

                foreach (SpaceBossRock rock in Rocks)
                {
                    Vector2 poz = rock.Position - Main.screenPosition;
                    Texture2D smolRock = ModContent.GetTexture((new string[3] { "SGAmod/Dimensions/Space/GlowAsteriod", "SGAmod/Dimensions/Space/GlowAsteriodalt", "SGAmod/Dimensions/Space/GlowAsteriodalt2" })[rock.variation % 3]);
                    int size = 80;
                    if (poz.X > -size && poz.X < Main.screenWidth + size && poz.Y > -size && poz.Y < Main.screenHeight + size)
                    {

                        fadeIn.Parameters["alpha"].SetValue(fadeOutAlpha);
                        fadeIn.Parameters["strength"].SetValue(rock.whiten);
                        fadeIn.Parameters["fadeColor"].SetValue(Color.White.ToVector3());
                        fadeIn.Parameters["blendColor"].SetValue(rock.color.ToVector3());

                        fadeIn.CurrentTechnique.Passes["FadeIn"].Apply();

                        Rectangle rect = new Rectangle(0, 0, smolRock.Width, smolRock.Height / 2);

                        spriteBatch.Draw(smolRock, rock.Position - Main.screenPosition, rect, rock.color, rock.Rotation, rect.Size() / 2f, rock.scale, SpriteEffects.None, 0f); ;
                    }
                }
            }

            //Tetheroids

            int index2 = 0;

            bool fightActive = npc.ai[0] > 999;

            foreach (Projectile asteriod in TetherAsteriods)
            {
                UnifiedRandom rand = new UnifiedRandom(asteriod.whoAmI);

                List<Vector2> toThem = new List<Vector2>();

                Vector2 beam = asteriod.Center + ((Vector2.UnitY).RotatedBy(asteriod.rotation - MathHelper.Pi)) * 50f;
                Vector2 distanceVector = beam - npc.Center;

                Vector2 previousHere = npc.Center;

                float sin1 = rand.NextFloat(48f, 72f);
                float sin2 = rand.NextFloat(16f, 32f);
                float sin3 = rand.NextFloat(80, 140f);

                for (float i = 0; i < distanceVector.Length(); i += rand.NextFloat(4f, 8f))
                {

                    Vector2 gothere = npc.Center + Vector2.Normalize(distanceVector) * i;
                    float percent = (160f * MathHelper.Clamp(1f - ((i / distanceVector.Length())), 0f, 1f));

                    percent = percent * MathHelper.Clamp(1f - ((i - 800) / 800f), 0f, 1f);
                    float dista = distanceVector.Length() - i;
                    percent = percent * MathHelper.Clamp(1f - ((dista) / 800f), 0f, 1f);

                    //Vector2 therenow = (Vector2.Normalize(gothere-previousHere).RotatedBy(MathHelper.PiOver2)) * ((float)noisy.Noise((int)gothere.X, (int)gothere.Y)*72f);
                    //Vector2 therenow = (Vector2.Normalize(distanceVector).RotatedBy(MathHelper.PiOver2)) * ((float)noisy.Noise((int)gothere.X - timePassed, (int)gothere.Y+timePassed) * percent));

                    float sint = (float)Math.Sin(((timePassed + i) / sin1) + asteriod.type);
                    sint = sint + (float)Math.Sin(((-timePassed + i) / sin2) + asteriod.type) * 0.15f;
                    sint = sint + (float)Math.Cos(((timePassed - i) / sin3) + asteriod.type) * 1.47f;

                    Vector2 therenow = (Vector2.Normalize(distanceVector).RotatedBy(MathHelper.PiOver2)) * (sint * percent);

                    gothere += therenow;

                    previousHere = gothere;

                    toThem.Add(gothere);

                }
                toThem.Add(beam);

                toThem.Reverse();

                TrailHelper trail = new TrailHelper(!fightActive ? "FadedBasicEffectPass" : "BasicEffectAlphaPass", !fightActive ? mod.GetTexture("TiledPerlin") : mod.GetTexture("ElectricFireNoColor"));
                trail.projsize = Vector2.Zero;
                trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
                trail.coordMultiplier = new Vector2(1f, fightActive ? 1f : 2f);

                trail.trailThickness = 16;
                trail.trailThicknessIncrease = -12;
                trail.strength = fightActive ? 4f : 0.65f;
                trail.doFade = false;

                float offset = (index2 * MathHelper.TwoPi) / 2.731f;

                trail.color = delegate (float percent)
                {
                    Color scrColor = (!fightActive ? Color.CornflowerBlue : Color.CornflowerBlue * 1f);
                    float sinner = (float)Math.Sin(offset + ((Main.GlobalTime * 4f)) + ((1f - percent) * MathHelper.TwoPi));
                    float wave = Math.Max(sinner, 0);
                    Color finalColor = Color.Lerp(Color.Transparent, scrColor * 0.75f, (wave));

                    if (!fightActive)
                        finalColor = Color.Lerp(scrColor * 0.75f, scrColor * 1.25f, MathHelper.SmoothStep(0f, 1f, wave));

                    return finalColor * (1f - MathHelper.Clamp((percent - 0f) * 1f, 0f, 1f));
                };
                trail.trailThicknessFunction = delegate (float percent)
                {
                    float sinner = (float)Math.Sin(offset + ((Main.GlobalTime * 4f)) + (percent * MathHelper.TwoPi));
                    return (16f + ((float)(Math.Max(sinner, 0) * 12f) * (1f - percent)) - (12f * percent)) * (fightActive ? 2f : 1f);
                };
                trail.DrawTrail(toThem, npc.Center);
                index2 += 1;

            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            ArmorShaderData stardustsshader = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye);

            DrawData value8 = new DrawData(rocklarge, new Vector2(300f, 300f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 320, 160)), Microsoft.Xna.Framework.Color.White, npc.rotation, firerockorig, npc.scale, SpriteEffects.None, 0);
            stardustsshader.UseColor(Color.CornflowerBlue.ToVector3());
            stardustsshader.UseOpacity(0.5f);
            stardustsshader.Apply(null, new DrawData?(value8));


            spriteBatch.Draw(rocklarge, (npc.Center) - Main.screenPosition, new Rectangle(0, 0, (int)rocklargeorig.X, (int)rocklargeorig.Y), Color.White * fadeOutAlpha, npc.rotation, rocklargeorig / 2f, new Vector2(1f, 1f), SpriteEffects.None, 0f);

            if (fadeOutAlpha > 0.1)
                DrawEyes(spriteBatch, false, false);

            //ArmorShaderData shader3 = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye); shader3.Apply(null);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            stardustsshader.UseColor(Color.Aqua.ToVector3());
            stardustsshader.UseOpacity(0.5f);
            stardustsshader.Apply(null, new DrawData?(value8));

            if ((FinalTeleportState > 0 && FinalTeleportState < 150) || npc.ai[3] > 1)
            {
                float alpha = npc.ai[3] > 1 ? 150 : FinalTeleportState;
                int index = 0;
                float extraadd = Math.Max(0, npc.ai[0] - 100000);
                for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 36f)
                {
                    index += 1;
                    spriteBatch.Draw(eyetex, npc.Center - Main.screenPosition, new Rectangle(0, 0, (int)eyetexorig.X * 2, (int)eyetexorig.Y), Color.White * 0.25f * MathHelper.Clamp(alpha / 200f, 0f, Math.Max(0.5f - (npc.ai[0] - 100000) * 0.025f, 0)), f + Main.GlobalTime * (index % 2 == 0 ? 1f : -1f), eyetexorig, new Vector2(0.75f, 4f * MathHelper.Clamp(alpha / 200f, 0f, 2f) + (extraadd / 5f)) * alpha / 24f, SpriteEffects.None, 0f);
                }
            }

            DrawGlowStuffs(spriteBatch,1);

            //Shield

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D[] texs = { mod.GetTexture("Doom_Harbinger_Resprite_eye"), mod.GetTexture("Noise"), mod.GetTexture("TiledPerlin") };

            int count = 0;
            float shieldAlpha = MathHelper.Clamp(((shieldeffect) / 30f), 0f, 1);
            float scale = 4f - (shieldAlpha * 3f);

            if (shieldAlpha > 0f)
            {

                foreach (Texture2D shieldstex in texs)
                {
                    Texture2D noise = shieldstex;
                    Vector2 noisesize = noise.Size();

                    DrawData value9 = new DrawData(noise, new Vector2(300f, 300f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, (int)noisesize.X, (int)noisesize.Y)), Microsoft.Xna.Framework.Color.White, npc.rotation, firerockorig, npc.scale, SpriteEffects.None, 0);
                    var deathShader = GameShaders.Misc["ForceField"];
                    deathShader.UseColor(new Vector3(1f, 1f, 1f));
                    GameShaders.Misc["ForceField"].Apply(new DrawData?(value9));
                    deathShader.UseOpacity(1f);

                    float angle = MathHelper.Pi;
                    Vector2 loc = new Vector2((float)((Math.Cos(angle) * 0f)), (float)((Math.Sin(angle) * 0f)));

                    if (count == 2)
                    {

                        if (npc.ai[0] < 1000)
                        {
                            float effect = MathHelper.Clamp((Main.GlobalTime / 4f) % 1.50f, 0f, 1f);
                            spriteBatch.Draw(noise, (npc.Center + loc) - Main.screenPosition, null, ((fightActive ? Color.White : Color.PaleTurquoise) * 0.25f * shieldAlpha * fadeOutAlpha) * (1f - (effect)), angle, noise.Size() / 2f, ((new Vector2(200f, 150f) / noisesize) * (1f + effect * 5.00f)) * scale, SpriteEffects.None, 0f);
                            continue;
                        }
                    }

                    if (count == 0)
                    {
                        spriteBatch.Draw(noise, (npc.Center + loc) - Main.screenPosition, null, (fightActive ? Color.Blue : Color.PaleTurquoise) * shieldAlpha * fadeOutAlpha, angle, noise.Size() / 2f, (new Vector2(200f, 150f) / noisesize) * scale, SpriteEffects.None, 0f);
                        spriteBatch.Draw(noise, (npc.Center + loc) - Main.screenPosition, null, (fightActive ? Color.Blue : Color.PaleTurquoise) * shieldAlpha * fadeOutAlpha, angle, noise.Size() / 2f, (-new Vector2(200f, 150f) / noisesize) * scale, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        spriteBatch.Draw(noise, (npc.Center + loc) - Main.screenPosition, null, (fightActive ? Color.White : Color.CadetBlue) * 0.40f * shieldAlpha * fadeOutAlpha, angle, noise.Size() / 2f, (new Vector2(200f, 150f) / noisesize) * scale, SpriteEffects.None, 0f);
                    }
                    count += 1;
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public void DrawEyes(SpriteBatch spriteBatch, bool glow, bool applyShader = true)
        {
            Texture2D rocklarge = mod.GetTexture("Dimensions/Space/GlowAsteriod");
            Vector2 firerockorig = rocklarge.Size() / 2f;
            Texture2D eyetex = mod.GetTexture("Doom_Harbinger_Resprite_pupil");
            Vector2 eyetexorig = eyetex.Size() / 2f;
            float percentive = (npc.ai[0] - 100000);

            Color EyeColor = Color.Lerp(Color.White, Color.Blue, MathHelper.Clamp(percentive / 250f, 0f, 1f));

            //Eyes (moved)

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            if (applyShader)
            {
                ArmorShaderData stardustsshader = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye);

                DrawData value8 = new DrawData(eyetex, new Vector2(300f, 300f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 320, 160)), Microsoft.Xna.Framework.Color.White, npc.rotation, eyetexorig, npc.scale, SpriteEffects.None, 0);
                stardustsshader.UseColor(Color.CornflowerBlue.ToVector3());
                stardustsshader.UseOpacity(0.5f);
                stardustsshader.Apply(null, new DrawData?(value8));
            }

            float scaleAlpha = glow ? 1 - fadeOutAlpha : fadeOutAlpha;
            float scale2Alpha = glow ? 1 - fadeOutAlpha : fadeOutAlpha;

            foreach (SpaceBossEye eye in Eyes)
            {
                Vector2 eyepos = Vector2.Normalize(eye.currentLook) * (eye.EyeDistance * eye.eyeMaxDistance);
                float scaleeye = MathHelper.Clamp(eye.sleep, 0f, 1f);

                if (scaleeye > 0 && !eye.exploded)
                    spriteBatch.Draw(eyetex, (eye.BasePosition) + (eye.offset + eyepos) - Main.screenPosition, null, EyeColor * scaleAlpha, 0, eyetexorig, new Vector2(1f, scaleeye) * (eye.eyeScale), SpriteEffects.None, 0f);
            }

            eyetex = mod.GetTexture("Extra_57b");
            eyetexorig = eyetex.Size() / 2f;

            foreach (SpaceBossEye eye in Eyes)
            {
                Vector2 eyepos = Vector2.Normalize(eye.currentLook) * (eye.EyeDistance * eye.eyeMaxDistance);
                float scaleeye = MathHelper.Clamp(eye.sleep, 0f, 1f);

                if (eye.customID == 1)
                {
                    float glowEye = MathHelper.Clamp((npc.ai[0] - 90) / 60f, 0f, 1f);
                    if (glowEye > 0)
                    {
                        for (float f = 0; f < 1f; f += 1 / 10f)
                        {
                            float scale2 = ((MathHelper.Clamp((float)Math.Sin(((npc.localAI[0] + (f * 60f)) * MathHelper.TwoPi) / 60), 0f, 1f))) * ((f / 2f) + 0.75f);
                            spriteBatch.Draw(eyetex, (eye.BasePosition) + (eye.offset + eyepos) - Main.screenPosition, null, EyeColor * (f / 2f) * 0.50f * scale2Alpha, 0, eyetexorig, ((eye.eyeScale * 1f) + scale2) * glowEye, SpriteEffects.None, 0);
                        }
                    }
                }

                if (scaleeye > 0 && !eye.exploded)
                    spriteBatch.Draw(eyetex, (eye.BasePosition) + (eye.offset + eyepos) - Main.screenPosition, null, EyeColor * scaleAlpha * 0.50f, 0, eyetexorig, new Vector2(0.75f, 0.75f * scaleeye) * (eye.eyeScale * Main.essScale), SpriteEffects.None, 0f);
            }

        }

        public void DrawThroughFog(SpriteBatch spriteBatch)
        {
            if (fadeOutAlpha < 1)
            {
                float percentive = (npc.ai[0] - 100000);

                Texture2D glowTex = mod.GetTexture("GlowOrb");

                Color EyeColor = Color.CornflowerBlue;

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                ArmorShaderData stardustsshader = GameShaders.Armor.GetShaderFromItemId(ItemID.LivingOceanDye);

                DrawData value8 = new DrawData(glowTex, new Vector2(64f, 64f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, glowTex.Width, glowTex.Height)), Microsoft.Xna.Framework.Color.White, npc.rotation, glowTex.Size() / 2f, npc.scale * 5f, SpriteEffects.None, 0);
                stardustsshader.UseColor(Color.CornflowerBlue.ToVector3() * 1f);
                stardustsshader.UseOpacity(0.5f);
                stardustsshader.Apply(null, new DrawData?(value8));

                for (float f2 = 5; f2 > 1; f2 -= 2f)
                {
                    for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 4f)
                    {
                        spriteBatch.Draw(glowTex, npc.Center - Main.screenPosition, new Rectangle(0, 0, glowTex.Width / 2, glowTex.Height), EyeColor * (1f / f2) * (1 - fadeOutAlpha) * 2f, f, glowTex.Size() / 2f, new Vector2(0.05f, 0.15f) * f2, SpriteEffects.None, 0);
                    }
                }
                //spriteBatch.Draw(glowTex, npc.Center - Main.screenPosition, null, EyeColor * (1 - fadeOutAlpha) * 0.95f, 0, glowTex.Size() / 2f, new Vector2(1f, 1f), SpriteEffects.None, 0);
                //spriteBatch.Draw(glowTex, npc.Center - Main.screenPosition, new Rectangle(0, 0, glowTex.Width, glowTex.Height/2), EyeColor * (1 - fadeOutAlpha) * 0.95f, 0, glowTex.Size() / 2f, new Vector2(3f, 6f), SpriteEffects.None, 0);



                //Save for later:
                /*

				Texture2D glowTex = mod.GetTexture("GlowOrb");
				ArmorShaderData stardustsshader = GameShaders.Armor.GetShaderFromItemId(ItemID.VortexDye);

				DrawData value8 = new DrawData(glowTex, new Vector2(30f, 30f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64)), Microsoft.Xna.Framework.Color.White, npc.rotation, glowTex.Size()/2f, npc.scale*5f, SpriteEffects.None, 0);
				stardustsshader.UseColor(Color.CornflowerBlue.ToVector3()*2f);
				stardustsshader.UseOpacity(0.15f);
				stardustsshader.Apply(null, new DrawData?(value8));

				for (float f2 = 1; f2 < 3; f2 += 1f)
				{
					for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 24)
					{
						spriteBatch.Draw(glowTex, npc.Center - Main.screenPosition, new Rectangle(0, 0, glowTex.Width / 2, glowTex.Height), EyeColor * (1f / f2) * (1 - fadeOutAlpha) * 0.95f, MathHelper.PiOver4 + f, glowTex.Size() / 2f, new Vector2(2f, 1f) * f2, SpriteEffects.None, 0);
					}
				}
				//spriteBatch.Draw(glowTex2, npc.Center - Main.screenPosition, null, EyeColor, 0, glowTex2.Size() / 2f, new Vector2(8f, 8f), SpriteEffects.None, 0);

				*/

                DrawEyes(spriteBatch, true);
            }
        }
    }

    public class SpaceBossBeam : HellionBeam, INonDestructableProjectile
    {

        public int timeWarning = 150;
        protected float scale3 = 0f;

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 200;
            projectile.damage = 15;
            projectile.width = 64;
            projectile.height = 64;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Beam");
        }

        public override string Texture
        {
            get { return "Terraria/Item_" + ItemID.GeyserTrap; }
        }

        public override void AI()
        {
            projectile.ai[1]++;


            scale2 += 1f / (float)timeWarning;


            if (projectile.ai[1] > timeWarning)
                scale3 = Math.Min(scale3 + 0.15f, 1.5f);


            projectile.ai[1] += 1;
            if ((int)projectile.ai[1] == timeWarning)
            {
                SoundEffectInstance beamsnd = Main.PlaySound(SoundID.DD2_BetsyFlameBreath, (int)projectile.Center.X, (int)projectile.Center.Y);
            }

            projectile.localAI[0] += 0.2f;

            base.AI();
        }

        public override bool CanDamage()
        {
            if (projectile.ai[1] < timeWarning)
                return false;

            return true;
        }

        public override void MoreAI(Vector2 dustspot)
        {
            SGAmod.updatelasers = false;
        }
        public override void OnHitPlayer(Player player, int damage, bool crit)
        {

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            Vector2 scale = new Vector2(MathHelper.Clamp((float)projectile.timeLeft / 20, 0f, 1f) * scale2, 1f);
            Vector2 scalez2 = new Vector2(MathHelper.Clamp((float)projectile.timeLeft / 60, 0f, 1f) * scale2, 1f);
            Vector2 scalez3 = new Vector2(MathHelper.Clamp((float)projectile.timeLeft / 30, 0f, 1f) * scale3, 1f);

            List<Vector2> vectors = new List<Vector2>();
            vectors.Add(hitspot);
            vectors.Add(projectile.Center);

            TrailHelper trail = new TrailHelper("FadedBasicEffectPass", mod.GetTexture("Space"));
            trail.projsize = Vector2.Zero;
            trail.coordOffset = new Vector2(0, -Main.GlobalTime * 1f);
            trail.coordMultiplier = new Vector2(1f, 60f);
            trail.doFade = false;
            trail.trailThickness = 64 * scale.X * (projectile.width / 64f);
            trail.color = delegate (float percent)
            {
                return Color.CornflowerBlue * scalez2.X * 1.0f;
            };
            trail.trailThicknessIncrease = 0;
            trail.DrawTrail(vectors, projectile.Center);

            trail = new TrailHelper("FadedBasicEffectPass", mod.GetTexture("TiledPerlin"));
            trail.projsize = Vector2.Zero;
            trail.coordOffset = new Vector2(0, -Main.GlobalTime * 1f);
            trail.coordMultiplier = new Vector2(1f, 60f);
            trail.doFade = false;
            trail.trailThickness = 8 * scale.X * (projectile.width / 64f);
            trail.color = delegate (float percent)
            {
                return Color.CornflowerBlue * MathHelper.Clamp(scalez2.X * 2.5f, 0f, 0.75f);
            };
            trail.trailThicknessIncrease = 0;
            trail.DrawTrail(vectors, projectile.Center);

            if (scalez3.X > 0)
            {

                trail = new TrailHelper("FadedBasicEffectPass", mod.GetTexture("Stain"));
                trail.projsize = Vector2.Zero;
                trail.coordOffset = new Vector2(0, -Main.GlobalTime * 3f);
                trail.coordMultiplier = new Vector2(1f, 80f);
                trail.doFade = false;
                trail.trailThickness = 24 * scalez3.X * (projectile.width / 64f);
                trail.color = delegate (float percent)
                {
                    return Color.CornflowerBlue * scalez3.X;
                };
                trail.trailThicknessIncrease = 0;
                trail.DrawTrail(vectors, projectile.Center);

            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


            Texture2D glowtex = ModContent.GetTexture("SGAmod/GlowOrb");

            Main.spriteBatch.Draw(glowtex, projectile.Center - Main.screenPosition, null, Color.CornflowerBlue * Math.Min(scale.X * 4f, 1f), (projectile.velocity).ToRotation() - ((float)Math.PI / 2f), glowtex.Size() / 2f, ((Vector2.One * 0.25f) + Vector2.One * (0.15f * scale.X * (projectile.width / 64f))) * MathHelper.Clamp(projectile.timeLeft / 30f, 0f, 1f), SpriteEffects.None, 0);

            glowtex = ModContent.GetTexture("SGAmod/Glow");

            Main.spriteBatch.Draw(glowtex, projectile.Center - Main.screenPosition, null, Color.White * Math.Min(scalez3.X * 4f, 1f), (projectile.velocity).ToRotation() - ((float)Math.PI / 2f), glowtex.Size() / 2f, ((Vector2.One * 1f) + Vector2.One * (1f * scalez3.X * (projectile.width / 64f)) * MathHelper.Clamp(projectile.timeLeft / 30f, 0f, 1f)), SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


            //Idglib.DrawTether(lasers[(int)projectile.localAI[0] % 3], hitspot, projectile.Center, projectile.Opacity* Math.Min(scale2,1f), scale.X, scale.Y, Color.White);
            //Texture2D captex = ModContent.GetTexture("SGAmod/NPCs/Hellion/end_and_start");
            //Main.spriteBatch.Draw(captex, projectile.Center - Main.screenPosition, null, Color.White * Math.Min(scale2,1f), (projectile.velocity).ToRotation() - ((float)Math.PI / 2f), new Vector2(captex.Width / 2, captex.Height / 2), new Vector2(scale.X, scale.Y), SpriteEffects.None, 0.0f);
            //Main.spriteBatch.Draw(captex, hitspot - Main.screenPosition, null, Color.White * Math.Min(scale2,1f), projectile.velocity.ToRotation() + ((float)Math.PI / 2f), new Vector2(captex.Width / 2, captex.Height / 2), new Vector2(scale.X, scale.Y), SpriteEffects.None, 0.0f);

            return false;
        }

    }

    public class SpaceBossExplode : ModProjectile
    {
        protected float timeleft = 150f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SpaceBoss is dying animation");
        }
        public override string Texture
        {
            get { return "SGAmod/Items/Weapons/Ammo/AcidRocket"; }
        }
        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
            projectile.width = 16;
            projectile.height = 16;
            projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 420;
            projectile.extraUpdates = 0;
            aiType = -1;
            projectile.aiStyle = -1;
        }

        public override void AI()
        {
            projectile.ai[0] += 1;
            projectile.localAI[0] += 0.20f;
            if (projectile.ai[0] > 300)
            {
                projectile.localAI[0] += 4f;
            }
            if (projectile.ai[0] == 60)
            {
                ScreenExplosion explode = SGAmod.AddScreenExplosion(projectile.Center, 340, 2f, 1600);
                if (explode != null)
                {
                    explode.warmupTime = 64;
                    explode.decayTime = 64;
                    explode.strengthBasedOnPercent = delegate (float percent)
                    {
                        return 2f + MathHelper.Clamp((percent - 0.75f) * 2f, 0f, 1f) * 1f;
                    };
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.glowMaskTexture[239];
            //if (projectile.ai[0] < 120)
            //{

            /*for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi/16f)
			{
				float there = f-Main.GlobalTime/10f;
				spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, null, (Color.Pink* 0.25f) * MathHelper.Clamp((projectile.timeLeft-60) / 60f, 0f, projectile.ai[0] / 800f), there, texture2.Size() / 2f, new Vector2(4f, 4f)+ (new Vector2(32f, 32f)/(1+(projectile.ai[0] / 300f))), SpriteEffects.None, 0f);
			}*/

            VertexBuffer vertexBuffer;
            Effect effect = SGAmod.TrailEffect;

            effect.Parameters["WorldViewProjection"].SetValue(WVP.View(Main.GameViewMatrix.Zoom) * WVP.Projection());
            effect.Parameters["imageTexture"].SetValue(SGAmod.Instance.GetTexture("Space"));
            effect.Parameters["coordOffset"].SetValue(0);
            effect.Parameters["coordMultiplier"].SetValue(4f);
            effect.Parameters["strength"].SetValue(MathHelper.Clamp((projectile.timeLeft - 30) / 80f, 0f, projectile.ai[0] / 250f));

            float adder = projectile.localAI[0];//MathHelper.Clamp((projectile.ai[0] - 300f)/1f, 0f, 300f);

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[6];

            Vector3 screenPos = (projectile.Center - Main.screenPosition).ToVector3();
            float size = projectile.localAI[0] * 3f;

            vertices[0] = new VertexPositionColorTexture(screenPos + new Vector3(-size, -size, 0), Color.White, new Vector2(0, 0));
            vertices[1] = new VertexPositionColorTexture(screenPos + new Vector3(-size, size, 0), Color.White, new Vector2(0, 1));
            vertices[2] = new VertexPositionColorTexture(screenPos + new Vector3(size, -size, 0), Color.White, new Vector2(1, 0));

            vertices[3] = new VertexPositionColorTexture(screenPos + new Vector3(size, size, 0), Color.White, new Vector2(1, 1));
            vertices[4] = new VertexPositionColorTexture(screenPos + new Vector3(-size, size, 0), Color.White, new Vector2(0, 1));
            vertices[5] = new VertexPositionColorTexture(screenPos + new Vector3(size, -size, 0), Color.White, new Vector2(1, 0));

            vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

            Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

            effect.CurrentTechnique.Passes["DefaultPassSinShade"].Apply();

            Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 5f)
            {
                float there = f + Main.GlobalTime / 10f;
                spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, (Color.White * 0.20f) * MathHelper.Clamp(projectile.timeLeft / 120f, 0f, MathHelper.Clamp(projectile.ai[0] / 180f, 0f, 1f)), there, texture.Size() / 2f, new Vector2(0.85f, 1f) * ((Math.Abs(20f - ((projectile.ai[0]) / 300f) * 20f)) + (adder / 60f)), SpriteEffects.None, 0f);
            }

            //}

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
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


    public class MineableAsteriodBossLock : MineableAsteriod, IMineableAsteriod
    {
        public int[] gems;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Asteriod Boss Lock");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            asteriodHealth = 750;
            splitting = false;
            rotateAngle = 0;
            glowColor = Color.CornflowerBlue;
            projectile.width = 8;
            projectile.height = 8;
            projectile.tileCollide = true;
            projectile.timeLeft = int.MaxValue - 1;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity != Vector2.Zero)
            {
                SoundEffectInstance snd = Main.PlaySound(SoundID.DD2_CrystalCartImpact, (int)projectile.Center.X, (int)projectile.Center.Y);
                if (snd != null)
                {
                    snd.Pitch = 0.50f;
                }
                projectile.velocity = Vector2.Zero;
            }
            return false;
        }

        public override void AI()
        {
            base.AI();
        }

        protected override void _AsteriodLoot()
        {
            base._AsteriodLoot();
        }
        protected override void _DrawAsteriod(SpriteBatch spriteBatch, Color lightColor)
        {
            lightColor = Lighting.GetColor((int)projectile.Center.X >> 4, (int)projectile.Center.Y >> 4);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D turrettex = mod.GetTexture(!SpaceDim.SpaceBossIsActive ? "Dimensions/Space/Tether" : "Dimensions/Space/TetherFancy");
            Texture2D turrettexGlow = mod.GetTexture(!SpaceDim.SpaceBossIsActive ? "Dimensions/Space/Tether_Glow" : "Dimensions/Space/TetherFancy_Glow");

            Vector2 vec = new Vector2(turrettex.Width, turrettex.Height * 0.90f) / 2f;
            Vector2 drawPos = projectile.Center - Main.screenPosition;

            UnifiedRandom rand = new UnifiedRandom(projectile.whoAmI);

            Vector2 offset = -Vector2.UnitY;

            spriteBatch.Draw(turrettex, drawPos + (offset * projectile.width * (rand.NextFloat(0.65f, 0.85f))).RotatedBy(projectile.rotation), new Rectangle(0, 0, turrettex.Width, turrettex.Height / 2), lightColor, projectile.rotation, vec, projectile.scale, SpriteEffects.None, 0f);

            ArmorShaderData stardustsshader = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye);
            DrawData value8 = new DrawData(turrettex, new Vector2(640, 640), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 640, 640)), Microsoft.Xna.Framework.Color.White, projectile.rotation, vec, projectile.scale, SpriteEffects.None, 0);
            stardustsshader.UseColor(Color.CornflowerBlue.ToVector3());
            stardustsshader.UseOpacity(1f);
            stardustsshader.Apply(null, new DrawData?(value8));

            spriteBatch.Draw(turrettexGlow, drawPos + (offset * projectile.width * (rand.NextFloat(0.65f, 0.85f))).RotatedBy(projectile.rotation), new Rectangle(0, 0, turrettex.Width, turrettex.Height / 2), Color.LightGray, projectile.rotation, vec, projectile.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(turrettexGlow, drawPos + (offset * projectile.width * (rand.NextFloat(0.65f, 0.85f))).RotatedBy(projectile.rotation), new Rectangle(0, turrettex.Height / 2, turrettex.Width, turrettex.Height / 2), Color.LightGray * MathHelper.Clamp((float)(0.3f + Math.Sin((Main.GlobalTime * 1.2f) + rand.NextFloat(MathHelper.TwoPi)) * 0.52f), 0f, 1f), projectile.rotation, vec, projectile.scale, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            base._DrawAsteriod(spriteBatch, lightColor);

        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //stuff
        }
    }

    public class SpaceBossHomingShot : ModProjectile
    {
        int extraparticles => 0;
        Effect effect => SGAmod.TrailEffect;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overseer's Shot");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 60;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override string Texture => "Terraria/Projectile_538";

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.alpha = 40;
            projectile.timeLeft = 500;
            projectile.light = 0.75f;
            projectile.extraUpdates = 0;
            projectile.ignoreWater = true;
        }

        public override bool PreKill(int timeLeft)
        {
            SoundEffectInstance snd = Main.PlaySound(SoundID.NPCKilled, (int)projectile.Center.X, (int)projectile.Center.Y, 7);

            if (snd != null)
            {
                snd.Pitch = Main.rand.NextFloat(-0.75f, -0.25f);
            }

            for (float num475 = -6 - extraparticles; num475 < 6 + extraparticles; num475 += 0.3f)
            {
                float anglehalf = (float)(((double)projectile.velocity.ToRotation()) + 2.0 * Math.PI);
                Vector2 startloc2 = projectile.velocity;
                startloc2.Normalize();
                Vector2 startloc = (projectile.Center + (startloc2 * 12f));
                int dust = Dust.NewDust(new Vector2(startloc.X, startloc.Y), 0, 0, DustID.BlueCrystalShard);

                float anglehalf2 = anglehalf + ((float)Math.PI / 2f);
                Main.dust[dust].position += anglehalf2.ToRotationVector2() * (float)((Main.rand.Next(-200, 200) / 10f));

                Main.dust[dust].scale = 2f - Math.Abs(num475) / 4f;
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                Main.dust[dust].velocity = (randomcircle / 3f);
                Main.dust[dust].velocity += (projectile.velocity * (num475 / 5f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].rotation = Main.dust[dust].velocity.ToRotation();
            }

            return true;
        }

        public override void AI()
        {
            projectile.localAI[0] += 1;
            float homing = MathHelper.Clamp((projectile.localAI[0]) / 300, 0, 0.05f) * MathHelper.Clamp((projectile.timeLeft - 200) / 160f, 0f, 1f);
            if (!Main.expertMode)
                homing = MathHelper.Clamp((projectile.localAI[0]) / 300, 0, 0.05f) * MathHelper.Clamp((projectile.timeLeft - 500) / 200f, 0f, 1f);

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y * projectile.spriteDirection, (double)projectile.velocity.X * projectile.spriteDirection) + 1.57f;

            base.AI();
            if (projectile.ai[0] <= 0)
            {
                projectile.ai[0] = (projectile.velocity).Length();
                SoundEffectInstance died = Main.PlaySound(SoundID.DD2_PhantomPhoenixShot, (int)projectile.Center.X, (int)projectile.Center.Y);

                if (died != null)
                {
                    died.Pitch = -0.8f;
                }
            }

            if (projectile.ai[0] < 15f)
            {
                projectile.ai[0] += 0.05f;
            }

            projectile.spriteDirection = (projectile.velocity.X > 0).ToDirectionInt();

            int target2 = Player.FindClosest(projectile.Center, projectile.width, projectile.height);
            if (target2 >= 0)
            {
                Player target = Main.player[target2];
                if ((target.Center - projectile.Center).Length() < 5000f)
                {
                    projectile.velocity = projectile.velocity + (projectile.DirectionTo(target.Center) * ((float)projectile.ai[0] * homing));
                    projectile.velocity = Vector2.Normalize(projectile.velocity) * (float)projectile.ai[0];
                }
            }

            if (Main.rand.Next(0, 200) < projectile.localAI[0])
            {
                Vector2 offset = Vector2.Normalize(projectile.velocity.RotatedByRandom(MathHelper.Pi / 12f)).RotatedBy(MathHelper.Pi);
                Vector2 offset2 = Vector2.Normalize(projectile.velocity.RotatedByRandom(MathHelper.PiOver2));
                int dustType = Main.rand.Next(100) < 25 ? ModContent.DustType<LeviDust>() : DustID.BlueCrystalShard;
                int dust = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) + offset2 * Main.rand.NextFloat(-12, 12), 0, 0, dustType);
                Main.dust[dust].scale = 1f;
                Main.dust[dust].alpha = 40;
                Main.dust[dust].rotation = offset.ToRotation();
                Main.dust[dust].velocity = Vector2.Normalize(offset) * (float)(1f * Main.rand.NextFloat(0f, 3f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].color = Color.White;
            }

            SGAmod.PostDraw.Add(new PostDrawCollection(new Vector3(projectile.Center.X, projectile.Center.Y, 48)));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            bool inrange = false;
            int bufferRange = 64;

            for (int i = 0; i < projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
            {
                //Main.NewText(projectile.oldPos[i]);
                if (projectile.localAI[0] < 2)
                    projectile.oldPos[i] = projectile.position;

                if (i % 5 == 0)
                {
                    Rectangle rect = new Rectangle((int)Main.screenPosition.X - bufferRange, (int)Main.screenPosition.Y - bufferRange, Main.screenWidth + bufferRange, Main.screenHeight + bufferRange);
                    if (rect.Contains(projectile.oldPos[i].ToPoint()))
                    {
                        inrange = true;
                        break;
                    }
                }
            }
            if (!inrange)
                return false;

            float alphaFade = MathHelper.Clamp(projectile.timeLeft / 120f, 0f, 1f);
            float alphaFade2 = alphaFade * MathHelper.Clamp(projectile.ai[0] / 8f, 0f, 1f);

            int maxLength = (int)(Math.Min((projectile.localAI[0] / 75f), 1f) * projectile.oldPos.Length);

            List<Vector2> positions = new List<Vector2>();
            for (int i = 0; i < maxLength; i += 1)//dumb hack to get the trails to not appear at 0,0
            {
                if (projectile.localAI[0] < 2)
                    projectile.oldPos[i] = projectile.position;

                positions.Add(projectile.oldPos[i].X == 0 ? projectile.oldPos[maxLength - 1] : projectile.oldPos[i]);
            }

            TrailHelper trail = new TrailHelper("FadedBasicEffectPass", SGAmod.Instance.GetTexture("TiledPerlin"));
            trail.coordMultiplier = new Vector2(0.25f, 0.20f * projectile.velocity.Length() * (maxLength / (float)projectile.oldPos.Length));
            trail.coordOffset = new Vector2(0, Main.GlobalTime * -2f);
            trail.projsize = projectile.Hitbox.Size() / 2f;
            trail.trailThickness = 12;
            trail.trailThicknessIncrease = 5;
            trail.color = delegate (float percent)
            {
                return Color.Lerp(Color.CornflowerBlue, Color.CadetBlue, percent) * alphaFade2;
            };

            trail.DrawTrail(positions, projectile.Center);



            Texture2D texture = Main.projectileTexture[mod.ProjectileType(this.GetType().Name)];
            Vector2 origin = texture.Size() / 2f;

            spriteBatch.Draw(texture, projectile.Center + new Vector2(1, 0) - Main.screenPosition, null, Color.White * alphaFade * 0.75f, 0, origin, new Vector2(1f, 1f) * 0.5f, projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            spriteBatch.Draw(texture, projectile.Center + new Vector2(1, 0) - Main.screenPosition, null, Color.White * alphaFade * 1f, projectile.localAI[0] / 20f, origin, new Vector2(1.25f, 1.25f), projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            spriteBatch.Draw(texture, projectile.Center + new Vector2(1, 0) - Main.screenPosition, null, Color.White * alphaFade, -projectile.localAI[0] / 20f, origin, new Vector2(1.5f, 1.5f), projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }

    }

    public class SpaceBossTelegraphedBasicShot : SpaceBossBasicShot, IDrawAdditive, IDrawThroughFog
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overseer's Shot");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 120;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.alpha = 40;
            projectile.timeLeft = 1000;
            projectile.light = 0.75f;
            projectile.extraUpdates = 5;
            projectile.ignoreWater = true;
        }

        public void DrawThroughFog(SpriteBatch spriteBatch)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            DrawAdditiveThroughFog(spriteBatch);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public void DrawAdditiveThroughFog(SpriteBatch spriteBatch)
        {
            Texture2D tex = Main.blackTileTexture;

            if (projectile.localAI[0] < 90)
            {
                float alphaFade = 1f - MathHelper.Clamp((projectile.localAI[0] - 50f) / 40f, 0f, 1f);

                for (float f = 3f; f > 0.40f; f -= 0.50f)
                {
                    spriteBatch.Draw(tex, startOrg - Main.screenPosition, new Rectangle(0, 0, 2, 2), Color.Turquoise * alphaFade * 0.50f, projectile.velocity.ToRotation(), new Vector2(0, 1f), new Vector2(projectile.localAI[0] * 20f * f, (2f * f) + (1f - alphaFade) * 9f), SpriteEffects.None, 0f);
                }
            }
        }
    }

    public class SpaceBossBasicShot : SpaceBossHomingShot, IDrawAdditive
    {
        public virtual bool DrawFlash => false;
        int extraparticles => 0;
        protected virtual Color BoomColor => Color.Blue;
        protected Vector2 startOrg = default;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overseer's Shot");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 120;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override string Texture => "Terraria/Projectile_538";

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.alpha = 40;
            projectile.timeLeft = 1200;
            projectile.light = 0.75f;
            projectile.extraUpdates = 5;
            projectile.ignoreWater = true;
        }

        public override bool PreKill(int timeLeft)
        {
            SoundEffectInstance snd = Main.PlaySound(SoundID.NPCKilled, (int)projectile.Center.X, (int)projectile.Center.Y, 7);

            if (snd != null)
            {
                snd.Pitch = Main.rand.NextFloat(-0.75f, -0.25f);
            }

            for (float num475 = -6 - extraparticles; num475 < 6 + extraparticles; num475 += 0.3f)
            {
                float anglehalf = (float)(((double)projectile.velocity.ToRotation()) + 2.0 * Math.PI);
                Vector2 startloc2 = projectile.velocity;
                startloc2.Normalize();
                Vector2 startloc = (projectile.Center + (startloc2 * 12f));
                int dust = Dust.NewDust(new Vector2(startloc.X, startloc.Y), 0, 0, 185);

                float anglehalf2 = anglehalf + ((float)Math.PI / 2f);
                Main.dust[dust].position += anglehalf2.ToRotationVector2() * (float)((Main.rand.Next(-200, 200) / 10f));

                Main.dust[dust].scale = 2f - Math.Abs(num475) / 4f;
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                Main.dust[dust].velocity = (randomcircle / 3f);
                Main.dust[dust].velocity += (projectile.velocity * (num475 / 1f));
                Main.dust[dust].noGravity = true;
            }

            return true;
        }

        public override void AI()
        {
            projectile.localAI[0] += 1;

            if (projectile.localAI[0] == 1)
            {
                startOrg = projectile.Center;
            }

            if (projectile.ai[0] < 15f)
            {
                projectile.ai[0] += 0.05f;
            }

            Vector2 offset = Vector2.Normalize(projectile.velocity.RotatedByRandom(MathHelper.Pi / 12f)).RotatedBy(MathHelper.Pi);
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.BlueCrystalShard);
            Main.dust[dust].scale = 0.5f;
            Main.dust[dust].alpha = 150;
            Main.dust[dust].velocity = Vector2.Normalize(offset) * (float)(1f * Main.rand.NextFloat(0f, 3f));
            Main.dust[dust].noGravity = true;

        }

        public virtual void DrawAdditiveReal(SpriteBatch spriteBatch)
        {

        }

        public void DrawAdditive(SpriteBatch spriteBatch)
        {
            DrawAdditiveReal(spriteBatch);
            if (GetType() == typeof(SpaceBossTelegraphedBasicShot) && SGAmod.fogDrawNPCsCounter<1)
            {
                (this as SpaceBossTelegraphedBasicShot).DrawAdditiveThroughFog(spriteBatch);

            }

            if (!DrawFlash)
                return;

            Texture2D glow = ModContent.GetTexture("SGAmod/LightBeam");
            Vector2 origin = new Vector2(glow.Width / 2, glow.Height * 0.15f);
            float rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;

            float alphaFade = MathHelper.Clamp(projectile.timeLeft / 60f, 0f, 1f) * MathHelper.Clamp(projectile.localAI[0] / 20f, 0f, 1f - MathHelper.Clamp((projectile.localAI[0] - 80f) / 50f, 0f, 1f));
            if (alphaFade <= 0)
                return;

            for (float f = -MathHelper.Pi; f <= MathHelper.Pi; f += MathHelper.Pi / 6f)
            {
                float scaleColor = (1f - Math.Abs(f / MathHelper.TwoPi)) * 1f;
                float anglef = f * ((projectile.localAI[0] - 40) / 20f);
                Vector2 scale = new Vector2(alphaFade * 0.20f, MathHelper.SmoothStep(0f, 1f, projectile.localAI[0] / 80f) * scaleColor * 1.50f);
                float colorScale = MathHelper.Clamp(((projectile.localAI[0] - 40) - Math.Abs(f * 4f)) / 60f, 0f, 1f);
                spriteBatch.Draw(glow, startOrg - Main.screenPosition, null, Color.Lerp(Color.White, BoomColor, colorScale) * alphaFade * scaleColor * 2f, rotation + (anglef / 3f), origin, scale * 0.40f, projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            }

            spriteBatch.Draw(glow, startOrg - Main.screenPosition, null, BoomColor * alphaFade * 2f, rotation, origin, new Vector2(0.20f + alphaFade, MathHelper.SmoothStep(0f, 2f, projectile.localAI[0] / 320f) * 6f) * 0.40f, projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            spriteBatch.Draw(glow, startOrg - Main.screenPosition, null, Color.White * alphaFade * 2f, rotation, origin, new Vector2(0.05f + alphaFade * 0.75f, MathHelper.SmoothStep(0f, 2f, projectile.localAI[0] / 320f) * 6f) * 0.30f, projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            float alphaFade = MathHelper.Clamp(projectile.timeLeft / 60f, 0f, 1f);

            Texture2D texture = Main.projectileTexture[mod.ProjectileType(this.GetType().Name)];
            Vector2 origin = texture.Size() / 2f;

            int index = 0;
            foreach (Vector2 pos in projectile.oldPos.Reverse())
            {
                float indexPercent = (index / (float)projectile.oldPos.Length);
                spriteBatch.Draw(texture, pos + (projectile.Hitbox.Size() * 0.50f) - Main.screenPosition, null, Color.White * alphaFade * 0.50f * indexPercent, 0, origin, new Vector2(1f, 1f) * 1f * indexPercent, projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
                index += 1;
            }
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White * alphaFade * 1f, 0, origin, 1f, projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);

            return false;
        }

    }

    public class SpaceBossBasicShotAccelerate : SpaceBossBasicShot, IDrawAdditive
    {
        public override bool DrawFlash => true;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overseer's Accelerating Shot");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 120;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override string Texture => "Terraria/Projectile_538";

        public override void AI()
        {
            base.AI();

            projectile.localAI[1] += 1;
            projectile.position -= projectile.velocity * (1f - MathHelper.Clamp(projectile.localAI[1] / 150f, 0f, 1f));

        }
    }

    public class OverseenHeadBossShield : OverseenHead
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overseen Watcher");
        }

        int MaxTime
        {
            get
            {
                int npc = NPC.FindFirstNPC(ModContent.NPCType<SpaceBoss>());

                if (npc >= 0)
                {
                    NPC npc2 = Main.npc[npc];

                    if (npc2.active)
                    {
                        SpaceBoss boss = npc2.modNPC as SpaceBoss;
                        return 300 + ((boss.phase - 1) * 250);
                    }

                }
                return 300;

            }
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.life = 100000;
            npc.lifeMax = 100000;
            npc.dontTakeDamage = true;
            npc.damage = 0;
            npc.aiStyle = -1;
        }

        public override void AI()
        {
            npc.ai[2] += 1;
            if (npc.localAI[3] == 0)
            {
                npc.Opacity = 0f;
                npc.ai[1] = Main.rand.NextFloat(MathHelper.TwoPi);
                npc.localAI[2] = Main.rand.NextFloat(MathHelper.TwoPi);
                npc.localAI[3] = 1;
                npc.netUpdate = true;
            }

            npc.localAI[0] += 1;
            NPC[] bossg = Main.npc.Where(testby => testby.active && testby.type == ModContent.NPCType<SpaceBoss>()).ToArray();

            SpaceBoss boss = null;

            if (bossg.Length > 0)
                boss = (SpaceBoss)(bossg[0].modNPC);

            Player target = Main.LocalPlayer;// Main.player[boss.npc.target];

            if (boss == null || !boss.npc.active || boss.npc.life < 1 || boss.npc.type != ModContent.NPCType<SpaceBoss>() || boss.npc.life >= boss.npc.lifeMax || (boss.shieldeffect <= 0.05 && npc.ai[2] > 2))
            {
                npc.StrikeNPCNoInteraction(npc.lifeMax * 3, 0, 0);
            }

            if (npc.localAI[0] < 0 || npc.ai[2] < 0)
                return;


            npc.Opacity = MathHelper.Clamp(npc.Opacity + 0.05f, 0f, 1f);

            //AI type 1
            if (npc.ai[0] < 2)
            {
                npc.Center = target.Center + Vector2.UnitX.RotatedBy(npc.localAI[2]) * (180f + npc.ai[3]);

                if (npc.ai[2] % MaxTime < 200)
                {
                    int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + (Vector2.UnitX.RotatedBy(npc.rotation) * -8f) + Main.rand.NextVector2Circular(4f, 4f), 0, 0, DustID.BlueCrystalShard);
                    Main.dust[dust].scale = (npc.ai[2] % MaxTime) / 200f;
                    Main.dust[dust].velocity = Vector2.UnitX.RotatedBy(npc.rotation) * -((((npc.ai[2] % MaxTime) / 200f) * 2f) + 4f);
                    Main.dust[dust].noGravity = true;
                }

                if (npc.ai[2] % MaxTime == 200)
                {

                    /*if (npc.ai[0] == 1)
					{
						Projectile.NewProjectile(npc.Center, (npc.rotation + MathHelper.Pi).ToRotationVector2() * 5f, ModContent.ProjectileType<SpaceBossBasicShotAccelerate>(), 30, 10f);
					}*/

                    if (npc.ai[0] == 0)
                    {
                        Projectile.NewProjectile(npc.Center, (npc.ai[1] + MathHelper.Pi).ToRotationVector2() * 5f, ModContent.ProjectileType<SpaceBossBasicShotAccelerate>(), 30, 10f);
                    }

                    SoundEffectInstance snd = Main.PlaySound(SoundID.NPCKilled, (int)npc.Center.X, (int)npc.Center.Y, 7);

                    if (snd != null)
                    {
                        snd.Pitch = Main.rand.NextFloat(0.25f, 0.50f);
                    }

                    npc.netUpdate = true;
                }

                if (npc.ai[2] % MaxTime == 299 && npc.ai[0] == 0)
                {
                    npc.ai[1] = Main.rand.NextFloat(MathHelper.TwoPi);
                    npc.netUpdate = true;
                }

                if (npc.ai[0] == 1)
                {
                    if (target.velocity.Length() > 0)
                        npc.ai[1] = target.velocity.ToRotation();
                    npc.netUpdate = true;
                }

                npc.localAI[2] = npc.localAI[2].AngleLerp(npc.ai[1], npc.ai[0] == 0 ? 0.02f : 0.01f + (npc.ai[3] / 10000f));
                npc.rotation = npc.localAI[2];



            }
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            base.PreDraw(spriteBatch, drawColor);

            float scaleeffect = 1f - (Math.Abs(((npc.ai[2]) % MaxTime) - 200f) / 90f);

            if (scaleeffect > 0)
            {

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                spriteBatch.Draw(Main.extraTexture[60], npc.Center - Main.screenPosition, null, Color.Aqua * MathHelper.Clamp(scaleeffect, 0f, 0.75f), npc.rotation - MathHelper.PiOver2, (Main.extraTexture[60].Size() / 2f) + new Vector2(0, 11), new Vector2(0.25f, 10f * scaleeffect), SpriteEffects.None, 0f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            }

            return false;
        }

    }

    public class SpaceBossShadowNebulaEnemy : OverseenHead, IDrawThroughFog
    {
        public SpaceBossRock spawnedFrom;
        public SpaceBossShadowNebulaEnemy masterBeforeMe;
        public Vector2 emptySpace;
        public bool Vulneverable => (masterBeforeMe == null || (masterBeforeMe.npc.ai[3] > 0 && masterBeforeMe.npc.life < 5));

        public List<(Vector2, int)> trailPositions = new List<(Vector2, int)>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Constellus Orbs");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.defense = 20;
            npc.life = 2500;
            npc.lifeMax = 2500;
            npc.width = 64;
            npc.height = 64;
            npc.damage = 0;
            timer = 5;
            npc.aiStyle = -1;
        }

        public override bool CheckDead()
        {
            //if (npc.ai[3] < -200)
            //	return true;

            if (npc.ai[3] < 1)
            {
                npc.ai[3] = 1;
                npc.life = 1;
                npc.dontTakeDamage = true;
            }

            npc.life = 1;
            npc.netUpdate = true;

            return false;

        }

        public override void HitEffect(int hitDirection, double damage)
        {
            SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_WitherBeastHurt, (int)npc.Center.X, (int)npc.Center.Y);
            if (sound != null)
            {
                float hpLeft = npc.life / (float)npc.lifeMax;
                sound.Pitch = MathHelper.Clamp((-0.75f + (hpLeft * 1.25f)) + Main.rand.NextFloat(0.1f, 0.1f), -0.80f, 0.80f);
            }
        }

        public void FizzleOut()
        {
            if (npc.ai[2] < 59)
            {
                for (float num475 = 0; num475 < npc.ai[2]/10f; num475 += 0.1f)
                {
                    Vector2 startloc = (npc.Center);
                    int dust = Dust.NewDust(new Vector2(startloc.X, startloc.Y), 0, 0, 185);

                    float anglehalf2 = Main.rand.NextFloat(MathHelper.TwoPi);

                    Main.dust[dust].scale = (3f - Math.Abs(num475) / 25f)*0.5f;
                    Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                    Main.dust[dust].velocity = (randomcircle * (1f+num475))*MathHelper.Clamp(npc.ai[2]/60f,0f,1f);
                    Main.dust[dust].noGravity = true;
                }
            }
        }

        public override void AI()
        {
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;

            npc.localAI[0] += 1;

            npc.localAI[1] = MathHelper.Clamp(npc.localAI[1] + (Vulneverable ? 0.01f : -0.01f), 0f, 1f);

            npc.ai[2] -= 1;
            FizzleOut();

            if (npc.ai[2] < -1)
            {
                goto despawn;
            }


            if (masterBeforeMe != null && masterBeforeMe.npc.active == false)
            {
                masterBeforeMe.npc.active = true;
                masterBeforeMe.npc.life = 1;
                masterBeforeMe.CheckDead();
            }


            float percent = MathHelper.Clamp(npc.localAI[0] / 450f, 0f, 1f);
            float percent2 = MathHelper.Clamp((npc.localAI[0] - 220f) / 420f, 0f, 1f);
            Vector2 poz = npc.Center - Main.screenPosition;
            float size = (npc.localAI[1] + (0.20f * percent)) * ((npc.life / (float)npc.lifeMax) + 0.20f) * 1200f;

            if (poz.X > -size && poz.X < Main.screenWidth + size && poz.Y > -size && poz.Y < Main.screenHeight + size)
            {
                SGAmod.PostDraw.Add(new PostDrawCollection(new Vector3(npc.Center.X, npc.Center.Y, size)));
            }


            if (spawnedFrom != null && emptySpace != default)
            {
                Vector2 whereWeWere = npc.Center;
                float speedspeedspeed = 1f;// 16f/whereWeWere.Length();

                npc.Center = Vector2.Lerp(spawnedFrom.Position, emptySpace, MathHelper.SmoothStep(0f, 1f, percent2 * speedspeedspeed));
                npc.velocity = (whereWeWere - npc.Center) / 3f;
            }
            else
            {
                goto despawn;
            }

            if (npc.velocity.Length() > 1)
            {
                trailPositions.Add((npc.Center, 30));
            }

            trailPositions = trailPositions.Select(testby => (testby.Item1, testby.Item2 - 1)).Where(testby => testby.Item2 > 0).ToList();

            //Main.NewText(trailPositions.Count);

            SGAnpcs sganpc = npc.SGANPCs();
            sganpc.dotImmune = true;

            sganpc.overallResist = Vulneverable ? 1f : 0f;
            if (Vulneverable)
                npc.dontTakeDamage = false;

            if (masterBeforeMe != null && masterBeforeMe.npc.life >= 5)
            {
                if (npc.life<npc.lifeMax)
                npc.life += 1;
            }

            if (npc.ai[3] > 0)
            {
                if (npc.life < 5 && (masterBeforeMe == null || masterBeforeMe.npc.life < 5))
                {
                    if (npc.ai[3] < 120)
                        npc.ai[3] += 1;
                }
                else
                {
                    npc.ai[3] -= 5;
                    if (npc.ai[3] < 0)
                        npc.ai[3] = 0;
                }

                sganpc.overallResist = 0f;
            }
            else
            {
                if (npc.localAI[0] < 450)
                {
                    if (npc.ai[3] > 0)
                        npc.ai[3]--;

                    npc.dontTakeDamage = false;
                    sganpc.overallResist = 0f;
                }
            }

            return;

        despawn:
            npc.active = false;
            npc.NPCLoot();

            return;
        }

        public override void NPCLoot()
        {
            SoundEffectInstance snd = Main.PlaySound(SoundID.NPCKilled, (int)npc.Center.X, (int)npc.Center.Y, 7);

            if (snd != null)
            {
                snd.Pitch = Main.rand.NextFloat(-0.75f, -0.25f);
            }

            for (float num475 = 4; num475 < 12f; num475 += 0.1f)
            {
                Vector2 startloc = (npc.Center);
                int dust = Dust.NewDust(new Vector2(startloc.X, startloc.Y), 0, 0, 185);

                float anglehalf2 = Main.rand.NextFloat(MathHelper.TwoPi);

                Main.dust[dust].scale = 3f - Math.Abs(num475) / 6f;
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                Main.dust[dust].velocity = (randomcircle * num475);
                Main.dust[dust].noGravity = true;
            }
        }

        public void DrawThroughFog(SpriteBatch spriteBatch)
        {

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {

            float overallAlpha = MathHelper.Clamp(npc.ai[2] / 50f, 0f, 1f);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            float trailGlow = MathHelper.Clamp(npc.velocity.Length() / 5f, 0f, 1f);

            if (trailPositions.Count > 0)
            {
                TrailHelper trail2 = new TrailHelper("FadedBasicEffectAlphaPass", mod.GetTexture("SmallLaser"));
                trail2.projsize = Vector2.Zero;
                trail2.coordOffset = new Vector2(0, -Main.GlobalTime * 0.5f);
                trail2.coordMultiplier = new Vector2(1f, 4f);
                trail2.doFade = false;
                trail2.trailThickness = 28f;
                trail2.strength = trailGlow * overallAlpha * overallAlpha * 2.5f;
                trail2.color = delegate (float percent)
                {
                    return Color.CornflowerBlue * percent;
                };
                trail2.trailThicknessIncrease = 24;
                trail2.DrawTrail(trailPositions.Select(testby => testby.Item1).ToList(), npc.Center);
            }


            if (1f - trailGlow > 0 && masterBeforeMe != null)
            {
                List<Vector2> drawsToHere = new List<Vector2>();

                Texture2D glowTrailEffect = mod.GetTexture("VerticleGlow");

                drawsToHere.Add(npc.Center);
                drawsToHere.Add(masterBeforeMe.npc.Center);

                float EffectAlpha = (0.40f + MathHelper.Clamp(npc.ai[3] / 120f, 0f, 0.35f)) * npc.localAI[1];

                if (EffectAlpha > 0)
                {
                    float str = (1f - trailGlow) * EffectAlpha * overallAlpha * overallAlpha;
                    for (int i = 0; i < 2; i += 1)
                    {
                        TrailHelper trail = new TrailHelper(i > 0 ? "FadedBasicEffectAlphaPass" : "FadedBasicEffectPass", i > 0 ? glowTrailEffect : mod.GetTexture("TiledPerlin"));
                        trail.projsize = Vector2.Zero;
                        if (i == 0)
                        {
                            trail.coordOffset = new Vector2(npc.whoAmI % 1f, Main.GlobalTime * 1f);
                            trail.coordMultiplier = new Vector2(0.25f, 1f);
                            trail.strength = str;
                        }
                        if (i == 1)
                        {
                            trail.coordOffset = new Vector2(0, Main.GlobalTime * 2f);
                            trail.coordMultiplier = new Vector2(1f, 1f);
                            trail.strength = str * 4f;
                        }

                        trail.doFade = false;
                        trail.trailThickness = 16f;
                        trail.color = delegate (float percent)
                        {
                            return Color.Aqua * 1f;
                        };
                        trail.trailThicknessIncrease = 24;
                        trail.DrawTrail(drawsToHere, npc.Center);
                    }
                }


            }


            Vector2 poz = npc.Center - Main.screenPosition;
            float size = 320f;

            if (poz.X > -size && poz.X < Main.screenWidth + size && poz.Y > -size && poz.Y < Main.screenHeight + size)
            {
                float percent = MathHelper.Clamp(npc.localAI[0] / 300f, 0f, 1f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


                Texture2D glowTex = mod.GetTexture("Glow");
                ArmorShaderData stardustsshader = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye);

                DrawData value8 = new DrawData(glowTex, new Vector2(30f, 30f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64)), Microsoft.Xna.Framework.Color.White, npc.rotation, glowTex.Size() / 2f, npc.scale * 5f, SpriteEffects.None, 0);
                Color colorz = Color.Lerp(Color.CornflowerBlue, Color.White, (npc.life / (float)npc.lifeMax));

                if (masterBeforeMe == null)
                    colorz = Color.Lerp(Color.CornflowerBlue, Main.hslToRgb((Main.GlobalTime / 3f) % 1f, 0.75f, 0.60f), (npc.life / (float)npc.lifeMax));

                stardustsshader.UseColor(colorz.ToVector3() * 2f);
                stardustsshader.UseOpacity(1f);
                stardustsshader.Apply(null, new DrawData?(value8));

                float scaler = (masterBeforeMe == null ? 2f : 1f) * (1f + (1f - (npc.life / (float)npc.lifeMax) * 1f) * 0.5f);

                for (float f2 = 1; f2 <= 3; f2 += 1f)
                {
                    for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 6f)
                    {
                        spriteBatch.Draw(glowTex, npc.Center - Main.screenPosition, null, Color.White * 0.75f * percent * overallAlpha, MathHelper.PiOver4 + f + (MathHelper.TwoPi * (f2 / 3f)) + (Main.GlobalTime / 3f * f2), glowTex.Size() / 2f, new Vector2(0.75f, 1f) * f2 * scaler, SpriteEffects.None, 0);
                    }
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            }
            return false;

        }

    }

    public class SpaceBossShadowNebulaRepair : OverseenHead, IDrawThroughFog
    {
        NPC healOrb = default;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Constellus Stealer");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.life = 7500;
            npc.lifeMax = 7500;
            npc.dontTakeDamage = false;
            npc.damage = 0;
            npc.aiStyle = -1;
            npc.width = 24;
            npc.height = 24;
            npc.damage = 0;
            npc.knockBackResist = 0f;
        }

        public override bool CheckDead()
        {
            if (npc.ai[3] < 1)
            {
                npc.ai[3] = 1;
                npc.netUpdate = true;
                npc.life = 1337;
                npc.dontTakeDamage = true;
                return false;
            }
            return npc.ai[3] > 150;
        }

        public override void AI()
        {
            healOrb = default;
            npc.velocity *= 0.90f;

            if (npc.localAI[3] == 0)
            {
                npc.Opacity = 0f;
                npc.ai[1] = Main.rand.NextFloat(MathHelper.TwoPi);
                npc.localAI[2] = 1;
                npc.localAI[3] = Main.rand.Next(1, 10);
                npc.netUpdate = true;
            }

            npc.localAI[0] += 1;

            if (npc.ai[3] > 0)
            {
                npc.ai[3] += 1;
                npc.velocity *= 0.98f;
                if (npc.ai[3] > 150)
                {
                    npc.StrikeNPCNoInteraction(10000, 0, 1);
                }
                return;
            }

            float alphaFinal = MathHelper.Clamp(npc.localAI[0] / 160f, 0f, 1f);
            NPC[] bossg = Main.npc.Where(testby => testby.active && testby.type == ModContent.NPCType<SpaceBoss>()).ToArray();

            SpaceBoss boss = null;

            if (bossg.Length > 0)
                boss = (SpaceBoss)(bossg[0].modNPC);

            NPC[] orbs = Main.npc.Where(testby => testby.active && testby.type == ModContent.NPCType<SpaceBossShadowNebulaEnemy>() && testby.life < testby.lifeMax).ToArray();

            if (orbs.Length < 1 || boss == null)
            {
                npc.StrikeNPCNoInteraction(10000, 0, 1);
                return;
            }

            //orbs = orbs.OrderBy(testby => (testby.ai[3] > 0 ? 0 : 10000) + (testby.life - ((testby.whoAmI - (npc.localAI[3])) * testby.lifeMax * 2))).ToArray();
            orbs = orbs.OrderBy(testby => ((testby.Center-npc.Center).Length()*12f) + (testby.life - ((testby.whoAmI - (npc.localAI[3])) * testby.lifeMax * 2))).ToArray();

            NPC temporb = orbs[0];

            Vector2 toOrb = temporb.Center - npc.Center;

            //npc.dontTakeDamage = true;

            if (toOrb.Length() > 64)
            {
                if (npc.localAI[0] > 160)
                    npc.velocity += Vector2.Normalize(toOrb).RotatedBy((float)Math.Sin(npc.localAI[0] / 30f) * (MathHelper.Pi / 6f)) * 0.50f * alphaFinal*(1f-npc.localAI[1]);

                npc.localAI[1] *= 0.99f;

                Vector2 Offset = Main.rand.NextVector2Circular(24f, 24f);

                for (int duster = 0; duster < 1 + (1f - alphaFinal) * 10f; duster += 1)
                {
                    int dust = Dust.NewDust(npc.Center + Offset * ((1f - alphaFinal) * 5f), 0, 0, ModContent.DustType<LeviDust>());
                    Main.dust[dust].velocity = Main.rand.NextVector2Circular(2f, 2f)+(npc.velocity*Main.rand.NextFloat(0.25f,0.75f));
                    Main.dust[dust].scale = alphaFinal;
                    Main.dust[dust].alpha = 200;
                    Main.dust[dust].noGravity = true;
                }
            }
            else
            {
                //npc.dontTakeDamage = false;
                npc.localAI[1] = MathHelper.Clamp(npc.localAI[1] + (1.25f - npc.localAI[1]) * 0.004f, 0f, 1f);
                healOrb = temporb;
                healOrb.life = Math.Min(healOrb.life + (int)(healOrb.lifeMax / 400f), healOrb.lifeMax);
                npc.Center = Vector2.Lerp(npc.Center, healOrb.Center, npc.localAI[1]);
            }



            Player target = Main.LocalPlayer;


        }


        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            float alphaFinal = MathHelper.Clamp(npc.localAI[0] / 160f, 0f, 1f);
            return base.PreDraw(spriteBatch, drawColor * alphaFinal);
        }

        public void DrawThroughFog(SpriteBatch spriteBatch)
        {

            float alphaFinal = MathHelper.Clamp(npc.localAI[0] / 160f, 0f, 1f)*(1f- (npc.ai[3]/150f));

            if (alphaFinal > 0)
            {
                float addedScale = npc.localAI[1] * 0.5f;
                Color colorz = Color.Lerp(Color.CornflowerBlue, Color.DarkMagenta * 3f, npc.localAI[1]);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                Texture2D glowTex = mod.GetTexture("GlowOrb");
                ArmorShaderData stardustsshader = GameShaders.Armor.GetShaderFromItemId(ItemID.VortexDye);

                DrawData value8 = new DrawData(glowTex, new Vector2(30f, 30f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64)), Microsoft.Xna.Framework.Color.White, npc.rotation, glowTex.Size() / 2f, npc.scale * 5f, SpriteEffects.None, 0);
                stardustsshader.UseColor(colorz.ToVector3());
                stardustsshader.UseOpacity(0.15f);
                stardustsshader.Apply(null, new DrawData?(value8));


                for (float f2 = 1; f2 < 3; f2 += 1f)
                {
                    for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 8f)
                    {
                        float angleSpin = Main.GlobalTime * f2;
                        spriteBatch.Draw(glowTex, npc.Center - Main.screenPosition, new Rectangle(0, 0, glowTex.Width / 2, glowTex.Height), colorz * (1f / f2) * (alphaFinal) * 0.75f, angleSpin + (MathHelper.PiOver4* (f2/2f)) + f, glowTex.Size() / 2f, new Vector2(1.60f, 0.50f) * ((f2 * 0.10f * (1f + (1f - alphaFinal) * 10f)) + addedScale)*new Vector2(1f-(npc.ai[3]/150f),1f+ npc.ai[3]/20f), SpriteEffects.None, 0);
                    }
                }
                //spriteBatch.Draw(glowTex2, npc.Center - Main.screenPosition, null, EyeColor, 0, glowTex2.Size() / 2f, new Vector2(8f, 8f), SpriteEffects.None, 0);



                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            }
        }

    }


}
