using Canisters.Content.Dusts;
using Canisters.Helpers;
using Terraria.Audio;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Projectiles.BlightedCanister;

public class FiredBlightedCanister : BaseFiredCanisterProjectile
{
	public override int TimeBeforeGravityAffected {
		get => 18;
	}

	public override string Texture {
		get => "Canisters/Content/Items/Canisters/BlightedCanister";
	}

	public override void Explode() {
		if (Main.myPlayer == Projectile.owner) {
			float rotationOffset = Main.rand.NextRadian();
			float sign = Main.rand.NextBool().ToDirectionInt();
			for (int i = 0; i < 5; i++) {
				float rot = (i / 5f * TwoPi) + rotationOffset;
				Vector2 velocity = rot.ToRotationVector2() * 3f;
				Vector2 positionOffset = velocity * 2f;
				Vector2 position = Projectile.Center + positionOffset;

				Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<BlightedBall>(), Projectile.damage / 3, Projectile.knockBack / 3f, Projectile.owner, sign);
			}

			Projectile.Explode(100, 100);
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, ModContent.DustType<BlightedDust>(), 15, 0f, 15f, 100, 150, 1f, 1.3f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, ModContent.DustType<BlightedDust>(), 10, 0f, 10f, 100, 150, 1.3f, 1.6f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, ModContent.DustType<BlightedDust>(), 8, 0f, 3f, 100, 150, 1f, 1.3f);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb with { PitchRange = (-0.8f, -0.4f) }, Projectile.Center);
	}
}
