using System.Collections.Generic;
using Canisters.Content.Items.Canisters;
using Canisters.Content.Projectiles.VolatileCanister;
using Canisters.DataStructures;
using Canisters.Helpers;
using Canisters.Helpers._Legacy.Abstracts;
using Canisters.Helpers.Enums;
using Stubble.Core;
using Terraria.DataStructures;
using Terraria.Enums;
using static Microsoft.Xna.Framework.MathHelper;

namespace Canisters.Content.Items.Weapons;

public class Resonance : BaseCanisterUsingWeapon
{
	public override CanisterFiringType CanisterFiringType {
		get => CanisterFiringType.Depleted;
	}

	public override Vector2 MuzzleOffset {
		get => new(52f, -2f);
	}

	public override Vector2? HoldoutOffset() {
		return new Vector2(-4f, 4f);
	}

	public override void SetDefaults() {
		Item.DefaultToCanisterUsingWeapon(8, 8, 12f, 16, 3f);
		Item.width = 60;
		Item.height = 20;
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(gold: 5));
	}

	public override IEnumerable<Projectile> ShootProjectiles(IEntitySource source, CanisterShootStats stats) {
		Vector2 normal = stats.Velocity.SafeNormalize(Vector2.Zero).RotatedBy(PiOver2) * Main.LocalPlayer.direction;
		yield return Projectile.NewProjectileDirect(source, stats.Position + (normal * 3f), stats.Velocity, stats.ProjectileType, stats.Damage, stats.Knockback, Main.myPlayer);
		yield return Projectile.NewProjectileDirect(source, stats.Position - (normal * 3f), stats.Velocity, stats.ProjectileType, stats.Damage, stats.Knockback, Main.myPlayer);
	}

	public override void AddRecipes() {
		CreateRecipe()
			.AddIngredient(ItemID.LightShard)
			.AddIngredient(ItemID.DarkShard)
			.AddIngredient(ItemID.MythrilBar, 16)
			.AddTile(TileID.MythrilAnvil)
			.Register();

		CreateRecipe()
			.AddIngredient(ItemID.LightShard)
			.AddIngredient(ItemID.DarkShard)
			.AddIngredient(ItemID.PalladiumBar, 16)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
