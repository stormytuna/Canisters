using Microsoft.Xna.Framework;
using Terraria;

namespace Canisters.Helpers;

public static class CollisionHelpers
{
	public static bool CanHit(Entity source, Vector2 targetPos, int targetWidth = 0, int targetHeight = 0) => Collision.CanHit(source.position, source.width, source.height, targetPos, targetWidth, targetHeight);

	public static bool CanHit(Vector2 source, Vector2 target) => Collision.CanHit(source, 0, 0, target, 0, 0);
}
