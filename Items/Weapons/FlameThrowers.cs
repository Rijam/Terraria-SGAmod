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
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class ShadeflameStaff : CorruptedTome
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadeflame Staff");
			Tooltip.SetDefault("Spews a stream of Shadow Flames\nFlames bounce off walls and fall to the ground for a while");
		}

		public override void SetDefaults()
		{
			Item.damage = 4;
			Item.DamageType = DamageClass.Magic;
			Item.width = 40;
			Item.height = 20;
			Item.mana = 5;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 0.0f;
			Item.value = 25000;
			Item.rare = ItemRarityID.Blue;
			Item.autoReuse = true;
			//item.UseSound = SoundID.Item34;
			Item.shootSpeed = 4f;
			Item.staff[Item.type] = true;
			Item.shoot = ModContent.ProjectileType<ShadeFlameProj>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.itemAnimation%4==0)
				SoundEngine.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 34,1f,Main.rand.NextFloat(-0.25f,0.25f));
			Vector2 formerposition = position;
				position += Vector2.Normalize(new Vector2(speedX, speedY))*42f;
			if (Collision.CanHitLine(position, 3, 3, formerposition, 3, 3))
			{
				int probg = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
				Main.projectile[probg].velocity.X = perturbedSpeed.X;
				Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			}
			return false;
		}

        public override void AddRecipes()
        {
            //nil
        }
    }

	public class ShadeFlameProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame");
		}

		public override void SetDefaults()
		{
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = true;
			Projectile.width = 4;
			Projectile.height = 4;
			AIType = ProjectileID.Bullet;
			Projectile.aiStyle = 0;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.light = 0.1f;
			Projectile.timeLeft = 90;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.ai[0] > 0)
				return false;
			return base.CanHitNPC(target);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{

			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X * 0.5f;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y / 3f;
			}

			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.timeLeft += 300;
				Projectile.netUpdate = true;
			}

			return false;
		}

		public override void AI()
		{
			Projectile.localAI[0] += 1;


			if (Projectile.ai[0] > 0)
			{
				Projectile.velocity.Y += 0.1f;
				Projectile.velocity.X /= 1.15f;
			}

			if ((int)Projectile.localAI[0] % 10 == 0)
			{
				foreach (NPC npc in Main.npc)
				{
					if (!npc.dontTakeDamage && !npc.friendly && !npc.townNPC)
					{
						Rectangle rec1 = new Rectangle((int)Projectile.position.X-24, (int)Projectile.Center.Y-48, Projectile.width+48, (int)Projectile.height + 64);
						Rectangle rec2 = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
						if (rec1.Intersects(rec2))
						{
							npc.AddBuff(BuffID.ShadowFlame, 60 * 2);
						}

					}
				}

			}
			Dust num126;
			if (Projectile.ai[0] < 1)
			{
				num126 = Dust.NewDustPerfect(Projectile.Center, 62, Projectile.velocity + new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-8, 0)), 0, Color.White, 3f);
				num126.noGravity = true;
				num126.velocity *= 0.25f;
			}

			num126 = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y) + new Vector2(10 - Main.rand.Next(0, 20), 10 - Main.rand.Next(0, 20)), 173, Projectile.velocity, 0, Color.Blue, 1f);
			num126.noGravity = false;
			num126.velocity += Vector2.Normalize(num126.position-Projectile.Center)+new Vector2(Main.rand.NextFloat(-6f, 6f)* Projectile.ai[0], Main.rand.NextFloat(-Projectile.ai[0]*6,0));

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override string Texture => "SGAmod/Invisible";

	}
		public class CorruptedTome : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrupted Tome");
			Tooltip.SetDefault("Spews a stream of Cursed Flames\nInflicts Cursed Inferno for far longer than usual and inflicts Everlasting Suffering\nEverlasting Suffering increases damage over time by 250% and makes other debuffs last until it ends");
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.DamageType = DamageClass.Magic;
			Item.width = 40;
			Item.height = 20;
			Item.useTime = 5;
			Item.mana = 8;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 0.20f;
			Item.value = 100000;
			Item.rare = ItemRarityID.Lime;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item34;
			Item.shootSpeed = 8f;
			Item.shoot = ProjectileID.EyeFire;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.CursedFlames, 1).AddIngredient(ItemID.Flamethrower, 1).AddIngredient(ItemID.SpectreBar, 8).AddIngredient(ItemID.CursedFlame, 10).AddIngredient(ItemID.SpellTome, 1).AddTile(TileID.CrystalBall).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X + (int)(speedX * 0f), position.Y + (int)(speedY * 0f), speedX, speedY, type, damage, knockBack, player.whoAmI);
			// Main.projectile[probg].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Main.projectile[probg].DamageType = DamageClass.Magic;
			// Main.projectile[probg].melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(7));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			IdgProjectile.AddOnHitBuff(probg, BuffID.CursedInferno, 60 * 20);
			IdgProjectile.AddOnHitBuff(probg, Mod.Find<ModBuff>("EverlastingSuffering").Type, 60 * 7);
			IdgProjectile.Sync(probg);
			return false;
		}
	}

}
