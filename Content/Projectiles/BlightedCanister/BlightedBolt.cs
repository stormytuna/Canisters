using Canisters.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.BlightedCanister;

/// <summary>
///     More of a helper projectile, it lets our lightning bolt have pierce
/// </summary>
public class BlightedBolt : ModProjectile
{
	public override void SetDefaults() {
		// Base stats
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 200;
		Projectile.extraUpdates = 200;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.penetrate = 5;
		Projectile.DamageType = DamageClass.Ranged;
	}

	private Vector2 startLocation;
	private bool firstFrame = true;

	public override void AI() {
		if (firstFrame) {
			startLocation = Projectile.Center;
			firstFrame = false;
		}
	}

	public override bool? CanCutTiles() => false;

	public override void Kill(int timeLeft) {
		DustHelpers.MakeLightningDust(startLocation, Projectile.Center, DustID.CursedTorch, 1.4f, 30f, 0.4f);
		DustHelpers.MakeDustExplosion(Projectile.Center, 2f, DustID.CursedTorch, Main.rand.Next(10, 15), 0f, 8f, 100, 150, 1f, 1.2f, true);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.CursedInferno, 600);
	}
}
