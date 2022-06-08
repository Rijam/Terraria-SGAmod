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

namespace SGAmod.Items.Weapons.Auras
{

	public class AuraBorealisStaff : AuraStaffBase, IAuroraItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aura Borealis Staff");
			Tooltip.SetDefault("Summons a Hallowed Celestial Aura around the player");
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

        public override string Texture => "SGAmod/Items/Weapons/Aurora/AuraBorealisStaff";

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
				AuraMinionBorealis shoot = Main.projectile[thetarget].ModProjectile as AuraMinionBorealis;
				tooltips.Add(new TooltipLine(Mod, "Bonuses", "Power Level: "+ shoot.thepower));
				tooltips.Add(new TooltipLine(Mod, "Bonuses", "Passive: Grants life and Mana Regen per Power Level"));
				tooltips.Add(new TooltipLine(Mod, "Bonuses", "Erases debuffs on allies if you are immune to them"));

				if (shoot.thepower >= 1.0)
					tooltips.Add(new TooltipLine(Mod, "Bonuses", "Lv1: Applies Betsy's Curse to enemies"));
				if (shoot.thepower >= 2.0)
				{
					tooltips.Add(new TooltipLine(Mod, "Bonuses", "Lv2: Applies Rust Burn to enemies"));
					tooltips.Add(new TooltipLine(Mod, "Bonuses", Buffs.RustBurn.RustText));
				}
				if (shoot.thepower >= 3.0)
					tooltips.Add(new TooltipLine(Mod, "Bonuses", "Lv3: Applies Moonlight Curse to enemies)"));

			}
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.knockBack = 3f;
			Item.mana = 20;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = 1;
			Item.value = Item.buyPrice(0, 0, 50, 0);
			Item.rare = 1;
			Item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<AuraBorealisBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<AuraMinionBorealis>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<AuroraTearAwoken>(), 1).AddIngredient(ModContent.ItemType<IlluminantEssence>(), 10).AddIngredient(ItemID.CrystalShard, 10).AddIngredient(ItemID.LunarBar, 12).AddTile(ModContent.TileType<LuminousAlter>()).Register();
		}

	}

	public class AuraMinionBorealis : AuraMinion
	{

		protected override int BuffType => ModContent.BuffType<AuraBorealisBuff>();

        protected override float _AuraSize => 120f;

        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Borealis");
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
		}

		public override void AuraAI(Player player)
		{
			Lighting.AddLight(Projectile.Center, Color.Pink.ToVector3() * 0.78f);
		}

		public override void InsideAura<T>(T type, Player player)
		{
			if (type is NPC)
			{
				NPC himas = (type as NPC);
				if (!(himas.townNPC || himas.friendly))
				{
					himas.AddBuff(BuffID.BetsysCurse, 3);
					if (thepower >= 2)
					{
						Buffs.RustBurn.ApplyRust(himas,3);
						himas.AddBuff(ModContent.BuffType<Buffs.RustBurn>(), 3);
					}
					if (thepower >= 3)
					{
						himas.AddBuff(ModContent.BuffType<Buffs.MoonLightCurse>(), 3);
					}
				}
			}
			if (type is Player alliedplayer && player.IsAlliedPlayer(alliedplayer))
			{
				alliedplayer.lifeRegen += (int)(thepower*2f);
				alliedplayer.SGAPly().manaBoost += (int)(thepower * 15f);

				foreach (int buffonotherplayer in alliedplayer.buffType)
                {
					if (player.buffImmune[buffonotherplayer])
                    {
						alliedplayer.DelBuff(alliedplayer.FindBuffIndex(buffonotherplayer));
					}
                }
			}
		}

		public override void AuraEffects(Player player, int type)
		{


			if (type == 1)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            }
            else
            {
				//player.SGAPly().manaBoost += (int)(thepower * 15000f);
			}

			UnifiedRandom rando = new UnifiedRandom(Projectile.whoAmI);

			Texture2D mainTex = Mod.Assets.Request<Texture2D>("Extra_57b").Value;
			Vector2 halfsize = mainTex.Size() / 2f;

			for (float i = 0; i < 360; i += 360f / Projectile.minionSlots)
			{
				float angle = MathHelper.ToRadians(i + Projectile.localAI[0] * 2f);
				Vector2 loc2 = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
				Vector2 loc = loc2 * 32f;
				//loc -= Vector2.UnitY * 54f * player.gravDir;

				if (type == 0)
				{
					/*float velmul = Main.rand.NextFloat(0.75f, 1f);
					Vector2 vels = loc2.RotatedBy(-90) * 4f * velmul;

					int dustIndex = Dust.NewDust(projectile.Center + loc - new Vector2(6, 6), 12, 12, DustID.AncientLight, 0, 0, 150, default(Color), 0.75f);
					Main.dust[dustIndex].velocity = vels + player.velocity;
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].color = Color.Lime;*/
				}

				if (type == 1)
				{
					float colorhue = rando.NextFloat();
					Main.spriteBatch.Draw(mainTex, (Projectile.Center + loc) - Main.screenPosition,null, Main.hslToRgb(colorhue, 0.75f,0.75f), angle*2f + MathHelper.ToRadians(90), halfsize, Projectile.scale, SpriteEffects.None, 0f);
					Main.spriteBatch.Draw(mainTex, (Projectile.Center + loc) - Main.screenPosition, null, Main.hslToRgb(colorhue, 0.5f, 0.75f), -angle*2f - MathHelper.ToRadians(90), halfsize, Projectile.scale*0.75f, SpriteEffects.None, 0f);
				}
			}

			if (type == 1)
			{
				Effect RadialEffect = SGAmod.RadialEffect;

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.PearlIceBackground);//SGAmod.PearlIceBackground
				RadialEffect.Parameters["alpha"].SetValue(0.75f);
				RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTimeWrappedHourly * 0.125f, Main.GlobalTimeWrappedHourly * 0.075f));
				RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(2f, 2f));
				RadialEffect.Parameters["ringScale"].SetValue(0.075f);
				RadialEffect.Parameters["ringOffset"].SetValue(0.50f);
				RadialEffect.Parameters["ringColor"].SetValue(Color.White.ToVector3());
				RadialEffect.Parameters["tunnel"].SetValue(false);


				RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

				Main.spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, Color.White, 0, halfsize, (new Vector2(thesize, thesize) /(halfsize*1.5f)) * MathHelper.Pi, default, 0);

				RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTimeWrappedHourly * -0.125f, Main.GlobalTimeWrappedHourly * 0.075f));
				RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(2f, 2f));
				RadialEffect.Parameters["ringScale"].SetValue(0.05f);
				RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

				Main.spriteBatch.Draw(mainTex, Projectile.Center - Main.screenPosition, null, Color.LightGray, 0, halfsize, ((new Vector2(thesize, thesize)+new Vector2(8f,8f)) / (halfsize * 1.5f)) * MathHelper.Pi, default, 0);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
			}

			/*if (type == 0)
			{
				for (float i = 0; i < 5f; i += 1f)
				{
					float angle = MathHelper.ToRadians(Main.rand.NextFloat(0, 360));
					Vector2 loc2 = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
					Vector2 loc = loc2 * thesize;

					Vector2 vels = loc2.RotatedBy(-90) * 0f;

					int dustIndex = Dust.NewDust(projectile.Center + loc, 0, 0, DustID.AncientLight, 0, 0, 150, default(Color), 0.65f);
					Main.dust[dustIndex].velocity = vels + player.velocity;
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].color = Color.Lime;
				}
			}*/

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			AuraEffects(Main.player[Projectile.owner], 1);
			//Texture2D tex = ModContent.GetTexture("SGAmod/Items/Weapons/Auras/StoneGolem");
			//spriteBatch.Draw(tex, projectile.Center+new Vector2(0,-32+(float)Math.Sin(projectile.localAI[0]/30f)*4f)-Main.screenPosition, null, lightColor, 0, new Vector2(tex.Width, tex.Height)/2f, projectile.scale, Main.player[projectile.owner].direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}

	}

	public class AuraBorealisBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallow Lights Aura");
			Description.SetDefault("An aura of Hallow Lights projects around you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/AuraBorealisBuff";
			return base.Autoload(ref name, ref texture);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<AuraMinionBorealis>()] > 0)
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