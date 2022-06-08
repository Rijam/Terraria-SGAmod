using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Effects;
using Terraria.Audio;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class MossYoyo : MangroveBow,IDankSlowText, IMangroveSet
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quagmire");
			Tooltip.SetDefault("Hits apply Dank Slow against your foes");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.Amarok);
			Item.damage = 25;
			Item.useTime = 24;
			Item.useAnimation = 22;
			Item.useStyle = 5;
			Item.channel = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Melee;
			Item.crit = 4;
			Item.knockBack = 4.5f;
			Item.value = 47000 * 5;
			Item.rare = 3;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.shoot = Mod.Find<ModProjectile>("MossYoyoProj").Type;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.WoodYoyo).AddIngredient(null, "BiomassBar", 8).AddTile(TileID.WorkBenches).Register();
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
			return false;
		}
	}
    public class Kelvin : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Kelvin");
            Tooltip.SetDefault("'Flaming!'\nLava Burns enemies for 2 seconds on hit\nDoes 25% increased damage against lava-immune enemies");
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
            Item.damage = 40;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = 5;
            Item.channel = true;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 2.5f;
            Item.value = 10000;
            Item.noMelee = true;
            Item.rare = 6;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.KelvinProj>();
            Item.UseSound = SoundID.Item19;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(null, "FieryShard", 10).AddIngredient(mod.ItemType("UnmanedBar"), 10).AddTile(TileID.MythrilAnvil).Register();
        }
    }

    public class Jaws : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jaws");
			Tooltip.SetDefault("Throws a yoyo with spinning teeth that halve enemy defense\nThe teeth break on tiles or after they hit enemies enough times");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.Amarok);
			Item.damage = 45;
			Item.useTime = 24;
			Item.useAnimation = 22;
			Item.useStyle = 5;
			Item.channel = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.crit = 4;
			Item.knockBack = 2.2f;
			Item.value = 100000;
			Item.rare = 5;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item19;
			Item.shoot = Mod.Find<ModProjectile>("JawsProj").Type;
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
			return false;
		}
	}

	public class Upheaval : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Upheaval");
			Tooltip.SetDefault("Unleashes flaming boulders as it's held out");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
			Item.damage = 105;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.noMelee = true;
			Item.useStyle = 5;
			Item.channel = true;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 4f;
			Item.value = 500000;
			Item.rare = 9;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item19;
			Item.shoot = Mod.Find<ModProjectile>("UpheavalProj").Type;
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
			return false;
		}
	}

	public class Tornado : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tornado");
			Tooltip.SetDefault("Control a living tornado that fires homing tornadoes at enemies as it's held out");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
			Item.damage = 125;
			Item.useTime = 22;
			Item.useAnimation = 22;
			Item.useStyle = 5;
			Item.channel = true;
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.knockBack = 2.5f;
			Item.value = 1000000;
			Item.rare = 9;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item19;
			Item.shoot = Mod.Find<ModProjectile>("TornadoProj").Type;
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
			return false;
		}
	}


}

namespace SGAmod.Items.Weapons
{
    public class CreepersThrow : ModItem, IDevItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mister Creeper's Explosive Throw");
            Tooltip.SetDefault("Controls a yoyo shaped creeper that lights a fuse when near enemies and explodes violently shortly after\nHowever, watch out as you can hurt yourself from the creeper's explosion");
        }

        public (string, string) DevName()
        {
            return ("Mister Creeper", "other (legacy)");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.LocalPlayer.GetModPlayer<SGAPlayer>().devempowerment[1] > 0)
            {
                tooltips.Add(new TooltipLine(Mod, "DevEmpowerment", "--- Enpowerment bonus ---"));
                tooltips.Add(new TooltipLine(Mod, "DevEmpowerment", "40% increased damage"));
                tooltips.Add(new TooltipLine(Mod, "DevEmpowerment", "Creates smaller explosions leading up to the larger one"));
            }
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
            Item.damage = 250;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = 5;
            Item.crit = 10;
            Item.channel = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.knockBack = 2.5f;
            Item.value = Item.sellPrice(0, 75, 0, 0);
            Item.rare = 11;
            Item.expert = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item19;
            Item.shoot = Mod.Find<ModProjectile>("CreepersThrowProj").Type;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ItemID.LunarBar, 10).AddIngredient(ItemID.FragmentSolar, 10).AddIngredient(ItemID.ExplosivePowder, 100).AddIngredient(ItemID.ChlorophyteBar, 10).AddIngredient(mod.ItemType("CosmicFragment"), 1).AddIngredient(mod.ItemType("MoneySign"), 10).AddTile(TileID.LunarCraftingStation).Register();
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            if (player.GetModPlayer<SGAPlayer>().devempowerment[1] > 0)
                add += 0.40f;
        }
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
    }

    public class CreepersThrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Creeper's Throw");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 999f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 500f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 20f;
        }

        public override string Texture
        {
            get { return ("SGAmod/Projectiles/CreepersThrow"); }
        }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.TheEyeOfCthulhu);
            Projectile.extraUpdates = 0;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 99;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1f;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (owner != null && !owner.dead)
            {
                if (Projectile.localAI[1] < 0)
                    Projectile.localAI[1] += 1;


                if (Projectile.localAI[1] < 1)
                {
                    NPC target = Main.npc[Idglib.FindClosestTarget(0, Projectile.Center, new Vector2(0f, 0f), true, true, true, Projectile)];
                    if (target != null && Vector2.Distance(target.Center, Projectile.Center) < 72)
                    {
                        Projectile.localAI[1] = 1;


                    }
                }
                else
                {
                    Projectile.localAI[1] += 1;

                    int dustIndexsmoke = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[dustIndexsmoke].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndexsmoke].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndexsmoke].noGravity = true;
                    Main.dust[dustIndexsmoke].position = Projectile.Center + new Vector2(0f, (float)(-(float)Projectile.height / 2)).RotatedBy((double)Projectile.rotation, default(Vector2)) * 1.1f;
                    dustIndexsmoke = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[dustIndexsmoke].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
                    Main.dust[dustIndexsmoke].noGravity = true;
                    Main.dust[dustIndexsmoke].position = Projectile.Center + new Vector2(0f, (float)(-(float)Projectile.height / 2 - 6)).RotatedBy((double)Projectile.rotation, default(Vector2)) * 1.1f;

                    if (Projectile.localAI[1] > 40 && Projectile.localAI[1] < 120 && Projectile.localAI[1] % 25 == 0 && owner.GetModPlayer<SGAPlayer>().devempowerment[1] > 0)
                    {

                        int thisone = Projectile.NewProjectile(Projectile.Center.X - 100, Projectile.Center.Y - 100, Vector2.Zero.X, Vector2.Zero.Y, ModContent.ProjectileType<CreepersThrowBoom>(), Projectile.damage * 2, Projectile.knockBack, Main.player[Projectile.owner].whoAmI, 0.0f, 0f);
                        Main.projectile[thisone].timeLeft = 2;
                        Main.projectile[thisone].width = 200;
                        Main.projectile[thisone].penetrate = 1;
                        Main.projectile[thisone].height = 200;
                        Main.projectile[thisone].scale = 0.001f;
                        Main.projectile[thisone].netUpdate = true;

                    }

                    if (Projectile.localAI[1] == 121)
                    {
                        Projectile.localAI[1] = -60;
                        for (int i = 0; i < 359; i += 36)
                        {
                            double angles = MathHelper.ToRadians(i);
                            float randomx = 64f;//Main.rand.NextFloat(54f, 96f);
                            Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles));

                            int thisone = Projectile.NewProjectile(Projectile.Center.X + (here.X * randomx) - 100, Projectile.Center.Y + (here.Y * randomx) - 100, here.X, here.Y, ModContent.ProjectileType<CreepersThrowBoom>(), Projectile.damage * 1, Projectile.knockBack, Main.player[Projectile.owner].whoAmI, 0.0f, 0f);
                            Main.projectile[thisone].timeLeft = 2;
                            Main.projectile[thisone].width = 200;
                            Main.projectile[thisone].height = 200;
                            Main.projectile[thisone].scale = 0.001f;
                            Main.projectile[thisone].netUpdate = true;

                        }
                    }
                    if (Projectile.localAI[1] == 120)
                    {

                        for (int i = 0; i < 359; i += 72)
                        {
                            double angles = MathHelper.ToRadians(i);
                            float randomx = 48f;//Main.rand.NextFloat(54f, 96f);
                            Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles));

                            int thisone = Projectile.NewProjectile(Projectile.Center.X + (here.X * randomx) - 100, Projectile.Center.Y + (here.Y * randomx) - 100, here.X, here.Y, ModContent.ProjectileType<CreepersThrowBoom>(), Projectile.damage * 1, Projectile.knockBack, Main.player[Projectile.owner].whoAmI, 0.0f, 0f);
                            Main.projectile[thisone].timeLeft = 2;
                            Main.projectile[thisone].width = 200;
                            Main.projectile[thisone].penetrate = 1;
                            Main.projectile[thisone].height = 200;
                            Main.projectile[thisone].scale = 0.001f;
                            Main.projectile[thisone].netUpdate = true;

                        }


                    }

                }

            }
        }

    }

    public class CreepersThrowBoom : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Creeper's KaBoom");
        }

        public override string Texture
        {
            get { return ("SGAmod/Projectiles/CreepersThrow"); }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (!target.friendly && !target.dontTakeDamage && target.immune[Main.player[Projectile.owner].whoAmI] > 0)
                return true;
            return base.CanHitNPC(target);
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.GrenadeIII);
            Projectile.scale = 0.001f;
            Projectile.DamageType = DamageClass.Melee;
            //projectile.penetrate = 1;
            AIType = ProjectileID.GrenadeIII;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.immune[Projectile.owner] = 1;
        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.type = ProjectileID.GrenadeIII;
            return true;
        }
    }

    public class CreepersThrowBoom2 : CreepersThrowBoom
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Creeper's KaBoom");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.timeLeft = 3;
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.scale = 0.001f;
            Projectile.timeLeft = 2;
            Projectile.penetrate = 1;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life - damage < 0)
            {
                Main.player[Projectile.owner].netLife = true;
                Main.player[Projectile.owner].statLife += 50;
                Main.player[Projectile.owner].GetModPlayer<SGAPlayer>().creeperexplosion = 0;
            }
        }


    }

    public class ThievesThrow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thieves' Throw");
            Tooltip.SetDefault("Steals items off the ground and teleports them to the player\n'Yes, it also steals Hearts, gotta put that joke in there'");
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
            Item.damage = 0;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = 5;
            Item.channel = true;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 2.5f;
            Item.value = 50000;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Green;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.shoot = Mod.Find<ModProjectile>("ThievesThrowProj").Type;
            Item.UseSound = SoundID.Item19;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, 0, knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
    }

    public class ThievesThrowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thieves Throw");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 300.0f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 1500f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 25f;
        }

        public override string Texture => "SGAmod/Projectiles/ThievesThrowProj";

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.TheEyeOfCthulhu);
            Projectile.extraUpdates = 0;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 99;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1f;
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            int dist = GetType() == typeof(RiftThrowProj) ? 50000 : 20000;

            foreach (Item item in Main.item.Where(testby => testby.active && (Projectile.Center - testby.Center).LengthSquared() < dist))
            {
                item.velocity += Collision.TileCollision(item.position, Vector2.Normalize(Projectile.Center - item.Center) * 0.40f, item.width, item.height);

                if ((item.Center - Projectile.Center).LengthSquared() < 600)
                {
                    item.Center = player.Center;
                }
            }
        }
    }

    public class RiftYoyo : ThievesThrow
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desolate Rift");
            Tooltip.SetDefault("Sucks in enemies and items, deals Damage over Time");
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
            Item.damage = 250;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.useStyle = 5;
            Item.channel = true;
            Item.DamageType = DamageClass.Melee;
            Item.value = 50000;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Yellow;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<RiftThrowProj>();
            Item.UseSound = SoundID.Item19;
        }

        public override string Texture => "SGAmod/Items/Weapons/RiftYoyo";

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<ThievesThrow>(), 1).AddIngredient(ModContent.ItemType<StygianCore>(), 1).AddIngredient(ModContent.ItemType<OverseenCrystal>(), 20).AddTile(TileID.LunarCraftingStation).Register();
        }
    }

    public class RiftThrowProj : ThievesThrowProj
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Desolate Rift");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 20.0f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 500f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 15f;
        }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.TheEyeOfCthulhu);
            Projectile.extraUpdates = 0;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 99;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
        }

        public override string Texture => "SGAmod/Projectiles/RiftYoyoProj";

        public override bool CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            base.AI();
            Player player = Main.player[Projectile.owner];
            foreach (NPC npc in Main.npc.Where(testby => testby.active && !testby.friendly && !testby.dontTakeDamage && testby.chaseable && (Projectile.Center - testby.Center).LengthSquared() < 60000))
            {
                if (!npc.IsDummy() && (npc.Center - Projectile.Center).LengthSquared() < 50000)
                {
                    npc.Center += Collision.TileCollision(npc.position, Vector2.Normalize(Projectile.Center - npc.Center) * 4f * (npc.knockBackResist + 0.25f), npc.width, npc.height);

                    npc.SGANPCs().nonStackingImpaled = Math.Max(npc.SGANPCs().nonStackingImpaled, Projectile.damage);
                }
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Dimensions.NPCs.SpaceBoss.DarknessNebulaEffect(Mod.Assets.Request<Texture2D>("GlowOrb").Value, 0f, Projectile.Center, 0.25f, Projectile.whoAmI, 10, -5f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            return base.PreDraw(spriteBatch, lightColor);
        }


    }

    public class LeechHead
    {
        public float leechSpeed = 12f;
        protected int _leechTime = 60;
        protected float averageDist = 1f;
        public int leechTime => (int)(_leechTime* averageDist);
        public int maxDist = 450000;

        public Vector2 position;
        public Vector2 Position => Vector2.Lerp(position, owner.Center, percent);
        public float percent = 0f;
        public int state = 0;
        public int damageLeeched = 0;
        public int timer = 0;
        public NPC target;
        public Projectile owner;

        public LeechHead(NPC target, Projectile owner)
        {
            this.owner = owner;
            position = this.owner.Center;
            this.target = target;

        }
        public void Update()
        {

            if (percent > 0 || owner.ai[0] < 0 || ((target.Center - owner.Center).LengthSquared() > (maxDist)) || target == null || !target.active)
            {
                percent = MathHelper.Clamp(percent + 0.05f, 0f, 1f);
                return;
            }

            if (target != null && target.active)
            {


                if (state < 1)
                {
                    Vector2 difference = target.Center - position;
                    if (difference.LengthSquared() < (leechSpeed * 2) * (leechSpeed * 2))
                    {
                        state = 1;
                        goto next;
                    }
                    position += Vector2.Normalize(difference) * leechSpeed;
                }

            next:

                if (state == 1)
                {
                    position = target.Center;
                    timer -= 1;
                    if (timer < 0)
                    {
                        var snd = SoundEngine.PlaySound(SoundID.Item, (int)Position.X, (int)Position.Y, 3);

                        if (snd != null)
                        {
                            snd.Pitch = -0.50f;
                            snd.Volume = 0.50f;
                        }

                        Vector2 difference = target.Center - owner.Center;
                        averageDist = Math.Max((difference.Length()/260f),1f);

                        timer = leechTime;

                        int damage = Main.DamageVar(owner.damage);
                        target.StrikeNPC(damage, 0, 1);
                        Main.player[owner.owner].addDPS(damage);
                        damageLeeched += damage;

                    }
                }
            }
        }
    }
    public class LeechYoyo : ThievesThrow
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Tongue");
            Tooltip.SetDefault("Leeches up to 5 nearby enemies of their life\nYou are healed when the yoyo returns to you, based off life leeched");
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
            Item.damage = 60;
            Item.useTime = 64;
            Item.useAnimation = 64;
            Item.useStyle = 5;
            Item.channel = true;
            Item.DamageType = DamageClass.Melee;
            Item.value = 10000;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Yellow;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<LeechYoyoProj>();
            Item.UseSound = SoundID.Item19;
        }

        public override string Texture => "SGAmod/Items/Weapons/TheTongue";

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0.0f, 0.0f);
            return false;
        }
    }

    public class LeechYoyoProj : ThievesThrowProj
    {
        //Extra 23-24, 
        //ProjectileID.MoonLeech

        //Chain 12-Leech
        //NPCID.LeechHead

        public virtual Texture2D HeadTex => SGAmod.ExtraTextures[120];
        public virtual Texture2D ChainTex => Main.chain12Texture;
        public virtual int maxDist => 150000;
        public virtual float DirectionAdd => MathHelper.PiOver2;


        public List<LeechHead> leeches = new List<LeechHead>();
        public int leechcooldown = 0;
        public virtual (int, int) MaxLeeches => (5, 30);




        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leech Yoyo");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 8.0f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 240f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 8f;
        }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.TheEyeOfCthulhu);
            Projectile.extraUpdates = 0;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = 99;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.scale = 1f;
        }

        public override string Texture => "SGAmod/Projectiles/TheTongueProj";

        
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage *= 1;
        }

        
        public override bool CanDamage()
        {
            return false;
        }
         

        public override bool PreKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            if ((Projectile.Center - player.Center).LengthSquared() < 3200)
            {
                Projectile vamp = new Projectile();
                vamp.owner = player.whoAmI;
                int healing = 0;
                foreach (LeechHead leech in leeches)
                {
                    healing += leech.damageLeeched;
                }

                bool lunar = GetType() == typeof(LunarLeechProj);

                healing = (int)((lunar ? (int)(healing * 0.15f) : (int)healing) *0.60f);
                vamp.vampireHeal(healing, Projectile.Center);

                if (lunar)
                {
                    //Main.player[projectile.owner].AddBuff(BuffID.MoonLeech, (int)(0.5 * healing));
                }
            }

            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            leechcooldown -= 1;

            if (leechcooldown < 1 && Projectile.ai[0] >= 0 && leeches.Where(testby => testby.percent <= 0).Count() < MaxLeeches.Item1)
            {
                foreach (NPC npc in Main.npc.Where(testby => testby.active && !testby.friendly && !testby.dontTakeDamage && testby.chaseable && !leeches.Where(testby3 => testby3.percent <= 0).Select(testby2 => testby2.target).Contains(testby) && (Projectile.Center - testby.Center).LengthSquared() < maxDist).OrderBy(testby => (Projectile.Center - testby.Center).LengthSquared()))
                {
                    if (!npc.IsDummy())// && (npc.Center - projectile.Center).LengthSquared() < maxDist)
                    {
                        leechcooldown = MaxLeeches.Item2;
                        LeechHead newLeech = GetType() == typeof(LunarLeechProj) ? new LunarLeechHead(npc, Projectile) : new LeechHead(npc, Projectile);

                        leeches.Add(newLeech);
                        break;
                    }
                }
            }

            foreach (LeechHead leech in leeches)
            {
                leech.Update();
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Dimensions.NPCs.SpaceBoss.DarknessNebulaEffect(mod.GetTexture("GlowOrb"), 0f, projectile.Center, 0.25f, projectile.whoAmI, 10, -5f);

            foreach (LeechHead leech in leeches)
            {
                if (leech.percent >= 1)
                    continue;

                List<Vector2> vectors = new List<Vector2>();

                Vector2 dist = (leech.Position - Projectile.Center);

                for (float ff = 0; ff <= 1f; ff += 0.01f)
                {
                    vectors.Add(Vector2.Lerp(Projectile.Center, leech.Position, ff));
                }

                float alphaPercent = (leech.timer / (float)leech.leechTime) * (1f - leech.percent);

                float scaleSize = MathHelper.Clamp((float)Math.Sin(alphaPercent * MathHelper.Pi) * 1.25f, 0f, 1f);

                TrailHelper trail = new TrailHelper("FadedBasicEffectPass", ChainTex);
                trail.projsize = Vector2.Zero;
                trail.coordOffset = new Vector2(0, 0);
                trail.coordMultiplier = new Vector2(1f, dist.Length() / (float)ChainTex.Height);
                trail.doFade = false;
                trail.trailThickness = 8;
                trail.trailThicknessFunction = delegate (float percent)
                {
                    float sizer = 8;
                    float maxSizeincrease = leech.timer >= 0 ? Math.Max(0, 1f - (Math.Abs(percent - (1f - alphaPercent)) * 5f)) : 0;

                    return sizer + (maxSizeincrease * 12f * scaleSize);
                };
                trail.color = delegate (float percent)
                {
                    Vector2 vex2 = Vector2.Lerp(Projectile.Center, leech.Position, percent);
                    Point there = new Point((int)(vex2.X / 16), (int)(vex2.Y) / 16);
                    return Lighting.GetColor(there.X, there.Y,Color.White);
                };
                trail.trailThicknessIncrease = 0;
                trail.DrawTrail(vectors, Projectile.Center);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Texture2D gurgleTex = Main.itemTexture[ItemID.Bubble];

            foreach (LeechHead leech in leeches)
            {
                if (leech.percent >= 1)
                    continue;

                float direction = (leech.Position - Projectile.Center).ToRotation();

                float alphaPercent = (leech.timer / (float)leech.leechTime) * (1f - leech.percent);

                float scaleSize = MathHelper.Clamp((float)Math.Sin(alphaPercent * MathHelper.Pi) * 1.25f, 0f, 1f);

                if (GetType() == typeof(LunarLeechProj))
                {
                    Point point = new Point((int)(leech.Position.X / 16), (int)(leech.Position.Y / 16));
                    gurgleTex = Main.itemTexture[ItemID.LivingUltrabrightFireBlock];
                    int frame = (leech.timer / 4) % 4;
                    Rectangle rect = new Rectangle(0, frame*(HeadTex.Height / 4), HeadTex.Width, HeadTex.Height / 4);
                    spriteBatch.Draw(HeadTex, leech.Position - Main.screenPosition, rect, Lighting.GetColor(point.X, point.Y), direction + DirectionAdd, rect.Size() / 2f, 1, SpriteEffects.None, 0f);

                    spriteBatch.Draw(gurgleTex, Vector2.Lerp(Projectile.Center, leech.Position, alphaPercent) - Main.screenPosition, null, Color.Aquamarine * scaleSize * 0.50f, direction, gurgleTex.Size() / 2f, (0.25f + scaleSize)*new Vector2(1.25f,0.60f), SpriteEffects.None, 0f);
                }
                else
                {
                    Point point = new Point((int)(leech.Position.X / 16), (int)(leech.Position.Y / 16));
                    spriteBatch.Draw(HeadTex, leech.Position - Main.screenPosition, null, Lighting.GetColor(point.X, point.Y), direction + DirectionAdd, HeadTex.Size() / 2f, 1, SpriteEffects.None, 0f);

                    spriteBatch.Draw(gurgleTex, Vector2.Lerp(Projectile.Center, leech.Position, alphaPercent) - Main.screenPosition, null, lightColor.MultiplyRGB(Color.Red) * scaleSize * 0.25f, direction, gurgleTex.Size() / 2f, 0.25f + scaleSize, SpriteEffects.None, 0f);
                }
            }

            spriteBatch.Draw(Main.projectileTexture[Projectile.type], Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, Main.projectileTexture[Projectile.type].Size() / 2f, 1, SpriteEffects.None, 0f);

            return false;// base.PreDraw(spriteBatch, lightColor);
        }


    }

    public class LunarLeechHead : LeechHead
    {
        public LunarLeechHead(NPC target, Projectile owner) : base(target, owner)
        {
            leechSpeed = 20;
            _leechTime = 20;
            maxDist = 600000;
        }
    }

    public class LunarLeechYoyo : LeechYoyo
    {
        public override string Texture => "SGAmod/Items/Weapons/LunarLeech";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Lunar Leech");
            Tooltip.SetDefault("'He only wants a taste...'\nSummons Moonlord tongues that Leeches up to 10 nearby enemies\nYou are healed when the yoyo returns to you, based off life leeched");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 150;
            Item.value = 100000;
            Item.shoot = ModContent.ProjectileType<LunarLeechProj>();
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient(ModContent.ItemType<EldritchTentacle>(), 30).AddIngredient(ModContent.ItemType<LeechYoyo>(), 1).AddTile(TileID.LunarCraftingStation).Register();
        }
    }

    public class LunarLeechProj : LeechYoyoProj
    {
        //Extra 23-24, 
        //ProjectileID.MoonLeech

        //Chain 12-Leech
        //NPCID.LeechHead

        public override Texture2D HeadTex => SGAmod.ExtraTextures[121];
        public override Texture2D ChainTex => Main.extraTexture[23];
        public override int maxDist => 150000;
        public override float DirectionAdd => -MathHelper.PiOver2;
        public override (int, int) MaxLeeches => (10, 20);




        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lunar Leech Yoyo");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 8.0f;
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 240f;
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 12f;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override string Texture => "SGAmod/Projectiles/LunarLeechProj";
    }
}
