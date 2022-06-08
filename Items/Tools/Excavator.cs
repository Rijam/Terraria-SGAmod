using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using SGAmod.Dimensions;

namespace SGAmod.Items.Tools
{
	public class TerraExcavator : Geyodo
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Excavator");
			Tooltip.SetDefault("Control a yoyo bound with even stronger pickaxes\nCarves tunnels out of earth AND your enemies, terra tier!");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
			Item.damage = 46;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = 5;
			Item.channel = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.knockBack = 7f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.pick = 200;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item19;
			Item.shoot = Mod.Find<ModProjectile>("TerraExcavatorProj").Type;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("Geyodo"), 1).AddIngredient(ItemID.SpectrePickaxe, 1).AddIngredient(ItemID.ShroomiteDiggingClaw, 1).AddIngredient(ItemID.ChlorophytePickaxe, 1).AddIngredient(ItemID.PickaxeAxe, 1).AddIngredient(ItemID.BrokenHeroSword, 1).AddTile(mod.TileType("ReverseEngineeringStation")).Register();

		}
	}
	public class TerraExcavatorProj : ExcavatorProj
	{
		public override int[] Pickaxes => new int[] { ItemID.SpectrePickaxe, ItemID.ShroomiteDiggingClaw, ItemID.ChlorophytePickaxe, ItemID.PickaxeAxe };
		public override int RealPickPower => 75;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Excavator");
			//ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 5000f;
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 8f;
		}
	}
	public class Geyodo : Excavator
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geyodo");
			Tooltip.SetDefault("Control a yoyo bound with stronger pickaxes\nCarves tunnels out of earth AND your enemies, even more so!");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
			Item.damage = 36;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = 5;
			Item.channel = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.knockBack = 7f;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Pink;
			Item.pick = 180;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item19;
			Item.shoot = Mod.Find<ModProjectile>("GeyodoProj").Type;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("Excavator"), 1).AddRecipeGroup("SGAmod:Tier5Pickaxe", 1).AddRecipeGroup("SGAmod:Tier6Pickaxe", 1).AddRecipeGroup("SGAmod:Tier7Pickaxe", 1).AddIngredient(ItemID.MoltenPickaxe, 1).AddIngredient(mod.ItemType("VirulentBar"), 5).AddIngredient(mod.ItemType("CryostalBar"), 5).AddIngredient(mod.ItemType("WraithFragment4"), 15).AddTile(mod.TileType("ReverseEngineeringStation")).Register();

		}
	}
		public class GeyodoProj : ExcavatorProj
	{
		public override int[] Pickaxes => new int[] { ItemID.CobaltPickaxe, ItemID.MythrilPickaxe, ItemID.AdamantitePickaxe, ItemID.MoltenPickaxe };
		public override int RealPickPower => 25;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geyodo Proj");
			//ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 5000f;
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 7f;
		}
	}

	public class Excavator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Excavator");
			Tooltip.SetDefault("Control a yoyo bound with pickaxes\nCarves tunnels out of earth AND your enemies!");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
			Item.damage = 20;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = 5;
			Item.channel = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.knockBack = 7f;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Orange;
			Item.pick = 70;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item19;
			Item.shoot = Mod.Find<ModProjectile>("ExcavatorProj").Type;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddRecipeGroup("SGAmod:Tier1Pickaxe", 1).AddRecipeGroup("SGAmod:Tier2Pickaxe", 1).AddRecipeGroup("SGAmod:Tier3Pickaxe", 1).AddRecipeGroup("SGAmod:Tier4Pickaxe", 1).AddIngredient(ItemID.WoodYoyo, 1).AddIngredient(mod.ItemType("EvilBossMaterials"), 15).AddTile(mod.TileType("ReverseEngineeringStation")).Register();

		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
			//(Main.projectile[proj].ModProjectile as ExcavatorProj).PickPower = item.pick;
			Main.projectile[proj].netUpdate = true;

			return false;
		}
	}

		public class ExcavatorProj : ModProjectile
	{
		public int PickPower = 0;
		public int PowerPick = 0;
		public virtual int RealPickPower => 15;
		public virtual int[] Pickaxes => new int[] { ItemID.CopperPickaxe, ItemID.IronPickaxe, ItemID.SilverPickaxe, ItemID.GoldPickaxe };
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Excavator Proj");
			//ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 5000f;
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 6f;
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			PickPower = reader.ReadInt32();
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(PickPower);
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.TheEyeOfCthulhu);
			Projectile.extraUpdates = 0;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = 99;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			if (owner != null && !owner.dead)
			{
				PickPower += 1;

				if (PickPower > 60)
				{
					int pickPower = RealPickPower;
					owner.SGAPly().forcedMiningSpeed = 10;
					//owner.SGAPly().UseTimeMulPickaxe
					if ((PickPower % (int)(20 * (owner.meleeSpeed/(owner.SGAPly().UseTimeMulPickaxe/owner.pickSpeed)))) == 0)
					{
						PowerPick += 1;

						int dist = 64 * 64;
						foreach(Projectile asteriodproj in Main.projectile.Where(testby => testby.active && testby.ModProjectile != null && testby.ModProjectile is IMineableAsteriod && (testby.Center - Projectile.Center).LengthSquared() < dist))
                        {
							IMineableAsteriod asteriod = asteriodproj.ModProjectile as IMineableAsteriod;
							asteriod.MineAsteriod(owner.HeldItem,false);
						}

						Point16 hereIAm = new Point16((int)Projectile.Center.X >> 4, (int)Projectile.Center.Y >> 4);
						for (int x = -3; x <= 3; x += 1)
						{
							for (int y = -3; y <= 3; y += 1)
							{
								if (new Vector2(x, y).LengthSquared() < 3*3 && !Main.tileAxe[Main.tile[hereIAm.X + x, hereIAm.Y + y].TileType])
									owner.PickTile(hereIAm.X + x, hereIAm.Y + y, pickPower);
							}
						}
					}
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Effect fadeIn = SGAmod.FadeInEffect;

			float percenthit = MathHelper.Clamp(PickPower / 60f, 0f, 1f);
			float percenthit2 = MathHelper.Clamp((PickPower - 45) / 60f, 0f, 1f);

			fadeIn.Parameters["alpha"].SetValue(1);
			fadeIn.Parameters["strength"].SetValue(1f-percenthit2);
			fadeIn.Parameters["fadeColor"].SetValue(Color.Goldenrod.ToVector3());
			fadeIn.Parameters["blendColor"].SetValue(lightColor.ToVector3());

			fadeIn.CurrentTechnique.Passes["FadeIn"].Apply();

			for (int i = 0; i < Pickaxes.Length; i += 1)
			{
				Texture2D tex = Main.itemTexture[Pickaxes[i]];
				Vector2 offset = new Vector2(0, tex.Height*percenthit);
				float angle = Projectile.rotation + MathHelper.TwoPi * (i / (float)Pickaxes.Length);
				//spriteBatch.Draw(tex, projectile.Center - Main.screenPosition,null, lightColor, angle, offset, projectile.scale, default, 0);

				spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, new Rectangle(0, (int)(tex.Height * (1f-percenthit)),(int)(tex.Width* percenthit), (int)(tex.Height* (percenthit))), Color.White, angle, offset, Projectile.scale, default, 0);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			/*Effect RadialEffect = SGAmod.RadialEffect;

			Texture2D mainTex = mod.GetTexture("GreyHeart");//Main.projectileTexture[projectile.type];

			RadialEffect.Parameters["overlayTexture"].SetValue(mod.GetTexture("Space"));
			RadialEffect.Parameters["alpha"].SetValue(1f);
			RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTimeWrappedHourly*0.125f, Main.GlobalTimeWrappedHourly * 0.275f));
			RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(2f,1f));
			RadialEffect.Parameters["ringScale"].SetValue(0.30f);
			RadialEffect.Parameters["ringOffset"].SetValue(0.50f);
			RadialEffect.Parameters["tunnel"].SetValue(false);

			RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

			spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.White, 0, mainTex.Size()/2f, 32f, default, 0);*/

			return true;
		}

	}

}