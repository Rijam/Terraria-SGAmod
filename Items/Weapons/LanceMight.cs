using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons
{
	public class LanceMight : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lance a-lot");
			Tooltip.SetDefault("'What's better than a lance? How about A LOT of them'");
		}

		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 10;
			Item.damage = 32;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.useTurn = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 15;
			Item.useStyle = 5;
			Item.useTime = 15;
			Item.knockBack = 5f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.value = 100000;
			Item.rare = 6;
			Item.shoot = Mod.Find<ModProjectile>("LanceMightProj").Type;
			Item.shootSpeed = 7f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Marble, 50).AddIngredient(null,"DankWood", 20).AddIngredient(ItemID.DarkLance, 1).AddTile(TileID.MythrilAnvil).Register();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
					for (int i = -120; i < 121; i += 40)
					{
				Vector2 speed = new Vector2(speedX, speedY);
				speed *= 1f - (float)(Math.Abs(i) / 200f);
				int thisoned = Projectile.NewProjectile(position.X, position.Y, speed.RotatedBy(MathHelper.ToRadians(i)).X, speed.RotatedBy(MathHelper.ToRadians(i)).Y, type, damage, knockBack, Main.myPlayer);
					}

			return false;
		}
	}
	public class LanceMightProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lance");
		}

		public override void SetDefaults()
		{
			Projectile.width = 42;
			Projectile.height = 42;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.aiStyle = 19;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 90;
			Projectile.hide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override void AI()
		{
			Main.player[Projectile.owner].direction = Projectile.direction;
			//Main.player[projectile.owner].heldProj = projectile.whoAmI;
			Main.player[Projectile.owner].itemTime = Main.player[Projectile.owner].itemAnimation;
			Projectile.position.X = Main.player[Projectile.owner].position.X + (float)(Main.player[Projectile.owner].width / 2) - (float)(Projectile.width / 2);
			Projectile.position.Y = Main.player[Projectile.owner].position.Y + (float)(Main.player[Projectile.owner].height / 2) - (float)(Projectile.height / 2);
			Projectile.position += Projectile.velocity * Projectile.ai[0];
			if (Main.rand.Next(4) == 0)
			{
				int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.TornadoDust>(), 0f, 0f, 254, default(Color), 0.3f);
				Main.dust[dustIndex].velocity += Projectile.velocity * 0.5f;
				Main.dust[dustIndex].velocity *= 0.5f;
				return;
			}
			if (Projectile.ai[0] == 0f)
			{
				Projectile.ai[0] = 3f;
				Projectile.netUpdate = true;
			}
			if (Main.player[Projectile.owner].itemAnimation < Main.player[Projectile.owner].itemAnimationMax / 3)
			{
				Projectile.ai[0] -= 2.4f;
				if (Projectile.localAI[0] == 0f && Main.myPlayer == Projectile.owner)
				{
					Projectile.localAI[0] = 1f;
					//Projectile.NewProjectile(projectile.Center.X + projectile.velocity.X, projectile.Center.Y + projectile.velocity.Y, projectile.velocity.X * 1f, projectile.velocity.Y * 1f, mod.ProjectileType("AcidSpear"), (int)((double)projectile.damage * 0.5), projectile.knockBack * 0.85f, projectile.owner, 0f, 0f);
				}
			}
			else
			{
				Projectile.ai[0] += 0.95f;
			}

			if (Main.player[Projectile.owner].itemAnimation == 0)
			{
				Projectile.Kill();
			}

			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 2.355f;
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation -= 1.57f;
			}
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
		{

			bool facingleft = Projectile.direction < 0f;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally;
			Texture2D texture = Main.projectileTexture[Projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			if (facingleft)
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), drawColor, ((Projectile.rotation - (float)Math.PI / 2) - (float)Math.PI / 2) + (facingleft ? (float)(1f * Math.PI) : 0f), origin, Projectile.scale, effect, 0);
			if (!facingleft)
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), drawColor, (Projectile.rotation - (float)Math.PI / 2) + (facingleft ? (float)(1f * Math.PI) : 0f), origin, Projectile.scale, SpriteEffects.None, 0);

			return false;
		}
	}
}
