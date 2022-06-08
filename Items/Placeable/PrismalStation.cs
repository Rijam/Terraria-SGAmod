using Terraria.ModLoader;

namespace SGAmod.Items.Placeable
{
	public class PrismalStation: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Extractor");
			Tooltip.SetDefault("It would seem the Lihzahrds knew a way to awaken Novus involving Novite to their full potiental...\nAllows transmuting of a Novus+Novite alloy into Prismal ore");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 26;
			Item.height = 14;
			Item.value = 0;
			Item.rare = 10;
			Item.alpha = 0;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("PrismalStation").Type;
		}
	}
}