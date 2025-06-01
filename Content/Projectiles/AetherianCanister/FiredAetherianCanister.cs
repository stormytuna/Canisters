using Terraria.Audio;

namespace Canisters.Content.Projectiles.AetherianCanister;

public class FiredAetherianCanister : BaseFiredCanisterProjectile
{
	private int _timer = 0;

	public override string Texture {
		get => "Canisters/Content/Items/Canisters/AetherianCanister";
	}

	public override void SetDefaults() {
		Projectile.DefaultToFiredCanister();
		Projectile.penetrate = -1;
	}

	public override void PostAI() {
		_timer++;

		if (_timer == 12f) {
			Explode(Main.player[Projectile.owner]);
		}

		if (_timer >= 24) {
			Projectile.Kill();
		}
	}

	protected override void ExplosionEffect() {
		if (Main.myPlayer == Projectile.owner) {
			Projectile.Explode(80, 80);
		}

		for (int numDust = 0; numDust < 15; numDust++) {
			Vector2 position = Projectile.Center + Main.rand.NextVector2Circular(20f, 20f);
			Vector2 velocity = Main.rand.NextVector2CircularEdge(6f, 6f) * Main.rand.NextFloat(0.8f, 1.2f);
			Dust dust = Dust.NewDustPerfect(position, DustID.SparkForLightDisc, velocity, 0, Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f), Main.rand.NextFloat(0.6f, 1.4f));
			dust.noGravity = true;
			dust.fadeIn = dust.scale + 0.05f;

			dust = Dust.CloneDust(dust);
			dust.color = Color.White;
			dust.scale -= 0.3f;
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 20f, Main.rand.Next(DustID.Confetti, DustID.Confetti_Yellow + 1), 5, 1f, 3f);

		SoundEngine.PlaySound(SoundID.NPCDeath63 with { PitchRange = (-0.8f, -0.2f), MaxInstances = 0 }, Projectile.Center);
	}
}
