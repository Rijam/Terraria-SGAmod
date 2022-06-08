using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SGAmod.HavocGear.Items.Accessories
{
	public class MudAbsorber : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mud Absorber");
            Tooltip.SetDefault("More damage and life regeneration when close to mud");
        }

        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = 10000;
			Item.rare = 2;
			//item.defense = 3;
			Item.accessory = true;
			Item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (!player.GetModPlayer<SGAPlayer>().mudbuff)
			{
				player.GetModPlayer<SGAPlayer>().mudbuff = true;
				bool nearMud = false; //                   8f
				int y_top_edge = (int)(player.position.Y - 16f) / 16;
				int y_bottom_edge = (int)(player.position.Y + (float)player.height + 16f) / 16;
				int x_left_edge = (int)(player.position.X - 16f) / 16;
				int x_right_edge = (int)(player.position.X + (float)player.width + 16f) / 16;

				for (int x = x_left_edge; x <= x_right_edge; x++)
				{
					for (int y = y_top_edge; y <= y_bottom_edge; y++)
					{
						if (Main.tile[x, y].TileType == TileID.Mud || Main.tile[x, y].TileType == TileID.JungleGrass || Main.tile[x, y].TileType == TileID.MushroomGrass)
						{
							nearMud = true;
							break;
						}
					}
					if (nearMud) break;
				}

				if (nearMud)
				{
					player.lifeRegen += 10;
					player.GetDamage(DamageClass.Melee) *= 1.2f;
					player.Throwing().thrownDamage *= 1.3f;
					player.GetDamage(DamageClass.Ranged) *= 1.2f;
					player.GetDamage(DamageClass.Magic) *= 1.3f;
					player.meleeSpeed *= 1.2f;
					player.GetDamage(DamageClass.Summon) *= 1.1f;
					player.Throwing().thrownCrit += 5;
					player.GetCritChance(DamageClass.Magic) += 5;
					player.GetCritChance(DamageClass.Melee) += 4;
					player.GetCritChance(DamageClass.Ranged) += 4;
				}
				else
				{
					//item.lifeRegen = 0;
				}
			}
		}
	}
}