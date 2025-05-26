using System.Collections.Generic;
using System.IO;
using System.Linq;
using Canisters.Content.Projectiles;
using Canisters.Content.Projectiles.BlightedCanister;
using Canisters.Content.Projectiles.LunarCanister;

namespace Canisters;

public class Canisters : Mod
{
	public override void HandlePacket(BinaryReader reader, int whoAmI) {
		MessageType message = (MessageType)reader.ReadByte();

		switch (message) {
			case MessageType.CanisterExplosionVisuals:
				int identity = reader.ReadInt32();
				Projectile projectile = Main.projectile.FirstOrDefault(x => x.ModProjectile is BaseFiredCanisterProjectile && x.identity == identity);
				if (projectile is null) {
					Logger.Error($"Couldn't find projectile with identity: {identity}");
					return;
				}

				BaseFiredCanisterProjectile canister = projectile.ModProjectile as BaseFiredCanisterProjectile;

				if (Main.netMode == NetmodeID.MultiplayerClient) {
					canister!.ReceiveExplosionSync();
					break;
				}

				canister!.BroadcastExplosionSync(-1, whoAmI, identity);
				break;

			case MessageType.BlightedBoltLightningBolt:
				Vector2 start = reader.ReadVector2();
				Vector2 end = reader.ReadVector2();

				if (Main.netMode == NetmodeID.Server) {
					BlightedBolt.BroadcastLightningBoltSync(-1, whoAmI, start, end);
					break;
				}

				BlightedBolt.LightningBoltEffects(start, end);
				break;

			case MessageType.LunarLightningEmitterLightningBolt:
				Vector2 start2 = reader.ReadVector2();
				Vector2 end2 = reader.ReadVector2();

				if (Main.netMode == NetmodeID.Server) {
					LunarLightningEmitter.BroadcastLightningBoltSync(-1, whoAmI, start2, end2);
					break;
				}

				LunarLightningEmitter.LightningBoltEffects(start2, end2);

				return;

			default:
				Logger.Error($"Unknown message type: {message}");
				return;
		}
	}

	internal enum MessageType : byte
	{
		CanisterExplosionVisuals,
		BlightedBoltLightningBolt,
		LunarLightningEmitterLightningBolt,
	}
}
