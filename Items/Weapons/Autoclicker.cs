using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

using SGAmod.NPCs.Cratrosity;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class Autoclicker : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Autoclicker");
			Tooltip.SetDefault("Summons Cursors to click on enemies\nClicks may spawn a cookie when this item is held, more likely with more max sentry summons\nCan pickup the cookie to gain health, minion range, and a click rate buff\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 30 seconds each"));
			ItemID.Sets.GamepadWholeScreenUseRange[Item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.knockBack = 5f;
			Item.mana = 5;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 4;
			Item.noUseGraphic = true;
			Item.useAnimation = 4;
			Item.useStyle = 1;
			Item.value = Item.buyPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.noUseGraphic = true;
			//item.UseSound = Main.soundt;

			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = ModContent.BuffType<AutoclickerMinionBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = ModContent.ProjectileType<AutoclickerMinion>();
			Item.shootSpeed = 32f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			SoundEngine.PlaySound(SoundID.MenuTick,(int)player.Center.X, (int)player.Center.Y,0);
			player.AddBuff(Item.buffType, 2);

			/*foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.type == ModContent.ProjectileType<AutoclickerMinion>()))
			{
				AutoclickerMinion click = (AutoclickerMinion)proj.ModProjectile;
				click.DoClick();
			}*/
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("ByteSoul"), 75).AddRecipeGroup("Fragment", 15).AddIngredient(ItemID.Mouse, 1).AddTile(TileID.LunarCraftingStation).Register();
		}

	}

	public class AutoclickerMinion : ModProjectile
	{
		protected float idleAccel = 0.05f;
		protected float spacingMult = 1f;
		protected float viewDist = 400f;
		protected float chaseDist = 200f;
		protected float chaseAccel = 6f;
		protected float inertia = 40f;
		protected float shootCool = 90f;
		protected float shootSpeed;
		protected int shoot;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Autoclicker");
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[Projectile.type] = 1;
			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[Projectile.type] = true;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minion = true;
			// Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			Projectile.minionSlots = 1f;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			Projectile.penetrate = -1;
			Projectile.knockBack = 8;
			Projectile.timeLeft = 60;
		}


		// Here you can decide if your minion breaks things like grass or pots
		public override bool? CanCutTiles()
		{
			return false;
		}

		// This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage()
		{
			return false;
		}

		Player ThePlayer => Main.player[Projectile.owner];
		int clickDelay = 0;

		bool ClickerBoost
        {
            get
            {
				return ThePlayer.HasBuff(ModContent.BuffType<AutoclickerSpeedBuff>());
			}
        }

		public override void AI()
		{
			//if (projectile.owner == null || projectile.owner < 0)
			//return;

			Player player = ThePlayer;

			int attackrate = 40;


			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<AutoclickerMinionBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<AutoclickerMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}
			Vector2 there = player.Center;
			float dist = Projectile.ai[1]<1 ? 64 : 8f;
			Projectile.localAI[0] += 1;
			Projectile.ai[0] += 1;
			Projectile.ai[1] -= 1;
			clickDelay -= 1;

			float id = 0f;
			float maxus = 0f;

			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (currentProjectile.active // Make sure the projectile is active
				&& currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
				&& currentProjectile.type == Projectile.type)
				{ // Make sure the projectile is of the same type as this javelin

					if (i == Projectile.whoAmI)
						id = maxus;
					maxus += 1f;

				}
			}
			Projectile.localAI[1] = id/maxus;

			NPC them = null;
			Entity focusOn = player;

			if (player.HasMinionAttackTargetNPC)
			{
				them = Main.npc[player.MinionAttackTargetNPC];
				there = them.Center;
				focusOn = them;
			}
			else
			{
				List<NPC> enemies = SGAUtils.ClosestEnemies(Projectile.Center, 2200, Projectile.ai[1] > 0 ? Projectile.Center : player.Center,checkWalls: false);
				if (enemies != null && enemies.Count > 0)
				{
					enemies = enemies.OrderBy(testby => testby.life).ToList();
					them = enemies[(int)id%enemies.Count];
					there = them.Center;
					focusOn = them;
				}
			}

			float angles = ((id / (float)maxus) * MathHelper.TwoPi)-player.SGAPly().timer/150f;
			Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles)) * dist;
			Vector2 where = there + here;
			Vector2 todist = (where - Projectile.Center);// +(focusOn != null ? focusOn.velocity : Vector2.Zero);
			Vector2 todistreal = (there - Projectile.Center);

			float lookat = todist.ToRotation();

			if (them == null)
			{
				lookat = (focusOn.Center - Projectile.Center).ToRotation();
			}

			if (todistreal.Length() > 0.01f)
			{
				if (todistreal.Length() > 600f)
				{
					Projectile.velocity += Vector2.Normalize(todist) *MathHelper.Clamp(todist.Length()/6f,0f,64f);
					Projectile.velocity *= 0.940f;

				}
                else
                {
					Projectile.Center += todist * (Projectile.ai[1] > -(attackrate/(ClickerBoost ? 10f : 5f)) ? 0.25f : 0.98f);
					Projectile.velocity *= 0.820f;
				}
			}
			if (Projectile.velocity.Length() > 1f)
			{
				float maxspeed = Math.Min(Projectile.velocity.Length(), 16 + (todist.Length() / 4f));
				Projectile.velocity.Normalize();
				Projectile.velocity *= maxspeed;
			}

			if (todistreal.Length() > 160f && (Projectile.ai[1] < 0 || them == null))
			{
				if (them != null)
                {
					lookat = 0;
				}
				Projectile.rotation = Projectile.rotation.AngleLerp(lookat, 0.05f);
			}
			else
			{
				lookat = (focusOn.Center - Projectile.Center).ToRotation();
				Projectile.rotation = Projectile.rotation.AngleLerp(lookat, 0.15f);
				if (them != null)
				{
					if (player.SGAPly().timer % (int)(attackrate * (ClickerBoost ? 0.5f : 1f)) == (int)(((id * (attackrate / maxus)))*(ClickerBoost ? 0.5f : 1f)))
					{
						DoClick();
					}
					if (clickDelay == 0)
					{
						SoundEngine.PlaySound(SoundID.MenuTick, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0);

						if (!them.IsDummy() && Main.rand.Next(500) < player.maxTurrets && player.HeldItem.type == ModContent.ItemType<Autoclicker>())
                        {
							bool cookieNearby = false;
							int range = 900 + (ClickerBoost ? 600 : 0);
							foreach(Item item in Main.item.Where(testby => testby.active && testby.type == ModContent.ItemType<ClickerCookie>() && (testby.Center-them.Center).LengthSquared()< (range * range)))
                            {
								cookieNearby = true;
							}
							if (!cookieNearby)
							Item.NewItem(them.Center, ModContent.ItemType<ClickerCookie>(),prefixGiven: PrefixID.Menacing,noGrabDelay: true);
                        }
						int damage = (int)(Projectile.damage * (ClickerBoost ? 0.60f : 1f));
						//them.SGANPCs().AddDamageStack(damage,60*5);
						Projectile.NewProjectile(them.Center, Projectile.rotation.ToRotationVector2(), ModContent.ProjectileType<AutoclickerClickProj>(), damage,Projectile.knockBack,Projectile.owner);


						/*int damazz = (Main.DamageVar((float)projectile.damage));
						them.StrikeNPC(damazz, projectile.knockBack, them.direction, false, false);
						player.addDPS(damazz);
						if (Main.netMode != NetmodeID.SinglePlayer)
						{
							NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, them.whoAmI, damazz, 16f, (float)1, 0, 0, 0);
						}*/
					}
				}

			}

			Projectile.velocity *= 0.96f;

			Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 0.78f);

		}

		public void DoClick()
		{
			if (Projectile.ai[1] < 1 && clickDelay<0)
			{
				Projectile.ai[1] = ClickerBoost ? 10 : 20;
				clickDelay = (int)(Projectile.ai[1] / 2);
			}
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Autoclicker"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[Projectile.type];

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((Projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
			Color color = Color.White;
			if (ClickerBoost)
			{
				for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
				{
					spriteBatch.Draw(tex, drawPos + Vector2.UnitX.RotatedBy(f) * 3f, null, Main.hslToRgb((Projectile.localAI[1] + (Main.GlobalTimeWrappedHourly/3f))%1f,1f,0.75f) * 0.32f, Projectile.rotation + MathHelper.PiOver2, drawOrigin, Projectile.scale / 2f, SpriteEffects.None, 0f);
				}
			}
			spriteBatch.Draw(tex, drawPos, null, color, Projectile.rotation+MathHelper.PiOver2, drawOrigin, Projectile.scale/2f, SpriteEffects.None, 0f);
			return false;
		}

	}

	public class AutoclickerClickProj : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Autoclicker");
		}

		public override string Texture => "Terraria/Item_" + ItemID.SugarCookie;

		public sealed override void SetDefaults()
		{
			Projectile.width = 72;
			Projectile.height = 72;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.hide = true;
			Projectile.minion = true;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			Projectile.penetrate = 1;
			Projectile.knockBack = 8;
			Projectile.timeLeft = 2;
		}

		// Here you can decide if your minion breaks things like grass or pots
		public override bool? CanCutTiles()
		{
			return false;
		}

		// This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage()
		{
			return false;
		}
	}

	public class ClickerCookie : ModItem, IConsumablePickup
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cookie");
			Tooltip.SetDefault("'Yes, this is literally Minecraft's cookie sprite'");
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
		}
		public override string Texture => "SGAmod/Items/Consumables/ClickerCookie";
		public override void SetDefaults()
        {
			Item.maxStack = 1;
        }

		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange += 160;
		}

		public override bool CanPickup(Player player)
        {
			return (player.SGAPly().AddCooldownStack(30, 1, true));
        }

        public override bool OnPickup(Player player)
		{
			if (player.SGAPly().AddCooldownStack(60 * 30, 1))
			{
				player.HealEffect(50);
				player.netLife = true;
				player.statLife += 50;
				player.AddBuff(ModContent.BuffType<AutoclickerSpeedBuff>(),60*20);
				SoundEffectInstance snd = SoundEngine.PlaySound(SoundID.Item,(int)player.Center.X, (int)player.Center.Y, 2);
				if (snd != null)
                {
					snd.Pitch = 0.75f;
				}
			}
			return false;
		}

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {

        Texture2D inner = Main.itemTexture[ModContent.ItemType<AssemblyStar>()];

			Vector2 drawPos = Item.position-Main.screenPosition;
			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);

			for (float i = 0; i < 1f; i += 0.20f)
			{
				spriteBatch.Draw(inner, drawPos-Vector2.UnitY.RotatedBy(rotation)*10f, null, Color.Yellow * (1f - ((i + (Main.GlobalTimeWrappedHourly / 2f)) % 1f)) * 0.25f, i * MathHelper.TwoPi, textureOrigin,(0.5f + 1.75f * (((Main.GlobalTimeWrappedHourly / 2f) + i) % 1f))*1f, SpriteEffects.None, 0f);
			}

			return true;
		}

	}

	public class AutoclickerSpeedBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cookie Power!");
			Description.SetDefault("Cursers click faster");
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/AutoclickerSpeedBuff";
			return true;
		}
	}

	public class AutoclickerMinionBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Auto Clickers");
			Description.SetDefault("'I can't believe its not Clicker Class!'");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/AutoclickerMinionBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<AutoclickerMinion>()] > 0)
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
