using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class TrueMoonlight : ModItem
	{
		bool altfired = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Moonlight");
			Tooltip.SetDefault("Hold left click to raise your sword to the stars and power up your slash!\nLv2 Slash inflicts Moon Light Curse on enemies\nThis debuff massively reduces their defense and taking damage over time\nLv3 slash does massive damage and homes in nearby enemies\nHold right click to auto swing basic slash waves at less accuracy\nHaving a faster Melee Swing Speed charges the blade faster");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 175;
			Item.crit = 5;
			Item.DamageType = DamageClass.Melee;
			Item.width = 44;
			Item.height = 52;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.knockBack = 10;
			Item.value = Item.sellPrice(1, 0, 0, 0);
			Item.rare = 9;
			//item.UseSound = SoundID.Item71;
			Item.shoot = Mod.Find<ModProjectile>("MoonlightWaveLv1").Type;
			Item.shootSpeed = 30f;
			Item.autoReuse = false;
			Item.useTurn = false;

		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{

			altfired = player.altFunctionUse == 2 ? true : false;

			if (!altfired)
			{
				Item.autoReuse = true;
				Item.channel = true;
				Item.useStyle = 5;
				Item.useTime = 60;
				Item.useAnimation = 60;
				Item.noMelee = true;
			}
			else
			{
				Item.autoReuse = false;
				Item.channel = false;
				Item.useStyle = 1;
				Item.useTime = 20;
				Item.useAnimation = 20;
			}

			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speedz = new Vector2(speedX, speedY);
			speedz.Normalize(); speedz *= 30f; speedX = speedz.X; speedY = speedz.Y;


			if (!altfired)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("TrueMoonlightCharging").Type, damage, knockBack, player.whoAmI);
				//Main.projectile[proj].localAI[2] = 60f/(float)player.itemAnimation;
				//Main.projectile[proj].netUpdate = true;
				return false;
			}

			float speed = 1.5f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(8);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			SoundEngine.PlaySound(SoundID.Item71, player.Center);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("MoonlightWaveLv1").Type, damage, knockBack, player.whoAmI);
			}
			return false;
		}

		public override bool? CanHitNPC(Player player, NPC target)
		{
			return (player.ownedProjectileCounts[Mod.Find<ModProjectile>("TrueMoonlightCharging").Type] < 1 && !target.friendly);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("TrueMoonlightCharging").Type] > 0)
				return;

			for (int bbbb = 52; bbbb < 128; bbbb += Main.rand.Next(8, 24))
			{
				Vector2 eree = ((player.itemRotation) + MathHelper.ToRadians(player.direction > 0 ? -40 : 220)).ToRotationVector2();
				int num467 = Dust.NewDust(new Vector2(player.Center.X - 2, player.Center.Y - 2) + (eree * bbbb * (Item.scale)), 4, 4, 235, eree.RotatedBy(MathHelper.ToRadians(90)).X, eree.RotatedBy(MathHelper.ToRadians(90)).Y, 100, default(Color), 0.50f);
				Main.dust[num467].noGravity = true;
			}

			for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 187);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
				Main.dust[dust].noGravity = true;
				//Main.dust[dust].velocity.Normalize();
				//Main.dust[dust].velocity+=new Vector2(player.velocity.X/4,0f);
				//Main.dust[dust].velocity*=(((float)Main.rand.Next(0,100))/30f);
			}

			for (int num475 = 3; num475 < 5; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 27);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 15f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 3f) + ((player.direction) * player.itemRotation.ToRotationVector2() * (float)num475);
				Main.dust[dust].noGravity = true;
				//Main.dust[dust].velocity.Normalize();
				//Main.dust[dust].velocity+=new Vector2(player.velocity.X/4,0f);
				//Main.dust[dust].velocity*=(((float)Main.rand.Next(0,100))/30f);
			}

			Lighting.AddLight(player.position, 0.1f, 0.1f, 0.9f);
		}

		public override void AddRecipes()
		{
			/*ModRecipe recipe = new StarMetalRecipes(mod);
			recipe.AddIngredient(mod.ItemType("SwordofTheBlueMoon"), 1);
			recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
			recipe.AddIngredient(mod.ItemType("IlluminantEssence"), 20);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 15);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();*/
		}


	}

	public class TrueMoonlightCharging : ModProjectile
	{

		int leveleffect = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/DragonRevolver"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override bool CanDamage()
		{
			return false;
		}

		/*public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)projectile.localAI[2]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.localAI[2] = (float)reader.ReadDouble();
		}*/

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			AIType = 0;
		}

		public override void AI()
		{
			Vector2 mousePos = Main.MouseWorld;
			Player player = Main.player[Projectile.owner];

			if (Projectile.ai[0] > 1000f || player.dead)
			{
				Projectile.Kill();
			}
			Projectile.ai[1] += Math.Max(0.75f, (player.GetModPlayer<SGAPlayer>().mspeed * 1f) - 0.00f);
			Projectile.localAI[1] += 0.2f;
			if ((!player.channel || Projectile.ai[0] > 0))
			{
				Projectile.ai[0] += 1;
				Projectile.netUpdate = true;
			}
			Projectile.timeLeft = 2;
			// Multiplayer support here, only run this code if the client running it is the owner of the projectile
			if (Projectile.owner == Main.myPlayer)
			{
				Vector2 diff = mousePos - player.Center;
				diff.Normalize();
				Projectile.velocity = diff;
				if (Projectile.ai[0] < 50f)
					Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
				Projectile.netUpdate = true;
				Projectile.Center = mousePos;
			}
			int dir = Projectile.direction;
			player.ChangeDir(dir);
			if (player.itemTime < 5)
				player.itemTime = 5;
			if (player.itemAnimation < 5)
				player.itemAnimation = 5;
			player.itemRotation = (float)Math.Atan2(-32f * dir, 0f * dir);

			if (Projectile.ai[1] > 150)
				Projectile.ai[1] = 150;

			for (float num475 = 0; num475 < Projectile.ai[1]; num475 += 5)
			{
				if (Main.rand.Next(0, 100) < 20)
				{
					int dust = Dust.NewDust(new Vector2(player.Center.X - 2, -56 + (player.Center.Y - (num475) / 2)), 0, 0, 185);

					Main.dust[dust].position.X += (float)Main.rand.Next(-200, 200) * 0.075f;
					Main.dust[dust].scale = 1.2f;
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Main.dust[dust].velocity = (randomcircle / 2f) - new Vector2(0, -4f);
					Main.dust[dust].noGravity = true;
				}
			}

			float val = 50;
			if (Projectile.ai[1] < val || (Projectile.ai[1] > val && leveleffect < 1))
			{
				if (Projectile.ai[1] > val)
				{
					leveleffect = 1;
					SoundEngine.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 60, 1f, 0.25f);
				}
				for (float num476 = 0; num476 < 3 + (leveleffect * 25); num476 += 1)
				{
					int dust = Dust.NewDust(new Vector2(player.Center.X - 2, -56 + (player.Center.Y - (val) / 2)), 0, 0, 187);

					Main.dust[dust].position.X += (float)Main.rand.Next(-200, 200) * 0.1f;
					Main.dust[dust].scale = 1.15f;
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Main.dust[dust].velocity.X += (randomcircle.X) * (float)leveleffect * 15f;
					Main.dust[dust].velocity.X += (randomcircle.Y) * (float)leveleffect * 2f;
					Main.dust[dust].noGravity = true;
				}

			}

			float val2 = 148;
			if (Projectile.ai[1] < val2 || (Projectile.ai[1] > val2 && leveleffect > 0 && leveleffect < 2))
			{
				int trueval = 0;
				if (Projectile.ai[1] > val2)
				{
					leveleffect = 2;
					trueval = 1;
					SoundEngine.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 60, 1f, 0.75f);
				}
				for (float num476 = 0; num476 < 3 + (trueval * 50); num476 += 1)
				{
					int dust = Dust.NewDust(new Vector2(player.Center.X - 2, -52 + (player.Center.Y - (val2) / 2)), 0, 0, 187);

					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Main.dust[dust].position += randomcircle * 3f;
					Main.dust[dust].scale = 1.15f;
					randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Main.dust[dust].velocity += (randomcircle);
					if (trueval > 0)
						Main.dust[dust].velocity += randomcircle * (float)(Main.rand.Next(5, 15));
					Main.dust[dust].noGravity = true;
				}

			}


			if (Projectile.ai[0] > 1)
			{
				player.itemTime = 30;
				player.itemAnimation = 20;
				player.HeldItem.useStyle = 1;
				player.HeldItem.noMelee = false;

				double damagemul = 1.0;
				int slashwaveprojtype = Mod.Find<ModProjectile>("MoonlightWaveLv1").Type;
				if (Projectile.ai[1] > 50)
				{
					slashwaveprojtype = Mod.Find<ModProjectile>("MoonlightWaveLv2").Type;
					damagemul = 2.50;
				}
				if (Projectile.ai[1] > 149)
				{
					slashwaveprojtype = Mod.Find<ModProjectile>("MoonlightWaveLv3").Type;
					damagemul = 4.00;
				}

				float numberProjectiles = 1;
				float rotation = MathHelper.ToRadians(2);
				SoundEngine.PlaySound(SoundID.Item71, player.Center);

				Vector2 projhere = player.Center + (Projectile.velocity * 42f);
				float velincrease = 26f + (Projectile.ai[1] / 6f);

				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = (Projectile.velocity * velincrease).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
					int proj = Projectile.NewProjectile(projhere.X, projhere.Y, perturbedSpeed.X, perturbedSpeed.Y, slashwaveprojtype, (int)(Projectile.damage * damagemul), Projectile.knockBack, player.whoAmI);
				}

				Projectile.Kill();

			}


		}


	}

	public class SwordofTheBlueMoon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword of the Blue Moon");
			Tooltip.SetDefault("Born from a great white being?\nI'm sure Moon Lord fits that description");
		}

		public override void SetDefaults()
		{
			Item.damage = 160;
			Item.DamageType = DamageClass.Melee;
			Item.width = 44;
			Item.height = 52;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.knockBack = 10;
			Item.value = 250000;
			Item.rare = 9;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = false;
			Item.useTurn = false;

		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 20);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
				Main.dust[dust].noGravity = true;
				//Main.dust[dust].velocity.Normalize();
				//Main.dust[dust].velocity+=new Vector2(player.velocity.X/4,0f);
				//Main.dust[dust].velocity*=(((float)Main.rand.Next(0,100))/30f);
			}

			for (int num475 = 3; num475 < 5; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 27);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 15f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 3f) + ((player.direction) * player.itemRotation.ToRotationVector2() * (float)num475);
				Main.dust[dust].noGravity = true;
				//Main.dust[dust].velocity.Normalize();
				//Main.dust[dust].velocity+=new Vector2(player.velocity.X/4,0f);
				//Main.dust[dust].velocity*=(((float)Main.rand.Next(0,100))/30f);
			}

			Lighting.AddLight(player.position, 0.1f, 0.1f, 0.9f);
		}
	}

}