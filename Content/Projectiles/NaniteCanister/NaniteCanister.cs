using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.NaniteCanister;

public class NaniteCanister : CanisterProjectile
{
	public override string Texture => "Canisters/Content/Items/Canisters/NaniteCanister";

	public override void Explode() {
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.TurnToExplosion(96, 96);
		Projectile.timeLeft = 3;

		int numDust = Main.rand.Next(30, 40);
		for (int i = 0; i < numDust; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Red);
			dust.noGravity = true;
			dust.velocity *= 2.5f;
		}

		for (int i = 0; i < 6; i++) {
			Vector2 velocity = Main.rand.NextVector2CircularEdge(5f, 5f) * Main.rand.NextFloat(0.5f, 1f);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<Nanites>(), Projectile.damage, 2f, Projectile.owner);
		}
	}
}