using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.DataStructures;
using System.IO;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{

	public class ExplosionBoomerang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BOOMerang");
			Tooltip.SetDefault("Explodes when it hits an enemy, does not explode on tiles\nAllows up to 2 at once\n'This is what happens when you let Demolitionist and Tinkerer have too much fun...'");
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.damage = 32;
			Item.crit = 10;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.useTurn = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noUseGraphic = true;
			Item.useTime = 30;
			Item.knockBack = 1f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 30;
			Item.consumable = true;
			Item.value = Item.buyPrice(silver: 15);
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<ExplosionBoomerangProj>();
			Item.shootSpeed = 15f;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 2;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speed = new Vector2(speedX, speedY);

			Projectile.NewProjectile(position.X, position.Y, speed.X, speed.Y, type, damage, knockBack, Main.myPlayer);

			return false;
		}
	}

	public class ExplosionBoomerangProj : ModProjectile
	{
		bool HitEnemy = false;


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BOOMerang");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/ExplosionBoomerang"); }
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.IceBoomerang);
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 0;
			Projectile.tileCollide = true;
		}

		public override void AI()
		{

			int dustIndexsmoke = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
			Main.dust[dustIndexsmoke].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
			Main.dust[dustIndexsmoke].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
			Main.dust[dustIndexsmoke].noGravity = true;
			Main.dust[dustIndexsmoke].position = Projectile.Center + new Vector2(0f, (float)(-(float)Projectile.height / 2)).RotatedBy((double)Projectile.rotation, default(Vector2)) * 1.1f;
			dustIndexsmoke = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
			Main.dust[dustIndexsmoke].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
			Main.dust[dustIndexsmoke].noGravity = true;
			Main.dust[dustIndexsmoke].position = Projectile.Center + new Vector2(0f, (float)(-(float)Projectile.height / 2 - 6)).RotatedBy((double)Projectile.rotation, default(Vector2)) * 1.1f;

		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			HitEnemy = true;
			Projectile.Kill();

		}

		public override bool PreKill(int timeLeft)
		{
			if (HitEnemy == true)
			{
				int proj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, Vector2.Zero.X, Vector2.Zero.Y, ModContent.ProjectileType<CreepersThrowBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0.0f, 0f);
				Main.projectile[proj].timeLeft = 2;
				Main.projectile[proj].netUpdate = true;
			}
			else
			{
				Player player = Main.player[Projectile.owner];
				if ((Projectile.Center - player.MountedCenter).Length() < 80)
					player.QuickSpawnItem(ModContent.ItemType<ExplosionBoomerang>(), 1);


			}

			return true;
		}

	}

	public class Specterang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Specterang");
			Tooltip.SetDefault("Throws a ghostly Boomerang that passes through enemies and walls\nLeaves behind ghostly images of itself that damages enemies");
		}

		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 10;
			Item.damage = 30;
			Item.crit = 10;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.useTurn = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 10;
			Item.useStyle = 5;
			Item.noUseGraphic = true;
			Item.useTime = 10;
			Item.knockBack = 1f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.value = Item.buyPrice(gold: 5);
			Item.rare = ItemRarityID.LightPurple;
			Item.shoot = ModContent.ProjectileType<SpecterangProj>();
			Item.shootSpeed = 20f;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.EnchantedBoomerang, 1).AddIngredient(ItemID.SoulofLight, 6).AddIngredient(ItemID.SpectreBar, 8).AddIngredient(ItemID.Ectoplasm, 6).AddTile(TileID.MythrilAnvil).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speed = new Vector2(speedX, speedY);

			Projectile.NewProjectile(position.X, position.Y, speed.X, speed.Y, type, damage, knockBack, player.whoAmI);

			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			/*if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[item.type];
				Texture2D texture2 = ModContent.GetTexture("SGAmod/Items/GlowMasks/Specterang_Glow");
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, item.position + new Vector2(0, 0) - Main.screenPosition, null, lightColor*0.5f, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, item.position + new Vector2(0, 0) - Main.screenPosition, null, lightColor*0.50f, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture2, item.position + new Vector2(0, 0) - Main.screenPosition, null, lightColor* 0.75f, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
			}*/
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[Item.type];
				Texture2D texture2 = ModContent.Request<Texture2D>("SGAmod/Items/GlowMasks/Specterang_Glow");

				Vector2 slotSize = new Vector2(52f, 52f);
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, Item.Center + new Vector2(0, 0) - Main.screenPosition, null, drawColor * 0.5f, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, Item.Center + new Vector2(0, 0) - Main.screenPosition, null, drawColor * 0.50f, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture2, Item.Center + new Vector2(0, 0) - Main.screenPosition, null, drawColor * 0.75f, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
	}

	public class SpecterangProj : ModProjectile, IDrawAdditive
	{
		protected virtual int ReturnTime => 20;
		protected virtual int ReturnTimeNoSlow => 70;
		protected virtual float SolidAmmount => 6f;
		protected float startSpeed = default;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Specterang");
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_658"); }
		}

		public static void DrawSpecterang(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.itemTexture[ModContent.ItemType<Specterang>()];
			Texture2D tex2 = ModContent.Request<Texture2D>("SGAmod/Items/GlowMasks/Specterang_Glow");

			float alpha = projectile.localAI[0];

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, lightColor * 0.50f * alpha, projectile.rotation, tex.Size() / 2f, projectile.scale, default, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * 0.50f * alpha, projectile.rotation, tex.Size() / 2f, projectile.scale, default, 0);
			spriteBatch.Draw(tex2, projectile.Center - Main.screenPosition, null, Color.White * 0.80f * alpha, projectile.rotation, tex.Size() / 2f, projectile.scale, default, 0);
		}

		public void DrawAdditive(SpriteBatch spriteBatch)
		{
			if (GetType() == typeof(SpecterangProj))
				DrawSpecterang(Projectile, spriteBatch, Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), Color.White));

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[Projectile.type];
			UnifiedRandom rand = new UnifiedRandom(Projectile.whoAmI);
			for (float a = 6f; a > 0; a -= 0.25f)
			{
				float offset = rand.NextFloat(-MathHelper.Pi / 4f, MathHelper.Pi / 4f);
				float scale = (1f - (a / 6f));
				spriteBatch.Draw(tex, Projectile.Center - (Projectile.velocity * a) - Main.screenPosition, null, Color.White * 0.05f * scale, Projectile.rotation + offset, tex.Size() / 2f, new Vector2(2f, 2f) * scale, default, 0);
			}

			return false;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.IceBoomerang);
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.aiStyle = -1;
			Projectile.scale = 1f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
			Projectile.extraUpdates = 2;
			Projectile.tileCollide = false;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void AI()
		{

			bool solid = Projectile.ai[0] >= ReturnTime && Projectile.ai[0] < ReturnTimeNoSlow;

			if (startSpeed == default)
			{
				startSpeed = Projectile.velocity.Length();
				Projectile.aiStyle = (Projectile.velocity.X > 0 ? 1 : -1) - 20;
			}

			Projectile.localAI[0] = 1f;// MathHelper.Clamp(projectile.localAI[0] += (solid ? 0.08f : -0.04f), 0.5f, 1f);

			Projectile.ai[0] += 1;

			Projectile.rotation += 0.4f * (float)(Projectile.aiStyle + 20f);

			if (GetType() == typeof(SpecterangProj))
			{

				if (Projectile.soundDelay == 0)
				{
					Projectile.soundDelay = 10;
					SoundEngine.PlaySound(SoundID.Item7, Projectile.position);
				}
				if (Projectile.ai[0] % 2 == 0)
				{
					int proj = Projectile.NewProjectile(Projectile.Center, -Projectile.velocity * 0.25f, ModContent.ProjectileType<SpecterangProj2>(), Projectile.damage, 0f, Projectile.owner);
					Main.projectile[proj].rotation = Projectile.rotation;
				}
			}

			if (Projectile.ai[0] >= ReturnTime)
			{
				Player owner = Main.player[Projectile.owner];

				Vector2 distmeasure = owner.MountedCenter - Projectile.Center;

				Projectile.velocity += Vector2.Normalize(distmeasure) * (0.70f);

				if (Projectile.ai[0] >= ReturnTimeNoSlow)
				{
					//projectile.velocity *= 0.75f;
					float dist = Math.Min(((Projectile.ai[0] - ReturnTimeNoSlow) / 4f), distmeasure.Length());
					Projectile.Center += Vector2.Normalize(distmeasure) * (0.50f) * dist;
				}

				Projectile.velocity *= 0.99f;
				if (Projectile.velocity.Length() > startSpeed)
					Projectile.velocity = Vector2.Normalize(Projectile.velocity) * startSpeed;

				if (Main.myPlayer == Projectile.owner)
				{
					Rectangle rectangle = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
					Rectangle value2 = new Rectangle((int)Main.player[Projectile.owner].position.X, (int)Main.player[Projectile.owner].position.Y, Main.player[Projectile.owner].width, Main.player[Projectile.owner].height);
					if (rectangle.Intersects(value2))
					{
						Projectile.Kill();
					}
				}

			}

		}

	}

	public class SpecterangProj2 : ModProjectile, IDrawAdditive
	{

		protected float startSpeed = default;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Specterang Shadow");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Specterang"); }
		}
		public void DrawAdditive(SpriteBatch spriteBatch)
		{
			SpecterangProj.DrawSpecterang(Projectile, spriteBatch, Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), Color.White));
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj2>()];
			float scale = (float)Projectile.timeLeft / 20f;
			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * 0.5f* scale, Projectile.rotation, tex.Size() / 2f, new Vector2(2f, 2f) * scale, default, 0);

			return false;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.IceBoomerang);
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 10;
			Projectile.aiStyle = -1;
			Projectile.scale = 1f;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.extraUpdates = 0;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Projectile.localAI[0] = (float)Projectile.timeLeft / 15f;
		}

	}

	public class Wirang : ModItem
	{
		static int wireType=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wirang");
			Tooltip.SetDefault("Throws multi-color Boomerang-Shaped wire bundles\nThese activate their respective wires when they pass over them\nThrows up to 4 at once");
		}

		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.damage = 32;
			Item.crit = 5;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.useTurn = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 10;
			Item.useStyle = 5;
			Item.noUseGraphic = true;
			Item.useTime = 10;
			Item.knockBack = 0f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.value = Item.buyPrice(gold: 5);
			Item.rare = ItemRarityID.LightPurple;
			Item.shoot = ModContent.ProjectileType<WirangProj>();
			Item.shootSpeed = 10f;
			Item.mech = true;
		}
		public override string Texture
		{
			get { return ("Terraria/UI/Wires_2"); }
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 4;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speed = new Vector2(speedX, speedY);
			speed.RotatedByRandom(MathHelper.Pi / 8f);

			int prog = Projectile.NewProjectile(position.X, position.Y, speed.X, speed.Y, type, damage, knockBack, Main.myPlayer);
			Main.projectile[prog].localAI[1] = wireType;
			Main.projectile[prog].netUpdate = true;
			wireType = (wireType + 1) % 4;

			return false;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Texture2D inner = Main.wireUITexture[2 + ((int)(Main.GlobalTimeWrappedHourly)%4)];

			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);

			spriteBatch.Draw(inner, drawPos, null, Color.White, 0, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);

			return false;
		}
	}

	public class WirangProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wirang");
		}

		public override string Texture
		{
			get { return ("Terraria/UI/Wires_2"); }
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write((int)Projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			Projectile.localAI[1] = reader.ReadInt16();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.wireUITexture[2+(int)Projectile.localAI[1]];
			spriteBatch.Draw(tex, Projectile.Center - Projectile.velocity - Main.screenPosition, null, lightColor, Projectile.rotation, tex.Size() / 2f, Projectile.scale, default, 0);
			return false;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenBoomerang);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 0;
		}

        public override void AI()
        {
			Point16 here = new Point16((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16);
			if (WorldGen.InWorld(here.X, here.Y))
			{
				Tile tile = Main.tile[here.X, here.Y];
				if ((tile.wire() && Projectile.localAI[1] == 0) ||
				(tile.wire3() && Projectile.localAI[1] == 1) ||
				(tile.wire2() && Projectile.localAI[1] == 2) ||
				(tile.wire4() && Projectile.localAI[1] == 3))
				{
					Wiring.TripWire(here.X, here.Y, 1, 1);
					if (Projectile.extraUpdates<1)
					Projectile.extraUpdates = 1;
				}
			}
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			knockback /= 2f;
		}

	}

	public class Fridgeflamarang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fridgeflamarang");
			Tooltip.SetDefault("Throws an Icy and Flaming Boomerang pair guided by a light that split apart on hit\nUsable only when less than 4 Boomerangs are active");
		}

		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 10;
			Item.damage = 50;
			Item.crit = 5;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.useTurn = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 10;
			Item.useStyle = 5;
			Item.noUseGraphic = true;
			Item.useTime = 10;
			Item.knockBack = 4f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.value = Item.buyPrice(gold: 5);
			Item.rare = ItemRarityID.LightPurple;
			Item.shoot = ModContent.ProjectileType<FridgeflamarangProj>();
			Item.shootSpeed = 25f;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1 && (player.ownedProjectileCounts[ProjectileID.Flamarang] + player.ownedProjectileCounts[ProjectileID.IceBoomerang] < 4);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Fridgeflame>(), 10).AddIngredient(ItemID.SoulofLight, 5).AddIngredient(ItemID.Flamarang, 1).AddIngredient(ItemID.IceBoomerang, 1).AddTile(TileID.MythrilAnvil).Register();
		}

		/*public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= Main.dayTime ? 0.50f : 1f;
		}*/

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speed = new Vector2(speedX, speedY);
			speed.RotatedByRandom(MathHelper.Pi / 8f);

			Projectile.NewProjectile(position.X, position.Y, speed.X, speed.Y, type, (int)(damage * 0.10f), knockBack, Main.myPlayer);

			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, Color.White, 0f, textureOrigin, 0.50f, SpriteEffects.FlipHorizontally, 0f);

				/*texture = Main.itemTexture[ItemID.IceBoomerang];
				textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);

				spriteBatch.Draw(texture, item.Center + new Vector2(-8, 0) - Main.screenPosition, null, lightColor, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(Main.itemTexture[ItemID.Flamarang], item.Center + new Vector2(8, 0) - Main.screenPosition, null, lightColor, 0f, textureOrigin, 1f, SpriteEffects.FlipHorizontally, 0f);*/
			}
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
				Vector2 slotSize = new Vector2(52f, 52f);

				Texture2D texture = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				Vector2 drawPos = (position) + (slotSize * Main.inventoryScale) *0.31f;
				spriteBatch.Draw(texture, drawPos, null, drawColor, 0f, textureOrigin, Main.inventoryScale/2.5f, SpriteEffects.None, 0f);

				/*texture = Main.itemTexture[ItemID.IceBoomerang];
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				drawPos = position + slotSize * Main.inventoryScale / 2f;
				textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, drawPos + new Vector2(-8, 0), null, drawColor, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);
				spriteBatch.Draw(Main.itemTexture[ItemID.Flamarang], drawPos + new Vector2(8, 0), null, drawColor, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.FlipHorizontally, 0f);*/
			}
		}
	}

	public class FridgeflamarangProj : CoralrangProj
	{
		protected override int maxOrbiters => 2;
		protected override float damageMul => 10f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunarang");
		}
		public FridgeflamarangProj()
		{
			orbitors = new Projectile[maxOrbiters];
			spinners = new float[maxOrbiters];
			projID = new int[] {ProjectileID.IceBoomerang, ProjectileID.Flamarang };
			spinDist = 10f;
			spinDiv = 16f;// (MathHelper.TwoPi*100f)+MathHelper.Pi;
			spinVelocity = 12f;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_658"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[Projectile.type];
			spriteBatch.Draw(tex, Projectile.Center - Projectile.velocity - Main.screenPosition, null, Color.White, Projectile.velocity.X / 20f, tex.Size() / 2f,0.5f, default, 0);
			return false;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.IceBoomerang);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 1;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			knockback /= 5f;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.Kill();
		}

	}

	public class Coralrang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coralrang");
			Tooltip.SetDefault("Throws a returning clump of razer sharp coral that splits apart on hit\nThe clump and splitting projectiles cannot hit the same target");
		}

		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 10;
			Item.damage = 42;
			Item.crit = 5;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.useTurn = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 35;
			Item.useStyle = 5;
			Item.noUseGraphic = true;
			Item.useTime = 35;
			Item.knockBack = 4f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<CoralrangProj>();
			Item.shootSpeed = 32f;
		}
		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Coral, 10).AddIngredient(ModContent.ItemType<HavocGear.Items.BiomassBar>(), 8).AddTile(TileID.WorkBenches).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speed = new Vector2(speedX, speedY);
			speed.RotatedByRandom(MathHelper.Pi / 8f);
			Projectile.NewProjectile(position.X, position.Y, speed.X, speed.Y, type, damage, knockBack, Main.myPlayer);

			return false;
		}
	}

	public class CoralrangProj : ModProjectile
	{
		protected virtual int maxOrbiters => 5;
		protected int[] projID;
		protected float spinDist = 4f;
		protected float spinRand = 0f;
		protected float spinDiv = 8f;
		protected float spinVelocity = 0f;
		protected float angleOffset = 0f;//MathHelper.Pi;
		protected Projectile[] orbitors;
		protected float[] spinners;
		protected virtual float damageMul => 0.75f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coralrang");
		}

		public CoralrangProj()
		{
			orbitors = new Projectile[maxOrbiters];
			spinners = new float[maxOrbiters];
			projID = new int[] {ModContent.ProjectileType<CoralrangProj2>()};
			spinRand = MathHelper.Pi / 2f;
		}

		public override string Texture
		{
			get { return ("Terraria/Tiles_" + TileID.Coral); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.IceBoomerang);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 0;
		}

		public override void SendExtraAI(System.IO.BinaryWriter writer)
		{
			for (int i = 0; i < orbitors.Length; i += 1)
			{
				if (orbitors[i]==null)
					writer.Write(-1);
				else
					writer.Write((ushort)orbitors[i].whoAmI);
			}
		}

		public override void ReceiveExtraAI(System.IO.BinaryReader reader)
		{
			for (int i = 0; i < orbitors.Length; i += 1)
			{
				int theyare = (int)reader.ReadUInt16();
				if (theyare > -1)
				{
					Projectile proj = Main.projectile[theyare];

					if (proj != null && proj.active && projID.FirstOrDefault(type => type == orbitors[i].type) != default)
						orbitors[i] = Main.projectile[theyare];
				}
			}
		}

		public override void AI()
		{

			if (Projectile.ai[1]>0)
			Projectile.ai[1] += 2.5f;

			if (Projectile.ai[0] > 0)
            {
				Player P = Main.player[Projectile.owner];
				Vector2 dist = P.Center - Projectile.Center;
				Projectile.velocity += Vector2.Normalize(dist) *MathHelper.Clamp(dist.Length()/180f,0f,1f);
            }

			for (int k = 0; k < orbitors.Length; k += 1)
			{
				if (spinners[k] == default)
				{
					spinners[k] = Main.rand.NextFloat(spinRand);
					Vector2 movespeed = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 2f));
					int newb = Projectile.NewProjectile(Projectile.Center, movespeed, projID[k % projID.Length], (int)(Projectile.damage * damageMul), Projectile.knockBack, Main.myPlayer, 0, Main.rand.Next(4));
					Main.projectile[newb].tileCollide = false;
					Main.projectile[newb].rotation += MathHelper.TwoPi*(k/ orbitors.Length);
					Main.projectile[newb].netUpdate = true;
					orbitors[k] = Main.projectile[newb];
					Projectile.netUpdate = true;
				}
				else
				{
					if (orbitors[k] != null)
					{
						if (projID.FirstOrDefault(type => type == orbitors[k].type) != default)
						{
							float rotate = MathHelper.TwoPi * (Projectile.rotation / spinDiv);

							if (spinDiv > 100f)
								rotate = Projectile.rotation + spinDiv;

							float anglex = (k / (float)orbitors.Length) * MathHelper.TwoPi;
							float anglez = (anglex + spinners[k]) + (rotate);

							Vector2 loc = new Vector2((float)Math.Cos(anglez), (float)Math.Sin(anglez));
							Vector2 gohere = Projectile.Center + loc;

							if (spinDiv <= 100f)
								orbitors[k].rotation = (loc).ToRotation() + angleOffset;

							orbitors[k].Center = gohere + loc * spinDist;
							if (spinVelocity > 0f)
							{
								orbitors[k].velocity = loc * spinVelocity;
							}

							if (orbitors[k].type == ModContent.ProjectileType<CoralrangProj2>())
							{
								orbitors[k].timeLeft = 300;
								orbitors[k].localAI[1] = 100;
							}
						}
					}
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int k = 0; k < orbitors.Length; k += 1)
			{
				if (orbitors[k] != null)
				{
					if (projID.FirstOrDefault(type => type == orbitors[k].type) != default)
					{
						orbitors[k].ai[0] = target.whoAmI;
						orbitors[k].localAI[1] = 3;
						orbitors[k].timeLeft = 300;
						if (orbitors[k].type != ModContent.ProjectileType<CoralrangProj2>())
							orbitors[k].tileCollide = true;
						orbitors[k].netUpdate = true;
					}
				}
			}

			Projectile.Kill();
		}
	}

	public class CoralrangProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coralrang");
		}

		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.ignoreWater = true;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 2;
			Projectile.DamageType = DamageClass.Melee;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return target.whoAmI != (int)Projectile.ai[0] && Projectile.localAI[1]<1;
		}

		public override string Texture
		{
			get { return ("Terraria/Tiles_" + TileID.Coral); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[Projectile.type];
			int frames = tex.Width / 6;
			Vector2 offset = new Vector2(frames,tex.Height) / 2f;

			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle((int)Projectile.ai[1] * frames, 0, frames, tex.Height), lightColor,Projectile.rotation,offset,Projectile.scale,SpriteEffects.None,0);
			return false;
		}

		public override void AI()
		{
			Projectile.localAI[1] -= 1;

			if (Projectile.localAI[1] < 97 && Projectile.localAI[1] > 80)
			{
				Projectile.localAI[1] = 9001;
				Projectile.Kill();
			}

			if ((int)Projectile.localAI[0] == 0)
			{
				Projectile.localAI[0] = Main.rand.NextFloat(3f, 30f) * (Main.rand.NextBool() ? 1f : -1f);
			}

			if (Projectile.localAI[1] < 1)
			{
				Projectile.tileCollide = true;
				Projectile.rotation += (Projectile.localAI[0]-Math.Sign(Projectile.localAI[0]*2.5f))*0.02f;
				Projectile.velocity.Y += 0.125f;
			}
			else
			{
				Projectile.position -= Projectile.velocity;
			}

			Vector2 velo = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-1f, 1f));
			Dust dust = Dust.NewDustPerfect(Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), Main.rand.NextFloat(Projectile.height)), 33, velo,150,Color.White,0.75f);
			dust.noGravity = false;

		}

		public override bool PreKill(int timeLeft)
		{
			if (Projectile.localAI[1] < 9001)
			{
				for (int i = 0; i < 20; i += 1)
				{
					Vector2 velo = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-1f, 1f));
					Dust dust = Dust.NewDustPerfect(Projectile.position + new Vector2(Main.rand.NextFloat(Projectile.width), Main.rand.NextFloat(Projectile.height)), 33, velo, 150, Color.White, 1f);
					dust.noGravity = false;
				}
			}
			return true;
		}

	}
}
