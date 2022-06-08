using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Idglibrary;
using Terraria.Audio;


namespace SGAmod.Items.Weapons.SeriousSam
{
	public class SBCCannon : SeriousSamWeapon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 0.60f;
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("SBC Cannon");
            Tooltip.SetDefault("Charge up piercing cannon balls that do a huge amount of damage\nBut lose power with each enemy they pass through, exploding when they run out of damage\nCharge longer for more speed and much more damage!\nCannonballs do not crit and explode on knockback immune enemies\nDamage is increased instead based on crit chance, and the explosion however can crit\nUses Lead Cannonballs as ammo\n'Lets get Serious!'");
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SBCCannonHolding>()] > 0)
				return false;
			return true;
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			add += (float)player.GetCritChance(DamageClass.Ranged) / 100f;
		}

		public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 48;
            Item.height = 28;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.value = 400000;
			Item.rare = 6;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SBCCannonHolding>();
            Item.shootSpeed = 1f;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.useAmmo = ModContent.ItemType<LeadCannonball>();
		}

		/*public override bool CanUseItem(Player player)
		{
			SGAPlayer modply = player.GetModPlayer<SGAPlayer>();
			return (modply.RefilPlasma());
		}*/

		public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }

		public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.Cannon, 1).AddIngredient(ItemID.StarCannon, 1).AddIngredient(null, "WraithFragment4", 8).AddIngredient(null, "AdvancedPlating", 8).AddIngredient(mod.ItemType("VirulentBar"), 4).AddIngredient(ItemID.MeteoriteBar, 8).AddIngredient(ItemID.HallowedBar, 6).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
        }

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = player.Center;
			Vector2 offset = new Vector2(speedX, speedY);
			offset.Normalize();
			offset *= 16f;
			//position += offset;


				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
				float scale = 1f;// - (Main.rand.NextFloat() * .2f);
				perturbedSpeed = perturbedSpeed * scale; 
				int prog=Projectile.NewProjectile(position.X+ offset.X, position.Y+ offset.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                IdgProjectile.Sync(prog);


			return false;
		}
	}

	public class SBCCannonHolding : ModProjectile
	{
		//public virtual float trans => 1f;
		public Player P;
		public virtual float chargeuprate => 3f;
		public virtual string soundcharge => "Sounds/Custom/Cannon/Prepare";
		public virtual string soundfire => "Sounds/Custom/Cannon/Fire";
		public virtual int cooldowntime => 90;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cannon");
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool CanHitPlayer(Player target)
		{
			return false;
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 3;
			Projectile.penetrate = -1;
			AIType = ProjectileID.WoodenArrowFriendly;
			Projectile.damage = 0;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/SeriousSam/SBCCannonProj"; }
		}

		public override void AI()
		{

			Player player = Main.player[Projectile.owner];

			if (player != null && player.active)
			{

				SGAPlayer modply = player.GetModPlayer<SGAPlayer>();

				if (Projectile.ai[0] > 3000 || player.dead)
				{
					Projectile.Kill();
				}
				else
				{

					Vector2 mousePos = Main.MouseWorld;

					///Holding

					if (Projectile.owner == Main.myPlayer)
					{
						Vector2 diff = mousePos - player.Center;
						diff.Normalize();
						if (player.channel && Projectile.ai[0]< 200 && Projectile.ai[1] < 1)
						{
							Projectile.velocity = diff;
							Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
						}
						Projectile.netUpdate = true;
					}

					int dir = Projectile.direction;
					player.ChangeDir(dir);
					Projectile.direction = dir;

					player.heldProj = Projectile.whoAmI;

					bool isholding = (player.channel && Projectile.ai[0] < 200 && Projectile.ai[1] < 1);

					if (isholding)
					{
						player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir);
						Projectile.rotation = player.itemRotation - MathHelper.ToRadians(90);
						Projectile.timeLeft = cooldowntime;
						Projectile.ai[0] += chargeuprate;
						if ((int)Projectile.ai[0]==(int)(chargeuprate*3f))
						SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, soundcharge).WithVolume(.7f).WithPitchVariance(.25f), Projectile.Center);
						//float speedzz = projectile.velocity.Length();
						//projectile.velocity.Normalize();
						//projectile.velocity*=(speedzz+0.05f);
					}
					Projectile.Center = (player.Center+new Vector2(dir*6, 0))+ (Projectile.velocity*12f)-(Projectile.velocity*(0.08f *Projectile.ai[0]));
					Projectile.position -= Projectile.velocity;
					player.itemTime = 2;
					player.itemAnimation = 2;

					//Projectiles



					if (!isholding)
					{
						Projectile.velocity /= 1.15f;

						if (Projectile.ai[1] < 1)
						{

							Vector2 position = Projectile.Center;
							Vector2 offset = new Vector2(Projectile.velocity.X, Projectile.velocity.Y);
							offset.Normalize();
							offset *= 16f;
							offset += Projectile.velocity;

							float scalar = GetType() == typeof(SBCCannonHoldingMK2) ? 30f : 30f;
							float damagescale = (Projectile.damage * (1f + (Projectile.ai[0] / scalar)));

							SGAmod.AddScreenShake(4f * (1f + (Projectile.ai[0] / scalar)), 240, player.Center);

							Vector2 perturbedSpeed = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(0));
							Vector2 perturbedSpeed2 = perturbedSpeed;
							perturbedSpeed2.Normalize();
							float basespeed = 1f + (GetType() == typeof(SBCCannonHoldingMK2) ? 0.4f : -0.4f);
							float scale = 1.5f+ ((Projectile.ai[0]/30f)* basespeed);// - (Main.rand.NextFloat() * .2f);
							perturbedSpeed = perturbedSpeed * (scale*4f);
							perturbedSpeed += perturbedSpeed2;
							offset -= perturbedSpeed;
							perturbedSpeed /= 2f;
							int prog = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("SBCBall").Type, (int)damagescale, Projectile.knockBack, player.whoAmI,GetType() == typeof(SBCCannonHolding) ? 0f : 100f,(float)Projectile.damage);
							IdgProjectile.Sync(prog);
							SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, soundfire).WithVolume(.7f).WithPitchVariance(.25f), Projectile.Center);
						}


						Projectile.ai[1] = 1;

					}
				}
			}
			else
			{
				Projectile.Kill();
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{


			Texture2D tex = Main.projectileTexture[Projectile.type];
			SpriteEffects effects = SpriteEffects.FlipHorizontally;
			Vector2 drawOrigin = new Vector2(tex.Width/2f, tex.Height / 2f);
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
			Color color = Projectile.GetAlpha(lightColor) * 1f; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			spriteBatch.Draw(tex, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction<1 ? effects : (SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally), 0f);

			return false;
		}


	}

	public class SBCBall : ModProjectile
	{
		bool hittile = false;
		public virtual bool hitwhilefalling => false;
		public virtual float trans => 1f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cannon Ball");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.SpikyBall);
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			// projectile.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.extraUpdates = 2;
			Projectile.penetrate = -1;
			AIType = ProjectileID.WoodenArrowFriendly;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Ammo/LeadCannonball"; }
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
		{
			bool facingleft = Projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
			Texture2D texture = Main.projectileTexture[Projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 scaler = new Vector2(Main.rand.NextFloat(0.75f, 1.25f), Main.rand.NextFloat(0.75f, 1.25f) * 0.5f + (Projectile.velocity.Length() / 10f));
			Main.spriteBatch.Draw(ModContent.Request<Texture2D>("SGAmod/HavocGear/Projectiles/HeatWave"), Projectile.Center - Main.screenPosition, new Rectangle?(), Color.White * MathHelper.Clamp((Projectile.velocity.Length() - 6f) / 4f, 0f, 1f), Projectile.velocity.ToRotation() + MathHelper.ToRadians(90),
				new Vector2(ModContent.Request<Texture2D>("SGAmod/HavocGear/Projectiles/HeatWave").Width * 0.5f, ModContent.Request<Texture2D>("SGAmod/HavocGear/Projectiles/HeatWave").Height * 0.5f), scaler, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), drawColor, Projectile.rotation, origin, Projectile.scale, facingleft ? effect : SpriteEffects.None, 0);

			return false;
		}

		public override bool PreAI()
		{

			for (int zz = 0; zz < Main.maxNPCs; zz += 1)
			{
				NPC npc = Main.npc[zz];
				if (!npc.dontTakeDamage && !npc.townNPC && npc.active && npc.life > 0)
				{
					Rectangle rech = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
					Rectangle rech2 = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
					if (rech.Intersects(rech2))
					{
						if (Projectile.localNPCImmunity[npc.whoAmI] < 1)
							npc.immune[Projectile.owner] = 0;
					}
				}
			}

			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = 90;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			crit = false;
			Projectile.damage -= target.life;
			if (Projectile.damage < 1 || (target.knockBackResist == 0f && Projectile.ai[0]<1f))
				Projectile.Kill();
		}

		public override bool PreKill(int timeLeft)
		{

			int theproj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("CannonBoom").Type, (int)(Projectile.ai[1]), Projectile.knockBack, Projectile.owner, 0f, 0f);
			Main.projectile[theproj].DamageType = DamageClass.Ranged;
			// Main.projectile[theproj].melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Math.Abs(oldVelocity.Length()) > 2)
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Cannon/Bounce").WithVolume(.7f).WithPitchVariance(.25f), Projectile.Center);

			return true;
		}

		public override void AI()
		{

			if (Math.Abs(Projectile.velocity.Length()) < 0.5f)
			{
				Projectile.Kill();
			}




			Projectile.velocity.Y -= 0.15f;
			Projectile.rotation += Projectile.velocity.X * 0.25f;

			Projectile.ai[1] += 0.5f;
		}


	}

	public class CannonBoom : ModProjectile
	{
		float ranspin = 0;
		float ranspin2 = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blast");
		}

		public void getstuff()
		{

			if (ranspin2 == 0)
			{
				ranspin2 = Main.rand.NextFloat(-0.2f, 0.2f);
			}
			else
			{
				ranspin += ranspin2;

			}
		}

		public override void SetDefaults()
		{
			Projectile.width = 128;
			Projectile.height = 128;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 60;
		}

		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Projectiles/BoulderBlast"); }
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.timeLeft < 56)
				return false;
			else
				return null;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			getstuff();
			Texture2D tex = SGAmod.ExtraTextures[93];
			float timeleft = ((float)Projectile.timeLeft / 60f);
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 3) / 2f;
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition));
			Color color = Color.White * timeleft; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = (int)(Projectile.localAI[0] / 3f);
			timing %= 3;
			timing *= ((tex.Height) / 3);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 3), color, ranspin, drawOrigin, (1f-timeleft)*2f, SpriteEffects.None, 0f);
		}


		public override void AI()
		{
			Projectile.localAI[0] += 1f;

			if (Projectile.ai[0] < 1)
			{
				Projectile.ai[0] = 1;
				SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
				for (float num315 = 0; num315 < 80; num315 += 0.25f)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int dustz = DustID.Fire;
					if (Main.rand.Next(0, 20) < 5)
						dustz = Mod.Find<ModDust>("HotDust").Type;
					int num316 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y)+ (randomcircle * (80f-num315) / 3f), 0,0, dustz, 0, 0, 50, Main.hslToRgb(0.15f, 1f, 1.00f), (float)num315 * 0.1f);
					Main.dust[num316].noGravity = true;
					randomcircle *= (float)Math.Pow((80-num315) / 5f,0.75);
					Main.dust[num316].velocity = randomcircle;
				}
			}

			Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.01f) / 255f, ((255 - Projectile.alpha) * 0.025f) / 255f, ((255 - Projectile.alpha) * 0.25f) / 255f);
			return;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(Mod.Find<ModBuff>("ThermalBlaze").Type, 300);
		}
	}

	public class LeadCannonball : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lead Cannonball");
			Tooltip.SetDefault("Cast-steel Cannonballs made from a Novus-Lead Alloy; for use with the SBC Cannon and MK2");
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Ammo/LeadCannonball"; }
		}

		public override void AddRecipes()
		{
			CreateRecipe(15).AddIngredient(ItemID.Cannonball, 100).AddIngredient(ItemID.LeadBar, 3).AddIngredient(null, "UnmanedBar", 2).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 30;
			Item.consumable = true;
			Item.knockBack = 1.5f;
			Item.value = 500;
			Item.rare = 5;
			Item.ammo = Item.type;
		}

		public override void UpdateInventory(Player player)
		{
			Item.maxStack = 30;
			//I don't think so Fargo, not this one
		}

	}

	public class SBCCannonMK2 : SBCCannon, ITechItem, IHellionDrop
	{
		int IHellionDrop.HellionDropAmmount() => 1;
		int IHellionDrop.HellionDropType() => ModContent.ItemType<SBCCannonMK2>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SBC Cannon MK2");
			Tooltip.SetDefault("SBC Cannon improved with a pressure gauge, bindings, and lunar materials\nCharges and recovers after firing faster, and launches cannonballs faster than it's precursor\nCharge up piercing cannon balls that do a huge amount of damage\nBut lose power with each enemy they pass through, exploding when they run out of damage\nCharge longer for more speed and much more damage!\nCannonballs do not crit and do not explode on knockback immune enemies\nDamage is increased instead based on crit chance, and the explosion however can crit\nUses Lead Cannonballs as ammo\n'LETS GET SERIOUS!!'");
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SBCCannonHoldingMK2>()] > 0)
				return false;
			return true;
		}


		public override void SetDefaults()
		{
			Item.damage = 2500;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 48;
			Item.height = 28;
			Item.useTime = 5;
			Item.useAnimation = 5;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.value = 1000000;
			Item.rare = 11;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SBCCannonHoldingMK2>();
			Item.shootSpeed = 1f;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.useAmmo = ModContent.ItemType<LeadCannonball>();
		}

		/*public override bool CanUseItem(Player player)
		{
			SGAPlayer modply = player.GetModPlayer<SGAPlayer>();
			return (modply.RefilPlasma());
		}*/

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, 0);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "SBCCannon", 1).AddIngredient(ItemID.PressureTrack, 5).AddIngredient(ItemID.Chain, 25).AddIngredient(ItemID.LihzahrdPressurePlate, 2).AddIngredient(null, "StarMetalBar", 12).AddIngredient(null, "DrakeniteBar", 15).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = player.Center;
			Vector2 offset = new Vector2(speedX, speedY);
			offset.Normalize();
			offset *= 16f;
			//position += offset;


			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
			float scale = 1f;// - (Main.rand.NextFloat() * .2f);
			perturbedSpeed = perturbedSpeed * scale;
			int prog = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			IdgProjectile.Sync(prog);


			return false;
		}
	}


	public class SBCCannonHoldingMK2 : SBCCannonHolding
	{
		//public virtual float trans => 1f;
		public Player P;
		public override float chargeuprate => 4f;
		public override string soundcharge => "Sounds/Custom/Cannon/Prepare";
		public override string soundfire => "Sounds/Custom/Cannon/Fire";
		public override int cooldowntime => 70;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cannon");
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/SeriousSam/SBCCannonProjMK2"; }
		}

	}


	}
