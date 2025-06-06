﻿using Canisters.DataStructures;
using Terraria.Audio;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class ChlorophyteSlugger : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(38f, 12f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-4f, 10f);
	}

	public override void SetStaticDefaults() {
		ItemID.Sets.SkipsInitialUseSound[Type] = true;
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(18, 18, 11f, 30, 1f);
		Item.width = 50;
		Item.height = 24;
		Item.SetShopValues(ItemRarityColor.Lime7, Item.buyPrice(gold: 5, silver: 50));
		Item.UseSound = SoundID.Item10 with { PitchRange = (0.8f, 1f), MaxInstances = 0 };
	}

	public override bool CanConsumeAmmo(Item ammo, Player player) {
		return Main.rand.NextBool(60, 100);
	}

	public override void ApplyWeaponStats(ref CanisterShootStats stats) {
		stats.TotalSpread += 0.35f;
	}

	public override bool? UseItem(Player player) {
		player.GetModPlayer<ChlorophyteSluggerPlayer>().UseItem();

		SoundEngine.PlaySound(Item.UseSound, player.Center);

		return base.UseItem(player);
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.ChlorophyteBar, 18)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}

public class ChlorophyteSluggerPlayer : ModPlayer
{
	private const int MaxRampUp = 20;
	private const int MaxRampDownDelayInitial = 25;
	private const int MaxRampDownDelay = 6;
	private const float RampUpStrength = 0.6f;

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
