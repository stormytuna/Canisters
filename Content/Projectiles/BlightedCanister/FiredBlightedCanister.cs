using System;
using Canisters.Content.Dusts;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Terraria.Audio;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Projectiles.BlightedCanister;

public class FiredBlightedCanister : FiredCanisterProjectile
{
	public override int TimeBeforeGravityAffected {
		get => 18;
	}

	public override string Texture {
		get => "Canisters/Content/Items/Canisters/BlightedCanister";
	}

	public override void OnExplode() {
		float rotationOffset = Main.rand.NextRadian();
		float sign = Main.rand.NextBool().ToDirectionInt();
		for (int i = 0; i < 5; i++) {
			float rot = (i / 5f * TwoPi) + rotationOffset;
			Vector2 velocity = rot.ToRotationVector2() * 3f;
			Vector2 positionOffset = velocity * 2f;
			Vector2 position = Projectile.Center + positionOffset;

			Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<BlightedBall>(), Projectile.damage / 3, Projectile.knockBack / 3f, Projectile.owner, sign);
		}

		Projectile.CreateExplosionLegacy(96, 96);
	}

	public override void ExplosionVisuals(Vector2 position, Vector2 velocity) {
		DustHelpers.MakeDustExplosion(position, 10f, ModContent.DustType<BlightedDust>(), 15, 0f, 15f, 100, 150, 1f, 1.3f, true);
		DustHelpers.MakeDustExplosion(position, 10f, ModContent.DustType<BlightedDust>(), 10, 0f, 10f, 100, 150, 1.3f, 1.6f, true);
		DustHelpers.MakeDustExplosion(position, 10f, ModContent.DustType<BlightedDust>(), 8, 0f, 3f, 100, 150, 1f, 1.3f);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, position);
	}
}
