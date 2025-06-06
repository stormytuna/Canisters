using Canisters.DataStructures;
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
		Item.DefaultToCanisterUsingWeapon(22, 22, 10f, 50, 6f);
		Item.width = 48;
		Item.height = 16;
		Item.SetShopValues(ItemRarityColor.Pink5, Item.buyPrice(gold: 4));
		Item.UseSound = SoundID.Item10 with { PitchRange = (-0.6f, -0.4f) };
	}

	public override bool AltFunctionUse(Player player) {
		return true;
	}

	public override bool CanUseItem(Player player) {
		if (player.altFunctionUse == 2) {
			Item.UseSound = SoundID.Item10 with { PitchRange = (0.6f, 0.9f) };
		}
		else {
			Item.UseSound = SoundID.Item10 with { PitchRange = (-0.6f, -0.4f) };
		}

		return base.CanUseItem(player);
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<GraniteCannon>()
			.AddIngredient(ItemID.HallowedBar, 10)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
