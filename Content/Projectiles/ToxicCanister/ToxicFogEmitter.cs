using Canisters.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.ToxicCanister;

public class ToxicFogEmitter : ModProjectile
{
	public override string Texture => CanisterHelpers.GetEmptyAssetString();

	public override void SetDefaults() {
		// Base stats
		Projectile.width = Projectile.height = 100;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 200;
		Projectile.penetrate = -1;
	}

	private ref float Timer => ref Projectile.ai[0];

	public override void AI() {
		if (Timer % 10 == 0 && Timer <= 140f && Projectile.owner == Main.myPlayer) {
			Vector2 offset = Main.rand.NextVector2Circular(50f, 50f);
			Vector2 spawnPosition = Projectile.Center + offset;
			Vector2 spawnVelocity = Main.rand.NextVector2Circular(0.5f, 0.5f);
			Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), spawnPosition, spawnVelocity, ModContent.ProjectileType<ToxicFog>(), 0, 0f, Projectile.owner);
		}

		for (int i = 0; i < 2; i++) {
			Vector2 offset = Main.rand.NextVector2Circular(50f, 50f);
			Vector2 spawnPosition = Projectile.Center + offset;
			Dust d = Dust.NewDustPerfect(spawnPosition, DustID.DemonTorch);
			d.noGravity = true;
			d.noLight = true;
			d.noLightEmittence = true;
		}

		Timer++;
	}
}
