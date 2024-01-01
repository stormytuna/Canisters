using Microsoft.Xna.Framework;
using Terraria;

namespace Canisters.Helpers;

public class TileHelpers
{
	public static float GetBrightness(Point point) => Lighting.Brightness(point.X, point.Y);

	public static float GetBrightness(Vector2 vector) => GetBrightness(vector.ToTileCoordinates());
}
