using Canisters.DataStructures;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class TheColony : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(18f, -4f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-10f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(25, 25, 12f, 36, 3f);
		Item.width = 32;
		Item.height = 24;
		Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(silver: 70));
	}

	public override void ApplyWeaponStats(ref CanisterShootStats stats) {
		stats.ProjectileCount *= 4;
		stats.Damage /= 4;
		stats.TotalSpread += 0.35f;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.BeeWax, 14)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
