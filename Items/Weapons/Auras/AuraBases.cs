using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Weapons.Auras
{
	public class AuraStaffBase : ModItem
	{
		public int prog = -1;
		public override bool Autoload(ref string name)
		{
			return GetType() != typeof(AuraStaffBase);
		}

		public override void SetDefaults()
		{
			Item.damage = 45;
			Item.knockBack = 3f;
			Item.mana = 10;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = 1;
			Item.value = Item.buyPrice(0, 20, 0, 0);
			Item.rare = 7;
			Item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			Item.noMelee = true;
			Item.DamageType = DamageClass.Summon;
			Item.buffType = Mod.Find<ModBuff>("AuraBuffStone").Type;
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			Item.shoot = Mod.Find<ModProjectile>("AuraMinionStone").Type;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Color c = Placeable.TechPlaceable.LuminousAlterItem.AuroraLineColor;
			tooltips.Add(new TooltipLine(Mod, "Plasma Item", Idglib.ColorText(c, "Aura weapons affect all around the player, friend and foe alike")));
			tooltips.Add(new TooltipLine(Mod, "AuraUse", Idglib.ColorText(c, "Reusing the item consumes an extra minion slot and increases the current Aura Strength")));
			//tooltips.Add(new TooltipLine(mod, "AuraUse", "Alt Fire to relocate the Aura, Alt Fire again to return it to you"));
		}

		public override bool CanUseItem(Player player)
		{
			return (((float)player.maxMinions - player.GetModPlayer<SGAPlayer>().GetMinionSlots) >= 1);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(Item.buffType, 2);

			if (player.ownedProjectileCounts[type] > 0)
			{
				for(int i = 0; i < Main.maxProjectiles; i += 1)
				{
					if (Main.projectile[i].type == Item.shoot && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].active)
					{
						if (((float)player.maxMinions - player.GetModPlayer<SGAPlayer>().GetMinionSlots) >= 1)
						{
							Main.projectile[i].ai[0] += 1f;
							Main.projectile[i].damage = damage;
							Main.projectile[i].netUpdate = true;
						}
					}
				}
				//Main.NewText("" + (player.maxMinions - player.GetModPlayer<SGAPlayer>().GetMinionSlots));
				return false;
			}

			return true;
		}
	}

	public class AuraMinion : ModProjectile
	{
		protected virtual int BuffType => ModContent.BuffType<AuraBuffStone>();
		protected Player Player => Main.player[Projectile.owner];
		protected virtual float _AuraSize => 160;
		protected virtual float AuraSize => _AuraSize;// * Player.SGAPly().auraBoosts.Item2;
		protected float thesize = 0;
		public float thepower = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Midas Portal");
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[Projectile.type] = 1;
			// This is necessary for right-click targeting
			//ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[Projectile.type] = true;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[Projectile.type] = true;
		}

		public virtual float CalcAuraPower(Player player)
		{
			float temp = (player.GetDamage(DamageClass.Summon) * (1f + (Projectile.minionSlots / 3f)));
			return temp;
		}

		public float CalcAuraPowerReal(Player player)
		{
			thepower = CalcAuraPower(player)+player.SGAPly().auraBoosts.Item1;
			return thepower;
		}

		public virtual float CalcAuraSize(Player player)
		{
			return (AuraSize * (float)Math.Pow((double)CalcAuraPowerReal(player),0.80));
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.minion = true;
			// Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			Projectile.minionSlots = 1f;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			Projectile.penetrate = -1;
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

		public virtual void InsideAura<T>(T type, Player player) where T : Entity
		{


		}

		public virtual void AuraEffects(Player player, int type)
		{

		}

		public virtual void AuraAI(Player player)
		{
			Lighting.AddLight(Projectile.Center, Color.ForestGreen.ToVector3() * 0.78f);
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			if (!player.dead && player.active)
			{
				for (int i = 0; i < Main.maxPlayers; i += 1)
				{
					if (Main.player[i].active && !Main.player[i].dead && (Main.player[i].Center - Projectile.Center).Length() < thesize)
						InsideAura(Main.player[i], player);
				}

				for (int i = 0; i < Main.maxNPCs; i += 1)
				{
					if (Main.npc[i].active && Main.npc[i].active && (Main.npc[i].Center - Projectile.Center).Length() < thesize)
						InsideAura(Main.npc[i], player);
				}
			}

				return true;
		}

		public override void AI()
		{
			//if (projectile.owner == null || projectile.owner < 0)
			//return;

			Player player = Main.player[Projectile.owner];
			if (player.HasBuff(BuffType))
			{
				Projectile.timeLeft = 2;
			}
			if (player.dead || !player.active)
			{
				player.ClearBuff(BuffType);
				return;
			}

			Projectile.Center = player.Center;

			Projectile.minionSlots = Projectile.ai[0]+1;

			thesize=CalcAuraSize(player);

			AuraAI(player);

			AuraEffects(player,0);
			Vector2 gothere = player.Center;
			Projectile.localAI[0] += 1;


		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Auras/StoneGolem"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

	}

}