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
            Item.value = Item.buyPrice(copper: 5);

            base.SetDefaults();
        }

        public override void AddRecipes() {
            CreateRecipe(300)
                .AddRecipeGroup(RecipeGroupID.IronBar)
                .AddIngredient(ItemID.Glass)
                .AddTile(TileID.Anvils)
                .Register();

            base.AddRecipes();
        }
    }

    public class EmptyCanisterGlobalNPC : GlobalNPC {
        public override void SetupShop(int type, Chest shop, ref int nextSlot) {
            if (type != NPCID.Demolitionist) {
                return;
            }

            shop.item[nextSlot].SetDefaults(ModContent.ItemType<EmptyCanister>());
            nextSlot++;

            base.SetupShop(type, shop, ref nextSlot);
        }
    }
}