using Terraria.Audio;

namespace Canisters.Content.Projectiles.LunarCanister;

public class FiredLunarCanister : BaseFiredCanisterProjectile
{
	private int _timer = 0;

	public override string Texture {
		get => "Canisters/Content/Items/Canisters/LunarCanister";
	}

	public override int TimeBeforeGravityAffected { get => 12; }

	public override void PostAI() {
		_timer++;

		if (_timer > TimeBeforeGravityAffected) {
			Projectile.Kill();
		}
	}

	protected override void ExplosionEffect() {
		if (Projectile.owner == Main.myPlayer) {
			Projectile.Explode(100, 100);
		
			Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.RotateRandom(0.3f), ModContent.ProjectileType<LunarLightningEmitter>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}

		SoundEngine.PlaySound(SoundID.Zombie103 with { Volume = 0.4f, PitchRange = (0.2f, 0.6f), MaxInstances = 2, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, Projectile.Center);
	}
}
