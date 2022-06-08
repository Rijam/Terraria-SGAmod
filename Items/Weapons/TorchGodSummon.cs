using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class TorchGodSummon : ModItem
	{
        public override bool Autoload(ref string name)
        {
			SGAPlayer.PostUpdateEquipsEvent += AddFunction;
			return true;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torch God's Summon");
			Tooltip.SetDefault("Summons 2 torches per minion slot, and 1 per empty minion slot\nAttacking with torches burns them out for 3 seconds\nGain +10 damage per max minions, and +1 pierce per max Sentries, Biome torches inflict debuffs\nTorches provide a small amount of light in the fog");
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 10;
			Item.DamageType = DamageClass.Summon;
			Item.width = 44;
			Item.height = 52;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 10;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.LightRed;
			//item.UseSound = SoundID.Item71;
			Item.shoot = 10;
			Item.shootSpeed = 30f;
			Item.autoReuse = true;
			Item.useTurn = false;
			Item.mana = 4;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/TorchGodSummon_Glow").Value;
			}
		}

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
			flat += player.maxMinions * 10;
        }

		public static int MaxTorches(Player player)
        {
			return player.maxMinions*2+(int)((player.maxMinions - (player.SGAPly().GetMinionSlots)) * 1);
		}

		public void AddFunction(SGAPlayer sgaply)
        {
			Player player = sgaply.Player;
			if (!player.dead && player.HeldItem.type == ModContent.ItemType<TorchGodSummon>())
			{
				for (int i = 0; i < MaxTorches(player) - player.ownedProjectileCounts[ModContent.ProjectileType<TorchGodSummonMinion>()]; i += 1)
				{
					Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<TorchGodSummonMinion>(), Item.damage, 10f, player.whoAmI, player.ownedProjectileCounts[ModContent.ProjectileType<TorchGodSummonMinion>()]+i, -i);
					if (proj != null)
					{

					}
				}
			}
        }

		public static TorchGodSummonMinion GetProjectileToShootFrom(Player player)
        {
			TorchGodSummonMinion sum = null;

			foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.owner == player.whoAmI && testby.ModProjectile != null && testby.ai[1]<0 && testby.type == ModContent.ProjectileType<TorchGodSummonMinion>()).OrderBy(testby => testby.ai[1]))
			{
				//Main.NewText("test");
				sum = (proj.ModProjectile) as TorchGodSummonMinion;
				break;
			}
			return sum;
		}

        public override bool CanUseItem(Player player)
        {
			return GetProjectileToShootFrom(player) != null;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speedz = new Vector2(speedX, speedY);
			speedz.Normalize(); speedz *= (30f+(player.maxMinions*2f)); speedX = speedz.X; speedY = speedz.Y;

			GetProjectileToShootFrom(player)?.ShootFlame(damage,knockBack, 12f + (player.maxMinions * 1f));

				//int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("TorchGodSummonProjectile"), damage, knockBack, player.whoAmI);
				//Main.projectile[proj].netUpdate = true;
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Torch, 200).AddRecipeGroup("SGAmod:Gems",10).AddTile(TileID.WorkBenches).Register();
		}


	}

	public class TorchGodSummonProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torched");
		}

		public override string Texture => "SGAmod/Invisible";

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			AIType = ProjectileID.Boulder;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.penetrate = 1;
			Projectile.light = 0f;
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.knockBack = 0.5f;
			// projectile.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.minion = true;
			Projectile.tileCollide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}

		public override bool PreKill(int timeLeft)
		{
			Microsoft.Xna.Framework.Audio.SoundEffectInstance snd = SoundEngine.PlaySound(SoundID.Item110, Projectile.Center);
			if (snd != null)
			{
				snd.Pitch = 0.50f;
			}

			for(int i = 0; i < 16; i++)
            {
				Vector2 velocity = Main.rand.NextVector2Circular(8f,8f) * Main.rand.NextFloat(0.2f, 1f);

				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight);
				Main.dust[dust].scale = 1.00f;
				Main.dust[dust].fadeIn = 1.5f;
				Main.dust[dust].color = TorchGodSummonMinion.TorchColors[(int)Projectile.ai[0] % 16];
				Main.dust[dust].alpha = 250;
				Main.dust[dust].velocity = velocity;
				Main.dust[dust].noGravity = true;

				velocity = Main.rand.NextVector2Circular(16f, 16f);

				dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight);
				Main.dust[dust].scale = 1.50f;
				Main.dust[dust].fadeIn = 0.25f;
				Main.dust[dust].color = TorchGodSummonMinion.TorchColors[(int)Projectile.ai[0] % 16];
				Main.dust[dust].alpha = 250;
				Main.dust[dust].velocity = velocity;
				Main.dust[dust].noGravity = true;

			}


			return true;
		}

		public override void AI()
		{

			if (Projectile.ai[1] < 10)
            {
				Projectile.ai[1] = 10;
				Projectile.penetrate = Main.player[Projectile.owner].maxTurrets;
			}
			Lighting.AddLight(Projectile.Center, TorchGodSummonMinion.TorchColors[(int)Projectile.ai[0] % 16].ToVector3());

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight);
			Main.dust[dust].scale = 1.00f;
			Main.dust[dust].fadeIn = 1.5f;
			Main.dust[dust].color = TorchGodSummonMinion.TorchColors[(int)Projectile.ai[0] % 16];
			Main.dust[dust].alpha = 250;
			Main.dust[dust].noGravity = true;

			dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight);
			Main.dust[dust].scale = 1.50f;
			Main.dust[dust].fadeIn = 0.25f;
			Main.dust[dust].color = TorchGodSummonMinion.TorchColors[(int)Projectile.ai[0] % 16];
			Main.dust[dust].alpha = 250;
			Main.dust[dust].noGravity = true;

		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int index = TorchGodSummonMinion.GetBuffIndex((int)Projectile.ai[0] % 16);
			if (index > 0)
				target.AddBuff(index, index == ModContent.BuffType< Buffs.MoonLightCurse >() ? 60 * 2 : 60*8);

		}
	}

	public class TorchGodSummonMinion : ModProjectile
	{

		public static Color[] TorchColors => new Color[] {Color.DarkOrange
		,Color.Blue
		,Color.Red
		,Color.Lime
		,Color.DarkViolet
		,Color.White
		,Color.Yellow
		,Color.DarkMagenta
		,Color.Green
		,Color.SkyBlue
		,Color.Gold
		,Color.DarkGoldenrod
		,Color.DarkTurquoise
		,Color.Gray
		,Color.Magenta
		,Color.Pink
		};

		public static int GetBuffIndex(int theIndex)
		{
			int index = -1;
			switch (theIndex)
			{
				case 7:
					index = BuffID.ShadowFlame;
					break;
				case 8:
					index = BuffID.CursedInferno;
					break;
				case 9:
					index = BuffID.Frostburn;
					break;
				case 10:
					index = BuffID.OnFire;
					break;
				case 11:
					index = BuffID.Ichor;
					break;
				case 12:
					index = ModContent.BuffType<Buffs.MoonLightCurse>();
					break;
			}

			return index;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torch God Summon Minion");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/TorchGodSummon"); }
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.minion = true;
			AIType = 0;
		}

		public void ShootFlame(int damage, float knockBack, float speed2)
		{
			Vector2 mousePos = Main.MouseWorld;
			Projectile.ai[1] = 180;
			Projectile.localAI[0] = 180;
			Projectile.netUpdate = true;

			Vector2 speed = Vector2.Normalize(mousePos - Projectile.Center) * speed2;

			SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);

			Projectile.NewProjectile(Projectile.Center, speed, ModContent.ProjectileType<TorchGodSummonProjectile>(), damage, knockBack, Projectile.owner, Projectile.ai[0]);

		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];

			if (Projectile.ai[1] < 1)
				Lighting.AddLight(Projectile.Center, TorchColors[(int)Projectile.ai[0] % 16].ToVector3());

			Projectile.ai[1] -= 1;

			Projectile.localAI[0] -= 1;

			if (Projectile.localAI[0] < 0 && Projectile.localAI[0] > -10)
			{
				int dust = Dust.NewDust(Projectile.position + new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6) + Projectile.height / 4), Projectile.width, Projectile.height, DustID.AncientLight);
				Main.dust[dust].scale = 1.50f;
				Main.dust[dust].fadeIn = 1.2f;
				Main.dust[dust].color = TorchGodSummonMinion.TorchColors[(int)Projectile.ai[0] % 16];
				Main.dust[dust].velocity = -Vector2.UnitY * 3f;
				Main.dust[dust].alpha = 200;
				Main.dust[dust].noGravity = true;

				if (Projectile.localAI[0] == -1)
				{
					SoundEffectInstance snd = SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);
					if (snd != null)
					{
						snd.Pitch = 0.6f;
					}
				}
			}

			float us = 0f;
			float maxus = 0f;

			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (currentProjectile.active // Make sure the projectile is active
				&& currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
				&& currentProjectile.type == Projectile.type)
				{

					if (i == Projectile.whoAmI)
						us = maxus;
					maxus += 1f;

				}
			}

			if (!player.active || player.dead || ((us >= TorchGodSummon.MaxTorches(player) || player.HeldItem.type != ModContent.ItemType<TorchGodSummon>()) && Projectile.ai[1] < 1))
			{
				Projectile.Kill();
			}

			float angle = (us / maxus) * MathHelper.TwoPi;
			float dist = 64f;
			Vector2 wegohere = player.MountedCenter + Vector2.UnitX.RotatedBy(angle) * dist;

			Projectile.Center = wegohere;

			if (Projectile.localAI[0]>-10)
			SGAmod.PostDraw.Add(new PostDrawCollection(new Vector3(wegohere.X - Main.screenPosition.X, wegohere.Y - Main.screenPosition.Y, 128)));

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D textureTorch = SGAmod.ExtraTextures[118];

			if (textureTorch != null)
			{

				Texture2D textureFlame = Main.FlameTexture[0];

				Vector2 offset = new Vector2(11, 11);
				Rectangle rect = new Rectangle(Projectile.ai[1] > -9999990 ? 66 : 0, (int)(Projectile.ai[0] % 16) * 22, 22, 22);
				Rectangle rect2 = new Rectangle(0, (int)(Projectile.ai[0] % 16) * 22, 22, 22);

				Vector2 flameoffset = new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));


				spriteBatch.Draw(textureTorch, Projectile.Center - Main.screenPosition, rect, Color.White, Projectile.rotation, offset, new Vector2(1f, 1f), Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);

				spriteBatch.Draw(textureFlame, Projectile.Center + (flameoffset / 6f) - Main.screenPosition, rect2, (Color.White * 0.75f) * MathHelper.Clamp((1f - Projectile.localAI[0] - 5f) / 7f, 0f, 1f), Projectile.rotation, offset, new Vector2(1f, 1f), Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);

				flameoffset = new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));

				spriteBatch.Draw(textureFlame, Projectile.Center + flameoffset - Main.screenPosition, rect2, Color.White * MathHelper.Clamp((1f - Projectile.localAI[0] - 5f) / 10f, 0f, 1f), Projectile.rotation, offset, new Vector2(1f, 1f), Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
			}

			return false;
		}

	}


}
