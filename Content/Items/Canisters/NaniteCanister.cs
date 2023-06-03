using Canisters.Content.Projectiles.NaniteCanister;
using Canisters.Helpers.Interfaces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

public class NaniteCanister : ModItem, ICanisterItem
{
	public int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.NaniteCanister.NaniteCanister>();
	public int DepletedProjectileType => ModContent.ProjectileType<NaniteBlob>();

	public override void SetStaticDefaults() {
		Item.ResearchUnlockCount = 99;
	}

	public override void SetDefaults() {
		// Base stats
		Item.width = 22;
		Item.height = 22;
		Item.maxStack = 999;
		Item.value = Item.sellPrice(silver: 3);
		Item.rare = ItemRarityID.Yellow;

		// Weapon stats
		Item.shoot = ModContent.ProjectileType<Projectiles.NaniteCanister.NaniteCanister>();
		Item.shootSpeed = 1f;
		Item.damage = 8;
		Item.knockBack = 2f;
		Item.DamageType = DamageClass.Ranged;
		Item.ammo = ModContent.ItemType<VolatileCanister>();
	}
}