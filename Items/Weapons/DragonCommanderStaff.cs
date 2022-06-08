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

namespace SGAmod.Items.Weapons
{

	public class DragonCommanderStaff : ModItem, IHitScanItem, IDevItem, IHellionDrop
	{
		int IHellionDrop.HellionDropAmmount() => 1;
		int IHellionDrop.HellionDropType() => ModContent.ItemType<DragonCommanderStaff>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragon Commander");
			Tooltip.SetDefault("Controls a very powerful familiar of Draken that scales with Total Expertise\nLeft click to use minion slots to summon orders\nClicking near an enemy commands your oldest order to follow that enemy\nHold CTRL and click to cancel all current orders\nDraken will rapid fly between these orders\nWhen not given orders, Draken does the following:\nWhen deselecting this weapon, give Draken a large aura before despawning\nWhen following you, Draken increases your life regen and damage resist\n'Jetpacks not included'");
		}

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
			SGAPlayer sga = player.SGAPly();
			mult = 0.25f+((sga.ExpertiseCollectedTotal/20000f)*0.75f);
		}
		public (string, string) DevName()
        {
			return ("IDGCaptainRussia94","other");
		}

        public override void SetDefaults()
		{
			Item.damage = 750;
			Item.DamageType = DamageClass.Summon;
			Item.width = 24;
			Item.height = 24;
			Item.useTime = 10;
			Item.mana = 25;
			Item.expert = true;
			Item.useAnimation = 10;
			Item.autoReuse = false;
			Item.useTurn = false;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 0f;
			Item.value = 100000;
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = SoundID.Item100;
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<DrakenSummonTargetProj>();
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Invisible").Value;
			}
		}
        public override bool CanUseItem(Player player)
        {
			if (player.statMana < 30)
				return false;

			Item.shoot = ModContent.ProjectileType<DrakenSummonTargetProj>();

			List<NPC> enemies = SGAUtils.ClosestEnemies(Main.MouseWorld, 64);

			List<Projectile> myproj = Main.projectile.Where(testproj => testproj.active && testproj.owner == player.whoAmI && testproj.type == ModContent.ProjectileType<DrakenSummonTargetProj>()).ToList();
			myproj = myproj.OrderBy(order => (100000 - order.localAI[1])+ (order.ai[1] >= 100000 ? 999999 : 0)).ToList();

			if (player.controlSmart)
            {
				if (myproj != null && myproj.Count > 0)
				{
					foreach(Projectile proj in myproj)
                    {
						proj.timeLeft = Math.Min(proj.timeLeft, 30);


					}
				}
					Item.shoot = 16;
				return true;
            }

			if (enemies != null && enemies.Count > 0 && player.ownedProjectileCounts[ModContent.ProjectileType<DrakenSummonTargetProj>()] > 0)
			{
				NPC enemy = enemies[0];

				if (myproj != null && myproj.Count > 0 && myproj[0].ai[1]<100000)
				{
					myproj[0].ai[1] = 100000 + enemy.whoAmI;
					myproj[0].netUpdate = true;
					Item.shoot = 16;
					return true;
				}


			}

			return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (type == ModContent.ProjectileType<DrakenSummonTargetProj>())
			Projectile.NewProjectile(Main.MouseWorld.X, Main.MouseWorld.Y, 0,0, type, damage, knockBack, player.whoAmI);
			return false;
        }
        /*public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 4) % 1f, 0.4f, 0.45f);
			tooltips.Add(new TooltipLine(mod, "IDG Dev Item", Idglib.ColorText(c, "IDGCaptainRussia94's other dev weapon")));
		}*/
	}

	public class DrakenSummonProj : ModProjectile
	{
		float alphaStrength => Math.Min(Projectile.timeLeft / 30f, 1f);
		float cosmeticTrailFade = 0f;
		Vector2 there = default;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Draken!");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[Projectile.type] = false;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = false;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public override bool CanDamage()
		{
			return Projectile.ai[0]>5;
		}

		public override void SetDefaults()
		{
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false;
			Projectile.width = 32;
			Projectile.height = 32;
			AIType = ProjectileID.Bullet;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.light = 0.1f;
			Projectile.timeLeft = 300;
			Projectile.minion = true;
			Projectile.extraUpdates = 2;
			//projectile.usesLocalNPCImmunity = false;
			//projectile.localNPCHitCooldown = 2;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.immune[Projectile.owner] = 2;
		}

        public override void AI()
		{
			Projectile.minion = false;
			Projectile.localAI[0] += 1;
			Projectile.ai[0] -= 1;
			float friction = 0.98f;
			float velosway = 60 / MathHelper.PiOver2 * (float)Math.Atan(Math.Abs(Projectile.velocity.X / 5f));
			Player owner = Main.player[Projectile.owner];
			if (there == default)
				there = Projectile.Center;

			int followPlayer = owner.ownedProjectileCounts[ModContent.ProjectileType<DrakenSummonTargetProj>()];
			if (followPlayer > 0)
			{
				if (Projectile.ai[0] < 900)//Move Order
				{
					List<Projectile> myproj = Main.projectile.Where(testproj => testproj.active && testproj.owner == Projectile.owner && testproj.type == ModContent.ProjectileType<DrakenSummonTargetProj>()).ToList();
					myproj = myproj.OrderBy(order => order.localAI[1]).ToList();
					if (myproj != null && myproj.Count > 0)
					{
						Projectile target = myproj[0];
						Projectile.damage = target.damage;

						if (target.DistanceSQ(Projectile.Center) > 200 * 200)
							there = target.Center + Vector2.Normalize(target.Center - Projectile.Center) * 640;

						Vector2 normal = Vector2.Normalize(target.Center - Projectile.Center);

						friction = 0.98f;

						Projectile.velocity += (there - Projectile.Center) / 150f;
						Projectile.velocity = Vector2.Normalize(Projectile.velocity) * Math.Min(Projectile.velocity.Length()/1f, 26f);

						if (Vector2.Dot(normal, Vector2.Normalize(Projectile.velocity)) < -0.9f && Projectile.DistanceSQ(target.Center) > 32 * 32 && Projectile.DistanceSQ(target.Center) < 96 * 96)
						{
							if (target.active)
							{
								target.localAI[1] += 1000;
								target.netUpdate = true;
								return;
							}
						}

						Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.velocity.ToRotation(),0.1f);
						int dir = Projectile.velocity.X > 0 ? 1 : -1;
						if (dir != Projectile.spriteDirection)
						{
							Projectile.spriteDirection = dir;
							Projectile.rotation += MathHelper.Pi;
						}

						Projectile.ai[0] = 30;
						cosmeticTrailFade = 1.2f;

					}
				}


			}
			else
			{

				if (owner.HeldItem.type == ModContent.ItemType<DragonCommanderStaff>())
				{
					if (Projectile.ai[0] < 1) 
					{
						there = owner.Center + new Vector2(-Projectile.spriteDirection * 48, -72);
						if (Projectile.velocity.Length() > 3f || Projectile.velocity.Length() < 0.25f)
						{
							int dir = Projectile.velocity.Length() > 3f ? (Projectile.rotation.ToRotationVector2().X > 0 ? 1 : -1) : owner.direction;
							if (dir != Projectile.spriteDirection)
							{
								Projectile.spriteDirection = dir;
								Projectile.rotation += MathHelper.Pi;
							}
						}
						Projectile.rotation = Projectile.rotation.AngleLerp(Projectile.velocity.Length() > 3f ? Projectile.velocity.ToRotation() : Projectile.spriteDirection < 0 ? MathHelper.Pi : 0, 0.01f);
						Projectile.velocity = (there - Projectile.Center) / 40f;

					}

					if (Projectile.ai[0] < 1 && owner.HeldItem.type == ModContent.ItemType<DragonCommanderStaff>())
					{
						Projectile.ai[0] = 0;
						Projectile.damage = owner.HeldItem.damage;
						owner.AddBuff(ModContent.BuffType<DrakenDefenseBuff>(), 3);
					}

				}
			}

			if (Projectile.ai[0] >=0)
			Projectile.timeLeft = 300;

			if (Projectile.ai[0] < -5 && Projectile.ai[0] > -240)
            {
				if (Projectile.ai[0] % 15 == 0)
                {
					List<NPC> enemies = SGAUtils.ClosestEnemies(Projectile.Center, 200,checkCanChase: false);
					if (enemies!=null && enemies.Count > 0) 
					{
						foreach (NPC enemy in enemies)
						{
							float damazz = (Main.DamageVar((float)Projectile.damage*2) * (1f-(enemy.DistanceSQ(Projectile.Center) / (200 * 200))));
							enemy.StrikeNPC((int)damazz, 0f, 0, false, true, true);
							owner.addDPS((int)damazz);
							if (Main.netMode != 0)
							{
								NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, enemy.whoAmI, damazz, 16f, (float)1, 0, 0, 0);
							}
						}
					}

				}


            }


			Projectile.velocity *= friction;
			cosmeticTrailFade -= 0.04f;

			Projectile.Opacity = alphaStrength;


		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D tex = Main.projectileTexture[Projectile.type];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition));
			Vector2 adder = Vector2.Zero;
			Player owner = Main.player[Projectile.owner];
			int timing = (int)(Main.GlobalTimeWrappedHourly * (8f));

			timing %= 4;

			int mydirection = Projectile.spriteDirection;

			if (timing == 0)
			{
				adder = ((Projectile.rotation + (float)Math.PI / 2f).ToRotationVector2() * (8f * mydirection));
			}

			if (Projectile.ai[0] > 1)
			{
				TrailHelper trail = new TrailHelper("FadedBasicEffectPass", Mod.Assets.Request<Texture2D>("noise").Value);
				trail.projsize = Projectile.Hitbox.Size() / 2f;
				trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -0.5f);
				trail.trailThickness = 6;
				trail.trailThicknessIncrease = 36;
				trail.strength = alphaStrength * MathHelper.Clamp(cosmeticTrailFade, 0f, 1f);
				trail.capsize = new Vector2(8, 0);
				trail.DrawTrail(Projectile.oldPos.ToList(), Projectile.Center);
			}


			if (Projectile.ai[0] < 28 && Projectile.ai[0] >= 0)
			{
				List<Vector2> Swirl = new List<Vector2>();

				UnifiedRandom rando = new UnifiedRandom(Projectile.whoAmI*753);

				Vector2 vex = Vector2.One.RotatedBy(rando.NextFloat(MathHelper.TwoPi)) * 48f;
				float[] rando2 = {rando.NextFloat(MathHelper.TwoPi), rando.NextFloat(MathHelper.TwoPi) , rando.NextFloat(MathHelper.TwoPi) };
				float[] rando3 = { rando.NextFloat(0.1f,0.25f), rando.NextFloat(0.1f, 0.25f) , rando.NextFloat(0.1f, 0.25f) };
				for (float ix = 0; ix < 24; ix += 0.10f)
                {
					float i = (-Projectile.localAI[0] / 2f) + ix;
					Matrix matrix = Matrix.CreateRotationZ((i * rando3[0]) + rando2[0])* Matrix.CreateRotationY((i * rando3[1]) + rando2[1]) *Matrix.CreateRotationX((i * rando3[2]) + rando2[2]);
					Swirl.Add(Vector2.Transform(vex, matrix)+owner.MountedCenter);

                }


				TrailHelper trail = new TrailHelper("FadedBasicEffectPass", Mod.Assets.Request<Texture2D>("Perlin").Value);
				trail.coordOffset = new Vector2(Main.GlobalTimeWrappedHourly * -0.5f,0f);
				trail.trailThickness = 6;
				trail.trailThicknessIncrease = 12;
				trail.strength = alphaStrength * MathHelper.Clamp(1f-(Projectile.ai[0] / 50f), 0f, 0.75f);
				trail.capsize = new Vector2(8, 0);
				trail.color = delegate (float percent)
				{
					return Main.hslToRgb(((1f-percent)+ Main.GlobalTimeWrappedHourly/3f) %1f,0.9f,0.75f);
				};
				trail.DrawTrail(Swirl, Projectile.Center);
			}

			if (Projectile.ai[0]<0)
			{
				for (float i = -1; i < 2; i += 0.4f)
				{
					Vector2 scaleup = new Vector2((float)Math.Abs(Math.Sin(Main.GlobalTimeWrappedHourly / 1.1694794f)), 1f) * MathHelper.Clamp(-Projectile.ai[0]/30, 0f, 1f) * alphaStrength;
					Texture2D texture7 = SGAmod.ExtraTextures[34];
					spriteBatch.Draw(texture7, Projectile.Center - Main.screenPosition, null, Main.hslToRgb((Main.GlobalTimeWrappedHourly) % 1f, 1f, 0.75f) * 0.50f, -Main.GlobalTimeWrappedHourly * 17.134f * i, new Vector2(texture7.Width / 2f, texture7.Height / 2f), scaleup, SpriteEffects.None, 0f);
					texture7 = SGAmod.HellionTextures[6];
					spriteBatch.Draw(texture7, Projectile.Center - Main.screenPosition, null, Main.hslToRgb((Main.GlobalTimeWrappedHourly) % 1f, 1f, 0.75f) * 0.50f, Main.GlobalTimeWrappedHourly * 17.134f * i, new Vector2(texture7.Width / 2f, texture7.Height / 2f), scaleup, SpriteEffects.None, 0f);
				}
			}


			timing *= ((tex.Height) / 4);

			for (int k = (Projectile.oldRot.Length/2) - 1; k >= 0; k -= 1)
			{


				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaxx = MathHelper.Clamp((Projectile.velocity.Length()-3f)/20f,0f,0.1f);
				float alphaz = (1f - (float)(k + 1) / (float)(Projectile.oldRot.Length + 2)) * (k>0 ? alphaxx : 1f);
				float scaleffect = 1f;
				//Color fancyColor = Main.hslToRgb(((k / projectile.oldRot.Length) + projectile.localAI[0] / 30f) % 1f, 1f, 0.75f);
				drawPos = ((Projectile.oldPos[k] - Main.screenPosition));
				spriteBatch.Draw(tex, drawPos + (drawOrigin / 4f) - adder, new Rectangle(0, timing + 2, tex.Width, (tex.Height - 1) / 4),
					((drawColor * alphaz) * (Projectile.Opacity))
					, Projectile.rotation - (float)(mydirection < 0 ? Math.PI : 0), drawOrigin, scaleffect, mydirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}


			return false;
		}

		public override string Texture => "SGAmod/NPCs/TownNPCs/DrakenFly";

	}

	public class DrakenSummonTargetProj : ModProjectile
	{
		float strength => Math.Min(Projectile.timeLeft / 30f, 1f);
		public Vector2 bezspot1=default;
		public Vector2 bezspot2=default;
		public Vector2 bezspot3 = default;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Draken Aim");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[Projectile.type] = true;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

        public override bool CanDamage()
        {
            return false;
        }

        public override void SetDefaults()
		{
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false;
			Projectile.width = 4;
			Projectile.height = 4;
			AIType = ProjectileID.Bullet;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.light = 0.1f;
			Projectile.timeLeft = 900;
			Projectile.minion = true;
			// Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			Projectile.minionSlots = 1f;
			Projectile.extraUpdates = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 2;
		}

        public override void AI()
		{
			Projectile.localAI[0] += 1;
			Player owner = Main.player[Projectile.owner];
			if (Projectile.localAI[0] == 1)
            {
				bezspot1 = owner.MountedCenter + Main.rand.NextVector2CircularEdge(200, 200);
				bezspot2 = Projectile.Center + Main.rand.NextVector2CircularEdge(500, 500);
				bezspot3 = owner.MountedCenter+new Vector2(0,-32f);
				Projectile.spriteDirection = (owner.MountedCenter - Projectile.Center).X < 0 ? 1 : -1;

			}


			if (owner != null && owner.active && Projectile.timeLeft>40 && owner.HeldItem.type == ModContent.ItemType<DragonCommanderStaff>())
			{
				Projectile.timeLeft += 1;
			}

			Projectile.velocity /= 1.25f;


			if (Projectile.localAI[0] > 30)
			{
				/*if (projectile.localAI[0] % 15 == 0) 
				{
				List<Projectile> myprojs = Main.projectile.Where(testproj => testproj.active && testproj.owner == projectile.owner && testproj.type == ModContent.ProjectileType<DrakenSummonTargetProj>() && testproj.whoAmI != projectile.whoAmI).ToList();
				myprojs = myprojs.OrderBy(order => order.localAI[0]).ToList();

					if (myprojs != null && myprojs.Count > 0 && myprojs[0] != projectile)
					{
						List<Projectile> draken = Main.projectile.Where(testproj => testproj.active && testproj.owner == projectile.owner && testproj.type == ModContent.ProjectileType<DrakenSummonProj>()).ToList();
						if (draken != null && draken.Count > 0)
						{

							//myprojs = myprojs.OrderBy(order => order.DistanceSQ(projectile.Center)).ToList();

							//foreach (Projectile zapperFriendly in myprojs)
							//{

							List<NPC> enemies = SGAUtils.ClosestEnemies(projectile.Center, 800);
							if (enemies != null && enemies.Count > 0)
							{

								NPC zapperFriendly = enemies[0];

								if (zapperFriendly.DistanceSQ(projectile.Center) < 1600 * 1600 && zapperFriendly.DistanceSQ(projectile.Center) > 120 * 120)
								{
									Vector2 center = projectile.Center + (zapperFriendly.Center - projectile.Center) + Main.rand.NextVector2Circular(16f, 16f);
									Projectile proj = Projectile.NewProjectileDirect(projectile.Center, Vector2.Normalize(zapperFriendly.Center - projectile.Center) * 8f, ProjectileID.GreenLaser, (int)(projectile.damage / 5f), 0f, projectile.owner);
									proj.usesIDStaticNPCImmunity = true;
									proj.idStaticNPCHitCooldown = 10;
									proj.netUpdate = true;
								}
							}
						}
					}




				}*/

					if (Projectile.ai[1] >= 100000)
				{
					NPC enemy = Main.npc[(int)Projectile.ai[1] - 100000];
					if (enemy != null && enemy.active)
					{
						Vector2 vectx = enemy.Center - Projectile.Center;
						if (vectx.LengthSquared()>64)
						Projectile.velocity += Vector2.Normalize(vectx) * ((vectx.Length() / 5f));


					}
					else
					{
						Projectile.ai[1] = 0;
						Projectile.netUpdate = true;

					}

                }
			}


		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D texture = SGAmod.ExtraTextures[110];
			Vector2 origin = texture.Size() / 2f;
			float timeAdvance = Main.GlobalTimeWrappedHourly * 2;

			Vector2 drawHere = IdgExtensions.BezierCurve(bezspot3, bezspot3, bezspot1,bezspot2, Projectile.Center, Math.Min(Projectile.localAI[0]/30f, 1f));
			drawHere -= Main.screenPosition;

			for (float i = 0f; i < MathHelper.TwoPi; i += MathHelper.PiOver4)
			{
				spriteBatch.Draw(texture, drawHere + (Vector2.One.RotatedBy(i - timeAdvance)) * 8f, null, Color.Lime * 0.75f * MathHelper.Clamp(Projectile.timeLeft / 20f, 0f, 1f), -MathHelper.PiOver4 + (i - timeAdvance), origin, 0.25f, SpriteEffects.None, 0);
			}
			texture = Main.projectileTexture[Projectile.type];
			spriteBatch.Draw(texture, drawHere, null, Color.White * 0.75f * strength, 0, texture.Size() / 2f, 1f, Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

			return false;
		}

		public override string Texture => "SGAmod/NPCs/TownNPCs/DrakenFly_Head";

	}

	public class DrakenDefenseBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Draken Defense");
			Description.SetDefault("Damage reduced to damage^0.90\nand massively boosted life regen");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/NPCs/TownNPCs/DrakenFly_Head";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 10;

		}
	}

}
