using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons.Ammo
{
	public class BlazeBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blaze Bullet");
			Tooltip.SetDefault("May inflict Thermal Blaze on enemies");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/BlazeBullet"); }
		}
		public override void SetDefaults()
		{
			Item.damage = 13;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 1.5f;
			Item.value = 25;
			Item.rare = 5;
			Item.shoot = ModContent.ProjectileType <Projectiles.BlazeBullet>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 5f;                  //The speed of the projectile
			Item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(mod.ItemType("WraithFragment4"), 2).AddIngredient(mod.ItemType("FieryShard"), 1).AddIngredient(ItemID.MusketBall, 50).AddTile(TileID.MythrilAnvil).Register();
		}
	}

	public class AcidBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Bullet");
			Tooltip.SetDefault("High chance of inflicting Acid Burn\nAcid Burn does more damage the more defense the enemy has, and reduces their defense by 5\nAcid Rounds quickly melt away after being fired and do not go far");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/AcidBullet"); }
		}
		public override void SetDefaults()
		{
			Item.damage = 8;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 1.5f;
			Item.value = 25;
			Item.rare = 5;
			Item.shoot = ModContent.ProjectileType<Projectiles.AcidBullet>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 2.5f;                  //The speed of the projectile
			Item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(mod.ItemType("VialofAcid"), 1).AddIngredient(ItemID.MusketBall, 50).AddTile(TileID.Anvils).Register();
		}
	}

	public class NoviteBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Bullet");
			Tooltip.SetDefault("Redirects when near enemies, but only once\nCosts 25 electric charge to change direction");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/NoviteBullet"); }
		}
		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 0f;
			Item.value = 20;
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<Projectiles.NoviteBullet>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 4.5f;                  //The speed of the projectile
			Item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(ModContent.ItemType<NoviteBar>(), 1).AddIngredient(ItemID.MusketBall, 50).AddTile(TileID.Anvils).Register();
		}
	}

	public class SeekerBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seeker Bullet");
			Tooltip.SetDefault("Does summon damage\nHomes in on the minion focused enemy\nThis includes enemies who otherwise can't be chased");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/SeekerBullet"); }
		}
        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
			if (GetType() == typeof(SeekerBullet))
			flat += Item.damage * player.GetDamage(DamageClass.Summon);
		}
        public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Summon;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.knockBack = 0f;
			Item.value = 20;
			Item.rare = ItemRarityID.LightPurple;
			Item.shoot = ModContent.ProjectileType<Projectiles.SeekerBullet>();
			Item.shootSpeed = 6f;
			Item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			CreateRecipe(250).AddIngredient(ItemID.AncientBattleArmorMaterial, 1).AddIngredient(ItemID.MusketBall, 250).AddTile(TileID.Anvils).Register();
		}
	}

	public class SoulboundBullet : SeekerBullet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soulbound Bullet");
			Tooltip.SetDefault("A bullet bound with souls, Does summon damage\nHitting an enemy focuses them for your minions to attack\nIf below 20% health, you will leech small amounts of life on hit\n"+Idglibrary.Idglib.ColorText(Color.Red,"Suffer self-damage when you miss and hit a tile"));
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/SoulboundBullet"); }
		}
		public override void SetDefaults()
		{
			Item.damage = 75;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.knockBack = 0f;
			Item.value = 20;
			Item.rare = ItemRarityID.Lime;
			Item.shoot = ModContent.ProjectileType<Projectiles.SoundboundBullet>();
			Item.shootSpeed = 6f;
			Item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			CreateRecipe(50).AddIngredient(ItemID.Ectoplasm, 1).AddIngredient(ItemID.MusketBall, 50).AddTile(TileID.Anvils).Register();
		}
	}

	public class TungstenBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tungsten Bullet");
			Tooltip.SetDefault("Isn't slowed down in water");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/TungstenBullet"); }
		}
		public override void SetDefaults()
		{
			Item.damage = 8;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 1.5f;
			Item.value = 10;
			Item.rare = 1;
			Item.shoot = ModContent.ProjectileType<Projectiles.TungstenBullet>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 4.5f;                  //The speed of the projectile
			Item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			CreateRecipe(70).AddIngredient(ItemID.TungstenBar, 1).AddIngredient(ItemID.MusketBall, 70).AddTile(TileID.Anvils).Register();
		}
	}

	public class AimBotBullet : ModItem, IHellionDrop
	{
		int IHellionDrop.HellionDropAmmount() => 999;
		int IHellionDrop.HellionDropType() => ModContent.ItemType<AimBotBullet>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aim-Bot Bullet");
			Tooltip.SetDefault("Adjusts your aim to target the scrub nearest your mouse cursor; bullet travels instantly\nAimbot bullets can pierce 2 targets ending on the 3rd, does not cause immunity frames\nBullets do 20% increased damage after each hit they pass through\n'GIT GUD, GET LMAOBOX!'\n(disclaimer, does not function in pvp)");
		}

		public override void SetDefaults()
		{
			Item.damage = 20;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 2.0f;
			Item.value = 300;
			Item.rare = 10;
			Item.shoot = ModContent.ProjectileType <Projectiles.AimBotBullet>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 1f;                  //The speed of the projectile
			Item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			CreateRecipe(250).AddIngredient(ModContent.ItemType<Entrophite>(), 20).AddIngredient(ModContent.ItemType<MoneySign>(), 1).AddIngredient(ModContent.ItemType<ByteSoul>(), 5).AddIngredient(ModContent.ItemType<DrakeniteBar>(), 1).AddIngredient(ItemID.MoonlordBullet, 100).AddIngredient(ItemID.MeteorShot, 75).AddIngredient(ItemID.ChlorophyteBullet, 75).AddTile(TileID.LunarCraftingStation).Register();
		}
	}
	public class PortalBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Portal Bullet");
			Tooltip.SetDefault("Portals appear at the mouse cursor which summon high velocity bullets to fly at the nearby enemies");
		}
		public override string Texture
		{
			get { return ("Terraria/Item_"+ItemID.HighVelocityBullet); }
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Texture2D inner = SGAmod.ExtraTextures[92];

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			spriteBatch.Draw(inner, position+ (new Vector2(4f,8f)*scale), null, drawColor, Main.GlobalTimeWrappedHourly, new Vector2(inner.Width / 2, inner.Height / 2), scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(Main.itemTexture[Item.type], position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			return false;
		}

		public override void SetDefaults()
		{
			Item.damage = 19;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 1.5f;
			Item.value = 100;
			Item.rare = 9;
			Item.shoot = ModContent.ProjectileType<Projectiles.PortalBullet>();   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 4.5f;                  //The speed of the projectile
			Item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			CreateRecipe(100).AddIngredient(mod.ItemType("StarMetalBar"), 1).AddIngredient(mod.ItemType("PlasmaCell"), 1).AddIngredient(ItemID.HighVelocityBullet, 100).AddTile(TileID.LunarCraftingStation).Register();
		}
	}


	public class PrismicBullet : ModItem
	{
		public virtual int maxvalue => 2;
		public int ammotype;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lesser Prismic Bullet");
			Tooltip.SetDefault("Shots cycle through your 2nd and 3rd ammo slots while placed in your first\nDefaults to a weak musket ball\nHas a 66% to not consume the fired ammo type");
		}
		public override string Texture
		{
			get { return ("Terraria/Item_"+ItemID.SilverBullet); }
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.IntenseRainbowDye);
			shader.UseOpacity(0.5f);
			shader.UseSaturation(0.25f);
			shader.Apply(null);
			spriteBatch.Draw(Main.itemTexture[Item.type], position, frame, drawColor,0, origin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			return false;
		}
		public override void SetDefaults()
		{
			Item.damage = 2;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 1.5f;
			Item.value = 25;
			Item.rare = 5;
			Item.shoot = ProjectileID.Bullet;   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 4.5f;                  //The speed of the projectile
			Item.ammo = AmmoID.Bullet;
		}
		public virtual int GetAmmo(Player player,out int weapontype,bool previous = false)
		{
			bool canuse = true;
			weapontype = Item.type;
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();
			Item newitem = new Item();
			newitem.SetDefaults(sgaplayer.ammoinboxes[sgaplayer.PrismalShots + (1)]);

			if (sgaplayer.ammoinboxes[1] == 0 || sgaplayer.ammoinboxes[1] == Item.type)
				canuse = false;
			if (sgaplayer.ammoinboxes[2] == 0 || sgaplayer.ammoinboxes[2] == Item.type)
				canuse = false;
			if ((sgaplayer.ammoinboxes[3] == 0 || sgaplayer.ammoinboxes[3] == Item.type) && maxvalue>2)
				canuse = false;
			if (newitem.ammo != Item.ammo)
				canuse = false;

			if (canuse)
			{
				if (!previous)
				{
					sgaplayer.PrismalShots += 1;
					sgaplayer.PrismalShots %= maxvalue;
				}
				ammotype = newitem.type;
				return newitem.shoot;
			}
			else
			{
				return Item.shoot;
			}

		}

		public override void OnConsumeAmmo(Player player)
		{
			if (ammotype!=Item.type && Main.rand.Next(maxvalue+1)<1)
			player.ConsumeItemRespectInfiniteAmmoTypes(ammotype);
		}

		public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
		{
			int nothing = 0;
		type = GetAmmo(player,out nothing);
		}

		public override void AddRecipes()
		{
			CreateRecipe(250).AddRecipeGroup("SGAmod:Tier1Bars", 1).AddRecipeGroup("SGAmod:Tier2Bars", 1).AddRecipeGroup("SGAmod:Tier4Bars", 1).AddIngredient(mod.ItemType("WraithFragment3"), 2).AddIngredient(ItemID.HallowedBar, 1).AddIngredient(ItemID.SilverBullet, 125).AddIngredient(ModContent.ItemType<TungstenBullet>(), 125).AddTile(TileID.ImbuingStation).Register();
		}
	}

	public class PrismalBullet : PrismicBullet
	{
		public override int maxvalue => 3;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Bullet");
			Tooltip.SetDefault("Highly increased damage over its precursor\nCycles through all your ammo slots when placed in your first; defaults to Musket Balls\nHas a 75% to not consume the fired ammo type");
		}
		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.SilverBullet); }
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.LivingRainbowDye);
			shader.Apply(null);
			spriteBatch.Draw(Main.itemTexture[Item.type], position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			return false;
		}
		public override void SetDefaults()
		{
			Item.damage = 17;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = 999;
			Item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			Item.knockBack = 1.5f;
			Item.value = 25;
			Item.rare = 10;
			Item.shoot = ProjectileID.Bullet;   //The projectile shoot when your weapon using this ammo
			Item.shootSpeed = 4.5f;                  //The speed of the projectile
			Item.ammo = AmmoID.Bullet;
		}

		public override void AddRecipes()
		{
			CreateRecipe(250).AddIngredient(mod.ItemType("PrismalBar"), 1).AddIngredient(mod.ItemType("PrismicBullet"), 250).AddTile(TileID.ImbuingStation).Register();
		}
	}


}
