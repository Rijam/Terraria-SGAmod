using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Projectiles;
using Idglibrary;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod.Items.Weapons
{
	public class BigBang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Big Bang");
			Tooltip.SetDefault("Honed by the elements, requires a small amount of mana to swing\nFunctions as both a sword and a staff\nHitting with the blade opens rifts that launch Enchanted Swords\nAfter the swing animation hold left mouse to open a rift to fire Cirno bolts!\nThis is 50% of the weapon's base damage multiplied by your magic damage multiplier");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 150;
			Item.DamageType = DamageClass.Melee;
			Item.width = 44;
			Item.height = 52;
			Item.useTime = 20;
			Item.useAnimation = 21;
			Item.crit = 15;
			Item.useStyle = 5;
			Item.autoReuse = true;
			Item.knockBack = 15;
			Item.value = 500000;
			Item.shootSpeed = 8f;
			Item.shoot = Mod.Find<ModProjectile>("ProjectilePortalBigBang").Type;
			Item.rare = 5;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = false;
			Item.useTurn = false;
			Item.channel = true;
			Item.mana = 7;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/BigBang_Glow").Value;
			}

		}

		public override bool CanUseItem(Player player)
		{

			//if (player.statMana<20 || player.ownedProjectileCounts[mod.ProjectileType("ProjectilePortalBigBang")]>0)
			//return false;
			//else
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			foreach(Projectile proj in Main.projectile.Where(testby => testby.owner == player.whoAmI && testby.type == Item.shoot))
            {
				proj.Kill();
            }


			Item.noMelee = false;
			Item.useStyle = 1;
			if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("ProjectilePortalBigBang").Type] > 0)
				return false;
			return true;
			//return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			Vector2 hereas = new Vector2(Main.rand.Next(-1000, 1000), Main.rand.Next(-1000, 1000)); hereas.Normalize();
			hereas *= Main.rand.NextFloat(100f, 200f);
			hereas += target.Center;
			 Vector2 gohere=(target.Center-hereas); gohere.Normalize(); gohere *= 10f;
			int proj = Projectile.NewProjectile(hereas, gohere, Mod.Find<ModProjectile>("ProjectilePortalBBHit").Type, damage, knockBack, player.whoAmI,ProjectileID.EnchantedBeam);
			Main.projectile[proj].penetrate = 2;
			Main.projectile[proj].netUpdate = true;
			IdgProjectile.Sync(proj);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, Mod.Find<ModDust>("TornadoDust").Type);// 20);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
				Main.dust[dust].noGravity = true;
			}

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
			CreateRecipe(1).AddIngredient(mod.ItemType("DormantSupernova"), 1).AddIngredient(mod.ItemType("ForagersBlade"), 1).AddIngredient(mod.ItemType("IceScepter"), 1).AddIngredient(mod.ItemType("RubiedBlade"), 1).AddIngredient(mod.ItemType("MangroveStriker"), 1).AddIngredient(mod.ItemType("CryostalBar"), 12).AddIngredient(mod.ItemType("VirulentBar"), 10).AddIngredient(mod.ItemType("WraithFragment4"), 16).AddIngredient(mod.ItemType("OmniSoul"), 6).AddIngredient(mod.ItemType("Fridgeflame"), 10).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
			CreateRecipe(1).AddIngredient(mod.ItemType("DormantSupernova"), 1).AddIngredient(mod.ItemType("RustworkBlade"), 1).AddIngredient(mod.ItemType("IceScepter"), 1).AddIngredient(mod.ItemType("RubiedBlade"), 1).AddIngredient(mod.ItemType("MangroveStriker"), 1).AddIngredient(mod.ItemType("CryostalBar"), 12).AddIngredient(mod.ItemType("VirulentBar"), 10).AddIngredient(mod.ItemType("WraithFragment4"), 16).AddIngredient(mod.ItemType("OmniSoul"), 6).AddIngredient(mod.ItemType("Fridgeflame"), 10).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}


	}


	public class ProjectilePortalBBHit : ProjectilePortal
	{
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
			Projectile.timeLeft = 100;
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

					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(10)) * Projectile.velocity.Length();
					int proj = Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), (int)Projectile.ai[0], Projectile.damage, Projectile.knockBack, owner.whoAmI);
					Main.projectile[proj].DamageType = DamageClass.Melee;
					Main.projectile[proj].netUpdate = true;
					IdgProjectile.Sync(proj);
				}

			}

		}

	}


	public class ProjectilePortalBigBang : ProjectilePortalDSupernova
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova");
		}

		public override int projectilerate => 40;
		public override int manacost => 15;
		public override int portalprojectile => Mod.Find<ModProjectile>("CirnoBoltPlayer").Type;
		public override int takeeffectdelay =>  Main.player[Projectile.owner].HeldItem.useTime;
		public override float damagescale => 0.50f * Main.player[Projectile.owner].GetDamage(DamageClass.Magic);
		public override int penetrate => 1;

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
