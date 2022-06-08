using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;


namespace SGAmod.Items.Weapons.Auras
{
	public class BeserkerAuraStaff : AuraStaffBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Berserker Aura Staff");
			Tooltip.SetDefault("Summons Berserker Gauntlets around the player to boost their attack power\nBut in your rage, you forget to breath");
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

			if (thetarget > -1 && Main.projectile[thetarget].active && Main.projectile[thetarget].type == Item.shoot)
			{
				AuraMinionBeserker shoot = Main.projectile[thetarget].ModProjectile as AuraMinionBeserker;
				tooltips.Add(new TooltipLine(Mod, "Bonuses", "Power Level: " + shoot.thepower));
				tooltips.Add(new TooltipLine(Mod, "Bonuses", "Power scales the breath drain and damage bonuses"));
				tooltips.Add(new TooltipLine(Mod, "Bonuses", "Grants 5% increased damage to allies in range per Power Level"));
				tooltips.Add(new TooltipLine(Mod, "Bonuses", "However their breath drains faster"));
			}
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.knockBack = 3f;
			Item.mana = 10;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = 1;
			Item.value = Item.buyPrice(0, 1, 50, 0);
			Item.rare = 1;
			Item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = Mod.Find<ModBuff>("AuraBuffBeserker").Type;
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<AuraMinionBeserker>();
		}

	}

	public class AuraMinionBeserker : AuraMinion
	{

		protected override int BuffType => ModContent.BuffType<AuraBuffBeserker>();
		protected override float _AuraSize => 60;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Midas Portal");
			Main.projFrames[Projectile.type] = 1;
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

		public override float CalcAuraPower(Player player)
		{
			float temp = 1f+(player.GetDamage(DamageClass.Summon) * (Projectile.minionSlots / 2f));
			return temp;
		}

		public override void AuraAI(Player player)
		{
			Lighting.AddLight(Projectile.Center, Color.Red.ToVector3() * 0.78f);
		}

		public override void InsideAura<T>(T type, Player player)
		{

			if (type is Player alliedplayer && player.IsAlliedPlayer(alliedplayer))
			{
				SGAPlayer theply = alliedplayer.SGAPly();
				theply.beserk[0] = 5;
				theply.beserk[1] = (int)((float)thepower*1f);

				theply.Player.BoostAllDamage((float)(theply.beserk[1] * 0.05f), 0);
			}

		}

		public override void AuraEffects(Player player, int type)
		{

			for (float i = 0; i < 360; i += 360f / Projectile.minionSlots)
			{
				float angle = MathHelper.ToRadians(i + Projectile.localAI[0] * -2f);
				Vector2 loc2 = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
				Vector2 loc = loc2 * thesize;
				Vector2 loc3 = loc2;
				loc3.Normalize();

				if (type == 1)
				{
					Texture2D tex = Main.itemTexture[ItemID.FireGauntlet];
					int frame = (int)((Projectile.localAI[0] + (i / 3f)) / 5f);
					frame %= 1;

					Main.spriteBatch.Draw(tex, (Projectile.Center + loc) - Main.screenPosition, new Rectangle(0, frame * (int)tex.Height / 1, tex.Width, (int)tex.Height / 1), Color.Red*0.5f, angle + MathHelper.ToRadians(90), new Vector2(tex.Width / 2f, (tex.Height / 5f) / 2f), Projectile.scale, SpriteEffects.None, 0f);

				}

			}

			if (type == 0)
			{
				for (float i = 0; i < 8f; i += 1f)
				{
					float angle = MathHelper.ToRadians(Main.rand.NextFloat(0, 360));
					Vector2 loc2 = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
					Vector2 loc = loc2 * thesize;

					Vector2 vels = loc2.RotatedBy(-90) * 0f;

					int dustIndex = Dust.NewDust(Projectile.Center + loc, 0, 0, DustID.Fire, 0, 0, 150, default(Color), 0.75f);
					Main.dust[dustIndex].velocity = vels + player.velocity;
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].color = Color.Red;
				}
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			AuraEffects(Main.player[Projectile.owner], 1);
			return false;
		}

	}

	public class AuraBuffBeserker : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Berserker Aura");
			Description.SetDefault("The Aura enrages your attacks, but you forget to breath");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[Mod.Find<ModProjectile>("AuraMinionBeserker").Type] > 0)
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
