using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Items.Weapons.SeriousSam;
using Idglibrary;
using Terraria.Audio;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class QuasarCannon : SeriousSamWeapon,ITechItem
	{
		public float ElectricChargeScalingPerUse() => 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quasar Cannon");
			Tooltip.SetDefault("Launches a ball of energy that explodes nearby enemies on hit\nHold fire to charge up the shot");
		}

		public override void SetDefaults()
		{
			Item.damage = 200;
			Item.DamageType = DamageClass.Magic;
			Item.width = 32;
			Item.height = 62;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0,75,0,0);
			Item.rare = 11;
			Item.mana = 10;
			//item.UseSound = SoundID.Item99;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<QuasarCannonCharging>();
			Item.shootSpeed = 0f;
			Item.channel=true;
			Item.reuseDelay = 5;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("PrismalBar"), 12).AddIngredient(ItemID.LunarBar, 8).AddIngredient(ItemID.FragmentVortex, 6).AddIngredient(ItemID.FragmentNebula, 5).AddIngredient(mod.ItemType("AdvancedPlating"), 8).AddIngredient(mod.ItemType("ManaBattery"), 2).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float rotation = MathHelper.ToRadians(0);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[type]<1){
			int proj=Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
		}
			return false;
		}

	}

	public class QuasarCannonCharging : ModProjectile
	{

		public static int chargeuptime=300;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Beam Charging");
		}

	public override bool? CanHitNPC(NPC target){return false;}

			public override string Texture
		{
			get { return("SGAmod/Projectiles/WaveProjectile");}
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile=false;
			Projectile.friendly=true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			AIType = 0;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void AI()
		{
		Player player=Main.player[Projectile.owner];

		if (player==null || player.dead)
		Projectile.Kill();

		Projectile.timeLeft=4;
		player.itemTime=6;
		player.itemAnimation=6;

			bool channeling = ((player.channel || Projectile.ai[0] < 5) && !player.noItems && !player.CCed);
			if (Main.netMode == NetmodeID.MultiplayerClient || Main.netMode==NetmodeID.SinglePlayer)
			{
				Vector2 direction = (Main.MouseWorld - player.Center);
				direction.Normalize();
				Projectile.velocity = direction;
				Projectile.netUpdate = true;
			}
			Projectile.Center = player.Center + Vector2.Normalize(Projectile.velocity) * 72;
			if (player.statMana >= 10)
			Projectile.ai[0] += 1;

			Vector2 directionmeasure = Projectile.velocity;

			int num315;

			if (channeling)
			{
				player.ChangeDir((directionmeasure.X > 0).ToDirectionInt());
				player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * player.direction, Projectile.velocity.X * player.direction);
			}

		if (Projectile.ai[0]>10 && player.CheckMana(new Item(), 12,false,false))
			{

				if (Projectile.ai[0]%20==0)
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 94, 0.40f, -0.5f+(Projectile.ai[0]/(float)chargeuptime)*1.25f);

				if (Projectile.ai[0]<chargeuptime)
				{

					if (Projectile.ai[0] % 20 == 0)
						player.CheckMana(new Item(), 10,true,false);

		for (num315 = 0; num315 < 2; num315 = num315 + 1)
			{
					Vector2 randomcircle=new Vector2(Main.rand.Next(-64,0),Main.rand.Next(-20,6)*player.direction); randomcircle =randomcircle.RotatedBy(directionmeasure.ToRotation());
					int num622 = Dust.NewDust(new Vector2(Projectile.Center.X-1,Projectile.Center.Y)+randomcircle, 0, 0, 173, 0f, 0f, 100, default(Color), 1f+Projectile.ai[0]/300f);

					Main.dust[num622].scale = 1f;
					Main.dust[num622].noGravity=true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Vector2 velxx = Projectile.velocity;
						velxx.Normalize(); velxx *= -1f;
						velxx.RotatedByRandom(MathHelper.ToRadians(40));
					Main.dust[num622].velocity.X = velxx.X;
					Main.dust[num622].velocity.Y = velxx.Y;
					Main.dust[num622].velocity *= Main.rand.NextFloat(1f, 2f);
					Main.dust[num622].alpha = 150;
			}
			}else{
		for (num315 = 0; num315 < 2; num315 = num315 + 1)
			{
						Vector2 randomcircle2 = new Vector2(Main.rand.Next(-64, 16), Main.rand.Next(-26, 12)*player.direction); randomcircle2=randomcircle2.RotatedBy(directionmeasure.ToRotation());
						Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(Projectile.Center.X-1,Projectile.Center.Y)+ randomcircle2, 0, 0, 173, 0f, 0f, 173, default(Color), 2f);

					Main.dust[num622].scale = 1.5f;
					Main.dust[num622].noGravity=true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = (randomcircle.X + Projectile.velocity.X)*2;
					Main.dust[num622].velocity.Y = (randomcircle.Y + Projectile.velocity.Y)*2;
					Main.dust[num622].alpha = 100;
			}
			}
		}

		if (Projectile.ai[0]==chargeuptime){
		for (num315 = 0; num315 < 60; num315 = num315 + 1)
			{
					Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(Projectile.Center.X-1,Projectile.Center.Y), 0, 0, 173, 0f, 0f, 100, default(Color), 2.5f);

					Main.dust[num622].scale = 2.8f;
					Main.dust[num622].noGravity=true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = randomcircle.X*16f;
					Main.dust[num622].velocity.Y = randomcircle.Y*16f;
					Main.dust[num622].alpha = 200;
			}
		}

		if (!channeling){

				float speed=5f;
				Vector2 perturbedSpeed = (Projectile.velocity * speed); // Watch out for dividing by 0 if there is only 1 projectile.

				if (Projectile.ai[0] > 30)
				{
					int proj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("QuasarCannonShot").Type, Projectile.damage, Projectile.knockBack, player.whoAmI);
					Main.projectile[proj].penetrate = 1;
					Main.projectile[proj].ai[0] = Projectile.ai[0]*1.10f;
					SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Wave_Beam_Charge_Shot").WithVolume(.7f).WithPitchVariance(.25f), Projectile.Center);
				}

		Projectile.Kill();
		}

	}

}

	public class QuasarCannonShot : ModProjectile
	{
		private float scale = 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charged Beamshot");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override string Texture => "SGAmod/Projectiles/ChargedWave";

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (Projectile.localAI[0] < 9999)
				return false;
			Vector2 drawOrigin = Main.projectileTexture[Projectile.type].Size() / 2f;
			Color alphacol = Projectile.GetAlpha(lightColor);

				for (int k = Projectile.oldPos.Length - 1; k > 0; k -= 1)
				{
					float sizer = (Projectile.scale - ((float)k/(float)Projectile.oldPos.Length)*0.15f)* scale;
					if (sizer > 0)
					{
						Vector2 drawPos = Projectile.oldPos[k]+ (Projectile.Hitbox.Size()/2f) - Main.screenPosition;
						Color color = alphacol * ((float)(Projectile.oldPos.Length - k) / Projectile.oldPos.Length);
						spriteBatch.Draw(Main.projectileTexture[Projectile.type], drawPos, null, color*((float)k/(float)Projectile.oldPos.Length), Projectile.rotation, drawOrigin, sizer, SpriteEffects.None, 0f);
					}
				}
			spriteBatch.Draw(Main.projectileTexture[Projectile.type], Projectile.Center-Main.screenPosition, null, alphacol, Projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 1000;
			Projectile.extraUpdates = 3;
			AIType = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			for (int num315 = 0; num315 < 30+(int)(Projectile.ai[0]/10f); num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, Projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), Projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 15f), 50, default(Color), 3.4f);
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f+ (Projectile.ai[0] / 300f);
				dust3.noGravity = true;
				dust3.scale = 2.0f + (Projectile.ai[0] / 300f);
				dust3.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
			}

			for (int zz = 0; zz < Main.maxNPCs; zz += 1)
			{
				NPC npc = Main.npc[zz];
				if (!npc.dontTakeDamage && !npc.townNPC && npc.active && npc.life > 0)
				{
					if (npc.Distance(Projectile.Center) < 32+ Projectile.ai[0])
					{
						SoundEngine.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 92, 0.75f, -0.5f);
						Projectile.NewProjectile(npc.Center, Vector2.Zero, Mod.Find<ModProjectile>("QuaserBoom").Type, Projectile.damage, Projectile.knockBack, Projectile.owner);
					}
				}
			}

			return true;
		}

		public override bool PreAI()
		{
			if (Projectile.localAI[0] < 10000)
			{
				Projectile.localAI[0] = 10000;
				Projectile.damage = (int)(Projectile.damage*(1f + (Projectile.ai[0] / 150f)));

			}
			scale = 0.5f + (Projectile.ai[0] / ((float)QuasarCannonCharging.chargeuptime))*0.5f;
			return true;
		}

		public override void AI()
		{
			for (float num315 = 0; num315 < Projectile.ai[0]/100f; num315 = num315 + 1f)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 173, 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.15f), 0.7f+(Projectile.ai[0]/800f));
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.3f;
				dust3.noGravity = true;
				dust3.scale = 1.8f;
				dust3.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
			}

			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;

		}

	}

	public class QuaserBoom : ModProjectile
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
				for (float num315 = 5f; num315 < 12f; num315 = num315 + 0.25f)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int num316 = Dust.NewDust(new Vector2(Projectile.Center.X-16, Projectile.Center.Y-16), 32,32, 173, 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.15f), 0.8f);
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= randomcircle* num315;
					dust3.noGravity = true;
					dust3.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}

				ranspin2 = Main.rand.NextFloat(-0.1f, 0.1f);
				ranspin = Main.rand.NextFloat((float)Math.PI * 2f);
			}
			else
			{
				ranspin += ranspin2;

			}
		}
        public override bool CanDamage()
        {
            return Projectile.timeLeft == 58;
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
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_"+ ProjectileID.MagnetSphereBall); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			getstuff();
			Texture2D tex = Main.projectileTexture[Projectile.type];
			float timeleft = ((float)Projectile.timeLeft / 60f);
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition));
			Color color = Color.Violet * Math.Min(0.75f,timeleft*3);

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 5) / 2f;
			int timing = (int)(Projectile.localAI[0] / 3f);
			timing %= 5;
			timing *= ((tex.Height) / 5);

			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 5), color, ranspin, drawOrigin, Math.Min(3f,(1f - timeleft) * 12f), SpriteEffects.None, 0f);
			return false;
		}


		public override void AI()
		{
			Projectile.localAI[0] += 1f;

			if (Projectile.ai[0] < 1)
			{
				Projectile.ai[0] = 1;
				SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
				for (float num315 = 0; num315 < 40; num315 += 2f)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int dustz = 173;
					int num316 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + (randomcircle * (80f - num315) / 3f), 0, 0, dustz, 0, 0, 50, Main.hslToRgb(0.15f, 1f, 1.00f), (float)num315 * 0.1f);
					Main.dust[num316].noGravity = true;
					randomcircle *= (float)Math.Pow((80 - num315) / 5f, 0.75);
					Main.dust[num316].velocity = randomcircle*(2f+ num315/20f);
				}
			}

			Lighting.AddLight(Projectile.Center, ((255 - Projectile.alpha) * 0.01f) / 255f, ((255 - Projectile.alpha) * 0.025f) / 255f, ((255 - Projectile.alpha) * 0.25f) / 255f);
			return;
		}
	}

}
