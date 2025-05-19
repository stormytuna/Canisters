using Canisters.Common;

namespace Canisters.Helpers;

public static class ProjectileHelpers
{
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
