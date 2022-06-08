using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SGAmod.HavocGear.Items.Weapons
{
	public class ThrownTrident : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elemental's Trident");
			//Tooltip.SetDefault("Shoots a spread of bullets");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.DayBreak);
			Item.damage = 15;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 56;
			Item.height = 28;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.noMelee = true;
			// item.melee = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// item.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// item.magic = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			// item.thrown = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Item.Throwing().DamageType = DamageClass.Throwing;
			Item.knockBack = 5;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("ThrownTridentFriendly").Type;
			Item.shootSpeed = 12f;

		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
				speedY-=1f;
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
				//float scale = (1f - (Main.rand.NextFloat() * .01f))*(player.thrownVelocity);
				//perturbedSpeed = perturbedSpeed * scale; 
				speedX=perturbedSpeed.X; speedY=perturbedSpeed.Y;		
			return true;
		}
	}
}
