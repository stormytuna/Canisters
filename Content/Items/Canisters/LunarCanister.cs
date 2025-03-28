﻿using Canisters.Content.Projectiles.LunarCanister;
using Canisters.Helpers._Legacy.Abstracts;

namespace Canisters.Content.Items.Canisters;

public class LunarCanister : CanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredLunarCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<LunarShot>();
	}

	public override Color CanisterColor {
		get => new(208, 253, 235);
	}

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.buyPrice(silver: 9);
		Item.rare = ItemRarityID.Cyan;

		// Weapon stats
		Item.shootSpeed = 4f;
		Item.damage = 22;
		Item.knockBack = 4f;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.LunarBar)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
