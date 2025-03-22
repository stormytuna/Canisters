using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.NaniteCanister;

public class FiredNaniteCanister : FiredCanisterProjectile
{
	public override string Texture {
		get => "Canisters/Content/Items/Canisters/NaniteCanister";
	}

	public override void OnExplode() {
		for (int i = 0; i < 6; i++) {
			Vector2 velocity = Main.rand.NextVector2CircularEdge(5f, 5f) * Main.rand.NextFloat(0.5f, 1f);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity,
				ModContent.ProjectileType<Nanites>(), Projectile.damage, 2f, Projectile.owner);
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Clentaminator_Cyan, Main.rand.Next(30, 40), 0f,
			2.5f, noGravity: true);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.CreateExplosionLegacy(96, 96);
	}
}
