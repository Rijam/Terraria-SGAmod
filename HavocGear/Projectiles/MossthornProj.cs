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
    public class MossthornProj : ProjectileSpearBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mossthorn");
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

            movein=3f;
            moveout=3f;
            thrustspeed=3.0f;
        }

        public new float movementFactor
        {
            get { return Projectile.ai[0]; }
            set { Projectile.ai[0] = value; }
        }
    }
}