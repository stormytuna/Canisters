using Canisters.Content.Projectiles.NaniteCanister;
using Canisters.DataStructures;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class NaniteCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredNaniteCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<NaniteLaserLine>();
	}

	public override Color CanisterColor {
		get => Color.LightCyan;
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(14, 3f, 1f);
		Item.SetShopValues(ItemRarityColor.Yellow8, Item.buyPrice(silver: 3));
	}

	public override void ApplyAmmoStats(ref CanisterShootStats stats) {
		if (stats.IsDepleted) {
			stats.Velocity *= 0.5f;
		}
	}

	public override void AddRecipes() {
		CreateRecipe(50)
			.AddIngredient<EmptyCanister>(50)
			.AddIngredient(ItemID.Nanites)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
