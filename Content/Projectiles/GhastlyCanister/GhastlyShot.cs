using Canisters.Helpers;

namespace Canisters.Content.Projectiles.GhastlyCanister;

public class GhastlyShot : ModProjectile
{
	public override void SetDefaults() {
		// Base stats
		Projectile.width = 18;
		Projectile.height = 18;
		Projectile.aiStyle = -1;
		Projectile.extraUpdates = 1;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		for (int i = 0; i < 2; i++) {
			var dust = Dust.NewDustPerfect(Projectile.Center, DustID.DungeonSpirit);
			dust.velocity += Projectile.velocity;
			dust.velocity *= 0.4f;
			dust.noGravity = true;
			dust.alpha = Main.rand.Next(120, 200);
		}
	}

	public override void OnKill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 4f, DustID.DungeonSpirit, 5, 0f, 1f, 120, 200, noGravity: true);
	}
}
