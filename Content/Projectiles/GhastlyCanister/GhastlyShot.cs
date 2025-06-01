namespace Canisters.Content.Projectiles.GhastlyCanister;

public class GhastlyShot : ModProjectile
{
	private int _timeInTiles = 0;

	public override void SetDefaults() {
		Projectile.width = 18;
		Projectile.height = 18;
		Projectile.aiStyle = -1;
		Projectile.extraUpdates = 2;
		Projectile.timeLeft = 3 * 60;
		Projectile.tileCollide = false;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = -1;
	}

	public override void AI() {
		bool inTile = Collision.SolidTiles(Projectile.Center, 1, 1);

		int numDust = inTile ? 3 : 6;
		for (int i = 0; i < numDust; i++) {
			Vector2 position = Projectile.Center + ((Projectile.velocity / numDust) * i);
			Dust dust = Dust.NewDustPerfect(position, DustID.DungeonSpirit);
			dust.position += Projectile.velocity * Main.rand.NextFloat(0.5f, 1f);
			dust.velocity *= inTile ? 0.01f : 0.1f;
			dust.noGravity = true;
			dust.noLight = true;
		}

		if (inTile) {
			_timeInTiles++;
		}

		if (_timeInTiles >= 40 && Main.myPlayer == Projectile.owner) {
			Projectile.Kill();
		}
	}

	public override void OnKill(int timeLeft) {
		// Adding velocity to position so our trail seems to line up properly
		// Our trail actually goes in front of the projectile
		DustHelpers.MakeDustExplosion(Projectile.Center + Projectile.velocity, 4f, DustID.DungeonSpirit, 5, 0f, 1f, 120, 200, noGravity: true);
	}
}
