using Canisters.Helpers;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.VolatileCanister;

public class FiredVolatileCanister : BaseFiredCanisterProjectile
{
	public override int TimeBeforeGravityAffected {
		get => 12;
	}

	public override string Texture {
		get => "Canisters/Content/Items/Canisters/VolatileCanister";
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.OnFire, 3 * 60);
	}

	public override void Explode() {
		if (Main.myPlayer == Projectile.owner) {
			Projectile.Explode(100, 100);
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 15f, DustID.Torch, Main.rand.Next(40, 55), 0f, 15f, 0, 0, 0.8f, 1.2f, true, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 13f, DustID.Torch, Main.rand.Next(10, 15), 0f, 10f, 0, 0, 1.2f, 1.5f, true, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Torch, Main.rand.Next(8, 12), 0f, 7f, 0, 0, 1.8f, 2.5f, true, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 20f, DustID.Torch, Main.rand.Next(8, 12), 0f, 4f, 0, 0, 0.8f, 1.2f, noLight: true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 50f, DustID.Smoke, Main.rand.Next(20, 25), 0f, 5f, 0, 0, 1f, 1.5f);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
	}
}
