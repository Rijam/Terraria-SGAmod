using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using SGAmod.HavocGear.Items.Tools;

using SGAmod.Dusts;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class MangroveBow : MangrovePickaxe, IMangroveSet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Bow");
			Tooltip.SetDefault("Shoots 2 arrows offsetted at angles at the cost of 1");
		}

		public override void SetDefaults()
		{
			Item.damage = 23;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 20;
			Item.height = 32;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = 25000;
			Item.rare = 5;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 50f;
			Item.useAmmo = AmmoID.Arrow;
		}

        	public override void AddRecipes()
        	{
            		CreateRecipe(1).AddIngredient(null, "DankWoodBow", 1).AddIngredient(null, "BiomassBar", 8).AddTile(TileID.Anvils).Register();
        	}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float numberProjectiles = 2 + Main.rand.Next(1);
			float rotation = MathHelper.ToRadians(10);
			speedX *= player.ArrowSpeed(); speedY *= player.ArrowSpeed();

			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}

	public class MangroveStriker : MangroveBow, IMangroveSet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Striker");
			Tooltip.SetDefault("Attacks may inflict Dryad's Bane, or bless yourself");
		}

		public override void SetDefaults()
		{
			Item.damage = 32;
			Item.DamageType = DamageClass.Melee;
			Item.width = 36;
			Item.height = 36;
			Item.useTime = 19;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.knockBack = 7;
			Item.value = 25000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (Main.rand.Next(0, 100) < 25)
			target.AddBuff(BuffID.DryadsWardDebuff, 60 * 4);
			if (Main.rand.Next(0, 100) < 25)
				player.AddBuff(BuffID.DryadsWard, 60 * 5);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "DankWoodSword", 1).AddIngredient(null, "BiomassBar", 8).AddTile(TileID.Anvils).Register();
		}
	}

	public class MangroveStaff : MangroveBow, IMangroveSet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Staff");
			Tooltip.SetDefault("Shoots 3 fast moving homing mangrove orbs");
		}

		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 5;
			Item.width = 38;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 30;
			Item.staff[Item.type] = true;
			Item.useStyle = 5;
			Item.knockBack = 0.5f;
			Item.value = 25000;
			Item.rare = 3;
			Item.noMelee = true;
			Item.shoot = Mod.Find<ModProjectile>("MangroveStaffOrb").Type;
			Item.shootSpeed = 12f;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "BiomassBar", 8).AddTile(TileID.Anvils).Register();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return true;
		}

	}

	public class MangroveShiv : MangroveStaff, IMangroveSet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Throwing Shiv");
			Tooltip.SetDefault("Shivs pierce and chain between enemies on hit\nEnemies hit who have Dryad's Bane allow an additional chain");
		}

		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.useStyle = 1;
			Item.knockBack = 1;
			Item.value = 25;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.rare = 3;
			Item.UseSound = SoundID.Item1;
			Item.shoot = Mod.Find<ModProjectile>("MangroveShivProj").Type;
			Item.shootSpeed = 15f;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;

			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi) * 0.04f);
			Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);

			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(null, "BiomassBar", 1).AddTile(TileID.Anvils).Register();
		}

	}

	class MangroveShivProj : ModProjectile
	{

		bool hitonce = false;
		List<Point> listOfPoints = new List<Point>();

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Mangrove Shiv");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Grenade);
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.timeLeft = 600;
			Projectile.aiStyle = -1;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.penetrate = 3;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Items/Weapons/MangroveShiv"); }
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			listOfPoints.Add(new Point(target.whoAmI,1000000));
			List<NPC> closestnpcs = SGAUtils.ClosestEnemies(Projectile.Center, 200,AddedWeight: listOfPoints,checkCanChase: false);

			if (target.HasBuff(BuffID.DryadsWardDebuff))
				Projectile.penetrate += 1;

			if (closestnpcs != null && closestnpcs.Count>0)
            {
				NPC target2 = closestnpcs[0];
				Vector2 dist = target2.Center - Projectile.Center;

				while (dist.Length() > 24f)
                {
					Projectile.Center += Vector2.Normalize(dist)*6f;
					dist = target2.Center - Projectile.Center;


					int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 8, Projectile.Center.Y - 8), 16, 16, ModContent.DustType<MangroveDust>());
					Main.dust[dust].scale = 1.75f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Vector2.Zero;
				}
				Projectile.velocity = Vector2.Normalize(dist) * 6f;
				Projectile.rotation = Projectile.velocity.ToRotation()+(MathHelper.Pi/4f);

			}
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			return true;
		}

		public override void AI()
		{

			if (Main.rand.Next(0, 3) == 0)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12), 24, 24, 96);
				Main.dust[dust].scale = 0.50f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = -Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			}

			Projectile.timeLeft -= 1;
			Projectile.velocity.Y += 0.25f;
			Projectile.velocity.X *= 0.98f;
			Projectile.rotation += Projectile.velocity.Length() * (float)(Math.Sign(Projectile.velocity.X * 1000f) / 1000f) * 10f;
		}

	}

}
