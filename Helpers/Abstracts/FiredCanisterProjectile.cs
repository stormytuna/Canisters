using System;

namespace Canisters.Helpers.Abstracts;

public abstract class FiredCanisterProjectile : ModProjectile
{
	public virtual int TimeBeforeGravityAffected {
		get => 20;
	}

	private bool HasGravity {
		get => Projectile.ai[1] == 1f;
		set => Projectile.ai[1] = value ? 1f : 0f;
	}

	public ref float Timer {
		get => ref Projectile.ai[0];
	}

	/// <summary>
	///     Use for client side effects, such as spawning projectiles or striking npcs. Only called for the owner of the projectile.
	///		For visuals, use <see cref="ExplosionVisuals"/>
	/// </summary>
	public virtual void OnExplode() { }
	
	/// <summary>
	/// Use for canister explosion visuals, such as dust explosions or gore.
	/// Use the provided position and velocity instead of referencing values on the Projectile to prevent visual issues on other clients
	/// </summary>
	public virtual void ExplosionVisuals(Vector2 position, Vector2 velocity) { }

	public virtual void SafeAI() { }

	public virtual bool SafeOnTileCollide(Vector2 oldVelocity) {
		return false;
	}

	public sealed override void SetDefaults() {
		Projectile.width = 22;
		Projectile.height = 22;
		Projectile.aiStyle = -1;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public sealed override void AI() {
		SafeAI();

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

	public sealed override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		TryExplode();
	}

	public sealed override bool OnTileCollide(Vector2 oldVelocity) {
		TryExplode();
		return SafeOnTileCollide(oldVelocity);
	}

	public void ReceiveExplosionSync(Vector2 position, Vector2 velocity) {
		MakeCanisterGore(position, velocity);
		ExplosionVisuals(position, velocity);	
	}
	
	public void BroadcastExplosionSync(int toWho, int fromWho, int canisterType, Vector2 position, Vector2 velocity) {
		ModPacket packet = Mod.GetPacket();
		packet.Write((byte)Canisters.MessageType.CanisterExplosionVisuals);
		packet.Write7BitEncodedInt(canisterType);
		packet.WriteVector2(position);
		packet.WriteVector2(velocity);
		packet.Send(toWho, fromWho);
	}
	
	protected void TryExplode() {
		// TODO: Decouple this from projectile alpha, just use a bool field?
		if (Projectile.owner != Main.myPlayer || Projectile.alpha == 255) {
			return;
		}

		OnExplode();
		
		ReceiveExplosionSync(Projectile.Center, Projectile.velocity);
		if (Main.netMode == NetmodeID.MultiplayerClient) {
			BroadcastExplosionSync(-1, Main.myPlayer, Type, Projectile.Center, Projectile.velocity);
		}
	}
	
	private void MakeCanisterGore(Vector2 position, Vector2 velocity) {
		var canisterGore1 = Mod.Find<ModGore>("BrokenCanister_01");
		var canisterGore2 = Mod.Find<ModGore>("BrokenCanister_02");
		Vector2 canisterVelocity = Main.rand.NextVector2Circular(3f, 3f);
		
		var gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), position, canisterVelocity + (velocity * 0.5f), canisterGore1.Type);
		gore.timeLeft = 120;
		gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), position, -canisterVelocity + (velocity * 0.5f), canisterGore2.Type);
		gore.timeLeft = 120;
	}
}
