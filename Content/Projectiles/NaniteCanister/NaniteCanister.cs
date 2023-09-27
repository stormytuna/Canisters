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

	public override void OnExplode() {
		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);

		Projectile.CreateExplosion(96, 96);

		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Clentaminator_Cyan, Main.rand.Next(30, 40), 0f, 2.5f, noGravity: true);

		for (int i = 0; i < 6; i++) {
			Vector2 velocity = Main.rand.NextVector2CircularEdge(5f, 5f) * Main.rand.NextFloat(0.5f, 1f);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<Nanites>(), Projectile.damage, 2f, Projectile.owner);
		}
	}
}