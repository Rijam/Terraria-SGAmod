using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles.Pets
{
	public class MiniPalladiumWraith : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mini Palladium Wraith");
			Main.projFrames[Projectile.type] = 5;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Parrot);//Vanilla Projectile_208
			Projectile.width = 18;
			Projectile.height = 36;
			AIType = ProjectileID.Parrot;
			drawOffsetX = -12;
			drawOriginOffsetY -= 2;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.parrot = false; // Relic from AIType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (player.HasBuff(ModContent.BuffType<Buffs.Pets.MiniPalladiumWraithBuff>()))
            {
                Projectile.timeLeft = 2;
            }
		}
	}
}