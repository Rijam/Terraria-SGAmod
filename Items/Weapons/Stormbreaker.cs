using System.Collections.Generic;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;

using System.Linq;

namespace SGAmod.Items.Weapons
{
	class Stormbreaker : ModItem, IDevItem
	{
		bool altfired = false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Stormbreaker");
			Tooltip.SetDefault("Left click to guide the Stormbreaker at enemies and deal an additional square-root of their max life on hit\nRight click to hold the hammer up and smite your foes, Consumes 100 Electric Charge per foe to be smited\nFoes must be marked via primary fire (40% chance if immune) or wet to be smited\n2 more bolts are summoned during a rainstorm, but overall are less accurate\n'At least it's not yet another Infinity Gauntlet'");
		}

		public (string, string) DevName()
		{
			return ("Mister Creeper", "(legacy)");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (Main.LocalPlayer.GetModPlayer<SGAPlayer>().devempowerment[1] > 0)
			{
				tooltips.Add(new TooltipLine(Mod, "DevEmpowerment", "--- Empowerment bonus ---"));
				tooltips.Add(new TooltipLine(Mod, "DevEmpowerment", "10% increased damage on Primary"));
				tooltips.Add(new TooltipLine(Mod, "DevEmpowerment", "Secondary will always summon lightning as if it were raining"));
			}
		}
		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.damage = 500;
			Item.shootSpeed = 45f;
			Item.shoot = Mod.Find<ModProjectile>("Stormbreakerproj").Type;
			Item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			Item.width = 20;
			Item.height = 28;
			Item.maxStack = 1;
			Item.knockBack = 9;
			Item.consumable = false;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 10;
			Item.useTime = 10;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.sellPrice(1, 0, 0, 0);
			Item.rare = 12;
			Item.channel = true;
			Item.expert = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.GetModPlayer<SGAPlayer>().devempowerment[1] > 0)
			{
				damage = (int)(damage * 1.10);
			}
			return true;
		}

		public override bool CanUseItem(Player player)
		{

			altfired = player.altFunctionUse == 2 ? true : false;

			if (altfired)
			{
				Item.useAnimation = 45;
				Item.useTime = 45;
				Item.useStyle = 4;
				Item.UseSound = SoundID.Item44;
				Item.channel = false;
				Item.shoot = Mod.Find<ModProjectile>("Stormbreaker2").Type;
				Item.useTurn = false;

			}
			else
			{
				Item.useStyle = 1;
				Item.shootSpeed = 45f;
				Item.shoot = Mod.Find<ModProjectile>("Stormbreakerproj").Type;
				//ProjectileID.CultistBossLightningOrbArc
				Item.UseSound = SoundID.Item1;
				Item.useAnimation = 10;
				Item.useTime = 10;
				Item.channel = true;
				Item.autoReuse = true;
				Item.useTurn = true;
			}
			if (player.ownedProjectileCounts[Item.shoot] > 0)
				return false;

			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.PossessedHatchet, 1).AddIngredient(mod.ItemType("LunarRoyalGel"), 25).AddIngredient(mod.ItemType("StarMetalBar"), 30).AddIngredient(mod.ItemType("OmniSoul"), 10).AddIngredient(mod.ItemType("CosmicFragment"), 1).AddTile(TileID.LunarCraftingStation).Register();
		}
	}

	public class Stormbreakerproj : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stormbreaker");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Stormbreaker"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.timeLeft = 120;
			Projectile.penetrate = 20;
			AIType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = -8;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.penetrate < 10)
				return false;
			else
				return base.CanHitNPC(target);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage += (int)Math.Pow((double)target.lifeMax, 0.5);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.velocity *= -0.5f;
			Vector2 dist = (target.Center - Projectile.Center);
			Vector2 distnorm = dist; distnorm.Normalize();
			Projectile.velocity -= distnorm * 30f;
			target.immune[Projectile.owner] = 7;
			target.AddBuff(Mod.Find<ModBuff>("InfinityWarStormbreaker").Type, 600);
			if (Main.rand.Next(0, 100) < 40)
				IdgNPC.AddBuffBypass(target.whoAmI, Mod.Find<ModBuff>("InfinityWarStormbreaker").Type, 400);
		}

		public override void AI()
		{
			Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
			Player player = Main.player[Projectile.owner];

			for (int num315 = 0; num315 < 1; num315 = num315 + 1)
			{
				int num622 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + positiondust, 0, 0, 185, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num622].velocity *= 1f;

				Main.dust[num622].noGravity = true;
				Main.dust[num622].color = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 5) % 1, 0.9f, 1f);
				Main.dust[num622].color.A = 10;
				Main.dust[num622].velocity.X = Projectile.velocity.X / 3 + (Main.rand.Next(-50, 51) * 0.005f);
				Main.dust[num622].velocity.Y = Projectile.velocity.Y / 3 + (Main.rand.Next(-50, 51) * 0.005f);
				Main.dust[num622].alpha = 100; ;
			}

			Projectile.ai[0] = Projectile.ai[0] + 1;
			bool channeling = ((Projectile.ai[0] < 299 && (player.channel || Projectile.ai[0] < 29)) && !player.noItems && !player.CCed);
			if (Projectile.ai[0] < 300 && Projectile.ai[0] < 600 && (Projectile.penetrate < 10 || !channeling))
				Projectile.ai[0] = 300;
			Projectile.timeLeft = 999;

			if (!Main.player[Projectile.owner].dead)
			{
				Vector2 flyto = Main.MouseWorld;//new Vector2( ((float)Main.mouseX + (float)Main.screenPosition.X - (float)player.position.X), ((float)Main.mouseY + (float)Main.screenPosition.Y - (float)player.position.Y) );
				Vector2 dist = (Main.player[Projectile.owner].Center - Projectile.Center);
				Vector2 distnorm = dist; distnorm.Normalize();

				Vector2 flytodist = (flyto - Projectile.Center);
				Vector2 flytodistnorm = flytodist; flytodistnorm.Normalize();

				if (Projectile.ai[0] > 299)
				{
					flytodist = dist;
					flytodistnorm = distnorm;
				}

				if (Main.LocalPlayer == player)
				{
					float speedBoost = (player.Throwing().thrownVelocity - 1f);
					Projectile.velocity += flytodistnorm * 8f * (speedBoost + 1f);
					Projectile.velocity /= (1.05f * ((speedBoost * 0.05f) + 1f));
					float maxspeed = 38f * (1f + ((player.Throwing().thrownVelocity - 1f) / 1f));
					if (Projectile.velocity.Length() > maxspeed)
					{
						Projectile.velocity.Normalize(); Projectile.velocity *= maxspeed;
					}
					Projectile.netUpdate = true;
				}
				//projectile.Center+=(dist*((float)(projectile.timeLeft-12)/28));


				if (Projectile.ai[0] < 600)
				{
					player.itemTime = (int)MathHelper.Clamp(player.itemTime, 5, 1111);
					player.itemAnimation = (int)MathHelper.Clamp(player.itemAnimation, 5, 1111);
				}

				if (dist.Length() < 64f && Projectile.ai[0] > 299)
				{
					Projectile.ai[0] = 600;
					if ((player.itemTime < 2) || Main.player[Projectile.owner].dead)
						Projectile.Kill();
				}

			}
			else { Projectile.Kill(); }

			/*NPC target=Main.npc[Idglib.FindClosestTarget(0,projectile.Center,new Vector2(0f,0f),true,true,true,projectile)];
			if (target!=null && projectile.penetrate>9){
			if ((target.Center-projectile.Center).Length()<500f){

	projectile.Center+=(projectile.DirectionTo(target.Center)*12f);

	}}*/

			Projectile.rotation = Projectile.rotation.AngleLerp(((float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f) + (float)(Math.PI / -4.0), 0.2f);
		}
	}

	public class Stormbreaker2 : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stormbreaker");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Stormbreaker"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.Throwing().DamageType = DamageClass.Throwing;
			Projectile.timeLeft = 120;
			Projectile.penetrate = 10;
			AIType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = -8;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override void AI()
		{
			Vector2 positiondust = Vector2.Normalize(new Vector2(Projectile.velocity.X, Projectile.velocity.Y)) * 8f;
			Player owner = Main.player[Projectile.owner];

			if (Projectile.ai[0] < 1f && owner.direction != 1)
				Projectile.rotation *= -1f;

			Projectile.ai[0] += 1;

			Projectile.velocity = new Vector2(0f, owner.itemAnimation > 12 ? -16f : 0);
			Projectile.Center = owner.Center + new Vector2(-24f + (owner.direction == 1 ? 10f : -5f), -16f);
			if (owner.itemAnimation < 8)
				Projectile.Center += new Vector2(0, (int)((8.0 - (double)owner.itemAnimation) * 2.5));
			if (owner.itemAnimation < 1) 
			{
				Projectile.Kill();
			}


				//if (owner.timeLeft < 2)
				//	projectile.Kill();


				Projectile.rotation += (((float)(Math.PI / -4.0)) - Projectile.rotation) / 6f; 
			if (Projectile.timeLeft == 110)
			{
				int dist = 1200 * 1200;
				int nummaxbolts = 5;
				foreach (NPC him in Main.npc.Where(testby => testby.active && (testby.GetGlobalNPC<SGAnpcs>().InfinityWarStormbreakerint > -0 || testby.GetGlobalNPC<SGAnpcs>().DosedInGas || testby.dripping) && (testby.Center - owner.Center).LengthSquared() < dist).OrderBy(testby => (testby.Center - owner.Center).LengthSquared()).Take(nummaxbolts))
				{
					if (owner.SGAPly().ConsumeElectricCharge(100, 150))
					{
						int rainmeansmore = (Main.raining || owner.GetModPlayer<SGAPlayer>().devempowerment[1] > 0) ? 2 : 0;

						for (int x = 0; x < rainmeansmore + 1; x++)
						{

							float rotation = MathHelper.ToRadians(5);
							Vector2 speed = new Vector2(0f, 72f);
							Vector2 perturbedSpeed = speed.RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) * 0.02f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
							Vector2 starting = new Vector2(him.Center.X + ((-200 + Main.rand.Next(0, 400)) * rainmeansmore), ((-150 + Main.rand.Next(0, 200)) * rainmeansmore) + him.Center.Y - Main.rand.Next(200, 540));
							int proj = Projectile.NewProjectile(starting.X, starting.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.CultistBossLightningOrbArc, (int)((Projectile.damage * 0.75f) * (1f - owner.manaSickReduction)), 15f, Main.player[Projectile.owner].whoAmI, (him.Center - starting).ToRotation());
							Main.projectile[proj].friendly = true;
							Main.projectile[proj].hostile = false;
							Main.projectile[proj].penetrate = -1;
							Main.projectile[proj].timeLeft = 300;
							//Main.projectile[proj].usesLocalNPCImmunity = true;
							Main.projectile[proj].localNPCHitCooldown = 8;
							Main.projectile[proj].Throwing().DamageType = DamageClass.Throwing;
							IdgProjectile.Sync(proj);

							for (int q = 0; q < 50; q++)
							{
								int dust = Dust.NewDust(Main.projectile[proj].position - new Vector2(100, 0), 200, 12, DustID.Smoke, 0f, 0f, 100, Main.hslToRgb(0.6f, 0.8f, 0.28f), 4f);
								Main.dust[dust].noGravity = true;
							}
						}
					}
				}
			}
		}






	}



}
