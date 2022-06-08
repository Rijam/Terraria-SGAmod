using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics;
using ReLogic.Graphics;
using Terraria.UI.Chat;
using SGAmod.Projectiles;
using SGAmod.NPCs.Hellion;
using Idglibrary;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class Enmity : ModItem, IHitScanItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enmity");
			Tooltip.SetDefault("'Live by the sword, die by the sword'\n'Ashes to Ashes, Dust to Dust...'-the Blade of Hellion, forged from the finest blades of this world\nRequires mana to swing, melee hits with the blade inflict bypassing Sundered Defense and summon Fireworks around hit targets\nTapping swings lv2 True Moonlight waves, Can tap up to 2 times to summon 2 portals at once\nUnleashes total laser hell...\nThe damage of these are less than the melee damage but are improved by your magic damage multiplier");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 1500;
			Item.crit = 5;
			Item.DamageType = DamageClass.Melee;
			Item.width = 44;
			Item.height = 52;
			Item.useTime = 15;
			Item.useAnimation = 10;
			Item.useStyle = 10;
			Item.knockBack = 15;
			Item.value = Item.sellPrice(10,0,0,0);
			Item.shootSpeed = 28f;
			Item.shoot = Mod.Find<ModProjectile>("ProjectilePortalEnmity").Type;
			Item.rare = 12;
			Item.UseSound = SoundID.Item71;
			Item.autoReuse = false;
			Item.useTurn = false;
			Item.channel = true;
			Item.mana = 40;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/Enmity_Glow").Value;
				Item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Main.hslToRgb((Main.GlobalTimeWrappedHourly*1.5f)%1f,0.8f,0.75f);
				};
			}

		}

		public override bool CanUseItem(Player player)
		{
			if (player.statMana < 30 || player.ownedProjectileCounts[Mod.Find<ModProjectile>("ProjectilePortalEnmity").Type] > 2)
			{
				return false;
			}
			else 
			{
				//item.useTime = 15;
				//item.useAnimation = 10;
				return true;
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Item.noMelee = false;
			Item.useStyle = 1;

			float speed = 1.5f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(4);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			SoundEngine.PlaySound(SoundID.Item, player.Center, 45);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X*1.5f, perturbedSpeed.Y * 1.5f, Mod.Find<ModProjectile>("MoonlightWaveLv2").Type, (int)((float)damage * 0.20f), knockBack / 3f, player.whoAmI);
				Main.projectile[proj].DamageType = DamageClass.Melee;
				// Main.projectile[proj].magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
				Main.projectile[proj].netUpdate = true;
				IdgProjectile.Sync(proj);

				//NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
			}
			//SGAPlayer.LimitProjectiles(player, 0, new ushort[] {(ushort)mod.ProjectileType("ProjectilePortalRealityShaper") });
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{

			for (int a = 0; a < 3; a += 1)
			{
				for (int i = 0; i < 360; i += 120)
				{
					float angle = MathHelper.ToRadians(i+(a*45));
					Vector2 hereas = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (200f + (160f * a));
					hereas += target.Center;
					Vector2 gohere = (target.Center - hereas); gohere.Normalize(); gohere *= (16f*a);
					int proj = Projectile.NewProjectile(hereas, gohere, Mod.Find<ModProjectile>("ProjectilePortalEmnityHit").Type, (int)(damage * 0.25f), knockBack, player.whoAmI, 167 + Main.rand.Next(4));
					// Main.projectile[proj].magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
					Main.projectile[proj].DamageType = DamageClass.Melee;
					Main.projectile[proj].timeLeft = 70;
					Main.projectile[proj].netUpdate = true;
					IdgProjectile.Sync(proj);

				}
			}

			IdgNPC.AddBuffBypass(target.whoAmI,Mod.Find<ModBuff>("SunderedDefense").Type, 60 * 12);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			/*for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 20);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
				Main.dust[dust].noGravity = true;
			}*/

			for (int num475 = 3; num475 < 5; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 27);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 15f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 3f) + ((player.direction) * player.itemRotation.ToRotationVector2() * (float)num475);
				Main.dust[dust].noGravity = true;
			}

			Lighting.AddLight(player.position, 0.1f, 0.1f, 0.9f);
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("RealityShaper"), 1).AddIngredient(mod.ItemType("BrimflameHarbinger"), 1).AddIngredient(mod.ItemType("GalacticInferno"), 1).AddIngredient(ItemID.TerraBlade, 1).AddIngredient(mod.ItemType("CelestialFlare"), 1).AddIngredient(mod.ItemType("Skylight"), 1).AddIngredient(mod.ItemType("TrueMoonlight"), 1).AddIngredient(mod.ItemType("SOATT"), 1).AddIngredient(mod.ItemType("TrueCaliburn"), 1).AddIngredient(mod.ItemType("ByteSoul"), 500).AddIngredient(mod.ItemType("HellionSummon"), 1).AddIngredient(mod.ItemType("CodeBreakerHead"), 1).AddIngredient(ItemID.AviatorSunglasses, 1).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string text = tooltips[0].Text;
			string newline = "";
					for (int i = 0; i < text.Length; i += 1)
					{
						newline += Idglib.ColorText(Main.hslToRgb((((-Main.GlobalTimeWrappedHourly*6f)+i)/(10f)) % 1f, 0.75f, Main.rand.NextFloat(0.25f, 0.5f)), text[i].ToString());
					}
				tooltips[0].Text = newline;
		}


	}

	public class ProjectilePortalEmnityHit : ProjectilePortal
	{
		public override int takeeffectdelay => 0;
		public override float damagescale => 1f;
		public override int penetrate => 1;
		public override int openclosetime => 16;

		public override void Explode()
		{

			if (Projectile.timeLeft == timeleftfirerate && Projectile.ai[0] > 0)
			{
				Player owner = Main.player[Projectile.owner];
				if (owner != null && !owner.dead)
				{

					Vector2 gotohere = new Vector2();
					gotohere = Projectile.velocity;//Main.MouseScreen - projectile.Center;
					gotohere.Normalize();

					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(3)) * Projectile.velocity.Length();
					int proj = Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), (int)Projectile.ai[0], (int)(Projectile.damage * damagescale), Projectile.knockBack / 10f, owner.whoAmI);
					Main.projectile[proj].DamageType = DamageClass.Magic;
					Main.projectile[proj].timeLeft = 300;
					Main.projectile[proj].penetrate = penetrate;
					IdgProjectile.Sync(proj);
				}

			}

		}

	}


	public class ProjectilePortalEnmity : ProjectilePortalDSupernova
	{
		public override int openclosetime => 20;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova");
		}

		public override int projectilerate => 25;
		public override int manacost => 9;
		public override int portalprojectile => Mod.Find<ModProjectile>("CirnoBolt").Type;
		public override int takeeffectdelay =>  Main.player[Projectile.owner].HeldItem.useTime;
		public override float damagescale => 0.4f * Main.player[Projectile.owner].GetDamage(DamageClass.Magic);
		public override int penetrate => 1;
		public override int startrate => 60;
		public override int drainrate => 5;
		public override int timeleftfirerate => 20;
		public override float portaldistfromsword => 128f;

		public int everyother = 0;

		public int chargeup = 0;

		public override void Explode()
		{
			chargeup += 1;

			if (Projectile.timeLeft == timeleftfirerate && Projectile.ai[0] > 0)
			{
				Player owner = Main.player[Projectile.owner];

				if (owner != null && !owner.dead && owner.channel)
				{
					everyother += 1;
					everyother %= 3;

					Vector2 gotohere = new Vector2();
					gotohere = Projectile.velocity;//Main.MouseScreen - projectile.Center;
					gotohere.Normalize();

					float accuracy = Math.Max(0.005f, 1f - ((chargeup) / 500f));

					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(0)) * Projectile.velocity.Length();

					for (int i = 2; i < 8; i += 1)
					{

						int proj = Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.Center.Y), (new Vector2(perturbedSpeed.X, perturbedSpeed.Y) * 2f).RotatedByRandom((MathHelper.ToRadians((i * 75))) * accuracy).RotatedByRandom(MathHelper.ToRadians(160f)* accuracy), Mod.Find<ModProjectile>("RainbowBolt").Type, (int)((Projectile.damage*0.60f) * damagescale), Projectile.knockBack / 10f, owner.whoAmI);
						Main.projectile[proj].DamageType = DamageClass.Magic;
						Main.projectile[proj].minion = false;
						IdgProjectile.Sync(proj);
					}


					if (everyother == 2)
					{

						Vector2 backthere = new Vector2(-100, 0).RotatedByRandom(MathHelper.ToRadians(80));

						//int proj2 = Projectile.NewProjectile(backthere, gohere, mod.ProjectileType("ProjectilePortalRealityShaperHit"), (int)(projectile.damage * damagescale), projectile.knockBack / 10f, owner.whoAmI, mod.ProjectileType("HotRound"));

						Func<Vector2, Vector2, float, float, Projectile, float> projectilefacingmore = delegate (Vector2 playerpos, Vector2 projpos, float time, float current, Projectile proj)
						{
							float val = current;
							if (Projectile.active)
							{
								if (time < 100)
									val = current.AngleLerp(Projectile.velocity.ToRotation() + proj.ai[1], 0.15f);
								else
									val = current.AngleLerp(Projectile.velocity.ToRotation() + proj.ai[1], 0.05f);

							}
							return val;
						};
						Func<Vector2, Vector2, float, Vector2, Projectile, Vector2> projectilemovingmore = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current, Projectile proj)
					   {


						   if (Projectile.active)
						   {
							   Vector2 normspeed = Projectile.velocity;
							   normspeed.Normalize();

							   Vector2 gothere333 = (playerpos + backthere.RotatedBy(Projectile.velocity.ToRotation())) - normspeed * 128f;
							   Vector2 slideover = gothere333 - projpos;
							   current = slideover / 2f;
						   }

						   current /= 1.125f;
						   if (Projectile.active)
						   {

							   Vector2 speedz = current;
							   float spzzed = speedz.Length();
							   speedz.Normalize();
							   if (spzzed > 100f)
								   current = (speedz * spzzed);
						   }
						   else
						   {
							   proj.timeLeft = Math.Min(proj.timeLeft, 20);
						   }

						   return current;
					   };

						Func<float, bool> projectilepattern = (time) => (time == 20);

						int ize2 = ParadoxMirror.SummonMirror(owner.Center, Vector2.Zero, (int)((Projectile.damage*3) * damagescale), 200, Projectile.velocity.ToRotation(), Mod.Find<ModProjectile>("HellionBeam").Type, projectilepattern, 2.5f, 145, true);
						(Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilefacingmore = projectilefacingmore;
						(Main.projectile[ize2].ModProjectile as ParadoxMirror).projectilemovingmore = projectilemovingmore;
						SoundEngine.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
						Main.projectile[ize2].owner = Projectile.owner;
						Main.projectile[ize2].aiStyle = -2;
						Main.projectile[ize2].ai[1] = Main.rand.NextFloat(-MathHelper.ToRadians(20), MathHelper.ToRadians(20));
						Main.projectile[ize2].friendly = true;
						Main.projectile[ize2].hostile = false;
						Main.projectile[ize2].usesLocalNPCImmunity = true;
						Main.projectile[ize2].localNPCHitCooldown = 15;
						Main.projectile[ize2].netUpdate = true;


						IdgProjectile.Sync(ize2);
					}

				}

			}

		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 10;
			Projectile.light = 0.5f;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 38;
		}

	}

	public class RainbowBolt : ModProjectile
	{
		Color rainbows = Color.White;
		public override void SetDefaults()
		{
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 200;
			Projectile.light = 0.1f;
			Projectile.extraUpdates = 300;
			AIType = -1;
			Main.projFrames[Projectile.type] = 1;
			rainbows = Main.hslToRgb(((Main.rand.NextFloat() * 0.40f) + Main.GlobalTimeWrappedHourly) % 1f, 0.9f, 0.75f);
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enmity Bolt");
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Kill();
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			for (int k = 0; k < 4; k++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= 1f;
				int num655 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, Projectile.velocity.X + randomcircle.X * 8f, Projectile.velocity.Y + randomcircle.Y * 8f, 100, rainbows, 2.0f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].velocity *= 0.5f;
			}


			return true;
		}

		public override void AI()
		{
			Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= 0.1f;
			int num655 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, Projectile.velocity.X + randomcircle.X * 8f, Projectile.velocity.Y + randomcircle.Y * 8f, 100, rainbows, 1.5f);
			Main.dust[num655].noGravity = true;
			Main.dust[num655].velocity *= 0.5f;

			if (Projectile.localAI[1] == 0f)
			{
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
			}
		}
	}


}