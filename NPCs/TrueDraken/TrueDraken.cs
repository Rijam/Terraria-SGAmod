#define TrueHellionUpdate

#if TrueHellionUpdate
using System.Linq;
using System;
using System.IO;
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
using SGAmod.NPCs.Hellion;
using Terraria.Audio;

namespace SGAmod.NPCs.TrueDraken
{
	[AutoloadHead]
	public class TrueDraken : ModNPC
	{

		private float[] oldRot = new float[8];
		private Vector2[] oldPos = new Vector2[8];
		float appear = 0.25f;

		int introstate = 0;
		int intro = 800;
		int StopMoving = 0;
		int NoFriction = 0;
		int Ramming = 0;
		float empowered = 0f;
		float circlepower = 0;
		int circlerad = 1000;
		int circletrap = 0;
		TrueDraken instance;
		Vector2 circleloc = Vector2.Zero;

		float stealth = 1f;

		public override bool Autoload(ref string name)
		{
			return true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("TRUE Draken");
			NPCID.Sets.MustAlwaysDraw[NPC.type] = true;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.650f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.6f);
		}

		public override void SetDefaults()
		{
			NPC.boss = true;
			NPC.friendly = false;
			NPC.width = 72;
			NPC.height = 72;
			NPC.aiStyle = -1;
			NPC.damage = 250;
			NPC.noTileCollide = true;
			NPC.defense = 0;
			NPC.netAlways = true;
			NPC.noGravity = true;
			NPC.lifeMax = 3000000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
		}

		public void GetHit(Player player, ref int damage, ref float knockback, ref bool crit)
		{
			int ogdamage = damage;
			if (NPC.HasBuff(BuffID.Ichor))
				damage += (int)(ogdamage * 0.15);
			if (NPC.HasBuff(BuffID.CursedInferno))
				damage += (int)(ogdamage * 0.15);
			if (NPC.HasBuff(BuffID.BetsysCurse))
				damage += (int)(ogdamage * 0.20);
			for (int i = 0; i < NPC.buffTime.Length; i += 1)
			{
				if (NPC.buffType[i] > 0)
				{
					damage += (int)(ogdamage * 0.05);
				}
			}
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Main.player[projectile.owner].active)
				GetHit(Main.player[projectile.owner], ref damage, ref knockback, ref crit);
		}

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			GetHit(player, ref damage, ref knockback, ref crit);
		}

		public override string Texture
		{
			get
			{
				return "SGAmod/NPCs/TownNPCs/DrakenFly";
			}
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return introstate > 1 && Ramming > 0;
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			intro = reader.ReadInt32();
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(intro);
		}

		public virtual void trailingeffect()
		{
			if (appear < 0.25f)
				appear += 0.005f;
			Rectangle hitbox = new Rectangle((int)NPC.position.X - 8, (int)NPC.position.Y - 8, NPC.height + 16, NPC.width + 16);

			for (int k = oldRot.Length - 1; k > 0; k--)
			{
				oldRot[k] = oldRot[k - 1];
				oldPos[k] = oldPos[k - 1];

				if (Main.rand.Next(0, 10) == 1)
				{
					int typr = Mod.Find<ModDust>("TornadoDust").Type;

					int dust = Dust.NewDust(new Vector2(oldPos[k].X, oldPos[k].Y), hitbox.Width, hitbox.Height, typr);
					Main.dust[dust].scale = (0.75f * appear) + (NPC.velocity.Length() / 50f);
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Vector2 normvel = NPC.velocity;
					normvel.Normalize(); normvel *= 2f;

					Main.dust[dust].velocity = ((randomcircle / 1f) + (-normvel)) - NPC.velocity;
					Main.dust[dust].noGravity = true;

				}

			}

			oldRot[0] = NPC.rotation;
			oldPos[0] = NPC.Center;
		}

		public override bool CheckActive()
		{
			return (!Main.player[NPC.target].active || Main.player[NPC.target].dead);
		}

		public override void AI()
		{

			if (Main.netMode < 1 && Main.LocalPlayer.name != "giuy")
				NPC.active = false;

			circletrap -= 1;
			circlepower = MathHelper.Clamp(circlepower + (circletrap > 0 ? 0.02f : -0.02f), 0f, 1f);
			intro += 1;
			NPC.dontTakeDamage = false;
			NPC.chaseable = true;
			stealth = Math.Min(1f, stealth+0.01f);
			if (introstate > 0)
				trailingeffect();
			if (DoingIntro())
			{
				NPC.dontTakeDamage = true;
				return;

			}

			if (!LivePlayer())
			{
				NPC.velocity.X *= 0.75f;
				NPC.velocity.Y -= 0.10f;
				return;
			}

			NPC.ai[0] += 1;

			NoEscape();

			if (NPC.ai[1] < 1)
				NPC.ai[0] = NPC.ai[0] % 1000;
			if (NPC.ai[1]>1 && (intro>1020) && empowered<1f)
				empowered+=0.01f;

				Player P = Main.player[NPC.target];
			Vector2 PWhere = P.Center;
			Vector2 DrakenWhere = NPC.Center;
			Vector2 FlyTo = PWhere - new Vector2(0, 200);
			Vector2 FlySpeed = new Vector2(0.10f, 0.10f);
			Vector2 FlyFriction = new Vector2(0.95f, 0.95f);
			StopMoving -= 1; NoFriction -= 1; Ramming -= 1;

			if (NPC.life < NPC.lifeMax * 0.90 && NPC.ai[1] == 10)//0
			{
				NPC.ai[1] = 1;
				intro = 960;
				NPC.netUpdate = true;
			}
			if (NPC.life < NPC.lifeMax * 0.950 && NPC.ai[1] == 0)//1
			{
				NPC.ai[1] = 2;
				NPC.life = NPC.lifeMax;
				intro = 960;
				NPC.netUpdate = true;
			}
			Vector2 Turntof = PWhere - DrakenWhere;
			Turntof.Normalize();


			if (NPC.ai[1] == 0 && NPC.ai[0]%200==190 && NPC.ai[0]>0)
			{
				StopMoving = 120;
				HellionPortals(P, ref PWhere, ref DrakenWhere, ref FlyTo, ref FlySpeed, ref FlyFriction);
				goto Movementstuff;

			}

			if (NPC.ai[1] == 2 && NPC.ai[0] < 1)
			{
				Phase2Touhou(P, ref PWhere, ref DrakenWhere, ref FlyTo, ref FlySpeed, ref FlyFriction,true);
				goto Movementstuff;

			}

			if (NPC.ai[0] % 2800 > 2200)
			{
				LaserBarrage(P, ref PWhere, ref DrakenWhere, ref FlyTo, ref FlySpeed, ref FlyFriction);
			}
			else
			{
				if ((NPC.ai[0] % 400 > 350 || NPC.ai[0] % 710 > 650) && NPC.ai[1] > 0)
				{
					SpinCycle(P, ref PWhere, ref DrakenWhere, ref FlyTo, ref FlySpeed, ref FlyFriction);
				}
				else
				{
					if (NPC.ai[0] % 1000 > 400)
					{
						Phase1Hover(P, ref PWhere, ref DrakenWhere, ref FlyTo, ref FlySpeed, ref FlyFriction);
					}
					else
					{
						Phase1Dash(P, ref PWhere, ref DrakenWhere, ref FlyTo, ref FlySpeed, ref FlyFriction);
					}
				}

			}



			Movementstuff:
			if (StopMoving < 1)
			{
				Vector2 Thereis = (FlyTo - DrakenWhere);
				if (FlySpeed.X < 0)
				{
					Thereis.Normalize();
					NPC.velocity += Thereis * -FlySpeed;
				}
				else
				{
					NPC.velocity += (Thereis) * FlySpeed;
				}

			}
			if (NoFriction < 1)
				NPC.velocity *= FlyFriction;


		}

		public void HellionPortals(Player P, ref Vector2 PWhere, ref Vector2 DrakenWhere, ref Vector2 FlyTo, ref Vector2 FlySpeed, ref Vector2 FlyFriction)
		{

			for (int rotz = 0; rotz < 360; rotz += 360 / 3)
			{

				Vector2 where = NPC.Center;
				Vector2 wheretogo2 = new Vector2(96f, rotz);
				Vector2 where2 = P.Center - NPC.Center;
				where2.Normalize();
				Vector2 wheretogoxxx = new Vector2(0.40f, 0f);
				Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
				{
					Vector2 wheretogoxxx2 = new Vector2(wheretogoxxx.X, wheretogoxxx.Y);
					float val = current;
					val = current.AngleLerp((playerpos - projpos).ToRotation(), wheretogoxxx2.X);

					return val;
				};
				Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
				{
					Vector2 wheretogo = new Vector2(wheretogo2.X, wheretogo2.Y);
					float angle = MathHelper.ToRadians(((wheretogo.Y + time * 2f)));
					Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

					if (TrueDraken.GetDraken() != null)
					{
						Vector2 gothere = TrueDraken.GetDraken().NPC.Center + instore;
						Vector2 slideover = gothere - projpos;
						current = slideover / 2f;
					}

					current /= 1.125f;

					Vector2 speedz = current;
					float spzzed = speedz.Length();
					speedz.Normalize();
					if (spzzed > 50f)
						current = (speedz * spzzed);

					return current;
				};
				Func<float, bool> projectilepattern = (time) => (time > 60 && time%200==0);

				int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 100, 1100, (NPC.Center - where).ToRotation(), ProjectileID.EmeraldBolt, projectilepattern, 15f, 200);
				(Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilefacing = projectilefacing;
				(Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilemoving = projectilemoving;
				SoundEngine.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
				Main.projectile[ize2].netUpdate = true;

			}


		}

		public void Phase2Touhou(Player P, ref Vector2 PWhere, ref Vector2 DrakenWhere, ref Vector2 FlyTo, ref Vector2 FlySpeed, ref Vector2 FlyFriction, bool introattack = false)
		{

			if (circletrap < 5)
			{
				circleloc = P.Center;
			}

			circletrap = 30;

			//Touhou Attack
			NPC.chaseable = false;
			Vector2 Turnto = PWhere - DrakenWhere;
			appear /= 1.15f;
			stealth = stealth/1.15f;
			if (introattack)
				NPC.dontTakeDamage = true;
			if ((NPC.ai[0]) % 100 == 0)
			{
				float i = MathHelper.ToDegrees(Turnto.ToRotation());
					int prog = Projectile.NewProjectile(introattack ? circleloc : NPC.Center, ((MathHelper.ToRadians(i)).ToRotationVector2() * 12f), Mod.Find<ModProjectile>("TrueDrakenCopyNoDeath").Type, 0, 1f, 255, 0, NPC.whoAmI);
					Main.projectile[prog].ai[1] = NPC.whoAmI;

					Func<Vector2, TrueDrakenCopy, int, bool> ProjectileAct = delegate (Vector2 playerpos, TrueDrakenCopy DrakenCopy, int timer)
							{
								Vector2 rotspeed = new Vector2(NPC.ai[0] % 200 >= 100 ? 0.25f : -0.25f, 0.75f);
								DrakenCopy.Projectile.ai[0] += 1;
								if (DrakenCopy.Projectile.timeLeft > 300)
									DrakenCopy.Projectile.timeLeft = 300;

								float arot = DrakenCopy.Projectile.velocity.ToRotation();
								float ogvel = DrakenCopy.Projectile.velocity.Length();
								if (DrakenCopy.Projectile.ai[0] < 2)
								{
									DrakenCopy.alocation = DrakenCopy.Projectile.Center;
									DrakenCopy.Projectile.rotation = arot;
								}

								Vector2 ShootThere = DrakenCopy.alocation - DrakenCopy.Projectile.Center;
								ShootThere.Normalize();

								float len = DrakenCopy.Projectile.velocity.Length();

								DrakenCopy.Projectile.rotation += MathHelper.ToRadians(rotspeed.X* rotspeed.Y);
								Vector2 Gowhere = DrakenCopy.Projectile.rotation.ToRotationVector2();
								if (ShootThere.Length() > 0)
									DrakenCopy.Projectile.Center -= ShootThere * 6f;
								DrakenCopy.Projectile.velocity = Gowhere * ogvel;

								if (DrakenCopy.Projectile.ai[0] % 30 == 0 && (DrakenCopy.Projectile.ai[0] > 100))
								{
									int prog2 = Projectile.NewProjectile(DrakenCopy.Projectile.Center, ShootThere * 15f, Mod.Find<ModProjectile>("TrueDrakenCopyNoDeath").Type, 80, 1f, 255, 150, NPC.whoAmI);
									Main.projectile[prog2].ai[0] = 200;
									Main.projectile[prog2].ai[1] = NPC.whoAmI;
									Main.projectile[prog2].rotation = -ShootThere.ToRotation();
									Main.projectile[prog2].timeLeft = 900;
									Main.projectile[prog2].netUpdate = true;
									SoundEngine.PlaySound(SoundID.NPCHit, (int)NPC.Center.X, (int)NPC.Center.Y, 37, 0.75f, 0.5f);
								}


								return true;
							};


					(Main.projectile[prog].ModProjectile as TrueDrakenCopy).ProjectileAct = ProjectileAct;
					Main.projectile[prog].netUpdate = true;

				//}

				SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 84, 0.75f, 0.5f);
			}

			//if (npc.ai[0] % 90 > 20)
				NPC.rotation = NPC.rotation.AngleLerp((Turnto).ToRotation(), 0.04f);

			FlyTo = circleloc;

			if (!introattack)
			{
				FlyTo = circleloc - new Vector2(256, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[0] * 7f));
				FlySpeed /= 2f;
			}
			else
			{
				FlySpeed /= 3.5f;
			}
		}

		public void Phase1Dash(Player P, ref Vector2 PWhere, ref Vector2 DrakenWhere, ref Vector2 FlyTo, ref Vector2 FlySpeed, ref Vector2 FlyFriction)
		{

			//Charging
			Vector2 Turnto = PWhere - DrakenWhere;
			if (NPC.ai[0] % 80 == 30)
			{
				Turnto = SGAUtils.PredictiveAim(100f, NPC.Center,P.MountedCenter,P.velocity, false) - DrakenWhere;
				Turnto.Normalize();
				NPC.velocity = Turnto * 100f;
				//npc.velocity += Turnto * 70f;
				NPC.rotation = Turnto.ToRotation();
				NPC.spriteDirection = (NPC.velocity.X) > 0 ? 1 : -1;
				Ramming = 30;
			}
			if (NPC.ai[0] % 80 == 0)
			{
				Turnto = Turnto.RotatedBy(MathHelper.ToRadians((Main.rand.Next(30, 60) * (Main.rand.NextBool() ? 1 : 1))));
				Turnto.Normalize();
				NPC.velocity = Turnto * 80f;
				NPC.velocity += Turnto * 30f;
				NPC.rotation = Turnto.ToRotation();
				NPC.spriteDirection = (NPC.velocity.X) > 0 ? 1 : -1;
				Ramming = 32;
			}
			if (NPC.ai[0] % 90 > 20)
				NPC.rotation = NPC.rotation.AngleLerp((Turnto).ToRotation(), 0.15f);
			StopMoving = 80;

		}

		public void SpinCycle(Player P, ref Vector2 PWhere, ref Vector2 DrakenWhere, ref Vector2 FlyTo, ref Vector2 FlySpeed, ref Vector2 FlyFriction)
		{
			//Draken Clone Spin Cycle
			Vector2 Turnto = PWhere - DrakenWhere;
			NPC.rotation = NPC.rotation.AngleLerp((Turnto).ToRotation(), 0.025f);
			Vector2 shootthere = SGAUtils.PredictiveAim(30f, NPC.Center, P.MountedCenter, P.velocity, false);
			NPC.Center = NPC.Center.RotatedBy(MathHelper.ToRadians(NPC.ai[0] % 800 > 400 ? 5f : -5f), PWhere);
			shootthere -= DrakenWhere;
			shootthere.Normalize();
			if (StopMoving < 3)
				StopMoving = 3;
			if (NPC.ai[0] % 3 == 0 && (NPC.ai[0] % 400 > 375 || NPC.ai[0] % 710 > 675))
			{
				int prog = Projectile.NewProjectile(NPC.Center, shootthere * 15f, Mod.Find<ModProjectile>("TrueDrakenCopy").Type, 80, 1f, 255, 150, NPC.whoAmI);
				Main.projectile[prog].ai[0] = 150;
				Main.projectile[prog].ai[1] = NPC.whoAmI;
				Main.projectile[prog].netUpdate = true;
				SoundEngine.PlaySound(SoundID.NPCHit, (int)NPC.Center.X, (int)NPC.Center.Y, 37, 0.75f, 0.5f);
			}

		}

		public void Phase1Hover(Player P, ref Vector2 PWhere, ref Vector2 DrakenWhere, ref Vector2 FlyTo, ref Vector2 FlySpeed, ref Vector2 FlyFriction)
		{

			//Hovering

			FlyFriction = new Vector2(0.95f, 0.95f);
			FlyTo = PWhere - new Vector2((float)Math.Sin(NPC.ai[0] / 73f) * 400f, 250);
			Vector2 Turnto = PWhere - DrakenWhere;
			Turnto.X *= 3f;
			NPC.rotation = NPC.rotation.AngleLerp((Turnto).ToRotation(), 0.025f);
			NPC.spriteDirection = (FlyTo.X - DrakenWhere.X) > 0 ? 1 : -1;
			FlySpeed = new Vector2(-8.20f, -8.20f) * Math.Min((FlyTo - DrakenWhere).Length() / 400f, 1f);

			Vector2 shootthere = PWhere - DrakenWhere;
			if (NPC.ai[0] % 10 == 0 && NPC.ai[0] % 100 > 60)
			{
				shootthere.Normalize();
				int prog = Projectile.NewProjectile(NPC.Center, shootthere * 30f, ProjectileID.CursedFlameHostile, 80, 1f);
			}
			if (NPC.ai[0] % 5 == 0 && NPC.ai[0] % 100 > 60 && NPC.ai[1] == 0)
				Idglib.Shattershots(NPC.Center, PWhere, new Vector2(0, 0), ProjectileID.CursedFlameHostile, 80, 50, 100, 3, false, 0, false, 100);

		}

		public void LaserBarrage(Player P, ref Vector2 PWhere, ref Vector2 DrakenWhere, ref Vector2 FlyTo, ref Vector2 FlySpeed, ref Vector2 FlyFriction)
		{
			//Laser Barrage
			StopMoving = 160;
			if (NPC.ai[0] % 2800 < 2300)
			{
				Vector2 Turnto = PWhere - DrakenWhere;
				Turnto.Normalize();
				NPC.velocity -= Turnto * 1f;
				NPC.rotation = NPC.rotation.AngleLerp(Turnto.ToRotation(), 0.15f);
			}
			else
			{
				Vector2 Turnto = PWhere - DrakenWhere;
				for (int i = -12; i < 5; i += 4)
				{
					if (NPC.ai[0] % 3 == 0 && NPC.ai[0] % 150 <= 25)
					{
						Vector2 Turnto2 = (PWhere - DrakenWhere);
						Turnto2 = Turnto2.RotatedBy(MathHelper.ToRadians(i));
						Turnto2.Normalize();

						int prog = Projectile.NewProjectile(NPC.Center, Turnto2 * 8f, SGAmod.Instance.Find<ModProjectile>("HellionBeam").Type, 100, 15f);


					}
				}
				NPC.Center = NPC.Center.RotatedBy(MathHelper.ToRadians(4), PWhere);
				Turnto.Normalize();
				NPC.velocity /= 1.15f;
				NPC.rotation = Turnto.ToRotation();
			}
		}
			public bool DoingIntro()
		{
			introstate = 0;
			if (intro < 1200)
			{
				if (intro>=950)
					introstate = 1;
				if (intro < 200)
				{
					NPC.velocity.Y -= 0.5f;
				}
				if (intro == 100)
					Idglib.Chat("Draken: ...", 0, 200, 0);
				if (intro == 200)
					Idglib.Chat("Draken: I...", 0, 200, 0);
				if (intro == 400)
					Idglib.Chat("Draken: Trusted you...", 0, 200, 0);
				if (intro == 600)
					Idglib.Chat("Draken: How could...", 0, 200, 0);
				if (intro == 700)
					Idglib.Chat("Draken: You do this?", 0, 200, 0);
				if (intro == 800)
					Idglib.Chat("Draken: how Could...!", 0, 200, 0);
				if (intro == 950)
				{
					Idglib.Chat("True Draken: YOU?!!", 200, 0, 0);
					RippleBoom.MakeShockwave(NPC.Center, 8f, 2f, 20f, 100, 3f, true);
					SoundEngine.PlaySound(15, (int)NPC.Center.X, (int)NPC.Center.Y, 2, 1f, -0.5f);
					NPC.velocity.Y = 0;
				}
				if (NPC.ai[1] < 1)
				{
					if (intro == 1100)
						Idglib.Chat("True Draken: Justice shall fall down on you!", 200, 0, 0);
					if (intro == 1199)
						Idglib.Chat("True Draken: Repent!", 200, 0, 0);
				}

				if (NPC.ai[1] == 1)
				{
					NPC.ai[0] = 2200;
					if (intro == 1000)
						Idglib.Chat("True Draken: What do you hope to gain?", 200, 0, 0);
					if (intro == 1150)
						Idglib.Chat("True Draken: Tell me what?!", 200, 0, 0);
				}
				if (NPC.ai[1] == 2)
				{
					NPC.ai[0] = -999;
					if (intro == 1000)
						Idglib.Chat("True Draken: I won't let you...", 200, 0, 0);
					if (intro == 1150)
						Idglib.Chat("True Draken: I WON'T LET YOU!!", 200, 0, 0);
				}

				if (intro > 700 && intro < 950)
				{
					NPC.velocity.Y -= (intro - 700f) / 1000f;
				}
				NPC.velocity /= 1.5f;
				return true;
			}
			introstate = 2;
			return false;


		}

		/*
		//From Joost, get perms yeah
		private Vector2 PredictiveAim(float speed, Vector2 origin, bool ignoreY)
		{
			Player P = Main.player[npc.target];
			Vector2 vel = (ignoreY ? new Vector2(P.velocity.X, 0) : P.velocity);
			Vector2 predictedPos = P.MountedCenter + P.velocity + (vel * (Vector2.Distance(P.MountedCenter, origin) / speed));
			predictedPos = P.MountedCenter + P.velocity + (vel * (Vector2.Distance(predictedPos, origin) / speed));
			//predictedPos = P.MountedCenter + P.velocity + (vel * (Vector2.Distance(predictedPos, origin) / speed));
			return predictedPos;
		}
		*/

		public static TrueDraken GetDraken()
		{
			if (NPC.CountNPCS(SGAmod.Instance.Find<ModNPC>("TrueDraken").Type) > 0)
			{
				return (Main.npc[NPC.FindFirstNPC(SGAmod.Instance.Find<ModNPC>("TrueDraken").Type)].ModNPC as TrueDraken);
			}
			else
			{
				return (null);
			}

		}

		public bool LivePlayer()
		{

			Player P = Main.player[NPC.target];
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(false);
				P = Main.player[NPC.target];
				if (!P.active || P.dead)
				{
					return false;
				}
			}
			return true;

		}

		public void NoEscape()
		{
			if (circlepower > 0.99f)
			{
				for (int i = 0; i <= Main.maxPlayers; i++)
				{
					Player thatplayer = Main.player[i];

					if (thatplayer.active && !thatplayer.dead)
					{
						Vector2 gohere = (thatplayer.Center- circleloc);
						if (gohere.Length() > circlerad)
						{
							float dist = gohere.Length() - circlerad;
							gohere.Normalize();

							Vector2 there = circleloc+(gohere* (circlerad-160f));

							thatplayer.Teleport(there,0);
						}
					}
				}

			}
		}


	public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{

			Texture2D tex = ModContent.Request<Texture2D>("SGAmod/NPCs/TownNPCs/DrakenFly");
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((NPC.Center - Main.screenPosition));
			Vector2 adder = Vector2.Zero;
			Color color = drawColor;
			int timing = (int)(Main.GlobalTimeWrappedHourly * (8f));
			if (introstate < 1)
				timing = (int)(Main.GlobalTimeWrappedHourly * 8f);

			timing %= 4;

			int mydirection = NPC.rotation.ToRotationVector2().X > 0 ? 1 : -1;

			if (timing == 0)
			{
				adder = ((NPC.rotation + (float)Math.PI / 2f).ToRotationVector2() * (8f * mydirection));
			}


			timing *= ((tex.Height) / 4);
			if (introstate < 1)
			{
				spriteBatch.Draw(tex, drawPos - adder, new Rectangle(0, timing + 2, tex.Width, (tex.Height - 1) / 4), color, 0, drawOrigin, NPC.scale, (NPC.Center - Main.LocalPlayer.Center).X < 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
			else
			{
				if (empowered > 0f)
				{
					for (int i = -1; i < 2; i += 2)
					{
						Texture2D texture7 = SGAmod.ExtraTextures[34];
						spriteBatch.Draw(texture7, NPC.Center - Main.screenPosition, null, Main.hslToRgb((Main.GlobalTimeWrappedHourly) % 1f, 1f, 0.75f) * 0.50f * empowered * stealth, -Main.GlobalTimeWrappedHourly * 17.134f * i, new Vector2(texture7.Width / 2f, texture7.Height / 2f), new Vector2((float)Math.Abs(Math.Sin(Main.GlobalTimeWrappedHourly / 1.1694794f)), 1f) * empowered, SpriteEffects.None, 0f);
						texture7 = SGAmod.HellionTextures[6];
						spriteBatch.Draw(texture7, NPC.Center - Main.screenPosition, null, Main.hslToRgb((Main.GlobalTimeWrappedHourly) % 1f, 1f, 0.75f) * 0.50f * empowered * stealth, Main.GlobalTimeWrappedHourly * 17.134f * i, new Vector2(texture7.Width / 2f, texture7.Height / 2f), new Vector2((float)Math.Abs(Math.Sin(Main.GlobalTimeWrappedHourly / 1.1694794f)), 1f) * empowered, SpriteEffects.None, 0f);
					}
				}

				for (int k = oldRot.Length - 1; k >= 0; k -= 1)
				{


					//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
					float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
					float alphaz2 = Math.Max((0.75f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f, 0f);
					for (float xx = 0; xx < 1f; xx += 0.05f)
					{
						float scaleffect = 2f - ((k + xx) / oldRot.Length);
						drawPos = ((oldPos[k] - Main.screenPosition)) + (NPC.velocity * xx);
						spriteBatch.Draw(tex, drawPos - adder, new Rectangle(0, timing + 2, tex.Width, (tex.Height - 1) / 4), ((Color.Lerp(drawColor, Main.hslToRgb(((k / oldRot.Length) + Main.GlobalTimeWrappedHourly) % 1f, 1f, 0.75f), alphaz2) * alphaz) * (appear)) * 0.25f, NPC.rotation - (float)(mydirection < 0 ? Math.PI : 0), drawOrigin, scaleffect, mydirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}

				drawPos = ((NPC.Center - Main.screenPosition));
				spriteBatch.Draw(tex, drawPos - adder, new Rectangle(0, timing + 2, tex.Width, (tex.Height - 1) / 4), Color.White * stealth, NPC.rotation - (float)(mydirection < 0 ? Math.PI : 0), drawOrigin, NPC.scale, mydirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				Texture2D texture6 = SGAmod.ExtraTextures[96];
				spriteBatch.Draw(texture6, NPC.Center - Main.screenPosition, null, Main.hslToRgb((Main.GlobalTimeWrappedHourly) % 1f, 1f, 0.75f) * 0.50f * empowered * stealth, Main.GlobalTimeWrappedHourly * 37.134f, new Vector2(texture6.Width / 2f, texture6.Height / 2f), new Vector2((float)Math.Abs(Math.Sin(Main.GlobalTimeWrappedHourly * 1.694794f)), 3f), SpriteEffects.None, 0f);



			}


			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 drawPos = circleloc - Main.screenPosition;
			float inrc = Main.GlobalTimeWrappedHourly / 310f;

			Texture2D tex = ModContent.Request<Texture2D>("SGAmod/NPCs/TownNPCs/DrakenFly");
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;

			int timing = (int)(Main.GlobalTimeWrappedHourly * (10f));
			timing %= 4;
			timing *= ((tex.Height) / 4);

			if (circlepower > 0)
			{
				float alphaeffect = MathHelper.Clamp(circlepower, 0f, 1f);

				for (float a = 5; a > 0; a -= 0.2f) {

					for (float i = 0; i < 360; i += 18)
					{
						float angle = MathHelper.ToRadians((float)(i-a) + (Main.GlobalTimeWrappedHourly * -100f));
						float dist = (circlerad / 2f)+((float)(circlerad) * (circlepower/2f));
						Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist), (float)(Math.Sin(angle) * dist));

						Vector2 drawthere = (circleloc + thisloc);

						//Rectangle rect = new Rectangle((int)drawthere.X - 128, (int)drawthere.Y - 128, (int)drawthere.X + 128, (int)drawthere.Y + 128);
						Vector2 calc1 = drawthere - Main.screenPosition;
						int boundingsize = 160;
						if (calc1.X > -boundingsize && calc1.Y > -boundingsize && calc1.X < Main.screenWidth + boundingsize && calc1.Y < Main.screenWidth + boundingsize)
						{

							Color glowingcolors1 = Main.hslToRgb((float)(((float)i / 720f) + Main.GlobalTimeWrappedHourly / 5f) % 1, 0.9f, 0.65f);

							spriteBatch.Draw(tex, drawthere - Main.screenPosition, new Rectangle(0, timing + 2, tex.Width, (tex.Height - 1) / 4), glowingcolors1 * alphaeffect*0.2f, angle + MathHelper.ToRadians(90), drawOrigin, circlepower*((a+5f)/10f), SpriteEffects.FlipHorizontally, 0f);

						}
					}
				}

			}
		}


	}

		public class TrueDrakenCopy : ModProjectile
	{

		public Vector2 alocation;

		public Func<Vector2, TrueDrakenCopy, int, bool> ProjectileAct = delegate (Vector2 playerpos, TrueDrakenCopy DrakenCopy, int timer)
		{

			float len = DrakenCopy.Projectile.velocity.Length();
			Vector2 Gowhere = playerpos - DrakenCopy.Projectile.Center;
			if (timer == DrakenCopy.Projectile.ai[0])
			{
				Gowhere.Normalize();
				DrakenCopy.Projectile.velocity = DrakenCopy.Projectile.rotation.ToRotationVector2() * len;

			}
			if (timer < DrakenCopy.Projectile.ai[0])
			{
				DrakenCopy.Projectile.rotation = DrakenCopy.Projectile.rotation.AngleLerp(Gowhere.ToRotation(), 0.10f);
				DrakenCopy.Projectile.position -= DrakenCopy.Projectile.velocity;
			}

			return true;
		};

		public Func<TrueDrakenCopy, int, bool> ProjectileDie = delegate (TrueDrakenCopy DrakenCopy, int timer)
		{
			SoundEngine.PlaySound(SoundID.Item45, DrakenCopy.Projectile.Center);
			for (int i = 0; i < 360; i+=360/6)
			{
				Vector2 perturbedSpeed = new Vector2(DrakenCopy.Projectile.velocity.X, DrakenCopy.Projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(i));
				int hoste=Projectile.NewProjectile(DrakenCopy.Projectile.Center.X, DrakenCopy.Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.SwordBeam, 40, DrakenCopy.Projectile.knockBack, DrakenCopy.Projectile.owner, 0f, 0f);
				Main.projectile[hoste].hostile = true;
				Main.projectile[hoste].friendly = false;
				Main.projectile[hoste].netUpdate = true;

			}
			return true;
		};

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.friendly = reader.ReadBoolean();
			Projectile.hostile = reader.ReadBoolean();
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.friendly);
			writer.Write(Projectile.hostile);
		}

		int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Draken's Pain");
		}

		private float[] oldRot = new float[12];
		private Vector2[] oldPos = new Vector2[12];
		float appear = 0.5f;

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			AIType = ProjectileID.Boulder;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.penetrate = 10;
			Projectile.light = 0.5f;
			Projectile.timeLeft = 400;
			Projectile.width = 64;
			Projectile.height = 64;
			Projectile.aiStyle = -99;
			Projectile.extraUpdates = 2;
			Projectile.tileCollide = false;
		}
		public override bool PreKill(int timeLeft)
		{
			ProjectileDie(this, timeLeft);
			return true;
		}

		public override void AI()
		{
			appear = Math.Max(Math.Min(0.75f, (float)timer / 200f), (float)Projectile.timeLeft / 400f);
			trailingeffect();
			NPC master = Main.npc[(int)Projectile.ai[1]];

				Player player = Main.player[master.target];

			timer += 1;

			if (ProjectileAct(player.Center, this, timer))
			{

				int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("TornadoDust").Type, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 20, Color.Lime * 0.25f, 0.15f);
				Main.dust[DustID2].noGravity = true;

			}


		}
		public override string Texture
		{
			get { return ("Terraria/Projectile_" + ProjectileID.DD2PetDragon); }
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{

			Texture2D tex = ModContent.Request<Texture2D>("SGAmod/NPCs/TownNPCs/DrakenFly");
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition));
			Vector2 adder = Vector2.Zero;
			Color color = drawColor;
			int timing = (int)(Main.GlobalTimeWrappedHourly * (8f));

			timing %= 4;

			int mydirection = Projectile.rotation.ToRotationVector2().X > 0 ? 1 : -1;

			if (timing == 0)
			{
				adder = ((Projectile.rotation + (float)Math.PI / 2f).ToRotationVector2() * (8f * mydirection));
			}


			timing *= ((tex.Height) / 4);

				for (int k = oldRot.Length - 1; k >= 0; k -= 1)
				{


					//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
					float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
					float alphaz2 = Math.Max((0.75f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f, 0f);
				float scaleffect = 1f;
						drawPos = ((oldPos[k] - Main.screenPosition));
						spriteBatch.Draw(tex, drawPos - adder, new Rectangle(0, timing + 2, tex.Width, (tex.Height - 1) / 4), ((Color.Lerp(drawColor, Main.hslToRgb(((k / oldRot.Length) + Main.GlobalTimeWrappedHourly) % 1f, 1f, 0.75f), alphaz2) * alphaz) * (appear)) * 0.35f, Projectile.rotation - (float)(mydirection < 0 ? Math.PI : 0), drawOrigin, scaleffect, mydirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				}


			return false;
		}

		public virtual void trailingeffect()
		{

			Rectangle hitbox = new Rectangle((int)Projectile.position.X - 5, (int)Projectile.position.Y - 5, Projectile.height + 10, Projectile.width + 10);

			for (int k = oldRot.Length - 1; k > 0; k--)
			{
				oldRot[k] = oldRot[k - 1];
				oldPos[k] = oldPos[k - 1];

			}

			oldRot[0] = Projectile.rotation;
			oldPos[0] = Projectile.Center;
		}
	}

	public class TrueDrakenCopyNoDeath : TrueDrakenCopy
	{

		public override bool PreKill(int timeLeft)
		{
			//null;
			return true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			ProjectileAct = delegate (Vector2 playerpos, TrueDrakenCopy DrakenCopy, int timer)
			{

				float len = DrakenCopy.Projectile.velocity.Length();
				Vector2 Gowhere = DrakenCopy.Projectile.velocity - DrakenCopy.Projectile.rotation.ToRotationVector2();
				if (timer == DrakenCopy.Projectile.ai[0])
				{
					Gowhere.Normalize();
					DrakenCopy.Projectile.velocity = DrakenCopy.Projectile.rotation.ToRotationVector2() * len;

				}
				if (timer < DrakenCopy.Projectile.ai[0])
				{
					DrakenCopy.Projectile.rotation = DrakenCopy.Projectile.rotation.AngleLerp(Gowhere.ToRotation(), 0.10f);
					DrakenCopy.Projectile.position -= DrakenCopy.Projectile.velocity;
				}

				return true;
			};
		}

	}

}
 
#endif