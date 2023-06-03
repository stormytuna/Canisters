using Canisters.Content.Projectiles.HarmonicCanister;
using Canisters.Helpers.Abstracts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class HarmonicCanister : CanisterItem
{
	public int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.HarmonicCanister.HarmonicCanister>();
	public int DepletedProjectileType => ModContent.ProjectileType<HelixBolt>();

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.sellPrice(silver: 9);
		Item.rare = ItemRarityID.LightRed;

		// Weapon stats
		Item.shootSpeed = 2f;
		Item.damage = 6;
		Item.knockBack = 4f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.SoulofNight)
			.AddIngredient(ItemID.SoulofLight)
			.AddTile(TileID.Bottles)
			.Register();
	}
}