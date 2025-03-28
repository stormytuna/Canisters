using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class ChlorophyteSlugger : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(38f, -2f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-2f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(8, 8, 11f, 21, 3f);
		Item.width = 50;
		Item.height = 24;
		Item.SetShopValues(ItemRarityColor.Lime7, Item.buyPrice(gold: 5, silver: 50));
	}
}
