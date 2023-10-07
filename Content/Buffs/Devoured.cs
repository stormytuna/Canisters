using System;
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
	private const int NaniteBaseDamage = 50;

	public override bool InstancePerEntity => true;

	public bool Devoured { get; set; }

	private float naniteBonusDamage;

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

		naniteBonusDamage += 0.1f;
	}

	public override void OnKill(NPC npc) {
		if (Devoured) {
			Vector2 velocity = Main.rand.NextVector2CircularEdge(5f, 5f) * Main.rand.NextFloat(0.5f, 1f);
			int damage = NaniteBaseDamage + (int)MathF.Ceiling(naniteBonusDamage);
			Projectile.NewProjectile(npc.GetSource_Death(), npc.Center, velocity, ModContent.ProjectileType<Nanites>(), damage, 2f, npc.lastInteraction);
		}
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