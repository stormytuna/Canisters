using System;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
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

	private void Explode() {
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		for (int i = 0; i < 80; i++) {
			// Our base dust properties
			Vector2 velocity = Main.rand.NextVector2Circular(15f, 15f);
			Color color = Main.rand.NextBool(10) ? Color.Red : default;
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, newColor: color, Scale: Main.rand.NextFloat(0.8f, 1.2f));
			dust.velocity = velocity;
			dust.noGravity = true;
			dust.noLight = true;

			if (Main.rand.NextBool(6)) {
				// 1/7 dust become large, slow dust
				float sizeMult = Main.rand.NextFloat(3f, 4f);
				dust.scale *= sizeMult;
				dust.velocity /= sizeMult;
			} else if (Main.rand.NextBool(3)) {
				// 1/4 dust becomes medium dust
				float sizeMult = Main.rand.NextFloat(2f, 3f);
				dust.scale *= sizeMult;
				dust.velocity /= sizeMult;
			} else {
				// Rest of dust are little sparks that are gravity affected
				dust.velocity.X /= 4f;
				dust.velocity.Y = MathF.Abs(dust.velocity.Y) / -4f;
				dust.noGravity = false;
			}
		}

		Projectile.TurnToExplosion(96, 96);

		// Smoke dust
		for (int i = 0; i < 20; i++) {
			Vector2 velocity = Main.rand.NextVector2Circular(5f, 5f);
			Dust dust = Dust.NewDustDirect(Projectile.position, 96, 96, DustID.Smoke, Scale: Main.rand.NextFloat(1f, 1.5f));
			dust.velocity = velocity;
			dust.noGravity = true;
		}
	}
}