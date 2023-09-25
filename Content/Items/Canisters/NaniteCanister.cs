using Canisters.Content.Projectiles.NaniteCanister;
using Canisters.Helpers.Abstracts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Canisters.Content.Items.Canisters;

// TODO: Probably make this blue, like the regular nanites 
public class NaniteCanister : CanisterItem
{
	public override int LaunchedProjectileType => ModContent.ProjectileType<Projectiles.NaniteCanister.NaniteCanister>();
	public override int DepletedProjectileType => ModContent.ProjectileType<NaniteMistEmitter>();
	public override Color CanisterColor => Color.LightCyan;

	public override void SafeSetDefaults() {
		// Base stats
		Item.value = Item.sellPrice(silver: 3);
		Item.rare = ItemRarityID.Yellow;

		// Weapon stats
		Item.shootSpeed = 4f;
		Item.damage = 16;
		Item.knockBack = 2f;
	}

	// TODO: Recipe lmao
}