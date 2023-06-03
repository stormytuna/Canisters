using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Canisters.Helpers.Abstracts;

public abstract class CanisterProjectile : ModProjectile
{
	/// <summary>
	///     Only called on clients
	/// </summary>
	public virtual void Explode() { }

	public virtual void SafeSetDefaults() { }
	public virtual void SafeOnHitNPC() { }
	public virtual bool SafeOnTileCollide(Vector2 oldVelocity) => base.OnTileCollide(oldVelocity);

	public sealed override void SetDefaults() {
		// Base stats
		Projectile.width = 22;
		Projectile.height = 22;
		Projectile.aiStyle = 2;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.penetrate = -1;
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