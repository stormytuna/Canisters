using System.Net.Mime;
using Canisters.Content.Dusts;
using Canisters.Helpers;

namespace Canisters.Content.Projectiles.BlightedCanister;

public class BlightedBolt : ModProjectile
{
	private bool firstFrame = true;

	private Vector2 startLocation;

	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}

	public override void SetDefaults() {
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 200;
		Projectile.extraUpdates = 200;

		Projectile.friendly = true;
		Projectile.penetrate = 5;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		if (firstFrame) {
			startLocation = Projectile.Center;
			firstFrame = false;
		}
	}

	public override bool? CanCutTiles() {
		return false;
	}

	public override void Kill(int timeLeft) {
		DustHelpers.MakeLightningDust(startLocation, Projectile.Center, ModContent.DustType<BlightedDust>(), 1.4f, 30f, 0.4f);
		DustHelpers.MakeDustExplosion(Projectile.Center, 2f, ModContent.DustType<BlightedDust>(), 5, 0f, 8f, 100, 150, 1f, 1.2f, true, true, true);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.CursedInferno, 600);
	}
}
