using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
	public class ThermalPikeProj : ProjectileSpearBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Pike");
		}

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            bool facingleft = Projectile.Center.X < Main.player[Projectile.owner].Center.X;
            Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.None;
            Texture2D texture = Main.projectileTexture[Projectile.type];
            Texture2D glow = Mod.Assets.Request<Texture2D>("Items/GlowMasks/ThermalPike_Glow").Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), drawColor, Projectile.rotation + (0.50f * MathHelper.Pi) + (facingleft ? (1.5f * MathHelper.Pi) : 0), origin, Projectile.scale, facingleft ? effect : SpriteEffects.FlipVertically, 0);
            Main.spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, new Rectangle?(), Color.White, Projectile.rotation  + (0.50f * MathHelper.Pi) + (facingleft ? (1.5f * MathHelper.Pi) : 0), origin, Projectile.scale, facingleft ? effect : SpriteEffects.FlipVertically, 0);
            return false;
        }

        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.ownerHitCheck = true;
            Projectile.aiStyle = 19;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 90;
            Projectile.hide = true;

            movein=1.6f;
            moveout=0.50f;
            thrustspeed=5f;
        }
        public override void AI()
        {
		base.AI();
        if (Main.rand.Next(0,30)<25){
        Vector2 adder=new Vector2(Main.rand.Next(-100,100),Main.rand.Next(-100,100))*0.15f;
        Vector2 center=new Vector2(Projectile.position.X+(float)(Projectile.width / 2),Projectile.position.Y+(float)(Projectile.width / 2));
        Vector2 launchvector=new Vector2((float)(Math.Cos(truedirection)),(float)(Math.Sin(truedirection)));
        int dust = Dust.NewDust((center+(launchvector*((Main.rand.Next(-50,80))*0.25f)))+adder, 0, 0, 6);
        Main.dust[dust].scale = 0.8f;
        Main.dust[dust].noGravity = false;
        Main.dust[dust].velocity = Projectile.velocity*(float)(Main.rand.Next(20,100)*0.002f);
        }
		Lighting.AddLight(Projectile.position, 0.6f, 0.5f, 0f);
	}

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += (target.defense / 4);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if(!(Main.rand.Next(8) == 0))
            {
            Player player = Main.player[Projectile.owner];
            target.immune[Projectile.owner] = 2;
            target.AddBuff(Mod.Find<ModBuff>("ThermalBlaze").Type, 120);
            }
        }
    }
}