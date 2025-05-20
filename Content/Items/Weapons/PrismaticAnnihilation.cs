using Canisters.Helpers;
using Canisters.Helpers.Enums;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class PrismaticAnnihilation : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}
	
	public override Vector2 MuzzleOffset {
		get => new(90f, -6f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-6f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(15, 15, 9f, 11, 1f);
		Item.width = 36;
		Item.height = 18;
		Item.SetShopValues(ItemRarityColor.StrongRed10, Item.sellPrice(gold: 10));
		// TODO: use sound
		Item.UseSound = SoundID.Item5 with { PitchRange = (0.6f, 1.2f), MaxInstances = 0, Volume = 0.6f };
	}
}
