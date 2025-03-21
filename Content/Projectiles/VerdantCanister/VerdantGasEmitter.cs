using Terraria.DataStructures;

namespace Canisters.Content.Projectiles.VerdantCanister;

/// <summary>
///     Helps shoot the verdant gas. This projectile will sit on the weapons muzzle and fire 3 verdant gas projectiles over
///     its lifetime
/// </summary>
public class VerdantGasEmitter : ModProjectile
{
	private int maxFireCounter;
	private int numFired;

	private Vector2 ownerOffset;
	private Vector2 startVelocity;

	private Player Owner {
		get => Main.player[Projectile.owner];
	}

	private ref float AI_FireCounter {
		get => ref Projectile.ai[0];
	}

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
	}

	public override void OnSpawn(IEntitySource source) {
		ownerOffset = Owner.Center - Projectile.Center;
		startVelocity = Projectile.velocity;

		Projectile.timeLeft = CombinedHooks.TotalUseTime(Owner.HeldItem.useTime, Owner, Owner.HeldItem);
		maxFireCounter = Projectile.timeLeft / 3;
	}

	public override void AI() {
		// Make sure our projectile stays on the end of the gun
		Projectile.Center = Owner.Center - ownerOffset;
		Projectile.velocity = Vector2.Zero;

		// Actually shoot the projectile
		if (AI_FireCounter <= 0 && Collision.CanHit(Owner.Center, 0, 0, Projectile.Center, 0, 0) && numFired < 3 &&
		    Main.myPlayer == Projectile.owner) {
			Vector2 velocity = startVelocity * Main.rand.NextFloat(0.95f, 1.05f);
			velocity = velocity.RotatedByRandom(0.1f);
			var gasProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, velocity,
				ModContent.ProjectileType<VerdantGas>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
			gasProj.originalDamage = Projectile.originalDamage / 3;
			AI_FireCounter = maxFireCounter;
			numFired++;

			// Make some dust
			for (int i = 0; i < 5; i++) {
				if (Main.rand.NextBool()) {
					var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
						DustID.GreenFairy, Alpha: Main.rand.Next(0, 50), Scale: Main.rand.NextFloat(0.6f, 1f));
					dust.velocity = startVelocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.01f, 0.8f);
				}
			}
		}

		// Make some small dust
		// Nature energy dust
		if (Main.rand.NextBool(2)) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenFairy,
				Alpha: Main.rand.Next(0, 50), Scale: Main.rand.NextFloat(0.6f, 0.8f));
			dust.velocity = startVelocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.01f, 0.8f);
		}

		AI_FireCounter--;
	}
}
