using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Items;
using Terraria.Audio;

namespace SGAmod.NPCs.SpiderQueen
{
	[AutoloadBossHead]
	public class SpiderQueen : ModNPC, ISGABoss
	{
		public string Trophy() => "SpiderQueenTrophy";
		public bool Chance() => Main.rand.Next(0, 10) == 0;
		public string RelicName() => "Spider_Queen";
		public void NoHitDrops() { }
		public string MasterPet() => "SpiderlingEggs";
		public bool PetChance() => Main.rand.Next(4) == 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spider Queen");
		}
		public override void SetDefaults()
		{
			NPC.width = 48;
			NPC.height = 48;
			NPC.damage = 70;
			NPC.defense = 5;
			NPC.boss = true;
			NPC.lifeMax = 5000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 50000f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			AIType = 0;
			music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/SpiderQueen");
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.value = 25000f;
			bossBag = Mod.Find<ModItem>("SpiderBag").Type;
		}

		public override void NPCLoot()
		{
			SGAWorld.downedSpiderQueen = true;
			Achivements.SGAAchivements.UnlockAchivement("Spider Queen", Main.LocalPlayer);
			if (Main.expertMode)
			{
				NPC.DropBossBags();
				return;
			}
			else
			{
				for (int i = 0; i <= Main.rand.Next(25, 45); i++)
				{
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height,ModContent.ItemType<VialofAcid>());
				}
				if (Main.rand.Next(0, 3) == 0)
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Weapons.Shields.CorrodedShield>());

				if (Main.rand.Next(7) == 0)
				{
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Armors.Vanity.SpiderQueenMask>());
				}
        
        				if (Main.rand.Next(0, 3) == 0)
					Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Accessories.AmberGlowSkull>());
			}

		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.625f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.6f);
		}

		public void GetAngleDifferenceBlushiMagic(Vector2 targetPos, out float angle1, out float angle2)
		{

			Vector2 offset = targetPos - NPC.Center;
			float rotation = MathHelper.PiOver2;
			if (offset != Vector2.Zero)
			{
				rotation = offset.ToRotation();
			}
			targetPos = Main.player[(int)NPC.target].Center;
			offset = targetPos - NPC.Center;
			float newRotation = MathHelper.PiOver2;
			if (offset != Vector2.Zero)
			{
				newRotation = offset.ToRotation();
			}
			if (newRotation < rotation - MathHelper.Pi)
			{
				newRotation += MathHelper.TwoPi;
			}
			else if (newRotation > rotation + MathHelper.Pi)
			{
				newRotation -= MathHelper.TwoPi;
			}

			angle1 = rotation;
			angle2 = newRotation;

		}

		public int phase
		{
			get
			{
				return (int)NPC.ai[1];

			}
			set
			{
				NPC.ai[1] = (int)value;

			}
		}

		public void DoPhase(int phasetype)
		{
			if (phasetype > 0)
			{
				if (phase == 1)
				{
					if (NPC.life < NPC.lifeMax * (Main.expertMode ? 0.5f : 0.33f))
					{
						NPC.ai[0] = 10000;
						phase = 2;
						return;
					}
				}
				if (phase == 0)
				{
					NPC.ai[0] = 10000;
					phase = 1;
					return;
				}


				//Phase 2-Charging
				if (NPC.ai[0] > 1999 && NPC.ai[0] < 3000)
				{

					if (NPC.ai[0] > 2998)
					{
						NPC.ai[0] = 0;
						return;
					}
					if (NPC.ai[0] % 210 < 90 && NPC.ai[0] % 210 > 25)
					{
						NPC.rotation = NPC.rotation.AngleLerp((P.Center - NPC.Center).ToRotation(), 0.15f);
						if (NPC.ai[0] % 20 == 0 && Main.expertMode)
						{
							Idglib.Shattershots(NPC.Center + NPC.rotation.ToRotationVector2() * 32, NPC.Center + NPC.rotation.ToRotationVector2() * 200, new Vector2(0, 0), ProjectileID.WebSpit, 15, 20, 35, 1, true, 0, false, 1600);
							SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 102, 0.25f, -0.25f);
						}

					}
					if (NPC.ai[0] % 210 > 100 && NPC.ai[0] % 210 < 200)
					{
						charge = true;
						if (NPC.ai[0] % 210 == 105)
							SoundEngine.PlaySound(SoundID.Roar, (int)NPC.Center.X, (int)NPC.Center.Y, 0, 1f, 0.25f);
						NPC.velocity += NPC.rotation.ToRotationVector2() * 2f;

						if ((NPC.ai[0] % (Main.expertMode ? 15 : 20)) == 0 && phase > 1)
						{
							Idglib.Shattershots(NPC.Center, NPC.Center + (NPC.rotation + MathHelper.Pi/2f).ToRotationVector2() * 64, new Vector2(0, 0), Mod.Find<ModProjectile>("SpiderVenom").Type, 10, 7, 35, 1, true, 0, true, 1600);
							Idglib.Shattershots(NPC.Center, NPC.Center + (NPC.rotation - MathHelper.Pi/2f).ToRotationVector2() * 64, new Vector2(0, 0), Mod.Find<ModProjectile>("SpiderVenom").Type, 10, 7, 35, 1, true, 0, true, 1600);
							SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 102, 0.25f, -0.25f);
						}

						if (NPC.velocity.Length() > 96f)
						{
							NPC.velocity.Normalize();
							NPC.velocity *= 96f;
						}



					}
					NPC.localAI[0] += NPC.velocity.Length() / 3f;
					NPC.velocity /= 1.15f;

				}
			}

			//Spinning Trap Webs
			if (NPC.ai[0] > 2999 && NPC.ai[0] < 4000) {
				if (NPC.ai[0] == 3005)
					SoundEngine.PlaySound(3, (int)NPC.Center.X, (int)NPC.Center.Y, 56, 0.25f, -0.25f);

				if (NPC.ai[0] > 3100 && NPC.ai[0] < 3300)
				{

					legdists = 72;
					float angle1; float angle2;
					GetAngleDifferenceBlushiMagic(new Vector2(NPC.localAI[1], NPC.localAI[2]), out angle1, out angle2);
					float rotSpeed = angle2 > angle1 ? 0.05f : -0.05f;
					rotSpeed *= 1f + ((float)(angle2 - angle1) * 0.2f);

					NPC.rotation += rotSpeed;
					if (NPC.ai[0] % 10 == 0)
					{
						//AcidicWebTile
						int type = ModContent.ProjectileType<TrapWeb>();
						Idglib.Shattershots(NPC.Center + NPC.rotation.ToRotationVector2() * 32, NPC.Center + NPC.rotation.ToRotationVector2() * 200, new Vector2(0, 0), type, 15, 7, 35, 1, true, 0, true, 1600);
						SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 102, 0.25f, -0.25f);
					}

					if (NPC.ai[0] % 150 == 31)
					{
						NPC.localAI[1] = P.Center.X;
						NPC.localAI[2] = P.Center.Y;
					}


				}

				if (NPC.ai[0] > 3350)
				{
					NPC.ai[0] = Main.rand.Next(2400, 2700);
					NPC.netUpdate = true;
				}

				NPC.velocity *= 0.96f;
			}


			//Wounded
			if (NPC.ai[0] > 9999)
			{
				NPC.velocity /= 1.25f;
				if (NPC.ai[0] == 10001)
					SoundEngine.PlaySound(3, (int)NPC.Center.X, (int)NPC.Center.Y, 51, 1f, 0.25f);
				NPC.rotation += Main.rand.NextFloat(1f, -1f) * 0.08f;


				if (NPC.ai[0] > 10100)
				{
					if (phase == 1)
					{
						NPC.ai[0] = 2000;
					}

				}
				if (NPC.ai[0] > 10100)
				{
					if (phase == 2)
					{
						NPC.ai[0] = 3000;
					}

				}

			}

		}

		public Player P;
		int legdists = 100;
		bool charge = false;

		public override void AI()
		{
			LegsMethod();
			charge = false;
			legdists = 128;
			NPC.direction = NPC.velocity.X > 0 ? -1 : 1;
			P = Main.player[NPC.target];
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
				if (SGAmod.DRMMode)
					phase = 2;
				NPC.dontTakeDamage = false;
				bool sighttoplayer = (Collision.CanHitLine(new Vector2(NPC.Center.X, NPC.Center.Y), 6, 6, new Vector2(P.Center.X, P.Center.Y), 6, 6));
				bool underground = Items.Consumables.AcidicEgg.Underground((int)NPC.Center.Y);//(int)((double)((npc.position.Y + (float)npc.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;
				if (!underground)
				{
					NPC.dontTakeDamage = true;
				}
				NPC.ai[3] -= 1;
				if (NPC.ai[3] < 1)
					NPC.ai[0] += 1;

				Vector2 playerangledif = P.Center - NPC.Center;
				float playerdist = playerangledif.Length();
				float maxspeed = 3f;
				if (Main.expertMode && !sighttoplayer)
					maxspeed += 3f;

				float maxrotate = 0.05f;
				playerangledif.Normalize();

				if (NPC.ai[0] < 1201)//Standard Attacks
				{
					if (phase == 0)
					{
						if (NPC.life < NPC.lifeMax * 0.75)
						{
							DoPhase(1);
							return;
						}
					}

					NPC.ai[0] %= 1200;
					if (NPC.ai[0] % 1200 < 600)
					{

						if (NPC.ai[0] == 10)
						{
							if (phase > 0)
								NPC.ai[0] = Main.rand.Next(50, 400);
							NPC.netUpdate = true;
						}

						NPC.localAI[0] += 1f;
						NPC.localAI[1] = P.Center.X;
						NPC.localAI[2] = P.Center.Y;
						if ((NPC.ai[0] + 26) % (Main.expertMode ? 60 : 90) == 0)//Chase after the player and Squirt
						{
							Idglib.Shattershots(NPC.Center + NPC.rotation.ToRotationVector2() * 32, NPC.Center + NPC.rotation.ToRotationVector2() * 200, new Vector2(0, 0), ModContent.ProjectileType<SpiderVenom>(), 15, 8, 60 + phase * 10, phase + 1, true, 0, true, 1600);
							SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 102, 0.25f, -0.25f);
						}
						NPC.rotation = NPC.rotation.AngleLerp((P.Center - NPC.Center).ToRotation(), maxrotate);
						NPC.velocity += NPC.rotation.ToRotationVector2() * 0.4f;
						NPC.velocity += playerangledif * 0.075f;
						if (NPC.velocity.Length() > maxspeed)
						{
							NPC.velocity.Normalize(); NPC.velocity *= maxspeed;
						}
					}
					else
					{
						//Acid Spin Attack
						if (NPC.ai[0] % 1200 == 601)
						{
							NPC.ai[0] += 1;
							NPC.ai[3] = 60;
							SoundEngine.PlaySound(SoundID.NPCHit, (int)NPC.Center.X, (int)NPC.Center.Y, 37, 0.50f, -0.25f);
						}
						if (NPC.ai[0] % 1200 > 602)
						{
							float angle1; float angle2;
							GetAngleDifferenceBlushiMagic(new Vector2(NPC.localAI[1], NPC.localAI[2]), out angle1, out angle2);
							float rotSpeed = angle2 > angle1 ? 0.05f : -0.05f;
							rotSpeed *= 1f + ((float)(angle2 - angle1) * 0.2f);

							legdists = 72;

							if (NPC.ai[0] % 150 < 60)
							{
								NPC.rotation += rotSpeed;
								if (NPC.ai[0] % (Main.expertMode ? 5 : 10) == 0 && NPC.ai[3] < 1)
								{
									int type = ModContent.ProjectileType<SpiderVenom>();
									Idglib.Shattershots(NPC.Center + NPC.rotation.ToRotationVector2() * 32, NPC.Center + NPC.rotation.ToRotationVector2() * 200, new Vector2(0, 0), type, 15, 7, 35 + (phase * 15), 1 + phase, true, 0, true, 1600);
									SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 102, 0.25f, -0.25f);
								}

								if (NPC.ai[0] % 150 == 61)
								{
									NPC.localAI[1] = P.Center.X;
									NPC.localAI[2] = P.Center.Y;
								}
							}
						}


						NPC.velocity *= 0.96f;

					}
					if (NPC.ai[0] == 1195 && phase > 0)
					{
						NPC.ai[0] = 2100;
					}

				}//Standard Attacks Over

				DoPhase(phase);


				if (sighttoplayer)
				{
					if (NPC.ai[2] > 1500)
					{
						NPC.ai[2] = 0;
						NPC.ai[0] = 3000;
						NPC.netUpdate = true;
					}

					if (phase > 1 && NPC.ai[0] < 3000)
						NPC.ai[2] += 1;

				}

			}


		}

		List<SpiderLeg> legs;

		public void LegsMethod()
		{
			if (legs == null)
			{
				legs = new List<SpiderLeg>();
				Vector2[] legbody = { new Vector2(-10, -12), new Vector2(0, -12), new Vector2(10, -12), new Vector2(20, -8) };
				Vector2[] legbodyExtended = { new Vector2(-12, -64), new Vector2(32, -84), new Vector2(72, -84), new Vector2(100, -80) };

				for (int xx = -1; xx < 2; xx += 2)
				{
					for (int i = 0; i < legbodyExtended.Length; i += 1)
					{
						legs.Add(new SpiderLeg(new Vector2(legbodyExtended[i].X, legbodyExtended[i].Y*xx), new Vector2(legbody[i].X, legbody[i].Y * xx),xx));
					}
				}
			}
			else
			{
				for (int i = 0; i < legs.Count; i += 1)
				{
					legs[i].LegUpdate(NPC.Center, NPC.rotation, legdists,NPC.velocity, charge);
				}
			}


		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D texBody = ModContent.Request<Texture2D>("SGAmod/NPCs/SpiderQueen/SpiderQueen");
			Texture2D texBodyGlow = ModContent.Request<Texture2D>("SGAmod/NPCs/SpiderQueen/SpiderQueen");
			Texture2D texSkull = ModContent.Request<Texture2D>("SGAmod/Items/Accessories/AmberGlowSkull");
			Texture2D texBodyOverlay = ModContent.Request<Texture2D>("SGAmod/NPCs/SpiderQueen/SpiderQueenOverlay");
			Vector2 drawOriginBody = new Vector2(texBody.Width, texBody.Height / 2);
			Vector2 drawPos = ((NPC.Center - Main.screenPosition)) + NPC.rotation.ToRotationVector2() * -46f;
			Vector2 drawPosHead = ((NPC.Center - Main.screenPosition)) + NPC.rotation.ToRotationVector2() * 38f;
			Color color = lightColor;


			if (legs != null)
			{
				for (int i = 0; i < legs.Count; i += 1)
				{
					legs[i].Draw(NPC.Center, NPC.rotation,false, NPC.velocity, spriteBatch);
				}
			}

			Vector2 floatypos = new Vector2((float)Math.Cos(Main.GlobalTimeWrappedHourly / 1f) * 6f, (float)Math.Sin(Main.GlobalTimeWrappedHourly / 1.37f) * 3f);
			spriteBatch.Draw(texBody, drawPosHead, null, color, NPC.rotation, drawOriginBody, NPC.scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texBodyGlow, drawPosHead, null, Color.White, NPC.rotation, drawOriginBody, NPC.scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texSkull, drawPos+ floatypos.RotatedBy(NPC.rotation), null, Color.White*0.75f, NPC.rotation+((float)NPC.whoAmI*0.753f), new Vector2(texSkull.Width / 2f, texSkull.Height / 2f), NPC.scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texBodyOverlay, drawPosHead, null, color, NPC.rotation, drawOriginBody, NPC.scale, SpriteEffects.None, 0f);

			return false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life < 1)
			{
				for (int i = 0; i < 6; i += 1) {
					Gore.NewGore(NPC.Center, NPC.velocity+new Vector2(Main.rand.NextFloat(-2,2), Main.rand.NextFloat(-2, 2)), Mod.GetGoreSlot("Gores/SpiderBody"));
				}
				for (int i = 0; i < 2; i += 1)
				{
					Gore.NewGore(NPC.Center+(NPC.rotation.ToRotationVector2()*24f), NPC.velocity + new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), Mod.GetGoreSlot("Gores/SpiderManible"));
				}
				for (int i = 0; i < 80; i++)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-58, -14), Main.rand.Next(-10, 14)).RotatedBy(NPC.rotation);
					int dust = Dust.NewDust(NPC.Center+new Vector2(randomcircle.X, randomcircle.Y), 0,0, Mod.Find<ModDust>("AcidDust").Type);
					Main.dust[dust].scale = 2f;
					Main.dust[dust].noGravity = false;
					Main.dust[dust].velocity = -NPC.velocity * (float)(Main.rand.Next(20, 120) * 0.01f)+(Main.rand.NextFloat(0,360).ToRotationVector2()*Main.rand.NextFloat(1f,6f));
				}
				if (legs != null)
				{
					for (int i = 0; i < legs.Count; i += 1)
					{
						legs[i].Draw(NPC.Center, NPC.rotation, true,NPC.velocity);
					}
				}
			}

	}

	public class SpiderLeg
	{
		Vector2 LegPos;
		Vector2 PreviousLegPos;
		Vector2 CurrentLegPos;
		float lerpvalue = 1;
		float maxdistance;
		Vector2 desirsedLegPos;
		Vector2 BodyLoc;
		int side;
		public SpiderLeg(Vector2 Startleg, Vector2 BodyLoc,int side)
		{
			LegPos = Startleg;
			PreviousLegPos = Startleg;
			CurrentLegPos = Startleg;
			desirsedLegPos = Startleg;
			this.BodyLoc = BodyLoc;
			this.side = side;
		}

		public void LegUpdate(Vector2 SpiderLoc, float SpiderAngle, float maxdistance,Vector2 SpiderVel,bool charge)
		{
			bool spin = maxdistance < 94;
			this.maxdistance = maxdistance;
			float dev = charge ? 2f : 5f;
			float forward = Math.Abs((SpiderVel.Length() - 4f) * 8f)* (desirsedLegPos.X>-0 ? desirsedLegPos.X/100f : 1f);
			if (spin)
				forward -= (125f-desirsedLegPos.X);
			Vector2 leghere = SpiderLoc+(new Vector2(forward,0f) + desirsedLegPos).RotatedBy(SpiderAngle);
			lerpvalue += (1f - lerpvalue) / dev;
			LegPos = Vector2.Lerp(PreviousLegPos, CurrentLegPos, lerpvalue);

			if ((LegPos - leghere).Length() > (maxdistance+((dev-4f)*16f))+ (charge ? 74 : 0))
			{
				PreviousLegPos = LegPos;
				CurrentLegPos = leghere+new Vector2(Main.rand.Next(-24,24), Main.rand.Next(-24, 24));
				lerpvalue = 0f;
			}

		}

			public void Draw(Vector2 SpiderLoc, float SpiderAngle, bool gibs, Vector2 velo, SpriteBatch spriteBatch = null)
			{

				int length1 = 58;//First Leg
				int length2 = 74;//Second Leg

				Vector2 start = SpiderLoc + BodyLoc.RotatedBy(SpiderAngle);

				Vector2 middle = LegPos - start;

				float angleleg1 = (LegPos - start).ToRotation() + (MathHelper.Clamp((MathHelper.Pi/2f) - MathHelper.ToRadians(middle.Length() / 1.6f), MathHelper.Pi / 12f, MathHelper.Pi / 2f) * side);

				Vector2 legdist = angleleg1.ToRotationVector2();
				legdist.Normalize();
				Vector2 halfway1 = legdist;
				legdist *= length1 - 8;

				Vector2 leg2 = (SpiderLoc + BodyLoc.RotatedBy(SpiderAngle)) + legdist;

				float angleleg2 = (LegPos - leg2).ToRotation();

				halfway1 *= length1 / 2;
				Vector2 halfway2 = leg2 + (angleleg2.ToRotationVector2() * length2 / 2);
				if (!gibs)
				{
					Texture2D texLeg1 = ModContent.Request<Texture2D>("SGAmod/NPCs/SpiderQueen/SpiderLeg");
					Texture2D texLeg2 = ModContent.Request<Texture2D>("SGAmod/NPCs/SpiderQueen/SpiderClaw");
					Color lighting = Lighting.GetColor((int)((start.X + halfway1.X) / 16f), (int)((start.Y + halfway1.Y) / 16f));
					spriteBatch.Draw(texLeg1, start - Main.screenPosition, null, lighting, angleleg1, new Vector2(4, texLeg1.Height / 2f), 1f, angleleg1.ToRotationVector2().X > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
					lighting = Lighting.GetColor((int)(halfway2.X / 16f), (int)(halfway2.Y / 16f));
					spriteBatch.Draw(texLeg2, leg2 - Main.screenPosition, null, lighting, angleleg2, new Vector2(4, texLeg2.Height / 2f), 1f, angleleg2.ToRotationVector2().X > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
					spriteBatch.Draw(ModContent.Request<Texture2D>("SGAmod/NPCs/SpiderQueen/SpiderClaw_Glow"), leg2 - Main.screenPosition, null, Color.White, angleleg2, new Vector2(4, texLeg2.Height / 2f), 1f, angleleg2.ToRotationVector2().X > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
				}
				else
				{
					Gore.NewGore(halfway1,velo+new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), SGAmod.Instance.GetGoreSlot("Gores/SpiderLeg1"));
					Gore.NewGore(halfway2, velo + new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)), SGAmod.Instance.GetGoreSlot("Gores/SpiderLeg2"));
				}
			}
		}

	}

	public class SpiderVenom : ModProjectile
	{
		private Vector2[] oldPos = new Vector2[6];
		private float[] oldRot = new float[6];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spider Venom");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 1200;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 5;
			AIType = ProjectileID.Bullet;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = ProjectileID.CursedFlameFriendly;

			for(int i = 0; i < 20; i++) {
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				randomcircle *= Main.rand.NextFloat(0f, 2f);
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("AcidDust").Type);
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
				Main.dust[dust].velocity += new Vector2(randomcircle.X, randomcircle.Y);
			}
			SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 111, 0.33f, 0.25f);

			if (Projectile.hostile)
			{
				for (int pro = 0; pro < Main.maxPlayers; pro += 1)
				{
					Player ply = Main.player[pro];
					if (ply.active && (ply.Center - Projectile.Center).Length() < 48)
					{
						ply.AddBuff(Mod.Find<ModBuff>("AcidBurn").Type, 45);
					}
				}
			}

			if (Projectile.friendly)
			{
				for (int pro = 0; pro < Main.maxNPCs; pro += 1)
				{
					NPC ply = Main.npc[pro];
					if (ply.active && !ply.friendly && (ply.Center - Projectile.Center).Length() < 72)
					{
						ply.AddBuff(Mod.Find<ModBuff>("AcidBurn").Type, 60*2);
					}
				}
			}

			return true;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + 14); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = ModContent.Request<Texture2D>("SGAmod/NPCs/SpiderQueen/SpiderVenom");
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.White, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldPos.Length + 2)) * 0.25f;
				spriteBatch.Draw(tex, drawPos, null, color * alphaz, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void AI()
		{

			if (Main.rand.Next(0, 3) == 1)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("AcidDust").Type);
				Main.dust[dust].scale = 0.75f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			}

			Projectile.position -= Projectile.velocity * 0.8f;

			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = Projectile.Center;

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(0, 3) < 2)
				target.AddBuff(Mod.Find<ModBuff>("AcidBurn").Type, 180 * (Main.rand.Next(0, 3) == 1 ? 2 : 1));
			Projectile.Kill();
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(0, 3) < 2)
				target.AddBuff(Mod.Find<ModBuff>("AcidBurn").Type, 60 * (Main.rand.Next(0, 3) == 1 ? 2 : 1));
			Projectile.Kill();
		}


	}

}

