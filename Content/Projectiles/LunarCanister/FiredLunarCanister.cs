using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.LunarCanister;

public class FiredLunarCanister : FiredCanisterProjectile
{
	public override string Texture {
		get => "Canisters/Content/Items/Canisters/LunarCanister";
	}

	public override void OnExplode() {
		var emitterProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center,
			Vector2.Zero, ModContent.ProjectileType<LunarLightningEmitter>(), Projectile.damage, Projectile.knockBack,
			Projectile.owner);
		emitterProj.originalDamage = Projectile.originalDamage;

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.CreateExplosion(96, 96);
	}
}
