using Canisters.Helpers;

namespace Canisters.Content.Projectiles.VerdantCanister;

public class VerdantVine : ModProjectile
{
	private bool _firstFrame = true;
	private int _timer;

	private ref float NumVines {
		get => ref Projectile.ai[0];
	}

	private ref float StartRotation {
		get => ref Projectile.ai[1];
	}

	private ref float AddedRotation {
		get => ref Projectile.ai[2];
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
		if (_firstFrame) {
			_firstFrame = false;
			Projectile.spriteDirection = Main.rand.NextBool().ToDirectionInt();
			Projectile.rotation = StartRotation;
		}

		Projectile.alpha += 15;
		if (Projectile.alpha >= 255) {
			Projectile.Kill();
		}

		if (_timer == 3 && Projectile.owner == Main.myPlayer && NumVines > 0) {
			Vector2 offset = Vector2.UnitY.RotatedBy(Projectile.rotation) * -20f;
			Vector2 position = Projectile.Center + offset;
			var nextVine = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), position, Vector2.Zero, Type, Projectile.damage, Projectile.knockBack, Projectile.owner, NumVines - 1, StartRotation + AddedRotation, AddedRotation);
		}

		if (Main.rand.NextBool(4)) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Grass, Alpha: Main.rand.Next(100, 150), Scale: Main.rand.NextFloat(0.8f, 1.2f));
			dust.noGravity = true;
		}

		_timer++;
	}

	public override void OnKill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 5f, DustID.Grass, Main.rand.Next(2, 4), 0f, 1f, 100, 150, 0.8f, 1.2f, true);
	}
}
