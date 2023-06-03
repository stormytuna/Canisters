using System.IO;
using Canisters.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.ToxicCanister;

/// <summary>
///     Vial of venom canister
/// </summary>
public class ToxicCanister : ModProjectile
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

	public override string Texture => "Canisters/Content/Items/Canisters/ToxicCanister";

	private bool hasExploded;

	private ref float AI_FrameCounter => ref Projectile.ai[0];

	public override void AI() {
		if (AI_FrameCounter == 45) {
			Explode();
		}

		if (hasExploded) { }
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
		Projectile.timeLeft = 300;
		hasExploded = true;

		// TODO: Dust explosion
		for (int i = 0; i < 60; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Venom);
			dust.velocity = Main.rand.NextVector2Circular(8f, 8f);
			dust.noGravity = true;
		}
	}

	public override void SendExtraAI(BinaryWriter writer) {
		writer.Write(hasExploded);
	}

	public override void ReceiveExtraAI(BinaryReader reader) {
		hasExploded = reader.ReadBoolean();
	}
}