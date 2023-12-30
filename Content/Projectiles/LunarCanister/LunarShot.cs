using System.Collections.Generic;
using Canisters.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.LunarCanister;

public class LunarShot : ModProjectile
{
	private const float TargetingRange = 80f * 16f;
	private const float Acceleration = 0.05f;
	private const float TopSpeed = 4f;

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 1;
		ProjectileID.Sets.TrailCacheLength[Type] = 50;
	}

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;
		Projectile.extraUpdates = 8;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.penetrate = 5;
		Projectile.DamageType = DamageClass.Ranged;
	}

	private AIState State {
		get => (AIState)Projectile.ai[0];
		set => Projectile.ai[0] = (float)value;
	}

	private NPC Target {
		get => Main.npc[(int)Projectile.ai[1]];
		set => Projectile.ai[1] = value.whoAmI;
	}

	private NPC LastTarget {
		get => Main.npc[(int)Projectile.ai[2]];
		set => Projectile.ai[2] = value.whoAmI;
	}

	public override void OnSpawn(IEntitySource source) {
		Projectile.ai[1] = Main.maxNPCs;
		Projectile.ai[2] = Main.maxNPCs;
	}

	public override void AI() {
		if (Main.rand.NextBool(4)) {
			Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Vortex);
			dust.noGravity = true;
		}

		if (State == AIState.Homing) {
			// Validate our target
			if (!Target.CanBeChasedBy()) {
				State = AIState.Idle;

				return;
			}

			GeneralHelpers.SmoothHoming(Projectile, Target.Center, Acceleration, TopSpeed, bufferZone: false);

			return;
		}

		// Clamp velocity
		if (Projectile.velocity.Length() > TopSpeed) {
			Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * TopSpeed;
		}

		// Try find new target
		List<int> excludedNPCs = new() {
			LastTarget.whoAmI
		};
		NPC closestValidNPC = NPCHelpers.FindClosestNPC(TargetingRange, Projectile.Center, excludedNPCs: excludedNPCs);
		if (closestValidNPC is not null) {
			Target = closestValidNPC;
			State = AIState.Homing;
		}
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<LunarExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
	}

	public override void Kill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 4f, DustID.Vortex, 8, 0f, 10f, noGravity: true);
	}

	public override Color? GetAlpha(Color lightColor) => CanisterHelpers.GetCanisterColor<Items.Canisters.LunarCanister>();

	public override bool PreDraw(ref Color lightColor) {
		Main.instance.LoadProjectile(Type);
		Texture2D texture = TextureAssets.Projectile[Type].Value;

		Vector2 drawOrigin = new(texture.Width / 2f, texture.Height / 2f);
		for (int i = 0; i < Projectile.oldPos.Length; i++) {
			Vector2 drawPosition = Projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
			Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length);
			float scale = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;
			Main.EntitySpriteDraw(texture, drawPosition, null, color, Projectile.rotation, drawOrigin, scale, SpriteEffects.None);
		}

		return true;
	}

	private enum AIState
	{
		Homing,
		Idle
	}
}
