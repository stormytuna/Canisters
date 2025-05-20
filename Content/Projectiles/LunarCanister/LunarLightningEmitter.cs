using Canisters.Common;
using Canisters.DataStructures;
using Canisters.Helpers;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Canisters.Content.Projectiles.LunarCanister;

public class LunarLightningEmitter : ModProjectile
{
	private ref float TimerForAttack {
		get => ref Projectile.ai[0];
	}

	private static Asset<Texture2D> _maskTexture;
	private static Asset<Texture2D> _maskBackgroundTexture;
	private static Asset<Effect> _maskEffect;

	private static RenderTarget2D _maskRenderTarget;

	private bool _firstFrame = true;
	private int _totalTimeLeft;
	private Vector2 _wibbleOffset;

	public override void Load() {
		if (Main.dedServ) {
			return;
		}

		_maskTexture = Mod.Assets.Request<Texture2D>("Content/Projectiles/LunarCanister/LunarLightningEmitter_Mask");
		_maskBackgroundTexture = Mod.Assets.Request<Texture2D>("Content/Projectiles/LunarCanister/LunarLightningEmitter_MaskBackground");
		_maskEffect = Mod.Assets.Request<Effect>("Assets/Effects/MaskEffect");

		Main.QueueMainThreadAction(() => ResizeRenderTarget(Main.ScreenSize));

		Main.OnResolutionChanged += resolution => { ResizeRenderTarget(resolution.ToPoint()); };
		On_Main.CheckMonoliths += PrepareRenderTarget;
	}

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

	public static void LightningBoltEffects(Vector2 start, Vector2 end) {
		Vector2 offset = start.DirectionTo(end) * 24f;
		DustHelpers.MakeLightningDust(start + offset, end, DustID.Vortex, 0.8f);
		DustHelpers.MakeDustExplosion(end, 4f, DustID.Vortex, 5, 0.5f, 2f, noGravity: true);
		SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap with { Volume = 0.3f, PitchRange = (0.5f, 0.8f), MaxInstances = 5, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, start);
		SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap with { Volume = 0.3f, PitchRange = (-0.2f, 0.3f), MaxInstances = 5, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest }, start);
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;

			_totalTimeLeft = Projectile.timeLeft;
			Projectile.rotation = Main.rand.NextRadian();
			DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Vortex, 10, 1f, 3f, noGravity: true);
		}

		TimerForAttack++;

		Projectile.rotation += 0.05f;

		_wibbleOffset = Main.rand.NextVector2Unit(8f, 8f);

		if (Projectile.timeLeft < 10) {
			Projectile.scale = float.Lerp(0.1f, 1f, (Projectile.timeLeft) / 10f);
			return;
		}

		if (Projectile.timeLeft > (_totalTimeLeft - 10)) {
			Projectile.scale = float.Lerp(1f, 0.1f, (Projectile.timeLeft - (_totalTimeLeft - 10)) / 10f);
			return;
		}

		Projectile.scale = 1f + Main.rand.NextFloat(0.05f);

		if (TimerForAttack >= 15 && Main.myPlayer == Projectile.owner) {
			float radiusMult = Main.LocalPlayer.GetModPlayer<CanisterModifiersPlayer>().CanisterLaunchedExplosionRadiusMult;
			System.Collections.Generic.List<NPC> closeNPCs = NpcHelpers.FindNearbyNPCs(30f * 16f * radiusMult, Projectile.Center, true);
			if (closeNPCs.Count > 0) {
				TimerForAttack = 0f;

				NPC nextTarget = Main.rand.Next(closeNPCs);
				NPC.HitInfo hitInfo = new() {
					Damage = Projectile.damage / 2,
					Knockback = 0f,
					DamageType = DamageClass.Ranged,
					HitDirection = (nextTarget.Center.X > Projectile.Center.X).ToDirectionInt(),
				};
				nextTarget.StrikeNPC(hitInfo);

				LightningBoltEffects(Projectile.Center, nextTarget.Center);

				if (Main.netMode != NetmodeID.SinglePlayer) {
					NetMessage.SendStrikeNPC(nextTarget, in hitInfo, Main.myPlayer);
					BroadcastLightningBoltSync(-1, Main.myPlayer, Projectile.Center, nextTarget.Center);
				}
			}
		}

		for (int i = 0; i < 3; i++) {
			float distance = Main.rand.NextFloat(32f, 38f);
			Vector2 offset = Main.rand.NextVector2CircularEdge(distance, distance);
			Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, ModContent.DustType<LunarLightningEmitterDust>());
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
			SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap with { Volume = 0.2f, PitchRange = (0.5f, 1f), MaxInstances = 0 }, Projectile.Center);
		}
	}

	public static void BroadcastLightningBoltSync(int toWho, int fromWho, Vector2 start, Vector2 end) {
		ModPacket packet = ModContent.GetInstance<Canisters>().GetPacket();
		packet.Write((byte)Canisters.MessageType.LunarLightningEmitterLightningBolt);
		packet.WriteVector2(start);
		packet.WriteVector2(end);
		packet.Send(toWho, fromWho);

	}

	public override void OnKill(int timeLeft) {
		DustHelpers.MakeDustExplosion(Projectile.Center, 10f, DustID.Vortex, 10, 1f, 3f, noGravity: true);
	}

	private static void ResizeRenderTarget(Point screenRes) {
		_maskRenderTarget?.Dispose();
		_maskRenderTarget = new RenderTarget2D(Main.graphics.GraphicsDevice, screenRes.X, screenRes.Y);
	}

	private static void PrepareRenderTarget(On_Main.orig_CheckMonoliths orig) {
		orig();

		if (Main.dedServ || Main.gameMenu) {
			return;
		}

		RenderTargetBinding[] bindings = Main.graphics.GraphicsDevice.GetRenderTargets();
		Main.graphics.GraphicsDevice.SetRenderTarget(_maskRenderTarget);
		Main.graphics.GraphicsDevice.Clear(Color.Transparent);
		
		Main.spriteBatch.Begin(SpriteBatchParams.Default with { TransformMatrix = Matrix.Identity });

		foreach (Projectile projectile in Main.ActiveProjectiles) {
			if (projectile.ModProjectile is not LunarLightningEmitter modProj) {
				continue;
			}

			DrawData drawData = new() {
				texture = _maskTexture.Value,
				position = (projectile.Center + modProj._wibbleOffset - Main.screenPosition).Floor(),
				sourceRect = _maskTexture.Frame(),
				color = Color.White,
				rotation = projectile.rotation,
				scale = new Vector2(projectile.scale),
				origin = _maskTexture.Size() / 2f,
			};
			drawData.Draw(Main.spriteBatch);
		}

		Main.spriteBatch.End();
		Main.graphics.GraphicsDevice.SetRenderTargets(bindings);
	}

	public override bool PreDraw(ref Color lightColor) {
		Texture2D texture = TextureAssets.Projectile[Type].Value;

		DrawData drawData = new() {
			texture = texture,
			position = (Projectile.Center + _wibbleOffset - Main.screenPosition).Floor(),
			sourceRect = texture.Frame(),
			color = Color.White,
			rotation = Projectile.rotation,
			scale = new Vector2(Projectile.scale),
			origin = texture.Size() / 2f,
		};
		drawData.Draw(Main.spriteBatch);
		
		Main.spriteBatch.TakeSnapshotAndEnd(out SpriteBatchParams snapshot);
		Main.spriteBatch.Begin(SpriteBatchParams.Default with { Effect = _maskEffect.Value });

		Main.graphics.GraphicsDevice.Textures[1] = _maskBackgroundTexture.Value;
		
		Vector2 positionOnScreen = (Projectile.Center - Main.screenPosition) / Main.ScreenSize.ToVector2();
		_maskEffect.Value.Parameters["position"].SetValue(positionOnScreen);
		_maskEffect.Value.Parameters["screenResolution"].SetValue(new Vector2((float)Main.screenWidth / (Main.screenWidth + Main.screenHeight), (float)Main.screenHeight / (Main.screenWidth + Main.screenHeight)));

		Main.spriteBatch.Draw(_maskRenderTarget, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

		Main.spriteBatch.Restart(snapshot);

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
