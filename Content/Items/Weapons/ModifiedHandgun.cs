using Canisters.Content.Items.Canisters;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons
{
    public class ModifiedHandgun : CanisterUsingWeapon
    {
        public override void SetStaticDefaults() {
            SacrificeTotal = 1;

            base.SetStaticDefaults();
        }

        public override void SetDefaults() {
            // Base stats
            Item.width = 36;
            Item.height = 18;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Blue;

            // Use stats
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = true;

            // Weapon stats
            Item.shoot = ModContent.ProjectileType<ModifiedHandgun_HeldProjectile>();
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
                .AddIngredient(ItemID.FlintlockPistol)
                .AddIngredient(ItemID.IllegalGunParts)
                .AddTile(TileID.Anvils)
                .Register();

            base.AddRecipes();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile.NewProjectile(source, player.Center, position, ModContent.ProjectileType<ModifiedHandgun_HeldProjectile>(), damage, knockback, player.whoAmI);

            return false;
        }
    }

    public class ModifiedHandgun_HeldProjectile : CanisterUsingHeldProjectile
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Modified Handgun");

            base.SetStaticDefaults();
        }

        public override void SetDefaults() {
            // Base stats
            Projectile.width = 36;
            Projectile.height = 18;
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

            // CanisterUsingHeldProjectile stats
            HoldOutOffset = 14f;
            CanisterFiringType = FiringType.Regular;
            RotationOffset = 0f;
            MuzzleOffset = new Vector2(16f, -6f);

            base.SetDefaults();
        }

        public override string Texture => "Canisters/Content/Items/Weapons/ModifiedHandgun";

        public override void Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            if (Collision.CanHit(player.Center, 0, 0, position, 0, 0)) {
                // TODO: Using this gun to debug canisters for now
                // Come back later and change these comments
                //velocity *= 1.2f;
                //knockback /= 1.2f;
                Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Owner.whoAmI);

                //proj.scale /= 1.2f;
            }

            base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
