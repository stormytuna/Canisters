using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class AncientSprayer : CanisterUsingWeapon
{
	public override FiringType FiringType => FiringType.Depleted;

	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 1;
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 60;
		Item.height = 14;
		Item.value = Item.sellPrice(silver: 40);
		Item.rare = ItemRarityID.Blue;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 10;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.channel = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 9f;
		Item.damage = 11;
		Item.knockBack = 1f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.FossilOre, 15)
			.AddRecipeGroup(RecipeGroupID.IronBar, 10)
			.AddTile(TileID.Anvils)
			.Register();
	}
}