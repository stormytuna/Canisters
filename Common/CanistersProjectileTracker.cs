using Canisters.Content.Items.Weapons;
using Canisters.DataStructures;
using Terraria.DataStructures;

namespace Canisters.Common;

public class CanistersProjectileTracker : GlobalProjectile
{
	public bool IsDepletedCanisterProjectile { get; private set; }
	public bool IsLaunchedCanisterProjectile { get; private set; }

	public override bool InstancePerEntity {
		get => true;
	}

	public override void OnSpawn(Projectile projectile, IEntitySource source) {
		if (source is EntitySource_ItemUse_WithAmmo { Item.ModItem: BaseCanisterUsingWeapon canisterWeapon }) {
			IsDepletedCanisterProjectile = canisterWeapon.CanisterFiringType == CanisterFiringType.Depleted;
			IsLaunchedCanisterProjectile = canisterWeapon.CanisterFiringType == CanisterFiringType.Launched;
		}

		if (source is EntitySource_Parent { Entity: Projectile parentProjectile }) {
			IsDepletedCanisterProjectile = parentProjectile.GetGlobalProjectile(this).IsDepletedCanisterProjectile;
			IsLaunchedCanisterProjectile = parentProjectile.GetGlobalProjectile(this).IsLaunchedCanisterProjectile;
		}
	}
}
