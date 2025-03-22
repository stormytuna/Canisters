using Canisters.Content.Dusts;
using Canisters.Helpers;
using Terraria.GameContent;

namespace Canisters.Content.Projectiles.BlightedCanister;

public class BlightedBall : ModProjectile
{
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 10;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}

	public override void SetDefaults() {
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.extraUpdates = 2;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 90;

		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		if (Main.rand.NextBool()) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<BlightedDust>(), Scale: Main.rand.NextFloat(1f, 1.2f));
			dust.noGravity = true;
			dust.noLight = true;
		}

		Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0] * 0.07f);
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
	}

	public override Color? GetAlpha(Color lightColor) {
		return Color.White;
	}

	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		width = 8;
		height = 8;

		return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.CursedInferno, 7 * 60);
	}

	public override void Kill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, ModContent.DustType<BlightedDust>(), 5, 0f, 15f, 100, 150, 1f, 1.3f, true, true, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, ModContent.DustType<BlightedDust>(), 3, 0f, 10f, 100, 150, 1.3f, 1.6f, true, true, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, ModContent.DustType<BlightedDust>(), 2, 0f, 3f, 100, 150, 1f, 1.3f, true, true);
	}

	public override bool PreDraw(ref Color lightColor) {
		Texture2D texture = TextureAssets.Projectile[Type].Value;

		for (int i = Projectile.oldPos.Length - 1; i >= 1; i -= 2) {
			Vector2 position = Projectile.oldPos[i] - Main.screenPosition +new Vector2(Projectile.width / 2, Projectile.height / 2);
			Rectangle sourceRect = new(0, 0, texture.Width, texture.Height);
			Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - (float)i) / Projectile.oldPos.Length);
			Vector2 origin = texture.Size() / 2f;
			Main.EntitySpriteDraw(texture, position, sourceRect, color, Projectile.rotation, origin, Projectile.scale,SpriteEffects.None);
		}

		return true;
	}
}
