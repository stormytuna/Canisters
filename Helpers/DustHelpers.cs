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
			//yield return newDust;
		}
	}
}