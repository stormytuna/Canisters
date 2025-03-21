using Canisters.Content.Projectiles.AetherianCanister;
using Canisters.Helpers.Abstracts;

namespace Canisters.Content.Items.Canisters;

public class AetherianCanister : CanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredAetherianCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<AetherBlob>();
	}

	public override Color CanisterColor {
		get => Color.Purple;
	}

	public override void SafeSetStaticDefaults() {
		ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<VolatileCanister>()] = Type;
	}

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.buyPrice(copper: 75);
		Item.rare = ItemRarityID.Green;

		// Ammo stats
		Item.shootSpeed = 2f;
		Item.damage = 6;
		Item.knockBack = 3f;
	}

	public override void ApplyAmmoStats(bool isLaunched, ref Vector2 velocity, ref Vector2 position, ref int damage,
		ref float knockBack,
		ref int amount, ref float spread) {
		if (isLaunched) {
			return;
		}

		damage /= 6;
		amount *= 6;
		spread += 0.3f;
	}
}
