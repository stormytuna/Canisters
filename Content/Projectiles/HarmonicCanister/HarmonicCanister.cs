using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.HarmonicCanister;

/// <summary>
///     Soul of night and soul of light canister
/// </summary>
public class HarmonicCanister : ModProjectile
{
	public override void SetDefaults() {
		// Base stats
		Projectile.width = 22;
		Projectile.height = 22;
		Projectile.aiStyle = 2;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override string Texture => "Canisters/Content/Items/Canisters/HarmonicCanister";

	private bool hasExploded;
	private int frameCounter;

	private Vector2 dustAxis;

	public override void AI() {
		if (!hasExploded) {
			frameCounter = 0;
			return;
		}

		Projectile.aiStyle = -1;
		Projectile.velocity = Vector2.Zero;

		if (frameCounter == 0f) {
			dustAxis = Main.rand.NextVector2Circular(1f, 1f).SafeNormalize(Vector2.Zero);
		}

		// Night explodey dust
		for (int i = 0; i < 10; i++) {
			Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.PurpleTorch);
			d.velocity = dustAxis.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(2f, 15f);
			d.noGravity = true;
		}

		// Light explodey dust
		for (int i = 0; i < 10; i++) {
			Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.PinkTorch);
			d.velocity = -dustAxis.RotatedByRandom(MathHelper.PiOver2) * Main.rand.NextFloat(2f, 15f);
			d.noGravity = true;
		}

		// Central ball dust
		for (int i = 0; i < 4; i++) {
			int dustType = Main.rand.NextBool() ? DustID.PinkTorch : DustID.PurpleTorch;
			Vector2 offset = Main.rand.NextVector2Circular(8f, 8f);
			Dust d = Dust.NewDustPerfect(Projectile.Center + offset, dustType);
			d.velocity = Vector2.Zero;
			d.noGravity = true;
		}

		// Rotate our dust axis
		dustAxis = dustAxis.RotatedBy(0.04f);

		frameCounter++;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		if (Projectile.alpha != 255) {
			Explode();
		}
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (Projectile.alpha != 255) {
			Explode();
			return false;
		}

		return base.OnTileCollide(oldVelocity);
	}

	private void Explode() {
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.TurnToExplosion(96, 96);
		Projectile.timeLeft = 120;
		hasExploded = true;

		// TODO: Dust explosion
	}

	public override void SendExtraAI(BinaryWriter writer) {
		writer.Write(hasExploded);
		writer.Write(frameCounter);
	}

	public override void ReceiveExtraAI(BinaryReader reader) {
		hasExploded = reader.ReadBoolean();
		frameCounter = reader.ReadInt32();
	}
}