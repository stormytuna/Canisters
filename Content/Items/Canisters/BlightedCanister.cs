using Canisters.Content.Projectiles.BlightedCanister;
using Canisters.Helpers;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class BlightedCanister : BaseCanisterItem
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

	public override void SetDefaults() {
		Item.DefaultToCanister(12, 2f, 4f);
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(silver: 9));
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.CursedFlame)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
