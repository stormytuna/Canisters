using Canisters.Helpers;

namespace Canisters.Content.Projectiles.GhastlyCanister;

public class GhastlyExplosionEmitter : ModProjectile
{
	private const float _explosionEmissionRange = 100f;

	private ref float Timer {
		get => ref Projectile.ai[0];
	}

	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 60;
		Projectile.penetrate = -1;
	}

	public override void AI() {
		if (Projectile.owner == Main.myPlayer && Timer % 6 == 0) {
			Vector2 position = Main.rand.NextVector2Circular(_explosionEmissionRange, _explosionEmissionRange) +
			                   Projectile.Center;
			var explosionProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), position, Vector2.Zero,
				ModContent.ProjectileType<GhastlyExplosion>(), Projectile.damage, Projectile.knockBack,
				Projectile.owner);
			explosionProj.originalDamage = Projectile.originalDamage;
		}

		Timer++;
	}
}
