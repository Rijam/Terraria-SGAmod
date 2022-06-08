using Terraria;
using Terraria.ModLoader;
using SGAmod.NPCs;
using Terraria.ID;

namespace SGAmod.Buffs
{
	public class Locked: ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Locked");
			Description.SetDefault("There is no escape");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().Lockedin = true;
		}

		//public override void Update(NPC npc, ref int buffIndex)
		//{
		//	npc.GetGlobalNPC<SGAWorld>(mod).eFlames = true;
		//}
	}
}
