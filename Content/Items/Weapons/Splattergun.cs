using Canisters.DataStructures;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
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

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(14, 14, 10f, 16, 3f);
		Item.width = 30;
		Item.height = 26;
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(gold: 2, silver: 50));
	}

	public override void ApplyWeaponStats(ref CanisterShootStats stats) {
		stats.Velocity *= 1.5f;
	}

	public override bool? UseItem(Player player) {
		player.GetModPlayer<SplatterGunPlayer>().UseItem();
		return base.UseItem(player);
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.PainterPaintballGun)
			.AddIngredient(ItemID.OrichalcumBar, 12)
			.AddTile(TileID.MythrilAnvil)
			.Register();

		CreateRecipe()
			.AddIngredient(ItemID.PainterPaintballGun)
			.AddIngredient(ItemID.MythrilBar, 12)
			.AddTile(TileID.MythrilAnvil)
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
