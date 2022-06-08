using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using Terraria.Audio;

namespace SGAmod.Items.Weapons.Trap
{
    public class NadeFlail : SpikeballFlail
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flail-O-Nades");
            Tooltip.SetDefault("Applies Sticky Grenades with a delayed blast to enemies it hits\nUnleashes long lasting Proxy Mines pretty much whenever it hits a wall\nFaster speeds release more\nHigh velocity impacts spawn a tier 3 Proxy Mine\nCounts as trap damage, doesn't crit\n'This is a REALLY great idea!'");
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 10;
            Item.damage = 50;
            Item.rare = 6;
            Item.noMelee = true;
            Item.useStyle = 5;
            Item.useAnimation = 10;
            Item.useTime = 24;
            Item.knockBack = 5f;
            Item.scale = 2f;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<NadeFlailBall>();
            Item.shootSpeed = 25.1f;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Melee;
            Item.channel = true;
            Item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(mod.ItemType("SpikeballFlail"), 1).AddIngredient(ItemID.ProximityMineLauncher, 1).AddIngredient(ItemID.StickyGrenade, 100).AddIngredient(ItemID.LandMine, 10).AddIngredient(ItemID.HallowedBar, 8).AddIngredient(ItemID.IllegalGunParts, 1).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
        }

    }

    public class NadeFlailBall : ModProjectile
    {
        float[] angles = new float[20];
        float[] dist = new float[20];
        bool doinit = false;
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.trap = true;
            Projectile.aiStyle = 15;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            
            if (oldVelocity.Length() > 3)
            {
                for (int num315 = 1; num315 < 0.5f + (oldVelocity.Length() / 6); num315 = num315 + 1)
                {
                    if (Main.player[Projectile.owner].ownedProjectileCounts[ProjectileID.ProximityMineI] < 30)
                    {
                        Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                        float velincrease = ((float)(num315 + 8) / 2f);
                        int thisone = Projectile.NewProjectile(Projectile.Center.X - Projectile.velocity.X, Projectile.Center.Y - Projectile.velocity.Y, randomcircle.X * velincrease, randomcircle.Y * velincrease, ProjectileID.ProximityMineI, (int)(Projectile.damage * 2.50), 0f, Projectile.owner, 0.0f, 0f);
                        Main.projectile[thisone].DamageType = DamageClass.Melee;
                        // Main.projectile[thisone].thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
                        Main.projectile[thisone].trap = true;
                        // Main.projectile[thisone].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
                        Main.projectile[thisone].localNPCHitCooldown = -1;
                        Main.projectile[thisone].usesLocalNPCImmunity = true;
                        Main.projectile[thisone].timeLeft = 60 * 20;
                        Main.projectile[thisone].netUpdate = true;
                        IdgProjectile.Sync(thisone);
                    }
                }

            }
            
            if (oldVelocity.Length() >  16)
            {
                for (int num315 = 15; num315 < 16; num315 = num315 + 1)
                {
                    if (Main.player[Projectile.owner].ownedProjectileCounts[ProjectileID.ProximityMineIII] < 3)
                    {
                        Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                        float velincrease = ((float)(num315 + 8) / 3f)*2f;
                        int thisone = Projectile.NewProjectile(Projectile.Center.X - Projectile.velocity.X, Projectile.Center.Y - Projectile.velocity.Y, randomcircle.X * velincrease, randomcircle.Y * velincrease, ProjectileID.ProximityMineIII, (int)(Projectile.damage * 5.00), 0f, Projectile.owner, 0.0f, 0f);
                        Main.projectile[thisone].DamageType = DamageClass.Melee;
                        // Main.projectile[thisone].thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
                        Main.projectile[thisone].trap = true;
                        // Main.projectile[thisone].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
                        Main.projectile[thisone].localNPCHitCooldown = -1;
                        Main.projectile[thisone].usesLocalNPCImmunity = true;
                        Main.projectile[thisone].netUpdate = true;
                        IdgProjectile.Sync(thisone);
                    }
                }

            }
            return true;
        }

        public override string Texture
        {
            get { return ("Terraria/Projectile_" + 14); }
        }


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nade Ball");
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int projer = Projectile.NewProjectile(Projectile.Center, Main.rand.NextVector2Circular(3f, 3f), ModContent.ProjectileType<NadeFlailStickyNade>(), Projectile.damage, Projectile.knockBack+5f,Projectile.owner);

            if (projer >= 0)
            {
                Projectile proj = Main.projectile[projer];

                proj.DamageType = DamageClass.Melee;
                // proj.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
                proj.trap = true;
                // proj.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
                proj.netUpdate = true;

                NadeFlailStickyNade flaier = proj.ModProjectile as NadeFlailStickyNade;
                flaier.StickTo(target);
            }

        }

        public override void AI()
        {
            if (doinit == false)
            {
                doinit = true;
                for (int num315 = 0; num315 < angles.Length; num315 = num315 + 1)
                {
                    angles[num315] = Main.rand.NextFloat((float)-Math.PI, (float)Math.PI);
                    dist[num315] = Main.rand.NextFloat(0f, 24f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {


                Texture2D texture = Main.chain2Texture;

            Vector2 position = Projectile.Center;
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = Projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1.35f, SpriteEffects.None, 0.0f);
                }
            }

            Matrix dothematrx = Matrix.CreateRotationZ(Projectile.rotation - MathHelper.ToRadians(90)) *
                Matrix.CreateScale(1f, 1f, 1f) *
            Matrix.CreateTranslation((new Vector3(Projectile.Center.X, Projectile.Center.Y, 0) - new Vector3(Main.screenPosition.X, Main.screenPosition.Y, 0)))
            * Main.GameViewMatrix.ZoomMatrix;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, dothematrx);
            for (int num315 = 0; num315 < angles.Length; num315 = num315 + 1)
            {
                Vector2 there = new Vector2((float)Math.Cos(angles[num315]), (float)Math.Sin(angles[num315])) * dist[num315];
                Texture2D tex2 = Main.itemTexture[ItemID.StickyGrenade];
                Main.spriteBatch.Draw(tex2, there, sourceRectangle, lightColor, 0, new Vector2(tex2.Width, tex2.Height)/2, 1f, SpriteEffects.None, 0.0f);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);


            return false;
        }
    }

	public class NadeFlailStickyNade : HavocGear.Projectiles.HotRound
    {

		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nade Flail Sticky Nide");
		}

        public override string Texture => "Terraria/Projectile_"+ProjectileID.StickyGrenade;

        public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = 200;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 3 * 60;
			Projectile.scale = 0.75f;
			AIType = 0;
		}

        public override bool CanDamage()
        {
            return false;
        }

        public override bool PreKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item, (int)Projectile.position.X, (int)Projectile.position.Y, 10);

            if (timeLeft < 2 && stickin >= 0)
            {
                var snd = SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
                if (snd != null)
                {
                    snd.Pitch = 0.75f;
                    if (SGAmod.ScreenShake < 16)
                        SGAmod.AddScreenShake(10, 1200, Projectile.Center);
                }

            }

				int proj = Projectile.NewProjectile(Projectile.Center, Vector2.Normalize(Projectile.velocity) * 2f, ProjectileID.StickyGrenade, Projectile.damage*2, Projectile.knockBack * 3f, Projectile.owner);
            if (proj >= 0)
            {
                Main.projectile[proj].DamageType = DamageClass.Melee;
                // Main.projectile[proj].thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
                // Main.projectile[proj].ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
                Main.projectile[proj].trap = true;
                Main.projectile[proj].timeLeft = 1;
                Main.projectile[proj].netUpdate = true;
            }

            return true;
		}

        public void StickTo(NPC npc)
        {
            Projectile.penetrate = 50;
            offset = (npc.Center - Projectile.Center);
            stickin = npc.whoAmI;
            Projectile.netUpdate = true;
        }

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//nope
		}
	}
}


