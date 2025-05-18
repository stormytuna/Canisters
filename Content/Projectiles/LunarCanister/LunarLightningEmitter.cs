using System;
using System.Collections.Generic;
using Canisters.Helpers;

namespace Canisters.Content.Projectiles.LunarCanister;

public class LunarLightningEmitter : ModProjectile
{
	private ref float Timer {
		get => ref Projectile.ai[0];
	}

	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 41;
		Projectile.penetrate = -1;
	}

	public override void AI() {
		if (Timer % 8 == 0) {
			Vector2 source = Projectile.Center + new Vector2(Main.rand.NextFloat(-300f, 300f), -1500f);
			// TODO: Make this destination a bit nicer, ie cling to nearby tiles/entities
			Vector2 destination = Projectile.Center + new Vector2(Main.rand.NextFloat(-8f, 8f), 0f);
			DustHelpers.MakeLightningDust(source, destination, DustID.Vortex, 1f);
			DustHelpers.MakeDustExplosion(destination, 4f, DustID.Vortex, 8, 0f, 10f, noGravity: true);

			if (Projectile.owner == Main.myPlayer) {
				IEnumerable<NPC> nearbyNpCs = NpcHelpers.FindNearbyNPCs(96f, Projectile.Center);
				foreach (NPC npc in nearbyNpCs) {
					NPC.HitInfo hitInfo = new() {
						Damage = Projectile.damage / 3,
						Knockback = Projectile.knockBack / 2f,
						DamageType = Projectile.DamageType,
						Crit = false,
						HitDirection = Math.Sign(Projectile.DirectionTo(npc.Center).X)
					};

					NpcHelpers.StrikeNPC(npc, hitInfo);
				}
			}
		}

		Projectile.velocity = Vector2.Zero;

		Timer++;
	}
}
