using Canisters.Helpers;
using Terraria.DataStructures;

namespace Canisters.Content.Projectiles.NaniteCanister;

// TODO: Probably abstract this and VerdantGasEmitter into base PlayerCenteredEmitter class
public class NaniteMistEmitter : ModProjectile
{
	private int _maxFireCounter;
	private int _numFired;

	private Vector2 _ownerOffset;
	private Vector2 _startVelocity;

	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}

	private Player Owner {
		get => Main.player[Projectile.owner];
	}

	private ref float AiFireCounter {
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
		_ownerOffset = Owner.Center - Projectile.Center;
		_startVelocity = Projectile.velocity;

		Projectile.timeLeft = CombinedHooks.TotalUseTime(Owner.HeldItem.useTime, Owner, Owner.HeldItem);
		_maxFireCounter = Projectile.timeLeft / 3;
	}

	public override void AI() {
		// Make sure our projectile stays on the end of the gun
		Projectile.Center = Owner.Center - _ownerOffset;
		Projectile.velocity = Vector2.Zero;

		// Actually shoot the projectile
		if (AiFireCounter <= 0 && Collision.CanHit(Owner.Center, 0, 0, Projectile.Center, 0, 0) && _numFired < 3 &&
		    Main.myPlayer == Projectile.owner) {
			Vector2 velocity = _startVelocity * Main.rand.NextFloat(0.95f, 1.05f);
			velocity = velocity.RotatedByRandom(0.1f);
			var mistProj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, velocity,
				ModContent.ProjectileType<NaniteMist>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
			mistProj.originalDamage = Projectile.originalDamage / 3;
			AiFireCounter = _maxFireCounter;
			_numFired++;

			// TODO: dust?
		}

		// TODO: Dust?

		AiFireCounter--;
	}
}
