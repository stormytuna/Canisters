﻿using Canisters.DataStructures;
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
		Item.UseSound = SoundID.Item5 with { PitchRange = (0.6f, 1.2f), MaxInstances = 0, Volume = 0.6f };
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
