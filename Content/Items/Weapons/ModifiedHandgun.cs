using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;

namespace Canisters.Content.Items.Weapons;

public class ModifiedHandgun : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(30f, -4f);
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 36;
		Item.height = 18;
		Item.value = Item.buyPrice(silver: 5);
		Item.rare = ItemRarityID.Blue;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 15;
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
		return new Vector2(-4f, 0f);
	}

	// TODO: Wtf is this crafting recipe??
	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.FlintlockPistol)
			.AddIngredient(ItemID.IllegalGunParts)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
