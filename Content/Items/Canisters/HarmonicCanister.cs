using System;
using Canisters.Content.Projectiles.HarmonicCanister;
using Canisters.Helpers.Abstracts;

namespace Canisters.Content.Items.Canisters;

public class HarmonicCanister : CanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredHarmonicCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<HelixBolt>();
	}

	public override Color CanisterColor {
		get => Color.Purple;
	}

	public override void SafeSetDefaults() {
		Item.value = Item.buyPrice(silver: 9);
		Item.rare = ItemRarityID.LightRed;

		Item.shootSpeed = 2.5f;
		Item.damage = 11;
		Item.knockBack = 4f;
	}

	public override void ApplyAmmoStats(bool isLaunched, ref Vector2 velocity, ref Vector2 position, ref int damage, ref float knockBack, ref int amount, ref float spread, ref Func<int, float[]> getAiCallback) {
		if (!isLaunched) {
			amount = 2;
			getAiCallback = (amount) => [0f, amount == 0 ? 0f : 1f, 0f];
		}
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.SoulofNight)
			.AddIngredient(ItemID.SoulofLight)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
