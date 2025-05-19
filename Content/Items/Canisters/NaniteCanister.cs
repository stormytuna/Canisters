using Canisters.Common;
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
		Item.DefaultToCanister(14, 3f, 1f);
		Item.SetShopValues(ItemRarityColor.Yellow8, Item.buyPrice(silver: 3));
	}

	public override void AddRecipes() {
		int amount = ServerConfig.Instance.LowGrind ? 100 : 50;
		CreateRecipe(amount)
			.AddIngredient<EmptyCanister>(amount)
			.AddIngredient(ItemID.Nanites)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
