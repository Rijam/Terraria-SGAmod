using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Buffs;
using SubworldLibrary;
using SGAmod.Items;
using Terraria.Audio;

namespace SGAmod.Dimensions.NPCs
{

    public class StygianVein : ModNPC,IPostEffectsDraw
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stygian Vein");
        }

        public override void SetDefaults()
        {
            NPC.width = 42;
            NPC.height = 42;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 1000;
            //npc.HitSound = SoundID.Ti;
            //npc.DeathSound = SoundID.LiquidsWaterHoney;
            NPC.value = 0f;
            NPC.knockBackResist = 1.1f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
            NPC.alpha = 0;
            NPC.rarity = 1;
            NPC.chaseable = false;
            NPC.SGANPCs().dotImmune = true;
            NPC.SGANPCs().TimeSlowImmune = true;
            for (int buff=0;buff<NPC.buffImmune.Length;buff++)
            {
                NPC.buffImmune[buff] = true;
            }
        }
        public override string Texture
        {
            get { return "Terraria/SunOrb"; }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            SoundEngine.PlaySound(SoundID.Tink, (int)NPC.Center.X, (int)NPC.Center.Y, 0, 1, 0.65f);
            NPC.ai[1] = ((int)damage*25)+120;
            if (NPC.life < 1)
            {
                SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_EtherianPortalDryadTouch, (int)NPC.Center.X, (int)NPC.Center.Y);
                if (sound != null)
                {
                    sound.Pitch = -0.5f;
                }
            }

        }

        public override void NPCLoot()
        {
            Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<StygianCore>());
            SpookyDarkSectorEye.Release(NPC.Center, false, Main.rand.NextVector2Circular(32f, 32f));
        }

        public override void AI()
        {
            NullWatcher.DoAwarenessChecks((int)NPC.ai[1], false, true, NPC.Center);
            NPC.ai[1] = 0;
            NPC.ai[0] += 1;
            NPC.velocity = new Vector2(0, (float)Math.Cos(NPC.ai[0] / 90f)) / 3f;
            foreach (NPC enemy in Main.npc.Where(testnpc => testnpc.active && !testnpc.friendly && testnpc.type != NPC.type && testnpc.Distance(NPC.Center) < 720))
            {
                enemy.AddBuff(BuffID.Invisibility, 120);
            }

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public void PostEffectsDraw(SpriteBatch spriteBatch,float drawScale=2f)
        {

            Texture2D inner = Main.npcTexture[NPC.type];

            Vector2 drawOrigin = inner.Size()/ 2f;


            Vector2 drawPos = (NPC.Center - Main.screenPosition)/ drawScale;
            float drawscale2 = 2f / drawScale;


            for (float i = 0; i < 1f; i += 0.10f)
            {
                float scale = (1f - 0.80f * (((Main.GlobalTimeWrappedHourly / 2f) + i) % 1f));
                Color color = Color.DarkMagenta * (1f - ((i + (Main.GlobalTimeWrappedHourly / 2f)) % 1f)) * 0.75f;

                float percent = (((Main.GlobalTimeWrappedHourly / 2f) + i) % 1f);

                Vector2 drawpos2 = new Vector2((float)Math.Sin((percent*MathHelper.Pi) - Main.GlobalTimeWrappedHourly*1.25f) *percent*26f, percent * -64f)* drawscale2;

                spriteBatch.Draw(inner, drawPos + drawpos2, null, color, i * MathHelper.TwoPi, drawOrigin, drawscale2 * scale, SpriteEffects.None, 0f);
            }

            for (float i = 0; i < 1f; i += 0.10f)
            {
                float scale = (1f - 0.25f * (((Main.GlobalTimeWrappedHourly / 2f) + i) % 1f));
                Color color = Color.Magenta * (1f - ((i + (Main.GlobalTimeWrappedHourly / 2f)) % 1f)) * 0.75f;

                spriteBatch.Draw(inner, drawPos, null, color, i * MathHelper.TwoPi, drawOrigin, drawscale2 * scale, SpriteEffects.None, 0f);
            }

        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];

            if (DimDingeonsWorld.darkSectors.Count < 1)
                return 0f;

            float chance = 0f;

            if (Main.rand.Next(7) == 0)
            {
                Vector2 spawnPos = new Vector2(spawnInfo.spawnTileX * 16, spawnInfo.spawnTileY * 16);
                foreach (DarkSector sector in DimDingeonsWorld.darkSectors)
                {
                    if (!Main.npc.Any(npc => npc.active && npc.type == ModContent.NPCType<StygianVein>() && Vector2.Distance(npc.Center, spawnPos) <= 720)
                    && (sector.PointInside(new Vector2(spawnInfo.spawnTileX * 16, spawnInfo.spawnTileY * 16), 8)))
                        chance = 10f;
                }
            }
                return chance;
            
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return item.pick>0;
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            return player!=null && player.heldProj == projectile.whoAmI && player.HeldItem.pick>0;
        }
    }

}
