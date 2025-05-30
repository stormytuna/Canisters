namespace Canisters.Content.Dusts;

public class NaniteDust : ModDust
{
	public override void OnSpawn(Dust dust) {
		dust.noGravity = true;
		dust.alpha = 50;
	}

	public override bool Update(Dust dust) {
		Lighting.AddLight(dust.position, 0.0f, 0f, 0.2f);
		dust.rotation = Main.rand.NextRadian();

		if (dust.customData is Projectile { active: true } projectile) {
			dust.position += projectile.velocity; 
		}

		dust.scale *= 0.96f;
		if (dust.scale < 0.5f) {
			dust.active = false;
		}

		return true;
	}
}
