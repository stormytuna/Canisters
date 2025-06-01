using Canisters.Content.Projectiles.HarmonicCanister;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class HarmonicCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredHarmonicCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<HelixBoltEmitter>();
	}

	public override Color CanisterColor {
		get => Color.Purple;
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(10, 3.5f, 4f);
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(silver: 9));
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.SoulofNight)
			.AddIngredient(ItemID.SoulofLight)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
