using Terraria.Audio;

namespace Canisters.Content.Projectiles.GhastlyCanister;

public class FiredGhastlyCanister : BaseFiredCanisterProjectile
{
	public override int TimeBeforeGravityAffected { get => 40; }

	public override string Texture {
		get => "Canisters/Content/Items/Canisters/GhastlyCanister";
	}

	public override void PostAI() {
		if (Main.myPlayer != Projectile.owner) {
			return;
		}

		bool nearMouse = Projectile.WithinRange(Main.MouseWorld, 8f * 16f);
		if (HasGravity || nearMouse) {
			Projectile.Kill();
		}
	}

	protected override void ExplosionEffect() {
		if (Main.myPlayer == Projectile.owner) {
			Projectile.Explode(100, 100);

			Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<GhastlyExplosionEmitter>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.DungeonSpirit, 22, 0f, 8f, 50, 120, 1f, 1.5f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.DungeonSpirit, 15, 4f, 14f, 70, 120, 1f, 1.3f, true);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb with { PitchRange = (0.6f, 1f) }, Projectile.Center);
	}
}
