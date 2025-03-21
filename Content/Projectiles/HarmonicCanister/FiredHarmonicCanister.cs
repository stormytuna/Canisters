using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.HarmonicCanister;

/// <summary>
///     Soul of night and soul of light canister
/// </summary>
public class FiredHarmonicCanister : FiredCanisterProjectile
{
	public override string Texture {
		get => "Canisters/Content/Items/Canisters/HarmonicCanister";
	}

	public override void OnExplode() {
		var explosionProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center,
			Vector2.Zero, ModContent.ProjectileType<HarmonicExplosion>(), Projectile.damage, Projectile.knockBack,
			Projectile.owner);
		explosionProj.originalDamage = Projectile.originalDamage;

		DustHelpers.MakeDustExplosion(Projectile.Center, 16f, DustID.PinkTorch, 15, 8f, 16f, noGravity: true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 16f, DustID.PurpleTorch, 15, 8f, 16f, noGravity: true);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.CreateExplosion(96, 96);
	}
}
