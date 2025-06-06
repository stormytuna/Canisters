﻿using Canisters.DataStructures;
using Terraria.Audio;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class FungalFury : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(42f, -2f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-4f, 0f);
	}

	public override void SetStaticDefaults() {
		ItemID.Sets.SkipsInitialUseSound[Type] = true;
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(10, 10 * 4, 12f, 60, 1f);
		Item.width = 60;
		Item.height = 32;
		Item.SetShopValues(ItemRarityColor.Lime7, Item.sellPrice(gold: 70));
		Item.UseSound = SoundID.Item5 with { PitchRange = (0.7f, 1.1f), MaxInstances = 0, Volume = 0.6f };

		Item.reuseDelay = 12;
		Item.consumeAmmoOnLastShotOnly = true;
	}

	public override bool CanConsumeAmmo(Item ammo, Player player) {
		return Main.rand.NextBool(20, 100);
	}

	public override bool? UseItem(Player player) {
		SoundEngine.PlaySound(Item.UseSound, player.Center);

		return true;
	}

	public override void ApplyWeaponStats(ref CanisterShootStats stats) {
		stats.TotalSpread += 0.08f;
	}
}

public class FungalFuryGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.Truffle;
	}

	public override void ModifyShop(NPCShop shop) {
		shop.InsertAfter(ItemID.MushroomSpear, ModContent.ItemType<FungalFury>(), Condition.DownedPlantera);
	}
}
