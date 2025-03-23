using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;

namespace Canisters.Content.Items.Weapons;

public class WoodenSlingshot : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(12f, -10f);
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 20;
		Item.height = 32;
		Item.value = Item.buyPrice(silver: 2);
		Item.rare = ItemRarityID.Blue;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 30;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<FiredVolatileCanister>();
		Item.shootSpeed = 9f;
		Item.damage = 11;
		Item.knockBack = 1f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<VolatileCanister>();
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(2f, -2f);
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Silk)
			.AddIngredient(ItemID.Wood, 8)
			.AddTile(TileID.WorkBenches)
			.Register();
	}
}
