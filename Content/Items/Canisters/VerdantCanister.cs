using Canisters.Content.Projectiles.Canisters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters {
    public class VerdantCanister : CanisterItem {
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
            Item.shoot = ModContent.ProjectileType<Projectiles.Canisters.VerdantCanister>();
            Item.shootSpeed = 8f;
            Item.damage = 1;
            Item.crit = 4;
            Item.knockBack = 6f;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = ModContent.ItemType<VolatileCanister>();

            // CanisterItem stats
            LaunchedProjectileType = Item.shoot;
            DepletedProjectileType = ModContent.ProjectileType<VerdantCanister_Depleted>();
            DamageWhenLaunched = 15;
            DamageWhenDepleted = 2;

            base.SetDefaults();
        }

        public override void AddRecipes() {
            CreateRecipe(300)
                .AddIngredient<EmptyCanister>(300)
                .AddIngredient(ItemID.JungleSpores)
                .AddTile(TileID.Bottles)
                .Register();

            base.AddRecipes();
        }
    }
}
