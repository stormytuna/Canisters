﻿using System.Collections.Generic;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Projectiles.LunarCanister;

public class LunarShot : ModProjectile
{
	private readonly List<int> _hitNpcs = new();

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 1;
		ProjectileID.Sets.TrailCacheLength[Type] = 5;
	}

	public override void SetDefaults() {
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 8 * 60;

		Projectile.friendly = true;
		Projectile.penetrate = 3;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = -1;
	}

	public override void AI() {
		Lighting.AddLight(Projectile.Center, CanisterHelpers.GetCanisterColor<Items.Canisters.LunarCanister>().ToVector3());

		Projectile.rotation = Projectile.velocity.ToRotation() + PiOver2;

		for (int i = 0; i < 2; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Vortex);
			dust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.2f, 0.8f);
			dust.velocity += Projectile.velocity * 0.5f;
			dust.scale = Main.rand.NextFloat(0.9f, 1.3f);
			dust.noGravity = true;
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LunarMark>(), Projectile.damage / 3, 0f, Main.myPlayer);

		_hitNpcs.Add(target.whoAmI);
		NPC nextTarget = NPCHelpers.FindRandomNearbyNPC(50f * 16f, Projectile.Center, true, _hitNpcs);
		if (nextTarget is not null) {
			Projectile.velocity = Projectile.DirectionTo(nextTarget.Center) * Projectile.velocity.Length();
			Projectile.netUpdate = true;
		}
	}

	public override void OnKill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Vortex, 5, 1f, 3f, noGravity: true);
	}
}
