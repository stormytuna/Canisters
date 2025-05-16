using Canisters.Helpers;
using Terraria.Audio;
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

	private ref float Timer {
		get => ref Projectile.ai[0];
	}

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 20;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}

	public override void SetDefaults() {
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.aiStyle = -1;
		Projectile.extraUpdates = 5;

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
		Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 2f;
		Projectile.rotation = Projectile.velocity.ToRotation() + PiOver2;

		NPC closestNpc = NpcHelpers.FindClosestNpc(50f * 16f, Projectile.Center);
		if (closestNpc is null) {
			return;
		}

		if (Timer >= _bounceTime && Main.myPlayer == Projectile.owner) {
			Timer = 0f;
			_bounceTime = Main.rand.NextFloat(-_bounceTimeVariance, _bounceTimeVariance) + _bounceTimeBase;

			float toTarget = Projectile.AngleTo(closestNpc.Center);
			float newAngle = Projectile.velocity.ToRotation().AngleTowards(toTarget, 1.1f);
			Projectile.velocity = newAngle.ToRotationVector2().RotateRandom(_bounceSpread) * 2f;

			Projectile.netUpdate = true;
		}

		Timer++;
	}

	public override void OnKill(int timeLeft) {
		for (int i = 0; i < Projectile.oldPos.Length; i++) {
			Vector2 position = Projectile.oldPos[i];
			var dust = Dust.NewDustPerfect(position, DustID.GolfPaticle);
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
