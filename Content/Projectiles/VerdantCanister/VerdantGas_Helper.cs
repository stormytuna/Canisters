using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.VerdantCanister;

/// <summary>
///     Helps shoot the verdant gas. This projectile will sit on the weapons muzzle and fire 3 verdant gas projectiles over its lifetime
/// </summary>
public class VerdantGas_Helper : ModProjectile
{
	public override void SetDefaults() {
		// Base stats
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;

		// Weapon stats
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
	}

	private Vector2 ownerOffset;
	private Vector2 startVelocity;
	private int maxFireCounter;
	private int numFired;

	private ref Player Owner => ref Main.player[Projectile.owner];

	private ref float AI_FireCounter => ref Projectile.ai[0];

	public override void OnSpawn(IEntitySource source) {
		ownerOffset = Owner.Center - Projectile.Center;
		startVelocity = Projectile.velocity;

		CanisterUsingHeldProjectile heldProj = Main.projectile[Owner.heldProj].ModProjectile as CanisterUsingHeldProjectile;
		Projectile.timeLeft = heldProj.UseTimeAfterBuffs;
		maxFireCounter = Projectile.timeLeft / 3;
	}

	public override void AI() {
		// Make sure our projectile stays on the end of the gun
		Projectile.Center = Owner.Center - ownerOffset;
		Projectile.velocity = Vector2.Zero;

		// Actually shoot the projectile
		if (AI_FireCounter <= 0 && Collision.CanHit(Owner.Center, 0, 0, Projectile.Center, 0, 0) && numFired < 3 && Main.myPlayer == Projectile.owner) {
			Vector2 velocity = startVelocity * Main.rand.NextFloat(0.95f, 1.05f);
			velocity = velocity.RotatedByRandom(0.1f);
			Projectile.NewProjectileDirect(Terraria.Entity.InheritSource(Projectile), Projectile.Center, velocity, ModContent.ProjectileType<VerdantGas>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
			AI_FireCounter = maxFireCounter;
			numFired++;

			// Make some dust
			for (int i = 0; i < 5; i++) {
				if (Main.rand.NextBool()) {
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenFairy, Alpha: Main.rand.Next(0, 50), Scale: Main.rand.NextFloat(0.6f, 1f));
					dust.velocity = startVelocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.01f, 0.8f);
				}
			}
		}

		// Make some small dust
		// Nature energy dust
		if (Main.rand.NextBool(2)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenFairy, Alpha: Main.rand.Next(0, 50), Scale: Main.rand.NextFloat(0.6f, 0.8f));
			dust.velocity = startVelocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.01f, 0.8f);
		}

		AI_FireCounter--;
	}
}