using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
    public class MossYoyoProj : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quagmire");
			ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 3f;
			ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 160f;
			ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 14f;
		}
       
	    public override void SetDefaults()
        {
        	Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Amarok);
			Projectile.extraUpdates = 0;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = 99;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.scale = 1f;
        }

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(0, 100) < 40 && !target.boss)
				target.AddBuff(ModContent.BuffType<Buffs.DankSlow>(), 60 * 40);
		}
	}
}