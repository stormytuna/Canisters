using System.Linq;
using Canisters.Common;
using Terraria.Enums;
using Terraria.GameContent.ItemDropRules;

namespace Canisters.Content.Items.Accessories;

public class PneumaticPump : ModItem
{
	public override void SetDefaults() {
		Item.width = 34;
		Item.height = 34;
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(gold: 2));
		Item.accessory = true;
	}

	public override void UpdateAccessory(Player player, bool hideVisual) {
		player.GetModPlayer<CanisterModifiersPlayer>().CanisterLaunchedExplosionRadiusMult += 0.5f;
		player.GetModPlayer<CanisterModifiersPlayer>().CanisterDepletedFireRateMult += 0.15f;
	}
}

public class PneumaticPumpDropCondition : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.Mimic;
	}

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		OneFromOptionsDropRule rule = npcLoot.Get()
			.First(x => x is LeadingConditionRule { condition: Conditions.NotRemixSeed })
			.ChainedRules
			.First(x => x.RuleToChain is OneFromOptionsDropRule)
			.RuleToChain as OneFromOptionsDropRule;

		System.Collections.Generic.List<int> drops = [.. rule.dropIds];
		drops.Add(ModContent.ItemType<PneumaticPump>());
		rule.dropIds = [.. drops];
	}
}
