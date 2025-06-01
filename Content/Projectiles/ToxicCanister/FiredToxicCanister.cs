using Canisters.Content.Dusts;
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

	protected override void ExplosionEffect() {
		if (Main.myPlayer == Projectile.owner) {
			Projectile.Explode(100, 100);

			for (int i = 0; i < 8; i++) {
				Vector2 velocity = Main.rand.NextVector2Circular(1f, 1f) * Main.rand.NextFloat();
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<ToxicFog>(), int.Max(Projectile.damage / 3, 1), 0f, Main.myPlayer);
			}
		}

		DustHelpers.MakeDustExplosion<ToxicDust>(Projectile.position, 10f, 10);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb with { PitchRange = (0.4f, 0.6f) }, Projectile.Center);
	}
}
