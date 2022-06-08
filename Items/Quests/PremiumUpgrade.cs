using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Quests
{
	public class PremiumUpgrade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Contracker");
			Tooltip.SetDefault("Activating this will grant it's owner the TF2 Emblem and allow Crate Drops\nThe crates will drop per world on activation, however only new characters will receive the TF2 Emblem");

		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
			Item.consumable = false;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 4;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
			Item.value = Item.buyPrice(0, 2, 0, 0);
		}
		//player.CountItem(mod.ItemType("ModItem"))

		public override bool? UseItem(Player ply)
		{
			SGAWorld.QuestCheck(0,ply);
			return true;
		}
	}
}