using Canisters.Content.Projectiles.NaniteCanister;
using Canisters.Helpers;

namespace Canisters.Content.Buffs;

public class Devoured : ModBuff
{
	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<DevouredGlobalNpc>().Devoured = true;
	}
}

public class DevouredGlobalNpc : GlobalNPC
{
	private const int _naniteBaseDamage = 50;

	private float _naniteBonusDamage;

	public bool Devoured { get; set; }

	public override bool InstancePerEntity {
		get => true;
	}

	public void SpawnNanite(NPC npc) {
		Vector2 velocity = Main.rand.NextVector2CircularEdge(5f, 5f) * Main.rand.NextFloat(0.5f, 1f);
		int damage = _naniteBaseDamage + MathHelpers.Ceiling(_naniteBonusDamage);
		Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, velocity, ModContent.ProjectileType<Nanites>(), damage, 2f, Main.myPlayer);
	}

	public override void ResetEffects(NPC npc) {
		Devoured = false;
	}

	public override void UpdateLifeRegen(NPC npc, ref int damage) {
		if (!Devoured) {
			return;
		}

		if (npc.lifeRegen > 0) {
			npc.lifeRegen = 0;
		}

		npc.lifeRegen -= 40;

		if (damage < 5) {
			damage = 5;
		}

		_naniteBonusDamage += 0.1f;
	}

	public override void OnKill(NPC npc) {
		if (!Devoured) {
			return;
		}

		SpawnNanite(npc);
	}

	public override void DrawEffects(NPC npc, ref Color drawColor) {
		if (!Devoured) {
			return;
		}

		if (Main.rand.NextBool(3)) {
			var dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Clentaminator_Cyan);
			dust.noGravity = true;
			dust.velocity *= 1f;
		}
	}
}
