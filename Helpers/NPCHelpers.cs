using System.Collections.Generic;
using System.Linq;

namespace Canisters.Helpers;

public static class NpcHelpers
{
	public static List<NPC> FindNearbyNPCs(float range, Vector2 worldPos, bool careAboutCollision = false, List<int> ignoredNPCs = null) {
		List<NPC> npcs = new(50);
		ignoredNPCs ??= [];
		
		foreach (var npc in Main.ActiveNPCs) {
			if (npc.WithinRange(worldPos, range) && !ignoredNPCs.Contains(npc.whoAmI) && npc.CanBeChasedBy()) {
				if (!careAboutCollision || CollisionHelpers.CanHit(npc, worldPos)) {
					npcs.Add(npc);
				}
			}	
		}
		
		return npcs;
	}

	public static NPC FindClosestNPC(float range, Vector2 worldPos, bool checkCollision = true, List<int> excludedNpCs = null) {
		excludedNpCs ??= [];
		NPC closestNpc = null;
		float closestNpcDistance = float.PositiveInfinity;

		foreach (NPC npc in Main.ActiveNPCs) {
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

	public static void StrikeNPC(NPC npc, NPC.HitInfo hitInfo) {
		if (Main.netMode == NetmodeID.SinglePlayer) {
			npc.StrikeNPC(hitInfo);
			return;
		}

		NetMessage.SendStrikeNPC(npc, hitInfo);
	}
}
