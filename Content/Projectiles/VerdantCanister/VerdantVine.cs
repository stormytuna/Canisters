using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Projectiles.VerdantCanister;

// TODO: Change the actual sprite of this so it's not /just/ the vilethorn
/// <summary>
///     The vines our verdant canister releases when it explodes
/// </summary>
public class VerdantVine : ModProjectile
{
    /// <summary>
    ///     The number of vine projectiles to create after this one. If this is 0, the texture will be replaced with the tip texture
    /// </summary>
    private ref float AI_Vines => ref Projectile.ai[0];

    /// <summary>
    ///     The amount of radians to rotate the next vine projectile
    /// </summary>
    private ref float AI_Rotation => ref Projectile.ai[1];

	private int timer;

	public override void SetDefaults() {
		// Base stats
		Projectile.width = 30;
		Projectile.height = 32;
		Projectile.alpha = 64;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override string Texture => AI_Vines == 0 ? "Canisters/Content/Projectiles/VerdantCanister/VerdantVine_Tip" : "Canisters/Content/Projectiles/VerdantCanister/VerdantVine_Base";

	private bool firstFrame = true;

	public override void AI() {
		// Flip our sprite sometimes for a bit of variety
		if (firstFrame) {
			firstFrame = false;
			Projectile.spriteDirection = MathF.Sign(Main.rand.NextFloat(-1f, 1f));
		}

		// Make our projectile disappear gradually
		Projectile.alpha += 15;
		if (Projectile.alpha >= 255) {
			Projectile.Kill();
		}

		// Create our next vine
		if (timer == 3 && Projectile.owner == Main.myPlayer && AI_Vines > 0) {
			Vector2 offset = Vector2.UnitY.RotatedBy(Projectile.rotation) * -20f;
			Vector2 position = Projectile.Center + offset;
			Projectile nextVine = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), position, Vector2.Zero, Type, Projectile.damage, Projectile.knockBack, Projectile.owner, AI_Vines - 1, AI_Rotation);
			nextVine.rotation = Projectile.rotation + AI_Rotation;
		}

		// Dust
		if (Main.rand.NextBool(4)) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, Alpha: Main.rand.Next(100, 150), Scale: Main.rand.NextFloat(0.8f, 1.2f));
			dust.noGravity = true;
		}

		timer++;
	}

	public override void Kill(int timeLeft) {
		int numDust = Main.rand.Next(3, 6);
		for (int i = 0; i < numDust; i++) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, Alpha: Main.rand.Next(100, 150), Scale: Main.rand.NextFloat(0.8f, 1.2f));
			dust.noGravity = true;
		}
	}
}