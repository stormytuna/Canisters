using Terraria.DataStructures;

namespace Canisters.Content.Projectiles.HarmonicCanister;

public class HelixBoltEmitter : ModProjectile
{
	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}

	public override void SetDefaults() {
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
	}

	public override void AI() {
		if (Projectile.owner != Main.myPlayer) {
			return;
		}

		var darkProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<HelixBolt>(), Projectile.damage, Projectile.knockBack);
		if (darkProj.ModProjectile is HelixBolt darkHelixBolt) {
			darkHelixBolt.IsLight = false;
		}

		var lightProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<HelixBolt>(), Projectile.damage, Projectile.knockBack);
		if (lightProj.ModProjectile is HelixBolt lightHelixBolt) {
			lightHelixBolt.IsLight = true;
		}
		
		Projectile.Kill();
	}
}
