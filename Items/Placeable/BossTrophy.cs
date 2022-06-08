using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable
{
	public class BossTrophy : ModItem
	{
		private int placeStyle;
		private string trophySprite = "SGAmod/Items/Placeable/BossTrophy";
		private string bossSprite = "SGAmod/Invisible";
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.consumable = true;
			Item.value = Item.buyPrice(gold: 1);
			Item.rare = 1;
			Item.createTile = Mod.Find<ModTile>("BossTrophies").Type;
			Item.placeStyle = placeStyle;
		}
        public override string Texture => trophySprite;

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			Texture2D texture = ModContent.Request<Texture2D>(bossSprite);
			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, textureOrigin, Main.inventoryScale/2f, SpriteEffects.None, 0f);
		}

        public override bool CloneNewInstances => true;

		public BossTrophy() { } // An empty constructor is needed for tModLoader to attempt Autoload
		public BossTrophy(int placeStyle, string bossSprite = "SGAmod/Invisible", string trophySprite="SGAmod/Items/Placeable/BossTrophy") // This is the real constructor we use in Autoload
		{
			//this.trophySprite = "SGAmod/Items/Placeable/"+GetType().Name;
			this.placeStyle = placeStyle;
			this.bossSprite = bossSprite;
			this.trophySprite = trophySprite;
		}

		public override bool Autoload(ref string name)
        {
			Mod.AddItem("MaggotBanner", new SGABanner("Maggot"));
			Mod.AddItem("MaggotFlyBanner", new SGABanner("MaggotFly"));
			Mod.AddItem("BlackLeechBanner", new SGABanner("BlackLeech"));
			Mod.AddItem("DankMimicBanner", new SGABanner("DankMimic"));
			Mod.AddItem("DankSlimeBanner", new SGABanner("DankSlime"));
			Mod.AddItem("FlyBanner", new SGABanner("Fly"));
			Mod.AddItem("FlySwarmBanner", new SGABanner("FlySwarm"));
			Mod.AddItem("GiantLizardBanner", new SGABanner("GiantLizard"));
			Mod.AddItem("IceFairyBanner", new SGABanner("IceFairy"));
			Mod.AddItem("MudBallBanner", new SGABanner("MudBall"));
			Mod.AddItem("MudMummyBanner", new SGABanner("MudMummy"));
			Mod.AddItem("SandscorchedGolemBanner", new SGABanner("SandscorchedGolem"));
			Mod.AddItem("SandscorchedSlimeBanner", new SGABanner("SandscorchedSlime"));
			Mod.AddItem("SwampBatBanner", new SGABanner("SwampBat"));
			Mod.AddItem("SwampJellyBanner", new SGABanner("SwampJelly"));
			Mod.AddItem("TidalElementalBanner", new SGABanner("TidalElemental"));
			Mod.AddItem("SkeletonCrossbowerBanner", new SGABanner("SkeletonCrossbower"));
			Mod.AddItem("SkeletonGunnerBanner", new SGABanner("SkeletonGunner"));
			Mod.AddItem("DungeonBatBanner", new SGABanner("DungeonBat"));
			Mod.AddItem("ChaosCasterBanner", new SGABanner("ChaosCaster"));
			Mod.AddItem("EvilCasterBanner", new SGABanner("EvilCaster"));
			Mod.AddItem("FastSkeletonBanner", new SGABanner("FastSkeleton"));
			Mod.AddItem("FlamingSkullBanner", new SGABanner("FlamingSkull"));
			Mod.AddItem("HellCasterBanner", new SGABanner("HellCaster"));
			Mod.AddItem("LaserSkeletonBanner", new SGABanner("LaserSkeleton"));
			Mod.AddItem("RuneCasterBanner", new SGABanner("RuneCaster"));

			Mod.AddItem("CopperWraithTrophy", new BossTrophy(0, trophySprite: "SGAmod/Items/Placeable/CopperWraithTrophy"));
			Mod.AddItem("CaliburnATrophy", new BossTrophy(1, trophySprite: "SGAmod/Items/Placeable/CaliburnATrophy"));
			Mod.AddItem("CaliburnBTrophy", new BossTrophy(2, trophySprite: "SGAmod/Items/Placeable/CaliburnBTrophy"));
			Mod.AddItem("CaliburnCTrophy", new BossTrophy(3, trophySprite: "SGAmod/Items/Placeable/CaliburnCTrophy"));
			Mod.AddItem("SpiderQueenTrophy", new BossTrophy(4, trophySprite: "SGAmod/Items/Placeable/SpiderQueenTrophy"));
			Mod.AddItem("MurkTrophy", new BossTrophy(5, trophySprite: "SGAmod/Items/Placeable/MurkTrophy"));

			Mod.AddItem("CirnoTrophy", new BossTrophy(6, trophySprite: "SGAmod/Items/Placeable/CirnoTrophy"));
			Mod.AddItem("CobaltWraithTrophy", new BossTrophy(7, trophySprite: "SGAmod/Items/Placeable/CobaltWraithTrophy"));
			Mod.AddItem("SharkvernTrophy", new BossTrophy(8, trophySprite: "SGAmod/Items/Placeable/SharkvernTrophy"));
			Mod.AddItem("CratrosityTrophy", new BossTrophy(9, trophySprite: "SGAmod/Items/Placeable/CratrosityTrophy"));
			Mod.AddItem("TwinPrimeDestroyersTrophy", new BossTrophy(10, bossSprite: "SGAmod/Items/Placeable/BossTrophy_TPD"));
			Mod.AddItem("DoomHarbingerTrophy", new BossTrophy(11, trophySprite: "SGAmod/Items/Placeable/DoomHarbingerTrophy"));

			Mod.AddItem("LuminiteWraithTrophy", new BossTrophy(12, trophySprite: "SGAmod/Items/Placeable/LuminiteWraithTrophy"));
			Mod.AddItem("CratrogeddonTrophy", new BossTrophy(13, trophySprite: "SGAmod/Items/Placeable/CratrogeddonTrophy"));
			Mod.AddItem("SupremePinkyTrophy", new BossTrophy(14, trophySprite: "SGAmod/Items/Placeable/SupremePinkyTrophy"));
			Mod.AddItem("HellionTrophy", new BossTrophy(15, trophySprite: "SGAmod/Items/Placeable/HellionTrophy"));
			Mod.AddItem("PhaethonTrophy", new BossTrophy(16, trophySprite: "SGAmod/Items/Placeable/PhaethonTrophy"));

			Mod.AddItem("PrismicBansheeTrophy", new BossTrophy(17, trophySprite: "SGAmod/Items/Placeable/PrismicBansheeTrophy"));
			Mod.AddItem("TinWraithTrophy", new BossTrophy(18, trophySprite: "SGAmod/Items/Placeable/TinWraithTrophy"));
			Mod.AddItem("PalladiumWraithTrophy", new BossTrophy(19, trophySprite: "SGAmod/Items/Placeable/PalladiumWraithTrophy"));

			return false;
        }
    }

}