using Canisters.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.ToxicCanister;

public class ToxicFogEmitter : ModProjectile
{
	public override string Texture => CanisterHelpers.GetEmptyAssetString();

	public override void SetDefaults() {
		// Base stats
		Projectile.width = Projectile.height = 200;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 200;
		Projectile.penetrate = -1;
	}

	public override void AI() {
		// TODO: Actually emit toxic fog
		for (int i = 0; i < 10; i++) {
			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.DemonTorch);
			d.noGravity = true;
			d.noLight = true;
			d.noLightEmittence = true;
		}
	}
}