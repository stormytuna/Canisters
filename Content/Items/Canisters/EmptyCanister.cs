namespace Canisters.Content.Items.Canisters;

public class EmptyCanister : ModItem
{
	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 99;
	}

	public override void SetDefaults() {
		Item.width = 22;
		Item.height = 22;
		Item.maxStack = 999;
		Item.value = Item.buyPrice(copper: 5);
	}

	public override void AddRecipes() {
		CreateRecipe(150)
			.AddRecipeGroup(RecipeGroupID.IronBar)
			.AddIngredient(ItemID.Glass)
			.AddTile(TileID.Anvils)
			.Register();
	}
}

public class EmptyCanisterGlobalNpc : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.Demolitionist;
	}

	public override void ModifyShop(NPCShop shop) {
		shop.InsertAfter(ItemID.Dynamite, ModContent.ItemType<EmptyCanister>());
	}
}
