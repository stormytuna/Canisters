using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;

namespace Canisters.Helpers;

public static class ItemHelpers
{
	public static void DefaultToCanister(this Item item, int damage, float shootSpeed, float knockback) {
		item.width = 22;
		item.height = 22;
		item.maxStack = Item.CommonMaxStack;

		item.damage = damage;
		item.shootSpeed = shootSpeed;
		item.knockBack = knockback;

		item.shoot = ModContent.ProjectileType<FiredVolatileCanister>();
		item.DamageType = DamageClass.Ranged;
		item.ammo = ModContent.ItemType<VolatileCanister>();
	}
}
