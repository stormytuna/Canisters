﻿using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers.Abstracts;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class VolatileCanister : CanisterItem
{
	public override int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.VolatileCanister.VolatileCanister>();
	public override int DepletedProjectileType => ModContent.ProjectileType<GelBall>();

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = 2;
		Item.rare = ItemRarityID.Blue;

		// Ammo stats
		Item.shootSpeed = 2f;
		Item.damage = 3;
		Item.knockBack = 5f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Gel)
			.AddTile(TileID.Bottles)
			.Register();
	}
}