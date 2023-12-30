using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class VolatileCanister : CanisterItem
{
	public override int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.VolatileCanister.VolatileCanister>();
	public override int DepletedProjectileType => ModContent.ProjectileType<GelBall>();
	public override Color CanisterColor => new(45, 144, 255, 255);

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = 2;
		Item.rare = ItemRarityID.Blue;

		// Ammo stats
		Item.shootSpeed = 2f;
		Item.damage = 6;
		Item.knockBack = 3f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Gel)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
