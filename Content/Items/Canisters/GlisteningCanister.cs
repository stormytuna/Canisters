using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters
{
    public class GlisteningCanister : ModItem, ICanisterItem
    {
        public int LaunchedProjectileType { get => ModContent.ProjectileType<Projectiles.GlisteningCanister.GlisteningCanister>(); }
        public int DepletedProjectileType { get => ModContent.ProjectileType<Projectiles.GlisteningCanister.GlisteningBall>(); }

        public override void SetStaticDefaults() {
            SacrificeTotal = 99;

            base.SetStaticDefaults();
        }



        public override void SetDefaults() {
            // Base stats
            Item.width = 22;
            Item.height = 22;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(silver: 9);
            Item.rare = ItemRarityID.LightRed;

            // Weapon stats
            Item.shoot = ModContent.ProjectileType<Projectiles.VerdantCanister.VerdantCanister>();
            Item.shootSpeed = 1f;
            Item.damage = 6;
            Item.knockBack = 4f;
            Item.DamageType = DamageClass.Ranged;
            Item.ammo = ModContent.ItemType<VolatileCanister>();

            base.SetDefaults();
        }

        public override void AddRecipes() {
            CreateRecipe(300)
                .AddIngredient<EmptyCanister>(300)
                .AddIngredient(ItemID.Ichor)
                .AddTile(TileID.Bottles)
                .Register();

            base.AddRecipes();
        }
    }
}
