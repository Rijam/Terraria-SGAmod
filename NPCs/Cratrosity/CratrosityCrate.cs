using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Cratrosity
{
	public class CratrosityCrate : ModNPC
	{
		public int counter = 0;
		private int realcounter = 0;
		public int cratetype = ItemID.WoodenCrate;
		public Vector2 apointzz = new Vector2(0, 0);
		protected virtual int CrateIndex => ItemID.WoodenCrate;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Servent of Microtransactions");
			Main.npcFrameCount[NPC.type] = 1;
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(0, 4) < 1 || cratetype > 2999)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, CrateIndex);
			}
		}
		public override string Texture
		{
			get { return "Terraria/Item_" + cratetype; }
		}
		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 40;
			NPC.defense = 0;
			NPC.lifeMax = 7500;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath43;
			NPC.value = 0f;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = -1;
			AIType = -1;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.value = 40000f;
			NPC.SGANPCs().treatAsNight = true;
		}
		public override void AI()
		{
			NPC.timeLeft = 900;
			realcounter += 1;
			if (apointzz == new Vector2(0, 0))
			{
				apointzz = new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-100, 100));
			}
			counter = counter + Main.rand.Next(-3, 15);
			int npctype = Mod.Find<ModNPC>("Cratrosity").Type;
			int npctype2 = Mod.Find<ModNPC>("Cratrogeddon").Type;
			int npctype3 = Mod.Find<ModNPC>("Hellion").Type;
			if (NPC.CountNPCS(npctype2) > 0) { npctype = Mod.Find<ModNPC>("Cratrogeddon").Type; }
			if (Hellion.Hellion.GetHellion() != null) { npctype = Hellion.Hellion.GetHellion().NPC.type; }
			if (NPC.CountNPCS(npctype) > 0)
			{
				NPC myowner = Main.npc[NPC.FindFirstNPC(npctype)];
				if (counter % 600 < 450)
				{
					NPC.velocity = (NPC.velocity + ((myowner.Center + apointzz - (NPC.position)) * 0.02f) * 0.035f) * 0.99f;
				}

			}
			else { NPC.active = false; }

		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return realcounter>100;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(Mod.Find<ModBuff>("Microtransactions").Type, 200, true);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D mainTex = Main.itemTexture[CrateIndex];
			if (GetType() == typeof(CratrosityCrateOfSlowing) || GetType().IsSubclassOf(typeof(CratrosityCrateOfSlowing)))
				mainTex = Main.npcTexture[NPC.type];
			//if (GetType() == typeof(CratrosityCrateDankCrate))
			//mainTex = ModContent.GetTexture(Texture);

			Main.spriteBatch.Draw(mainTex, NPC.Center - Main.screenPosition, null, drawColor, 0, mainTex.Size()/2f, NPC.scale, default, 0);
			return false;
		}


    }
}

