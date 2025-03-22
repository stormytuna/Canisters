using Canisters.Helpers;

namespace Canisters.Content.Projectiles.VerdantCanister;

public class VerdantVine : ModProjectile
{
	private bool firstFrame = true;
	private int timer;

	/// <summary>
	///     The number of vine projectiles to create after this one. If this is 0, the texture will be replaced with the tip
	///     texture
	/// </summary>
	private ref float NumVines {
		get => ref Projectile.ai[0];
	}

	/// <summary>
	///     The amount of radians to rotate the next vine projectile
	/// </summary>
	private ref float NextVineRotation {
		get => ref Projectile.ai[1];
	}

	public override string Texture {
		get => NumVines == 0
			? "Canisters/Content/Projectiles/VerdantCanister/VerdantVine_Tip"
			: "Canisters/Content/Projectiles/VerdantCanister/VerdantVine_Base";
	}

	public override void SetDefaults() {
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.alpha = 64;

		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		if (firstFrame) {
			firstFrame = false;
			Projectile.spriteDirection = Main.rand.NextBool().ToDirectionInt();
		}

		Projectile.alpha += 15;
		if (Projectile.alpha >= 255) {
			Projectile.Kill();
		}

		if (timer == 3 && Projectile.owner == Main.myPlayer && NumVines > 0) {
			Vector2 offset = Vector2.UnitY.RotatedBy(Projectile.rotation) * -20f;
			Vector2 position = Projectile.Center + offset;
			var nextVine = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), position, Vector2.Zero, Type, Projectile.damage, Projectile.knockBack, Projectile.owner, NumVines - 1, NextVineRotation);
			nextVine.originalDamage = Projectile.originalDamage;
			nextVine.rotation = Projectile.rotation + NextVineRotation;
		}

		if (Main.rand.NextBool(4)) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, Alpha: Main.rand.Next(100, 150), Scale: Main.rand.NextFloat(0.8f, 1.2f));
			dust.noGravity = true;
		}

		timer++;
	}

	public override void Kill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 5f, DustID.Grass, Main.rand.Next(2, 4), 0f, 1f, 100, 150, 0.8f, 1.2f, true);
	}
}
