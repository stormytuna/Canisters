using Canisters.Content.Projectiles.GhastlyCanister;
using Canisters.DataStructures;
using Canisters.Helpers;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class GhastlyCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredGhastlyCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<GhastlyShot>();
	}

	public override Color CanisterColor {
		get => Color.Cyan;
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(16, 6f, 3f);
		Item.SetShopValues(ItemRarityColor.Yellow8, Item.buyPrice(copper: 5));
	}

	public override void ApplyAmmoStats(ref CanisterShootStats stats) {
		if (stats.IsLaunched) {
			return;
		}

		stats.Damage /= 6;
		stats.Knockback /= 6f;
		stats.ProjectileCount *= 6;
		stats.TotalSpread += 0.15f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Ectoplasm)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
