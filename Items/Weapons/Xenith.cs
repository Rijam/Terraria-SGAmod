using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Items;
using SGAmod.Items.Weapons.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace SGAmod.Items.Weapons
{
	public class Xenith : ModItem
	{

		public static int[] XenithBowTypes
        {
            get
            {
				int[] types = new int[]{
				ItemID.TinBow,
				ItemID.BeesKnees,
				ItemID.HellwingBow,
				ItemID.Marrow,
				ItemID.IceBow,
				ItemID.PulseBow,
				ItemID.DD2PhoenixBow,
				ModContent.ItemType<DeltaWing>(),
				ItemID.DD2BetsyBow,
				ItemID.Phantasm,
				ModContent.ItemType<HavocGear.Items.Weapons.Shadeflare>(),
				};

				return types;

			}
        }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Xenith");
			Tooltip.SetDefault("'Basically Zenith bow'");
		}
		
		public override void SetDefaults()
		{
			Item.damage = 100;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 32;
			Item.height = 62;
			Item.useTime = 15;
			Item.useAnimation = 20;
			Item.useStyle = 5;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = 50000;
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = SoundID.Item17;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<XenithCharging>();
			Item.channel = true;
			Item.shootSpeed = 50f;
			Item.useAmmo = AmmoID.Arrow;
			/*if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/Shadeflare_Glow");
			}*/
		}

        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<XenithBowProj>()] < 1)
            {
				Projectile.NewProjectile(null, player.Center, Vector2.Zero, ModContent.ProjectileType<XenithBowProj>(), 1, 1,player.whoAmI);
            }
        }

        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<ByteSoul>(), 100).AddTile(TileID.LunarCraftingStation).Register();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			type = ModContent.ProjectileType<XenithCharging>();
			return true;
		}
	}

	public class XenithCharging : HavocGear.Items.Weapons.ShadeflareCharging
	{
		int varityshot=0;
		public override int chargeuptime => 400;
		public override float velocity => 72f;
		public override float spacing => 16f;
		public override int fireRate => 30;
		public override (float, float) AimSpeed
		{


			get
			{
				float perc = MathHelper.Clamp(Projectile.ai[0] / (float)chargeuptime, 0f, 1f);
				float rate = 0.5f;



				return (rate, 0f);
			}
		}
		//public override int FireCount => 600;
		int chargeUpTimer=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Xenith Charging");
		}

		public override string Texture => "Terraria/Projectile_" + ProjectileID.ShadowFlameArrow;

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
			AIType = 0;
		}

		public override void ChargeUpEffects()
		{
			foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.owner == Projectile.owner && testby.type == ModContent.ProjectileType<XenithBowProj>()))
			{
				proj.damage = Projectile.damage;
				XenithBowProj proj2 = proj.ModProjectile as XenithBowProj;
				proj.rotation = Projectile.velocity.ToRotation();
				proj2.Projectile.ai[0] = 5;
				proj2.Projectile.netUpdate = true;
		}
	}

		public override bool DoChargeUp()
		{
			return true;
		}

		public override void FireWeapon(Vector2 direction)
		{
			float perc = MathHelper.Clamp(Projectile.ai[0] / (float)chargeuptime, 0f, 1f);

			Projectile.Kill();
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			/*
			BlendState blind = new BlendState
			{

				ColorSourceBlend = Blend.Zero,
				ColorDestinationBlend = Blend.InverseSourceColor,

				AlphaSourceBlend = Blend.Zero,
				AlphaDestinationBlend = Blend.InverseSourceColor

			};

			float perc = MathHelper.Clamp(projectile.ai[0] / (float)chargeuptime, 0f, 1f);

			Texture2D mainTex = SGAmod.ExtraTextures[96];

			float alpha2 = perc * MathHelper.Clamp(chargeUpTimer, 0f, 1f);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, blind, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Effect effect = SGAmod.TextureBlendEffect;

			for (int f = 0; f < 3; f += 1)
			{

				if (f == 2)
                {
					spriteBatch.End();
					spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
				}

				effect.Parameters["coordMultiplier"].SetValue(new Vector2(1f, 1f));
				effect.Parameters["coordOffset"].SetValue(new Vector2(0f, 0f));
				effect.Parameters["noiseMultiplier"].SetValue(new Vector2(1f, 1f));
				effect.Parameters["noiseOffset"].SetValue(new Vector2(0f, 0f));

				effect.Parameters["Texture"].SetValue(SGAmod.Instance.GetTexture("SmallLaser"));
				effect.Parameters["noiseTexture"].SetValue(SGAmod.Instance.GetTexture(f == 3 ? "SmallLaser" : "Extra_49c"));
				effect.Parameters["noiseProgress"].SetValue(Main.GlobalTimeWrappedHourly+f);
				effect.Parameters["textureProgress"].SetValue(0);
				effect.Parameters["noiseBlendPercent"].SetValue(1f);
				effect.Parameters["strength"].SetValue(MathHelper.Clamp(alpha2*3f,0f,1f));

				Color colorz = f < 2 ? (f < 1 ? Color.White : Color.Lime) : Color.Purple;
				effect.Parameters["colorTo"].SetValue(colorz.ToVector4());
				effect.Parameters["colorFrom"].SetValue(Color.Black.ToVector4());

				effect.CurrentTechnique.Passes["TextureBlend"].Apply();

				Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.White, projectile.velocity.ToRotation(), mainTex.Size() / 2f, (2f-(f/2f))*new Vector2(0.5f+perc*0.50f,0.5f+(perc* perc * 1.5f)), default, 0);
			}

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			*/

			return false;
        }

    }

		public class XenithBowProj : ModProjectile
	{
		public Player Owner => Main.player[Projectile.owner];
		public int TimeToShootPerBow => 30;
		public class HoveringBow
        {
			public XenithBowProj owner;
			public Player player;
			public int index = 0;
			public int time = 0;
			public int timeSinceAttack = 0;
			public int IndexMax => Xenith.XenithBowTypes.Length;
			public int BowType => Xenith.XenithBowTypes[index];
			public float rotation = 0;
			public float swapPositions = 0;
			public Vector2 _position;
			public Vector2 Position
            {
                get
                {
					return Vector2.SmoothStep(_position,bowPosition, MathHelper.Clamp(swapPositions,0f,1f));
                }
                set
                {
					_position = value;
                }
            }
			public Vector2 bowPosition = default;

			public SpriteEffects SpriteDirection => rotation % MathHelper.TwoPi > MathHelper.Pi ? SpriteEffects.FlipVertically : 0;

			public HoveringBow(XenithBowProj owner,Player player,Vector2 position,int index)
            {
				this.owner = owner;
				this.player = player;
				this._position = position;
				this.index = index;
			}

			public void UpdateShooting()
			{
				swapPositions = Math.Min(swapPositions + 0.05f,1f);

				rotation = rotation.AngleLerp(owner.Projectile.rotation,0.20f);
				int timer = (int)(((index / (float)IndexMax)* owner.TimeToShootPerBow) +player.SGAPly().timer);

				if (timer % owner.TimeToShootPerBow == 0)
				{
					if (player.HasAmmo(player.HeldItem, true))
					{

						timeSinceAttack = 0;
						Item item = new Item();
						item.SetDefaults(BowType);

						SoundEngine.PlaySound(item.UseSound,(int)owner.Projectile.Center.X,(int)owner.Projectile.Center.Y);

						int projType = item.shoot;

						bool canShoot = true;
						int damage = owner.Projectile.damage;
						float knockback = owner.Projectile.knockBack;
						float speed = 32f;
						if (projType == ProjectileID.WoodenArrowFriendly || projType == 10)
						{
							if (item.type == ItemID.MoltenFury)
								projType = ProjectileID.FireArrow;

							player.PickAmmo(item, ref projType, ref speed, ref canShoot, ref damage, ref knockback, false);
						}

						bool hide = false;

						if (item.type == ModContent.ItemType<HavocGear.Items.Weapons.Shadeflare>())
							projType = ProjectileID.ShadowFlameArrow;
						if (item.type == ItemID.PulseBow)
							projType = ProjectileID.PulseBolt;
						if (item.type == ItemID.HellwingBow)
						{
							projType = ProjectileID.Hellwing;
							damage = damage / 2;
						}
						if (item.type == ItemID.BeesKnees)
						{
							projType = ProjectileID.BeeArrow;
							damage = damage / 2;
						}
						if (item.type == ItemID.DD2BetsyBow)
						{
							projType = ProjectileID.DD2BetsyArrow;
							damage = damage / 2;
						}
						if (item.type == ItemID.Marrow)
						{
							speed += 64;
							projType = ProjectileID.BoneArrow;
						}
						if (item.type == ItemID.IceBow)
							projType = ProjectileID.FrostArrow;
						if (item.type == ItemID.DD2PhoenixBow)
						{
							foreach (Projectile proj2 in Main.projectile.Where(testby => testby.active && testby.owner == player.whoAmI && testby.type == ProjectileID.DD2PhoenixBow))
							{
								proj2.Kill();
							}
							hide = true;
							damage = (int)(damage *0.75f);

							//projType = ProjectileID.DD2PhoenixBowShot;
						}
						if (item.type == ItemID.Phantasm)
						{
							foreach(Projectile proj2 in Main.projectile.Where(testby => testby.active && testby.owner == player.whoAmI && testby.type == ProjectileID.Phantasm))
                            {
								proj2.Kill();
							}
							hide = true;
							damage = (int)(damage * 0.25f);
						}

						if (item.type == ModContent.ItemType<HavocGear.Items.Weapons.MangroveBow>())
						{
							speed *= 0.75f;
							for (float ff = -1f; ff < 2.1f; ff += 2f)
							{
								Projectile proj = Projectile.NewProjectileDirect(Position, rotation.ToRotationVector2().RotatedBy(ff/5f) * speed * player.ArrowSpeed(), projType, damage/2, knockback, owner.Projectile.owner);
								proj.Center = Position;
								proj.usesIDStaticNPCImmunity = true;
								proj.idStaticNPCHitCooldown = 15;
								proj.timeLeft = Math.Min(proj.timeLeft, 300);
							}
						}
						else
						{
							Projectile proj = Projectile.NewProjectileDirect(Position, rotation.ToRotationVector2() * speed * player.ArrowSpeed(), projType, damage, knockback, owner.Projectile.owner);
							proj.Center = Position;
							proj.usesIDStaticNPCImmunity = true;
							proj.idStaticNPCHitCooldown = 15;
							proj.timeLeft = Math.Min(proj.timeLeft, 300);
							if (hide)
								proj.Opacity = 0f;
						}


					}

				}
			}

			public void Draw(SpriteBatch sb,Color lighting)
            {
				Texture2D tex = Main.itemTexture[BowType];

				sb.Draw(tex, Position - Main.screenPosition, null, lighting, rotation, tex.Size() / 2f, 1, SpriteDirection, 0);

			}
		}

		public List<HoveringBow> bows = new List<HoveringBow>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("XenithBowProj");
		}
		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.light = 0f;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 22;
			Projectile.extraUpdates = 0;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.arrow = true;
			Projectile.netImportant = true;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override string Texture => "Terraria/Projectile_" + ProjectileID.PhantasmArrow;

		public override bool CanDamage()
		{
			return false;
		}

		public override void AI()
		{
			Projectile.localAI[0]++;
			Projectile.ai[0] -= 1;

			Projectile.Opacity = MathHelper.Clamp(Projectile.timeLeft / 20f, 0f, Math.Min(Projectile.localAI[0] / 15f, 1f));

			bool staySummoned = true;

			if (Projectile.timeLeft <= 20)
			{
				staySummoned = false;
				Projectile.velocity *= 0.94f;
			}

			if (Owner == null || Owner.dead || Owner.HeldItem.type != ModContent.ItemType<Xenith>())
            {
				staySummoned = false;
			}

			if (!staySummoned)
			{
				return;
			}

			if (Projectile.localAI[0] == 1)
            {
				for (int i = 0; i < Xenith.XenithBowTypes.Length; i += 1)
				{
					HoveringBow bow = new HoveringBow(this, Owner, Owner.Center, i);
					bows.Add(bow);
				}
			}

			if (!Main.dedServ)
			{
				if (Projectile.ai[0] < 1)
				{
					Projectile.rotation = (Main.MouseWorld - Projectile.Center).ToRotation();
					Projectile.netUpdate = true;
				}
			}

			float followSpeedRate = Projectile.ai[0] > 0 ? 15f : 1f;


			foreach (HoveringBow bow in bows)
            {
				bow.time++;
				bow.timeSinceAttack++;
				float percentOne = 1f / (float)bow.IndexMax;
				float percent = bow.index / (float)bow.IndexMax;

				float rotangle = (percent * MathHelper.TwoPi) + bow.time / 32f;
				rotangle %= MathHelper.TwoPi;
				bow._position = Projectile.Center+Vector2.UnitX.RotatedBy(rotangle) * new Vector2(256f, 64f);

				Matrix transMatrix = Matrix.CreateScale(1f, 3f, 0f) * Matrix.CreateRotationZ(-MathHelper.PiOver2 + (percent * MathHelper.Pi)+ ((percentOne * MathHelper.Pi)*0.5f)) * Matrix.CreateRotationZ(Projectile.rotation) * Matrix.CreateTranslation(Projectile.Center.X, Projectile.Center.Y, 0);

				bow.bowPosition = Vector2.Transform(Vector2.UnitX*128f,transMatrix);

				if (Projectile.ai[0] > 0)
				{
					bow.UpdateShooting();
                }
                else
                {
					bow.swapPositions = Math.Max(bow.swapPositions - 0.05f,0);
					bow.rotation %= MathHelper.TwoPi;
					bow.rotation = bow.rotation.AngleLerp(rotangle, MathHelper.Clamp(bow.timeSinceAttack/300f,0f,1f));
				}

			}

			Vector2 gotohere = Owner.MountedCenter + (Projectile.ai[0]>0 ? Vector2.Zero : new Vector2(0, -96f));

			Projectile.velocity *= 0.96f;

			Owner.itemRotation = Projectile.rotation;

			Projectile.Center += (gotohere - Projectile.Center) / (24f/followSpeedRate);
			Projectile.velocity += (gotohere - Projectile.Center)/(320f/ followSpeedRate);

			Projectile.timeLeft = 22;

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[Projectile.type];

			Effects.TrailHelper trail = new Effects.TrailHelper("FadedBasicEffectAlphaPass", Mod.Assets.Request<Texture2D>("SmallLaser").Value);
			trail.color = delegate (float percent)
			{
				return Main.hslToRgb((percent+Main.GlobalTimeWrappedHourly/3f)%1f,1f,0.75f);
			};

			trail.projsize = Vector2.Zero;
			trail.coordOffset = new Vector2(0, Main.GlobalTimeWrappedHourly * -1f);
			trail.coordMultiplier = new Vector2(1f, 3f);
			trail.trailThickness = 16f;
			trail.trailThicknessIncrease = 0;
			trail.doFade = false;
			trail.connectEnds = true;
			trail.strength = 1f* Projectile.Opacity;

			List<Vector2> spots = bows.Select(testby => testby.Position).ToList();

			trail.DrawTrail(spots, Projectile.Center);

			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Black * Projectile.Opacity*0.5f, Projectile.rotation-MathHelper.PiOver2, tex.Size() / 2f, Projectile.scale, Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

			foreach (HoveringBow bow in bows.OrderBy(testby => testby.Position.Y))
			{
				bow.Draw(spriteBatch, lightColor* Projectile.Opacity);
			}

				return false;
		}

	}

}
