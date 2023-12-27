using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Items.Weapons;

public class Resonance : CanisterUsingWeapon
{
	public override FiringType FiringType => FiringType.Depleted;

	public override Vector2 MuzzleOffset => new(52f, -2f);

	public override void SetDefaults() {
		// Base stats
		Item.width = 60;
		Item.height = 20;
		Item.value = Item.buyPrice(gold: 5);
		Item.rare = ItemRarityID.LightRed;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 8;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 12f;
		Item.damage = 16;
		Item.knockBack = 3f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.LightShard)
			.AddIngredient(ItemID.DarkShard)
			.AddIngredient(ItemID.MythrilBar, 16)
			.AddTile(TileID.MythrilAnvil)
			.Register();

		CreateRecipe()
			.AddIngredient(ItemID.LightShard)
			.AddIngredient(ItemID.DarkShard)
			.AddIngredient(ItemID.PalladiumBar, 16)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}

	public override Vector2? HoldoutOffset() => new Vector2(-4f, 4f);

	public override void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		velocity = velocity.RotatedByRandom(0.08f);
	}

	public override void ShootProjectile(EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int owner) {
		Vector2 normal = velocity.SafeNormalize(Vector2.Zero).RotatedBy(PiOver2) * Main.LocalPlayer.direction;
		Projectile.NewProjectile(source, position + normal * 3f, velocity, type, damage, knockback, owner);
		Projectile.NewProjectile(source, position - normal * 3f, velocity, type, damage, knockback, owner);
	}
}