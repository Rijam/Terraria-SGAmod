using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace SGAmod.Items.Tools
{
	public class Braxsaw : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Braxsaw");
			Tooltip.SetDefault("Mine through the oldest fabric of the universe!");
		}

		public override void SetDefaults()
		{
			Item.damage = 150;
			Item.DamageType = DamageClass.Melee;
			Item.width = 56;
			Item.height = 22;
			Item.useTime = 1;
			Item.useAnimation = 18;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.pick = 250;
			Item.axe = 150;
			Item.tileBoost += 5;
			Item.useStyle = 5;
			Item.knockBack = 5;
			Item.value = 3000000;
			Item.rare = 11;
			Item.UseSound = SoundID.Item23;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("BraxsawProj").Type;
			/*
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/Braxsaw_Glow");
				item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Color.White * (0.75f+MathHelper.Clamp((float)(Math.Sin(Main.GlobalTimeWrappedHourly)*0.35f)+0.75f,0f,1f)*0.25f);
				};
			}
			*/
			Item.shootSpeed = 40f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<StarMetalBar>(), 32).AddIngredient(ModContent.ItemType<LunarRoyalGel>(), 10).AddIngredient(ItemID.Drax, 1).AddIngredient(ModContent.ItemType <BoreicDrill>(), 1).AddIngredient(ModContent.ItemType <HavocGear.Items.Tools.VirulentDrill>(), 1).AddTile(TileID.LunarCraftingStation).Register();
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
        float vel = Item.velocity.X / 6f;
			//new Vector2(0, Main.itemTexture[item.type].Height - 4)
			Vector2 drawPos = (Item.Center) - Main.screenPosition;

			Color glowColor = Color.White * (0.60f + MathHelper.Clamp((float)(Math.Sin(Main.GlobalTimeWrappedHourly) * 0.65f) + 0.50f, 0f, 1f) * 0.40f);

			Texture2D glow = Mod.Assets.Request<Texture2D>("Items/GlowMasks/Braxsaw/Braxsaw_Glow").Value;

			spriteBatch.Draw(glow, drawPos, null, glowColor, vel, Main.itemTexture[Item.type].Size() / 2f,scale, SpriteEffects.None, 0f);
		}
	}

	public class BraxsawProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boreic Drill");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Tools/BraxsawProj"); }
		}

		public override void SetDefaults()
		{
			int[] itta = { ProjectileID.SolarFlareDrill, ProjectileID.NebulaDrill , ProjectileID.StardustDrill , ProjectileID.VortexDrill };
			Projectile.CloneDefaults(itta[Main.rand.Next(0, itta.Length)]);
			Projectile.glowMask = 0;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Vector2 drawPos = (Projectile.Center) - Main.screenPosition;

			Texture2D glow = Mod.Assets.Request<Texture2D>("Items/GlowMasks/Braxsaw/BraxsawProj_Glow").Value;
			Texture2D itemtex = Main.projectileTexture[Projectile.type];

			Color glowColor = Color.White * (0.60f + MathHelper.Clamp((float)(Math.Sin(Main.GlobalTimeWrappedHourly) * 0.65f) + 0.50f, 0f, 1f) * 0.40f);

			SpriteEffects effect = Projectile.rotation.ToRotationVector2().Y < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Vector2 offset = new Vector2(glow.Width / 2, (int)(glow.Height * 0.35f));

			spriteBatch.Draw(itemtex, drawPos, null, lightColor, Projectile.rotation, offset, Projectile.scale, effect, 0f);



			List<(Texture2D, float)> glowTexts = new List<(Texture2D, float)>();
			for (int i = 0; i < 4; i += 1)
			{
				float percent = (i / 4f) * MathHelper.TwoPi;
				float fadeInAndOut = Math.Max(0, 0.35f + ((float)(Math.Sin((-Main.GlobalTimeWrappedHourly*1.5f) + percent) * 0.50f)));
				glowTexts.Add((Mod.Assets.Request<Texture2D>("Items/GlowMasks/Braxsaw/BraxsawProj_Glow" + (i + 1)).Value, fadeInAndOut));
			}
			int index = 0;
			foreach ((Texture2D, float) tex in glowTexts.OrderBy(testby => 10f-testby.Item2))
			{
				Color glowColor2 = Color.White * tex.Item2;
				spriteBatch.Draw(tex.Item1, drawPos, null, glowColor2, Projectile.rotation, offset, Projectile.scale, effect, 0f);
				index += 1;
			}


			spriteBatch.Draw(glow, drawPos, null, glowColor, Projectile.rotation, offset, Projectile.scale, effect, 0f);
			return false;
		}

    }

}