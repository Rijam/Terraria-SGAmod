using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.HavocGear.Projectiles;
using SGAmod.Effects;
using Idglibrary;
using System.Linq;
using SGAmod.Buffs;

namespace SGAmod.Items.Weapons
{
	public class CrackedMirror : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cracked Mirror");
			Tooltip.SetDefault("'You can almost hear a petrifying scream coming from the mirror'\nReleases petrifying apparitions around the player on use that petrify enemies\nThese petrified enemies take far more damage to mining tools\nHowever, far less damage to anything else\nOnly affects specific enemy types\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 60 seconds"));
		}
		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.width = 22;
			Item.height = 22;
			Item.useTime = 100;
			Item.useAnimation = 100;
			Item.useStyle = 5;
			Item.knockBack = 10;
			Item.value = 10000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item72;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<CrackedMirrorProj>();
			Item.shootSpeed = 2;
			Item.staff[Item.type] = true;
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.SGAPly().AddCooldownStack(60 * 60);
			float numberProjectiles = 10; // 3, 4, or 5 shots
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 40f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy((i / (float)numberProjectiles) * MathHelper.TwoPi);
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, 1, knockBack, player.whoAmI);
			}
			return false;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 4);
		}
	}

	public class CrackedMirrorProj : ModProjectile
	{

		float scalePercent => MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, Math.Min(Projectile.localAI[0] / 25f, 0.75f));
		Vector2 startingloc = default;
		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 300;
			Projectile.light = 0.1f;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			AIType = -1;
			Main.projFrames[Projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Say STONE!");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public static bool AffectNPC(NPC npctest)
        {
			return npctest.active && !npctest.friendly && !npctest.immortal && !npctest.boss && !npctest.noGravity && !npctest.noTileCollide;
		}

		public override void AI()
		{
			if (startingloc == default)
			{
				startingloc = Projectile.Center;
			}

			//projectile.velocity = Collision.TileCollision(projectile.position, projectile.velocity, projectile.width, projectile.height, true);
			Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.localAI[0] / 10000f, Vector2.Zero);

			foreach (NPC enemy in Main.npc.Where(npctest => AffectNPC(npctest) &&
			npctest.Hitbox.Intersects(Projectile.Hitbox)))
			{
				enemy.AddBuff(ModContent.BuffType<Petrified>(), 600);
			}

			Projectile.localAI[0] += 1;
			int num126 = Dust.NewDust(Projectile.position - new Vector2(2, 2), Main.rand.Next(Projectile.width + 6), Main.rand.Next(Projectile.height + 6), DustID.t_Marble, 0, 0, 240, Color.White, scalePercent);
			Main.dust[num126].noGravity = true;
			Main.dust[num126].velocity = Projectile.velocity * 0.5f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 0; i < Projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
			{
				if (Projectile.oldPos[i] == default)
					Projectile.oldPos[i] = Projectile.position;
			}

			TrailHelper trail = new TrailHelper("DefaultPass", Mod.Assets.Request<Texture2D>("noise").Value);
			trail.color = delegate (float percent)
			{
				return Color.White;
			};
			trail.projsize = Projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -1f);
			trail.trailThickness = 4;
			trail.trailThicknessIncrease = 6;
			trail.capsize = new Vector2(6f, 0f);
			trail.strength = scalePercent;
			trail.DrawTrail(Projectile.oldPos.ToList(), Projectile.Center);

			return false;
		}
	}

}
