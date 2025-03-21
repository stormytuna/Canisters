using Canisters.Helpers;

namespace Canisters.Content.Dusts;

public class NaniteDust : ModDust
{
	public override void OnSpawn(Dust dust) {
		dust.noGravity = true;
		dust.alpha = 50;
	}

	public override bool Update(Dust dust) {
		Lighting.AddLight(dust.position, 0.2f, 0f, 0f);
		dust.rotation = Main.rand.NextRadian();

		if (dust.customData is Projectile { active: true } projectile) {
			dust.position = Main.rand.NextVector2Circular(projectile.width / 2, projectile.height / 2) +
			                projectile.Center;
			return false;
		}

		dust.scale *= 0.96f;
		if (dust.scale < 0.5f) {
			dust.active = false;
		}

		return true;
	}
}
