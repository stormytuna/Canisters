using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Items.Weapons;

public class ModifiedHandgun : CanisterUsingWeapon
{
	public override FiringType FiringType => FiringType.Depleted;

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
		Item.channel = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 9f;
		Item.damage = 11;
		Item.knockBack = 1f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
	}

	public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		Vector2 muzzleOffset = velocity.SafeNormalize(Vector2.Zero) * 44f;
		muzzleOffset += velocity.SafeNormalize(Vector2.Zero).RotatedBy(PiOver2) * player.direction * -4f;
		if (CollisionHelpers.CanHit(position, position + muzzleOffset)) {
			position += muzzleOffset;
		}
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.FlintlockPistol)
			.AddIngredient(ItemID.IllegalGunParts)
			.AddTile(TileID.Anvils)
			.Register();
	}
}