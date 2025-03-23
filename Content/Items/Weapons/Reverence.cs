using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;

namespace Canisters.Content.Items.Weapons;

// TODO: Sprite looks awful
public class Reverence : CanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(22f, -2f);
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 48;
		Item.height = 16;
		Item.value = Item.buyPrice(gold: 4);
		Item.rare = ItemRarityID.Pink;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 22;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<FiredVolatileCanister>();
		Item.shootSpeed = 10f;
		Item.damage = 16;
		Item.knockBack = 3f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<VolatileCanister>();
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient<GraniteCannon>()
			.AddIngredient(ItemID.HallowedBar, 10)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-6f, 0f);
	}
}
