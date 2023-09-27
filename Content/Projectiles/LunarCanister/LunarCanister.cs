using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.LunarCanister;

public class LunarCanister : CanisterProjectile
{
	public override string Texture => "Canisters/Content/Items/Canisters/LunarCanister";

	public override void OnExplode() {
		Projectile.CreateExplosion(96, 96);

		Projectile emitterProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LunarLightningEmitter>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		emitterProj.originalDamage = Projectile.originalDamage;

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
	}
}