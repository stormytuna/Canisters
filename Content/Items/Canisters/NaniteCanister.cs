using Canisters.Content.Projectiles.NaniteCanister;
using Canisters.Helpers.Abstracts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class NaniteCanister : CanisterItem
{
	public override int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.NaniteCanister.NaniteCanister>();
	public override int DepletedProjectileType => ModContent.ProjectileType<NaniteBlob>();

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.sellPrice(silver: 3);
		Item.rare = ItemRarityID.Yellow;

		// Weapon stats
		Item.shootSpeed = 4f;
		Item.damage = 16;
		Item.knockBack = 2f;
	}
}