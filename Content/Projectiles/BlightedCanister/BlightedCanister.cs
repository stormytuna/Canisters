using System;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.BlightedCanister;

/// <summary>
///     Cursed flame canister
/// </summary>
public class BlightedCanister : CanisterProjectile
{
	public override string Texture => "Canisters/Content/Items/Canisters/BlightedCanister";

	public override void OnExplode() {
		float rotationOffset = Main.rand.NextRadian();
		float sign = MathF.Sign(Main.rand.NextFloat(-1f, 1f));
		for (int i = 0; i < 5; i++) {
			float rot = i / 5f * MathHelper.TwoPi + rotationOffset;
			Vector2 velocity = rot.ToRotationVector2() * 3f;
			Vector2 positionOffset = velocity * 2f;
			Vector2 position = Projectile.Center + positionOffset;

			Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<BlightedBall>(), Projectile.damage / 3, Projectile.knockBack / 3f, Projectile.owner, sign);
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.CursedTorch, Main.rand.Next(50, 65), 0f, 15f, 100, 150, 1f, 1.3f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.CursedTorch, Main.rand.Next(12, 20), 0f, 10f, 100, 150, 1.3f, 1.6f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.CursedTorch, Main.rand.Next(10, 15), 0f, 3f, 100, 150, 1f, 1.3f);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.CreateExplosion(96, 96);
	}
}