using Canisters.Content.Projectiles.GlisteningCanister;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class GlisteningCanister : CanisterItem
{
	public override int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.GlisteningCanister.GlisteningCanister>();
	public override int DepletedProjectileType => ModContent.ProjectileType<GlisteningBall>();
	public override Color CanisterColor => Color.Yellow;

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.buyPrice(silver: 9);
		Item.rare = ItemRarityID.LightRed;

		// Weapon stats
		Item.shootSpeed = 2f;
		Item.damage = 12;
		Item.knockBack = 4f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Ichor)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
