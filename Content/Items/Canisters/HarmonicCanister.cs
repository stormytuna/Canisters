using Canisters.Content.Projectiles.HarmonicCanister;
using Canisters.DataStructures;
using Canisters.Helpers;
using Terraria.Enums;

namespace Canisters.Content.Items.Canisters;

public class HarmonicCanister : BaseCanisterItem
{
	public override int LaunchedProjectileType {
		get => ModContent.ProjectileType<FiredHarmonicCanister>();
	}

	public override int DepletedProjectileType {
		get => ModContent.ProjectileType<HelixBolt>();
	}

	public override Color CanisterColor {
		get => Color.Purple;
	}

	public override void SetDefaults() {
		Item.DefaultToCanister(11, 2.5f, 4f);
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(silver: 9));
	}

	public override void ApplyAmmoStats(ref CanisterShootStats stats) {
		if (stats.IsDepleted) {
			stats.ProjectileCount = 2;
		}
	}

	public override void ModifyProjectile(Projectile projectile, int numInTotalAmount) {
		if (projectile.ModProjectile is HelixBolt bolt) {
			bolt.IsLight = numInTotalAmount % 2 == 0;
		}
	}

	public override void AddRecipes() {
		CreateRecipe(300)
			.AddIngredient<EmptyCanister>(300)
			.AddIngredient(ItemID.SoulofNight)
			.AddIngredient(ItemID.SoulofLight)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
