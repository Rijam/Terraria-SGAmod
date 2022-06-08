using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Items.Tools;
using SGAmod.NPCs.SpiderQueen;
using Idglibrary;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class AcidScepter : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Scepter");
			Tooltip.SetDefault("Hold the left mouse button to charge up to 10 Acid Venom shots\nRelease to send them flying at the mouse cursor\nHas debuff splash damage, has higher DOT than other acid weapons");
			Item.staff[Item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			Item.damage = 22;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 5;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 35;
			Item.useAnimation = 35;
            Item.channel = true;
			Item.useStyle = 5;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.knockBack = 5;
			Item.value = 40000;
			Item.rare = 3;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("AcidScepterProj").Type;
			Item.shootSpeed = 4f;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("UnmanedStaff"), 1).AddIngredient(mod.ItemType("VialofAcid"), 10).AddTile(TileID.Anvils).Register();
		}
	}

    public class AcidScepterProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Acid Scepter");
        }

        public override string Texture
        {
            get { return ("Terraria/Projectile_" + 14); }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.Boulder);
            AIType = ProjectileID.Boulder;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 10;
            Projectile.light = 0.5f;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.tileCollide = false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool PreKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);
            
            return true;
        }

        public override void AI()
        {

            int DustID2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("AcidDust").Type, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 20, default(Color), 1f);
            Main.dust[DustID2].noGravity = true;
            Player player = Main.player[Projectile.owner];
            Projectile.ai[1] += 1;
            if (player.dead)
            {
                Projectile.Kill();
            }
            else
            {
                if ((Projectile.ai[0] > 0 || player.statMana<1) || !player.channel)
                {
                    Projectile.ai[0] += 1;
                    if (Projectile.ai[0] == 4)
                    {
                        for(int pro = 0; pro < Main.maxProjectiles; pro += 1)
                        {
                            Projectile proj = Main.projectile[pro];
                            if (proj.ai[0] == 0 && proj.type == ModContent.ProjectileType<AcidScepterVenom>() && proj.owner==Projectile.owner)
                            {
                                proj.ai[0] = 1;
                                proj.netUpdate = true;
                            }
                        }
                        Projectile.Kill();
                    }
                }
                else
                {
                    Projectile.timeLeft = 30;
                }
                if (Projectile.ai[0] < 3)
                {
                    Vector2 mousePos = Main.MouseWorld;

                    if (Projectile.owner == Main.myPlayer)
                    {
                        Vector2 diff = mousePos - player.Center;
                        diff.Normalize();
                        Projectile.velocity = diff;
                        Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                        Projectile.netUpdate = true;
                        Projectile.Center = mousePos;
                    }
                    Projectile.position -= Projectile.Center;
                    int dir = Projectile.direction;
                    player.ChangeDir(dir);
                    player.itemTime = 40;
                    player.itemAnimation = 40;
                    float rotvalue = MathHelper.ToRadians(-90f + (float)Math.Sin(Projectile.ai[1] / 10f) * 35f);
                    if (player.manaRegenDelay<15)
                    player.manaRegenDelay = 15;
                    if (Projectile.ai[0] > 0)
                    {
                        player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir);
                    }
                    else
                    {
                        player.itemRotation = MathHelper.ToRadians(-90f*player.direction + (float)Math.Sin(Projectile.ai[1] / 10f) * 20f);

                        int counter = 0;

                        for (int pro = 0; pro < Main.maxProjectiles; pro += 1)
                        {
                            Projectile proj = Main.projectile[pro];
                            if (proj.ai[0] == 0 && proj.type == ModContent.ProjectileType<AcidScepterVenom>() && proj.owner == Projectile.owner)
                            {
                                counter += 1;
                            }
                        }

                        if (Projectile.ai[1] % 20 == 0 && Projectile.ai[1]>20 && counter<10)
                        {

                            int thisoned = Projectile.NewProjectile(player.Center.X, player.Center.Y-12, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), Mod.Find<ModProjectile>("AcidScepterVenom").Type, Projectile.damage, Projectile.knockBack, Main.myPlayer);
                            IdgProjectile.Sync(thisoned);
                            SoundEngine.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 9, 0.25f, -0.25f);
                            player.CheckMana(player.HeldItem, 5, true);

                        }

                    }

                    Projectile.Center = (player.Center + rotvalue.ToRotationVector2() * 26f) + new Vector2(0, -24);
                    Projectile.velocity *= 8f;

                }

            }


        }
    }

    public class AcidScepterVenom : SpiderVenom
    {
        private Vector2[] oldPos = new Vector2[6];
        private float[] oldRot = new float[6];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Acid Blast");
        }

        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 5;
            AIType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            if (Projectile.ai[0] < 1)
            {
                Player player = Main.player[Projectile.owner];
                float rotvalue = MathHelper.ToRadians(-90f + (float)Math.Sin(Projectile.ai[1] / 10f) * 20f);
                Vector2 toit = (player.Center + rotvalue.ToRotationVector2() * 26f) + new Vector2(0, -24);


                if ((Projectile.Center - toit).Length() > 8)
                {
                    Vector2 norm = toit - Projectile.Center;
                    norm.Normalize();
                    Projectile.velocity += norm * 0.075f;
                    Projectile.velocity *= 0.995f;
                }

                Projectile.timeLeft = 200*5;
            }
            else { Projectile.ai[0] += 1; }

            if (Projectile.ai[0] == 2 || Main.player[Projectile.owner].dead)
            {
                Player player = Main.player[Projectile.owner];
                Vector2 toit;
                if (Main.player[Projectile.owner].dead)
                    toit = Projectile.velocity;
                else
                    toit = Main.MouseWorld;
                if (Projectile.owner == Main.myPlayer)
                {
                    Vector2 norm = toit- Projectile.Center;
                    norm.Normalize();
                    Projectile.velocity = norm.RotatedByRandom(MathHelper.ToRadians(25))*9f;
                    Projectile.netUpdate = true;

                }

            }


        base.AI();
        }

    }


    }