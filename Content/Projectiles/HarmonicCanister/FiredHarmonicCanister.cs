using Canisters.Helpers;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.HarmonicCanister;

public class FiredHarmonicCanister : BaseFiredCanisterProjectile
{
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

	public override void Explode() {
		if (Main.myPlayer == Projectile.owner) {
			Projectile.Explode(100, 100);
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 16f, DustID.PinkTorch, 15, 8f, 16f, noGravity: true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 16f, DustID.PurpleTorch, 15, 8f, 16f, noGravity: true);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
	}

	public override void PostAI() {
		if (HasGravity && Main.myPlayer == Projectile.owner) {
			Projectile.Kill();
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Explode();
		if (Main.netMode == NetmodeID.MultiplayerClient) {
			Main.NewText(Projectile.identity);
			BroadcastExplosionSync(-1, Main.myPlayer, Projectile.identity);
		}
	}
}
