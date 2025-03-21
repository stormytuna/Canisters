using System;

namespace Canisters.Helpers.Abstracts;

public abstract class FiredCanisterProjectile : ModProjectile
{
	public virtual int TimeBeforeGravityAffected {
		get => 20;
	}

	private bool HasGravity {
		get => Projectile.ai[1] == 1f;
		set => Projectile.ai[1] = value ? 1f : 0f;
	}

	public ref float Timer {
		get => ref Projectile.ai[0];
	}

	/// <summary>
	///     Only called for the owner of the projectile
	/// </summary>
	public virtual void OnExplode() { }

	public virtual void SafeAI() { }

	public virtual bool SafeOnTileCollide(Vector2 oldVelocity) {
		return false;
	}

	public sealed override void SetDefaults() {
		Projectile.width = 22;
		Projectile.height = 22;
		Projectile.aiStyle = -1;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public sealed override void AI() {
		SafeAI();

		Timer++;

		Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * Projectile.direction;

		if (HasGravity) {
			Projectile.velocity.Y += 0.4f;
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}

			Projectile.velocity.X *= 0.97f;
		}
		else if (Timer >= TimeBeforeGravityAffected) {
			HasGravity = true;
		}
	}

	public sealed override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		TryExplode();
	}

	public sealed override bool OnTileCollide(Vector2 oldVelocity) {
		TryExplode();
		return SafeOnTileCollide(oldVelocity);
	}

	protected void TryExplode() {
		// TODO: Decouple this from projectile alpha, just use a bool field?
		if (Projectile.owner != Main.myPlayer || Projectile.alpha == 255) {
			return;
		}

		MakeCanisterGore();
		OnExplode();
	}

	private void MakeCanisterGore() {
		var canisterGore1 = Mod.Find<ModGore>("BrokenCanister_01");
		var canisterGore2 = Mod.Find<ModGore>("BrokenCanister_02");
		Vector2 canisterVelocity = Main.rand.NextVector2Circular(3f, 3f);
		var gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center,
			canisterVelocity + (Projectile.velocity * 0.5f), canisterGore1.Type);
		gore.timeLeft = 120;
		gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center,
			-canisterVelocity + (Projectile.velocity * 0.5f), canisterGore2.Type);
		gore.timeLeft = 120;
	}
}
