using Canisters.Content.Projectiles.NaniteCanister;
using Canisters.Helpers;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class NaniteCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredNaniteCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<NaniteMistEmitter>();
	}

	public override Color CanisterColor {
		get => Color.LightCyan;
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(16, 4f, 2f);
		Item.SetShopValues(ItemRarityColor.Yellow8, Item.buyPrice(silver: 3));
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Nanites, 5)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
