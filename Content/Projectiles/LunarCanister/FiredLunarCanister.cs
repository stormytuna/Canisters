using Terraria.Audio;

namespace Canisters.Content.Projectiles.LunarCanister;

public class FiredLunarCanister : BaseFiredCanisterProjectile
{
	public override string Texture {
		get => "Canisters/Content/Items/Canisters/LunarCanister";
	}

	public override void Explode() {
		if (Projectile.owner == Main.myPlayer) {
			Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<LunarLightningEmitter>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}

		SoundEngine.PlaySound(SoundID.DD2_GoblinBomb, Projectile.Center);
	}
}
