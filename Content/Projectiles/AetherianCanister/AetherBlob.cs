using Terraria.Audio;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Projectiles.AetherianCanister;

public class AetherBlob : ModProjectile
{
	private ref float Timer {
		get => ref Projectile.ai[0];
	}

	public override void SetDefaults() {
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 90;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 10;
	}

	public override void AI() {
		Timer++;
		if (Timer >= 5f) {
			Projectile.velocity.Y += 0.3f;
			Projectile.velocity *= 0.98f;
		}

		Projectile.rotation = Projectile.velocity.ToRotation() + PiOver2;

		for (int i = 0; i < 3; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PinkTorch);
			dust.velocity += Projectile.velocity * 0.6f;
			dust.velocity *= 0.6f;
			dust.noGravity = true;
		}
	}

	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		width = 4;
		height = 4;

		return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Projectile.velocity.BounceOffTiles(oldVelocity, 0.6f, 0.95f);

		SoundEngine.PlaySound(SoundID.Item56 with { Volume = 0.3f, PitchRange = (0.6f, 1.2f), MaxInstances = 3, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew }, Projectile.Center);

		return false;
	}

	public override void OnKill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 5f, DustID.PinkTorch, 5, 0.5f, 2f, noGravity: true);
		SoundEngine.PlaySound(SoundID.DoubleJump with { Volume = 0.3f, PitchRange = (0.5f, 1f) }, Projectile.Center);
	}

	public override Color? GetAlpha(Color lightColor) {
		return Color.White * 0.6f;
	}
}
