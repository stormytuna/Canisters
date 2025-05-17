using Terraria.DataStructures;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Projectiles.GhastlyCanister;

public class GhastlyExplosionEmitter : ModProjectile
{
	private const float ExplosionEmissionRange = 100f;

	private ref float Timer {
		get => ref Projectile.ai[0];
	}

	/*
	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}
	*/

	public override void SetDefaults() {
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 60;
		Projectile.Opacity = 0.8f;
	}

	public override void OnSpawn(IEntitySource source) {
		Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 8f;
	}

	public override Color? GetAlpha(Color lightColor) {
		return Color.Lerp(Color.White, lightColor, 0.5f);
	}

	public override void AI() {
		if (Projectile.owner == Main.myPlayer && Timer % 6 == 0) {
			Vector2 position = Main.rand.NextVector2Circular(ExplosionEmissionRange, ExplosionEmissionRange) + Projectile.Center;
			var explosionProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), position, Vector2.Zero,
				ModContent.ProjectileType<GhastlyExplosion>(), Projectile.damage, Projectile.knockBack,
				Projectile.owner);
			explosionProj.originalDamage = Projectile.originalDamage;
		}

		Projectile.frameCounter++;
		Projectile.spriteDirection = ((Projectile.frameCounter / 8) % 2 == 0).ToDirectionInt();
		Projectile.rotation = Projectile.velocity.ToRotation() - PiOver2;

		var dust = Dust.NewDustDirect(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.DungeonSpirit);
		dust.noGravity = true;
		dust.velocity *= 3f;

		Timer++;
	}
}
