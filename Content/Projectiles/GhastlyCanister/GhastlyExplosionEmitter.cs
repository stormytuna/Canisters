using Canisters.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.GhastlyCanister;

public class GhastlyExplosionEmitter : ModProjectile
{
	private const float ExplosionEmissionRange = 100f;

	private ref float Timer => ref Projectile.ai[0];

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 60;
		Projectile.penetrate = -1;
	}

	public override string Texture => CanisterHelpers.GetEmptyAssetString();

	public override void AI() {
		if (Projectile.owner == Main.myPlayer && Timer % 6 == 0) {
			Vector2 position = Main.rand.NextVector2Circular(ExplosionEmissionRange, ExplosionEmissionRange) + Projectile.Center;
			Projectile explosionProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), position, Vector2.Zero, ModContent.ProjectileType<GhastlyExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
			explosionProj.originalDamage = Projectile.originalDamage;
		}

		Timer++;
	}
}
