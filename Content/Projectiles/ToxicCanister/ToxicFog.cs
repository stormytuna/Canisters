namespace Canisters.Content.Projectiles.ToxicCanister;

public class ToxicFog : ModProjectile
{
	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 3;
	}

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 60;
		Projectile.height = 60;
		Projectile.aiStyle = -1;
		Projectile.alpha = 40;
		Projectile.penetrate = -1;
		Projectile.alpha = 120;

		// First frame
		Projectile.frame = Main.rand.Next(Main.projFrames[Type]);
	}

	public override void AI() {
		// Fade out
		Projectile.alpha++;
		if (Projectile.alpha >= 255) {
			Projectile.Kill();
		}

		// Scale up
		Projectile.scale += 0.01f;

		// Velocity slowdown
		Projectile.velocity *= 0.99f;

		// Animate
		Projectile.frameCounter++;
		if (Projectile.frameCounter >= 12) {
			Projectile.frameCounter = 0;
			Projectile.frame++;
			if (Projectile.frame >= Main.projFrames[Type]) {
				Projectile.frame = 0;
			}
		}
	}

	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough,
		ref Vector2 hitboxCenterFrac) {
		width = 20;
		height = 20;

		return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}
}
