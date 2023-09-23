﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace Canisters.Helpers;

public static class DustHelpers
{
	public static void MakeDustExplosion(Vector2 position, float spawnRadius, int dustType, int amount, float speed = 1f, int alpha = 0, float scale = 1f, bool noGravity = false, bool noLight = false, bool noLightEmmittance = false) => MakeDustExplosion(position, spawnRadius, dustType, amount, speed, speed, alpha, alpha, scale, scale, noGravity, noLight, noLightEmmittance);

	public static void MakeDustExplosion(Vector2 position, float spawnRadius, int dustType, int amount, float minSpeed, float maxSpeed, int alpha = 0, float scale = 1f, bool noGravity = false, bool noLight = false, bool noLightEmmittance = false) => MakeDustExplosion(position, spawnRadius, dustType, amount, minSpeed, maxSpeed, alpha, alpha, scale, scale, noGravity, noLight, noLightEmmittance);

	public static void MakeDustExplosion(Vector2 position, float spawnRadius, int dustType, int amount, float minSpeed, float maxSpeed, int minAlpha, int maxAlpha, float scale = 1f, bool noGravity = false, bool noLight = false, bool noLightEmmittance = false) => MakeDustExplosion(position, spawnRadius, dustType, amount, minSpeed, maxSpeed, minAlpha, maxAlpha, scale, scale, noGravity, noLight, noLightEmmittance);

	public static void MakeDustExplosion(Vector2 position, float spawnRadius, int dustType, int amount, float minSpeed, float maxSpeed, int minAlpha, int maxAlpha, float minScale, float maxScale, bool noGravity = false, bool noLight = false, bool noLightEmmittance = false) {
		for (int i = 0; i < amount; i++) {
			Vector2 spawnPosition = position + Main.rand.NextVector2Circular(spawnRadius, spawnRadius);
			Dust newDust = Dust.NewDustPerfect(spawnPosition, dustType);
			newDust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(minSpeed, maxSpeed);
			newDust.alpha = Main.rand.Next(minAlpha, maxAlpha);
			newDust.scale = Main.rand.NextFloat(minScale, maxScale);
			newDust.noGravity = noGravity;
			newDust.noLight = noLight;
			newDust.noLightEmittence = noLightEmmittance;
		}
	}

	/// <summary>
	///     Creates a lightning bolt made of dust from the source to the destination
	/// </summary>
	/// <param name="source">The source location, where the lightning bolt starts</param>
	/// <param name="dest">The destination location, where the lightning bolt ends</param>
	/// <param name="dustId">The dust id of the dust to create along the lightning bolt</param>
	/// <param name="scale">The scale of the dust</param>
	/// <param name="sway">How far away from the center line the zigzag offset is allowed to be</param>
	/// <param name="jagednessNumerator">How strictly the sway is moved back towards the center, usually don't make this higher than 2</param>
	public static void MakeLightningDust(Vector2 source, Vector2 dest, int dustId, float scale, float sway = 80f, float jagednessNumerator = 1f) {
		List<Vector2> dustPoints = GeneralHelpers.CreateLightningBolt(source, dest, sway, jagednessNumerator);

		for (int i = 1; i < dustPoints.Count; i++) {
			Vector2 start = dustPoints[i - 1];
			Vector2 end = dustPoints[i];
			float numDust = (end - start).Length() * 0.4f;

			for (int j = 0; j < numDust; j++) {
				float lerp = j / numDust;
				Vector2 dustPosition = Vector2.Lerp(start, end, lerp);

				Dust d = Dust.NewDustPerfect(dustPosition, dustId, Scale: scale);
				d.noGravity = true;
				d.velocity = Main.rand.NextVector2Circular(0.3f, 0.3f);
			}
		}
	}
}