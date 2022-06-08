using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.GameContent.UI;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Tools
{
	public class HammerEditor : ModItem
	{
		int myid = -1;

		public override bool Autoload(ref string name)
		{
			return false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hammer Editor");
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {

            Color c = Main.hslToRgb((float)(Main.GlobalTimeWrappedHourly/15)%1f, 0.2f, 0.45f);
            tooltips.Add(new TooltipLine(Mod,"Things", Idglib.ColorText(c, "All the functionality of the Grand Design And Cell phone")));
			tooltips.Add(new TooltipLine(Mod, "Things", Idglib.ColorText(c, "Grants surperior world building powers and mining speed while in inventory")));
			tooltips.Add(new TooltipLine(Mod, "Things", Idglib.ColorText(c, "Hold Shift and left click to teleport home")));
		}

		public override void SetDefaults()
		{
			myid = Item.type;
			Item.CloneDefaults(ItemID.WireKite);
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D tex = Mod.Assets.Request<Texture2D>("Items/Tools/HammerEditor").Value;
			Vector2 origin2 = new Vector2(tex.Width/2, tex.Height / 2);
			spriteBatch.Draw(tex, position, null, drawColor, 0f, origin2,1f, SpriteEffects.None, 0f);
			return false;
			//return base.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}

		public override void PostUpdate()
		{
			Item.type = myid;
		}

		public override bool CanUseItem(Player player)
		{
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
				Item.type = ItemID.CellPhone;
			return true;
		}

		public override void UpdateInventory(Player player)
		{
			Item.type = myid;
			if (player.HeldItem == Item)
			{
				Item.type = ItemID.WireKite;
				//if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
				//item.type = ItemID.CellPhone;
				Item.CloneDefaults(Item.type);
				Item.SetNameOverride("Hammer Editor");
				player.InfoAccMechShowWires = true;
				player.rulerLine = true;
				player.rulerGrid = true;
			}

			player.tileSpeed += 1f;
			player.wallSpeed += 1f;
			player.blockRange += 5;
			player.pickSpeed -= 0.5f;

			player.autoActuator = true;
			player.autoPaint = true;
			if (player.whoAmI == Main.myPlayer)
			{
				Player.tileRangeX += 3;
				Player.tileRangeY += 2;
			}



			player.accWatch = 3;
			player.accDepthMeter = 1;
			player.accCompass = 1;
			player.accFishFinder = true;
			player.accWeatherRadio = true;
			player.accCalendar = true;
			player.accThirdEye = true;
			player.accJarOfSouls = true;
			player.accCritterGuide = true;
			player.accStopwatch = true;
			player.accOreFinder = true;
			player.accDreamCatcher = true;

			Item.type = myid;




		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.WireKite, 1).AddIngredient(ItemID.CellPhone, 1).AddIngredient(ItemID.ActuationAccessory, 1).AddIngredient(ItemID.ArchitectGizmoPack, 1).AddIngredient(ItemID.BuilderPotion, 10).AddIngredient(mod.ItemType("CosmicFragment"), 1).AddTile(mod.GetTile("ReverseEngineeringStation")).Register();
		}


	}

}