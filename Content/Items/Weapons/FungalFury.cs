using Canisters.DataStructures;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class FungalFury : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(42f, -2f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-4f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(10, 10, 9f, 11, 1f);
		Item.width = 60;
		Item.height = 32;
		Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 40));
	}

	public override void ApplyWeaponStats(ref CanisterShootStats stats) {
		stats.Velocity = stats.Velocity.RotatedByRandom(0.4f);
	}
}

public class FungalFuryGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.Truffle;
	}

	public override void ModifyShop(NPCShop shop) {
		shop.InsertAfter(ItemID.MushroomSpear, ModContent.ItemType<FungalFury>());
	}
}
