using Microsoft.Xna.Framework;
using System;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod.Items.Tools
{
	public class Spanner : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spanner");
			Tooltip.SetDefault("Activates wired devices without wires");
		}

		public override void SetDefaults()
		{
			Item.damage = 0;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.knockBack = 6;
			Item.value = Item.buyPrice(0,2);
			Item.rare = 2;
			Item.UseSound = SoundID.Item55;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
			Item.useTurn = true;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Tools/Spanner").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -18;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = 6;
				Item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Lighting.GetColor((int)player.Center.X/16, (int)player.Center.Y / 16,Color.White);
				};
			}
		}

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(18,6);
        }

		public override bool? UseItem(Player player)
        {
			if (Main.netMode != NetmodeID.Server)
			{
				Point16 there = new Point16(Player.tileTargetX, Player.tileTargetY);
				if (player.Distance(there.ToVector2()*16) < (Math.Sqrt(Player.tileRangeX * Player.tileRangeY)+player.blockRange) * 16)
				{
					if (WorldGen.InWorld(there.X, there.Y))
					{
						Wiring.blockPlayerTeleportationForOneIteration = true;

						//Vanilla, fuck you
						MethodInfo stupidPrivateMethodWhy = typeof(Wiring).GetMethod("HitWireSingle", BindingFlags.NonPublic | BindingFlags.Static);
						stupidPrivateMethodWhy.Invoke(null, new object[] { there.X, there.Y });

						//Wiring.TripWire(there.X, there.Y,1,1);
						//NetMessage.SendData(MessageID.HitSwitch, -1, -1, null, there.X, there.Y);
					}

				}
			}
			return true;
        }

	}

}