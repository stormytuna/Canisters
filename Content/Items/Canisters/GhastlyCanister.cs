using System;
using Canisters.Content.Projectiles.GhastlyCanister;
using Canisters.Helpers._Legacy.Abstracts;

namespace Canisters.Content.Items.Canisters;

public class GhastlyCanister : CanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredGhastlyCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<GhastlyShot>();
	}

	public override Color CanisterColor {
		get => Color.Cyan;
	}

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.buyPrice(copper: 5);
		Item.rare = ItemRarityID.Yellow;

		// Weapon stats
		Item.shootSpeed = 6f;
		Item.damage = 16;
		Item.knockBack = 3f;
	}

	public override void ApplyAmmoStats(bool isLaunched, ref Vector2 velocity, ref Vector2 position, ref int damage, ref float knockBack, ref int amount, ref float spread, ref Func<int, float[]> getAiCallback) {
		if (isLaunched) {
			return;
		}

		damage /= 6;
		amount *= 6;
		spread += 0.25f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Ectoplasm)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
