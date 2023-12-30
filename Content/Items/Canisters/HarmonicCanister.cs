using Canisters.Content.Projectiles.HarmonicCanister;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class HarmonicCanister : CanisterItem
{
	public override int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.HarmonicCanister.HarmonicCanister>();
	public override int DepletedProjectileType => ModContent.ProjectileType<HelixBolt>();
	public override Color CanisterColor => Color.Purple;

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.sellPrice(silver: 9);
		Item.rare = ItemRarityID.LightRed;

		// Weapon stats
		Item.shootSpeed = 2.5f;
		Item.damage = 11;
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
