using Canisters.Content.Dusts;
using Canisters.Helpers;
using Terraria.DataStructures;

namespace Canisters.Content.Projectiles.NaniteCanister;

// TODO: rewrite?
public class Nanites : ModProjectile
{
	private const float DetectionRange = 15f * 16f;
	private const float RangeToKeepDetection = 25f * 16f;
	private const float Acceleration = 0.3f;
	private const float TopSpeed = 12f;

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
		Projectile.width = 18;
		Projectile.height = 18;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 3 * 60;

		Projectile.friendly = true;
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
				Dust naniteDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<NaniteDust>());
				naniteDust.customData = Projectile;
			}
		}

		if (Main.rand.NextBool(5)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Cyan);
			dust.noGravity = true;
		}

		if (State == AiState.Idle) {
			NPC closestNpc = NPCHelpers.FindClosestNPC(DetectionRange, Projectile.Center);
			if (closestNpc is not null) {
				State = AiState.Homing;
				Target = closestNpc;
				return;
			}

			Projectile.velocity *= 0.98f;

			return;
		}

		if (!Target.CanBeChasedBy() || !Target.WithinRange(Projectile.Center, RangeToKeepDetection)) {
			State = AiState.Idle;
			return;
		}

		MathHelpers.SmoothHoming(Projectile, Target.Center, Acceleration, TopSpeed, Target.velocity);

		Projectile.timeLeft++;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		// TODO: MP
		target.GetGlobalNPC<NaniteGlobalNpc>().NaniteCount++;
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

	public override void OnKill(int timeLeft) {
		int numDust = Main.rand.Next(2, 6);
		for (int i = 0; i < numDust; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Cyan);
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
