using Canisters.DataStructures;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class AbyssalBlaster : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(40f, 0f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-4f, 2f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(50, 50, 10f, 36, 4f);
		Item.width = 56;
		Item.height = 18;
		Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 7));
		// TODO: revisit sound
		Item.UseSound = SoundID.Item10 with { PitchRange = (-1f, -0.8f) };
	}

	public override void ApplyWeaponStats(ref CanisterShootStats stats) {
		stats.ProjectileCount *= 3;
		stats.TotalSpread += 0.2f;
	}

	// TODO: acquisition
}
