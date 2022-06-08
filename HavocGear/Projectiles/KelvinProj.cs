using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
    public class KelvinProj : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kelvin");
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 2.5f;
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 220f;
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 14f;
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
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			if (target.lavaImmune)
			{
				damage = (int)(damage * 1.25f);
			}

		}

        public override void AI()
	{
		if (Main.rand.Next(3) == 0)
		{
			Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("HotDust").Type);
		}
        int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6);
        Main.dust[dust].scale = 0.8f;
        Main.dust[dust].noGravity = false;
        Main.dust[dust].velocity = Projectile.velocity*(float)(Main.rand.Next(20,100)*0.005f);

		Lighting.AddLight(Projectile.position, 0.6f, 0.5f, 0f);
	}
	
	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        	Player player = Main.player[Projectile.owner];
		target.immune[Projectile.owner] = 2;
		target.AddBuff(ModContent.BuffType<Buffs.LavaBurn>(), 120);
		}
    }
}