using Canisters.Common;
using Canisters.Content.Projectiles.LunarCanister;
using Canisters.DataStructures;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class LunarCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredLunarCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<LunarShot>();
	}

	public override Color CanisterColor {
		get => Color.LightCyan;
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(22, 4f, 4f);
		Item.SetShopValues(ItemRarityColor.Cyan9, Item.buyPrice(silver: 9));
	}

	public override void ApplyAmmoStats(ref CanisterShootStats stats) {
		if (stats.IsLaunched) {
			stats.TotalSpread += 0.3f;
		}
	}

	public override void AddRecipes() {
		int amount = ServerConfig.Instance.LowGrind ? 600 : 300;
		CreateRecipe(amount)
			.AddIngredient<EmptyCanister>(amount)
			.AddIngredient(ItemID.LunarBar)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
