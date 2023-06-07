using Canisters.Content.Items.Canisters;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class WoodenSlingshot : CanisterUsingWeapon
{
	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 1;
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 20;
		Item.height = 32;
		Item.value = Item.sellPrice(silver: 2);
		Item.rare = ItemRarityID.Blue;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 30;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;
		Item.channel = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<WoodenSlingshot_HeldProjectile>();
		Item.shootSpeed = 9f;
		Item.damage = 11;
		Item.knockBack = 1f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<VolatileCanister>();
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.Silk)
			.AddIngredient(ItemID.Wood, 8)
			.AddTile(TileID.WorkBenches)
			.Register();

		base.AddRecipes();
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<WoodenSlingshot_HeldProjectile>(), damage, knockback, player.whoAmI);

		return false;
	}
}

public class WoodenSlingshot_HeldProjectile : CanisterUsingHeldProjectile
{
	public override void SetDefaults() {
		// Base stats
		Projectile.width = 20;
		Projectile.height = 32;
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

		// CanisterHeldProjectile stats
		HoldOutOffset = 10f;
		CanisterFiringType = FiringType.Launched;
		RotationOffset = 0f;
		MuzzleOffset = new Vector2(0, -10f);
		ShootSound = SoundID.Item5;
		TotalRandomSpread = 0.2f;
	}

	public override string Texture => "Canisters/Content/Items/Weapons/WoodenSlingshot";
}