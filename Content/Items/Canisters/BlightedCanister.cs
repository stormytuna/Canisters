using Canisters.Content.Projectiles.BlightedCanister;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class BlightedCanister : CanisterItem
{
	public override int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.BlightedCanister.BlightedCanister>();
	public override int DepletedProjectileType => ModContent.ProjectileType<BlightedBolt>();
	public override Color CanisterColor => Color.Lime;

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.sellPrice(silver: 9);
		Item.rare = ItemRarityID.LightRed;

		// Weapon stats
		Item.shootSpeed = 2f;
		Item.damage = 12;
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