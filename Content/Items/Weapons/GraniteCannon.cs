﻿using System.Linq;
using Canisters.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ItemDropRules;

namespace Canisters.Content.Items.Weapons;

public class GraniteCannon : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Launched;
	}

	public override Vector2 MuzzleOffset {
		get => new(36f, 0f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-2f, 0f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(42, 42, 13f, 24, 3f);
		Item.width = 44;
		Item.height = 16;
		Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 30));
		Item.UseSound = SoundID.Item10 with { PitchRange = (-0.6f, -0.4f) };
	}
}

public class GraniteCannonGlobalNpc : GlobalNPC
{
	private static readonly int[] _graniteEnemies = [NPCID.GraniteFlyer, NPCID.GraniteGolem];

	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return _graniteEnemies.Contains(entity.type);
	}

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GraniteCannon>(), 20));
	}
}
