using Canisters.Content.Dusts;
using Canisters.Helpers;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.ToxicCanister;

public class FiredToxicCanister : BaseFiredCanisterProjectile
{
	public override int TimeBeforeGravityAffected {
		get => 18;
	}

	public override string Texture {
		get => "Canisters/Content/Items/Canisters/ToxicCanister";
	}

	public override void Explode() {
		if (Main.myPlayer == Projectile.owner) {
			Projectile.Explode(100, 100);

			foreach (float angle in Main.rand.NextSegmentedAngles(10, 0.2f)) {
				Vector2 spawnPosition = Projectile.Center + Main.rand.NextVector2Circular(20f, 20f);
				Vector2 velocity = angle.ToRotationVector2() * Main.rand.NextFloat(1.5f, 4f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPosition, velocity, ModContent.ProjectileType<ToxicFog>(), int.Max(Projectile.damage / 10, 1), 0f, Main.myPlayer);
			}
		}

		DustHelpers.MakeDustExplosion<ToxicDust>(Projectile.position, 10f, 10);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb with { PitchRange = (0.4f, 0.6f) }, Projectile.Center);
	}
}
