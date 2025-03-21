using Canisters.Helpers.Abstracts;

namespace Canisters.Content.Projectiles.AetherianCanister;

public class FiredAetherianCanister : FiredCanisterProjectile
{
	public override string Texture {
		get => "Canisters/Content/Items/Canisters/AetherianCanister";
	}

	public override bool SafeOnTileCollide(Vector2 oldVelocity) {
		// TODO: Bounce
		return false;
	}
}
