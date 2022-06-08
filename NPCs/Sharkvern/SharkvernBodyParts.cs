using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

//This file houses all the "Follow me" worm parts of the boss, I have GREATLY reduced the file size by parenting them

namespace SGAmod.NPCs.Sharkvern
{    
    public class SharkvernBase : ModNPC
    {
        public Color sharkGlowColor = Color.Transparent;

        public override void DrawEffects(ref Color drawColor)
        {
            sharkGlowColor = drawColor;
            base.DrawEffects(ref drawColor);
        }

        public override bool Autoload(ref string name)
        {
            return GetType() != typeof(SharkvernBase);
        }
    }

    public class SharkvernTail : SharkvernBase
    {

    public Vector2 localdist;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharkvern");
		}
		
		public override void SetDefaults()
        {
            NPC.width = 52;     
            NPC.height = 66;    
            NPC.damage = 25;
            NPC.defense = 5;
            NPC.lifeMax = 27000;
            NPC.knockBackResist = 0.0f;
            NPC.behindTiles = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.dontCountMe = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.chaseable = true;
            NPC.lavaImmune = false;
            NPC.aiStyle = -1;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.penetrate!=1 && (!SGAprojectile.IsTrueMelee(projectile, player)))
            damage = (int)(damage * (GetType() == typeof(SharkvernTail) ? 1f : 0.75f));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                Gore.NewGore(NPC.Center + new Vector2(0, 0), NPC.velocity / 2f, Mod.GetGoreSlot("Gores/Sharkvern_tail_gib"), 1f);
            }
        }

        public virtual void KeepUpright(float dirX, float dirY)
        {
        localdist=new Vector2(dirX,dirY);
        }

        public override bool CheckActive()
        {
            return !Main.npc[(int)NPC.ai[3]].active;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !NPC.dontTakeDamage;
        }

        public static void DoDust(NPC npc)
        {
            float devider = Main.rand.NextFloat(0,1f);
            float angle = MathHelper.TwoPi * devider;
            Vector2 thecenter = new Vector2((float)((Math.Cos(angle) * 80)), (float)((Math.Sin(angle) * 80)));
            thecenter = thecenter.RotatedByRandom(MathHelper.ToRadians(20));
            int DustID2 = Dust.NewDust(npc.Center + (thecenter * 2.5f), 0, 0, SGAmod.Instance.Find<ModDust>("TornadoDust").Type, thecenter.X * 0.8f, thecenter.X * 0.8f, 255-(int)(npc.Opacity*255f), default(Color), 1.5f);
            Main.dust[DustID2].noGravity = true;
            Main.dust[DustID2].velocity = new Vector2(thecenter.X * 0.2f, thecenter.Y * 0.2f) * -1f;

        }

        public override bool PreAI()
        {
            //npc.AddBuff(ModContent.BuffType<Buffs.LavaBurn>(),900);

            NPC.Opacity = MathHelper.Clamp(NPC.Opacity + (NPC.dontTakeDamage ? -0.01f : 0.02f), 0.2f, 1f);

            if (NPC.ai[3] > 0)
                NPC.realLife = (int)NPC.ai[3];
            if (NPC.target < 0 || NPC.target == byte.MaxValue || Main.player[NPC.target].dead)
                NPC.TargetClosest(true);
            if (!Main.npc[(int)NPC.ai[3]].active){
            NPC.timeLeft = 0;
            NPC.active=false;
          }else{NPC.timeLeft=500;}

            if (Main.netMode != 1)
            {
                if (!Main.npc[(int)NPC.ai[1]].active)
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                }
            }

            NPC.dontTakeDamage = Main.npc[(int)NPC.ai[1]].dontTakeDamage;
            if (NPC.dontTakeDamage)
                DoDust(NPC);

            if (NPC.ai[1] < (double)Main.npc.Length)
            {
               
                Vector2 npcCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
               
                float dirX = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - npcCenter.Y;
                KeepUpright(dirX,dirY);
                NPC.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - (float)NPC.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + posX;
                NPC.position.Y = NPC.position.Y + posY;
            }
            return false;
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[NPC.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            //Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor*npc.Opacity, npc.rotation, origin, npc.scale,localdist.X>0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;   
        }
    }


    public class SharkvernBody : SharkvernTail
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sharkvern");
        }

        public override string Texture
        {
            get { return ("SGAmod/NPCs/Sharkvern/SharkvernBody2"); }
        }

        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 48;
            NPC.damage = 36;
            NPC.defense = 25;
            NPC.lifeMax = 27000;
            NPC.knockBackResist = 0.0f;
            NPC.behindTiles = true;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.dontCountMe = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.chaseable = false;
            NPC.lavaImmune = false;
            NPC.aiStyle = -1;
        }

        public override bool PreAI()
        {
            base.PreAI();
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life < 1)
            {
                if (GetType() == typeof(SharkvernBody))
                    Gore.NewGore(NPC.Center + new Vector2(0, 0), NPC.velocity / 2f, Mod.GetGoreSlot("Gores/Sharkvern_body_gib"), 1f);

                if (GetType() == typeof(SharkvernNeck))
                {
                    Gore.NewGore(NPC.Center + new Vector2(16, 0).RotatedBy(NPC.rotation), NPC.velocity / 2f, Mod.GetGoreSlot("Gores/Sharkvern_body_gib"), 1f);
                    Gore.NewGore(NPC.Center + new Vector2(16, 0).RotatedBy(NPC.rotation), NPC.velocity / 2f, Mod.GetGoreSlot("Gores/Sharkvern_fin_gib"), 1f);
                }
                if (GetType() == typeof(SharkvernBody) || GetType() == typeof(SharkvernBody3))
                    Gore.NewGore(NPC.Center + new Vector2(0, 0), NPC.velocity / 2f, Mod.GetGoreSlot("Gores/Sharkvern_body_gib"), 1f);
            }

            if (NPC.life > 0 && Main.netMode != 1 && Main.rand.Next(1) == 0)
            {
                SharkvernHead jawsbrain = Main.npc[(int)NPC.ai[3]].ModNPC as SharkvernHead;
                float percent = Main.npc[(int)NPC.ai[3]].life;
                float percent2 = Main.npc[(int)NPC.ai[3]].lifeMax;
                if (jawsbrain.sergedout < 1 && (percent / percent2) < 0.8f)
                {
                    jawsbrain.sergedout = (int)(60f * (8f + ((percent / percent2) * (Main.expertMode ? 15f : 25f))));
                    int randomSpawn = Main.rand.Next(0);
                    if (randomSpawn == 0)
                    {
                        randomSpawn = Mod.Find<ModNPC>("AquaSurge").Type;
                    }

                    int num660 = NPC.NewNPC((int)(NPC.position.X + (float)(NPC.width / 2)), (int)(NPC.position.Y + (float)NPC.height), randomSpawn, 0, 0f, 0f, 0f, 0f, 255);
                }
            }
        }
    }


    public class SharkvernBody2 : SharkvernBody
    {

                public override string Texture
        {
            get { return ("SGAmod/NPCs/Sharkvern/SharkvernBody3"); }
        }
        
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 52;             
            NPC.height = 54;           
            NPC.damage = 36;
            NPC.defense = 45;
            NPC.lifeMax = 27000;
        }

    }

        public class SharkvernBody3 : SharkvernBody
    {

                public override string Texture
        {
            get { return("SGAmod/NPCs/Sharkvern/SharkvernBody4"); }
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 52;             
            NPC.height = 52;           
            NPC.damage = 36;
            NPC.defense = 45;
            NPC.lifeMax = 27000;
        }

    }


    public class SharkvernNeck : SharkvernBody
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sharkvern");
        }

        public override string Texture
        {
            get { return("SGAmod/NPCs/Sharkvern/SharkvernBody1"); }
        }
        
        public override void SetDefaults()
        {
            base.SetDefaults();
            NPC.width = 52;             
            NPC.height = 56;           
            NPC.damage = 36;
            NPC.defense = 45;
            NPC.lifeMax = 27000;
        }

        public override bool PreAI()
        {
        base.PreAI();
            if (NPC.ai[0] % 600 == 3)  //Npc spawn rate
            {
            //NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("DarkProbe"));  //NPC name
            }
            //No idea what npc was was meant to be, :/ -IDG
            //trying to spawn non-existant stuff can corrupt saves, this is here incase it's used

           return false;
        }


    }



}
