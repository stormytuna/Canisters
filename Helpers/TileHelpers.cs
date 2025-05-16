namespace Canisters.Helpers;

public class TileHelpers
{
	public static float GetBrightness(Point point) {
		return Lighting.Brightness(point.X, point.Y);
	}

	public static float GetBrightness(Vector2 vector) {
		return GetBrightness(vector.ToTileCoordinates());
	}
}
