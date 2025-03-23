﻿using Canisters.Content.Dusts;
using Canisters.Helpers;

namespace Canisters.Content.Projectiles.BlightedCanister;

public class BlightedBolt : ModProjectile
{
	private bool _firstFrame = true;

	private Vector2 _startLocation;

	public override string Texture {
		get => CanisterHelpers.GetEmptyAssetString();
	}

	public override void SetDefaults() {
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.aiStyle = -1;
		Projectile.timeLeft = 200;
		Projectile.extraUpdates = 200;

		Projectile.friendly = true;
		Projectile.penetrate = 5;
		Projectile.DamageType = DamageClass.Ranged;
	}

	public override void AI() {
		if (_firstFrame) {
			_firstFrame = false;
			_startLocation = Projectile.Center;
		}
	}

	public override bool? CanCutTiles() {
		return false;
	}

	public static void MakeDustLightningBolt(Vector2 start, Vector2 end) {
		DustHelpers.MakeLightningDust(start, end, ModContent.DustType<BlightedDust>(), 1.4f, 30f, 0.4f);
		DustHelpers.MakeDustExplosion(end, 2f, ModContent.DustType<BlightedDust>(), 5, 0f, 8f, 100, 150, 1f, 1.2f, true, true, true);
	}

	public override void OnKill(int timeLeft) {
		if (Main.myPlayer != Projectile.owner) {
			return;
		}

		MakeDustLightningBolt(_startLocation, Projectile.Center);
		if (Main.netMode == NetmodeID.MultiplayerClient) {
			BroadcastLightningBoltSync(-1, Main.myPlayer, _startLocation, Projectile.Center);
		}
	}

	public static void BroadcastLightningBoltSync(int toWho, int fromWho, Vector2 start, Vector2 end) {
		ModPacket packet = ModContent.GetInstance<Canisters>().GetPacket();
		packet.Write((byte)Canisters.MessageType.BlightedBoltLightningBolt);
		packet.WriteVector2(start);
		packet.WriteVector2(end);
		packet.Send(toWho, fromWho);
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
		target.AddBuff(BuffID.CursedInferno, 600);
	}
}
