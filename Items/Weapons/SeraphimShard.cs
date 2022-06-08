using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SGAmod.Effects;
using Microsoft.Xna.Framework.Audio;
using Idglibrary;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{

	public class SeraphimShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraphim Shard");
			Tooltip.SetDefault("'Some things are better left forgotten...'\nThis shard summons powerful copies to protect its user\n"+ Idglib.ColorText(Color.Red, "At a cost, you lose 50 max life per active shard\nSummon can't be used if life dropped exceeds max vanilla health"));
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.knockBack = 4f;
			Item.mana = 25;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = Item.buyPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.noUseGraphic = true;
			//item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<SeraphimShardBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType< SeraphimShardProjectile>();
			/*if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = Main.itemTexture[item.type];

				item.GetGlobalItem<ItemUseGlow>().CustomDraw = delegate (Item item, PlayerDrawInfo drawInfo, Vector2 position, float angle, Color glowcolor)
				{
					Texture2D texture = Main.itemTexture[item.type];
					Vector2 origin = texture.Size() / 2f;
					float timeAdvance = drawInfo.drawPlayer.itemAnimation/drawInfo.drawPlayer.itemAnimationMax;
					angle = drawInfo.drawPlayer.itemRotation + (drawInfo.drawPlayer.direction < 0 ? MathHelper.Pi : 0);
					Player drawPlayer = drawInfo.drawPlayer;

					Vector2 drawHere = drawPlayer.MountedCenter + new Vector2(drawPlayer.direction * 16f, drawPlayer.gravDir * -26f) - Main.screenPosition;

					DrawData value = new DrawData(texture, drawHere, null, Color.White * 0.75f * MathHelper.Clamp(1f-timeAdvance, 0f, 1f), 0, origin, timeAdvance*2f, drawInfo.spriteEffects, 0);
					//Main.playerDrawData.Add(value);

				};
			}*/
		}

        public override bool CanUseItem(Player player)
        {
			int count = Main.projectile.Where(currentProjectile => currentProjectile.active
	&& currentProjectile.owner == Main.myPlayer
	&& currentProjectile.type == Item.shoot).ToList().Count;

			return player.statLifeMax>100+(count*50);
        }

        public override Color? GetAlpha(Color lightColor)
        {
			return Main.hslToRgb((Main.GlobalTimeWrappedHourly / 4f) % 1f, 1f, 0.85f)*0.75f;
        }
        public override string Texture
		{
			get { return ("SGAmod/Projectiles/SeraphimShardProjectile"); }
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = player.MountedCenter + new Vector2(player.direction * 16f, -player.gravDir * 26f);
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(Item.buffType, 2);
			SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_BetsySummon, (int)position.X, (int)position.Y);
			if (sound != null)
				sound.Pitch += 0.50f;

			return true;
		}

	}

	public class SeraphimShardProjectile : ModProjectile,IDrawAdditive
	{
		float startupDelay => MathHelper.Clamp((Projectile.localAI[0] - 75f) / 150f, 0f, 1f);
		Color prismColor => Main.hslToRgb(((Projectile.whoAmI / 255f) * 100f) % 1f, 1f, 0.85f);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraphim Shard");
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.Homing[Projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
			Projectile.localNPCHitCooldown = 60;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.timeLeft = 60;
			Projectile.extraUpdates = 1;
		}
		public override bool CanDamage() => Projectile.ai[0]>0;
		public override bool MinionContactDamage() => Projectile.ai[0] > 0;
		float SeekDist => 720f;
		float AttackDist => 280f;
		float HoverDist => 128f;
		float us = 0f;
		float maxus = 0f;
		Player player;
		private void DoAttack(NPC enemy)
        {
			int attackperiod = 32;
			int attackperiod2 = 48+(int)maxus*2;
			int delay = (int)((us / maxus) * attackperiod2);


			if (Projectile.ai[0] < -45 && Projectile.Distance(enemy.Center)<AttackDist && (Main.player[Projectile.owner].SGAPly().timer+delay) % attackperiod2 == 0)
            {
				Projectile.ai[0] = attackperiod;
				Projectile.localAI[1] = 0;

			}
			if (Projectile.ai[0] > 0)
			{
				if (Projectile.ai[0] < 2)
                {
					Vector2 dotProduct = enemy.Center - Projectile.Center;
					if (Vector2.Dot(Vector2.Normalize(dotProduct), Vector2.Normalize(Projectile.velocity)) > .50f && Collision.CanHitLine(enemy.Center, 1, 1, Projectile.Center, 1, 1))
					Projectile.ai[0] = 2;
				}

				Projectile.velocity *= 0.99f;

				if (Projectile.ai[0] == attackperiod-10)
				{
					SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaivePierce, (int)Projectile.Center.X, (int)Projectile.Center.Y);
					if (sound != null)
						sound.Pitch += 0.50f;
				}

				if (Projectile.ai[0] > attackperiod-10)
				{
					Projectile.rotation = Projectile.rotation.AngleTowards((enemy.Center - Projectile.Center).ToRotation()+MathHelper.PiOver2, 3.5f);
                }
                else
                {
					Projectile.rotation = Projectile.rotation.AngleTowards((enemy.Center - Projectile.Center).ToRotation() + MathHelper.PiOver2, 0.1f);

					Projectile.velocity += (Projectile.rotation- MathHelper.PiOver2).ToRotationVector2();
					if (Projectile.velocity.Length() > 8f + enemy.velocity.Length())
                    {
						Projectile.velocity = Vector2.Normalize(Projectile.velocity) * (8f + enemy.velocity.Length());
					}

				}

				Projectile.localAI[1] = Math.Min(Projectile.localAI[1]+2.50f,100f);
			}

		}

		public override bool Autoload(ref string name)
		{
			SGAPlayer.PostUpdateEquipsEvent += PostUpdatePlayer;
			return true;
		}

		public void PostUpdatePlayer(SGAPlayer sgaplayer)
        {
			int count = Main.projectile.Where(currentProjectile => currentProjectile.active
				&& currentProjectile.owner == Main.myPlayer
				&& currentProjectile.type == Projectile.type).ToList().Count;
			//sgaplayer.player.statManaMax2 -= count * 50;
			sgaplayer.Player.statLifeMax2 -= count * 50;
		}

		public override void AI()
		{
			//if (projectile.owner == null || projectile.owner < 0)
			//return;

			player = Main.player[Projectile.owner];
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<SeraphimShardBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<SeraphimShardBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			bool toplayer = true;
			Vector2 gothere = player.Center+new Vector2(player.direction*-32,0);
			Projectile.localAI[0] += 1;
			Projectile.localAI[1] -= 1;
			if (startupDelay < 0)
            {
				Projectile.ai[0] = 0;
			}
			Projectile.ai[0] -= 1;

			List<NPC> closestnpcs = new List<NPC>();
			
			for(int i = 0; i < Main.maxNPCs; i += 1)
			{
				//bool colcheck= Collision.CheckAABBvLineCollision(Main.npc[i].position, new Vector2(Main.npc[i].width, Main.npc[i].height), Main.npc[i].Center,projectile.Center)
				//	&& Collision.CanHitLine(Main.npc[i].Center,0,0, projectile.Center,0,0);

				if (Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].townNPC && !Main.npc[i].dontTakeDamage && Main.npc[i].CanBeChasedBy() &&
					Collision.CheckAABBvLineCollision(Main.npc[i].position, new Vector2(Main.npc[i].width, Main.npc[i].height), Main.npc[i].Center, Projectile.Center)
					&& Collision.CanHitLine(Main.npc[i].Center, 0, 0, Projectile.Center, 0, 0)
					&& (Main.npc[i].Center-player.Center).Length()< SeekDist)
					closestnpcs.Add(Main.npc[i]);
			}

			//int it=player.grappling.OrderBy(n => (Main.projectile[n].active ? 0 : 999999) + Main.projectile[n].timeLeft).ToArray()[0];
			NPC them = closestnpcs.Count<1 ? null : closestnpcs.ToArray().OrderBy(npc => Projectile.Distance(npc.Center)).ToList()[0];
			NPC oldthem = null;

			if (player.HasMinionAttackTargetNPC)
			{
				oldthem = them;
				them = Main.npc[player.MinionAttackTargetNPC];
				//gothere = them.Center + new Vector2(them.direction * 96, them.direction==0 ? -96 : 0);
			}

			if (them != null && them.active)
			{
				toplayer = false;
				//if (!player.HasMinionAttackTargetNPC)
				gothere = them.Center + Vector2.Normalize(Projectile.Center- them.Center) * HoverDist;
			}
			maxus = 0;
			us = 0;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile currentProjectile = Main.projectile[i];
				if (currentProjectile.active
				&& currentProjectile.owner == Main.myPlayer
				&& currentProjectile.type == Projectile.type)
				{
					if (i == Projectile.whoAmI)
						us = maxus;
					maxus += 1f;
				}
			}
			Vector2 there = player.Center;

			int timer = player.SGAPly().timer * 2;
			double angles = MathHelper.ToRadians(((float)((us / maxus) * 360.00) - 90f)+ timer);
			float dist = 16f;//Main.rand.NextFloat(54f, 96f);
			float aval = (float)timer+ (us*83f);
			Vector2 here;
			if (!toplayer && startupDelay>0)
			{
				here = (new Vector2((float)Math.Sin(aval / 60f) * 6f, 20f * ((float)Math.Sin(aval / 70f)))).RotatedBy((them.Center - gothere).ToRotation());
				if (Projectile.ai[0]<1)
				Projectile.rotation = Projectile.rotation.AngleTowards(0, 0.1f);

				DoAttack(them);
			}
			else
			{
				float anglz = (float)(Math.Cos(MathHelper.ToRadians(aval)) * player.direction) / 4f;
				if (startupDelay>0)
				Projectile.rotation = Projectile.rotation.AngleTowards(((player.direction * 45) + anglz), 0.05f);
				//gothere -= (Vector2.UnitX * player.direction) * maxus;
				here = new Vector2((float)Math.Cos(angles) / 2f, (float)Math.Sin(angles)) * dist;
			}

			if (Projectile.ai[0] < -15)
			{

				Vector2 where = gothere + here;
				Vector2 difference = where - Projectile.Center;

				if ((where - Projectile.Center).Length() > 0f)
				{
					if (toplayer)
					{
						Projectile.velocity += (where - Projectile.Center) * 0.25f;
						Projectile.velocity *= 0.725f;
					}
					else
					{
						Projectile.velocity += (where - Projectile.Center) * 0.005f;
						Projectile.velocity *= 0.925f;
					}
				}

				float maxspeed = Math.Min(Projectile.velocity.Length(), 12 + (toplayer ? player.velocity.Length() : 0)) * startupDelay;
				Projectile.velocity.Normalize();
				Projectile.velocity *= maxspeed;
			}

			Lighting.AddLight(Projectile.Center, prismColor.ToVector3() * 0.78f);

		}

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/SeraphimShardProjectile"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[Projectile.type];
			Player player = Main.player[Projectile.owner];

			float angleadd = MathHelper.Clamp(-Projectile.ai[0] / 45f, 0f, 1f);
			float velAdd = angleadd*(Projectile.velocity.X) * 0.07f;

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			Color color = Color.Lerp((Projectile.GetAlpha(lightColor) * 0.5f), prismColor, 0.75f);

			for (int i = 0; i < Projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
			{
				if (Projectile.oldPos[i] == default)
					Projectile.oldPos[i] = Projectile.position;
			}

			TrailHelper trail = new TrailHelper("DefaultPass", Mod.Assets.Request<Texture2D>("noise").Value);
			trail.color = delegate (float percent)
			{
				return color;
			};
			trail.projsize = Projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -1f);
			trail.trailThickness = 4;
			trail.trailThicknessIncrease = 6;
			trail.strength = startupDelay;
			trail.DrawTrail(Projectile.oldPos.ToList(), Projectile.Center);

			Texture2D tex2 = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];

			if (Projectile.localAI[1] > 0)
			{
				for (int i = Projectile.oldPos.Length-1; i > 0; i -= 1)
				{
					spriteBatch.Draw(tex2, Projectile.oldPos[i]+(new Vector2(Projectile.width,Projectile.height)/2f) - Main.screenPosition, null, (prismColor* 0.50f) * MathHelper.Clamp(Projectile.localAI[1] / 75f, 0f, 1f) * (1f-(i / (float)Projectile.oldPos.Length)), velAdd+Projectile.rotation + MathHelper.Pi, tex2.Size() / 2f, Projectile.scale, default, 0);
				}
			}

			Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
			spriteBatch.Draw(tex2, drawPos2, null, prismColor * 1f, velAdd + Projectile.rotation + MathHelper.Pi, tex2.Size() / 2f, Projectile.scale, default, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Effect hallowed = SGAmod.HallowedEffect;

			hallowed.Parameters["alpha"].SetValue(MathHelper.Clamp((Projectile.localAI[0]) / 90f, 0f, 1f));
			hallowed.Parameters["prismAlpha"].SetValue(1f);
			hallowed.Parameters["prismColor"].SetValue(color.ToVector3());
			hallowed.Parameters["overlayTexture"].SetValue(Mod.Assets.Request<Texture2D>("Perlin").Value);
			float perc = ((Projectile.whoAmI / 255f) * 100f) % 1f;
			hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, Main.GlobalTimeWrappedHourly / 4f, (Main.GlobalTimeWrappedHourly / 5f)+ perc));
			hallowed.Parameters["overlayAlpha"].SetValue(0.75f);
			hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1f,0f, 0f));
			hallowed.Parameters["overlayMinAlpha"].SetValue(0.25f);
			hallowed.Parameters["rainbowScale"].SetValue(0.25f);
			hallowed.Parameters["overlayScale"].SetValue(new Vector2(0.15f, 0.25f));
			hallowed.CurrentTechnique.Passes["Prism"].Apply();

			spriteBatch.Draw(tex, drawPos, null, Color.White, velAdd + Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);


			return false;
		}

		public void DrawAdditive(SpriteBatch spriteBatch)
		{
			Texture2D tex = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];
			Vector2 drawPos = Projectile.Center - Main.screenPosition;

			float angleadd = MathHelper.Clamp(1f - (Projectile.ai[0] / 45f), 0f, 1f);
			float velAdd = angleadd * (Projectile.velocity.X) * 0.07f;

			spriteBatch.Draw(tex, drawPos, null, prismColor * 0.25f, velAdd + Projectile.rotation - MathHelper.Pi, tex.Size() / 2f, Projectile.scale / 1.5f, default, 0);
		}

	}

	public class SeraphimShardBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraphim Shards");
			Description.SetDefault("'What's a Terraprisma?'");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/SeraphimShardBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SeraphimShardProjectile>()] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}

}