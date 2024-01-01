using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers.Abstracts;
using Canisters.Helpers.Enums;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Weapons;

public class RepurposedSprayBottle : CanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType => CanisterFiringType.Depleted;

	public override Vector2 MuzzleOffset => new(10f, -4f);

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
		Item.shoot = ModContent.ProjectileType<VolatileCanister>();
		Item.shootSpeed = 5f;
		Item.damage = 36;
		Item.knockBack = 2f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<Canisters.VolatileCanister>();
	}

	public override Vector2? HoldoutOffset() => new Vector2(2f, 2f);

	public override void SafeModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
		velocity = velocity.RotatedByRandom(0.22f);
	}
}

public class RepurposedSprayBottleGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.Stylist;

	public override void ModifyShop(NPCShop shop) {
		shop.Add<RepurposedSprayBottle>();
	}
}
