using Microsoft.Xna.Framework;
using SGAmod.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class ClayMore : ModItem
	{
		int clayCounter = 0;
		public override void SetDefaults()
		{
			base.SetDefaults();

			Item.damage = 24;
			Item.width = 19;
			Item.height = 22;
			Item.DamageType = DamageClass.Melee;
			Item.rare = 2;
			Item.useStyle = 1;
			Item.useAnimation = 25;
			Item.autoReuse = true;
			Item.useTime = 26;
			Item.useTurn = true;
			Item.knockBack = 9;
			Item.value = 1000;
			Item.shoot = 10;
			Item.shootSpeed = 10f;
			Item.consumable = false;
			Item.UseSound = SoundID.Item1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clay-More");
			Tooltip.SetDefault("Launches Clay from the player's inventory at a distance\nEvery 10th clay launches a clay pot instead\nThe sword swings slower when launching clay");
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.ClayBlock, 30).AddIngredient(mod.ItemType("BottledMud"), 2).AddIngredient(ItemID.Gel, 15).AddIngredient(ItemID.Vine, 2).AddTile(TileID.Anvils).Register();
		}
		private bool HasClay(Player player)
        {
			return ((player.Center - Main.MouseWorld).Length() > 300 && player.HasItem(ItemID.ClayBlock));
		}
        public override float MeleeSpeedMultiplier(Player player)
        {
			return player.SGAPly().claySlowDown>0 ? 0.75f : 1f;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			float rotation = MathHelper.Pi / 16f;
			Vector2 speed = new Vector2(speedX, speedY);
			Vector2 perturbedSpeed = (new Vector2(1f, 1f) * speed).RotatedBy((-rotation/2f)+(Main.rand.NextFloat()* rotation)); // Watch out for dividing by 0 if there is only 1 projectile.
			if (HasClay(player))
			{
				clayCounter += 1;
				clayCounter = clayCounter % 10;
				player.ConsumeItem(ItemID.ClayBlock);
				int typeproj = clayCounter == 9 ? ModContent.ProjectileType<ClayPot>() : ModContent.ProjectileType<ClayBall>();
				Projectile clayball = Projectile.NewProjectileDirect(position, perturbedSpeed, typeproj, damage/2, knockBack/5f, player.whoAmI);
				player.SGAPly().claySlowDown = (int)(player.itemTime * 1.25f);
				player.itemTime = (int)(player.itemTime * 1.25f);
			}

			return false;
        }

    }
}
namespace SGAmod.Projectiles
{

	public class ClayBall : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ball of Clay");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.MudBall);
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 300;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = 1;
			Projectile.DamageType = DamageClass.Melee;
			AIType = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.aiStyle = -1;
			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 10);
			for (int num315 = 0; num315 < 15; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Dirt, Projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), Projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 15f), 50, Color.SandyBrown, 1.5f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!target.friendly)
				Projectile.Kill();
		}

		public override void AI()
		{
			Projectile.rotation += ((Projectile.velocity.Length()/16f) * Math.Sign(-0.1f + (Projectile.velocity.X * 1.1f)))*0.1f;
			Projectile.velocity.Y += 0.25f;
			for (int num315 = 0; num315 < 2; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Dirt, 0f, 0f, 50, Color.SandyBrown, 1.0f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.3f;
			}

		}

	}

	public class ClayPot : ClayBall
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pot of Clay?!");
		}

        public override string Texture => "Terraria/Item_"+ItemID.ClayPot;

        public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 24;
			Projectile.height = 24;
		}

		public override bool PreKill(int timeLeft)
		{
			Projectile.aiStyle = -1;
			SoundEngine.PlaySound(SoundID.Shatter, (int)Projectile.position.X, (int)Projectile.position.Y, 0);
			for (int num315 = 0; num315 < 15; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Dirt, Projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), Projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 15f), 50, Color.SandyBrown, 1.5f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
			}

			for (int num315 = 0; num315 < 4; num315 = num315 + 1)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 2000)); randomcircle.Normalize();
				float velincrease = Main.rand.NextFloat(2f,6f);
				int thisone = Projectile.NewProjectile(Projectile.Center.X - Projectile.velocity.X, Projectile.Center.Y - Projectile.velocity.Y, randomcircle.X * velincrease, randomcircle.Y * velincrease, ModContent.ProjectileType<ClayBall>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack, Projectile.owner, 0.0f, 0f);
			}

			return true;
		}

	}

}
