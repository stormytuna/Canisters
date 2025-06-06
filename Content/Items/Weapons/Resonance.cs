﻿using System.Collections.Generic;
using System.Linq;
using Canisters.DataStructures;
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
		Item.DefaultToCanisterUsingWeapon(14, 14, 12f, 34, 3f);
		Item.width = 60;
		Item.height = 20;
		Item.SetShopValues(ItemRarityColor.LightRed4, Item.buyPrice(gold: 5));
		Item.UseSound = SoundID.Item42 with { PitchRange = (0.1f, 0.4f) };
	}

	public override bool CanConsumeAmmo(Item ammo, Player player) {
		return Main.rand.NextBool(30, 100);
	}

	public override IEnumerable<Projectile> ShootProjectiles(IEntitySource source, CanisterShootStats stats) {
		Vector2 normal = stats.Velocity.SafeNormalize(Vector2.Zero).RotatedBy(PiOver2) * Main.LocalPlayer.direction;

		IEnumerable<Projectile> bottom = DefaultShoot(source, stats with { Position = stats.Position + (normal * 3f) });
		IEnumerable<Projectile> top = DefaultShoot(source, stats with { Position = stats.Position - (normal * 3f) });
		foreach (Projectile proj in bottom.Concat(top)) {
			yield return proj;
		}
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
			.AddIngredient(ItemID.OrichalcumBar, 16)
			.AddTile(TileID.MythrilAnvil)
			.Register();
	}
}
