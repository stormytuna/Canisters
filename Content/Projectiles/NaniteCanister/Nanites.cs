using Canisters.Content.Buffs;
using Canisters.Content.Dusts;
using Canisters.Helpers;
using Terraria.DataStructures;

namespace Canisters.Content.Projectiles.NaniteCanister;

public class Nanites : ModProjectile
{
	private const float DetectionRange = 25f * 16f;
	private const float RangeToKeepDetection = 50f * 16f;
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
				var naniteDust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<NaniteDust>());
				naniteDust.customData = Projectile;
			}
		}

		if (Main.rand.NextBool(5)) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Cyan);
			dust.noGravity = true;
		}

		if (State == AiState.Idle) {
			NPC closestNpc = NpcHelpers.FindClosestNpc(DetectionRange, Projectile.Center);
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

		EntityHelpers.SmoothHoming(Projectile, Target.Center, Acceleration, TopSpeed, Target.velocity, false);

		Projectile.timeLeft++;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(ModContent.BuffType<Devoured>(), 15 * 60);

		// If we kill the npc with this projectile it won't get a chance to update buffs
		if (!target.active) {
			target.GetGlobalNPC<DevouredGlobalNpc>().SpawnNanite(target);
		}
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
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Clentaminator_Cyan);
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
