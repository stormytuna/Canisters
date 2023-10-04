using System;
using Canisters.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.HarmonicCanister;

/// <summary>
///     The projectile shot when harmonic canister is depleted
/// </summary>
public class HelixBolt : ModProjectile
{
	public override void SetDefaults() {
		// Base stats
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.aiStyle = -1;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	private ref float AI_FrameCount => ref Projectile.ai[0];

	/// <summary>
	///     1f == dark + 1f multiplier <br />
	///     -1f == light + -1f multiplier
	/// </summary>
	private ref float AI_State => ref Projectile.ai[1];

	private bool firstFrame = true;
	private float startVelocityRotation;
	private float speed;
	private short DustType => AI_State == 1f ? DustID.PinkTorch : DustID.PurpleTorch;

	public override void AI() {
		// Rotated velocity sine wave motion
		if (firstFrame) {
			if (AI_State == 0f) {
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, -1f);
				AI_State = 1f;
			}

			startVelocityRotation = Projectile.velocity.ToRotation();
			speed = Projectile.velocity.Length();
			firstFrame = false;
		}

		// Set our new velocity
		float radians = AI_FrameCount / 40f * MathHelper.TwoPi;
		float offset = MathF.Cos(radians) * AI_State * 0.2f;
		float newRotation = offset + startVelocityRotation;
		Projectile.velocity = newRotation.ToRotationVector2() * speed;

		for (int i = 0; i < 4; i++) {
			Dust dust = Dust.NewDustPerfect(Projectile.Center, DustType);
			dust.scale = Main.rand.NextFloat(1f, 1.3f);
			dust.velocity *= 2f;
			dust.noGravity = true;
		}

		AI_FrameCount++;
	}

	public override void Kill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 4f, DustType, 15, 0f, 5f, 0, 0, 1f, 1.3f, true);

		// Sound
		SoundStyle soundStyle = SoundID.Item30 with {
			MaxInstances = 0,
			Volume = 0.2f,
			PitchRange = (-0.4f, -0.1f)
		};
		SoundEngine.PlaySound(soundStyle, Projectile.Center);
	}
}