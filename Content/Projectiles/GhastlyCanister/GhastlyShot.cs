using Canisters.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.GhastlyCanister;

public class GhastlyShot : ModProjectile
{
	public override void SetDefaults() {
		// Base stats
		Projectile.width = 18;
		Projectile.height = 18;
		Projectile.aiStyle = -1;
		Projectile.extraUpdates = 1;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		if (Main.rand.NextBool()) {
			Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.BlueFairy);
			dust.alpha = Main.rand.Next(120, 200);
			dust.velocity *= 0.4f;
		}
	}

	public override void Kill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 4f, DustID.BlueFairy, 5, 0f, 1f, 120, 200);
	}
}