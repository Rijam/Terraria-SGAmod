using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

using Terraria.Utilities;
using SGAmod.Tiles;
using SGAmod.Tiles.TechTiles;
using SGAmod.Buffs;

namespace SGAmod.Items.Weapons.Auras
{

	public class AuraStellarStaff : AuraStaffBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Main-Sequence Staff");
			Tooltip.SetDefault("Summons a Miniature Sun above the player");//\nThe radiance burns all in the aura\nPlayers recive immunity to burning debuffs\nLights a large area out of the blinding fog");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			int thetarget = -1;
			if (Main.LocalPlayer.ownedProjectileCounts[Item.shoot] > 0)
			{
				for (int i = 0; i < Main.maxProjectiles; i += 1)
				{
					if (Main.projectile[i].active && Main.projectile[i].type == Item.shoot && Main.projectile[i].owner == Main.LocalPlayer.whoAmI)
					{
						thetarget = i;
						break;
					}
				}
			}


			if (thetarget > -1 && Main.projectile[thetarget].active && Main.projectile[thetarget].type==Item.shoot)
			{
				AuraMinionStellar shoot = Main.projectile[thetarget].ModProjectile as AuraMinionStellar;
				tooltips.Add(new TooltipLine(Mod, "Bonuses", "Power Level: "+ shoot.thepower));
				tooltips.Add(new TooltipLine(Mod, "Bonuses", "Passive: Protects players against burning Debuffs, depending on minions put in"));
				tooltips.Add(new TooltipLine(Mod, "Bonuses", "In this order: Sunburn, OnFire!, Burning, Thermal Blaze, Lava Burn, Severe Lava Burn"));

				tooltips.Add(new TooltipLine(Mod, "Bonuses", "Each level scales the damage higher"));
				tooltips.Add(new TooltipLine(Mod, "Bonuses", "Each level scales a light that clears the fog"));


				if (shoot.thepower >= 1.0)
					tooltips.Add(new TooltipLine(Mod, "Bonuses", "Lv1: THEY WILL BURN!"));
				if (shoot.thepower >= 2.0)
					tooltips.Add(new TooltipLine(Mod, "Bonuses", "Lv2: Applies Lava Burn to enemies"));
				if (shoot.thepower >= 3.0)
					tooltips.Add(new TooltipLine(Mod, "Bonuses", "Lv3: Applies Daybroken to enemies"));

			}
		}

		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.knockBack = 2f;
			Item.mana = 15;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.buyPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<AuraStellarBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<AuraMinionStellar>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<HavocGear.Items.FieryShard>(), 20).AddIngredient(ModContent.ItemType<StygianCore>(), 1).AddIngredient(ModContent.ItemType<OverseenCrystal>(), 20).AddIngredient(ModContent.ItemType<HeliosFocusCrystal>(), 1).AddIngredient(ItemID.FragmentSolar, 10).AddTile(TileID.LunarCraftingStation).Register();
		}

	}

	public class AuraMinionStellar : AuraMinion
	{

		protected override int BuffType => ModContent.BuffType<AuraStellarBuff>();

        protected override float _AuraSize => 240f;
		private float timeLeftScale = 0f;

		public override float CalcAuraSize(Player player)
		{
			return (AuraSize * (float)Math.Pow((double)CalcAuraPowerReal(player), 0.60));
		}

		public override float CalcAuraPower(Player player)
		{
			float temp = 1f + (player.GetDamage(DamageClass.Summon) * (Projectile.minionSlots / 4f));
			return temp;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Sun");
			Main.projFrames[Projectile.type] = 1;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 60;
			Projectile.hide = true;
		}

		public override void AuraAI(Player player)
		{
			SGAmod.PostDraw.Add(new PostDrawCollection(new Vector3(SunOffset.X - Main.screenPosition.X, SunOffset.Y - Main.screenPosition.Y, 640)));
			Lighting.AddLight(Projectile.Center, Color.Orange.ToVector3() * 1f);
		}
		Player player => Main.player[Projectile.whoAmI];
		Vector2 SunOffset => Projectile.Center - new Vector2(0, player.gravDir * 128f);
		Color SequenceColor => Color.Lerp(Color.Lerp(Color.Red, Color.Yellow, MathHelper.Clamp((timeLeftScale - 1f) / 2f, 0f, 1f)), Color.CornflowerBlue, MathHelper.Clamp((timeLeftScale - 2.5f) / 2f, 0f, 1f));


		public override void InsideAura<T>(T type, Player player)
		{
			if (type is NPC)
			{
				float powerf = CalcAuraPowerReal(player);
				NPC himas = (type as NPC);
				himas.SGANPCs().impaled += (int)(Projectile.damage * powerf*(himas.realLife>=0 ? 0.75f : 1f));

				if (!(himas.townNPC || himas.friendly))
				{

					for (float i = 0; i < 2f; i += 1f)
					{
						Vector2 loc2 = Vector2.Normalize(himas.Center - SunOffset);

						int dustIndex = Dust.NewDust(himas.position, himas.width, himas.height, DustID.Fire, 0, 0, 150, default(Color), 1.5f);
						Main.dust[dustIndex].velocity = loc2 * Main.rand.NextFloat(1f, 3f) * powerf;
						Main.dust[dustIndex].noGravity = true;
						Main.dust[dustIndex].alpha = 200;
						Main.dust[dustIndex].color = SequenceColor;

						//Main.dust[dustIndex].fadeIn = -1f;
					}

					if (thepower >= 2)
					{
						himas.AddBuff(ModContent.BuffType<Buffs.LavaBurn>(), 3);
					}
					if (thepower >= 3)
					{
						himas.AddBuff(BuffID.Daybreak, 3);
					}
				}
			}
			if (type is Player alliedplayer && player.IsAlliedPlayer(alliedplayer))
			{
				int[] typesofdebuffs = new int[] { ModContent.BuffType<Sunburn>(),BuffID.OnFire, BuffID.Burning, ModContent.BuffType<ThermalBlaze>(), ModContent.BuffType<LavaBurnLight>(), ModContent.BuffType<LavaBurn>() };

				int index = 0;

				foreach (int typeofbuff in typesofdebuffs)
				{
					if (index < Projectile.minionSlots && alliedplayer.HasBuff(typeofbuff))
						alliedplayer.DelBuff(alliedplayer.FindBuffIndex(typeofbuff));

					index += 1;
				}
			}
		}

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsBehindNPCs.Add(index);
		}

        public override void AuraEffects(Player player, int type)
		{

			if (type == 1)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			}

			float trueScale = (float)Math.Pow(timeLeftScale, 0.75);

			if (type == 0)
			{
				timeLeftScale += (CalcAuraPowerReal(player) - timeLeftScale) / 30f;

				Lighting.AddLight(Projectile.Center, SequenceColor.ToVector3() * 0.75f);

				for (float i = 0; i < 8f; i += 1f)
				{
					float angle = MathHelper.ToRadians(Main.rand.NextFloat(0, 360));
					Vector2 loc2 = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
					Vector2 loc = loc2 * thesize * Main.rand.NextFloat(0.20f, 0.75f);


					int dustIndex = Dust.NewDust(SunOffset + loc, 0, 0, DustID.Fire, 0, 0, 150, default(Color), 1f);
					Main.dust[dustIndex].velocity = (Vector2.Normalize(loc2) * Main.rand.NextFloat(2f, 8f)) + player.velocity;
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].alpha = 200;
					//Main.dust[dustIndex].fadeIn = -1f;
					Main.dust[dustIndex].color = SequenceColor;
				}
			}

			UnifiedRandom rando = new UnifiedRandom(Projectile.whoAmI);

			if (type == 1)
			{

				Texture2D mainTex = Mod.Assets.Request<Texture2D>("Extra_57b").Value;
				Vector2 halfsize = mainTex.Size() / 2f;

				Effect RadialEffect = SGAmod.RadialEffect;
				float scale = 1f;// (1f+ CalcAuraPowerReal(player)*0.25f)*0.50f;

				Texture2D texture = ModContent.Request<Texture2D>("SGAmod/Stain");
				Texture2D texture2 = ModContent.Request<Texture2D>("SGAmod/Voronoi");
				Texture2D texture3 = ModContent.Request<Texture2D>("SGAmod/TiledPerlin");

				for (float scalaa = 0.2f; scalaa <= 1; scalaa += 0.50f)
				{
					for (float f = -1; f < 2; f += 2)
					{
						RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Stain").Value);
						RadialEffect.Parameters["alpha"].SetValue(MathHelper.Clamp(timeLeftScale / 2f,0f,1f));
						RadialEffect.Parameters["texOffset"].SetValue(new Vector2(Main.GlobalTimeWrappedHourly * 0.250f * f, -Main.GlobalTimeWrappedHourly * 0.575f));
						RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(5f, 0.5f));
						RadialEffect.Parameters["ringScale"].SetValue(0.40f* scalaa);
						RadialEffect.Parameters["ringOffset"].SetValue(0.22f);
						RadialEffect.Parameters["ringColor"].SetValue((SequenceColor.ToVector3() * 2f)*0.75f);
						RadialEffect.Parameters["tunnel"].SetValue(false);

						RadialEffect.CurrentTechnique.Passes["RadialAlpha"].Apply();

						Main.spriteBatch.Draw(texture, SunOffset - Main.screenPosition, null, Color.White, 0, texture.Size() / 2f, (0.75f + ((trueScale) * 1.50f)) * 2f * scale, SpriteEffects.None, 0f);
					}
				}

				RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.Assets.Request<Texture2D>("Stain").Value);
				RadialEffect.Parameters["alpha"].SetValue(MathHelper.Clamp(timeLeftScale / 2f, 0f, 2f));
				RadialEffect.Parameters["texOffset"].SetValue(new Vector2(0, -Main.GlobalTimeWrappedHourly * 0.575f));
				RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(3f, 0.75f));
				RadialEffect.Parameters["ringScale"].SetValue(0.14f);
				RadialEffect.Parameters["ringOffset"].SetValue(0.36f);
				RadialEffect.Parameters["ringColor"].SetValue((SequenceColor.ToVector3()*2f)*0.75f);
				RadialEffect.Parameters["tunnel"].SetValue(false);

				RadialEffect.CurrentTechnique.Passes["Radial"].Apply();
				Main.spriteBatch.Draw(texture, SunOffset - Main.screenPosition, null, Color.White, 0, texture.Size() / 2f, (0.75f + ((trueScale) * 1.50f)) * 1f * scale, SpriteEffects.None, 0f);


				SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(SequenceColor.ToVector4() * timeLeftScale);
				SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(texture);
				SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(1f, 1f));
				SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(-Main.GlobalTimeWrappedHourly/2f,0));
				SGAmod.SphereMapEffect.Parameters["softEdge"].SetValue(2f);

				SGAmod.SphereMapEffect.CurrentTechnique.Passes["SphereMap"].Apply();

				Main.spriteBatch.Draw(texture, SunOffset - Main.screenPosition, null, Color.White, 0, texture.Size() / 2f, (0.25f + ((trueScale) * 0.50f)) * scale, SpriteEffects.None, 0f);

				SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(SequenceColor.ToVector4() * timeLeftScale * 0.950f);
				SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(texture3);
				SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(1f, 1f));
				SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(-Main.GlobalTimeWrappedHourly / 2.5f,0));
				SGAmod.SphereMapEffect.Parameters["softEdge"].SetValue(2f);

				SGAmod.SphereMapEffect.CurrentTechnique.Passes["SphereMapAlpha"].Apply();

				Main.spriteBatch.Draw(texture, SunOffset - Main.screenPosition, null, Color.White, 0, texture.Size() / 2f, (0.25f + ((trueScale) * 0.50f)) * scale, SpriteEffects.None, 0f);

				SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(SequenceColor.ToVector4() * timeLeftScale * 0.500f);
				SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(texture2);
				SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(1f,1f));
				SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(-Main.GlobalTimeWrappedHourly / 3.5f, 0));
				SGAmod.SphereMapEffect.Parameters["softEdge"].SetValue(2f);

				SGAmod.SphereMapEffect.CurrentTechnique.Passes["SphereMapAlpha"].Apply();

				Main.spriteBatch.Draw(texture, SunOffset - Main.screenPosition, null, Color.White, 0, texture.Size() / 2f, (0.25f + ((trueScale) * 0.50f)) * scale, SpriteEffects.None, 0f);

				//Main.spriteBatch.End();
				//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);



				for (float ff = 0; ff < 1f; ff += 1 / 5f)
				{

					RadialEffect.Parameters["overlayTexture"].SetValue(ModContent.Request<Texture2D>("SGAmod/TrailEffectSideways"));//SGAmod.PearlIceBackground
					RadialEffect.Parameters["alpha"].SetValue(1f);
					RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTimeWrappedHourly * 0.125f, ff+(Main.GlobalTimeWrappedHourly * 0.1f)));
					RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(5f, 5f));
					RadialEffect.Parameters["ringScale"].SetValue(0.025f);
					RadialEffect.Parameters["ringOffset"].SetValue(0.50f);
					RadialEffect.Parameters["ringColor"].SetValue(SequenceColor.ToVector3() * 2f);
					RadialEffect.Parameters["tunnel"].SetValue(false);


					RadialEffect.CurrentTechnique.Passes["RadialAlpha"].Apply();

					Main.spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, Color.White, 0, halfsize, (new Vector2(thesize, thesize) / (halfsize * 1.5f)) * MathHelper.Pi, default, 0);

					RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTimeWrappedHourly * 0.125f, ((ff + Main.GlobalTimeWrappedHourly) * -0.1f)));
					RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(4f, 5f));
					RadialEffect.Parameters["ringScale"].SetValue(0.010f);
					RadialEffect.CurrentTechnique.Passes["RadialAlpha"].Apply();

					Main.spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, Color.LightGray, 0, halfsize, ((new Vector2(thesize, thesize) + new Vector2(8f, 8f)) / (halfsize * 1.5f)) * MathHelper.Pi, default, 0);

				}

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			AuraEffects(Main.player[Projectile.owner], 1);
			//Texture2D tex = ModContent.GetTexture("SGAmod/Items/Weapons/Auras/StoneGolem");
			//spriteBatch.Draw(tex, projectile.Center+new Vector2(0,-32+(float)Math.Sin(projectile.localAI[0]/30f)*4f)-Main.screenPosition, null, lightColor, 0, new Vector2(tex.Width, tex.Height)/2f, projectile.scale, Main.player[projectile.owner].direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}

	}

	public class AuraStellarBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Main-Sequence Aura");
			Description.SetDefault("Pure hydrogen plasma radiance burns everything around you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			return base.Autoload(ref name, ref texture);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<AuraMinionStellar>()] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}

}