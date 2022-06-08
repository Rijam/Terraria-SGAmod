using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Idglibrary;
using Terraria.DataStructures;
using SGAmod.Buffs;
using Terraria.Audio;

namespace SGAmod.Items.Weapons.SeriousSam
{
	public class LavaRocksGun : SeriousSamWeapon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 1.25f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Rocks Gun");
			Tooltip.SetDefault("Launches Molten rocks that split apart when hitting a target\nA direct hit with the large rock does 3X damage\nSplash damage engulfs enemies in lava for 5 seconds");
		}
		
		public override void SetDefaults()
		{
			Item.damage = 60;
			Item.crit = 10;
			Item.useStyle = 5;
			Item.autoReuse = true;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.width = 50;
			Item.height = 20;
			Item.shoot = Mod.Find<ModProjectile>("LavaRocks").Type;
			Item.UseSound = SoundID.Item11;
			Item.shootSpeed = 8f;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.knockBack = 7f;
			Item.rare = 7;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 8;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.JackOLanternLauncher, 1).AddIngredient(ItemID.MeteoriteBar, 8).AddIngredient(ItemID.LihzahrdPowerCell, 2).AddIngredient(mod.ItemType("ManaBattery"), 3).AddIngredient(mod.ItemType("FieryShard"), 5).AddIngredient(mod.ItemType("AdvancedPlating"), 5).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, -6);
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			float speed=8f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(7);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY)*speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0,100)/100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				int proj=Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI,Main.rand.Next(0,3));
				Main.projectile[proj].friendly=true;
				Main.projectile[proj].hostile=false;
				Main.projectile[proj].timeLeft=600;
				Main.projectile[proj].knockBack=Item.knockBack;
				if (i > 0)
				{
					Main.projectile[proj].width = 15;
					Main.projectile[proj].height = 15;
				}
				Main.projectile[proj].netUpdate = true;
			}
			return false;
		}
				
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(1) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 15);
            }
        }
	
	}

	public class LavaRocks : ModProjectile
	{

		bool hittile = false;
		public virtual bool hitwhilefalling => false;
		public virtual float trans => 1f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Rocks");
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.usesLocalNPCImmunity = true;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public void DrawLava()
        {
			bool facingleft = Projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
			Texture2D texture = SGAmod.ExtraTextures[104 + (int)Projectile.ai[0]];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), Color.White * trans, Projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f), origin, Projectile.scale, facingleft ? effect : SpriteEffects.None, 0);

		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
		{
			if (!SGAConfigClient.Instance.LavaBlending)
			DrawLava();
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//target.immune[projectile.owner] = 15;
		}

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			if (Projectile.width > 16)
				damage *= 3;

		}

        public override bool PreKill(int timeLeft)
		{
			Projectile.type = ProjectileID.Fireball;
			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			for (int num315 = 0; num315 < 40; num315 = num315 + 1)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				randomcircle*=Main.rand.NextFloat(2f, 6f);
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X - 1, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type, 0,0, 50, Main.hslToRgb(0.15f, 1f, 1.00f), Projectile.scale*2f);
				Main.dust[num316].noGravity = false;
				Main.dust[num316].velocity = new Vector2(randomcircle.X, randomcircle.Y);		
			}
			if (Projectile.width > 16)
			{
				for (int num315 = 1; num315 < 4; num315 = num315 + 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					float velincrease = Main.rand.NextFloat(4f,8f);
					int thisone = Projectile.NewProjectile(Projectile.Center.X - Projectile.velocity.X, Projectile.Center.Y - Projectile.velocity.Y, randomcircle.X * velincrease, randomcircle.Y * velincrease, ModContent.ProjectileType<LavaRocks>(), (int)(Projectile.damage * 0.75), Projectile.knockBack, Projectile.owner, 0.0f, 0f);
					Main.projectile[thisone].netUpdate = true;
					Main.projectile[thisone].friendly = Projectile.friendly;
					Main.projectile[thisone].hostile = Projectile.hostile;
					Main.projectile[thisone].width = Main.projectile[thisone].width - 6;
					Main.projectile[thisone].height = Main.projectile[thisone].height - 6;
					if (hittile)
					Main.projectile[thisone].velocity.Y = -Math.Abs(Main.projectile[thisone].velocity.Y);
					IdgProjectile.Sync(thisone);
				}
			}

			int theproj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("Explosion").Type, (int)((double)Projectile.damage * 0.25f), Projectile.knockBack, Projectile.owner, 0f, 0f);
			Main.projectile[theproj].DamageType = projectile.magic;
			Main.projectile[theproj].usesLocalNPCImmunity = true;
			Main.projectile[theproj].localNPCHitCooldown = -1;
			Main.projectile[theproj].penetrate = -1;
			IdgProjectile.AddOnHitBuff(theproj, ModContent.BuffType<LavaBurn>(),60*5);
			Main.projectile[theproj].netUpdate = true;

			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			hittile = true;
			return true;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.velocity.Y<0 && hitwhilefalling)
			return false;
			if (Projectile.ai[1] < 5)
			return false;
			return base.CanHitNPC(target);
		}

		public override void AI()
		{

			Point16 loc = new Point16((int)Projectile.Center.X >> 4, (int)Projectile.Center.Y >> 4);
			if (WorldGen.InWorld(loc.X, loc.Y))
			{
				Tile tile = Main.tile[loc.X, loc.Y];
				if (tile != null)
					if (tile.liquid > 64)
						Projectile.Kill();
			}

			Projectile.scale = ((float)Projectile.width / 24f);
			for (int i = 0; i < Projectile.scale + 0.5; i++)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type);
				Main.dust[dust].scale = 2.25f*Projectile.scale;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Main.dust[dust].color * 0.25f;
				Main.dust[dust].velocity = -Projectile.velocity * (float)(Main.rand.Next(20, 100 + (i * 40)) * 0.005f);
			}




			Projectile.velocity.Y += 0.1f;
			Projectile.rotation += Projectile.velocity.X*0.05f;//(float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

			Projectile.ai[1] += 1;
		}


	}


}