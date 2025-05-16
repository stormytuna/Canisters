namespace Canisters.Content.Dusts;

public class VolatileCanisterDust : ModDust
{
	public override void OnSpawn(Dust dust) {
		dust.noGravity = true;
		dust.alpha = Main.rand.Next(120, 180);
		dust.scale = Main.rand.NextFloat(0.8f, 1f);
		dust.frame = new Rectangle(0, Main.rand.Next(4) * 10, 10, 10);
	}

	public override bool Update(Dust dust) {
		dust.position += dust.velocity;
		dust.rotation += dust.scale * 0.2f;

		dust.alpha += 3;
		dust.scale -= 0.01f;
		if (dust.alpha >= 255 || dust.scale < 0.2f) {
			dust.active = false;
		}

		return false;
	}
}
