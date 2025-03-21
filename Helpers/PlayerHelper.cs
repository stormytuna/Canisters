namespace Canisters.Helpers;

public static class PlayerHelper
{
	public static bool TryGetWeaponAmmo(this Player player, Item item, out int usedAmmoItemId) {
		return player.PickAmmo(item, out _, out _, out _, out _, out usedAmmoItemId, true);
	}
}
