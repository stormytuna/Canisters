using Canisters.Content.Dusts;
using Canisters.Helpers;
using Terraria.DataStructures;

namespace Canisters.Content.Projectiles.NaniteCanister;

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
		_ownerOffset = Owner.Center - Projectile.Center;
		_startVelocity = Projectile.velocity;

		Projectile.timeLeft = CombinedHooks.TotalUseTime(Owner.HeldItem.useTime, Owner, Owner.HeldItem);
		_maxFireCounter = Projectile.timeLeft / 3;
	}

	public override void AI() {
		Projectile.Center = Owner.Center - _ownerOffset;
		Projectile.velocity = Vector2.Zero;

		if (ShootTimer <= 0 && Collision.CanHit(Owner.Center, 0, 0, Projectile.Center, 0, 0) && _numFired < 3 && Main.myPlayer == Projectile.owner) {
			Vector2 velocity = _startVelocity * Main.rand.NextFloat(0.95f, 1.05f);
			velocity = velocity.RotatedByRandom(0.2f);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ModContent.ProjectileType<Nanites>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner);
			ShootTimer = _maxFireCounter;
			_numFired++;
			
			Rectangle dustSpawnBox = Projectile.Hitbox;
			dustSpawnBox.Inflate(4, 4);
			var dust = Dust.NewDustDirect(dustSpawnBox.TopLeft(), dustSpawnBox.Width, dustSpawnBox.Height, ModContent.DustType<NaniteDust>(), Alpha: Main.rand.Next(0, 50), Scale: Main.rand.NextFloat(0.6f, 1f));
			dust.velocity = _startVelocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.01f, 0.8f);
		}
		
		if (Main.rand.NextBool(2)) {
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<NaniteDust>(), Alpha: Main.rand.Next(0, 50), Scale: Main.rand.NextFloat(0.6f, 0.8f));
			dust.velocity = _startVelocity.RotatedByRandom(0.4f) * Main.rand.NextFloat(0.01f, 0.8f);
		}

		ShootTimer--;
	}
}
