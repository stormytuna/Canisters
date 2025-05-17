using Canisters.Helpers;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.GhastlyCanister;

public class FiredGhastlyCanister : BaseFiredCanisterProjectile
{
	public override int TimeBeforeGravityAffected { get => 25; }

	public override string Texture {
		get => "Canisters/Content/Items/Canisters/GhastlyCanister";
	}

	public override void PostAI() {
		if (HasGravity && Main.myPlayer == Projectile.owner) {
			Projectile.Kill();
		}
	}

	public override void Explode() {
		if (Main.myPlayer == Projectile.owner) {
			Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<GhastlyExplosionEmitter>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.DungeonSpirit, 22, 0f, 8f, 50, 120, 1f, 1.5f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.DungeonSpirit, 15, 4f, 14f, 70, 120, 1f, 1.3f, true);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
	}
}
