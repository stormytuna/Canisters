namespace Canisters.Helpers;

public static class CollisionHelpers
{
	public static bool CanHit(Entity source, Vector2 targetPos, int targetWidth = 0, int targetHeight = 0) {
		return Collision.CanHit(source.position, source.width, source.height, targetPos, targetWidth, targetHeight);
	}

	public static bool CanHit(Vector2 source, Vector2 target) {
		return Collision.CanHit(source, 0, 0, target, 0, 0);
	}
}
