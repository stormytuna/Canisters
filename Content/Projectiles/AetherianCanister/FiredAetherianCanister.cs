using Canisters.Helpers;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.AetherianCanister;

public class FiredAetherianCanister : BaseFiredCanisterProjectile
{
	public override string Texture {
		get => "Canisters/Content/Items/Canisters/AetherianCanister";
	}

	public override void PostAI() {
		if (Timer >= 20) {
			Projectile.Kill();
		}
	}

	public override void Explode() {
		if (Main.myPlayer == Projectile.owner) {
			Projectile.Explode(60, 60);
		}

		for (int numDust = 0; numDust < 7; numDust++) {
			Vector2 position = Projectile.Center + Main.rand.NextVector2Circular(20f, 20f);
			Vector2 velocity = Main.rand.NextVector2Circular(6f, 6f);
			Dust dust = Dust.NewDustPerfect(position, DustID.SparkForLightDisc, velocity, 0, Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f), Main.rand.NextFloat(0.6f, 1.4f));
			dust.noGravity = true;
			dust.fadeIn = dust.scale + 0.05f;

			dust = Dust.CloneDust(dust);
			dust.color = Color.White;
			dust.scale -= 0.3f;
		}

		// TODO: come back and fix this, party popper effect would be cool?
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
	}
}
