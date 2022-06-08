using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Projectiles;
using Terraria.Audio;

namespace SGAmod.Items.Weapons.Technical
{

	public class EngineerSentrySummon : NoviteTowerSummon, ITechItem
	{
		public float ElectricChargeScalingPerUse() => 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Engineering Rod");
			Tooltip.SetDefault("Deploys a TR12 Gauss Auto-Turret infront of you\nCam only deploy when the hologram is Blue");
		}

		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = DamageClass.Summon;
			Item.sentry = true;
			Item.width = 24;
			Item.height = 30;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.noMelee = true;
			Item.knockBack = 0.5f;
			Item.value = Item.buyPrice(0, 1, 25, 0);
			Item.rare = ItemRarityID.Orange;
			Item.autoReuse = false;
			Item.useTurn = false;
			Item.shootSpeed = 0f;
			//item.UseSound = SoundID.Item78;
			Item.shoot = ModContent.ProjectileType<EngineerSentryProj>();
		}

		Vector2 DeployArea(Player player) => player.MountedCenter + new Vector2(player.direction * 32f, 0);

		public override void HoldItem(Player player)
		{				
				Vector2 pos = DeployArea(player);
				bool valid = TurretPositionValid(Item, player, ref pos);
				Projectile.NewProjectile(pos.X, pos.Y, player.direction, 0, ModContent.ProjectileType<EngineerSentryProjHologram>(), 0, 0, player.whoAmI, 0f, valid ? 1 : 0);
		}

		public static bool TurretPositionValid(Item item,Player player,ref Vector2 where)
        {
			int pushYUp = -1;
			player.FindSentryRestingSpotBetter(where, out var worldX, out var worldY, out pushYUp);

			where.X = worldX;
			where.Y = worldY- pushYUp;

			if (Collision.CanHitLine(player.MountedCenter,8,8, where, 8, 8) && (where-player.MountedCenter).Length()< 72)
            {
				return true;

            }
			return false;
        }


		public override bool CanUseItem(Player player)
        {
			Vector2 pos = DeployArea(player);
			return TurretPositionValid(Item,player, ref pos);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse != 2)
			{
				Vector2 pos = DeployArea(player);
				TurretPositionValid(Item, player, ref pos);
				Projectile.NewProjectile(pos.X, pos.Y, player.direction, 0, type, damage, knockBack, player.whoAmI, 0f, 0f);
				player.UpdateMaxTurrets();
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<NoviteTowerSummon>(), 1).AddIngredient(ModContent.ItemType<AdvancedPlating>(), 5).AddIngredient(ModContent.ItemType<ManaBattery>(), 2).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}
	}

	public class EngineerSentryProj : NoviteTower
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("RoR2 Sentry");
			//ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.FrostHydra);
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.sentry = true;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.aiStyle = -1;
		}

		Vector2 LookFrom => Projectile.Center+new Vector2(0,-8);

		public override void AI()
		{

			Player player = Main.player[base.Projectile.owner];
			Projectile.localAI[0] += 1;

			if (Projectile.localAI[0] == 1)
            {
				if (Projectile.velocity.X < 0)
				{
					Projectile.spriteDirection = -1;
					Projectile.rotation = MathHelper.Pi;
					Projectile.ai[1] = MathHelper.Pi;
				}

				if (Projectile.localAI[0] == 1)
				{
					SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/RoR2sndTurretDeploy").WithVolume(0.75f).WithPitchVariance(.15f), Projectile.Center);
				}
				Projectile.velocity.X = 0;
			}

			if (Projectile.localAI[0] > 10)
			{
				bool solidtiles = false;
				Point tilehere = ((Projectile.position) / 16).ToPoint();
				tilehere.Y += 2;
				for (int i = 0; i < 3; i += 1)
				{
					Tile tile = Framing.GetTileSafely(tilehere.X + i, tilehere.Y);
					if (WorldGen.InWorld(tilehere.X+i, tilehere.Y) && tile != null && tile.HasTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]))
					{
						solidtiles = true;
						break;
					}
				}

				if (solidtiles)
				{
					Projectile.velocity = new Vector2(Projectile.velocity.X, 0);
				}
				else
				{
					Projectile.velocity = new Vector2(Projectile.velocity.X, Projectile.velocity.Y + 0.25f);
				}
			}

			if (Projectile.localAI[0] < 120)
				return;

			float aimTo = Projectile.ai[1];

			Vector2 dotRotation = Projectile.rotation.ToRotationVector2();

			List<NPC> enemies = SGAUtils.ClosestEnemies(LookFrom, 640);

			if (enemies != null && enemies.Count > 0)
            {
				float bulletspeed = 8f;
				NPC target = enemies[0];
				Vector2 dist;// = target.Center - LookFrom;

				//Vector3 aimpos = SGAUtils.PredictAimingPos(LookFrom.ToVector3(), target.Center.ToVector3(), target.velocity.ToVector3(), bulletspeed, 0f);

				Vector2 offset = dotRotation.RotatedBy(MathHelper.PiOver2 * (Projectile.spriteDirection)) * -6f;
				dist = SGAUtils.PredictiveAim(bulletspeed*4f, LookFrom, target.Center, target.velocity, false)- (LookFrom+ offset);

				float toRotation = dist.ToRotation();
				Projectile.netUpdate = true;

				aimTo = toRotation;

				if (Vector2.Dot(dotRotation, Vector2.Normalize(dist)) > 0.98f && Projectile.localAI[1]<1)
                {
					Projectile.localAI[1] = 1;
					Projectile proj = Projectile.NewProjectileDirect(LookFrom+ offset + dotRotation * 24f, dotRotation* bulletspeed, ModContent.ProjectileType<EngineerSentryShotProj>(),Projectile.damage,Projectile.knockBack+2,Projectile.owner);
					proj.rotation = proj.velocity.ToRotation();
					SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/RoR2sndTurretFire").WithVolume(0.25f).WithPitchVariance(.25f), Projectile.Center);
				}
			}

			Projectile.rotation = Projectile.rotation.AngleTowards(aimTo, 0.15f);

			if (Projectile.localAI[1] > 0)//Firing animation
				Projectile.localAI[1]++;

			if (Projectile.localAI[1] > 30)//Firerate
            {
				Projectile.localAI[1] = 0;
			}

			Projectile.spriteDirection = dotRotation.X < 0 ? -1 : 1;

			if (Main.rand.Next(100) < 1)
            {
				Projectile.ai[1] = Main.rand.NextFloat(-0.75f, 0.75f)+(Projectile.rotation.ToRotationVector2().X>0 ? 0 : MathHelper.Pi);
				Projectile.netUpdate = true;
			}

		}

		public virtual void DrawTurret(SpriteBatch spriteBatch, Color lightColor,Vector2 offset = default)
        {
			float alpha = MathHelper.Clamp(Projectile.localAI[0] / 30f, 0f, 1f);
			Texture2D turretTex = ModContent.Request<Texture2D>("SGAmod/Items/Weapons/Technical/EngineerSentryProj");
			Texture2D baseTex = ModContent.Request<Texture2D>("SGAmod/Items/Weapons/Technical/EngineerSentryStand");
			Texture2D glowTex = ModContent.Request<Texture2D>("SGAmod/Items/GlowMasks/EngineerSentryProjGlow");
			Vector2 offset2 = offset == default ? Vector2.Zero : offset;

			int frame = (int)(Projectile.localAI[1] / 3);
			if (frame > 4)
				frame = 0;

			Vector2 turretOrig = new Vector2(18, Projectile.spriteDirection>0 ? 14 : (turretTex.Height/5)-14);
			Rectangle turretFrame = new Rectangle(0, frame * (turretTex.Height / 5), turretTex.Height, turretTex.Height / 5);
			Vector2 scaleInAnimation = new Vector2(MathHelper.SmoothStep(0.5f,1f, MathHelper.Clamp((Projectile.localAI[0]-10) / 20f, 0f, 1f)), MathHelper.SmoothStep(0.5f, 1f, MathHelper.Clamp((Projectile.localAI[0]) / 20f, 0f, 1f)));
			float riseAnimation = MathHelper.SmoothStep(12f, 0f, MathHelper.Clamp((Projectile.localAI[0]-40) / 60f, 0f, 1f));
			float anglelerp = MathHelper.PiOver2.AngleLerp(Projectile.rotation, MathHelper.SmoothStep(0f, 1f, MathHelper.Clamp((Projectile.localAI[0] - 90) / 25f, 0f, 1f)));


			spriteBatch.Draw(baseTex, Projectile.Center+ offset2 - Main.screenPosition, null, lightColor * alpha, 0f, baseTex.Size() / 2f, Projectile.scale* scaleInAnimation, SpriteEffects.None, 0f);

			for (int i = 0; i < 2; i += 1)
			{
				Texture2D tex = i == 0 ? turretTex : glowTex;
				Color glowColor = i == 0 ? lightColor : Color.White;
				spriteBatch.Draw(tex, LookFrom + offset2 + new Vector2(0, riseAnimation) - Main.screenPosition, turretFrame, glowColor * alpha, anglelerp, turretOrig, Projectile.scale * new Vector2(1f, scaleInAnimation.X), Projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
			}

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			DrawTurret(spriteBatch, lightColor);
			return false;
		}
	}

	public class EngineerSentryProjHologram : EngineerSentryProj
	{
        public override string Texture => "SGAmod/Items/Weapons/Technical/EngineerSentryProj";
        public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("RoR2 Hologram Sentry");
			//ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.timeLeft = 2;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 1;
		}

        public override void AI()
		{
			Projectile.localAI[0] = 150;
			Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
			Projectile.rotation = MathHelper.PiOver2 - (Projectile.spriteDirection * MathHelper.PiOver2);
			Projectile.position -= Projectile.velocity;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			SGAmod.FadeInEffect.Parameters["fadeColor"].SetValue(((Projectile.ai[1] > 0 ? Color.Aqua : Color.Red) * 1.0f).ToVector3());
			SGAmod.FadeInEffect.Parameters["alpha"].SetValue(0.30f+(float)Math.Sin(Main.GlobalTimeWrappedHourly*4f)*0.20f);



			for (float f = 0; f <= 8; f += 1)
			{
				SGAmod.FadeInEffect.CurrentTechnique.Passes[f>4 ? "LumaRecolorPass" : "LumaRecolorAlphaPass"].Apply();
				Vector2 randomizer = Main.rand.Next(100) < 1 ? Main.rand.NextVector2Circular(8,8) : default;
				DrawTurret(spriteBatch, (Projectile.ai[1] > 0 ? Color.Aqua : Color.Red) * 1.0f, randomizer);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


			return false;
		}

	}

		public class EngineerSentryShotProj : NPCs.Hellion.HellionCorePlasmaAttack
    {
		public override Color Color => Color.White*0.20f;
		public override Color Color2 => Color.Lime*0.50f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Engie Plasma");
		}
		public override string Texture => "SGAmod/Items/Weapons/Technical/EngineerSentryShotProj";

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.CloneDefaults(ProjectileID.ImpFireball);

			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 3;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 1;
			Projectile.width = 12;
			Projectile.height = 12;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.localAI[0] = 100;
		}

        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);
        }

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			//hjkkj
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.penetrate = 10000;
		}

		public override bool CanDamage()
		{
			return Projectile.penetrate < 10;
		}

		public override void AI()
		{
			Projectile.localAI[0] += 1;
			Projectile.ai[0] += 1;

			if (Projectile.penetrate > 10)
            {
				Projectile.timeLeft = Math.Min(Projectile.timeLeft, 90);
				Projectile.timeLeft -= 3;

			}

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			base.PreDraw(spriteBatch, lightColor);
			Texture2D texture = Main.projectileTexture[Projectile.type];

			float alpha = MathHelper.Clamp(Projectile.localAI[0] / 30f, 0f, 1f);

			float timeLeft = Math.Min(Projectile.timeLeft / 90f, 1f)* alpha;

			float maxtrail2 = (float)(Projectile.oldPos.Length - 1f)/2f;

			Vector2 drawOrigin2 = texture.Size() / 2f;

			float detail = 1f;

			for (float f = maxtrail2; f > 1; f -= 0.5f)
			{
				Vector2 pos = Vector2.Lerp(Projectile.oldPos[(int)f - 1], Projectile.oldPos[(int)f], f % 1f);
				float rot = Projectile.oldRot[(int)f - 1];
				float alphaShot = 1f-((f-1f) / maxtrail2);
				spriteBatch.Draw(texture, pos + (Projectile.Hitbox.Size() / 2f) - Main.screenPosition, null, Color.White* timeLeft * alphaShot*0.25f, rot + MathHelper.PiOver2, drawOrigin2, Projectile.scale* alphaShot, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White * timeLeft, Projectile.rotation + MathHelper.PiOver2, drawOrigin2, Projectile.scale, SpriteEffects.None, 0f);

			return false;
			/*
			Texture2D texture = Main.projectileTexture[projectile.type];
			Texture2D textureGlow = ModContent.GetTexture("SGAmod/Glow");

			float realVelocity = (MathHelper.Clamp((projectile.ai[0] - 300) / 300, 0f, 1f));
			float realAlpha = MathHelper.Clamp(projectile.localAI[0] / 300, 0f, 1f);
			float timeLeft = Math.Min(projectile.timeLeft / 50f, 1f);

			Vector2 drawOrigin2 = texture.Size() / 2f;
			Vector2 drawOrigin3 = textureGlow.Size() / 2f;

			//Color color = Color.White;
			//Color color2 = Color.Purple;

			float alpha = MathHelper.Clamp(projectile.localAI[0] / 30f, 0f, 1f);

			float scale = 2f - MathHelper.Clamp(projectile.localAI[0] / 70f, 0f, 1f);

			float scaledpre = MathHelper.Clamp((projectile.localAI[0] - 20) / 45f, 0f, 1f) * scale;

			float detail = 1f;// + projectile.velocity.Length();

			float maxtrail = (float)(projectile.oldPos.Length - 15f);
			float maxtrail2 = (float)(projectile.oldPos.Length - 1f);

			for (float f = maxtrail2; f >= 3f; f -= 0.25f)
			{
				Vector2 pos = Vector2.Lerp(projectile.oldPos[(int)f - 1], projectile.oldPos[(int)f], f % 1f);
				float rot = projectile.oldRot[(int)f - 1];
				spriteBatch.Draw(texture, pos + (projectile.Hitbox.Size() / 2f) - Main.screenPosition, null, Color * timeLeft * (1f / detail) * (1f - (f / maxtrail2)) * alpha, rot + MathHelper.PiOver2, drawOrigin2, new Vector2(0.2f, 2.0f) * scaledpre, SpriteEffects.None, 0f);
			}

			for (float f = maxtrail; f >= 1f; f -= 0.5f)
			{
				Vector2 pos = Vector2.Lerp(projectile.oldPos[(int)f - 1], projectile.oldPos[(int)f], f % 1f);
				float rot = projectile.oldRot[(int)f - 1];
				spriteBatch.Draw(texture, pos + (projectile.Hitbox.Size() / 2f) - Main.screenPosition, null, Color * timeLeft * (1f / detail) * (1f - (f / maxtrail)) * alpha, rot + MathHelper.PiOver2, drawOrigin2, scaledpre, SpriteEffects.None, 0f);
			}
			for (float f = maxtrail; f >= 1f; f -= 0.5f)
			{
				Vector2 pos = Vector2.Lerp(projectile.oldPos[(int)f - 1], projectile.oldPos[(int)f], f % 1f);
				float rot = projectile.oldRot[(int)f - 1];
				spriteBatch.Draw(textureGlow, pos + (projectile.Hitbox.Size() / 2f) - Main.screenPosition, null, Color2 * timeLeft * (0.75f / detail) * (1f - (f / maxtrail)) * alpha, rot + MathHelper.PiOver2, drawOrigin3, new Vector2(0.4f, 1.0f) * scaledpre, SpriteEffects.None, 0f);
			}

			return false;
			*/
		}
	}


}
