using Canisters.Content.Projectiles.NaniteCanister;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class NaniteCanister : CanisterItem
{
	public override int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.NaniteCanister.NaniteCanister>();
	public override int DepletedProjectileType => ModContent.ProjectileType<NaniteMistEmitter>();
	public override Color CanisterColor => Color.LightCyan;

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.sellPrice(silver: 3);
		Item.rare = ItemRarityID.Yellow;

		// Weapon stats
		Item.shootSpeed = 4f;
		Item.damage = 16;
		Item.knockBack = 2f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Nanites, 5)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
