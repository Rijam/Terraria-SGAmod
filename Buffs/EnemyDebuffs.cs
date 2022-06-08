using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Buffs
{
	public class Sunburn : ModBuff
	{
		public override string Texture => "Terraria/Images/Buff_" + BuffID.OnFire;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun Burn");
			Description.SetDefault("'The air would be burning if there was oxygen...'");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (!player.buffImmune[BuffID.OnFire])
			{
				if (Main.rand.Next(100) < 10)
					player.onFire = true;

				if (player.lifeRegen > 0)
				{
					player.lifeRegen = 0;
				}
				player.lifeRegenTime = 0;
				player.lifeRegen -= 2;
			}
		}
	}
	public class TechnoCurse : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Techno Curse");
			Description.SetDefault("Technological damage is reduced by 50%");
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
	public class MiningFatigue : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mining Fatigue");
			Description.SetDefault("Mining speed is reduced!");
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().UseTimeMulPickaxe /= 4f;
		}
	}
	public class Watched : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Watched");
			Description.SetDefault("You are exposed, tread carefully...");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().watcherDebuff += 500;
		}
        public override void Update(NPC npc, ref int buffIndex)
        {
			npc.SGANPCs().watched = 10;
		}
    }
	public class NoFly : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snowfrosted");
			Description.SetDefault("Cirno's presence is making flight difficult...");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = false; //true now?
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().NoFly = true;
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			if (NPC.CountNPCS(Mod.Find<ModNPC>("Cirno").Type) < 1 && !SGAWorld.downedCirno)
				tip += "\nBeat Cirno to remove this effect";
			if (NPC.CountNPCS(Mod.Find<ModNPC>("Hellion").Type) > 0)
				tip += "\nHellion's Army has Cirno using this debuff against you";
		}
	}

}