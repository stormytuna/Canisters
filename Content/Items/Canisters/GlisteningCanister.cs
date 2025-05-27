using Canisters.Common;
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
		get => Color.Yellow;
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(13, 2f, 0f);
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(silver: 9));
	}

	public override void AddRecipes() {
		int amount = ServerConfig.Instance.LowGrind ? 300 : 150;
		CreateRecipe(amount)
			.AddIngredient<EmptyCanister>(amount)
			.AddIngredient(ItemID.Ichor)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
