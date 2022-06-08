using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics;
using ReLogic.Graphics;
using Terraria.UI.Chat;
using SGAmod.Projectiles;
using SGAmod.NPCs.Hellion;
using Idglibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class Maldal : ElementalCascade
	{
		int projectiletype = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Maldal");
			Tooltip.SetDefault("Floods your screen with files that explode when they touch an enemy!\nNot usable while your files are on the system");
			Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			Item.damage = 200;
			Item.crit = 15;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 250;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = 4;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 5;
			Item.value = 1000000;
			Item.rare = 10;
			Item.UseSound = SoundID.Item78;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("MagoldFiles").Type;
			Item.shootSpeed = 8f;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTimeWrappedHourly / 6f) % 1f, 0.85f, 0.45f);
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Mod.Find<ModProjectile>("MagoldFiles").Type] < 1;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Accessories/LostNotes"); }
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("ByteSoul"), 75).AddRecipeGroup("Fragment", 15).AddIngredient(ItemID.SpellTome, 1).AddIngredient(ItemID.Worm, 1).AddTile(TileID.LunarCraftingStation).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int a = -100; a < 101; a += 10)
			{
				for (int i = -100; i < 101; i += 10)
				{
					int probg = Projectile.NewProjectile(player.Center.X + a * 10f, player.Center.Y + i * 10f, 0, 0, type, damage, knockBack, player.whoAmI);
					Main.projectile[probg].friendly = true;
					Main.projectile[probg].hostile = false;
					Main.projectile[probg].netUpdate = true;
					IdgProjectile.Sync(probg);

				}
			}
			return false;
		}

		public class MagoldFiles : ModProjectile
		{
			private Vector2[] oldPos = new Vector2[6];
			private float[] oldRot = new float[6];
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("RaVe");
			}

			public override string Texture
			{
				get { return ("SGAmod/Items/Accessories/LostNotes"); }
			}

			public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
			{
				Vector2 size = Main.fontDeathText.MeasureString("RaVe" + Projectile.whoAmI);
				spriteBatch.DrawString(Main.fontMouseText, "RaVe" + Projectile.whoAmI, (Projectile.Center + new Vector2(-size.X / 6f, 24)) - Main.screenPosition, Color.White);
				return base.PreDraw(spriteBatch, Color.White);
			}

			public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
			{
				for (int i = 0; i < 359; i += 359)
				{
					double angles = MathHelper.ToRadians(i);
					float randomx = 48f;//Main.rand.NextFloat(54f, 96f);
					Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles));

					SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y,61);
					if (sound != null)
						sound.Pitch = 0.925f;

					int thisone = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0,0, ModContent.ProjectileType<MaldalBlast>(), Projectile.damage, Projectile.knockBack, Main.player[Projectile.owner].whoAmI, 0.0f, 0f);
					IdgProjectile.Sync(thisone);


				}
			}

			public override void SetDefaults()
			{
				//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
				Projectile.width = 16;
				Projectile.height = 20;
				Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
				Projectile.hostile = false;
				Projectile.friendly = true;
				Projectile.tileCollide = true;
				Projectile.DamageType = DamageClass.Magic;
				Projectile.timeLeft = 450;
				AIType = ProjectileID.Bullet;
			}

		}

	}

	public class MaldalBlast : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Viral Blast");
		}

		public override void SetDefaults()
		{
			Projectile.width = 96;
			Projectile.height = 96;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 60;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Projectiles/BoulderBlast"); }
		}

        public override bool CanDamage()
        {
            return Projectile.ai[0]<30;
        }

        public override void AI()
		{
			Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.01f) / 255f, ((255 - Projectile.alpha) * 0.025f) / 255f, ((255 - Projectile.alpha) * 0.25f) / 255f);
			Projectile.ai[0] += 1;

			float size = Projectile.ai[0] < 2 ? 0f : 1f;

			for (int i = 0; i < 1 + (Projectile.ai[0] < 2 ? 32 : 0); i++)
			{
				Vector2 randomcircle = Main.rand.NextVector2CircularEdge(1f, 1f);
				int num655 = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.width)* size, 0, 0, ModContent.DustType<Dusts.ViralDust>(), Projectile.velocity.X + randomcircle.X * (4f), Projectile.velocity.Y + randomcircle.Y * (4f), 150, Main.hslToRgb(Main.rand.NextFloat(), 1f, (Projectile.timeLeft / 60f)), 0.5f);
				Main.dust[num655].noGravity = true;
			}
		}
	}


}
