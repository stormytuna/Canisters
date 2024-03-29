﻿using Canisters.Content.Dusts;
using Canisters.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.VolatileCanister;

/// <summary>
///     The gel ball fired from a depleted volatile canister
/// </summary>
public class GelBall : ModProjectile
{
	public override void OnSpawn(IEntitySource source) {
		Projectile.rotation = Main.rand.NextRadian();
	}

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
		Projectile.alpha = 120;
		Projectile.extraUpdates = 1;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.DamageType = DamageClass.Ranged;
	}

	/// <summary>
	///     0f == Unaffected by gravity <br />
	///     1f == Affected by gravity
	/// </summary>
	private ref float AI_State => ref Projectile.ai[0];

	private ref float AI_FrameCounter => ref Projectile.ai[1];

	public override void AI() {
		// Increment our frame counter
		AI_FrameCounter++;

		// Make dust
		if (Main.rand.NextBool()) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<VolatileCanisterDust>());
			dust.velocity *= 0.4f;
			//dust.noGravity = true;
		}

		// Spinny
		Projectile.rotation += 0.08f;

		// State 0 => Unaffected by gravity
		if (AI_State == 0f) {
			if (AI_FrameCounter >= 20f) {
				AI_State = 1f;
			}

			return;
		}

		// State 1 => Affected by gravity
		Projectile.velocity += Vector2.UnitY * 0.15f;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(3)) {
			target.AddBuff(BuffID.Oiled, 180);
		}
	}

	public override void Kill(int timeLeft) {
		foreach (Dust dust in DustHelpers.MakeDustExplosion<VolatileCanisterDust>(Projectile.Center, 5f, 10)) {
			dust.velocity *= Main.rand.NextFloat(1.5f);
		}

		// Tiny sound
		SoundStyle soundStyle = SoundID.SplashWeak with {
			MaxInstances = 0,
			Volume = 0.2f,
			PitchRange = (0.9f, 1f)
		};
		SoundEngine.PlaySound(soundStyle, Projectile.Center);
	}
}
