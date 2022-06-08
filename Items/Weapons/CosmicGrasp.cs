using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.HavocGear.Projectiles;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class CosmicGrasp : ModItem, IHitScanItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Grasp");
			Tooltip.SetDefault("'With more harmonious convergence, its power is greatly increased'\n" +
				"Launches a piercing spear of cosmic light that creates Quasar Beam Explosions on hit" +
				"\nShadowflame tendrils explode from enemies killed with the spear of light and on crit"+
				"\nExplosion doesn't crit, damage falls off over distance\nFirst hit does 3X damage, No immunity frames are caused by this weapon");
		}
		public override void SetDefaults()
		{
			Item.damage = 120;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 20;
			Item.crit = 25;
			Item.width = 22;
			Item.height = 22;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 5;
			Item.knockBack = 10;
			Item.value = 2000000;
			Item.rare = 12;
			Item.UseSound = SoundID.Item72;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("CGraspSpear").Type;
			Item.shootSpeed = 10;
			Item.staff[Item.type] = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/CosmicGrasp_Glow").Value;
			}
		}
		public override bool Shoot (Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = Mod.Find<ModProjectile>("CGraspSpear").Type;
			float numberProjectiles = 1; // 3, 4, or 5 shots
			float rotation = MathHelper.ToRadians(Main.rand.Next(33));
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY); // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, (int)damage*3, knockBack, player.whoAmI);
			}
			return false;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 4);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("Cosmillash"), 1).AddIngredient(ItemID.ShadowbeamStaff, 1).AddIngredient(mod.ItemType("PrismalBar"), 10).AddIngredient(mod.ItemType("EldritchTentacle"), 15).AddIngredient(mod.ItemType("LunarRoyalGel"), 12).AddIngredient(ItemID.Amethyst, 1).AddTile(TileID.LunarCraftingStation).Register();
		}
	}

	public class QuasarOrbLessParticles : QuasarOrb
	{
		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}
	}

		public class CGraspSpear : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 6;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 300;
			Projectile.light = 0.1f;
			Projectile.extraUpdates = 300;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			AIType = -1;
			Main.projFrames[Projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Grasp");
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			explode(Projectile.Center, Projectile.damage, Projectile.knockBack, false);
			Projectile.Kill();
			return false;
		}

		public void explode(Vector2 target, int damage, float knockback, bool crit)
		{
			for (int i = 0; i < 1; i++)
			{
				int proj = Projectile.NewProjectile(target.X, target.Y, Vector2.Zero.X, Vector2.Zero.Y, Mod.Find<ModProjectile>("QuasarOrbLessParticles").Type, Projectile.damage, Projectile.knockBack, Projectile.owner);
				Main.projectile[proj].timeLeft = 2;
				Main.projectile[proj].netUpdate = true;
				IdgProjectile.Sync(proj);
				Main.projectile[proj].Kill();
			}

		}

		public static void BeamBurst(Vector2 where, int damage, int owner,int count)
		{
			float[] speeds = { 1.5f, 5f };
			if (count > 5)
				speeds = new float[] {5f,9f };
			for (int i = 0; i < count; i++)
			{
				Vector2 velorand = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
				velorand.Normalize();
				int a = Projectile.NewProjectile(where.X, where.Y, velorand.X * Main.rand.NextFloat(speeds[0], speeds[1]), velorand.Y * Main.rand.NextFloat(speeds[0], speeds[1]), ProjectileID.ShadowFlame, (int)(damage * 1.25f), 0, owner);
				Main.projectile[a].tileCollide = true;
				Main.projectile[a].timeLeft = (int)((120 + Main.rand.Next(80)));
				Main.projectile[a].netUpdate = true;
				Main.projectile[a].usesLocalNPCImmunity = true;
				Main.projectile[a].localNPCHitCooldown = -1;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			explode(target.Center, Projectile.damage, knockback, crit);

			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.damage /= 3;

			}

			if (crit)
				CGraspSpear.BeamBurst(target.Center, Projectile.damage, Projectile.owner, 1);
			if (target.life - damage < 1)
				CGraspSpear.BeamBurst(target.Center, Projectile.damage, Projectile.owner, 8);
		}

		public override void AI()
		{
			int num126 = Dust.NewDust(Projectile.Center, 0, 0, 173, Projectile.velocity.X, Projectile.velocity.Y, 0, default(Color), 3.0f);
			Main.dust[num126].noGravity = true;
			Main.dust[num126].velocity = Projectile.velocity * 0.5f;
		}
	}

}
