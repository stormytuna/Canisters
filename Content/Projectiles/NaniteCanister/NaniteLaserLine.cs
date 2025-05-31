using Canisters.Content.Dusts;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Canisters.Content.Projectiles.NaniteCanister;

public class NaniteLaserLine : ModProjectile
{
	private static Asset<Texture2D> _sourceTexture;

	private Vector2 _topSource;
	private Vector2 _bottomSource;
	private bool _firstFrame = true;
	private Vector2 _tangent;

	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}

	public override void Load() {
		_sourceTexture = Mod.Assets.Request<Texture2D>("Content/Projectiles/NaniteCanister/NaniteLaserLine_Source");
	}

	public override void SetDefaults() {
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;

		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.ignoreWater = true;
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;

			_tangent = Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2);
			_topSource = Projectile.Center + _tangent;
			_bottomSource = Projectile.Center - _tangent;
		}

		float length = _topSource.Distance(_bottomSource);
		if (length < 3f * 16f) {
			_topSource += _tangent * 1.5f;
			_bottomSource -= _tangent * 1.5f;
		}

		if (Main.rand.NextBool(15)) {
			System.Collections.Generic.List<Dust> lightningDusts = DustHelpers.MakeLightningDust(_topSource, _bottomSource, ModContent.DustType<NaniteDust>(), 1.2f, 50f, 0.3f);
			foreach (Dust lightningDust in lightningDusts) {
				lightningDust.customData = Projectile;
			}
		}

		for (float i = Main.rand.NextFloat(10f); i < length; i += 10f) {
			Vector2 offset = _topSource.DirectionTo(_bottomSource) * Main.rand.NextFloat(length);
			Dust dust = Dust.NewDustPerfect(_topSource + offset, ModContent.DustType<NaniteDust>());
			dust.velocity *= 0.2f;
			dust.customData = Projectile;
		}

		_topSource += Projectile.velocity;
		_bottomSource += Projectile.velocity;
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
		Rectangle topHitbox = new((int)_topSource.X - 4, (int)_topSource.Y - 4, 8, 8);
		if (topHitbox.Intersects(targetHitbox)) {
			return true;
		}

		Rectangle bottomHitbox = new((int)_bottomSource.X - 4, (int)_bottomSource.Y - 4, 8, 8);
		if (bottomHitbox.Intersects(targetHitbox)) {
			return true;
		}

		return base.Colliding(projHitbox, targetHitbox);
	}

	public override void OnKill(int timeLeft) {
		DustHelpers.MakeDustExplosion(_topSource, 4f, ModContent.DustType<NaniteDust>(), 4, 1f, 2f, noGravity: true);
		DustHelpers.MakeDustExplosion(_bottomSource, 4f, ModContent.DustType<NaniteDust>(), 4, 1f, 2f, noGravity: true);
		SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap with { Volume = 0.8f, PitchRange = (0.5f, 0.8f) }, Projectile.Center);
	}

	public override bool PreDraw(ref Color lightColor) {
		DrawData topDrawData = new() {
			texture = _sourceTexture.Value,
			position = _topSource - Main.screenPosition,
			sourceRect = _sourceTexture.Value.Frame(),
			color = Projectile.GetAlpha(lightColor),
			rotation = Projectile.rotation,
			scale = new Vector2(Projectile.scale),
			origin = _sourceTexture.Value.Size() / 2f
		};

		DrawData bottomDrawData = topDrawData with {
			position = _bottomSource - Main.screenPosition,
		};

		topDrawData.Draw(Main.spriteBatch);
		bottomDrawData.Draw(Main.spriteBatch);

		return false;
	}
}
