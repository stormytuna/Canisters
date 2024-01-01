using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace Canisters.Helpers;

public static class MathHelpers
{
	public static int Ceiling(float fl) => (int)MathF.Ceiling(fl);

	/// <summary>
	///     Ease in interpolation between the start and end
	/// </summary>
	/// <param name="start">The starting value, will return this when amount == 0</param>
	/// <param name="end">The ending value, will return this when amount == 1</param>
	/// <param name="amount">The amount to lerp by</param>
	/// <param name="exponent">The exponent of the easing curve to use, larger values cause more easing</param>
	/// <returns>Returns the ease in interpolation between start and end</returns>
	public static float EaseIn(float start, float end, float amount, int exponent) {
		if (amount <= 0f) {
			return start;
		}

		if (amount >= 1f) {
			return end;
		}

		float amountExp = MathF.Pow(amount, exponent);
		return MathHelper.Lerp(start, end, amountExp);
	}

	// Stolen The Story of Red Cloud https://github.com/Zeodexic/tsorcRevamp/blob/main/tsorcRevampUtils.cs#L400
	/// <summary>
	///     Returns a vector pointing from a source, to a target, with a speed.
	///     Simplifies basic projectile, enemy dash, etc aiming calculations to a single call.
	///     If "ballistic" is true it adjusts for gravity. Default is 0.1f, may be stronger or weaker for some projectiles though.
	/// </summary>
	/// <param name="source">The start point of the vector</param>
	/// <param name="target">The end point it is aiming towards</param>
	/// <param name="speed">The length of the resulting vector</param>
	public static Vector2 GenerateTargetingVector(Vector2 source, Vector2 target, float speed) {
		Vector2 distance = target - source;
		distance.Normalize();
		return distance * speed;
	}

	public static Vector2 RotateTowards(Vector2 source, Vector2 target, Vector2 origin = default) {
		target -= origin;

		float length = source.Length();
		float targetAngle = target.ToRotation();
		return targetAngle.ToRotationVector2() * length;
	}

	public static List<Vector2> CreateLightningBolt(Vector2 source, Vector2 dest, float sway = 80f, float jaggednessNumerator = 1f) {
		List<Vector2> results = new();
		Vector2 tangent = dest - source;
		Vector2 normal = Vector2.Normalize(new Vector2(tangent.Y, -tangent.X));
		float length = tangent.Length();

		List<float> positions = new() {
			0
		};

		for (int i = 0; i < length / 16f; i++) {
			positions.Add(Main.rand.NextFloat());
		}

		positions.Sort();

		float jaggedness = jaggednessNumerator / sway;

		Vector2 prevPoint = source;
		float prevDisplacement = 0f;
		for (int i = 1; i < positions.Count; i++) {
			float pos = positions[i];

			// used to prevent sharp angles by ensuring very close positions also have small perpendicular variation.
			float scale = length * jaggedness * (pos - positions[i - 1]);

			// defines an envelope. Points near the middle of the bolt can be further from the central line.
			float envelope = pos > 0.95f ? 20 * (1 - pos) : 1;

			float displacement = Main.rand.NextFloat(-sway, sway);
			displacement -= (displacement - prevDisplacement) * (1 - scale);
			displacement *= envelope;

			Vector2 point = source + pos * tangent + displacement * normal;
			results.Add(point);
			prevPoint = point;
			prevDisplacement = displacement;
		}

		results.Add(prevPoint);
		results.Add(dest);
		results.Insert(0, source);

		return results;
	}
}
