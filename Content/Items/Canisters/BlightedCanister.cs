using Canisters.Content.Projectiles.BlightedCanister;
using Canisters.Helpers.Abstracts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class BlightedCanister : CanisterItem
{
	public int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.BlightedCanister.BlightedCanister>();
	public int DepletedProjectileType => ModContent.ProjectileType<BlightedBolt>();

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.sellPrice(silver: 9);
		Item.rare = ItemRarityID.LightRed;

		// Weapon stats
		Item.shootSpeed = 1f;
		Item.damage = 6;
		Item.knockBack = 4f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.CursedFlame)
			.AddTile(TileID.Bottles)
			.Register();
	}
}