using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Canisters.Helpers.Abstracts;

public abstract class CanisterProjectile : ModProjectile
{
	/// <summary>
	///     Only called for the owner of the projectile
	/// </summary>
	public virtual void OnExplode() { }

	public virtual void SafeSetDefaults() { }
	public virtual void SafeOnHitNPC() { }
	public virtual bool SafeOnTileCollide(Vector2 oldVelocity) => false;

	public sealed override void SetDefaults() {
		SafeSetDefaults();

		// Base stats
		Projectile.width = 22;
		Projectile.height = 22;
		Projectile.aiStyle = 2;

		// Weapon stats
		Projectile.penetrate = 1;
		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public sealed override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		SafeOnHitNPC();
		TryExplode();
	}

	public sealed override bool OnTileCollide(Vector2 oldVelocity) {
		TryExplode();
		return SafeOnTileCollide(oldVelocity);
	}

	protected void TryExplode() {
		if (Projectile.owner != Main.myPlayer || Projectile.alpha == 255) {
			return;
		}

		MakeCanisterGore();
		OnExplode();
	}

	private void MakeCanisterGore() {
		ModGore canisterGore1 = Mod.Find<ModGore>("BrokenCanister_01");
		ModGore canisterGore2 = Mod.Find<ModGore>("BrokenCanister_02");
		Vector2 canisterVelocity = Main.rand.NextVector2Circular(3f, 3f);
		Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, canisterVelocity + Projectile.velocity * 0.5f, canisterGore1.Type);
		gore.timeLeft = 120;
		gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), Projectile.Center, -canisterVelocity + Projectile.velocity * 0.5f, canisterGore2.Type);
		gore.timeLeft = 120;
	}
}