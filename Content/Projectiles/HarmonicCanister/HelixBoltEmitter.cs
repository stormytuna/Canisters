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

		Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<HelixBolt>(), Projectile.damage, Projectile.knockBack);

		// ai1 == 1f means light projectile to our helix bolt
		Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<HelixBolt>(), Projectile.damage, Projectile.knockBack, ai1: 1f);

		Projectile.Kill();
	}
}
