using Canisters.Content.Projectiles.GlisteningCanister;
using Canisters.Helpers.Abstracts;

namespace Canisters.Content.Items.Canisters;

public class GlisteningCanister : CanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredGlisteningCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<GlisteningBall>();
	}

	public override Color CanisterColor {
		get => Color.Yellow;
	}

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.buyPrice(silver: 9);
		Item.rare = ItemRarityID.LightRed;

		// Weapon stats
		Item.shootSpeed = 2f;
		Item.damage = 12;
		Item.knockBack = 4f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Ichor)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
