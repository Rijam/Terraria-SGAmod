using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class ThermalBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Blade");
			Tooltip.SetDefault("'Furious!'\n50% to Thermal Blaze enemies for 5 seconds on hit\nCrits enemies burning in lava");
		}
		
		public override void SetDefaults()
		{
			Item.damage = 56;
			Item.DamageType = DamageClass.Melee;
			Item.width = 44;
			Item.height = 52;
			Item.useTime = 28;
			Item.useAnimation = 20;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = 100000;
			Item.rare = 6;
	        Item.UseSound = SoundID.Item1;		
			Item.autoReuse = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/GlowMasks/ThermalBlade_Glow").Value;
			}
		}

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(null, "FieryShard", 12).AddIngredient(mod.ItemType("UnmanedBar"), 10).AddTile(TileID.MythrilAnvil).Register();
        }

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(8) == 0)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, Mod.Find<ModDust>("HotDust").Type);
		Main.dust[dust].scale = 0.8f;
        Main.dust[dust].noGravity = false;
        Main.dust[dust].velocity = player.velocity*(float)(Main.rand.Next(20,100)*0.002f);
			}
			Lighting.AddLight(player.position, 0.6f, 0.5f, 0f);
		}

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (target.lavaWet)
            {
				crit = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if(!(Main.rand.Next(2) == 0))
			{
				target.AddBuff(ModContent.BuffType<Buffs.ThermalBlaze>(), 300);
			}
		}
    }
}