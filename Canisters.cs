using System.IO;
using System.Linq;
using Canisters.Content.Projectiles;
using Canisters.Content.Projectiles.BlightedCanister;

namespace Canisters;

public class Canisters : Mod
{
	public override void HandlePacket(BinaryReader reader, int whoAmI) {
		var message = (MessageType)reader.ReadByte();

		switch (message) {
			case MessageType.CanisterExplosionVisuals:
				int identity = reader.ReadInt32();
				Logger.Info($"Received identity: {identity}");
				Projectile projectile = Main.projectile.FirstOrDefault(x => x.ModProjectile is BaseFiredCanisterProjectile && x.identity == identity);
				if (projectile is null) {
					Logger.Error($"Couldn't find projectile with identity: {identity}");
					return;
				}

				var canister = projectile.ModProjectile as BaseFiredCanisterProjectile;

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

				BlightedBolt.MakeDustLightningBolt(start, end);
				break;
			default:
				Logger.Error($"Unknown message type: {message}");
				return;
		}
	}

	internal enum MessageType : byte
	{
		CanisterExplosionVisuals,
		BlightedBoltLightningBolt
	}
}
