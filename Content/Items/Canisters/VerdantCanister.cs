﻿using Canisters.Content.Projectiles.VerdantCanister;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class VerdantCanister : ModItem, ICanisterItem
{
	public int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.VerdantCanister.VerdantCanister>();
	public int DepletedProjectileType => ModContent.ProjectileType<VerdantGas_Helper>();

	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 99;
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 22;
		Item.height = 22;
		Item.maxStack = 999;
		Item.value = 2;
		Item.rare = ItemRarityID.Green;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<Projectiles.VerdantCanister.VerdantCanister>();
		Item.shootSpeed = 1f;
		Item.damage = 4;
		Item.knockBack = 5f;
		Item.DamageType = DamageClass.Ranged;
		Item.ammo = ModContent.ItemType<VolatileCanister>();
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.JungleSpores)
			.AddTile(TileID.Bottles)
			.Register();
	}
}