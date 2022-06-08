using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.NPCs;
using Idglibrary;

using System.Linq;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class Snowfall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snowfall");
			Tooltip.SetDefault("Summon a cloud to rain hardened snowballs on your foes\nLimits 2 clouds at a time");
		}

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Summon;
			Item.damage = 40;
			Item.mana = 50;
			Item.width = 44;
			Item.height = 52;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.value = 75000;
			Item.noMelee = true;
			Item.rare = ItemRarityID.Pink;
			Item.shoot = 10;
			Item.shootSpeed = 10f;
			Item.UseSound = SoundID.Item60;
			Item.autoReuse = true;
			Item.useTurn = false;

		}

		public void Limit(Player player)
        {
			SGAPlayer.LimitProjectiles(player, 1, new int[] { ModContent.ProjectileType<SnowfallCloud>(), ModContent.ProjectileType<SnowCloud>(), ModContent.ProjectileType<CursedHailCloud>(), ModContent.ProjectileType<CursedHailProj>(), ModContent.ProjectileType<YellowWinterCloud>(), ModContent.ProjectileType<YellowWinterProj>() });
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			Limit(player);

			int theproj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("SnowfallCloud").Type, damage, knockBack, player.whoAmI);
			float num12 = (float)Main.mouseX + Main.screenPosition.X;
			float num13 = (float)Main.mouseY + Main.screenPosition.Y;
			HalfVector2 half = new HalfVector2(num12, num13);
			Main.projectile[theproj].ai[0] = ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
			Main.projectile[theproj].netUpdate = true;
			return false;
		}


		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, Mod.Find<ModDust>("HotDust").Type);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = randomcircle / 2f;
				Main.dust[dust].noGravity = true;
				//Main.dust[dust].velocity.Normalize();
				//Main.dust[dust].velocity+=new Vector2(player.velocity.X/4,0f);
				//Main.dust[dust].velocity*=(((float)Main.rand.Next(0,100))/30f);
			}
			Lighting.AddLight(player.position, 0.9f, 0.9f, 0f);
		}


	}


	public class SnowfallCloud : ModProjectile
	{

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			//projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			Projectile.timeLeft = 60 * 30;
			Projectile.tileCollide = false;
		}

		public override string Texture
		{
			get { return "Terraria/Item_" + 5; }
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}
		public override bool CanHitPlayer(Player target)
		{
			return false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
		public override void AI()
		{

			Vector2 gohere = new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(Projectile.ai[0]) }.ToVector2();
			Vector2 anglevect = (gohere - Projectile.position);
			float length = anglevect.Length();
			anglevect.Normalize();
			Projectile.velocity = (10f * anglevect);
			bool reached = (length < Projectile.velocity.Length() + 1f);
			for (int q = 0; q < (reached ? 40 : 4); q++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				float reachfloat = reached ? 0f : 1f;
				int dust = Dust.NewDust(Projectile.position - new Vector2(8, 0), 16, 16, DustID.Smoke, ((Projectile.velocity.X * 0.75f) * reachfloat) + (randomcircle * (reached ? 12f : 0f)).X, ((Projectile.velocity.Y * 0.75f) * reachfloat) + (randomcircle * (reached ? 4f : 0f)).Y, 100, Main.hslToRgb(0.6f, 0.8f, 0.8f), 3f);
				Main.dust[dust].noGravity = true;
			}
			if (reached)
			{
				int theproj = Projectile.NewProjectile(Projectile.position.X + 16f, Projectile.position.Y, 0f, 0f, Mod.Find<ModProjectile>("SnowCloud").Type, Projectile.damage, Projectile.knockBack, Projectile.owner);
				Main.projectile[theproj].friendly = true;
				Main.projectile[theproj].hostile = false;
				Main.projectile[theproj].timeLeft = Projectile.timeLeft;
				Main.projectile[theproj].netUpdate = true;
				Projectile.Kill();
			}
		}

	}

	public class CursedHail : Snowfall
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Hail");
			Tooltip.SetDefault("Summon a corrupted cloud to rain cursed ice shards on your foes\nLimits 2 clouds at a time");
		}

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Summon;
			Item.damage = 65;
			Item.mana = 50;
			Item.width = 44;
			Item.height = 52;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.value = 200000;
			Item.noMelee = true;
			Item.rare = ItemRarityID.Lime;
			Item.shoot = 10;
			Item.shootSpeed = 10f;
			Item.UseSound = SoundID.Item60;
			Item.autoReuse = true;
			Item.useTurn = false;

		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			Limit(player);

			int theproj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("CursedHailProj").Type, damage, knockBack, player.whoAmI);
			float num12 = (float)Main.mouseX + Main.screenPosition.X;
			float num13 = (float)Main.mouseY + Main.screenPosition.Y;
			HalfVector2 half = new HalfVector2(num12, num13);
			Main.projectile[theproj].ai[0] = ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
			Main.projectile[theproj].netUpdate = true;
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Snowfall>(), 1).AddIngredient(ModContent.ItemType<Consumables.GasPasser>(), 3).AddIngredient(ItemID.CursedFlame, 12).AddIngredient(ItemID.BlizzardStaff, 1).AddTile(TileID.MythrilAnvil).Register();

		}


	}

	public class CursedHailProj : SnowfallCloud
	{

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			//projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			Projectile.timeLeft = 60 * 30;
			Projectile.tileCollide = false;
		}

		public override string Texture
		{
			get { return "Terraria/Item_" + 5; }
		}
		public override void AI()
		{

			Vector2 gohere = new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(Projectile.ai[0]) }.ToVector2();
			Vector2 anglevect = (gohere - Projectile.position);
			float length = anglevect.Length();
			anglevect.Normalize();
			Projectile.velocity = (10f * anglevect);
			bool reached = (length < Projectile.velocity.Length() + 1f);
			for (int q = 0; q < (reached ? 40 : 4); q++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				float reachfloat = reached ? 0f : 1f;
				int dust = Dust.NewDust(Projectile.position - new Vector2(8, 0), 16, 16, DustID.Smoke, ((Projectile.velocity.X * 0.75f) * reachfloat) + (randomcircle * (reached ? 12f : 0f)).X, ((Projectile.velocity.Y * 0.75f) * reachfloat) + (randomcircle * (reached ? 4f : 0f)).Y, 100, Color.LimeGreen, 3f);
				Main.dust[dust].noGravity = true;
			}
			if (reached)
			{
				int theproj = Projectile.NewProjectile(Projectile.position.X + 16f, Projectile.position.Y, 0f, 0f, Mod.Find<ModProjectile>("CursedHailCloud").Type, Projectile.damage, Projectile.knockBack, Projectile.owner);
				Main.projectile[theproj].friendly = true;
				Main.projectile[theproj].hostile = false;
				Main.projectile[theproj].timeLeft = Projectile.timeLeft;
				Main.projectile[theproj].netUpdate = true;
				Projectile.Kill();

			}

		}

	}


	public class CursedHailCloud : SnowCloud
	{

		public override int projectileid => ModContent.ProjectileType<CursedHailProjectile>();
		public override Color colorcloud => Color.LimeGreen;
		public override int rate => 4;

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			//projectile.aiStyle = 1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = false;
		}

	}

	public class CursedHailProjectile : ModProjectile
	{

		int fakeid = ProjectileID.FrostShard;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Hail");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.CloneDefaults(fakeid);
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.penetrate = 5;
			Projectile.minion = true;
			Projectile.extraUpdates = 2;
			Projectile.coldDamage = true;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + fakeid; }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (Projectile.localAI[0] < 100)
				Projectile.localAI[0] = 100 + Main.rand.Next(0, 3);
			Texture2D tex = SGAmod.HellionTextures[5];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 5) / 2f;
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
			int timing = (int)(Projectile.localAI[0] - 100);
			timing %= 5;
			timing *= ((tex.Height) / 5);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 5), lightColor, MathHelper.ToRadians(180) + Projectile.velocity.X * -0.08f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = fakeid;
			return true;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.velocity += new Vector2(0, 10);
				Projectile.velocity *= 0.75f;
			}
			int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 75);
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.NextFloat(0.1f, 0.25f));
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.CursedInferno, 60 * 5);
			target.immune[Projectile.owner] = 4;
		}

	}

	public class YellowWinter : Snowfall
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yellow Winter");
			Tooltip.SetDefault("Summon a golden cloud to shower your foes with yellow snow\nLimits 2 clouds at a time");
		}

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Summon;
			Item.damage = 40;
			Item.mana = 50;
			Item.width = 44;
			Item.height = 52;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 1;
			Item.knockBack = 2;
			Item.value = 100000;
			Item.noMelee = true;
			Item.rare = ItemRarityID.Yellow;
			Item.shoot = 10;
			Item.shootSpeed = 10f;
			Item.UseSound = SoundID.Item60;
			Item.autoReuse = true;
			Item.useTurn = false;

		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			Limit(player);

			int theproj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, Mod.Find<ModProjectile>("YellowWinterProj").Type, damage, knockBack, player.whoAmI);
			float num12 = (float)Main.mouseX + Main.screenPosition.X;
			float num13 = (float)Main.mouseY + Main.screenPosition.Y;
			HalfVector2 half = new HalfVector2(num12, num13);
			Main.projectile[theproj].ai[0] = ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
			Main.projectile[theproj].netUpdate = true;
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Snowfall>(), 1).AddIngredient(ModContent.ItemType<Consumables.DivineShower>(), 1).AddIngredient(ModContent.ItemType<Consumables.Jarate>(), 5).AddIngredient(ItemID.CandyCorn, 100).AddTile(TileID.MythrilAnvil).Register();

		}


	}

	public class YellowWinterProj : SnowfallCloud
	{

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			//projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			Projectile.timeLeft = 60 * 30;
			Projectile.tileCollide = false;
		}

		public override string Texture
		{
			get { return "Terraria/Item_" + 5; }
		}
		public override void AI()
		{

			Vector2 gohere = new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(Projectile.ai[0]) }.ToVector2();
			Vector2 anglevect = (gohere - Projectile.position);
			float length = anglevect.Length();
			anglevect.Normalize();
			Projectile.velocity = (10f * anglevect);
			bool reached = (length < Projectile.velocity.Length() + 1f);
			for (int q = 0; q < (reached ? 40 : 4); q++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				float reachfloat = reached ? 0f : 1f;
				int dust = Dust.NewDust(Projectile.position - new Vector2(8, 0), 16, 16, DustID.Smoke, ((Projectile.velocity.X * 0.75f) * reachfloat) + (randomcircle * (reached ? 12f : 0f)).X, ((Projectile.velocity.Y * 0.75f) * reachfloat) + (randomcircle * (reached ? 4f : 0f)).Y, 100, Color.Yellow, 3f);
				Main.dust[dust].noGravity = true;
			}
			if (reached)
			{
				int theproj = Projectile.NewProjectile(Projectile.position.X + 16f, Projectile.position.Y, 0f, 0f, ModContent.ProjectileType<YellowWinterCloud>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				Main.projectile[theproj].friendly = true;
				Main.projectile[theproj].hostile = false;
				Main.projectile[theproj].timeLeft = Projectile.timeLeft;
				Main.projectile[theproj].netUpdate = true;
				Projectile.Kill();

			}

		}

	}
	public class YellowWinterCloud : SnowCloud
	{

		public override int projectileid => ModContent.ProjectileType<JarateShurikensProg>();
		public override Color colorcloud => Color.Yellow;
		public override int rate => 5;

		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			//projectile.aiStyle = 1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = false;
		}

	}


	public class RubiedBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rubied Blade");
			Tooltip.SetDefault("Rains down ruby bolts from the sky above enemies hit by the blade");
		}
		public override void SetDefaults()
		{
			Item.damage = 42;
			Item.DamageType = DamageClass.Melee;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = 1;
			Item.knockBack = 4;
			Item.crit = 7;
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = 5;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/RubiedBlade_Glow").Value;
			}

		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			Vector2 hereas = new Vector2(Main.rand.Next(-1000, 1000), Main.rand.Next(-1000, 1000)); hereas.Normalize();
			hereas *= Main.rand.NextFloat(50f, 100f);
			hereas += target.Center;
			hereas -= new Vector2(0, 800);
			Vector2 gohere = ((hereas + new Vector2(Main.rand.NextFloat(-100f, 100f), 800f)) - hereas); gohere.Normalize(); gohere *= 15f;
			int proj = Projectile.NewProjectile(hereas, gohere, ProjectileID.RubyBolt, (int)(damage * 1), knockBack, player.whoAmI);
			Main.projectile[proj].DamageType = DamageClass.Melee;
			// Main.projectile[proj].magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Main.projectile[proj].timeLeft = 300;
			IdgProjectile.Sync(proj);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			int dustIndex = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 32, 32, 113);
			Dust dust = Main.dust[dustIndex];
			Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
			dust.velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
			dust.scale *= 1.5f + Main.rand.Next(-30, 31) * 0.01f;
			dust.fadeIn = 0f;
			dust.noGravity = true;
			dust.color = Color.LightSkyBlue;
		}

	}

	public class Starburster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Burster");
			Tooltip.SetDefault("Fires 4 stars in bursts at the cost of 1, but requires a small amount of mana\nuses Fallen Stars for Ammo, the stars cannot pass through platforms");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.GoldenShower);
			Item.damage = 38;
			Item.useAnimation = 18;
			Item.useTime = 5;
			Item.reuseDelay = 20;
			Item.knockBack = 6;
			Item.value = 75000;
			Item.rare = ItemRarityID.Pink;
			// item.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Item.DamageType = DamageClass.Ranged;
			Item.mana = 10;
			Item.shoot = ProjectileID.StarAnise;
			Item.shootSpeed = 9f;
			Item.useAmmo = AmmoID.FallenStar;
		}

		public override bool CanUseItem(Player player)
		{
			return (player.CountItem(ItemID.FallenStar) > 0);
		}

		public override bool ConsumeAmmo(Player player)
		{
			if (player.itemAnimation > 6)
				return false;

			return base.ConsumeAmmo(player);
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int Itd = type;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			float rander = Main.rand.Next(7000, 8000) / 2000;
			if (type == ProjectileID.StarAnise)
				Itd = 9;
			int probg = Terraria.Projectile.NewProjectile(position.X + (int)speedX * 4, position.Y + (int)speedY * 4, speedX * (rander), speedY * (rander), Itd, damage, knockBack, player.whoAmI);
			Main.projectile[probg].DamageType = DamageClass.Ranged;
			Main.projectile[probg].tileCollide = true;
			return false;
		}

	}

	public class Starfishburster : Starburster
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star'Fish' Burster");
			Tooltip.SetDefault("Fires 4 starfish in bursts at the cost of 1, but requires a small amount of mana\nStarfish bounce off walls and pierce\nUses Starfish as ammo\n66% to not consume ammo");
		}
		public override bool CanUseItem(Player player)
		{
			return (player.CountItem(ItemID.Starfish) > 0);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.GoldenShower);
			Item.damage = 50;
			Item.useAnimation = 18;
			Item.useTime = 5;
			Item.reuseDelay = 20;
			Item.knockBack = 6;
			Item.value = 250000;
			Item.rare = 7;
			// item.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Item.DamageType = DamageClass.Ranged;
			//item.shoot = mod.ProjectileType("SunbringerFlare");
			Item.shoot = Mod.Find<ModProjectile>("StarfishProjectile").Type;
			Item.shootSpeed = 11f;
			Item.useAmmo = 2626;
		}

		/*
		public override bool ConsumeAmmo(Player player)
		{
			return Main.rand.Next(100) < 33;
		}
		*/


		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "StarfishBlaster", 1).AddIngredient(null, "Starburster", 1).AddIngredient(null, "WraithFragment4", 10).AddIngredient(null, "CryostalBar", 8).AddIngredient(null, "SharkTooth", 100).AddIngredient(ItemID.Coral, 8).AddIngredient(ItemID.Starfish, 5).AddIngredient(ItemID.HallowedBar, 6).AddTile(mod.TileType("ReverseEngineeringStation")).Register();
		}

	}

	public class IceScepter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Scepter");
			Tooltip.SetDefault("Spews homing ice bolts with duo swirling ice shards\nShards fly off when the bolt hits something and do 50% of base damage\nAlt Fire summons a wall of ice blocks");
		}

		public override void SetDefaults()
		{
			Item.damage = 36;
			Item.DamageType = DamageClass.Magic;
			Item.width = 34;
			Item.mana = 8;
			Item.height = 24;
			Item.useTime = 17;
			Item.useAnimation = 17;
			Item.useStyle = 5;
			Item.knockBack = 2;
			Item.value = 50000;
			Item.rare = ItemRarityID.Pink;
			Item.shootSpeed = 8f;
			Item.noMelee = true;
			Item.shoot = 14;
			Item.shootSpeed = 11f;
			Item.UseSound = SoundID.Item8;
			Item.autoReuse = true;
			Item.staff[Item.type] = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			if (player.altFunctionUse == 2)
			{
				if (player.statMana > 70)
				{
					speedX *= 2f;
					speedY *= 2f;
					position += Vector2.Normalize(new Vector2(speedX, speedY)) * 48f;
					for (int i = -4; i < 5; i += 1)
					{

						float rotation = MathHelper.ToRadians(32);
						Vector2 perturbedSpeed = (new Vector2(speedX, speedY)).RotatedBy(MathHelper.ToRadians(i * 5)) * 1.2f;
						Vector2 dister = perturbedSpeed;
						dister.Normalize();

						int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.IceBlock, (int)(damage / 5), knockBack, player.whoAmI, ((position + dister * 160f).X) / 16f, (position + dister * 160f).Y / 16f);
						Main.projectile[proj].friendly = true;
						Main.projectile[proj].hostile = false;
						Main.projectile[proj].timeLeft = 240;
						Main.projectile[proj].knockBack = Item.knockBack;
						player.itemTime = 90;

					}
					player.CheckMana(Item,60,true);
					player.manaRegenDelay = 1200;
				}


			}
			else
			{

				float rotation = MathHelper.ToRadians(4);
				speedX *= 4f;
				speedY *= 4f;
				position += Vector2.Normalize(new Vector2(speedX, speedY)) * 48f;
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY)).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f;

				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, Mod.Find<ModProjectile>("CirnoBoltPlayer").Type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].timeLeft = 240;
				Main.projectile[proj].knockBack = Item.knockBack;

			}
			return false;
		}

	}

	class IcicleFall : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Icicle Fall");
			Tooltip.SetDefault("Throws an ice chunk that splits apart into 6 shards when falling downwards\nThese shards do full base damage");
		}

		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.damage = 20;
			Item.crit = 0;
			Item.knockBack = 0f;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.maxStack = 1;
			Item.shootSpeed = 12f;
			Item.shoot = Mod.Find<ModProjectile>("CirnoIceShardPlayerCore").Type;
			Item.value = Item.buyPrice(0, 2, 5, 0);
			Item.rare = 6;
		}

	}

	public class CirnoIceShardPlayerCore : ModProjectile
	{

		int fakeid = ProjectileID.FrostShard;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Baka Ice Core");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(fakeid);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.coldDamage = true;
			Projectile.extraUpdates = 0;
			Projectile.aiStyle = -1;
		}

		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.FrostCore; }
		}

		public override void AI()
		{

			Projectile.rotation += Projectile.velocity.X * 0.02f;
			Projectile.velocity.Y += 0.2f;
			if (Projectile.velocity.Y > 0)
			{
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 30, 1f, -0.5f);
				for (float xx = 0f; xx < 6f; xx += 1f)
				{
					int proj2 = Projectile.NewProjectile(Projectile.Center, Projectile.velocity + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0, 3f)), Mod.Find<ModProjectile>("CirnoIceShardPlayer").Type, (int)((float)Projectile.damage * 1f), 0f, Projectile.owner);
					Main.projectile[proj2].friendly = true;
					Main.projectile[proj2].hostile = false;
					Main.projectile[proj2].netUpdate = true;
				}
				Projectile.Kill();
			}
			for (float i = 0f; i < 4f; i += 1f)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Ice);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.NextFloat(0.1f, 0.25f) * i);
			}
		}

	}

	public class CirnoIceShardPlayer : CirnoIceShard
	{

		int fakeid = ProjectileID.FrostShard;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Baka Ice Shard");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.CloneDefaults(fakeid);
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.coldDamage = true;
			Projectile.extraUpdates = 0;
			Projectile.aiStyle = -1;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + fakeid; }
		}

		public override void AI()
		{
			base.AI();
			Projectile.hostile = false;
			Projectile.friendly = true;
			if (Projectile.ai[1]<=0)
			Projectile.velocity.Y += 0.1f;
		}
	}

	public class CirnoIceShardPlayerOrbit : CirnoIceShardPlayer
	{

		int fakeid = ProjectileID.FrostShard;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Baka Ice Shard");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			// projectile.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 0;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			Projectile.ai[0] += 0.1f;
			Projectile.localAI[1] += 0.2f;
			Projectile parent = Main.projectile[(int)Projectile.ai[1]];
			if (parent.active && parent.type == Mod.Find<ModProjectile>("CirnoBoltPlayer") .Type&& Projectile.ai[0]>-5000)
			{
				Projectile.velocity = (Projectile.ai[0] + MathHelper.ToRadians(90)).ToRotationVector2() * 7;
				Projectile.Center = parent.Center + (Projectile.ai[0]).ToRotationVector2()*Math.Min(Projectile.localAI[1],12f); 
			}
			else
			{
				Projectile.ai[0] = -10000;
			}

			base.AI();
		}

	}

	public class RodOfTheMistyLake : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rod of The Misty Lake");
			Tooltip.SetDefault("Summons Ice Fairies to fight for you with blasts of ice\nThey orbit around you and seek out enemies\nPeriodically converts projectiles nearby into Cold Damage\nThis also boosts their damage by 25% of your summon damage\nCosts 2 minion slots");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.knockBack = 3f;
			Item.mana = 10;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = 1;
			Item.value = Item.buyPrice(0, 2, 50, 0);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<CirnoMinionBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<CirnoMinionProjectile>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(Item.buffType, 2);

			return true;
		}

	}

	public class CirnoMinionProjectile : ModProjectile
	{
		protected float idleAccel = 0.05f;
		protected float spacingMult = 1f;
		protected float viewDist = 400f;
		protected float chaseDist = 200f;
		protected float chaseAccel = 6f;
		protected float inertia = 40f;
		protected float shootCool = 90f;
		protected float shootSpeed;
		protected int shoot;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Fairy Minion");
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 2f;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 60;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		// This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage()
		{
			return false;
		}

		public virtual void CreateDust()
		{
		}

		public virtual void SelectFrame()
		{
		}

		public override void AI()
		{
			//if (projectile.owner == null || projectile.owner < 0)
			//return;

			Projectile.ai[0]++;
			Projectile.localAI[0]++;
			Player player = Main.player[Projectile.owner];
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<CirnoMinionBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<CirnoMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			Vector2 gothere = player.Center;



			float us = 0f;
			float maxus = 0f;

			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (currentProjectile.active // Make sure the projectile is active
				&& currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
				&& currentProjectile.type == Projectile.type)
				{ // Make sure the projectile is of the same type as this javelin

					if (i == Projectile.whoAmI)
						us = maxus;
					maxus += 1f;

				}

			}

			float percent = us / maxus;
			int ptimer = player.SGAPly().timer;

			double angles = (percent*MathHelper.TwoPi) + ((ptimer * player.direction) / 30f);
			float dist = 64f;
			Vector2 here = Vector2.One.RotatedBy(angles) * dist;
			Vector2 where = gothere + here;

			NPC enemy = null;

			List<NPC> enemies = SGAUtils.ClosestEnemies(Projectile.Center,1200,player.Center);

			if (enemies != null && enemies.Count > 0)
            {
				enemy = enemies[0];
			}

			if (player.HasMinionAttackTargetNPC)
            {
				enemy = Main.npc[player.MinionAttackTargetNPC];
			}

			if (enemy != null)
            {
				where = enemy.Center + Vector2.Normalize(Projectile.Center - enemy.Center).RotatedBy((-MathHelper.PiOver2+(angles*MathHelper.Pi))/1.5f) * (dist*2);

				Vector2 generalDist = enemy.Center - Projectile.Center;

				Projectile.spriteDirection = generalDist.X > 0 ? 1 : -1;

				if ((int)Projectile.ai[0]%80==0 && generalDist.Length()< dist + 360)
                {
					for (float xx = 0f; xx < 6f; xx += 1f)
					{
						Vector2 aiming = Vector2.Normalize(generalDist) * 32f + new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3, 3f));

						Vector2 distxxx = SGAUtils.PredictiveAim(aiming.Length(), Projectile.Center, enemy.Center, enemy.velocity, false) - (Projectile.Center);

						int proj2 = Projectile.NewProjectile(Projectile.Center,(Vector2.Normalize(distxxx) * aiming.Length()) + new Vector2(Main.rand.NextFloat(-8f, 8f), Main.rand.NextFloat(-8, 8f)), ProjectileID.IceBolt, (int)((float)Projectile.damage * 1f), 0f, Projectile.owner);
						Main.projectile[proj2].friendly = true;
						Main.projectile[proj2].minion = true;
						Main.projectile[proj2].timeLeft = 30;
						// Main.projectile[proj2].melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
						Main.projectile[proj2].hostile = false;
						Main.projectile[proj2].netUpdate = true;
					}
				}

            }
            else
            {
				Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
			}

			if ((int)(ptimer+ percent * 30)%30 == 0)
            {
				List<Projectile> allfriendlyprojs = Main.projectile.Where(testby => testby.active && !testby.coldDamage && testby.damage>0 && testby.owner == Projectile.owner && Projectile.type != testby.type && (testby.Center - Projectile.Center).LengthSquared()<640000).OrderBy(testby => (testby.Center-Projectile.Center).LengthSquared()).ToList();

				if (allfriendlyprojs.Count > 0)
				{
					Projectile coldenproj = allfriendlyprojs[0];
					coldenproj.coldDamage = true;
					if (!coldenproj.hostile)
					{
						coldenproj.damage = (int)(coldenproj.damage*(1f+(player.GetDamage(DamageClass.Summon)-1f)*0.25f));
					}
					coldenproj.netUpdate = true;

					Vector2 generalDist = coldenproj.Center - Projectile.Center;

					for (int i = 0; i < 60; i += 1)
                    {
						int dustx = Dust.NewDust(new Vector2(coldenproj.position.X, coldenproj.position.Y), coldenproj.width, coldenproj.height, 80);
						Main.dust[dustx].scale = 1.25f;
						Main.dust[dustx].velocity = Main.rand.NextVector2Circular(4f,4f)+Vector2.Normalize(generalDist)*1f;
						Main.dust[dustx].noGravity = true;
					}

					SoundEngine.PlaySound(SoundID.Item, (int)coldenproj.Center.X, (int)coldenproj.Center.Y, 30, 1f, -0.5f);
				}


            }


			//				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 30, 1f, -0.5f);


			if ((where - Projectile.Center).Length() > 8f)
			{
				Projectile.velocity += Vector2.Normalize((where - Projectile.Center)) * 0.01f;
				Projectile.velocity += (where - Projectile.Center) * 0.0025f;
				Projectile.velocity *= 0.975f;
			}
			float maxspeed = Math.Min(Projectile.velocity.Length(), 32);
			Projectile.velocity.Normalize();
			Projectile.velocity *= maxspeed;

			int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135);
			Main.dust[dust].scale = 0.50f;
			Main.dust[dust].velocity = Projectile.velocity * 0.2f;
			Main.dust[dust].noGravity = true;

			Lighting.AddLight(Projectile.Center, Color.Aqua.ToVector3() * 0.78f);

		}

		public override string Texture
		{
			get { return ("SGAmod/NPCs/IceFairy"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[Projectile.type];

			int maxFrames = 4;

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / maxFrames) / 2f;
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
			Color color = Color.Lerp((Projectile.GetAlpha(lightColor) * 0.5f), Color.White, 0.75f);
			int timing = (int)(Projectile.localAI[0] / 8f);
			timing %= maxFrames;
			timing *= ((tex.Height) / maxFrames);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height) / maxFrames), color*MathHelper.Clamp(Projectile.localAI[0]/30f,0f,1f), Projectile.velocity.X * 0.04f, drawOrigin, Projectile.scale, Projectile.spriteDirection>0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			return false;
		}

	}
	public class CirnoMinionBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Fairies");
			Description.SetDefault("They serve the new strongest");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/IceFairiesBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<CirnoMinionProjectile>()] > 0)
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


namespace SGAmod.NPCs
{

	public class CirnoBoltPlayer : CirnoBolt
	{


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cirno's Grace");
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.coldDamage = true;
			keepspeed = 0.0;
			homing = 0.05f;
		}

		public override void AI()
		{
			base.AI();
			if (Projectile.ai[0] == 2)
			{
				float rand = Main.rand.Next(0, 360);
				for (int i = 0; i < 360; i += 180)
				{
					int proj2 = Projectile.NewProjectile(Projectile.Center, Projectile.velocity + new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0, 3f)), Mod.Find<ModProjectile>("CirnoIceShardPlayerOrbit").Type, (int)((float)Projectile.damage * 0.5f), 0f, Projectile.owner);
					Main.projectile[proj2].ai[0] = MathHelper.ToRadians(i+ rand);
					Main.projectile[proj2].ai[1] = Projectile.whoAmI;
					// Main.projectile[proj2].Throwing().thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
					Main.projectile[proj2].DamageType = projectile.magic;
					Main.projectile[proj2].netUpdate = true;
				}
			}

		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.IceBolt; }
		}

	}

}
