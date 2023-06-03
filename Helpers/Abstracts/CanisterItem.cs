using Canisters.Content.Items.Canisters;
using Canisters.Helpers.Interfaces;
using Terraria.ModLoader;
using VolatileCanisterProjectile = Canisters.Content.Projectiles.VolatileCanister.VolatileCanister;

namespace Canisters.Helpers.Abstracts;

/// <summary>
///     This is an abstract class that sets common values we use on all Canisters
/// </summary>
public abstract class CanisterItem : ModItem, ICanisterItem
{
	public virtual void SafeSetStaticDefaults() { }
	public virtual void SafeSetDefaults() { }

	public sealed override void SetStaticDefaults() {
		SafeSetStaticDefaults();

		Item.ResearchUnlockCount = 99;
	}

	public override void SetDefaults() {
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