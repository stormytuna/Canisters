using Canisters.Content.Dusts;

namespace Canisters.Content.Projectiles.ToxicCanister;

public class ToxicFog : ModProjectile
{
	private const int MaxLifeTime = 2 * 60;
	private int animationSpeed = 12;

	private bool firstFrame = true;

	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 3;
	}

	public override void SetDefaults() {
		Projectile.width = 60;
		Projectile.height = 60;
		Projectile.aiStyle = -1;
		Projectile.alpha = 200;
		Projectile.timeLeft = MaxLifeTime;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.penetrate = -1;

		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 8;
	}

	public override void AI() {
		if (firstFrame) {
			firstFrame = false;
			Projectile.frame = Main.rand.Next(Main.projFrames[Type]);
			animationSpeed = Main.rand.Next(10, 15);
		}

		Projectile.Opacity = MathHelper.Lerp(0f, 0.5f, Projectile.timeLeft / (float)MaxLifeTime);

		Projectile.scale += 0.01f;

		Projectile.velocity *= 0.95f;

		if (Main.rand.NextBool(10)) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ToxicDust>());
			dust.alpha = Main.rand.Next(Projectile.alpha - 20, Projectile.alpha + 20);
			dust.velocity *= Projectile.Opacity;
		}

		Projectile.frameCounter++;
		if (Projectile.frameCounter >= animationSpeed) {
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
