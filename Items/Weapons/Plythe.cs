using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;


namespace SGAmod.Items.Weapons
{
	class Plythe : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plythe");
			Tooltip.SetDefault("Rapidly throw out seeking Scythes that fly back to the player on hit, can hit up to 3 times\nStacks up to 5 allowing several Plythe blades out at once\nThey reap that which they sow");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Plythe"); }
		}
		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.Throwing().DamageType=DamageClass.Throwing;
			Item.damage = 110;
			Item.crit = 10;
			Item.shootSpeed = 45f;
			Item.shoot = ModContent.ProjectileType<Scythe>();
			Item.useTurn = true;
			Item.width = 54;
			Item.height = 32;
			Item.maxStack = 5;
			Item.knockBack = 0;
			Item.consumable = false;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 8;
			Item.useTime = 8;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(1, 0, 0, 0);
			Item.rare = ItemRarityID.Cyan;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < Item.stack;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//speedX *= player.thrownVelocity;
			//speedY *= player.thrownVelocity;
			return true;
		}

		public override void AddRecipes()
		{
            CreateRecipe(2).AddIngredient(ItemID.LightDisc, 1).AddIngredient(mod.ItemType("StarMetalBar"), 6).AddIngredient(mod.ItemType("CryostalBar"), 4).AddIngredient(mod.ItemType("PrismalBar"), 2).AddTile(TileID.LunarCraftingStation).Register();
		}
	}

	public class Scythe : ModProjectile
	{

		float hittime = 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scythe");
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 64;
			Projectile.height = 64;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.timeLeft = 120;
			Projectile.penetrate = 12;
			AIType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = -8;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_658"); }
		}

		public Scythe()
		{
			Projectile.localAI[0] = 0.5f;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.penetrate < 10)
				return false;
			else
				return base.CanHitNPC(target);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.velocity *= -1f;
			target.immune[Projectile.owner] = 5;
			hittime = 150f;
		}

		public override void AI()
		{

			Lighting.AddLight(Projectile.Center, Color.Aquamarine.ToVector3() * 0.5f);

			hittime = Math.Max(1f, hittime / 1.5f);
			;
			float dist2 = 54f;

			//Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 8f;
			for (float num315 = 0; num315 < MathHelper.Pi + 0.04; num315 = num315 + MathHelper.Pi)
			{
				float angle = (Projectile.rotation + MathHelper.Pi / 5f) + num315;
				Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist2), (float)(Math.Sin(angle) * dist2));
				Vector2 offset = (thisloc * Projectile.localAI[0])+Projectile.velocity;
				int num316 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y) + offset, 0, 0, Mod.Find<ModDust>("NovusSparkleBlue").Type, 0f, 0f, 50, Color.White, 1.5f);
				Main.dust[num316].noGravity = true;
				Main.dust[num316].velocity = thisloc / 30f;
			}

			Projectile.ai[0] = Projectile.ai[0] + 1;
			Projectile.velocity.Y += 0.1f;
			if (Projectile.ai[0] > 14f && !Main.player[Projectile.owner].dead)
			{
				Vector2 dist = (Main.player[Projectile.owner].Center - Projectile.Center);
				Vector2 distnorm = dist; distnorm.Normalize();
				Projectile.velocity += distnorm * 5f;
				Projectile.velocity /= 1.05f;
				//projectile.Center+=(dist*((float)(projectile.timeLeft-12)/28));
				if (dist.Length() < 80)
					Projectile.Kill();
			}

			NPC target = Main.npc[Idglib.FindClosestTarget(0, Projectile.Center, new Vector2(0f, 0f), true, true, true, Projectile)];
			if (target != null && Projectile.penetrate > 9)
			{
				if ((target.Center - Projectile.Center).Length() < 500f)
				{

					Projectile.Center += (Projectile.DirectionTo(target.Center) * (Projectile.ai[0] > 14f ? (50f * Main.player[Projectile.owner].thrownVelocity) / hittime : 12f));

				}
			}

			Projectile.localAI[0] += ((hittime > 10 ? 3.0f : 0.25f) - Projectile.localAI[0])/ 10f;
			Projectile.localAI[0] = MathHelper.Clamp(Projectile.localAI[0], 0.5f, 1f);
			Projectile.rotation += 0.38f + (hittime/50f);
		}


		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/ScytheGlow").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Weapons/Plythe").Value;
			float scale = Projectile.localAI[0];

			spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);

			Texture2D tex = Main.projectileTexture[Projectile.type];

			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Aqua* scale, Projectile.rotation + MathHelper.Pi / 4f, tex.Size() / 2f, new Vector2(0.5f, 1.75f*scale), default, 0);

			return false;
		}


	}

}
