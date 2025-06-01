using Canisters.Content.Dusts;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.NaniteCanister;

public class FiredNaniteCanister : BaseFiredCanisterProjectile
{
	public override string Texture {
		get => "Canisters/Content/Items/Canisters/NaniteCanister";
	}

	protected override void ExplosionEffect() {
		if (Main.myPlayer == Projectile.owner) {
			Projectile.Explode(100, 100);

			for (int i = 0; i < 3; i++) {
				Vector2 velocity = Main.rand.NextVector2CircularEdge(5f, 5f) * Main.rand.NextFloat(0.5f, 1f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<HomingNaniteBolt>(), Projectile.damage / 4, 0f, Projectile.owner);
			}
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, ModContent.DustType<NaniteDust>(), Main.rand.Next(30, 40), 5f, 10f, scale: 1.3f, noGravity: true);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
	}
}
