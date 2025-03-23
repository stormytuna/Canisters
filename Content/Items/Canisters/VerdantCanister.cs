using Canisters.Content.Projectiles.VerdantCanister;
using Canisters.Helpers._Legacy.Abstracts;

namespace Canisters.Content.Items.Canisters;

public class VerdantCanister : CanisterItem
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

	public override void SafeSetDefaults() {
		Item.value = Item.buyPrice(copper: 75);
		Item.rare = ItemRarityID.Green;

		Item.shootSpeed = 2f;
		Item.damage = 8;
		Item.knockBack = 2f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.JungleSpores)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
