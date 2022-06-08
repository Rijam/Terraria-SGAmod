
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace SGAmod.Items.Armors.Magatsu
{

	[AutoloadEquip(EquipType.Head)]
	public class MagatsuHood : ModItem
	{
        public override bool Autoload(ref string name)
        {
			//if (GetType() == typeof(ValkyrieHelm))
				//SGAPlayer.PostUpdateEquipsEvent += SetBonus;

			return true;
        }
		protected string tooltip = "1% increased Apocalyptical Chance\n20% increased Apocalyptical Strength";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magatsu Hood");
			Tooltip.SetDefault(tooltip);
		}

		public static void ActivateDecoy(SGAPlayer sgaplayer)
        {
			if (sgaplayer.AddCooldownStack(60 * 60, 2))
            {
				bool decoyExists = Main.npc.Where(testby => testby.active && testby.type == ModContent.NPCType<MagatsuDecoy>() && testby.ai[3] == sgaplayer.Player.whoAmI).Count() > 0;
				if (decoyExists)
					return;

				Vector2 spot = sgaplayer.Player.Center;
				int npc2 = NPC.NewNPC((int)spot.X, (int)spot.Y, ModContent.NPCType<MagatsuDecoy>(), ai3: sgaplayer.Player.whoAmI);
				Main.npc[npc2].life = sgaplayer.Player.statLifeMax2*3;
				Main.npc[npc2].lifeMax = Main.npc[npc2].life;
				Main.npc[npc2].defense = sgaplayer.Player.statDefense*2;
				SoundEngine.PlaySound(SoundID.Item, (int)spot.X, (int)spot.Y, 78, 1f, -0.8f);
				var snd = SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, (int)spot.X, (int)spot.Y);
				if (snd != null)
                {
					snd.Pitch = -0.80f;
                }
			}
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0,15,0,0);
			Item.rare = ItemRarityID.Lime;
			Item.defense = 8;
			Item.lifeRegen = 0;
		}

		public static void SetBonus(SGAPlayer sgaplayer)
        {
			Player player = sgaplayer.Player;

			sgaplayer.apocalypticalStrength += 0.40f;
			for (int i = 0; i < sgaplayer.apocalypticalChance.Length; i += 1)
				sgaplayer.apocalypticalChance[i] += 2.0;

			sgaplayer.magatsuSet = true;

			if (player.ownedProjectileCounts[ModContent.ProjectileType<Items.Armors.Magatsu.ArmorBoundDarkSectorEye>()] < (player.maxMinions - player.SGAPly().GetMinionSlots))
			{
				Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<Items.Armors.Magatsu.ArmorBoundDarkSectorEye>(), 0, 0, player.whoAmI);
			}

		}

		/*public static void SetBonus(SGAPlayer sgaplayer)
		{

		}*/
		public Color ArmorGlow(Player player, int index)
		{
			return Color.White * 0.50f;
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod,typeof(SGAPlayer).Name) as SGAPlayer;

			sgaplayer.apocalypticalStrength += 0.20f;
			for (int i = 0; i < sgaplayer.apocalypticalChance.Length; i += 1)
				sgaplayer.apocalypticalChance[i] += 1.0;

		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[0] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowcolor[0] = ArmorGlow;
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<WovenEntrophite>(), 25).AddIngredient(ModContent.ItemType<StygianCore>(), 1).AddTile(TileID.Loom).Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class MagatsuRobes : MagatsuHood
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magatsu Robes");
			Tooltip.SetDefault(tooltip);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.defense = 12;
			Item.lifeRegen = 0;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[1] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowmasks[2] = "SGAmod/Items/GlowMasks/" + Name + "_GlowArms";
				sgaplayer.armorglowmasks[4] = "SGAmod/Items/GlowMasks/" + Name + "_GlowFemale";
				sgaplayer.armorglowcolor[1] = ArmorGlow;
				sgaplayer.armorglowcolor[2] = ArmorGlow;
				sgaplayer.armorglowcolor[4] = ArmorGlow;
			}
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class MagatsuPants : MagatsuHood
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magatsu Pants");
			Tooltip.SetDefault(tooltip);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Lime;
			Item.defense = 5;
			Item.lifeRegen = 0;
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(Mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[3] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowcolor[3] = ArmorGlow;
			}
		}
	}

	public class ExplosionDarkSectorEye : Dimensions.NPCs.SpookyDarkSectorEye, IPostEffectsDraw
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voided Null Seeker Explosion");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.extraUpdates = 2;
			Projectile.timeLeft = 60;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void AI()
		{
			Projectile.velocity /= 1.05f;

			Projectile.localAI[0] += 1;

		}

		public override void PostEffectsDraw(SpriteBatch spriteBatch, float drawScale = 2f)
		{

			float alpha = Math.Min((Projectile.timeLeft) / 30f,1f);

			if (alpha <= 0)
				return;

			Texture2D tex = ModContent.Request<Texture2D>("SGAmod/Dimensions/NPCs/NullWatcher");
			Rectangle rect = new Rectangle(0, (tex.Height / 7) * (2 + (int)(Math.Min(Projectile.localAI[0] / 10f, 4))), tex.Width, tex.Height / 7);
			Rectangle recteye = new Rectangle(0, 0, tex.Width, tex.Height / 7);

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 7) / 2f;

			float scale = (1f - (Projectile.timeLeft / 60f)) * 5f;

			for (int k = 0; k < 1; k++)//projectile.oldPos.Length
			{
				Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY)) / drawScale;
				float coloralpha = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

				spriteBatch.Draw(tex, drawPos, rect, Color.GreenYellow * coloralpha * 0.75f * alpha, Projectile.rotation, drawOrigin, Projectile.scale * 1f * scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(tex, drawPos + Vector2.Zero, recteye, Color.White * coloralpha * 0.75f * alpha, Projectile.rotation, drawOrigin, Projectile.scale * scale, SpriteEffects.None, 0f);
			}

		}
	}

		public class ArmorBoundDarkSectorEye : Dimensions.NPCs.SpookyDarkSectorEye, IPostEffectsDraw
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Voided Null Seeker");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.netImportant = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

		public override void AI()
		{
			Projectile.velocity /= 1.05f;

			Projectile.localAI[0] += 1;
			P = Main.player[Projectile.owner];

			bool remove = false;
			int index = 0;
			int countedindex = 0;
			
			foreach(Projectile projectile2 in Main.projectile.Where(testby => testby.active && testby.type == Projectile.type))
            {
				countedindex += 1;
				if (projectile2 == Projectile)
                {
					index = countedindex;
				}
			}
			//index -= 1;
			countedindex += 1;

			if (!P.active || P.dead || !P.SGAPly().magatsuSet || index > (P.maxMinions-P.SGAPly().GetMinionSlots))
			{
				Projectile.Kill();
				return;
			}

			Vector2 halfcircle = new Vector2(0,-P.gravDir*48f).RotatedBy(MathHelper.PiOver2 - ((index / (float)countedindex) * MathHelper.Pi))*new Vector2(1.00f,2.0f);
			Vector2 PCenter = P.Center;

			if (SGAPlayer.centerOverrideTimerIsActive > 0)
			{
				if (P.SGAPly().centerOverrideTimer > 0)
					PCenter = P.SGAPly().centerOverridePosition;
			}

			Vector2 gohere = PCenter + new Vector2(P.direction*0f,P.gravDir*48f)+halfcircle;
			Projectile.timeLeft = 3;

			if (Projectile.ai[1] < 1000)
            {
				lookat = Main.MouseWorld;
				Projectile.netUpdate = true;
			}

			Projectile.ai[0] = -1;

			eyeDist = MathHelper.Clamp((Projectile.ai[1] - 500) / 500f, 0f, 1f) * 4f;

			Projectile.ai[1] = MathHelper.Clamp(Projectile.ai[1] - 1f, 0f, 1250);
			int dists = 2400 * 2400;

			int inndx = 0;
			IEnumerable<NPC> targets = Main.npc.Where(testby => testby.active && !testby.dontTakeDamage && testby.chaseable && (testby.Center - Projectile.Center).LengthSquared() < dists && (testby.Center - Projectile.Center).LengthSquared() < dists * 3 && Collision.CanHitLine(testby.Center, 0, 0, P.Center, 0, 0)).OrderBy(testby => (testby.life));

			if (targets.Count() > 0)
			{
				//.OrderBy(testby => (testby.Center - projectile.Center)
				foreach (NPC target in targets)
				{
					Projectile.ai[1] += 2;
					inndx += 1;
					if (Projectile.ai[1] > 1000 && index == inndx)
					{
						Projectile.ai[0] = target.whoAmI;
					}
				}
			}

			if (Projectile.ai[0] >= 0)
            {
				NPC them = Main.npc[(int)Projectile.ai[0]];
				Vector2 dist = them.Center - Projectile.Center;
				gohere = them.Center+Vector2.Normalize(-dist).RotatedBy(MathHelper.Pi/6f)*64f;
				lookat = them.Center;
				if (dist.LengthSquared() < 240 * 240)
                {
					them.AddBuff(ModContent.BuffType<Buffs.Watched>(), 10);
				}
			}



			//Main.NewText(projectile.ai[1]);
				Projectile.velocity += (gohere - Projectile.Center) / 600f;
			Projectile.Center += (gohere - Projectile.Center) / (Projectile.ai[1]<1000 ? 5f : 100f);


		}

		public override void PostEffectsDraw(SpriteBatch spriteBatch, float drawScale = 2f)
        {
            float alpha = 1f;
            if (Projectile.ai[0] < 1000 && Projectile.localAI[0] >= 0)
                alpha = Math.Max((Projectile.localAI[0] - 60) / 200f, 0);
            if (Projectile.localAI[0] < 0)
                alpha = 1f + Projectile.ai[0] / 120f;

            if (alpha <= 0)
                return;

            Texture2D tex = ModContent.Request<Texture2D>("SGAmod/Items/Armors/Magatsu/MagatsuNullWatcher");
            Rectangle rect = new Rectangle(0, (tex.Height / 7) * (2+(int)(Math.Min(Projectile.ai[1]/250f,4))), tex.Width, tex.Height / 7);
            Rectangle recteye = new Rectangle(0, 0, tex.Width, tex.Height / 7);

            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 7) / 2f;

            for (int k = 0; k < 1; k++)//projectile.oldPos.Length
			{
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY)) / drawScale;
                float coloralpha = ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                float scale = (1f + (Projectile.localAI[0] < 0 ? -Projectile.localAI[0] / drawScale : 0)) * (2f / drawScale);

                spriteBatch.Draw(tex, drawPos, rect, Color.GreenYellow * coloralpha * 0.75f * alpha, Projectile.rotation, drawOrigin, Projectile.scale * 1f * scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(tex, drawPos + (lookat == null || lookat == Projectile.Center ? Vector2.Zero : (Vector2.Normalize(lookat - Projectile.Center) * eyeDist)), recteye, Color.White * coloralpha*0.75f * alpha, Projectile.rotation, drawOrigin, Projectile.scale * scale, SpriteEffects.None, 0f);
            }

        }

    }

	public class MagatsuDecoy : ModNPC, IPostEffectsDraw
	{

		public override bool Autoload(ref string name)
		{
			name = "Magatsu Decoy";
			return Mod.Properties.Autoload;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from .lang files, but the commented line below is the normal approach.
			// DisplayName.SetDefault("Example Person");
			Main.npcFrameCount[NPC.type] = 1;
			NPCID.Sets.ExtraFramesCount[NPC.type] = 1;
			NPCID.Sets.AttackFrameCount[NPC.type] = 1;
			NPCID.Sets.DangerDetectRange[NPC.type] = 0;
			NPCID.Sets.AttackType[NPC.type] = 0;
			NPCID.Sets.AttackTime[NPC.type] = 90;
			NPCID.Sets.AttackAverageChance[NPC.type] = 30;
			NPCID.Sets.HatOffsetY[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.townNPC = false;
			NPC.friendly = true;
			NPC.width = 32;
			NPC.height = 50;
			NPC.aiStyle = -1;
			NPC.damage = 0;
			NPC.noGravity = true;
			NPC.defense = 50;
			NPC.lifeMax = 500;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.knockBackResist = 0.75f;
			//npc.immortal = true;
			//AnimationType = NPCID.Guide;
			NPC.noGravity = true;
			NPC.homeless = true;
			//npc.rarity = 1;

		}
		public override string Texture
		{
			get
			{
				return "SGAmod/Projectiles/FieryRock";
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			return false;
        }

		public override bool PreAI()
		{
			NPC.localAI[0] += 1f;
			//npc.ai[0] = 0;
			//npc.ai[1] = 0;
			//npc.ai[2] = 0;
			//npc.ai[3] = 0;
			NPC.velocity /= 1.1f;

			//Main.NewText(npc.ai[3]);

			if (NPC.localAI[0]>150)
				NPC.life -= 1;

			if (NPC.ai[3] >= -90)
            {
				Player owner = Main.player[(int)NPC.ai[3]];

				if (owner.dead)
                {
					NPC.life = 0;
					return false;
                }

				if (owner.active)
                {
					SGAPlayer.centerOverrideTimerIsActive = 5;
					owner.SGAPly().centerOverrideTimer = (int)Math.Max(owner.SGAPly().centerOverrideTimer,3);
					owner.SGAPly().centerOverridePosition = NPC.Center;
                }

            }

			return true;
		}

        public void PostEffectsDraw(SpriteBatch spriteBatch, float drawScale = 2f)
		{
			int framesTotal = 20;
			int frame = 0;

			Texture2D texHead = Main.playerTextures[0, 0];
			Texture2D texBody = Main.playerTextures[0, 3];
			Texture2D texlegs = Main.playerTextures[0, 10];

			Vector2 drawPos = ((NPC.Center - Main.screenPosition));

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D[] texes = new Texture2D[] { texlegs, texBody, texHead };

			Terraria.Utilities.UnifiedRandom rand = new Terraria.Utilities.UnifiedRandom(NPC.whoAmI);

			float trans = MathHelper.Clamp(NPC.localAI[0] / 64f, 0f, 1f);
			float smooth = MathHelper.SmoothStep(128f, 0f, trans);

			for (int i = 0; i < 3; i += 1)
			{
				Vector2 loc = Vector2.UnitX.RotatedBy((rand.NextFloat(0.30f, 0.65f)*Main.GlobalTimeWrappedHourly * (rand.NextBool() ? 1f : -1f))+rand.NextFloat(MathHelper.TwoPi))*(rand.NextFloat(1f, 2f)) *(smooth);
				Texture2D tex = texes[i];
				Rectangle rect = new Rectangle(0, 0, tex.Width, tex.Height / 20);
				Rectangle rect2 = new Rectangle(0, frame * rect.Height, tex.Width, rect.Height);

				spriteBatch.Draw(tex, (drawPos+ loc)/ 2, rect2, Color.Black* trans, 0, rect.Size() / 2f, NPC.scale/2, SpriteEffects.FlipHorizontally, 0f);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
		}

	}


}