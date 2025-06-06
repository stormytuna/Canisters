using Canisters.Common;
using Canisters.Content.Projectiles.AetherianCanister;
using Canisters.DataStructures;
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
		get => new Color(227, 134, 224, 100);
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
			stats.Damage = (int)(stats.Damage / 3.5f);
			stats.Knockback = 0f;
			stats.ProjectileCount *= 6;
			
			if (Main.LocalPlayer.GetModPlayer<CanisterModifiersPlayer>().CanisterLaunchedExplosionRadiusMult > 1f) {
				stats.ProjectileCount++;
			}
			
			if (Main.LocalPlayer.GetModPlayer<CanisterModifiersPlayer>().CanisterLaunchedExplosionRadiusMult > 1.5f) {
				stats.ProjectileCount++;
			}
			
			stats.TotalSpread += 0.7f;
		}
	}
}
