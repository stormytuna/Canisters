using Terraria.Audio;
using Terraria.GameContent;

namespace Canisters.Content.Projectiles.GhastlyCanister;

public class GhastlyExplosion : ModProjectile
{
	private const float LifeTime = 35;

	private bool _firstFrame = true;

	private ref float Timer {
		get => ref Projectile.ai[0];
	}

	public override void SetStaticDefaults() {
		Main.projFrames[Type] = 7;
	}

	public override void SetDefaults() {
		Projectile.width = 150;
		Projectile.height = 150;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;

		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;
		Projectile.usesIDStaticNPCImmunity = true;
		Projectile.idStaticNPCHitCooldown = 20;
	}

	public override Color? GetAlpha(Color lightColor) {
		return Color.Lerp(Color.White, lightColor, 0.5f);
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;
			
			Projectile.scale = Main.rand.NextFloat(0.8f, 1.2f);
			Projectile.spriteDirection = Main.rand.NextBool().ToDirectionInt();
			
			DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.DungeonSpirit, 14, 0f, 8f, 50, 120, 1f, 1.5f, true);
			DustHelpers.MakeDustExplosion(Projectile.Center, 8f, DustID.DungeonSpirit, 8, 4f, 14f, 70, 120, 1f, 1.3f, true);
			
			SoundEngine.PlaySound(SoundID.Item14 with { Volume = 0.2f, PitchRange = (0.7f, 0.9f), MaxInstances = 3, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest });
		}

		Projectile.frame = (int)(Main.projFrames[Type] * Timer / LifeTime);

		Projectile.velocity = Vector2.Zero;

		if (Timer >= LifeTime) {
			Projectile.Kill();
		}

		Timer++;
	}

	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
		modifiers.Knockback *= 0f;
	}

	public override bool PreDraw(ref Color lightColor) {
		var texture = TextureAssets.Projectile[Type].Value;
		var sourceRect = texture.Frame(verticalFrames: Main.projFrames[Type], frameY: Projectile.frame);
		var position = (Projectile.Center - Main.screenPosition).Floor();
		var effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
		Main.EntitySpriteDraw(texture, position, sourceRect, Projectile.GetAlpha(lightColor), Projectile.rotation, sourceRect.Size() / 2f, Projectile.scale, effects);
		return false;
	}
}
