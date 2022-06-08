using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;

namespace SGAmod.Buffs
{
	public class ThermalBlaze : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Blaze");
			Description.SetDefault("Incinerated by the burning air...");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().thermalblaze = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().thermalblaze = true;
		}
	}
	public class AcidBurn : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Burn");
			Description.SetDefault("Reduced Defense and your defense works again your life");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().acidburn = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.SGANPCs().acidburn = true;
			npc.SGANPCs().reducedDefense += 5;
		}
	}
	public class RadioDebuff : ModBuff
	{

		public static string RadioactiveDebuffText => Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) ? "Irradiated enemies explode on death, doing damage around them based the irradiated ammounts\nGains more strength and deals extra damage if exposed to radiation sources for longer\nRate gained and max capped is based on a per-weapon basis" : "(Hold LEFT CONTROL for more info on Irradiated)";

		public override string Texture => "SGAmod/Buffs/BuffTemplate";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Irradiated");
			Description.SetDefault("Not for players to see!");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.buffTime[buffIndex] < 200)
			{
				npc.GetGlobalNPC<SGAnpcs>().IrradiatedAmmount_ = 0;
				npc.GetGlobalNPC<SGAnpcs>().IrradiatedAmmount = 0;
			}
		}
	}

	public class Targeted : ModBuff
	{
		public override string Texture => "SGAmod/Buffs/TargetedDebuff";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Doused In Gas");
			Description.SetDefault("You are coated in a highly flammable substance");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}

	public class Petrified : ModBuff
	{
		public override string Texture => "SGAmod/Buffs/PetrifiedDebuff";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Petrified");
			Description.SetDefault("You are coated in a highly flammable substance");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.SGANPCs().petrified = true;
		}
	}

	public class Marked : ModBuff
	{
		public override string Texture => "SGAmod/Buffs/MarkedDebuff";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marked");
			Description.SetDefault("You are coated in a highly flammable substance");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			SGAnpcs npcs = npc.GetGlobalNPC<SGAnpcs>();
			npcs.marked = true;
			npcs.damagemul += 0.10f;

		}
	}

	public class InfinityWarStormbreaker : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("IWS");
			Description.SetDefault("Players arn't meant to have this debuff!");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			//player.statDefense /= 2;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().InfinityWarStormbreaker = true;
		}
	}
	public class NinjaSmokedDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ninja Smoked");
			Description.SetDefault("Enemies more likely to dodge your attacks");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().NinjaSmoked = true;
		}
	}
	public class Gourged : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gouged");
			Description.SetDefault("Halved defense");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense /= 2;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.buffImmune[BuffID.Bleeding])
			{
				npc.DelBuff(buffIndex);
				return;
			}
			npc.GetGlobalNPC<SGAnpcs>().Gourged = true;
		}
	}

	public class SunderedDefense : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sundered Defense");
			Description.SetDefault("Your immunity frames are wrecked");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.immuneTime = Math.Max(player.immuneTime - 3, 0);
			player.GetModPlayer<SGAPlayer>().SunderedDefense = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().SunderedDefense = true;
		}
	}

	public class DankSlow : ModBuff
	{

		public static string DankText => Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) ? "Attacks may slow enemies\nConsecutive attacks will stack the debuff, making the slowing effect stronger, but this soft caps after a point\nDoes not affect Bosses or enemies who are immune to poisoned are also immune to Dank Slow" : "(Hold LEFT CONTROL for more info on Dank Slow)";
		public override string Texture => "SGAmod/Buffs/DankSlowDebuff";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dank Slow");
			Description.SetDefault("Players arn't meant to have this debuff!");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			npc.buffTime[buffIndex] = (int)Math.Pow(npc.buffTime[buffIndex] + (int)time, 0.98);
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			//player.statDefense /= 2;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().TimeSlow += (npc.buffTime[buffIndex] / (60f * 5f));
			npc.GetGlobalNPC<SGAnpcs>().DankSlow = true;
		}
	}

	public class GildingAuraBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aureation Aura");
			Description.SetDefault("Nearby NPCs are gilded with Midas");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            foreach(NPC npc in Main.npc.Where(testby => testby.DistanceSQ(player.MountedCenter) < 600 * 600))
            {
				npc.AddBuff(BuffID.Midas, 2);
			}
        }

    }

		public class RustBurn : ModBuff
	{
		public static string RustText => Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) ? "Rustburn lowers defense by 25 and is effective against inorganic enemies\nInorganic enemies with Rustburn take even more damage from Acid Burn\nOrganic enemies only take a bit of damage over time" : "(Hold LEFT CONTROL for more info on Rust Burn)";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rust Burn");
			Description.SetDefault("Slowness is happening");
			Main.debuff[Type] = true; 
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override string Texture => "SGAmod/Buffs/RustBurnDebuff";

		public static bool ApplyRust(NPC npc,int time)
        {
			int bufftype = ModContent.BuffType<RustBurn>();
			if (npc.buffImmune[bufftype] || npc.HasBuff(bufftype))
				return false;

				npc.AddBuff(ModContent.BuffType<RustBurn>(), time);
			SoundEngine.PlaySound(SoundID.Splash,(int)npc.Center.X, (int)npc.Center.Y, 1, 1f,0.75f);
			for (float f = 0; f < 16; f += 1)
			{
				Vector2 randomcircle = Main.rand.NextVector2CircularEdge(Main.rand.NextFloat(2f, 4f), Main.rand.NextFloat(2f, 4f)).RotatedBy(Main.rand.NextFloat());
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 27);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity = randomcircle * 3f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].shader = GameShaders.Armor.GetShaderFromItemId(ItemID.PhaseDye);
			}

				return true;
		}

		public static bool IsInorganic(NPC npc)
        {
			return npc.HitSound == SoundID.NPCHit4 || npc.HitSound == SoundID.NPCHit7;

		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			int timer = (int)((10 + (IsInorganic(npc) ? 40 : 0)) * MathHelper.Min(npc.buffTime[buffIndex] / 150f, 1f));
			npc.SGANPCs().impaled += 10 + (IsInorganic(npc) ? 40 : 0);
			npc.SGANPCs().reducedDefense += IsInorganic(npc) ? 25 : 0;
		}
	}
	public class LavaBurn : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Severe Lava Burn");
			Description.SetDefault("Magma scorches your body\nObsidian Skin is disabled");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}
		public override string Texture => "SGAmod/Buffs/LavaBurnDebuff";
		public override void Update(Player player, ref int buffIndex)
		{
			if (GetType() == typeof(LavaBurn))
			player.lavaImmune = false;
			player.SGAPly().lavaBurn = true;

		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().lavaBurn = true;
		}
	}
	public class LavaBurnLight : LavaBurn
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Burn");
			Description.SetDefault("Magma burns your skin");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}
	}
	public class MoonLightCurse : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Light Curse");
			Description.SetDefault("Defense and life are shattered");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.SGANPCs().MoonLightCurse = true;
			npc.SGANPCs().reducedDefense += 50;
		}
	}
	public class MassiveBleeding : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Massive Bleeding");
			Description.SetDefault("You are bleeding out");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().MassiveBleeding = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.buffImmune[BuffID.Bleeding])
			{
				npc.DelBuff(buffIndex);
				return;
			}
			npc.GetGlobalNPC<SGAnpcs>().MassiveBleeding = true;
		}

		//public override void Update(NPC npc, ref int buffIndex)
		//{
		//	npc.GetGlobalNPC<SGAWorld>(mod).eFlames = true;
		//}
	}
	public class PiercedVulnerable : MoonLightCurse
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pierced N Vulnerable");
			Description.SetDefault("Defense is reduced");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 10;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.SGANPCs().reducedDefense += 10;
		}
	}
	public class SnapFade : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SnapFade");
			Description.SetDefault("");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().Snapfading = true;
		}
	}
	public class EverlastingSuffering : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Everlasting Suffering");
			Description.SetDefault("50% increased Damage-over-time");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
		}

		public override string Texture => "SGAmod/Buffs/MoonLightCurse";

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().ELS = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().ELS = true;
		}
	}
}