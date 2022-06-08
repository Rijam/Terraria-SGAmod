using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Buffs;
using SGAmod.Projectiles;
using Idglibrary;
using Terraria.Audio;

namespace SGAmod.Items.Consumables
{
    class GasPasser : ModItem
    {

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Gas Passer");
			Tooltip.SetDefault("Throws gasoline canisters on your enemies dousing them in gas, which you can ignite them for massive damage over time!\nDoes more damage against enemies with more max HP\nCombustion increases the damage of burning-based debuffs greatly\nLess ass then the source material\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 45 seconds"));

		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/GasPasser"); }
		}

		public override void SetDefaults()
		{
			Item.useStyle = 1;
			Item.shootSpeed = 12f;
			Item.shoot = Mod.Find<ModProjectile>("GasPasserProj").Type;
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 10;
			Item.consumable = true;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.value = Item.buyPrice(0, 0, 20, 0);
			Item.rare = 2;
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}

		public override bool ConsumeItem(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/gas_can_throw").WithVolume(.5f).WithPitchVariance(.15f), Item.Center);
			player.SGAPly().AddCooldownStack(45 * 60);
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);

		}

		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ItemID.Explosives).AddIngredient(mod.ItemType("IceFairyDust"), 2).AddIngredient(mod.ItemType("WraithFragment4"),4).AddIngredient(mod.ItemType("BottledMud"), 2).AddIngredient(mod.ItemType("MurkyGel"), 8).AddIngredient(ItemID.CursedFlame, 2).AddTile(TileID.WorkBenches).Register();
		}

	}

	public class GasPasserProj : DosedArrow
	{

		double keepspeed = 0.0;
		float homing = 0.06f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gas Can");
		}
		public override float beginhoming => 99990f;

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/GasPasser"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.timeLeft = 240;
			Projectile.arrow = true;
			Projectile.damage = 0;
			AIType = ProjectileID.WoodenArrowFriendly;
		}

		public override void AI()
		{
			effects(0);

			Projectile.ai[0] = Projectile.ai[0] + 1;
			Projectile.velocity.Y += 0.15f;
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57f;


			NPC target = Main.npc[Idglib.FindClosestTarget(0, Projectile.Center, new Vector2(0f, 0f), true, true, true, Projectile)];
			if (target != null)
			{
				if (new Rectangle((int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height).Intersects
					(new Rectangle((int)target.position.X, (int)target.position.Y, target.width, target.height)))
				{
					Projectile.Kill();
				}
			}


		}

		public override void effects(int type)
		{
			base.effects(type);
			if (type == 1)
			{
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/gas_can_explode").WithVolume(.5f).WithPitchVariance(.15f), Projectile.Center);
				Projectile.type = 0;
				int proj = Projectile.NewProjectile(new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0, 0), Mod.Find<ModProjectile>("GasCloud").Type, 1, Projectile.knockBack, Main.player[Projectile.owner].whoAmI);
			}

		}


	}

}



