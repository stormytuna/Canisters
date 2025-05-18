using Canisters.Helpers;
using Terraria.GameContent;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Projectiles.LunarCanister;

public class LunarMark : ModProjectile
{
	private float _timer = 0f;
	private bool _firstFrame = true;
	private int _totalTimeLeft;

	public override void SetDefaults() {
		Projectile.width = 200;
		Projectile.height = 200;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 150;

		Projectile.friendly = true;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 20;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;
			_totalTimeLeft = Projectile.timeLeft;
		}

		Lighting.AddLight(Projectile.Center, CanisterHelpers.GetCanisterColor<Items.Canisters.LunarCanister>().ToVector3());

		Projectile.rotation += 0.05f;

		if (Projectile.timeLeft < 10) {
			Projectile.scale = float.Lerp(0f, 1f, (Projectile.timeLeft) / 10f);
			return;
		}

		if (Projectile.timeLeft > _totalTimeLeft - 10) {
			Projectile.scale = float.Lerp(1f, 0f, (Projectile.timeLeft - (_totalTimeLeft - 10)) / 10f);
			return;
		}

		_timer += 0.1f;
		float scaleOffset = float.Sin(_timer % TwoPi) * 0.1f;
		Projectile.scale = 1f + scaleOffset;

		Vector2 offset = Main.rand.NextVector2CircularEdge(120f, 120f);
		Vector2 position = Projectile.Center + offset;
		var dust = Dust.NewDustPerfect(position, DustID.Vortex);
		dust.noGravity = true;
		dust.velocity *= 0.1f;
		if (Main.rand.NextBool()) {
			dust.velocity = position.DirectionTo(Projectile.Center) * Main.rand.NextFloat(2f, 5f);
		}
	}

	public override bool PreDraw(ref Color lightColor) {
		var texture = TextureAssets.Projectile[Type].Value;
		var position = (Projectile.Center - Main.screenPosition).Floor();
		var origin = texture.Size() / 2f;

		Main.EntitySpriteDraw(texture, position, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);

		return false;
	}
}
