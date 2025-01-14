﻿using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Projectiles;

using SGAmod.Buffs;
using SGAmod.Effects;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Items.Weapons.Technical;
using Terraria.ModLoader.IO;
using SGAmod.Items;

namespace SGAmod.Items.Weapons.Technical
{
	public class AssaultRifle : SeriousSamWeapon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 0.1f;
		int firemode = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tactical SMG Rifle");
			Tooltip.SetDefault("Adjustable Machine Gun!\nVery Fast! But no ammo saving chance and causes very bad recoil if held down for too long.\nRight click to toggle fire modes");
		}

		public override void SetDefaults()
		{
			item.damage = 25;
			item.ranged = true;
			item.width = 42;
			item.height = 16;
			item.useTime = 3;
			item.useAnimation = 3;
			item.useStyle = 5;
			item.reuseDelay = 0;
			item.noMelee = true;
			item.knockBack = 1;
			item.value = 750000;
			item.rare = 8;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 40f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(firemode);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			firemode = reader.ReadInt32();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.itemTime > 0 && player.altFunctionUse == 2)
				return false;
			if (player.altFunctionUse == 2)
			{
				string[] things = { "Fully Automatic", "Burst"};
				item.reuseDelay = 15;
				player.itemTime = 20;
				firemode += 1;
				firemode %= 2;
				Main.PlaySound(40, player.Center);
				if (Main.myPlayer == player.whoAmI)
				Main.NewText("Toggled: " + things[firemode] + " mode");
			}
			else
			{
				item.reuseDelay = 0;
				item.useTime = 3;
				item.useAnimation = 3;
				if (firemode == 1)
				{
					item.useTime = 2;
					item.useAnimation = 12;
					item.reuseDelay = 25;
				}

			}
			return (player.altFunctionUse!=2);
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 2);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			if (player.GetModPlayer<SGAPlayer>().recoil<75f)
			player.GetModPlayer<SGAPlayer>().recoil += firemode==1 ? 0.4f : 0.75f;

			Main.PlaySound(SoundID.Item41, player.Center);

			float speed = 1.5f;
			float numberProjectiles = 2;
			float rotation = MathHelper.ToRadians(1+ player.GetModPlayer<SGAPlayer>().recoil);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 20f;

			Vector2 perturbedSpeed2 = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = perturbedSpeed2.RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(-0.1f, 0.1f, i/ (numberProjectiles-1)))); // Watch out for dividing by 0 if there is only 1 projectile.

				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].knockBack = item.knockBack;
				player.itemRotation = Main.projectile[proj].velocity.ToRotation();
				if (player.direction < 0)
					player.itemRotation += (float)Math.PI;
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IllegalGunParts, 1);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 8);
			recipe.AddIngredient(ItemID.ChainGun, 1);
			recipe.AddIngredient(ItemID.ClockworkAssaultRifle, 1);
			recipe.AddIngredient(ItemID.Gatligator, 1);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 15);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class OnyxTacticalShotgun : SeriousSamWeapon,ITechItem
	{
		public float ElectricChargeScalingPerUse() => 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Onyx Tactical Shotgun");
			Tooltip.SetDefault("Fires a Spread of Bullets and Onyx Rockets");
		}

		public override void SetDefaults()
		{
			item.damage = 27;
			item.ranged = true;
			item.width = 56;
			item.height = 28;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 5;
			item.value = 500000;
			item.rare = 7;
			item.UseSound = SoundID.Item38;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 16f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -4);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.OnyxBlaster, 1);
			recipe.AddIngredient(ItemID.TacticalShotgun, 1);
			recipe.AddIngredient(null, "SharkTooth", 50);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 10);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 normz = new Vector2(speedX, speedY);normz.Normalize();
			position += normz * 24f;

			int numberProjectiles = 9 + Main.rand.Next(2);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale;
				int prog = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, (int)(damage*0.75), knockBack, player.whoAmI);
				IdgProjectile.Sync(prog);
			}
			numberProjectiles = 3 + Main.rand.Next(1);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
				float scale = 1f - (Main.rand.NextFloat() * .6f);
				perturbedSpeed = perturbedSpeed * scale;
				int prog = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.BlackBolt, (int)(damage*2.0), knockBack, player.whoAmI);
				IdgProjectile.Sync(prog);

			}
			return false;
		}
	}

	public class CircuitBreakerBlade : SeriousSamWeapon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 0.05f;
		public static NPC FindClosestTarget(Player ply,Vector2 loc, Vector2 size, bool block = true, bool friendlycheck = true, bool chasecheck = false)
		{
			List<Point> hitenemies = new List<Point>();
			foreach (NPC hitenemy in Main.npc.Where(testby => testby.immune[0] > 0))
				hitenemies.Add(new Point(hitenemy.whoAmI, 99999));

			List<NPC> closestnpcs = SGAUtils.ClosestEnemies(loc, 400f, AddedWeight: hitenemies);
			return closestnpcs?[0];//Closest

		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Circuit Breaker Blade");
			Tooltip.SetDefault("Melee hits against enemies discharge bolts of energy at nearby enemies that chain to other enemies on hit\nChains up to a max of 3 times, and each bolt may hit 2 targets max\nConsumes 250 Electric Charge to discharge bolts\nCounts as a True Melee sword");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();

			item.damage = 80;
			item.width = 48;
			item.height = 48;
			item.melee = true;
			item.rare = ItemRarityID.Lime;
			item.value = 400000;
			item.useStyle = 1;
			item.useAnimation = 35;
			item.useTime = 35;
			item.knockBack = 8;
			item.autoReuse = true;
			item.consumable = false;
			item.useTurn = false;
			item.UseSound = SoundID.Item1;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/CircuitBreakerBlade_Glow");
			}
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			Vector2 position = player.Center;
			Vector2 eree = player.itemRotation.ToRotationVector2();
			eree *= player.direction;

			position += (eree * Main.rand.NextFloat(58f,160f));

				target.immune[player.whoAmI] = 15;
				NPC target2 = CircuitBreakerBlade.FindClosestTarget(player, position, new Vector2(0, 0));
				if (target2 != null)
				{
					if (player.SGAPly().ConsumeElectricCharge(250, 50))
					{
					Vector2 Speed = (target2.Center - target.Center);
					Speed.Normalize(); Speed *= 2f;
					int prog = Projectile.NewProjectile(target.Center.X, target.Center.Y, Speed.X, Speed.Y, ModContent.ProjectileType<CBreakerBolt>(), (int)(damage * 0.75), knockBack / 2f, player.whoAmI, 3);
					IdgProjectile.Sync(prog);
					Main.PlaySound(SoundID.Item93, position);
					}

				}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("TeslaStaff"), 1);
			recipe.AddIngredient(ItemID.BreakerBlade, 1);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.Cog, 50);
			recipe.AddIngredient(mod.ItemType("ManaBattery"), 2);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 10);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class CBreakerBolt : ModProjectile
	{
		bool transboost = false;
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 2;
			projectile.melee = true;
			projectile.timeLeft = 120;
			projectile.light = 0.1f;
			projectile.extraUpdates = 120;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(projectile.localAI[0]);
			writer.Write(projectile.localAI[1]);
			writer.Write(projectile.minion);
			writer.Write(projectile.melee);
			writer.Write(projectile.magic);
			writer.Write(projectile.usesLocalNPCImmunity);
			writer.Write(projectile.localNPCHitCooldown);
			writer.Write(transboost);

		}

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			projectile.localAI[0] = reader.ReadInt32();
			projectile.localAI[1] = reader.ReadInt32();
			projectile.minion = reader.ReadBoolean();
			projectile.melee = reader.ReadBoolean();
			projectile.magic = reader.ReadBoolean();
			projectile.usesLocalNPCImmunity = reader.ReadBoolean();
			projectile.localNPCHitCooldown = reader.ReadInt32();
			transboost = reader.ReadBoolean();
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Breaker Bolt");
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.ignoreWater = true;
			projectile.Kill();
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.ai[0] > 0)
			{
				projectile.ai[0] -= 1;
				NPC target2 = CircuitBreakerBlade.FindClosestTarget(Main.player[projectile.owner], projectile.Center, new Vector2(0, 0));
				if (target2 != null)
				{
					Vector2 Speed = (target2.Center - target.Center);
					Speed.Normalize(); Speed *= 2f;
					int prog = Projectile.NewProjectile(target.Center.X, target.Center.Y, Speed.X, Speed.Y, ModContent.ProjectileType<CBreakerBolt>(),projectile.damage, projectile.knockBack / 2f, projectile.owner, projectile.ai[0]);
					Main.projectile[prog].melee = projectile.melee;
					Main.projectile[prog].magic = projectile.magic;
					Main.projectile[prog].aiStyle = -200;

					IdgProjectile.Sync(prog);
					Main.PlaySound(SoundID.Item93, projectile.Center);
				}
			}
			if (projectile.penetrate < 1)
			{
				projectile.ignoreWater = true;
				projectile.Kill();
			}
		}

		public override bool PreKill(int timeLeft)
		{
			if (projectile.ignoreWater) {
				for (int k = 0; k < 10; k++)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= 1f;
					int num655 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 206, projectile.velocity.X + randomcircle.X * 8f, projectile.velocity.Y + randomcircle.Y * 8f, 100, new Color(30, 30, 30, 20), 1.5f*(1f+(projectile.ai[0]/3f)));
					Main.dust[num655].noGravity = true;
					Main.dust[num655].velocity *= 0.5f;
				}
			}


			return true;
		}

		Vector2 basepoint=Vector2.Zero;

		public override void AI()
		{			
			if (projectile.localAI[1] == 0f)
			{
				projectile.localAI[1] = Main.rand.NextFloat();
				projectile.netUpdate = true;
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			}


			Vector2 gothere = projectile.velocity;
			gothere=gothere.RotatedBy(MathHelper.ToRadians(90));
			gothere.Normalize();
			Player player = Main.player[projectile.owner];

			if (basepoint == Vector2.Zero)
			{
				SGAPlayer sgaply = player.SGAPly();
				if (sgaply.transformerAccessory && !transboost && projectile.aiStyle>-100)
				{
					projectile.aiStyle = -150;
					projectile.ai[0] += 1;
					transboost = true;
				}

				basepoint = projectile.Center;
				projectile.localAI[1] = (float)player.SGAPly().timer;
			}
			else
			{
				basepoint += projectile.velocity;
			}

			float theammount = ((float)projectile.timeLeft + (float)(projectile.whoAmI*6454f)+(projectile.localAI[1]* 72.454f));
			float scale = (1f - projectile.ai[1]);


			Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= 0.1f;
			int num655 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 206, projectile.velocity.X + randomcircle.X * 8f, projectile.velocity.Y + randomcircle.Y * 8f, 100, new Color(30, 30, 30, 20), 1f * (1f + (projectile.ai[0] / 3f)));
			Main.dust[num655].noGravity = true;
			Main.dust[num655].velocity *= 0.5f;


			projectile.Center += ((gothere * ((float)Math.Sin((double)theammount / 7.10) * (1.97f * scale)))+ (gothere * ((float)Math.Cos((double)theammount / -13.00) * (2.95f * scale)))+ (gothere * ((float)Math.Sin((double)theammount / 4.34566334) * (2.1221f * scale)))
				*(1f - projectile.localAI[0]));


		}
	}

	public class TeslaStaff : SeriousSamWeapon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 0.01f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tesla Staff");
			Tooltip.SetDefault("Zaps nearby enemies with a shock of electricity that is able to pierce twice\nRequires 50 Electric Charge to discharge bolts");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.magic = true;
			item.mana = 2;
			item.width = 40;
			item.height = 40;
			item.useTime = 4;
			item.useAnimation = 4;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 0;
			item.value = 75000;
			item.rare = 3;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("UnmanedBolt");
			item.shootSpeed = 4f;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/TeslaStaff_Glow");
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 normz = new Vector2(speedX, speedY); normz.Normalize();
			position += normz * 58f;


			NPC target2 = CircuitBreakerBlade.FindClosestTarget(player, position, new Vector2(0, 0));
			if (target2 != null)
			{
				if (player.SGAPly().ConsumeElectricCharge(50, 60))
				{
					Vector2 Speed = (target2.Center - position);
					Speed.Normalize(); Speed *= 2f;
					int prog = Projectile.NewProjectile(position.X, position.Y, Speed.X, Speed.Y, ModContent.ProjectileType<CBreakerBolt>(), (int)(damage * 1), knockBack, player.whoAmI);
					Main.projectile[prog].melee = false;
					Main.projectile[prog].magic = true;
					Main.projectile[prog].usesLocalNPCImmunity = false;
					IdgProjectile.Sync(prog);
					Main.PlaySound(SoundID.Item93, position);
				}
			}

			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wire, 50);
			recipe.AddIngredient(mod.ItemType("UnmanedStaff"), 1);
			recipe.AddIngredient(mod.ItemType("ManaBattery"), 1);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 6);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class Massacre : SeriousSamWeapon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Massacre Prototype");
			Tooltip.SetDefault("Fires a chain of Stardust Explosions at a cost of 50 HP\nFiring this weapon throws you back and resets life regen\n'Ansaksie would not approve'");
		}

		public override void SetDefaults()
		{
			item.damage = 250;
			item.magic = true;
			item.width = 56;
			item.height = 28;
			item.useTime = 90;
			item.useAnimation = 90;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 5;
			item.value = Item.sellPrice(platinum: 1);
			item.rare = 11;
			item.UseSound = SoundID.Item122;
			item.autoReuse = false;
			item.shoot = 14;
			item.mana = 100;
			item.shootSpeed = 200f;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/Massacre_Glow");
				item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -18;
			}
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PrismalLauncher", 1);
			recipe.AddIngredient(null, "QuasarCannon", 1);
			recipe.AddIngredient(ItemID.ProximityMineLauncher, 1);
			recipe.AddIngredient(ItemID.Stynger, 1);
			recipe.AddIngredient(ItemID.FragmentStardust, 10);
			recipe.AddIngredient(mod.ItemType("ManaBattery"), 4);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 15);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool CanUseItem(Player player)
		{
			if (player.statLife <= 50)
			{
				if (Main.netMode < NetmodeID.Server)
				{
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.OrangeRed, "Insufficient Health", false, true);
					Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 93, 0.75f, 0.5f);
				}
				return false;

			}
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.Red, 50, false, false);
			if (Main.netMode == 1 && player.whoAmI == Main.myPlayer)
			{
				NetMessage.SendData(35, -1, -1, null, player.whoAmI, 50, 0f, 0f, 0, 0, 0);
			}
			player.statLife -= 50;
			player.netLife = true;
			player.lifeRegenTime = 0;
			player.lifeRegenCount = 0;

			player.velocity += new Vector2(Math.Sign(-player.direction) * 20, (-10f - (speedY / 15f)));
			int numberProjectiles = 4;// + Main.rand.Next(2);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale;
				int prog = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<MassacreShot>(), damage, knockBack, player.whoAmI);
				IdgProjectile.Sync(prog);
			}
			return false;
		}
	}

	public class MassacreShot : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.timeLeft = 10;
			projectile.light = 0.1f;
			projectile.extraUpdates = 0;
			projectile.tileCollide = false;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Massa proj");
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}


		public override void AI()
		{
			projectile.velocity = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(25));
			Vector2 vex = Main.rand.NextVector2Circular(160, 160);
			int prog = Projectile.NewProjectile(projectile.Center.X+ vex.X, projectile.Center.Y+ vex.Y, 0,0, ProjectileID.StardustGuardianExplosion, projectile.damage, projectile.knockBack, projectile.owner,0f,8f);
			Main.projectile[prog].scale = 3f;
			Main.projectile[prog].usesLocalNPCImmunity = true;
			Main.projectile[prog].localNPCHitCooldown = -1;
			Main.projectile[prog].magic = true;
			Main.projectile[prog].minion = false;
			Main.projectile[prog].netUpdate = true;
			IdgProjectile.Sync(prog);
		}
	}

	public class RodofEnforcement : SeriousSamWeapon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rod of Enforcement");
			Tooltip.SetDefault("Conjures an impacting force of energy at the mouse cursor\nRelease to send the force flying towards your cursor; pierces many times");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.magic = true;
			item.width = 56;
			item.height = 28;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 18;
			item.value = 75000;
			item.rare = 4;
			item.UseSound = SoundID.Item100;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("ROEproj");
			item.shootSpeed = 16f;
			item.mana = 15;
			item.channel = true;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/RodofEnforcement_Glow");
			}
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -4);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 8);
			recipe.AddIngredient(ItemID.Actuator, 10);
			recipe.AddIngredient(mod.ItemType("ManaBattery"), 1);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 10);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//Vector2 normz = new Vector2(speedX, speedY); normz.Normalize();
			//position += normz * 24f;

			
			return true;
		}
	}



	public class ROEproj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rod of Enforcing");
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + 14); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 10;
			projectile.light = 0.5f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.magic = true;
			projectile.tileCollide = false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.ai[0]<1)
			return false;
			return null;
		}

		public override bool PreKill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item45, projectile.Center);

			return true;
		}

		public override void AI()
		{

			//int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("AcidDust"), projectile.velocity.X * 1f, projectile.velocity.Y * 1f, 20, default(Color), 1f);

			bool cond = projectile.ai[1] < 1 || projectile.ai[0] == 2 || projectile.timeLeft == 2;
			for (int num621 = 0; num621 < (cond ? 30 : 1); num621++)
			{
				int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, projectile.velocity.X * (cond ? 1.5f : 0.5f), projectile.velocity.Y * (cond ? 1.5f : 0.5f), 20, default(Color), 1f);
				Main.dust[num622].velocity *= 1f;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
				Main.dust[num622].noGravity = true;
			}


			Player player = Main.player[projectile.owner];
			projectile.ai[1] += 1;
			if (player.dead)
			{
				projectile.Kill();
			}
			else
			{
				if (((projectile.ai[0] > 0 || player.statMana < 1) || !player.channel) && projectile.ai[1]>1)
				{
					projectile.ai[0] += 1;
					if (projectile.ai[0] == 1)
					{
						Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 113, 0.5f, -0.25f);

					}


					if (projectile.ai[0] == 4)
					{

					}
					int dir2 = projectile.direction;
					Vector2 distz = projectile.Center - player.Center;
					player.itemRotation = (float)Math.Atan2(distz.Y * dir2, distz.X * dir2);
				}
				else
				{
					if (projectile.ai[0] < 1)
					{
						Vector2 mousePos = Main.MouseWorld;
						if (projectile.owner == Main.myPlayer && projectile.ai[1] < 2)
						{
							projectile.Center = mousePos;
							projectile.netUpdate = true;
						}
						if (projectile.owner == Main.myPlayer && mousePos!= projectile.Center)
						{
							Vector2 diff2 = mousePos - projectile.Center;
							diff2.Normalize();
							projectile.velocity = diff2 * 20f;
							projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
							projectile.netUpdate = true;
							//projectile.Center = mousePos;

						}

						projectile.timeLeft = 40;
						projectile.position -= projectile.velocity;


						//projectile.position -= projectile.Center;
						int dir = projectile.direction;
						player.ChangeDir(dir);
						player.itemTime = 40;
						player.itemAnimation = 38;

						Vector2 distz = projectile.Center - player.Center;
						player.itemRotation = (float)Math.Atan2(distz.Y * dir, distz.X * dir);


						//projectile.Center = (player.Center + projectile.velocity * 26f) + new Vector2(0, -24);
					}
				}
			}
		}
	}

	public class BeamCannon : SeriousSamWeapon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 0.20f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beam Cannon");
			Tooltip.SetDefault("Fires discharged bolts of piercing plasma\nThe less mana you have, the more your bolts fork out from where you aim\nAlt fire drains Plasma Cells to fire a wide spread of bolts");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 4));
			SGAmod.UsesPlasma.Add(SGAmod.Instance.ItemType("BeamCannon"), 1000);
		}

		public override void SetDefaults()
		{
			item.damage = 150;
			item.magic = true;	
			item.crit = 20;
			item.width = 56;
			item.height = 28;
			item.useTime = 7;
			item.useAnimation = 7;
			item.useStyle = 5;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.knockBack = 1;
			item.value = 1000000;
			item.rare = 9;
			item.UseSound = SoundID.Item115;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("BeamCannonHolding");
			item.shootSpeed = 16f;
			item.mana = 8;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[mod.ProjectileType("BeamCannonHolding")] > 0)
				return false;
			SGAPlayer modply = player.GetModPlayer<SGAPlayer>();
				return (modply.RefilPlasma());
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StarMetalBar>(), 16);
			recipe.AddIngredient(ModContent.ItemType<PlasmaCell>(), 3);
			recipe.AddIngredient(ModContent.ItemType<ManaBattery>(), 5);
			recipe.AddIngredient(ModContent.ItemType< AdvancedPlating>(), 8);
			recipe.AddIngredient(ModContent.ItemType<BeamGun>(), 1);
			recipe.AddIngredient(ItemID.LunarBar, 8);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			SGAPlayer modply = player.GetModPlayer<SGAPlayer>();
			position = player.Center;
			Vector2 offset = new Vector2(speedX, speedY);
			offset.Normalize();
			offset *= 16f;
			//position += offset;

			if (player.altFunctionUse == 2)
			{
				modply.plasmaLeftInClip -= 50;
				player.CheckMana(item,40, true);
				player.itemTime *= 5;
				player.itemAnimation *= 5;
				Main.PlaySound(SoundID.Item, player.Center, 122);
			}
			else
			{
				modply.plasmaLeftInClip -= 5;
			}
				if (modply.plasmaLeftInClip < 1)
				{
					player.itemTime = 90;
				}

			for (float i = -40; i < 41; i += 20)
			{
				if (player.altFunctionUse == 2 || i == 0f)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
					Vector2 perturbedSpeed2 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(Math.Max(1f,-20f+(60f-(float)((float)player.statMana/ (float)player.statManaMax2)*60f))));
					float scale = 1f;// - (Main.rand.NextFloat() * .2f);
					perturbedSpeed = perturbedSpeed * scale;

					float rotation = MathHelper.ToRadians(3);
					Vector2 speed = new Vector2(0f, 72f);
					Vector2 starting = new Vector2(position.X + offset.X, position.Y + offset.Y);
					float aithis = (perturbedSpeed2).ToRotation() + MathHelper.ToRadians(i);
					int proj = Projectile.NewProjectile(starting.X+ Math.Sign(perturbedSpeed.X) * 6, starting.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.CultistBossLightningOrbArc, damage, 15f, player.whoAmI, aithis, modply.timer);
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].hostile = false;
					//Main.projectile[proj].usesLocalNPCImmunity = true;
					Main.projectile[proj].localNPCHitCooldown = 5;
					Main.projectile[proj].penetrate = -1;
					Main.projectile[proj].timeLeft = 300;
					Main.projectile[proj].magic = true;
					if (player.altFunctionUse != 2)
					{
						Main.projectile[proj].GetGlobalProjectile<SGAprojectile>().shortlightning = 14;
						Main.projectile[proj].extraUpdates = 2;
					}
					Main.projectile[proj].ai[0] = aithis;
					Main.projectile[proj].netUpdate = true;
					IdgProjectile.Sync(proj);
				}


			}
			offset /= 8f;
			int prog = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, offset.X, offset.Y, mod.ProjectileType("BeamCannonHolding"), damage, knockBack, player.whoAmI);

			return false;
		}
	}

	public class BeamCannonHolding : ModProjectile
	{
		public virtual bool bounce => true;
		//public virtual float trans => 1f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beam Gun");
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool CanHitPlayer(Player target)
		{
			return false;
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.timeLeft = 3;
			projectile.penetrate = -1;
			aiType = ProjectileID.WoodenArrowFriendly;
			projectile.damage = 0;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override void AI()
		{
			projectile.localAI[0] += 1f;

			Player player = Main.player[projectile.owner];

			if (player != null && player.active)
			{

				SGAPlayer modply = player.GetModPlayer<SGAPlayer>();

				if (player.dead)
				{
					projectile.Kill();
				}
				else
				{


					player.heldProj = projectile.whoAmI;
					player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * projectile.direction, projectile.velocity.X * projectile.direction);
					projectile.rotation = player.itemRotation - MathHelper.ToRadians(90);
					projectile.Center = (player.Center + new Vector2(player.direction * 6, 0)) + (projectile.velocity * 10f);


					projectile.position -= projectile.velocity;
					if (player.itemTime > 0)
					projectile.timeLeft = 2;
					Vector2 position = projectile.Center;
					Vector2 offset = new Vector2(projectile.velocity.X, projectile.velocity.Y);
					offset.Normalize();
					offset *= 16f;

				}
			}
			else
			{
				projectile.Kill();
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = ModContent.GetTexture("SGAmod/Items/Weapons/Technical/BeamCannon");
			int frames = 4;
			//Texture2D texGlow = ModContent.GetTexture("SGAmod/Items/Weapons/SeriousSam/BeamGunProjGlow");
			SpriteEffects effects = SpriteEffects.FlipHorizontally;
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / frames) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
			Color color = projectile.GetAlpha(lightColor) * 1f; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = (int)(Main.GlobalTime*8f);
			timing %= frames;
			timing *= ((tex.Height) / frames);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / frames), color, projectile.rotation - MathHelper.ToRadians(90*projectile.direction), drawOrigin, projectile.scale, projectile.direction < 1 ? effects : (SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally), 0f);
			//spriteBatch.Draw(texGlow, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / frames), Color.White, projectile.rotation - MathHelper.ToRadians(90 * projectile.direction), drawOrigin, projectile.scale, projectile.direction < 1 ? effects : (SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally), 0f);

			return false;
		}


	}


	public class ReRouterSummon : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Re-Router");
			Tooltip.SetDefault("Throws a handheld turret device that when, shots are fired into it are rerouted to nearby enemies (or Minion targeted enemy)\nThe shots have their damage Amped by your Technological Damage bonus\nThe max velocity you can throw the device is boosted by your Throwing Velocity\nConsumes Electric Charge per reroute based on projectile damage\nThis counts as a sentry summon");
		}

		public override void SetDefaults()
		{
			item.summon = true;
			item.sentry = true;
			item.width = 24;
			item.height = 30;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 1;
			item.noMelee = true;
			item.knockBack = 2f;
			item.noUseGraphic = true;
			item.value = Item.buyPrice(0, 0, 5, 0);
			item.rare = ItemRarityID.Green;
			item.autoReuse = false;
			item.shootSpeed = 20f;
			item.consumable = true;
			item.maxStack = 30;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<ReRouterProjectile>();
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse != 2)
			{

				Vector2 speedto = Main.MouseWorld - position;
				Vector2 speed = Vector2.Normalize(speedto) * (Math.Min(400f*player.Throwing().thrownVelocity, speedto.Length()));

				speed /= 30f;

				speedX = speed.X;
				speedY = speed.Y;

				//position = Main.MouseWorld;
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
				player.UpdateMaxTurrets();
			}
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("LaserMarker"), 2);
			recipe.AddIngredient(mod.ItemType("NoviteBar"), 3);
			recipe.AddIngredient(mod.ItemType("VialofAcid"), 2);
			recipe.AddIngredient(ItemID.MeteoriteBar, 1);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this,3);
			recipe.AddRecipe();
		}
	}

	public class ReRouterProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Re-Router");
			//ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
		}

        public override string Texture => "SGAmod/Items/Weapons/Technical/ReRouterSummon";

        public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.sentry = true;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.penetrate = -1;
		}

		public override void AI()
		{

			Player player = Main.player[base.projectile.owner];
			projectile.ai[0] += 1;
			projectile.localAI[0] += 1;

			projectile.rotation += projectile.velocity.X / 24f;

			if (projectile.ai[0] < 30)
            {
				projectile.velocity.Y += 0.30f;
				return;

            }
			if (projectile.tileCollide)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X,(int)projectile.Center.Y, 78,0.75f,0.75f);
				projectile.tileCollide = false;
			}
			projectile.velocity /= 1.25f;

			if (projectile.ai[0] % 1 == 0)
			{

				List<Point> targetthem = new List<Point>();

				if (player.HasMinionAttackTargetNPC)
				{
					NPC target2 = Main.npc[player.MinionAttackTargetNPC];
					if (target2 != null && target2.active && target2.life > 0)
					{
						targetthem.Add(new Point(target2.whoAmI, -1000));
					}
				}

				List<NPC> closestnpcs = SGAUtils.ClosestEnemies(projectile.Center, 600f, AddedWeight: targetthem);

				NPC target = closestnpcs?[0];//Closest

				if (target != null && target.active && target.life > 0 && Vector2.Distance(target.Center, projectile.Center) < 600)
				{

					foreach (Projectile proj in Main.projectile)
					{
						bool contact = Main.projPet[proj.type];
						if (proj.active && proj.friendly && proj.damage>0 && player.heldProj != proj.whoAmI && (!contact))
						{
							if (new Rectangle((int)proj.position.X, (int)proj.position.Y, proj.width, proj.height).Intersects(new Rectangle((int)projectile.position.X-8, (int)projectile.position.Y-8, projectile.width+16, projectile.height+16)))
							{
								SGAprojectile sgaProj = proj.GetGlobalProjectile<SGAprojectile>();
								if (sgaProj.rerouted == false && player.SGAPly().ConsumeElectricCharge(proj.damage, 60))
								{
									Vector2 there = projectile.Center + new Vector2(0, 0f);
									Vector2 Speed = (target.Center - there);
									Speed.Normalize();

									proj.Center = projectile.Center;
									proj.velocity = Speed*(proj.velocity.Length());
									proj.damage = (int)(proj.damage * player.SGAPly().techdamage);
									proj.netUpdate = true;
									sgaProj.rerouted = true;

									projectile.rotation = Speed.ToRotation();
									Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 114, 0.75f, 0.75f);
									break;
								}
							}
						}

					}

				}

			}

			for (int ii = -1; ii < 2; ii += 2)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Vector2 loc = new Vector2(Main.rand.NextFloat(-12f, 12f), 8f* ii) + randomcircle * new Vector2(2f, 1f);
				loc = loc.RotatedBy(projectile.rotation);
				int num622 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) + loc, 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.75f);
				Main.dust[num622].scale = 0.25f;
				Main.dust[num622].noGravity = true;
				Main.dust[num622].velocity.X = randomcircle.RotatedBy(MathHelper.ToRadians(-90)).X * 2f;
				Main.dust[num622].velocity.Y = randomcircle.RotatedBy(MathHelper.ToRadians(-90)).Y;
				Main.dust[num622].velocity.Y += Main.rand.NextFloat(2f,5f);
				Main.dust[num622].alpha = 150;
			}

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texa = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(texa, projectile.Center - Main.screenPosition, null, lightColor * MathHelper.Clamp(projectile.localAI[0] / 15f, 0f, 1f), projectile.rotation, new Vector2(texa.Width, texa.Height) / 2f, new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.ai[0] = Math.Max(projectile.ai[0], 20);

			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}

			projectile.velocity /= 2f;
			return false;
		}

		public override bool CanDamage()
		{
			return false;
		}
	}

	public class VolcanicSpaceBlaster : NoviteBlaster, ITechItem
	{
		public new float ElectricChargeScalingPerUse() => 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("V.S.B");
			Tooltip.SetDefault("Fires a tight spread of Lasers\nConsumes Electric Charge, 300 up front, 360 over time to charge up\nAt max charge, releases a powerful volcanic fireball that engulfs enemies in lava\nVolcanic Space Blaster: where Energy: Matter(s)");
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.magic = true;
			item.width = 32;
			item.height = 62;
			item.useTime = 70;
			item.useAnimation = 70;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 2;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Lime;
			//item.UseSound = SoundID.Item99;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<VocanicBeaterCharging>();
			item.shootSpeed = 50f;
			item.noUseGraphic = false;
			item.channel = true;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/VolcanicSpaceBlaster_Glow");
				item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -6;
			}
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, -0);
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().ConsumeElectricCharge(300, 0, consume: false);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SpaceGun, 1);
			recipe.AddIngredient(mod.ItemType("HeatBeater"), 1);
			recipe.AddIngredient(ItemID.Nanites, 100);
			recipe.AddIngredient(null, "AdvancedPlating", 8);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float rotation = MathHelper.ToRadians(0);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[mod.ProjectileType("VocanicBeaterCharging")] < 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("VocanicBeaterCharging"), damage, knockBack, player.whoAmI);
				player.SGAPly().ConsumeElectricCharge(300, 120);
			}
			return false;
		}

	}

	public class VocanicBeaterCharging : NovaBlasterCharging
	{

		public override int chargeuptime => 120;
		public override float velocity => 72f;
        public override float spacing => 96f;
		public override int fireRate => 30;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vocanic Beater Charging");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.magic = true;
			aiType = 0;
		}

		public override void ChargeUpEffects()
		{

			if (projectile.ai[0] < chargeuptime)
			{
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 3) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y) + randomcircle * 20, 0, 0, DustID.Fire, 0f, 0f, 100, default(Color), 0.75f);

						Main.dust[num622].scale = 1f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = -randomcircle.X;
						Main.dust[num622].velocity.Y = -randomcircle.Y;
						Main.dust[num622].alpha = 150;
					}
				}
			}
			else
			{
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 2) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y), 0, 0, DustID.Fire, 0f, 0f, 100, default(Color), 0.75f);

						Main.dust[num622].scale = 1.5f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = randomcircle.X * 2;
						Main.dust[num622].velocity.Y = randomcircle.Y * 2;
						Main.dust[num622].alpha = 100;
					}
				}
			}


			if (projectile.ai[0] == chargeuptime)
			{
				for (int num315 = 0; num315 < 35; num315 = num315 + 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y), 0, 0, DustID.Fire, 0f, 0f, 100, default(Color), 0.5f);

					Main.dust[num622].scale = 2.8f;
					Main.dust[num622].noGravity = true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = randomcircle.X * 4f;
					Main.dust[num622].velocity.Y = randomcircle.Y * 4f;
					Main.dust[num622].alpha = 150;
				}
			}

		}

		public override bool DoChargeUp()
		{
			return player.SGAPly().ConsumeElectricCharge(3, 60);
		}

		public override void FireWeapon(Vector2 direction)
		{
			float perc = MathHelper.Clamp(projectile.ai[0] / (float)chargeuptime, 0f, 1f);

			float speed = 8f + perc * 4f;

			Vector2 perturbedSpeed = (new Vector2(direction.X, direction.Y) * speed); // Watch out for dividing by 0 if there is only 1 projectile.

			projectile.Center += projectile.velocity;

			int damage = projectile.damage;

			int type = ProjectileID.LaserMachinegunLaser;

			if (projectile.ai[0] >= chargeuptime)
			{
				type = ModContent.ProjectileType<VolcanicShot>();
				Vector2 center = projectile.Center;
				int prog = Projectile.NewProjectile(center.X, center.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage*8, 8f, player.whoAmI);
				IdgProjectile.AddOnHitBuff(prog, ModContent.BuffType<LavaBurn>(), (int)(120 + (perc * 180f)));

				Main.PlaySound(SoundID.Item73, player.Center);
				player.velocity -= perturbedSpeed;

					for (int num315 = 0; num315 < 35; num315 = num315 + 1)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y), 0, 0, DustID.Fire, 0f, 0f, 100, default(Color), 0.5f);

						Main.dust[num622].scale = 3.2f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = randomcircle.X * 6f;
						Main.dust[num622].velocity.Y = randomcircle.Y * 6f;
						Main.dust[num622].alpha = 50;
					}

			}
			else
			{

				for (float i = -1f; i < 1.01f; i += 0.5f)
				{
					Vector2 center = projectile.Center + ((new Vector2(1f, 0f) * i * 8f).RotatedBy(perturbedSpeed.ToRotation() + MathHelper.PiOver2));
					int prog = Projectile.NewProjectile(center.X, center.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, (int)(2f + perc * 4f), player.whoAmI);
					Main.projectile[prog].usesLocalNPCImmunity = true;
					Main.projectile[prog].localNPCHitCooldown = -1;
					Main.projectile[prog].penetrate = 3;
					Main.projectile[prog].netUpdate = true;
					//IdgProjectile.Sync(prog);
				}
				Main.PlaySound(SoundID.Item91, player.Center);
			}

			projectile.Kill();
		}

	}

	public class VolcanicShot : FieryRock
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Volcanic Shot");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 120;
			projectile.timeLeft = 600;
			projectile.width = 24;
			projectile.extraUpdates = 1;
			projectile.height = 24;
			projectile.magic = true;
			projectile.tileCollide = true;
		}

        public override bool CanDamage()
        {
            return projectile.timeLeft > 60;
        }

        public override string Texture => "SGAmod/Projectiles/FieryRock";

		public override bool PreKill(int timeLeft)
		{
			return true;
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.Center / 16f, (Color.Orange * 0.25f).ToVector3());
			if (projectile.penetrate < 100 && projectile.timeLeft > 60)
				projectile.timeLeft = 60;
			int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 20, default(Color), 1f);
			Main.dust[DustID2].noGravity = true;

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			for (int i = 0; i < projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
			{
				if (projectile.oldPos[i] == default)
					projectile.oldPos[i] = projectile.position;
			}

			Texture2D tex = Main.projectileTexture[projectile.type];

			TrailHelper trail = new TrailHelper("DefaultPass", mod.GetTexture("Noise"));
			trail.color = delegate (float percent)
			{
				return Color.Orange;
			};
			trail.projsize = tex.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
			trail.trailThickness = 4;
			trail.trailThicknessIncrease = 6;
			trail.strength = MathHelper.Clamp((projectile.timeLeft-20f)/60f,0f,1f);
			trail.DrawTrail(projectile.oldPos.ToList(), projectile.Center);

			Vector2 offset = tex.Size() / 2f;

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.Orange * Math.Min(projectile.timeLeft / 20f, 1f), projectile.rotation, offset, projectile.scale, projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

			return false;
		}
	}

	public class SirianGun : NoviteBlaster, ITechItem
	{
		public new float ElectricChargeScalingPerUse() => 0.25f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sirian Gun");
			Tooltip.SetDefault("charges up several piercing bolts of electricity\nConsumes Electric Charge, hold the fire button to charge a stronger, more accurate shot");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.damage = 18;
			item.magic = true;
			item.useTime = 60;
			item.useAnimation = 60;
			item.knockBack = 1;
			item.value = Item.buyPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Pink;
			//item.UseSound = SoundID.Item99;
			item.autoReuse = true;
			item.shootSpeed = 50f;
			item.noUseGraphic = false;
			item.channel = true;
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().ConsumeElectricCharge(150, 0, consume: false);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NoviteBlaster>(), 1);
			recipe.AddIngredient(ItemID.MeteoriteBar, 8);
			recipe.AddIngredient(ModContent.ItemType<WraithFragment4>(), 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SirianGunCharging>()] < 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<SirianGunCharging>(), damage, knockBack, player.whoAmI);
				player.SGAPly().ConsumeElectricCharge(150, 200);
			}
			return false;
		}

	}

	public class SirianGunCharging : NovaBlasterCharging
	{

		public override int chargeuptime => 120;
		public override float velocity => 6f;
		public override float spacing => 48f;
		public override int fireRate => 5;
		public override int FireCount => 1 + (int)(projectile.ai[0] / 20f);

		public override (float, float) AimSpeed => (0.20f, 0.03f);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sirian Gun Charging");
		}

		public override void ChargeUpEffects()
		{

			if (projectile.ai[0] < chargeuptime)
			{
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 5) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y) + randomcircle * 20, 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.75f);

						Main.dust[num622].scale = 1f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = -randomcircle.X;
						Main.dust[num622].velocity.Y = -randomcircle.Y;
						Main.dust[num622].alpha = 150;
					}
				}
			}
			else
			{
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 5) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y), 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.75f);

						Main.dust[num622].scale = 1.5f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = randomcircle.X * 2;
						Main.dust[num622].velocity.Y = randomcircle.Y * 2;
						Main.dust[num622].alpha = 100;
					}
				}
			}


			if (projectile.ai[0] == chargeuptime)
			{
				for (int num315 = 0; num315 < 15; num315 = num315 + 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y), 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.5f);

					Main.dust[num622].scale = 2.8f;
					Main.dust[num622].noGravity = true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = randomcircle.X * 4f;
					Main.dust[num622].velocity.Y = randomcircle.Y * 4f;
					Main.dust[num622].alpha = 150;
				}
			}

		}

		public override void FireWeapon(Vector2 direction)
		{
			float perc = MathHelper.Clamp((firedCount / (float)FireCount), 0f, 1f) * 0.80f;
			float perc2 = 1f - perc;

			float speed = 1f + (perc2 * velocity)* (projectile.ai[0]/(float)chargeuptime);

			//projectile.Center += projectile.velocity;

			for (int i = 0; i < 5; i += 1)
			{
				Vector2 perturbedSpeed = (new Vector2(direction.X, direction.Y) * speed).RotatedBy((Main.rand.NextFloat(-perc, perc)) * 0.75f); // Watch out for dividing by 0 if there is only 1 projectile.
				int prog = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<CBreakerBolt>(), projectile.damage, projectile.knockBack, player.whoAmI, 0, 0.10f);
				Main.projectile[prog].localAI[0] = (perc * 0.90f);
				//Main.projectile[prog].localAI[1] = i;
				Main.projectile[prog].magic = true;
				Main.projectile[prog].melee = false;
				Main.projectile[prog].netUpdate = true;
			}

			Main.PlaySound(SoundID.Item91, player.Center);

			if (firedCount >= FireCount)
				projectile.Kill();
		}

		public override bool DoChargeUp()
		{
			return player.SGAPly().ConsumeElectricCharge(4, 75);
		}
	}


}


namespace SGAmod.HavocGear.Items.Weapons
{
	public class Starduster : NoviteBlaster, ITechItem
	{
		public new float ElectricChargeScalingPerUse() => 0.01f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starduster");
			Tooltip.SetDefault("'Purge your enemies through celestial flames...'\nUses Gel to spray a furry of Stars\nConsumes Electric Charge, Stars fly further the longer you hold\nStars that strike enemies will spawn more stars nearby\nDoes extra damage the less Stars there are\nDoesn't get boosted by Fridgeflame Canister");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.damage = 60;
			item.ranged = true;
			item.crit = 12;
			item.useAmmo = AmmoID.Gel;
			item.useTime = 60;
			item.useAnimation = 60;
			item.knockBack = 1;
			item.value = Item.buyPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Yellow;
			//item.UseSound = SoundID.Item99;
			item.autoReuse = true;
			item.shootSpeed = 10f;
			item.noUseGraphic = false;
			item.channel = true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<StardusterCharging>()] >0)
			{
				return true;
			}
				return player.SGAPly().ConsumeElectricCharge(150, 0, consume: false);
		}

        public override bool ConsumeAmmo(Player player)
        {
			return player.SGAPly().timer%30==0;
        }
        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<XOPFlamethrower>(), 1);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 20);
			recipe.AddIngredient(ModContent.ItemType<PrismalBar>(), 12);
			recipe.AddIngredient(ModContent.ItemType<WraithFragment4>(), 25);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[ModContent.ProjectileType<StardusterCharging>()] < 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<StardusterCharging>(), damage, knockBack, player.whoAmI);
			}
			return false;
		}

	}

	public class StardusterCharging : NovaBlasterCharging,IDrawAdditive
	{
		public List<StardusterProjectile> stardust = new List<StardusterProjectile>();
		public class StardusterProjectile
		{
			public Vector2 position;
			public Vector2 velocity;
			public int timeLeft = 0;
			public int timeStart = 0;
			public int timeLeftMax = 0;
			public int rando = 0;
			public Vector2 scale;
			public Projectile owner;
			public virtual Vector2 Position
            {
				get
				{
					return Vector2.Lerp(owner.Center,position,1F);
				}
			}
			public StardusterProjectile(Vector2 position, Vector2 velocity)
			{
				scale = Vector2.One;
				this.position = position;
				this.velocity = velocity;
				timeLeft = 30;
				timeLeftMax = 30;
				rando = Main.rand.Next();
			}
			public virtual void Update()
            {
				position += velocity;
				timeLeft -= 1;
				timeStart += 1;
			}
		}

		public override int chargeuptime => 250000;
		public override float velocity => 6f;
		public override float spacing => 48f;
		public override int fireRate => 1;
		public override int FireCount => 20;
		public override (float, float) AimSpeed => (0.16f, 0.16f);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starduster Charging");
		}

        public override void SetDefaults()
        {
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ranged = true;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 8;
		}

        public override bool CanDamage()
        {
			return true;
        }

		public override void ChargeUpEffects()
		{
			foreach (StardusterProjectile dust in stardust)
			{
				dust.Update();
			}
			stardust = stardust.Where(testby => testby.timeLeft > 0).ToList();
		}

		public override void FireWeapon(Vector2 direction)
		{
			projectile.localAI[1] += 0.05f;
			//Main.PlaySound(SoundID.Item91, player.Center);

			if (firedCount >= FireCount)
				projectile.Kill();
		}

		public override bool DoChargeUp()
		{

			if (!player.SGAPly().ConsumeElectricCharge(3, 25))
				return false;

			Item item = new Item();
			item.SetDefaults(ModContent.ItemType<Starduster>());

			int projType = item.shoot;

			int damage = projectile.damage;
			float speed = projectile.velocity.Length();
			float kb = projectile.knockBack;

			bool tr = false;
			player.PickAmmo(player.HeldItem, ref projType, ref speed, ref tr, ref damage, ref kb, false);

			if (tr)
			{

				if ((int)(projectile.localAI[0]) % 30 == 0)
				{

					var snd = Main.PlaySound(SoundID.DD2_BookStaffCast, projectile.Center);
					if (snd != null)
					{
						snd.Pitch = -0.75f;
					}
				}

				float chargePercent = MathHelper.Clamp((projectile.ai[0] / 60f), 0f, 1f);

				for (int i = 0; i < 2; i++)
				{
						float Velocity = chargePercent * (Main.rand.NextFloat(6f, 12f)) * (1f + (projectile.localAI[1] * 3f)) * 1.25f;
						float randomAngle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi) / (10f + (chargePercent * 10f));
						StardusterProjectile starduster = new StardusterProjectile(projectile.Center + Vector2.Normalize(projectile.velocity) * 32f, Vector2.Normalize(projectile.velocity).RotatedBy(randomAngle) * Velocity);
						starduster.owner = projectile;
					starduster.timeLeft = Main.rand.Next(25, 50);
					starduster.timeLeftMax = starduster.timeLeft;

						stardust.Add(starduster);
					
				}
			}


			return true;
		}

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			damage = (int)(damage * (1f+(1f-((float)stardust.Count / 200f)) * 3f));
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (player.SGAPly().ConsumeElectricCharge(3, 25, consume: false))
			{
				for (int i = 0; i < 3; i++)
				{
					if (Main.rand.Next(250) > stardust.Count)
					{
						StardusterProjectile starduster = new StardusterProjectile(target.Center + Main.rand.NextVector2Circular(256, 256), Main.rand.NextVector2Circular(2, 2));
						starduster.scale = Vector2.One * 1.5f;
						starduster.timeLeft = Main.rand.Next(10, 80);
						starduster.timeLeftMax = starduster.timeLeft;
						starduster.owner = projectile;

						stardust.Add(starduster);

					}
				}
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			foreach (StardusterProjectile stardustProj in stardust)
			{
				Rectangle recto = new Rectangle((int)stardustProj.Position.X - (int)(8 * stardustProj.scale.X), (int)stardustProj.Position.Y - (int)(8 * stardustProj.scale.Y), (int)(16 * stardustProj.scale.X), (int)(16* stardustProj.scale.Y));
				if (targetHitbox.Intersects(recto))
				{
					return true;
				}
			}
			return false;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			float overallAlpha = (1f - Math.Min(projectile.localAI[1], 1f)) * Math.Min(projectile.localAI[0] / 120f, 1f);
			foreach (StardusterProjectile stardustProj in stardust)
			{

				Texture2D startex = Main.starTexture[stardustProj.rando % Main.starTexture.Length];
				Vector2 startexorigin = new Vector2(startex.Width, startex.Height) / 2f;

				float timeLeft = (stardustProj.timeLeft / (float)stardustProj.timeLeftMax)*MathHelper.Clamp((stardustProj.timeStart* 3f) / (float)stardustProj.timeLeftMax,0f,1f);

				float alphaFade = MathHelper.Clamp(timeLeft * 1.5f, 0f, 1f);

				Color color = Color.White;

				spriteBatch.Draw(startex, stardustProj.Position - Main.screenPosition, null, color * alphaFade * overallAlpha, stardustProj.rando%MathHelper.TwoPi, startexorigin, stardustProj.scale, SpriteEffects.None, 0f);
			}

			return false;
        }

        public void DrawAdditive(SpriteBatch spriteBatch)
		{
			float overallAlpha = (1f - Math.Min(projectile.localAI[1], 1f))*Math.Min(projectile.localAI[0]/120f,1f);

			Texture2D glow = ModContent.GetTexture("SGAmod/Glow");
			Vector2 origin = new Vector2(glow.Width, glow.Height) / 2f;

			foreach (StardusterProjectile stardustProj in stardust)
			{

				float timeLeft = (stardustProj.timeLeft / (float)stardustProj.timeLeftMax) * MathHelper.Clamp((stardustProj.timeStart * 3f) / (float)stardustProj.timeLeftMax, 0f, 1f);

				float alphaFade = MathHelper.Clamp(timeLeft *1.5f, 0f, 1f);

				Color color = Color.White;

				spriteBatch.Draw(glow, stardustProj.Position - Main.screenPosition, null, color * alphaFade * overallAlpha, stardustProj.rando % MathHelper.TwoPi, origin, stardustProj.scale, SpriteEffects.None, 0f);
			}
		}

	}


	public class PlasmaGun : NoviteBlaster, ITechItem,IHitScanItem
	{
		public new float ElectricChargeScalingPerUse() => 0.05f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasmic Rail Gun");
			Tooltip.SetDefault("Charges up a powerful piercing railgun shot\nHyper charged plasma melts targets from a long range\nRequires Plasma Cells and Electric Charge to fire\nYou can fire a weaker beam if you run out of Plasma\nRight click to zoom out");
			SGAmod.UsesPlasma.Add(SGAmod.Instance.ItemType("PlasmaGun"), 1000);
		}

		public override void SetDefaults()
		{
			item.damage = 7500;
			item.magic = true;
			item.width = 32;
			item.height = 62;
			item.useTime = 70;
			item.useAnimation = 70;
			item.useStyle = 5;
			item.crit = 25;
			item.noMelee = true;
			item.knockBack = 25;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Cyan;
			//item.UseSound = SoundID.Item99;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<PlasmaGunCharging>();
			item.shootSpeed = 32f;
			item.noUseGraphic = false;
			item.channel = true;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, -0);
		}
		public override void HoldItem(Player player)
		{
			player.scope = true;
		}
		public override bool CanUseItem(Player player)
		{

			return player.SGAPly().RefilPlasma() && player.SGAPly().ConsumeElectricCharge(300, 60, false, false);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);

			recipe.AddIngredient(mod.ItemType("VolcanicSpaceBlaster"), 1);
			recipe.AddIngredient(ItemID.SniperRifle, 1);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 6);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 16);
			recipe.AddIngredient(mod.ItemType("PlasmaCell"), 3);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 5);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float rotation = MathHelper.ToRadians(0);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[item.shoot] < 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, item.shoot, damage, knockBack, player.whoAmI);
				player.SGAPly().ConsumeElectricCharge(200, 300);
			}
			return false;
		}

	}

	public class PlasmaGunCharging : NovaBlasterCharging
	{

		public override int chargeuptime => 180;
		public override float velocity => 72f;
		public override float spacing => 64f;
		public override int fireRate => 30;
		public override (float, float) AimSpeed => (0.10f, 0f);
		int chargeUpTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Gun Charging");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.magic = true;
			aiType = 0;
		}

		public override void ChargeUpEffects()
		{
			chargeUpTimer += 1;

			if (projectile.ai[0] < chargeuptime)
			{
				if (chargeUpTimer % 4 == 0)
				{
					float perc = MathHelper.Clamp(projectile.ai[0] / (float)chargeuptime, 0f, 1f);
					SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_LightningAuraZap, (int)projectile.Center.X, (int)projectile.Center.Y);
					if (sound != null)
					{
						sound.Pitch = -0.5f + perc;
					}
				}
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 3) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y) + randomcircle * 20, 0, 0, DustID.AncientLight, 0f, 0f, 100, Color.Magenta, 0.75f);

						Main.dust[num622].scale = 1f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = -randomcircle.X;
						Main.dust[num622].velocity.Y = -randomcircle.Y;
						Main.dust[num622].alpha = 150;
					}
				}
			}
			else
			{
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 2) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y), 0, 0, DustID.AncientLight, 0f, 0f, 100, Color.Magenta, 0.75f);

						Main.dust[num622].scale = 1.5f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = randomcircle.X * 2;
						Main.dust[num622].velocity.Y = randomcircle.Y * 2;
						Main.dust[num622].alpha = 100;
					}
				}
			}


			if (projectile.ai[0] == chargeuptime)
			{
				SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_LightningBugZap, (int)projectile.Center.X, (int)projectile.Center.Y);
				if (sound != null)
				{
					sound.Pitch -= 0.5f;
				}

				for (int num315 = 0; num315 < 35; num315 = num315 + 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y), 0, 0, DustID.AncientLight, 0f, 0f, 100, Color.Magenta, 0.5f);

					Main.dust[num622].scale = 2.8f;
					Main.dust[num622].noGravity = true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = randomcircle.X * 4f;
					Main.dust[num622].velocity.Y = randomcircle.Y * 4f;
					Main.dust[num622].alpha = 150;
				}
			}

		}

		public override bool DoChargeUp()
		{
			int plasma = player.SGAPly().plasmaLeftInClip;
			player.SGAPly().plasmaLeftInClip = Math.Max(plasma - 1, 0);
			return player.SGAPly().ConsumeElectricCharge(7, 120,consume: plasma>0) && player.SGAPly().plasmaLeftInClip>0;
		}

		public override void FireWeapon(Vector2 direction)
		{
			float perc = MathHelper.Clamp(projectile.ai[0] / (float)chargeuptime, 0f, 1f);

			float speed = 4f+ perc*4f;

			Vector2 perturbedSpeed = (new Vector2(direction.X, direction.Y) * speed); // Watch out for dividing by 0 if there is only 1 projectile.

			projectile.Center += projectile.velocity;

			int damage = (int)(projectile.damage*(projectile.ai[0]/ chargeuptime));

			if (projectile.ai[0] >= chargeuptime || player.SGAPly().plasmaLeftInClip<1)
			{
				int type = ModContent.ProjectileType<PlasmaBeam>();
				Vector2 center = projectile.Center-Vector2.Normalize(perturbedSpeed)*32f;
				int prog = Projectile.NewProjectile(center.X, center.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, projectile.knockBack, player.whoAmI);
				//if (projectile.ai[0] >= chargeuptime)
				//IdgProjectile.AddOnHitBuff(prog, ModContent.BuffType<LavaBurn>(), (int)(120 + (perc * 300f)));

				player.velocity -= perturbedSpeed;

				SGAmod.AddScreenShake(42f,600,player.Center);

				Main.PlaySound(SoundID.Item73, player.Center);
				SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, (int)center.X, (int)center.Y);
				if (sound != null)
				{
					sound.Pitch += 0.5f;
				}

				for (int num315 = 0; num315 < 35; num315 = num315 + 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(center, 0, 0, DustID.Shadowflame, 0f, 0f, 100, default(Color), 0.5f);

					Main.dust[num622].scale = 3.2f;
					Main.dust[num622].noGravity = true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = randomcircle.X * Main.rand.NextFloat(4f,8f);
					Main.dust[num622].velocity.Y = randomcircle.Y * Main.rand.NextFloat(4f, 8f);
					Main.dust[num622].alpha = 50;
				}

				for (int num315 = 0; num315 < 20; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 2) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(center, 0, 0, DustID.AncientLight, 0f, 0f, 100, Color.Magenta, 0.75f);

						Main.dust[num622].scale = 1.5f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						randomcircle *= Main.rand.NextFloat(-2f, 4f+ num315/4f);
						Main.dust[num622].velocity.X = randomcircle.X;
						Main.dust[num622].velocity.Y = randomcircle.Y;
						Main.dust[num622].alpha = 100;
					}
				}

			}
			projectile.Kill();
		}

	}

	public class PlasmaBeam : NPCs.Hellion.HellionBeam
    {
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.timeLeft = 60;
			projectile.damage = 15;
			projectile.width = 64;
			projectile.height = 64;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Beam");
		}

		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.GeyserTrap; }
		}

		public override bool CanDamage()
		{
			if (projectile.ai[1] < 2)
				return true;
			return false;
		}

		public override void AI()
		{

			scale2 = Math.Min(scale2 + 0.5f, 1.5f);

			projectile.ai[1] += 1;

			if (projectile.ai[1] < 40 && projectile.ai[1]%6==0)
			{
				SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_LightningBugZap, (int)projectile.Center.X, (int)projectile.Center.Y);
				if (sound != null)
				{
					sound.Pitch = -1f+(projectile.timeLeft / 60f)*0.80f;
					sound.Volume = projectile.timeLeft/60f;
				}
			}

			if (projectile.ai[1] == 1)
			{

				for (int num315 = 0; num315 < 2200f; num315 = num315 + 4)
				{
					Vector2 offset2 = Vector2.Normalize(projectile.velocity);
					int num622 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y)+ (offset2 * num315), 0, 0, DustID.Shadowflame, 0f, 0f, 100, Color.Magenta, 0.5f);
					float speedz = Main.rand.NextFloat(0.25f, 6f);

					Main.dust[num622].scale = 2.0f;
					Main.dust[num622].noGravity = true;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					offset2 = offset2.RotatedBy(Main.rand.NextBool() ? MathHelper.PiOver2 : -MathHelper.PiOver2);
					Main.dust[num622].velocity.X = offset2.X * speedz;
					Main.dust[num622].velocity.Y = offset2.Y * speedz;
					Main.dust[num622].alpha = 50;
				}

			}

			Vector2 offset = Vector2.Normalize(projectile.velocity);
			Lighting.AddLight(new Vector2(projectile.Center.X, projectile.Center.Y) + (offset * Main.rand.NextFloat(MaxDistance))/16,(Color.Magenta*0.50f).ToVector3());

			projectile.localAI[0] += 0.2f;
			base.AI();
		}

		public override void MoreAI(Vector2 dustspot)
		{
			//nil
			//SGAmod.updatelasers = true;
		}

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            //nah
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (Main.dedServ)
				return false;

			Color colortex = Color.Purple * 0.75f;
			Vector2 scale = new Vector2(MathHelper.Clamp(projectile.timeLeft / 8f, 0f, 1f) * scale2, 1f);

			List<Vector2> vectors = new List<Vector2>();
			vectors.Add(projectile.Center);
			vectors.Add(hitspot);

			TrailHelper trail = new TrailHelper("FadedBasicEffectPass", Main.extraTexture[21]);
			trail.projsize = Vector2.Zero;
			trail.coordOffset = new Vector2(0, projectile.localAI[0] * -0.22f);
			trail.coordMultiplier = new Vector2(1f, 120f);
			trail.doFade = false;
			trail.trailThickness = 18 * scale.X;
			trail.trailThicknessIncrease = 0;
			trail.color = delegate (float percent)
			{
				return colortex;
			};
			trail.DrawTrail(vectors, projectile.Center);

			trail = new TrailHelper("FadedBasicEffectPass", Main.extraTexture[21]);
			trail.projsize = Vector2.Zero;
			trail.coordOffset = new Vector2(0, projectile.localAI[0] * 0.22f);
			trail.coordMultiplier = new Vector2(2f, 180f);
			trail.doFade = false;
			trail.trailThickness = 10 * scale.X;
			trail.trailThicknessIncrease = 0;
			trail.color = delegate (float percent)
			{
				return Color.Lerp(Color.White,Color.Purple*0.25f,1f-(projectile.timeLeft/60f));
			};
			trail.DrawTrail(vectors, projectile.Center);

			Texture2D glowStar = mod.GetTexture("Extra_57b");
			Vector2 glowSize = glowStar.Size();

			Terraria.Utilities.UnifiedRandom random = new Terraria.Utilities.UnifiedRandom(projectile.whoAmI);

			float alphaIK = MathHelper.Clamp(projectile.timeLeft / 32f, 0f, 1f);

			for (float ff = 1f; ff > 0.25f; ff -= 0.05f)
			{
				float explodeSize = ((2f - ff)* scale.X)*0.60f;
				Color color = Color.Lerp(Color.Blue, Color.MediumPurple, alphaIK);
				float rot = random.NextFloat(0.05f, 0.15f) * (random.NextBool() ? 1f : -1f) * (Main.GlobalTime * 8f);
				spriteBatch.Draw(glowStar, projectile.Center - Main.screenPosition, null, color * alphaIK * 0.75f, random.NextFloat(MathHelper.TwoPi) + rot, glowSize / 2f, (new Vector2(random.NextFloat(0.15f, 0.50f), 0.5f) + new Vector2(ff, ff))* explodeSize, SpriteEffects.None, 0);
			}

			return false;
		}

	}


	public class BigDakka : SeriousSamWeapon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 0.05f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Big Dakka");
			Tooltip.SetDefault("Rapidly fires 2 pairs of bullets from the smaller chambers\nContinue holding to charge up the central barrel to fire a homing shot that unleashes fiery death!\n75% to not consume ammo per bullet fired");
		}

		public override void SetDefaults()
		{
			item.damage = 75;
			item.ranged = true;
			item.width = 32;
			item.height = 62;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 2;
			item.value = Item.sellPrice(0, 75, 0, 0);
			item.rare = 10;
			//item.UseSound = SoundID.Item99;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 50f;
			item.channel = true;
			item.reuseDelay = 5;
			item.useAmmo = AmmoID.Bullet;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/BigDakka_Glow");
				item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -26;
			}
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Technical/BigDakka"); }
		}

		public override bool ConsumeAmmo(Player player)
		{
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-26, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SnowmanCannon, 1);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 12);
			recipe.AddIngredient(mod.ItemType("MoneySign"), 8);
			recipe.AddIngredient(mod.ItemType("FieryShard"), 8);
			recipe.AddIngredient(mod.ItemType("Entrophite"), 50);
			recipe.AddIngredient(mod.ItemType("StygianCore"), 2);
			recipe.AddIngredient(mod.ItemType("CalamityRune"), 3);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float speed = 1.5f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(0);
			//Main.NewText(ammotype);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[mod.ProjectileType("BigDakkaCharging")] < 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("BigDakkaCharging"), damage, knockBack, player.whoAmI);
				Main.projectile[proj].ai[1] = type;
				Main.projectile[proj].netUpdate = true;
			}
			return false;
		}

	}

	public class BigDakkaCharging : ModProjectile
	{
		int thestart;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Big Dakka Charging");
		}

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/WaveProjectile"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ranged = true;
			projectile.timeLeft = 600;
			aiType = 0;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.localAI[0]);
			writer.Write(projectile.localAI[1]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.localAI[0] = reader.ReadInt32();
			projectile.localAI[1] = reader.ReadInt32();
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			int DustID2;
			if (projectile.ai[0] < 150)
			{
				DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), projectile.velocity.X * 1.2f, projectile.velocity.Y * 1.2f, 20, default(Color), 1f);
				Main.dust[DustID2].noGravity = true;
			}
			else
			{
				if (projectile.ai[0] == 150)
				{
					for (float new1 = 0.5f; new1 < 2f; new1 = new1 + 0.05f)
					{
						float angle2 = MathHelper.ToRadians(Main.rand.Next(150, 210));
						Vector2 angg2 = projectile.velocity.RotatedBy(angle2);
						DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), 0, 0, 20, default(Color), 2f);
						Main.dust[DustID2].velocity = new Vector2(angg2.X, angg2.Y) * new1;
						Main.dust[DustID2].noGravity = true;
						Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 117, 0.75f, 0.5f);
					}

				}
				for (float new1 = -1f; new1 < 2f; new1 = new1 + 2f)
				{
					float angle = MathHelper.Pi/1.45f;
					Vector2 angg = projectile.velocity.RotatedBy(angle * new1);
					DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y)-Vector2.Normalize(projectile.velocity) * 3f, projectile.width, projectile.height, mod.DustType("HotDust"), 0, 0, 20, default(Color), 1f);
					Main.dust[DustID2].velocity = new Vector2(angg.X * 2f, angg.Y * 2f);
					Main.dust[DustID2].noGravity = true;
				}

				for (float new1 = 0.2f; new1 < 0.8f; new1 = new1 + 0.2f)
				{
					if (Main.rand.Next(0, 10) < 2)
					{
						float angle2 = MathHelper.ToRadians(Main.rand.Next(150, 210));
						Vector2 angg2 = projectile.velocity.RotatedBy(angle2);
						Vector2 posz = new Vector2(projectile.position.X, projectile.position.Y);
						Vector2 norm = projectile.velocity; norm.Normalize();
						posz += norm * -76f;
						posz += norm.RotatedBy(MathHelper.ToRadians(-90 * player.direction)) * 16f;
						DustID2 = Dust.NewDust(posz, projectile.width, projectile.height, mod.DustType("HotDust"), 0, 0, 20, default(Color), 1.5f);
						Main.dust[DustID2].velocity = (new Vector2(angg2.X, angg2.Y - Main.rand.NextFloat(0f, 1f)) * new1);
						Main.dust[DustID2].noGravity = true;
					}
				}

			}

			if (thestart == 0) { thestart = player.itemTime; }
			if (!player.channel || player.dead || projectile.timeLeft < 300)
			{
				projectile.tileCollide = true;
				if (projectile.timeLeft > 6)
				{
					if (projectile.ai[0] > 150)
					{

						int proj = Projectile.NewProjectile(projectile.Center, projectile.velocity, mod.ProjectileType("DakkaShot"), projectile.damage * 6, projectile.knockBack, player.whoAmI);
						Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 116, 0.75f, 0.5f);

					}
					projectile.timeLeft = 6;

				}
			}
			else
			{
				projectile.ai[0] += 1;
				Vector2 mousePos = Main.MouseWorld;

				if (projectile.owner == Main.myPlayer)
				{
					Vector2 diff = mousePos - player.Center;
					diff.Normalize();
					projectile.velocity = diff;
					projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
					projectile.netUpdate = true;
					projectile.Center = mousePos;
				}
				int dir = projectile.direction;
				player.ChangeDir(dir);
				player.itemTime = 40;
				player.itemAnimation = 40;
				player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);

				projectile.Center = player.Center + (projectile.velocity * 56f);
				projectile.velocity *= 8f;



				if (projectile.timeLeft < 600)
				{
					projectile.timeLeft = 600 + thestart;
					player.itemTime = thestart;
					Vector2 perturbedSpeed = projectile.velocity; // Watch out for dividing by 0 if there is only 1 projectile.
					perturbedSpeed *= 1.25f;
					int ammotype = (int)player.GetModPlayer<SGAPlayer>().myammo;
					if (ammotype > 0)
					{
						Item ammo2 = new Item();
						ammo2.SetDefaults(ammotype);
						int ammo = ammo2.shoot;
						int damageproj = projectile.damage;
						float knockbackproj = projectile.knockBack;
						float sppez = perturbedSpeed.Length();
						if (ammo2.modItem != null)
							ammo2.modItem.PickAmmo(player.HeldItem, player, ref ammo, ref sppez, ref projectile.damage, ref projectile.knockBack);
						int type = ammo;


						Vector2 normal = perturbedSpeed; normal.Normalize();
						Vector2 newspeed = normal; newspeed *= sppez;
						for (int num315 = -1; num315 < 2; num315 = num315 + 2)
						{
							if (player.HasItem(ammotype))
							{
								for (int new1 = 0; new1 < 5; new1 = new1 + 3)
								{
									Vector2 newloc = projectile.Center;
									newloc -= normal * 58f;
									newloc += (normal.RotatedBy(MathHelper.ToRadians(90)) * num315) * (10 + new1);
									int proj = Projectile.NewProjectile(newloc.X, newloc.Y, newspeed.X * 1.5f, newspeed.Y * 1.5f, type, damageproj, knockbackproj, player.whoAmI);
									if (Main.rand.Next(100) < 75 && (ammo2.modItem != null && ammo2.modItem.ConsumeAmmo(player)) && ammo2.maxStack > 1)
										player.ConsumeItemRespectInfiniteAmmoTypes(ammotype);
								}

							}
						}
						Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 38, 0.5f, 0.5f);
					}

				}
			}
		}
	}

	public class DakkaShot : UnmanedArrow
	{
		protected override float homing => 0.1f;
		protected override float gravity => 0f;
		protected override float maxhoming => 10000f;
		protected override float homingdist => 2000f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dakka Shot");
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/WaveProjectile"); }
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 800;
			projectile.extraUpdates = 5;
			projectile.tileCollide = false;
			aiType = ProjectileID.WoodenArrowFriendly;
		}

		public override bool PreKill(int timeLeft)
		{
			return false;
		}

		public override void effects(int type)
		{
			if (type == 0)
			{
				int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 235, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 20, default(Color), 1.5f);
				Main.dust[DustID2].noGravity = true;

				if (projectile.ai[0] % 30 == 0)
				{
					Main.PlaySound(SoundID.Item45, projectile.Center);
					int numProj = 2;
					//float rotation = MathHelper.ToRadians(1);
					for (int i = 0; i < numProj; i++)
					{
						//Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
						int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("BoulderBlast"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
						if (proj >= 0)
						{
							Main.projectile[proj].ranged = true;
							Main.projectile[proj].netUpdate = true;
						}
					}

				}

			}

		}


	}

}
