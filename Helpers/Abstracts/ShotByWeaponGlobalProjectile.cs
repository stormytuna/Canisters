using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Canisters.Helpers.Abstracts;

public abstract class ShotByWeaponGlobalProjectile<TWeapon> : GlobalProjectile
	where TWeapon : CanisterUsingWeapon
{
	protected bool ShouldApply { get; set; }

	public override bool InstancePerEntity => true;

	public override void OnSpawn(Projectile projectile, IEntitySource source) {
		bool shotByTWeapon = source is EntitySource_ItemUse_WithAmmo withAmmoSource && withAmmoSource.Item.ModItem is TWeapon;
		bool appliesToParent = source is EntitySource_Parent { Entity: Projectile parentProjectile } && parentProjectile.GetGlobalProjectile(this).ShouldApply;
		ShouldApply = shotByTWeapon || appliesToParent;
	}
}