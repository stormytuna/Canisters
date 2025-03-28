using Canisters.Common;
using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;
using Terraria.GameContent.ItemDropRules;

namespace Canisters.Content.Items.Weapons;

public class SlimeySlinger : CanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(12f, -10f);
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 20;
		Item.height = 32;
		Item.value = Item.buyPrice(gold: 1);
		Item.rare = ItemRarityID.Pink;

		// Use stats
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.useTime = 22;
		Item.useAnimation = 22;
		Item.autoReuse = true;
		Item.noMelee = true;
		Item.noUseGraphic = true;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<FiredVolatileCanister>();
		Item.shootSpeed = 15f;
		Item.damage = 16;
		Item.knockBack = 3f;
		Item.DamageType = DamageClass.Ranged;
		Item.useAmmo = ModContent.ItemType<VolatileCanister>();
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(2f, -2f);
	}
}

public class SlimySlingerGlobalProjectileLegacy : ShotByWeaponGlobalProjectileLegacy<SlimeySlinger>
{
	private int _numBounces = 3;

	public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity) {
		if (!ShouldApply) {
			return true;
		}

		_numBounces--;
		if (_numBounces <= 0) {
			return true;
		}

		if (projectile.velocity.X != oldVelocity.X) {
			projectile.velocity.X = oldVelocity.X * -0.99f;
		}

		if (projectile.velocity.Y != oldVelocity.Y) {
			projectile.velocity.Y = oldVelocity.Y * -0.99f;
		}

		return false;
	}

	// TODO: Some visuals as it depletes bounces?
}

public class SlimeySlingerGlobalNpc : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.QueenSlimeBoss;
	}

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		LeadingConditionRule notExpertLeadingRule = new(new Conditions.NotExpert());
		notExpertLeadingRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SlimeySlinger>(), 4));
	}
}

public class SlimeySlingerGlobalItem : GlobalItem
{
	public override bool AppliesToEntity(Item entity, bool lateInstantiation) {
		return entity.type == ItemID.QueenSlimeBossBag;
	}

	public override void ModifyItemLoot(Item item, ItemLoot itemLoot) {
		itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SlimeySlinger>(), 4));
	}
}
