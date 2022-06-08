using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.Enums;
using SGAmod.Items.Weapons.Trap;
using SGAmod.Projectiles;
using Idglibrary;
using Terraria.Audio;


namespace SGAmod.Items.Weapons.Trap
{

	public class TrapWeapon : ModItem
	{

		public override bool Autoload(ref string name)
		{
			return GetType() != typeof(TrapWeapon) && GetType() != typeof(DefenseTrapWeapon);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{

			if (GetType() != typeof(SuperDartTrapGun) && Item.accessory != true && Item.damage > 0)
			{
				tooltips.RemoveAt(2);
			}

		}

	}

	public class DartTrapGun : TrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dart Trap 'gun'");
			Tooltip.SetDefault("'At least those traps might be of some use in a fight now'" +
				"\nUses Darts as ammo, launches dart trap darts\nTrap Darts Pierce infinitely, but don't crit or count as player damage (they won't activate on damage buffs, for example)");
		}

		public override void SetDefaults()
		{
			Item.damage = 28;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 1;
			Item.value = 100000;
			Item.rare = 4;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.shootSpeed = 9f;
			Item.shoot = ProjectileID.PoisonDart;
			Item.useAmmo = AmmoID.Dart;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Blowgun, 1).AddIngredient(ItemID.DartTrap, 1).AddIngredient(ItemID.IllegalGunParts, 1).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//if (type == ProjectileID.WoodenArrowFriendly)
			//{
			type = ProjectileID.PoisonDartTrap;
			//}
			int probg = Projectile.NewProjectile(position.X + (int)speedX * 4, position.Y + (int)speedY * 4, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].DamageType = DamageClass.Ranged;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			return false;
		}

	}

	public class PortableMakeshiftSpearTrap : DartTrapGun
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Portable 'Makeshift' Spear Trap");
			Tooltip.SetDefault("It's not the same as found in the temple, but it'll do" +
				"\nLaunches piercing spears at close range" + "\nHold attack to stick the spear into a wall and grapple towards it" +
	"\nCounts as trap damage, pierces infinitely, but doesn't crit");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Trap/MakeshiftSpearTrapGun"); }
		}

		public override void SetDefaults()
		{
			Item.damage = 35;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 1;
			Item.value = 100000;
			Item.rare = 4;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.shootSpeed = 12f;
			Item.shoot = Mod.Find<ModProjectile>("TrapSpearGun").Type;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("AdvancedPlating"), 5).AddIngredient(ItemID.DartTrap, 1).AddIngredient(ItemID.Spear, 1).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X + (int)speedX * 4, position.Y + (int)speedY * 4, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].DamageType = DamageClass.Melee;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			return false;
		}

	}

	public class PortableSpearTrapGun : PortableMakeshiftSpearTrap
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Portable Spear Trap");
			Tooltip.SetDefault("'Now we're stabbing'" +
				"\nVery quickly launches piercing spears at medium range" + "\nHold attack to stick the spear into a wall and grapple towards it" +
	"\nCounts as trap damage, pierces infinitely, but doesn't crit");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Trap/PortableSpearTrap"); }
		}


		public override void SetDefaults()
		{
			Item.damage = 120;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 5;
			Item.useAnimation = 5;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 1;
			Item.value = 100000;
			Item.rare = 9;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item11;
			Item.shootSpeed = 10f;
			Item.shoot = Mod.Find<ModProjectile>("TrapSpearGun2").Type;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("PortableMakeshiftSpearTrap"), 1).AddIngredient(ItemID.SpearTrap, 5).AddIngredient(ItemID.LihzahrdPowerCell, 2).AddIngredient(ItemID.LihzahrdBrick, 25).AddIngredient(ItemID.LihzahrdPressurePlate, 1).AddIngredient(mod.ItemType("AdvancedPlating"), 5).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X + (int)speedX * 4, position.Y + (int)speedY * 4, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].DamageType = DamageClass.Melee;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			return false;
		}

	}


	public class SuperDartTrapGun : DartTrapGun
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Dart Trap 'gun'");
			Tooltip.SetDefault("'With this, you can carry their own tech against them'" +
				"\nLaunches Darts at fast speeds!\nConverts Poison Darts into Dart Trap Darts\nTrap Darts Pierce infinitely, but don't crit");
		}

		public override void SetDefaults()
		{
			Item.damage = 85;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 20;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 2;
			Item.value = 100000;
			Item.rare = 9;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item99;
			Item.shootSpeed = 15f;
			Item.shoot = ProjectileID.PoisonDart;
			Item.useAmmo = AmmoID.Dart;
		}
		public override void AddRecipes()
		{
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 4);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			if (type == ProjectileID.PoisonDartBlowgun || type == ProjectileID.PoisonDart)
			{
				type = ProjectileID.PoisonDartTrap;
			}
			//}
			int probg = Projectile.NewProjectile(position.X + (int)speedX * (type == ProjectileID.PoisonDartTrap ? 2 : 0), position.Y + (int)speedY * (type == ProjectileID.PoisonDartTrap ? 2 : 0), speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].DamageType = DamageClass.Ranged;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			IdgProjectile.Sync(probg);
			return false;
		}

	}

	public class FlameTrapThrower : DartTrapGun
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("FlameTrap 'Thrower'");
			Tooltip.SetDefault("'Of course the hottest flames are found within the temple dedicated to the sun'\nSprays fire that remains in place for a couple of seconds" +
				"\nUses Gel as ammo, 50% chance to not consume gel\nPress Alt Fire to spray the flames in a wide arc instead\nCounts as trap damage, pierces infinitely, but doesn't crit");
		}

		public override bool ConsumeAmmo(Player player)
		{
			if (Main.rand.Next(0, 100) <= 50)
				return false;

			return base.ConsumeAmmo(player);
		}

		public override void SetDefaults()
		{
			Item.damage = 60;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 20;
			Item.useTime = 10;
			Item.useAnimation = 20;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 0.25f;
			Item.value = 100000;
			Item.rare = 9;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item34;
			Item.shootSpeed = 10f;
			Item.shoot = ModContent.ProjectileType<TrapFlames>();
			Item.useAmmo = AmmoID.Gel;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.EldMelter, 1).AddIngredient(ItemID.FlameTrap, 1).AddIngredient(mod.ItemType("AdvancedPlating"), 5).AddIngredient(mod.ItemType("CryostalBar"), 5).AddIngredient(ItemID.Nanites, 50).AddIngredient(ItemID.LihzahrdPowerCell, 1).AddIngredient(ItemID.LihzahrdPressurePlate, 1).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 4);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X + (int)(speedX * 2f), position.Y + (int)(speedY * 2f), speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].DamageType = DamageClass.Ranged;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(player.altFunctionUse == 2 ? 60 : 5));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			IdgProjectile.Sync(probg);
			return false;
		}

	}

	public class TrapFlames : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trap Flames");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/JarateShurikens"); }
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.FlamethrowerTrap);
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			// projectile.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Projectile.ai[0] += 1;
			if (Projectile.ai[0] % 12 == 1)
			{
				SoundEngine.PlaySound(SoundID.Item34, Projectile.position);
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile proj = Projectile.NewProjectileDirect(Projectile.position, Projectile.velocity, 188, Projectile.damage, Projectile.knockBack, Projectile.owner);
					proj.friendly = true;
					proj.hostile = false;
					proj.usesIDStaticNPCImmunity = true;
					proj.idStaticNPCHitCooldown = 6;
					proj.netUpdate = true;


				}
			}
			Projectile.position -= Projectile.velocity;


		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}


	}

	class ThrowableBoulderTrap : TrapWeapon
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("'Throwable' Boulder Trap");
			Tooltip.SetDefault("'Rolling Stones from the palm of your hand!'\nCounts as trap damage, Pierce infinitely, but don't crit");
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + ProjectileID.BoulderStaffOfEarth); }
		}

		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.damage = 160;
			Item.shootSpeed = 1f;
			Item.shoot = ProjectileID.Boulder;
			Item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			Item.width = 8;
			Item.height = 28;
			Item.knockBack = 5;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 150;
			Item.useTime = 150;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(0, 0, 0, 50);
			Item.rare = 3;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].Throwing().DamageType = DamageClass.Throwing;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(25));
			//Main.projectile[probg].velocity.X = perturbedSpeed.X * player.thrownVelocity;
			//Main.projectile[probg].velocity.Y = perturbedSpeed.Y * player.thrownVelocity;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			IdgProjectile.Sync(probg);
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(25).AddIngredient(ItemID.Boulder, 25).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}


		public override bool CanUseItem(Player player)
		{
			return true;
		}

	}

	class ThrowableTrapSpikyball : TrapWeapon
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("'Throwable' Trap Spiky Ball");
			Tooltip.SetDefault("Dunno how, but hey, it's pretty neat!\nCounts as trap damage, Pierce infinitely, but don't crit");
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + ProjectileID.SpikyBallTrap); }
		}

		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.damage = 90;
			Item.shootSpeed = 8f;
			Item.shoot = ProjectileID.SpikyBallTrap;
			Item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			Item.width = 8;
			Item.height = 28;
			Item.knockBack = 1;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(0, 0, 1, 0);
			Item.rare = 8;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].Throwing().DamageType = DamageClass.Throwing;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			Main.projectile[probg].netUpdate = true;
			IdgProjectile.Sync(probg);
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(100).AddIngredient(ItemID.LihzahrdBrick, 10).AddIngredient(ItemID.SpikyBall, 100).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}


		public override bool CanUseItem(Player player)
		{
			return true;
		}

	}


	public class TrapSpearGun2 : TrapSpearGun
	{

		public override int stuntime => 4;
		public override float traveldist => 600;
		int fakeid = ProjectileID.SpearTrap;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spear Trap");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.extraUpdates = 2;
		}
	}

	public class TrapSpearGun : ModProjectile
	{

		public virtual int stuntime => 5;
		public virtual float traveldist => 450;
		public int touchedWall = 0;
		int fakeid = ProjectileID.SpearTrap;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spear Trap");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.CloneDefaults(ProjectileID.SpearTrap);
			Projectile.aiStyle = -1;
			//projectile.type = ProjectileID.SpearTrap;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + fakeid; }
		}

		public override bool PreAI()
		{
			Projectile.type = ProjectileID.SpearTrap;

			if (touchedWall > 0 && touchedWall < 2)
			{
				Player basep = Main.player[Projectile.owner];
				if (basep.controlUseItem)
				{
					basep.velocity = Vector2.Normalize(Projectile.Center - basep.Center) * new Vector2(Math.Abs(Projectile.velocity.X), Math.Abs(Projectile.velocity.Y));
				}
				else
				{
					touchedWall = 2;
				}

			}

			return base.PreAI();
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = fakeid;

			return true;
		}

		public override void AI()
		{
			Projectile.type = fakeid;
			Player basep = Main.player[Projectile.owner];
			basep.itemAnimation = stuntime;
			basep.itemTime = stuntime;
			if (basep == null || basep.dead)
			{
				Projectile.Kill();
				return;
			}

			if (Projectile.ai[1] == 0f)
			{
				Projectile.ai[1] = 1f;

			}
			Vector2 anglez = basep.Center - Projectile.Center;
			anglez.Normalize(); anglez *= 5f;
			Projectile.localAI[0] = basep.Center.X - anglez.X * (-1.5f);
			Projectile.localAI[1] = basep.Center.Y - anglez.Y * (-1.5f);

			Vector2 value8 = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
			Projectile.rotation = (basep.Center - value8).ToRotation() - 1.57079637f;
			basep.direction = ((Projectile.Center - basep.Center).X > 0).ToDirectionInt();
			basep.itemRotation = (Projectile.rotation + (float)(Math.PI / 2)) + (basep.direction < 0 ? (float)Math.PI : 0f);
			if (Projectile.ai[0] == 0f)
			{
				if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.velocity *= -1f;
					Projectile.ai[0] += 1f;
					touchedWall = 1;
					return;
				}
				float num384 = Vector2.Distance(Projectile.Center, value8);
				if (num384 > traveldist)
				{
					Projectile.velocity *= -1f;
					Projectile.ai[0] += 1f;
					return;
				}
			}
			else if (Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height) || Vector2.Distance(Projectile.Center, value8) < Projectile.velocity.Length() + 5f)
			{
				Projectile.Kill();
				return;
			}

			if (Projectile.ai[0] > 0)
			{
				float speezx = Projectile.velocity.Length();
				Projectile.velocity = basep.Center - Projectile.Center;
				Projectile.velocity.Normalize();
				Projectile.velocity *= (speezx + 0.15f);

			}

			if (touchedWall == 1)
			{
				Projectile.Center -= Projectile.velocity;
			}

		}

		/*public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 15;
		}*/

	}

	public class SpikeballFlail : TrapWeapon
	{
		public override void SetDefaults()
		{

			Item.width = 30;
			Item.height = 10;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = 3;
			Item.noMelee = true;
			Item.useStyle = 5;
			Item.useAnimation = 20;
			Item.useTime = 44;
			Item.knockBack = 6f;
			Item.damage = 30;
			Item.scale = 1f;
			Item.noUseGraphic = true;
			Item.shoot = Mod.Find<ModProjectile>("SpikeballFlailProj").Type;
			Item.shootSpeed = 14f;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spike Ball Flail");
			Tooltip.SetDefault("At least this... I can buy being made into a weapon" +
				"\nCounts as trap damage, doesn't crit\nEnemies hit by the flail at high speeds may become Gourged; cutting their defense in half");
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Chain, 25).AddIngredient(ItemID.Spike, 25).AddIngredient(ItemID.Hook, 1).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}



	}

	public class SpikeballFlailProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.aiStyle = 15;
			Projectile.trap = true;
			Projectile.scale = 1f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dungeon Spikeball");
		}
		public override string Texture
		{
			get { return ("Terraria/NPC_" + NPCID.SpikeBall); }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Projectile.velocity.Length() > 13)
				target.AddBuff(Mod.Find<ModBuff>("Gourged").Type, 60 * 5);
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.chainTexture;

			Vector2 position = Projectile.Center;
			Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
			Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
			float num1 = (float)texture.Height;
			Vector2 vector2_4 = mountedCenter - position;
			float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
			bool flag = true;
			if (float.IsNaN(position.X) && float.IsNaN(position.Y))
				flag = false;
			if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
				flag = false;
			while (flag)
			{
				if ((double)vector2_4.Length() < (double)num1 + 5.0)
				{
					flag = false;
				}
				else
				{
					Vector2 vector2_1 = vector2_4;
					vector2_1.Normalize();
					position += vector2_1 * num1;
					vector2_4 = mountedCenter - position;
					Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
					color2 = Projectile.GetAlpha(color2);
					Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1.35f, SpriteEffects.None, 0.0f);
				}
			}
			return true;
		}
	}

	public class BookOfJones : TrapWeapon
	{
		public override void SetDefaults()
		{
			Item.damage = 160;
			Item.width = 16;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = 3;
			Item.noMelee = true;
			Item.useStyle = 4;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.knockBack = 10f;
			Item.scale = 1f;
			Item.shoot = Mod.Find<ModProjectile>("JonesBoulderSummon").Type;
			Item.shootSpeed = 14f;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 40;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Book Of Jones");
			Tooltip.SetDefault("'Their turn to go running escaping danger'\nSummons portals above the player that rains down boulders in both directions" +
				"\nCounts as trap damage, doesn't crit");
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 8;// + Main.rand.Next(2);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));
				HalfVector2 half = new HalfVector2(player.Center.X + (i - (numberProjectiles / 2)) * 20, player.Center.Y - 200);
				int prog = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, ai1: ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue));
				Main.projectile[prog].netUpdate = true;
				IdgProjectile.Sync(prog);
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("Landslide"), 1).AddIngredient(ItemID.StaffofEarth, 1).AddIngredient(mod.ItemType("ThrowableBoulderTrap"), 100).AddIngredient(mod.ItemType("PrismalBar"), 10).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}
	}
	public class JonesBoulderSummon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rocks");
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
			Projectile.width = 48;
			Projectile.timeLeft = 3;
			Projectile.height = 48;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = true;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);

			int proj = Projectile.NewProjectile(Projectile.Center, new Vector2(Projectile.velocity.X, Projectile.velocity.Y / 3f), Mod.Find<ModProjectile>("ProjectilePortalJones").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, ProjectileID.Boulder);
			Main.projectile[proj].penetrate = 2;
			Main.projectile[proj].netUpdate = true;
			IdgProjectile.Sync(proj);

			return true;
		}

		public override void AI()
		{
			Projectile.timeLeft += 2;
			bool cond = Projectile.timeLeft == 4;
			for (int num621 = 0; num621 < (cond ? 15 : 1); num621++)
			{
				int num622 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 226, Projectile.velocity.X * (cond ? 1.5f : 0.5f), Projectile.velocity.Y * (cond ? 1.5f : 0.5f), 20, Color.Red, 0.5f);
				Main.dust[num622].velocity *= 1f;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
				Main.dust[num622].noGravity = true;
			}


			Player player = Main.player[Projectile.owner];
			Projectile.ai[0] += 1;


			Vector2 speedz = Projectile.velocity;
			float atspeed = speedz.Length();
			Vector2 gohere = new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(Projectile.ai[1]) }.ToVector2();
			//ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
			speedz = gohere - Projectile.Center;
			speedz.Normalize(); speedz *= atspeed;
			Projectile.velocity = speedz;

			if ((Projectile.Center - gohere).Length() < atspeed + 8 || Projectile.timeLeft > 300)
			{
				Projectile.Kill();
			}

		}
	}

	public class ProjectilePortalJones : ProjectilePortal
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spawner");
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			//projectile.aiStyle = 1;
			Projectile.friendly = true;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			Projectile.timeLeft = 70;
			Projectile.tileCollide = false;
			AIType = -1;
		}

		public override void Explode()
		{

			if (Projectile.timeLeft == 30 && Projectile.ai[0] > 0)
			{
				Player owner = Main.player[Projectile.owner];
				if (owner != null && !owner.dead)
				{

					Vector2 gotohere = new Vector2();
					gotohere = Projectile.velocity;//Main.MouseScreen - projectile.Center;
					gotohere.Normalize();

					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(20)) * Projectile.velocity.Length();
					int proj = Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), (int)Projectile.ai[0], Projectile.damage, Projectile.knockBack, owner.whoAmI);
					Main.projectile[proj].DamageType = DamageClass.Magic;
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].hostile = false;
					Main.projectile[proj].netUpdate = true;
					IdgProjectile.Sync(proj);
				}

			}

		}

	}
	class WreckerBalls : ThrowableTrapSpikyball
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Spatial Spheres");
			Tooltip.SetDefault("Throws a spatial spiky ball that links with 6 others with a damaging laser\nThese are in relation to you and the main ball\nEnemies on the corners take much more damage\nCan only throw out 1 at a time\nCounts as trap damage, Pierce infinitely, but don't crit");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 100;
			Item.shootSpeed = 8f;
			Item.shoot = ModContent.ProjectileType<WreckerBallProj>();
			Item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			Item.knockBack = 2;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 70;
			Item.useTime = 70;
			Item.value = Item.buyPrice(0, 0, 7, 50);
			Item.rare = ItemRarityID.Red;
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return Main.hslToRgb((Main.GlobalTimeWrappedHourly * 1.20f) % 1f, 1f, 0.75f);
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			Effect effect = SGAmod.TextureBlendEffect;
			Texture2D texture = Main.itemTexture[Item.type];

			effect.Parameters["coordMultiplier"].SetValue(new Vector2(1f, 1f));
			effect.Parameters["coordOffset"].SetValue(new Vector2(0f, 0f));
			effect.Parameters["noiseMultiplier"].SetValue(new Vector2(1f, 1f));
			effect.Parameters["noiseOffset"].SetValue(new Vector2(0f, 0f));

			effect.Parameters["Texture"].SetValue(texture);
			effect.Parameters["noiseTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Extra_49c").Value);
			effect.Parameters["textureProgress"].SetValue(0);
			effect.Parameters["noiseBlendPercent"].SetValue(1f);

			effect.Parameters["strength"].SetValue(1f);
			effect.Parameters["alphaChannel"].SetValue(false);

			Color colorz = Main.hslToRgb((Main.GlobalTimeWrappedHourly * 0.60f) % 1f, 1f, 0.75f);

			effect.Parameters["noiseProgress"].SetValue(Main.GlobalTimeWrappedHourly%1f);
				effect.Parameters["colorTo"].SetValue(colorz.ToVector4());
				effect.Parameters["colorFrom"].SetValue(Color.Black.ToVector4());

				effect.CurrentTechnique.Passes["TextureBlend"].Apply();


			Color glowColor = Color.White;

			Vector2 slotSize = new Vector2(52f, 52f) * scale;
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;

			slotSize.X /= 1.0f;
			slotSize.Y = -slotSize.Y / 4f;

			spriteBatch.Draw(Main.itemTexture[Item.type], drawPos, null, glowColor, -Main.GlobalTimeWrappedHourly*0.75f, Main.itemTexture[Item.type].Size() / 2f, Main.inventoryScale * 2f, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			return false;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			return base.Shoot(player,ref position,ref speedX,ref speedY,ref type,ref damage,ref knockBack);
        }

        public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(ModContent.ItemType<ByteSoul>(), 5).AddIngredient(ModContent.ItemType<ThrowableTrapSpikyball>(), 50).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}


		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[ModContent.ProjectileType<WreckerBallProj>()]<3;
		}

	}
	public class WreckerBallProj : ModProjectile
	{
		public int BallCount => 6;
		public Player MyPlayer => Main.player[Projectile.owner];


		public Vector2 BallPosition(float angle, Vector2 from)
		{
			Vector2 here = Projectile.Center;
			Vector2 differ = from - here;
			return here.RotatedBy(angle, from);

		}

		public List<Vector2> AllPoints
        {

            get
            {
				List<Vector2> vecors = new List<Vector2>();

				
				for (float f2 = 0; f2 < MathHelper.TwoPi-0.001f; f2 += MathHelper.TwoPi/((float)BallCount))
				{
					float f = f2;
					Vector2 projerPos = Projectile.Center + (((Projectile.Center-MyPlayer.Center).ToRotation()) + f).ToRotationVector2() * 96f;
					Vector2 basepose = BallPosition(f, MyPlayer.Center);
					Vector2 lerper = Vector2.Lerp(MyPlayer.Center, basepose, MathHelper.SmoothStep(0f, 1f, MathHelper.Clamp(Projectile.timeLeft/60f, 0f, 1f)));
					vecors.Add(lerper);
				}
				return vecors;

			}

        }

        public override bool CanDamage()
        {
			return Projectile.timeLeft > 60;
        }

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrecker");
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + ProjectileID.SpikyBallTrap); }
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			AIType = ProjectileID.Boulder;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 4;
			Projectile.light = 0.5f;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 600;
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.tileCollide = true;
			Projectile.trap = true;
		}

		public override void AI()
		{
			Projectile.localAI[0] += 1;
			if (Projectile.localAI[0] > 30)
			{
				Projectile.velocity *= 0.98f;
			}

			if (Projectile.localAI[0] == 1)
			{
				foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.whoAmI != Projectile.whoAmI && testby.type == Projectile.type && testby.owner == MyPlayer.whoAmI))
				{
					proj.timeLeft = Math.Min(60, proj.timeLeft);
					proj.netUpdate = true;
				}
			}

			if (Projectile.timeLeft > 60 && Projectile.localAI[0] % 3 == 0)
			{
				foreach (Vector2 points in AllPoints)
				{
					Projectile.NewProjectile(points, Vector2.Zero, ModContent.ProjectileType<WreckerExplosion>(), (int)(Projectile.damage*2.5f),Projectile.knockBack*2, Projectile.owner);
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			var snd = SoundEngine.PlaySound(SoundID.DD2_BetsyFireballImpact, Projectile.Center);
			if (snd != null)
				snd.Pitch = 0.80f;
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
		}

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			crit = false;
			//damage /= 3;
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {

			int index = 0;

			foreach (Vector2 points in AllPoints)
			{
				float percent = index / (float)AllPoints.Count;

				index += 1;

				Vector2 dist1 = AllPoints[(index + 1) % AllPoints.Count];
				//Vector2 dist2 = dist1 - points;
				//Vector2 sizer = new Vector2((float)dist2.Length() / (float)texture2.Width, 1f);

				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), points, dist1))
				{
					return true;
				}
			}


			return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[Projectile.type];
			Texture2D texture2 = ModContent.Request<Texture2D>("SGAmod/Voronoi");
			Vector2 position = Projectile.Center;
			float alphaAdd = MathHelper.Clamp(Projectile.localAI[0]/12f, 0f, Math.Min(Projectile.timeLeft / 60f,1f));

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Effect effect = SGAmod.TextureBlendEffect;


				effect.Parameters["noiseMultiplier"].SetValue(new Vector2(1f, 1f));
				effect.Parameters["noiseOffset"].SetValue(new Vector2(0f, 0f));

				effect.Parameters["Texture"].SetValue(texture2);
				effect.Parameters["noiseTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("SmallLaserHorz").Value);
				effect.Parameters["textureProgress"].SetValue(0);
				effect.Parameters["noiseBlendPercent"].SetValue(1f);
				effect.Parameters["strength"].SetValue(alphaAdd);
				effect.Parameters["alphaChannel"].SetValue(false);


			int index = 0;

			foreach (Vector2 points in AllPoints)
			{
				float percent = index / (float)AllPoints.Count;

				Color colorz = Main.hslToRgb(((Main.GlobalTimeWrappedHourly * 0.60f) + percent) % 1f, 1f, 0.75f);
				effect.Parameters["noiseProgress"].SetValue((Projectile.localAI[0]/120f) + percent);
				effect.Parameters["colorTo"].SetValue(colorz.ToVector4());
				effect.Parameters["colorFrom"].SetValue(Color.Black.ToVector4());

				index += 1;

				Vector2 dist1 = AllPoints[(index + 1) % AllPoints.Count];
				Vector2 dist2 = dist1 - points;
				Vector2 sizer = new Vector2((float)dist2.Length() / (float)texture2.Width, 1f);

				effect.Parameters["coordMultiplier"].SetValue(sizer);
				effect.Parameters["coordOffset"].SetValue(new Vector2((Projectile.localAI[0]/200f)/sizer.X, 0f));

				effect.CurrentTechnique.Passes["TextureBlend"].Apply();

				Main.spriteBatch.Draw(texture2, points - Main.screenPosition, null, Color.White, dist2.ToRotation(), new Vector2(0, texture2.Height/2f), new Vector2(dist2.Length() / (float)texture2.Width,0.1f), default, 0);
			}

			index = 0;


			effect.Parameters["coordMultiplier"].SetValue(new Vector2(1f, 1f));
			effect.Parameters["coordOffset"].SetValue(new Vector2(0f, 0f));
			effect.Parameters["noiseMultiplier"].SetValue(new Vector2(1f, 1f));
			effect.Parameters["noiseOffset"].SetValue(new Vector2(0f, 0f));
			effect.Parameters["strength"].SetValue(alphaAdd*2f);

			effect.Parameters["Texture"].SetValue(texture);
			effect.Parameters["noiseTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Extra_49c").Value);

			foreach (Vector2 points in AllPoints)
			{
				float percent = index / (float)AllPoints.Count;

				Color colorz = Main.hslToRgb(((Main.GlobalTimeWrappedHourly * 0.60f)+ percent) % 1f, 1f, 0.75f);
				effect.Parameters["noiseProgress"].SetValue((Projectile.localAI[0] / 120f) + percent);
				effect.Parameters["colorTo"].SetValue(colorz.ToVector4());
				effect.Parameters["colorFrom"].SetValue(Color.Black.ToVector4());

				effect.CurrentTechnique.Passes["TextureBlend"].Apply();

				index += 1;

				Main.spriteBatch.Draw(texture, points - Main.screenPosition, null, Color.White, Projectile.localAI[0] / 60f, texture.Size() / 2f, 5f, default, 0);
			}

			effect.Parameters["Texture"].SetValue(SGAmod.ExtraTextures[119]);
			effect.Parameters["noiseTexture"].SetValue(SGAmod.ExtraTextures[119]);
			effect.Parameters["noiseProgress"].SetValue((Projectile.localAI[0] / 60f));
			effect.CurrentTechnique.Passes["TextureBlend"].Apply();


			Main.spriteBatch.Draw(SGAmod.ExtraTextures[119], Projectile.Center - Main.screenPosition, null, Color.White, Projectile.localAI[0] / -60f, SGAmod.ExtraTextures[119].Size() / 2f, 1f, default, 0);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}

		public class WreckerExplosion : Explosion
		{
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Wrecker Explosion");
			}

			public override void SetDefaults()
			{
				Projectile.width = 96;
				Projectile.height = 96;
				Projectile.friendly = true;
				Projectile.hostile = false;
				Projectile.ignoreWater = true;
				Projectile.tileCollide = false;
				Projectile.Throwing().DamageType = DamageClass.Throwing;
				Projectile.trap = true;
				Projectile.hide = true;
				Projectile.timeLeft = 2;
				Projectile.penetrate = 3;
				Projectile.usesIDStaticNPCImmunity = true;
				Projectile.idStaticNPCHitCooldown = -1;
			}

		}

	}


}

//Trap Acc's
namespace SGAmod.Items.Accessories
{

	public class JindoshBuckler : TrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jindosh Buckler");
			Tooltip.SetDefault("'Inventive, yet strikingly crude and cruel'\nTrap Damage ignores 50% of enemy defense\nTrap damage may inflict Massive Bleeding\n10% of extra Trap Damage is dealt directly to your enemy's life\nThis ignores defense and damage reduction\n" +
				"Trap Damage increased by 10%\nYou reflect 2 times the damage you take back to melee attackers (Corruption Worlds)\n+20 Max HP and hearts give +5 Health (Crimson Worlds)");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = 8;
			Item.defense = 5;
			Item.accessory = true;
		}

		public void UpdateAccessoryThing(Player player, bool hideVisual, bool corrcrim)
		{
			player.GetModPlayer<SGAPlayer>().JaggedWoodenSpike = true;
			if (WorldGen.crimson || corrcrim)
				Mod.GetItem("HeartGuard").UpdateAccessory(player, hideVisual);
			if (!WorldGen.crimson || corrcrim)
				Mod.GetItem("JuryRiggedSpikeBuckler").UpdateAccessory(player, hideVisual);
			Mod.GetItem("GoldenCog").UpdateAccessory(player, hideVisual);

		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			UpdateAccessoryThing(player, hideVisual,false);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.SilkRopeCoil, 2).AddIngredient(ModContent.ItemType<PrismalBar>(), 5).AddIngredient(ModContent.ItemType<JaggedOvergrownSpike>(), 4).AddIngredient(ModContent.ItemType<GoldenCog>(), 1).AddIngredient(ModContent.ItemType<JuryRiggedSpikeBuckler>(), 1).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
			CreateRecipe(1).AddIngredient(ItemID.SilkRopeCoil, 2).AddIngredient(ModContent.ItemType<PrismalBar>(), 5).AddIngredient(ModContent.ItemType<JaggedOvergrownSpike>(), 4).AddIngredient(ModContent.ItemType<GoldenCog>(), 1).AddIngredient(ModContent.ItemType<HeartGuard>(), 1).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}
	}

	public class JaggedOvergrownSpike : TrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jagged Overgrown Spike");
			Tooltip.SetDefault("Trap Damage ignores 40% of enemy defense\nTrap Damage may inflict Massive Bleeding");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 7;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.GetModPlayer<SGAPlayer>().JaggedWoodenSpike = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.WoodenSpike, 10).AddIngredient(ItemID.Nail, 20).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}
	}

	public class GoldenCog : TrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Cog");
			Tooltip.SetDefault("10% of extra Trap Damage is dealt directly to your enemy's life\nThis ignores defense and damage reduction");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 6;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.GetModPlayer<SGAPlayer>().GoldenCog = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Cog, 100).AddRecipeGroup("SGAmod:Tier4Bars", 5).AddIngredient(mod.ItemType("SharkTooth"), 50).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}
	}

	public class HeartGuard : TrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart Guard");
			Tooltip.SetDefault("Trap Damage increased by 10%\n+20 Max HP and hearts give +5 Health\nEffect of Rusted Bulwark and Aversion Charm");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 4;
			Item.defense = 4;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			Mod.GetItem("RustedBulwark").UpdateAccessory(player, hideVisual);
			SGAPlayer sgaply = player.SGAPly();
			sgaply.aversionCharm = true;
			sgaply.HeartGuard = true;
			sgaply.TrapDamageMul += 0.1f;
			player.statLifeMax2 += 20;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("RustedBulwark"), 1).AddIngredient(mod.ItemType("AversionCharm"), 1).AddIngredient(ItemID.LifeCrystal, 1).AddIngredient(ItemID.HeartreachPotion, 1).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}
	}

	public class JuryRiggedSpikeBuckler : TrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("'JuryRigged' Spike Buckler");
			Tooltip.SetDefault("Trap Damage increased by 10% and ignores 10% of enemy defense\nYou reflect 2 times the damage you take back to melee attackers\nEffect of Rusted Bulwark and Aversion Charm");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = 4;
			Item.defense = 4;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			Mod.GetItem("RustedBulwark").UpdateAccessory(player, hideVisual);
			SGAPlayer sgaply = player.SGAPly();
			sgaply.aversionCharm = true;
			sgaply.JuryRiggedSpikeBuckler = true;
			sgaply.TrapDamageMul += 0.1f;
			player.thorns += 2f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("RustedBulwark"), 1).AddIngredient(mod.ItemType("AversionCharm"), 1).AddIngredient(ItemID.Spike, 40).AddIngredient(ItemID.ThornsPotion, 1).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}
	}

	[AutoloadEquip(EquipType.HandsOn)]
	public class GrippingGloves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gripping Gloves");
			Tooltip.SetDefault("'For holding onto the big things in your life'\nReduces the movement speed slowdown of Non-Stationary Defenses\nYou can turn around while holding a Non-Stationary Defense\n10% increased Trap Damage");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().TrapDamageMul += 0.10f;
			player.GetModPlayer<SGAPlayer>().grippinggloves = Math.Max(player.GetModPlayer<SGAPlayer>().grippinggloves,1);
			player.GetModPlayer<SGAPlayer>().SlowDownResist += 2f;
			player.GetModPlayer<SGAPlayer>().grippingglovestimer = 3;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 32;
			Item.height = 32;
			Item.value = 15000;
			Item.rare = 2;
			Item.accessory = true;
		}
	}

	[AutoloadEquip(EquipType.HandsOn)]
	public class HandlingGloves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Handling Gloves");
			Tooltip.SetDefault("'For handling extreme situations!'\nImmunity to knockback and fire blocks!\nReduces the effects of holding radioactive materials\n+8 defense while holding a Non-Stationary Defense\nGreatly reduces the movement speed slowdown of Non-Stationary Defenses\nYou can turn around while holding a Non-Stationary Defense\n15% increased Trap Damage and 10% increased Trap Armor Penetration");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().TrapDamageMul += 0.15f;
			player.GetModPlayer<SGAPlayer>().TrapDamageAP += 0.10f;
			player.GetModPlayer<SGAPlayer>().grippinggloves = Math.Max(player.GetModPlayer<SGAPlayer>().grippinggloves, 2);
			player.GetModPlayer<SGAPlayer>().grippingglovestimer = 3;
			player.GetModPlayer<SGAPlayer>().SlowDownResist += 8f;
			player.noKnockback = true;
			player.fireWalk = true;
			if (SGAmod.NonStationDefenses.ContainsKey(player.HeldItem.type))
				player.statDefense += 8;

		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 32;
			Item.height = 32;
			Item.value = 75000;
			Item.rare = 6;
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("GrippingGloves"), 1).AddIngredient(ItemID.ObsidianShield, 1).AddIngredient(ItemID.ChlorophyteBar, 10).AddIngredient(ItemID.HellstoneBar, 5).AddIngredient(ItemID.LeadBar, 6).AddIngredient(mod.ItemType("SharkTooth"), 50).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}
	}

}
