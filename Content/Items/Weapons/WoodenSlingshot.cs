using Canisters.Content.Items.Canisters;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons {
    public class WoodenSlingshot : CanisterUsingWeapon {
        public override void SetStaticDefaults() {
            SacrificeTotal = 1;

            base.SetStaticDefaults();
        }

        public override void SetDefaults() {
            // Base stats
            Item.width = 20;
            Item.height = 32;
            Item.value = Item.sellPrice(silver: 2);
            Item.rare = ItemRarityID.Blue;

            // Use stats
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 30;
            Item.autoReuse = true;
            Item.noMelee = true;

            // Weapon stats
            Item.shoot = ItemID.PurificationPowder;
            Item.shootSpeed = 9f;
            Item.damage = 11;
            Item.crit = 4;
            Item.knockBack = 1f;
            Item.DamageType = DamageClass.Ranged;
            Item.useAmmo = ModContent.ItemType<VolatileCanister>();

            base.SetDefaults();
        }

        public override void AddRecipes() {
            CreateRecipe()
                .AddIngredient(ItemID.Silk)
                .AddIngredient(ItemID.Wood, 8)
                .AddTile(TileID.WorkBenches)
                .Register();

            base.AddRecipes();
        }


    }

    public class WoodenSlingshot_HeldProjectile : CanisterUsingHeldProjectile {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Wooden Slingshot");

            base.SetStaticDefaults();
        }

        public override void SetDefaults() {
            // Base stats
            Projectile.width = 20;
            Projectile.height = 32;
            Projectile.aiStyle = -1;

            // Weapon stats
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;

            // Held projectile stats
            Projectile.tileCollide = false;
            Projectile.hide = true;
            Projectile.ignoreWater = true;

            // CanisterHeldProjectile stats
            HoldOutOffset = 0f;
            CanisterFiringType = FiringType.Canister;
            RotationOffset = 0f;
            MuzzleOffset = new Vector2(0, -10f);

            base.SetDefaults();
        }

        public override string Texture => "Canisters/Content/Items/Weapons/WoodenSlingshot";

        public override void Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (Collision.CanHit(player.Center, 0, 0, position, 0, 0)) {
                Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
            }

            base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
