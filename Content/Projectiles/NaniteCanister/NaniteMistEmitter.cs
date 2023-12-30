﻿using Canisters.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.NaniteCanister;

// TODO: Probably abstract this and VerdantGasEmitter into base PlayerCenteredEmitter class
public class NaniteMistEmitter : ModProjectile
{
	public override string Texture => CanisterHelpers.GetEmptyAssetString();

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
		Projectile.penetrate = -1;
	}

	private Vector2 ownerOffset;
	private Vector2 startVelocity;
	private int maxFireCounter;
	private int numFired;

	private Player Owner => Main.player[Projectile.owner];

	private ref float AI_FireCounter => ref Projectile.ai[0];

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
		if (AI_FireCounter <= 0 && Collision.CanHit(Owner.Center, 0, 0, Projectile.Center, 0, 0) && numFired < 3 && Main.myPlayer == Projectile.owner) {
			Vector2 velocity = startVelocity * Main.rand.NextFloat(0.95f, 1.05f);
			velocity = velocity.RotatedByRandom(0.1f);
			Projectile mistProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<NaniteMist>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
			mistProj.originalDamage = Projectile.originalDamage / 3;
			AI_FireCounter = maxFireCounter;
			numFired++;

			// TODO: dust?
		}

		// TODO: Dust?

		AI_FireCounter--;
	}
}
