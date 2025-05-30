using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace Canisters.Common;

public class ServerConfig : ModConfig
{
	public override ConfigScope Mode {
		get => ConfigScope.ServerSide;
	}

	public static ServerConfig Instance {
		get => ModContent.GetInstance<ServerConfig>();
	}
}
