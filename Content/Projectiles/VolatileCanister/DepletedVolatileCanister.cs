using System;
using Canisters.Content.Dusts;
using Canisters.Helpers;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Canisters.Content.Projectiles.VolatileCanister;

public class DepletedVolatileCanister : ModProjectile
{
	private bool HasGravity {
		get => Projectile.ai[0] == 1f;
		set => Projectile.ai[0] = value ? 1f : 0f;
	}

	private ref float Timer {
		get => ref Projectile.ai[1];
	}

	public override void OnSpawn(IEntitySource source) {
		// TODO: Check if this works in mp?
		Projectile.rotation = Main.rand.NextRadian();
	}

	public override void SetDefaults() {
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
		Projectile.alpha = 120;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		Timer++;

		if (Main.rand.NextBool()) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<VolatileCanisterDust>());
			dust.velocity *= 0.4f;
		}

		Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * Projectile.direction;

		if (HasGravity) {
			Projectile.velocity.Y += 0.4f;
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}
		}
		else if (Timer >= 10f) {
			HasGravity = true;
		}
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

		SoundStyle soundStyle = SoundID.SplashWeak with { MaxInstances = 0, Volume = 0.5f, PitchRange = (0.9f, 1f) };
		SoundEngine.PlaySound(soundStyle, Projectile.Center);
	}
}
