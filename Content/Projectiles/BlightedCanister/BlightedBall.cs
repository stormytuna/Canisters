using Canisters.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.BlightedCanister;

/// <summary>
///     Balls that the blighted canister releases when it explodes
/// </summary>
public class BlightedBall : ModProjectile
{
	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailCacheLength[Type] = 10;
		ProjectileID.Sets.TrailingMode[Type] = 0;
	}

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 20;
		Projectile.height = 20;
		Projectile.aiStyle = -1;
		Projectile.extraUpdates = 2;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 90;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		if (Main.rand.NextBool()) {
			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch, Scale: Main.rand.NextFloat(1f, 1.2f));
			d.noGravity = true;
			d.noLight = true;
		}

		Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0] * 0.07f);
		Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
	}

	public override Color? GetAlpha(Color lightColor) => Color.White;

	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		width = 8;
		height = 8;

		return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.CursedInferno, 600);
	}

	public override void Kill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.CursedTorch, Main.rand.Next(12, 15), 0f, 15f, 100, 150, 1f, 1.3f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.CursedTorch, Main.rand.Next(3, 6), 0f, 10f, 100, 150, 1.3f, 1.6f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.CursedTorch, Main.rand.Next(3, 6), 0f, 3f, 100, 150, 1f, 1.3f);
	}

	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;

		// Draw afterimages
		for (int i = 1; i < Projectile.oldPos.Length; i += 2) {
			Vector2 position = Projectile.oldPos[i] - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.height / 2);
			Rectangle sourceRect = new(0, 0, texture.Width, texture.Height);
			Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - (float)i) / Projectile.oldPos.Length);
			Vector2 origin = texture.Size() / 2f;
			Main.EntitySpriteDraw(texture, position, sourceRect, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);
		}

		return true;
	}
}
