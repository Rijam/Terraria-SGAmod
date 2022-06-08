using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Idglibrary;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{

	public class SoldierRocketLauncher : ModItem,IDedicatedItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soldier's Rocket Launcher");
			Tooltip.SetDefault("Becomes stronger with higher tier TF2 emblems you equip\nBlasts from the rockets will push players away with sizable force");
			SGAmod.UsesClips.Add(SGAmod.Instance.Find<ModItem>("SoldierRocketLauncher").Type, 6);
		}

		public string DedicatedItem()
		{
			return "To Rick May's passing, and the legecy he left behind in Team Fortress 2";
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			SGAPlayer sgaply = Main.LocalPlayer.SGAPly();
			if (sgaply.tf2emblemLevel > 0)
			tooltips.Add(new TooltipLine(Mod, "SoldierLine", "Tier 1: Damage Increased by 20%, Reload and firing speed are faster per level"));
			if (sgaply.tf2emblemLevel > 1)
				tooltips.Add(new TooltipLine(Mod, "SoldierLine", "Tier 2: Damage Increased by 50%; rockets explode larger and move faster per level"));
			if (sgaply.tf2emblemLevel > 2)
				tooltips.Add(new TooltipLine(Mod, "SoldierLine", "Tier 3: Damage Increased by 100%; Rockets slow targets, gain a firing speed boost when you rocket jump"));
			if (sgaply.tf2emblemLevel > 3)
				tooltips.Add(new TooltipLine(Mod, "SoldierLine", "Tier 4: Damage Increased by 300%; Rocket jumping restores WingTime, slow is stronger, direct hits against slowed enemies will always crit"));
			Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 4) % 1f, 0.4f, 0.6f);
			tooltips.Add(new TooltipLine(Mod, "RIP Rick May", Idglib.ColorText(c, "'He didn't fly into heaven, he rocket jumped into heaven'")));
			c = Main.hslToRgb((float)((Main.GlobalTimeWrappedHourly+5.77163f) / 4) % 1f, 0.35f, 0.65f);
			tooltips.Add(new TooltipLine(Mod, "RIP Rick May", Idglib.ColorText(c, "RIP Rick May: 1940-2020")));
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			SGAPlayer sgaply = player.SGAPly();
			if (sgaply.tf2emblemLevel > 0)
				mult += 0.20f;
			if (sgaply.tf2emblemLevel > 1)
				mult += 0.50f;
			if (sgaply.tf2emblemLevel > 2)
				mult += 1.00f;
			if (sgaply.tf2emblemLevel > 3)
				mult += 3.00f;
		}
		public override void SetDefaults()
		{
			var itemsnd=Item.UseSound;
			Item.CloneDefaults(ItemID.RocketLauncher);
			Item.UseSound = itemsnd;
			Item.damage = 35;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.knockBack = 6;
			Item.crit = 10;
			Item.value = 250000;
			Item.DamageType = DamageClass.Ranged;
			Item.rare = 8;
			Item.shootSpeed = 7f;
			Item.shoot= Mod.Find<ModProjectile>("SoldierRocketLauncherProj").Type;
			Item.noMelee = true;
			Item.useAmmo = AmmoID.Rocket;
			Item.expert = true;
		}
		public override float UseTimeMultiplier(Player player)
		{
			SGAPlayer sgaply = player.SGAPly();
			return (sgaply.soldierboost > 0 ? 2.5f : 1f) + ((float)sgaply.tf2emblemLevel * 0.25f);
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -6);
		}

		public override void HoldItem(Player player)
		{
			SGAPlayer sply = player.SGAPly();
			if (sply.timer % (50- (int)(((float)sply.previoustf2emblemLevel*6)*sply.RevolverSpeed)) == 0 && sply.timer > (sply.previoustf2emblemLevel > 3 ? 80 : 90) - (sply.previoustf2emblemLevel * 12) && sply.ammoLeftInClip < sply.ammoLeftInClipMax)
			{
				sply.ammoLeftInClip += 1;
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/rocket_reload").WithVolume(.9f).WithPitchVariance(.15f), player.Center);
			}
		}

		public override bool CanUseItem(Player player)
		{
			SGAPlayer sply = player.SGAPly();
			return sply.ConsumeAmmoClip(false);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.SGAPly().timer = 1;
			type = Mod.Find<ModProjectile>("SoldierRocketLauncherProj").Type;

			SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/rocket_shoot").WithVolume(.4f).WithPitchVariance(.15f), player.Center);

			position = player.Center;

			player.SGAPly().ConsumeAmmoClip();

			knockBack = player.SGAPly().tf2emblemLevel;
			//position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			int theproj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, knockBack);
			return false;

		}


	}

	public class SoldierRocketLauncherProj : ModProjectile
	{

		double keepspeed = 0.0;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soldier's Rocket");
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = 3;
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
			AIType = -1;
			Projectile.aiStyle = -1;
			Projectile.extraUpdates = 2;
		}

		public override string Texture
		{
			get { return "SGAmod/Projectiles/SoldierRocketLauncherProj"; }
		}

		bool hitonce = false;

		public override bool PreKill(int timeLeft)
		{
			float size = Projectile.ai[1];

			for (int i = 0; i < Main.maxPlayers; i += 1)
			{
				Player pp = Main.player[i];
				float dist = (pp.Center - Projectile.Center).Length();
				float dist2 = 120 + (int)Math.Max(size - 1, 0) * 30;
				if (pp.active && dist < dist2)
				{
					Vector2 norm = (pp.Center - Projectile.Center); norm.Normalize();
					pp.velocity += (norm) * (1f - (dist / dist2)) * (24f + (Projectile.ai[1] * 2f));

					if (Projectile.ai[1] > 3)
					pp.wingTime = MathHelper.Clamp(pp.wingTime+((1f - (dist / dist2)) * (25f)),0, pp.wingTimeMax);
					if (Projectile.ai[1] > 2)
					{
						pp.SGAPly().soldierboost = Math.Max(pp.SGAPly().soldierboost, (int)((Projectile.ai[1] > 3 ? 150 : 80)* (1f - (dist / dist2))));

					}
				}
			}

			for (int i = 0; i < 125; i += 1)
			{
				float randomfloat = Main.rand.NextFloat(1f, 4f+ Projectile.ai[1]);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();

				int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 32, Projectile.Center.Y - 32), 64, 64, DustID.Fire);
				Main.dust[dust].scale = 2.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (randomcircle * randomfloat);
			}

			float perc = 0.25f;
			if (Projectile.ai[1]>3)
				perc = 0.50f;

			int theproj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("Explosion").Type, (int)((float)Projectile.damage * perc), Projectile.knockBack, Projectile.owner, 0f, Projectile.ai[1]);
			Main.projectile[theproj].DamageType = DamageClass.Ranged;
			Main.projectile[theproj].usesLocalNPCImmunity = true;
			Main.projectile[theproj].localNPCHitCooldown = -1;
			Main.projectile[theproj].width = 120 + (int)Math.Max(size - 1, 0) * 30;
			Main.projectile[theproj].height = 120 + (int)Math.Max(size - 1, 0) * 30;
			Main.projectile[theproj].Center = Projectile.Center;

			Projectile.velocity = default(Vector2);
			Projectile.type = size>2 ? ProjectileID.GrenadeIII : size > 0 ? ProjectileID.GrenadeI : ProjectileID.Grenade;
			return true;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Projectile.ai[1] > 3 && target.HasBuff(Mod.Find<ModBuff>("DankSlow").Type))
			{
				crit = true;
			}
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!hitonce)
			{
				hitonce = true;

				if (Projectile.ai[1] > 2)
				{
					Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("RocketShockBoom").Type, 0, Projectile.knockBack, Projectile.owner, 0f, 0f);
					target.AddBuff(Mod.Find<ModBuff>("DankSlow").Type, 60 * (Projectile.ai[1] > 3 ? 7 : 4));
					target.velocity /= (Projectile.ai[1] > 3 ? 20f : 5f);
				}

				Projectile.timeLeft = 1;

			}
			//projectile.Center -= new Vector2(48,48);
		}

		public override void AI()
		{
			Projectile.rotation = ((float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f)-MathHelper.ToRadians(-90f);

			if (Projectile.ai[0] == 0)
			{
				Vector2 velocs = Projectile.velocity;
				velocs.Normalize();
				if (Projectile.ai[1] > 1)
					Projectile.velocity += velocs * ((Projectile.ai[1]-1f)/2f);
			}

			Projectile.ai[0] = Projectile.ai[0] + 1;

			if (Main.rand.Next(0, 3) == 0)
			{
				for (float i = 0; i < 2.5; i += 0.75f)
				{
					int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Main.rand.Next(0, 100) < 15 ? DustID.Fire : DustID.Smoke);
					Main.dust[dust].scale = 1.25f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = -Projectile.velocity * (float)(Main.rand.Next(20, 50 + (int)(i * 40f)) * 0.01f) / 2f;
				}
			}
		}


	}

	public class RocketShockBoom : ModProjectile,IDrawAdditive
	{
		float ranspin = 0;
		float ranspin2 = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soldier Blast");
		}

		public void getstuff()
		{

			if (ranspin2 == 0)
			{
				ranspin2 = Main.rand.NextFloat(-0.2f, 0.2f);
			}
			else
			{
				ranspin += ranspin2;

			}
		}

		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 20;
			Projectile.penetrate = -1;
			Projectile.damage = 0;
		}

		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Projectiles/BoulderBlast"); }
		}

		public override bool CanDamage()
		{
			return false;
		}

		public void DrawAdditive(SpriteBatch spriteBatch)
        {
			Texture2D tex = SGAmod.ExtraTextures[119];

			float timeleft = ((float)Projectile.timeLeft / 20f);
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition));
			Color color = Color.White * MathHelper.Clamp(MathHelper.SmoothStep(0f,2f, timeleft),0f,1f);
			float scale = MathHelper.SmoothStep(6f + (Projectile.ai[1] * 12f),0, timeleft);
			spriteBatch.Draw(tex, drawPos, null, color, ranspin, drawOrigin, scale*1f, SpriteEffects.None, 0f);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			getstuff();
			Texture2D tex = SGAmod.ExtraTextures[96];

			float timeleft = ((float)Projectile.timeLeft / 20f);
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition));
			Color color = Color.White * timeleft;
			spriteBatch.Draw(tex, drawPos, null, color, ranspin, drawOrigin, (1f - timeleft) * (7f+(Projectile.ai[1]*3f)), SpriteEffects.None, 0f);
			return false;
		}


		public override void AI()
		{
			Projectile.localAI[0] += 1f;

			if (Projectile.ai[0] < 1)
			{
				Projectile.ai[0] = 1;
			}

			return;
		}
	}

	public class PrismalLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Launcher");
			Tooltip.SetDefault("Launches a trio of rockets that may inflict a myriad of debuffs\n'Something something rocket launcher upgrade'");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.RocketLauncher);
			Item.damage = 80;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.knockBack = 6;
			Item.value = 500000;
			Item.DamageType = DamageClass.Ranged;
			Item.rare = 9;
			Item.shootSpeed = 14f;
			Item.noMelee = true;
			Item.useAmmo = AmmoID.Rocket;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/PrismalLauncher_Glow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -8;
			}
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, -0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float speed = 4f;
			float rotation = MathHelper.ToRadians(4);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;

			for (int i = 0; i < 7; i += 3)
			{

				Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * (speed + ((float)i))).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				speedX = perturbedSpeed.X;
				speedY = perturbedSpeed.Y;

				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].timeLeft = 600;
				Main.projectile[proj].knockBack = Item.knockBack;

				if (Main.rand.Next(0, 100) < 20)
					IdgProjectile.AddOnHitBuff(proj, Mod.Find<ModBuff>("ThermalBlaze").Type, 60 * 10);
				if (Main.rand.Next(0, 100) < 20)
					IdgProjectile.AddOnHitBuff(proj, BuffID.DryadsWardDebuff, 60 * 10);
				if (Main.rand.Next(0, 100) < 20)
					IdgProjectile.AddOnHitBuff(proj, BuffID.ShadowFlame, 60 * 10);
				if (Main.rand.Next(0, 100) < 20)
					IdgProjectile.AddOnHitBuff(proj, BuffID.Venom, 60 * 10);

			}

			return false;

		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.RocketLauncher, 1).AddIngredient(ItemID.GrenadeLauncher, 1).AddIngredient(mod.ItemType("PrismalBar"), 12).AddTile(mod.TileType("PrismalStation")).Register();
		}

	}

	public class RadioactiveSnowballCannon : ModItem, IRadioactiveItem, IRadioactiveDebuffText
	{
		public int RadioactiveHeld() => 2;
		public int RadioactiveInventory() => 1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Radioactive Snowball Cannon");
			Tooltip.SetDefault("'Sure Brain, sure...\nEnriches normal snowballs with radioactive isotopes");// + "\n" + Idglib.ColorText(Color.Red, "You suffer Radiation 2 while holding this") + "\n" + Idglib.ColorText(Color.Red, "Radiation 1 if only in inventory"));
		}

		public override void SetDefaults()
		{
			var snd = Item.UseSound;
			Item.CloneDefaults(ItemID.SnowballCannon);
			Item.damage = 25;
			Item.UseSound = snd;
			Item.width = 48;
			Item.height = 48;
			Item.knockBack = 6;
			Item.value = 100000;
			Item.DamageType = DamageClass.Ranged;
			Item.rare = ItemRarityID.Yellow;
			Item.shootSpeed += 1f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -6);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 32f;
			player.SGAPly().timer = 1;
			if (type == ProjectileID.SnowBallFriendly)
				type = Mod.Find<ModProjectile>("UraniumSnowballsProg").Type;

			SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_GoblinBomberThrow, (int)position.X, (int)position.Y);
			if (sound != null)
				sound.Pitch -= 0.525f;

			return true;

		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.SnowballCannon, 1).AddIngredient(ModContent.ItemType<UraniumSnowballs>(), 250).AddTile(TileID.LunarCraftingStation).Register();
		}

	}

}