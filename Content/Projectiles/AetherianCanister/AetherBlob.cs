using System.Reflection;
using Canisters.Helpers;
using Terraria.GameContent;

namespace Canisters.Content.Projectiles.AetherianCanister;

public class AetherBlob : ModProjectile
{
	private ref float Timer {
		get => ref Projectile.ai[0];
	}

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 5;
		ProjectileID.Sets.TrailCacheLength[Type] = 7;
	}

	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}

	public override void SetDefaults() {
		Projectile.width = 6;
		Projectile.height = 6;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 90;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		Timer++;
		if (Timer >= 5f) {
			Projectile.velocity.Y += 0.3f;
			Projectile.velocity *= 0.98f;
		}

		for (int i = 0; i < 5; i++) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PinkTorch);
			dust.velocity += Projectile.velocity;
			dust.velocity *= 0.6f;
			dust.noGravity = true;
		}
	}

	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		width = 4;
		height = 4;
		
		return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (Projectile.velocity.X != oldVelocity.X) {
			Projectile.velocity.X = oldVelocity.X * -0.6f;
		}
		
		if (Projectile.velocity.Y != oldVelocity.Y) {
			Projectile.velocity.Y = oldVelocity.Y * -0.95f;
		}
		
		return false;
	}

	public override bool PreDraw(ref Color lightColor) {
		var texture = TextureAssets.Projectile[Type].Value;
		Vector2 origin = texture.Size() / 2f;

		for (int i = Projectile.oldPos.Length - 1; i >= 0; i--) {
			float progress = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
			Vector2 drawPos = (Projectile.oldPos[i] + origin - Main.screenPosition).Floor();
			float scale = float.Lerp(0.2f, 1f, progress * progress);
			float opacity = float.Lerp(0.5f, 1f, progress * progress);
			// TODO: sync colour to canister?
			Main.spriteBatch.Draw(texture, drawPos, null, Color.Pink with { A = 0 } * opacity, 0f, origin, scale, SpriteEffects.None, 0);
		}
		
		return false;
	}
}
