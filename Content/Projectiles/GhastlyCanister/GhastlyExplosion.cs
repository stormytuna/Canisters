using Canisters.Helpers;

namespace Canisters.Content.Projectiles.GhastlyCanister;

public class GhastlyExplosion : ModProjectile
{
	private const float _lifeTime = 35;

	private bool _firstFrame = true;

	private ref float Timer {
		get => ref Projectile.ai[0];
	}

	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 7;
	}

	public override void SetDefaults() {
		Projectile.width = 96;
		Projectile.height = 96;
		Projectile.aiStyle = -1;

		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
	}
	
	public override Color? GetAlpha(Color lightColor) {
		return Color.Lerp(Color.White, lightColor, 0.5f);
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;
			DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.DungeonSpirit, 14, 0f, 8f, 50, 120, 1f, 1.5f, true);
			DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.DungeonSpirit, 8, 4f, 14f, 70, 120, 1f, 1.3f, true);
		}

		Projectile.frame = (int)(Main.projFrames[Type] * Timer / _lifeTime);

		Projectile.velocity = Vector2.Zero;

		if (Timer >= _lifeTime) {
			Projectile.Kill();
		}

		Timer++;
	}
}
