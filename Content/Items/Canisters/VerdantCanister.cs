using Canisters.Content.Projectiles.Canisters;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters {
    public class VerdantCanister : ModItem, ICanisterItem {
        public int LaunchedProjectileType { get => ModContent.ProjectileType<Projectiles.Canisters.VerdantCanister>(); }
        public int DepletedProjectileType { get => ModContent.ProjectileType<Projectiles.Canisters.VerdantCanister_Depleted>(); }

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
