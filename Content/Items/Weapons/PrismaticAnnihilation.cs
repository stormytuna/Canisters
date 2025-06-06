using System.Linq;
using Canisters.Content.Projectiles;
using Canisters.DataStructures;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ItemDropRules;

namespace Canisters.Content.Items.Weapons;

public class PrismaticAnnihilation : BaseCanisterUsingWeapon
{
	private int _shootCount = 0;

	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(90f, -6f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-6f, 0f);
	}

	public override void SetStaticDefaults() {
		ItemID.Sets.SkipsInitialUseSound[Type] = true;
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(18, 18, 11f, 92, 2f);
		Item.width = 36;
		Item.height = 18;
		Item.SetShopValues(ItemRarityColor.StrongRed10, Item.sellPrice(gold: 10));
		Item.UseSound = SoundID.Item5 with { PitchRange = (0.6f, 1.2f), MaxInstances = 0, Volume = 0.6f };
	}

	public override bool CanConsumeAmmo(Item ammo, Player player) {
		return Main.rand.NextBool(50, 100);
	}

	public override bool? UseItem(Player player) {
		player.GetModPlayer<PrismaticAnnihilationPlayer>().UseItem();

		SoundEngine.PlaySound(Item.UseSound, player.Center);

		return base.UseItem(player);
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (_shootCount++ > 1) {
			_shootCount = 0;
			Projectile.NewProjectile(source, position, velocity * 0.8f, ModContent.ProjectileType<PrismaticAnnihilationStar>(), damage / 2, knockback);
			SoundEngine.PlaySound(SoundID.Item9, player.Center);
		}

		return base.Shoot(player, source, position, velocity, type, damage, knockback);
	}
}

public class PrismaticAnnihilationPlayer : ModPlayer
{
	private const int MaxRampUp = 25;
	private const int MaxRampDownDelayInitial = 35;
	private const int MaxRampDownDelay = 8;
	private const float RampUpStrength = 0.5f;

	private int _rampUp = 0;
	private int _rampDownDelayTimer = 0;

	public void UseItem() {
		_rampUp = int.Clamp(_rampUp + 1, 0, MaxRampUp);
		_rampDownDelayTimer = MaxRampDownDelayInitial;
	}

	public override void PostUpdateMiscEffects() {
		_rampDownDelayTimer -= 1;
		if (_rampDownDelayTimer <= 0) {
			_rampUp = int.Clamp(_rampUp - 1, 0, MaxRampUp);
			_rampDownDelayTimer = MaxRampDownDelay;
		}
	}

	public override float UseTimeMultiplier(Item item) {
		if (item.ModItem is not PrismaticAnnihilation) {
			return base.UseTimeMultiplier(item);
		}

		float rampUpProgress = _rampUp / (float)MaxRampUp;
		return 1f - (rampUpProgress * RampUpStrength);
	}
}

public class PrismaticAnnihilationGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC entity, bool lateInstantiation) {
		return entity.type == NPCID.MoonLordCore;
	}

	public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
		FromOptionsWithoutRepeatsDropRule rule = npcLoot.Get()
			.First(x => x is LeadingConditionRule { condition: Conditions.NotExpert })
			.ChainedRules
			.First(x => x.RuleToChain is FromOptionsWithoutRepeatsDropRule)
			.RuleToChain as FromOptionsWithoutRepeatsDropRule;

		System.Collections.Generic.List<int> drops = [.. rule.dropIds];
		drops.Add(ModContent.ItemType<PrismaticAnnihilation>());
		rule.dropIds = [.. drops];
	}
}

public class PrismaticAnnihilationGlobalItem : GlobalItem
{
	public override bool AppliesToEntity(Item item, bool lateInstantiation) {
		return item.type == ItemID.MoonLordBossBag;
	}

	public override void ModifyItemLoot(Item item, ItemLoot itemLoot) {
		FromOptionsWithoutRepeatsDropRule rule = itemLoot.Get()
			.First(x => x is FromOptionsWithoutRepeatsDropRule)
			as FromOptionsWithoutRepeatsDropRule;

		System.Collections.Generic.List<int> drops = [.. rule.dropIds];
		drops.Add(ModContent.ItemType<PrismaticAnnihilation>());
		rule.dropIds = [.. drops];
	}
}
