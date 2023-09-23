using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.HarmonicCanister;

/// <summary>
///     Soul of night and soul of light canister
/// </summary>
public class HarmonicCanister : CanisterProjectile
{
	public override string Texture => "Canisters/Content/Items/Canisters/HarmonicCanister";

	public override void Explode() {
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.CreateExplosion(96, 96);

		Projectile explosionProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HarmonicExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		explosionProj.originalDamage = Projectile.originalDamage;

		// TODO: Dust explosion
	}
}