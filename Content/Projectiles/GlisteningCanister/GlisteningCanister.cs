using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.GlisteningCanister;

/// <summary>
///     Ichor canister
/// </summary>
public class GlisteningCanister : CanisterProjectile
{
	public override string Texture => "Canisters/Content/Items/Canisters/GlisteningCanister";

	public override void OnExplode() {
		for (int i = 0; i < 4; i++) {
			Vector2 velocity = new(Main.rand.NextFloat(0.8f, 3f), Main.rand.NextFloat(0.8f, 1.5f));
			velocity *= Main.rand.NextBool() ? -1f : 1f;
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<GlisteningBlob>(), Projectile.damage / 3, Projectile.knockBack / 3f, Projectile.owner);
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.IchorTorch, 20, 0f, 8f, 90, 150, 0.8f, 1.5f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.IchorTorch, Main.rand.Next(50, 65), 0f, 15f, 100, 150, 0.8f, 1.2f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.IchorTorch, Main.rand.Next(13, 21), 0f, 10f, 100, 150, 1f, 1.4f, true);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
		Projectile.CreateExplosion(96, 96);
	}
}
