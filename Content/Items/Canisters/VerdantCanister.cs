using Canisters.Common;
using Canisters.Content.Items.Weapons;
using Canisters.Content.Projectiles.VerdantCanister;
using Canisters.Helpers;
using Canisters.Helpers.Enums;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class VerdantCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredVerdantCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<VerdantGasEmitter>();
	}

	public override Color CanisterColor {
		get => Color.Green;
	}

	public override bool SuppressWeaponUseSound(BaseCanisterUsingWeapon weapon) {
		return weapon.CanisterFiringType == CanisterFiringType.Depleted;
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(8, 1.5f, 0f);
		Item.SetShopValues(ItemRarityColor.Green2, Item.buyPrice(copper: 75));
	}

	public override void AddRecipes() {
		int amount = ServerConfig.Instance.LowGrind ? 400 : 200;
		CreateRecipe(amount)
			.AddIngredient<EmptyCanister>(amount)
			.AddIngredient(ItemID.JungleSpores)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
