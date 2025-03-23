using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers._Legacy.Abstracts;
using Terraria.Enums;
using Terraria.GameContent.Creative;

namespace Canisters.Content.Items.Canisters;

public class VolatileCanister : BaseCanisterItem
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

	public override void SetDefaults() {
		Item.DefaultToCanister();
		Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(copper: 75));
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Gel)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
