using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary.Bases;

namespace SGAmod.HavocGear.Projectiles
{
    public class TidalWaveProj : ProjectileSpearBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tidal Wave");
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.scale = 1.2f;
            Projectile.aiStyle = 19;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 90;
            Projectile.hide = true;

            movein=0.8f;
            moveout=0.2f;
            thrustspeed=3f;
        }

        public override void MakeProjectile()
        {
            Vector2 center=new Vector2(Projectile.position.X+(float)(Projectile.width / 2),Projectile.position.Y+(float)(Projectile.width / 2));
                Vector2 launchvector=new Vector2((float)(Math.Cos(truedirection)),(float)(Math.Sin(truedirection)));
                int num54 = Projectile.NewProjectile(center.X+launchvector.X*42,center.Y+launchvector.Y*42, launchvector.X*8, launchvector.Y*8, Mod.Find<ModProjectile>("TidalWaveProj2").Type, 1, 0f);
                Main.projectile[num54].damage=(int)(Projectile.damage);
                Main.projectile[num54].owner=Projectile.owner;
        }


    }

}