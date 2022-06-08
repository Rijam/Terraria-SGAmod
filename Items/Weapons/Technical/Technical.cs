using System;
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
using Terraria.Audio;

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
			Item.damage = 25;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 42;
			Item.height = 16;
			Item.useTime = 3;
			Item.useAnimation = 3;
			Item.useStyle = 5;
			Item.reuseDelay = 0;
			Item.noMelee = true;
			Item.knockBack = 1;
			Item.value = 750000;
			Item.rare = 8;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 40f;
			Item.useAmmo = AmmoID.Bullet;
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
				Item.reuseDelay = 15;
				player.itemTime = 20;
				firemode += 1;
				firemode %= 2;
				SoundEngine.PlaySound(40, player.Center);
				if (Main.myPlayer == player.whoAmI)
				Main.NewText("Toggled: " + things[firemode] + " mode");
			}
			else
			{
				Item.reuseDelay = 0;
				Item.useTime = 3;
				Item.useAnimation = 3;
				if (firemode == 1)
				{
					Item.useTime = 2;
					Item.useAnimation = 12;
					Item.reuseDelay = 25;
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

			SoundEngine.PlaySound(SoundID.Item41, player.Center);

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
				Main.projectile[proj].knockBack = Item.knockBack;
				player.itemRotation = Main.projectile[proj].velocity.ToRotation();
				if (player.direction < 0)
					player.itemRotation += (float)Math.PI;
			}
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.IllegalGunParts, 1).AddIngredient(mod.ItemType("PrismalBar"), 8).AddIngredient(ItemID.ChainGun, 1).AddIngredient(ItemID.ClockworkAssaultRifle, 1).AddIngredient(ItemID.Gatligator, 1).AddIngredient(mod.ItemType("AdvancedPlating"), 15).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
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
			Item.damage = 27;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 56;
			Item.height = 28;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.value = 500000;
			Item.rare = 7;
			Item.UseSound = SoundID.Item38;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -4);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.OnyxBlaster, 1).AddIngredient(ItemID.TacticalShotgun, 1).AddIngredient(null, "SharkTooth", 50).AddIngredient(mod.ItemType("AdvancedPlating"), 10).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
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

			Item.damage = 80;
			Item.width = 48;
			Item.height = 48;
			Item.DamageType = DamageClass.Melee;
			Item.rare = ItemRarityID.Lime;
			Item.value = 400000;
			Item.useStyle = 1;
			Item.useAnimation = 35;
			Item.useTime = 35;
			Item.knockBack = 8;
			Item.autoReuse = true;
			Item.consumable = false;
			Item.useTurn = false;
			Item.UseSound = SoundID.Item1;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/CircuitBreakerBlade_Glow").Value;
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
					SoundEngine.PlaySound(SoundID.Item93, position);
					}

				}
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("TeslaStaff"), 1).AddIngredient(ItemID.BreakerBlade, 1).AddIngredient(ItemID.HallowedBar, 10).AddIngredient(ItemID.Cog, 50).AddIngredient(mod.ItemType("ManaBattery"), 2).AddIngredient(mod.ItemType("AdvancedPlating"), 10).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}

	}

	public class CBreakerBolt : ModProjectile
	{
		bool transboost = false;
		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 2;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 120;
			Projectile.light = 0.1f;
			Projectile.extraUpdates = 120;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			AIType = -1;
			Main.projFrames[Projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(Projectile.localAI[0]);
			writer.Write(Projectile.localAI[1]);
			writer.Write(Projectile.minion);
			writer.Write(Projectile.melee);
			writer.Write(Projectile.magic);
			writer.Write(Projectile.usesLocalNPCImmunity);
			writer.Write(Projectile.localNPCHitCooldown);
			writer.Write(transboost);

		}

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			Projectile.localAI[0] = reader.ReadInt32();
			Projectile.localAI[1] = reader.ReadInt32();
			Projectile.minion = reader.ReadBoolean();
			Projectile.DamageType = reader.ReadBoolean();
			Projectile.DamageType = reader.ReadBoolean();
			Projectile.usesLocalNPCImmunity = reader.ReadBoolean();
			Projectile.localNPCHitCooldown = reader.ReadInt32();
			transboost = reader.ReadBoolean();
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Breaker Bolt");
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ignoreWater = true;
			Projectile.Kill();
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.ai[0] > 0)
			{
				Projectile.ai[0] -= 1;
				NPC target2 = CircuitBreakerBlade.FindClosestTarget(Main.player[Projectile.owner], Projectile.Center, new Vector2(0, 0));
				if (target2 != null)
				{
					Vector2 Speed = (target2.Center - target.Center);
					Speed.Normalize(); Speed *= 2f;
					int prog = Projectile.NewProjectile(target.Center.X, target.Center.Y, Speed.X, Speed.Y, ModContent.ProjectileType<CBreakerBolt>(),Projectile.damage, Projectile.knockBack / 2f, Projectile.owner, Projectile.ai[0]);
					Main.projectile[prog].DamageType = projectile.melee;
					Main.projectile[prog].DamageType = projectile.magic;
					Main.projectile[prog].aiStyle = -200;

					IdgProjectile.Sync(prog);
					SoundEngine.PlaySound(SoundID.Item93, Projectile.Center);
				}
			}
			if (Projectile.penetrate < 1)
			{
				Projectile.ignoreWater = true;
				Projectile.Kill();
			}
		}

		public override bool PreKill(int timeLeft)
		{
			if (Projectile.ignoreWater) {
				for (int k = 0; k < 10; k++)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= 1f;
					int num655 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 206, Projectile.velocity.X + randomcircle.X * 8f, Projectile.velocity.Y + randomcircle.Y * 8f, 100, new Color(30, 30, 30, 20), 1.5f*(1f+(Projectile.ai[0]/3f)));
					Main.dust[num655].noGravity = true;
					Main.dust[num655].velocity *= 0.5f;
				}
			}


			return true;
		}

		Vector2 basepoint=Vector2.Zero;

		public override void AI()
		{			
			if (Projectile.localAI[1] == 0f)
			{
				Projectile.localAI[1] = Main.rand.NextFloat();
				Projectile.netUpdate = true;
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			}


			Vector2 gothere = Projectile.velocity;
			gothere=gothere.RotatedBy(MathHelper.ToRadians(90));
			gothere.Normalize();
			Player player = Main.player[Projectile.owner];

			if (basepoint == Vector2.Zero)
			{
				SGAPlayer sgaply = player.SGAPly();
				if (sgaply.transformerAccessory && !transboost && Projectile.aiStyle>-100)
				{
					Projectile.aiStyle = -150;
					Projectile.ai[0] += 1;
					transboost = true;
				}

				basepoint = Projectile.Center;
				Projectile.localAI[1] = (float)player.SGAPly().timer;
			}
			else
			{
				basepoint += Projectile.velocity;
			}

			float theammount = ((float)Projectile.timeLeft + (float)(Projectile.whoAmI*6454f)+(Projectile.localAI[1]* 72.454f));
			float scale = (1f - Projectile.ai[1]);


			Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= 0.1f;
			int num655 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 206, Projectile.velocity.X + randomcircle.X * 8f, Projectile.velocity.Y + randomcircle.Y * 8f, 100, new Color(30, 30, 30, 20), 1f * (1f + (Projectile.ai[0] / 3f)));
			Main.dust[num655].noGravity = true;
			Main.dust[num655].velocity *= 0.5f;


			Projectile.Center += ((gothere * ((float)Math.Sin((double)theammount / 7.10) * (1.97f * scale)))+ (gothere * ((float)Math.Cos((double)theammount / -13.00) * (2.95f * scale)))+ (gothere * ((float)Math.Sin((double)theammount / 4.34566334) * (2.1221f * scale)))
				*(1f - Projectile.localAI[0]));


		}
	}

	public class TeslaStaff : SeriousSamWeapon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 0.01f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tesla Staff");
			Tooltip.SetDefault("Zaps nearby enemies with a shock of electricity that is able to pierce twice\nRequires 50 Electric Charge to discharge bolts");
			Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 2;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 4;
			Item.useAnimation = 4;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 0;
			Item.value = 75000;
			Item.rare = 3;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("UnmanedBolt").Type;
			Item.shootSpeed = 4f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/TeslaStaff_Glow").Value;
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
					// Main.projectile[prog].melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
					Main.projectile[prog].DamageType = DamageClass.Magic;
					Main.projectile[prog].usesLocalNPCImmunity = false;
					IdgProjectile.Sync(prog);
					SoundEngine.PlaySound(SoundID.Item93, position);
				}
			}

			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Wire, 50).AddIngredient(mod.ItemType("UnmanedStaff"), 1).AddIngredient(mod.ItemType("ManaBattery"), 1).AddIngredient(mod.ItemType("AdvancedPlating"), 6).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
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
			Item.damage = 250;
			Item.DamageType = DamageClass.Magic;
			Item.width = 56;
			Item.height = 28;
			Item.useTime = 90;
			Item.useAnimation = 90;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(platinum: 1);
			Item.rare = 11;
			Item.UseSound = SoundID.Item122;
			Item.autoReuse = false;
			Item.shoot = 14;
			Item.mana = 100;
			Item.shootSpeed = 200f;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/Massacre_Glow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -18;
			}
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -0);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "PrismalLauncher", 1).AddIngredient(null, "QuasarCannon", 1).AddIngredient(ItemID.ProximityMineLauncher, 1).AddIngredient(ItemID.Stynger, 1).AddIngredient(ItemID.FragmentStardust, 10).AddIngredient(mod.ItemType("ManaBattery"), 4).AddIngredient(mod.ItemType("PrismalBar"), 10).AddIngredient(mod.ItemType("LunarRoyalGel"), 15).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}

		public override bool CanUseItem(Player player)
		{
			if (player.statLife <= 50)
			{
				if (Main.netMode < NetmodeID.Server)
				{
					CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.OrangeRed, "Insufficient Health", false, true);
					SoundEngine.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 93, 0.75f, 0.5f);
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
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 10;
			Projectile.light = 0.1f;
			Projectile.extraUpdates = 0;
			Projectile.tileCollide = false;
			AIType = -1;
			Main.projFrames[Projectile.type] = 1;
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
			Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(25));
			Vector2 vex = Main.rand.NextVector2Circular(160, 160);
			int prog = Projectile.NewProjectile(Projectile.Center.X+ vex.X, Projectile.Center.Y+ vex.Y, 0,0, ProjectileID.StardustGuardianExplosion, Projectile.damage, Projectile.knockBack, Projectile.owner,0f,8f);
			Main.projectile[prog].scale = 3f;
			Main.projectile[prog].usesLocalNPCImmunity = true;
			Main.projectile[prog].localNPCHitCooldown = -1;
			Main.projectile[prog].DamageType = DamageClass.Magic;
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
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Magic;
			Item.width = 56;
			Item.height = 28;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 18;
			Item.value = 75000;
			Item.rare = 4;
			Item.UseSound = SoundID.Item100;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("ROEproj").Type;
			Item.shootSpeed = 16f;
			Item.mana = 15;
			Item.channel = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/RodofEnforcement_Glow").Value;
			}
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -4);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.MeteoriteBar, 8).AddIngredient(ItemID.Actuator, 10).AddIngredient(mod.ItemType("ManaBattery"), 1).AddIngredient(mod.ItemType("AdvancedPlating"), 10).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
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
			AIType = ProjectileID.Boulder;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 10;
			Projectile.light = 0.5f;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.ai[0]<1)
			return false;
			return null;
		}

		public override bool PreKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);

			return true;
		}

		public override void AI()
		{

			//int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("AcidDust"), projectile.velocity.X * 1f, projectile.velocity.Y * 1f, 20, default(Color), 1f);

			bool cond = Projectile.ai[1] < 1 || Projectile.ai[0] == 2 || Projectile.timeLeft == 2;
			for (int num621 = 0; num621 < (cond ? 30 : 1); num621++)
			{
				int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 226, Projectile.velocity.X * (cond ? 1.5f : 0.5f), Projectile.velocity.Y * (cond ? 1.5f : 0.5f), 20, default(Color), 1f);
				Main.dust[num622].velocity *= 1f;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
				Main.dust[num622].noGravity = true;
			}


			Player player = Main.player[Projectile.owner];
			Projectile.ai[1] += 1;
			if (player.dead)
			{
				Projectile.Kill();
			}
			else
			{
				if (((Projectile.ai[0] > 0 || player.statMana < 1) || !player.channel) && Projectile.ai[1]>1)
				{
					Projectile.ai[0] += 1;
					if (Projectile.ai[0] == 1)
					{
						SoundEngine.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 113, 0.5f, -0.25f);

					}


					if (Projectile.ai[0] == 4)
					{

					}
					int dir2 = Projectile.direction;
					Vector2 distz = Projectile.Center - player.Center;
					player.itemRotation = (float)Math.Atan2(distz.Y * dir2, distz.X * dir2);
				}
				else
				{
					if (Projectile.ai[0] < 1)
					{
						Vector2 mousePos = Main.MouseWorld;
						if (Projectile.owner == Main.myPlayer && Projectile.ai[1] < 2)
						{
							Projectile.Center = mousePos;
							Projectile.netUpdate = true;
						}
						if (Projectile.owner == Main.myPlayer && mousePos!= Projectile.Center)
						{
							Vector2 diff2 = mousePos - Projectile.Center;
							diff2.Normalize();
							Projectile.velocity = diff2 * 20f;
							Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
							Projectile.netUpdate = true;
							//projectile.Center = mousePos;

						}

						Projectile.timeLeft = 40;
						Projectile.position -= Projectile.velocity;


						//projectile.position -= projectile.Center;
						int dir = Projectile.direction;
						player.ChangeDir(dir);
						player.itemTime = 40;
						player.itemAnimation = 38;

						Vector2 distz = Projectile.Center - player.Center;
						player.itemRotation = (float)Math.Atan2(distz.Y * dir, distz.X * dir);


						//projectile.Center = (player.Center + projectile.velocity * 26f) + new Vector2(0, -24);
					}
				}
			}
		}
	}

	public class BeamCannon : SeriousSamWeapon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 0.25f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beam Cannon");
			Tooltip.SetDefault("Fires discharged bolts of piercing plasma\nThe less mana you have, the more your bolts fork out from where you aim\nAlt fire drains Plasma Cells to fire a wide spread of bolts");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(7, 4));
			SGAmod.UsesPlasma.Add(SGAmod.Instance.Find<ModItem>("BeamCannon").Type, 1000);
		}

		public override void SetDefaults()
		{
			Item.damage = 150;
			Item.DamageType = DamageClass.Magic;	
			Item.crit = 15;
			Item.width = 56;
			Item.height = 28;
			Item.useTime = 7;
			Item.useAnimation = 7;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.knockBack = 1;
			Item.value = 1000000;
			Item.rare = 9;
			Item.UseSound = SoundID.Item115;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("BeamCannonHolding").Type;
			Item.shootSpeed = 16f;
			Item.mana = 8;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("BeamCannonHolding").Type] > 0)
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
			CreateRecipe(1).AddIngredient(null, "StarMetalBar", 16).AddIngredient(null, "PlasmaCell", 3).AddIngredient(null, "ManaBattery", 5).AddIngredient(null, "AdvancedPlating", 8).AddIngredient(ItemID.ChargedBlasterCannon, 1).AddIngredient(ItemID.LunarBar, 8).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
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
				player.CheckMana(Item,40, true);
				player.itemTime *= 5;
				player.itemAnimation *= 5;
				SoundEngine.PlaySound(SoundID.Item, player.Center, 122);
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
					Main.projectile[proj].DamageType = DamageClass.Magic;
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
			int prog = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, offset.X, offset.Y, Mod.Find<ModProjectile>("BeamCannonHolding").Type, damage, knockBack, player.whoAmI);

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
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 3;
			Projectile.penetrate = -1;
			AIType = ProjectileID.WoodenArrowFriendly;
			Projectile.damage = 0;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override void AI()
		{
			Projectile.localAI[0] += 1f;

			Player player = Main.player[Projectile.owner];

			if (player != null && player.active)
			{

				SGAPlayer modply = player.GetModPlayer<SGAPlayer>();

				if (player.dead)
				{
					Projectile.Kill();
				}
				else
				{


					player.heldProj = Projectile.whoAmI;
					player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * Projectile.direction, Projectile.velocity.X * Projectile.direction);
					Projectile.rotation = player.itemRotation - MathHelper.ToRadians(90);
					Projectile.Center = (player.Center + new Vector2(player.direction * 6, 0)) + (Projectile.velocity * 10f);


					Projectile.position -= Projectile.velocity;
					if (player.itemTime > 0)
					Projectile.timeLeft = 2;
					Vector2 position = Projectile.Center;
					Vector2 offset = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
					offset.Normalize();
					offset *= 16f;

				}
			}
			else
			{
				Projectile.Kill();
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>("SGAmod/Items/Weapons/Technical/BeamCannon");
			int frames = 4;
			//Texture2D texGlow = ModContent.GetTexture("SGAmod/Items/Weapons/SeriousSam/BeamGunProjGlow");
			SpriteEffects effects = SpriteEffects.FlipHorizontally;
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / frames) / 2f;
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
			Color color = Projectile.GetAlpha(lightColor) * 1f; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = (int)(Main.GlobalTimeWrappedHourly*8f);
			timing %= frames;
			timing *= ((tex.Height) / frames);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / frames), color, Projectile.rotation - MathHelper.ToRadians(90*Projectile.direction), drawOrigin, Projectile.scale, Projectile.direction < 1 ? effects : (SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally), 0f);
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
			Item.DamageType = DamageClass.Summon;
			Item.sentry = true;
			Item.width = 24;
			Item.height = 30;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.noMelee = true;
			Item.knockBack = 2f;
			Item.noUseGraphic = true;
			Item.value = Item.buyPrice(0, 0, 5, 0);
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = false;
			Item.shootSpeed = 20f;
			Item.consumable = true;
			Item.maxStack = 30;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<ReRouterProjectile>();
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
			CreateRecipe(3).AddIngredient(mod.ItemType("LaserMarker"), 2).AddIngredient(mod.ItemType("NoviteBar"), 3).AddIngredient(mod.ItemType("VialofAcid"), 2).AddIngredient(ItemID.MeteoriteBar, 1).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
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
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.sentry = true;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{

			Player player = Main.player[base.Projectile.owner];
			Projectile.ai[0] += 1;
			Projectile.localAI[0] += 1;

			Projectile.rotation += Projectile.velocity.X / 24f;

			if (Projectile.ai[0] < 30)
            {
				Projectile.velocity.Y += 0.30f;
				return;

            }
			if (Projectile.tileCollide)
			{
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X,(int)Projectile.Center.Y, 78,0.75f,0.75f);
				Projectile.tileCollide = false;
			}
			Projectile.velocity /= 1.25f;

			if (Projectile.ai[0] % 1 == 0)
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

				List<NPC> closestnpcs = SGAUtils.ClosestEnemies(Projectile.Center, 600f, AddedWeight: targetthem);

				NPC target = closestnpcs?[0];//Closest

				if (target != null && target.active && target.life > 0 && Vector2.Distance(target.Center, Projectile.Center) < 600)
				{

					foreach (Projectile proj in Main.projectile)
					{
						bool contact = Main.projPet[proj.type];
						if (proj.active && proj.friendly && proj.damage>0 && player.heldProj != proj.whoAmI && (!contact))
						{
							if (new Rectangle((int)proj.position.X, (int)proj.position.Y, proj.width, proj.height).Intersects(new Rectangle((int)Projectile.position.X-8, (int)Projectile.position.Y-8, Projectile.width+16, Projectile.height+16)))
							{
								SGAprojectile sgaProj = proj.GetGlobalProjectile<SGAprojectile>();
								if (sgaProj.rerouted == false && player.SGAPly().ConsumeElectricCharge(proj.damage, 60))
								{
									Vector2 there = Projectile.Center + new Vector2(0, 0f);
									Vector2 Speed = (target.Center - there);
									Speed.Normalize();

									proj.Center = Projectile.Center;
									proj.velocity = Speed*(proj.velocity.Length());
									proj.damage = (int)(proj.damage * player.SGAPly().techdamage);
									proj.netUpdate = true;
									sgaProj.rerouted = true;

									Projectile.rotation = Speed.ToRotation();
									SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 114, 0.75f, 0.75f);
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
				loc = loc.RotatedBy(Projectile.rotation);
				int num622 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + loc, 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.75f);
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
			Texture2D texa = Main.projectileTexture[Projectile.type];
			spriteBatch.Draw(texa, Projectile.Center - Main.screenPosition, null, lightColor * MathHelper.Clamp(Projectile.localAI[0] / 15f, 0f, 1f), Projectile.rotation, new Vector2(texa.Width, texa.Height) / 2f, new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ai[0] = Math.Max(Projectile.ai[0], 20);

			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}

			Projectile.velocity /= 2f;
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
			Item.damage = 50;
			Item.DamageType = DamageClass.Magic;
			Item.width = 32;
			Item.height = 62;
			Item.useTime = 70;
			Item.useAnimation = 70;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Lime;
			//item.UseSound = SoundID.Item99;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<VocanicBeaterCharging>();
			Item.shootSpeed = 50f;
			Item.noUseGraphic = false;
			Item.channel = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/VolcanicSpaceBlaster_Glow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -6;
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
			CreateRecipe(1).AddIngredient(ItemID.SpaceGun, 1).AddIngredient(mod.ItemType("HeatBeater"), 1).AddIngredient(ItemID.Nanites, 100).AddIngredient(null, "AdvancedPlating", 8).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float rotation = MathHelper.ToRadians(0);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("VocanicBeaterCharging").Type] < 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("VocanicBeaterCharging").Type, damage, knockBack, player.whoAmI);
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
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			AIType = 0;
		}

		public override void ChargeUpEffects()
		{

			if (Projectile.ai[0] < chargeuptime)
			{
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 3) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y) + randomcircle * 20, 0, 0, DustID.Fire, 0f, 0f, 100, default(Color), 0.75f);

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
						int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y), 0, 0, DustID.Fire, 0f, 0f, 100, default(Color), 0.75f);

						Main.dust[num622].scale = 1.5f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = randomcircle.X * 2;
						Main.dust[num622].velocity.Y = randomcircle.Y * 2;
						Main.dust[num622].alpha = 100;
					}
				}
			}


			if (Projectile.ai[0] == chargeuptime)
			{
				for (int num315 = 0; num315 < 35; num315 = num315 + 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y), 0, 0, DustID.Fire, 0f, 0f, 100, default(Color), 0.5f);

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
			float perc = MathHelper.Clamp(Projectile.ai[0] / (float)chargeuptime, 0f, 1f);

			float speed = 8f + perc * 4f;

			Vector2 perturbedSpeed = (new Vector2(direction.X, direction.Y) * speed); // Watch out for dividing by 0 if there is only 1 projectile.

			Projectile.Center += Projectile.velocity;

			int damage = Projectile.damage;

			int type = ProjectileID.LaserMachinegunLaser;

			if (Projectile.ai[0] >= chargeuptime)
			{
				type = ModContent.ProjectileType<VolcanicShot>();
				Vector2 center = Projectile.Center;
				int prog = Projectile.NewProjectile(center.X, center.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage*8, 8f, player.whoAmI);
				IdgProjectile.AddOnHitBuff(prog, ModContent.BuffType<LavaBurn>(), (int)(120 + (perc * 180f)));

				SoundEngine.PlaySound(SoundID.Item73, player.Center);
				player.velocity -= perturbedSpeed;

					for (int num315 = 0; num315 < 35; num315 = num315 + 1)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y), 0, 0, DustID.Fire, 0f, 0f, 100, default(Color), 0.5f);

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
					Vector2 center = Projectile.Center + ((new Vector2(1f, 0f) * i * 8f).RotatedBy(perturbedSpeed.ToRotation() + MathHelper.PiOver2));
					int prog = Projectile.NewProjectile(center.X, center.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, (int)(2f + perc * 4f), player.whoAmI);
					Main.projectile[prog].usesLocalNPCImmunity = true;
					Main.projectile[prog].localNPCHitCooldown = -1;
					Main.projectile[prog].penetrate = 3;
					Main.projectile[prog].netUpdate = true;
					//IdgProjectile.Sync(prog);
				}
				SoundEngine.PlaySound(SoundID.Item91, player.Center);
			}

			Projectile.Kill();
		}

	}

	public class VolcanicShot : FieryRock
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Volcanic Shot");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			AIType = ProjectileID.Boulder;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 120;
			Projectile.timeLeft = 600;
			Projectile.width = 24;
			Projectile.extraUpdates = 1;
			Projectile.height = 24;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = true;
		}

        public override bool CanDamage()
        {
            return Projectile.timeLeft > 60;
        }

        public override string Texture => "SGAmod/Projectiles/FieryRock";

		public override bool PreKill(int timeLeft)
		{
			return true;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center / 16f, (Color.Orange * 0.25f).ToVector3());
			if (Projectile.penetrate < 100 && Projectile.timeLeft > 60)
				Projectile.timeLeft = 60;
			int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 20, default(Color), 1f);
			Main.dust[DustID2].noGravity = true;

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			for (int i = 0; i < Projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
			{
				if (Projectile.oldPos[i] == default)
					Projectile.oldPos[i] = Projectile.position;
			}

			Texture2D tex = Main.projectileTexture[Projectile.type];

			TrailHelper trail = new TrailHelper("DefaultPass", Mod.Assets.Request<Texture2D>("noise").Value);
			trail.color = delegate (float percent)
			{
				return Color.Orange;
			};
			trail.projsize = tex.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -1f);
			trail.trailThickness = 4;
			trail.trailThicknessIncrease = 6;
			trail.strength = MathHelper.Clamp((Projectile.timeLeft-20f)/60f,0f,1f);
			trail.DrawTrail(Projectile.oldPos.ToList(), Projectile.Center);

			Vector2 offset = tex.Size() / 2f;

			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Orange * Math.Min(Projectile.timeLeft / 20f, 1f), Projectile.rotation, offset, Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

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
			Item.damage = 18;
			Item.DamageType = DamageClass.Magic;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.knockBack = 1;
			Item.value = Item.buyPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Pink;
			//item.UseSound = SoundID.Item99;
			Item.autoReuse = true;
			Item.shootSpeed = 50f;
			Item.noUseGraphic = false;
			Item.channel = true;
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().ConsumeElectricCharge(150, 0, consume: false);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<NoviteBlaster>(), 1).AddIngredient(ItemID.MeteoriteBar, 8).AddIngredient(ModContent.ItemType<WraithFragment4>(), 15).AddTile(TileID.MythrilAnvil).Register();
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
		public override int FireCount => 1 + (int)(Projectile.ai[0] / 20f);

		public override (float, float) AimSpeed => (0.20f, 0.03f);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sirian Gun Charging");
		}

		public override void ChargeUpEffects()
		{

			if (Projectile.ai[0] < chargeuptime)
			{
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 5) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y) + randomcircle * 20, 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.75f);

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
						int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y), 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.75f);

						Main.dust[num622].scale = 1.5f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = randomcircle.X * 2;
						Main.dust[num622].velocity.Y = randomcircle.Y * 2;
						Main.dust[num622].alpha = 100;
					}
				}
			}


			if (Projectile.ai[0] == chargeuptime)
			{
				for (int num315 = 0; num315 < 15; num315 = num315 + 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y), 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.5f);

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

			float speed = 1f + (perc2 * velocity)* (Projectile.ai[0]/(float)chargeuptime);

			//projectile.Center += projectile.velocity;

			for (int i = 0; i < 5; i += 1)
			{
				Vector2 perturbedSpeed = (new Vector2(direction.X, direction.Y) * speed).RotatedBy((Main.rand.NextFloat(-perc, perc)) * 0.75f); // Watch out for dividing by 0 if there is only 1 projectile.
				int prog = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<CBreakerBolt>(), Projectile.damage, Projectile.knockBack, player.whoAmI, 0, 0.10f);
				Main.projectile[prog].localAI[0] = (perc * 0.90f);
				//Main.projectile[prog].localAI[1] = i;
				Main.projectile[prog].DamageType = DamageClass.Magic;
				// Main.projectile[prog].melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
				Main.projectile[prog].netUpdate = true;
			}

			SoundEngine.PlaySound(SoundID.Item91, player.Center);

			if (firedCount >= FireCount)
				Projectile.Kill();
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
			Item.damage = 60;
			Item.DamageType = DamageClass.Ranged;
			Item.crit = 12;
			Item.useAmmo = AmmoID.Gel;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.knockBack = 1;
			Item.value = Item.buyPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Yellow;
			//item.UseSound = SoundID.Item99;
			Item.autoReuse = true;
			Item.shootSpeed = 10f;
			Item.noUseGraphic = false;
			Item.channel = true;
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
			CreateRecipe(1).AddIngredient(ModContent.ItemType<XOPFlamethrower>(), 1).AddIngredient(ModContent.ItemType<OverseenCrystal>(), 20).AddIngredient(ModContent.ItemType<PrismalBar>(), 12).AddIngredient(ModContent.ItemType<WraithFragment4>(), 25).AddTile(TileID.MythrilAnvil).Register();
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
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8;
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
			Projectile.localAI[1] += 0.05f;
			//Main.PlaySound(SoundID.Item91, player.Center);

			if (firedCount >= FireCount)
				Projectile.Kill();
		}

		public override bool DoChargeUp()
		{

			if (!player.SGAPly().ConsumeElectricCharge(3, 25))
				return false;

			Item item = new Item();
			item.SetDefaults(ModContent.ItemType<Starduster>());

			int projType = item.shoot;

			int damage = Projectile.damage;
			float speed = Projectile.velocity.Length();
			float kb = Projectile.knockBack;

			bool tr = false;
			player.PickAmmo(player.HeldItem, ref projType, ref speed, ref tr, ref damage, ref kb, false);

			if (tr)
			{

				if ((int)(Projectile.localAI[0]) % 30 == 0)
				{

					var snd = SoundEngine.PlaySound(SoundID.DD2_BookStaffCast, Projectile.Center);
					if (snd != null)
					{
						snd.Pitch = -0.75f;
					}
				}

				float chargePercent = MathHelper.Clamp((Projectile.ai[0] / 60f), 0f, 1f);

				for (int i = 0; i < 2; i++)
				{
						float Velocity = chargePercent * (Main.rand.NextFloat(6f, 12f)) * (1f + (Projectile.localAI[1] * 3f)) * 1.25f;
						float randomAngle = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi) / (10f + (chargePercent * 10f));
						StardusterProjectile starduster = new StardusterProjectile(Projectile.Center + Vector2.Normalize(Projectile.velocity) * 32f, Vector2.Normalize(Projectile.velocity).RotatedBy(randomAngle) * Velocity);
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
			float overallAlpha = (1f - Math.Min(Projectile.localAI[1], 1f)) * Math.Min(Projectile.localAI[0] / 120f, 1f);
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
			float overallAlpha = (1f - Math.Min(Projectile.localAI[1], 1f))*Math.Min(Projectile.localAI[0]/120f,1f);

			Texture2D glow = ModContent.Request<Texture2D>("SGAmod/Glow");
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
			SGAmod.UsesPlasma.Add(SGAmod.Instance.Find<ModItem>("PlasmaGun").Type, 1000);
		}

		public override void SetDefaults()
		{
			Item.damage = 7500;
			Item.DamageType = DamageClass.Magic;
			Item.width = 32;
			Item.height = 62;
			Item.useTime = 70;
			Item.useAnimation = 70;
			Item.useStyle = 5;
			Item.crit = 25;
			Item.noMelee = true;
			Item.knockBack = 25;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			//item.UseSound = SoundID.Item99;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<PlasmaGunCharging>();
			Item.shootSpeed = 32f;
			Item.noUseGraphic = false;
			Item.channel = true;
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
			CreateRecipe(1).AddIngredient(mod.ItemType("VolcanicSpaceBlaster"), 1).AddIngredient(ItemID.SniperRifle, 1).AddIngredient(mod.ItemType("PrismalBar"), 6).AddIngredient(mod.ItemType("StarMetalBar"), 16).AddIngredient(mod.ItemType("PlasmaCell"), 3).AddIngredient(mod.ItemType("AdvancedPlating"), 5).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float rotation = MathHelper.ToRadians(0);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[Item.shoot] < 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, Item.shoot, damage, knockBack, player.whoAmI);
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
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			AIType = 0;
		}

		public override void ChargeUpEffects()
		{
			chargeUpTimer += 1;

			if (Projectile.ai[0] < chargeuptime)
			{
				if (chargeUpTimer % 4 == 0)
				{
					float perc = MathHelper.Clamp(Projectile.ai[0] / (float)chargeuptime, 0f, 1f);
					SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, (int)Projectile.Center.X, (int)Projectile.Center.Y);
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
						int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y) + randomcircle * 20, 0, 0, DustID.AncientLight, 0f, 0f, 100, Color.Magenta, 0.75f);

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
						int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y), 0, 0, DustID.AncientLight, 0f, 0f, 100, Color.Magenta, 0.75f);

						Main.dust[num622].scale = 1.5f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = randomcircle.X * 2;
						Main.dust[num622].velocity.Y = randomcircle.Y * 2;
						Main.dust[num622].alpha = 100;
					}
				}
			}


			if (Projectile.ai[0] == chargeuptime)
			{
				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, (int)Projectile.Center.X, (int)Projectile.Center.Y);
				if (sound != null)
				{
					sound.Pitch -= 0.5f;
				}

				for (int num315 = 0; num315 < 35; num315 = num315 + 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y), 0, 0, DustID.AncientLight, 0f, 0f, 100, Color.Magenta, 0.5f);

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
			float perc = MathHelper.Clamp(Projectile.ai[0] / (float)chargeuptime, 0f, 1f);

			float speed = 4f+ perc*4f;

			Vector2 perturbedSpeed = (new Vector2(direction.X, direction.Y) * speed); // Watch out for dividing by 0 if there is only 1 projectile.

			Projectile.Center += Projectile.velocity;

			int damage = (int)(Projectile.damage*(Projectile.ai[0]/ chargeuptime));

			if (Projectile.ai[0] >= chargeuptime || player.SGAPly().plasmaLeftInClip<1)
			{
				int type = ModContent.ProjectileType<PlasmaBeam>();
				Vector2 center = Projectile.Center-Vector2.Normalize(perturbedSpeed)*32f;
				int prog = Projectile.NewProjectile(center.X, center.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, Projectile.knockBack, player.whoAmI);
				//if (projectile.ai[0] >= chargeuptime)
				//IdgProjectile.AddOnHitBuff(prog, ModContent.BuffType<LavaBurn>(), (int)(120 + (perc * 300f)));

				player.velocity -= perturbedSpeed;

				SGAmod.AddScreenShake(42f,600,player.Center);

				SoundEngine.PlaySound(SoundID.Item73, player.Center);
				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, (int)center.X, (int)center.Y);
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
			Projectile.Kill();
		}

	}

	public class PlasmaBeam : NPCs.Hellion.HellionBeam
    {
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 60;
			Projectile.damage = 15;
			Projectile.width = 64;
			Projectile.height = 64;
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
			if (Projectile.ai[1] < 2)
				return true;
			return false;
		}

		public override void AI()
		{

			scale2 = Math.Min(scale2 + 0.5f, 1.5f);

			Projectile.ai[1] += 1;

			if (Projectile.ai[1] < 40 && Projectile.ai[1]%6==0)
			{
				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_LightningBugZap, (int)Projectile.Center.X, (int)Projectile.Center.Y);
				if (sound != null)
				{
					sound.Pitch = -1f+(Projectile.timeLeft / 60f)*0.80f;
					sound.Volume = Projectile.timeLeft/60f;
				}
			}

			if (Projectile.ai[1] == 1)
			{

				for (int num315 = 0; num315 < 2200f; num315 = num315 + 4)
				{
					Vector2 offset2 = Vector2.Normalize(Projectile.velocity);
					int num622 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y)+ (offset2 * num315), 0, 0, DustID.Shadowflame, 0f, 0f, 100, Color.Magenta, 0.5f);
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

			Vector2 offset = Vector2.Normalize(Projectile.velocity);
			Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y) + (offset * Main.rand.NextFloat(MaxDistance))/16,(Color.Magenta*0.50f).ToVector3());

			Projectile.localAI[0] += 0.2f;
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
			Vector2 scale = new Vector2(MathHelper.Clamp(Projectile.timeLeft / 8f, 0f, 1f) * scale2, 1f);

			List<Vector2> vectors = new List<Vector2>();
			vectors.Add(Projectile.Center);
			vectors.Add(hitspot);

			TrailHelper trail = new TrailHelper("FadedBasicEffectPass", Main.extraTexture[21]);
			trail.projsize = Vector2.Zero;
			trail.coordOffset = new Vector2(0, Projectile.localAI[0] * -0.22f);
			trail.coordMultiplier = new Vector2(1f, 120f);
			trail.doFade = false;
			trail.trailThickness = 18 * scale.X;
			trail.trailThicknessIncrease = 0;
			trail.color = delegate (float percent)
			{
				return colortex;
			};
			trail.DrawTrail(vectors, Projectile.Center);

			trail = new TrailHelper("FadedBasicEffectPass", Main.extraTexture[21]);
			trail.projsize = Vector2.Zero;
			trail.coordOffset = new Vector2(0, Projectile.localAI[0] * 0.22f);
			trail.coordMultiplier = new Vector2(2f, 180f);
			trail.doFade = false;
			trail.trailThickness = 10 * scale.X;
			trail.trailThicknessIncrease = 0;
			trail.color = delegate (float percent)
			{
				return Color.Lerp(Color.White,Color.Purple*0.25f,1f-(Projectile.timeLeft/60f));
			};
			trail.DrawTrail(vectors, Projectile.Center);

			Texture2D glowStar = Mod.Assets.Request<Texture2D>("Extra_57b").Value;
			Vector2 glowSize = glowStar.Size();

			Terraria.Utilities.UnifiedRandom random = new Terraria.Utilities.UnifiedRandom(Projectile.whoAmI);

			float alphaIK = MathHelper.Clamp(Projectile.timeLeft / 32f, 0f, 1f);

			for (float ff = 1f; ff > 0.25f; ff -= 0.05f)
			{
				float explodeSize = ((2f - ff)* scale.X)*0.60f;
				Color color = Color.Lerp(Color.Blue, Color.MediumPurple, alphaIK);
				float rot = random.NextFloat(0.05f, 0.15f) * (random.NextBool() ? 1f : -1f) * (Main.GlobalTimeWrappedHourly * 8f);
				spriteBatch.Draw(glowStar, Projectile.Center - Main.screenPosition, null, color * alphaIK * 0.75f, random.NextFloat(MathHelper.TwoPi) + rot, glowSize / 2f, (new Vector2(random.NextFloat(0.15f, 0.50f), 0.5f) + new Vector2(ff, ff))* explodeSize, SpriteEffects.None, 0);
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
			Item.damage = 75;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 32;
			Item.height = 62;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 75, 0, 0);
			Item.rare = 10;
			//item.UseSound = SoundID.Item99;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 50f;
			Item.channel = true;
			Item.reuseDelay = 5;
			Item.useAmmo = AmmoID.Bullet;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/BigDakka_Glow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -26;
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
			CreateRecipe(1).AddIngredient(ItemID.SnowmanCannon, 1).AddIngredient(mod.ItemType("AdvancedPlating"), 12).AddIngredient(mod.ItemType("MoneySign"), 8).AddIngredient(mod.ItemType("FieryShard"), 8).AddIngredient(mod.ItemType("Entrophite"), 50).AddIngredient(mod.ItemType("StygianCore"), 2).AddIngredient(mod.ItemType("CalamityRune"), 3).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float speed = 1.5f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(0);
			//Main.NewText(ammotype);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("BigDakkaCharging").Type] < 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("BigDakkaCharging").Type, damage, knockBack, player.whoAmI);
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
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 600;
			AIType = 0;
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
			writer.Write(Projectile.localAI[0]);
			writer.Write(Projectile.localAI[1]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.localAI[0] = reader.ReadInt32();
			Projectile.localAI[1] = reader.ReadInt32();
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			int DustID2;
			if (Projectile.ai[0] < 150)
			{
				DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type, Projectile.velocity.X * 1.2f, Projectile.velocity.Y * 1.2f, 20, default(Color), 1f);
				Main.dust[DustID2].noGravity = true;
			}
			else
			{
				if (Projectile.ai[0] == 150)
				{
					for (float new1 = 0.5f; new1 < 2f; new1 = new1 + 0.05f)
					{
						float angle2 = MathHelper.ToRadians(Main.rand.Next(150, 210));
						Vector2 angg2 = Projectile.velocity.RotatedBy(angle2);
						DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type, 0, 0, 20, default(Color), 2f);
						Main.dust[DustID2].velocity = new Vector2(angg2.X, angg2.Y) * new1;
						Main.dust[DustID2].noGravity = true;
						SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 117, 0.75f, 0.5f);
					}

				}
				for (float new1 = -1f; new1 < 2f; new1 = new1 + 2f)
				{
					float angle = MathHelper.Pi/1.45f;
					Vector2 angg = Projectile.velocity.RotatedBy(angle * new1);
					DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y)-Vector2.Normalize(Projectile.velocity) * 3f, Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type, 0, 0, 20, default(Color), 1f);
					Main.dust[DustID2].velocity = new Vector2(angg.X * 2f, angg.Y * 2f);
					Main.dust[DustID2].noGravity = true;
				}

				for (float new1 = 0.2f; new1 < 0.8f; new1 = new1 + 0.2f)
				{
					if (Main.rand.Next(0, 10) < 2)
					{
						float angle2 = MathHelper.ToRadians(Main.rand.Next(150, 210));
						Vector2 angg2 = Projectile.velocity.RotatedBy(angle2);
						Vector2 posz = new Vector2(Projectile.position.X, Projectile.position.Y);
						Vector2 norm = Projectile.velocity; norm.Normalize();
						posz += norm * -76f;
						posz += norm.RotatedBy(MathHelper.ToRadians(-90 * player.direction)) * 16f;
						DustID2 = Dust.NewDust(posz, Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type, 0, 0, 20, default(Color), 1.5f);
						Main.dust[DustID2].velocity = (new Vector2(angg2.X, angg2.Y - Main.rand.NextFloat(0f, 1f)) * new1);
						Main.dust[DustID2].noGravity = true;
					}
				}

			}

			if (thestart == 0) { thestart = player.itemTime; }
			if (!player.channel || player.dead || Projectile.timeLeft < 300)
			{
				Projectile.tileCollide = true;
				if (Projectile.timeLeft > 6)
				{
					if (Projectile.ai[0] > 150)
					{

						int proj = Projectile.NewProjectile(Projectile.Center, Projectile.velocity, Mod.Find<ModProjectile>("DakkaShot").Type, Projectile.damage * 6, Projectile.knockBack, player.whoAmI);
						SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 116, 0.75f, 0.5f);

					}
					Projectile.timeLeft = 6;

				}
			}
			else
			{
				Projectile.ai[0] += 1;
				Vector2 mousePos = Main.MouseWorld;

				if (Projectile.owner == Main.myPlayer)
				{
					Vector2 diff = mousePos - player.Center;
					diff.Normalize();
					Projectile.velocity = diff;
					Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
					Projectile.netUpdate = true;
					Projectile.Center = mousePos;
				}
				int dir = Projectile.direction;
				player.ChangeDir(dir);
				player.itemTime = 40;
				player.itemAnimation = 40;
				player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir);

				Projectile.Center = player.Center + (Projectile.velocity * 56f);
				Projectile.velocity *= 8f;



				if (Projectile.timeLeft < 600)
				{
					Projectile.timeLeft = 600 + thestart;
					player.itemTime = thestart;
					Vector2 perturbedSpeed = Projectile.velocity; // Watch out for dividing by 0 if there is only 1 projectile.
					perturbedSpeed *= 1.25f;
					int ammotype = (int)player.GetModPlayer<SGAPlayer>().myammo;
					if (ammotype > 0)
					{
						Item ammo2 = new Item();
						ammo2.SetDefaults(ammotype);
						int ammo = ammo2.shoot;
						int damageproj = Projectile.damage;
						float knockbackproj = Projectile.knockBack;
						float sppez = perturbedSpeed.Length();
						if (ammo2.ModItem != null)
							ammo2.ModItem.PickAmmo(player.HeldItem, player, ref ammo, ref sppez, ref Projectile.damage, ref Projectile.knockBack);
						int type = ammo;


						Vector2 normal = perturbedSpeed; normal.Normalize();
						Vector2 newspeed = normal; newspeed *= sppez;
						for (int num315 = -1; num315 < 2; num315 = num315 + 2)
						{
							if (player.HasItem(ammotype))
							{
								for (int new1 = 0; new1 < 5; new1 = new1 + 3)
								{
									Vector2 newloc = Projectile.Center;
									newloc -= normal * 58f;
									newloc += (normal.RotatedBy(MathHelper.ToRadians(90)) * num315) * (10 + new1);
									int proj = Projectile.NewProjectile(newloc.X, newloc.Y, newspeed.X * 1.5f, newspeed.Y * 1.5f, type, damageproj, knockbackproj, player.whoAmI);
									if (Main.rand.Next(100) < 75 && (ammo2.ModItem != null && ammo2.ModItem.ConsumeAmmo(player)) && ammo2.maxStack > 1)
										player.ConsumeItemRespectInfiniteAmmoTypes(ammotype);
								}

							}
						}
						SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 38, 0.5f, 0.5f);
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
			ProjectileID.Sets.Homing[Projectile.type] = true;
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
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 2;
			Projectile.timeLeft = 800;
			Projectile.extraUpdates = 5;
			Projectile.tileCollide = false;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override bool PreKill(int timeLeft)
		{
			return false;
		}

		public override void effects(int type)
		{
			if (type == 0)
			{
				int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 235, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 20, default(Color), 1.5f);
				Main.dust[DustID2].noGravity = true;

				if (Projectile.ai[0] % 30 == 0)
				{
					SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);
					int numProj = 2;
					//float rotation = MathHelper.ToRadians(1);
					for (int i = 0; i < numProj; i++)
					{
						//Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
						int proj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("BoulderBlast").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
						if (proj >= 0)
						{
							Main.projectile[proj].DamageType = DamageClass.Ranged;
							Main.projectile[proj].netUpdate = true;
						}
					}

				}

			}

		}


	}

}
