using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.HavocGear.Items.Accessories;
using Idglibrary;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using Terraria.Audio;

namespace SGAmod.Items.Accessories.Dedicated
{
	public class UkraineArms : HeliosFocusCrystal, IDedicatedItem
	{
		public override Color MainColor => Color.Yellow * 1f;
        public override Color BackColor => Color.Blue*1f;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ukrainian Valor");
			Tooltip.SetDefault("Grants a strong defensive buff against invasions on your town of 10 NPCs or more\nThis buff is granted to both Teammates and townNPCs\n"+Idglib.ColorText(Color.Blue,"My heart out to all those fighting for their nation")+"\n"+ Idglib.ColorText(Color.Yellow, "That you will survive this invasion on your homeland"));
		}

		public string DedicatedItem()
		{
			//tooltips.Add(new TooltipLine(mod, "Dedicated", 
			//Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly / 5f) % 1f, 0.45f, 0.65f);
			return "To the boundless valor displayed by Ukrainians during Russia's invasion";
		}

		public override string Texture => "SGAmod/Items/Accessories/Dedicated/UkraineArms";

        public override bool Autoload(ref string name)
        {
            SGAPlayer.PostPostUpdateEquipsEvent += UkrainianValorCheck;
            SGAnpcs.DoModifiesLateEvent += SGAnpcs_DoModifiesLateEvent;
			return true;
        }

		private void SGAnpcs_DoModifiesLateEvent(NPC npc, Player player, Projectile projectile, Item item, ref int sourcedamage, ref int damage, ref float knockback, ref bool crit)
		{
			int buff = ModContent.BuffType<UkraineValorBuff>();
			if (npc.HasBuff(buff))
			{
				damage = (int)(damage/200f);
			}
		}

        private void UkrainianValorCheck(SGAPlayer player)
        {
			int buff = ModContent.BuffType<UkraineValorBuff>();

			if (player.UkraineArms)
			{
				if (UkraineArms.IsItAPerfectlyGoodTimeForAnInvasion(player.Player))
				{
					if (player.UkraineArmsBuff < 1)
					{
						Projectile.NewProjectile(player.Player.Center, Vector2.Zero, ModContent.ProjectileType<UkraineArmsProj>(), 100, 0, player.Player.whoAmI);
					}
					player.Player.blackBelt = true;
					player.Player.AddBuff(buff, 60);
				}


				if (player.UkraineArmsBuff>0)
				{
					player.Player.statDefense = (int)(player.Player.statDefense * 1.25f);
					player.Player.endurance += 0.15f;
				}
			}
        }

		public static bool IsItAPerfectlyGoodTimeForAnInvasion(Player player,NPC npc = default) => (((Main.invasionProgressDisplayLeft > 0 && Main.invasionSize > 0) || Terraria.GameContent.Events.DD2Event.Ongoing) && (npc != default || (player != null && player.townNPCs >= 10)));

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaply = player.SGAPly();
			if (IsItAPerfectlyGoodTimeForAnInvasion(player))
			sgaply.UkraineArms = true;
		}

		public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
		{
			if (line.Mod == "Terraria" && line.Name == "ItemName")
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);

				Effect hallowed = SGAmod.HallowedEffect;

				Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.Blue);

				Color color = Color.Blue;

				hallowed.Parameters["alpha"].SetValue(1f);
				hallowed.Parameters["prismAlpha"].SetValue(1f);
				hallowed.Parameters["prismColor"].SetValue(Color.Blue.ToVector3());
				hallowed.Parameters["rainbowScale"].SetValue(0f);
				hallowed.Parameters["overlayScale"].SetValue(new Vector2(1f, 1f));
				hallowed.Parameters["overlayTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Voronoi").Value);
				hallowed.Parameters["overlayProgress"].SetValue(new Vector3(-Main.GlobalTimeWrappedHourly / 20f, -Main.GlobalTimeWrappedHourly / 4f, Main.GlobalTimeWrappedHourly * 1f));
				hallowed.Parameters["overlayAlpha"].SetValue(1.5f);
				hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1f, 0f, 0f));
				hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
				hallowed.CurrentTechnique.Passes["PrismNoRainbow"].Apply();

				Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.White);

				hallowed.Parameters["alpha"].SetValue(0.30f);
				hallowed.Parameters["prismAlpha"].SetValue(0.2f);
				hallowed.Parameters["prismColor"].SetValue(Color.Gold.ToVector3());
				hallowed.Parameters["rainbowScale"].SetValue(0f);
				hallowed.Parameters["overlayScale"].SetValue(new Vector2(0.05f, 2f));
				hallowed.Parameters["overlayTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("SmallLaserHorz").Value);
				hallowed.Parameters["overlayProgress"].SetValue(new Vector3(Main.GlobalTimeWrappedHourly / 20f, Main.GlobalTimeWrappedHourly / 2f, Main.GlobalTimeWrappedHourly * 1f));
				hallowed.Parameters["overlayAlpha"].SetValue(1.5f);
				hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1f, 0f, 0f));
				hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
				hallowed.CurrentTechnique.Passes["PrismNoRainbow"].Apply();

				Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.White);

				hallowed.Parameters["alpha"].SetValue(0.20f);
				hallowed.Parameters["prismAlpha"].SetValue(1f);
				hallowed.Parameters["prismColor"].SetValue(Color.Gold.ToVector3());
				hallowed.Parameters["rainbowScale"].SetValue(0f);
				hallowed.Parameters["overlayScale"].SetValue(new Vector2(0.05f, 2f));
				hallowed.Parameters["overlayTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("SmallLaserHorz").Value);
				hallowed.Parameters["overlayProgress"].SetValue(new Vector3(Main.GlobalTimeWrappedHourly / 20f, 0.5f+(Main.GlobalTimeWrappedHourly / 2f), Main.GlobalTimeWrappedHourly * 1f));
				hallowed.Parameters["overlayAlpha"].SetValue(1.5f);
				hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1f, 0f, 0f));
				hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
				hallowed.CurrentTechnique.Passes["PrismNoRainbow"].Apply();

				Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.White);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
				return false;
			}
			return true;
		}

		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.width = 16;
			Item.height = 16;
			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}

	}

	public class UkraineValorBuff : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/UkraineValorBuff";
			return true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ukrainian Valor");
			Description.SetDefault("Banding together against an opressive foe...\nGives players Black Belt, 25% more defense, and 15% increased endurance\nGives Friendly NPCs 50% damage resistance and heals them");
			Main.debuff[Type] = false;
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (UkraineArms.IsItAPerfectlyGoodTimeForAnInvasion(player))
			{
				int buffType = ModContent.BuffType<UkraineValorBuff>();
				//if (player.buffTime[buffIndex] % 20 == 0)
				//{
					//player.AddBuff(buffType, 60);
				//}

			}


			//GiveValor(default,player, ref buffIndex);
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (UkraineArms.IsItAPerfectlyGoodTimeForAnInvasion(default, npc) && npc.townNPC)
			{
				int buffType = ModContent.BuffType<UkraineValorBuff>();
				if (npc.buffTime[buffIndex] % 2 == 0)
				{
					//npc.AddBuff(buffType, 60);
				}
				npc.lifeRegen += 10;
			}

			//GiveValor(npc, default, ref buffIndex);
		}
	}
	public class UkraineArmsProj : ModProjectile
	{
		public List<UkraineFlagDust> flagDusts = new List<UkraineFlagDust>();
		public class UkraineFlagDust : HavocGear.Items.Weapons.StardusterCharging.StardusterProjectile
		{
			public float rotation = 0;
			public float rotationAdd = 0;
			public float fadeTime = 1f;
			public float fadeIn = 1f;
			public float alpha = 1f;
			public Color Color => yellow ? Color.Blue : Color.Yellow;
			public bool yellow = false;

			public override Vector2 Position
			{
				get
				{
					return position;
				}
			}
			public UkraineFlagDust(Vector2 position, Vector2 velocity, int timeLeft = 30,bool yellow = false) : base(position, velocity)
			{
				this.yellow = yellow;
				scale = Vector2.One;
				this.position = position;
				this.velocity = velocity;
				this.timeLeft = timeLeft;
				timeLeftMax = timeLeft;
				rando = Main.rand.Next();
				rotation = Main.rand.NextFloat(MathHelper.TwoPi);
				rotationAdd = Main.rand.NextFloat(-1f, 1f) * 0.15f;
			}
			public override void Update()
			{
				rotation += rotationAdd;
				base.Update();
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ukraine's Bravery");
		}
		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 30;
			Projectile.tileCollide = false;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.FallingStar; }
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			return true;
		}
		public override void AI()
		{

			float timeTime = MathHelper.Clamp(Projectile.timeLeft / 30f, 0f, Math.Min(Projectile.localAI[0] / 20f, 1));
			float realTimeLeft = MathHelper.SmoothStep(0f, 1f, timeTime);
			Vector2 scaler = (Vector2.One * (Math.Max(1f, 1f + (float)Math.Sin(Projectile.localAI[0] / 6f) * 0.2f)) * realTimeLeft);

			Projectile.localAI[0] += 1;

			Entity owner = null;
			if (Projectile.owner>=255 || Projectile.owner<0)
			{
				owner = Main.npc[(int)Projectile.ai[0]];
			}
			else
			{
				owner = Main.player[Projectile.owner];
			}

			if (Projectile.localAI[0] == 1)
            {
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/P5defense").WithVolume(1f).WithPitchVariance(.25f), Projectile.Center);
			}


			if (owner != null && owner.active)
			{
				bool ownerExists = false;
				if (owner is Player ply && !ply.dead && ply.SGAPly().UkraineArms && (UkraineArms.IsItAPerfectlyGoodTimeForAnInvasion(ply)))
				{
					ownerExists = true;
					ply.SGAPly().UkraineArmsBuff = 60;
					ply.AddBuff(ModContent.BuffType<UkraineValorBuff>(), 10);

					List<NPC> helpedNPCs = new List<NPC>();

					foreach (NPC npcTown in Main.npc.Where(testby => testby.active && testby.SGANPCs().UkraineArmsBuff < 1 && (testby.townNPC && testby.type != NPCID.DD2EterniaCrystal) && (testby.Center - ply.MountedCenter).LengthSquared() < 1000000))
					{
						helpedNPCs.Add(npcTown);
					}

					foreach (NPC npcTown in helpedNPCs)
					{
						for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 2f)
						{
							Vector2 angle = ((f + Projectile.localAI[0] / 20f).ToRotationVector2() * new Vector2(npcTown.width/2f, 16f));
							UkraineFlagDust uadust = new UkraineFlagDust(npcTown.Center+new Vector2(0,npcTown.height/2) + angle, -Vector2.UnitY * Main.rand.NextFloat(1f, 1.5f) * ((float)npcTown.height/9f), Main.rand.Next(8, 12), f <= 0);
							uadust.scale = new Vector2(0.06f, 0.12f);
							uadust.rotationAdd = 0;
							uadust.rotation = 0;
							uadust.fadeIn = 1f;
							uadust.fadeTime = 1f;
							flagDusts.Add(uadust);
						}
						npcTown.SGANPCs().UkraineArmsBuff = 1;
						npcTown.AddBuff(ModContent.BuffType<UkraineValorBuff>(), 10);
					}
				}

				if (owner is NPC npc && UkraineArms.IsItAPerfectlyGoodTimeForAnInvasion(default,npc))
				{
					ownerExists = true;
					npc.SGANPCs().UkraineArmsBuff = 60;
					npc.AddBuff(ModContent.BuffType<UkraineValorBuff>(), 10);
				}

				if (ownerExists)
				{
					Projectile.timeLeft = 30;
					Projectile.Center = owner.Center + new Vector2(0, ((-owner.height)+16) - scaler.Y * 24f);
				}
			}

			for(float f=0;f<MathHelper.TwoPi; f += MathHelper.TwoPi / 2f)
            {
				Vector2 angle = ((f+Projectile.localAI[0]/20f).ToRotationVector2() * new Vector2(24f, 16f));

				UkraineFlagDust uadust = new UkraineFlagDust(Projectile.Center + angle, -Vector2.UnitY * Main.rand.NextFloat(1f, 1.25f)*1.25f,Main.rand.Next(16,24),f<=0);
				uadust.scale = new Vector2(0.1f, 0.2f);
				uadust.rotationAdd = 0;
				float smoothLerp = MathHelper.SmoothStep(uadust.velocity.ToRotation(),owner.velocity.ToRotation(), MathHelper.Clamp(owner.velocity.LengthSquared()/96f,0f,1f));
				uadust.rotation = smoothLerp + MathHelper.PiOver2;
				uadust.fadeIn = 1f;
				uadust.fadeTime = 1f;

				flagDusts.Add(uadust);

				uadust = new UkraineFlagDust(Projectile.Center + (angle * new Vector2(Main.rand.NextFloat(0f,1f), Main.rand.NextFloat(0f, 1f))), -Vector2.UnitY * Main.rand.NextFloat(1f, 1.45f), Main.rand.Next(38, Main.rand.Next(60, 120)), f <= 0);
				uadust.scale = new Vector2(0.075f, 0.075f);
				uadust.fadeIn = 0.40f;
				uadust.fadeTime = Main.rand.NextFloat(4f, 36f);

				flagDusts.Add(uadust);
			}

			flagDusts = flagDusts.Where(testby => testby.timeLeft > 0).ToList();
			flagDusts.ForEach(testby => testby.Update());
		}

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsBehindNPCs.Add(index);
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float timeTime = MathHelper.Clamp(Projectile.timeLeft/30f,0f,Math.Min(Projectile.localAI[0]/20f,1f));
			float introEffect = Projectile.localAI[0] < 300 || Projectile.timeLeft <28 ? 30f : 90f;
			float timeTimeRepeat = MathHelper.Clamp((Projectile.timeLeft / 30f), 0f, Math.Min((Projectile.localAI[0] % 320f) / introEffect, 1));

			float realTimeLeft = MathHelper.SmoothStep(0f, 1f, timeTime);
			float realTimeLeftRepeatGlow = MathHelper.SmoothStep(0f, 1f, timeTimeRepeat);
			Vector2 scaler = (Vector2.One * (Math.Max(1f, 1f + (float)Math.Sin(Projectile.localAI[0] / 6f) * 0.2f)) * realTimeLeft);


			Texture2D coat = Main.itemTexture[ModContent.ItemType<UkraineArms>()];
			Texture2D glow = ModContent.Request<Texture2D>("SGAmod/Glow");

			Vector2 drawPos = Projectile.Center;
			Vector2 coatCenter = coat.Size() / 2f;
			Vector2 glowCenter = glow.Size() / 2f;

			float appearGlow = Math.Min(1f, realTimeLeftRepeatGlow * 1f);
			float alphaGlow = Math.Min(1f - appearGlow * 1f, appearGlow);

			if (alphaGlow > 0)
			{
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

				Effect effect = SGAmod.TextureBlendEffect;

				effect.Parameters["coordMultiplier"].SetValue(new Vector2(1f, 1f));
				effect.Parameters["coordOffset"].SetValue(new Vector2(0f, 0f));
				effect.Parameters["noiseMultiplier"].SetValue(new Vector2(1f, 1f));
				effect.Parameters["noiseOffset"].SetValue(new Vector2(0f, 0f));

				effect.Parameters["Texture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Extra_49c").Value);
				effect.Parameters["noiseTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Extra_49c").Value);
				effect.Parameters["noiseProgress"].SetValue(realTimeLeftRepeatGlow);
				effect.Parameters["textureProgress"].SetValue(0f);
				effect.Parameters["noiseBlendPercent"].SetValue(1f);
				effect.Parameters["strength"].SetValue(alphaGlow);
				effect.Parameters["alphaChannel"].SetValue(false);

				effect.Parameters["colorTo"].SetValue(Color.Yellow.ToVector4());
				effect.Parameters["colorFrom"].SetValue(Color.Black.ToVector4());

				effect.CurrentTechnique.Passes["TextureBlend"].Apply();

				Main.spriteBatch.Draw(glow, drawPos - Main.screenPosition, null, Color.White, Projectile.rotation, glowCenter, appearGlow*4f, default, 0);

				effect.Parameters["strength"].SetValue(alphaGlow*2f);
				effect.Parameters["colorTo"].SetValue(Color.Blue.ToVector4());
				effect.Parameters["colorFrom"].SetValue(Color.Black.ToVector4());

				effect.CurrentTechnique.Passes["TextureBlend"].Apply();

				Main.spriteBatch.Draw(glow, drawPos - Main.screenPosition, null, Color.White, Projectile.rotation, glowCenter, 2f+(appearGlow * 1f), default, 0);

			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			foreach (UkraineFlagDust udust in flagDusts)
            {
				Color udustColor = udust.Color;
				float percentdust = udust.timeLeft / (float)udust.timeLeftMax;
				float timeLeftDust = MathHelper.Clamp(percentdust, 0f,Math.Min(udust.timeStart/12f,1f));
				float timeLeftDustAlpha = MathHelper.Clamp(percentdust*2f, 0f, Math.Min(udust.timeStart / udust.fadeTime, 1f));

				Vector2 dustPos = udust.Position;

				Main.spriteBatch.Draw(glow, dustPos - Main.screenPosition, null, udustColor* timeLeftDustAlpha* udust.fadeIn* realTimeLeft, udust.rotation, glowCenter, udust.scale*realTimeLeft * 4f, default, 0);
			}

			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 6f)
			{
				spriteBatch.Draw(coat, (drawPos + (Vector2.UnitX.RotatedBy(f + Main.GlobalTimeWrappedHourly * 2f) * 3f)) - Main.screenPosition, null, Color.Blue * 0.50f* realTimeLeft, 0, coatCenter, scaler, SpriteEffects.None, 0f);
			}


			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


			spriteBatch.Draw(coat, drawPos-Main.screenPosition, null, Color.Yellow* realTimeLeft, 0f, coatCenter, scaler, SpriteEffects.None, 0f);

			return false;
		}
	}
}