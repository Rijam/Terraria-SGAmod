using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Projectiles;
using System.Linq;
using Terraria.Utilities;
using SGAmod.Effects;

using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace SGAmod.Items.Weapons.Almighty
{
	public class AlmightyWeapon : ModItem
	{
		public override bool Autoload(ref string name)
		{
			return GetType() != typeof(AlmightyWeapon);
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			float[] highestDamage = { player.GetDamage(DamageClass.Melee), player.GetDamage(DamageClass.Magic), player.GetDamage(DamageClass.Summon), player.GetDamage(DamageClass.Ranged), player.Throwing().thrownDamage };
			add += highestDamage.OrderBy(testby => testby).Reverse().ToArray()[0] - 1f;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			// Get the vanilla damage tooltip
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] thetext = tt.Text.Split(' ');
				string newline = "";
				List<string> valuez = new List<string>();
				foreach (string text2 in thetext)
				{
					valuez.Add(text2 + " ");
				}

				valuez.Insert(1, "Almighty ");
				foreach (string text3 in valuez)
				{
					newline += text3;
				}
				tt.Text = newline;
			}
			if (GetType() == typeof(NuclearOption) && SGAmod.Calamity.Item1)
				tooltips.Add(new TooltipLine(Mod, "NuclearInferdumbFallout", "Will instantly kill any calamity enemies at max charge"));
			tooltips.Add(new TooltipLine(Mod, "AlmightyText", "Almighty Deals armor-piercing damage that scales off your highest stat"));
			tooltips.Add(new TooltipLine(Mod, "AlmightyText", "Almighty also skips crits and goes straight to Apocalypticals"));
		}
	}

	public class Megido : AlmightyWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Megido");
			Tooltip.SetDefault("Targets 4 enemies nearby your cursor on use\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 20 seconds"));
		}
		public override void SetDefaults()
		{
			base.SetDefaults();

			Item.damage = 150;
			Item.width = 48;
			Item.height = 48;
			Item.useTurn = true;
			Item.rare = ItemRarityID.Orange;
			Item.value = 500;
			Item.useStyle = 1;
			Item.useAnimation = 50;
			Item.useTime = 50;
			Item.knockBack = 8;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.noMelee = true;
			Item.shootSpeed = 2f;
			Item.maxStack = 30;
			Item.shoot = ModContent.ProjectileType<MegidoProj>();
		}

		public bool UseStacks(SGAPlayer sgaply, int time, int count = 1)
		{
			Player player = sgaply.Player;
			if (Main.rand.Next(100) < 20 && sgaply.personaDeck)
			{
				sgaply.Player.QuickSpawnItem(ModContent.ItemType<TheJoker>(), count);
				SoundEngine.PlaySound(SoundID.Item16.WithPitchVariance(0.20f), sgaply.Player.Center);

				int HPlost = count * 20;

				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.Red, HPlost, dramatic: false, dot: false);

				player.statLife -= HPlost;
				if (player.statLife < 1)
				{
					player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " had a fatal change of heart"), 1337, 0);
				}

				sgaply.AddCooldownStack((int)(time * (sgaply.personaDeck ? 0.50f : 1f)), count);

				return false;
			}

			for(int i = 0; i < count; i += 1)
            {
				if (player.HasItem(ModContent.ItemType<TheJoker>()))
				{
					player.ConsumeItem(ModContent.ItemType<TheJoker>(), true);
					player.HealEffect(20);
					player.netLife = true;
					player.statLife += 20;
					SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/P5Loot").WithVolume(1f).WithPitchVariance(.10f), player.Center);
				}
            }

			return sgaply.AddCooldownStack((int)(time * (sgaply.personaDeck ? 0.50f : 1f)), count);

		}

		public override bool CanUseItem(Player player)
		{
			if (player.SGAPly().AddCooldownStack(100, testOnly: true))
			{
				NPC[] findnpc = SGAUtils.ClosestEnemies(player.Center, 1500, checkWalls: false, checkCanChase: true)?.ToArray();
				if (findnpc != null && findnpc.Length > 0)
					return true;
			}
			return false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/MegidoSnd").WithVolume(.7f).WithPitchVariance(.15f), player.Center);
			UseStacks(player.SGAPly(),60 * 20);

			for (int i = 0; i < 4; i += 1)
			{
				NPC[] findnpc = SGAUtils.ClosestEnemies(player.Center, 1500,Main.MouseWorld, checkWalls: false, checkCanChase: true)?.ToArray();
				NPC target = findnpc[i % findnpc.Count()];

				Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.UnitX.RotatedBy(MathHelper.PiOver4 + (i * (MathHelper.TwoPi / 4f))) * 8f, ModContent.ProjectileType<MegidoProj>(), damage, knockBack, player.whoAmI, 0, target.whoAmI);
				proj.ai[1] = target.whoAmI;
				proj.netUpdate = true;
			}



			return false;
		}
	}

	public class MegidoProj : NPCs.PinkyMinionKilledProj
	{

		protected override float ScalePercent => MathHelper.Clamp(Projectile.timeLeft / 10f, 0f, Math.Min(Projectile.localAI[0] / 3f, 0.75f));
		protected override float SpinRate => 0.20f;

		Vector2 startingloc = default;
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Megido Proj");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public void CheckApoco(ref int damage,NPC npc,Projectile proj,bool always=false)
        {
			float kb = 0f;
			bool crit = false;
			double[] highestApoco = Main.player[Projectile.owner].SGAPly().apocalypticalChance.OrderBy(testby => 10000 - testby).ToArray();
			damage += npc.defense / 2;

			if (npc.realLife >= 0)
				damage = (int)(damage * 0.10f);

			if (always || Main.rand.NextFloat(100f)< highestApoco[0])
			npc.SGANPCs().DoApoco(npc, proj, Main.player[Projectile.owner], null, ref damage, ref kb, ref crit,2,true);
        }

		public override void ReachedTarget(NPC target)
		{
			Player player = Main.player[Projectile.owner];
			int damage = Main.DamageVar(Projectile.damage) + target.defense / 2;
			CheckApoco(ref damage, target, Projectile);
			target.StrikeNPC(damage, 0, 1, false);
			SGAmod.AddScreenShake(6f, 2400, target.Center);
			Main.player[Projectile.owner].addDPS(damage);

			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, target.whoAmI, Projectile.damage, 0f, (float)1, 0, 0, 0);
			}

			Projectile.velocity = Vector2.Zero;

			for (int i = 0; i < 24; i += 1)
			{
				Vector2 position = Main.rand.NextVector2Circular(16f, 16f);
				int num128 = Dust.NewDust(Projectile.Center + position, 0, 0, DustID.AncientLight, 0, 0, 240, Color.Aqua, ScalePercent);
				Main.dust[num128].noGravity = true;
				Main.dust[num128].alpha = 180;
				Main.dust[num128].color = Color.Lerp(Color.Aqua, Color.Blue, Main.rand.NextFloat() % 1f);
				Main.dust[num128].velocity = (Vector2.Normalize(position) * Main.rand.NextFloat(2f, 5f));
			}

			var sound = SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 86);
			if (sound != null)
				sound.Pitch -= 0.50f;

			Projectile.ai[0] += 1;
			Projectile.timeLeft = (int)MathHelper.Clamp(Projectile.timeLeft, 0, 10);
			Projectile.netUpdate = true;
		}



		public override void AI()
		{
			if (startingloc == default)
			{
				startingloc = Projectile.Center;
			}

			Projectile.localAI[0] += 0.25f;

			List<Point> weightedPoints2 = new List<Point>();

			int index = 0;
			int us = 0;

			NPC findnpc = Main.npc[(int)Projectile.ai[1]];

			if (findnpc != null && findnpc.active)
			{
				Projectile.velocity *= 0.94f;
				if (Projectile.localAI[0] > 8f && Projectile.ai[0] < 1)
				{
					NPC target = findnpc;
					int dist = 60 * 60;
					Vector2 distto = target.Center - Projectile.Center;
					Projectile.velocity += Vector2.Normalize(distto).RotatedBy((MathHelper.Clamp(1f - (Projectile.localAI[0] - 8f) / 5f, 0f, 1f) * 0.85f) * SpinRate) * 3.20f;
					Projectile.velocity = Vector2.Normalize(Projectile.velocity) * MathHelper.Clamp(Projectile.velocity.Length(), 0f, 32f + Projectile.localAI[0]);

					if (Projectile.timeLeft > 10 && Projectile.ai[0] < 1 && distto.LengthSquared() < dist)
					{
						ReachedTarget(target);
					}
				}
			}
			else
			{
				Projectile.timeLeft = (int)MathHelper.Clamp(Projectile.timeLeft, 0, 10);
			}

			Projectile.velocity *= 0.97f;

			if (Projectile.ai[0] > 0)
			{
				Projectile.ai[0] += 1;
			}

			int num126 = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(8f, 8f), 0, 0, DustID.t_Marble, 0, 0, 240, Color.Aqua, ScalePercent);
			Main.dust[num126].noGravity = true;
			Main.dust[num126].velocity = Projectile.velocity * Main.rand.NextFloat(0.1f, 0.5f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 0; i < Projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
			{
				if (Projectile.oldPos[i] == default)
					Projectile.oldPos[i] = Projectile.position;
			}


			TrailHelper trail = new TrailHelper("DefaultPass", Mod.Assets.Request<Texture2D>("noise").Value);
			//UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI);
			Color colorz = Color.Turquoise;
			trail.color = delegate (float percent)
			{
				return Color.Lerp(colorz, Color.DarkCyan, MathHelper.Clamp(Projectile.ai[0] / 7f, 0f, 1f));
			};
			trail.projsize = Projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -1f);
			trail.trailThickness = 4 + MathHelper.Clamp(Projectile.ai[0], 0f, 30f) / 20f;
			trail.trailThicknessIncrease = 6;
			//trail.capsize = new Vector2(6f, 0f);
			trail.strength = ScalePercent;
			trail.DrawTrail(Projectile.oldPos.ToList(), Projectile.Center);

			trail = new TrailHelper("BasicEffectDarkPass", Mod.Assets.Request<Texture2D>("TrailEffect").Value);
			//UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI);
			trail.projsize = Projectile.Hitbox.Size() / 2f;
			trail.coordMultiplier = new Vector2(1f, 2f);
			trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -2f);
			trail.trailThickness = 3 + MathHelper.Clamp(Projectile.ai[0], 0f, 30f) / 20f;
			trail.trailThicknessIncrease = 6;
			//trail.capsize = new Vector2(6f, 0f);
			trail.strength = ScalePercent;
			trail.DrawTrail(Projectile.oldPos.ToList(), Projectile.Center);


			Texture2D mainTex = SGAmod.ExtraTextures[96];

			float blobSize = (MathHelper.Clamp(Projectile.localAI[0], 0f, 4f) * 0.1f) + (MathHelper.Clamp(Projectile.ai[0], 0f, 30f) * 0.150f);

			Main.spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, Color.Lerp(colorz, Color.Black, 0.40f) * trail.strength, 0, mainTex.Size() / 2f, blobSize, default, 0);
			Main.spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, Color.Lerp(colorz,Color.White,0.25f)*0.75f * trail.strength, 0, mainTex.Size() / 2f, blobSize*0.75f, default, 0);

			UnifiedRandom random = new UnifiedRandom(Projectile.whoAmI);
			for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 32f)
			{
				float angle = random.NextFloat(MathHelper.TwoPi);
				Vector2 loc = Vector2.UnitX.RotatedBy(angle) * (random.NextFloat(6f, 26f) * blobSize);

				Main.spriteBatch.Draw(mainTex, Projectile.Center + loc - Main.screenPosition, null, Color.Lerp(Color.Turquoise, Color.Black, 0.50f) * 0.50f * trail.strength, angle, mainTex.Size() / 2f, new Vector2(blobSize / 12f, blobSize / 6f), default, 0);
			}

			return false;
		}
	}

	public class Megidola : Megido
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Megidola");
			Tooltip.SetDefault("Targets 3 enemies nearby your cursor on use and spawns orbs near them\nEach of these orbs zap nearby enemies 4 times for the listed damage\n" + Idglib.ColorText(Color.Orange, "Requires 2 Cooldown stack, adds 30 seconds"));
		}
		public override void SetDefaults()
		{
			base.SetDefaults();

			Item.damage = 350;
			Item.width = 48;
			Item.height = 48;
			Item.useTurn = true;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = 500;
			Item.useStyle = 1;
			Item.useAnimation = 50;
			Item.useTime = 50;
			Item.knockBack = 8;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.noMelee = true;
			Item.shootSpeed = 2f;
			Item.maxStack = 30;
			Item.shoot = ModContent.ProjectileType<MegidolaProj>();
		}

		public override bool CanUseItem(Player player)
		{
			if (player.SGAPly().AddCooldownStack(200, 2, testOnly: true))
			{
				NPC[] findnpc = SGAUtils.ClosestEnemies(player.Center, 1500, checkWalls: false, checkCanChase: true)?.ToArray();
				if (findnpc != null && findnpc.Length > 0)
					return true;
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Megido>(), 2).AddIngredient(ModContent.ItemType<WovenEntrophite>(), 10).AddTile(TileID.MythrilAnvil).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Megido").WithVolume(1.0f).WithPitchVariance(.15f), player.Center);
			if (UseStacks(player.SGAPly(), 60 * 30, 2))
			{

				for (int i = 0; i < 3; i += 1)
				{
					NPC[] findnpc = SGAUtils.ClosestEnemies(player.Center, 1500, Main.MouseWorld, checkWalls: false, checkCanChase: true)?.ToArray();
					NPC target = findnpc[i % findnpc.Count()];

					Projectile proj = Projectile.NewProjectileDirect(target.Center + Vector2.UnitX.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(72f, 160f), Vector2.Zero, ModContent.ProjectileType<MegidolaProj>(), damage, knockBack, player.whoAmI);
					proj.ai[0] = -12f * i;
					proj.netUpdate = true;
				}
			}

			return false;
		}
	}

	public class MegidolaProj : MegidoProj
	{
		Vector2 startingloc = default;
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 72;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Megidola Proj");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void AI()
		{
			if (startingloc == default)
			{
				startingloc = Projectile.Center;
			}

			if (Projectile.ai[0]%4 == 0 && Projectile.ai[0]>16 && Projectile.ai[0] < 32)
            {
				List<NPC> enemies = SGAUtils.ClosestEnemies(Projectile.Center, 640, Projectile.Center,checkWalls: false);
				if (enemies != null && enemies.Count > 0)
				{
					NPC target = enemies[((int)Projectile.ai[1])%enemies.Count];

					Projectile proj = Projectile.NewProjectileDirect(Projectile.Center, target.Center - Projectile.Center, ModContent.ProjectileType<MegidolaLaserProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
					proj.ai[1] = target.whoAmI;
					proj.netUpdate = true;

					//var snd2 = Main.PlaySound(50, (int)projectile.Center.X, (int)projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/Megido"));
					//snd.PlaySound(ref snd2,1f,0f,SoundType.Custom);

					SoundEffectInstance snd = SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/MegidoSnd"), Projectile.Center);
					if (snd != null)
						snd.Pitch = -0.75f+(Projectile.ai[1]/4f);
					//	snd.PlaySound(ref snd2, 1f, 0f, SoundType.Custom);

					Projectile.ai[1]++;
					Projectile.netUpdate = true;
				}

            }

			if (Projectile.ai[0] < 1)
				Projectile.timeLeft += 1;

			Projectile.localAI[0] += 1f;
			Projectile.ai[0] += 1;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float alpha = MathHelper.Clamp(Projectile.ai[0] / 12f, 0f, 1f);
			alpha *= MathHelper.Clamp((Projectile.timeLeft+20) / 8f, 0f, 1f);
			float blobSize = (MathHelper.Clamp(Projectile.ai[0]/2f, 0f, 8f) * 0.1f);
			blobSize *= MathHelper.Clamp(Projectile.timeLeft / 10f, 0f, 1f);

			Texture2D mainTex = SGAmod.ExtraTextures[96];
			Color colorz = Color.Turquoise;


			//blob orb
			Main.spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, colorz * alpha, 0, mainTex.Size() / 2f, blobSize, default, 0);
			Main.spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, Color.Lerp(colorz, Color.Black,50) * alpha, 0, mainTex.Size() / 2f, blobSize*0.85f, default, 0);


			UnifiedRandom random = new UnifiedRandom(Projectile.whoAmI);
			for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 32f)
			{
				float rotter = random.NextFloat(0.05f, 0.125f) * (random.NextBool() ? -5f : 5f)*Main.GlobalTimeWrappedHourly;
				float angle = random.NextFloat(MathHelper.TwoPi)+ rotter;
				Vector2 loc = Vector2.UnitX.RotatedBy(angle) * (random.NextFloat(6f, 26f) * blobSize);

				Main.spriteBatch.Draw(mainTex, Projectile.Center + loc - Main.screenPosition, null, Color.Lerp(Color.Turquoise, Color.Black, 0.50f) * 0.50f * alpha, angle, mainTex.Size() / 2f, new Vector2(blobSize / 12f, blobSize / 6f), default, 0);
			}

			return false;
		}
	}

	public class MegidolaLaserProj : MegidoProj
	{
		Vector2 startingloc = default;
		Vector2 hitboxchoose = default;
		protected Color color1 = Color.DarkCyan;
		protected Color color2 = Color.Turquoise;
		protected virtual float AppearTime => 6f;

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 16;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Megidola Laser Proj");
		}

		public override void AI()
		{
			NPC enemy = Main.npc[(int)Projectile.ai[1]];

			if (startingloc == default)
			{
				startingloc = Projectile.Center;
			}
			
			Projectile.localAI[0] += 1f;
			//projectile.ai[0] += 1;

			if (enemy != null && enemy.active && Projectile.localAI[1] < 1)
            {
				if (hitboxchoose == default)
                {
					hitboxchoose = new Vector2(Main.rand.Next(enemy.width), Main.rand.Next(enemy.height));
                }
				Projectile.velocity = (enemy.position+hitboxchoose) - Projectile.Center;

				if (Projectile.localAI[0] == 8)
				{
					int damage = Main.DamageVar(Projectile.damage);
					CheckApoco(ref damage, enemy, Projectile);
					enemy.StrikeNPC(damage, 0, 1, false);
					SGAmod.AddScreenShake(6f, 2400, enemy.Center);
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
			float alpha = MathHelper.Clamp(Projectile.localAI[0] / AppearTime, 0f, 1f)* MathHelper.Clamp(Projectile.timeLeft / AppearTime, 0f, 1f);

			Vector2[] points = new Vector2[] { startingloc, startingloc+Projectile.velocity };

			TrailHelper trail = new TrailHelper("BasicEffectAlphaPass", Mod.Assets.Request<Texture2D>("SmallLaser").Value);
			//UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI);
			Color colorz = color1;
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
			trail.strength = alpha*2f;
			trail.DrawTrail(points.ToList(), Projectile.Center);

			Texture2D mainTex = SGAmod.ExtraTextures[96];
			Main.spriteBatch.Draw(mainTex, startingloc + Projectile.velocity - Main.screenPosition, null, Color.Lerp(colorz2, Color.Black, 0.40f)*1f, 0, mainTex.Size() / 2f, alpha * 0.50f, default, 0);
			Main.spriteBatch.Draw(mainTex, startingloc + Projectile.velocity - Main.screenPosition, null, Color.Lerp(colorz2, Color.White, 0.25f) * 0.75f, 0, mainTex.Size() / 2f, alpha * 0.32f, default, 0);


			return false;
		}
	}

	public class Megidolaon : Megido
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Megidolaon");
			Tooltip.SetDefault("Calls down a big boom on the ground below where you use it\n" + Idglib.ColorText(Color.Orange, "Requires 3 Cooldown stacks, adds 45 seconds"));
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 2500;
			Item.width = 48;
			Item.height = 48;
			Item.useTurn = true;
			Item.rare = ItemRarityID.Yellow;
			Item.value = 500;
			Item.useStyle = 1;
			Item.useAnimation = 50;
			Item.useTime = 50;
			Item.knockBack = 8;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.noMelee = true;
			Item.shootSpeed = 2f;
			Item.maxStack = 30;
			Item.shoot = ModContent.ProjectileType<MegidolaonProj>();
		}

		public override bool CanUseItem(Player player)
		{
			if (player.SGAPly().AddCooldownStack(100, 3, testOnly: true))
			{
				return true;
			}
			return false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (UseStacks(player.SGAPly(), 60 * 45, 3))
			{
				int pushYUp = -1;
				player.FindSentryRestingSpot(Item.shoot, out var worldX, out var worldY, out pushYUp);

				Projectile proj = Projectile.NewProjectileDirect(new Vector2(worldX, worldY), Vector2.Zero, ModContent.ProjectileType<MegidolaonProj>(), damage, knockBack, player.whoAmI);
			}

			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Megidola>(), 2).AddIngredient(ModContent.ItemType<OverseenCrystal>(), 6).AddIngredient(ItemID.BeetleHusk, 2).AddTile(TileID.LunarCraftingStation).Register();
		}
	}

	public class MegidolaonProj : MegidoProj
	{
		public class CloudBoom
		{
			public Vector2 position;
			public Vector2 speed;
			public float angle;
			public int cloudType;
			public Vector2 scale = new Vector2(1f, 1f);

			public int timeLeft = 20;
			public int timeLeftMax = 20;
			public CloudBoom(Vector2 position, Vector2 speed, float angle, int cloudtype)
			{
				this.position = position;
				this.speed = speed;
				this.angle = angle;
				this.cloudType = cloudtype;
			}
			public void Update()
			{
				timeLeft -= 1;
				position += speed;
			}

		}

		public List<CloudBoom> boomOfClouds = new List<CloudBoom>();

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Megidolaon Proj");
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			Projectile.ai[0] += 1;
			Projectile.localAI[0] += 1;
			if (Projectile.localAI[0] == 5)
			{
				SGAmod.AddScreenShake(20f, 2000, Projectile.Center);

				ScreenExplosion explode = SGAmod.AddScreenExplosion(Projectile.Center, Projectile.timeLeft+40, 2f, 1200);
				if (explode != null)
				{
					explode.warmupTime = 16;
					explode.decayTime = 180;
					explode.strengthBasedOnPercent = delegate (float percent)
					{
						return 2f;
					};
				}

				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/MegidolaonSnd").WithVolume(1f).WithPitchVariance(.15f), Projectile.Center);
			}

			/*if (projectile.timeLeft == 300)
			{
				ScreenExplosion explode = SGAmod.AddScreenExplosion(projectile.Center, 300, 1.25f, 2400);
				if (explode != null)
				{
					explode.warmupTime = 200;
					explode.decayTime = 64;
				}
			}*/

			if (Projectile.ai[0] > 5 && Projectile.timeLeft > 30)
			{
				if (SGAmod.ScreenShake < 16)
					SGAmod.AddScreenShake(MathHelper.Clamp(Projectile.timeLeft / 150f, 0f, 1f) * 6f, 2000 * MathHelper.Clamp(Projectile.timeLeft / 200f, 0f, 1f), Projectile.Center);
				if (Projectile.ai[0] % 10 == 0 && Projectile.timeLeft > 30)
				{
					int dist = (int)((500 * 500) * ScaleUpeffect * BoomScaleup);
					foreach (NPC enemy in Main.npc.Where(testby => testby.IsValidEnemy() && testby.Center.Y < Projectile.Center.Y && (testby.Center - Projectile.Center).LengthSquared() < dist))
					{
						bool crit = true;
						int damage = (int)(Main.DamageVar(Projectile.damage));
						if (Projectile.localNPCImmunity[enemy.whoAmI] < 100)
						{
							Projectile.localNPCImmunity[enemy.whoAmI] = 99999;
						}
						else
						{
							crit = false;
							damage = (int)(damage / 10f);
						}
						CheckApoco(ref damage, enemy, Projectile, false);
						enemy.StrikeNPC(damage, 0, 1, crit);
						Main.player[Projectile.owner].addDPS(damage);
					}
				}
			}

			if (Projectile.ai[0] > 5)
			{
				for (int i = 0; i < 12*(ScalePercent*3f); i += 1)
				{
					float maxspread = 256;
					Vector2 locSpawn = Main.rand.NextVector2Circular(maxspread, 30f);

					float xPercent = ((locSpawn.X) / (maxspread))*1f;

					/*
					if (xPercent < 0)
						xPercent = Math.Max(xPercent + 0.75f, 0);
					if (xPercent > 0)
						xPercent = Math.Min(xPercent - 0.75f, 0);
					*/



					Vector2 velocity = (-Vector2.UnitY) * Main.rand.NextFloat(1f, 4f);// * (0.45f + (ScaleUpeffect / 3f));
					CloudBoom boomer = new CloudBoom(new Vector2(Main.rand.NextFloat(-maxspread, maxspread),24f+ locSpawn.Y), velocity.RotatedBy(xPercent*MathHelper.PiOver2*0.32f), Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.Next(1, 7));
					boomer.scale = Vector2.One * (0.25f) * new Vector2(Main.rand.NextFloat(0.50f, 0.75f), Main.rand.NextFloat(0.75f, 1f));

					boomOfClouds.Add(boomer);
				}

				boomOfClouds = boomOfClouds.Where(testby => testby.timeLeft > 0).ToList();

				foreach (CloudBoom cb in boomOfClouds)
				{
					cb.Update();
				}
			}
		}

		float ScaleUpeffect => 0.60f + Projectile.localAI[0] / 300f;
		float BoomScaleup => 1f - (1f / ((Projectile.localAI[0] / 30f) + 1f));

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D beamTex = ModContent.Request<Texture2D>("SGAmod/LightBeam");
			Texture2D glowOrb = ModContent.Request<Texture2D>("SGAmod/GlowOrb");
			Vector2 offsetbeam = new Vector2(beamTex.Width / 2f, beamTex.Height / 4f);
			float timeLeft = MathHelper.Clamp(Projectile.timeLeft / 30f, 0f, 1f);
			float timeLeft2 = MathHelper.Clamp(Projectile.timeLeft / 200f, 0f, 1f);
			UnifiedRandom random = new UnifiedRandom(Projectile.whoAmI);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			//Boom flash

			float orbExplosion = (Projectile.localAI[0]-5) / 4f;
			float orbAlpha = MathHelper.Clamp(2f - ((Projectile.localAI[0]-5) / 8f), 0f, 1f);

			if (orbAlpha>0 && Projectile.localAI[0]>4)
			Main.spriteBatch.Draw(glowOrb, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.White, Color.Turquoise, 0.50f)* orbAlpha, 0, glowOrb.Size()/2f, orbExplosion, default, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			//Sky Beam
			Vector2 beamscale = new Vector2(1f, 3f);
			float beamAlpha = MathHelper.Clamp((Projectile.localAI[0]) / 4f, 0f, 1f) * (1f - MathHelper.Clamp((Projectile.localAI[0] - 3) / 20f, 0f, 1f));
			Main.spriteBatch.Draw(beamTex, Projectile.Center + new Vector2(0,-1200) - Main.screenPosition, null, Color.Lerp(Color.White, Color.Aqua, 0.50f)*timeLeft, 0, offsetbeam, beamscale*new Vector2(beamAlpha,2f), default, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			float scaleup = (Projectile.scale * BoomScaleup) * (1f + ScaleUpeffect) * 1f;

			Texture2D noise = ModContent.Request<Texture2D>("SGAmod/TiledPerlin");

			float flashBoomGrow = MathHelper.Clamp((Projectile.localAI[0] - 5) / 20f, 0f, 1f);
			float flashBoomColor = MathHelper.Clamp((Projectile.localAI[0] - 5) / 60f, 0f, 1f);
			float flashBoomColor2 = MathHelper.Clamp((Projectile.localAI[0] - 5) / 300f, 0f, 1f);

			//Big Booms (Alpha Blend)

			Color whiteMix = Color.Lerp(Color.White, Color.DarkTurquoise, 0.25f);
			Color boomColor = Color.Lerp(whiteMix, Color.Lerp(Color.Turquoise, Color.DarkTurquoise, timeLeft2), flashBoomColor);
			Color boomColor2 = Color.Lerp(whiteMix, Color.Lerp(Color.Lerp(Color.Turquoise,Color.White, 0.0050f), Color.DarkTurquoise, timeLeft2/2f), flashBoomColor);

			SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(boomColor.ToVector4()* timeLeft2* flashBoomGrow);
			SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(ModContent.Request<Texture2D>("SGAmod/TiledPerlin"));
			SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(2f, 0.5f));
			SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(0,Main.GlobalTimeWrappedHourly * 1f));
			SGAmod.SphereMapEffect.Parameters["softEdge"].SetValue(2f);
			SGAmod.SphereMapEffect.Parameters["softHalf"].SetValue(new Vector2(8f,0.50f));

			SGAmod.SphereMapEffect.CurrentTechnique.Passes["HalfSphereMap"].Apply();

			spriteBatch.Draw(noise, Projectile.Center - Main.screenPosition, null, Color.White * timeLeft, 0, noise.Size() / 2f, scaleup, SpriteEffects.None, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			
			//Big Booms (Additive)

			SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(boomColor2.ToVector4()*2f * timeLeft2 * flashBoomGrow);
			SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(ModContent.Request<Texture2D>("SGAmod/TiledPerlin"));
			SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(1f, 0.25f));
			SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(0.50f, Main.GlobalTimeWrappedHourly * 0.60f));
			SGAmod.SphereMapEffect.Parameters["softEdge"].SetValue(4f);
			SGAmod.SphereMapEffect.Parameters["softHalf"].SetValue(new Vector2(8f, 0.50f));

			SGAmod.SphereMapEffect.CurrentTechnique.Passes["HalfSphereMapAlpha"].Apply();

			spriteBatch.Draw(noise, Projectile.Center - Main.screenPosition, null, Color.White * timeLeft, 0, noise.Size() / 2f, scaleup, SpriteEffects.None, 0);

			SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(0.5f, 0.45f));
			SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(0.50f, Main.GlobalTimeWrappedHourly * 0.20f));
			SGAmod.SphereMapEffect.Parameters["softEdge"].SetValue(3f);
			SGAmod.SphereMapEffect.CurrentTechnique.Passes["HalfSphereMapAlpha"].Apply();

			spriteBatch.Draw(noise, Projectile.Center - Main.screenPosition, null, Color.White * timeLeft, 0, noise.Size() / 2f, scaleup, SpriteEffects.None, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			//Black Clouds

			Color BlackColors = Color.Lerp(Color.White,Color.Lerp(Color.DarkTurquoise, Color.Black, flashBoomColor2), BoomScaleup);
											//Color.Lerp(Color.DarkTurquoise,Color.Black,0.25f)*0.75f;// 

			float boomScale = MathHelper.Clamp(timeLeft2 * 1.5f, 0f, 1f);

			foreach (CloudBoom cb in boomOfClouds.Where(testby => testby.timeLeft > 0))
			{
				Texture2D cloudTex = ModContent.Request<Texture2D>("SGAmod/NPCs/Hellion/Clouds" + cb.cloudType);
				float cbalpha = MathHelper.Clamp((cb.timeLeft / (float)cb.timeLeftMax)*6f, 0f, 1f);
				float cloudfadeAlpha = Math.Min((cb.timeLeftMax - cb.timeLeft) / 6f, 1f) * 1f;
				float cloudSideAlpha = MathHelper.Clamp(2.4f-Math.Abs(cb.position.X) / 120f,0f,1f);

				Vector2 posser = Projectile.Center + cb.position* scaleup;

				Main.spriteBatch.Draw(cloudTex, posser - Main.screenPosition, null, BlackColors.MultiplyRGBA(new Color(1f,1f,1f, 0.32f*boomScale * cbalpha * cloudfadeAlpha)) * cloudSideAlpha, cb.angle, cloudTex.Size() / 2f, cb.scale* (1f+(scaleup-1f)), default, 0);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			//Glowing inside of clouds

			Color glowingColors = Color.Lerp(Color.White, Color.Lerp(Color.White, Color.DarkTurquoise, flashBoomColor2), BoomScaleup)*1.5f;
			//Color.Lerp(Color.DarkTurquoise,Color.Black,0.25f)*0.75f;

			foreach (CloudBoom cb in boomOfClouds.Where(testby => testby.timeLeft > 0))
			{
				Texture2D cloudTex = ModContent.Request<Texture2D>("SGAmod/NPCs/Hellion/Clouds" + cb.cloudType);
				float cbalpha = MathHelper.Clamp((cb.timeLeft / (float)cb.timeLeftMax)*6f, 0f, 1f);
				float cloudfadeAlpha = Math.Min((cb.timeLeftMax - cb.timeLeft) / 6f, 1f) * 1f;
				float cloudSideAlpha = MathHelper.Clamp(2.4f - Math.Abs(cb.position.X) / 120f, 0f, 1f);

				Vector2 posser = Projectile.Center + cb.position* scaleup;

				Main.spriteBatch.Draw(cloudTex, posser - Main.screenPosition, null, glowingColors.MultiplyRGBA(new Color(1f,1f,1f, 0.75f*boomScale * cbalpha * cloudfadeAlpha*0.25f))* cloudSideAlpha, cb.angle, cloudTex.Size() / 2f, (cb.scale* (1f+(scaleup-1f)))*0.50f, default, 0);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}
	}



	public class MorningStar : Megido
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morning Star");
			Tooltip.SetDefault("Calls down Lucifer's signature move to bring massive destruction in a wide area\n" + Idglib.ColorText(Color.Orange, "Requires 4 Cooldown stacks, adds 80 seconds"));
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 1000;
			Item.width = 48;
			Item.height = 48;
			Item.useTurn = true;
			Item.rare = ItemRarityID.Red;
			Item.value = 500;
			Item.useStyle = 1;
			Item.useAnimation = 50;
			Item.useTime = 50;
			Item.knockBack = 8;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.noMelee = true;
			Item.shootSpeed = 2f;
			Item.maxStack = 30;
			Item.shoot = ModContent.ProjectileType<MorningStarProj>();
		}

		public override bool CanUseItem(Player player)
		{
			if (player.SGAPly().AddCooldownStack(100, 4, testOnly: true))
			{
				return true;
			}
			return false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (UseStacks(player.SGAPly(), 60 * 80, 4))
			{
				int pushYUp = -1;
				player.FindSentryRestingSpot(Item.shoot, out var worldX, out var worldY, out pushYUp);

				Projectile proj = Projectile.NewProjectileDirect(new Vector2(worldX, worldY), Vector2.Zero, ModContent.ProjectileType<MorningStarProj>(), damage, knockBack, player.whoAmI);
			}

			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Megidolaon>(), 2).AddIngredient(ModContent.ItemType<IlluminantEssence>(), 6).AddRecipeGroup("SGAmod:CelestialFragments",3).AddTile(TileID.LunarCraftingStation).Register();
		}
	}

	public class MorningStarProj : MegidoProj
	{
		public class CloudBoom
		{
			public Vector2 position;
			public Vector2 speed;
			public float angle;
			public int cloudType;
			public Vector2 scale = new Vector2(1f, 1f);

			public int timeLeft = 20;
			public int timeLeftMax = 20;
			public CloudBoom(Vector2 position, Vector2 speed, float angle, int cloudtype)
			{
				this.position = position;
				this.speed = speed;
				this.angle = angle;
				this.cloudType = cloudtype;
			}
			public void Update()
			{
				timeLeft -= 1;
				position += speed;
			}

		}

		public List<CloudBoom> boomOfClouds = new List<CloudBoom>();

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 500;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Morning Star Proj");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}



		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			Projectile.ai[0] += 1;
			Projectile.localAI[0] += 1;
			if (Projectile.localAI[0] == 1)
			{
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/MorningStar").WithVolume(1f).WithPitchVariance(.15f), Projectile.Center);
			}

			if (Projectile.ai[0] == 180)
            {
				ScreenExplosion explode = SGAmod.AddScreenExplosion(Projectile.Center, Projectile.timeLeft, 2f, 1600);
				if (explode != null)
				{
					explode.warmupTime = 16;
					explode.decayTime = 64;
					explode.strengthBasedOnPercent = delegate (float percent)
					{
						return 2f+MathHelper.Clamp((percent-0.5f)*2f,0f,1f)*1f;
					};
				}
			}

			/*if (projectile.timeLeft == 300)
			{
				ScreenExplosion explode = SGAmod.AddScreenExplosion(projectile.Center, 300, 1.25f, 2400);
				if (explode != null)
				{
					explode.warmupTime = 200;
					explode.decayTime = 64;
				}
			}*/

			if (Projectile.ai[0] > 180)
			{
				bool endhit = Projectile.timeLeft == 30;
				if (SGAmod.ScreenShake < 16)
					SGAmod.AddScreenShake(6f, 3200, Projectile.Center);
				if ((Projectile.ai[0] % 10 == 0 && Projectile.timeLeft > 30) || endhit)
				{
					foreach (NPC enemy in Main.npc.Where(testby => testby.IsValidEnemy()))
					{
						Rectangle rect = new Rectangle((int)Projectile.Center.X - 240, (int)Projectile.Center.Y - 1000, 480, 1200);
						if (endhit)
							rect = new Rectangle((int)Projectile.Center.X - 1600, (int)Projectile.Center.Y - 1600, 3200, 3200);


						if (enemy.Hitbox.Intersects(rect))
						{
							int damage = (int)((Main.DamageVar((Projectile.damage))) * (endhit ? 5f : 1f));
							CheckApoco(ref damage, enemy, Projectile, endhit);
							enemy.StrikeNPC(damage, 0, 1, false);
							Main.player[Projectile.owner].addDPS(damage);
						}
					}
				}


				float scaleUpeffect = 0.75f + ((float)Math.Pow((Projectile.localAI[0] - 180f) / 160f, 4f));

				for (int i = 0; i < 8; i += 1)
				{
					CloudBoom boomer = new CloudBoom(Projectile.Center + Main.rand.NextVector2Circular(260f, 120f), Vector2.UnitX.RotatedBy(-Main.rand.NextFloat(MathHelper.Pi)) * Main.rand.NextFloat(20f, 26f) * (0.45f + (scaleUpeffect / 3f)), Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.Next(1, 7));
					boomer.scale = Vector2.One * (0.60f * scaleUpeffect) * new Vector2(Main.rand.NextFloat(0.50f, 0.75f), Main.rand.NextFloat(0.75f, 1f));

					boomOfClouds.Add(boomer);
				}

				boomOfClouds = boomOfClouds.Where(testby => testby.timeLeft > 0).ToList();

				foreach (CloudBoom cb in boomOfClouds)
				{
					cb.Update();
				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float alpha = 1f;
			Texture2D statTex = ModContent.Request<Texture2D>("SGAmod/Extra_57b");
			Texture2D beamTex = ModContent.Request<Texture2D>("SGAmod/LightBeam");
			Texture2D glowOrb = ModContent.Request<Texture2D>("SGAmod/GlowOrb");
			Vector2 offsetbeam = new Vector2(beamTex.Width / 2f, beamTex.Height / 4f);

			Vector2 starHalf = statTex.Size() / 2f;
			float timeLeft = MathHelper.Clamp(Projectile.timeLeft / 30f, 0f, 1f);
			float beamAlpha = MathHelper.Clamp((Projectile.localAI[0] - 180) / 20f, 0f, 1f);
			float scaleUpeffect = 0.75f + ((float)Math.Pow((Projectile.localAI[0] - 180f) / 160f, 4f));
			float endalpha = (1f - MathHelper.Clamp((scaleUpeffect - 3f) / 6f, 0f, 1f));


			List<(float, Vector2, float, float, Color)> listofstuff = new List<(float, Vector2, float, float, Color)>();

			UnifiedRandom random = new UnifiedRandom(Projectile.whoAmI);

			//Stars

			for (int i = 10; i < 160; i += 1)
			{
				float progress = (random.NextFloat(1f) +
					Main.GlobalTimeWrappedHourly * (random.NextFloat(0.04f, 0.075f) * (1f + beamAlpha * 25f))
					) % 1f;

				Vector2 pos = new Vector2(random.Next(-256, 256), -1200 + (progress * 1500f));
				float alphaentry = (1f - MathHelper.Clamp(((i * 2) - Projectile.localAI[0]) / 60f, 0f, 1f)) * ((float)Math.Sin(progress * MathHelper.Pi));
				float rot = (random.NextFloat(MathHelper.TwoPi) + (random.NextFloat(-0.01f, 0.01f) * Main.GlobalTimeWrappedHourly)) * (1f - beamAlpha);
				Color color = Main.hslToRgb(random.NextFloat(1f), 0.85f, 0.95f) * 0.5f;
				listofstuff.Add((progress, pos, alphaentry, rot, color));
			}

			foreach ((float, Vector2, float, float, Color) entry in listofstuff.OrderBy(testby => testby.Item1))
			{
				if (entry.Item3 > 0)
					Main.spriteBatch.Draw(statTex, Projectile.Center + entry.Item2 - Main.screenPosition, null, entry.Item5 * entry.Item3 * endalpha * timeLeft, entry.Item4, starHalf / 2f, new Vector2(1f, 1f + beamAlpha * 2f) * 0.50f, default, 0);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			//Sky Beams

			for (int i = 0; i < 10; i += 1)
			{
				float alpha3 = (MathHelper.Clamp((Projectile.localAI[0] - (i * 1.25f)) / 160f, 0f, 1f));
				Vector2 beamscale = new Vector2(1f + Projectile.localAI[0] / 240f, 8f + Projectile.localAI[0] / 320f);
				Vector2 offset = new Vector2(random.NextFloat(-64f, 64f), -640);
				float rot = 0f;

				Main.spriteBatch.Draw(beamTex, Projectile.Center + offset - Main.screenPosition, null, Color.Lerp(Color.White, Color.Aqua, 0.50f) * endalpha * alpha3 * timeLeft * (0.20f + (beamAlpha * 0.05f)), rot, offsetbeam, beamscale, default, 0);

			}

			//Big ass laser comes down!
			if (Projectile.localAI[0] > 180)
			{
				UnifiedRandom randomz = new UnifiedRandom(Projectile.whoAmI);

				float max = 3;
				//3 trails as the lasers
				for (int ii = 0; ii < max; ii += 1)
				{
					List<Vector2> poses = new List<Vector2>();
					for (float f = 0; f < 2200; f += 25)
					{
						poses.Add(new Vector2(Projectile.Center.X + (float)Math.Sin((ii * (MathHelper.TwoPi / max)) + (Main.GlobalTimeWrappedHourly * 12f) + (f / 400f)) * 90f, (Projectile.Center.Y - f)));
					}

					TrailHelper trail = new TrailHelper("BasicEffectAlphaPass", Mod.Assets.Request<Texture2D>("TrailEffect").Value);
					//UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI);
					Color colorz = Color.Aqua;
					trail.projsize = Projectile.Hitbox.Size() / 2f;
					trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * randomz.NextFloat(6.2f, 9f));
					trail.coordMultiplier = new Vector2(1f, randomz.NextFloat(1.5f, 4f));

					trail.strength = beamAlpha * endalpha * timeLeft * 8f;
					trail.strengthPow = 2f;
					trail.doFade = true;

					trail.color = delegate (float percent)
					{
						float alphacol = beamAlpha;
						return Color.Lerp(Color.Turquoise, colorz, MathHelper.Clamp(Projectile.ai[0] / 7f, 0f, 1f));
					};


					float extra = randomz.NextFloat(MathHelper.TwoPi);
					float randc = randomz.NextFloat(4f, 6f);
					float randd = randomz.NextFloat(2f, 4f);
					float rande = 1f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * randomz.NextFloat(1f, 1.25f)) * 0.15f;

					trail.trailThicknessFunction = delegate (float percent)
					{
						float math = (float)Math.Sin((Main.GlobalTimeWrappedHourly * -randc) + (percent * MathHelper.TwoPi * randd) + extra);
						float beamzz = MathHelper.Clamp((beamAlpha * 2f) - percent, 0f, 1f);

						return (90f + math * 45f) * (beamzz * rande);
					};

					trail.DrawTrail(poses, Projectile.Center);

				}

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				//Expanding orb at epicenter

				Color orbcolor = Color.Lerp(Color.PaleTurquoise, Color.White, MathHelper.Clamp((scaleUpeffect - 2f) / 6f, 0f, 1f)) * beamAlpha * timeLeft;

				Vector2 halfGlow = glowOrb.Size() / 2f;
				float scaleUpeffect2 = 0.75f + ((float)Math.Pow((Projectile.localAI[0] - 180f) / 240f, 15f));

				Main.spriteBatch.Draw(glowOrb, Projectile.Center - Main.screenPosition, null, orbcolor, 0, halfGlow, (new Vector2(1.45f, 1.15f) * scaleUpeffect) * beamAlpha, default, 0);

				//Smoke and clouds

				foreach (CloudBoom cb in boomOfClouds.Where(testby => testby.timeLeft > 0))
				{
					Texture2D cloudTex = ModContent.Request<Texture2D>("SGAmod/NPCs/Hellion/Clouds" + cb.cloudType);
					float cbalpha = MathHelper.Clamp(cb.timeLeft / (float)cb.timeLeftMax, 0f, 1f);
					float cloudfadeAlpha = Math.Min((cb.timeLeftMax - cb.timeLeft) / 12f, 1f) * 0.75f;

					Main.spriteBatch.Draw(cloudTex, cb.position - Main.screenPosition, null, Color.Lerp(Color.Lerp(Color.Aqua, Color.DarkCyan, cbalpha), Color.White, MathHelper.Clamp((scaleUpeffect - 2f) / 3f, 0f, 1f)) * beamAlpha * timeLeft * cbalpha * cloudfadeAlpha * endalpha, cb.angle, cloudTex.Size() / 2f, cb.scale, default, 0);
				}

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				//Expanding That covers all in the end

				Main.spriteBatch.Draw(glowOrb, Projectile.Center - Main.screenPosition, null, orbcolor * MathHelper.Clamp((scaleUpeffect - 1f) / 12f, 0f, 1f) * (MathHelper.Clamp((Projectile.timeLeft - 20f) / 20f, 0f, 1f)), 0, halfGlow, (new Vector2(0.8f, 0.6f) * scaleUpeffect2) * beamAlpha, default, 0);

			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}
	}

	public class RaysOfControl : Megido,IHellionDrop
	{
		int IHellionDrop.HellionDropAmmount() => 1 + Main.rand.Next(3);
		int IHellionDrop.HellionDropType() => ModContent.ItemType<RaysOfControl>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rays Of Control");
			Tooltip.SetDefault("'Unleash the wrath of all of mankind's greatest sins in one unholy blast'\n" + Idglib.ColorText(Color.Orange, "Requires 5 Cooldown stacks, adds 120 seconds"));
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 25000;
			Item.width = 48;
			Item.height = 48;
			Item.useTurn = true;
			Item.rare = ItemRarityID.Red;
			Item.value = 500;
			Item.useStyle = 1;
			Item.useAnimation = 50;
			Item.useTime = 50;
			Item.knockBack = 8;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.noMelee = true;
			Item.shootSpeed = 2f;
			Item.maxStack = 30;
			Item.shoot = ModContent.ProjectileType<RaysOfControlProj>();
		}

		public override bool CanUseItem(Player player)
		{
			if (player.SGAPly().AddCooldownStack(100, 5, testOnly: true))
			{
				return true;
			}
			return false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (UseStacks(player.SGAPly(), 60 * 120, 5))
			{
				position = player.Center - new Vector2(0, 320);

				Projectile proj = Projectile.NewProjectileDirect(position, Vector2.Zero, ModContent.ProjectileType<RaysOfControlProj>(), damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<MorningStar>(), 2).AddIngredient(ModContent.ItemType<DrakeniteBar>(), 2).AddIngredient(ModContent.ItemType<ByteSoul>(), 6).AddIngredient(ModContent.ItemType<AncientFabricItem>(), 6).AddIngredient(ModContent.ItemType<StygianCore>(), 1).AddTile(TileID.LunarCraftingStation).Register();
		}
    }

	public class RaysOfControlProj : MegidoProj
	{
		public class CloudBoom
		{
			public Vector2 position;
			public Vector2 speed;
			public float angle;
			public int cloudType;
			public Vector2 scale = new Vector2(1f, 1f);

			public int timeLeft = 20;
			public int timeLeftMax = 20;
			public CloudBoom(Vector2 position, Vector2 speed, float angle, int cloudtype)
			{
				this.position = position;
				this.speed = speed;
				this.angle = angle;
				this.cloudType = cloudtype;
			}
			public void Update()
			{
				timeLeft -= 1;
				position += speed;// *(timeLeft/(float)timeLeftMax);
			}
		}

		public class BeamOfLight
		{
			public List<Vector2> positions;

			public int timeLeft = 20;
			public int timeLeftMax = 20;
			public float random;
			public Func<float, float> thickness;

			public BeamOfLight(List<Vector2> positions)
			{
				this.positions = positions;
				random = Main.rand.NextFloat();
			}
			public void Update()
			{
				timeLeft -= 1;
			}
		}

		public List<CloudBoom> boomOfClouds = new List<CloudBoom>();
		public List<BeamOfLight> controlRays = new List<BeamOfLight>();

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rays Of Control Proj");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}



		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (!Main.dedServ)
			{
				RaysOfControlOrb.Load();
				RaysOfControlOrb.oneUpdate = true;

				Vector2 offset = Vector2.UnitX.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
				float timeIn = 1f - MathHelper.Clamp(Projectile.ai[0] / 90f, 0f, 1f);
				float timeIn2 = 1f - MathHelper.Clamp(Projectile.ai[0] / 110f, 0f, 1f);

				RaysOfControlOrb.OrbParticles orbFliesIn = new RaysOfControlOrb.OrbParticles(Vector2.Zero + offset * timeIn * 320f,
					(-offset * timeIn * 24f) + Main.rand.NextVector2Circular(4f, 4f) * timeIn2,
					Main.rand.NextFloat(MathHelper.TwoPi),
					Color.Red);

				RaysOfControlOrb.OrbParticles orbinMiddle = new RaysOfControlOrb.OrbParticles(Vector2.Zero,
	Main.rand.NextVector2Circular(4f, 4f),
	Main.rand.NextFloat(MathHelper.TwoPi),
	Color.Red);

				orbinMiddle.timeLeft = 4;
				orbinMiddle.timeLeftMax = 4;
				RaysOfControlOrb.partices.Add(orbinMiddle);
				orbFliesIn.scale = Vector2.One * (1f - timeIn2);
				RaysOfControlOrb.partices.Add(orbFliesIn);

				//CataLogo.DrawToRenderTarget();
			}

			Projectile.ai[0] += 1;
			Projectile.localAI[0] += 1;

			if (Projectile.ai[0] == 1)
			{
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/RaysOfControl").WithVolume(1f).WithPitchVariance(.15f), Projectile.Center);
				ScreenExplosion explode = SGAmod.AddScreenExplosion(Projectile.Center, Projectile.timeLeft-60, 2f, 4800);
				if (explode != null)
				{
					explode.warmupTime = 1;
					explode.decayTime = 64;
					explode.strengthBasedOnPercent = delegate (float percent)
					{
						float timer = Projectile.ai[0] - 120;
						return MathHelper.Clamp((Projectile.ai[0] - 120)/60f, 0f, 1f);// * MathHelper.Clamp(projectile.timeLeft/150f, 0f, 1f);
					};
				}
			}

			//if (projectile.localAI[0])
			//SGAmod.AddScreenExplosion(projectile.Center, 18, projectile.localAI[0]/60f, 1600);

			if (Projectile.ai[0] < 80 && Projectile.ai[0] > 10 && Projectile.ai[0] % 3 == 0)
			{
				List<Vector2> spots = new List<Vector2>();

				float direction = Main.rand.NextFloat(MathHelper.TwoPi);
				float direction2 = 0;
				Vector2 rando = direction.ToRotationVector2();

				Vector2 centerPoint = Projectile.Center + rando * (Projectile.localAI[0] * 1.25f);
				float rotteradd = (0.005f + Main.rand.NextFloat(0.025f)) * (Main.rand.NextBool() ? 1f : -1f);
				float rotscaler = 4f;

				for (float f = 0; f < 48; f += 1f)
				{
					spots.Add(centerPoint);
				}
				for (float f = 0; f < 1600; f += 20f)
				{
					spots.Add(centerPoint + rando.RotatedBy(direction2) * f);
					direction2 += rotteradd * rotscaler;
					rotscaler *= 0.96f;
				}
				spots.Reverse();
				BeamOfLight beam = new BeamOfLight(spots);
				beam.timeLeft = 32;
				beam.timeLeftMax = 32;
				beam.thickness = delegate (float percent)
				{
					float size = MathHelper.SmoothStep(0f, 1f, (float)Math.Sin(percent * (MathHelper.Pi - 0.5f)));

					//float size = MathHelper.SmoothStep(0f,1f,MathHelper.Clamp(1f - (Math.Abs(percent - percent) * 3f), 0f, 1f));

					return size * 32f;
				};
				controlRays.Add(beam);

			}

			if (Projectile.ai[0] > 120)
            {
				float timer = Projectile.ai[0] - 120;

				for (int i = 0; i < 5; i += 1)
				{
					float scaleUpeffect = 1f + (float)Math.Pow(timer / 20f, 1.5f);
					float scaleUpeffect2 = 1f / (1f + timer / 4f);
					float scaleUpeffect3 = 1f + (float)Math.Pow(timer / 20f, 3f);

					float growscale = scaleUpeffect3;
					if (scaleUpeffect3 > 32f)
						scaleUpeffect3 = 32f;

					Vector2 velocity = new Vector2(Main.rand.NextFloat(-24f, 24f), -16f);// -12f+(-(scaleUpeffect - 4f) * 4f));
					Vector2 offset = new Vector2(Main.rand.Next(Main.screenWidth), Main.screenHeight + Main.rand.NextFloat(160f,320f));

					//Vector2 centerboom = Vector2.SmoothStep(Vector2.Zero, offset, MathHelper.Clamp(timer / 120f, 0f, 1f));

					CloudBoom boomer = new CloudBoom(offset, velocity, 0, Main.rand.Next(1, 7));
					boomer.scale = new Vector2(0.5f,1f)+((new Vector2(0.32f, 0.5f) * ((scaleUpeffect) * 1.25f))/6f);
					boomer.timeLeft = Main.rand.Next(16,16+ (int)scaleUpeffect3);
					boomer.timeLeftMax = boomer.timeLeft;

					boomOfClouds.Add(boomer);

				}

				if (!Main.dedServ)
				{
					if (Projectile.ai[0] < 160)
                    {
						SGAmod.AddScreenShake(2, 2600, Projectile.Center);
					}

					RaysOfControlOrb.Load();
					RaysOfControlOrb.oneUpdate = true;

					Vector2 offset2 = Vector2.UnitX.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
					float timeIn = MathHelper.Clamp(timer / 60f, 0f, 1f);
					float timeIn2 = MathHelper.Clamp(timer / 70f, 0f, 1f);

					RaysOfControlOrb.OrbParticles orbFliesOut = new RaysOfControlOrb.OrbParticles(Vector2.Zero,
						(offset2 * timeIn * 0f),
						Main.rand.NextFloat(MathHelper.TwoPi),
						Color.Red);
					orbFliesOut.scale = new Vector2(4f,1f)* timeIn;
					orbFliesOut.angle = offset2.ToRotation();

					RaysOfControlOrb.partices.Add(orbFliesOut);

					//CataLogo.DrawToRenderTarget();
				}


			}

			if (Projectile.ai[0] == 150 || (Projectile.ai[0]>150 && Projectile.ai[0] <260 && Projectile.ai[0]%10==0))
			{
				bool bigboom = Projectile.ai[0] == 150;

				if (!Main.dedServ && Main.myPlayer == player.whoAmI)
				SGAmod.AddScreenShake(bigboom ? 30 : 5,600, player.MountedCenter);

				foreach (NPC enemy in Main.npc.Where(testby => testby.IsValidEnemy()))
				{
					Vector2 oldpos = Projectile.Center;
					if ((enemy.Hitbox.Center()-Projectile.Center).Length()<6000)
					{
						Projectile.Center = enemy.Hitbox.Center();
						int vardamage = (int)(Projectile.damage * (bigboom ? 1f : 0.15f));
						int damage = (int)((Main.DamageVar(vardamage) * (Projectile.ai[0] == 150 ? 0.5f : 1f)));
						CheckApoco(ref damage, enemy, Projectile, Projectile.ai[0] == 150);
						enemy.StrikeNPC(damage, 0, 1, false);
						Main.player[Projectile.owner].addDPS(damage);
					}
					Projectile.Center = oldpos;
				}

			}


			controlRays = controlRays.Where(testby => testby.timeLeft > 0).ToList();
			boomOfClouds = boomOfClouds.Where(testby => testby.timeLeft > 0).ToList();

			foreach (BeamOfLight ray in controlRays)
			{
				ray.Update();
			}

			foreach (CloudBoom boomOfClouds in boomOfClouds)
			{
				boomOfClouds.Update();
				boomOfClouds.speed.Y -= (Projectile.ai[0] - 120)/60f;
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float alpha = 1f;
			UnifiedRandom random = new UnifiedRandom(Projectile.whoAmI);
			Texture2D statTex = ModContent.Request<Texture2D>("SGAmod/Extra_57b");
			Texture2D beamTex = ModContent.Request<Texture2D>("SGAmod/LightBeam");
			Texture2D glowOrb = Main.itemTexture[ModContent.ItemType<StygianCore>()];// ModContent.GetTexture("SGAmod/GlowOrb");
			Texture2D glowOrb2 = ModContent.Request<Texture2D>("SGAmod/GlowOrb");
			Vector2 offsetbeam = new Vector2(beamTex.Width / 2f, beamTex.Height / 4f);

			Vector2 starHalf = statTex.Size() / 2f;
			Vector2 orbHalf = glowOrb.Size() / 2f;

			float timeLeft = MathHelper.Clamp(Projectile.timeLeft / 30f, 0f, 1f);
			float timeStartUp = MathHelper.SmoothStep(0f,1f,MathHelper.Clamp(Projectile.localAI[0] / 60f, 0f, 1f))* timeLeft;
			float orbAlpha = MathHelper.Clamp((timeStartUp * 1.25f), 0f, 1f) * timeLeft;
			Color orbColor = Color.White;


			foreach (BeamOfLight ray in controlRays)
			{
				float beamtimeleft = ray.timeLeft / (float)ray.timeLeftMax;
				TrailHelper trail = new TrailHelper("FadedBasicEffectPass", Main.extraTexture[21]);
				Color colorz = Color.Aqua;

				trail.coordOffset = new Vector2(0, (beamtimeleft+((Main.GlobalTimeWrappedHourly)*3f))*3f);
				trail.coordMultiplier = new Vector2(1f, 4f);

				trail.strength = 2f* timeStartUp*MathHelper.Clamp(beamtimeleft*2f,0f,1f);// (1f-MathHelper.Clamp(beamtimeleft*4f,0f,1f));
				trail.trailThickness = 128f;
				trail.strengthPow = 1f;
				trail.doFade = false;

				float alphafloat = beamtimeleft;// (float)(Math.Sin(beamtimeleft*MathHelper.Pi)*4f)-2f;

				trail.color = delegate (float percent)
				{
					float beamoutside = 1f - (Math.Abs(beamtimeleft - (1f-percent))*1f);
					float alphafat = alphafloat;
					float alphacol = MathHelper.SmoothStep(0f, 1f, MathHelper.Clamp(Math.Max(beamoutside, 0), 0f, 1f));
					return Color.Red * alphacol*MathHelper.Clamp((float)Math.Sin(percent*MathHelper.Pi)*1f,0f,1f);

				};

				trail.trailThicknessFunction = delegate (float percent)
				{
					float beamoutside = 1f-Math.Abs((beamtimeleft)-percent);
					float alphafat = alphafloat;


					float percentofwidth = MathHelper.Clamp(Math.Max(beamoutside, 0),0f,1f);
					float width = MathHelper.CatmullRom(0f, 0.1f, 0.4f, 6f, percentofwidth);

					float alphacol = 0f+ width * MathHelper.Clamp(32f+ Projectile.localAI[0],0f,80f);
					return (alphacol);
				};
				trail.DrawTrail(ray.positions, Projectile.Center);
			}


				Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


			Effect hallowed = SGAmod.HallowedEffect;
			//for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.TwoPi / 12f)
			//{
				Vector2 orbScale = new Vector2(4f, 4f) * timeStartUp;

			//hallowed.Parameters["alpha"].SetValue(0.750f * orbAlpha);
			//hallowed.CurrentTechnique.Passes["Prism"].Apply();

			//Main.spriteBatch.Draw(glowOrb, projectile.Center+(i.ToRotationVector2()*8f* orbScale.X) - Main.screenPosition, null, orbColor * orbAlpha, i, orbHalf, orbScale, default, 0);

			float scaleUpeffect = 1f + (float)Math.Pow(MathHelper.Max((Projectile.ai[0] - 120) / 30f,0),3f);

			float alphaboom = MathHelper.Clamp(1.05f- ((scaleUpeffect-1f)/20f),0,1f);
			float alphaboom2 = MathHelper.Clamp((Projectile.ai[0]-120f)/60f, 0, 1f);
			float alphaboom4 = MathHelper.Clamp((Projectile.ai[0] - 100f) / 72, 0, 1f);
			float alphaboomfinal = MathHelper.Clamp((Projectile.ai[0] - 240f) / 60f, 0, 1f);
			float alphaboomfinal2 = 1f- alphaboomfinal;


			Vector2 boomCenter = RaysOfControlOrb.orbSurface.Size() / 2f;

			Main.spriteBatch.Draw(glowOrb2, Projectile.Center - Main.screenPosition, null, Color.Wheat * orbAlpha * alphaboom2, 0, glowOrb2.Size() / 2f, orbScale * scaleUpeffect, default, 0);

			Color cloudColor = Color.Lerp(Color.Maroon, Color.Wheat, MathHelper.Clamp((Projectile.ai[0] - 120f) / 300f, 0, 1f))* alphaboomfinal2;

			foreach (CloudBoom cb in boomOfClouds)
			{
				Texture2D cloudTex = ModContent.Request<Texture2D>("SGAmod/NPCs/Hellion/Clouds" + cb.cloudType);
				Vector2 pos = cb.position;
				float angle = cb.angle;

				float timeleft = cb.timeLeft;
				float timeleftperc = cb.timeLeft / (float)cb.timeLeftMax;
				float timeleftpercinvert = 1f - timeleftperc;

				float finalAlpha = MathHelper.Clamp(timeleftperc * 6f, 0f, Math.Min(timeleftpercinvert * 16f, 1f)) * 0.25f* alphaboom4*(1f- alphaboom);

				Vector2 scale = cb.scale;

				Main.spriteBatch.Draw(cloudTex, pos, null, cloudColor * orbAlpha * finalAlpha, angle, cloudTex.Size()/2f, scale, default, 0);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			float alphaboom1b = MathHelper.Clamp(1.15f - ((scaleUpeffect - 1f) / 64f), 0, 1f);

			if (alphaboom<1f)
			Main.spriteBatch.Draw(RaysOfControlOrb.orbSurface, Projectile.Center - Main.screenPosition, null, Color.Black * orbAlpha * alphaboom1b * alphaboomfinal2, 0, boomCenter, orbScale * scaleUpeffect, default, 0);


			Main.spriteBatch.Draw(RaysOfControlOrb.orbSurface, Projectile.Center - Main.screenPosition, null, orbColor * orbAlpha* alphaboom* alphaboomfinal2, 0, boomCenter, orbScale* scaleUpeffect, default, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Main.spriteBatch.Draw(glowOrb2, Projectile.Center - Main.screenPosition, null, orbColor * orbAlpha * alphaboomfinal, 0, glowOrb2.Size() / 2f, orbScale * scaleUpeffect, default, 0);

			//}


			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}
	}


	public class CataNukePlayer : ModPlayer
	{
		private int _charge = 0;
		public int Charge
        {
            get
            {
				int charger = _charge;
				if (!IdgNPC.bossAlive && !UnlimitedPower)
					charger = Math.Min(charger, ChargeMax / 2);
				return charger;
            }
            set
            {
				_charge = value;
            }

        }

		public bool UnlimitedPower => Player.HasItem(ModContent.ItemType<Consumables.Debug10>());

		public int ChargeMax => 100000;
		public float ChargePercent => (float)Charge / (float)ChargeMax;
		public int ChargeSpeed => (int)(((10+Math.Min(Player.lifeRegen / 3, 10)) * MathHelper.Clamp(Player.lifeRegenTime / 400f, 0f, 5f)*(HeldNuke ? 1f : 0.25f))* (UnlimitedPower ? 100f : 1f));

		public bool HasNuke => Player.HasItem(ModContent.ItemType<NuclearOption>());
		public bool HeldNuke => Player.HeldItem.type == ModContent.ItemType<NuclearOption>();


		public override void ResetEffects()
		{
			//nil
		}

		public override void PostUpdate()
		{
			if (HasNuke)
			{
				if (HeldNuke)
				{
					float square = 96f * 96f;
					foreach (Projectile proj in Main.projectile.Where(testby => testby.active && !testby.friendly && testby.hostile && (testby.Center - Player.Center).LengthSquared() < square && testby.SGAProj().grazed == false))
					{
						proj.SGAProj().grazed = true;
						var snd = SoundEngine.PlaySound(SoundID.Item35, (int)proj.Center.X, (int)proj.Center.Y);
						if (snd != null)
						{
							snd.Pitch = -0.75f;
						}
						Charge += 500 + (proj.damage * 15);
					}
				}
				Charge = (int)MathHelper.Clamp(Charge + ChargeSpeed, 0f, ChargeMax);
			}
			else
			{
				Charge = 0;
			}
		}
	}

	public class NuclearOption : Megido, IRadioactiveDebuffText
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nuclear Option");
			Tooltip.SetDefault("'Unleash the full raw, unfiltered, cataclysmic wrath of the British...'\nCharges up by holding this item, based off life regen and by grazing projectiles\nWill charge up very slowly if not actively held\nAt 50% charge or higher, activate to unleash a Nuclear Explosion\nSends out a initial shock wave, afterwards only the fireball does damage\nVaporizes most projectiles, and has more range and damage at higher charge\n" + Idglib.ColorText(Color.Red, "Getting hurt and losing health will halve your current charge")+"\n"+Idglib.ColorText(Color.Red, "Only charges up to half outside of boss fights"));
		}

        public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 250;
			Item.width = 48;
			Item.height = 48;
			Item.useTurn = true;
			Item.rare = ItemRarityID.Cyan;
			Item.value = 500;
			Item.useStyle = 1;
			Item.useAnimation = 50;
			Item.useTime = 50;
			Item.knockBack = 8;
			Item.autoReuse = false;
			Item.noUseGraphic = true;
			Item.consumable = false;
			Item.noMelee = true;
			Item.shootSpeed = 1f;
			Item.maxStack = 1;
			Item.shoot = ModContent.ProjectileType<NuclearOptionProj>();
		}

        public override bool CanUseItem(Player player)
		{
			if (player.GetModPlayer<CataNukePlayer>().ChargePercent>=0.50f)
			{
				return true;
			}
			return false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.lifeRegen = 0;
			player.lifeRegenTime = 0;
			float perc = (player.GetModPlayer<CataNukePlayer>().ChargePercent * player.GetModPlayer<CataNukePlayer>().ChargePercent);
			Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<NuclearOptionProj>(), (int)(damage* perc), knockBack, player.whoAmI);
			proj.ai[1] = player.GetModPlayer<CataNukePlayer>().ChargePercent;
			player.GetModPlayer<CataNukePlayer>().Charge = 0;
			proj.netUpdate = true;
			return false;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Texture2D inner = Mod.Assets.Request<Texture2D>("BoostBar").Value;

			Vector2 slotSize = new Vector2(52f, 52f)* scale;
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = Vector2.Zero;

			slotSize.X /= 1.0f;
			slotSize.Y = -slotSize.Y / 4f;

			Vector2 HPHeight = new Vector2(1f, 1f);

			spriteBatch.Draw(Main.itemTexture[Item.type], drawPos, null, drawColor, Main.GlobalTimeWrappedHourly, Main.itemTexture[Item.type].Size() / 2f, Main.inventoryScale, SpriteEffects.None, 0f);

			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			Vector2 scalerr = Vector2.One * new Vector2(slotSize.X/2f, 1f);
			if (Main.rand.Next(100) < 999)
			{
				spriteBatch.Draw(inner, drawPos - new Vector2(slotSize.X / 2, slotSize.Y), new Rectangle(2, 0, 2, inner.Height), Color.White, 0, textureOrigin, scalerr * HPHeight, SpriteEffects.None, 0f);
				CataNukePlayer cataply = Main.LocalPlayer.GetModPlayer<CataNukePlayer>();
				spriteBatch.Draw(inner, drawPos - new Vector2(slotSize.X / 2, slotSize.Y), new Rectangle(2, 0, 2, inner.Height), Color.Turquoise, 0, textureOrigin, scalerr * new Vector2(cataply.ChargePercent, 1f) * HPHeight, SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(inner, drawPos - new Vector2(0, slotSize.Y), new Rectangle(0, 2, 2, inner.Height), Color.White, 0, textureOrigin, Main.inventoryScale * HPHeight, SpriteEffects.None, 0f);
			spriteBatch.Draw(inner, drawPos - new Vector2(0, slotSize.Y), new Rectangle(0, 0, 2, inner.Height), Color.White, 0, textureOrigin, Main.inventoryScale * HPHeight, SpriteEffects.FlipHorizontally, 0f);


			//spriteBatch.Draw(inner, drawPos - new Vector2(-slotSize.X / 2, slotSize.Y), new Rectangle(inner.Width - 2, 0, 2, inner.Height), Color.White, 0, textureOrigin, Main.inventoryScale * HPHeight, SpriteEffects.None, 0f);

			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Deferred, default, default, default, default, null, Main.UIScaleMatrix);

			return false;
		}

	}

	public class NuclearOptionProj : MorningStarProj
	{

		public List<CloudBoom> raysOfLight = new List<CloudBoom>();

		Vector2 OverallScale => (Vector2.One*3f*Projectile.ai[1])*((float)Math.Pow(Projectile.localAI[0] / 30f, 0.32f));

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.friendly = true;
		}

		public override string Texture
		{
			get { return "Terraria/Misc/MoonExplosion/Explosion"; }
		}

        public override bool Autoload(ref string name)
        {
            SGAmod.PostUpdateEverythingEvent += SGAmod_PostUpdateEverythingEvent;
			return true;
        }

        private void SGAmod_PostUpdateEverythingEvent()
        {
			CataLogo.DrawToRenderTarget();
		}

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nuclear Option Proj");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (!Main.dedServ)
			{
				CataLogo.Load();
				CataLogo.oneUpdate = true;
				//CataLogo.DrawToRenderTarget();
			}

			Projectile.ai[0] += 1;
			Projectile.localAI[0] += 1;
			if (Projectile.localAI[0] == 1)
			{
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/MegidolaonSnd").WithVolume(1f).WithPitchVariance(.15f), Projectile.Center);
				SGAmod.AddScreenShake(64f, 2400, Projectile.Center);
			}
			float lenn = 512 * (Projectile.ai[1]* Projectile.ai[1]) +(Projectile.ai[0]<60 ? Projectile.ai[0]*25 : 0)+(OverallScale.X * 16f);

			if (Projectile.ai[0] % 10 == 0 && Projectile.timeLeft>30)
			{
				foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.hostile && !testby.friendly && (testby.Center - Projectile.Center).Length() < lenn))
				{
					bool canDelete = (proj.ModProjectile != null && ((proj.ModProjectile is INonDestructableProjectile) || (proj.ModProjectile is Dimensions.IMineableAsteriod)));

					if (proj.timeLeft > 3 && !proj.SGAProj().raindown && proj.whoAmI != Projectile.whoAmI && proj.damage>0 && proj.hostile && !proj.friendly && (proj.ModProjectile == null && !canDelete))
					{
						proj.SGAProj().raindown = true;
						proj.timeLeft = 3;

						for (int i = 0; i < 24; i += 1)
						{
							Vector2 position = Main.rand.NextVector2Circular(16f, 16f);
							int num128 = Dust.NewDust(proj.Center + position, 0, 0, DustID.AncientLight, 0, 0, 240, Color.Aqua, 3.25f - (i / 12f));
							Main.dust[num128].noGravity = true;
							Main.dust[num128].alpha = 160;
							Main.dust[num128].color = Color.Lerp(Color.Aqua, Color.Blue, Main.rand.NextFloat() % 1f);
							Main.dust[num128].velocity = (Vector2.Normalize(position) * Main.rand.NextFloat(2f, 5f)) + (i*Vector2.Normalize(proj.Center - Projectile.Center)*0.075f);
						}
					}
				}

				foreach (NPC npc in Main.npc.Where(testby => testby.IsValidEnemy() && (testby.Center - Projectile.Center).Length() < lenn))
				{
					int damage = Main.DamageVar(Projectile.damage);
					CheckApoco(ref damage, npc, Projectile);
					npc.StrikeNPC(damage, 0, 1, false);
					player.addDPS(damage);
					npc.SGANPCs().IrradiatedAmmount = Math.Min(npc.SGANPCs().IrradiatedAmmount + 30, Projectile.damage * 3);
					npc.AddBuff(ModContent.BuffType<Buffs.RadioDebuff>(), 60 * 20);

					if (Projectile.ai[1] >= 1f)
					{
						if (SGAmod.Calamity.Item1)
						{
							if (npc.ModNPC != null && npc.ModNPC.Mod.Name == "CalamityMod")
							{
								npc.life = 1;
								npc.StrikeNPC(666, 1337, 1, true);
								if (npc.active)
								{
									npc.active = false;
									npc.ModNPC.NPCLoot();
								}
							}
						}
					}

					for (int i = 0; i < 16; i += 1)
					{
						Vector2 position = Main.rand.NextVector2Circular(16f, 16f);
						int num128 = Dust.NewDust(npc.Center + position, 0, 0, DustID.AncientLight, 0, 0, 240, Color.Aqua, 1.50f - (i / 24f));
						Main.dust[num128].noGravity = true;
						Main.dust[num128].alpha = 130;
						Main.dust[num128].color = Color.Lerp(Color.Aqua, Color.Blue, Main.rand.NextFloat() % 1f);
						Main.dust[num128].velocity = (Vector2.Normalize(position) * Main.rand.NextFloat(6f, 12f)) + Vector2.Normalize(npc.Center - Projectile.Center)*20f;
					}
				}
			}



			if (Projectile.timeLeft > 30)
            {
				if (SGAmod.ScreenShake<10)
				SGAmod.AddScreenShake(5f, 720+(Projectile.timeLeft*4), Projectile.Center);
			}

			float scaleUpeffect = 1f;

			float explodScale = 16f / MathHelper.Clamp(1f + (Projectile.timeLeft / 4f), 0.001f, 100f);
			float cataScale = 8f / MathHelper.Clamp(1f + (Projectile.timeLeft / 4f), 0.001f, 100f);

			for (int i = 0; i < 2; i += 1)
			{
				Vector2 velo = new Vector2(Main.rand.NextFloat(-1f, 1f))*0.05f;
				CloudBoom boomer = new CloudBoom(new Vector2(Main.rand.NextFloat(MathHelper.TwoPi),0), velo, Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.Next(1, 7));
				boomer.scale = (Vector2.One * (1f * scaleUpeffect) * new Vector2(Main.rand.NextFloat(0.50f, 0.75f), Main.rand.NextFloat(0.75f, 1f))) * 0.50f;

				boomer.angle = Main.rand.NextFloat(MathHelper.TwoPi);

				raysOfLight.Add(boomer);
			}
			foreach (CloudBoom cb in raysOfLight.Where(testby => testby.timeLeft > 0))
			{
				cb.angle += cb.speed.X;
				cb.position -= cb.speed;
				cb.Update();
			}



			for (int i = 0; i < 32; i += 1)
			{
				Vector2 velo = Vector2.UnitX.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(12f, 18f)* (1f+explodScale+ cataScale) *(OverallScale*0.20f);
				CloudBoom boomer = new CloudBoom(Projectile.Center + (Vector2.Normalize(velo)* explodScale), velo * (0.45f + (scaleUpeffect / 3f)), Main.rand.NextFloat(MathHelper.TwoPi), Main.rand.Next(1, 7));
				boomer.scale = (Vector2.One * (1f * scaleUpeffect) * new Vector2(Main.rand.NextFloat(0.50f, 0.75f), Main.rand.NextFloat(0.75f, 1f)))*0.50f;

				boomOfClouds.Add(boomer);
			}
			foreach (CloudBoom cb in boomOfClouds.Where(testby => testby.timeLeft > 0))
			{
				cb.Update();
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float alpha = MathHelper.Clamp(Projectile.timeLeft/60f,0f,1f);
			float alpha2 = MathHelper.Clamp(Projectile.timeLeft / 20f, 0f, 1f);
			float alpha3 = 1f-MathHelper.Clamp((Projectile.timeLeft-20f) / 90f, 0f, 1f);
			float alpha4 = 1f - MathHelper.Clamp(Projectile.localAI[0]/60f, 0f, 1f);
			float alpha5 = MathHelper.Clamp((Projectile.timeLeft - 20f) / 20f, 0f, 1f);

			Texture2D explosionTex = Main.projectileTexture[Projectile.type];
			Texture2D lightBeamTex = ModContent.Request<Texture2D>("SGAmod/LightBeam");
			Texture2D glowOrbTex = ModContent.Request<Texture2D>("SGAmod/GlowOrb");

			Vector2 exploorig = new Vector2(explosionTex.Width, explosionTex.Height / 7) / 2f;
			Vector2 lightorig = new Vector2(lightBeamTex.Width, lightBeamTex.Height / 4) / 2f;
			Vector2 orgCenter = glowOrbTex.Size() / 2f;

			float explodScale = 16f / MathHelper.Clamp(1f + (Projectile.timeLeft / 4f), 0.001f, 100f);
			float explodScale2 = 4f / MathHelper.Clamp(1f + (Projectile.timeLeft / 16f), 0.001f, 100f);
			float cataScale = 64f / MathHelper.Clamp(1f + (Projectile.timeLeft / 4f), 0.001f, 100f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			if (alpha4 > 0f)
				Main.spriteBatch.Draw(glowOrbTex, Projectile.Center - Main.screenPosition, null, Color.Aqua * alpha4 * 1f, 0, orgCenter, OverallScale * (20f * (1f - alpha4)), default, 0);


			Main.spriteBatch.Draw(glowOrbTex, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Turquoise, Color.White, MathHelper.Clamp(explodScale * 2f, 0f, 1f)) * alpha * 0.50f, 0, orgCenter, OverallScale * 3f, default, 0);

			Color boomColor = Color.Aqua;

			Main.spriteBatch.Draw(glowOrbTex, Projectile.Center - Main.screenPosition, null, boomColor * alpha2, 0, orgCenter, OverallScale* explodScale, default, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


			foreach (CloudBoom cb in raysOfLight)
			{
				float timePercent = (cb.timeLeft / (float)cb.timeLeftMax);
				float timePercentBack = 1f - (cb.timeLeft / (float)cb.timeLeftMax);
				float cbAlpha = MathHelper.Clamp(timePercent * 4f, 0f, 1f);
				Color color = Color.DarkTurquoise;
				Vector2 explosionSize = (Vector2.One * 0.20f) + ((Vector2.One * 0.80f) * timePercent) * cb.scale;

				Main.spriteBatch.Draw(lightBeamTex, Projectile.Center - Main.screenPosition, null, color * cbAlpha * alpha * 0.50f, -MathHelper.PiOver2+cb.angle+(cb.position.X), lightorig, explosionSize*new Vector2(0.5f,1.5f) * OverallScale, default, 0);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			foreach (CloudBoom cb in boomOfClouds)
			{
				float timePercent = (cb.timeLeft / (float)cb.timeLeftMax);
				float timePercentBack = 1f-(cb.timeLeft / (float)cb.timeLeftMax);
				float cbAlpha = MathHelper.Clamp(timePercent * 4f, 0f, 1f);
				Color color = Color.White;
				Vector2 explosionSize = (Vector2.One * 0.20f)+((Vector2.One*0.80f)*timePercent)*cb.scale;

				Rectangle rect = new Rectangle(0, (explosionTex.Height / 7)* (int)(timePercentBack * 7), explosionTex.Width, explosionTex.Height / 7);
				Main.spriteBatch.Draw(explosionTex, cb.position - Main.screenPosition, rect, color * cbAlpha*alpha*0.25f, cb.angle, exploorig / 2f, explosionSize* OverallScale, default, 0);
			}

			CataLogo.Draw(Projectile.Center-Main.screenPosition,alpha* alpha5, new Vector2(3f,3f)*OverallScale* cataScale);



			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Main.spriteBatch.Draw(glowOrbTex, Projectile.Center - Main.screenPosition, null, Color.Turquoise * alpha5 * 1f, 0, orgCenter, OverallScale * cataScale * 0.025f, default, 0);

			Main.spriteBatch.Draw(glowOrbTex, Projectile.Center - Main.screenPosition, null, Color.White * alpha2* alpha3, 0, orgCenter, OverallScale * explodScale, default, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


			return false;
		}

	}

	public class TheJoker : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Joker");
			Tooltip.SetDefault("'The jokes on you!'\nIs Consumed when successfully using Almighty cards, and restores 20 HP per stack");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.Mod == "Terraria" && line.Name == "ItemName")
				{
					line.OverrideColor = Color.Lerp(Color.Red, Color.Black, 0.5f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 6f));
				}
			}
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 14;
			Item.height = 14;
			Item.value = 0;
			Item.rare = ItemRarityID.Red;
		}
	}

	public class RaysOfControlOrb
	{
		public class OrbParticles
		{
			public Vector2 position;
			public Vector2 speed;
			public float angle;
			public Vector2 scale = new Vector2(1f, 1f);
			public Color color;

			public int timeAdd = 0;
			public int timeLeft = 20;
			public int timeLeftMax = 20;
			public OrbParticles(Vector2 position, Vector2 speed, float angle,Color color)
			{
				this.position = position;
				this.speed = speed;
				this.angle = angle;
				this.color = color;
			}
			public void Update()
			{
				timeLeft -= 1;
				position += speed;
				timeAdd++;
			}

		}

		public static RenderTarget2D orbSurface;

		public static bool hasLoaded = false;
		public static bool oneUpdate = false;
		public static float progress = 0f;

		public static List<OrbParticles> partices = new List<OrbParticles>();
		public static int timeLeft = 0;

		public static void UpdateAll()
        {
			if (partices.Count>0 || timeLeft > 0)
            {
				partices = partices.Where(testby => testby.timeLeft > 0).ToList();
				foreach(OrbParticles particle in partices)
                {
					particle.Update();
				}
			}
		}


		public static void Load()
		{
			if (hasLoaded)
				return;

			orbSurface = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.blackTileTexture.Width * 32, Main.blackTileTexture.Height * 32, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
			hasLoaded = true;
		}

		public static void Unload()
		{
			if (!hasLoaded)
				return;

			if (!orbSurface.IsDisposed)
				orbSurface.Dispose();
			/*if (!cataEffect.IsDisposed)
				cataEffect.Dispose();
			if (!radialEffect.IsDisposed)
				radialEffect.Dispose();*/
		}

		public static void DrawToRenderTarget()
		{
			if (Main.dedServ || !hasLoaded || !oneUpdate)
				return;

			oneUpdate = false;

			if (orbSurface == null || orbSurface.IsDisposed)
				return;

			Texture2D glowOrb = Main.itemTexture[ModContent.ItemType<StygianCore>()];// ModContent.GetTexture("SGAmod/GlowOrb");

			RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();

			Main.graphics.GraphicsDevice.SetRenderTarget(orbSurface);
			Main.graphics.GraphicsDevice.Clear(Color.Transparent);

			//Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

			Effect effect = SGAmod.TextureBlendEffect;

			effect.Parameters["Texture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("TiledPerlin").Value);
			effect.Parameters["noiseTexture"].SetValue(glowOrb);// SGAmod.Instance.GetTexture("Extra_49c"));
			effect.Parameters["coordMultiplier"].SetValue(new Vector2(1f,1f));
			effect.Parameters["coordOffset"].SetValue(new Vector2(0f, 0f));
			effect.Parameters["noiseMultiplier"].SetValue(new Vector2(1f, 1f));
			effect.Parameters["noiseOffset"].SetValue(new Vector2(0f, 0f));
			effect.Parameters["noiseProgress"].SetValue(Main.GlobalTimeWrappedHourly);
			effect.Parameters["textureProgress"].SetValue(Main.GlobalTimeWrappedHourly*2f);
			effect.Parameters["noiseBlendPercent"].SetValue(1f);
			effect.Parameters["strength"].SetValue(0.25f);
			effect.Parameters["alphaChannel"].SetValue(false);

			foreach (OrbParticles particle in partices)
			{
				float timeLeft = particle.timeLeft / (float)particle.timeLeftMax;
				float strength = MathHelper.Clamp(timeLeft * 3f, 0f, Math.Min(particle.timeAdd * 3f, 1f));

				effect.Parameters["colorTo"].SetValue(particle.color.ToVector4());
				effect.Parameters["colorFrom"].SetValue(Color.Black.ToVector4());
				effect.Parameters["strength"].SetValue(strength);

				effect.CurrentTechnique.Passes["TextureBlend"].Apply();
				Main.spriteBatch.Draw(glowOrb, orbSurface.Size()/2f+particle.position, null, Color.White, particle.angle, glowOrb.Size() / 2f, particle.scale, default, 0);
			}

			Main.spriteBatch.End();

			Main.graphics.GraphicsDevice.SetRenderTargets(binds);

			progress = 0f;
		}

	}

	public class CataLogo
	{
		public static RenderTarget2D cataSurface;

		public static Effect cataEffect;
		public static Effect radialEffect;
		public static bool hasLoaded = false;
		public static bool oneUpdate = false;


		public static void Load()
		{
			RaysOfControlOrb.Load();

			if (hasLoaded)
				return;

			cataEffect = SGAmod.CataEffect;
			radialEffect = SGAmod.RadialEffect;
			cataSurface = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.blackTileTexture.Width * 32, Main.blackTileTexture.Height * 32, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
			hasLoaded = true;
		}

		public static void Unload()
		{
			RaysOfControlOrb.Unload();

			if (!hasLoaded)
				return;

			if (!cataSurface.IsDisposed)
				cataSurface.Dispose();
			/*if (!cataEffect.IsDisposed)
				cataEffect.Dispose();
			if (!radialEffect.IsDisposed)
				radialEffect.Dispose();*/
		}

		public static void DrawToRenderTarget()
		{
			RaysOfControlOrb.DrawToRenderTarget();
			if (Main.dedServ || !hasLoaded || !oneUpdate)
				return;

			oneUpdate = false;

			if (cataSurface == null || cataSurface.IsDisposed)
				return;

			BlendState negaBlending = new BlendState
			{

				ColorSourceBlend = Blend.Zero,
				ColorDestinationBlend = Blend.InverseSourceColor,

				AlphaSourceBlend = Blend.Zero,
				AlphaDestinationBlend = Blend.InverseSourceColor

			};

			RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();

			Main.graphics.GraphicsDevice.SetRenderTarget(cataSurface);
			Main.graphics.GraphicsDevice.Clear(Color.Transparent);

			//Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

			float edgesize = 0.40f;// + (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.10f);
			float ballsize = 0.05f;// + (float)(Math.Sin(Main.GlobalTimeWrappedHourly) * 0.05f);
			float ballgapsize = 0.05f;// + (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 1.2f) * 0.02f);

			Effect RadialEffect = radialEffect;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

			for (int i = 0; i < 3; i += 1)
			{
				for (float f = -1f; f < 2; f += 2)
				{
					RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Fire").Value);
					RadialEffect.Parameters["alpha"].SetValue(0.50f);
					RadialEffect.Parameters["texOffset"].SetValue(new Vector2(f * Main.GlobalTimeWrappedHourly * 0.25f, -Main.GlobalTimeWrappedHourly * 0.575f));
					RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(3f, 1f + i));
					RadialEffect.Parameters["ringScale"].SetValue(0.36f);
					RadialEffect.Parameters["ringOffset"].SetValue(0.16f);
					RadialEffect.Parameters["ringColor"].SetValue(Color.Turquoise.ToVector3());
					RadialEffect.Parameters["tunnel"].SetValue(false);

					RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

					Main.spriteBatch.Draw(Main.blackTileTexture, Main.blackTileTexture.Size() * 16f, null, Color.White, 0, Main.blackTileTexture.Size() * 0.5f, 96f, SpriteEffects.None, 0f);
				}
			}

			RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Fire").Value);
			RadialEffect.Parameters["alpha"].SetValue(5f);
			RadialEffect.Parameters["texOffset"].SetValue(new Vector2(0, -Main.GlobalTimeWrappedHourly * 0.2575f));
			RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(0.5f, 1f));
			RadialEffect.Parameters["ringScale"].SetValue(0.1f);
			RadialEffect.Parameters["ringOffset"].SetValue((ballsize + ballgapsize) * (32f / 96f) * 2.5f);
			RadialEffect.Parameters["ringColor"].SetValue(Color.Turquoise.ToVector3());
			RadialEffect.Parameters["tunnel"].SetValue(false);

			RadialEffect.CurrentTechnique.Passes["RadialAlpha"].Apply();

			Main.spriteBatch.Draw(Main.blackTileTexture, Main.blackTileTexture.Size() * 16f, null, Color.White, 0, Main.blackTileTexture.Size() * 0.5f, 96f, SpriteEffects.None, 0f);

			Main.spriteBatch.End();

			Main.spriteBatch.Begin(SpriteSortMode.Immediate, negaBlending, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

			cataEffect.Parameters["angleAdd"].SetValue(Main.GlobalTimeWrappedHourly * 1f);
			cataEffect.Parameters["edges"].SetValue(3);
			cataEffect.Parameters["ballSize"].SetValue(ballsize);
			cataEffect.Parameters["edgeSize"].SetValue(edgesize);
			cataEffect.Parameters["ballEdgeGap"].SetValue(ballgapsize);

			cataEffect.CurrentTechnique.Passes["CataLogoInverse"].Apply();

			Main.spriteBatch.Draw(Main.blackTileTexture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 32f, SpriteEffects.None, 0f);

			Main.spriteBatch.End();

			Main.graphics.GraphicsDevice.SetRenderTargets(binds);
		}

		public static void Draw(Vector2 where, float alpha, Vector2 scale)
		{

			Effect RadialEffect = radialEffect;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Fire").Value);
			RadialEffect.Parameters["alpha"].SetValue(4f * alpha);
			RadialEffect.Parameters["texOffset"].SetValue(new Vector2(0, Main.GlobalTimeWrappedHourly * 0.575f));
			RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(3f, 0.75f));
			RadialEffect.Parameters["ringScale"].SetValue(0.20f);
			RadialEffect.Parameters["ringOffset"].SetValue(0.14f);
			RadialEffect.Parameters["ringColor"].SetValue(Color.Turquoise.ToVector3());
			RadialEffect.Parameters["tunnel"].SetValue(true);

			RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

			Main.spriteBatch.Draw(Main.blackTileTexture, where, null, Color.White, 0, Main.blackTileTexture.Size() * 0.5f, scale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


			Main.spriteBatch.Draw(cataSurface, where, null, Color.White * alpha, 0, (Vector2.One * cataSurface.Size()) / 2f, scale / 18f, SpriteEffects.None, 0f);


			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


		}
	}
}
