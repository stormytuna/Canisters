using Canisters.Content.Projectiles.VerdantCanister;
using Canisters.Helpers.Abstracts;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class VerdantCanister : CanisterItem
{
	public override int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.VerdantCanister.VerdantCanister>();
	public override int DepletedProjectileType => ModContent.ProjectileType<VerdantGas_Helper>();

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = 2;
		Item.rare = ItemRarityID.Green;

		// Weapon stats
		Item.shootSpeed = 1f;
		Item.damage = 4;
		Item.knockBack = 5f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.JungleSpores)
			.AddTile(TileID.Bottles)
			.Register();
	}
}