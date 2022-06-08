using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Tools
{
	public class UnmanedPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Pickaxe");
		}

		public override void SetDefaults()
		{
			Item.damage = 5;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useAnimation = 26;
			Item.useTime = 12;
			Item.pick = 55;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("UnmanedBar"), 10).AddTile(TileID.Anvils).Register();
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(7) == 0)
			{
				SGAPlayer sgaplayer = player.GetModPlayer(Mod,typeof(SGAPlayer).Name) as SGAPlayer;
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, Mod.Find<ModDust>("NovusSparkle").Type);
				Main.dust[dust].color=new Color(180, 60, 140);
				Main.dust[dust].alpha=(sgaplayer.Novusset>0 ? 181 : 180);

			}
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
		SGAPlayer sgaplayer = player.GetModPlayer(Mod,typeof(SGAPlayer).Name) as SGAPlayer;
        if (sgaplayer.Novusset>0)
		mult = 1.2f;

			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
		}

	}

	public class UnmanedAxe : UnmanedPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Axe");
		}

		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.axe = 12;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = 10000;
			Item.rare = 2;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("UnmanedBar"), 8).AddTile(TileID.Anvils).Register();
		}

	}

}