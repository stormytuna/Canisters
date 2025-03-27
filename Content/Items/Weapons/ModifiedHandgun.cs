using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.Helpers;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class ModifiedHandgun : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(30f, -4f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-4f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(15, 15, 9f, 11, 1f);
		Item.width = 36;
		Item.height = 18;
		Item.SetShopValues(ItemRarityColor.Blue1, Item.sellPrice(gold: 5));
	}
}

public class ModifiedHangunGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.Merchant;
	}

	public override void ModifyShop(NPCShop shop) {
		shop.InsertAfter(ItemID.Sickle, ModContent.ItemType<ModifiedHandgun>());	
	}
}
