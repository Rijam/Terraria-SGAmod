using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Projectiles;
using Idglibrary;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class RealityShaper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reality Shaper");
			Tooltip.SetDefault("The elements are yours to shape\nRequires a small amount of mana to swing, Double tab to launch 2 heat waves in succession\nFunctions as both a sword and a staff\nHitting with the blade opens several rifts that launch Sky Fracture Blades into the target\nAfter the swing animation hold left mouse to open a rift\nThis will fire fast moving Cirno bolts\nFurthermore, portals appear behind you that summon Hot Rounds!\nThe damage of these are less than the melee damage but are improved by your magic damage multiplier\nIf done after a double tap, you'll summon 2 portals to shoot twice as many projectiles, at double the mana costs");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 340;
			Item.crit = 15;
			Item.DamageType = DamageClass.Melee;
			Item.width = 44;
			Item.height = 52;
			Item.useTime = 10;
			Item.useAnimation = 11;
			Item.useStyle = 5;
			Item.knockBack = 15;
			Item.value = 1500000;
			Item.shootSpeed = 28f;
			Item.shoot = Mod.Find<ModProjectile>("ProjectilePortalRealityShaper").Type;
			Item.rare = 11;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = false;
			Item.useTurn = false;
			Item.channel = true;
			Item.mana = 20;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/RealityShaper_Glow").Value;
			}

		}

		public override bool CanUseItem(Player player)
		{
			if (player.statMana<20 || player.ownedProjectileCounts[Mod.Find<ModProjectile>("ProjectilePortalRealityShaper").Type]>1)
			return false;
			else
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Item.noMelee = false;
			Item.useStyle = 1;

			float speed = 1.5f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(8);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			SoundEngine.PlaySound(SoundID.Item, player.Center, 45);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("HeatWave").Type, (int)((float)damage * 0.15f), knockBack / 3f, player.whoAmI);
				Main.projectile[proj].DamageType = DamageClass.Melee;
				// Main.projectile[proj].magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
				Main.projectile[proj].netUpdate = true;
				IdgProjectile.Sync(proj);

				//NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
			}
			//SGAPlayer.LimitProjectiles(player, 0, new ushort[] {(ushort)mod.ProjectileType("ProjectilePortalRealityShaper") });
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{

			for (int i = 0; i < 360; i += 36)
			{
				float angle = MathHelper.ToRadians(i);
				Vector2 hereas = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 256;
				hereas += target.Center;
				Vector2 gohere = (target.Center - hereas); gohere.Normalize(); gohere *= 16f;
				int proj = Projectile.NewProjectile(hereas, gohere, Mod.Find<ModProjectile>("ProjectilePortalRealityShaperFracturePortal").Type, (int)(damage*0.2f), knockBack, player.whoAmI, ProjectileID.SkyFracture);
				// Main.projectile[proj].magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
				Main.projectile[proj].DamageType = DamageClass.Melee;
				Main.projectile[proj].timeLeft = 40;
				Main.projectile[proj].netUpdate = true;
				IdgProjectile.Sync(proj);

			}
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			/*for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 20);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
				Main.dust[dust].noGravity = true;
			}*/

			for (int num475 = 3; num475 < 5; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 27);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 15f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 3f) + ((player.direction) * player.itemRotation.ToRotationVector2() * (float)num475);
				Main.dust[dust].noGravity = true;
			}

			Lighting.AddLight(player.position, 0.1f, 0.1f, 0.9f);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("BigBang"), 1).AddIngredient(mod.ItemType("HeatWave"), 1).AddIngredient(ItemID.SkyFracture, 1).AddIngredient(ItemID.ChristmasTreeSword, 1).AddIngredient(ItemID.InfluxWaver, 1).AddIngredient(mod.ItemType("CircuitBreakerBlade"), 1).AddIngredient(ItemID.DD2SquireBetsySword, 1).AddIngredient(mod.ItemType("OmegaSigil"), 1).AddIngredient(mod.ItemType("OmniSoul"), 8).AddIngredient(mod.ItemType("PrismalBar"), 10).AddIngredient(mod.ItemType("StarMetalBar"), 10).AddIngredient(mod.ItemType("Entrophite"), 100).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}


	}

	public class ProjectilePortalRealityShaperFracturePortal : ProjectilePortal
	{
		public override int takeeffectdelay => 0;
		public override float damagescale => 1f;
		public override int penetrate => 1;
		public override int openclosetime => 16;

	}

	public class ProjectilePortalRealityShaperHit : ProjectilePortal
	{
		public override int takeeffectdelay => 0;
		public override float damagescale => 1f;
		public override int penetrate => 1;
		public override int openclosetime => 16;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spawner");
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			//projectile.aiStyle = 1;
			Projectile.friendly = true;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			Projectile.timeLeft = 40;
			Projectile.tileCollide = false;
			AIType = -1;
		}

		public override void Explode()
		{

			if (Projectile.timeLeft == 30 && Projectile.ai[0] > 0)
			{
				Player owner = Main.player[Projectile.owner];
				if (owner != null && !owner.dead)
				{

					Vector2 gotohere = new Vector2();
					gotohere = Projectile.velocity;//Main.MouseScreen - projectile.Center;
					gotohere.Normalize();

					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(6)) * Projectile.velocity.Length();
					int proj = Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), (int)Projectile.ai[0], Projectile.damage, Projectile.knockBack, owner.whoAmI);
					Main.projectile[proj].DamageType = DamageClass.Melee;
					IdgProjectile.Sync(proj);
				}

			}

		}

	}


	public class ProjectilePortalRealityShaper : ProjectilePortalDSupernova
	{
		public override int openclosetime => 20;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova");
		}

		public override int projectilerate => 25;
		public override int manacost => 6;
		public override int portalprojectile => Mod.Find<ModProjectile>("CirnoBoltPlayer").Type;
		public override int takeeffectdelay =>  Main.player[Projectile.owner].HeldItem.useTime;
		public override float damagescale => 0.50f * Main.player[Projectile.owner].GetDamage(DamageClass.Magic);
		public override int penetrate => 1;
		public override int startrate => 60;
		public override int drainrate => 5;
		public override int timeleftfirerate => 20;

		public int everyother = 0;

		public override void Explode()
		{

			if (Projectile.timeLeft == timeleftfirerate && Projectile.ai[0] > 0)
			{
				Player owner = Main.player[Projectile.owner];

					if (owner != null && !owner.dead && owner.channel)
				{
					everyother += 1;
					everyother %= 2;

					Vector2 gotohere = new Vector2();
					gotohere = Projectile.velocity;//Main.MouseScreen - projectile.Center;
					gotohere.Normalize();

					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(15)) * Projectile.velocity.Length();
					if (everyother == 1)
					{
						int proj = Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y)*2f, (int)Projectile.ai[0], (int)(Projectile.damage*1f* damagescale), Projectile.knockBack / 10f, owner.whoAmI);
						Main.projectile[proj].DamageType = DamageClass.Magic;
						// Main.projectile[proj].melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
						Main.projectile[proj].timeLeft = 300;
						Main.projectile[proj].penetrate = penetrate;
						IdgProjectile.Sync(proj);
					}


					Vector2 backthere = (owner.Center- gotohere*32)-(new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(135))*96);
					Vector2 gohere = Main.MouseWorld - backthere;
					gohere.Normalize();
					gohere *= 28f;
					gohere = gohere.RotatedByRandom(MathHelper.ToRadians(20));

					int proj2 = Projectile.NewProjectile(backthere, gohere, Mod.Find<ModProjectile>("ProjectilePortalRealityShaperHit").Type, (int)(Projectile.damage*1.25f * damagescale), Projectile.knockBack / 6f, owner.whoAmI, Mod.Find<ModProjectile>("HotRound").Type);
					// Main.projectile[proj2].melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
					IdgProjectile.Sync(proj2);

				}

			}

		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 10;
			Projectile.light = 0.5f;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 38;
		}

	}

}
