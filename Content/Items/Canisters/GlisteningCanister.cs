using Canisters.Content.Projectiles.GlisteningCanister;
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
		get => new Color(237, 211, 44, 200);
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(13, 2f, 0f);
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(silver: 9));
	}

	public override void AddRecipes() {
		CreateRecipe(150)
			.AddIngredient<EmptyCanister>(150)
			.AddIngredient(ItemID.Ichor)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
