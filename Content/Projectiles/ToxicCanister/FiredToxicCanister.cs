using Canisters.Helpers;
using Canisters.Helpers._Legacy.Abstracts;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.ToxicCanister;

/// <summary>
///     Vial of venom canister
/// </summary>
// TODO: Visuals, balancing
public class FiredToxicCanister : FiredCanisterProjectile
{
	public override string Texture {
		get => "Canisters/Content/Items/Canisters/ToxicCanister";
	}

	public override void OnExplode() {
		// TODO: Make it appear so that it doesn't collide with tiles
		var emitterProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center,
			Vector2.Zero, ModContent.ProjectileType<ToxicFogEmitter>(), Projectile.damage, 0f, Projectile.owner);
		emitterProj.originalDamage = Projectile.originalDamage;

		// TODO: Dust explosion

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.CreateExplosionLegacy(200, 200);
	}
}
