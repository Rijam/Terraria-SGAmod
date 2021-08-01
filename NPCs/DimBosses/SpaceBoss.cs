
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
				lookTo = lookat - (spacex.Position +spacex.bossOffset + offset);
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
			if (sleep>0)
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
		public SpaceBossEye eye;
		public int state = 0;
		public int timer = 0;
		public int airReady = 0;
		public Vector2 velocity = Vector2.Zero;
		public MineableAsteriod grabrock;
		public float whiten = 0;

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
			randomRotation2 = Main.rand.NextFloat(-0.1f,0.1f);
			scale = Main.rand.NextFloat(0.5f,0.8f);
		}

		public void UpdateRotation(float phi)
        {
			baserotation += phi;
        }

		public void BossRocksUpdate()
        {
			timer += 1;

			Vector2 posa = Position;


			bool ragegrab = boss.tossRoids;
			bool chance = Main.rand.Next(0, 1000) < boss.phase || (boss.tossRoids && Main.rand.Next(0, 100)<1);

			if (state < 1 && boss.state<10 && chance)
            {
				Projectile[] roids = boss.AllAsteriods.Where(testby => testby.velocity.LengthSquared()<20f && !testby.ignoreWater).OrderBy(testby => (testby.Center- posa).LengthSquared()+(testby.damage>0 ? 1000000 : 0)).ToArray();

				if (roids.Count() > 0)
				{
					MineableAsteriod firstones = roids[0].modProjectile as MineableAsteriod;
					Vector2 diff = firstones.projectile.Center - posa;

					if (diff.Length() < 1000+(ragegrab ? 2000 : 0))
					{
						firstones.projectile.timeLeft = 600;
						firstones.projectile.ignoreWater = true;
						LockUnlock(false);
						velocity += Vector2.Normalize(firstones.projectile.Center-Position).RotatedBy(MathHelper.Pi+(Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi)*0.5f)) * 32f;
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
					Vector2 toPlayer = Main.player[boss.npc.target].Center-posa;
					velocity += Vector2.Normalize(toPlayer) * 32f;

					grabrock.projectile.velocity = Vector2.Normalize(toPlayer) * 24f;
					grabrock.projectile.damage = 50;
					grabrock.projectile.hostile = true;
					grabrock.projectile.friendly = false;
					grabrock.projectile.ignoreWater = false;
					state = 101;
				}
			}

			if (state == 101)//Throw Roid
			{

				Vector2 diff = boss.npc.Center - Position;
				bossOffset += Vector2.Normalize(diff) * (ragegrab ? 72f : 24f);

				UnifiedRandom randomrock = new UnifiedRandom((int)(randomRotation2 * 32));
				if (diff.Length() < 8f + randomrock.NextFloat(16f, 72f))
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

				this.bossOffset = -(boss.npc.Center- this.bossOffset);
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
		SpaceBoss boss;
		NPC npc;
		public SpaceBossAI(SpaceBoss boss)
        {
			this.boss = boss;
			npc = boss.npc;
		}

		public void StateShadowNebula()
		{
			boss.state = 100;
			npc.life = (int)MathHelper.Clamp(npc.life + (int)(npc.lifeMax / 10000), 0, npc.lifeMax);
			boss.friction = 0.975f;

			if (npc.ai[0] > 50000)
            {
				npc.dontTakeDamage = true;
				if ((int)npc.ai[0] % 30 == 0 && npc.ai[0]<50300)
				{
					SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, (int)npc.Center.X, (int)npc.Center.Y);
					if (sound != null)
					{
						sound.Pitch -= (npc.ai[0]-50000f)/600f;
					}
				}

				for (int i = 48; i < 320; i += 16)
				{
					Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
					int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + offset * Main.rand.NextFloat(32f,i+ (npc.ai[0]-50000)*8f), 0, 0, 68);
					Main.dust[dust].scale = 1.5f;
					Main.dust[dust].velocity = Vector2.Normalize(offset) * (float)(8f * Main.rand.NextFloat(0.10f, 0.50f));
					Main.dust[dust].noGravity = true;
				}
				if (npc.ai[0] == 50005)
                {
					boss.goingDark = 60 * 60;
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
					npc.ai[0] = 999;
				}

			}

			foreach (SpaceBossEye eyxxe in boss.Eyes)
			{
				if (Main.rand.Next(0, 75) == 0)
					eyxxe.NewLook(npc.Center + new Vector2((Main.rand.Next(24, 160))).RotatedBy(MathHelper.TwoPi), 0.04f, true, true);
			}
		}

		public void StateGrabAsteriodsAndSpin()
		{

			if (boss.tossRoids)
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

		public void StateCreateBeamX()
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
				boss.LaunchTethers();
			}

			if (npc.ai[0] > 10120)
			{
				npc.ai[0] = 1000;
			}

		}
		public void StateShielded()
		{
			boss.state = 10;
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
			if (boss.goingDark>0)
				thisthing = boss.visitedEmptySpaces.Where(testby => (testby-Main.player[npc.target].Center).LengthSquared()< 6000 * 6000).OrderBy(testby => (testby - Main.player[npc.target].Center).LengthSquared()).ToArray();
			else
				thisthing = boss.visitedEmptySpaces.OrderBy(testby => (testby - npc.Center).LengthSquared()).ToArray();

			if (thisthing.Length > 0)
			{
				int index2 = Main.rand.Next(0, boss.visitedEmptySpaces.Count / 20);
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

			Main.NewText("RocksAreas left " + boss.visitedEmptySpaces.Count);

			//Spawn Guys

			NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<OverseenHead>());

			if (boss.phase > 1)
			{
				SpaceBossRock[] closerocksbutnottooclose = boss.RocksAreas.Where(testby => (testby.Position - Main.player[npc.target].Center).LengthSquared() < (3200f * 3200f) && (testby.Position - Main.player[npc.target].Center).LengthSquared() > (640f * 640f)).OrderBy(testby => (testby.Position - Main.player[npc.target].Center).LengthSquared()).ToArray();

				int index = Main.rand.Next(0, closerocksbutnottooclose.Length / 3);

				NPC.NewNPC((int)closerocksbutnottooclose[index].Position.X, (int)closerocksbutnottooclose[index].Position.Y, ModContent.NPCType<OverseenHead>());
			}

			ChooseAttack();

		}

		enum SpaceBossAttackTypes
		{
			Nothing,
			XBeams,
			TossRoids,
			ShadowNebula,

		}

		public bool ChooseAttack()
		{

			List<int> ChooseableAttacks = new List<int>();

			ChooseableAttacks.Add((int)SpaceBossAttackTypes.XBeams);
			ChooseableAttacks.Add((int)SpaceBossAttackTypes.TossRoids);
			if (boss.goingDark < -60 * 150 && boss.specialCooldown<1 && boss.phase>1)
            {
			ChooseableAttacks.Add((int)SpaceBossAttackTypes.ShadowNebula);
				ChooseableAttacks.Add((int)SpaceBossAttackTypes.ShadowNebula);
				ChooseableAttacks.Add((int)SpaceBossAttackTypes.ShadowNebula);
			}

			if (npc.life / (float)npc.lifeMax < boss.healthphase)//Heal if phase requires it
			{
				boss.phase += 1;
				boss.healthphase -= 0.25f;
				npc.ai[0] = 10000;//Shields up
				return true;
			}

			if (npc.ai[0] > 1200 && npc.ai[0] < 10000)//Do special move
			{
			tryagain:
				int attack = ChooseableAttacks[Main.rand.Next(ChooseableAttacks.Count)];

				if (attack == (int)SpaceBossAttackTypes.ShadowNebula)//Choose: Shadow Nebula
				{
					boss.specialCooldown = 80 * 60;
					npc.ai[0] = 50000;//Shadow Nebula
					return true;
				}

				if (ToEnemy.Length() > 3200)//Heal if too far away

				{
					npc.ai[0] = 10000;//Shields up
					return true;
				}

				if (attack == (int)SpaceBossAttackTypes.XBeams)//Choose: X Beams
				{
					SpaceBossRock[] nearbyrocks = boss.RocksAreas.Where(testby => (testby.Position - Main.player[npc.target].Center).LengthSquared() < (2000f * 2000f) && (testby.Position - Main.player[npc.target].Center).LengthSquared() > (640f * 640f)).ToArray();
					if (nearbyrocks.Length > 0)//Need atleast 3 nearby to use this
					{
						Main.NewText("laser");
						npc.ai[0] = 10200;//X Beams
						return true;
					}
					else
					{
						ChooseableAttacks.Add((int)SpaceBossAttackTypes.Nothing);
						ChooseableAttacks.RemoveAll(testby => testby == (int)SpaceBossAttackTypes.XBeams);
						goto tryagain;
					}
				}

				if (attack == (int)SpaceBossAttackTypes.TossRoids)//Choose: Toss Roids
				{
					npc.ai[0] = 10500;//Toss Roids
					return true;
				}

			}

			return false;

		}

		public Vector2 ToEnemy => Main.player[npc.target].Center - npc.Center;

		public void StateMove()
		{
			float movementSpeed = 0.25f + (boss.phase / 8f);
			boss.state = 2;
			npc.rotation += npc.velocity.X * 0.002f;

			Vector2 dista = boss.goToEmptySpace - npc.Center;
			boss.friction = 0.95f;

			if ((dista).Length() < 160)
			{
				if (npc.velocity.Length() < 4f)
					boss.timeHere += 1;
				if (boss.timeHere > 10 + (npc.life / (float)npc.lifeMax) * 60)
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

		public void StateActive()
		{
			boss.state = 1;
			npc.ai[0] += 1;
			if (boss.goingDark > 0)
            {
				npc.chaseable = false;
			}
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

				SoundEffectInstance sound = Main.PlaySound(SoundID.NPCHit, (int)npc.Center.X, (int)npc.Center.Y, 57);
				if (sound != null)
				{
					sound.Volume = 0.99f;
					sound.Pitch -= 0.75f;
				}

			}

			if (npc.ai[0] > 1000)
			{

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
								Main.PlaySound(SoundID.Drown, (int)player.Center.X, (int)player.Center.Y, 0, 1f, 0.50f);
								player.breath = (int)MathHelper.Clamp(player.breath + MathHelper.Clamp((int)((rockyplace.airReady / 600f) * 30f), 0, 30), 0, player.breathMax);
								sgaplayer.sufficate = player.breath;
								rockyplace.airReady = -Main.rand.Next(1000, 2000);
							}
						}
					}
				}

				npc.dontTakeDamage = false;


				foreach (SpaceBossRock rockyplace in boss.Rocks)
				{
					rockyplace.BossRocksUpdate();
				}

				if (npc.ai[0] >= 50000 && npc.ai[0] < 51000)//Shadow Nebula
				{
					StateShadowNebula();
					return;
				}

				if (npc.ai[0] >= 10000 && npc.ai[0] < 10200)//Shield Startup
				{
					StateStartShield();
					return;
				}

				if (npc.ai[0] >= 10200 && npc.ai[0] < 10400)//Beams
				{
					StateCreateBeamX();
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
					boss.darknessAura = MathHelper.Clamp(boss.darknessAura - 0.025f,0.5f,1f);
					StateShielded();
				}
				else
				{
					boss.shieldeffect = MathHelper.Clamp(boss.shieldeffect - 1f, 0f, 30f);
					if (boss.shieldeffect <= 0f)
						StateMove();
				}

			}

		}

	}


	public class SpaceBoss : ModNPC
	{

		public float darknessAura = 0.25f;
		public float timeHere = 0f;
		public float shieldeffect = 30f;
		public int phase = 0;
		public int state = 0;
		public float healthphase = 0.80f;
		public bool tossRoids => (int)npc.ai[0] > 10600 && (int)npc.ai[0] < 10800;
		public float friction = 1f;
		public int specialCooldown = 0;
		public Vector2[] screenPos;

		public Film film = new Film();

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

		public override bool Autoload(ref string name)
		{
			name = "Phaethon";
			SGAmod.ModifyTransformMatrixEvent += MoveCamera;
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
			npc.width = 96;
			npc.height = 96;
			npc.aiStyle = -1;
			npc.damage = 0;
			npc.noGravity = true;
			npc.defense = 0;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.5f;
			//npc.immortal = true;
			animationType = NPCID.Guide;
			npc.rarity = 1;

			npc.lifeMax = 100000;
			npc.friendly = false;
			//npc.immortal = true;
			npc.dontTakeDamage = true;
			npc.noTileCollide = true;
			npc.knockBackResist = 0f;
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
				Projectile projectile = Projectile.NewProjectileDirect(npc.Center + (velocity* Main.rand.NextFloat(-8f, 20f)), velocity*Main.rand.NextFloat(8f,16f), projtype, 0, 0);
				projectile.damage = 5;
			}
			

			SpaceDim.crystalAsteriods = true;
		}

        /*public override bool? CanBeHitByItem(Player player, Item item)
		{
			return ((WakingUp || Sleeping) ? false : CanBeHitByItem(player, item));
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return ((WakingUp || Sleeping) ? false : CanBeHitByProjectile(projectile));
		}
        public override bool? CanHitNPC(NPC target)
        {
			return ((WakingUp || Sleeping) ? false : CanHitNPC(target));
		}*/

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
				Main.NewText("I exist at " + npc.Center);
				Main.NewText("you are at " + Main.LocalPlayer.Center);
			}


			if (timePassed == 0)
			{

				screenPos = new Vector2[10];
				for (int i = 0; i < screenPos.Length; i += 1)
				{
					screenPos[i] = npc.Center - Main.screenPosition;
				}

				ai = new SpaceBossAI(this);

				for (int i = 0; i < 32; i += 1)
				{

					Vector2 offsetGaussian = SpaceDim.Gaussian2D(Main.rand.NextFloat(), Main.rand.NextFloat()) * (320f);
					offsetGaussian += Vector2.Normalize(offsetGaussian) * 128f;

					SpaceBossRock rock = new SpaceBossRock(npc.Center + offsetGaussian, this);

					Rocks.Add(rock);
					SpaceBossEye eye3 = new SpaceBossEye(rock.bossOffset + Vector2.UnitY, rock, new Vector2(0, 0));
					eye3.eyeScale = Main.rand.NextFloat(0.25f, 0.75f);
					eye3.sleep = Main.rand.NextFloat(-2f, -0.5f);
					rock.eye = eye3;
					Eyes.Add(eye3);
				}

				SpaceBossEye eye = new SpaceBossEye(npc.Center + Vector2.UnitY, this, new Vector2(0, 0));
				eye.customID = 1;
				//SpaceBossEye eye2 = new SpaceBossEye(npc.Center + Vector2.UnitY, this, new Vector2(0, 0));
				//eye2.customID = -1;

				Eyes.Add(eye);
				//Eyes.Add(eye2);

				LaunchTethers();
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
		public bool Activestate => npc.active && npc.ai[0] >= 300 && !DyingState;

		public bool DyingState => npc.ai[0] >= 100000;
		public int timePassed = 0;
		public Projectile deathproj;

		public void StateDying()
		{
			npc.ai[0] += 1f;
			npc.dontTakeDamage = true;
			if (goingDark>0)
			goingDark /= 4;
			npc.velocity /= 2f;

			if (npc.ai[0] == 100001)
			{

				Deathsnd = Main.PlaySound(SoundID.Shatter, (int)npc.Center.X, (int)npc.Center.Y, 0);
				cutscenestartpos = new Vector3(Main.screenPosition.X, Main.screenPosition.Y, 1f);

				deathproj = Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ModContent.ProjectileType<SpaceBossExplode>(), 0, 0);

				scenecam = cutscenestartpos;
				//scenecamend = npc.Center - (new Vector2(Main.screenWidth, Main.screenHeight) / 2f);
				camlength = 60;
				camlength2 = 700;
				film = new Film();
				film.AppendSequence(camlength, FilmSetCamera);
				film.AppendSequence(camlength2 - camlength, FilmSetCamera);
				//for(int i=0;i<150;i++)
				film.AppendSequence(300, FilmSetCamera);

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
				Deathsnd.Pitch = MathHelper.Clamp(0.8f-(npc.ai[0]-100000f)/45f,-0.9f,0.9f);
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
					rock.bossOffset += Vector2.Normalize(npc.Center- rock.Position) * ((time - index) * 0.025f);
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
			if (evt.Frame<2)
				cutscenestartpos = new Vector3(Main.screenPosition.X, Main.screenPosition.Y,1f);

			if (evt.AbsoluteFrame < camlength)
			{
				Vector2 diff = npc.Center - (new Vector2(Main.screenWidth, Main.screenHeight) / 2f);
				scenecamend = Vector3.Lerp(cutscenestartpos, new Vector3(diff.X, diff.Y,1f), evt.Frame / (float)evt.Duration);
			}

			if (evt.AbsoluteFrame > camlength2)
			{
				Vector2 diff = Main.LocalPlayer.Center - (new Vector2(Main.screenWidth, Main.screenHeight) / 2f);
				scenecamend = Vector3.Lerp(cutscenestartpos, new Vector3(diff.X, diff.Y, 1f-MathHelper.Clamp(((evt.Frame / (float)evt.Duration)*3f)-2f, 0f,1f)), (evt.Frame/(float)evt.Duration)*1.05f);
			}

			scenecam += (scenecamend - scenecam) * 0.05f;
			//Main.screenPosition = scenecam;
		}

		private void PerFrameSettings(FrameEventData evt)
		{
			cutsceneposition = 2;
		}

		private void MoveCamera(ref SpriteViewMatrix transform)
        {
			if (SpaceBoss.cutsceneposition > 0 && SGAPocketDim.WhereAmI == typeof(SpaceDim))
			{
				Vector2 oldpos = Main.screenPosition;
				Main.screenPosition = Vector2.Lerp(new Vector2(SpaceBoss.scenecam.X, SpaceBoss.scenecam.Y), Main.screenPosition, MathHelper.Clamp(1f-SpaceBoss.scenecam.Z,0f,1f));
			}
		}


		public void StateWakingUp()
		{
			bool canAdvance = true;

			npc.TargetClosestUpgraded(false, npc.Center);

			if (npc.ai[0] == 1)
            {

						camlength = 300;
						camlength2 = 1200;
		cutscenestartpos = new Vector3(Main.screenPosition.X, Main.screenPosition.Y,1f);

				scenecam = cutscenestartpos;

		//scenecamend = npc.Center - (new Vector2(Main.screenWidth, Main.screenHeight) / 2f);
				film.AppendSequence(camlength, FilmSetCamera);
				film.AppendSequence(camlength2- camlength, FilmSetCamera);
				//for(int i=0;i<150;i++)
				film.AppendSequence(150, FilmSetCamera);

				film.AddSequence(0, film.AppendPoint, PerFrameSettings);

				CinematicManager.Instance.PlayFilm(film);
			}

			foreach (SpaceBossRock rock in Rocks)
			{
				rock.randomRotation2 *= 0.98f;
				rock.UpdateRotation(rock.randomRotation2);
			}

			if (npc.ai[0] > 120)
			{
				shieldeffect -= 1f;
				if (shieldeffect < 0)
					shieldeffect = 0;

				UnifiedRandom randomrock = new UnifiedRandom(npc.whoAmI*8);
				foreach (SpaceBossEye eye in Eyes)
				{
					if (eye.boss != this)
					{
						eye.sleep += 0.015f;
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
							rock.bossOffset += Vector2.Normalize(toboss) * MathHelper.Clamp((eye.sleep-1.5f)*6f,0f,12f);
							if (toboss.Length() < 8f+ randomrock.NextFloat(8f,64f))
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

				for (int i = 0; i < (boom ? 64 : 2); i += 1)
				{
					Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
					int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + offset * Main.rand.NextFloat(16f, 32f), 0, 0, DustID.BlueCrystalShard);
					Main.dust[dust].scale = 1.5f;
					Main.dust[dust].velocity = Vector2.Normalize(-offset) * (float)((boom ? 8f : 1f) *Main.rand.NextFloat(0.50f, 2.50f));
					Main.dust[dust].noGravity = true;
				}
			}


				if (npc.ai[0]<280 || canAdvance)
			npc.ai[0] += 1f;

			if ((int)npc.ai[0] < 2)
			{
				foreach (SpaceBossEye eye in Eyes)
				{
					eye.NewLook(npc.Center+Vector2.UnitY*120f,0.005f);
				}
			}

			npc.velocity *= 0.96f;
		}

		public void LaunchTethers(int ammount=3)
        {
			List<Vector2> vex = SpaceDim.FilledSpaces;
			Vector2 previousone = default;

			for (int i = 0; i < ammount; i += 1)
			{
				Vector2[] thisthing = vex.OrderBy(testby => (testby - npc.Center).LengthSquared() + (previousone == default ? 0 :
				Vector2.Dot(Vector2.Normalize(testby - npc.Center), Vector2.Normalize(previousone - npc.Center)) > 0.5 ? 999999 : 0)).ToArray();
				Vector2 position1 = thisthing[0];
				vex.Remove(position1);
				previousone = position1;

				Projectile proj = Projectile.NewProjectileDirect(npc.Center, Vector2.Normalize(position1 - npc.Center) * 16f, ModContent.ProjectileType<MineableAsteriodBossLock>(), 0, 0);
				if (proj != null)
				{
					proj.rotation = proj.velocity.ToRotation() - MathHelper.PiOver2;
				}
			}

		}















		public override void AI()
		{
			Initiate();
			state = 0;
			npc.localAI[0] += 1;

			timePassed += 1;
			cutsceneposition -= 1;

			friction = 0.95f;
			npc.chaseable = true;

			//if (goingDark > 0)
				goingDark -= 1;
			specialCooldown -= 1;

			for (int i = 1; i < screenPos.Length; i += 1)
			{
				screenPos[i] = screenPos[i-1];
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
				if (darknessAura<(phase > 1 ? 1f : 0.75f) && npc.ai[0]>120)
				darknessAura += 0.005f;

				if (WakingUp)
					StateWakingUp();
				if (Activestate)
					ai.StateActive();

			}

			if (goingDark > 0)
            {
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

				foreach(SpaceBossRock rock in RocksAreas)
                {
					if (rock.airReady > 0)
					{
						int size = (int)MathHelper.Clamp(rock.airReady * 2, 0, 1500);
						Vector2 poz = rock.Position - Main.screenPosition;
						if (poz.X>-size && poz.X < Main.screenWidth+size && poz.Y > -size && poz.Y < Main.screenHeight + size)
						SGAmod.PostDraw.Add(new PostDrawCollection(new Vector3(rock.Position.X, rock.Position.Y, size)));
					}
				}

            }

			npc.velocity *= friction;
		}

		public void AlterSky(SpaceSky sky, int index, SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
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

					UnifiedRandom alwaysthesame = new UnifiedRandom(DimDungeonsProxy.DungeonSeeds-5643);

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

				//Dark Aura

				//Main.spriteBatch.End();
				//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


				value28 = new DrawData(sunGlow, new Vector2(240, 240), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 240, 240)), Microsoft.Xna.Framework.Color.White, MathHelper.PiOver2, stain.Size() / 2f, 1f, SpriteEffects.None, 0);

				//Vector2 dir = ((npc.Center - Main.screenPosition) - sky.sunPosition);
				//float angle = dir.ToRotation();

				//Sun absorb healing

				if (npc.ai[0]>800 && shieldeffect > 0 && sky.skyalpha>0)
				{
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


					Texture2D tex = mod.GetTexture("Extra_57b");
					Vector2 starhalf = tex.Size() / 2f;

					List<(float, float, float, Vector2)> ordering = new List<(float, float, float,Vector2)>();

					UnifiedRandom rando = new UnifiedRandom(npc.whoAmI);

					for (float f = 0f; f < 1f; f += 1/100f)
					{
						float prog = MathHelper.Clamp((f + (Main.GlobalTime * 0.05f)+ rando.NextFloat(1f)) % (1f), 0f, 1f);
						float alpha = MathHelper.Clamp((prog * 5f) * MathHelper.Clamp(1.5f - (prog * 1.5f), 0f, 1f), 0f, 1f)*MathHelper.Clamp((float)Math.Sin((Main.GlobalTime * 0.1f) + rando.NextFloat(MathHelper.TwoPi)),0f,1f);
						ordering.Add((prog, alpha, rando.NextFloat(MathHelper.TwoPi)+(Main.GlobalTime*rando.NextFloat(-0.5f,0.5f)),rando.NextVector2Circular(256f,256f)));
					}
					ordering = ordering.OrderBy(testby => testby.Item1).ToList();

					int index2 = 0;

					foreach ((float, float, float, Vector2) prog in ordering)
					{
						//float prog = MathHelper.Clamp((f+(Main.GlobalTime*0.25f))%(1f),0f,1f);
						//shader.UseColor(Color.Lerp(Color.White, Color.Blue, MathHelper.Clamp(prog.Item1 * 2f - 1f, 0f, 1f)).ToVector3());
						//shader.UseOpacity(1f);
						//shader.Apply(null, new DrawData?(value28));

						float angle = prog.Item3;

						Vector2 pos = Vector2.Lerp(sky.sunPosition, npc.Center-Main.screenPosition, prog.Item1);

						spriteBatch.Draw(tex, pos+(prog.Item4*prog.Item1), null, (Color.Lerp(Color.Yellow, Color.Blue,prog.Item1) * (prog.Item2)*(shieldeffect/15f))* sky.skyalpha, angle, starhalf, (0.25f + prog.Item1 * 0.5f), SpriteEffects.None, 0f);

						index2 += 1;
					}
				}

				//Twilight Nebula

				float alphaik = ((1f - sky.darkalpha) * 0.50f * darknessAura)*MathHelper.Clamp(1f-(npc.ai[0]-100000f)/180f,0f,1f);

				if (alphaik > 0 && screenPos!=null)
				{
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


					stain = mod.GetTexture("Doom_Harbinger_Resprite_pupil");

					List<(float, float, float)> ordering = new List<(float, float, float)>();

					UnifiedRandom rando = new UnifiedRandom(npc.whoAmI);

					for (float f = 0f; f < 1f; f += 1f / (float)screenPos.Length)
					{
						float prog = MathHelper.Clamp((f + (Main.GlobalTime * 0.25f)) % (1f), 0f, 1f);
						float alpha = MathHelper.Clamp((prog * 5f) * MathHelper.Clamp(1.5f - (prog * 1.5f), 0f, 1f), 0f, 1f);
						ordering.Add((prog, alpha, rando.NextFloat(MathHelper.TwoPi)));
					}
					ordering = ordering.OrderBy(testby => testby.Item1).ToList();
					float scalee = ((sky.darkalpha*5f) * 6f);

					int index2 = 0;

					foreach ((float, float, float) prog in ordering)
					{
						//float prog = MathHelper.Clamp((f+(Main.GlobalTime*0.25f))%(1f),0f,1f);
						shader.UseColor(Color.Lerp(Color.White, Color.Blue, MathHelper.Clamp(prog.Item1 * 2f - 1f, 0f, 1f)).ToVector3());
						shader.UseOpacity(1f);
						shader.Apply(null, new DrawData?(value28));

						float angle = prog.Item3;

						Vector2 pos = npc.Center - Main.screenPosition;// Vector2.Lerp(npc.Center-Main.screenPosition,new Vector2(Main.screenWidth, Main.screenHeight)/2f, prog.Item1 / 5f);

						spriteBatch.Draw(sunGlow, pos, null, Color.White * (prog.Item2 * alphaik), angle, half, (scalee) +(0.05f + prog.Item1 * 1.25f), SpriteEffects.None, 0f);

						index2 += 1;
					}
				}

			}


		}


		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D firerock = ModContent.GetTexture("SGAmod/Projectiles/FieryRock");
			Texture2D rocksmall = mod.GetTexture("Dimensions/Space/OverseenHead");
			Texture2D rocklarge = mod.GetTexture("Dimensions/Space/GlowAsteriod");
			Texture2D eyetex = mod.GetTexture("Doom_Harbinger_Resprite_pupil");

			Vector2 firerockorig = firerock.Size() / 2f;
			Vector2 rocksmallorig = new Vector2(rocksmall.Width, rocksmall.Height / 2f);
			Vector2 rocklargeorig = new Vector2(rocklarge.Width, rocklarge.Height);
			Vector2 eyetexorig = eyetex.Size()/2f;


			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			//Rocks

			foreach (SpaceBossRock rock in Rocks)
			{
				if (rock.eye != null) 
				{
					float alpha = rock.eye.sleep-0.60f;
					if (alpha <= 0 && rock.state < 1000)
						continue;

					if (rock.whiten > 0)
                    {
						alpha = MathHelper.Clamp(1f-Main.rand.NextFloat(0f, rock.whiten),0,1);
					}

				List<Vector2> toThem = new List<Vector2>();

					toThem.Add(rock.Position);
					toThem.Add(npc.Center);

					TrailHelper trail = new TrailHelper("FadedBasicEffectPass", Main.sunTexture);
					trail.projsize = Vector2.Zero;
					trail.coordOffset = new Vector2(0, 0f);
					trail.coordMultiplier = new Vector2(1f, 1f);
					trail.trailThickness = 16;
					trail.trailThicknessIncrease = -12;
					trail.doFade = false;
					trail.color = delegate (float percent)
					{
						return Color.CornflowerBlue * (MathHelper.Clamp((alpha+percent)-1f, 0f, 1f));
					};
					trail.DrawTrail(toThem, npc.Center);
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

				foreach (SpaceBossRock rock in RocksAreas)
				{
					Vector2 poz = rock.Position - Main.screenPosition;
					int size = 128;
					if (rock.airReady > 0 && poz.X > -size && poz.X < Main.screenWidth + size && poz.Y > -size && poz.Y < Main.screenHeight + size)
					{

						spriteBatch.Draw(Main.bubbleTexture, rock.Position - Main.screenPosition, null, rock.color * MathHelper.Clamp(rock.airReady / 120f, 0f, 1f), (float)Math.Sin(rock.Rotation) * 0.4f, Main.bubbleTexture.Size() / 2f, MathHelper.Clamp(rock.airReady / 600f, 0f, 1f) * 4f, SpriteEffects.None, 0f);
					}
				}
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Effect fadeIn = SGAmod.FadeInEffect;

			foreach (SpaceBossRock rock in Rocks)
			{
				Vector2 poz = rock.Position - Main.screenPosition;
				int size = 80;
				if (poz.X > -size && poz.X < Main.screenWidth + size && poz.Y > -size && poz.Y < Main.screenHeight + size)
				{

					fadeIn.Parameters["alpha"].SetValue(1);
					fadeIn.Parameters["strength"].SetValue(rock.whiten);
					fadeIn.Parameters["fadeColor"].SetValue(Color.White.ToVector3());
					fadeIn.Parameters["blendColor"].SetValue(rock.color.ToVector3());

					fadeIn.CurrentTechnique.Passes["FadeIn"].Apply();

					spriteBatch.Draw(rocklarge, rock.Position - Main.screenPosition, null, rock.color, rock.Rotation, rocklargeorig / 2f, rock.scale, SpriteEffects.None, 0f);
				}
			}

			//Tetheroids

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

				for (float i = 0; i < distanceVector.Length() ;i += rand.NextFloat(4f, 8f))
                {

					Vector2 gothere = npc.Center + Vector2.Normalize(distanceVector) * i;
					float percent = (160f * MathHelper.Clamp(1f - ((i / distanceVector.Length())), 0f, 1f));

					percent = percent*MathHelper.Clamp(1f-((i - 800) / 800f), 0f, 1f);
					float dista = distanceVector.Length() - i;
					percent = percent * MathHelper.Clamp(1f - ((dista) / 800f), 0f, 1f);

					//Vector2 therenow = (Vector2.Normalize(gothere-previousHere).RotatedBy(MathHelper.PiOver2)) * ((float)noisy.Noise((int)gothere.X, (int)gothere.Y)*72f);
					//Vector2 therenow = (Vector2.Normalize(distanceVector).RotatedBy(MathHelper.PiOver2)) * ((float)noisy.Noise((int)gothere.X - timePassed, (int)gothere.Y+timePassed) * percent));

					float sint = (float)Math.Sin(((timePassed + i) / sin1)+ asteriod.type);
					sint = sint + (float)Math.Sin(((-timePassed + i) / sin2) + asteriod.type) * 0.15f;
					sint = sint+(float)Math.Cos(((timePassed - i) / sin3) + asteriod.type) * 1.47f;

					Vector2 therenow = (Vector2.Normalize(distanceVector).RotatedBy(MathHelper.PiOver2)) * (sint * percent);

					gothere += therenow;

					previousHere = gothere;

					toThem.Add(gothere);

				}
				toThem.Add(beam);

				toThem.Reverse();

				TrailHelper trail = new TrailHelper("FadedBasicEffectPass", npc.ai[0]>999 ? mod.GetTexture("TiledPerlin") : SGAmod.ExtraTextures[21]);
				trail.projsize = Vector2.Zero;
				trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
				trail.trailThickness = 16;
				trail.trailThicknessIncrease = -12;
				trail.doFade = false;
				trail.color = delegate (float percent)
				{
					return (npc.ai[0] > 999 ? Color.CornflowerBlue : Color.White) * (1f-MathHelper.Clamp((percent-0.7f)*4f,0f,1f));
				};
				trail.DrawTrail(toThem, npc.Center);

			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			ArmorShaderData stardustsshader = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye);

			DrawData value8 = new DrawData(rocklarge, new Vector2(300f, 300f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 320, 320)), Microsoft.Xna.Framework.Color.White, npc.rotation, firerockorig, npc.scale, SpriteEffects.None, 0);
			stardustsshader.UseColor(Color.CornflowerBlue.ToVector3());
			stardustsshader.UseOpacity(0.5f);
			stardustsshader.Apply(null,new DrawData?(value8));


			spriteBatch.Draw(rocklarge, (npc.Center) - Main.screenPosition, new Rectangle(0,0, (int)rocklargeorig.X, (int)rocklargeorig.Y), Color.White, npc.rotation, rocklargeorig/2f, new Vector2(1f, 1f), SpriteEffects.None, 0f);

			//Eyes

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			//ArmorShaderData shader3 = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye); shader3.Apply(null);

			float percentive = (npc.ai[0] - 100000);

			Color EyeColor = Color.Lerp(Color.White, Color.Blue, MathHelper.Clamp(percentive / 250f, 0f, 1f));

			foreach (SpaceBossEye eye in Eyes)
			{
				Vector2 eyepos = Vector2.Normalize(eye.currentLook) * (eye.EyeDistance*eye.eyeMaxDistance);
				float scaleeye = MathHelper.Clamp(eye.sleep, 0f, 1f);

				if (scaleeye>0 && ! eye.exploded)
				spriteBatch.Draw(eyetex, (eye.BasePosition) + (eye.offset + eyepos) - Main.screenPosition,null, EyeColor, 0, eyetexorig, new Vector2(1f, scaleeye)*(eye.eyeScale), SpriteEffects.None, 0f);
			}

			eyetex = mod.GetTexture("Extra_57b");
			eyetexorig = eyetex.Size() / 2f;

			foreach (SpaceBossEye eye in Eyes)
			{
				Vector2 eyepos = Vector2.Normalize(eye.currentLook) * (eye.EyeDistance * eye.eyeMaxDistance);
				float scaleeye = MathHelper.Clamp(eye.sleep, 0f, 1f);

				if (scaleeye > 0 && !eye.exploded)
					spriteBatch.Draw(eyetex, (eye.BasePosition) + (eye.offset + eyepos) - Main.screenPosition, null, EyeColor * 0.50f, 0, eyetexorig, new Vector2(0.75f,0.75f*scaleeye) * (eye.eyeScale * Main.essScale), SpriteEffects.None, 0f);
			}

			//Shield

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D[] texs = { mod.GetTexture("Doom_Harbinger_Resprite_eye"), mod.GetTexture("Noise"), mod.GetTexture("TiledPerlin") };

			int count = 0;
			float shieldAlpha = MathHelper.Clamp(((shieldeffect) /30f),0f,1);
			float scale = 4f - (shieldAlpha *3f);

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
							spriteBatch.Draw(noise, (npc.Center + loc) - Main.screenPosition, null, ((npc.ai[0] > 999 ? Color.White : Color.PaleTurquoise) * 0.25f * shieldAlpha) * (1f - (effect)), angle, noise.Size() / 2f, ((new Vector2(200f, 150f) / noisesize) * (1f + effect * 5.00f)) * scale, SpriteEffects.None, 0f);
							continue;
						}
					}

					if (count == 0)
					{
						spriteBatch.Draw(noise, (npc.Center + loc) - Main.screenPosition, null, (npc.ai[0] > 999 ? Color.Blue : Color.PaleTurquoise) * shieldAlpha, angle, noise.Size() / 2f, (new Vector2(200f, 150f) / noisesize) * scale, SpriteEffects.None, 0f);
						spriteBatch.Draw(noise, (npc.Center + loc) - Main.screenPosition, null, (npc.ai[0] > 999 ? Color.Blue : Color.PaleTurquoise) * shieldAlpha, angle, noise.Size() / 2f, (-new Vector2(200f, 150f) / noisesize) * scale, SpriteEffects.None, 0f);
					}
					else
					{
						spriteBatch.Draw(noise, (npc.Center + loc) - Main.screenPosition, null, (npc.ai[0] > 999 ? Color.White : Color.CadetBlue) * 0.40f * shieldAlpha, angle, noise.Size() / 2f, (new Vector2(200f, 150f) / noisesize) * scale, SpriteEffects.None, 0f);
					}
					count += 1;
				}
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}
	}

	public class SpaceBossBeam : HellionBeam
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


				scale2 += 1f/ (float)timeWarning;


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
			if (Main.dedServ)
				return false;

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
			trail.trailThickness = 64 * scale.X*(projectile.width/64f);
			trail.color = delegate (float percent)
			{
				return Color.CornflowerBlue * scalez2.X*1.0f;
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
				return Color.CornflowerBlue * MathHelper.Clamp(scalez2.X * 2.5f,0f,0.75f);
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
			effect.Parameters["strength"].SetValue(MathHelper.Clamp((projectile.timeLeft-30) / 80f, 0f, projectile.ai[0] / 250f));

			float adder = projectile.localAI[0];//MathHelper.Clamp((projectile.ai[0] - 300f)/1f, 0f, 300f);

			VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[6];

			Vector3 screenPos = (projectile.Center - Main.screenPosition).ToVector3();
			float size = projectile.localAI[0]*3f;

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
				spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, (Color.White * 0.20f) * MathHelper.Clamp(projectile.timeLeft / 120f, 0f, MathHelper.Clamp(projectile.ai[0]/180f,0f,1f)), there, texture.Size() / 2f, new Vector2(0.85f, 1f) * ((Math.Abs(20f-((projectile.ai[0])/ 300f)*20f))+ (adder/60f)), SpriteEffects.None, 0f);
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

			Texture2D turrettex = mod.GetTexture("Dimensions/Space/Tether");
			Texture2D turrettexGlow = mod.GetTexture("Dimensions/Space/Tether_Glow");

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

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			base._DrawAsteriod(spriteBatch, lightColor);

		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//stuff
		}
	}
}