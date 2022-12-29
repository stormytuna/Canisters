using Canisters.Content.Projectiles.Canisters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters {
    public class MagmaCanister : CanisterItem {
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

            // Weapon stats
            Item.shoot = ModContent.ProjectileType<Projectiles.Canisters.MagmaCanister>();
            Item.shootSpeed = 2f;
            Item.damage = 1;
            Item.crit = 4;
            Item.knockBack = 8f;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = Type;

            // CanisterItem stats
            LaunchedProjectileType = Item.shoot;
            DepletedProjectileType = ModContent.ProjectileType<MagmaCanister_Depleted>();
            DamageWhenLaunched = 14;
            DamageWhenDepleted = 3;

            base.SetDefaults();
        }

        public override void AddRecipes() {
            CreateRecipe(300)
                .AddIngredient<EmptyCanister>(300)
                .AddTile(TileID.Bottles)
                .AddCondition(Recipe.Condition.NearLava)
                .Register();

            base.AddRecipes();
        }
    }
}
