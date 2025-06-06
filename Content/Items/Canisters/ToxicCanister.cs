using Canisters.Content.Projectiles.ToxicCanister;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class ToxicCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredToxicCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<ToxicBarb>();
	}

	public override Color CanisterColor {
		get => new(87, 17, 172, 200);
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(15, 2f, 2f);
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(silver: 3));
	}

	public override void AddRecipes() {
		CreateRecipe(50)
			.AddIngredient<EmptyCanister>(50)
			.AddIngredient(ItemID.VialofVenom)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
