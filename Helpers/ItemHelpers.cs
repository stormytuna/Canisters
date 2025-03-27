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

	public static void DefaultToCanisterUsingWeapon(this Item item, int useTime, int useAnim, float shootSpeed, int damage, float knockback) {
		item.useStyle = ItemUseStyleID.Shoot;
		item.useTime = useTime;
		item.useAnimation = useAnim;
		item.autoReuse = true;
		item.noMelee = true;
		item.noUseGraphic = true;
		item.shoot = ModContent.ProjectileType<FiredVolatileCanister>();
		item.shootSpeed = shootSpeed;
		item.damage = damage;
		item.knockBack = knockback;
		item.DamageType = DamageClass.Ranged;
		item.useAmmo = ModContent.ItemType<VolatileCanister>();
	}
}
