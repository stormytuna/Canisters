using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;
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
		Item.DefaultToCanisterUsingWeapon(20, 20, 10f, 31, 2f);
		Item.width = 20;
		Item.height = 32;
		Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(silver: 85));
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<WoodenSlingshot>()
			.AddIngredient(ItemID.Vine, 2)
			.AddIngredient(ItemID.JungleSpores, 10)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
