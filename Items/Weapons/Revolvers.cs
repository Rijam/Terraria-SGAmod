using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Items.Weapons;

using System.IO;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using SGAmod.HavocGear.Items.Weapons;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class RevolverBase : ModItem
	{
		public bool forcedreload = false;
		public virtual int RevolverID => 0;
		public override bool Autoload(ref string name)
		{
			return GetType() != typeof(RevolverBase);
		}

		public override bool CanUseItem(Player player)
		{
			forcedreload = false;
			Item.autoReuse = false;

			if (player.GetModPlayer<SGAPlayer>().ReloadingRevolver > 0 || forcedreload)
				return false;

			if (!player.SGAPly().ConsumeAmmoClip(false)) { Item.UseSound = SoundID.Item98; forcedreload = true; Item.useTime = 4; Item.useAnimation = 4; Item.noUseGraphic = true; }
			return true;
		}

		public override void HoldItem(Player player)
		{
			if (player.itemAnimation < 1)
			{
				SGAPlayer sgaply = player.SGAPly();
				int val = 6;
				SGAmod.UsesClips.TryGetValue(player.HeldItem.type, out val);
				if (sgaply.ammoLeftInClipMax != val + sgaply.ammoLeftInClipMaxAddedAmmo)
				{
					sgaply.ammoLeftInClip = 0;
				}

				sgaply.ammoLeftInClipMaxLastHeld = sgaply.ammoLeftInClipMax;
			}
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "RevolverHomingPenalty", Idglib.ColorText(Color.Red, "Damage from Homing ammo is reduced by 75%")));
		}
		public bool HomingAmmo(int type)
        {
			return ProjectileID.Sets.Homing[type];
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			damage = HomingAmmo(type) ? (int)(damage * 0.25f) : damage;
			return false;
        }
    }
	public class DragonRevolver : RevolverBase,IDevItem
	{
		bool altfired = false;
		public override int RevolverID => Mod.Find<ModProjectile>("DragonRevolverReloading").Type;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serpent's Redemption");
			Tooltip.SetDefault("Hold Left Click and hover your mouse over targets to mark them for execution: releasing a dragon-fire burst on them!\nYou may mark targets as long as you have ammo in the clip and nothing is blocking your way\nUp to 6 targets may be marked for execution; a target that resists however can be marked more than once\nThe explosion is unable to crit but hits several times\nAlt Fire shoots 3 accurate rounds at once if the bullet does not pierce more than 3 times, otherwise 1\nThe extra bullets do only 50% base damage\n'Thy time has come'ith for dragon slayers, repent!'");
			SGAmod.UsesClips.Add(SGAmod.Instance.Find<ModItem>("DragonRevolver").Type, 6);
		}
		public (string, string) DevName()
		{
			return ("IDGCaptainRussia94", "");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			if (Main.LocalPlayer.GetModPlayer<SGAPlayer>().devempowerment[0] > 0)
			{
				tooltips.Add(new TooltipLine(Mod, "DevEmpowerment", "--- Enpowerment bonus ---"));
				tooltips.Add(new TooltipLine(Mod, "DevEmpowerment", "Primary Explosion is larger"));
				tooltips.Add(new TooltipLine(Mod, "DevEmpowerment", "Secondary fires faster"));
			}
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Revolver);
			Item.damage = 2000;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.knockBack = 10;
			Item.value = Item.sellPrice(2, 0, 0, 0);
			Item.rare = 12;
			Item.shootSpeed = 8f;
			Item.noMelee = true;
			Item.useAmmo = AmmoID.Bullet;
			Item.autoReuse = false;
			Item.shoot = 10;
			Item.shootSpeed = 50f;
			Item.noUseGraphic = false;
			Item.UseSound = Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Wide_Beam_Shot");
			Item.useStyle = 5;
			Item.expert = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{

			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (sgaplayer.ReloadingRevolver > 0)
				return false;

				if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("RevolverTarget").Type] > 0)
				return false;

			altfired = player.altFunctionUse == 2 ? false : true;
			forcedreload = false;
			Item.noUseGraphic = false;

			if (altfired && sgaplayer.ConsumeAmmoClip(false))
			{
				Item.useAnimation = 5;
				Item.useTime = 5;
				Item.useStyle = 5;
				Item.UseSound = SoundID.Item35;
				Item.channel = true;
				Item.shoot = Mod.Find<ModProjectile>("DragonRevolverAiming").Type;
			} else {
				Item.useStyle = 5;
				int firerate = sgaplayer.devempowerment[0] > 0 ? 45 : 60;
				Item.useTime = firerate;
				Item.useAnimation = firerate;
				Item.UseSound = SoundID.Item38;
				Item.channel = false;
				Item.shoot = 10;
				if (!sgaplayer.ConsumeAmmoClip(false)) { Item.UseSound = SoundID.Item98; forcedreload = true; Item.useTime = 4; Item.useAnimation = 4; Item.noUseGraphic = true; }
			}
			return true;
		}

		public override bool ConsumeAmmo(Player player)
		{
			return (Item.shoot != Mod.Find<ModProjectile>("DragonRevolverAiming").Type);
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, 2);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			//base.Shoot(player,ref position,ref speedX,ref speedY,ref type,ref damage,ref knockBack);
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;

			if (!altfired && sgaplayer.ConsumeAmmoClip(false))
			{
				sgaplayer.ConsumeAmmoClip(true);
				Projectile proj = new Projectile();
				proj.SetDefaults(type);

				if (proj.penetrate < 4 && proj.penetrate > -1)
				{

					for (int i = 0; i < 2; i += 1)
					{
						int thisoned = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)(damage / 2), knockBack, Main.myPlayer);
					}
				}
			}

			if (!sgaplayer.ConsumeAmmoClip(false) || forcedreload)
			{
				player.itemTime = 50;
				player.itemAnimation = 50;
				if (forcedreload) {
					player.itemTime = 1;
					player.itemAnimation = 1;
				}
				int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, RevolverID, 0, knockBack, Main.myPlayer, 0.0f, 0f);
				// Main.projectile[thisone].spriteDirection=normalizedspeed.X>0f ? 1 : -1;
				//Main.projectile[thisone].rotation=(new Vector2(speedX,speedY)).ToRotation();

				return !forcedreload;
			}

			if (altfired) {
				int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("DragonRevolverAiming").Type, 1, 0f, Main.myPlayer, 0.0f, 0f);
				return false;
			}

			//if (sgaplayer.ammoLeftInClip > 0)
			//{
			//}
			return (sgaplayer.ConsumeAmmoClip(false));
		}




		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("TheJacob"), 1).AddIngredient(mod.ItemType("LunarRoyalGel"), 25).AddIngredient(mod.ItemType("StarMetalBar"), 20).AddIngredient(mod.ItemType("PrismalBar"), 8).AddIngredient(mod.ItemType("CosmicFragment"), 1).AddTile(TileID.LunarCraftingStation).Register();
		}

	}

	public class DragonRevolverAiming : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/DragonRevolver"); }
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
			Projectile.timeLeft = 180;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.damage = 1;
			AIType = 0;
		}

		public override bool? CanCutTiles() { return false; }

		public override bool? CanHitNPC(NPC target)
		{
			Player player = Main.player[Projectile.owner];
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			int ownedproj = player.ownedProjectileCounts[Mod.Find<ModProjectile>("RevolverTarget").Type];
			if (!target.HasBuff(Mod.Find<ModBuff>("Targeted").Type) && !target.friendly && sgaplayer.ConsumeAmmoClip(false) && ownedproj < 6 && Projectile.ai[0] < 1 && (Collision.CanHitLine(new Vector2(target.Center.X, target.Center.Y), 1, 1, new Vector2(player.Center.X, player.Center.Y), 1, 1))) {
				return true;
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			int ownedproj = player.ownedProjectileCounts[Mod.Find<ModProjectile>("RevolverTarget").Type];

			IdgNPC.AddBuffBypass(target.whoAmI, Mod.Find<ModBuff>("Targeted").Type, 3, false);
			int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("RevolverTarget").Type, 0, 0f, Projectile.owner, 0.0f, 0f);
			Main.projectile[thisone].ai[0] = target.whoAmI;
			Main.projectile[thisone].netUpdate = true;
			sgaplayer.ConsumeAmmoClip();
			//Main.PlaySound(mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/Wide_Beam_Shot"),(int)Main.player[projectile.owner].position.X,(int)Main.player[projectile.owner].position.Y,1,1.15f,((float)ownedproj)/4f);
			//Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Wide_Beam_Shot").WithVolume(1.1f).WithPitchVariance(.25f));
			SoundEngine.PlaySound(SoundLoader.customSoundType, (int)Main.player[Projectile.owner].position.X, (int)Main.player[Projectile.owner].position.Y, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/Wide_Beam_Shot"), 1.15f, ((float)-0.4 + (ownedproj) / 6f));
		}

		public override void AI()
		{
			Vector2 mousePos = Main.MouseWorld;
			Player player = Main.player[Projectile.owner];

			if (Projectile.ai[0] > 1000f || player.dead)
			{
				Projectile.Kill();
			}
			if (!player.channel || Projectile.ai[0] > 0) {
				Projectile.ai[0] += 1;
				if (Projectile.ai[0] < 5f)
					Projectile.ai[0] = 79f;
			}
			// Multiplayer support here, only run this code if the client running it is the owner of the projectile
			if (Projectile.owner == Main.myPlayer)
			{
				Vector2 diff = mousePos - player.Center;
				diff.Normalize();
				Projectile.velocity = diff;
				if (Projectile.ai[0] < 50f)
					Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
				Projectile.netUpdate = true;
				Projectile.Center = mousePos;
			}
			int dir = Projectile.direction;
			player.ChangeDir(dir);
			player.itemTime = 5;
			player.itemAnimation = 5;
			if (Projectile.ai[0] < 50f)
				player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir);



			for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(Projectile.position, 16, 16, 20);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
				Main.dust[dust].noGravity = true;
			}

			Projectile.timeLeft = 2;


			if (Projectile.ai[0] > 65) { Projectile.ai[0] = 58;
				int ownedproj = player.ownedProjectileCounts[Mod.Find<ModProjectile>("RevolverTarget").Type];
				Projectile thetarget;
				thetarget = null;
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile him = Main.projectile[i];
					if (him.type == Mod.Find<ModProjectile>("RevolverTarget").Type) {
						if (him.active && him.owner == Projectile.owner) {
							thetarget = him;
							break;
						} } }
				if (thetarget != null) {
					Vector2 angle = thetarget.Center - Main.player[Projectile.owner].Center;
					Projectile.direction = angle.X > player.position.X ? 1 : -1;
					player.itemRotation = (float)Math.Atan2(angle.Y * dir, angle.X * dir);
					angle.Normalize();
					int proj = Projectile.NewProjectile(thetarget.Center.X, thetarget.Center.Y, 0f, 0f, Mod.Find<ModProjectile>("SlimeBlast").Type, (int)(player.GetModPlayer<SGAPlayer>().devempowerment[0] > 0 ? 4000 : 4000 * (player.GetDamage(DamageClass.Ranged))), 15f, Projectile.owner, 0f, 0f);
					Main.projectile[proj].direction = Projectile.direction;
					Main.projectile[proj].DamageType = DamageClass.Ranged;
					if (player.GetModPlayer<SGAPlayer>().devempowerment[0] > 0)
					{
						Main.projectile[proj].width += 128;
						Main.projectile[proj].height += 128;
						Main.projectile[proj].Center -= new Vector2(64, 64);


					}
					Main.projectile[proj].netUpdate = true;
					SoundEngine.PlaySound(SoundID.Item45, thetarget.Center);
					SoundEngine.PlaySound(SoundID.Item41, player.Center);
					thetarget.Kill();
				} else {
					SoundEngine.PlaySound(SoundID.Item63, player.Center);
					player.itemTime = 80;
					player.itemAnimation = 80;
					Projectile.Kill();
				}
			}



		}


		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return true;
		}

	}


	public class RevolverTarget : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/FieryMoon"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 100;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			AIType = 0;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			NPC target = Main.npc[(int)Projectile.ai[0]];

			if (!target.active || player.dead)
			{
				Projectile.Kill();
			}
			Projectile.Center = target.Center;
			IdgNPC.AddBuffBypass(target.whoAmI, Mod.Find<ModBuff>("Targeted").Type, 3, false);
			Projectile.timeLeft = 3;

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			Color glowingcolors1 = Color.Red;//Main.hslToRgb((float)lightColor.R*0.08f,(float)lightColor.G*0.08f,(float)lightColor.B*0.08f);
			spriteBatch.Draw(Main.blackTileTexture, drawPos, new Rectangle(0, 0, 120, 10), glowingcolors1, Projectile.rotation, new Vector2(60, 5), new Vector2(1, 1), SpriteEffects.None, 0f);
			spriteBatch.Draw(Main.blackTileTexture, drawPos, new Rectangle(0, 0, 10, 120), glowingcolors1, Projectile.rotation, new Vector2(5, 60), new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}

	}

	public class TheJacob : RevolverBase
	{
		bool altfired = false;
		public override int RevolverID => Mod.Find<ModProjectile>("TheJacobReloading").Type;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Jakob");
			Tooltip.SetDefault("Right click to fan the hammer-rapidly fire the remaining clip with less accuracy\nIf it took more than 1 shot, you weren't using a Jakob's!'");
			SGAmod.UsesClips.Add(SGAmod.Instance.Find<ModItem>("TheJacob").Type, 6);
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Revolver);
			Item.damage = 200;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.knockBack = 10;
			Item.value = 10000;
			Item.rare = 5;
			Item.crit = 15;
			Item.shootSpeed = 8f;
			Item.noMelee = true;
			Item.useAmmo = AmmoID.Bullet;
			Item.autoReuse = false;
			Item.shoot = 10;
			Item.shootSpeed = 50f;
			Item.noUseGraphic = false;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{

			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (sgaplayer.ReloadingRevolver > 0)
				return false;


			altfired = player.altFunctionUse == 2 ? true : false;
			forcedreload = false;
			Item.noUseGraphic = false;

			if (altfired && sgaplayer.ConsumeAmmoClip(false))
			{
				Item.useAnimation = 2000;
				Item.useTime = 10;
				Item.UseSound = SoundID.Item38;
			}
			else
			{
				Item.useTime = 40;
				Item.useAnimation = 40;
				Item.UseSound = SoundID.Item38;
				if (!sgaplayer.ConsumeAmmoClip(false)) { Item.UseSound = SoundID.Item98; forcedreload = true; Item.useTime = 4; Item.useAnimation = 4; Item.noUseGraphic = true; }
			}
			return true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, 2);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.ammoLeftInClip -= 1;
			if (Item.useAnimation > 1000)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
				speedX = perturbedSpeed.X;
				speedY = perturbedSpeed.Y;
				SoundEngine.PlaySound(SoundID.Item38, player.Center);
			}
			if (!sgaplayer.ConsumeAmmoClip(false) || forcedreload)
			{
				player.itemTime = 40;
				player.itemAnimation = 40;
				int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, RevolverID, 0, knockBack, Main.myPlayer, 0.0f, 0f);
				// Main.projectile[thisone].spriteDirection=normalizedspeed.X>0f ? 1 : -1;
				//Main.projectile[thisone].rotation=(new Vector2(speedX,speedY)).ToRotation();
				return !forcedreload;
			}
			return (sgaplayer.ConsumeAmmoClip(false));
		}




		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("RevolverUpgrade"), 1).AddIngredient(ItemID.HallowedBar, 8).AddIngredient(ItemID.SoulofFright, 20).AddTile(TileID.MythrilAnvil).Register();
			CreateRecipe(1).AddIngredient(mod.ItemType("GuerrillaPistol"), 1).AddIngredient(ItemID.HallowedBar, 10).AddIngredient(ItemID.SoulofFright, 20).AddTile(TileID.MythrilAnvil).Register();
		}

	}

	public class RevolverUpgrade : RevolverBase
	{
		bool altfired = false;
		public override int RevolverID => Mod.Find<ModProjectile>("TheRevolverReloading").Type;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Revolving West");
			Tooltip.SetDefault("Right click to fire an extra bullet at the closest enemy\nBut this halves the damage of both bullets");
			SGAmod.UsesClips.Add(SGAmod.Instance.Find<ModItem>(GetType().Name).Type, 6);
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Revolver);
			Item.damage = 42;
			Item.width = 48;
			Item.height = 24;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.knockBack = 10;
			Item.value = 50000;
			Item.rare = 3;
			Item.noMelee = true;
			Item.useAmmo = AmmoID.Bullet;
			Item.autoReuse = false;
			Item.shoot = 10;
			Item.shootSpeed = 40f;
			Item.noUseGraphic = false;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.GetModPlayer<SGAPlayer>().ReloadingRevolver > 0)
				return false;
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			altfired = player.altFunctionUse == 2 ? true : false;
			forcedreload = false;
			Item.noUseGraphic = false;

			if (altfired)
			{
				Item.useAnimation = 16;
				Item.useTime = 16;
				Item.UseSound = SoundID.Item38;
			}
			else
			{
				Item.useTime = 12;
				Item.useAnimation = 12;
				Item.UseSound = SoundID.Item38;
			}
			if (!sgaplayer.ConsumeAmmoClip(false)) { Item.UseSound = SoundID.Item98; forcedreload = true; Item.useTime = 4; Item.useAnimation = 4; Item.noUseGraphic = true; }
			return true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, 2);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.ConsumeAmmoClip();
			if (player.altFunctionUse == 2)
			{
				damage = (int)(damage * 0.5f);
				int target2 = Idglib.FindClosestTarget(0, position, new Vector2(0, 0));
				NPC them = Main.npc[target2];
				Vector2 where = them.Center - position;
				where.Normalize();
				Vector2 perturbedSpeed = new Vector2(where.X, where.Y) * (new Vector2(speedX, speedY).Length() * 1.25f);


				if (them.active && (them.Center - player.Center).Length() > 800)
				{
					perturbedSpeed = new Vector2(speedX, speedY) * 1.25f;
				}

				SoundEngine.PlaySound(SoundID.Item38, player.Center);
				int thisoned = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, Main.myPlayer);
			}
			if (!sgaplayer.ConsumeAmmoClip(false) || forcedreload)
			{
				player.itemTime = 40;
				player.itemAnimation = 40;
				int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, RevolverID, 0, knockBack, Main.myPlayer, 0.0f, 0f);
				return !forcedreload;
			}
			return (sgaplayer.ConsumeAmmoClip(false));
		}




		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Revolver, 1).AddRecipeGroup("SGAmod:Tier5Bars", 8).AddTile(TileID.Anvils).Register();
		}

	}
	/*
	public class TheRevolverReloading : ClipWeaponReloading
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.Revolver); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 30;
			projectile.height = 24;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 180;
			projectile.penetrate = 10;
			projectile.scale = 0.7f;
			AIType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = 8;
			drawHeldProjInFrontOfHeldItemAndArms = true;
		}

	}

	public class TheJacobReloading : ClipWeaponReloading
	{

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
			projectile.width = 30;
			projectile.height = 24;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 180;
			projectile.scale = 0.7f;
			AIType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = 8;
			drawHeldProjInFrontOfHeldItemAndArms = true;
		}

	}

	public class DragonRevolverReloading : ClipWeaponReloading
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/DragonRevolver"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 30;
			projectile.height = 24;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 180;
			projectile.penetrate = 10;
			projectile.scale = 0.7f;
			AIType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = 8;
			drawHeldProjInFrontOfHeldItemAndArms = true;
		}

	}
	*/

		public class ClipWeaponReloading : ModProjectile
	{
		private string tex;
		private int timeLeft;
		public int ammoMax;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public ClipWeaponReloading(string tex,int timeLeft=100,int ammoMax = 6)
        {
			this.tex = tex;
			this.timeLeft = timeLeft;
			this.ammoMax = ammoMax;
		}
        public override bool CanDamage()
        {
            return false;
        }
        public override bool CloneNewInstances => true;

		public static void SetupRevolverHoldingTypes()
        {
			SGAmod.Instance.AddProjectile("TheRevolverReloading", new ClipWeaponReloading("SGAmod/Items/Weapons/RevolverUpgrade",ammoMax: 6));
			SGAmod.Instance.AddProjectile("TheJacobReloading", new ClipWeaponReloading("SGAmod/Items/Weapons/TheJacob",150, ammoMax: 6));
			SGAmod.Instance.AddProjectile("DragonRevolverReloading", new ClipWeaponReloading("SGAmod/Items/Weapons/DragonRevolver",200, ammoMax: 6));
			SGAmod.Instance.AddProjectile("GuerrillaPistolReloading", new ClipWeaponReloading("SGAmod/HavocGear/Items/Weapons/GuerrillaPistol", ammoMax: 6));
			SGAmod.Instance.AddProjectile("GunarangReloading", new ClipWeaponReloading("SGAmod/Items/Weapons/Gunarang", ammoMax: 6));

			SGAmod.Instance.AddProjectile("RustyRifleReloading", new ClipWeaponReloading("SGAmod/HavocGear/Items/Weapons/RustyRifle", 150, ammoMax: 4));

		}

		public override bool Autoload(ref string name)
		{
			return false;
		}

		public override string Texture
		{
			get { return (tex); }
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.timeLeft = timeLeft;
			Projectile.penetrate = 10;
			AIType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = 8;
			drawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[Projectile.type];
			Vector2 offset = new Vector2(Projectile.spriteDirection<0 ? tex.Width-8 : 8, tex.Height / 2);
			if (Main.player[Projectile.owner].itemAnimation < 1)
			spriteBatch.Draw(tex, Projectile.Center-Main.screenPosition, null, lightColor*Math.Min(Projectile.timeLeft/20f,1f), Projectile.rotation, offset,Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
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

			if (owner.itemAnimation > 0)
			{
				Projectile.timeLeft += 1;
				if (owner.itemAnimation == 1)
					Projectile.timeLeft = (int)((float)Projectile.timeLeft / owner.GetModPlayer<SGAPlayer>().RevolverSpeed);
			}
			else
			{
				owner.GetModPlayer<SGAPlayer>().ReloadingRevolver = 3;
				Projectile.spriteDirection = (owner.direction > 0).ToDirectionInt();
				owner.heldProj = Projectile.whoAmI;
				Projectile.ai[0] += 1;
				Projectile.velocity = new Vector2(0f, 0f);
				//projectile.rotation = projectile.rotation.AngleLerp((float)(Math.PI/-(4.0*(double)projectile.spriteDirection)),0.15f);
				owner.bodyFrame.Y = owner.bodyFrame.Height * 3;

				if (Projectile.timeLeft == 18)
				{
					SGAPlayer sgaplayer = owner.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
					sgaplayer.ammoLeftInClip = ammoMax+sgaplayer.ammoLeftInClipMaxStack;
					sgaplayer.ammoLeftInClipMax = sgaplayer.ammoLeftInClip;
					sgaplayer.ammoLeftInClipMaxAddedAmmo = sgaplayer.ammoLeftInClipMaxStack;
					SoundEngine.PlaySound(SoundID.Item65, owner.Center);
				}

				/*if (owner.velocity.X<0)
				owner.direction=-1;
				projectile.spriteDirection=owner.direction;*/
			}

			//projectile.velocity=new Vector2(projectile.velocity.X,0f);
			Projectile.Center = owner.Center + new Vector2(Projectile.spriteDirection * 6, -4f);

		}


	}

		public class Gunarang : RevolverBase
	{
		public override int RevolverID => Mod.Find<ModProjectile>("GunarangReloading").Type;
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Throws the gun, bounces off walls once, shoots at enemies it hits\n'When the gun just gets a little loose...'");
			DisplayName.SetDefault("Gunarang");
			SGAmod.UsesClips.Add(Item.type, 6);
		}

		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 10;
			Item.damage = 60;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.useTurn = true;
			Item.noUseGraphic = true;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.useStyle = 1;
			Item.knockBack = 1f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.maxStack = 1;
			Item.value = Item.buyPrice(gold: 5);
			Item.rare = ItemRarityID.LightPurple;
			Item.shoot = ModContent.ProjectileType<SpecterangProj>();
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Bullet;
		}
        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }
        public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<GunarangProj>()] > 0)
				return false;

			Item.UseSound = SoundID.Item1;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.noUseGraphic = true;

			return base.CanUseItem(player);
		}
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			add -= player.GetDamage(DamageClass.Ranged);
			add += (player.GetDamage(DamageClass.Melee) + player.GetDamage(DamageClass.Ranged)) / 2f;
			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
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
				valuez.RemoveAt(1);
				valuez.Insert(1, "Melee/Ranged ");
				foreach (string text3 in valuez)
				{
					newline += text3;
				}
				tt.Text = newline;
			}

			tt = tooltips.FirstOrDefault(x => x.Name == "CritChance" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] thetext = tt.Text.Split(' ');
				string newline = "";
				List<string> valuez = new List<string>();
				int counter = 0;
				foreach (string text2 in thetext)
				{
					counter += 1;
					if (counter > 1)
						valuez.Add(text2 + " ");
				}
				int thecrit = Main.GlobalTimeWrappedHourly % 3f >= 1.5f ? Main.LocalPlayer.GetCritChance(DamageClass.Melee) : Main.LocalPlayer.GetCritChance(DamageClass.Ranged);
				string thecrittype = Main.GlobalTimeWrappedHourly % 3f >= 1.5f ? "Melee " : "Ranged ";
				valuez.Insert(0, thecrit + "% " + thecrittype);
				foreach (string text3 in valuez)
				{
					newline += text3;
				}
				tt.Text = newline;
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			SGAPlayer sgaplayer = player.SGAPly();

			if (!sgaplayer.ConsumeAmmoClip(false) || forcedreload)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, RevolverID, 0, knockBack, Main.myPlayer, 0.0f, 0f);
				return !forcedreload;
			}
			if (sgaplayer.ConsumeAmmoClip(false))
			{
				int thisone = Projectile.NewProjectile(position.X, position.Y, speedX / player.meleeSpeed, speedY / player.meleeSpeed, ModContent.ProjectileType<GunarangProj>(), damage, knockBack, Main.myPlayer);
				(Main.projectile[thisone].ModProjectile as GunarangProj).ammoType = type;
				Main.projectile[thisone].netUpdate = true;
			}
			return false;
		}

	}

	public class GunarangProj : SpecterangProj
	{
		protected override int ReturnTime => 30;
		protected override int ReturnTimeNoSlow => 70;
		protected override float SolidAmmount => 6f;
		public int ammoType = -1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gunarang");
		}

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			ammoType = reader.ReadInt32();
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(ammoType);
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Gunarang"); }
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
			Projectile.extraUpdates = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.tileCollide = true;
		}

		public override void AI()
        {
            base.AI();

			Player owner = Main.player[Projectile.owner];

			if (owner != null && owner.active)
			{
				if (Projectile.ai[1] > 0)
				{
					NPC target = Main.npc[(int)Projectile.ai[1] - 1];
					if (target != null && target.active)
					{
						if (Projectile.ai[0] % 15 == 0)
						{

							int ammotype = (int)owner.GetModPlayer<SGAPlayer>().myammo;
							if (ammotype > 0 && owner.HasItem(ammotype))
							{
								Item ammo2 = new Item();
								ammo2.SetDefaults(ammotype);
								int ammo = ammo2.shoot;
								int damageproj = Projectile.damage;
								float knockbackproj = Projectile.knockBack;
								float sppez = 16f;
								if (ammo2.ModItem != null)
									ammo2.ModItem.PickAmmo(owner.HeldItem, owner, ref ammo, ref sppez, ref Projectile.damage, ref Projectile.knockBack);
								int type = ammo;

								if (owner.SGAPly().ConsumeAmmoClip())
								{
									owner.ConsumeItemRespectInfiniteAmmoTypes(ammo2.type);
									Projectile.NewProjectile(Projectile.Center, Vector2.Normalize(target.Center - Projectile.Center) * 16f, type, Projectile.damage, Projectile.knockBack,Projectile.owner);

									SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 41);
									if (sound != null)
										sound.Pitch += 0.50f;
								}
							}

						}
                    }
                    else
                    {
						if (Projectile.ai[1] > 0)
						Projectile.ai[1] = 0;
                    }

				}
			}
		}

        public override bool CanDamage()
		{
			return true;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Projectile.ai[1] = target.whoAmI+1;

			Vector2 angledif = Vector2.Normalize(target.Center - Projectile.Center);

			float leftOrRight = 1;

			if (angledif.ToRotation() - Projectile.velocity.ToRotation() > 0)
				leftOrRight = -1;

			Projectile.velocity = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-MathHelper.Pi / 10f, MathHelper.Pi / 10f) +leftOrRight*MathHelper.PiOver2) *1f;
			Projectile.netUpdate = true;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
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
			Projectile.tileCollide = false;
			Projectile.netUpdate = true;
			return false;
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[Projectile.type];
			Texture2D tex2 = ModContent.Request<Texture2D>("SGAmod/Items/GlowMasks/Gunarang_Glow");
			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, tex.Size() / 2f, new Vector2(1f, 1f) * Projectile.scale, default, 0);
			spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, Color.White * 1f, Projectile.rotation, tex.Size() / 2f, new Vector2(1f, 1f) * Projectile.scale, default, 0);

			return false;
		}

	}

}
namespace SGAmod.HavocGear.Items.Weapons
{

	public class RustyRifle : RevolverBase
	{
		public override int RevolverID => Mod.Find<ModProjectile>("RustyRifleReloading").Type;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rusty Rifle");
			Tooltip.SetDefault("Consumes all bullets in the clip to increase damage");
			SGAmod.UsesClips.Add(Item.type, 4);
		}

		public override void SetDefaults()
		{
			Item.damage = 8;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 20;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 7;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item40;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override bool CanUseItem(Player player)
		{
			Item.noUseGraphic = false;
			Item.UseSound = SoundID.Item11;
			Item.useTime = 30;
			Item.useAnimation = 30;
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			SGAPlayer sgaplayer = player.SGAPly();
			int clipsize = sgaplayer.ammoLeftInClip;
			sgaplayer.ConsumeAmmoClip(true, clipsize);
			damage *= clipsize;

			if (!sgaplayer.ConsumeAmmoClip(false) || forcedreload)
			{
				int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, RevolverID, 0, knockBack, Main.myPlayer, 0.0f, 0f);
				return !forcedreload;
			}
			return (sgaplayer.ConsumeAmmoClip(false));
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
	}

	public class GuerrillaPistol : RevolverBase
	{
		public override int RevolverID => Mod.Find<ModProjectile>("GuerrillaPistolReloading").Type;
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoots a powerful, high velocity bullet");
			DisplayName.SetDefault("Guerrilla Pistol");
			SGAmod.UsesClips.Add(Item.type, 6);
		}

		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 40;
			Item.height = 20;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = 10000;
			Item.rare = 1;
			Item.UseSound = SoundID.Item11;
			Item.autoReuse = true;
			Item.shoot = 10;
			Item.shootSpeed = 16f;
			Item.useAmmo = AmmoID.Bullet;
		}

        public override bool CanUseItem(Player player)
        {
			Item.noUseGraphic = false;
			Item.UseSound = SoundID.Item11;
			Item.useTime = 30;
			Item.useAnimation = 30;
			return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			if (type == ProjectileID.Bullet)
			{
				type = ProjectileID.BulletHighVelocity;
			}
			SGAPlayer sgaplayer = player.SGAPly();
			sgaplayer.ConsumeAmmoClip();

			if (!sgaplayer.ConsumeAmmoClip(false) || forcedreload)
			{
				int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, RevolverID, 0, knockBack, Main.myPlayer, 0.0f, 0f);
				return !forcedreload;
			}
			return (sgaplayer.ConsumeAmmoClip(false));
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
	}
}
