using Canisters.Common;
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
		Item.DefaultToCanister(10, 3.5f, 4f);
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
		int amount = ServerConfig.Instance.LowGrind ? 600 : 300;
		CreateRecipe(amount)
			.AddIngredient<EmptyCanister>(amount)
			.AddIngredient(ItemID.SoulofNight)
			.AddIngredient(ItemID.SoulofLight)
			.AddTile(TileID.Bottles)
			.Register();
	}
}
