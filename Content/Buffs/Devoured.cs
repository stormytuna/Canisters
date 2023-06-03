using Canisters.Content.Projectiles.NaniteCanister;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Buffs;

public class Devoured : ModBuff
{
	public override void SetStaticDefaults() {
		Main.debuff[Type] = true;
	}

	public override void Update(NPC npc, ref int buffIndex) {
		npc.GetGlobalNPC<DevouredGlobalNPC>().Devoured = true;
	}
}

public class DevouredGlobalNPC : GlobalNPC
{
	public override bool InstancePerEntity => true;

	public bool Devoured { get; set; }

	public override void ResetEffects(NPC npc) {
		Devoured = false;
	}

	public override void OnKill(NPC npc) {
		if (Main.netMode != NetmodeID.MultiplayerClient && Devoured) {
			Vector2 velocity = Main.rand.NextVector2CircularEdge(5f, 5f) * Main.rand.NextFloat(0.5f, 1f);
			Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, velocity, ModContent.ProjectileType<Nanites>(), 1000, 2f, npc.lastInteraction);
		}
	}

	public override void DrawEffects(NPC npc, ref Color drawColor) {
		if (Devoured) {
			drawColor = Color.Lerp(drawColor, Color.Red, 0.5f);
		}
	}
}