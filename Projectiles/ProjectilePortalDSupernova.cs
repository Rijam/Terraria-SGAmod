using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;


namespace SGAmod.Projectiles
{
    public class ProjectilePortalDSupernova : ProjectilePortal
    {
        public int counterfire=0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nova");
        }


        public virtual int projectilerate => 35;
        public virtual float angleoffset => 0;
        public virtual int manacost => 4;
        public virtual int startrate => 90;
        public virtual int drainrate => 5;
        public virtual int portalprojectile => Mod.Find<ModProjectile>("UnmanedBolt").Type;
        public virtual float portaldistfromsword => 60f;
        public virtual float velmulti => 8f;

        private int ogdamage = 0;

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 10;
            Projectile.light = 0.5f;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 90;
        }

        public virtual void WhileFiring()
        {
            //nothin



        }

        public override void AI()
        {

            Projectile.ai[0] = portalprojectile;
            base.AI();

            Player player = Main.player[Projectile.owner];
            if (player == null || (!player.channel || player.dead || (player.statMana < manacost || (player.HasBuff(BuffID.ManaSickness) && player.buffTime[player.FindBuffIndex(BuffID.ManaSickness)]>60*6))))
            {
                if (Projectile.timeLeft > 29)
                {
                    Projectile.timeLeft = 29;
                }
            }
            else
            {

                if (counter - takeeffectdelay > 0)
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
                        player.HeldItem.noMelee = true;
                    }
                    int dir = Projectile.direction;
                    player.ChangeDir(dir);
                    player.itemTime = 40;
                    player.itemAnimation = 40;
                    player.HeldItem.useStyle = 5;
                    player.itemRotation = (float)Math.Atan2(Projectile.velocity.Y * dir, Projectile.velocity.X * dir)+(angleoffset* player.direction);
                    Projectile.damage=player.GetWeaponDamage(player.HeldItem);

                    WhileFiring();

                    Projectile.Center = player.Center + (Projectile.velocity * portaldistfromsword);
                    Projectile.velocity *= velmulti;

                    if (player.manaRegenDelay<15)
                    player.manaRegenDelay = 15;
                    if (Projectile.timeLeft < timeleftfirerate && player.channel)
                    {
                        if (ogdamage == 0)
                        {
                            ogdamage = Projectile.damage;
                        }
                        else
                        {
                            Projectile.damage = (int)(ogdamage * (1f - player.manaSickReduction));
                        }
                        SoundEngine.PlaySound(SoundID.Item20, player.Center);
                        Projectile.timeLeft = Math.Max(startrate - (int)counterfire * drainrate, projectilerate);
                        counterfire += 1;
                        player.CheckMana(player.HeldItem, (int)(manacost), true);

                    }
                }
                else
                {
                  if (player == null || (!player.channel || player.dead || !player.CheckMana(player.HeldItem, (int)(manacost), false)))
                  Projectile.Kill();

                }
            }


        }




    }


}