using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters {
    public class VolatileCanister : ModItem, ICanisterItem {
        public int LaunchedProjectileType { get => ModContent.ProjectileType<Projectiles.Canisters.VolatileCanister>(); }
        public int DepletedProjectileType { get => ModContent.ProjectileType<Projectiles.Canisters.VolatileCanister_Depleted>(); }

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
            Item.rare = ItemRarityID.Blue;

            // Ammo stats
            Item.shoot = ModContent.ProjectileType<Projectiles.Canisters.VolatileCanister>();
            Item.shootSpeed = 2f;
            Item.damage = 3;
            Item.knockBack = 5f;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = Type;
            Item.consumable = true;

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
