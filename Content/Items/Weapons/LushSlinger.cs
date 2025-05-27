using Canisters.DataStructures;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class LushSlinger : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(12f, -10f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(2f, -2f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(35, 35, 9f, 31, 4f);
		Item.width = 20;
		Item.height = 32;
		Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(silver: 85));
		Item.UseSound = SoundID.Item5 with { Pitch = 0.5f, PitchVariance = 0.08f };
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.RichMahogany, 8)
			.AddIngredient(ItemID.Vine, 2)
			.AddIngredient(ItemID.JungleSpores, 10)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
