using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class LushSlinger : CanisterUsingWeapon
{
	public override FiringType FiringType => FiringType.Launched;

	public override void SetDefaults() {
		// Base stats
		Item.width = 20;
		Item.height = 32;
		Item.value = Item.sellPrice(silver: 60);
		Item.rare = ItemRarityID.Orange;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 20;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.channel = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 10f;
		Item.damage = 31;
		Item.knockBack = 2f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<WoodenSlingshot>()
			.AddIngredient(ItemID.Vine, 2)
			.AddIngredient(ItemID.JungleSpores, 10)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}