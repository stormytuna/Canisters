using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.VerdantCanister;

/// <summary>
///     Jungle spore canister
/// </summary>
public class VerdantCanister : ModProjectile
{
	public override void SetDefaults() {
		// Base stats
		Projectile.width = 22;
		Projectile.height = 22;
		Projectile.aiStyle = 2;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override string Texture => "Canisters/Content/Items/Canisters/VerdantCanister";

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Projectile.alpha != 255) {
			Explode();
		}
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (Projectile.alpha != 255) {
			Explode();
			return false;
		}

		return base.OnTileCollide(oldVelocity);
	}

	private void Explode() {
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

		// Leafy dust
		for (int i = 0; i < 80; i++) {
			// Our base dust properties
			Vector2 velocity = Main.rand.NextVector2Circular(15f, 15f);
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, Alpha: Main.rand.Next(100, 150), Scale: Main.rand.NextFloat(0.8f, 1.2f));
			dust.velocity = velocity;
			dust.noGravity = true;

			if (Main.rand.NextBool(3)) {
				// 1/3 dust becomes medium dust
				float sizeMult = Main.rand.NextFloat(1f, 1.5f);
				dust.scale *= sizeMult;
				dust.velocity /= sizeMult;
			} else if (Main.rand.NextBool(4)) {
				// 1/4 of the rest become little grass that's gravity effected
				dust.velocity.X /= 4f;
				dust.velocity.Y = MathF.Abs(dust.velocity.Y) / -4f;
				dust.noGravity = false;
			}
		}

		// Nature energy dust
		for (int i = 0; i < 15; i++) {
			Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenFairy, Alpha: Main.rand.Next(0, 50), Scale: Main.rand.NextFloat(0.8f, 1.5f));
			dust.velocity = velocity;
		}

		// TODO: leaf gore

		Projectile.TurnToExplosion(96, 96);
	}
}