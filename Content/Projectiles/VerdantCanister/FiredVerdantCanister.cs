using System.Collections.Generic;
using Canisters.Content.Items.Weapons;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Canisters.Content.Projectiles.VerdantCanister;

public class FiredVerdantCanister : BaseFiredCanisterProjectile
{
	private bool _buffFromLushSlingshot = false;

	public override int TimeBeforeGravityAffected {
		get => 15;
	}

	public override string Texture {
		get => "Canisters/Content/Items/Canisters/VerdantCanister";
	}

	public override void OnSpawn(IEntitySource source) {
		if (source is EntitySource_ItemUse { Item.ModItem: LushSlinger }) {
			_buffFromLushSlingshot = true;
		}
	}

	protected override void ExplosionEffect() {
		if (Main.myPlayer == Projectile.owner) {
			int numTotalVines = _buffFromLushSlingshot ? 6 : 4;
			List<float> startRots = Main.rand.NextSegmentedAngles(numTotalVines, 0.3f);
			for (int i = 0; i < numTotalVines; i++) {
				int numVines = _buffFromLushSlingshot ? Main.rand.Next(5, 8) : Main.rand.Next(3, 6);
				float vineRot = Main.rand.NextGaussian(0f, 0.15f);
				float startRot = startRots[i];
				int damage = _buffFromLushSlingshot ? (int)(Projectile.damage * 1.2f) : Projectile.damage;
				Vector2 offset = Vector2.UnitY.RotatedBy(startRot) * -20f;
				Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center + offset, Vector2.Zero, ModContent.ProjectileType<VerdantVine>(), damage / 3, 0f, Projectile.owner, numVines, startRot, vineRot);
			}

			Projectile.Explode(100, 100);
		}

		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Grass, Main.rand.Next(50, 65), 0f, 15f, 100, 150, 1f, 1.3f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Grass, Main.rand.Next(12, 20), 0f, 10f, 100, 150, 1.3f, 1.6f, true);
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Grass, Main.rand.Next(10, 15), 0f, 3f, 100, 150, 1f, 1.3f);
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.GreenFairy, Main.rand.Next(3, 5), 0f, 4f, 0, 50, 1.3f);

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb with { Volume = 0.6f, PitchRange = (0.6f, 0.8f) }, Projectile.Center);
	}
}
