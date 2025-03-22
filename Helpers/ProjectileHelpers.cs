﻿namespace Canisters.Helpers;

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
		var hitbox = projectile.Hitbox;
		hitbox.Inflate(width - projectile.width, height - projectile.height);
		
		foreach (var npc in Main.ActiveNPCs) {
			if (hitbox.Intersects(npc.Hitbox)) {
				int direction = float.Sign(projectile.DirectionTo(npc.Center).X);
				npc.SimpleStrikeNPC(damage ?? projectile.damage, direction, false, knockback ?? projectile.knockBack, DamageClass.Ranged, true, Main.LocalPlayer.luck);
			}
		}	
	}
}
