namespace Canisters.Helpers;

public static class ProjectileHelpers
{
	/// <summary>
	///     Creates an explosion using the projectile. Calls <see cref="Projectile.Damage" />, then
	///     <see cref="Projectile.Kill" />.
	/// </summary>
	/// <param name="proj">The projectile</param>
	/// <param name="width">The width of the explosion</param>
	/// <param name="height">The height of the explosion</param>
	/// <param name="kill"></param>
	/// <param name="knockback">The knockback of the explosion, if null, uses the projectile's knockback</param>
	public static void CreateExplosionLegacy(this Projectile proj, int width, int height, bool kill = true, float? knockback = null) {
		proj.penetrate = -1;
		proj.tileCollide = false;
		proj.alpha = 255;
		proj.Resize(width, height);
		proj.knockBack = knockback ?? proj.knockBack;

		proj.Damage();
		if (kill) {
			proj.Kill();
		}
	}

	public static void Explode(this Projectile projectile, int width, int height, int? damage = null, float? knockback = null) {
		if (Main.myPlayer != projectile.owner) {
			return;
		}

		int oldPenetrate = projectile.penetrate;
		bool oldTileCollide = projectile.tileCollide;
		int oldDamage = projectile.damage;
		float oldKnockback = projectile.knockBack;
		var oldSize = projectile.Size.ToPoint();

		projectile.penetrate = -1;
		projectile.tileCollide = false;
		projectile.damage = damage ?? projectile.damage;
		projectile.knockBack = knockback ?? projectile.knockBack;
		projectile.Resize(width, height);

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
	}
}
