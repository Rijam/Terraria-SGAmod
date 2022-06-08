using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Tools
{
	public class Powerjack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Powerjack");
			Tooltip.SetDefault("While in hand:\n15% increase to all movement speed, 25 life restored on kill\n"+Idglib.ColorText(Color.Red,"20% increased damage taken")+"\nCan smash altars");
		}

        /*public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

            Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly/4)%1f, 0.4f, 0.45f);
            string potion="[i:" + ItemID.RedPotion + "]";
            tooltips.Add(new TooltipLine(mod,"IDG Debug Item", potion+Idglib.ColorText(c,"Mister Creeper's dev weapon")+potion));
        }*/

		public override void SetDefaults()
		{
			Item.damage = 25;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 12;
			Item.useAnimation = 24;
			Item.hammer = 80;
			Item.useStyle = 1;
			Item.knockBack = 8;
			Item.value = 10000;
			Item.rare = 6;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.useTurn = true;
		}

	}

}