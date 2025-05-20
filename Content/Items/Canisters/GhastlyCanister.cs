using Canisters.Common;
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
		Item.DefaultToCanister(12, 4.5f, 1f);
		Item.SetShopValues(ItemRarityColor.Yellow8, Item.buyPrice(copper: 5));
	}

	public override void ApplyAmmoStats(ref CanisterShootStats stats) {
		if (stats.IsDepleted) {
			stats.Damage /= 5;
			stats.Knockback /= 5f;
			stats.ProjectileCount *= 6;
			stats.TotalSpread += 0.1f;
		}
	}

	public override void AddRecipes() {
		int amount = ServerConfig.Instance.LowGrind ? 300 : 150;
		CreateRecipe(amount)
			.AddIngredient<EmptyCanister>(amount)
			.AddIngredient(ItemID.Ectoplasm)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
