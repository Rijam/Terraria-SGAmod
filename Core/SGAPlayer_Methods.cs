using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Idglibrary;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Shaders;
using SGAmod.NPCs;
using SGAmod.NPCs.Wraiths;
using SGAmod.NPCs.Cratrosity;
using SGAmod.NPCs.Murk;
using SGAmod.NPCs.Sharkvern;
using SGAmod.NPCs.SpiderQueen;
using SGAmod.NPCs.Hellion;
//using CalamityMod;

using Terraria.Utilities;
using SGAmod.SkillTree;
//using CalamityMod.Projectiles.Ranged;
using SGAmod.Buffs;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Items.Weapons.Shields;
using Terraria.Audio;

namespace SGAmod
{
	public class ActionCooldownStack
	{
		public int timeleft;
		public int timerup;
		public int maxtime;
		public int index;

		public ActionCooldownStack(int timeleft, int index)
		{
			this.timeleft = timeleft;
			this.maxtime = timeleft;
			this.index = index;
			timerup = 0;
		}
	}

	public partial class SGAPlayer : ModPlayer
	{

		public delegate void FirstHurtDelegate(SGAPlayer player, PlayerDeathReason damageSource, ref int Damage, ref int hitDirection, bool pvp, bool quiet, ref bool Crit, int cooldownCounter);
		public static event FirstHurtDelegate FirstHurtEvent;

		public static void DoHurt(SGAPlayer player, PlayerDeathReason damageSource, ref int Damage, ref int hitDirection, bool pvp, bool quiet, ref bool Crit, int cooldownCounter)
        {
			if (FirstHurtEvent != null)
				FirstHurtEvent.Invoke(player, damageSource, ref Damage, ref hitDirection, pvp, quiet, ref Crit, cooldownCounter);
        }

		public bool CalamityAbyss
		{
			get
			{
				/*Player player2 = (this as ModPlayer).player;
				if (ModLoader.GetMod("CalamityMod") != null)
				{
					CalamityPlayer CPlayer = player2.GetModPlayer(ModLoader.GetMod("CalamityMod"), "CalamityPlayer") as CalamityPlayer;
					Type CType = CPlayer.GetType();
					PropertyInfo CProperty = CType.GetProperty("ZoneAbyss");

					if (CProperty != null)
					{
						return !CPlayer.ZoneAbyss;
					}
				}*/
				return false;

			}
		}

		public void ToogleSatanicMode(bool on)
        {
			if (_satanplayer == false && on == true)
            {
				if (SGAmod.LocalPlayerPlayTime<60)
				SGASatanicGlobalItem.DoCheatVisualEffect(Main.LocalPlayer,false);
			}
				_satanplayer = on;
        }

		public void AddEntropy(int ammount)
		{
			entropyCollected += ammount;
			while (entropyCollected > Items.EntropyTransmuter.MaxEntropy)
			{
				entropyCollected -= Items.EntropyTransmuter.MaxEntropy;
				for (int fgf = 0; fgf < 20; fgf += 1)
				{
					int type = -1;
					if (Player.HasItem(ItemID.CrimtaneOre))
						type = ItemID.CrimtaneOre;
					if (Player.HasItem(ItemID.DemoniteOre))
						type = ItemID.DemoniteOre;
					if (type > -1)
					{
						Player.ConsumeItem(type);
						Player.QuickSpawnItem(Player.GetSource_OpenItem(type), Mod.Find<ModItem>("Entrophite").Type);
					}
				}
			}
		}

		public void ShieldRecharge()
		{

		}

		public void StartShieldRecharge()
		{
			SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Zombie, (int)Player.Center.X, (int)Player.Center.Y, 71);
			if (sound != null)
				sound.Pitch += 0.5f;
		}

		public void ShieldDepleted()
		{
			CauseShieldBreak(60 * 7);
		}

		(int, int) shieldAmmounts = (5, 7);

		public bool TakeShieldHit(ref int damage)
		{

			int takenshielddamage = (int)(jellybruSet ? damage * Math.Max(Player.manaCost, 0.10f) : damage);

			if (GetEnergyShieldAmmountAndRecharge.Item1 > takenshielddamage)
			{

				if (!Player.immune)
				{
					SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Item, (int)Player.Center.X, (int)Player.Center.Y, 93);
					if (sound != null)
						sound.Pitch = MathHelper.Clamp(-0.8f + ((GetEnergyShieldAmmountAndRecharge.Item1 / (float)GetEnergyShieldAmmountAndRecharge.Item2) * 1.60f), -0.75f, 0.80f);

					energyShieldAmmountAndRecharge.Item3 = 60 * (tpdcpu ? shieldAmmounts.Item1 : shieldAmmounts.Item2);
					energyShieldAmmountAndRecharge.Item1 -= takenshielddamage;

					//Main.NewText(takenshielddamage);

					Player.immune = true;
					Player.immuneTime = 20;
				}

				return true;
			}
			damage -= GetEnergyShieldAmmountAndRecharge.Item1;

			if (GetEnergyShieldAmmountAndRecharge.Item1 > 0)
			{
				ShieldDepleted();
				energyShieldAmmountAndRecharge.Item1 = 0;
			}
			return false;
		}

		public void StackDebuff(int type, int time)
		{
			Player.AddBuff(ModContent.BuffType<PlaceHolderDebuff>(), time);
			if (Player.FindBuffIndex(ModContent.BuffType<PlaceHolderDebuff>()) >= 0)
			{
				Player.buffType[Player.FindBuffIndex(ModContent.BuffType<PlaceHolderDebuff>())] = type;
			}
		}

		public float GetMinionSlots
		{
			get
			{
				float ammount = 0f;
				for (int num27 = 0; num27 < Main.maxProjectiles; num27 += 1)
				{
					if (Main.projectile[num27].active && Main.projectile[num27].owner == Player.whoAmI && Main.projectile[num27].minion && ProjectileID.Sets.MinionSacrificable[Main.projectile[num27].type])
						ammount += Main.projectile[num27].minionSlots;
				}
				return ammount;
			}
		}

		public void RestoreBreath(int ammount, bool texteffect = true)
		{
			SGAPlayer sgaplayer = Player.GetModPlayer<SGAPlayer>();
			SoundEngine.PlaySound(SoundID.Drown, (int)Player.Center.X, (int)Player.Center.Y, 0, 1f, 0.50f);
			Player.breath = (int)MathHelper.Clamp(Player.breath + ammount, 0, Player.breathMax);
			sgaplayer.sufficate = Player.breath;
			if (texteffect)
				CombatText.NewText(Player.Hitbox, Color.Aqua, "+" + (ammount / 20) + " bubbles");

		}

		public bool AddCooldownStack(int time, int count = 1, bool testOnly = false)
		{
			bool weHaveStacks = CooldownStacks.Count + (count - 1) < MaxCooldownStacks;

			bool worked = false;

			for (int i = 0; i < count; i += 1)
			{

				bool illuSet = illuminantSet.Item1 > 4 && weHaveStacks && !testOnly && Main.rand.Next(4) == 0;

				if (illuSet)
				{
					worked = true;
					continue;
				}
				if (weHaveStacks)
				{
					//if (player.HasBuff(mod.BuffType("CondenserBuff")))
					//	time = (int)((float)time * 1.15f);

					if (!testOnly)
					{
						int time2 = (int)((float)time * ActionCooldownRate);

						CooldownStacks.Add(new ActionCooldownStack(time2, CooldownStacks.Count));
					}
					worked = true;
				}
			}
			return worked && weHaveStacks;

		}

		public void AddElectricCharge(int ammount)
		{
			electricCharge += ammount;
		}

		public void CauseShieldBreak(int time)
		{
			if (!tpdcpu)
			{
				Player.AddBuff(ModContent.BuffType<ShieldBreak>(), time);
				CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height), Color.Aquamarine, "Shield Break!", true, false);
				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.NPCHit, (int)Player.Center.X, (int)Player.Center.Y, 53);
				if (sound != null)
					sound.Pitch -= 0.5f;

				for (int i = 0; i < 20; i += 1)
				{
					int dust = Dust.NewDust(new Vector2(Player.Center.X - 4, Player.Center.Y - 8), 8, 16, 269);
					Main.dust[dust].scale = 0.50f;
					Main.dust[dust].noGravity = false;
					Main.dust[dust].velocity = Main.rand.NextVector2Circular(6f, 6f);
				}
			}
		}

		public bool ConsumeElectricCharge(int requiredcharge, int delay, bool damage = false, bool consume = true)
		{
			int newcharge = (int)Math.Max(requiredcharge * electricChargeCost, 1);
			if (electricCharge > newcharge)
			{
				if (consume)
				{
					electricdelay = Math.Max(delay * electricChargeReducedDelay, electricdelay);
					electricCharge -= newcharge;
				}
				return true;
			}
			else
			{
				if (damage && ShieldType > 0 && electricCharge > 0)
				{
					electricCharge = 0;
					electricdelay = 30;
					CauseShieldBreak(60 * 5);
				}
			}

			return false;
		}

		public bool HandleFluidDisplacer(int tier)
		{
			if (tidalCharm < 0 && ConsumeElectricCharge(tier, 60 * tier, true))
				return true;


			return false;


		}
		public bool ConsumeAmmoClip(bool doConsume = true, int ammoCheck = 1)
		{
			if (ammoLeftInClip >= ammoCheck)
			{
				if (doConsume)
					ammoLeftInClip -= ammoCheck;

				return true;
			}
			return false;

		}
		public void StackAttack(ref int damage, Projectile proj)
		{

			SGAPlayer sgaplayer = Player.GetModPlayer<SGAPlayer>();
			if (Player.HeldItem != null)
			{
				if (sgaplayer.IDGset && Player.HeldItem.DamageType == DamageClass.Ranged)
				{

					int bonusattacks = (int)(((float)sgaplayer.digiStacks / (float)sgaplayer.digiStacksMax) * (float)sgaplayer.digiStacksCount);

					int ammotype = (int)Player.GetModPlayer<SGAPlayer>().myammo;
					if (ammotype > 0 && bonusattacks > 0 && proj.damage > 100)
					{
						Item ammo2 = new Item();
						ammo2.SetDefaults(ammotype);
						int ammo = ammo2.shoot;
						int damageproj = proj.damage;
						float knockbackproj = proj.knockBack;
						float sppez = proj.velocity.Length();
						if (ammo2.ModItem != null)
							ammo2.ModItem.PickAmmo(Player.HeldItem, Player, ref ammo, ref sppez, ref proj.damage, ref proj.knockBack);
						int type = ammo;

						if (proj.type == Mod.Find<ModProjectile>("SoldierRocketLauncherProj").Type)
							type = Mod.Find<ModProjectile>("SoldierRocketLauncherProj").Type;


						for (int i = 0; i < bonusattacks; i += 1)
						{
							int subtracter = sgaplayer.digiStacks -= damage;
							if (i > -1)
							{
								float angle = MathHelper.ToRadians(sgaplayer.timer + ((((float)i - 1) / (float)bonusattacks) * 360f));
								Vector2 apos = Player.Center + new Vector2((float)Math.Cos(angle) * 64, (float)Math.Sin(angle) * 12);
								int probg = Projectile.NewProjectile(null, new Vector2(apos.X, apos.Y), proj.velocity, type, (int)(damage * 0.50f), 0f, Player.whoAmI);
								Main.projectile[probg].GetGlobalProjectile<SGAprojectile>().stackedattack = true;
							}
							else
							{
								if (subtracter > 0)
								{
									//float thedamage = (sgaplayer.digiStacks / sgaplayer.digiStacksMax);
									//damage = damage + (int)thedamage;
								}
							}
							if (subtracter > 0)
							{
								sgaplayer.digiStacks = subtracter;
							}
						}
					}
				}

			}

		}

		public int IsRevolver(Item item)
		{
			if (item == null || item.IsAir)
				return 0;
			if (SGAmod.UsesClips.ContainsKey(item.type))
			{
				if (item.type == Mod.Find<ModItem>("SoldierRocketLauncher").Type)
					return 2;
				return 1;
			}
			return 0;
		}
		public bool RefilPlasma(bool checkagain = false)
		{
			if (plasmaLeftInClip > 0)
			{
				return true;
			}

			if (plasmaLeftInClip < 1 || checkagain)
			{

				if (Player.HasItem(Mod.Find<ModItem>("PlasmaCell").Type))
				{
					Player.ConsumeItem(Mod.Find<ModItem>("PlasmaCell").Type);
					plasmaLeftInClip = plasmaLeftInClipMax;//Math.Min(plasmaLeftInClip + 1000, plasmaLeftInClipMax);
					CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height), Color.LawnGreen, "Plasma Recharged!", false, false);
					if (plasmaLeftInClip < plasmaLeftInClipMax && checkagain)
					{
						RefilPlasma(true);

					}
					return true;
				}
			}
			return false;
		}

		public static void DrunkAiming()
        {
			if (Main.gameMenu)
				return;

			SGAPlayer sgaply = Main.LocalPlayer.SGAPly();
			if (sgaply.aimingDrunkTime > 0)
			{
				float timeLeft = (sgaply.aimingDrunkTime / 30f);
				int val1 = (int)(Math.Cos(sgaply.aimingDrunkTime / 40f) * timeLeft);
				int val2 = (int)(Math.Sin(sgaply.aimingDrunkTime / 46f) * timeLeft);

				Main.mouseX += val1;
				Main.mouseY += val2;
				Main.lastMouseX = Main.mouseX;
				Main.lastMouseY = Main.mouseY;
			}
		}

		public static void LimitProjectiles(Player player, int maxprojs, int[] types)
		{

			int projcount = 0;
			for (int a = 0; a < types.Length; a++)
			{
				projcount += player.ownedProjectileCounts[(int)types[a]];
			}

			Projectile removethisone = null;
			int timecheck = 99999999;

			if (projcount > maxprojs)
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile him = Main.projectile[i];
					if (types.Any(x => x == Main.projectile[i].type))
					{
						if (him.active && him.owner == player.whoAmI && him.timeLeft < timecheck)
						{
							removethisone = him;
							timecheck = him.timeLeft;
						}
					}
				}
				if (removethisone != null)
				{
					removethisone.Kill();
				}
			}

		}

		public bool HasGucciGauntlet()
		{
			Item minecart = Player.miscEquips[4];
			if (!minecart.IsAir)
			{
				if (minecart.ModItem != null)
				{
					if (minecart.type == Mod.Find<ModItem>("GucciGauntlet").Type)
					{
						return true;
					}
				}
			}
			return false;
		}

		public void AddCritDamage(float amt)
        {
			_critDamage += amt;
		}

		public void LookForThrowingGear(ref int type)
		{
			int[] typesToCheck = { ItemID.Shuriken, ItemID.ThrowingKnife, ItemID.PoisonedKnife, ItemID.FrostDaggerfish, ItemID.StarAnise, ItemID.BoneDagger };

			for (int i = 0; i < Player.inventory.Length; i += 1)
			{
				Item item = Player.inventory[i];

				var counter = typesToCheck.Where(testby => testby == item.type);

				if (counter.Count() > 0)
				{
					type = counter.ToArray()[0];
					break;
				}
			}
		}

		public void UpgradeTF2()
		{
			if (!gottf2 && Player == Main.LocalPlayer)
			{
				Main.NewText("You have received your TF2 Emblem!", 150, 150, 150);
				Player.QuickSpawnItem(null, Mod.Find<ModItem>("TF2Emblem").Type, 1);
				gottf2 = true;
			}
		}

		public int ActionStackOverflow(ActionCooldownStack stack, int stackIndex)
		{
			if (MaxCooldownStacks <= stackIndex && stack.timerup % 2 == 0)
				return 0;

			return 1;
		}

		public void DoCooldownUpdate()
		{
			if (Main.netMode != NetmodeID.Server)//ugh
			{

				if (!noCooldownRate && CooldownStacks.Count > 0)
				{
					for (int stackindex = 0; stackindex < CooldownStacks.Count; stackindex += 1)
					{
						ActionCooldownStack stack = CooldownStacks[stackindex];
						stack.timeleft -= ActionStackOverflow(stack, stackindex);
						stack.timerup += 1;
						if (stack.timeleft < 1)
							CooldownStacks.RemoveAt(stackindex);
					}
				}
				activestacks = CooldownStacks.Count;
			}

		}

		private bool ShieldJustBlock(int blocktime, Projectile shield, Vector2 where, ref int damage, int damageSourceIndex)
		{
			if (blocktime < 30 && AddCooldownStack(60 * 3))
			{
				for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.Pi / 10f)
				{
					Vector2 randomcircle = Main.rand.NextVector2CircularEdge(2f, 4f).RotatedBy(shield.velocity.ToRotation());
					int dust = Dust.NewDust(shield.Center, 0, 0, 27);
					Main.dust[dust].scale = 1.5f;
					Main.dust[dust].velocity = randomcircle * 3f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].shader = GameShaders.Armor.GetShaderFromItemId(ItemID.AcidDye);
				}
				Player.GetModPlayer<SGAPlayer>().realIFrames = 30;
				SoundEngine.PlaySound(3, (int)Player.position.X, (int)Player.position.Y, 4, 1f, -0.5f);

				(shield.ModProjectile as Items.Weapons.Shields.CorrodedShieldProj).JustBlock(blocktime, where, ref damage, damageSourceIndex);

				if (enchantedShieldPolish)
				{
					Player.statMana = Math.Min(Player.statMana + damage, Player.statManaMax2);
					Player.ManaEffect(damage);
				}

				if (damageSourceIndex > 0)
				{
					if (rustedBulwark)
					{
						RustBurn.ApplyRust(Main.npc[damageSourceIndex - 1], (2 + damage) * 20);
					}

					if (diesIraeStone)
					{
						float empty = 5f;
						bool emptyCrit = true;
						Main.npc[damageSourceIndex - 1].SGANPCs().DoApoco(Main.npc[damageSourceIndex - 1], shield, Player, null, ref damage, ref empty, ref emptyCrit, 4, true);
					}
				}

				return true;
			}
			return false;

		}

		protected bool ShieldDamageCheck(Vector2 where, ref int damage, int damageSourceIndex)
		{
			Vector2 itavect = where - Player.Center;
			itavect.Normalize();


			if (Player.SGAPly().heldShield >= 0 && Player.ownedProjectileCounts[Mod.Find<ModProjectile>("CapShieldToss").Type] < 1)
			{
				int heldShield = Player.SGAPly().heldShield;

				//if (SGAPlayer.ShieldTypes.ContainsKey(player.HeldItem.type))
				//{
				int foundhim = -1;

				int xxxz = 0;
				int thetype;
				/*SGAPlayer.ShieldTypes.TryGetValue(player.HeldItem.type, out thetype);
				Projectile proj = null;
				for (xxxz = 0; xxxz < Main.maxProjectiles; xxxz++)
				{
					if (Main.projectile[xxxz].active && Main.projectile[xxxz].type == thetype && Main.projectile[xxxz].owner == player.whoAmI)
					{
						foundhim = xxxz;
						proj = Main.projectile[xxxz];
						break;
					}
				}*/
				Projectile proj = Main.projectile[heldShield];
				if (proj.active)
				{
					foreach (Projectile proj2 in Main.projectile.Where(testby => testby.ModProjectile != null && testby.ModProjectile is CorrodedShieldProj))
					{
						proj = proj2;
						foundhim = heldShield;

						if (foundhim > -1)
						{
							CorrodedShieldProj modShieldProj = proj.ModProjectile as CorrodedShieldProj;
							if (modShieldProj == null)
								return false;
							int blocktime = modShieldProj.Blocktimer;
							bool blocking = modShieldProj.Blocking;

							if (proj == null || blocktime < 2 || !blocking)
								continue;// return false;



							Vector2 itavect2 = Main.projectile[foundhim].Center - Player.Center;
							itavect2.Normalize();
							Vector2 ang1 = Vector2.Normalize(proj.velocity);
							float diff = Vector2.Dot(itavect, ang1);


							if (diff > (proj.ModProjectile as CorrodedShieldProj).BlockAnglePublic - Player.SGAPly().shieldBlockAngle)
							{
								if (ShieldJustBlock(blocktime, proj, where, ref damage, damageSourceIndex))
									return true;

								float damageval = 1f - modShieldProj.BlockDamagePublic;
								damage = (int)(damage * damageval);

								SoundEngine.PlaySound(3, (int)Player.position.X, (int)Player.position.Y, 4, 0.6f, 0.5f);

								if (!NoHitCharm && !(proj.ModProjectile as CorrodedShieldProj).HandleBlock(ref damage, Player))
									return true;

								continue;// return false;
							}
						}
					}
				}

			}
			return false;
		}

		public bool ChainBolt()
		{
			WeightedRandom<int> rando = new WeightedRandom<int>();

			if (ConsumeElectricCharge(750, 120))
			{

				for (int i = 0; i < Main.maxNPCs; i += 1)
				{
					if (Main.npc[i].active && !Main.npc[i].townNPC && !Main.npc[i].friendly)
					{
						if (Main.npc[i].CanBeChasedBy() && !Main.npc[i].dontTakeDamage)
						{
							float dist = Main.npc[i].Distance(Player.Center);
							if (dist < 250)
							{
								rando.Add(i, 250.00 - (double)dist);
							}
						}
					}
				}

				if (rando.elements.Count > 0)
				{
					NPC luckyguy = Main.npc[rando.Get()];

					Vector2 Speed = (luckyguy.Center - Player.Center);
					Speed.Normalize(); Speed *= 2f;
					int prog = Projectile.NewProjectile(null, Player.Center.X, Player.Center.Y, Speed.X, Speed.Y, Mod.Find<ModProjectile>("CBreakerBolt").Type, 30 + ((int)((float)(Player.statDefense * techdamage))), 3f, Player.whoAmI, 3);
					IdgProjectile.Sync(prog);
					SoundEngine.PlaySound(SoundID.Item93, Player.Center);
					return true;
				}
			}



			return false;
		}

		public bool DashBlink()
		{
			if (noModTeleport || maxblink < 1 || (Player.mount != null && Player.mount.Active))
				return false;



			int previousdash = Player.dash;
			Player.dash = 1;

			if (Math.Abs(Player.dashTime) > 0 && Player.dashDelay < 1 && Player.dash > 0 && (Player.controlLeft || Player.controlRight))
			{
				int bufftime = 0;
				if (Player.HasBuff(BuffID.ChaosState))
					bufftime = Player.buffTime[Player.FindBuffIndex(BuffID.ChaosState)];


				if (bufftime < maxblink && (Player.controlUp))
				{
					Player.Teleport(Player.Center + new Vector2(Player.dashTime > 0 ? -8 : 0, -20), 1);
					for (int i = 0; i < 30; i += 1)
					{
						if (Collision.CanHitLine(Player.Center, 16, 16, Player.Center + new Vector2(Math.Sign(Player.dashTime) * 8, 0), 16, 16))
						{
							Player.Center += new Vector2(Math.Sign(Player.dashTime) * 8, 0);

						}
						else
						{
							Player.Center -= new Vector2(Math.Sign(Player.dashTime) * 16, 0);
							break;
						}
					}
					Player.Teleport(Player.Center + new Vector2(Player.dashTime > 0 ? -8 : 0, -20), 1);
					Player.dashTime = 0;
					Player.dashDelay = 5;
					Player.AddBuff(BuffID.ChaosState, bufftime + 120);

					return true;
				}

				Player.dash = previousdash;

			}
			return false;
		}

		public void OnLifeRegen()
        {
			/*
			int npfgf;
			Assist.SpawnOnPlayerButNoTextAndReturnValue(player.whoAmI,NPCID.TaxCollector,out npfgf);
			Main.npc[npfgf].aiStyle = 69;
			Main.npc[npfgf].friendly = false;
			Main.npc[npfgf].damage = 100;
			Main.npc[npfgf].defDamage = 100;
			*/


			//player.Hurt(PlayerDeathReason.ByCustomReason(player.name + " clear sucks"), 1, 1,true,false,true,2);
			//player.statMana += 3;
			//Main.NewText("test this");
		}

		public void ShuffleYourFeetElectricCharge()
		{
			if (Noviteset > 0 && electricChargeMax > 0)
			{
				if (Math.Abs(Player.velocity.X) > 4f && Player.velocity.Y == 0.0 && !Player.mount.Active && !Player.mount.Cart)
				{
					electricrechargerate += 10;
				}
			}
		}

		public void CharmingAmuletCode()
		{
			if (EnhancingCharm > 0 || Player.manaRegenBuff)
			{
				for (int g = 0; g < Player.MaxBuffs; g += 1)
				{
					if (Player.manaRegenBuff && (SGAConfig.Instance.ManaPotionChange || SGAmod.DRMMode))
					{
						if (Player.buffType[g] == BuffID.ManaSickness && Player.buffTime[g] > 3)
						{
							if (timer % 4 > 0)
								Player.buffTime[g] += 1;
						}
					}
					if (EnhancingCharm > 0)
					{
						if (potionsicknessincreaser > 0)
						{
							if (Player.buffType[g] == BuffID.PotionSickness && Player.buffTime[g] > 10)
							{
								if (timer % potionsicknessincreaser == 0)
									Player.buffTime[g] += 1;
							}
						}
					}
				}
				if (EnhancingCharm > 0)
				{
					if (timer % (EnhancingCharm) == 0)
					{
						//longerExpertDebuff
						for (int i = 0; i < Player.MaxBuffs; i += 1)
						{
							if (!Player.BlackListedBuffs(i))
							{
								ModBuff buff = ModContent.GetModBuff(Player.buffType[i]);
								bool isdebuff = Main.debuff[Player.buffType[i]];
								if (Player.buffTime[i] > 10 && ((buff != null && ((isdebuff) || !isdebuff)) || buff == null))
								{
									Player.buffTime[i] += isdebuff ? -1 : 1;
								}
							}
						}
					}
				}
			}

			if (anticipationLevel > 0)
			{
				if (IdgNPC.bossAlive && !Player.HasBuff(Mod.Find<ModBuff>("BossHealingCooldown").Type) && anticipation < 20 * anticipationLevel)
				{
					anticipation = 100;
					CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height), Color.Green, "Anticipated!", false, false);
					CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y - 48, Player.width, Player.height), Color.Green, "+" + anticipationLevel * 100 + "!", false, false);
					Player.AddBuff(Mod.Find<ModBuff>("BossHealingCooldown").Type, 120 * 60);
					Player.netLife = true;
					Player.statLife += anticipationLevel * 100;
				}
				Item helditem = Player.HeldItem;
				if (!Player.HeldItem.IsAir)
				{
					if (helditem.DamageType == DamageClass.Throwing)
						Player.GetDamage(DamageClass.Throwing) += (float)(anticipation / 3000f);
					if (helditem.DamageType == DamageClass.Magic)
						Player.GetDamage(DamageClass.Magic) += (float)(anticipation / 3000f);
					if (helditem.DamageType == DamageClass.Summon)
						Player.GetDamage(DamageClass.Summon) += (float)(anticipation / 3000f);
					if (helditem.DamageType == DamageClass.Ranged)
						Player.GetDamage(DamageClass.Ranged) += (float)(anticipation / 3000f);
					if (helditem.DamageType == DamageClass.Melee)
						Player.GetDamage(DamageClass.Melee) += (float)(anticipation / 3000f);
				}
			}

			int adderlevel = Math.Max(-1, (int)Math.Pow(anticipationLevel, 0.75));
			int[] ammounts = { 0, 150, 400, 900 };
			int adder2;
			if (anticipationLevel > -1)
				adder2 = ammounts[anticipationLevel];
			else
				adder2 = -1;

			anticipation = (int)MathHelper.Clamp(anticipation + (IdgNPC.bossAlive ? (adderlevel) : -1), 0, (100 + (adder2)) * 3);
		}

		public void FlaskEffects(Rectangle rect, Vector2 speed)
		{
			if (flaskBuff != default)
			{
				flaskBuff.FlaskEffect(rect, speed);
			}
		}

		internal bool Sequence => !Main.dedServ && ((Player.controlHook && Player.controlUp && Player.controlInv && Main.MouseScreen.X > Main.screenWidth - 128 && Main.MouseScreen.Y > Main.screenHeight - 128) || ModLoader.GetMod("HeavensMechanic") != null);

		public static void DoPotionFatigue(SGAPlayer sgaply)
		{
			if (!SGAConfig.Instance.PotionFatigue && !sgaply.nightmareplayer && !SGAmod.DRMMode)
			{
				sgaply.potionFatigue = Math.Max(sgaply.potionFatigue - 20, 0);
				return;
			}

			Player player = sgaply.Player;

			int count = 0;
			bool noPotions = true;
			List<int> badBuffSlots = new List<int>();

			for (int bufftype = 0; bufftype < player.buffTime.Length; bufftype += 1)
			{
				if (player.buffTime[bufftype] > 0 && !Main.buffNoTimeDisplay[player.buffType[bufftype]])
				{
					int found = SGAmod.BuffsThatHavePotions.Where(testby => testby == player.buffType[bufftype]).Count();

					if (SGAmod.BuffsThatHavePotions.Where(testby => testby == player.buffType[bufftype]).Count() > 0)
					{
						count += 1;
						badBuffSlots.Add(bufftype);
						noPotions = false;
					}
				}
			}

			int countEm = SGAmod.TotalCheating ? (int)(8 - (SGAmod.PlayingPercent * 6)) : 8;

			if (count <= countEm)
			{
				sgaply.potionFatigue = Math.Max(sgaply.potionFatigue - 20, 0);
				return;
			}

			sgaply.potionFatigue += (count - countEm) * 1;

			int fatigue = (int)sgaply.potionFatigue;

			if (fatigue > 10000)
			{
				if (Main.rand.Next(1000000) < fatigue / 1)
				{
					if (badBuffSlots.Count > 0)
					{
						int[] badBuffs = { ModContent.BuffType<PiercedVulnerable>(), ModContent.BuffType<PoisonStack>(), ModContent.BuffType<Gourged>(), ModContent.BuffType<WorseWeakness>(), ModContent.BuffType<MiningFatigue>(), ModContent.BuffType<Sunburn>(), ModContent.BuffType<MurkyDepths>(), BuffID.Rabies, ModContent.BuffType<TechnoCurse>(), ModContent.BuffType<EverlastingSuffering>() };

						badBuffSlots = badBuffSlots.OrderBy(testby => player.buffTime[testby]).ToList();
						player.buffType[badBuffSlots[0]] = badBuffs[Main.rand.Next(badBuffs.Length)];

						sgaply.potionFatigue -= SGAmod.TotalCheating ? 2000 : 5000;

						var snd = SoundEngine.PlaySound(SoundID.Zombie, (int)player.Center.X, (int)player.Center.Y, 31);
						if (snd != null)
						{
							snd.Pitch = 0.75f;
						}
						if (SGAmod.TotalCheating)
                        {
							sgaply.disabledAccessories = Math.Max(sgaply.disabledAccessories, (int)SGAmod.PlayingPercent * 300);

						}


					}
				}
			}
		}
	}

}