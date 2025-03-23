using Canisters.Content.Buffs;
using Canisters.Content.Dusts;
using Canisters.Helpers;
using Terraria.DataStructures;

namespace Canisters.Content.Projectiles.NaniteCanister;

public class Nanites : ModProjectile
{
	private const float _detectionRange = 25f * 16f;
	private const float _rangeToKeepDetection = 50f * 16f;
	private const float _acceleration = 0.3f;
	private const float _topSpeed = 12f;

	private bool _firstFrame = true;

	private AiState State {
		get => (AiState)Projectile.ai[0];
		set => Projectile.ai[0] = (float)value;
	}

	private NPC Target {
		get {
			int index = (int)Projectile.ai[1];
			return Main.npc.IndexInRange(index) ? Main.npc[index] : Main.npc[Main.maxNPCs];
		}
		set => Projectile.ai[1] = value.whoAmI;
	}

	public override void SetStaticDefaults() {
		ProjectileID.Sets.CultistIsResistantTo[Type] = true;
	}

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 18;
		Projectile.height = 18;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 3 * 60;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.penetrate = 1;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void OnSpawn(IEntitySource source) {
		State = AiState.Idle;
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;
			int numDust = Main.rand.Next(8, 12);
			for (int i = 0; i < numDust; i++) {
				var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
					ModContent.DustType<NaniteDust>());
				dust.customData = Projectile;
			}
		}

		// Movement
		if (State == AiState.Idle) {
			// Try find target
			NPC closestNpc = NpcHelpers.FindClosestNpc(_detectionRange, Projectile.Center);
			if (closestNpc is not null) {
				State = AiState.Homing;
				Target = closestNpc;
			}

			// Otherwise, just slow down
			Projectile.velocity *= 0.98f;

			return;
		}

		if (!Target.CanBeChasedBy() || !Target.WithinRange(Projectile.Center, _rangeToKeepDetection)) {
			State = AiState.Idle;
			return;
		}

		EntityHelpers.SmoothHoming(Projectile, Target.Center, _acceleration, _topSpeed, Target.velocity, false);

		Projectile.timeLeft++;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(ModContent.BuffType<Devoured>(), 5 * 60);
		target.GetGlobalNPC<DevouredGlobalNpc>().Devoured = true; // Hack because buff won't update if we one shot it
		// TODO: Won't work in multiplayer
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (Projectile.velocity.X == 0f) {
			Projectile.velocity.X = -oldVelocity.X;
		}

		if (Projectile.velocity.Y == 0f) {
			Projectile.velocity.Y = -oldVelocity.Y;
		}

		return false;
	}

	public override void Kill(int timeLeft) {
		int numDust = Main.rand.Next(2, 6);
		for (int i = 0; i < numDust; i++) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
				DustID.Clentaminator_Cyan);
			dust.noGravity = true;
			dust.velocity *= 2f;
		}
	}

	private enum AiState
	{
		Homing,
		Idle
	}
}
