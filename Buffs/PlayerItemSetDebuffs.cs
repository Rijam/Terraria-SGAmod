using Terraria;
using Terraria.ModLoader;
using SGAmod.NPCs;
using Terraria.ID;

namespace SGAmod.Buffs
{
	public class Pressured: ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pressured");
			Description.SetDefault("You've been breathing Pressurized air; removing your suit is going to deal great damage");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().Pressured=true;
		}
	}
	public class CleansedPerception : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Histoplasma");
			Description.SetDefault("The doors of perception have been cleansed!");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}

		public override string Texture => "SGAmod/Buffs/CleansedPerceptionBuff";

		public override void Update(Player player, ref int buffIndex)
		{
			//if (Main.netMode != NetmodeID.Server)
			//	Main.buffTexture[ModContent.BuffType<CleansedPerception>()] = Main.buffTexture[Main.rand.Next(Main.buffTexture.Length)];
		}
	}
	public class ShieldBreak: ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shield Break");
			Description.SetDefault("No Electric Charge/Barrier Regen\nTaking off an Energy Shield will hurt the player");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = false;
		}


		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().Shieldbreak = true;
		}
	}
	public class PlaceHolderDebuff : ModBuff
	{
		public override string Texture => "SGAmod/Buffs/BrokenImmortalityDebuff";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Place Holder Debuff");
			Description.SetDefault("Your not suppose to see this! No Seriously this debuff is NEVER meant to be active! It is Swapped out instantly to stack same-type debuffs!");
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
	public class BIPBuff : ModBuff
	{

		public override string Texture => "SGAmod/Buffs/BrokenImmortalityDebuff";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Broken Immortality");
			Description.SetDefault("You've lost your godly defense!");
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
	public class WorseWeakness : ModBuff
	{
		public override string Texture => "Terraria/Images/Buff_" + BuffID.Weak;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Consumed Weakness");
			Description.SetDefault("That potion has left you massively drained...");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = false; //true now?
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetAttackSpeed(DamageClass.Melee) -= 0.051f;
			player.statDefense -= 15;
			player.moveSpeed -= 0.2f;
			player.SGAPly().UseTimeMul -= 0.25f;
		}
	}
	public class CheekiBreekiDebuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cheeki Breeki Aiming");
			Description.SetDefault("can't aim straight to shoot straight, too much Vodka");
			Main.pvpBuff[Type] = true;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = false;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = false; //true now?
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().aimingDrunkTime = player.buffTime[buffIndex];
		}
        public override bool ReApply(Player player, int time, int buffIndex)
        {
			player.buffTime[buffIndex] += time;
			return true;
        }
    }
}
