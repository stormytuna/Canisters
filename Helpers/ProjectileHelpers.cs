using Terraria;

namespace Canisters.Helpers;

public static class ProjectileHelpers
{
	/// <summary>
	///     Creates an explosion using the projectile. Calls <see cref="Projectile.Damage" />, then <see cref="Projectile.Kill" />.
	/// </summary>
	/// <param name="proj">The projectile</param>
	/// <param name="width">The width of the explosion</param>
	/// <param name="height">The height of the explosion</param>
	/// <param name="knockback">The knockback of the explosion, if null, uses the projectile's knockback</param>
	public static void CreateExplosion(this Projectile proj, int width, int height, float? knockback = null) {
		proj.penetrate = -1;
		proj.tileCollide = false;
		proj.alpha = 255;
		proj.Resize(width, height);
		proj.knockBack = knockback ?? proj.knockBack;

		proj.Damage();
		proj.Kill();
	}
}
