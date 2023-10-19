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

	public override void OnExplode() {
		Projectile explosionProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HarmonicExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		explosionProj.originalDamage = Projectile.originalDamage;

		DustHelpers.MakeDustExplosion(Projectile.Center, 16f, DustID.PinkTorch, 15, 8f, 16f, noGravity: true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 16f, DustID.PurpleTorch, 15, 8f, 16f, noGravity: true);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.CreateExplosion(96, 96);
	}
}