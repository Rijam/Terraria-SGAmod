using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{

    public class StarfishAmmo : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Starfish)
            {
                item.shoot = Mod.Find<ModProjectile>("StarfishProjectile").Type;
                item.ammo = item.type;
            }
        }
    }
    public class StarfishBlaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfish Blaster");
            Tooltip.SetDefault("Uses starfish as ammo.\n90% to not consume ammo");
        }

        public override bool ConsumeAmmo(Player player)
        {

            return Main.rand.Next(100)<10;
        }

        public override void SetDefaults()
        {
            Item.damage = 27;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = 5;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2;
            Item.value = 10;
            Item.rare = 8;
            Item.UseSound = SoundID.Item11;
            Item.autoReuse = true;
            Item.shoot = Mod.Find<ModProjectile>("StarfishProjectile").Type;
            Item.shootSpeed = 13f;
            Item.useAmmo = 2626;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, -2);
        }   
    }
}             