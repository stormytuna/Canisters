using Canisters.Content.Items.Canisters;
using Canisters.Content.Items.Weapons;

namespace Canisters.Helpers;

public static class CanisterHelpers
{
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

	public static Color GetCanisterColor<TCanister>() where TCanister : BaseCanisterItem {
		return GetCanisterColor(ModContent.ItemType<TCanister>());
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
