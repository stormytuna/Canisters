using Canisters.DataStructures;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class RepurposedSprayBottle : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(10f, -4f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(2f, 2f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(8, 8, 5f, 36, 2f);
		Item.width = 14;
		Item.height = 22;
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(gold: 25));
	}

	public override void ApplyWeaponStats(ref CanisterShootStats stats) {
		stats.TotalSpread += 0.25f;
	}
}

public class RepurposedSprayBottleGlobalNpc : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.Stylist;
	}

	public override void ModifyShop(NPCShop shop) {
		shop.Add<RepurposedSprayBottle>();
	}
}
