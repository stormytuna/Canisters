using System;
using System.IO;
using Canisters.Helpers.Abstracts;

namespace Canisters;

public class Canisters : Mod
{
	internal enum MessageType : byte
	{
		CanisterExplosionVisuals,
	}

	public override void HandlePacket(BinaryReader reader, int whoAmI) {
		MessageType message = (MessageType)reader.ReadByte();

		switch (message) {
			case MessageType.CanisterExplosionVisuals:
				int canisterProjectileType = reader.Read7BitEncodedInt();
				if (ProjectileLoader.GetProjectile(canisterProjectileType) is not FiredCanisterProjectile canister) {
					Logger.Error($"Received type is not a canister projectile: {canisterProjectileType}");
					return;
				}
				
				Vector2 position = reader.ReadVector2();
				Vector2 velocity = reader.ReadVector2();
				
				if (Main.netMode == NetmodeID.MultiplayerClient) {
					canister.ReceiveExplosionSync(position, velocity);
					break;
				}
				
				canister.BroadcastExplosionSync(-1, whoAmI, canisterProjectileType, position, velocity);
				break;
			default:
				Logger.Error($"Unknown message type: {message}");
				return;
		}
	}
}
