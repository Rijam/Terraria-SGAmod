using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using Terraria.Utilities;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Items.Consumables;
using Terraria.Audio;

namespace SGAmod.NPCs
{
	public class ResonantWisp : ModNPC
	{
		int buffed = 0;
		AStarPathFinder astar;
		public List<PathNode> Path = new List<PathNode>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Resonant Wisp");
		}
		public override bool Autoload(ref string name)
		{
			return SGAmod.VibraniumUpdate;
		}
		public override void SetDefaults()
		{
			NPC.width = 48;
			NPC.height = 48;
			NPC.damage = 100;
			NPC.defense = 0;
			NPC.lifeMax = 1200;
			//npc.HitSound = SoundID.NPCHit1;
			//npc.DeathSound = SoundID.NPCDeath1;
			NPC.value = 0f;
			NPC.knockBackResist = 0.2f;
			NPC.aiStyle = -1;
			AIType = 0;
			AnimationType = 0;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.value = 1500f;
			NPC.netAlways = true;
		}

        public override string Texture => "SGAmod/Items/VibraniumCrystal";

        public override void NPCLoot()
		{
			if (buffed == 2)
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("VibraniumCrystal").Type, Main.rand.Next(3, 6));
		}

        public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life > 0)
            {
				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact, (int)NPC.Center.X, (int)NPC.Center.Y);
				if (sound != null)
				{
					sound.Pitch = 0.85f-((NPC.life/(float)NPC.lifeMax)*0.85f);
				}
            }
            else
            {
				SoundEffectInstance sound = SoundEngine.PlaySound(SoundID.Shatter, (int)NPC.Center.X, (int)NPC.Center.Y);
				if (sound != null)
				{
					sound.Pitch = 0.75f;
				}
			}

			foreach(Player player in Main.player.Where(testby => NPC.Distance(testby.MountedCenter) < damage))
            {
				float dist = NPC.Distance(player.MountedCenter);
				//float dister = (1f-MathHelper.Clamp(dist/(1200f*(float)buffed),0f,1f));
				if (dist < 200)
				{
					player.SGAPly().StackDebuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationOne").Type, (int)((60f * 3f)));
				}
            }

        }
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			projectile.SGAProj().damageReduce += (0.25f);
			projectile.SGAProj().damageReduceTime = 60;
		}

		public override void AI()
		{
			if (astar == null)
			{
				astar = new AStarPathFinder();
				astar.recursionLimit = 200;
				astar.wallsWeight = 100;
				astar.seed = NPC.whoAmI;
			}
			NPC.localAI[0] += 1;

			NPC.spriteDirection = NPC.velocity.X > 0 ? -1 : 1;
			//Main.NewText((int)astar.state + " " + npc.ai[1] + " " + npc.ai[0]+" "+ Path.Count);

			Player P = Main.player[NPC.target];
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}
			else
			{
				if (buffed == 0)
				{
					bool bigtool = Main.player.Where(testby => testby.active && !testby.dead && testby.inventory.Where(testby2 => !testby2.IsAir && testby2.pick > 230).Count()>0).Count()>0;
					buffed = bigtool || Dimensions.SpaceDim.postMoonLord ? 2 : 1;
					if (buffed == 2)
					{
						NPC.life *= 5;
						NPC.lifeMax *= 5;
						NPC.knockBackResist = 0f;
						NPC.value *= 4;
					}
				}

				if (NPC.localAI[0] % 60 == 0)
				{
					if (Collision.CanHitLine(P.MountedCenter, 1, 1, NPC.Center, 1, 1))
					{
						Vector2 dist = P.Center- NPC.Center;

						if (dist.Length() < 200+(buffed * 200))
						{
							Projectile.NewProjectile(NPC.Center, dist, ModContent.ProjectileType<Items.Armors.Vibranium.VibraniumZapEffect>(), 0, 0);
							SoundEffectInstance snd = SoundEngine.PlaySound(SoundID.Item, (int)NPC.Center.X, (int)NPC.Center.Y, 91);
							P.AddBuff(BuffID.WitheredArmor, 60 * 5);
							P.AddBuff(BuffID.WitheredWeapon, 60 * 5);
							P.SGAPly().StackDebuff(ModLoader.GetMod("IDGLibrary").GetBuff("Radiation" + (buffed == 2 ? "Two" : "One")).Type, 60*6);
						}
					}
				}

				//Lets just assume netAlways can handle locations please?
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//Time for the fancy AStar Nonsense!
					if (astar.state != (int)PathState.Calculating)
					{
						NPC.ai[1] = 0;
						if (astar.state == (int)PathState.Finished || astar.state == (int)PathState.Failed)
						{
							//Gotta reverse it, so we don't start with the destination
							Path = new List<PathNode>(astar.Path);
							Path.Reverse();

							astar.state = (int)PathState.Ready;
						}
						NPC.ai[0] += (Path.Count<2 && NPC.ai[0]<190) ? 3 : 1;
						if (NPC.ai[0] > 200)
						{
							astar.startingPosition = new Point16((int)NPC.Center.X / 16, (int)NPC.Center.Y / 16);
							if (astar.AStarTiles(new Point16((int)P.Center.X / 16, (int)P.Center.Y / 16), 1))
                            {
								NPC.ai[0] = Main.rand.Next(-50, 50);
								NPC.netUpdate = true;
							}
						}

					}
					else
					{
						NPC.ai[1] += 1;

						if (NPC.ai[1] == 250)
                        {
							//make portal thing
							for (int i = 160; i < 320; i += 8)
                            {
								Vector2 checkhere = P.MountedCenter+(Vector2.UnitX.RotatedBy(Main.rand.NextFloat(0f,MathHelper.TwoPi))*i);
								if (Collision.CanHitLine(P.MountedCenter, 1, 1, checkhere, 1, 1))
								{
									NPC.Center = checkhere;
									NPC.netUpdate = true;
								}
							}
                        }

						if (NPC.ai[1] > 300 && Path.Count < 5)
						{
							astar.state = (int)PathState.Ready;
						}

					}
				}

			}

			//if (astar.state != (int)PathState.Calculating)
			//{
				if (Path.Count > 0)
				{
					if (NPC.localAI[0] % 1 == 0)
					{
						Vector2 gothere = Path[0].location.ToVector2() * 16;

						if (NPC.Distance(gothere) > 16)
						{
							NPC.velocity = Vector2.Normalize(gothere - NPC.Center) * (buffed==2 ? 16 : 4);
						}
						else
						{
							Path.RemoveAt(0);
						}
					}
				}
				else
				{

				}
			//}

			NPC.velocity *= (buffed == 2 ? 0.72f : 0.92f);

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			DrawResonantWisp(NPC,NPC.Center,NPC.localAI[0], spriteBatch, lightColor);
			return false;
		}

		public static void DrawResonantWisp(Entity id,Vector2 drawWhere,float timer,SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.npcTexture[ModContent.NPCType<ResonantWisp>()];
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

			UnifiedRandom rando = new UnifiedRandom(id.whoAmI);

			List<Vector3> drawThere = new List<Vector3>();

			Texture2D star = Main.itemTexture[ModContent.ItemType<Items.StygianCore>()];

			for (int i = 8; i < 48;i+=2) 
			{
				Vector3 offset = new Vector3(i, 0,0);

				float mulEffect = timer / 2f;

				Matrix matrix = Matrix.CreateRotationZ((rando.NextFloat(-0.25f, 0.25f) * mulEffect) + rando.NextFloat(MathHelper.TwoPi)) *
					Matrix.CreateRotationY((rando.NextFloat(-0.25f, 0.25f) * mulEffect) + rando.NextFloat(MathHelper.TwoPi)) *
					Matrix.CreateRotationX((rando.NextFloat(-0.25f, 0.25f) * mulEffect) + rando.NextFloat(MathHelper.TwoPi));

				offset = Vector3.Transform(offset, matrix);

				drawThere.Add(offset); 

			}

			//List<Vector2> drawThereCopy = new List<Vector2>(drawThere);

			spriteBatch.Draw(star, drawWhere - Main.screenPosition, null, Color.Blue*0.25f, 0, star.Size() / 2f, 0.95f, SpriteEffects.None, 0f);
			spriteBatch.Draw(star, drawWhere - Main.screenPosition, null, Color.Red*0.75f, 0, star.Size() / 2f, 0.35f, SpriteEffects.None, 0f);

			foreach (Vector3 position in drawThere.OrderBy(testnpc => 100000-testnpc.LengthSquared()))
			{
				Vector3 posa = Vector3.Normalize(position);
				if (posa.Z > 0)
				{
					float scaler = 0.75f+(posa.Z * 0.25f);
					spriteBatch.Draw(texture, drawWhere + new Vector2(position.X, position.Y) - Main.screenPosition, null, lightColor * posa.Z, rando.NextFloat(MathHelper.TwoPi), origin, new Vector2(scaler, scaler), SpriteEffects.None, 0f);
				}
			}


		}



	}
}
