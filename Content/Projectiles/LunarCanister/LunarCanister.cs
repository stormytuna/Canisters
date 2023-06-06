using System;
using System.Collections.Generic;
using Canisters.Common.Systems;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace Canisters.Content.Projectiles.LunarCanister;

public class LunarCanister : CanisterProjectile
{
	public override string Texture => "Canisters/Content/Items/Canisters/LunarCanister";

	private ref float Timer => ref Projectile.ai[0];

	private bool hasExploded;

	public override void AI() {
		if (!hasExploded) {
			return;
		}

		if (Timer % 8 == 0) {
			Vector2 source = Projectile.Center + new Vector2(Main.rand.NextFloat(-300f, 300f), -1500f);
			Vector2 destination = Projectile.Center + new Vector2(Main.rand.NextFloat(-8f, 8f), 0f);
			LightningSystem.MakeDust(source, destination, DustID.Vortex, 1f);
			DustHelpers.MakeDustExplosion(destination, 4f, DustID.Vortex, 8, 0f, 10f, noGravity: true);

			if (Projectile.owner == Main.myPlayer) {
				IEnumerable<NPC> nearbyNPCs = NPCHelpers.FindNearbyNPCs(96f, Projectile.Center);
				foreach (NPC npc in nearbyNPCs) {
					NPC.HitInfo hitInfo = new() {
						Damage = Projectile.damage / 3,
						Knockback = Projectile.knockBack / 2f,
						DamageType = Projectile.DamageType,
						Crit = false,
						HitDirection = Math.Sign(Projectile.DirectionTo(npc.Center).X)
					};

					NPCHelpers.StrikeNPC(npc, hitInfo);
				}
			}
		}

		Projectile.velocity = Vector2.Zero;

		Timer++;
	}

	public override void Explode() {
		Projectile.TurnToExplosion(96, 96);
		Projectile.timeLeft = 40;
		Projectile.aiStyle = -1;
		Projectile.position += new Vector2(0f, 16f); // This moves our projectile closer to where it should be
		Projectile.velocity = Vector2.Zero;
		hasExploded = true;

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
	}
}