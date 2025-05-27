using Canisters.DataStructures;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class TheColony : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(18f, -10f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-6f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(25, 25, 8f, 18, 3f);
		Item.width = 32;
		Item.height = 24;
		Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(silver: 70));
		Item.UseSound = SoundID.Item97 with { Volume = 0.7f, PitchRange = (-0.4f, 0f) };
	}

	public override void ApplyWeaponStats(ref CanisterShootStats stats) {
		stats.ProjectileCount *= 4;
		stats.TotalSpread += 0.35f;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.BeeWax, 14)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
