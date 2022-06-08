using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Buffs;
using Idglibrary;
using Terraria.Audio;

namespace SGAmod.Items.Consumables
{
    class Jarate : ModItem
    {

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Jarate");
			Tooltip.SetDefault("Throws a jar of 'nature's rain', which inflicts ichor on everyone in a large area for an extended time\nIf it directly hits an enemy, they will get Sodden instead even if immune\nThis increases any further damage they take by 33%\n'Heads up!'\n"+Idglib.ColorText(Color.Orange,"Requires 1 Cooldown stack, adds 30 seconds"));

		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Consumables/Jarate"); }
		}

		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.shootSpeed = 12f;
			Item.shoot = Mod.Find<ModProjectile>("JarateProj").Type;
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 10;
			Item.consumable = true;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.value = Item.buyPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Yellow;
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}

		public override bool ConsumeItem(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.SGAPly().AddCooldownStack(30 * 60);
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);

		}

		public override void AddRecipes()
		{
			CreateRecipe(3).AddIngredient(mod.ItemType("FieryShard"), 2).AddIngredient(mod.ItemType("MurkyGel"), 5).AddIngredient(ItemID.Bottle, 3).AddIngredient(ItemID.Ichor, 2).AddTile(TileID.WorkBenches).Register();
		}

	}

	public class JarateProj : GasPasserProj
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jar of Piss");
		}
		public override float beginhoming => 99990f;

		public override string Texture
		{
			get { return ("SGAmod/Items/Consumables/Jarate"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 240;
			Projectile.arrow = true;
			Projectile.damage = 0;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override void AI()
		{
			effects(0);

			Projectile.ai[0] = Projectile.ai[0] + 1;
			Projectile.velocity.Y += 0.15f;
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;


			NPC target = Main.npc[Idglib.FindClosestTarget(0, Projectile.Center, new Vector2(0f, 0f), true, true, true, Projectile)];
			if (target != null)
			{
				if (new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height).Intersects
					(new Rectangle((int)target.position.X, (int)target.position.Y, target.width, target.height)))
				{
					IdgNPC.AddBuffBypass(target.whoAmI, Mod.Find<ModBuff>("Sodden").Type, 60*45);
					if (Main.player[Projectile.owner].GetModPlayer<SGAPlayer>().MVMBoost)
						IdgNPC.AddBuffBypass(target.whoAmI,Mod.Find<ModBuff>("SoddenSlow").Type, 60 * 45);
					Projectile.Kill();
				}
			}


		}

		public override void effects(int type)
		{
			if (type == 0)
			base.effects(type);
			if (type == 1)
			{

				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/jar_explode").WithVolume(.7f).WithPitchVariance(.25f),Projectile.Center);
				Projectile.type = ProjectileID.IchorSplash;
				//int proj = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(0, 0), mod.ProjectileType("GasCloud"), 1, projectile.knockBack, Main.player[projectile.owner].whoAmI);
				for (int q = 0; q < 100; q++)
				{
					float randomfloat = Main.rand.NextFloat(5f, 15f);
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Vector2 speedz = new Vector2(randomcircle.X, randomcircle.Y) * randomfloat;


					int dust = Dust.NewDust(Projectile.Center-new Vector2(24,24), 48, 48, 75, speedz.X, speedz.Y, 80, Color.LightGoldenrodYellow * 0.85f, 8f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = speedz;
					Main.dust[dust].fadeIn = 2f;
					Main.dust[dust].noLight = true;
				}

				for (int num172 = 0; num172 < Main.maxNPCs; num172 += 1)
				{
					NPC target = Main.npc[num172];
					float damagefalloff = 1F - ((target.Center - Projectile.Center).Length() / 180f);
					// && target.ModNPC.CanBeHitByProjectile(projectile) == true)
					if ((target.Center - Projectile.Center).Length() < 180f && !target.friendly && !target.dontTakeDamage && (target.ModNPC == null || (target.ModNPC != null)))
					{
						target.AddBuff(BuffID.Ichor,(60*5)+(int)(damagefalloff*60f*10));
					}
				}

			}

		}


	}

}



