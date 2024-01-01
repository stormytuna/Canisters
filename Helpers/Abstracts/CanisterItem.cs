using Canisters.Content.Items.Canisters;
using Canisters.Helpers.Enums;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using VolatileCanisterProjectile = Canisters.Content.Projectiles.VolatileCanister.VolatileCanister;

namespace Canisters.Helpers.Abstracts;

/// <summary>
///     This is an abstract class that sets common values we use on all Canisters
/// </summary>
public abstract class CanisterItem : ModItem
{
	public abstract int LaunchedProjectileType { get; }
	public abstract int DepletedProjectileType { get; }
	public abstract Color CanisterColor { get; }

	public virtual void SafeSetStaticDefaults() { }
	public virtual void SafeSetDefaults() { }
	public virtual void SafePickAmmo(Item weapon, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback) { }

	public virtual void ApplyAmmoStats(bool isLaunched, ref Vector2 velocity, ref Vector2 position, ref int damage, ref float knockBack, ref int amount, ref float spread) { }

	public sealed override void SetStaticDefaults() {
		SafeSetStaticDefaults();

		Item.ResearchUnlockCount = 99;
	}

	public sealed override void SetDefaults() {
		SafeSetDefaults();

		// Base stats
		Item.width = 22;
		Item.height = 22;
		Item.maxStack = 999;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<VolatileCanisterProjectile>();
		Item.DamageType = DamageClass.Ranged;
		Item.ammo = ModContent.ItemType<VolatileCanister>();
	}

	public sealed override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback) {
		if (weapon.ModItem is not CanisterUsingWeapon canisterWeapon) {
			return;
		}

		type = canisterWeapon.CanisterFiringType == CanisterFiringType.Depleted ? DepletedProjectileType : LaunchedProjectileType;
		SafePickAmmo(weapon, player, ref type, ref speed, ref damage, ref knockback);
	}
}
