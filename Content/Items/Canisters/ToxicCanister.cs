﻿using Canisters.Common;
using Canisters.Content.Projectiles.ToxicCanister;
using Canisters.Helpers;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class ToxicCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredToxicCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<ToxicBarb>();
	}

	public override Color CanisterColor {
		get => Color.MediumPurple;
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(15, 2f, 2f);
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(silver: 3));
	}

	public override void AddRecipes() {
		int amount = ServerConfig.Instance.LowGrind ? 100 : 50;
		CreateRecipe(amount)
			.AddIngredient<EmptyCanister>(amount)
			.AddIngredient(ItemID.VialofVenom)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
