using Terraria;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items
{
	public class Rootfish : ModItem
	{
		public override void SetDefaults()
		{

			Item.questItem = true;
			Item.maxStack = 1;
			Item.width = 26;
			Item.height = 26;
			Item.uniqueStack = true;
			Item.rare = -11;
		}

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Rootfish");
      Tooltip.SetDefault("");
    }


		public override bool IsQuestFish()
		{
			return true;
		}

		public override bool IsAnglerQuestAvailable()
		{
            return true;
		}

		public override void AnglerQuestChat(ref string description, ref string catchLocation)
		{
			description = "I was walking through a dank place on my way while looking for some nice worms to fish with, until a mossy Rootfish leaped into a nearby flooded area! I was so scared that I dropped all my worms, and I want revenge! Catch it and bring it to me!";
			catchLocation = "\nCaught in a Dank Shrine";
		}
	}
}
