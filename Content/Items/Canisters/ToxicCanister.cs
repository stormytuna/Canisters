using Canisters.Content.Projectiles.ToxicCanister;
using Canisters.Helpers.Abstracts;

namespace Canisters.Content.Items.Canisters;

public class ToxicCanister : CanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredToxicCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<ToxicBarb>();
	}

	public override Color CanisterColor {
		get => Color.MediumPurple;
	}

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.buyPrice(silver: 3);
		Item.rare = ItemRarityID.LightRed;

		// Weapon stats
		Item.shootSpeed = 3f;
		Item.damage = 12;
		Item.knockBack = 4f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.VialofVenom)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
