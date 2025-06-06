﻿using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Projectiles.ToxicCanister;

public class ToxicBarb : ModProjectile
{
	private float _bounceSpread;
	private float _bounceTime;
	private float _bounceTimeBase;
	private float _bounceTimeVariance;
	private float _speed;
	private bool _firstFrame = true;

	private ref float Timer {
		get => ref Projectile.ai[0];
	}

	private ref float LastAngleChange {
		get => ref Projectile.ai[1];
	}

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 20;
		ProjectileID.Sets.TrailingMode[Type] = 0;
		ProjectileID.Sets.CultistIsResistantTo[Type] = true;
	}

	public override void SetDefaults() {
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.aiStyle = -1;
		Projectile.extraUpdates = 5;
		Projectile.timeLeft = 1200;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void OnSpawn(IEntitySource source) {
		_bounceTimeBase = Main.rand.NextFloat(50f, 80f);
		_bounceTime = _bounceTimeBase;
		_bounceTimeVariance = Main.rand.NextFloat(5f, 15f);
		_bounceSpread = Main.rand.NextFloat(0.5f);
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;
			_speed = Projectile.velocity.Length() / Projectile.MaxUpdates;
		}

		Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * _speed;
		Projectile.rotation = Projectile.velocity.ToRotation() + PiOver2;

		NPC closestNpc = NPCHelpers.FindClosestNPC(50f * 16f, Projectile.Center);
		if (closestNpc is null) {
			return;
		}

		if (Timer == 0f) {
			SoundEngine.PlaySound(SoundID.Zombie53 with { Volume = 0.1f + (LastAngleChange / 20f), PitchRange = (0.6f, 1f), MaxInstances = 5, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, Projectile.Center);

			for (int i = 0; i < 3; i++) {
				Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.GolfPaticle);
				dust.color = Color.Lerp(Color.HotPink, Color.Purple, Main.rand.NextFloat());
				dust.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(PiOver2) * Main.rand.NextFloat(-2f, 2f);
				dust.velocity += Projectile.velocity;
				dust.noGravity = true;
			}
		}

		Timer++;

		if (Timer >= _bounceTime && Main.myPlayer == Projectile.owner) {
			Timer = 0f;
			_bounceTime = Main.rand.NextFloat(-_bounceTimeVariance, _bounceTimeVariance) + _bounceTimeBase;

			float toTarget = Projectile.AngleTo(closestNpc.Center);
			float newAngle = Projectile.velocity.ToRotation().AngleTowards(toTarget, 1.1f);
			Projectile.velocity = newAngle.ToRotationVector2().RotateRandom(_bounceSpread) * 2f;

			// Jank, but lets sounds play properly on other clients without custom packet
			LastAngleChange = float.Abs(toTarget - newAngle);

			Projectile.netUpdate = true;
		}
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Projectile.velocity.BounceOffTiles(oldVelocity);

		return false;
	}

	public override void OnKill(int timeLeft) {
		for (int i = 0; i < Projectile.oldPos.Length; i++) {
			Vector2 position = Projectile.oldPos[i];
			Dust dust = Dust.NewDustPerfect(position, DustID.GolfPaticle);
			dust.color = Color.Lerp(Color.HotPink, Color.Purple, i / (float)Projectile.oldPos.Length);
			dust.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(PiOver2) * Main.rand.NextFloat(-2f, 2f);
			dust.velocity += Projectile.velocity;
			dust.noGravity = true;
		}

		SoundEngine.PlaySound(SoundID.DoubleJump with { Pitch = 0.4f, PitchVariance = 0.2f, MaxInstances = 0 }, Projectile.Center);
	}

	public override bool PreDraw(ref Color lightColor) {
		Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

		for (int i = Projectile.oldPos.Length - 1; i >= 0; i--) {
			Vector2 position = Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.height / 2);
			Vector2 tangent = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(PiOver2);
			position += tangent * Main.rand.NextFloat(-3f, 3f);

			Rectangle sourceRect = new(0, 0, texture.Width, texture.Height);
			Vector2 origin = texture.Size() / 2f;

			float lerpAmount = i / (float)Projectile.oldPos.Length;
			Color color = Color.Lerp(Color.HotPink, Color.Purple, lerpAmount) with { A = 0 };
			float scale = Lerp(1f, 0.8f, lerpAmount * lerpAmount);

			Main.EntitySpriteDraw(texture, position, sourceRect, color, Projectile.rotation, origin, scale, SpriteEffects.None);
		}

		return false;
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		// Move hitbox deeper into trail so that it looks nicer when colliding
		projHitbox.X = (int)Projectile.oldPos[6].X;
		projHitbox.Y = (int)Projectile.oldPos[6].Y;
		return projHitbox.Intersects(targetHitbox);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Main.rand.NextBool(3)) {
			target.AddBuff(BuffID.Venom, 120);
		}
	}
}
