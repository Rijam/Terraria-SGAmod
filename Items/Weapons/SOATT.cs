using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class SOATT : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword of a Thousand Truths");
			Tooltip.SetDefault("Strikes apply Truth Be Told, a stacking debuff that makes this weapon deal damage through defense\n4% is added per strike and up to 100% defense penetration can be applied\nAt max Truth Be Told, you inflict immunity-bypassing Sundered Defense\nThis causes enemies to lose many of their immunity frames to all attacks\nGotta start somewhere you know-This was IDGCaptainRussia's first modded item");
		}
		public override void SetDefaults()
		{
			Item.damage = 125;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.useStyle = 1;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = 6;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
		SGAnpcs nyx=target.GetGlobalNPC<SGAnpcs>();
		float it=nyx.truthbetold;
		nyx.truthbetold=it+0.02f;
		if (nyx.truthbetold>0.5f){nyx.truthbetold=0.5f;
		IdgNPC.AddBuffBypass(target.whoAmI, Mod.Find<ModBuff>("SunderedDefense").Type, 60 * 3);
		}
		damage=(int)(damage+(target.defense*nyx.truthbetold));
		//Idglib.Chat("Defense: "+nyx.truthbetold,244, 179, 66);
		}

	}
}
