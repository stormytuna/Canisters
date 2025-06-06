﻿using Canisters.DataStructures;
using Terraria.Audio;
using Terraria.Enums;

namespace Canisters.Content.Items.Weapons;

public class Splattergun : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(26f, -2f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(0f, 0f);
	}

	public override void SetStaticDefaults() {
		ItemID.Sets.SkipsInitialUseSound[Type] = true;
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(14, 14, 12f, 38, 1f);
		Item.width = 30;
		Item.height = 26;
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(gold: 2, silver: 50));
		Item.UseSound = SoundID.Item5 with { PitchRange = (0.9f, 1.3f), MaxInstances = 0, Volume = 0.6f };
	}

	public override bool CanConsumeAmmo(Item ammo, Player player) {
		return Main.rand.NextBool(25, 100);
	}

	public override bool? UseItem(Player player) {
		player.GetModPlayer<SplatterGunPlayer>().UseItem();

		SoundEngine.PlaySound(Item.UseSound, player.Center);

		return base.UseItem(player);
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.PainterPaintballGun)
			.AddIngredient(ItemID.PalladiumBar, 12)
			.AddTile(TileID.Anvils)
			.Register();

		CreateRecipe()
			.AddIngredient(ItemID.PainterPaintballGun)
			.AddIngredient(ItemID.CobaltBar, 12)
			.AddTile(TileID.Anvils)
			.Register();
	}
}

public class SplatterGunPlayer : ModPlayer
{
	private const int MaxRampUp = 25;
	private const int MaxRampDownDelayInitial = 30;
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
		if (item.ModItem is not Splattergun) {
			return base.UseTimeMultiplier(item);
		}

		float rampUpProgress = _rampUp / (float)MaxRampUp;
		return 1f - (rampUpProgress * RampUpStrength);
	}
}
