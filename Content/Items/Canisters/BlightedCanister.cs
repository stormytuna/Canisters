using Canisters.Content.Projectiles.BlightedCanister;
using Canisters.Helpers.Abstracts;

namespace Canisters.Content.Items.Canisters;

public class BlightedCanister : CanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredBlightedCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<BlightedBolt>();
	}

	public override Color CanisterColor {
		get => Color.Lime;
	}

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.buyPrice(silver: 9);
		Item.rare = ItemRarityID.LightRed;

		// Weapon stats
		Item.shootSpeed = 2f;
		Item.damage = 12;
		Item.knockBack = 4f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.CursedFlame)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
