using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;


namespace SGAmod.HavocGear.Projectiles
{
    public class ThrownTridentFriendly : ThrownTrident
    {
        public override void SetStaticDefaults()
	    {
		    DisplayName.SetDefault("Trident");
	    }
		
		public override void SetDefaults()
        { 
            Projectile.width = 12;      
            Projectile.height = 12; 
            Projectile.friendly = true;     
            // projectile.melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
            Projectile.Throwing().DamageType = DamageClass.Throwing;
            Projectile.tileCollide = true;   
            Projectile.penetrate = 5;     
            Projectile.timeLeft = 500;  
            Projectile.light = 0.25f;   
            Projectile.extraUpdates = 1;
   		    Projectile.ignoreWater = true;   
        }

        public override string Texture
        {
            get { return("SGAmod/HavocGear/Projectiles/ThrownTrident"); }
        }

        public override void AI()           //this make that the projectile will face the corect way
        {
            Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;  
		Projectile.velocity=Projectile.velocity+new Vector2(0,0.2f);
       }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            bool facingleft = Projectile.velocity.X>0;
            Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
            Texture2D texture = Main.projectileTexture[Projectile.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle?(), drawColor, Projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f), origin, Projectile.scale, facingleft ? effect : SpriteEffects.None, 0);
            return false;
        }
    }
}