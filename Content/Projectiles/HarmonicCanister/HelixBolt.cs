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
		Projectile.width = 8;
		Projectile.height = 8;
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

	public override void AI() {
		// Rotated velocity sine wave motion
		if (firstFrame) {
			if (AI_State == 0f) {
				Projectile.NewProjectile(Terraria.Entity.InheritSource(Entity), Projectile.Center, Projectile.velocity, Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, -1f);
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
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

		for (int i = 0; i < 3; i++) {
			int dustType = AI_State == 1f ? DustID.PinkTorch : DustID.PurpleTorch;
			Dust d = Dust.NewDustPerfect(Projectile.Center, dustType);
			d.scale = Main.rand.NextFloat(1f, 1.3f);
			d.velocity *= 0.5f;
			d.noGravity = true;
		}

		AI_FrameCount++;
	}

	public override void Kill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 4f, AI_State == 1f ? DustID.PinkTorch : DustID.PurpleTorch, 15, 0f, 5f, 0, 0, 1f, 1.3f, true);

		// Sound
		SoundStyle soundStyle = SoundID.Item30 with {
			MaxInstances = 0,
			Volume = 0.2f,
			PitchRange = (-0.4f, -0.1f)
		};
		SoundEngine.PlaySound(soundStyle, Projectile.Center);
	}
}