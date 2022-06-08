using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons 
{
	public class SwampSeeds : ModItem
	{
		public override void SetDefaults()
		{
			Item.autoReuse = true;

			Item.useTurn = true;
			Item.useStyle = 1;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.placeStyle = 0;
			Item.width = 12;
			Item.height = 14;
			Item.value = 10;
			Item.createTile = Mod.Find<ModTile>("SwampGrassGrow").Type;
		}

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dank Seeds");
      Tooltip.SetDefault("'Harvested from Dank Grass, grows on Moist Stone'");
    }


        public override bool? UseItem(Player player)
        {
            if (Main.tile[Player.tileTargetX, Player.tileTargetY+1].type == ModContent.TileType<Tiles.MoistStone>() && !Main.tile[Player.tileTargetX, Player.tileTargetY + 1].active())
            {
				string[] onts = new string[] { "SwampGrassGrow", "SwampGrassGrow2", "SwampGrassGrow3" };
				Main.tile[Player.tileTargetX, Player.tileTargetY].type = (ushort)mod.TileType(onts[Main.rand.Next(onts.Length)]);
            }
            else
            {
                item.stack++;
            }
            return true;
        }
	}
}
