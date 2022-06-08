using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.Enums;
using SGAmod.Items.Weapons;
using Idglibrary;
using SGAmod.Projectiles;
using System.Linq;
using SGAmod.Effects;
using System.IO;
using Terraria.DataStructures;
using Terraria.Utilities;
using SGAmod.NPCs.Hellion;

namespace SGAmod.Items.Weapons
{

	public class BoyfriendsMic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boyfriend's Mic");
			Tooltip.SetDefault("'So, I heard you like funking on a friday night...'\nChallenge everyone around you to a beatdown of beats\nDeals damage per note hit to random enemies, per Max Sentries, halved for each hit on the same enemy\nScoring SICK beats spawns hearts and awards more points\nSICK beats also become crits\nDeals your score as damage to ALL enemies near you on completion\nRequires atleast 4 minion slots, spawns an arrow for each slot, each arrow has 8 notes\n" + Idglib.ColorText(Color.Red, "You will be unable to use other items while rapping"));
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 500;
			Item.knockBack = 50;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.autoReuse = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = Item.buyPrice(1, 0, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			//item.buffType = ModContent.BuffType<FlyMinionBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<FlySwarmMinion>();
		}
		public override bool CanUseItem(Player player)
		{
			return (((float)player.maxMinions - player.GetModPlayer<SGAPlayer>().GetMinionSlots) > 4f) && player.ownedProjectileCounts[ModContent.ProjectileType<BFMicMasterProjectile>()] < 1;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies

			Projectile proj = Projectile.NewProjectileDirect(player.MountedCenter, Vector2.UnitY * 2f, ModContent.ProjectileType<BFMicMasterProjectile>(), damage, knockBack, player.whoAmI);
			proj.minionSlots = player.maxMinions - player.SGAPly().GetMinionSlots;
			proj.netUpdate = true;

			return false;
		}

	}

	public class BFMicMasterProjectile : HellionFNFArrowMinigameMasterProjectile
	{

		public override float AlphaPeriod => 64f;
		public override float TimeoutPeriod => 80f;
		public float ScoreScale => Owner.GetDamage(DamageClass.Summon) * 5f;
		public override int ArrowType => ModContent.ProjectileType<BoyfriendFNFArrow>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BF FNF Tracklist");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 80;
			Projectile.minionSlots = 4;
			Projectile.scale = 1 / 3f;
			Projectile.minion = true;
		}

		public override void GenerateNotes()
		{
			int maxNotes = 8;
			int maxslots = (int)Projectile.minionSlots;
			int arrowindex = 0;// Main.rand.Next(maxslots);
			float rotter = arrowindex * MathHelper.PiOver2;

			for (int i = 0; i < maxslots; i += 1)
			{
				float half = (0.5f / (float)maxslots);
				float percent = (i / (float)maxslots) + half;
				float mid = percent - 0.5f;
				float max = maxslots * 42f;
				rotter = (arrowindex * MathHelper.PiOver2);

				Func<Vector2> arrowpos = delegate ()
				{
					Vector2 vector = new Vector2(i, 0);
					return new Vector2(mid * max, (float)(Math.Sin((vector.X/ 8f)+(Main.GlobalTimeWrappedHourly*2f)))*0f +(Math.Abs(mid) * 0f)+12);
				};
				HUDArrow upArrow = new HUDArrow(arrowpos);
				upArrow.rotation = rotter;
				upArrow.scaleBy = 1f / 3f;
				hudNotes.Add(upArrow);

				AddNote(-1, 20);
				for (int iii = 0; iii < maxNotes; iii += 1)
				{
					AddNote(arrowindex, Math.Max(30 - (maxslots * Main.rand.Next(1, 4)), 5));
				}
				AddNote(-1, 30);

				arrowindex++;
				arrowindex %= maxslots;
			}

			notesToSpawn = notesToSpawn.OrderBy(testby => Main.rand.Next()).ToList();
			AddNote(-1, 60);
			notesToSpawn.Reverse();
			AddNote(-1, 30);

		}
		public override void MoveIntoPosition()
		{
			if (!IsDone)
			{
				Owner.itemTime = 80;
				Owner.itemAnimation += 1;
			}

			Projectile.position.X = Owner.position.X;
			Projectile.position.Y = Owner.position.Y - MathHelper.SmoothStep(420f, 160f, MathHelper.Clamp(Projectile.localAI[0] / 80f, 0f, 1f));
		}

		public override void OnEnded()
		{
			if (missed > -10)
			{
				NPC[] enemies = Main.npc.Where(testby => testby.IsValidEnemy() && (testby.Center - Owner.MountedCenter).LengthSquared() < 4000000).OrderBy(testby => Main.rand.Next()).ToArray();

				if (enemies.Length > 0)
				{
					foreach (NPC enemy in enemies)
					{
						enemy.StrikeNPC(score, Projectile.knockBack, 0, false);
						Owner.addDPS(score);
					}
				}
				var explode = SGAmod.AddScreenExplosion(Projectile.Center, 60, 2f, 2000);
				explode.warmupTime = 60;
				explode.decayTime = 24;
				missed = -10;
			}
		}

		public override void AddNote(int type, int delay)
		{
			if (type >= 0)
				scoreMax += (int)(50 * ScoreScale);

			notesToSpawn.Add((type, delay));
		}

		public override void HitNote(HellionFNFArrow fnfarrow)
		{
			int timeWindow = fnfarrow.Projectile.timeLeft;

			float[] directions = new float[] { -MathHelper.PiOver2 / 2f, -MathHelper.PiOver2 / 8f, MathHelper.PiOver2 / 3f, MathHelper.PiOver2 / 8f };

			Owner.itemRotation = directions[(int)(fnfarrow.Projectile.ai[1]%4)]*Owner.direction;

			fnfarrow.GotNote(false, timeWindow);
			hudNotes[(int)fnfarrow.Projectile.ai[1]].HitBeat(timeWindow, fnfarrow);

			if (timeWindow < 8)
				score += ((int)(MathHelper.Clamp(8 - fnfarrow.Projectile.timeLeft, 0, 5))) * ((int)(25 * ScoreScale));
			score += (int)(50 * ScoreScale);
		}

		public override void SpawnNote(int type, int delay)
		{
			base.SpawnNote(type, delay);
		}

		public override void FailedNote(HellionFNFArrow fnfarrow)
		{
			//base.FailedNote(fnfarrow);
		}
	}

	public class BoyfriendFNFArrow : HellionFNFArrow
	{
		public override float MovementRate => 320f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Friday Night Funked, BF edition");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 100;
			Projectile.aiStyle = -1;
			Projectile.scale = 1 / 3f;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void DoUpdate()
		{
			Projectile owner = Main.projectile[(int)Projectile.ai[0]];
			Projectile.position += owner.velocity;
		}


		public override void GotNote(bool failed, int timeleft = 999)
		{
			Player Owner = Main.player[Projectile.owner];
			if (!failed)
			{
				HellionFNFArrowMinigameMasterProjectile master = Main.projectile[(int)Projectile.ai[0]].ModProjectile as HellionFNFArrowMinigameMasterProjectile;

				NPC[] enemies = Main.npc.Where(testby => testby.IsValidEnemy() && (testby.Center - Owner.MountedCenter).LengthSquared() < 4000000).OrderBy(testby => testby.boss ? 0 : Main.rand.Next()).ToArray();

				if (enemies.Length > 0)
				{
					int damage = Projectile.damage;
					for (int i = 0; i < Owner.maxTurrets; i += 1)
					{
						NPC enemy = enemies[i % enemies.Length];

						Vector2 where = master.Projectile.Center+master.hudNotes[(int)Projectile.ai[1]].Position;

						Projectile proj = Projectile.NewProjectileDirect(where, Vector2.Zero, ModContent.ProjectileType<NoteLaserProj>(), damage, Projectile.knockBack, Projectile.owner, Main.rgbToHsl(color).X, enemy.whoAmI);
						proj.penetrate = timeleft < 8 ? 100 : -1;
						proj.netUpdate = true;

						//enemy.StrikeNPC(damage, projectile.knockBack, 0, timeleft < 8);
						//Owner.addDPS(damage);
						/*if (timeleft < 8)
						{
							var explode = SGAmod.AddScreenExplosion(projectile.Center, 8, 1.5f + (5f - timeleft) / 5f, 320);
							explode.warmupTime = 16;
							explode.decayTime = 8;
						}*/
						damage /= 2;
					}
				}
			}

			base.GotNote(failed, timeleft);
		}

	}

	public class NoteLaserProj : Almighty.MegidolaLaserProj
	{
		Vector2 startingloc = default;
		Vector2 hitboxchoose = default;
		protected override float AppearTime => 4f;
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 60;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Funky Laser Proj");
		}

		public override void AI()
		{
			NPC enemy = Main.npc[(int)Projectile.ai[1]];

			if (startingloc == default)
			{
				color1 = Main.hslToRgb(Projectile.ai[0], 1f, 0.70f);
				color2 = Color.Lerp(color1, Color.White, 0.25f);
				startingloc = Projectile.Center;
			}

			Projectile.localAI[0] += 1f;

			if (enemy != null && enemy.active && Projectile.localAI[1] < 1)
			{
				if (hitboxchoose == default)
				{
					hitboxchoose = new Vector2(Main.rand.Next(enemy.width), Main.rand.Next(enemy.height));
				}
				Projectile.velocity = (enemy.position + hitboxchoose) - Projectile.Center;

				if (Projectile.localAI[0] == 5)
				{
					int damage = Main.DamageVar(Projectile.damage);
					enemy.StrikeNPC(damage, 0, 1, Projectile.penetrate >99);
					if (SGAmod.ScreenShake<12)
					SGAmod.AddScreenShake(4f, 420, Projectile.Center);
					Main.player[Projectile.owner].addDPS(damage);
				}
			}
			else
			{
				Projectile.localAI[1]++;
			}

			Projectile.position -= Projectile.velocity;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			float alpha = MathHelper.Clamp(Projectile.localAI[0] / AppearTime, 0f, 1f) * MathHelper.Clamp((Projectile.timeLeft-50) / AppearTime, 0f, 1f);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			if (alpha > 0)
			{

				Vector2[] points = new Vector2[] { startingloc, startingloc + Projectile.velocity };

				TrailHelper trail = new TrailHelper("BasicEffectAlphaPass", Mod.Assets.Request<Texture2D>("SmallLaser").Value);
				//UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI);
				Color colorz = Color.Lerp(color1,Color.White,0.50f);
				Color colorz2 = color2;
				trail.color = delegate (float percent)
				{
					return Color.Lerp(colorz, colorz2, percent);
				};
				trail.projsize = Vector2.Zero;
				trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -1f);
				trail.coordMultiplier = new Vector2(1f, 1f);
				trail.doFade = false;
				trail.trailThickness = 16;
				trail.trailThicknessIncrease = 0;
				//trail.capsize = new Vector2(6f, 0f);
				trail.strength = alpha * 1f;
				trail.DrawTrail(points.ToList(), Projectile.Center);

			}

			Texture2D mainTex = SGAmod.ExtraTextures[96];
			Texture2D glowTex = ModContent.Request<Texture2D>("SGAmod/Glow");

			float alpha2 = MathHelper.Clamp(Projectile.localAI[0] / 3f, 0f, 1f) * MathHelper.Clamp(Projectile.timeLeft / 25f, 0f, 1f);

			if (GetType() != typeof(Accessories.RefractorLaserProj))
			{

				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

				Effect effect = SGAmod.TextureBlendEffect;

				effect.Parameters["coordMultiplier"].SetValue(new Vector2(1f, 1f));
				effect.Parameters["coordOffset"].SetValue(new Vector2(0f, 0f));
				effect.Parameters["noiseMultiplier"].SetValue(new Vector2(1f, 1f));
				effect.Parameters["noiseOffset"].SetValue(new Vector2(0f, 0f));

				effect.Parameters["Texture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Extra_49c").Value);
				effect.Parameters["noiseTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Extra_49c").Value);
				effect.Parameters["noiseProgress"].SetValue(Projectile.localAI[0] / 30f);
				effect.Parameters["textureProgress"].SetValue(0f);
				effect.Parameters["noiseBlendPercent"].SetValue(1f);
				effect.Parameters["strength"].SetValue(alpha2);
				effect.Parameters["alphaChannel"].SetValue(false);

				effect.Parameters["colorTo"].SetValue(color1.ToVector4() * new Vector4(0.5f, 0.5f, 0.5f, 1f));
				effect.Parameters["colorFrom"].SetValue(Color.Black.ToVector4());

				effect.CurrentTechnique.Passes["TextureBlend"].Apply();

				Main.spriteBatch.Draw(mainTex, startingloc + Projectile.velocity - Main.screenPosition, null, Color.White, Projectile.rotation, mainTex.Size() / 2f, (alpha2 + (Projectile.localAI[0] / 60f)) * 0.75f, default, 0);
			}

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			if (GetType() != typeof(Accessories.RefractorLaserProj))
				Main.spriteBatch.Draw(glowTex, startingloc + Projectile.velocity - Main.screenPosition, null, Color.White* alpha2, Projectile.rotation, glowTex.Size() / 2f, 0.2f+((alpha2 + (Projectile.localAI[0] / 80f)) * 0.42f), default, 0);

			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			base.PostDraw(spriteBatch, lightColor);
		}
	}

}
