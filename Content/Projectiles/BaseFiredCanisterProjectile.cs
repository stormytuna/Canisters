using System;
using Canisters.Helpers;

namespace Canisters.Content.Projectiles;

public abstract class BaseFiredCanisterProjectile : ModProjectile
{
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
	///     Use for client side explosion effects, such as dealing damage or spawning projectiles
	/// </summary>
	public virtual void Explode() { }

	/// <summary>
	///     Use for explosion visuals. Use the provided position and velocity for any calculations
	/// </summary>
	public virtual void ExplosionVisuals() {
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
		Explode();
		MakeCanisterGore();
	}

	public void ReceiveExplosionSync() {
		Explode();
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

		var canisterGore1 = Mod.Find<ModGore>("BrokenCanister_01");
		var canisterGore2 = Mod.Find<ModGore>("BrokenCanister_02");
		Vector2 canisterVelocity = Main.rand.NextVector2Circular(3f, 3f);

		var gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, canisterVelocity + (Projectile.velocity * 0.5f), canisterGore1.Type);
		gore.timeLeft = 120;
		gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, -canisterVelocity + (Projectile.velocity * 0.5f), canisterGore2.Type);
		gore.timeLeft = 120;
	}
}
