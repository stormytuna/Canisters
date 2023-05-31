﻿using Canisters.Content.Projectiles.ToxicCanister;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class ToxicCanister : ModItem, ICanisterItem
{
	public int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.ToxicCanister.ToxicCanister>();
	public int DepletedProjectileType => ModContent.ProjectileType<ToxicBarb>();

	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 99;
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 22;
		Item.height = 22;
		Item.maxStack = 999;
		Item.value = Item.sellPrice(silver: 3);
		Item.rare = ItemRarityID.LightRed;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<Projectiles.ToxicCanister.ToxicCanister>();
		Item.shootSpeed = 2f;
		Item.damage = 6;
		Item.knockBack = 4f;
		Item.DamageType = DamageClass.Ranged;
		Item.ammo = ModContent.ItemType<VolatileCanister>();
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.VialofVenom)
			.AddTile(TileID.Bottles)
			.Register();
	}
}