using Terraria.Audio;

namespace Canisters.Content.Projectiles.LunarCanister;

public class FiredLunarCanister : BaseFiredCanisterProjectile
{
	public override string Texture {
		get => "Canisters/Content/Items/Canisters/LunarCanister";
	}

	public override int TimeBeforeGravityAffected { get => 12; }

	public override void PostAI() {
		if (HasGravity) {
			Projectile.Kill();
		}
	}

	public override void Explode() {
		if (Projectile.owner == Main.myPlayer) {
			Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, ModContent.ProjectileType<LunarLightningEmitter>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
	}
}
