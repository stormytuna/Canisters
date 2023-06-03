using System;
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

	public override void Explode() {
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		for (int i = 0; i < 4; i++) {
			Vector2 velocity = new(Main.rand.NextFloat(0.8f, 3f), Main.rand.NextFloat(0.8f, 1.5f));
			velocity *= Main.rand.NextBool() ? -1f : 1f;
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<GlisteningBlob>(), Projectile.damage / 3, Projectile.knockBack / 3f, Projectile.owner);
		}

		// Ichor dust
		for (int i = 0; i < 20; i++) {
			Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ichor, Alpha: Main.rand.Next(90, 150), Scale: Main.rand.NextFloat(0.8f, 1.5f));
			dust.velocity = velocity;
			dust.noGravity = true;
		}

		// More dust 
		for (int i = 0; i < 90; i++) {
			// Our base dust properties
			Vector2 velocity = Main.rand.NextVector2Circular(15f, 15f);
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IchorTorch, Alpha: Main.rand.Next(100, 150), Scale: Main.rand.NextFloat(0.8f, 1.2f));
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

		Projectile.TurnToExplosion(96, 96);
	}
}