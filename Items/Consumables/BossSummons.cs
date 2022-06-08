#define DefineHellionUpdate

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.NPCs.Hellion;
using Terraria.GameContent.Events;
using SGAmod.Items.Weapons;
using SGAmod.NPCs;
using Terraria.Localization;
using Microsoft.Xna.Framework.Audio;
using SGAmod.HavocGear.Items;
using Terraria.Audio;

namespace SGAmod.Items.Consumables
{

	public class BaseBossSummon : ModItem
	{
		public override bool Autoload(ref string name)
		{
			return GetType()!=typeof(BaseBossSummon);
		}

		public override bool CanUseItem(Player player)
		{
			if (SGAmod.anysubworld)
			{
				if (player == Main.LocalPlayer)
					Main.NewText("This cannot be used outside the normal folds of reality...", 75, 75, 80);

				return false;
			}
			return base.CanUseItem(player);
		}


	}

		public class WraithCoreFragment3 : WraithCoreFragment
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Wraith Core Fragment");
			Tooltip.SetDefault("Summons forth the third and final of the Wraiths, who has stolen your ability to make Luminite Bars (and also the Ancient Manipulator from the Cultist)");
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Consumables/LunarCore"; }
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = 8;
		}

		public override bool CanUseItem(Player player)
		{
			if (SGAWorld.downedWraiths == 3 && !NPC.downedMoonlord)
			{
				Item.consumable = false;
			} else {
				Item.consumable = true;
			}
			return base.CanUseItem(player);
		}

		public override bool? UseItem(Player player)
		{
			if (item.consumable == false) {
				if (player == Main.LocalPlayer)
					Main.NewText("Our time has not yet come", 25, 25, 80);
				return false;
			} else {
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("LuminiteWraith"));
				Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			}
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddRecipeGroup("Fragment", 4).AddIngredient(null, "WraithCoreFragment2", 1).AddTile(TileID.WorkBenches).Register();
		}

	}

	public class WraithCoreFragment2 : WraithCoreFragment
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empowered Wraith Core Fragment");
			Tooltip.SetDefault("Summons forth the second of the Wraiths");
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Consumables/EmpoweredCore"; }
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.rare = 5;
		}

		public override bool? UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("CobaltWraith"));
			Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(null, "WraithCoreFragment", 1).AddRecipeGroup("SGAmod:Tier1HardmodeOre", 10).AddIngredient(mod.ItemType("WraithFragment3"), 5).AddTile(TileID.WorkBenches).Register();
		}

	}
	public class WraithCoreFragment : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wraith Core Fragment");
			Tooltip.SetDefault("Summons forth the first of the Wraiths, who watches you craft bars with envy...");
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Consumables/BasicCore"; }
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 2;
			Item.useAnimation = 2;
			Item.useStyle = 4;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.value = 0;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
		}

		public override bool CanUseItem(Player player)
		{
			if (Main.netMode==NetmodeID.Server)
			SGAmod.Instance.Logger.Warn("DEBUG SERVER: item canuse");
			if (Main.netMode == NetmodeID.MultiplayerClient)
				SGAmod.Instance.Logger.Warn("DEBUG CLIENT: item canuse");
			if (!NPC.AnyNPCs(Mod.Find<ModNPC>("CopperWraith").Type) && !NPC.AnyNPCs(Mod.Find<ModNPC>("CobaltWraith").Type) && !NPC.AnyNPCs(Mod.Find<ModNPC>("LuminiteWraith").Type))
			{
				return base.CanUseItem(player);
			} else {
				return false;
			}
		}
		public override bool? UseItem(Player player)
		{
			if (Main.netMode == NetmodeID.Server)
				SGAmod.Instance.Logger.Warn("DEBUG SERVER: item used");
			if (Main.netMode == NetmodeID.MultiplayerClient)
				SGAmod.Instance.Logger.Warn("DEBUG CLIENT: item used");

			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("CopperWraith"));
			Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			return true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddRecipeGroup("SGAmod:Tier1Ore", 15).AddIngredient(ItemID.FallenStar, 2).AddTile(TileID.WorkBenches).Register();
		}
	}

	public class ConchHorn : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Conch Horn");
			Tooltip.SetDefault("'It's call pierces the depths of the ocean.' \nSummons the Sharkvern");
		}
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 99;
			Item.rare = 3;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.useStyle = 4;
			Item.UseSound = SoundID.Item44;
			Item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ZoneBeach && !NPC.AnyNPCs(Mod.Find<ModNPC>("SharkvernHead").Type)) {
				return base.CanUseItem(player);
			} else {
				if (player == Main.LocalPlayer)
					Main.NewText("The couch blows but no waves are shaken by its ring...", 100, 100, 250);
				return false;

			}
		}

		public override bool? UseItem(Player player)
		{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SharkvernHead"));
				Main.PlaySound(SoundID.Roar, player.position, 0);
				return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Seashell, 1).AddIngredient(ItemID.SharkFin, 1).AddIngredient(ItemID.ChlorophyteBar, 5).AddTile(TileID.MythrilAnvil).Register();
		}
	}

	public class AcidicEgg : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acidic Egg");
			Tooltip.SetDefault("'No words for this...' \nSummons the Spider Queen\nRotten Eggs drop from spiders");
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.RottenEgg, 1).AddIngredient(ItemID.Cobweb, 25).AddRecipeGroup("SGAmod:EvilBossMaterials", 5).AddTile(TileID.DemonAltar).Register();
		}
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 99;
			Item.rare = 2;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.useStyle = 4;
			Item.UseSound = SoundID.Item44;
			Item.consumable = true;
		}

		public static bool Underground(Entity player) => (int)((double)((player.position.Y + (float)player.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;
		public static bool Underground(int here) => (int)((double)((here / 16f)*2.0) - Main.worldSurface * 2.0) > 0;

		public override bool CanUseItem(Player player)
		{
			//bool underground = (int)((double)((player.position.Y + (float)player.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;

			if (Underground(player) && !NPC.AnyNPCs(Mod.Find<ModNPC>("SpiderQueen").Type))
			{
				return true;
			}
			else
			{
				if (player == Main.LocalPlayer)
					Main.NewText("There are no spiders here, try using it underground", 30, 200, 30);
				return false;

			}
		}
		public override bool? UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SpiderQueen"));
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}
	}

	public class PrismaticBansheeStar : BaseBossSummon,IAuroraItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismatic Star");
			Tooltip.SetDefault("'Fallen star imbued with Luminous energy, a fabricated Prismic Egg'\nThrow it on Pearlstone in the underground Hallow\nAfter a short while summons the Aurora Banshee, an empowered version");
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(Item.Center, Color.Lerp(Color.BlueViolet, Color.HotPink, (float)Math.Sin((Main.essScale - 0.70f) / 0.30f)).ToVector3() * 0.85f * Main.essScale);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 30;
			Item.width = 26;
			Item.height = 14;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.consumable = true;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = 4;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.value = 0;
			Item.rare = 9;
			Item.UseSound = SoundID.Item35;
		}
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
			if (Item.velocity.Y == 0 && Item.stack<2)
			{
				//Main.NewText("Debug Message!");
				Point tilePosition = new Point((int)(Item.Center.X / 16), ((int)(Item.Center.Y) / 16) + 2);
				Tile tile = Framing.GetTileSafely(tilePosition.X, tilePosition.Y);
				//Main.NewText(tile.type + " this type "+item.position);
				if (tile.TileType == TileID.Pearlstone && AcidicEgg.Underground(Item))
				{
					Item.ownTime += 2;

					if (Item.ownTime % 50 == 0)
					{
						SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_DarkMageCastHeal, Item.Center);
						if (sound != null)
						{
							sound.Pitch = -0.65f + (Item.ownTime / 1000f);
						}
					}

					if (Item.ownTime > 600 && Item.stack<2)
                    {
						NPC.NewNPC((int)Item.Center.X, (int)Item.Center.Y, ModContent.NPCType<PrismBanshee>());
						Item.active = false;
                    }
				}

			}
		}
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
			PrismBanshee.DrawPrismCore(spriteBatch, lightColor, Item.Center, Item.ownTime*0.8f, Item.scale, 96f * (Item.ownTime/600f));
			return true;
        }
        public override bool CanUseItem(Player player)
		{
			bool underground = (int)((double)((player.position.Y + (float)player.height) * 2f / 16f) - Main.worldSurface * 2.0) > 0;
			;
			if (underground && player.ZoneHoly && !NPC.AnyNPCs(Mod.Find<ModNPC>("PrismBanshee").Type))
			{
				if (player == Main.LocalPlayer)
					Main.NewText("Here is good, rest it on some pearlstone!", 200, 100, 150);
				return false;
			}
			else
			{
				if (player == Main.LocalPlayer)
					Main.NewText("'it rejects activating where it was not originally from...'", 200, 100, 150);
				return false;

			}
		}
		public override bool? UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("PrismBanshee"));
			Main.PlaySound(SoundID.Roar, player.position, 1);
			return true;
		}
	}

	public class RoilingSludge : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Roiling Sludge");
			Tooltip.SetDefault("'Ew, Gross!' \nSummons the Murk");
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<WraithFragment3>(), 5).AddIngredient(ModContent.ItemType<MoistSand>(), 10).AddIngredient(ItemID.MudBlock, 10).AddIngredient(ItemID.Gel, 20).AddIngredient(ItemID.Bone, 5).AddTile(TileID.Furnaces).Register();
		}
		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 12;
			Item.maxStack = 99;
			Item.rare = 2;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.useStyle = 4;
			Item.UseSound = SoundID.Item44;
			Item.consumable = true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ZoneJungle && !NPC.AnyNPCs(Mod.Find<ModNPC>("Murk").Type) && !NPC.AnyNPCs(Mod.Find<ModNPC>("BossFlyMiniboss1").Type)) {
				return base.CanUseItem(player);
			} else {
				if (player == Main.LocalPlayer)
					Main.NewText("There is a lack of mud and sludge for Murk to even exist here...", 40, 180, 60);
				return false;

			}
		}

		public override bool? UseItem(Player player)
		{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType(SGAWorld.downedMurk == 0 || TheWholeExperience.Check() ? "BossFlyMiniboss1" : "Murk"));
				Main.PlaySound(SoundID.Roar, player.position, 0);
				return true;
		}
	}

	public class Prettygel : BaseBossSummon,IAuroraItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminous Gel");
			Tooltip.SetDefault("Makes pinky very JELLLLYYYYY");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 2;
			Item.useAnimation = 2;
			Item.useStyle = 4;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.value = 0;
			Item.rare = ItemRarityID.Cyan;
			Item.UseSound = SoundID.Item1;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (SGAmod.Calamity.Item1)
				tooltips.Add(new TooltipLine(Mod, "NoU", "Summoning this boss with automatically disable Revengence and Death Modes"));
		}

		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(Mod.Find<ModNPC>("SPinky").Type) && !NPC.AnyNPCs(50) && !Main.dayTime)
			{
				return base.CanUseItem(player);
			} else {
				if (player == Main.LocalPlayer)
					Main.NewText("this gel shimmers only in moonlight...", 100, 40, 100);
				return false;
			}
		}

		public override bool? UseItem(Player player)
		{
			if (item.consumable == true) {
				SGAmod.CalamityNoRevengenceNoDeathNoU();
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("SPinky"));
				Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
				//player.GetModPlayer<SGAPlayer>().Locked=new Vector2(player.Center.X-2000,4000);
			}
			return true;
		}

		public override void AddRecipes()
		{
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 3);
			recipe.AddIngredient(mod.ItemType("IlluminantEssence"), 3);
			recipe.AddIngredient(3111, 10); //pink gel
			recipe.AddTile(220); //Soldifier
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 5);
			recipe.AddIngredient(mod.ItemType("IlluminantEssence"), 3);
			recipe.AddIngredient(mod.ItemType("MurkyGel"), 20);
			recipe.AddTile(220); //Soldifier
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}

	public class Nineball : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nineball");
			Tooltip.SetDefault("Summons the strongest ice fairy\nMake sure you don't hit a wall and aim up");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = 4;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.value = 0;
			Item.noUseGraphic = true;
			Item.rare = 9;
			Item.ammo = AmmoID.Snowball;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ProjectileID.SnowBallFriendly;
			Item.shootSpeed = 10f;
		}

		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(Mod.Find<ModNPC>("Cirno").Type) && Main.projectile.FirstOrDefault(proj => proj.type == ModContent.ProjectileType<CirnoBall>() && proj.active) == default)
			{
				if (!Main.dayTime || !player.ZoneSnow)
				{
				if (player == Main.LocalPlayer)
					Main.NewText("It's power lies in the snow biome during the day", 50, 50, 250);
					return false;
				}
				else
				{
					return true;
				}
			}
			return false;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = ModContent.ProjectileType<CirnoBall>();
			return true;
		}
        public override bool? UseItem(Player player)
		{
			if (item.consumable == false)
			{

			}
			else
			{
				//NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Cirno"));
				//Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			}
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Snowball, 1).AddIngredient(ItemID.SoulofNight, 2).AddIngredient(ItemID.SoulofLight, 2).AddIngredient(mod.ItemType("FrigidShard"), 9).AddIngredient(mod.ItemType("IceFairyDust"), 9).AddTile(TileID.IceMachine).Register();
		}
	}

	public class CirnoBall : JarateShurikensProg
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cirno Ball");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Consumables/Nineball"); }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//nil
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.SnowBallFriendly);
			// projectile.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.tileCollide = true;
			Projectile.friendly = false;
			Projectile.hostile = false;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void AI()
		{
			Projectile.localAI[1] += 1;
			if (Projectile.localAI[1] > 60)
            {
				Projectile.aiStyle = -1;
				Projectile.velocity *= 0.90f;

				if ((int)Projectile.localAI[1] % 30 == 0)
				{
					SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 25);
					if (sound!=null)
						sound.Pitch += (Projectile.localAI[1] - 60) / 420f;
				}

				for (int num654 = 0; num654 < 1 + Projectile.localAI[1]/9f; num654++)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 9.00);
					Dust num655 = Dust.NewDustPerfect(Projectile.Center+new Vector2(2,2) + ogcircle * 10f, 59, -Projectile.velocity + randomcircle * 2f, 150, Color.Aqua, 1.5f);
					num655.noGravity = true;
					num655.noLight = true;
				}

				if (Projectile.localAI[1] > 360)
                {
					NPC FakeNPC = new NPC();
						FakeNPC.SetDefaults(ModContent.NPCType<Cirno>());
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						int npc = NPC.NewNPC((int)Projectile.Center.X, (int)Projectile.Center.Y + 24, ModContent.NPCType<Cirno>());
					}
					else
					{
						if (Main.netMode != NetmodeID.Server && Main.myPlayer == Projectile.owner)
						{
							ModPacket packet = Mod.GetPacket();
							packet.Write((ushort)999);
							packet.Write((int)Projectile.Center.X);
							packet.Write((int)Projectile.Center.Y);
							packet.Write(ModContent.NPCType<NPCs.Cirno>());
							packet.Write(0);
							packet.Write(0);
							packet.Write(0);
							packet.Write(0);
							packet.Write(Projectile.owner);
							packet.Send();
						}
					}

					string typeName2 = FakeNPC.TypeName;
					if (Main.netMode == 0)
					{
						Main.NewText(Language.GetTextValue("Announcement.HasAwoken", typeName2), 175, 75);
					}
					else if (Main.netMode == 2)
					{
						NetMessage.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", FakeNPC.GetTypeNetName()), new Color(175, 75, 255));
					}

					SoundEngine.PlaySound(SoundID.Roar, (int)Projectile.position.X, (int)Projectile.position.Y, 0);
					for (float num654 = 0; num654 < 25 + Projectile.localAI[1] / 10f; num654+=0.25f)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
						int num655 = Dust.NewDust(Projectile.Center + new Vector2(2, 2) + ogcircle * 16f, 0, 0, 88, -Projectile.velocity.X + randomcircle.X * 6f, -Projectile.velocity.Y + randomcircle.Y * 6f, 150, Color.Aqua, 1.6f);
						Main.dust[num655].noGravity = true;
						Main.dust[num655].noLight = true;
					}
					Projectile.Kill();

				}

			}

		}

	}

	public class CaliburnCompess : BaseBossSummon
	{
		private float effect = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn Compass");
			Tooltip.SetDefault("When held, it points to Caliburn Altars in your world\nCan be used in hardmode to fight a stronger Caliburn spirit\nNon-Consumable");
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.rare = 2;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 4;
		}

		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(Mod.Find<ModNPC>("CaliburnGuardianHardmode").Type) && player.GetModPlayer<SGAPlayer>().DankShrineZone && Main.hardMode)
			{
				return base.CanUseItem(player);
			}
			else
			{
				if (player == Main.LocalPlayer)
					Main.NewText("The compass points the way to a shrine...", 0, 75, 0);
				return false;
			}
		}

		public override bool? UseItem(Player player)
		{
			if (Main.hardMode && player.GetModPlayer<SGAPlayer>().DankShrineZone)
			{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("CaliburnGuardianHardmode"));
				Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			}
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (SGAWorld.darknessVision)
			{
				tooltips.Add(new TooltipLine(Mod, "CaliburnCompessUpgrade", Idglib.ColorText(Color.MediumPurple, "Upgraded to also point to Dark Sectors in the world")));
				tooltips.Add(new TooltipLine(Mod, "CaliburnCompessUpgrade", Idglib.ColorText(Color.MediumPurple, "Darkness from Dark Sectors is reduced while in your inventory")));
			}
		}

	}
	public class Mechacluskerf : BaseBossSummon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mechanical Clusterfuck");
			Tooltip.SetDefault("Summons the Twin-Prime-Destroyers\nIt is highly encourged you do not fight this before late hardmode...");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 2;
			Item.useAnimation = 2;
			Item.useStyle = 4;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.value = 0;
			Item.rare = 9;
			Item.UseSound = SoundID.Item1;
		}

		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(Mod.Find<ModNPC>("TPD").Type) && !NPC.AnyNPCs(50))
			{
				if (Main.dayTime)
				{
					Item.consumable = false;
				}
				else
				{
					Item.consumable = true;
				}
				return base.CanUseItem(player);
			}
			else
			{
				return false;
			}
		}
		public override bool? UseItem(Player player)
		{
			if (item.consumable == false || Main.dayTime)
			{
				if (player == Main.LocalPlayer)
					Main.NewText("Their terror only rings at night", 150, 5, 5);
			}
			else
			{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("TPD"));
				Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			}
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(544, 1).AddIngredient(556, 1).AddIngredient(557, 1).AddIngredient(547, 3).AddIngredient(548, 3).AddIngredient(549, 3).AddTile(TileID.MythrilAnvil).Register();
		}
	}
	public class TruelySusEye : BaseBossSummon
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Truly Suspicious Looking Eye");
			Tooltip.SetDefault("Summons the Servants of the lord of the moon...\nOnly useable after Tier 3 Old One's Army event and Martians are beaten" +
				"\nUse at Night\nDoesn't work online");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (SGAmod.Calamity.Item1)
				tooltips.Add(new TooltipLine(Mod, "NoU", "Summoning this boss with automatically disable Revengence and Death Modes"));
		}
		public override bool CanUseItem(Player player)
	{
			if ((DD2Event.DownedInvasionT3 && NPC.downedMartians) && !Main.dayTime && Main.netMode<1)
			{
				if (NPC.CountNPCS(Mod.Find<ModNPC>("Harbinger").Type)<1 && NPC.CountNPCS(NPCID.MoonLordFreeEye) < 1)
				{
					return base.CanUseItem(player);
				}
				else
				{
					return false;
				}
			}
			else
			{
				Main.NewText("No...", 0, 0, 75);
				return false;
			}
	}
	public override bool? UseItem(Player player)
	{
			SGAmod.CalamityNoRevengenceNoDeathNoU();
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Harbinger"));
		//Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
		return true;
	}

	public override void AddRecipes()
	{
		CreateRecipe(1).AddIngredient(ItemID.Ectoplasm, 5).AddIngredient(ItemID.SuspiciousLookingEye, 1).AddTile(TileID.CrystalBall).Register();
	}

	public override void SetDefaults()
	{
		Item.rare = 9;
		Item.maxStack = 999;
		Item.consumable = true;
		Item.width = 40;
		Item.height = 40;
		Item.useTime = 30;
		Item.useAnimation = 30;
		Item.useStyle = 4;
		Item.noMelee = true; //so the item's animation doesn't do damage
		Item.value = 0;
		Item.UseSound = SoundID.Item8;
	}


}

#if DefineHellionUpdate
	public class HellionSummon : BaseBossSummon
	{
		public static ModItem instance;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reality's Sunder");
		}

		public override bool ConsumeItem(Player player)
		{
			return false;
		}

		public override bool Autoload(ref string name)
		{
			instance = this;
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (SGAWorld.downedHellion < 2)
			{
				TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Material" && x.Mod == "Terraria");
				if (tt != null)
				{
					int index = tooltips.FindIndex(here => here == tt);
					tooltips.RemoveAt(index);
				}
			}
			else
			{
				tooltips.Add(new TooltipLine(Mod, "Nmxx", "Useable in crafting"));
			}

			if (SGAWorld.downedSPinky && SGAWorld.downedCratrosityPML && SGAWorld.downedWraiths>3)
			{
				if (SGAWorld.downedHellion > 0)
					tooltips.Add(new TooltipLine(Mod, "Nmxx", "Hold 'left control' while you use the item to skip Hellion Core, this costs 25 Souls of Byte"));
				if (SGAWorld.downedHellion < 2)
				{
					if (SGAWorld.downedHellion == 0)
					{
						tooltips.Add(new TooltipLine(Mod, "Nmxx", "'Well done " + SGAmod.HellionUserName + ". Yes, I know your real name behind that facade you call " + Main.LocalPlayer.name + ".'"));
						tooltips.Add(new TooltipLine(Mod, "Nmxx", "'And thanks to your Dragon's signal, I have found my way to your world, this one tear which will let me invade your puny little " + Main.worldName + "'"));
						tooltips.Add(new TooltipLine(Mod, "Nmxx", "'Spend what little time you have left meaningful, if you were expecting to save him, I doubt it'"));
						tooltips.Add(new TooltipLine(Mod, "Nmxx", "'But let us not waste anymore time, come, face me'"));
					}
					else
					{
						tooltips.Add(new TooltipLine(Mod, "Nmxx", "'Getting closer, I guess now I'll just have to use more power to stop you'"));
						tooltips.Add(new TooltipLine(Mod, "Nmxx", "'But enough talk, lets finish this'"));
					}
				}
				else
				{
					tooltips.Add(new TooltipLine(Mod, "Nmxx", "'Hmp, very Well done " + SGAmod.HellionUserName + ", you've bested me, this time"));
					tooltips.Add(new TooltipLine(Mod, "Nmxx", "But next time you won't be so lucky..."));
					tooltips.Add(new TooltipLine(Mod, "Nmxx", "My tears have stablized..."));
					tooltips.Add(new TooltipLine(Mod, "Nmxx", "Enjoy your fancy reward, you've earned that much..."));
					tooltips[0].Text += " (Stablized)";
				}
				tooltips.Add(new TooltipLine(Mod, "Nmxx", "Tears a hole in the bastion of reality to bring forth the Paradox General, Helen 'Hellion' Weygold"));
				tooltips.Add(new TooltipLine(Mod, "Nmxx", "Non Consumable"));


					foreach (TooltipLine line in tooltips)
				{
					string text = line.Text;
					string newline = "";
					for (int i = 0; i < text.Length; i += 1)
					{
						newline += Idglib.ColorText(Color.Lerp(Color.White, Main.hslToRgb((Main.rand.NextFloat(0, 1)) % 1f, 0.75f, Main.rand.NextFloat(0.25f, 0.5f)),MathHelper.Clamp(0.5f+(float)Math.Sin(Main.GlobalTimeWrappedHourly*2f)/1.5f,0.2f,1f)), text[i].ToString());


					}
					line.Text = newline;
				}
			}
			else
			{
				tooltips = new List<TooltipLine>();
			}

		}

		public override bool CanUseItem(Player player)
		{
			if (Main.netMode > 0)
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					Hellion hell = new Hellion();
					hell.HellionTaunt("This fight is not possible in Multiplayer, comeback in Single Player");
				}
				return false;
            }

			if (Hellion.GetHellion()==null && !IdgNPC.bossAlive && SGAWorld.downedSPinky && SGAWorld.downedCratrosityPML && SGAWorld.downedWraiths > 3 && NPC.CountNPCS(Mod.Find<ModNPC>("HellionMonolog").Type)<1)
			{
				if (!Main.expertMode)
				{
					Hellion hell = new Hellion();
					hell.HellionTaunt("What makes you think I'm going to challenge a NORMAL difficulty player? You shouldn't even have this item, cheater...");
					return false;
				}
				return base.CanUseItem(player);
			}
			else
			{
				return false;
			}
		}
		public override bool? UseItem(Player player)
		{
			if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) && player.CountItem(mod.ItemType("ByteSoul")) > 24 && SGAWorld.downedHellion > 0)
			{
				for (int i = 0; i < 25; i += 1)
				{
					player.ConsumeItem(mod.ItemType("ByteSoul"));
				}
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Hellion"));
			}
			else
			{
				NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("HellionCore"));
			}
			//Main.PlaySound(15, (int)player.position.X, (int)player.position.Y, 0);
			return true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(mod.ItemType("WraithCoreFragment3"), 1).AddIngredient(mod.ItemType("RoilingSludge"), 1).AddIngredient(mod.ItemType("Mechacluskerf"), 1).AddIngredient(mod.ItemType("Nineball"), 1).AddIngredient(mod.ItemType("AcidicEgg"), 1).AddIngredient(mod.ItemType("Prettygel"), 1).AddIngredient(mod.ItemType("ConchHorn"), 1).AddIngredient(mod.ItemType("CosmicFragment"), 1).AddIngredient(mod.ItemType("MoneySign"), 10).AddIngredient(mod.ItemType("LunarRoyalGel"), 20).AddTile(TileID.LunarCraftingStation).Register();
		}

		public override void SetDefaults()
		{
			Item.rare = 12;
			Item.maxStack = 1;
			Item.consumable = false;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = 4;
			Item.noMelee = true; //so the item's animation doesn't do damage
			Item.value = 0;
			Item.rare = -1;
			Item.expert = true;
			Item.UseSound = SoundID.Item8;
		}

		public override string Texture
		{
			get { return ("Terraria/Extra_19"); }
		}

		private static void drawit(Vector2 where, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI, Matrix zoomitz)
		{

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, zoomitz);

			int width = 32; int height = 32;

			Texture2D beam = new Texture2D(Main.graphics.GraphicsDevice, width, height);
			var dataColors = new Color[width * height];

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					float dist = (new Vector2(x, y) - new Vector2(width / 2, height / 2)).Length();
					if (Main.rand.NextFloat(dist, 32)<16f)
					{
						float alg = ((-Main.GlobalTimeWrappedHourly + ((float)(dist) / 4f)) / 2f);
						dataColors[x + y * width] = Main.hslToRgb(alg % 1f, 0.75f, 0.5f);
					}
				}
			}

			beam.SetData(0, null, dataColors, 0, width * height);
			spriteBatch.Draw(beam, where + new Vector2(2, 2), null, Color.White, 0, new Vector2(beam.Width / 2, beam.Height / 2), scale * 2f, SpriteEffects.None, 0f);

		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			float gg = 0f;
			drawit(position + new Vector2(11, 11), spriteBatch, drawColor, drawColor, ref gg, ref scale, 1, Main.UIScaleMatrix);
			return false;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{


			drawit(Item.Center - Main.screenPosition, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}

	}
#endif


}
