using Canisters.Content.Items.Canisters;
using Canisters.Content.Items.Weapons;
using Canisters.Helpers._Legacy.Abstracts;

namespace Canisters.Helpers;

public static class CanisterHelpers
{
	public static Color GetCanisterColorLegacy<TCanister>() where TCanister : CanisterItem {
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

	public static Color GetCanisterColor(Item item) {
		return GetCanisterColor(item.type);
	}

	public static Color GetCanisterColorForHeldWeapon(Player player) {
		if (player.HeldItem.ModItem is BaseCanisterUsingWeapon && player.TryGetWeaponAmmo(player.HeldItem, out int canisterItemId)) {
			return GetCanisterColor(canisterItemId);
		}

		return Color.White;
	}

	public static string GetEmptyAssetString() {
		return $"{nameof(Canisters)}/Assets/Empty";
	}
}
