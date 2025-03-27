﻿using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class WoodenSlingshot : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(12f, -10f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(2f, -2f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(30, 30, 11, 9, 1f);
		Item.width = 20;
		Item.height = 32;
		Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 2));
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Silk)
			.AddIngredient(ItemID.Wood, 8)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
