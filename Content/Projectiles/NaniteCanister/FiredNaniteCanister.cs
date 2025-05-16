using Canisters.Content.Buffs;
using Canisters.Helpers;
using Terraria.Audio;

namespace Canisters.Content.Projectiles.NaniteCanister;

public class FiredNaniteCanister : BaseFiredCanisterProjectile
{
	public override string Texture {
		get => "Canisters/Content/Items/Canisters/NaniteCanister";
	}

	public override void Explode() {
		if (Main.myPlayer == Projectile.owner) {
			for (int i = 0; i < 6; i++) {
				Vector2 velocity = Main.rand.NextVector2CircularEdge(5f, 5f) * Main.rand.NextFloat(0.5f, 1f);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<Nanites>(), Projectile.damage, 2f, Projectile.owner);
			}
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Clentaminator_Cyan, Main.rand.Next(30, 40), 0f, 2.5f, noGravity: true);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(ModContent.BuffType<Devoured>(), 15 * 60);

		// If we kill the npc with this projectile it won't get a chance to update buffs
		if (!target.active) {
			target.GetGlobalNPC<DevouredGlobalNpc>().SpawnNanite(target);
		}
	}
}
