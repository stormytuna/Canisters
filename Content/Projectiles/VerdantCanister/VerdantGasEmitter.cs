using Terraria.DataStructures;

namespace Canisters.Content.Projectiles.VerdantCanister;

public class VerdantGasEmitter : ModProjectile
{
	private int maxFireCounter;
	private int numFired;

	private Vector2 ownerOffset;
	private Vector2 startVelocity;

	private Player Owner {
		get => Main.player[Projectile.owner];
	}

	private ref float ShootTimer {
		get => ref Projectile.ai[0];
	}

	public override void SetDefaults() {
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
	}

	public override void OnSpawn(IEntitySource source) {
		ownerOffset = Owner.Center - Projectile.Center;
		startVelocity = Projectile.velocity;

		Projectile.timeLeft = CombinedHooks.TotalUseTime(Owner.HeldItem.useTime, Owner, Owner.HeldItem);
		maxFireCounter = Projectile.timeLeft / 3;
	}

	public override void AI() {
		Projectile.Center = Owner.Center - ownerOffset;
		Projectile.velocity = Vector2.Zero;

		if (ShootTimer <= 0 && Collision.CanHit(Owner.Center, 0, 0, Projectile.Center, 0, 0) && numFired < 3 && Main.myPlayer == Projectile.owner) {
			Vector2 velocity = startVelocity * Main.rand.NextFloat(0.95f, 1.05f);
			velocity = velocity.RotatedByRandom(0.1f);
			var gasProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<VerdantGas>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
			gasProj.originalDamage = Projectile.originalDamage / 3;
			ShootTimer = maxFireCounter;
			numFired++;

			Rectangle dustSpawnBox = Projectile.Hitbox;
			dustSpawnBox.Inflate(4, 4);
			var dust = Dust.NewDustDirect(dustSpawnBox.TopLeft(), dustSpawnBox.Width, dustSpawnBox.Height, DustID.GreenFairy, Alpha: Main.rand.Next(0, 50), Scale: Main.rand.NextFloat(0.6f, 1f));
			dust.velocity = startVelocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.01f, 0.8f);
		}

		if (Main.rand.NextBool(2)) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenFairy, Alpha: Main.rand.Next(0, 50), Scale: Main.rand.NextFloat(0.6f, 0.8f));
			dust.velocity = startVelocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.01f, 0.8f);
		}

		ShootTimer--;
	}
}
