using Canisters.Content.Items.Canisters;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Canisters.Common.Systems;

public class CanisterColorSystem : ModSystem
{
	// TODO: Finalise colours, possibly add some cycling between colours?
	public static Color Volatile => new(45, 144, 255, 255);
	public static Color Verdant => Color.Green;
	public static Color Glistening => Color.Yellow;
	public static Color Blighted => Color.Lime;
	public static Color Harmonic => Color.Purple;
	public static Color Nanite => Color.Red;
	public static Color Ghastly => Color.Cyan;
	public static Color Lunar => new(208, 253, 235);

	public static Color GetCanisterColor(int canisterItemId) {
		if (canisterItemId == ModContent.ItemType<VolatileCanister>()) {
			return Volatile;
		}

		if (canisterItemId == ModContent.ItemType<VerdantCanister>()) {
			return Verdant;
		}

		if (canisterItemId == ModContent.ItemType<GlisteningCanister>()) {
			return Glistening;
		}

		if (canisterItemId == ModContent.ItemType<BlightedCanister>()) {
			return Blighted;
		}

		if (canisterItemId == ModContent.ItemType<HarmonicCanister>()) {
			return Harmonic;
		}

		if (canisterItemId == ModContent.ItemType<NaniteCanister>()) {
			return Nanite;
		}

		if (canisterItemId == ModContent.ItemType<GhastlyCanister>()) {
			return Ghastly;
		}

		if (canisterItemId == ModContent.ItemType<LunarCanister>()) {
			return Lunar;
		}

		// Should never be hit, but compiler shouts at us without it
		return Color.White;
	}
}