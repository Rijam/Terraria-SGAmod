using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.Enums;
using SGAmod.Items.Weapons;
using Idglibrary;
using SGAmod.Projectiles;
using System.Linq;
using SGAmod.Effects;
using System.IO;
using Terraria.DataStructures;

namespace SGAmod.Items.Weapons
{
		public class CrystalComet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Comet");
			Tooltip.SetDefault("Manifests a purple Comet that pierces infinitely, releasing shards as it flies");
		}

        public override void SetDefaults()
		{
			Item.damage = 120;
			Item.DamageType = DamageClass.Magic;
			Item.width = 24;
			Item.height = 24;
			Item.useTime = 40;
			Item.mana = 60;
			Item.crit = 0;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 0.15f;
			Item.value = 100000;
			Item.rare = ItemRarityID.Yellow;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item105;
			Item.shootSpeed = 8f;
			Item.shoot = ModContent.ProjectileType<PrismicShowerProj>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.CrystalStorm, 1).AddIngredient(ModContent.ItemType<OverseenCrystal>(), 20).AddIngredient(ModContent.ItemType<PrismalBar>(), 15).AddTile(mod.TileType("PrismalStation")).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X + (int)(speedX * 0f), position.Y + (int)(speedY * 0f), speedX, speedY, type, damage, knockBack, player.whoAmI,ai0: Main.rand.Next(600));
			// Main.projectile[probg].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Main.projectile[probg].DamageType = DamageClass.Magic;
			// Main.projectile[probg].melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			//IdgProjectile.AddOnHitBuff(probg, BuffID.CursedInferno, 60 * 20);
			IdgProjectile.Sync(probg);
			return false;
		}
	}

	public class PrismicShowerProj : ModProjectile
	{
		float strength => Math.Min(Projectile.timeLeft / 120f, 1f);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismic Storm");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false;
			Projectile.width = 4;
			Projectile.height = 4;
			AIType = ProjectileID.Bullet;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.light = 0.1f;
			Projectile.timeLeft = 300;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 3;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

        public override bool CanDamage()
        {
			return true;
        }

		public override void AI()
		{
			Projectile.ai[0] += 1;

			Vector2 offset = Vector2.Normalize(Projectile.velocity).RotatedBy(MathHelper.Pi/2);
			float speed = Main.rand.NextFloat(0f, 8f) * (Main.rand.NextBool() ? 1f : -1f);

			if ((int)Projectile.ai[0] % 3 == 0)
			{
				Projectile proj = Projectile.NewProjectileDirect(Projectile.Center + offset * Main.rand.NextFloat(-64, 64), offset * speed, ProjectileID.CrystalStorm, (int)(Projectile.damage), Projectile.knockBack, Projectile.owner);
				proj.timeLeft = (int)(Projectile.timeLeft / 5f);

			}
			Dust num126;
				num126 = Dust.NewDustPerfect(Projectile.Center+(offset* Main.rand.NextFloat(-12, 12f)), 112, (offset* Main.rand.NextFloat(-9f, 3f)*(Projectile.velocity.X>0 ? -1f : 1f)) + Projectile.velocity + new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-8, 0)), 255-(int)(100f*strength), Color.White, 1f);
				num126.noGravity = true;

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 0; i < Projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
			{
				if (Projectile.oldPos[i] == default)
					Projectile.oldPos[i] = Projectile.position;
			}

			TrailHelper trail = new TrailHelper("DefaultPass", Mod.Assets.Request<Texture2D>("noise").Value);
			trail.color = delegate (float percent)
			{
				return Color.Magenta;
			};
			trail.projsize = Projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -1f);
			trail.trailThickness = 13;
			trail.capsize = new Vector2(8f, 0f);
			trail.strength = strength;
			trail.trailThicknessIncrease = 15;
			trail.DrawTrail(Projectile.oldPos.ToList(), Projectile.Center);

			return false;
		}

		public override string Texture => "SGAmod/Items/Weapons/UnmanedBolt";

	}

	public class ShootingStar : CrystalComet, IHellionDrop, IDedicatedItem
	{
		int IHellionDrop.HellionDropAmmount() => 1;
		int IHellionDrop.HellionDropType() => ModContent.ItemType<ShootingStar>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shooting Star");
			Tooltip.SetDefault("Controls a very inspirational Star\nReleases a nova of stars when hitting past your mouse cursor\nSpeeds up the longer you use the item\n'The more you know!'");
		}

		public override void SetDefaults()
		{
			Item.damage = 250;
			Item.DamageType = DamageClass.Magic;
			Item.width = 24;
			Item.height = 24;
			Item.useTime = 40;
			Item.mana = 50;
			Item.crit = 20;
			Item.useAnimation = 1;
			Item.useTurn = false;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 10f;
			Item.value = 100000;
			Item.rare = ItemRarityID.Purple;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item105;
			Item.shootSpeed = 12f;
			Item.channel = true;
			Item.shoot = ModContent.ProjectileType<ShootingStarProj>();
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Main.itemTexture[Item.type];

				Item.GetGlobalItem<ItemUseGlow>().CustomDraw = delegate (Item item, PlayerDrawInfo drawInfo, Vector2 position, float angle,Color glowcolor)
				{
					Texture2D texture = SGAmod.ExtraTextures[110];
					Vector2 origin = texture.Size() / 2f;
					float timeAdvance = Main.GlobalTimeWrappedHourly*2;
					angle = drawInfo.drawPlayer.itemRotation + (drawInfo.drawPlayer.direction < 0 ? MathHelper.Pi : 0);
					Player drawPlayer = drawInfo.drawPlayer;

					Vector2 drawHere = drawPlayer.MountedCenter+(angle.ToRotationVector2())*32 - Main.screenPosition;
					for (float i = 0f; i < MathHelper.TwoPi; i += MathHelper.PiOver4)
					{
						DrawData value = new DrawData(texture, drawHere + (Vector2.One.RotatedBy(i- timeAdvance)) * 8f, null, glowcolor*0.75f*MathHelper.Clamp(drawPlayer.itemAnimation/60f,0f,1f), -MathHelper.PiOver4+(i- timeAdvance), origin, 0.25f, drawInfo.spriteEffects, 0);
						Main.playerDrawData.Add(value);
					}

				};
			}
	}

		public string DedicatedItem()
        {
			//tooltips.Add(new TooltipLine(mod, "Dedicated", 
			//Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 5f) % 1f, 0.45f, 0.65f);
			return "To Cringe's meme in IDG's Den";
		}
		public override Color? GetAlpha(Color lightColor)
        {
            return lightColor.MultiplyRGBA(Color.Lerp(Color.White,Main.DiscoColor,Main.essScale-0.8f));
        }
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType < CrystalComet>(), 1).AddIngredient(ModContent.ItemType < StarMetalBar>(), 20).AddIngredient(ModContent.ItemType < DrakeniteBar>(), 15).AddIngredient(ModContent.ItemType < StygianCore>(), 2).AddIngredient(ModContent.ItemType<StarMetalMold>(), 1).AddIngredient(ItemID.FragmentSolar, 8).AddIngredient(ItemID.ManaCrystal, 3).AddTile(TileID.LunarCraftingStation).Register();
		}
	}

	public class ShootingStarProj : ModProjectile
	{
		float strength => Math.Min(Projectile.timeLeft / 250f, 1f);
		Vector2 there=default;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shooting Star");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 180;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			damage = (int)(damage * (1f-Main.player[Projectile.owner].manaSickReduction));

		}

        public override void SetDefaults()
		{
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false;
			Projectile.width = 4;
			Projectile.height = 4;
			AIType = ProjectileID.Bullet;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.light = 0.1f;
			Projectile.timeLeft = 300;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 10;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 2;
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.WriteVector2(there);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			there = reader.ReadVector2();
		}

        public override void AI()
		{
			Projectile.ai[0] += 1;
			Projectile.ai[1] += 1;
			Projectile.localAI[0] += 1;

			if (Projectile.ai[0] < -1000)
            {
				return;
            }

			Player player = Main.player[Projectile.owner];
			if (!player.channel || player.dead)
			{
				Projectile.ai[0] = -10000;
			}
			else
			{
				Projectile.timeLeft = 250;
				Vector2 mousePos = Main.MouseWorld;
				player.itemTime = 60;
				player.itemAnimation = 60;

				if (Projectile.owner == Main.myPlayer)
				{
					Vector2 diff = mousePos - Projectile.Center;
					if (diff.Length() > 1800 || Projectile.ai[1]>1200)
					{

						if (!player.CheckMana(player.HeldItem,8, true))
						{
							Projectile.ai[0] = -10000;
							Projectile.netUpdate = true;
							return;
						}



						if (Projectile.localAI[0] > 1000)
						{
							Projectile.extraUpdates = (int)MathHelper.Clamp(Projectile.extraUpdates + 1, 10, 40);
							Projectile.numUpdates = Projectile.extraUpdates;
						}
						diff.Normalize();
						Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
						Vector2 randAng = Main.rand.NextVector2CircularEdge(1500f, 1500f);
						Projectile.Center = mousePos + randAng;
						Projectile.velocity = Vector2.Normalize(mousePos-Projectile.Center)*(Projectile.velocity.Length());
						there = Main.MouseWorld;
						Projectile.ai[1] = 0;

						Vector2 dir = (there - player.Center);
						player.ChangeDir(dir.X > 0 ? 1 : -1);
						player.itemRotation = dir.ToRotation()-(player.direction<0 ? MathHelper.Pi : 0);

						for (int i = 0; i < Projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
						{
							Projectile.oldPos[i] = Projectile.position;
						}
						Projectile.netUpdate = true;

					}
				}


				if (there != default)
				{
					if (Vector2.Dot(Vector2.Normalize(there - Projectile.Center), Vector2.Normalize(Projectile.velocity)) < -0.9f)
					{
						Projectile.ai[1] = 1000;
						there = default;
						NoiseGenerator noise = new NoiseGenerator(Projectile.whoAmI);
						noise.Amplitude = 5;
						noise.Frequency = 0.5;
						for (float i = 0; i < 1f; i += 1f / 5f)
						{
							Vector2 there2 = (Projectile.velocity); there.Normalize(); there2 = there2.RotatedBy(i * MathHelper.TwoPi);
							int prog = Projectile.NewProjectile(Projectile.Center, Vector2.Normalize(there2) * (2f + (float)noise.Noise((int)(i * 80), (int)Projectile.ai[0])) * 5f, ProjectileID.HallowStar, (int)((1f - Main.player[Projectile.owner].manaSickReduction) / 5f), Projectile.knockBack / 10f, Projectile.owner);
							Main.projectile[prog].timeLeft = 20 + (int)(noise.Noise((int)(i * 40), (int)Projectile.ai[0] + 800) * 40);
							Main.projectile[prog].alpha = 150;
							Main.projectile[prog].localNPCHitCooldown = -1;
							Main.projectile[prog].penetrate = 2;
							Main.projectile[prog].usesLocalNPCImmunity = true;
							Main.projectile[prog].netUpdate = true;
						}

					}
				}

			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 0; i < Projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
			{
				if (Projectile.oldPos[i] == default)
					Projectile.oldPos[i] = Projectile.position;
			}

			TrailHelper trail = new TrailHelper("DefaultPass", Mod.Assets.Request<Texture2D>("noise").Value);
			trail.color = delegate (float percent)
			{
				return Main.hslToRgb((percent*1f)%1f,0.85f,0.75f);
			};
			trail.projsize = Projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -1f);
			trail.trailThickness = 13;
			trail.capsize = new Vector2(8f, 0f);
			trail.strength = strength;
			trail.trailThicknessIncrease = 15;
			trail.DrawTrail(Projectile.oldPos.ToList(), Projectile.Center);


			Texture2D texaz = SGAmod.ExtraTextures[110];

			for (float xx = -3; xx < 3.5f; xx += 0.5f)
			{
				for (float i = 1f; i < 3; i += 0.4f)
				{
					float scalerz = 0.85f + (float)Math.Cos(Main.GlobalTimeWrappedHourly * 1.25f * (Math.Abs(xx) + i)) * 0.3f;
					spriteBatch.Draw(texaz, (Projectile.Center + ((Projectile.velocity.ToRotation() + (float)Math.PI / 4f)).ToRotationVector2() * (xx * 9f)) - Main.screenPosition, null, Color.Yellow * (0.5f / (i + xx)) * 0.25f, Projectile.velocity.ToRotation() + (float)Math.PI / 2f, new Vector2(texaz.Width / 2f, texaz.Height / 4f), (new Vector2(1 + i, 1 + i * 1.5f) / (1f + Math.Abs(xx))) * scalerz * Projectile.scale, SpriteEffects.None, 0f);
				}
			}

			return false;
		}

		public override string Texture => "SGAmod/Items/Weapons/UnmanedBolt";

	}

}
