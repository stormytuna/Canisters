using Canisters.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.LunarCanister;

public class LunarExplosion : ModProjectile
{
	private const float LifeTime = 35;

	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 7;
	}

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 96;
		Projectile.height = 96;
		Projectile.aiStyle = -1;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
	}

	private ref float Timer => ref Projectile.ai[0];

	private bool firstFrame = true;

	public override void AI() {
		if (firstFrame) {
			firstFrame = false;
			DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.Vortex, 14, 0f, 8f, 50, 120, 1f, 1.5f, true);
			DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.Vortex, 8, 4f, 14f, 70, 120, 1f, 1.3f, true);
		}

		Projectile.frame = (int)(Main.projFrames[Type] * Timer / LifeTime);

		Projectile.velocity = Vector2.Zero;

		if (Timer >= LifeTime) {
			Projectile.Kill();
		}

		Timer++;
	}
}