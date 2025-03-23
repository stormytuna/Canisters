using Canisters.Helpers;
using Canisters.Helpers._Legacy.Abstracts;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.HarmonicCanister;

public class FiredHarmonicCanister : FiredCanisterProjectile
{
	public override int TimeBeforeGravityAffected {
		get => 35;
	}

	public override string Texture {
		get => "Canisters/Content/Items/Canisters/HarmonicCanister";
	}

	public override void SafeSetDefaults() {
		Projectile.penetrate = -1;
	}

	public override void OnExplode() {
		Projectile.Explode(100, 100);
	}

	public override void SafeAI() {
		if (HasGravity && Main.myPlayer == Projectile.owner) {
			TryExplode();
			Projectile.Kill();
		}
	}

	public override bool SafeOnTileCollide(Vector2 oldVelocity) {
		return true;
	}

	public override void ExplosionVisuals(Vector2 position, Vector2 velocity) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 16f, DustID.PinkTorch, 15, 8f, 16f, noGravity: true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 16f, DustID.PurpleTorch, 15, 8f, 16f, noGravity: true);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
	}
}
