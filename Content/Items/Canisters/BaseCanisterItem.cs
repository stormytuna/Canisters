using Canisters.Content.Items.Weapons;
using Canisters.DataStructures;
using Canisters.Helpers.Enums;
using Terraria.DataStructures;

namespace Canisters.Content.Items.Canisters;

public abstract class BaseCanisterItem : ModItem
{
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
			_ => 0
		};
	}
}

public class CanisterItemApplyAmmoStats : GlobalProjectile
{
	// Hooking in here since calling ApplyAmmoStats immediately after Projectile.NewProjectile causes them not to be synced
	public override void OnSpawn(Projectile projectile, IEntitySource source) {
	}
}
