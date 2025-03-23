using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;

namespace Canisters.Content.Items.Weapons;

public class RepurposedSprayBottle : CanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(10f, -4f);
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 14;
		Item.height = 22;
		Item.value = Item.buyPrice(gold: 25);
		Item.rare = ItemRarityID.LightRed;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = Item.useAnimation = 8;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<FiredVolatileCanister>();
		Item.shootSpeed = 5f;
		Item.damage = 36;
		Item.knockBack = 2f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<VolatileCanister>();
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(2f, 2f);
	}

	public override void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type,
		ref int damage, ref float knockback) {
		velocity = velocity.RotatedByRandom(0.22f);
	}
}

public class RepurposedSprayBottleGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.Stylist;
	}

	public override void ModifyShop(NPCShop shop) {
		shop.Add<RepurposedSprayBottle>();
	}
}
