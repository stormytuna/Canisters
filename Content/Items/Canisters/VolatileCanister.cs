using Canisters.Content.Projectiles.Canisters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters {
    public class VolatileCanister : CanisterItem {
        public override void SetStaticDefaults() {
            SacrificeTotal = 99;

            base.SetStaticDefaults();
        }

        public override void SetDefaults() {
            // Base stats
            Item.width = 22;
            Item.height = 22;
            Item.maxStack = 999;
            Item.value = 2;

            // Ammo stats
            Item.shoot = ModContent.ProjectileType<Projectiles.Canisters.VolatileCanister>();
            Item.shootSpeed = 2f;
            Item.damage = 1;
            Item.knockBack = 0f;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = Type;
            Item.consumable = true;

            // CanisterItem stats
            LaunchedProjectileType = Item.shoot;
            DepletedProjectileType = ModContent.ProjectileType<VolatileCanister_Depleted>();
            DamageWhenLaunched = 14;
            DamageWhenDepleted = 3;

            base.SetDefaults();
        }

        public override void AddRecipes() {
            CreateRecipe(300)
                .AddIngredient<EmptyCanister>(300)
                .AddIngredient(ItemID.Gel)
                .AddTile(TileID.Bottles)
                .Register();

            base.AddRecipes();
        }
    }
}
