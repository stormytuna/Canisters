namespace Canisters.Content.Projectiles.NaniteCanister;

public class NaniteGlobalNpc : GlobalNPC
{
	private const int MaxNanites = 10;

	private int _naniteTimer = 0;

	public int NaniteCount;

	public float NaniteProgress {
		get => (float)NaniteCount / MaxNanites;
	}

	public int NaniteDamage {
		get => (int)(10 * NaniteProgress);
	}

	public bool Devoured {
		get => NaniteCount > 0;
	}

	public override bool InstancePerEntity {
		get => true;
	}

	public void SpawnNanite(NPC npc) {
		Vector2 velocity = Main.rand.NextVector2CircularEdge(5f, 5f) * Main.rand.NextFloat(0.5f, 1f);
		// TODO: make this npc immune for a short time
		Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, velocity, ModContent.ProjectileType<Nanites>(), NaniteDamage, 0f, Main.myPlayer);
	}

	public override void UpdateLifeRegen(NPC npc, ref int damage) {
		if (!Devoured) {
			return;
		}

		if (npc.lifeRegen > 0) {
			npc.lifeRegen = 0;
		}

		npc.lifeRegen -= NaniteCount * 2 * NaniteDamage;
		if (damage < NaniteCount * NaniteDamage) {
			damage = NaniteCount * NaniteDamage;
		}
	}

	public override void PostAI(NPC npc) {
		if (!Devoured) {
			return;
		}

		_naniteTimer++;
		if (_naniteTimer >= 5 * 60 && Main.netMode != NetmodeID.MultiplayerClient) {
			_naniteTimer = 0;
			NaniteCount--;
			SpawnNanite(npc);
		}
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
			Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Clentaminator_Cyan);
			dust.noGravity = true;
			dust.velocity *= 1f;
		}
	}
}
