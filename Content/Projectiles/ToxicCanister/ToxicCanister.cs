using System.IO;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Canisters.Content.Projectiles.ToxicCanister;

/// <summary>
///     Vial of venom canister
/// </summary>
// TODO: Visuals, balancing
public class ToxicCanister : CanisterProjectile
{
	public override string Texture => "Canisters/Content/Items/Canisters/ToxicCanister";

	private bool hasExploded;

	public override void AI() {
		if (!hasExploded) {
			return;
		}

		Projectile.aiStyle = -1;
		Projectile.velocity = Vector2.Zero;

		for (int i = 0; i < 10; i++) {
			Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.DemonTorch);
			d.noGravity = true;
			d.noLight = true;
			d.noLightEmittence = true;
		}
	}

	public override void Explode() {
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.TurnToExplosion(200, 200);
		Projectile.knockBack = 0f;
		Projectile.timeLeft = 200;
		hasExploded = true;
		Projectile.netUpdate = true;

		// TODO: Dust explosion
	}

	public override void SendExtraAI(BinaryWriter writer) {
		writer.Write(hasExploded);
	}

	public override void ReceiveExtraAI(BinaryReader reader) {
		hasExploded = reader.ReadBoolean();
	}
}