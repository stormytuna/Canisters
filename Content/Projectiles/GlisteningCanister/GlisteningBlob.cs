using Canisters.Helpers;

namespace Canisters.Content.Projectiles.GlisteningCanister;

// TODO: Change this effect, kinda don't like it
/// <summary>
///     Bouncy balls that the glistening canister explodes into
/// </summary>
public class GlisteningBlob : ModProjectile
{
	public override void SetDefaults() {
		// Base stats
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.aiStyle = -1;
		Projectile.extraUpdates = 1;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.penetrate = 3;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		// Gravity
		Projectile.velocity.Y += 0.1f;

		// Dust
		if (Main.rand.NextBool(3)) {
			var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IchorTorch,
				Alpha: Main.rand.Next(100, 200), Scale: Main.rand.NextFloat(1f, 1.2f));
			d.noGravity = true;
			d.noLight = true;
		}

		if (Main.rand.NextBool(3)) {
			var d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ichor,
				Alpha: Main.rand.Next(100, 200), Scale: Main.rand.NextFloat(1f, 1.2f));
			d.noGravity = true;
			d.noLight = true;
		}

		// Lighting
		Lighting.AddLight(Projectile.Center, TorchID.Ichor);

		// Rotation
		Projectile.rotation += 0.125f * Projectile.direction;
	}

	public override Color? GetAlpha(Color lightColor) {
		return Color.White;
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (Projectile.velocity.X != oldVelocity.X) {
			Projectile.velocity.X = -oldVelocity.X;
		}

		if (Projectile.velocity.Y != oldVelocity.Y) {
			Projectile.velocity.Y = -3f;
		}

		Projectile.penetrate--;

		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.IchorTorch, 10, 0f, 8f, 100, 150, 1f, 1.2f, true);

		return false;
	}

	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough,
		ref Vector2 hitboxCenterFrac) {
		width = 8;
		height = 8;

		return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.Ichor, 600);
	}
}
