using Canisters.Helpers;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.GlisteningCanister;

public class GlisteningBall : ModProjectile
{
	private bool _firstFrame = true;

	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}

	private bool IsParent {
		get => Projectile.ai[0] == 0f;
	}

	public override void SetDefaults() {
		Projectile.width = 8;
		Projectile.height = 8;
		Projectile.aiStyle = -1;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;
			if (!IsParent) {
				Projectile.timeLeft = 15;
				Projectile.extraUpdates = 1;
				Projectile.tileCollide = false;
			}
		}

		Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IchorTorch, Alpha: Main.rand.Next(100, 200), Scale: Main.rand.NextFloat(1f, 1.2f));
		dust.position += Projectile.velocity * 0.6f;
		dust.velocity *= 0.3f;
		dust.noGravity = true;
		dust.noLight = true;

		dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ichor, Alpha: Main.rand.Next(100, 200), Scale: Main.rand.NextFloat(1f, 1.2f));
		dust.position += Projectile.velocity * 0.6f;
		dust.velocity *= 0.3f;
		dust.noGravity = true;
		dust.noLight = true;

		Lighting.AddLight(Projectile.Center, TorchID.Ichor);
	}

	public override void OnKill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 4f, DustID.IchorTorch, 5, 0f, 8f, 100, 150, 1f, 1.2f, true);

		if (!IsParent) {
			return;
		}

		SoundStyle soundStyle = SoundID.Item154 with { MaxInstances = 0, Volume = 0.5f, PitchRange = (-0.8f, -0.6f) };
		SoundEngine.PlaySound(soundStyle, Projectile.Center);

		if (Main.myPlayer == Projectile.owner) {
			Vector2 velocity = Main.rand.NextVector2CircularEdge(5f, 5f);

			Vector2 offset = velocity * -15f;
			Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + offset, velocity, Type, Projectile.damage / 3, 0f, Projectile.owner, 1f);
			Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center - offset, -velocity, Type, Projectile.damage / 3, 0f, Projectile.owner, 1f);
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.Ichor, 600);
	}
}
