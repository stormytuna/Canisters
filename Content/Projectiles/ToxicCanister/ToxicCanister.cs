using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Terraria.Audio;
using Terraria.ID;

namespace Canisters.Content.Projectiles.ToxicCanister;

/// <summary>
///     Vial of venom canister
/// </summary>
// TODO: Actually make this
public class ToxicCanister : CanisterProjectile
{
	public override string Texture => "Canisters/Content/Items/Canisters/ToxicCanister";

	private void Explode() {
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.TurnToExplosion(96, 96);
	}
}