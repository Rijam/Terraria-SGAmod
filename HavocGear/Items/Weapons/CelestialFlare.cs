using System.Collections.Generic;
using Idglibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class CelestialFlare : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Flare");
			Tooltip.SetDefault("Engulfs enemies in a devastating inferno\nSends an Explosion through the enemy on hit\nCounts as a True Melee sword");
		}
		
		public override void SetDefaults()
		{
			Item.damage = 240;
			Item.DamageType = DamageClass.Melee;
			Item.width = 44;
			Item.height = 52;
			Item.useTime = 30;
			Item.useAnimation = 12;
			Item.useStyle = 1;
			Item.knockBack = 10;
			Item.value = 500000;
			Item.rare = 10;
	        Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = false;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/CelestialFlare_Glow").Value;
			}
		}

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(mod.ItemType("Mudmore"), 1).AddIngredient(mod.ItemType("ThermalBlade"), 1).AddIngredient(ItemID.FragmentSolar, 8).AddIngredient(mod.ItemType("StarMetalBar"), 12).AddIngredient(mod.ItemType("IlluminantEssence"), 20).AddIngredient(ItemID.SoulofLight, 6).AddTile(TileID.LunarCraftingStation).Register();

		}

	public override void MeleeEffects(Player player, Rectangle hitbox)
	{

		for (int num475 = 0; num475 < 3; num475++)
		{
		int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, Mod.Find<ModDust>("HotDust").Type);
		Main.dust[dust].scale=0.5f+(((float)num475)/3.5f);
		Main.dust[dust].velocity=new Vector2(0f,-5f);
		Main.dust[dust].velocity.Normalize();
		Main.dust[dust].velocity+=new Vector2(player.velocity.X/4,0f);
		Main.dust[dust].velocity*=(((float)Main.rand.Next(0,100))/30f);
		}
		Lighting.AddLight(player.position, 0.9f, 0.9f, 0f);
	}
	
	public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
	{

		if (!(Main.rand.Next(5) == 0))
		{
			target.AddBuff(Mod.Find<ModBuff>("ThermalBlaze").Type, 600, false);
			target.AddBuff(BuffID.Daybreak, 600, true);
			target.AddBuff(BuffID.OnFire, 600, true);
		}
			Projectile.NewProjectile(target.Center, Vector2.Normalize(target.Center - player.Center) * 12f, ProjectileID.SolarWhipSwordExplosion, (int)(damage*0.4), knockback/5f,player.whoAmI,0f, 0.85f + Main.rand.NextFloat() * 1.15f);
	}
    }
}