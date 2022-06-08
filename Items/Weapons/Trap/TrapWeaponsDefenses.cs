using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.Enums;
using SGAmod.Items.Weapons.Trap;
using SGAmod.Projectiles;
using Idglibrary;
using Terraria.Audio;


namespace SGAmod.Items.Weapons
{
	public class DefenseTrapWeapon : TrapWeapon
	{

		protected virtual float SlowDown => 0.25f;

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{

			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] thetext = tt.Text.Split(' ');
				string newline = "";
				List<string> valuez = new List<string>();
				foreach (string text2 in thetext)
				{
					valuez.Add(text2 + " ");
				}
				valuez.Insert(1, "trap ");
				foreach (string text3 in valuez)
				{
					newline += text3;
				}
				tt.Text = newline;
			}

			tooltips.Add(new TooltipLine(Mod, "DefenseTrapWeaponLine", "Base damage is boosted based on the average of your damage type increases"));
			tooltips.Add(new TooltipLine(Mod, "DefenseTrapWeaponLine", "Trap damage boosts the damage projectiles do on hit"));
			tooltips.Add(new TooltipLine(Mod, "DefenseTrapWeaponLine", Idglib.ColorText(Color.Red, "Will greatly limit you without wearing Gripping Gloves")));
			tooltips.Add(new TooltipLine(Mod, "DefenseTrapWeaponLine", Idglib.ColorText(Color.Red, SlowDown * 100 + "% slower movement effect, gloves reduce slowdown")));

			base.ModifyTooltips(tooltips);

		}

		public override void HoldItem(Player player)
		{
			player.GetModPlayer<SGAPlayer>().SlowDownDefense += SlowDown;
			player.GetModPlayer<SGAPlayer>().SlowDownReset = 5;
		}

		public void DoTurn(Player player)
		{
			if (player.GetModPlayer<SGAPlayer>().grippinggloves<1 && player.velocity.X != 0)
				player.ChangeDir(player.velocity.X > 0 ? 1 : -1);

		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			add += (((player.GetDamage(DamageClass.Melee) + player.GetDamage(DamageClass.Ranged) + player.GetDamage(DamageClass.Magic) + player.GetDamage(DamageClass.Summon) + player.Throwing().thrownDamage) - 5f) / 5f);
		}

	}


	public class NonStationaryBunnyCannonLauncher : NonStationaryCannonLauncher
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Non-Stationary Portable Bunny Cannon");
			Tooltip.SetDefault("Shoots bunnies that deal damage twice against whoever they hit" + "\nRight click to change the firing Arc" +
	"\nCounts as trap damage, doesn't crit");
			SGAmod.NonStationDefenses.Add(SGAmod.Instance.Find<ModItem>("NonStationaryBunnyCannonLauncher").Type, SGAmod.Instance.Find<ModProjectile>("NonStationaryBunnyCannonHolding").Type);
		}

		public override int cannontypeitem => ItemID.ExplosiveBunny;
		public override int cannontypeproj => ProjectileID.ExplosiveBunny;

		protected override float SlowDown => 0.80f;

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.BunnyCannon); }
		}

		public override void SetDefaults()
		{
			Item.damage = 350;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 3.5f;
			Item.value = 100000;
			Item.noUseGraphic = true;
			Item.rare = 6;
			Item.autoReuse = true;
			Item.useTurn = false;
			//item.UseSound = SoundID.n;
			Item.shootSpeed = 12f;
			Item.shoot = ProjectileID.CannonballFriendly;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BunnyCannon, 1).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}

	}


		public class NonStationaryCannonLauncher : DefenseTrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Non-Stationary Portable Cannon");
			Tooltip.SetDefault("Shoots Cannonballs that pierce and explode when they hit a tile" + "\nRight click to change the firing Arc" +
	"\nCounts as trap damage, doesn't crit");
			SGAmod.NonStationDefenses.Add(SGAmod.Instance.Find<ModItem>("NonStationaryCannonLauncher").Type, SGAmod.Instance.Find<ModProjectile>("NonStationaryCannonHolding").Type);
		}

		public virtual int cannontypeitem => ItemID.Cannonball;
		public virtual int cannontypeproj => ProjectileID.CannonballFriendly;

		protected override float SlowDown => 0.95f;

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.Cannon); }
		}


		public override void SetDefaults()
		{
			Item.damage = 300;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 3.5f;
			Item.value = 100000;
			Item.noUseGraphic = true;
			Item.rare = 6;
			Item.autoReuse = true;
			Item.useTurn = false;
			//item.UseSound = SoundID.n;
			Item.shootSpeed = 12f;
			Item.shoot = ProjectileID.CannonballFriendly;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Cannon, 1).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			Item.useTurn = false;
			return player.CountItem(cannontypeitem) > 0 ? base.CanUseItem(player) : false;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			SGAPlayer sgaplay = player.GetModPlayer<SGAPlayer>();
			if (player.altFunctionUse == 2)
			{
				sgaplay.DefenseFrame += 1;
				sgaplay.DefenseFrame = sgaplay.DefenseFrame % 5;
				player.itemTime /= 4;
				player.itemAnimation /= 4;

			}
			else
			{
				DoTurn(player);
				int fireangle = sgaplay.DefenseFrame;
				if (player.direction < 0)
				{
					fireangle = 8 - fireangle;
				}
				int playerdic = player.direction;
				//Main.PlaySound(SoundID.Item11, player.position);
				int ittaz = NonStationaryDefenseHolding.ShootFromCannon((int)player.Center.X + (playerdic * 24), (int)player.Center.Y, fireangle, cannontypeitem, cannontypeproj, damage, knockBack, player.whoAmI);
				if (ittaz > -1)
				{
					Main.projectile[ittaz].trap = true;
					// Main.projectile[ittaz].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
					Main.projectile[ittaz].netUpdate = true;
					IdgProjectile.Sync(ittaz);
				}

			}

			return false;
		}

	}

	public class NonStationaryBunnyCannonHolding : NonStationaryCannonHolding
	{
		public override string Texture
		{
			get { return ("Terraria/Tiles_" + TileID.Cannon); }
		}
		protected override Vector2 offsetholding => new Vector2(0, -16);

		protected override Vector2 offsetdraw => new Vector2(18*4, 0);

	}

		public class NonStationaryCannonHolding : NonStationaryDefenseHolding
	{
		public override string Texture
		{
			get { return ("Terraria/Tiles_" + TileID.Cannon); }
		}
		protected override Vector2 offsetholding => new Vector2(0, -16);
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[Projectile.type];
			//Texture2D texGlow = ModContent.GetTexture("SGAmod/Items/Weapons/SeriousSam/BeamGunProjGlow");
			Color color = Projectile.GetAlpha(lightColor) * 1f; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

			int xindex = 0;
			int yindex = 0;

			int dir = Main.player[Projectile.owner].direction;

			Vector2 drawPos = ((Projectile.Center - Main.screenPosition)) + new Vector2(Main.player[Projectile.owner].direction * offsetholding.X, offsetholding.Y);

			drawPos = new Vector2((int)(drawPos.X - (dir*12)) + (dir<1 ? -16 : 0), (int)drawPos.Y-8);

			int attackframe = Main.player[Projectile.owner].GetModPlayer<SGAPlayer>().DefenseFrame;

			if (Main.player[Projectile.owner].direction < 0)
			{
				attackframe = 8 - attackframe;
			}

			int yoffset = attackframe*(18*3);

			for (int x = 0; x < 18 * 4; x += 18)
			{
				yindex = 0;
				int drawx = x - xindex;
				for (int y = 0; y < 18 * 3; y += 18)
				{
					int drawy = y - yindex;

					Rectangle rect = new Rectangle(x+ (int)offsetdraw.X, y+ (int)offsetdraw.Y + yoffset, 16, 16);

					spriteBatch.Draw(tex, drawPos + new Vector2(drawx, drawy), rect, color, 0, new Vector2(0, 0), Projectile.scale, SpriteEffects.None, 0f);
					yindex += 2;
				}
				xindex += 2;
			}

			return false;
		}

	}



	public class NonStationarySnowballLauncher : DefenseTrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Non-Stationary Snowball Launcher");
			Tooltip.SetDefault("'A frosty gatling gun in the palm of your... hands'" +
				"\nRapidly fires snowballs from your inventory" + "\nCan only fire in in a forward arc" +
	"\nCounts as trap damage, doesn't crit");
			SGAmod.NonStationDefenses.Add(SGAmod.Instance.Find<ModItem>("NonStationarySnowballLauncher").Type, SGAmod.Instance.Find<ModProjectile>("NonStationarySnowballLauncherHolding").Type);
		}

		public override string Texture
		{
			get { return ("Terraria/Item_"+ItemID.SnowballLauncher); }
		}

		public override void SetDefaults()
		{
			Item.damage = 27;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = 1;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 3.5f;
			Item.value = 100000;
			Item.noUseGraphic = true;
			Item.rare = 4;
			Item.autoReuse = true;
			Item.useTurn = false;
			//item.UseSound = SoundID.n;
			Item.shootSpeed = 12f;
			Item.useAmmo = AmmoID.Snowball;
			Item.shoot = ProjectileID.SnowBallFriendly;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.SnowballLauncher, 1).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}

		public override bool CanUseItem(Player player)
		{
			Item.useTurn = false;
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			DoTurn(player);
			SoundEngine.PlaySound(SoundID.Item11, player.position);
			float num2 = 12f + (float)Main.rand.Next(450) * 0.01f;
			float num3 = (float)Main.rand.Next(85, 105);
			float num4 = (float)Main.rand.Next(-35, 11);
			//int type = 166;
			//int damage = 35;
			//float knockBack = 3.5f;
			Vector2 vector = new Vector2(player.Center.X, player.Center.Y+4);
			if (player.direction <1)
			{
				num3 *= -1f;
				vector.X -= 36f;
			}
			else
			{
				vector.X += 36f;
			}
			float num5 = num3;
			float num6 = num4;
			float num7 = (float)Math.Sqrt((double)(num5 * num5 + num6 * num6));
			num7 = num2 / num7;
			num5 *= num7;
			num6 *= num7;
			int ittaz = Projectile.NewProjectile(vector.X, vector.Y, num5*(type==Mod.Find<ModProjectile>("JarateShurikensProg") .Type? 1.25f : 1f), num6, type, damage, knockBack, Main.myPlayer, 0f, 0f);
			Main.projectile[ittaz].trap = true;
			// Main.projectile[ittaz].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Main.projectile[ittaz].netUpdate = true;
			IdgProjectile.Sync(ittaz);

			return false;
		}

	}

	public class NonStationarySnowballLauncherHolding : NonStationaryDefenseHolding
	{
		public override string Texture
		{
			get { return ("Terraria/Tiles_"+ TileID.SnowballLauncher); }
		}
		protected override Vector2 offsetholding => new Vector2(0, -16);
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[Projectile.type];
			int frames = 4;
			//Texture2D texGlow = ModContent.GetTexture("SGAmod/Items/Weapons/SeriousSam/BeamGunProjGlow");
			SpriteEffects effects = SpriteEffects.FlipHorizontally;
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / frames) / 2f;
			Color color = Projectile.GetAlpha(lightColor) * 1f; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = (int)(Main.GlobalTimeWrappedHourly * 8f);
			timing %= frames;
			timing *= ((tex.Height) / frames);

			int spritetiles = 3;
			int xindex = 0;
			int yindex = 0;

			Vector2 drawPos = ((Projectile.Center - Main.screenPosition)) + new Vector2(Main.player[Projectile.owner].direction * offsetholding.X, offsetholding.Y);

			drawPos = new Vector2((int)drawPos.X, (int)drawPos.Y);

			//Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1,1,1));
			//Main.spriteBatch.Draw(Main.blackTileTexture, new Vector2(0, 0), new Rectangle(0, 0, rend.Width, rend.Height), Color.Red, 0, new Vector2(0, 0), new Vector2(1f, 1f), SpriteEffects.None, 0f);

			int xoffset = Main.player[Projectile.owner].direction > 0 ? 18 * spritetiles : 0;

			for (int x = 0; x < 18 * spritetiles; x += 18)
			{
				yindex = 0;
				int drawx = x - xindex;
				for (int y = 0; y < 18 * spritetiles; y += 18)
				{
					int drawy = y - yindex;

					Rectangle rect = new Rectangle(x + xoffset, y, 16, 16);

					spriteBatch.Draw(tex, drawPos + new Vector2(drawx, drawy), rect, color, 0, new Vector2(0, 0), Projectile.scale, SpriteEffects.None, 0f);
					yindex += 2;
				}
				xindex += 2;
			}

			return false;
		}

	}


	public class NonStationaryDefenseHolding : ModProjectile
	{
		protected virtual Vector2 offsetholding => new Vector2(0,0);
	protected virtual Vector2 offsetdraw => new Vector2(0, 0);

	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/TheJacob"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 100;
			Projectile.penetrate = 10;
			Projectile.timeLeft = 2;
			AIType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = 8;
			drawHeldProjInFrontOfHeldItemAndArms = false;
		}

        public override bool CanDamage()
        {
			return false;
        }

        public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override bool PreAI()
		{
			Player owner = Main.player[Projectile.owner];
			if (owner == null)
				Projectile.Kill();
			return true;
		}

		public override void AI()
		{
			Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
			Player owner = Main.player[Projectile.owner];
			if (owner == null)
				Projectile.Kill();
			if (owner.dead)
				Projectile.Kill();
			owner.heldProj = Projectile.whoAmI;
			int tryget;
			SGAmod.NonStationDefenses.TryGetValue(owner.HeldItem.type, out tryget);
			if (tryget == Projectile.type)
			{
				Projectile.timeLeft += 1;
			}
			else
			{
				Projectile.Kill();
			}


				if (Main.LocalPlayer == owner)
				{
					Vector2 direction = (Main.MouseWorld - owner.Center);
					if (owner.GetModPlayer<SGAPlayer>().grippinggloves>0)
					{
						Projectile.direction = (direction.X > 0).ToDirectionInt();
					}
					else
					{
						Projectile.direction = (owner.direction > 0).ToDirectionInt();
					}

				}

				if (owner.GetModPlayer<SGAPlayer>().grippinggloves>0)
				{
				owner.ChangeDir(Projectile.direction);
				}
				Projectile.spriteDirection = Projectile.direction;
				owner.heldProj = Projectile.whoAmI;
				Projectile.ai[0] += 1;
				Projectile.velocity = new Vector2(0f, 0f);
				owner.bodyFrame.Y = owner.bodyFrame.Height * 3;

			Projectile.Center = owner.Center + new Vector2(owner.direction < 0 ? -Projectile.width * 2 : 0, -4f);

		}




		public static int ShootFromCannon(int x, int y, int angle, int ammo,int type, int Damage, float KnockBack, int owner)
		{
			float num = 14f;
			float num2 = 0f;
			float num3 = 0f;
			Main.player[owner].ConsumeItem(ammo);
			if (angle == 0)
			{
				num2 = 10f;
				num3 = 0f;
			}
			if (angle == 1)
			{
				num2 = 7.5f;
				num3 = -2.5f;
			}
			if (angle == 2)
			{
				num2 = 5f;
				num3 = -5f;
			}
			if (angle == 3)
			{
				num2 = 2.75f;
				num3 = -6f;
			}
			if (angle == 4)
			{
				num2 = 0f;
				num3 = -10f;
			}
			if (angle == 5)
			{
				num2 = -2.75f;
				num3 = -6f;
			}
			if (angle == 6)
			{
				num2 = -5f;
				num3 = -5f;
			}
			if (angle == 7)
			{
				num2 = -7.5f;
				num3 = -2.5f;
			}
			if (angle == 8)
			{
				num2 = -10f;
				num3 = 0f;
			}
			Vector2 vector = new Vector2(x,y);//new Vector2((float)((x + 2) * 16), (float)((y + 2) * 16));
			float num5 = num2;
			float num6 = num3;
			float num7 = (float)Math.Sqrt((double)(num5 * num5 + num6 * num6));
			/*if (ammo == 4 || ammo == 5)
			{
				if (angle == 4)
				{
					vector.X += 5f;
				}
				vector.Y += 5f;
			}*/
			num7 = num / num7;
			num5 *= num7;
			num6 *= num7;
			if (Main.myPlayer != owner && Main.netMode == 2)// && (ammo == 4 || ammo == 5))
			{
				NetMessage.SendData(MessageID.WiredCannonShot, owner, -1, null, Damage, KnockBack, (float)x, (float)y, angle, ammo, owner);
				return -1;
			}
			if (Main.netMode == 2)
			{
				owner = Main.myPlayer;
			}
			return Projectile.NewProjectile(vector.X, vector.Y, num5, num6, type, Damage, KnockBack, owner, 0f, 0f);
		}


	}




}
