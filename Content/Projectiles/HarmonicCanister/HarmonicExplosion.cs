using Canisters.Helpers;

namespace Canisters.Content.Projectiles.HarmonicCanister;

public class HarmonicExplosion : ModProjectile
{
	private Vector2 dustAxis;
	private int frameCounter;

	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}

	public override void SetDefaults() {
		// Base stats
		Projectile.width = Projectile.height = 96;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 120;

		// Weapon stats
		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		if (frameCounter == 0f) {
			dustAxis = Main.rand.NextVector2Circular(1f, 1f).SafeNormalize(Vector2.Zero);
		}

		// Night explodey dust
		for (int i = 0; i < 10; i++) {
			var d = Dust.NewDustPerfect(Projectile.Center, DustID.PurpleTorch);
			d.velocity = dustAxis.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(2f, 15f);
			d.noGravity = true;
		}

		// Light explodey dust
		for (int i = 0; i < 10; i++) {
			var d = Dust.NewDustPerfect(Projectile.Center, DustID.PinkTorch);
			d.velocity = -dustAxis.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(2f, 15f);
			d.noGravity = true;
		}

		// Central ball dust
		for (int i = 0; i < 4; i++) {
			int dustType = Main.rand.NextBool() ? DustID.PinkTorch : DustID.PurpleTorch;
			Vector2 offset = Main.rand.NextVector2Circular(8f, 8f);
			var d = Dust.NewDustPerfect(Projectile.Center + offset, dustType);
			d.velocity = Vector2.Zero;
			d.noGravity = true;
		}

		// Rotate our dust axis
		dustAxis = dustAxis.RotatedBy(0.04f);

		frameCounter++;
	}

	public override void OnKill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 16f, DustID.PinkTorch, 15, 8f, 16f, noGravity: true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 16f, DustID.PurpleTorch, 15, 8f, 16f, noGravity: true);
	}
}
