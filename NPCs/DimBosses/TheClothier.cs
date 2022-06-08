using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.NPCs.Wraiths
{

	public class SkeletronClothier : ModNPC
	{
		public int bossvaluetimer = 0;
		public int phase = 0;
		public int aioverridetimer=0;
		public int aiattackstatething = 0;
		public Vector2 previousgo;
		public Vector2 Startloc;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeletron?");
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.NeedsExpertScaling[NPC.type] = true;
		}
		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 32;
			NPC.damage = 10;
			NPC.defense = 0;
			NPC.lifeMax = 10000;
			NPC.HitSound = SoundID.NPCHit5;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.knockBackResist = 1f;
			NPC.aiStyle = -1;
			NPC.boss = true;
			music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Copperig");
			//music =MusicID.Boss5;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.value = Item.buyPrice(0, 1, 0, 0);
		}
		public override string Texture
		{
			get { return ("Terraria/NPC_" + NPCID.SkeletronHead); }
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossLifeScale);
			NPC.damage = (int)(NPC.damage * 0.6f);
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.LesserRestorationPotion;
		}

		public override void NPCLoot()
		{
			//filler
		}
		private bool UpdateAI()
		{

			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest(false);
				Player P = Main.player[NPC.target];
				if (!P.active || P.dead)
				{
					return false;
				}
			}
			return true;
		}

		private void DoAI()
        {

			Vector2 gothere = P.Center - NPC.Center;
			float gotheredist = gothere.Length();
			gothere.Normalize();


		}

		Player P;
		float lifePercent;
		bool endReturn;

		public override bool PreAI()
		{
			if (Main.netMode < 1 && Main.LocalPlayer.name != "giuy")
				NPC.active = false;


			NPC.dontTakeDamage = false;
			if (bossvaluetimer < 1)
			{
				bossvaluetimer = 1;
				Startloc = NPC.Center;
			}
			endReturn = false;
			//npc.netUpdate = true;
			P = Main.player[NPC.target];
			lifePercent = (float)NPC.life / (float)NPC.lifeMax;

			if (NPC.ai[0] == 0f)
				NPC.ai[0] = 1f;

			NPC.dontTakeDamage = NPC.CountNPCS(NPCID.SkeletronHand) > 0;

			if (UpdateAI())
			{
				DoAI();
			}
			else
			{
				NPC.aiStyle = 11;
			}
				return endReturn;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D tex2 = Main.npcTexture[NPCID.Clothier];
			Texture2D tex = Main.npcTexture[NPC.type];

			int Clothcount = Main.npcFrameCount[NPCID.Clothier];
			spriteBatch.Draw(tex2, NPC.Center - Main.screenPosition, null, Color.Gray * 0.50f, NPC.rotation, tex.Size() / 2f, new Vector2(tex2.Width, tex2.Height/Clothcount)/2f, SpriteEffects.None, 0f);
			if (endReturn)
			spriteBatch.Draw(tex, NPC.Center - Main.screenPosition, null, Color.Gray*0.50f, NPC.rotation, tex.Size() / 2f, new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}
	}
}

