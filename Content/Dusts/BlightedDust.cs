namespace Canisters.Content.Dusts;

public class BlightedDust : ModDust
{
	public override void OnSpawn(Dust dust) {
		dust.noGravity = true;
		dust.alpha = 100;
	}

	public override bool Update(Dust dust) {
		Lighting.AddLight(dust.position, 0f, 0.3f, 0f);
		dust.scale *= 0.96f;
		if (dust.scale < 0.5f) {
			dust.active = false;
		}
		
		return true;
	}

	public override Color? GetAlpha(Dust dust, Color lightColor) {
		return Color.Lerp(lightColor, Color.White, 0.5f);
	}
}
