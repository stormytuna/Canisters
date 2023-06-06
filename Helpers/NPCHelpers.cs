using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace Canisters.Helpers;

public static class NPCHelpers
{
	public static IEnumerable<NPC> FindNearbyNPCs(float range, Vector2 worldPos, List<int> ignoredNPCs = null) {
		ignoredNPCs ??= new List<int>();
		return Main.npc.SkipLast(1).Where(npc => npc.DistanceSQ(worldPos) < range * range && npc.active && !npc.CountsAsACritter && !npc.friendly && !npc.dontTakeDamage && !npc.immortal && !ignoredNPCs.Contains(npc.type));
	}

	public static NPC FindClosestNPC(float range, Vector2 worldPos, bool checkCollision = true, List<int> excludedNPCs = null) {
		excludedNPCs ??= new List<int>();
		NPC closestNPC = null;
		float closestNPCDistance = float.PositiveInfinity;

		foreach (NPC npc in Main.npc.SkipLast(1)) {
			float distance = Vector2.Distance(npc.Center, worldPos);
			if (!npc.CanBeChasedBy() || distance > range || distance > closestNPCDistance || excludedNPCs.Contains(npc.whoAmI)) {
				continue;
			}

			if (checkCollision && !CollisionHelpers.CanHit(npc, worldPos)) {
				continue;
			}

			closestNPC = npc;
			closestNPCDistance = distance;
		}

		return closestNPC;
	}

	public static void StrikeNPC(NPC npc, NPC.HitInfo hitInfo) {
		if (Main.netMode == NetmodeID.SinglePlayer) {
			npc.StrikeNPC(hitInfo);
			return;
		}

		NetMessage.SendStrikeNPC(npc, hitInfo);
	}
}