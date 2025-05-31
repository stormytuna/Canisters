using Terraria.Audio;

namespace Canisters.Content.Projectiles.HarmonicCanister;

public class FiredHarmonicCanister : BaseFiredCanisterProjectile
{
	private int _timer = 0;
	
	public override int TimeBeforeGravityAffected {
		get => 35;
	}

	public override string Texture {
		get => "Canisters/Content/Items/Canisters/HarmonicCanister";
	}

	public override void SetDefaults() {
		Projectile.DefaultToFiredCanister();
		Projectile.penetrate = -1;
	}

	public override void PostAI() {
		_timer++;
		
		if (_timer > TimeBeforeGravityAffected && Main.myPlayer == Projectile.owner) {
			Projectile.Kill();
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Explode(Main.player[Projectile.owner]);
		if (Main.netMode == NetmodeID.MultiplayerClient) {
			BroadcastExplosionSync(-1, Main.myPlayer, Projectile.identity);
		}
	}

	protected override void ExplosionEffect() {
		if (Main.myPlayer == Projectile.owner) {
			Projectile.Explode(100, 100);
		}

		int dustMult = HasGravity ? 2 : 1;

		DustHelpers.MakeDustExplosion(Projectile.Center, 30f, DustID.PinkTorch, 15 * dustMult, 6f, 10f, noGravity: true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 30f, DustID.PurpleTorch, 15 * dustMult, 6f, 10f, noGravity: true);

		SoundEngine.PlaySound(SoundID.Zombie103 with { Volume = 0.2f, PitchRange = (-0.4f, -0.1f), MaxInstances = 2, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, Projectile.Center);
	}
}
