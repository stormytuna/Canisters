using Canisters.Common;
using Canisters.Content.Items.Weapons;
using Canisters.Content.Projectiles.BlightedCanister;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class BlightedCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredBlightedCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<BlightedBolt>();
	}

	public override Color CanisterColor {
		get => Color.Lime;
	}

	public override bool SuppressWeaponUseSound(BaseCanisterUsingWeapon weapon) {
		return weapon.CanisterFiringType == CanisterFiringType.Depleted;
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(12, 4f, 0f);
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(silver: 9));
	}

	public override void AddRecipes() {
		int amount = ServerConfig.Instance.LowGrind ? 300 : 150;
		CreateRecipe(amount)
			.AddIngredient<EmptyCanister>(amount)
			.AddIngredient(ItemID.CursedFlame)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
