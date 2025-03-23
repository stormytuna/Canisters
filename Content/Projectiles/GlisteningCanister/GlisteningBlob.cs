﻿using Canisters.Helpers;
using Terraria.DataStructures;

namespace Canisters.Content.Projectiles.GlisteningCanister;

public class GlisteningBlob : ModProjectile
{
	private bool _firstFrame = true;
	
	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 4;
	}

	public override void SetDefaults() {
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.aiStyle = -1;
		Projectile.extraUpdates = 1;

		Projectile.friendly = true;
		Projectile.penetrate = 3;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;
			Projectile.frame = Main.rand.Next(Main.projFrames[Type]);
			Projectile.direction = Main.rand.NextBool().ToDirectionInt();
		}
		
		Projectile.velocity.Y += 0.1f;
		Projectile.rotation += 0.125f * Projectile.direction;

		if (Main.rand.NextBool(3)) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Ichor, Alpha: Main.rand.Next(100, 200), Scale: Main.rand.NextFloat(1f, 1.2f));
			dust.noGravity = true;
			dust.noLight = true;
		}

		Lighting.AddLight(Projectile.Center, TorchID.Ichor);
	}

	public override Color? GetAlpha(Color lightColor) {
		return Color.White;
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		if (Projectile.velocity.X != oldVelocity.X) {
			Projectile.velocity.X = -oldVelocity.X;
		}

		if (Projectile.velocity.Y != oldVelocity.Y) {
			Projectile.velocity.Y = -oldVelocity.Y;
		}

		Projectile.velocity *= 0.8f;
		
		Projectile.penetrate--;

		DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.IchorTorch, 4, 0f, 4f, 80, 120, 0.8f, 1.2f, true);

		return false;
	}

	public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
		width = 8;
		height = 8;

		return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.Ichor, 600);
	}
}
