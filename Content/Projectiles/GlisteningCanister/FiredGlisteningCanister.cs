using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.GlisteningCanister;

public class FiredGlisteningCanister : FiredCanisterProjectile
{
	public override string Texture {
		get => "Canisters/Content/Items/Canisters/GlisteningCanister";
	}

	public override int TimeBeforeGravityAffected {
		get => 12;
	}

	public override void OnExplode() {
		for (int i = 0; i < 3; i++) {
			Vector2 velocity = new(Main.rand.NextFloat(0.1f, 2.5f), Main.rand.NextFloat(1f, 5f));
			velocity *= Main.rand.NextBool().ToDirectionInt();
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<GlisteningBlob>(), Projectile.damage / 3, Projectile.knockBack / 3f, Projectile.owner);
		}

		Projectile.CreateExplosionLegacy(96, 96);
	}

	public override void ExplosionVisuals(Vector2 position, Vector2 velocity) {
		DustHelpers.MakeDustExplosion(position, 8f, DustID.IchorTorch, 20, 0f, 8f, 90, 150, 0.8f, 1.5f, true);
		DustHelpers.MakeDustExplosion(position, 8f, DustID.IchorTorch, Main.rand.Next(50, 65), 0f, 15f, 100,150, 0.8f, 1.2f, true);
		DustHelpers.MakeDustExplosion(position, 8f, DustID.IchorTorch, Main.rand.Next(13, 21), 0f, 10f, 100, 150, 1f, 1.4f, true);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, position);
	}
}
