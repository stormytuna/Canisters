using Canisters.Content.Items.Canisters;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons {
    public class ModifiedHandgun : CanisterWeapon {
        public override void SetStaticDefaults() {
            SacrificeTotal = 1;

            base.SetStaticDefaults();
        }

        public override void SetDefaults() {
            // Base stats
            Item.width = 28;
            Item.height = 20;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Blue;

            // Use stats
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 24;
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
            Item.useAmmo = ModContent.ItemType<MagmaCanister>();

            base.SetDefaults();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Vector2 spawnPos = player.RotatedRelativePoint(player.MountedCenter, true);
            Projectile.NewProjectile(source, player.Center, spawnPos, ModContent.ProjectileType<ModifiedHandgun_HeldProjectile>(), damage, knockback, player.whoAmI);

            return false;
        }
    }

    public class ModifiedHandgun_HeldProjectile : CanisterHeldProjectile {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Modified Handgun");

            base.SetStaticDefaults();
        }

        public override void SetDefaults() {
            // Base stats
            Projectile.width = 28;
            Projectile.height = 20;
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
            HoldOutOffset = 14f;
            CanisterFiringType = FiringType.Regular;
            RotationOffset = MathHelper.PiOver2;

            base.SetDefaults();
        }

        public override string Texture => "Canisters/Content/Items/Weapons/ModifiedHandgun";

        public override void Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, Owner.whoAmI);

            base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}
