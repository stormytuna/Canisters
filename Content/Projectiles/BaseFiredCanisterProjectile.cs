using System;
using Canisters.Helpers;

namespace Canisters.Content.Projectiles;

public abstract class BaseFiredCanisterProjectile : ModProjectile
{
		/// <summary>
    	/// Amount of time in frames this projectile will fly straight before being gravity affected.
    	/// Defaults to 20.
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
    	
	    public virtual void Explode(Vector2 position) { }
	    
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
		    Explode(Projectile.position);
	    }

	    public void ReceiveExplosionSync(Vector2 position, Vector2 velocity) {
		    Explode(position);
    	}
    	
    	public void BroadcastExplosionSync(int toWho, int fromWho, int canisterType, Vector2 position, Vector2 velocity) {
    		ModPacket packet = Mod.GetPacket();
    		packet.Write((byte)Canisters.MessageType.CanisterExplosionVisuals);
    		packet.Write7BitEncodedInt(canisterType);
    		packet.WriteVector2(position);
    		packet.WriteVector2(velocity);
    		packet.Send(toWho, fromWho);
    	}
    	
    	protected void MakeCanisterGore(Vector2 position, Vector2 velocity) {
    		var canisterGore1 = Mod.Find<ModGore>("BrokenCanister_01");
    		var canisterGore2 = Mod.Find<ModGore>("BrokenCanister_02");
    		Vector2 canisterVelocity = Main.rand.NextVector2Circular(3f, 3f);
    		
    		var gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), position, canisterVelocity + (velocity * 0.5f), canisterGore1.Type);
    		gore.timeLeft = 120;
    		gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), position, -canisterVelocity + (velocity * 0.5f), canisterGore2.Type);
    		gore.timeLeft = 120;
    	}
}
