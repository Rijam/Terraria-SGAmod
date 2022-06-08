using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Idglibrary;
using SGAmod.Buffs;
using Terraria.Graphics.Effects;

namespace SGAmod.Items.Consumables
{
	public class TimePotion : ModItem, IPotionCantBeInfinite
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Matrix Potion");
			Tooltip.SetDefault("'The very fabric of time itself folds around you, compressing the flow of anything passing by'\nGrants a aura around the player that greatly slows down any enemies or projectiles" +
				"\nTime counts down faster per enemy and projectile affected, bosses slowed make it count down 2X as fast\n" + Idglib.ColorText(Color.Orange, "Requires 3 Cooldown stacks, adds 150 seconds each"));
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count + 2 < player.SGAPly().MaxCooldownStacks && !player.HasBuff(Mod.Find<ModBuff>("MatrixBuff").Type);
		}

		public override bool ConsumeItem(Player player)
		{
			return true;
		}

		public override bool? UseItem(Player player)
		{
			player.SGAPly().AddCooldownStack(60 * 150, 3);
			return true;
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = 8;
			Item.value = 1000;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.buffType = Mod.Find<ModBuff>("MatrixBuff").Type;
			Item.buffTime = 60 * 60;
		}

		public override void AddRecipes()
		{
			CreateRecipe(3).AddIngredient(ItemID.LesserRestorationPotion).AddIngredient(ItemID.StrangeBrew).AddIngredient(ItemID.AncientCloth, 2).AddIngredient(mod.ItemType("VirulentBar"), 4).AddIngredient(mod.ItemType("Entrophite"), 40).AddIngredient(ItemID.DD2KoboldBanner).AddIngredient(ItemID.FossilOre, 10).AddIngredient(ItemID.Amber, 3).AddTile(TileID.AlchemyTable).Register();
		}
	}

	public class TimeEffect : ModProjectile
	{
		public virtual bool bounce => true;
		//public virtual float trans => 1f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beam Gun");
		}

		public override bool Autoload(ref string name)
		{
			SGAmod.RenderTargetsCheckEvent += SGAmod_RenderTargetsCheckEvent;
			SGAmod.RenderTargetsEvent += SGAmod_RenderTargetsEvent;
			SGAmod.PostUpdateEverythingEvent += SGAmod_PostUpdateEverythingEvent;
			return true;
		}

		public static RenderTarget2D DistortRenderTarget2D;
		public static int queueRenderTargetUpdate = 0;

		public static void SGAmod_RenderTargetsCheckEvent(ref bool yay)
		{
			yay &= !((DistortRenderTarget2D == null || DistortRenderTarget2D.IsDisposed));
		}

		public static void SGAmod_RenderTargetsEvent()
		{
			if (TimeEffect.queueRenderTargetUpdate > 1)
			{
				if (TimeEffect.DistortRenderTarget2D == null || TimeEffect.DistortRenderTarget2D.IsDisposed)
				{
					int width = Main.screenWidth / 2;
					int height = Main.screenHeight / 2;
					TimeEffect.DistortRenderTarget2D = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
				}
			}
		}

			private void SGAmod_PostUpdateEverythingEvent()
		{
			if (Main.dedServ)
				return;

			if (TimeEffect.queueRenderTargetUpdate > 0)
			{
				TimeEffect.queueRenderTargetUpdate -= 1;

				if (TimeEffect.queueRenderTargetUpdate > 1)
				{
					SGAmod_RenderTargetsEvent();

					if (TimeEffect.DistortRenderTarget2D != null && !TimeEffect.DistortRenderTarget2D.IsDisposed)
					{
						RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();

						//Main.spriteBatch.End();
						Main.graphics.GraphicsDevice.SetRenderTarget(TimeEffect.DistortRenderTarget2D);
						Main.graphics.GraphicsDevice.Clear(Color.Transparent);

						Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, default, default, default, Matrix.Identity);

						Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 0.50f, SpriteEffects.None, 0f);

						Main.spriteBatch.End();

						Main.graphics.GraphicsDevice.SetRenderTargets(binds);
					}

				}
				if (TimeEffect.queueRenderTargetUpdate == 1)
				{
					if (TimeEffect.DistortRenderTarget2D != null && !TimeEffect.DistortRenderTarget2D.IsDisposed)
					{
						TimeEffect.DistortRenderTarget2D.Dispose();
					}
				}
			}

		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			return true;
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			//projectile.hide = true;
			Projectile.timeLeft = 10;
			Projectile.penetrate = -1;
			AIType = ProjectileID.WoodenArrowFriendly;
			Projectile.damage = 0;
		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			//drawCacheProjsOverWiresUI.Add(index);
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override void AI()
		{


			Projectile.localAI[0] += 1f;

			Player player = Main.player[Projectile.owner];

			int buffid = player.FindBuffIndex(Mod.Find<ModBuff>("MatrixBuff").Type);

				if (player != null && player.active)
			{

				SGAPlayer modply = player.GetModPlayer<SGAPlayer>();

				Projectile.scale = Projectile.timeLeft / 60f;

				if (player.dead || buffid < 0)
				{
					//j
				}
				else
				{

					Projectile.timeLeft = Math.Min(Projectile.timeLeft + 2, 60);
					Projectile.ai[0] = 256;


					Vector2 mousePos = Main.MouseWorld;

					if (Projectile.owner == Main.myPlayer)
					{
						Vector2 diff = mousePos - player.Center;
						diff.Normalize();
						//projectile.velocity = player.velocity;
						Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
						Projectile.netUpdate = true;
					}

					Projectile.velocity = default(Vector2);
					Projectile.Center = (player.Center);

					float counterx = 0;

					if (player.buffTime[buffid] > 5)
					{

						for (int i = 0; i < Main.maxNPCs; i += 1)
						{
							NPC proj = Main.npc[i];
							if (proj != null && proj.active)
							{
								if (!proj.townNPC && (proj.Center - Projectile.Center).Length() < Projectile.ai[0])
								{
									//proj.position -= proj.velocity * 0.75f;
									proj.GetGlobalNPC<SGAnpcs>().TimeSlow += 3;
									if (Projectile.ai[1] < 1)
									{

										if (proj.boss)
											player.buffTime[buffid] -= 1;
										else
											counterx += 0.25f;


									}
								}
							}
						}

						player.buffTime[buffid] -= (int)counterx;
						if (player.buffTime[buffid] % 4 < ((counterx * 4) % 4))
							player.buffTime[buffid] -= (int)1;

						counterx = 0;

						int[] nonolist = { Mod.Find<ModProjectile>("HellionCascadeShot").Type, Mod.Find<ModProjectile>("HellionCascadeShot2") .Type};

						for (int i = 0; i < Main.maxProjectiles; i += 1)
						{
							Projectile proj = Main.projectile[i];
							if (proj != null && proj.active)
							{
								if (proj.hostile && (proj.Center - Projectile.Center).Length() < Projectile.ai[0])
								{
									if (nonolist.Any(type => type != proj.type))
									{
										for (int z = 0; z < proj.MaxUpdates; z += 1)
										{
											proj.position -= proj.velocity * 0.75f;
											counterx += 0.10f;
										}
									}

								}
							}
						}
					}

					player.buffTime[buffid] -= (int)counterx;
					if (player.buffTime[buffid] % 10 < ((counterx * 10) % 10))
						player.buffTime[buffid] -= (int)1;


				}
			}
			else
			{
				Projectile.Kill();
			}

		}

		public static void DrawDistort(SGAPlayer sga)
		{
			if (sga.Player.ownedProjectileCounts[ModContent.ProjectileType<Items.Consumables.TimeEffect>()] > 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, default, default, default, Main.GameViewMatrix.TransformationMatrix);

				Projectile[] array = (Main.projectile.Where(testby => testby.active && testby.owner == sga.Player.whoAmI && testby.type == ModContent.ProjectileType<Items.Consumables.TimeEffect>())).ToArray();
				if (array.Length > 0)
				{

					Projectile projectile = array[0];

					Filter filtershader = Filters.Scene["SGAmod:ScreenTimeDistort"];
					ScreenShaderData shader = filtershader.GetShader();

					float aspectRatio = Main.screenWidth / (float)Main.screenHeight;
					Vector2 position = (projectile.Center - Main.screenPosition) / new Vector2(Main.screenWidth, Main.screenHeight);
					position.X *= aspectRatio;
					float percent = projectile.timeLeft / 60f;

					//shader.UseImage(SGAmod.Instance.GetTexture("Fire"), 1, SamplerState.AnisotropicWrap);
					//shader.Apply();
					shader.Shader.Parameters["distortionTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Fire").Value);
					shader.Shader.Parameters["uTargetPosition"].SetValue(position);
					shader.Shader.Parameters["uScreenResolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
					shader.Shader.Parameters["uColor"].SetValue(new Vector3(2.25f, 10f * percent, 0f));
					shader.Shader.Parameters["uIntensity"].SetValue(1f);
					shader.Shader.Parameters["uOpacity"].SetValue(percent);
					shader.Shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly / 4f);
					shader.Shader.CurrentTechnique.Passes["TimeDistort"].Apply();

					Items.Consumables.TimeEffect.queueRenderTargetUpdate = 5;

					if (Items.Consumables.TimeEffect.DistortRenderTarget2D != null && !Items.Consumables.TimeEffect.DistortRenderTarget2D.IsDisposed)
						Main.spriteBatch.Draw(Items.Consumables.TimeEffect.DistortRenderTarget2D, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0f);

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.TransformationMatrix);

					int buffid = sga.Player.FindBuffIndex(SGAmod.Instance.Find<ModBuff>("MatrixBuff").Type);
					float timeleft = 0f;
					if (buffid > -1)
						timeleft = (float)sga.Player.buffTime[buffid];


					for (int i = 0; i < 360; i += 360 / 12)
					{
						float angle = MathHelper.ToRadians(i);
						Vector2 hereas = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 256;

						Vector2 drawPos = ((hereas * projectile.scale) + projectile.Center) - Main.screenPosition;
						Color glowingcolors1 = Color.White;//Main.hslToRgb((float)lightColor.R*0.08f,(float)lightColor.G*0.08f,(float)lightColor.B*0.08f);
						Main.spriteBatch.Draw(Main.blackTileTexture, drawPos, new Rectangle(0, 0, 80, 10), glowingcolors1 * projectile.scale, projectile.rotation + MathHelper.ToRadians(i), new Vector2(80, 5), new Vector2(1, 1) * projectile.scale, SpriteEffects.None, 0f);

					}

				}
			}
		}

		public override bool PreDrawExtras(SpriteBatch spriteBatch)
		{

			Player player = Main.player[Projectile.owner];
			SGAPlayer modplayer = player.GetModPlayer<SGAPlayer>();

			int buffid = player.FindBuffIndex(Mod.Find<ModBuff>("MatrixBuff").Type);
			float timeleft = 0f;
			if (buffid > -1)
				timeleft = (float)player.buffTime[buffid];

			Texture2D tex = ModContent.Request<Texture2D>("SGAmod/MatrixArrow");
			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.scale, MathHelper.ToRadians(timeleft * (360 / 60)) + MathHelper.ToRadians(-90), new Vector2(0, tex.Height / 2), new Vector2(2, 2) * Projectile.scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * Projectile.scale, MathHelper.ToRadians(timeleft * (360 / 60)) / 60f + MathHelper.ToRadians(-90), new Vector2(0, tex.Height / 2), new Vector2(1, 1) * Projectile.scale, SpriteEffects.None, 0f);

			return false;
		}


	}

}