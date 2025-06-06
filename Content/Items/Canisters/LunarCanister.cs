using Canisters.Content.Projectiles.LunarCanister;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class LunarCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredLunarCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<LunarShot>();
	}

	public override Color CanisterColor {
		get => new(73, 243, 185, 100);
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(22, 4f, 4f);
		Item.SetShopValues(ItemRarityColor.Cyan9, Item.buyPrice(silver: 9));
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.LunarBar)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
