using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Items.Consumables;
using SGAmod.Items.Weapons;
using Terraria.Audio;

namespace SGAmod.Items.Weapons.Ammo
{

	public class Jarocket : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jarocket");
			Tooltip.SetDefault("Rocket Propelled Jar based karate!");
		}
		public override void SetDefaults()
		{
			Item.damage = 72;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 3.5f;
			Item.value = 250;
			Item.rare = 8;
			Item.shoot = ModContent.ProjectileType<JarocketProj>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 4.5f;                  //The speed of the projectile
			Item.ammo = AmmoID.Rocket;
		}

		public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
		{
			if (weapon.type != ItemID.GrenadeLauncher && weapon.type != ItemID.FireworksLauncher && weapon.type != ItemID.ElectrosphereLauncher)
			{
				if (type != ProjectileID.GrenadeI || type != ProjectileID.GrenadeII || type != ProjectileID.GrenadeII || type != ProjectileID.GrenadeIV)
					type = ModContent.ProjectileType<JarocketProj>();
			}
			if (weapon.shoot == ProjectileID.GrenadeI)
			{
				type = ProjectileID.GrenadeI;
			}
			if (weapon.shoot == ProjectileID.ElectrosphereMissile)
			{
				type = ProjectileID.ElectrosphereMissile;
			}
		}


		public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(mod.ItemType("StarMetalBar"), 3).AddIngredient(mod.ItemType("Jarate"), 1).AddIngredient(mod.ItemType("LuminiteWraithNotch"), 1).AddIngredient(ItemID.RocketIII, 50).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}
	}
	public class AcidRocket : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Rocket");
			Tooltip.SetDefault("Explodes into a cloud of acid on hit\nAcid quickly melt away the rocket after being fired and does not go far");
		}
		public override void SetDefaults()
		{
			Item.damage = 64;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 3.5f;
			Item.value = 200;
			Item.rare = ItemRarityID.Lime;
			Item.shoot = ModContent.ProjectileType<AcidRocketProj>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 3f;                  //The speed of the projectile
			Item.ammo = AmmoID.Rocket;
		}

		public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
		{
			if (weapon.type != ItemID.GrenadeLauncher && weapon.type != ItemID.FireworksLauncher && weapon.type != ItemID.ElectrosphereLauncher)
			{
				if (type != ProjectileID.GrenadeI || type != ProjectileID.GrenadeII || type != ProjectileID.GrenadeII || type != ProjectileID.GrenadeIV)
					type = ModContent.ProjectileType<AcidRocketProj>();
			}
			if (weapon.shoot == ProjectileID.GrenadeI)
			{
				type = ProjectileID.GrenadeI;
			}
			if (weapon.shoot == ProjectileID.ElectrosphereMissile)
			{
				type = ProjectileID.ElectrosphereMissile;
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(mod.ItemType("VialofAcid"), 3).AddIngredient(ItemID.RocketIII, 50).AddTile(TileID.MythrilAnvil).Register();
		}
	}

	public class JackpotRocketItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rigged Jackpot");
			Tooltip.SetDefault("Rigged to allow launchers to shoot jackpot rockets!\nNon Consumable Ammo Type");
		}
		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 3.5f;
			Item.value = 500000;
			Item.rare = 10;
			Item.shoot = Mod.Find<ModProjectile>("JackpotRocket").Type;   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 4.5f;                  //The speed of the projectile
			Item.ammo = AmmoID.Rocket;
		}

		public override bool ConsumeAmmo(Player player)
		{
			return false;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/RiggedJackpot"); }
		}

		public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
		{
			if (type!=ProjectileID.GrenadeI || type != ProjectileID.GrenadeII || type != ProjectileID.GrenadeII || type != ProjectileID.GrenadeIV)
			type = Mod.Find<ModProjectile>("JackpotRocket").Type;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("CrateBossWeaponRanged"), 1).AddIngredient(mod.ItemType("MoneySign"), 8).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}
	}

	public class JarocketProj : JarateProj
	{

		double keepspeed=0.0;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jarocket");
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile=false;
			Projectile.friendly=true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(Mod.Find<ModBuff>("Sodden").Type, 60 * 10);
			if (Main.player[Projectile.owner].GetModPlayer<SGAPlayer>().MVMBoost)
				target.AddBuff(Mod.Find<ModBuff>("SoddenSlow").Type, 60 * 10);
			target.immune[Projectile.owner] = 5;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type=ProjectileID.RocketIII;
			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
			int theproj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("Explosion").Type, (int)((double)Projectile.damage * 0.75f), Projectile.knockBack, Projectile.owner, 0f, 0f);
			Main.projectile[theproj].DamageType = DamageClass.Ranged;
			effects(1);

			return true;
		}

		public override void AI()
		{
		Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
		for (int num315 = 0; num315 < 3; num315 = num315 + 1)
			{
				Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X-1, Projectile.position.Y)+positiondust, Projectile.width, Projectile.height, 75, 0f, 0f, 50, Main.hslToRgb(0.10f, 0.5f, 0.75f), 1.8f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity = (-Projectile.velocity)+(randomcircle*(0.5f))*((float)num315/3f);
				dust3.velocity.Normalize();
			}

		for (int num315 = 1; num315 < 16; num315 = num315 + 1)
			{
				if (Main.rand.Next(0,100)<25){
				Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X-1, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 50,Color.Goldenrod, 1.33f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity = (randomcircle*2.5f*Main.rand.NextFloat())+(Projectile.velocity);
				dust3.velocity.Normalize();
			}}

		Projectile.ai[0]=Projectile.ai[0]+1;
		Projectile.velocity.Y+=0.1f;
		Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f; 
		}


	}

	public class AcidRocketProj : ModProjectile
	{

		double keepspeed = 0.0;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Rocket");
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
			Projectile.timeLeft = 200;
			AIType = -1;
			Projectile.aiStyle = -1;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Ammo/AcidRocket"; }
		}

		bool hitonce = false;

		public override bool PreKill(int timeLeft)
		{
			if (!hitonce)
			{
				Projectile.width = 200;
				Projectile.height = 200;
				Projectile.position -= new Vector2(100, 100);
			}

			for (int i = 0; i < 125; i += 1)
			{
				float randomfloat = Main.rand.NextFloat(1f, 6f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();

				int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 64, Projectile.Center.Y - 64), 128, 128, Mod.Find<ModDust>("AcidDust").Type);
				Main.dust[dust].scale = 3.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (Projectile.velocity * (float)(Main.rand.Next(10, 50) * 0.01f)) + (randomcircle * randomfloat);
			}

			int theproj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("Explosion").Type, (int)((double)Projectile.damage * 1f), Projectile.knockBack, Projectile.owner, 0f, 0f);
			Main.projectile[theproj].DamageType = projectile.magic;
			IdgProjectile.AddOnHitBuff(theproj, Mod.Find<ModBuff>("AcidBurn").Type, 120);

			Projectile.velocity = default(Vector2);
			Projectile.type = ProjectileID.GrenadeIII;
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!hitonce)
			{
				hitonce = true;
				Projectile.position -= new Vector2(100, 100);
				Projectile.width = 200;
				Projectile.height = 200;
				Projectile.timeLeft = 1;
			}
			//projectile.Center -= new Vector2(48,48);

			target.AddBuff(Mod.Find<ModBuff>("AcidBurn").Type, 200);
		}

		public override void AI()
		{
			Projectile.ai[0] = Projectile.ai[0] + 1;
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

			if (Projectile.ai[0]>20 && Projectile.ai[0] < 70)
			{
				Vector2 speedz = Projectile.velocity;
				Vector2 speedzc = speedz; speedzc.Normalize();
				Projectile.velocity = speedzc * (speedz.Length() + 0.4f);

			}

			for (float i = 0; i < 2.5; i += 0.75f)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Main.rand.Next(0,100)<15 ? DustID.Fire : Mod.Find<ModDust>("AcidDust").Type);
				Main.dust[dust].scale = 1.15f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = -Projectile.velocity * (float)(Main.rand.Next(20, 50+(int)(i*40f)) * 0.01f)/2f;
			}
			Projectile.timeLeft -= 1;
		}


	}

}