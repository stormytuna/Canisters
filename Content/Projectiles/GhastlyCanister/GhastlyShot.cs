using Canisters.Helpers;

namespace Canisters.Content.Projectiles.GhastlyCanister;

// TODO: Revisit visuals
public class GhastlyShot : ModProjectile
{
	private int _timeInTiles = 0;

	public override void SetDefaults() {
		Projectile.width = 18;
		Projectile.height = 18;
		Projectile.aiStyle = -1;
		Projectile.extraUpdates = 2;
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		bool inTile = Collision.SolidTiles(Projectile.Center, 1, 1);

		for (int i = 0; i < 2; i++) {
			Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.DungeonSpirit);
			dust.position += Projectile.velocity * 0.6f;
			dust.velocity *= 0.4f;
			dust.noGravity = true;
			dust.alpha = Main.rand.Next(50, 100);
			if (inTile) {
				dust.alpha *= 2;
			}
		}

		if (inTile) {
			_timeInTiles++;
		}

		if (_timeInTiles >= 12 && Main.myPlayer == Projectile.owner) {
			Projectile.Kill();
		}
	}

	public override void OnKill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 4f, DustID.DungeonSpirit, 5, 0f, 1f, 120, 200, noGravity: true);
	}
}
