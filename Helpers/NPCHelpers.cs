using System.Collections.Generic;
using System.Linq;

namespace Canisters.Helpers;

public static class NpcHelpers
{
	public static IEnumerable<NPC> FindNearbyNpCs(float range, Vector2 worldPos, List<int> ignoredNpCs = null) {
		ignoredNpCs ??= new List<int>();
		return Main.npc.SkipLast(1).Where(npc =>
			npc.DistanceSQ(worldPos) < range * range && npc.active && !npc.CountsAsACritter && !npc.friendly &&
			!npc.dontTakeDamage && !npc.immortal && !ignoredNpCs.Contains(npc.type));
	}

	public static NPC FindClosestNpc(float range, Vector2 worldPos, bool checkCollision = true,
		List<int> excludedNpCs = null) {
		excludedNpCs ??= new List<int>();
		NPC closestNpc = null;
		float closestNpcDistance = float.PositiveInfinity;

		foreach (NPC npc in Main.npc.SkipLast(1)) {
			float distance = Vector2.Distance(npc.Center, worldPos);
			if (!npc.CanBeChasedBy() || distance > range || distance > closestNpcDistance ||
				excludedNpCs.Contains(npc.whoAmI)) {
				continue;
			}

			if (checkCollision && !CollisionHelpers.CanHit(npc, worldPos)) {
				continue;
			}

			closestNpc = npc;
			closestNpcDistance = distance;
		}

		return closestNpc;
	}

	public static void StrikeNpc(NPC npc, NPC.HitInfo hitInfo) {
		if (Main.netMode == NetmodeID.SinglePlayer) {
			npc.StrikeNPC(hitInfo);
			return;
		}

		NetMessage.SendStrikeNPC(npc, hitInfo);
	}
}
