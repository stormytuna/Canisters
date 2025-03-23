using Canisters.Content.Items.Weapons;
using Canisters.DataStructures;
using Canisters.Helpers;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;

namespace Canisters.Content.Items.Canisters;

public abstract class BaseCanisterItem : ModItem
{
	// Need it to be able to:
	// Apply changes to Projectile defaults based on which projectile this is
	// Make weapon fire more projectiles in a spread
	
	public abstract int LaunchedProjectileType { get; }
	public abstract int DepletedProjectileType { get; }
	public abstract Color CanisterColor { get; }
	
	public virtual void ModifyProjectile(Projectile projectile, int numInTotalAmount) { }

	public virtual void ApplyAmmoStats(ref CanisterShootStats stats) { }

	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 99;
	}

	public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback) {
		if (weapon.ModItem is not BaseCanisterUsingWeapon canisterWeapon) {
			return;
		}

		type = canisterWeapon.CanisterFiringType switch {
			CanisterFiringType.Depleted => DepletedProjectileType,
			CanisterFiringType.Launched => LaunchedProjectileType,
			_ => 0,
		};
	}
}
