using System;
using Canisters.Helpers;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.HarmonicCanister;

public class HelixBolt : ModProjectile
{
	private bool _firstFrame = true;
	private float _speed;
	private float _startVelocityRotation;

	private ref float Timer {
		get => ref Projectile.ai[0];
	}

	public bool IsLight {
		get => Projectile.ai[1] == 0f;
		set => Projectile.ai[1] = value ? 0f : 1f;
	}

	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}

	private short DustType {
		get => IsLight ? DustID.PinkTorch : DustID.PurpleTorch;
	}

	public override void SetDefaults() {
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.aiStyle = -1;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;
			_startVelocityRotation = Projectile.velocity.ToRotation();
			_speed = Projectile.velocity.Length();
		}

		float radians = Timer / 40f * MathHelper.TwoPi;
		float offset = MathF.Cos(radians) * IsLight.ToDirectionInt() * 0.2f;
		float newRotation = offset + _startVelocityRotation;
		Projectile.velocity = newRotation.ToRotationVector2() * _speed;

		var dust = Dust.NewDustPerfect(Projectile.Center, DustType);
		dust.scale = Main.rand.NextFloat(1f, 1.3f);
		dust.velocity *= 2f;
		dust.noGravity = true;

		Timer++;
	}

	public override void Kill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 4f, DustType, 5, 0f, 5f, 0, 0, 1f, 1.3f, true);

		SoundStyle soundStyle = SoundID.Item30 with { MaxInstances = 0, Volume = 0.2f, PitchRange = (-0.4f, -0.1f) };
		SoundEngine.PlaySound(soundStyle, Projectile.Center);
	}
}
