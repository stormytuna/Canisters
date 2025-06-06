using Canisters.Content.Items.Weapons;
using Canisters.Content.Projectiles.BlightedCanister;
using Canisters.DataStructures;
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
		get => new Color(30, 246, 51, 220);
	}

	public override bool SuppressWeaponUseSound(BaseCanisterUsingWeapon weapon) {
		return weapon.CanisterFiringType == CanisterFiringType.Depleted;
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(12, 4f, 0f);
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(silver: 9));
	}

	public override void AddRecipes() {
		CreateRecipe(150)
			.AddIngredient<EmptyCanister>(150)
			.AddIngredient(ItemID.CursedFlame)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
