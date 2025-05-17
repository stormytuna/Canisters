using Canisters.Helpers;
using Canisters.Helpers.Enums;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class Reverence : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => Main.LocalPlayer.altFunctionUse == 2
			? CanisterFiringType.Depleted
			: CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(22f, -2f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-4f, 0f);
	}

	public override void SetStaticDefaults() {
		ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(22, 22, 10f, 16, 3f);
		Item.width = 48;
		Item.height = 16;
		Item.SetShopValues(ItemRarityColor.Pink5, Item.buyPrice(gold: 4));
	}

	public override bool AltFunctionUse(Player player) {
		return true;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<GraniteCannon>()
			.AddIngredient(ItemID.HallowedBar, 10)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
