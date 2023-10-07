using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

// TODO: Change sprite, it's ugly :(
public class InfernalCannon : CanisterUsingWeapon
{
	public override FiringType FiringType => FiringType.Launched;

	public override Vector2 MuzzleOffset => new(36f, 0f);

	public override void SetDefaults() {
		// Base stats
		Item.width = 44;
		Item.height = 16;
		Item.value = Item.sellPrice(silver: 60);
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

	public override Vector2? HoldoutOffset() => new Vector2(-2f, 0f);

	public override void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		velocity = velocity.RotatedByRandom(0.11f);
	}

	// TODO: Recipe lol
}

public class InfernalCannonGlobalProjectile : ShotByWeaponGlobalProjectile<InfernalCannon>
{
	// TODO: Dust while travelling	

	public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
		if (!ShouldApply || Main.rand.NextBool(2, 3)) {
			return;
		}

		target.AddBuff(BuffID.ShadowFlame, 180);
	}
}