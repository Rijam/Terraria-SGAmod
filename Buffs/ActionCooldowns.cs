using Terraria;
using Terraria.ModLoader;
using System;
using Idglibrary;
using SGAmod.NPCs;
using Terraria.ID;

namespace SGAmod.Buffs
{
	public class ActionCooldown: ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Action Cooldown");
			Description.SetDefault("Cannot use a special item yet");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = false; //true now?
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().ActionCooldown = true;
		}
	}

	public class StarStormCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Divive Shower Cooldown");
			Description.SetDefault("Another Night...");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = false; //true now?
		}

		public override string Texture => "SGAmod/Buffs/ActionCooldown";
	}
	public class BossHealingCooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anticipation Cooldown");
			Description.SetDefault("Your pre-boss fight healing cannot activate again");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = false; //true now?
		}


		public override string Texture => "SGAmod/Buffs/AnticipationCooldownDebuff";

		public override void Update(Player player, ref int buffIndex)
		{
			if (IdgNPC.bossAlive)
				player.buffTime[buffIndex] = Math.Max(player.buffTime[buffIndex], 2);
		}
	}


}
