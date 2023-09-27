using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.ToxicCanister;

/// <summary>
///     Vial of venom canister
/// </summary>
// TODO: Visuals, balancing
public class ToxicCanister : CanisterProjectile
{
	public override string Texture => "Canisters/Content/Items/Canisters/ToxicCanister";

	public override void OnExplode() {
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.CreateExplosion(200, 200);

		// TODO: Make it appear so that it doesn't collide with tiles
		Projectile emitterProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ToxicFogEmitter>(), Projectile.damage, 0f, Projectile.owner);
		emitterProj.originalDamage = Projectile.originalDamage;

		// TODO: Dust explosion
	}
}