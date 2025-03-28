using Canisters.Common;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
using Terraria.Enums;
using Terraria.GameContent.ItemDropRules;

namespace Canisters.Content.Items.Weapons;

public class SlimeySlinger : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(12f, -10f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(2f, -2f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(22, 22, 15f, 16, 3f);
		Item.width = 20;
		Item.height = 32;
		Item.SetShopValues(ItemRarityColor.Pink5, Item.buyPrice(gold: 1));
	}
}

public class SlimySlingerGlobalProjectile : ShotByWeaponGlobalProjectile<SlimeySlinger>
{
	private int _numBounces = 3;

	public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity) {
		if (!IsActive) {
			return base.OnTileCollide(projectile, oldVelocity);
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
