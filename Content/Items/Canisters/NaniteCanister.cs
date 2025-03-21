using Canisters.Content.Projectiles.NaniteCanister;
using Canisters.Helpers.Abstracts;

namespace Canisters.Content.Items.Canisters;

public class NaniteCanister : CanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredNaniteCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<NaniteMistEmitter>();
	}

	public override Color CanisterColor {
		get => Color.LightCyan;
	}

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.buyPrice(silver: 3);
		Item.rare = ItemRarityID.Yellow;

		// Weapon stats
		Item.shootSpeed = 4f;
		Item.damage = 16;
		Item.knockBack = 2f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Nanites, 5)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
