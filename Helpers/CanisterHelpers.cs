using Canisters.Content.Items.Canisters;
using Canisters.Helpers._Legacy.Abstracts;

namespace Canisters.Helpers;

public static class CanisterHelpers
{
	public static Color GetCanisterColor<TCanister>() where TCanister : CanisterItem {
		return GetCanisterColorLegacy(ModContent.ItemType<TCanister>());
	}

	public static Color GetCanisterColorLegacy(int canisterItemId) {
		ModItem modItem = ModContent.GetModItem(canisterItemId);
		if (modItem is CanisterItem canisterItem) {
			return canisterItem.CanisterColor;
		}

		return Color.White;
	}

	public static Color GetCanisterColor(int canisterItemId) {
		ModItem modItem = ModContent.GetModItem(canisterItemId);
		if (modItem is BaseCanisterItem canisterItem) {
			return canisterItem.CanisterColor;
		}
		
		return Color.White;
	}

	public static Color GetCanisterColorForHeldItem(Player player) {
		if (player.HeldItem.ModItem is CanisterUsingWeapon &&
		    player.TryGetWeaponAmmo(player.HeldItem, out int canisterItemId)) {
			return GetCanisterColorLegacy(canisterItemId);
		}

		return Color.White;
	}

	public static string GetEmptyAssetString() {
		return $"{nameof(Canisters)}/Assets/Empty";
	}
}
