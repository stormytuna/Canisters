using Canisters.Helpers;

namespace Canisters.Content.Projectiles.LunarCanister;

public class LunarShot : ModProjectile
{
	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 1;
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
	}

	public override void SetDefaults() {
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;

		Projectile.friendly = true;
		Projectile.penetrate = 3;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		Lighting.AddLight(Projectile.Center, CanisterHelpers.GetCanisterColor<Items.Canisters.LunarCanister>().ToVector3());

		for (int i = 0; i < 3; i++) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex);
			dust.velocity += Projectile.velocity;
			dust.velocity *= 0.6f;
			dust.noGravity = true;
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<LunarMark>(), Projectile.damage, 0f, Main.myPlayer);
	}

}
