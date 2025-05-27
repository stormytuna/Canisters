using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Dusts;

public class ToxicDust : ModDust
{
	public override string Texture {
		get => "Terraria/Images/Dust";
	}

	public override void OnSpawn(Dust dust) {
		dust.noGravity = true;
		dust.frame = DustHelpers.FrameVanillaDust(DustID.Venom);

		dust.velocity = Main.rand.NextVector2Circular(1f, 1f) * Main.rand.NextFloat(2f, 4f);
		dust.scale = Main.rand.NextFloat(0.8f, 1.5f);
		dust.alpha = Main.rand.Next(50, 100);
		dust.rotation = Main.rand.NextRadian();
	}

	public override bool Update(Dust dust) {
		if (dust.firstFrame) {
			dust.firstFrame = false;
			dust.customData = dust.scale;
		}

		dust.position += dust.velocity;
		dust.velocity *= 0.98f;
		dust.rotation += dust.velocity.Length() * 0.07f;

		if (dust.customData is float originalScale) {
			float time = (float)Main.timeForVisualEffects * 0.02f % 1;
			float scaleVariance = Lerp(0.7f, 1f, float.Sin(time * TwoPi));
			dust.scale = originalScale + scaleVariance;
		}

		dust.alpha += 2;
		if (dust.alpha >= 255) {
			dust.active = false;
		}

		return false;
	}
}
