using Terraria;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.ToxicCanister;

public class ToxicBarb : ModProjectile
{
	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 3;
	}

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.aiStyle = 0;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	private readonly bool firstFrame = false;

	public override void AI() {
		// Randomise our frame
		if (firstFrame) {
			Projectile.frame = Main.rand.Next(0, 3);
		}
	}
}