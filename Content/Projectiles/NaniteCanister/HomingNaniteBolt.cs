using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Projectiles.NaniteCanister;

public class HomingNaniteBolt : ModProjectile
{
	private static readonly VertexStrip _vertexStrip = new();

	private NPC _target = null;
	private int _timer = 0;

	public override void SetStaticDefaults() {
		ProjectileID.Sets.TrailingMode[Type] = 5;
		ProjectileID.Sets.TrailCacheLength[Type] = 10;
	}

	public override void SetDefaults() {
		Projectile.width = 8;
		Projectile.height = 8;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 3 * 60;

		Projectile.friendly = true;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		_timer++;
		Projectile.friendly = false;

		Projectile.rotation = Projectile.velocity.ToRotation() + PiOver2;

		if (Main.rand.NextBool()) {
			Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GolfPaticle);
			dust.color = CanisterHelpers.GetCanisterColor<Items.Canisters.NaniteCanister>();
			dust.position += Projectile.velocity * 0.6f;
			dust.velocity *= Main.rand.NextFloat(0.1f, 0.3f);
			dust.scale = Main.rand.NextFloat(0.8f, 1.2f);
			dust.noGravity = true;
		}

		if (_timer >= 10) {
			Projectile.friendly = true;
			if (_target is null || !_target.active) {
				_target = NPCHelpers.FindClosestNPC(25f * 16f, Projectile.Center);
			}
		}

		if (_target is null) {
			return;
		}

		MathHelpers.SmoothHoming(Projectile, _target.Center, 0.5f, 16f, _target.velocity);
	}

	public override bool OnTileCollide(Vector2 oldVelocity) {
		Projectile.velocity = Projectile.velocity.BounceOffTiles(oldVelocity);

		return false;
	}

	public override bool PreDraw(ref Color lightColor) {
		MiscShaderData miscShaderData = GameShaders.Misc["LightDisc"];
		miscShaderData.UseSaturation(-2.8f);
		miscShaderData.UseOpacity(2f);
		miscShaderData.Apply();

		_vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColor, StripHalfWidth, (Projectile.Size / 2f) - Main.screenPosition);
		_vertexStrip.DrawTrail();

		Main.pixelShader.CurrentTechnique.Passes[0].Apply();

		return true;
	}

	private Color StripColor(float progress) {
		float inverse = 1f - progress;
		Color result = CanisterHelpers.GetCanisterColor<Items.Canisters.NaniteCanister>() * float.Pow(inverse, 4) * 0.5f;
		return result with { A = 0 };
	}

	private float StripHalfWidth(float progress) {
		return 2f;
	}
}
