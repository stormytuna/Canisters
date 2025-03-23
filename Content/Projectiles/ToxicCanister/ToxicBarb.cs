using System;
using Canisters.Helpers;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Projectiles.ToxicCanister;

public class ToxicBarb : ModProjectile
{
	private const float _timeToBounce = 25f;

	private ref float Timer {
		get => ref Projectile.ai[0];
	}

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.aiStyle = 0;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	private Dust MakeDust(Vector2 position, int width, int height) {
		var dust = Dust.NewDustDirect(position, width, height, DustID.DemonTorch);
		dust.scale = Main.rand.NextFloat(1f, 1.5f);
		dust.noGravity = true;
		dust.noLight = true;
		dust.noLightEmittence = true;
		return dust;
	}

	private void Visuals() {
		Projectile.rotation = Projectile.velocity.ToRotation() + PiOver2;

		if (Main.rand.NextBool()) {
			Dust dust = MakeDust(Projectile.position, Projectile.width, Projectile.height);
			dust.velocity *= Main.rand.NextFloat(0.5f, 2f);
		}
	}

	public override void AI() {
		Visuals();

		NPC closestNpc = NpcHelpers.FindClosestNpc(50f * 16f, Projectile.Center);
		if (closestNpc is null) {
			return;
		}

		float timeUntilBounce = _timeToBounce - Timer;
		Vector2 bouncePosition = Projectile.Center + (Projectile.velocity * timeUntilBounce);
		MakeDust(bouncePosition, 0, 0);

		if (Timer >= _timeToBounce) {
			Timer = 0f;

			Vector2 oldVelocity = Projectile.velocity;
			Projectile.velocity = MathHelpers.RotateTowards(Projectile.velocity, closestNpc.Center, Projectile.Center);

			Vector2 bisection = (oldVelocity + Projectile.velocity).SafeNormalize(Vector2.Zero);
			Vector2 dustDirection = bisection.RotatedBy(3 * PiOver2);
			float strength = Lerp(0.5f, 1f, MathF.Abs(oldVelocity.AngleTo(Projectile.velocity)) / Pi);
			Main.NewText(strength);
			for (int i = 0; i < 10; i++) {
				Dust dust = MakeDust(Projectile.Center, 0, 0);
				dust.velocity = dustDirection.RotatedByRandom(PiOver4) * Main.rand.NextFloat(8f, 16f) * strength;
			}
		}

		Timer++;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(3)) {
			target.AddBuff(BuffID.Venom, 120);
		}
	}
}
