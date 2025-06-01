using System;

namespace Canisters.Content.Projectiles;

public abstract class BaseFiredCanisterProjectile : ModProjectile
{
	public static event Action<Player, Projectile> OnExplode;

	/// <summary>
	///     Amount of time in frames this projectile will fly straight before being gravity affected.
	///     Defaults to 20.
	/// </summary>
	public virtual int TimeBeforeGravityAffected {
		get => 20;
	}

	public bool HasGravity {
		get => Projectile.ai[1] == 1f;
		set => Projectile.ai[1] = value ? 1f : 0f;
	}

	public ref float Timer {
		get => ref Projectile.ai[0];
	}

	/// <summary>
	/// Use for explosion effects, such as dealing damage, spawning projectiles, or creating dust. Called on all clients and the server.
	/// <para/> NOTE: To manually cause an explosion for a canister, call the Explode method, not this one!
	/// </summary>
	protected virtual void ExplosionEffect() { }

	public void Explode(Player player) {
		ExplosionEffect();
		OnExplode?.Invoke(player, Projectile);
	}

	public override void SetDefaults() {
		Projectile.DefaultToFiredCanister();
	}

	public sealed override void AI() {
		Timer++;

		Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.03f * Projectile.direction;

		if (HasGravity) {
			Projectile.velocity.Y += 0.4f;
			if (Projectile.velocity.Y > 16f) {
				Projectile.velocity.Y = 16f;
			}

			Projectile.velocity.X *= 0.97f;
		}
		else if (Timer >= TimeBeforeGravityAffected) {
			HasGravity = true;
		}
	}

	public override void OnKill(int timeLeft) {
		Explode(Main.player[Projectile.owner]);
		MakeCanisterGore();
	}

	public void ReceiveExplosionSync() {
		Explode(Main.player[Projectile.owner]);
	}

	public void BroadcastExplosionSync(int toWho, int fromWho, int identity) {
		ModPacket packet = Mod.GetPacket();
		packet.Write((byte)Canisters.MessageType.CanisterExplosionVisuals);
		packet.Write(identity);
		packet.Send(toWho, fromWho);
	}

	protected void MakeCanisterGore() {
		if (Main.netMode == NetmodeID.Server) {
			return;
		}

		ModGore canisterGore1 = Mod.Find<ModGore>("BrokenCanister_01");
		ModGore canisterGore2 = Mod.Find<ModGore>("BrokenCanister_02");
		Vector2 canisterVelocity = Main.rand.NextVector2Circular(3f, 3f);

		Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, canisterVelocity + (Projectile.velocity * 0.5f), canisterGore1.Type);
		gore.timeLeft = 120;
		gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, -canisterVelocity + (Projectile.velocity * 0.5f), canisterGore2.Type);
		gore.timeLeft = 120;
	}

	/// <summary>
	/// Prevents hitting targets that are out of line of sight.
	/// </summary>
	public override bool? CanHitNPC(NPC target) {
		if (!CollisionHelpers.CanHit(target, Projectile.Center)) {
			return false;
		}

		return base.CanHitNPC(target);
	}

	/// <summary>
	/// Reduces damage against the Eater of Worlds' segments in Expert mode.
	/// </summary>
	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		if (Main.expertMode && target.type is NPCID.EaterofWorldsBody or NPCID.EaterofWorldsHead or NPCID.EaterofWorldsTail) {
			modifiers.FinalDamage /= 5;
		}
	}
}
