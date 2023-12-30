using Canisters.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.NaniteCanister;

// TODO: Improve and distinct-ify visuals
public class NaniteMist : ModProjectile
{
	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 3;
	}

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 60;
		Projectile.height = 60;
		Projectile.aiStyle = -1;
		Projectile.alpha = 40;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;

		// First frame
		Projectile.frame = Main.rand.Next(Main.projFrames[Type]);
	}

	public override void AI() {
		// Fade out
		Projectile.alpha += 5;
		if (Projectile.alpha >= 255) {
			Projectile.Kill();
		}

		// Scale up
		Projectile.scale += 0.08f;

		// Velocity slowdown
		Projectile.velocity *= 0.95f;

		// Animate
		Projectile.frameCounter++;
		if (Projectile.frameCounter >= 12) {
			Projectile.frameCounter = 0;
			Projectile.frame++;
			if (Projectile.frame >= Main.projFrames[Type]) {
				Projectile.frame = 0;
			}
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(ModContent.BuffType<Devoured>(), 5 * 60);
	}

	public override bool OnTileCollide(Vector2 oldVelocity) => false;

	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		width = 20;
		height = 20;

		return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}
}
