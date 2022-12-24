using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters {
    public class EmptyCanister : ModItem {
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

            base.SetDefaults();
        }

        public override void AddRecipes() {
            CreateRecipe(150)
                .AddRecipeGroup(RecipeGroupID.IronBar)
                .AddIngredient(ItemID.Glass)
                .AddTile(TileID.Anvils)
                .Register();

            base.AddRecipes();
        }
    }
}