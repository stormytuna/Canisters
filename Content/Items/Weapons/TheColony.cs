using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class TheColony : CanisterUsingWeapon
{
	public override FiringType FiringType => FiringType.Depleted;

	public override Vector2 MuzzleOffset => new(18f, -4f);

	public override void SetDefaults() {
		// Base stats
		Item.width = 32;
		Item.height = 24;
		Item.value = Item.buyPrice(silver: 70);
		Item.rare = ItemRarityID.Orange;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 25;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 12f;
		Item.damage = 36;
		Item.knockBack = 3f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
	}

	public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player) => base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);

	public override Vector2? HoldoutOffset() => new Vector2(-10f, 0f);

	public override void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		velocity = velocity.RotatedByRandom(0.22f);
	}

	public override void ApplyShootStats(ref Vector2 velocity, ref Vector2 position, ref int damage, ref float knockBack, ref int amount, ref float spread) {
		amount *= 4;
		damage /= 4;
		spread += 0.35f;
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.BeeWax, 14)
			.AddTile(TileID.Anvils)
			.Register();
	}
}
