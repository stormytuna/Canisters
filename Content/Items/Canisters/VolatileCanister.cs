using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers.Interfaces;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class VolatileCanister : ModItem, ICanisterItem
{
	public int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.VolatileCanister.VolatileCanister>();
	public int DepletedProjectileType => ModContent.ProjectileType<GelBall>();

	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 99;
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 22;
		Item.height = 22;
		Item.maxStack = 999;
		Item.value = 2;
		Item.rare = ItemRarityID.Blue;

		// Ammo stats
		Item.shoot = ModContent.ProjectileType<Projectiles.VolatileCanister.VolatileCanister>();
		Item.shootSpeed = 2f;
		Item.damage = 3;
		Item.knockBack = 5f;
		Item.DamageType = DamageClass.Ranged;
		Item.ammo = Type;
		Item.consumable = true;
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.Gel)
			.AddTile(TileID.Bottles)
			.Register();
	}
}