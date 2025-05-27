using Canisters.Common;
using Canisters.Content.Items.Canisters;
using Canisters.Content.Items.Weapons;
using Canisters.Content.Projectiles.VolatileCanister;

namespace Canisters.Helpers;

public static class CanisterHelpers
{
	public static Color GetCanisterColor(int canisterItemId) {
		ModItem modItem = ModContent.GetModItem(canisterItemId);
		if (modItem is BaseCanisterItem canisterItem) {
			return canisterItem.CanisterColor;
		}

		return Color.White;
	}

	public static Color GetCanisterColor(Item item) {
		return GetCanisterColor(item.type);
	}

	public static Color GetCanisterColor<TCanister>() where TCanister : BaseCanisterItem {
		return GetCanisterColor(ModContent.ItemType<TCanister>());
	}


	public static Color GetCanisterColorForHeldWeapon(Player player) {
		if (player.HeldItem.ModItem is BaseCanisterUsingWeapon && player.TryGetWeaponAmmo(player.HeldItem, out int canisterItemId)) {
			return GetCanisterColor(canisterItemId);
		}

		return Color.White;
	}

	public static string GetEmptyAssetString() {
		return $"{nameof(Canisters)}/Assets/Empty";
	}
	
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
	
	public static void Explode(this Projectile projectile, int width, int height, int? damage = null, float? knockback = null) {
		if (Main.myPlayer != projectile.owner) {
			return;
		}

		Player owner = Main.LocalPlayer;
		float sizeMult = owner.GetModPlayer<CanisterModifiersPlayer>().CanisterLaunchedExplosionRadiusMult;
		width = (int)(width * sizeMult);
		height = (int)(height * sizeMult);

		int oldPenetrate = projectile.penetrate;
		bool oldTileCollide = projectile.tileCollide;
		int oldDamage = projectile.damage;
		float oldKnockback = projectile.knockBack;
		Point oldSize = projectile.Size.ToPoint();

		projectile.penetrate = -1;
		projectile.tileCollide = false;
		projectile.damage = damage ?? projectile.damage;
		projectile.knockBack = knockback ?? projectile.knockBack;
		projectile.Resize(width, height);

		projectile.Damage();

		projectile.penetrate = oldPenetrate;
		projectile.tileCollide = oldTileCollide;
		projectile.damage = oldDamage;
		projectile.knockBack = oldKnockback;
		projectile.Resize(oldSize.X, oldSize.Y);
	}

	public static void DefaultToFiredCanister(this Projectile projectile) {
		projectile.width = 22;
		projectile.height = 22;
		projectile.aiStyle = -1;

		projectile.friendly = true;
		projectile.DamageType = DamageClass.Ranged;
		projectile.usesLocalNPCImmunity = true;
		projectile.localNPCHitCooldown = 10;
	}
}
