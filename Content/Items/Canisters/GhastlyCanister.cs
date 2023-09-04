using Canisters.Content.Projectiles.GhastlyCanister;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class GhastlyCanister : CanisterItem
{
	public override int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.GhastlyCanister.GhastlyCanister>();
	public override int DepletedProjectileType => ModContent.ProjectileType<GhastlyShot>();

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.sellPrice(copper: 5);
		Item.rare = ItemRarityID.Yellow;

		// Weapon stats
		Item.shootSpeed = 6f;
		Item.damage = 16;
		Item.knockBack = 3f;
	}

	public override void ApplyAmmoStats(bool isLaunched, ref Vector2 velocity, ref Vector2 position, ref int damage, ref float knockBack, ref int amount, ref float spread) {
		if (isLaunched) {
			return;
		}

		damage /= 6;
		amount += 5;
		spread = 0.25f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Ectoplasm)
			.AddTile(TileID.Bottles)
			.Register();
	}
}