using Canisters.Content.Projectiles.VerdantCanister;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class VerdantCanister : CanisterItem
{
	public override int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.VerdantCanister.VerdantCanister>();
	public override int DepletedProjectileType => ModContent.ProjectileType<VerdantGasEmitter>();
	public override Color CanisterColor => Color.Green;

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = 2;
		Item.rare = ItemRarityID.Green;

		// Weapon stats
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