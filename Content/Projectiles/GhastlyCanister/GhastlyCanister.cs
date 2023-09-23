using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.GhastlyCanister;

public class GhastlyCanister : CanisterProjectile
{
	private ref float Timer => ref Projectile.ai[0];

	public override string Texture => "Canisters/Content/Items/Canisters/GhastlyCanister";

	public override void AI() {
		if (Timer >= 30f) {
			Explode();
		}
	}

	public override void Explode() {
		Projectile.CreateExplosion(96, 96);
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile emitterProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GhastlyExplosionEmitter>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		emitterProj.originalDamage = Projectile.originalDamage;

		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.BlueFairy, 22, 0f, 8f, 50, 120, 1f, 1.5f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.BlueFairy, 15, 4f, 14f, 70, 120, 1f, 1.3f, true);
	}
}