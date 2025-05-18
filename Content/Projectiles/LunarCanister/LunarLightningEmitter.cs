using System;
using System.Collections.Generic;
using Canisters.Common;
using Canisters.Helpers;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;

namespace Canisters.Content.Projectiles.LunarCanister;

public class LunarLightningEmitter : ModProjectile
{
	private ref float Timer {
		get => ref Projectile.ai[0];
	}

	/*
	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}
	*/

	public override void SetDefaults() {
		Projectile.width = 60;
		Projectile.height = 60;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 120;
		Projectile.tileCollide = false;
	}

	public override void OnSpawn(IEntitySource source) {
		Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 3f;
	}

	private bool _firstFrame = true;

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;
			Projectile.rotation = Main.rand.NextRadian();
			DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Vortex, 10, 1f, 3f, noGravity: true);
		}
		
		Timer++;
		
		Projectile.rotation += 0.05f;
		
		// TODO: make this and the lunar mark not rely on timeleft when starting
		if (Projectile.timeLeft < 10) {
			Projectile.scale = float.Lerp(0.1f, 1f, (Projectile.timeLeft) / 10f);
			return;
		}

		if (Projectile.timeLeft > 110) {
			Projectile.scale = float.Lerp(1f, 0.1f, (Projectile.timeLeft - 110) / 10f);
			return;
		}

		// TODO: MP compat
		if (Timer >= 15) {
			var closeNPCs = NpcHelpers.FindNearbyNPCs(30f * 16f * Main.LocalPlayer.GetModPlayer<CanisterModifiersPlayer>().CanisterLaunchedExplosionRadiusMult, Projectile.Center, true);
			if (closeNPCs.Count > 0) {
				Timer = 0f;
				
				var nextTarget = Main.rand.Next(closeNPCs);
				nextTarget.StrikeNPC(new NPC.HitInfo {
					Damage = Projectile.damage,
					Knockback = Projectile.knockBack,
					DamageType = DamageClass.Ranged,
					HitDirection = (nextTarget.Center.X > Projectile.Center.X).ToDirectionInt(),
				});
				
				DustHelpers.MakeLightningDust(Projectile.Center, nextTarget.Center, DustID.Vortex, 0.8f);
			}
		}

		for (int i = 0; i < 3; i++) {
			float distance = Main.rand.NextFloat(32f, 38f);
			Vector2 offset = Main.rand.NextVector2CircularEdge(distance, distance);
			var dust = Dust.NewDustPerfect(Projectile.Center + offset, ModContent.DustType<LunarLightningEmitterDust>());
			dust.customData = Projectile;
			if (Main.rand.NextBool(2, 3)) {
				dust.velocity = offset.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(0.5f, 2f);
			}
			else {
				dust.velocity *= 0.1f;
			}
		}

		if (Main.rand.NextBool(18)) {
			Vector2 offset = Main.rand.NextVector2Unit() * Main.rand.NextFloat(80f, 120f);
			DustHelpers.MakeLightningDust(Projectile.Center + (offset * 0.2f), Projectile.Center + offset, DustID.Vortex, Main.rand.NextFloat(0.5f, 0.8f), 20f, 1.2f);
		}
	}

	public override void OnKill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Vortex, 10, 1f, 3f, noGravity: true);
	}

	private static Asset<Texture2D> MaskTexture;
	private static Asset<Texture2D> MaskBackgroundTexture;
	private static Asset<Effect> MaskEffect;
	private static RenderTarget2D MaskRenderTarget;

	public override void Load() {
		MaskTexture = Mod.Assets.Request<Texture2D>("Content/Projectiles/LunarCanister/LunarLightningEmitter_Mask");
		MaskBackgroundTexture = Mod.Assets.Request<Texture2D>("Content/Projectiles/LunarCanister/LunarLightningEmitter_MaskBackground");
		MaskEffect = Mod.Assets.Request<Effect>("Assets/Effects/Effect");
		
		Main.QueueMainThreadAction(() => MaskRenderTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth, Main.screenHeight));
		Main.OnResolutionChanged += resolution => {
			MaskRenderTarget.Dispose();
			MaskRenderTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, (int)resolution.X
				, (int)resolution.Y, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
		};
		
		On_Main.CheckMonoliths += static orig => {
			orig();

			if (Main.dedServ) {
				return;
			}
			
			var bindings = Main.graphics.GraphicsDevice.GetRenderTargets();
			Main.graphics.GraphicsDevice.SetRenderTarget(MaskRenderTarget);
			Main.graphics.GraphicsDevice.Clear(Color.Transparent);
			
			Main.spriteBatch.Begin(default, default, Main.DefaultSamplerState, default, RasterizerState.CullNone, default);

			foreach (var projectile in Main.ActiveProjectiles) {
				if (projectile.ModProjectile is not LunarLightningEmitter) {
					continue;
				}
				
				var drawData = new DrawData {
					texture = MaskTexture.Value,
					position = (projectile.Center - Main.screenPosition).Floor(),
					sourceRect = MaskTexture.Frame(),
					color = Color.White,
					rotation = projectile.rotation,
					scale = new Vector2(projectile.scale),
					origin = MaskTexture.Size() / 2f,
				};
				drawData.Draw(Main.spriteBatch);
			}
			
			Main.spriteBatch.End();
			Main.graphics.GraphicsDevice.SetRenderTargets(bindings);
		};
	}

	public override bool PreDraw(ref Color lightColor) {
		var texture = TextureAssets.Projectile[Type].Value;

		var drawData = new DrawData {
			texture = texture,
			position = (Projectile.Center - Main.screenPosition).Floor(),
			sourceRect = texture.Frame(),
			color = Color.White,
			rotation = Projectile.rotation,
			scale = new Vector2(Projectile.scale),
			origin = texture.Size() / 2f,
		};
		drawData.Draw(Main.spriteBatch);
		
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, MaskEffect.Value, Main.GameViewMatrix.TransformationMatrix);

		Main.graphics.GraphicsDevice.Textures[1] = MaskBackgroundTexture.Value;
		Vector2 positionOnScreen = (Projectile.Center - Main.screenPosition) / Main.ScreenSize.ToVector2();
		MaskEffect.Value.Parameters["position"].SetValue(positionOnScreen);
		MaskEffect.Value.Parameters["screenResolution"].SetValue(new Vector2((float)Main.screenWidth / (Main.screenWidth + Main.screenHeight), (float)Main.screenHeight / (Main.screenWidth + Main.screenHeight)));
		
		Main.spriteBatch.Draw(MaskRenderTarget, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
		
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		
		return false;
	}
}

public class LunarLightningEmitterDust : ModDust
{
	public override string Texture { get => "Terraria/Images/Dust"; }

	public override void OnSpawn(Dust dust) {
		dust.frame = DustHelpers.FrameVanillaDust(DustID.Vortex);
		dust.noGravity = true;
	}

	public override bool Update(Dust dust) {
		dust.position += dust.velocity;

		if (dust.customData is Projectile projectile) {
			dust.position += projectile.position - projectile.oldPosition;
		}

		dust.scale *= 0.92f;
		if (dust.scale < 0.2f) {
			dust.active = false;
		}

		return false;
	}
}
