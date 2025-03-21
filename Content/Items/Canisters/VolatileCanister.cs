using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers.Abstracts;

namespace Canisters.Content.Items.Canisters;

public class VolatileCanister : CanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredVolatileCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<DepletedVolatileCanister>();
	}

	public override Color CanisterColor {
		get => new(45, 144, 255, 255);
	}

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.buyPrice(copper: 75);
		Item.rare = ItemRarityID.Blue;

		// Ammo stats
		Item.shootSpeed = 2f;
		Item.damage = 6;
		Item.knockBack = 3f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Gel)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
