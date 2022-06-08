using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;
using Terraria.DataStructures;

namespace SGAmod
{
	//DO NOT EDIT THESE CLASSES DIRECTLY! EDIT THE ONES IN LOOTBOXEXAMPLE AND COPY CODE TO OVERRIDE FROM HERE IF YOU WANT
	//Lootboxes! By IDGCaptainRussia94, originally made specifically for Daimgamer
	//Rawr <3
	public class LootBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Loot Box!");
			Tooltip.SetDefault("'Totally banned in Norway'\nAlso, your not suppose to have this!");
		}

		public override bool IsLoadingEnabled(Mod mod)
		{
			return !(GetType()==typeof(LootBox));
		}

		public override bool CanUseItem(Player player)
		{
			bool canopen = true;
			for(int i = 0; i < Main.maxProjectiles; i += 1)
			{
				if (Main.projectile[i].ModProjectile != null)
				{
					Type boxtype = Main.projectile[i].ModProjectile.GetType();
					if (Main.projectile[i].owner == player.whoAmI && Main.projectile[i].timeLeft>0 && (boxtype == typeof(LootBoxOpen) || boxtype.BaseType == typeof(LootBoxOpen) || boxtype.IsSubclassOf(typeof(LootBoxOpen))))
					{
						canopen = false;
						break;
					}
				}
			}
			return canopen;
		}

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.maxStack = 30;
			Item.rare = 8;
			Item.value = 1000;
			Item.useStyle = 2;
			Item.useAnimation = 17;
			Item.useTime = 17;
			Item.useTurn = true;
			Item.UseSound = SoundID.Item3;
			Item.consumable = true;
			Item.shoot = Mod.Find<ModProjectile>("LootBoxOpen").Type;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (Main.netMode == NetmodeID.Server)
				return false;
			Projectile.NewProjectile(null, player.Center.X, player.Center.Y, Vector2.Zero.X, Vector2.Zero.Y, type, damage, knockback, player.whoAmI);
			return false;
		}
	}

	public class LootBoxContents
	{
		public int intid;
		public int ammount;

		public LootBoxContents(int intid,int ammount)
		{
			this.intid = intid;
			this.ammount = ammount;
		}
		public LootBoxContents(int intid)
		{
			this.intid = intid;
			this.ammount = 1;
		}

	}

	public class LootBoxOpen : ModProjectile
	{
		protected virtual int size => 48; //How Far each entry is spaced apart
		protected virtual float speed => 0.15f; //How fast the items scroll be, be mindful of this value to not go over the max items!
		protected virtual int maxItems => 1000; //Max Items of course, try not to set this any higher as it's unneeded stress
		protected virtual int slowDownRate => 200; //This is how many frames it takes before the ticker comepletely stops when slowing down
		protected virtual int itemsVisible => 10; //How many items would be visble left as well as right, so really it's twice this value
		protected List<LootBoxContents> loots;//List of items, no changey
		private int lastitem = 0;
		//public virtual float trans => 1f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Loot Box?");
		}

		public override bool ? CanDamage()
		{
			return false;
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.ignoreWater = true;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			AIType = ProjectileID.WoodenArrowFriendly;
			Projectile.scale = 0;

			
			Projectile.timeLeft = 800 + Main.rand.Next(-100, 300);//Adjust how long the ticker goes, again be mindful of the max item count
			Projectile.localAI[0] = Main.rand.Next(40, 60);//Starting position, make sure this is higher than items Visible
		}

		public virtual void ExtraItem(WeightedRandom<LootBoxContents> WR)
        {
			//WR.Add(new LootBoxContents(ItemID.Handgun, 1), 4);
		}

		//Fun part :p, Control what goes into the loot box! This is per item
		protected virtual void FillLootBox(WeightedRandom<LootBoxContents> WR)
		{
			WR.Add(new LootBoxContents(ItemID.TwilightDye,5), 1);
			WR.Add(new LootBoxContents(ItemID.CoinGun,1), 0.1);
			WR.Add(new LootBoxContents(ItemID.AleThrowingGlove,1), 0.01);
			WR.Add(new LootBoxContents(ItemID.Handgun,1), 4);
			loots.Add(WR.Get());
		}

		public override bool IsLoadingEnabled(Mod mod)
		{
			return !(GetType() == typeof(LootBoxOpen));
		}

		protected virtual void TickEffect()
		{
			//Lets you play a sound or otherwise make a client sided effect when the counter ticks over something
		}

		protected virtual void AwardItem(int itemtype)
		{
			//Lets you make effects when you get the item, keep in mind this is CLIENT SIDED!
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			Projectile.extraUpdates = 0;

			if (owner != null && owner.active && owner.SGAPly().liquidGambling > 0)
            {
				Projectile.extraUpdates = 3;
            }


			if (loots==null)
			{
				WeightedRandom<LootBoxContents> WR = new WeightedRandom<LootBoxContents>();
				loots = new List<LootBoxContents>();
				for (int i = 0; i < maxItems; i += 1)
				{
					FillLootBox(WR);
					ExtraItem(WR);
					WR.Clear();
					WR.needsRefresh = true;
				}
			}

			Projectile.localAI[0] += Math.Max(0f,((Projectile.timeLeft - (slowDownRate)) / (float)slowDownRate) * speed);

			if (Projectile.localAI[0] > loots.Count)
				Projectile.localAI[0] = 0f;

			if (lastitem!= (int)Projectile.localAI[0])
			{
				lastitem = (int)Projectile.localAI[0];
				TickEffect();
			}


				Player player = Main.player[Projectile.owner];

			if (player != null && player.active)
			{

				Projectile.scale = Math.Min(Projectile.scale+(1f/60f), Math.Min(1f,Projectile.timeLeft / 60f));

				if (player.dead)
				{
					Projectile.Kill();
				}
				else
				{
					if (Projectile.timeLeft == 200 && Main.netMode!=NetmodeID.Server)
					{
						LootBoxContents itemtype = loots[(int)Projectile.localAI[0]];
						player.QuickSpawnItem(null, itemtype.intid, itemtype.ammount);
						AwardItem(itemtype.intid);
					}

					Vector2 mousePos = Main.MouseWorld;

					if (Projectile.owner == Main.myPlayer)
					{
						Vector2 diff = mousePos - player.Center;
						diff.Normalize();
						Projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
						Projectile.netUpdate = true;
					}

					Projectile.velocity = default(Vector2);
					Projectile.Center = (player.Center);

				}
			}
			else
			{
				Projectile.Kill();
			}

		}

		public override bool PreDraw(ref Color lightColor)
		{

			Player player = Main.player[Projectile.owner];
			for (int f = 0; f < loots.Count; f += 1)
				{
				int lootboxsize = size;
				float offsetsizer = (Projectile.localAI[0]* -lootboxsize) + ((float)lootboxsize*f);
					Vector2 hereas = new Vector2((f-Projectile.localAI[0])* (float)lootboxsize, -64);

					Vector2 drawPos = ((hereas * Projectile.scale)+ Projectile.Center) - Main.screenPosition;
					Color glowingcolors1 = Color.White;

				float alpha = MathHelper.Clamp(1f-Math.Abs((((float)f-Projectile.localAI[0])/(float)itemsVisible)),0f,1f);

					if (alpha > 0f)
					{

					if ((int)Projectile.localAI[0] == f)
						glowingcolors1 = Color.Red;

					Texture2D tex = Terraria.GameContent.TextureAssets.Item[loots[f].intid].Value;
					//spriteBatch.Draw(tex, drawPos, null, glowingcolors1 * Projectile.scale * alpha, 0, new Vector2(tex.Width / 2, tex.Height / 2), (0.5f + (0.5f * alpha)) * Projectile.scale, SpriteEffects.None, 0f);
					}
				}
			//spriteBatch.Draw(Terraria.GameContent.TextureAssets.BlackTile.Value, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 4, 200), Color.Red * Projectile.scale, MathHelper.Pi, new Vector2(2, 8), new Vector2(1, 1) * Projectile.scale, SpriteEffects.None, 0f);

			return false;
		}


	}



}