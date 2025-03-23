namespace Canisters.Content.Projectiles.VerdantCanister;

public class VerdantGas : ModProjectile
{
	private bool _firstFrame = true;

	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 3;
	}

	public override void SetDefaults() {
		Projectile.width = 60;
		Projectile.height = 60;
		Projectile.aiStyle = -1;
		Projectile.alpha = 40;

		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;
			Projectile.scale = 0.5f; // Terraria resizes projectiles if they set scale in SetDefaults
			Projectile.frame = Main.rand.Next(Main.projFrames[Type]);
		}

		Projectile.alpha += 5;
		if (Projectile.alpha >= 255) {
			Projectile.Kill();
		}

		Projectile.scale += 0.03f;

		Projectile.velocity *= 0.95f;

		Projectile.frameCounter++;
		if (Projectile.frameCounter >= 12) {
			Projectile.frameCounter = 0;
			Projectile.frame++;
			if (Projectile.frame >= Main.projFrames[Type]) {
				Projectile.frame = 0;
			}
		}
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		return false;
	}

	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		width = 20;
		height = 20;

		return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}
}
