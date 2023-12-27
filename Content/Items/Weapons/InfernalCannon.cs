using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class InfernalCannon : CanisterUsingWeapon
{
	public override FiringType FiringType => FiringType.Launched;

	public override Vector2 MuzzleOffset => new(36f, 0f);

	public override void SetDefaults() {
		// Base stats
		Item.width = 54;
		Item.height = 16;
		Item.value = Item.buyPrice(silver: 50);
		Item.rare = ItemRarityID.Orange;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 36;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 13f;
		Item.damage = 42;
		Item.knockBack = 8f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
	}

	public override Vector2? HoldoutOffset() => new Vector2(-8f, 0f);

	public override void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		velocity = velocity.RotatedByRandom(0.11f);
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.HellstoneBar)
			.AddTile(TileID.Hellforge)
			.Register();
	}
}

public class InfernalCannonGlobalProjectile : ShotByWeaponGlobalProjectile<InfernalCannon>
{
	public override void AI(Projectile projectile) {
		if (!ShouldApply || projectile.hide || Main.rand.NextBool(4, 5)) {
			return;
		}

		Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.ShadowbeamStaff);
		dust.scale = Main.rand.NextFloat(1.5f, 2f);
		dust.noGravity = true;
		dust.noLight = true;
	}

	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!ShouldApply || Main.rand.NextBool(2, 3)) {
			return;
		}

		target.AddBuff(BuffID.ShadowFlame, 180);
	}
}