using Canisters.Content.Projectiles.ToxicCanister;
using Canisters.Helpers.Abstracts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class ToxicCanister : CanisterItem
{
	public override int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.ToxicCanister.ToxicCanister>();
	public override int DepletedProjectileType => ModContent.ProjectileType<ToxicBarb>();

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.sellPrice(silver: 3);
		Item.rare = ItemRarityID.LightRed;

		// Weapon stats
		Item.shootSpeed = 3f;
		Item.damage = 12;
		Item.knockBack = 4f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.VialofVenom)
			.AddTile(TileID.Bottles)
			.Register();
	}
}