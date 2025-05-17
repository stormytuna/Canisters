using Canisters.Content.Items.Weapons;
using Canisters.Helpers.Enums;

namespace Canisters.Common;

public class CanisterModifiersPlayer : ModPlayer
{
	public float CanisterLaunchedExplosionRadiusMult = 1f;
	public float CanisterDepletedFireRateMult = 1f;

	public override void ResetEffects() {
		CanisterLaunchedExplosionRadiusMult = 1f;
		CanisterDepletedFireRateMult = 1f;
	}

	public override float UseSpeedMultiplier(Item item) {
		if (item.ModItem is not BaseCanisterUsingWeapon { CanisterFiringType: CanisterFiringType.Depleted }) {
			return base.UseSpeedMultiplier(item);
		}

		return CanisterDepletedFireRateMult;
	}
}
