using Terraria.DataStructures;
using Terraria.GameContent;

namespace Canisters.Content.Projectiles;

public class PrismaticAnnihilationStar : ModProjectile
{
	private bool _firstFrame = true;
	private Color _starColor;

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 2;
		ProjectileID.Sets.TrailCacheLength[Type] = 8;
	}

	public override void SetDefaults() {
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.extraUpdates = 1;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = -1;
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;

			float hue = ((float)Main.timeForVisualEffects / 120f) % 1f;
			_starColor = Color.Lerp(Main.hslToRgb(hue, 1f, 0.5f), Color.White, 0.3f) with { A = 50 };
			Projectile.rotation = Main.rand.NextRadian();
		}

		Projectile.direction = (Projectile.velocity.X > 0f).ToDirectionInt();
		Projectile.rotation += 0.04f * Projectile.direction;
	}

	public override bool PreDraw(ref Color lightColor) {
		Texture2D texture = TextureAssets.Projectile[Type].Value;

		Vector2 position = (Projectile.Center - Main.screenPosition).Floor();
		Vector2 origin = texture.Size() / 2f;

		DrawData data = new() {
			texture = texture,
			position = position,
			sourceRect = texture.Frame(),
			color = _starColor,
			rotation = Projectile.rotation,
			scale = new Vector2(Projectile.scale * 0.8f),
			origin = origin,
		};

		for (int i = Projectile.oldPos.Length - 1; i >= 0; i--) {
			float progress = i / (float)ProjectileID.Sets.TrailCacheLength[Type];

			Vector2 trailPos = (Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2f).Floor();
			float trailRot = Projectile.oldRot[i];

			DrawData trailData = data with {
				position = trailPos,
				rotation = trailRot,
				scale = new Vector2(float.Lerp(Projectile.scale, 0.6f, progress * progress)),
				color = Color.Lerp(_starColor, Color.Transparent, progress * progress),
			};
			trailData.Draw(Main.spriteBatch);
		}

		data.Draw(Main.spriteBatch);

		return false;
	}

	public override void OnKill(int timeLeft) {
		for (int i = 0; i < 5; i++) {
			Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.GolfPaticle);
			dust.color = _starColor;
			dust.velocity = Main.rand.NextVector2Circular(3f, 3f);
			dust.noGravity = true;
		}
	}

	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		width = 8;
		height = 8;
		return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}
}
