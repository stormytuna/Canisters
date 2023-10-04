using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Canisters.Content.Projectiles.VolatileCanister;

/// <summary>
///     Gel canister
/// </summary>
public class VolatileCanister : CanisterProjectile
{
	public override string Texture => "Canisters/Content/Items/Canisters/VolatileCanister";

	public override void OnExplode() {
		DustHelpers.MakeDustExplosion(Projectile.Center, 15f, DustID.Torch, Main.rand.Next(40, 55), 0f, 15f, 0, 0, 0.8f, 1.2f, true, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 13f, DustID.Torch, Main.rand.Next(10, 15), 0f, 10f, 0, 0, 1.2f, 1.5f, true, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Torch, Main.rand.Next(8, 12), 0f, 7f, 0, 0, 1.8f, 2.5f, true, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 20f, DustID.Torch, Main.rand.Next(8, 12), 0f, 4f, 0, 0, 0.8f, 1.2f, noLight: true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 50f, DustID.Smoke, Main.rand.Next(20, 25), 0f, 5f, 0, 0, 1f, 1.5f);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.CreateExplosion(96, 96);
	}
}