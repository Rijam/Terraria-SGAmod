using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using SGAmod.Projectiles;
using Idglibrary;
using SGAmod.Buffs;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class GlassSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glass Sword");
			Tooltip.SetDefault("Shatters on the first hit, throwing out several glass shards\nEnemies hit with the broken edge afterwords are cut deep with gourged\nThis weapon ignores enemy defense");
			Item.staff[Item.type] = true; 
		}

		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.maxStack = 99;
			Item.crit = 0;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 2;
			Item.useAnimation = 22;
			Item.reuseDelay = 30;
			Item.consumable = true;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.noUseGraphic = true;
			Item.value = Item.sellPrice(0, 0, 0, 5);
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.useTurn = false;
			Item.autoReuse = true;
		}

		public override bool ConsumeItem(Player player)
		{
			return player.itemAnimation>0;
		}

		public override bool CanUseItem(Player player)
		{
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Weapons/GlassSword").Value;
				Main.itemTexture[Item.type] = Mod.Assets.Request<Texture2D>("Items/Weapons/GlassSword").Value;
			}
			Item.width = 54;
			Item.height = 54;
			Item.knockBack = 2;
			Item.noMelee = false;
			return true;
		}

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
			damage += target.defense/2;
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (Item.knockBack > 0)
			{

				player.ConsumeItem(Item.type);
				SoundEngine.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 27, 0.75f, 0f);

				for (float i = 24; i < 80; i += 20)
				{
					Vector2 position = player.Center;

					Vector2 eree = player.itemRotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(-45f * player.direction));
					eree *= player.direction;

					position += eree * i;

					int thisoned = Projectile.NewProjectile(position.X, position.Y, eree.X * Main.rand.NextFloat(2.4f, 5f), eree.X * Main.rand.NextFloat(0.5f, 2f), Mod.Find<ModProjectile>("BrokenGlass").Type, damage, 0f, Main.myPlayer);

				}

				if (!Main.dedServ)
				{
					Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Weapons/GlassSwordBreak").Value;
				}

            }
            else
            {
				target.AddBuff(ModContent.BuffType<Gourged>(), 60 * 12);
			}
			player.itemWidth = 24;
			player.itemHeight = 24;

			if (!Main.dedServ)
			{
				Main.itemTexture[Item.type] = Mod.Assets.Request<Texture2D>("Items/Weapons/GlassSwordBreakSmol").Value;
				Item.width = Main.itemTexture[Item.type].Height;
				Item.height = Main.itemTexture[Item.type].Width;
			}



			Item.width = 24;
			Item.height = 24;

			Item.knockBack = 0;
		}

		public override void AddRecipes()
		{
			CreateRecipe(20).AddIngredient(ItemID.Glass, 4).AddTile(TileID.WorkBenches).Register();
		}
	
	}

	public class BrokenGlass : ModProjectile
	{

		public virtual bool hitwhilefalling => false;
		public virtual float trans => 1f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Broken Glass");
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Melee;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
		{
			//HairShaderData shader = GameShaders.Hair.GetShaderFromItemId(ItemID.LeinforsAccessory);
			bool facingleft = Projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
			Texture2D texture = Main.itemTexture[ItemID.GlassBowl];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), drawColor * trans, Projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f), origin, Projectile.scale, facingleft ? effect : SpriteEffects.None, 0);
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 50, 0.50f, 0f);
			Projectile.type = ProjectileID.Fireball;
			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			for (int num315 = 0; num315 < 40; num315 = num315 + 1)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				randomcircle *= Main.rand.NextFloat(0f, 3f);
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Ice, 0, 0, 50, Color.Gray, Projectile.scale * 0.5f);
				Main.dust[num316].noGravity = false;
				Main.dust[num316].velocity = new Vector2(randomcircle.X, randomcircle.Y)+Projectile.velocity;
			}

			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return true;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.velocity.Y < 0 && hitwhilefalling)
				return false;
			if (Projectile.ai[1] < 5)
				return false;
			return base.CanHitNPC(target);
		}

		public override void AI()
		{

			Tile tile = Main.tile[(int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16];
			if (tile != null)
				if (tile.liquid > 64)
					Projectile.Kill();

			//projectile.scale = ((float)projectile.width / 14f);

			Projectile.velocity.Y += 0.1f;
			Projectile.rotation += Projectile.velocity.X * 0.1f;

			Projectile.ai[1] += 1;
		}


	}



}