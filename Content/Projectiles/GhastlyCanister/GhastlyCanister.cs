using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.GhastlyCanister;

public class GhastlyCanister : CanisterProjectile
{
	private const float ClusterRange = 100f;

	public override string Texture => "Canisters/Content/Items/Canisters/GhastlyCanister";

	private ref float Timer => ref Projectile.ai[0];

	private bool exploded;

	public override void AI() {
		if (!exploded) {
			if (Timer >= 30f) {
				Explode();
			}

			return;
		}

		if (Projectile.owner == Main.myPlayer && Timer % 6 == 0) {
			Vector2 position = Main.rand.NextVector2Circular(ClusterRange, ClusterRange) + Projectile.Center;
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, Vector2.Zero, ModContent.ProjectileType<GhastlyExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}

		Timer++;
	}

	public override void Explode() {
		Projectile.TurnToExplosion(96, 96);
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.aiStyle = -1;
		Projectile.timeLeft = 60;
		Timer = 0f;
		exploded = true;

		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.BlueFairy, 22, 0f, 8f, 50, 120, 1f, 1.5f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.BlueFairy, 15, 4f, 14f, 70, 120, 1f, 1.3f, true);
	}
}