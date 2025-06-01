using Canisters.Content.Dusts;

namespace Canisters.Content.Projectiles.ToxicCanister;

public class ToxicFog : ModProjectile
{
	private const int MaxLifeTime = 4 * 60;
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

		Projectile.penetrate = -1;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 20;
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

		foreach (var proj in Main.ActiveProjectiles) {
			if (proj.ModProjectile is not ToxicFog) {
				continue;
			}

			if (Projectile.Hitbox.Intersects(proj.Hitbox)) {
				Vector2 pushAwayVelocity = Projectile.DirectionTo(proj.Center) * 0.004f;
				if (pushAwayVelocity.HasNaNs() || pushAwayVelocity == Vector2.Zero) {
					pushAwayVelocity = new Vector2(1f, -1f)		;
				}
				
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.velocity - pushAwayVelocity, 0.6f);
				proj.velocity = Vector2.Lerp(proj.velocity, proj.velocity + pushAwayVelocity, 0.6f);
			}
		}
		
		Vector2 testPosition = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * 16f;
		if (Collision.IsWorldPointSolid(testPosition, true)) {
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.velocity - Projectile.velocity.SafeNormalize(Vector2.Zero), 0.5f);
		}

		if (Main.rand.NextBool(10)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ToxicDust>());
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

	public override bool? CanHitNPC(NPC target) {
		if (!CollisionHelpers.CanHit(target, Projectile.Center)) {
			return false;
		}
		
		return base.CanHitNPC(target);
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Projectile.velocity.BounceOffTiles(oldVelocity); 
		
		return false;
	}

	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		width = 20;
		height = 20;

		return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}
}
