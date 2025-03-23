using Canisters.Content.Projectiles.GlisteningCanister;
using Canisters.Helpers._Legacy.Abstracts;

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
		Item.value = Item.buyPrice(silver: 9);
		Item.rare = ItemRarityID.LightRed;

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
