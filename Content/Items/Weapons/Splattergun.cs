using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class Splattergun : CanisterUsingWeapon
{
	public override FiringType FiringType => FiringType.Depleted;

	public override Vector2 MuzzleOffset => new(26f, -2f);

	public override void SetDefaults() {
		// Base stats
		Item.width = 30;
		Item.height = 26;
		Item.value = Item.buyPrice(gold: 2, silver: 50);
		Item.rare = ItemRarityID.LightRed;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 8;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 10f;
		Item.damage = 16;
		Item.knockBack = 3f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.PainterPaintballGun)
			.AddIngredient(ItemID.OrichalcumBar, 12)
			.AddTile(TileID.MythrilAnvil)
			.Register();

		CreateRecipe()
			.AddIngredient(ItemID.PainterPaintballGun)
			.AddIngredient(ItemID.MythrilBar, 12)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}

	public override Vector2? HoldoutOffset() => new Vector2(0f, 0f);

	public override void ApplyShootStats(ref Vector2 velocity, ref Vector2 position, ref int damage, ref float knockBack, ref int amount, ref float spread) {
		velocity *= 1.5f;
	}
}
