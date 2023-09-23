using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Canisters.Helpers;

public static class CanisterHelpers
{
	public static Color GetCanisterColor<TCanister>() where TCanister : CanisterItem => GetCanisterColor(ModContent.ItemType<TCanister>());

	public static Color GetCanisterColor(int canisterItemId) {
		ModItem modItem = ModContent.GetModItem(canisterItemId);
		if (modItem is CanisterItem canisterItem) {
			return canisterItem.CanisterColor;
		}

		return Color.White;
	}
}