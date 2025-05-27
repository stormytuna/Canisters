using Terraria.Audio;

namespace Canisters.Content.Projectiles.GlisteningCanister;

public class FiredGlisteningCanister : BaseFiredCanisterProjectile
{
	public override string Texture {
		get => "Canisters/Content/Items/Canisters/GlisteningCanister";
	}

	public override int TimeBeforeGravityAffected {
		get => 12;
	}

	protected override void ExplosionEffect() {
		if (Main.myPlayer == Projectile.owner) {
			for (int i = 0; i < 3; i++) {
				Vector2 velocity = new(Main.rand.NextFloat(0.1f, 2.5f), Main.rand.NextFloat(1f, 5f));
				velocity *= Main.rand.NextBool().ToDirectionInt();
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<GlisteningBlob>(), Projectile.damage / 6, 0f, Projectile.owner);
			}

			Projectile.Explode(100, 100);
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.IchorTorch, 20, 0f, 8f, 90, 150, 0.8f, 1.5f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.IchorTorch, Main.rand.Next(50, 65), 0f, 15f, 100, 150, 0.8f, 1.2f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.IchorTorch, Main.rand.Next(13, 21), 0f, 10f, 100, 150, 1f, 1.4f, true);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb with { PitchRange = (-0.4f, 0f) }, Projectile.Center);
	}
}
