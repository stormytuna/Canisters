using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;
using Terraria.DataStructures;

namespace Canisters.Content.Items.Weapons;

public class Pneumaticannon : CanisterUsingWeapon
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
		Item.height = 14;
		Item.value = Item.sellPrice(gold: 75);
		Item.rare = ItemRarityID.Pink;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 32;
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

	public override Vector2? HoldoutOffset() {
		return new Vector2(-6f, 0f);
	}

	public override void ShootProjectile(EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity,
		int type, int damage, float knockback, int owner, float ai0, float ai1, float ai2) {
		var canister = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, owner);
		canister.extraUpdates = 2; // TODO: Wont work in MP!!
	}
}

public class PneumaticannonGlobalNpc : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.Steampunker;
	}

	public override void ModifyShop(NPCShop shop) {
		shop.Add<Pneumaticannon>();
	}
}
