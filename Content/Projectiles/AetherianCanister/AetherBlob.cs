namespace Canisters.Content.Projectiles.AetherianCanister;

public class AetherBlob : ModProjectile
{
	public override string Texture {
		get => $"Terraria/Item_{ItemID.MagicMirror}";
	}

	public override void SetDefaults() {
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	// TODO: revisit visuals, look at other shimmer ammo 
}
