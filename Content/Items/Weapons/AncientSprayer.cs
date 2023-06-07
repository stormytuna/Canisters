using Canisters.Content.Items.Canisters;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class AncientSprayer : CanisterUsingWeapon
{
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
		Item.shoot = ModContent.ProjectileType<AncientSprayer_HeldProjectile>();
		Item.shootSpeed = 9f;
		Item.damage = 11;
		Item.knockBack = 1f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<VolatileCanister>();
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.FlintlockPistol)
			.AddIngredient(ItemID.IllegalGunParts)
			.AddTile(TileID.Anvils)
			.Register();
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<AncientSprayer_HeldProjectile>(), damage, knockback, player.whoAmI);

		return false;
	}
}

public class AncientSprayer_HeldProjectile : CanisterUsingHeldProjectile
{
	public override void SetDefaults() {
		// Base stats
		Projectile.width = 60;
		Projectile.height = 14;
		Projectile.aiStyle = -1;

		// Weapon stats
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Ranged;

		// Held projectile stats
		Projectile.tileCollide = false;
		Projectile.hide = true;
		Projectile.ignoreWater = true;

		// CanisterUsingHeldProjectile stats
		HoldOutOffset = 32f;
		CanisterFiringType = FiringType.Depleted;
		RotationOffset = 0f;
		MuzzleOffset = new Vector2(32f, -6f);
		TotalRandomSpread = 0.35f;
	}

	public override string Texture => "Canisters/Content/Items/Weapons/AncientSprayer";
}