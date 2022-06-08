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

namespace SGAmod.NPCs
{
	public class IceFairy : ModNPC
	{
		int shooting = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Fairy");
			Main.npcFrameCount[NPC.type] = 4;
		}
		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 40;
			NPC.damage = 60;
			NPC.defense = 8;
			NPC.lifeMax = 400;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.2f;
			NPC.aiStyle = 22;
			AIType = 0;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.value = 300f;
			NPC.coldDamage = true;
			banner = NPC.type;
			bannerItem = Mod.Find<ModItem>("IceFairyBanner").Type;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];
			bool underground = Items.Consumables.AcidicEgg.Underground(spawnInfo.spawnTileY);
			return !spawnInfo.playerInTown && !spawnInfo.invasion && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse && !underground && Main.dayTime && spawnInfo.Player.ZoneSnow && Main.hardMode ? 0.25f : 0f;
		}

		public override void NPCLoot()
		{

			for (int i = 0; i <= Main.rand.Next(3, 6); i++)
			{
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("IceFairyDust").Type);
			}

		}


		public override void AI()
		{
			NPC.spriteDirection = NPC.velocity.X > 0 ? -1 : 1;
			shooting = shooting + 1;
			if (shooting % 200 == 0)
			{
				NPC.netUpdate = true;
				Player P = Main.player[NPC.target];
				if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
				{
				}
				else
				{
					if (Collision.CanHitLine(new Vector2(NPC.Center.X, NPC.Center.Y), 1, 1, new Vector2(P.Center.X, P.Center.Y), 1, 1))
					{
						Idglib.Shattershots(NPC.Center, P.position, new Vector2(P.width, P.height), 118, NPC.lifeMax > 2000 ? 25 : 40, (float)Main.rand.Next(60, 80) / 6, 35, 2, true, 0, true, 100);
					}
				}


			}
		}


		public override void FindFrame(int frameHeight)
		{
			int frats = (int)((1 + Math.Sin(shooting / 15) * 1.5));

			NPC.frame.Y = frats * 40;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			/*Texture2D orgtex=mod.GetTexture("NPCs/TPD");
			Vector2 origin = new Vector2((float)orgtex.Width * 0.5f, (float)orgtex.Height * 0.5f);

			spriteBatch.End();

			var text = "something";
			var measure = Main.fontMouseText.MeasureString(text);
			var target = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)100, (int)100);
			var position = new Vector2(100, 100);

			//RenderTargetUsage.PreserveContents = true;
			Main.graphics.GraphicsDevice.SetRenderTarget(target);
			//Main.graphics.GraphicsDevice.Clear(Color.Black);
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			spriteBatch.Draw(orgtex, new Vector2((float)Main.rand.Next(0,100),(float)Main.rand.Next(0,100)),null, lightColor, 0f, origin,new Vector2(3f,3f), SpriteEffects.None, 0f);

			//Utils.DrawBorderStringFourWay(spriteBatch, encounterFont, "Test", Vector2.Zero.X, Vector2.Zero.Y, new Color(255, 255, 255, 150), new Color(0, 0, 0, 150), new Vector2());
			spriteBatch.End(); // call it anyway
			target.GraphicsDevice.SetRenderTarget(null);

			//target.GraphicsDevice.SetRenderTarget(null);
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);

			spriteBatch.Draw(target, npc.Center - Main.screenPosition,null, lightColor, npc.velocity.X/5f, origin,new Vector2(1f,1f), SpriteEffects.None, 0f);
			target = null;
			//RenderTargetUsage.PreserveContents = false;
			//Main.spriteBatch.Draw(target, position, Color.White);
			//spriteBatch.End(); // if Begin was called by game
			*/

			return true;

		}



	}
}
