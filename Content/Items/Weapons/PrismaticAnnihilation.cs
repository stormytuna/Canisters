using Canisters.Content.Projectiles;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
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

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(18, 18, 11f, 71, 2f);
		Item.width = 36;
		Item.height = 18;
		Item.SetShopValues(ItemRarityColor.StrongRed10, Item.sellPrice(gold: 10));
		// TODO: use sound
		Item.UseSound = SoundID.Item5 with { PitchRange = (0.6f, 1.2f), MaxInstances = 0, Volume = 0.6f };
	}

	public override bool? UseItem(Player player) {
		player.GetModPlayer<PrismaticAnnihilationPlayer>().UseItem();	
		return base.UseItem(player);
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
		if (_shootCount++ > 1) {
			_shootCount = 0;
			Projectile.NewProjectile(source, position, velocity * 0.8f, ModContent.ProjectileType<PrismaticAnnihilationStar>(), damage / 2, knockback);
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
		if (item.ModItem is not ChlorophyteSlugger) {
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
		npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PrismaticAnnihilation>(), 5));
	}
}
