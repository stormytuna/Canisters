using Canisters.Content.Projectiles.AetherianCanister;
using Canisters.DataStructures;
using Canisters.Helpers;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class AetherianCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<AetherBlob>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<FiredAetherianCanister>();
	}

	public override Color CanisterColor {
		get => Color.Purple;
	}

	public override void SetStaticDefaults() {
		ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<VolatileCanister>()] = Type;
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(7, 3f, 0f);
		Item.SetShopValues(ItemRarityColor.Green2, Item.buyPrice(copper: 75));
	}

	public override void ApplyAmmoStats(ref CanisterShootStats stats) {
		if (stats.IsLaunched) {
			stats.Damage /= 5;
			stats.Knockback /= 5f;
			stats.ProjectileCount *= 6;
			stats.TotalSpread += 0.7f;
		}
	}
}
