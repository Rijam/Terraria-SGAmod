using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;

namespace SGAmod.Items.Weapons
{
	public class FSRG : Vibranium.VibraniumText
    {
		int shootCount = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("F.S.R.G");
			Tooltip.SetDefault("'Furious Sting-Ray Gun'\nRapidly fires multi-hitting flaming stingers that cause no immunity frames\nThese inflict Gourged and leave behind poison clouds\nStingers are most effective against larger enemies\n75% to not consume ammo");
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
			Item.value = 750000;
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = SoundID.Item99;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 20f;
			Item.useAmmo = AmmoID.Dart;
		}

        public override bool ConsumeAmmo(Player player)
        {
			return Main.rand.Next(100) < 25;
        }

        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-24, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			shootCount += 1;
			float speed=1.5f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(4);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;

			for (int i = 0; i < numberProjectiles; i++)
			{
				int typeOfShot = Mod.Find<ModProjectile>("FlamingStinger").Type;
				if (false)
					typeOfShot = type;

				Vector2 perturbedSpeed = (new Vector2(speedX, speedY)*speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0,100)/100f)) * .3f; // Watch out for dividing by 0 if there is only 1 projectile.
				int proj=Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y,typeOfShot , damage, knockBack, player.whoAmI);
				Main.projectile[proj].friendly=true;
				Main.projectile[proj].hostile=false;
				Main.projectile[proj].knockBack=Item.knockBack;
				Main.projectile[proj].ai[0] = (int)Main.rand.Next(0, 80);
				Main.projectile[proj].netUpdate = true;
				Main.projectile[proj].localNPCHitCooldown = 3;
				Main.projectile[proj].usesLocalNPCImmunity = true;

				IdgProjectile.AddOnHitBuff(proj,BuffID.OnFire,60*6);
				IdgProjectile.AddOnHitBuff(proj, Mod.Find<ModBuff>("Gourged").Type, 60 * 6);
				IdgProjectile.Sync(proj);
			}
			return false;
		}

		public override void AddRecipes()
		{
            CreateRecipe(1).AddIngredient(ModContent.ItemType <Gatlipiller>(), 1).AddIngredient(ItemID.Stinger, 12).AddIngredient(ModContent.ItemType <HavocGear.Items.Weapons.SharkTooth>(), 50).AddIngredient(ModContent.ItemType<HavocGear.Items.VirulentBar>(), 5).AddIngredient(ModContent.ItemType<IlluminantEssence>(), 20).AddIngredient(ModContent.ItemType<VibraniumBar>(), 8).AddIngredient(ItemID.LunarBar, 5).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}

	}

	public class FlamingStinger : ModProjectile
	{

		int fakeid=ProjectileID.Stinger;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flaming Stinger");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 3;
			Projectile.penetrate = 5;
			Projectile.timeLeft = 300;
			Projectile.localNPCHitCooldown = 3;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + fakeid; }
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type=fakeid;
			return true;
		}

		public override void AI()
		{
			Projectile.ai[0] += 1;
			if (Projectile.ai[0] % 40 == 0)
			{
				Vector2 avel = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[0] % 80==0 ? 90 : -90))/5f;
				int proj=Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, avel.X, avel.Y, ProjectileID.SporeGas3, Projectile.damage*2, Projectile.knockBack, Projectile.owner);
				Main.projectile[proj].usesLocalNPCImmunity = true;
				Main.projectile[proj].localNPCHitCooldown = -1;
				Main.projectile[proj].scale = 0.5f;
				Main.projectile[proj].extraUpdates = 1;
				Main.projectile[proj].DamageType = DamageClass.Ranged;
				Main.projectile[proj].netUpdate = true;
				IdgProjectile.AddOnHitBuff(proj, Mod.Find<ModBuff>("AcidBurn").Type, 60 * 2);
				IdgProjectile.Sync(proj);
			}

			int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6);
        Main.dust[dust].scale = 0.8f;
        Main.dust[dust].noGravity = false;
        Main.dust[dust].velocity = Projectile.velocity*(float)(Main.rand.Next(20,100)*0.005f);
        Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}

	}


}
