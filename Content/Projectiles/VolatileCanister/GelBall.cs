using System;
using Canisters.Content.Dusts;
using Canisters.Helpers;
using Terraria.Audio;
using Terraria.GameContent;

namespace Canisters.Content.Projectiles.VolatileCanister;

public class GelBall : ModProjectile
{
	private bool HasGravity {
		get => Projectile.ai[0] == 1f;
		set => Projectile.ai[0] = value ? 1f : 0f;
	}

	private ref float Timer {
		get => ref Projectile.ai[1];
	}

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 8;
		ProjectileID.Sets.TrailingMode[Type] = 3;
	}

	public override void SetDefaults() {
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
		Projectile.Opacity = 0.5f;

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

	public override bool PreDraw(ref Color lightColor) {
		Texture2D texture = TextureAssets.Projectile[Type].Value;
		Rectangle sourceRect = texture.Frame();
		Vector2 origin = sourceRect.Size() / 2f;

		for (int i = Projectile.oldPos.Length - 1; i >= 0; i--) {
			Vector2 position = Projectile.oldPos[i] - Main.screenPosition + origin;
			float t = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
			Color color = Projectile.GetAlpha(lightColor) * MathHelper.Lerp(0.5f, 1f, t * t);
			float scale = MathHelper.Lerp(0.5f, 1f, t * t);
			Main.EntitySpriteDraw(texture, position, sourceRect, color, Projectile.rotation, origin, scale, SpriteEffects.None);
		}

		return true;
	}

	public override void OnKill(int timeLeft) {
		foreach (Dust dust in DustHelpers.MakeDustExplosion<VolatileCanisterDust>(Projectile.Center, 5f, 10)) {
			dust.velocity *= Main.rand.NextFloat(1.5f);
		}

		SoundStyle soundStyle = SoundID.SplashWeak with { MaxInstances = 0, Volume = 0.5f, PitchRange = (0.9f, 1f) };
		SoundEngine.PlaySound(soundStyle, Projectile.Center);
	}
}
