using Canisters.Content.Projectiles.GlisteningCanister;
using Canisters.Helpers;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class GlisteningCanister : BaseCanisterItem
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

	public override void SetDefaults() {
		Item.DefaultToCanister(12, 2f, 4f);
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(silver: 9));
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Ichor)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
