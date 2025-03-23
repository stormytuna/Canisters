using Canisters.Helpers;
using Canisters.Helpers._Legacy.Abstracts;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.GhastlyCanister;

public class FiredGhastlyCanister : FiredCanisterProjectile
{
	public override string Texture {
		get => "Canisters/Content/Items/Canisters/GhastlyCanister";
	}

	public override void SafeAi() {
		if (Timer >= 30f) {
			TryExplode();
		}
	}

	public override void OnExplode() {
		var emitterProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center,
			Vector2.Zero, ModContent.ProjectileType<GhastlyExplosionEmitter>(), Projectile.damage, Projectile.knockBack,
			Projectile.owner);
		emitterProj.originalDamage = Projectile.originalDamage;

		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.BlueFairy, 22, 0f, 8f, 50, 120, 1f, 1.5f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.BlueFairy, 15, 4f, 14f, 70, 120, 1f, 1.3f, true);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.CreateExplosionLegacy(96, 96);
	}
}
