using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Items.Tools;
using Idglibrary;
using SGAmod.Buffs;
using SGAmod.NPCs.Hellion;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class ElementalCascade : ModItem
	{
		int projectiletype = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elemental Cascade");
			Tooltip.SetDefault("Unleashes 4 elemental beams in cardinal directions towards the mouse cursor, swapping elements with each fire\nThe beams bounce off walls and are non solid until they stop moving, and deal different debuffs to enemies");
			Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 15;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 45;
			Item.useAnimation = 45;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 5;
			Item.value = 10000;
			Item.rare = 6;
			Item.UseSound = SoundID.Item78;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("UnmanedBolt").Type;
			Item.shootSpeed = 4f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("Fridgeflame"), 5).AddIngredient(mod.ItemType("CryostalBar"), 5).AddIngredient(mod.ItemType("VirulentBar"), 5).AddIngredient(mod.ItemType("OmniSoul"), 5).AddIngredient(ItemID.SpellTome, 1).AddTile(TileID.Bookcases).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			projectiletype += 1;
			projectiletype = projectiletype % 4;
			type = Mod.Find<ModProjectile>("ElementalCascadeShot").Type;

			for (int i = 0; i < 4; i += 1)
			{

				Vector2 speez = new Vector2(speedX, speedY);
				speez=speez.RotatedBy(MathHelper.ToRadians(90 *i));
				Vector2 offset = speez;
				offset.Normalize();
				offset *= 48f;
			int probg = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, speez.X, speez.Y, type, damage, knockBack, player.whoAmI, ((i + projectiletype) % 4));
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speez.X, speez.Y).RotatedByRandom(MathHelper.ToRadians(5));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
				Main.projectile[probg].netUpdate = true;

				IdgProjectile.Sync(probg);

			}


			return false;

		}


	}
	public class LunarCascade : ElementalCascade
	{
		int projectiletype = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Cascade");
			Tooltip.SetDefault("Unleashes several beams in a complete circle around the player that travel far and effectively melt enemies\nThe beams bounce off walls and are non solid until they stop moving\nBeams deal different powerful debuffs to enemies");
			Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			Item.damage = 60;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 30;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 5;
			Item.useAnimation = 50;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 5;
			Item.value = 10000;
			Item.rare = 10;
			Item.UseSound = SoundID.Item78;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("UnmanedBolt").Type;
			Item.shootSpeed = 8f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("ElementalCascade"), 1).AddRecipeGroup("Fragment", 6).AddIngredient(mod.ItemType("PrismalBar"), 8).AddIngredient(ItemID.LunarBar, 6).AddTile(TileID.LunarCraftingStation).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			projectiletype += 1;
			projectiletype = projectiletype % 4;
			type = Mod.Find<ModProjectile>("LunarCascadeShot").Type;

			Vector2 speez = new Vector2(speedX, speedY);
			speez = speez.RotatedBy(MathHelper.ToRadians((float)player.itemAnimation * (360f / player.itemAnimationMax)));
			Vector2 offset = speez;
			offset.Normalize();
			offset *= 48f;
			int probg = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, speez.X, speez.Y, type, damage, knockBack, player.whoAmI, (projectiletype));
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speez.X, speez.Y).RotatedByRandom(MathHelper.ToRadians(5));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			Main.projectile[probg].netUpdate = true;

			IdgProjectile.Sync(probg);

			return false;

		}

	}

	public class HellionCascade : LunarCascade, IHellionDrop
	{
		int IHellionDrop.HellionDropAmmount() => 1;
		int IHellionDrop.HellionDropType() => ModContent.ItemType<HellionCascade>();
		int projectiletype = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellion's Cascade");
			Tooltip.SetDefault("Unleashes several beams in a complete spiral around the player that travel far, absolutely melting enemies\nThe beams pass through walls and are non solid until they stop moving\nBeams deal different very powerful debuffs to enemies");
			Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			Item.damage = 500;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 100;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 90;
			Item.useAnimation = 90;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 5;
			Item.value = 10000;
			Item.rare = 11;
			Item.UseSound = SoundID.Item84;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("HellionCascadeShotPlayer").Type;
			Item.shootSpeed = 9f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("LunarCascade"), 1).AddRecipeGroup("Fragment", 10).AddIngredient(mod.ItemType("ByteSoul"), 100).AddIngredient(mod.ItemType("DrakeniteBar"), 10).AddTile(TileID.LunarCraftingStation).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			projectiletype += 1;
			projectiletype = projectiletype % 4;

			for (int a = 0; a < 360; a += 360 / 4)
			{
				for (int i = -4; i < 5; i += 8)
				{
					Vector2 speez = new Vector2(speedX, speedY);
					speez = speez.RotatedBy(MathHelper.ToRadians(a+(i>0 ? 45 : 0)));
					Vector2 offset = speez;
					offset.Normalize();
					offset *= 48f;
					int probg = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, speez.X, speez.Y, type, damage, knockBack, player.whoAmI, (projectiletype),(float)i/1.5f);
					Main.projectile[probg].friendly = true;
					Main.projectile[probg].hostile = false;
					Main.projectile[probg].netUpdate = true;
					IdgProjectile.Sync(probg);

				}
			}

			return false;

		}

	}

	public class LunarCascadeShot : ElementalCascadeShot
	{
		public override int stopmoving => 240;
		public override int fadeinouttime => 30;

		//public Color[] colors = { Color.Orange, Color.Purple, Color.LimeGreen, Color.Yellow };
		//public int[] buffs = { ModContent.BuffType<ThermalBlaze>(), BuffID.ShadowFlame, ModContent.BuffType<AcidBurn>(), BuffID.Ichor };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Cascade");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.light = 0.25f;
			Projectile.width = 24;
			Projectile.timeLeft = 400;
			Projectile.height = 24;
			Projectile.extraUpdates = 1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			//buffs = new int[4] { BuffID.Daybreak, mod.BuffType("EverlastingSuffering"), mod.BuffType("AcidBurn"), mod.BuffType("MoonLightCurse") };
			colors = new Color[4] { Color.Orange, Color.Purple, Color.LimeGreen, Color.Yellow };
			buffs = new int[4] { Mod.Find<ModBuff>("ThermalBlaze").Type, BuffID.ShadowFlame, BuffID.CursedInferno, BuffID.Ichor};
		}

	}

	public class HellionCascadeShotPlayer : ElementalCascadeShot
	{
		public override int stopmoving => 540;
		public override int fadeinouttime => 30;
		public Vector2 whereat;

		//public Color[] colors = { Color.Orange, Color.Purple, Color.LimeGreen, Color.Yellow };
		//public int[] buffs = { ModContent.BuffType<ThermalBlaze>(), BuffID.ShadowFlame, ModContent.BuffType<AcidBurn>(), BuffID.Ichor };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellion Cascade");
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.light = 0.25f;
			Projectile.width = 24;
			Projectile.timeLeft = 1000;
			Projectile.height = 24;
			Projectile.extraUpdates = 3;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
			buffs = new int[4] { BuffID.Daybreak, Mod.Find<ModBuff>("EverlastingSuffering").Type, Mod.Find<ModBuff>("AcidBurn").Type, Mod.Find<ModBuff>("MoonLightCurse") .Type};
		}

		public override void AI()
		{
			if (Projectile.velocity.Length() > 0)
			{
				if (whereat == null)
				{
					whereat = Main.player[Projectile.owner].Center;
				}
				Projectile.ai[1] *= 0.990f;
				float ogspeed = Projectile.velocity.Length();
				Projectile.velocity=Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[1]), whereat);
				Projectile.velocity.Normalize();
				Projectile.velocity *= ogspeed;
			}
			base.AI();
		}

	}

	public class ElementalCascadeShot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elemental Cascade");
		}

		private List<Vector2> oldPos = new List<Vector2>();
		public Color[] colors = { Color.Orange, Color.Purple, Color.LawnGreen, Color.Aqua };
		public int[] buffs = {BuffID.OnFire,BuffID.ShadowFlame,BuffID.DryadsWardDebuff,BuffID.Frostburn};

		public virtual int stopmoving => 90;
		public virtual int fadeinouttime => 30;
		public virtual int bufftime => 60 * 8;

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1000;
			Projectile.light = 0.25f;
			Projectile.width = 24;
			Projectile.timeLeft = 60 * 3;
			Projectile.height = 24;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			foreach (Vector2 position in oldPos)
			{
				projHitbox.X = (int)position.X;
				projHitbox.Y = (int)position.Y;
				if (projHitbox.Intersects(targetHitbox))
				{
					return true;
				}
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(buffs[(int)Projectile.ai[0]],bufftime);
			/*if (this.GetType() == typeof(LunarCascadeShot))
			{
				target.immune[projectile.owner] -= 5;
			}*/
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(buffs[(int)Projectile.ai[0]],bufftime);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = SGAmod.ExtraTextures[96];
			float fadin = MathHelper.Clamp(1f-((float)Projectile.timeLeft-stopmoving) / fadeinouttime, 0.1f,0.75f);
			if (Projectile.timeLeft<(int)fadeinouttime)
				fadin = ((float)Projectile.timeLeft/ fadeinouttime) *0.75f;
			for (int i = 0; i < oldPos.Count; i += 1)
			{
				Color thecolor = colors[(int)Projectile.ai[0]];
			if (GetType()==typeof(HellionCascadeShot) || GetType() == typeof(HellionCascadeShot2) || GetType() == typeof(HellionCascadeShotPlayer))
				thecolor = Main.hslToRgb((((i+ Projectile.ai[0]*26f)/80f) + (-Main.GlobalTimeWrappedHourly / 0.6f))% 1f, 0.85f,0.7f);
				Vector2 drawPos = oldPos[i] - Main.screenPosition;
				spriteBatch.Draw(texture, drawPos, null, Color.Lerp(lightColor, thecolor, 0.75f)* fadin, 1, new Vector2(texture.Width / 2f, texture.Height / 2f), new Vector2(0.4f, 0.4f), SpriteEffects.None, 0f);
			}
			return false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.velocity.Length() > 0f || Projectile.timeLeft < fadeinouttime)
				return false;
			return base.CanHitNPC(target);
		}

		public override bool CanHitPlayer(Player target)
		{
			if (Projectile.velocity.Length()>0f || Projectile.timeLeft < fadeinouttime)
			return false;
			return base.CanHitPlayer(target);
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + 5; }
		}


		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			{
				SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}

		public override void AI()
		{
			if (Projectile.timeLeft < stopmoving+ (fadeinouttime/2))
			{
				Projectile.velocity = default(Vector2);
			}
			else
			{
				oldPos.Add(Projectile.Center);
			}



		}
	}


}
