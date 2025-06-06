﻿using Canisters.Content.Projectiles.GhastlyCanister;
using Canisters.DataStructures;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class GhastlyCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredGhastlyCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<GhastlyShot>();
	}

	public override Color CanisterColor {
		get => new(112, 253, 255, 122);
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(12, 4.5f, 1f);
		Item.SetShopValues(ItemRarityColor.Yellow8, Item.buyPrice(copper: 5));
	}

	public override void ApplyAmmoStats(ref CanisterShootStats stats) {
		if (stats.IsDepleted) {
			stats.Damage = (int)(stats.Damage / 3.5f);
			stats.Knockback /= 5f;
			stats.ProjectileCount *= 6;
			stats.TotalSpread += 0.1f;
		}
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Ectoplasm)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
