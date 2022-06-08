using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using SGAmod.Projectiles;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class GalacticInferno : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Galactic Inferno");
			Tooltip.SetDefault("Swings dark energy in the direction of the blade rather than at your mouse cursor.");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 200;
			Item.crit = 0;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 2;
			Item.useAnimation = 22;
			Item.reuseDelay = 30;
			Item.useStyle = 1;
			Item.knockBack = 5;
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = 11;
			Item.UseSound = SoundID.Item45;
			Item.shoot = Mod.Find<ModProjectile>("SurtCharging").Type;
			Item.shootSpeed = 20f;
			Item.useTurn = false;
			Item.autoReuse = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/GalacticInferno_Glow").Value;
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("UnmanedSword"), 1).AddIngredient(ItemID.DD2SquireDemonSword, 1).AddIngredient(ItemID.BrokenHeroSword, 1).AddIngredient(mod.ItemType("IlluminantEssence"), 20).AddIngredient(mod.ItemType("StarMetalBar"), 12).AddTile(TileID.LunarCraftingStation).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			float speed = 1.5f;
			float numberProjectiles = 3;
			float rotation = MathHelper.ToRadians(16);
			//Main.PlaySound(SoundID.Item, player.Center,45);

			float speedvel = new Vector2(speedX, speedY).Length();

			Vector2 eree = player.itemRotation.ToRotationVector2();
			eree *= player.direction;
			speedX = eree.X * speedvel;
			speedY = eree.Y * speedvel;

			position += eree * 45f;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				perturbedSpeed.RotatedBy(MathHelper.ToRadians(-45));
				perturbedSpeed *= Main.rand.NextFloat(0.8f, 1.2f);
				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.DD2DarkMageBolt, (int)((float)damage * 1.00f), knockBack / 3f, player.whoAmI);
				Main.projectile[proj].DamageType = DamageClass.Melee;
				// Main.projectile[proj].magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].timeLeft = 180;
				Main.projectile[proj].localAI[0] = 1f;
				Main.projectile[proj].netUpdate = true;
				Main.projectile[proj].velocity = perturbedSpeed;
				IdgProjectile.Sync(proj);

				//NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
			}

			return false;
		}


		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			//if (player.ownedProjectileCounts[mod.ProjectileType("TrueMoonlightCharging")]>0)
			//return;

			for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 27);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + ((player.itemRotation.ToRotationVector2() * 5f).RotatedBy(MathHelper.ToRadians(-90)));
				Main.dust[dust].noGravity = true;
			}

			Lighting.AddLight(player.position, 0.9f, 0.1f, 0.5f);
		}




	}



}