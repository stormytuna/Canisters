using System.Collections.Generic;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.VerdantCanister;

/// <summary>
///     Jungle spore canister
/// </summary>
public class VerdantCanister : CanisterProjectile
{
	public override string Texture => "Canisters/Content/Items/Canisters/VerdantCanister";

	public override void OnExplode() {
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		List<float> startRots = Main.rand.NextSegmentedAngles(4, 0.5f);
		if (Main.myPlayer == Projectile.owner) {
			for (int i = 0; i < 4; i++) {
				int numVines = Main.rand.Next(3, 6);
				float vineRot = Main.rand.NextGaussian(0f, 0.15f);
				float startRot = startRots[i];
				Vector2 offset = Vector2.UnitY.RotatedBy(startRot) * -20f;
				Projectile vine = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + offset, Vector2.Zero, ModContent.ProjectileType<VerdantVine>(), Projectile.damage, Projectile.knockBack / 3f, Projectile.owner, numVines, vineRot);
				vine.rotation = startRot;
			}
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Grass, Main.rand.Next(50, 65), 0f, 15f, 100, 150, 1f, 1.3f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Grass, Main.rand.Next(12, 20), 0f, 10f, 100, 150, 1.3f, 1.6f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Grass, Main.rand.Next(10, 15), 0f, 3f, 100, 150, 1f, 1.3f);
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.GreenFairy, Main.rand.Next(3, 5), 0f, 4f, 0, 50, 0.8f, 1.3f);

		// TODO: leaf gore

		Projectile.CreateExplosion(96, 96);
	}
}