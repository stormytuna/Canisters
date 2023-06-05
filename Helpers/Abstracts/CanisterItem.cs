using Canisters.Content.Items.Canisters;
using Microsoft.Xna.Framework;
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

	public virtual void SafeSetStaticDefaults() { }
	public virtual void SafeSetDefaults() { }
	public virtual void ApplyAmmoStats(bool isLaunched, ref Vector2 velocity, ref Vector2 offset, ref Vector2 position, ref int damage, ref float knockBack, ref int amount, ref float spread) { }

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
}