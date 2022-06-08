using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using SGAmod.NPCs.Sharkvern;

using SGAmod.Items.Weapons;
using SGAmod.HavocGear.Items.Weapons;
using Terraria.Utilities;
using SGAmod.Items.Consumables;
using Terraria.DataStructures;
using SGAmod.Buffs;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Dusts;
using SGAmod.Items.Accessories;
using SGAmod.Items.Weapons.Vibranium;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	class ThrowerGlove : ModItem
	{
		public static string disc = "\nMay also be worn in place of a Grapple Hook to throw grenades with the grapple key\nHowever, the grenades are slower and have a cooldown";
		public virtual int level => 0;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Grenadier's Glove");
			Tooltip.SetDefault("Throws hand grenades further, and increases their damage\nCan be reforged"+ disc);
		}

		public static int FindGrenadeToThrow(Mod mod,Player player, int level)
		{
		List<int> grenadetypes = new List<int>();
			grenadetypes.Add(ItemID.Grenade);
			grenadetypes.Add(ItemID.BouncyGrenade);
			grenadetypes.Add(ItemID.StickyGrenade);
			grenadetypes.Add(ItemID.PartyGirlGrenade);
			grenadetypes.Add(ItemID.Beenade);
			grenadetypes.Add(Mod.Find<ModItem>("AcidGrenade").Type);
			grenadetypes.Add(Mod.Find<ModItem>("SwarmGrenade").Type);
			grenadetypes.Add(Mod.Find<ModItem>("ThermalGrenade").Type);
			if ((level != 2 && level>0) || level == 3)
			{
				grenadetypes.Add(ModContent.ItemType<CelestialCocktail>());
				grenadetypes.Add(ModContent.ItemType<SludgeBomb>());
				grenadetypes.Add(ItemID.MolotovCocktail);
				grenadetypes.Add(ItemID.Bone);
				grenadetypes.Add(ItemID.Ale);
				grenadetypes.Add(ItemID.SpikyBall);
				grenadetypes.Add(ModContent.ItemType<Trap.ThrowableTrapSpikyball>());
				grenadetypes.Add(ModContent.ItemType<Trap.WreckerBalls>());
			}

			if (level == 2 || level == 3)
			{
				grenadetypes.Add(ItemID.Dynamite);
				grenadetypes.Add(ItemID.BombFish);
				grenadetypes.Add(ItemID.BouncyDynamite);
				grenadetypes.Add(ItemID.StickyDynamite);
				grenadetypes.Add(ItemID.Bomb);
				grenadetypes.Add(ItemID.StickyBomb);
				grenadetypes.Add(ItemID.BouncyBomb);
			}

			for (int i = 0; i < 58; i++)
			{
				int type = grenadetypes.Find(x => x == player.inventory[i].type);
				if (type > 0 && player.inventory[i].stack > 0)
				{
					return player.inventory[i].type;
				}
			}
			return -1;


		}
		public static int FindProjectile(int proj,int weapon)
		{
			if (proj == ProjectileID.Bone)
				proj = ProjectileID.BoneGloveProj;
			if (weapon == ItemID.AleThrowingGlove || proj == ItemID.AleThrowingGlove)
				proj = ProjectileID.Ale;
			return proj;

		}

		public static int FindItem(int weapon)
		{
			if (weapon == ItemID.Ale)
				weapon = ItemID.AleThrowingGlove;
			return weapon;

		}

		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.damage = 0;
			Item.shootSpeed = 5f;
			Item.shoot = ProjectileID.Grenade;
			Item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			Item.width = 24;
			Item.height = 24;
			Item.maxStack = 1;
			Item.knockBack = 9;
			Item.consumable = false;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 60;
			Item.useTime = 60;
			//item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<GrenadeNotAHook>();
			Item.value = Item.buyPrice(0, 1, 50, 0);
			Item.rare = 3;
		}

		public override bool CanUseItem(Player player)
		{
			return (ThrowerGlove.FindGrenadeToThrow(Mod,player, level) >0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int basetype = ThrowerGlove.FindGrenadeToThrow(Mod, player, level);
			//if (player.CountItem(mod.ItemType("AcidGrenade"))>0)
			//basetype = mod.ItemType("AcidGrenade");

			Vector2 basespeed = new Vector2(speedX, speedY);
			float speedbase = basespeed.Length()*player.Throwing().thrownVelocity;

			basespeed.Normalize();

			Item basetype2 = new Item();
			int itemtype = FindItem(basetype);
			basetype2.SetDefaults(itemtype);
			float baseumtli = (Item.useTime/player.GetModPlayer<SGAPlayer>().ThrowingSpeed) /60f;
			if (itemtype == ItemID.Beenade)
				baseumtli = 1f;
			player.itemAnimation = (int)(basetype2.useAnimation * baseumtli);
			player.itemAnimationMax = (int)(basetype2.useAnimation * baseumtli);
			player.itemTime = (int)(basetype2.useTime* baseumtli);
			type = FindProjectile(basetype2.shoot, basetype);

			if (itemtype == Mod.Find<ModItem>("ThrowableTrapSpikyball").Type)
				speedbase /= 1f;

			basespeed *= (basetype2.shootSpeed + speedbase);
			speedX = basespeed.X;
			speedY = basespeed.Y;
			if (type!=ProjectileID.Beenade)
			damage += (int)((float)basetype2.damage * player.Throwing().thrownDamage);

			damage += (int)(basetype2.type == ItemID.MolotovCocktail || basetype2.type == ItemID.AleThrowingGlove || basetype2.type == ItemID.Bone || basetype2.type == ItemID.SpikyBall ? damage * 1f : 0);


			if (TrapDamageItems.SavingChanceMethod(player,true))
			player.ConsumeItem(basetype);

			Projectile proj = Main.projectile[Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI)];
			proj.Throwing().DamageType = DamageClass.Throwing;
			// proj.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// proj.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			proj.friendly = true;
			proj.hostile = false;
			proj.netUpdate = true;
			IdgProjectile.Sync(proj.whoAmI);

			return false;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/GLOOOVE_FINAL123"); }
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Silk, 5).AddIngredient(ItemID.BeeWax, 4).AddIngredient(ItemID.Leather, 6).AddRecipeGroup("SGAmod:Tier2Bars", 4).AddTile(TileID.WorkBenches).Register();
			CreateRecipe(1).AddIngredient(ItemID.Silk, 5).AddIngredient(ItemID.BeeWax, 4).AddIngredient(ItemID.TatteredCloth, 2).AddRecipeGroup("SGAmod:Tier2Bars", 4).AddTile(TileID.WorkBenches).Register();
		}

	}

	class ThrowerGloveMK2 : ThrowerGlove
	{
		public override int level => 1;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Rioter's Glove");
			Tooltip.SetDefault("Throws hand grenades further, and increases their damage\nUpgraded to now throw Ale, Molotovs, Spiky balls, and Bones! And improving their damage\n" + disc);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = 1;
			Item.damage = 6;
			Item.shoot = ModContent.ProjectileType<GrenadeNotAHook2>();
			Item.shootSpeed = 5.5f;
			Item.value = Item.buyPrice(0, 2, 50, 0);
			Item.rare = 4;
			Item.expert = true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/MolotovGlove"); }
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("ThrowerGlove"), 1).AddIngredient(mod.ItemType("DivineShower"), 2).AddIngredient(ItemID.AleThrowingGlove, 1).AddIngredient(ItemID.BoneGlove, 1).AddIngredient(ItemID.AncientCloth, 5).AddRecipeGroup("SGAmod:Tier5Bars", 12).AddTile(TileID.WorkBenches).AddTile(TileID.MythrilAnvil).Register();
		}

	}

	class ThrowerGloveDynamite : ThrowerGlove
	{
		public override int level => 2;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Demolitionist's Glove");
			Tooltip.SetDefault("Throws hand grenades further, and increases their damage\nUpgraded to now throw Bombs and Dynamite!\n" + disc);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = 1;
			Item.shoot = ModContent.ProjectileType<GrenadeNotAHook3>();
			Item.shootSpeed = 5.5f;
			Item.value = Item.buyPrice(0, 2, 50, 0);
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/DynamiteGlove"); }
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("ThrowerGlove"), 1).AddIngredient(ItemID.BeeWax, 16).AddIngredient(ItemID.Dynamite, 8).AddIngredient(ItemID.StickyBomb, 16).AddRecipeGroup("SGAmod:Tier3Bars", 8).AddTile(TileID.WorkBenches).Register();
		}
	}


	class GucciGauntlet : ThrowerGlove, IHellionDrop
	{
		int IHellionDrop.HellionDropAmmount() => 1;
		int IHellionDrop.HellionDropType() => ModContent.ItemType<GucciGauntlet>();
		public override int level => 3;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Gucci Gauntlet");
			Tooltip.SetDefault("'ah shit... here we go again'-CJ\nThrows everything previous gloves can\nGains extra powers when worn as the player gains Large Gems" + disc);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = 1;
			Item.damage = 11;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<GrenadeNotAHook4>();
			Item.shootSpeed = 5.75f;
			Item.useAnimation = 50;
			Item.crit = 10;
			Item.useTime = 50;
			Item.value = Item.buyPrice(1, 0, 0, 0);
			Item.rare = ItemRarityID.Quest;
			Item.expert = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Weapons/IDontFeelSoGucci").Value;
				Item.GetGlobalItem<ItemUseGlow>().angleAdd = MathHelper.ToRadians(45f);
				Item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Main.hslToRgb((Main.GlobalTimeWrappedHourly * 2.5f) % 1f, 0.9f, 0.75f)*MathHelper.Clamp((float)Math.Sin(((float)player.itemAnimation/ (float)player.itemAnimationMax)/2f)*3f,0f,1f);
				};
			}
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/IDontFeelSoGucci"); }
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
			{

				if (Main.LocalPlayer.ownedLargeGems[0])
					tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.Purple, "Amethyst Gem grants you a chance of Shadowflames on all your attacks")));
				if (Main.LocalPlayer.ownedLargeGems[1])
					tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.Yellow, "Topaz Gem increases your mining speed")));
				if (Main.LocalPlayer.ownedLargeGems[2])
					tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.Blue, "Saphire Gem grants unrestricted movement in water")));
				if (Main.LocalPlayer.ownedLargeGems[3])
					tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.Green, "Emerald Gem grants 1 minion slot and persistant Dryad's Blessing")));
				if (Main.LocalPlayer.ownedLargeGems[4])
					tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.Crimson, "Ruby Gem gives your attacks a very low chance to life steal")));
				if (Main.LocalPlayer.ownedLargeGems[5])
					tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.LightSlateGray, "Diamond Gem provides a minor boost to all stats")));
				if (Main.LocalPlayer.ownedLargeGems[6])
					tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.Goldenrod, "Amber Gem reduces the cost of all items in shops")));
			}

			tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.White, "When you have all 7 gems, Equip the glove and throw a 'grapple hook' to 'snap'")));
			tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.White, "Snapping will erase half the enemies, with a cooldown")));
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
			{
			tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.White, "Snapping will erase enemies with less than 100000 HP times the average of your damage bonuses")));
			tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.White, "100000 * ((Melee+Ranged+Magic+Summon+Throwing)/5)")));
			tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.White, "If an enemy's life exceeds this they will will receive the fading debuff")));
			tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.White, "This debuff does massive damage over time, and fades enemies out of existance when they run out of life")));
			tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.White, "Bosses affected will only receive the fading debuff")));
			tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.White, "Enemies who fade out of existance do not drop anything (unless a boss) or count as being killed")));

			}
			else
			{
			tooltips.Add(new TooltipLine(Mod, "GucciGlove", Idglib.ColorText(Color.White, "However this power is 'Balanced', Hold the Control key for more details")));
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("ThrowerGloveMK2"), 1).AddIngredient(mod.ItemType("ThrowerGloveDynamite"), 1).AddRecipeGroup("Fragment", 15).AddIngredient(ItemID.LunarBar, 10).AddIngredient(mod.ItemType("DrakeniteBar"), 12).AddTile(TileID.LunarCraftingStation).Register();
		}

	}

	internal class GrenadeNotAHook : ModProjectile
	{
		public virtual int level => 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
			Projectile.tileCollide = false;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/AcidGrenade"); }
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			player.GetModPlayer<SGAPlayer>().greandethrowcooldown = 120;
			int basetype = ThrowerGlove.FindGrenadeToThrow(Mod, player, level);
			//if (player.CountItem(mod.ItemType("AcidGrenade"))>0)
			//basetype = mod.ItemType("AcidGrenade");

			Vector2 basespeed = (Projectile.velocity/2f);
			float speedbase = basespeed.Length() * player.Throwing().thrownVelocity;
			basespeed.Normalize();

			Item basetype2 = new Item();
			basetype2.SetDefaults(ThrowerGlove.FindItem(basetype));
			player.itemTime = basetype2.useTime;
			int type = ThrowerGlove.FindProjectile(basetype2.shoot, basetype);
			basespeed *= (basetype2.shootSpeed + speedbase);
			int damage = (int)(basetype2.damage * player.Throwing().thrownDamage);

			damage += (int)(basetype2.type == ItemID.MolotovCocktail || basetype2.type == ItemID.AleThrowingGlove || basetype2.type == ItemID.Bone || basetype2.type == ItemID.SpikyBall ? damage * 1.5f : 0);

			int proj=Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, basespeed.X, basespeed.Y, type, damage, basetype2.knockBack, player.whoAmI);
			Main.projectile[proj].friendly = true;
			Main.projectile[proj].hostile = false;
			Main.projectile[proj].netUpdate = true;
			IdgProjectile.Sync(proj);

			if (TrapDamageItems.SavingChanceMethod(player, true))
				player.ConsumeItem(basetype);

			Projectile.Kill();
		}

		// Use this hook for hooks that can have multiple hooks mid-flight: Dual Hook, Web Slinger, Fish Hook, Static Hook, Lunar Hook
		public override bool? CanUseGrapple(Player player)
		{
			return (player.GetModPlayer<SGAPlayer>().greandethrowcooldown<1 && ThrowerGlove.FindGrenadeToThrow(Mod, player, level) >-1);
		}

	}

	internal class GrenadeNotAHook2 : GrenadeNotAHook
	{
		public override int level => 1;

	}

	internal class GrenadeNotAHook3 : GrenadeNotAHook
	{
		public override int level => 2;
	}

	internal class GrenadeNotAHook4 : GrenadeNotAHook
	{
		public override int level => 3;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");
		}

		public override bool CanDamage()
		{
			return false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			if (SGAWorld.SnapCooldown < 1)
			{
				if (owner.ownedLargeGems[0] && owner.ownedLargeGems[1] && owner.ownedLargeGems[2] && owner.ownedLargeGems[3] && owner.ownedLargeGems[4] && owner.ownedLargeGems[5] && owner.ownedLargeGems[6])
				{
					bool finalGem = owner.SGAPly().finalGem > 0;
					float basevalues = (owner.GetDamage(DamageClass.Melee) + owner.GetDamage(DamageClass.Magic) + owner.GetDamage(DamageClass.Summon) + owner.GetDamage(DamageClass.Ranged) + owner.Throwing().thrownDamage)/5f;
					Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<GucciSnap>(), finalGem ? int.MaxValue-1 : (int)(100000f*basevalues), 10f, Projectile.owner);
					Projectile.Kill();
					SGAWorld.SnapCooldown = finalGem ? 60 : 60 * 300;
					if (Main.dedServ)
					{
						ModPacket packet = SGAmod.Instance.GetPacket();
						packet.Write((ushort)MessageType.Snapped);
						packet.Write(finalGem ? 60 : 60 * 300);
						packet.Send();
					}
					return;
				}
			}
			base.AI();
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
			Projectile.tileCollide = false;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/AcidGrenade"); }
		}

	}

	class GucciSnap : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Snap");
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			Projectile.ai[0] += 1;
			if (Projectile.ai[0] == 2)
			{
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Snap").WithVolume(1.0f).WithPitchVariance(.25f), Projectile.Center);
				RippleBoom.MakeShockwave(Projectile.Center, 5f, 1f, 10f, 120, 1f, true);
				CombatText.NewText(new Rectangle((int)owner.position.X, (int)owner.position.Y, owner.width, owner.height), Color.Red, "snap...", true, false);
			}
			if (Projectile.ai[0] == 15)
			{
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Fade").WithVolume(1.0f).WithPitchVariance(.25f), new Vector2(-1, -1));
				for(int i = 0; i < Main.maxNPCs; i += 1)
				{
					NPC npc = Main.npc[i];
					if (npc != null && npc.active)
					{
						if (!npc.friendly && !npc.townNPC && !npc.immortal && !npc.dontTakeDamage && npc.realLife<1)
						{
							if (Main.rand.Next(0, 2) == 0)
							{
								if (npc.life < Projectile.damage && !npc.boss)
								{
									npc.GetGlobalNPC<SGAnpcs>().Snapped = 200;
								}
								else
								{
									IdgNPC.AddBuffBypass(npc.whoAmI,Mod.Find<ModBuff>("SnapFade").Type, (int)((float)Projectile.damage / 500f));
								}
							}
						}
					}
				}
			}
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Grenade);
			Projectile.timeLeft = 300;
			Projectile.aiStyle = -1;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.tileCollide = false;
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.HerbBag); }
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

	}

		class AcidGrenade : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Acid Grenade");
			Tooltip.SetDefault("Deals Acid Burn to all affected, but does not last long before exploding");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Grenade);
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			// item.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Item.damage = 60;
			Item.shootSpeed = 3f;
			Item.shoot = Mod.Find<ModProjectile>("AcidGrenadeProj").Type;
			Item.value = Item.buyPrice(0, 0, 2, 0);
			Item.rare = 3;
		}
		public override void AddRecipes()
		{
			CreateRecipe(15).AddIngredient(ItemID.Grenade, 15).AddIngredient(mod.ItemType("VialofAcid"), 1).AddTile(TileID.WorkBenches).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			return true;
		}

	}

	class AcidGrenadeProj : ModProjectile
	{

		bool hitonce = false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Acid Grenade");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Grenade);
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.timeLeft = 180;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/AcidGrenade"); }
		}

		public override bool PreKill(int timeLeft)
		{
			if (!hitonce)
			{
				Projectile.width = 160;
				Projectile.height = 160;
				Projectile.position -= new Vector2(80, 80);
			}

			for (int i = 0; i < 100; i += 1)
			{
				float randomfloat = Main.rand.NextFloat(1f, 6f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();

				int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 32, Projectile.Center.Y - 32), 64, 64, Mod.Find<ModDust>("AcidDust").Type);
				Main.dust[dust].scale = 2.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (Projectile.velocity * (float)(Main.rand.Next(10, 20) * 0.01f)) + (randomcircle * randomfloat);
			}

			int theproj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("Explosion").Type, (int)((double)Projectile.damage * 1.5f), Projectile.knockBack, Projectile.owner, 0f, 0f);
			Main.projectile[theproj].Throwing().DamageType = DamageClass.Throwing;
			IdgProjectile.AddOnHitBuff(theproj, Mod.Find<ModBuff>("AcidBurn").Type, 120);

			Projectile.velocity = default(Vector2);
			Projectile.type = ProjectileID.Grenade;
			return true;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("AcidDust").Type);
			Main.dust[dust].scale = 0.75f;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			Projectile.timeLeft -= 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!hitonce)
			{
				hitonce = true;
				Projectile.position -= new Vector2(64, 64);
				Projectile.width = 160;
				Projectile.height = 160;
				Projectile.timeLeft = 1;
			}
			//projectile.Center -= new Vector2(48,48);

			target.AddBuff(Mod.Find<ModBuff>("AcidBurn").Type, 120);
		}

	}
	class ThermalGrenade : AcidGrenade
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Thermal Grenade");
			Tooltip.SetDefault("Deals Thermal Blaze to all affected");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Grenade);
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			// item.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Item.damage = 72;
			Item.shootSpeed = 3f;
			Item.shoot = Mod.Find<ModProjectile>("ThermalGrenadeProj").Type;
			Item.value = Item.buyPrice(0, 0, 2, 0);
			Item.rare = 5;
		}
		public override void AddRecipes()
		{
			CreateRecipe(15).AddIngredient(ItemID.Grenade, 15).AddIngredient(mod.ItemType("FieryShard"), 1).AddTile(TileID.WorkBenches).Register();
		}

	}

	class ThermalGrenadeProj : AcidGrenadeProj
	{

		bool hitonce = false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Thermal Grenade");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Grenade);
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.timeLeft = 240;
		}

	public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/ThermalGrenade"); }
		}

		public override bool PreKill(int timeLeft)
		{
			if (!hitonce)
			{
				Projectile.width = 160;
				Projectile.height = 160;
				Projectile.position -= new Vector2(80, 80);
			}

			for (int i = 0; i < 100; i += 1)
			{
				float randomfloat = Main.rand.NextFloat(1f, 6f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();

				int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 32, Projectile.Center.Y - 32), 64, 64, Mod.Find<ModDust>("HotDust").Type);
				Main.dust[dust].scale = 2.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (Projectile.velocity * (float)(Main.rand.Next(10, 20) * 0.01f)) + (randomcircle * randomfloat);
			}

			int theproj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("Explosion").Type, (int)((double)Projectile.damage * 1f), Projectile.knockBack, Projectile.owner, 0f, 0f);
			Main.projectile[theproj].Throwing().DamageType = DamageClass.Throwing;
			IdgProjectile.AddOnHitBuff(theproj, Mod.Find<ModBuff>("ThermalBlaze").Type, 60 * 3);

			Projectile.velocity = default(Vector2);
			Projectile.type = ProjectileID.Grenade;
			return true;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type);
			Main.dust[dust].scale = 0.75f;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!hitonce)
			{
				hitonce = true;
				Projectile.position -= new Vector2(64, 64);
				Projectile.width = 128;
				Projectile.height = 128;
				Projectile.timeLeft = 1;
			}
			//projectile.Center -= new Vector2(48,48);

			target.AddBuff(Mod.Find<ModBuff>("ThermalBlaze").Type, 60*4);
		}
	}

	class SludgeBomb : AcidGrenade, IDankSlowText
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Sludge Bomb");
			Tooltip.SetDefault("Explodes into sludge that sticks to walls and damage enemies\nEnemies near the sludge get Oiled, Confused, and Dank Slowed");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Grenade);
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			// item.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Item.damage = 80;
			Item.shootSpeed = 4f;
			Item.shoot = ModContent.ProjectileType<SludgeBombProj>();
			Item.value = Item.buyPrice(0, 0, 2, 0);
			Item.rare = 5;
		}
		public override void AddRecipes()
		{
			//nil
		}

	}

	class SludgeBombProj : AcidGrenadeProj
	{

		bool hitonce = false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Sludge Bomb");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Grenade);
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.timeLeft = 240;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/SludgeBomb"); }
		}

		public override bool PreKill(int timeLeft)
		{
			if (!hitonce)
			{
				Projectile.width = 160;
				Projectile.height = 160;
				Projectile.position -= new Vector2(80, 80);
			}

			for (int i = 0; i < 100; i += 1)
			{
				float randomfloat = Main.rand.NextFloat(1f, 6f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Vector2 randomzer = Dimensions.SpaceDim.Gaussian2D(Main.rand.NextFloat(), Main.rand.NextFloat());

				int dust = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y)+ randomzer*24f, 0, 0, 184, 0, 0, 100, new Color(30, 30, 30, 20), 1f);
				Main.dust[dust].scale = 2.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (Projectile.velocity * (float)(Main.rand.Next(10, 20) * 0.01f)) + (randomzer * randomfloat);
			}
			for (int i = 0; i < 10; i += 1)
			{
				Projectile proj = Projectile.NewProjectileDirect(Projectile.Center, Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(2f, 10f), ModContent.ProjectileType<SludgeProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Main.rand.Next(3, 30));
			}

			Projectile.velocity = default(Vector2);
			Projectile.type = ProjectileID.Grenade;
			return true;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 184,0,0,100, new Color(30, 30, 30, 20), 1f);
			Main.dust[dust].scale = 0.75f;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!hitonce)
			{
				hitonce = true;
				Projectile.position -= new Vector2(64, 64);
				Projectile.width = 128;
				Projectile.height = 128;
				Projectile.timeLeft = 1;
			}
		}
	}

	public class SludgeProj : ModProjectile
	{

		float scalePercent => MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, Math.Min(Projectile.localAI[0] / 10f, 1f));

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
			Projectile.timeLeft = 400;
			Projectile.light = 0.1f;
			Projectile.extraUpdates = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			AIType = -1;
			Main.projFrames[Projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/MudBlob"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ew Gross!");
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void AI()
		{

			if (Projectile.localAI[1] < 10)
			{
				Projectile.localAI[1] = Main.rand.Next(3)+100;
			}

			Projectile.ai[1] += 1;
			Projectile.localAI[0] += 1;

			Point16 point = new Point16((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16);

			if (Projectile.localAI[0] > Projectile.ai[0] && WorldGen.InWorld(point.X, point.Y))
			{
				if (Main.tile[point.X, point.Y].WallType > 0) 
				{
					if (Projectile.localAI[0] < 10000)
					{
						Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
						Projectile.localAI[0] += 10000;
					}

					Projectile.velocity /= 1.25f;
					Projectile.rotation += (Projectile.velocity.X + Projectile.velocity.Y) / 8f;

					Projectile.scale = Math.Min(Projectile.scale + (4 - Projectile.scale) / 3f, 3f);


					foreach (NPC enemy in Main.npc.Where(npctest => npctest.active && !npctest.friendly && !npctest.immortal &&
					npctest.DistanceSQ(Projectile.Center)< (32 * Projectile.scale* (32 * Projectile.scale))))
					{
						if (!enemy.boss)
						enemy.AddBuff(ModContent.BuffType<DankSlow>(), 20);
						enemy.AddBuff(BuffID.Oiled, 60*1);
						enemy.AddBuff(BuffID.Confused, 3);
						enemy.SGANPCs().nonStackingImpaled = Projectile.damage;

						if (Projectile.ai[1]%20==0)
						enemy.SGANPCs().AddDamageStack(Projectile.damage/3,120);
					}

					for (int num654 = 0; num654 < 1 + (Projectile.localAI[0]<10003 ? 10 : 0); num654++)
					{
						Vector2 randomcircle = Dimensions.SpaceDim.Gaussian2D(Main.rand.NextFloat(), Main.rand.NextFloat());

						//Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); 
						Vector2 ogcircle = randomcircle;// randomcircle *= (float)(num654 / 10.00);
						int num655 = Dust.NewDust(Projectile.Center, 0,0, 184, Projectile.velocity.X + randomcircle.X * 1f, Projectile.velocity.Y + randomcircle.Y * 1f, 200, new Color(30, 30, 30, 20), 1f);
						Main.dust[num655].noGravity = true;
						Main.dust[num655].position += (randomcircle * 20f);

					}

					return;
                }
                else
                {
					if (Projectile.localAI[0]>8000)
					Projectile.Kill();
                }
			}
				Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.PiOver2;
				Projectile.velocity.Y += 0.25f;

				int num126 = Dust.NewDust(Projectile.position - new Vector2(2, 2), Main.rand.Next(Projectile.width + 6), Main.rand.Next(Projectile.height + 6), 184, 0, 0, 140, new Color(30, 30, 30, 20), 1f);
				Main.dust[num126].noGravity = true;
				Main.dust[num126].velocity = Projectile.velocity * 0.5f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			Texture2D tex = SGAmod.ExtraTextures[107+ (int)Projectile.localAI[1]%3];
			if (Projectile.localAI[0] < 9000)
            {
				tex = Main.projectileTexture[Projectile.type];
				spriteBatch.Draw(tex, drawPos, new Rectangle(0,(Projectile.whoAmI*(tex.Height/3))%tex.Height, tex.Width, tex.Height/3), (lightColor.MultiplyRGB(Color.Brown) * 0.75f)* scalePercent, Projectile.rotation,new Vector2(tex.Width,tex.Height/3f)/2f, Projectile.scale, SpriteEffects.None, 0f);
				return false;
			}
			spriteBatch.Draw(tex, drawPos, null, lightColor.MultiplyRGB(Color.Brown) * 0.5f* scalePercent, Projectile.rotation, tex.Size()/2f, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}

	class RottenEggshels : ModItem, IAuroraItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Calabash Nebula");
			Tooltip.SetDefault("'Rotten Eggs Infused with so much Starlight it might as well be...'");
		}
		public override string Texture => "Terraria/Projectile_"+ProjectileID.RottenEgg;

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb(Main.GlobalTimeWrappedHourly % 1f, 1f, 0.75f);
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Grenade);
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			// item.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Item.damage = 100;
			Item.shootSpeed = 6f;
			Item.shoot = ProjectileID.RottenEgg;
			Item.value = Item.buyPrice(0, 0, 2, 0);
			Item.rare = 0;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

			Vector2 position2 = new Vector2(speedX, speedY);

			Projectile.NewProjectile(position, position2.RotatedByRandom(Math.PI/20f), ModContent.ProjectileType<GammBurstBeam>(), damage, knockBack * 3f, player.whoAmI);

			for(int i = 0; i < 30; i += 1)
            {
				Projectile.NewProjectile(position, position2.RotatedByRandom(Math.PI * (0.5f/(1f+(i/5f))))*Main.rand.NextFloat(2.75f,5f), type, (int)(damage/3f), knockBack, player.whoAmI);
			}

			return false;
        }
        public override void AddRecipes()
		{
			//nil
		}

	}


	class CelestialCocktail : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Cosmic Cocktail");
			Tooltip.SetDefault("'Made from the sweet celestial essence of the lunar slime princess'\nExplodes violently, sending celestial projectiles in the general direction the projectile was traveling");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.MolotovCocktail);
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			// item.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Item.damage = 40;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.maxStack = 999;
			Item.shootSpeed = 8f;
			Item.shoot = Mod.Find<ModProjectile>("CelestialCocktailProj").Type;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = 10;
		}
		public override void AddRecipes()
		{
			CreateRecipe(75).AddIngredient(ItemID.MolotovCocktail, 75).AddRecipeGroup("Fragment", 3).AddIngredient(mod.ItemType("LunarRoyalGel"), 2).AddTile(TileID.LunarCraftingStation).Register();
			CreateRecipe(150).AddIngredient(ItemID.Silk, 10).AddIngredient(ItemID.Torch, 10).AddIngredient(ItemID.Ale, 50).AddRecipeGroup("Fragment", 1).AddIngredient(mod.ItemType("LunarRoyalGel"), 1).AddTile(TileID.LunarCraftingStation).Register();

		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			return true;
		}

	}

	class CelestialCocktailProj : ModProjectile
	{

		bool hitonce = false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Celestial Cocktail");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Grenade);
			// projectile.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// projectile.Throwing().thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.timeLeft = 180;
		}

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/CelestialCocktail"); }
		}

		public override bool? CanHitNPC(NPC target)
		{
			int player = Projectile.owner;
			return base.CanHitNPC(target);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			if (!hitonce)
			{
				Projectile.width = 128;
				Projectile.height = 128;
				Projectile.position -= new Vector2(64, 64);
			}

			Main.player[Projectile.owner].SGAPly().molotovLimit = 60;

			Vector2 gotohere = Projectile.velocity;
			gotohere.Normalize();

			int[] projectiletype = { ProjectileID.NebulaBlaze1, ProjectileID.VortexBeaterRocket, ProjectileID.HeatRay, ProjectileID.DD2LightningBugZap };
			float[] projectiledamage = { 1f, 1f, 2.5f, 0.25f };
			int[] projectilecount = { 9, 9,12,7 };
			for (int zz = 0; zz < 4; zz += 1)
			{
				for (int i = 0; i < projectilecount[zz]; i += 1)
				{
					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(120)) * Main.rand.NextFloat(6, 12);
					int proj = Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), projectiletype[zz], (int)((Projectile.damage * 1f)* projectiledamage[zz]), Projectile.knockBack / 10f, Projectile.owner);
					Main.projectile[proj].Throwing().DamageType = DamageClass.Throwing;
					// Main.projectile[proj].magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
					// Main.projectile[proj].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].velocity = new Vector2(perturbedSpeed.X, perturbedSpeed.Y);
					Main.projectile[proj].hostile = false;
					if (i != 2)
					{
						Main.projectile[proj].timeLeft = 300;
						Main.projectile[proj].penetrate = 4;
					}
					if (zz==3)
					Main.projectile[proj].penetrate = 15;
					Projectile.netUpdate = true;
					IdgProjectile.Sync(proj);
				}

			}

			int theproj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("SlimeBlast").Type, (int)((double)Projectile.damage * 1.5f), Projectile.knockBack, Projectile.owner, 0f, 0f);
			Main.projectile[theproj].Throwing().DamageType = DamageClass.Throwing;

			Projectile.velocity = default(Vector2);
			Projectile.type = ProjectileID.Grenade;
			return true;
		}

		public override bool PreAI()
		{
			
				for (int zz = 0; zz < Main.maxNPCs; zz += 1)
				{
				NPC npc = Main.npc[zz];
				if (!npc.dontTakeDamage && !npc.townNPC  && npc.active && npc.life>0)
				{
					Rectangle rech = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
					Rectangle rech2 = new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height);
					if (rech.Intersects(rech2))
					{
						Projectile.Damage();
						Projectile.Kill();

					}
				}
			}

			return true;
		}

		public override void AI()
		{

				int[] dustype = { Mod.Find<ModDust>("AcidDust").Type, Mod.Find<ModDust>("HotDust").Type, Mod.Find<ModDust>("MangroveDust").Type, Mod.Find<ModDust>("TornadoDust") .Type};
			int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustype[Main.rand.Next(0,4)]);
			Main.dust[dust].scale = 0.75f;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			Projectile.timeLeft -= 1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!hitonce)
			{
				hitonce = true;
				Projectile.position -= new Vector2(64, 64);
				Projectile.width = 128;
				Projectile.height = 128;
				Projectile.timeLeft = 1;
			}
			//projectile.Center -= new Vector2(48,48);
		}

	}

	class JarateShurikens : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Jarate Shuriken");
			Tooltip.SetDefault("Throws a fan of 5 'yellow' Snowballs outwards\nThese inflict Ichor and Sodden\nRequires and consumes 5 of the item per use");
		}

		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.damage = 18;
			Item.shootSpeed = 9f;
			Item.shoot = Mod.Find<ModProjectile>("JarateShurikensProg").Type;
			Item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			Item.width = 8;
			Item.height = 8;
			Item.knockBack = 6;
			Item.maxStack = 999;
			Item.consumable = false;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(0, 0, 1, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.ammo = AmmoID.Snowball;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int i = 0; i < 5; i += 1)
			{
				player.ConsumeItem(Mod.Find<ModItem>(GetType().Name).Type);
			}

			int numberProjectiles = 5;
			float rotation = MathHelper.ToRadians(20);
			for (int i = 0; i < numberProjectiles; i += 1)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY)).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)i / ((float)numberProjectiles - 1f)));
				int thisoned = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, Main.myPlayer);
			}


			return false;
		}

		public override bool CanUseItem(Player player)
		{
			return player.CountItem(Mod.Find<ModItem>(GetType().Name).Type)>4;
		}

		public override void AddRecipes()
		{
			CreateRecipe(150).AddIngredient(ItemID.Snowball, 150).AddIngredient(mod.ItemType("Jarate"), 1).AddTile(TileID.Bottles).Register();

		}

	}

	public class JarateShurikensProg : ModProjectile
	{

		protected int fakeid = ProjectileID.SnowBallFriendly;
		public int guyihit = -10;
		public int cooldown = -10;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jarate Shuriken");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/JarateShurikens"); }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Ichor, 60 * 10);
			if (Main.rand.Next(0, 5) == 1)
			{
				if (Main.player[Projectile.owner].GetModPlayer<SGAPlayer>().MVMBoost)
					target.AddBuff(Mod.Find<ModBuff>("SoddenSlow").Type, 60 * 4);
				target.AddBuff(Mod.Find<ModBuff>("Sodden").Type, 60 * 4);
			}
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.SnowBallFriendly);
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			// projectile.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// projectile.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.extraUpdates = 1;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = fakeid;

			for (int num654 = 0; num654 < 16; num654++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
				int num655 = Dust.NewDust(Projectile.position + Vector2.UnitX * -20f, Projectile.width + 40, Projectile.height, 75, -Projectile.velocity.X + randomcircle.X * 4f, -Projectile.velocity.Y + randomcircle.Y * 4f, 150, Color.Goldenrod, 0.8f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].noLight = true;
			}

			return true;
		}

		public override void AI()
		{


		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[Projectile.type].Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				Color color = Projectile.GetAlpha(lightColor) * ((float)Projectile.oldPos.Length / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[Projectile.type], drawPos, null, color*0.5f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

	}

	class UraniumSnowballs : JarateShurikens, IRadioactiveItem, IRadioactiveDebuffText
	{
		public int RadioactiveHeld() => 2;
		public int RadioactiveInventory() => 1;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Uranium Filled Snowballs");
			Tooltip.SetDefault("'Excuse me? Yeah, this is a thing'\nSnowballs leave behind Irradiated pools, infecting enemies\nEnemies killed while Irradiated explode, crits force an explosion\nRight click to throw an empowered shot that sticks to enemies\n" + Idglib.ColorText(Color.Orange, "Requires 2 Cooldown stacks, adds 20 seconds each"));
			//	+"\n"+ Idglib.ColorText(Color.Red,"You suffer Radiation 2 while holding these")+"\n"+Idglib.ColorText(Color.Red,"Radiation 1 if only in inventory"));
		}

        public override string Texture => "Terraria/Item_"+ItemID.Snowball;

        public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			Item.width = 8;
			Item.height = 8;
			Item.knockBack = 6;
			Item.maxStack = 999;
			Item.damage = 72;
			Item.crit = 15;
			Item.consumable = true;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(0, 0, 1, 0);
			Item.rare = ItemRarityID.Lime;
			Item.shootSpeed = 25f;
			Item.shoot = Mod.Find<ModProjectile>("UraniumSnowballsProg").Type;
			Item.ammo = AmmoID.Snowball;
		}

        public override Color? GetAlpha(Color lightColor)
        {
			return Color.Lerp(Color.Lime, Color.Green, 0.50f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f) / 2f);
        }
        /*public override void UpdateInventory(Player player)
        {
			player.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationOne").Type, 60*3);
		}*/
        public override void HoldItem(Player player)
        {
			//if (!player.HasAccessoryEquipped(ModContent.ItemType<HandlingGloves>()) || !player.HasAccessoryEquipped(ModContent.ItemType<BlinkTechGear>()))
		}

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			int numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(0);
			//for (int i = 0; i < numberProjectiles; i += 1)
			//{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY)).RotatedBy(MathHelper.Lerp(-rotation, rotation, Main.rand.NextFloat()));
				int thisoned = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, Main.myPlayer);
				Main.projectile[thisoned].Throwing().DamageType = DamageClass.Throwing;
				// Main.projectile[thisoned].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			if (player.altFunctionUse == 2 && player.SGAPly().AddCooldownStack(60 * 20, 2))
            {
				Main.projectile[thisoned].ai[1] = 1;
				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_DrakinShot, (int)position.X, (int)position.Y);
				if (sound != null)
					sound.Pitch -= 0.525f;
			}
			Main.projectile[thisoned].netUpdate = true;
			//}

			return false;
		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(ItemID.Snowball, 50).AddIngredient(ItemID.LunarOre, 1).AddTile(TileID.LunarCraftingStation).Register();
		}

	}

	public class UraniumSnowballsProg : JarateShurikensProg
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Uranium Filled Snowballs");
		}
		public override string Texture => "Terraria/Item_" + ItemID.Snowball;
		int hitnpc = -1;
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (crit)
			{
				target.SGANPCs().IrradiatedExplosion(target, damage);
			}
			hitnpc = target.whoAmI;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.SnowBallFriendly);
			// projectile.Throwing().thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.DamageType = DamageClass.Ranged;
			// projectile.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.extraUpdates = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.type = fakeid;

			for (float num654 = 0; num654 < 8; num654+=0.25f)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); randomcircle *= (float)(num654 / 10.00);
				int num655 = Dust.NewDust(Projectile.position + Vector2.UnitX * -6f, Projectile.width + 12, Projectile.height+12, 75, 0,0, 150, Color.Lime, 1.8f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].noLight = true;
				Main.dust[num655].velocity = new Vector2(randomcircle.X * 12f, randomcircle.Y * 12f);
			}

			if (Projectile.ai[1] < 1)
			{
				int proj = Projectile.NewProjectile(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RadioactivePool>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
			else
			{
				int proj = Projectile.NewProjectile(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RadioactivePool>(), Projectile.damage*2, Projectile.knockBack, Projectile.owner);
				Main.projectile[proj].width += 160;
				Main.projectile[proj].height += 160;
				Main.projectile[proj].timeLeft = 600;
				Main.projectile[proj].Center -= new Vector2(80, 80);
				if (hitnpc > -1)
                {
					//Main.projectile[proj].ai[1] = -(hitnpc+1);
				}
				Main.projectile[proj].netUpdate = true;
			}

			SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_SkyDragonsFuryShot, (int)Projectile.Center.X, (int)Projectile.Center.Y);
			if (sound != null)
				sound.Pitch -= 0.525f;

			return true;
		}

		public override void AI()
		{
			if (Projectile.ai[1] > 0)
			{
				Projectile.localAI[1] += 1;

				if (Projectile.localAI[1]%3==0)
				{
					int proj = Projectile.NewProjectile(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RadioactivePool>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
					Main.projectile[proj].timeLeft = 50;
				}
			}

			for (int num654 = 0; num654 < 2; num654++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
				int num655 = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(16,16), 0, 0, 75, randomcircle.X * 3f, randomcircle.Y * 3f, 150, Color.Lime, 1f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].noLight = true;
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[Projectile.type].Width * 0.5f, Projectile.height * 0.5f);
			for (int k = Projectile.oldPos.Length-1; k > 0; k-=1)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				Color color = Color.Lime * (1f-((float)k / ((float)Projectile.oldPos.Length)));
				spriteBatch.Draw(Main.projectileTexture[Projectile.type], drawPos, null, color * 0.75f, Projectile.rotation, drawOrigin, Projectile.scale+0.5f, SpriteEffects.None, 0f);
			}

			Texture2D texture = SGAmod.ExtraTextures[96];
			spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.Lime*0.50f, 0, texture.Size() / 2f, 0.5f, SpriteEffects.None, 0f);

			spriteBatch.Draw(Main.projectileTexture[Projectile.type], Projectile.Center-Main.screenPosition, null, Color.GreenYellow, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}

	public class RadioactivePool : SludgeProj
	{

		float scalePercent => MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, Math.Min(Projectile.localAI[0] / 10f, 1f));

		public override void SetDefaults()
		{
			Projectile.width = 128;
			Projectile.height = 128;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			// projectile.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.timeLeft = 160;
			Projectile.light = 0.1f;
			Projectile.extraUpdates = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			AIType = -1;
			Main.projFrames[Projectile.type] = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pool of Radioactive Sludge");
		}

		public override bool CanDamage()
		{
			return Projectile.ai[1]>0;
		}

        public override void AI()
		{
			if (Projectile.localAI[1] < 10)
			{
				Projectile.localAI[1] = Main.rand.Next(3) + 100;
			}
			Projectile.localAI[0] += 1;

			/*if (projectile.ai[1] < 0)
			{
				NPC npc = Main.npc[(int)(-projectile.ai[1] + 1)];
				if (npc!=null && npc.active)
				{
					projectile.Center = npc.Center;
                }
                else
                {
					projectile.ai[1] = 0;
                }
            }*/

			Point16 point = new Point16((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16);

			float boomspeed = Projectile.ai[1] > 0 ? 8 : 0;

			if (Projectile.timeLeft > 30 && boomspeed<1)
			{
				foreach (NPC enemy in Main.npc.Where(npctest => npctest.active && !npctest.friendly && !npctest.immortal &&
				npctest.Distance(Projectile.Center) < Projectile.width * Projectile.scale))
				{
					SGAnpcs sganpc = enemy.SGANPCs();
					sganpc.nonStackingImpaled = Projectile.damage * 3;
					sganpc.impaled += Projectile.damage/2;
					sganpc.IrradiatedAmmount = Math.Min(sganpc.IrradiatedAmmount + 1, Projectile.damage * 3);
					enemy.AddBuff(Mod.Find<ModBuff>("RadioDebuff").Type, 60 * 18);
				}
			}

			for (int num654 = 0; num654 < 4+ boomspeed * 4; num654++)
			{
				if (boomspeed > 0 || Main.rand.Next(160) < Projectile.timeLeft)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
					int num655 = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.width), 0, 0, 184, Projectile.velocity.X + randomcircle.X * (4f), Projectile.velocity.Y + randomcircle.Y * (4f), 150, Color.Lime, 0.5f);
					Main.dust[num655].noGravity = true;
				}
			}

			for (int num654 = 1; num654 < 3 + boomspeed*4; num654++)
			{
				if (boomspeed > 0 || Main.rand.Next(160) < Projectile.timeLeft)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
					int num655 = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width/2, Projectile.width/2), 0, 0, ModContent.DustType<RadioDust>(), Projectile.velocity.X + randomcircle.X * (2f), Projectile.velocity.Y + randomcircle.Y * (2f), boomspeed > 0 ? 140 : 220, boomspeed > 0 ? Color.Orange : Color.Lime, 0.5f + (boomspeed / 20f));
					Main.dust[num655].noGravity = true;
				}
			}

			if (Main.rand.Next(24) < Projectile.timeLeft)
			{
				int num126 = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width/2, Projectile.width/2), 0, 0, 184, 0, 0, 140, new Color(30, 30, 30, 20), 1f);
				Main.dust[num126].noGravity = true;
				Main.dust[num126].velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-6f, 1f));
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
	}


	class ThrowingStars : JarateShurikens
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Throwing Star");
			Tooltip.SetDefault("'Throws a... fallen star'\nThrowing Stars pierce enemies infinitely but do cause immunity frames");
		}

		public override string Texture => "Terraria/Projectile_" + ProjectileID.HallowStar;

		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			Item.width = 8;
			Item.height = 8;
			Item.knockBack = 6;
			Item.maxStack = 999;
			Item.damage = 40;
			Item.crit = 15;
			Item.consumable = true;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(0, 0, 0, 20);
			Item.rare = ItemRarityID.LightRed;
			Item.shootSpeed = 25f;
			Item.shoot = Mod.Find<ModProjectile>("ThrowingStarsProg").Type;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lerp(Color.Yellow, Color.Pink, 0.50f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f) / 2f);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			float rotation = MathHelper.ToRadians(0);
			Vector2 perturbedSpeed = (new Vector2(speedX, speedY)).RotatedBy(MathHelper.Lerp(-rotation, rotation, Main.rand.NextFloat()));
			int thisoned = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, Main.myPlayer);
			//Main.projectile[thisoned].Throwing().thrown = true;
			//Main.projectile[thisoned].ranged = false;
			Main.projectile[thisoned].netUpdate = true;
			return false;
		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(25).AddIngredient(ItemID.FallenStar, 8).AddIngredient(ItemID.Meteorite, 4).AddIngredient(ItemID.Shuriken, 25).AddTile(TileID.Anvils).Register();
		}

	}

	public class ThrowingStarsProg : JarateShurikensProg
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thrown Star");
		}
		public override string Texture => "Terraria/Item_" + ItemID.FallenStar;
		int hitnpc = -1;

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.FallingStar);
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.aiStyle = -1;
			// projectile.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// projectile.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// projectile.melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.extraUpdates = 0;
			Projectile.penetrate = -1;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //nil
        }

        public override bool PreKill(int timeLeft)
		{
			fakeid = ProjectileID.FallingStar;
			Projectile.type = fakeid;

			for (float num654 = 0; num654 < 8; num654 += 0.25f)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); randomcircle *= (float)(num654 / 10.00);
				int num655 = Dust.NewDust(Projectile.position + Vector2.UnitX * -6f, Projectile.width + 12, Projectile.height + 12, 75, 0, 0, 150, Color.Yellow, 1.8f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].noLight = true;
				Main.dust[num655].velocity = new Vector2(randomcircle.X * 12f, randomcircle.Y * 12f);
			}

			SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaivePierce, (int)Projectile.Center.X, (int)Projectile.Center.Y);
			if (sound != null)
				sound.Pitch -= 0.525f;

			return true;
		}

		public override void AI()
		{
			Projectile.localAI[0] += 1f;
			if (Projectile.ai[1] == 0)
			{
				for (int num654 = 0; num654 < 2; num654++)
				{
					Vector2 movehere = Main.rand.NextVector2Circular(16, 16);
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
					randomcircle.Normalize();
					randomcircle *= (float)(num654 / 10.00);
					int num655 = Dust.NewDust(Projectile.Center + movehere, 0, 0, 75, randomcircle.X * 2f, randomcircle.Y * 2f, 43, Color.Yellow, 2f);
					Main.dust[num655].noGravity = true;
					Main.dust[num655].noLight = true;
				}
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[Projectile.type].Width, Main.projectileTexture[Projectile.type].Height)*0.50f;
			for (int k = Projectile.oldPos.Length - 1; k > 0; k -= 1)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				Color color = Color.Yellow * (1f - ((float)k / ((float)Projectile.oldPos.Length)));
				spriteBatch.Draw(Main.projectileTexture[Projectile.type], drawPos, null, color * Projectile.Opacity * 1f, Projectile.rotation + ((Projectile.localAI[0]+k*4f)/8f), drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}

			Texture2D texture = SGAmod.ExtraTextures[96];
			spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.Lime * Projectile.Opacity * 0.50f, 0, texture.Size() / 2f, 0.5f, SpriteEffects.None, 0f);

			spriteBatch.Draw(Main.projectileTexture[Projectile.type], Projectile.Center - Main.screenPosition, null, Color.Pink * Projectile.Opacity, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);

			Texture2D texaz = SGAmod.ExtraTextures[110];

			for (float xx = -2f; xx < 2.5f; xx += 0.5f)
			{
				for (float i = 1f; i < 3; i += 0.4f)
				{
					float scalerz = (0.85f + (float)Math.Cos(Main.GlobalTimeWrappedHourly * 1.25f * (Math.Abs(xx) + i)) * 0.3f)*0.45f;
					spriteBatch.Draw(texaz, (Projectile.Center + ((Projectile.velocity.ToRotation() + (float)Math.PI / 1f)).ToRotationVector2() * (xx * 9f)) - Main.screenPosition, null, Color.Yellow* Projectile.Opacity * (0.5f / (i + xx)) * 0.25f, Projectile.velocity.ToRotation() + (float)Math.PI / 2f, new Vector2(texaz.Width / 2f, texaz.Height / 4f), (new Vector2(1 + i, 1 + i * 1.5f) / (1f + Math.Abs(xx))) * scalerz * Projectile.scale, SpriteEffects.None, 0f);
				}
			}

			return false;
		}
	}

	class SuperThrowingStars : ThrowingStars
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Super Star Thrower");
			Tooltip.SetDefault("'Throws a literal star'\nStars inflict Lava Burn and spawn with orbiting flares\nCreates a powerful explosion when it hits a surface\nDoes not cause immunity frames");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 9));
		}

		public override string Texture => "SGAmod/Items/Weapons/SuperThrowingStars";

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			Item.width = 8;
			Item.height = 8;
			Item.knockBack = 1;
			Item.maxStack = 999;
			Item.damage = 60;
			Item.crit = 20;
			Item.consumable = true;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.shootSpeed = 25f;
			Item.shoot = ModContent.ProjectileType<SuperThrowingStarsProg>();
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Lerp(Color.Orange, Color.Red, 0.50f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 4f) / 2f);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			float rotation = MathHelper.ToRadians(0);
			Vector2 perturbedSpeed = (new Vector2(speedX, speedY)).RotatedBy(MathHelper.Lerp(-rotation, rotation, Main.rand.NextFloat()));
			int thisoned = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, Main.myPlayer);
			//Main.projectile[thisoned].Throwing().thrown = true;
			//Main.projectile[thisoned].ranged = false;
			Main.projectile[thisoned].netUpdate = true;
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(25).AddIngredient(ModContent.ItemType<ThrowingStars>(), 25).AddIngredient(ItemID.FragmentSolar, 1).AddIngredient(ModContent.ItemType<OverseenCrystal>(), 2).AddTile(TileID.LunarCraftingStation).Register();
		}

	}

	public class SuperThrowingStarsProg : ThrowingStarsProg
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Thrown Realistic Star");
		}
		public override string Texture => "Terraria/Item_" + ItemID.FallenStar;
		int hitnpc = -1;
		float TimeLeftScale => MathHelper.Clamp((Projectile.timeLeft-5f) / 30f, 0f, 1f);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = 600;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.width = 96;
			Projectile.height = 96;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<LavaBurn>(), 150);
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if (Projectile.timeLeft > 30)
            {
				Projectile.timeLeft = 30;
			}
			return false;
        }

        public override bool PreKill(int timeLeft)
		{
			return true;
		}

		public override void AI()
		{
			Projectile.localAI[0] += 1;

			if (Projectile.localAI[0] < 60)
			{
				Projectile.velocity *= 0.97f;
			}

			if (Projectile.localAI[0] == 1)
			{

				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath, (int)Projectile.Center.X, (int)Projectile.Center.Y);
				if (sound != null)
					sound.Pitch = 0.525f;

				for (int i = 0; i < 10; i += 1)
				{
					Projectile proj = Projectile.NewProjectileDirect(Projectile.Center, Main.rand.NextVector2CircularEdge(16f, 16f) * Main.rand.NextFloat(1f, 2f), ModContent.ProjectileType<SuperThrowingStarsProg2>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.whoAmI);
					proj.netUpdate = true;
				}
			}

			if (Projectile.timeLeft == 29)
			{

				foreach(NPC npc in Main.npc.Where(testby => testby.active && (testby.Center - Projectile.Center).LengthSquared() < (260 * 260)))
                {
					if (!npc.dontTakeDamage && !npc.friendly)
					{
						int ammount = Projectile.damage * 4;
						npc.StrikeNPC(ammount, Projectile.knockBack * 16f, Math.Sign(npc.Center.X - Projectile.Center.X));
						Main.player[Projectile.owner].addDPS(Projectile.damage * 4);
						if (Main.netMode != 0)
						{
							NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, ammount, 16f, (float)1, 0, 0, 0);
						}
					}
				}

				SGAmod.AddScreenShake(32f, 420, Projectile.Center);

				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_BetsyFlameBreath, (int)Projectile.Center.X, (int)Projectile.Center.Y);
				if (sound != null)
					sound.Pitch = -0.525f;
			}

			if (TimeLeftScale < 1)
			{
				Projectile.velocity /= 2f;
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D texa = Main.itemTexture[ModContent.ItemType<SuperThrowingStars>()];

			float scale = Math.Min(Projectile.localAI[0] / 15f, 1f) * 5f;

			/*Vector2 orig = new Vector2(texa.Width, texa.Height / 9);

			spriteBatch.Draw(texa, projectile.Center - Main.screenPosition, new Rectangle(0, (int)(((Main.GlobalTimeWrappedHourly * 2f) % 9)*orig.Y), (int)orig.X, (int)orig.Y), Color.White, 0, orig/2f, 1f+(0.5f + ((1f - TimeLeftScale) * 0.50f)) * scale, SpriteEffects.None, 0f);*/

			Projectile[] projj = Main.projectile.Where(testby => testby.type == Projectile.type).OrderBy(testby => testby.whoAmI).ToArray();

			texa = ModContent.Request<Texture2D>("SGAmod/GlowOrb");

			if (projj.Length > 0 && projj[0] == Projectile)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

				foreach(Projectile prog2 in projj)
				spriteBatch.Draw(texa, prog2.Center - Main.screenPosition, null, Color.Yellow*0.40f* ((SuperThrowingStarsProg)prog2.ModProjectile).TimeLeftScale, 0, texa.Size() / 2f, (0.20f + ((1f - ((SuperThrowingStarsProg)prog2.ModProjectile).TimeLeftScale) * 0.15f)) * scale, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D texture = ModContent.Request<Texture2D>("SGAmod/Stain");
			Texture2D texture2 = ModContent.Request<Texture2D>("SGAmod/Voronoi");
			Texture2D texture3 = ModContent.Request<Texture2D>("SGAmod/TiledPerlin");


			Effect RadialEffect = SGAmod.RadialEffect;

			RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Stain").Value);
			RadialEffect.Parameters["alpha"].SetValue(TimeLeftScale/2f);
			RadialEffect.Parameters["texOffset"].SetValue(new Vector2(0, -Main.GlobalTimeWrappedHourly * 0.575f));
			RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(5f, 0.5f));
			RadialEffect.Parameters["ringScale"].SetValue(0.32f);
			RadialEffect.Parameters["ringOffset"].SetValue(0.32f);
			RadialEffect.Parameters["ringColor"].SetValue(Color.Yellow.ToVector3());
			RadialEffect.Parameters["tunnel"].SetValue(false);

			RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

			spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, texture.Size() / 2f, (0.75f + ((1f - TimeLeftScale) * 0.50f)) * scale, SpriteEffects.None, 0f);

			SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(Color.Yellow.ToVector4()* TimeLeftScale);
			SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(texture);
			SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(0.25f,0.25f));
			SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(-Projectile.Center.X/64f, -Projectile.Center.Y/64f));
			SGAmod.SphereMapEffect.Parameters["softEdge"].SetValue(4f);

			SGAmod.SphereMapEffect.CurrentTechnique.Passes["SphereMap"].Apply();

			spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, texture.Size() / 2f, (0.25f + ((1f - TimeLeftScale) * 0.50f))* scale, SpriteEffects.None, 0f);

			/*SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(Color.Orange.ToVector4() * TimeLeftScale*0.950f);
			SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(texture2);
			SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(0.5f, 0.5f));
			SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(-projectile.Center.X / 64f, -projectile.Center.Y / 64f));
			SGAmod.SphereMapEffect.Parameters["softEdge"].SetValue(20f);

			SGAmod.SphereMapEffect.CurrentTechnique.Passes["SphereMapAlpha"].Apply();

			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, 0, texture.Size() / 2f, (0.25f + ((1f - TimeLeftScale) * 0.50f)) * scale, SpriteEffects.None, 0f);*/

			SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(Color.Orange.ToVector4() * TimeLeftScale * 0.950f);
			SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(texture3);
			SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(0.5f,0.5f));
			SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(-Projectile.Center.X / 64f, -Projectile.Center.Y / 64f));
			SGAmod.SphereMapEffect.Parameters["softEdge"].SetValue(4f);

			SGAmod.SphereMapEffect.CurrentTechnique.Passes["SphereMapAlpha"].Apply();

			spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, texture.Size() / 2f, (0.25f + ((1f - TimeLeftScale) * 0.50f)) * scale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}
	}

	public class SuperThrowingStarsProg2 : ThrowingStarsProg
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thrown Star Flare");
		}
		public override string Texture => "Terraria/Item_" + ItemID.FallenStar;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.tileCollide = false;
			Projectile.Opacity = 0f;
			Projectile.hide = true;
		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			if (Projectile.ai[1] != 0)
				drawCacheProjsBehindProjectiles.Add(index);
		}

        public override void AI()
		{
			base.AI();


			Projectile owner = Main.projectile[(int)Projectile.ai[0]];

			if (owner.active && owner.type == ModContent.ProjectileType<SuperThrowingStarsProg>() && owner.timeLeft>30)
            {
				if (Projectile.localAI[0] == 2 || Projectile.ai[1] == 0)
				{
					Projectile.ai[1] = Main.rand.NextFloat(0.15f, 0.20f) * (Main.rand.NextBool() ? 1f : -1f);
					Projectile.Opacity = 0f;
					Projectile.netUpdate = true;
				}
				Projectile.Opacity = Math.Min(Projectile.Opacity + 0.10f, 1f);
				Projectile.tileCollide = false;
				Projectile.hide = true;

				Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[1]);
				Projectile.Center = owner.Center + ((Vector2.Normalize(Projectile.velocity)*16f)+(Projectile.velocity*MathHelper.Clamp(owner.localAI[0]/15f,0f,1f)*2f));
			}
            else
            {
				Projectile.ai[1] = 0;
				Projectile.hide = false;
				Projectile.Opacity = Math.Min(Projectile.Opacity + 0.10f,1f);
				Projectile.tileCollide = true;
			}
		}
	}


	class SoulPincher : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Lunar Fork");
			Tooltip.SetDefault("Throws a lunar fork that does an extra 500% damage to the most healthy enemy nearby\n'Moon Lord's favorite!'");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.useTurn = true;
			Item.width = 8;
			Item.height = 8;
			Item.knockBack = 6;
			Item.damage = 100;
			Item.crit = 0;
			//item.UseSound = SoundID.Item1;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.shootSpeed = 5f;
			Item.shoot = ModContent.ProjectileType<SoulPincherProjectile>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			int numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(0);
			//for (int i = 0; i < numberProjectiles; i += 1)
			//{
			Vector2 perturbedSpeed = (new Vector2(speedX, speedY)).RotatedBy(MathHelper.Lerp(-rotation, rotation, Main.rand.NextFloat()));
			int thisoned = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, Main.myPlayer);
			Main.projectile[thisoned].netUpdate = true;
			//}

			SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 66);
			if (sound != null)
				sound.Pitch -= 0.525f;

			return false;
		}

	}

	public class SoulPincherProjectile : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Pincher Projectile");
		}
		public override string Texture => "SGAmod/Items/Weapons/SoulPincher";
		int hitnpc = -1;

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.SnowBallFriendly);
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 5;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

        public override bool CanDamage()
        {
			return Projectile.timeLeft > 100;
        }

        public override bool PreKill(int timeLeft)
		{

			if (timeLeft > 100)
			{

				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 43);
				if (sound != null)
					sound.Pitch -= 0.525f;

				sound = SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaiveImpactGhost, (int)Projectile.Center.X, (int)Projectile.Center.Y);
				if (sound != null)
					sound.Pitch -= 0.525f;

				List<NPC> enemies = SGAUtils.ClosestEnemies(Projectile.Center, 600);
				if (enemies != null)
				{
					enemies = enemies.OrderBy(testby => -testby.life).ToList();

					if (enemies.Count > 0)
					{
						NPC targetenemy = enemies[0];
						int ammount = Projectile.damage * 5;
						targetenemy.StrikeNPC(ammount, 20f, Math.Sign(Projectile.velocity.X));
						Main.player[Projectile.owner].addDPS(ammount);
						if (Main.netMode != 0)
						{
							NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, targetenemy.whoAmI, ammount, 16f, (float)1, 0, 0, 0);
						}
						for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.Pi / 4f)
						{
							Projectile prog = Projectile.NewProjectileDirect(targetenemy.Center + Vector2.UnitX.RotatedBy(f) * 128f, Vector2.UnitX.RotatedBy(f) * -3f, Projectile.type, 0, 0, Projectile.owner);
							prog.timeLeft = 100;
						}
						Projectile prog2 = Projectile.NewProjectileDirect(Projectile.Center, Vector2.Normalize(targetenemy.Center-Projectile.Center)*4f, Projectile.type, 0, 0, Projectile.owner);
						prog2.timeLeft = 100;
					}
				}
            }
            else
            {
				SoundEffectInstance sound2 = SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 104);
				if (sound2 != null)
					sound2.Pitch -= 0.525f;

			}

			return true;
		}

		public override void AI()
		{
			Projectile.ai[0] += 1;
			Projectile.rotation = Projectile.velocity.ToRotation()+MathHelper.PiOver4;
			if (Projectile.damage < -50)
			{
				for (int num654 = 0; num654 < 2; num654++)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
					int num655 = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(16, 16), 0, 0, DustID.AncientLight, randomcircle.X * 3f, randomcircle.Y * 3f, 150, Color.PaleTurquoise, 1f);
					Main.dust[num655].noGravity = true;
					Main.dust[num655].noLight = true;
				}
			}

		}

		public void DrawAdditive(SpriteBatch spriteBatch)
		{
			float alphatime = MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, Math.Min(Projectile.ai[0]/20f,1f));
			Texture2D texbrew = Main.projectileTexture[Projectile.type];
			Vector2 drawOrigin = new Vector2(texbrew.Width * 0.5f, texbrew.Height * 0.5f);

			for (int k = Projectile.oldPos.Length - 1; k > 0; k -= 1)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				Color color = Color.PaleTurquoise * (1f - ((float)k / ((float)Projectile.oldPos.Length)));
				spriteBatch.Draw(Main.projectileTexture[Projectile.type], drawPos, null, color * 0.75f*alphatime, Projectile.rotation, drawOrigin, Projectile.scale + 0.5f, SpriteEffects.None, 0f);
			}

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float alphatime = MathHelper.Clamp(Projectile.timeLeft / 60f, 0f, Math.Min(Projectile.ai[0] / 20f, 1f));
			Texture2D texbrew = Main.projectileTexture[Projectile.type];
			Vector2 drawOrigin = new Vector2(texbrew.Width * 0.5f, texbrew.Height * 0.5f);

			spriteBatch.Draw(texbrew, Projectile.Center - Main.screenPosition, null, Color.White*alphatime, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}

	class TheJellyBrew : ModItem,IDevItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("The Jelly Brew");
			Tooltip.SetDefault("Creates expanding Neurotoxic bubbles on impact with enemy or solid tiles\nBubbles damage enemies and deflects incoming projectiles\nExplodes into Neurotoxic fumes when another bubble is created or after 5 seconds\nStrike enemies with the bottle to mark them for the fumes to chase after");
		}

		public (string, string) DevName()
		{
			return ("JellyBru", "");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (Main.LocalPlayer.GetModPlayer<SGAPlayer>().devempowerment[2] > 0)
			{
				tooltips.Add(new TooltipLine(Mod, "DevEmpowerment", "--- Empowerment bonus ---"));
				tooltips.Add(new TooltipLine(Mod, "DevEmpowerment", "Damage is now enhanced by Magic Damage scaling"));
				tooltips.Add(new TooltipLine(Mod, "DevEmpowerment", "Neurotoxic fumes do Magic Damage instead of Throwing"));
			}
			base.ModifyTooltips(tooltips);
		}

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            if (player.SGAPly().jellybruSet)
            {
				add += player.GetDamage(DamageClass.Magic);
			}
        }

        public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.useTurn = true;
			Item.width = 8;
			Item.height = 8;
			Item.knockBack = 6;
			Item.damage = 150;
			Item.crit = 0;
			//item.UseSound = SoundID.Item1;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = false;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.shootSpeed = 14f;
			Item.mana = 75;
			Item.expert = true;
			Item.shoot = Mod.Find<ModProjectile>("TheJellyBrewProjectile").Type;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			int numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(0);
			//for (int i = 0; i < numberProjectiles; i += 1)
			//{
			Vector2 perturbedSpeed = (new Vector2(speedX, speedY)).RotatedBy(MathHelper.Lerp(-rotation, rotation, Main.rand.NextFloat()));
			int thisoned = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, Main.myPlayer);
			Main.projectile[thisoned].netUpdate = true;
			//}

			SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 106);
			if (sound != null)
				sound.Pitch -= 0.525f;

			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.PinkGel, 25).AddIngredient(ModContent.ItemType<IlluminantEssence>(), 20).AddIngredient(ItemID.ToxicFlask, 1).AddIngredient(ModContent.ItemType<CosmicFragment>(), 1).AddTile(ModContent.TileType<Tiles.TechTiles.LuminousAlter>()).Register();
		}

	}

	public class TheJellyBrewProjectile : JarateShurikensProg,IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("TheJelly Brew Projectile");
		}
		public override string Texture => "SGAmod/Projectiles/TheJellyBrewProjectile";
		int hitnpc = -1;

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.SnowBallFriendly);
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 0;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            foreach(Projectile cloud in Main.projectile.Where(testby => testby.active && testby.type == ModContent.ProjectileType<TheJellyBrewProjectileCloud>() && testby.owner == Projectile.owner))
            {
				cloud.ai[1] = target.whoAmI+1;
				/*if (cloud.velocity.Length() < 10)
                {
					cloud.velocity += Vector2.Normalize(cloud.velocity) * 10f;
				}*/
				cloud.netUpdate = true;
			}
        }

        public override bool PreKill(int timeLeft)
		{

			Projectile.NewProjectile(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<TheJellyBrewProjectileBubble>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

			SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 107);
			if (sound != null)
				sound.Pitch -= 0.525f;

			sound = SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaiveImpactGhost, (int)Projectile.Center.X, (int)Projectile.Center.Y);
			if (sound != null)
				sound.Pitch -= 0.525f;

			return true;
		}

		public override void AI()
		{
			Projectile.velocity.Y += Player.defaultGravity;

			Projectile.rotation += Projectile.velocity.X*0.05f;
			for (int num654 = 0; num654 < 2; num654++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
				int num655 = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(16, 16), 0, 0, DustID.AncientLight, randomcircle.X * 3f, randomcircle.Y * 3f, 150, Color.Lime, 1f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].noLight = true;
			}

		}

		public void DrawAdditive(SpriteBatch spriteBatch)
		{
			Texture2D texbrew = Main.projectileTexture[Projectile.type];
			Vector2 drawOrigin = new Vector2(texbrew.Width * 0.5f, texbrew.Height * 0.5f);

			for (int k = Projectile.oldPos.Length - 1; k > 0; k -= 1)
			{
				Vector2 drawPos = Projectile.oldPos[k]+(Projectile.Hitbox.Size()/2f) - Main.screenPosition;
				Color color = Color.PaleTurquoise * (1f - ((float)k / ((float)Projectile.oldPos.Length)));
				spriteBatch.Draw(Main.projectileTexture[Projectile.type], drawPos, null, color * 0.75f, Projectile.rotation, drawOrigin, Projectile.scale + 0.5f, SpriteEffects.None, 0f);
			}

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texbrew = Main.projectileTexture[Projectile.type];
			Vector2 drawOrigin = new Vector2(texbrew.Width * 0.5f, texbrew.Height * 0.5f);

			spriteBatch.Draw(texbrew, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}

	public class TheJellyBrewProjectileBubble : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("TheJelly Brew Projectile Bubble");
		}
		public override string Texture => "SGAmod/Projectiles/TheJellyBrewProjectile";
		int hitnpc = -1;

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.SnowBallFriendly);
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.width = 480;
			Projectile.height = 480;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 600;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.extraUpdates = 0;
			Projectile.penetrate = -1;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		float Scale => 0.5f+(Projectile.ai[0]/60f);

		public override bool PreKill(int timeLeft)
		{

			for (int i = 0; i < (int)(Scale * 3f) + 2; i += 1)
			{
				float velocityproj = 4 + (i / 10f);
				Vector2 velocityposspeed = Main.rand.NextVector2Circular(velocityproj, velocityproj);
				Projectile proj = Projectile.NewProjectileDirect(Projectile.Center+Vector2.Normalize(velocityposspeed)*(Scale * 24f), velocityposspeed * (Main.player[Projectile.owner].Throwing().thrownVelocity), ModContent.ProjectileType<TheJellyBrewProjectileCloud>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				if (proj != null)
                {
					proj.timeLeft = (int)(150 + (Scale*30f) + (i * 4f));
					proj.netUpdate = true;
				}
			}


			SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 112);
			if (sound != null)
				sound.Pitch -= 0.525f;

			sound = SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y,111);
			if (sound != null)
				sound.Pitch -= 0.525f;

			return true;
		}

		bool InTheCircle(Rectangle targetHitbox)
        {
			Vector2 circle = Projectile.Center;

			float closestX = MathHelper.Clamp(circle.X, targetHitbox.X, targetHitbox.X + targetHitbox.Width);
			float closestY = MathHelper.Clamp(circle.Y, targetHitbox.Y, targetHitbox.Y + targetHitbox.Height);

			// Calculate the distance between the circle's center and this closest point
			float distanceX = circle.X - closestX;
			float distanceY = circle.Y - closestY;

			// If the distance is less than the circle's radius, an intersection occurs
			float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
			return distanceSquared < (72f * 72f) * Scale;

		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			return InTheCircle(targetHitbox);
        }

        public override void AI()
		{

			if (Projectile.ai[0] < 1)
			{
				Projectile[] oldest = Main.projectile.Where(testby => testby.active && testby.type == ModContent.ProjectileType<TheJellyBrewProjectileBubble>()).OrderBy(testby => testby.timeLeft).ToArray();

				if (oldest.Length > 1)
				{
					oldest[0].timeLeft = 2;
					oldest[0].netUpdate = true;
				}
			}

			float scalle = (Scale * 24f)* (Scale * 24f);

			foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.whoAmI != Projectile.whoAmI && testby.velocity.Length() > 0.5f && (Projectile.Center - testby.Center).LengthSquared() < scalle))
			{
				if (Vector2.Dot(Vector2.Normalize(proj.velocity), Vector2.Normalize(Projectile.Center - proj.Center)) > 0)
				{
					proj.velocity = Vector2.Normalize(proj.Center-Projectile.Center).RotatedBy(Main.rand.NextFloat(-0.25f, 0.25f)) * proj.velocity.Length();
				}
			}

			Projectile.ai[0] += Main.player[Projectile.owner].SGAPly().ThrowingSpeed;

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D bubbles = SGAmod.ExtraTextures[115];
			Vector2 drawOrigin = new Vector2(bubbles.Width, bubbles.Height/2f)*0.50f;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Effect hallowed = SGAmod.HallowedEffect;

			float alphapercent = 1f;

			hallowed.Parameters["alpha"].SetValue(alphapercent);
			hallowed.Parameters["prismColor"].SetValue(Color.Magenta.ToVector3());
			hallowed.Parameters["prismAlpha"].SetValue(0f);
			hallowed.Parameters["overlayTexture"].SetValue(Mod.Assets.Request<Texture2D>("Stain").Value);
			hallowed.Parameters["overlayProgress"].SetValue(new Vector3(Main.GlobalTimeWrappedHourly / 5f, Main.GlobalTimeWrappedHourly / 3f, 0f));
			hallowed.Parameters["overlayAlpha"].SetValue(0.25f);
			hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2f, 0.10f, Main.GlobalTimeWrappedHourly / 4f));
			hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
			hallowed.Parameters["rainbowScale"].SetValue(0.8f);
			hallowed.Parameters["overlayScale"].SetValue(new Vector2(1f, 1f));

			hallowed.CurrentTechnique.Passes["Prism"].Apply();


			//Texture2D bubbles = ModContent.GetTexture("Terraria/NPC_" + NPCID.DetonatingBubble);
			//Vector2 half = new Vector2(bubbles.Width, bubbles.Height / 2f) / 2f;

			spriteBatch.Draw(bubbles, Projectile.Center - Main.screenPosition, new Rectangle(0, bubbles.Height / 2, bubbles.Width, bubbles.Height / 2), Color.LightPink * alphapercent, -(float)Math.Sin(Main.GlobalTimeWrappedHourly) / 4f, drawOrigin, (Vector2.One * Scale) + (new Vector2((float)Math.Sin(Main.GlobalTimeWrappedHourly), (float)Math.Cos(Main.GlobalTimeWrappedHourly))) * 0.25f, SpriteEffects.None, 0f);

			spriteBatch.Draw(bubbles, Projectile.Center - Main.screenPosition, new Rectangle(0, bubbles.Height / 2, bubbles.Width, bubbles.Height / 2), Color.LightPink * alphapercent, (float)Math.Sin(Main.GlobalTimeWrappedHourly) / 4f, drawOrigin, (Vector2.One * Scale) + (new Vector2((float)Math.Cos(Main.GlobalTimeWrappedHourly + MathHelper.PiOver2), (float)Math.Sin(Main.GlobalTimeWrappedHourly - MathHelper.PiOver2))) * 0.25f, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			//spriteBatch.Draw(texbrew, projectile.Center - Main.screenPosition, null, Color.GreenYellow, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}

	public class TheJellyBrewProjectileCloud : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(" Neurotoxic Fume");
		}
		public override string Texture => "Terraria/Projectile_"+ProjectileID.SporeCloud;
		int hitnpc = -1;

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.SnowBallFriendly);
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 150;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.extraUpdates = 0;
			Projectile.penetrate = -1;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			return true;
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			if (owner != null && owner.active && !owner.dead && owner.SGAPly().jellybruSet)
            {
				// projectile.Throwing().thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
				Projectile.DamageType = DamageClass.Magic;
			}

			if (Projectile.timeLeft < 30)
				Projectile.velocity *= 0.90f;

			if (Projectile.velocity.Length() > 1f)
			{
				if (Projectile.ai[1] < 1)
				{
					/*List<NPC> target = SGAUtils.ClosestEnemies(projectile.Center, 600);

					if (target != null && target.Count > 1)
					{
						NPC mytarget = target[1];
						projectile.ai[1] = mytarget.whoAmI + 1;
					}*/
                }
                else
                {
					NPC mytarget = Main.npc[(int)Projectile.ai[1]-1];
					if (mytarget.active && mytarget.chaseable)
					{
						Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards((mytarget.Center - Projectile.Center).ToRotation(), 0.05f).ToRotationVector2() * Projectile.velocity.Length();
                    }
                    else
                    {
						Projectile.ai[1] = 0;
					}
				}

			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D cloudtex = Main.projectileTexture[Projectile.type];
			Vector2 drawOrigin = new Vector2(cloudtex.Width, cloudtex.Height/5f) * 0.50f;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Effect hallowed = SGAmod.HallowedEffect;

			float alphapercent = MathHelper.Clamp(Projectile.timeLeft/80f,0f,1f);

			hallowed.Parameters["alpha"].SetValue(alphapercent);
			hallowed.Parameters["prismColor"].SetValue(Color.PaleTurquoise.ToVector3());
			hallowed.Parameters["prismAlpha"].SetValue(1f);
			hallowed.Parameters["overlayTexture"].SetValue(Mod.Assets.Request<Texture2D>("Stain").Value);
			hallowed.Parameters["overlayProgress"].SetValue(new Vector3(Main.GlobalTimeWrappedHourly / 5f, Main.GlobalTimeWrappedHourly / 3f, 0f));
			hallowed.Parameters["overlayAlpha"].SetValue(0.65f);
			hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2f, 0.10f, Main.GlobalTimeWrappedHourly / 4f));
			hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
			hallowed.Parameters["rainbowScale"].SetValue(0.8f);
			hallowed.Parameters["overlayScale"].SetValue(new Vector2(1f, 1f));

			hallowed.CurrentTechnique.Passes["Prism"].Apply();

			spriteBatch.Draw(cloudtex, Projectile.Center - Main.screenPosition, new Rectangle(0, (int)((Projectile.timeLeft/8f)%5)*(cloudtex.Height / 5), cloudtex.Width, cloudtex.Height / 5), Color.LightPink * alphapercent, 0, drawOrigin, (Vector2.One * 1f), SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			//spriteBatch.Draw(texbrew, projectile.Center - Main.screenPosition, null, Color.GreenYellow, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}

	class SharkBait : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Shark Bait");
			Tooltip.SetDefault("'Contains many yummy snacks a Sharkvern needs in their diet!'\nThrows a bucket of Shark Bait, which erupts into fish\nFish may spawn hungry sharks\nProduces more fish when thrown into water\nDoubles as fishing bait");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.MolotovCocktail);
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			// item.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Item.damage = 50;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.bait = 60;
			Item.maxStack = 999;
			Item.shootSpeed = 8f;
			Item.shoot = Mod.Find<ModProjectile>("SharkFoodProj").Type;
			Item.value = Item.buyPrice(0, 0, 5, 0);
			Item.rare = 6;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			return true;
		}

	}

	class SharkFoodProj : ModProjectile
	{

		bool hitonce = false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Shark Food");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Grenade);
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.timeLeft = 600;
			Projectile.aiStyle = -1;
			Projectile.width = 24;
			Projectile.height = 24;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/SharkBait"); }
		}

		public override bool? CanHitNPC(NPC target)
		{
			int player = Projectile.owner;
			return base.CanHitNPC(target);
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}

		public override bool PreKill(int timeLeft)
		{

			int theproj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("SharkFoodProj3").Type,Projectile.damage, timeLeft>8000 ? 2 : 1, Projectile.owner, 0f, 0f);
			return true;
		}

		public override void AI()
		{
			Tile tile = Main.tile[(int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16];
			if (tile != null)
			{
				if (tile.liquid > 64)
				{
					Projectile.timeLeft = 9999;
					Projectile.Kill();
				}
			}

			int[] dustype = {DustID.Blood};

			int dust = Dust.NewDust(new Vector2(Projectile.Center.X-16, Projectile.Center.Y-16), 32, 32, dustype[Main.rand.Next(0, dustype.Length)]);
			Main.dust[dust].scale = 0.75f;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);

			Projectile.timeLeft -= 1;
			Projectile.velocity.Y += 0.25f;
			Projectile.velocity.X *= 0.98f;
			Projectile.rotation += Projectile.velocity.Length() * (float)(Math.Sign(Projectile.velocity.X))*0.01f;
		}

	}

	public class SharkFoodProj3 : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yum Yum!");
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.HerbBag); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 72;
			Projectile.height = 24;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.extraUpdates = 0;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.aiStyle = -1;

		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 30)
			{
				for (int i = 0; i < 20; i += 1)
				{
					int dust2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33);
					Main.dust[dust2].scale = 2.5f;
					Main.dust[dust2].noGravity = false;
					Main.dust[dust2].velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-12f, 1f));
				}
				for (float xx = 5f; xx < 20f; xx += Projectile.knockBack>1 ? 0.85f : 1.75f)
				{
					int proj2 = Projectile.NewProjectile(Projectile.Center, new Vector2(Main.rand.NextFloat(-5f, 5f), -Main.rand.NextFloat(0, xx)), Mod.Find<ModProjectile>("SharkFoodProj2").Type, Projectile.damage, 0f,Projectile.owner,255);
					Main.projectile[proj2].friendly = false;
					Main.projectile[proj2].hostile = true;
					Main.projectile[proj2].ai[0] = Main.rand.Next(40, 100);
					Main.projectile[proj2].netUpdate = true;
				}
				SoundEngine.PlaySound(19, (int)Projectile.position.X, (int)Projectile.position.Y, 1, 1f, 0.25f);
			}

			for (int i = 0; i < 3; i += 1)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-1f, 1f));
			}
		}

	}


	public class SharkFoodProj2 : RandomOceanCrap
	{

		int fakeid = ProjectileID.FrostShard;
		int fishtype = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Yum Yum!");
		}

		public override bool CanDamage()
		{
			return Projectile.velocity.Y > -3f && Projectile.ai[1]>2;
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.velocity.Y <= -3f)
				return false;
			return base.CanHitNPC(target);
		}

		public override void AI()
		{
			base.AI();
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.ai[1] += 1;
			if (Projectile.ai[1] > Projectile.ai[0])
			{
				Projectile.ai[1] = -999;
				int proj2 = Projectile.NewProjectile(Projectile.Center+ new Vector2(Math.Sign(Projectile.velocity.X) * 150,42), new Vector2(-Math.Sign(Projectile.velocity.X)*12,-2.25f), Mod.Find<ModProjectile>("FlyingSharkProj").Type, Projectile.damage, 4, Projectile.owner);
			}
			if (Projectile.velocity.Y > 0)
				Projectile.tileCollide = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.CloneDefaults(fakeid);
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.extraUpdates = 0;
			Projectile.tileCollide = false;
			Projectile.aiStyle = -1;

			int[] types = { ItemID.Fish, ItemID.Trout, ItemID.TundraTrout, ItemID.ReaverShark, ItemID.Goldfish, ItemID.Ebonkoi, ItemID.MirageFish, ItemID.PrincessFish, ItemID.FrostDaggerfish };
			fishtype = types[Main.rand.Next(types.Length)];
		}

	}

	public class FlyingSharkProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shark");
		}

		public override string Texture
		{
			get { return ("Terraria/NPC_" + NPCID.Shark); }
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.WaterBolt);
			Projectile.extraUpdates = 0;
			Projectile.width = 72;
			Projectile.height = 42;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 180;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.scale = 1f;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 33);
			Main.dust[dust].scale = 2f;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-1f, 1f));

			Projectile.localAI[0] += 1;
			Projectile.velocity.Y += 0.1f;
			if (Collision.SolidCollision(Projectile.position,Projectile.width,Projectile.height))
				Projectile.timeLeft = Math.Min(Projectile.timeLeft, 30);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.npcTexture[ModContent.NPCType<SharvernMinion>()];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
			Color color = Projectile.GetAlpha(lightColor) * 1f; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = (int)(Projectile.localAI[0] / 8f);
			timing %= 4;
			timing *= ((tex.Height) / 4);
			float yspeed = Projectile.velocity.Y;
			if (Math.Abs(Projectile.velocity.Y) > 2f)
			{
				yspeed = (Math.Sign(yspeed) * 2f) + yspeed / 5f;
			}
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 4), 
				color*MathHelper.Clamp((float)Projectile.timeLeft/30f,0f,(float)Math.Min(Projectile.localAI[0]/15f,1f)),yspeed * (0.15f* Math.Sign(Projectile.velocity.X))
				, drawOrigin, Projectile.scale, Projectile.velocity.X<1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			return false;
		}

	}

	class NinjaBombProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Ninja Bomb");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.SmokeBomb);
			Projectile.DamageType = DamageClass.Throwing;
			Projectile.timeLeft = 200;
			Projectile.damage = 0;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_"+ProjectileID.SmokeBomb); }
		}

		public override bool PreKill(int timeLeft)
		{
			for (float i = 0f; i < 12f; i += 0.1f)
			{
				Vector2 circle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
				circle=circle.SafeNormalize(Vector2.Zero);
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke, circle.X*i, circle.Y * i);
				Main.dust[dust].scale = Main.rand.NextFloat(2f,6f);
				Main.dust[dust].noGravity = false;
				Main.dust[dust].alpha = 100;
				Main.dust[dust].velocity = circle * i;
			}

			for (int num172 = 0; num172 < Main.maxNPCs; num172 += 1)
			{
				NPC target = Main.npc[num172];
				float damagefalloff = 1f - ((target.Center - Projectile.Center).Length() / 320f);
				if ((target.Center - Projectile.Center).Length() < 320f && !target.friendly && !target.dontTakeDamage)// && ((target.ModNPC!=null && target.ModNPC.CanBeHitByProjectile(projectile)==true) || target.ModNPC==null))
				{
					Player owner = Main.player[Projectile.owner];
					//if (owner.active)
					//owner.ApplyDamageToNPC(target, (int)(projectile.damage * damagefalloff), 0f, 1, false);
					float damazz = (Main.DamageVar((float)2000) * damagefalloff);
					target.AddBuff(Mod.Find<ModBuff>("NinjaSmokedDebuff").Type,(int)damazz);
					if (Main.player[Projectile.owner].SGAPly().ninjaSash > 2)
						IdgNPC.AddBuffBypass(target.whoAmI,Mod.Find<ModBuff>("NinjaSmokedDebuff").Type, (int)damazz);
				}
			}

			foreach(Player player in Main.player.Where(testby => testby.active && (testby.Center-Projectile.Center).Length()<200 && Main.player[Projectile.owner].IsAlliedPlayer(testby)))
            {
				player.NinjaDodge();
			}

			return true;
		}

		public override void AI()
		{

			int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Smoke);
			Main.dust[dust].scale = 2f;
			Main.dust[dust].alpha = 100;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			Projectile.timeLeft -= 1;

			for (int num172 = 0; num172 < Main.maxNPCs; num172 += 1)
			{
				NPC target = Main.npc[num172];
				float damagefalloff = 1f - ((target.Center - Projectile.Center).Length() / 80f);
				if ((target.Center - Projectile.Center).Length() < 80f && !target.friendly && !target.dontTakeDamage)
				{
					Player owner = Main.player[Projectile.owner];
					//if (owner.active)
					//owner.ApplyDamageToNPC(target, (int)(projectile.damage * damagefalloff), 0f, 1, false);
					float damazz = (Main.DamageVar((float)500) * damagefalloff);
					target.AddBuff(Mod.Find<ModBuff>("NinjaSmokedDebuff").Type, (int)damazz);
					if (Main.player[Projectile.owner].SGAPly().ninjaSash > 2)
						IdgNPC.AddBuffBypass(target.whoAmI, Mod.Find<ModBuff>("NinjaSmokedDebuff").Type, (int)damazz);
				}
			}

		}

	}
	class RockProj : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Rock");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Grenade);
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.timeLeft = 300;
			Projectile.aiStyle = -1;
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.penetrate = 1;
		}

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/RockProj"); }
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			for (int i = 0; i < 8; i += 1)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 8, 8, 1);
				Main.dust[dust].scale = 0.75f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(2f, 2f);
			}
			SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Tink, (int)Projectile.Center.X, (int)Projectile.Center.Y);
				if (sound!=null)
				sound.Pitch -= 0.525f;

			//			SoundEffectInstance sound = Main.PlaySound(SoundID.Tink, (int)projectile.Center.X, (int)projectile.Center.Y);
			//if (sound!=null)
			//	sound.Pitch -= 0.525f;
			return true;
		}

		public override void AI()
		{
			Projectile.velocity.Y += 0.30f;
			Projectile.rotation += Projectile.velocity.Length() * (float)(Math.Sign(Projectile.velocity.X * 1000f) / 1000f) * 10f;
		}

	}


}

namespace SGAmod.HavocGear.Items.Weapons
{


	class SwarmGrenade : AcidGrenade
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Gnat-ade");
			Tooltip.SetDefault("Explodes into a swarm of seeking flies");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.Grenade);
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			// item.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Item.damage = 35;
			Item.shootSpeed = 4f;
			Item.shoot = Mod.Find<ModProjectile>("SwarmGrenadeProj").Type;
			Item.value = Item.buyPrice(0, 0, 5, 0);
			Item.rare = 5;
		}

        public override void AddRecipes()
        {
            //Nil
        }
    }


	class SwarmGrenadeProj : AcidGrenadeProj
	{

		bool hitonce = false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Swarm Grenade");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Grenade);
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.timeLeft = 320;
		}

		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Items/Weapons/SwarmGrenade"); }
		}

		public override bool PreKill(int timeLeft)
		{
			if (!hitonce)
			{
				Projectile.width = 160;
				Projectile.height = 160;
				Projectile.position -= new Vector2(80, 80);
			}

			for (int i = 0; i < 100; i += 1)
			{
				float randomfloat = Main.rand.NextFloat(1f, 6f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();

				int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 32, Projectile.Center.Y - 32), 64, 64, DustID.Buggy);
				Main.dust[dust].scale = 0.25f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (Projectile.velocity * (float)(Main.rand.Next(10, 20) * 0.01f)) + (randomcircle * randomfloat);
			}

			for(int i = 0; i < 8; i += 1)
            {
				double angle = (i / 8.0) * (Math.PI * 2.0);
				Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 4f;
				Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, velocity.X, velocity.Y, ModContent.ProjectileType<FlyProjectileThrown>(), (int)((double)Projectile.damage), Projectile.knockBack/5f, Projectile.owner, 0f, 0f);
			}

			int theproj = Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("Explosion").Type, (int)((double)Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, 0f, 0f);
			Main.projectile[theproj].Throwing().DamageType = DamageClass.Throwing;
			Main.projectile[theproj].netUpdate = true;
			Projectile.type = ProjectileID.Grenade;
			return true;
		}

		public override void AI()
		{
			int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Buggy);
			Main.dust[dust].scale = 0.35f;
			Main.dust[dust].noGravity = false;
			Main.dust[dust].velocity = Projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!hitonce)
			{
				hitonce = true;
				Projectile.position -= new Vector2(64, 64);
				Projectile.width = 128;
				Projectile.height = 128;
				Projectile.timeLeft = 1;
			}
		}

	}

	public class FlyProjectileThrown : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Fly");
			//ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
		}

		public override string Texture => "SGAmod/NPCs/Dank/Fly";

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 5;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 8;
		}

        public override bool CanDamage()
        {
            return Projectile.penetrate>1;
        }

        public override void AI()
		{

			if (Projectile.timeLeft > 30 && Projectile.penetrate < 2)
				Projectile.timeLeft = 30;

			Player player = Main.player[base.Projectile.owner];
			Projectile.localAI[0] += 1;


			if (Projectile.localAI[0] % 20 == 0)
			{

				List<NPC> closestnpcs = SGAUtils.ClosestEnemies(Projectile.Center, 300f);

				NPC target = closestnpcs?[0];//Closest

				//projectile.ai[0] = 0;

				if (target != null && target.active && target.life > 0 && Vector2.Distance(target.Center, Projectile.Center) < 600)
				{
					Projectile.ai[0] = target.whoAmI+1;
						Projectile.netUpdate = true;
				}
			}

			if ((Projectile.localAI[0]+(Projectile.whoAmI*7)) % 60 == 0 || (int)Projectile.ai[1] == 0)
			{
				float left = (0.40f + (1f-(Projectile.timeLeft/300f))*0.20f)*(GetType() == typeof(FlyProjectileThrown) ? 1f : 0.60f);
				Projectile.ai[1] = Main.rand.NextFloat(-MathHelper.Pi * left, MathHelper.Pi * left);
				Projectile.netUpdate = true;
			}

			if (Projectile.ai[0] > 0)
            {
				NPC target = Main.npc[(int)Projectile.ai[0]-1];
				if (target != null && target.active && target.life > 0)
                {
					Vector2 difference = target.Center - Projectile.Center;

					Projectile.velocity += Vector2.Normalize(difference).RotatedBy(Projectile.ai[1])/3f;
					Projectile.velocity /= 1.03f;

					if (Projectile.velocity.Length() > 16)
                    {
						Projectile.velocity.Normalize(); Projectile.velocity *= 16f;
					}



				}

			}




		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texa = Main.projectileTexture[Projectile.type];
			int frame = texa.Height / 4;
			float alpha = MathHelper.Clamp(Projectile.localAI[0] / 10f, 0f, Math.Min(Projectile.timeLeft/30f,1f));
			spriteBatch.Draw(texa, Projectile.Center - Main.screenPosition, new Rectangle(0,((int)((Projectile.localAI[0] /3f)% 4)) * frame, texa.Width, frame), lightColor * alpha, Projectile.rotation, new Vector2(texa.Width, frame) / 2f, Projectile.scale, Projectile.velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{

			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}

			Projectile.velocity /= 1.25f;
			return false;
		}
	}

}
