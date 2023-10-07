using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class ModifiedHandgun : CanisterUsingWeapon
{
	public override FiringType FiringType => FiringType.Depleted;

	public override Vector2 MuzzleOffset => new(30f, -4f);

	public override void SetDefaults() {
		// Base stats
		Item.width = 36;
		Item.height = 18;
		Item.value = Item.sellPrice(gold: 2);
		Item.rare = ItemRarityID.Blue;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 15;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 9f;
		Item.damage = 11;
		Item.knockBack = 1f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
	}

	public override Vector2? HoldoutOffset() => new Vector2(-4f, 0f);

	public override void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		velocity = velocity.RotatedByRandom(0.22f);
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.FlintlockPistol)
			.AddIngredient(ItemID.IllegalGunParts)
			.AddTile(TileID.Anvils)
			.Register();
	}
}