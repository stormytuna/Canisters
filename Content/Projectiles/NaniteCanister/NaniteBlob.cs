using Canisters.Content.Dusts;
using Canisters.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.NaniteCanister;

public class NaniteBlob : ModProjectile
{
	public override void SetDefaults() {
		// Base stats
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.aiStyle = 2;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.DamageType = DamageClass.Ranged;
	}

	private bool firstFrame = true;

	public override void AI() {
		if (!firstFrame) {
			return;
		}

		firstFrame = false;
		int numDust = Main.rand.Next(25, 30);
		for (int i = 0; i < numDust; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<NaniteDust>());
			dust.customData = Projectile;
		}
	}

	public override void Kill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 15f, DustID.Clentaminator_Red, 15, 0f, 5f, 0, 0, 1f, 1.3f, true);

		if (Projectile.owner == Main.myPlayer) {
			for (int i = 0; i < 3; i++) {
				Vector2 velocity = Main.rand.NextVector2CircularEdge(5f, 5f) * Main.rand.NextFloat(0.5f, 1f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<Nanites>(), Projectile.damage, 2f, Projectile.owner);
			}
		}
	}
}