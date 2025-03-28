using Canisters.DataStructures;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class Splattergun : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(26f, -2f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(0f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(8, 8, 10f, 16, 3f);
		Item.width = 30;
		Item.height = 26;
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(gold: 2, silver: 50));
	}

	public override void ApplyWeaponStats(ref CanisterShootStats stats) {
		stats.Velocity *= 1.5f;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.PainterPaintballGun)
			.AddIngredient(ItemID.OrichalcumBar, 12)
			.AddTile(TileID.MythrilAnvil)
			.Register();

		CreateRecipe()
			.AddIngredient(ItemID.PainterPaintballGun)
			.AddIngredient(ItemID.MythrilBar, 12)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
