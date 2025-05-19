using Canisters.Common;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class VolatileCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredVolatileCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<GelBallEmitter>();
	}

	public override Color CanisterColor {
		get => new(45, 144, 255, 255);
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(6, 1f, 0f);
		Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(copper: 75));
	}

	public override void AddRecipes() {
		int amount = ServerConfig.Instance.LowGrind ? 200 : 100;
		CreateRecipe(amount)
			.AddIngredient<EmptyCanister>(amount)
			.AddIngredient(ItemID.Gel)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
