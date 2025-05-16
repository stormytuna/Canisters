using Canisters.Content.Projectiles.VerdantCanister;
using Canisters.Helpers;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class VerdantCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredVerdantCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<VerdantGasEmitter>();
	}

	public override Color CanisterColor {
		get => Color.Green;
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(8, 2f, 2f);
		Item.SetShopValues(ItemRarityColor.Green2, Item.buyPrice(copper: 75));
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.JungleSpores)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
