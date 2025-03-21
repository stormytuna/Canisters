using System;
using System.Collections.Generic;
using Terraria.Utilities;

namespace Canisters.Helpers;

public static class RandomHelpers
{
	// Adapted from here https://bitbucket.org/Superbest/superbest-random/src/f067e1dc014c31be62c5280ee16544381e04e303/Superbest%20random/RandomExtensions.cs#lines-19
	/// <summary>
	///     Generates normally distributed numbers. Each operation makes two Gaussians for the price of one, and apparently
	///     they can be cached or something for better performance, but who cares.
	/// </summary>
	/// <param name="r"></param>
	/// <param name="mu">Mean of the distribution</param>
	/// <param name="sigma">Standard deviation</param>
	/// <returns></returns>
	public static float NextGaussian(this UnifiedRandom r, float mu = 0, float sigma = 1) {
		float u1 = r.NextFloat();
		float u2 = r.NextFloat();

		float stdNormal = MathF.Sqrt(-2f * MathF.Log(u1)) * MathF.Sin(2f * MathHelper.Pi * u2);

		float normal = mu + (sigma * stdNormal);

		return normal;
	}

	/// <summary>
	///     Generates a list of angles generated within segments of a circle
	/// </summary>
	/// <param name="rand">The UnifiedRandom to use</param>
	/// <param name="numSegments">The number of segments, or the number of angles to generate</param>
	/// <param name="overlap">
	///     The amount of overlap between each segment. Note that this is the total overlap, not the overlap
	///     on each side of the segment
	/// </param>
	/// <param name="randomOffset">
	///     Whether or not a random offset will be added to the angles. If set to false, no offset is
	///     added
	/// </param>
	/// <returns></returns>
	public static List<float> NextSegmentedAngles(this UnifiedRandom rand, int numSegments, float overlap = 0f,
		bool randomOffset = true) {
		List<float> angles = new();

		// Build our list
		float offset = randomOffset ? rand.NextFloat(MathHelper.TwoPi) : 0f;
		for (int i = 0; i < numSegments; i++) {
			float angle = (i / (float)numSegments * MathHelper.TwoPi) + offset;
			angles.Add(angle);
		}

		// Randomly rotate our angles
		for (int i = 0; i < angles.Count; i++) {
			float rotationMax = (MathHelper.TwoPi / numSegments) + overlap;
			float rotation = rand.NextFloat(-rotationMax / 2f, rotationMax / 2f);
			angles[i] += rotation;
		}

		return angles;
	}

	/// <summary>
	///     Generates a random float between 0 and two pi
	/// </summary>
	public static float NextRadian(this UnifiedRandom rand) {
		return rand.NextFloat(MathHelper.TwoPi);
	}
}
