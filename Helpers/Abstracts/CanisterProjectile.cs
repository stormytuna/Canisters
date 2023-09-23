using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Canisters.Helpers.Abstracts;

public abstract class CanisterProjectile : ModProjectile
{
	/// <summary>
	///     Only called for the owner of the projectile
	/// </summary>
	public virtual void Explode() { }

	public virtual void SafeSetDefaults() { }
	public virtual void SafeOnHitNPC() { }
	public virtual bool SafeOnTileCollide(Vector2 oldVelocity) => false;

	public sealed override void SetDefaults() {
		// Base stats
		Projectile.width = 22;
		Projectile.height = 22;
		Projectile.aiStyle = 2;

		// Weapon stats
		Projectile.penetrate = 1;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public sealed override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Projectile.alpha != 255) {
			Explode();
		}
	}

	public sealed override bool OnTileCollide(Vector2 oldVelocity) {
		if (Projectile.alpha != 255) {
			Explode();
		}

		return SafeOnTileCollide(oldVelocity);
	}
}